# coding: utf-8
from odoo import _, api, fields, models


class AccountJournal(models.Model):
    _inherit = 'account.journal'

    @api.one
    @api.depends('outbound_payment_method_ids')
    def _compute_check_printing_payment_method_selected(self):
        self.check_printing_payment_method_selected = any(pm.code == 'check_printing' for pm in self.outbound_payment_method_ids)

    #~ @api.one
    #~ @api.depends('check_manual_sequencing')
    #~ def _get_check_next_number(self):
        #~ if self.check_sequence_id:
        #~ self.check_next_number = self.check_sequence_id.number_next_actual
        #~ else:
        #~ self.check_next_number = 1

    @api.one
    def _set_check_next_number(self):
        if self.check_next_number < self.check_sequence_id.number_next_actual:
            raise ValidationError(_("The last check number was %s. In order to avoid a check being rejected "
                                    "by the bank, you can only use a greater number.") % self.check_sequence_id.number_next_actual)
        if self.check_sequence_id:
            self.check_sequence_id.sudo().number_next_actual = self.check_next_number

    #~ check_manual_sequencing = fields.Boolean('Manual Numbering', default=False,
        #~ help="Check this option if your pre-printed checks are not numbered.")
    check_sequence_id = fields.Many2one('ir.sequence', 'Check Sequence', readonly=True, copy=False,
                                        help="Checks numbering sequence.")
    check_next_number = fields.Integer('Next Check Number', compute='_get_check_next_number', inverse='_set_check_next_number',
                                       help="Sequence number of the next printed check.")
    check_printing_payment_method_selected = fields.Boolean(compute='_compute_check_printing_payment_method_selected',
                                                            help="Technical feature used to know whether check printing was enabled as payment method.")
    checkbook_ids = fields.One2many(
        'account.checkbook',
        'journal_id',
        'Checkbooks',
    )

    @api.model
    def create(self, vals):
        rec = super(AccountJournal, self).create(vals)
        issue_checks = self.env.ref(
            'l10n_pa_account_check.account_payment_method_issue_check')
        if (issue_checks in rec.outbound_payment_method_ids and
                not rec.checkbook_ids):
            rec._create_checkbook()
        return rec

    @api.one
    def _create_checkbook(self):
        """ Create a check sequence for the journal """
        checkbook = self.checkbook_ids.create({
            'journal_id': self.id,
        })
        checkbook.state = 'active'

    @api.model
    def _enable_issue_check_on_bank_journals(self):
        """ Enables issue checks payment method
            Called upon module installation via data file.
        """
        issue_checks = self.env.ref(
            'l10n_pa_account_check.account_payment_method_issue_check')
        domain = [('type', '=', 'bank')]
        force_company_id = self._context.get('force_company_id')
        if force_company_id:
            domain += [('company_id', '=', force_company_id)]
        bank_journals = self.search(domain)
        for bank_journal in bank_journals:
            if not bank_journal.checkbook_ids:
                bank_journal._create_checkbook()
            bank_journal.write({
                'outbound_payment_method_ids': [(4, issue_checks.id, None)],
            })

#########################

    #~ @api.model
    #~ def create(self, vals):
        #~ rec = super(AccountJournal, self).create(vals)
        #~ if not rec.check_sequence_id:
            #~ rec._create_check_sequence()
        #~ return rec

    @api.one
    @api.returns('self', lambda value: value.id)
    def copy(self, default=None):
        rec = super(AccountJournal, self).copy(default)
        rec._create_check_sequence()
        return rec

    @api.one
    def _create_check_sequence(self):
        """ Create a check sequence for the journal """
        self.check_sequence_id = self.env['ir.sequence'].sudo().create({
            'name': self.name + _(" : Check Number Sequence"),
            'implementation': 'no_gap',
            'padding': 5,
            'number_increment': 1,
            'company_id': self.company_id.id,
        })

    def _default_outbound_payment_methods(self):
        methods = super(AccountJournal, self)._default_outbound_payment_methods()
        return methods + self.env.ref('l10n_pa_account_check.account_payment_method_check')

    @api.multi
    def get_journal_dashboard_datas(self):
        domain_checks_to_print = [
            ('journal_id', '=', self.id),
            ('payment_method_id.code', '=', 'check_printing'),
            ('state', '=', 'posted')
        ]
        return dict(
            super(AccountJournal, self).get_journal_dashboard_datas(),
            num_checks_to_print=len(self.env['account.payment'].search(domain_checks_to_print))
        )

    @api.multi
    def action_checks_to_print(self):
        return {
            'name': _('Checks to Print'),
            'type': 'ir.actions.act_window',
            'view_mode': 'list,form,graph',
            'res_model': 'account.payment',
            'context': dict(
                self.env.context,
                search_default_checks_to_send=1,
                journal_id=self.id,
                default_journal_id=self.id,
                default_payment_type='outbound',
                default_payment_method_id=self.env.ref('l10n_pa_account_check.account_payment_method_check').id,
            ),
        }
