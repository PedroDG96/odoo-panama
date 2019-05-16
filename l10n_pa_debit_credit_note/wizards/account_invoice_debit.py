# -*- coding: utf-8 -*-

from odoo import models, fields, api, _
from odoo.tools.safe_eval import safe_eval
from odoo.exceptions import UserError


class AccountInvoiceDebit(models.TransientModel):
    """Refunds invoice"""

    _name = "account.invoice.debit"
    _description = "Debit Note"

    @api.model
    def _get_reason(self):
        context = dict(self._context or {})
        active_id = context.get('active_id', False)
        if active_id:
            inv = self.env['account.invoice'].browse(active_id)
            return inv.name
        return ''

    @api.model
    def _get_journal(self):
        journal_type = self._context.get('journal_type')
        journal = False
        if journal_type == 'sale':
            journal = self.env.ref('l10n_pa_debit_credit_note.account_journal_sale_debit_note')
        elif journal_type == 'purchase':
            journal = self.env.ref('l10n_pa_debit_credit_note.account_journal_purchase_debit_note')
        return journal and journal.id or False

    date_invoice = fields.Date(string='Refund Date', default=fields.Date.context_today, required=True)
    date = fields.Date(string='Accounting Date')
    description = fields.Char(string='Reason', required=True, default=_get_reason)
    journal_id = fields.Many2one('account.journal', string='Journal',
                                 default =_get_journal,
                                 domain="[('type', 'in', {'out_invoice': ['sale'], 'out_refund': ['sale'], 'in_refund': ['purchase'], 'in_invoice': ['purchase']}.get(type, []))]")
    filter_debit = fields.Selection([('debit', 'Create a draft debit note')],
        default='debit', string='Debit Note Method', required=True, help='Debit Note base on this type. You can not Modify and Cancel if the invoice is already reconciled')


    @api.multi
    def compute_debit(self, mode='debit'):
        inv_obj = self.env['account.invoice']
        inv_tax_obj = self.env['account.invoice.tax']
        inv_line_obj = self.env['account.invoice.line']
        context = dict(self._context or {})
        xml_id = False

        for form in self:
            created_inv = []
            date = False
            description = False
            for inv in inv_obj.browse(context.get('active_ids')):
                if inv.state in ['draft', 'proforma2', 'cancel']:
                    raise UserError(_('Cannot refund draft/proforma/cancelled invoice.'))
                if inv.reconciled and mode in ('cancel', 'modify'):
                    raise UserError(_('Cannot refund invoice which is already reconciled, invoice should be unreconciled first. You can only refund this invoice.'))

                date = form.date or False
                description = form.description or inv.name
                journal_id = form.journal_id or inv.journal_id

                invoice = inv.copy()

                invoice.update({
                    'type': inv.type == 'in_invoice' and 'in_refund' or inv.type == 'out_invoice' and 'out_refund' or inv.type,
                    'refund_type': 'debit',
                    'name': description,
                    'date_invoice': form.date_invoice,
                    'date': date,
                    'journal_id': journal_id.id,
                })

                created_inv.append(invoice.id)

                xml_id = (inv.type in ['out_refund', 'out_invoice']) and 'action_invoice_tree3' or \
                         (inv.type in ['in_refund', 'in_invoice']) and 'action_invoice_tree4'
                # Put the reason in the chatter
                subject = _("Debit Note")
                body = description
                invoice.message_post(body=body, subject=subject)
        if xml_id:
            result = self.env.ref('l10n_ar_debit_credit_note.%s' % (xml_id)).read()[0]
            invoice_domain = safe_eval(result['domain'])
            invoice_domain.append(('id', 'in', created_inv))
            result['domain'] = invoice_domain
            return result
        return True

    @api.multi
    def invoice_debit(self):
        data_debit = self.read(['filter_debit'])[0]['filter_debit']
        return self.compute_debit(data_debit)
