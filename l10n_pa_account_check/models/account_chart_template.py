# coding: utf-8
from odoo import models, api, fields


class AccountChartTemplate(models.Model):
    _inherit = 'account.chart.template'

    check_rejected_account_id = fields.Many2one(
        'account.account.template',
        'Cuenta de Cheques Rechazados',
        help='Cuenta de cheques de rechazo, por ejemplo. "cheques rechazados"',
        # domain=[('type', 'in', ['other'])],
    )
    check_deferred_account_id = fields.Many2one(
        'account.account.template',
        'Cuenta de Cheques Diferidos',
        help='Cuenta de cheques diferidos, por ejemplo. "cheques diferidos"',
        # domain=[('type', 'in', ['other'])],
    )
    check_holding_account_id = fields.Many2one(
        'account.account.template',
        'Cuenta de Cheques Retenidos',
        help='Retencion de cheques cuenta para terceros cheques, "por ejemplo." "retención de cheques"',
        # domain=[('type', 'in', ['other'])],
    )

    @api.multi
    def _load_template(self, company, code_digits=None, transfer_account_id=None, account_ref=None, taxes_ref=None):
        account_ref, taxes_ref = super(AccountChartTemplate, self)._load_template(
            company,
            code_digits=code_digits,
            transfer_account_id=transfer_account_id,
            account_ref=account_ref,
            taxes_ref=taxes_ref)
        for field in [
                'check_rejected_account_id',
                'check_deferred_account_id',
                'check_holding_account_id']:
            account_field = self[field]
            # TODO we should send it in the context and overwrite with
            # lower hierichy values
            if account_field:
                company.update({field: account_ref[account_field.id]})
        return account_ref, taxes_ref


class WizardMultiChartsAccounts(models.TransientModel):
    _inherit = 'wizard.multi.charts.accounts'

    @api.multi
    def _create_bank_journals_from_o2m(self, company, acc_template_ref):
        """
        Bank - Cash journals are created with this method
        Inherit this function in order to add checks to cash and bank
        journals. This is because usually will be installed before chart loaded
        and they will be disable by default
        """
        res = super(
            WizardMultiChartsAccounts, self)._create_bank_journals_from_o2m(
            company, acc_template_ref)

        # creamos diario para cheques de terceros
        received_third_check = self.env.ref(
            'l10n_pa_account_check.account_payment_method_received_third_check')
        delivered_third_check = self.env.ref(
            'l10n_pa_account_check.account_payment_method_delivered_third_check')
        self.env['account.journal'].create({
            'name': 'Cheques de Terceros',
            'type': 'cash',
            'company_id': company.id,
            'inbound_payment_method_ids': [
                (4, received_third_check.id, None)],
            'outbound_payment_method_ids': [
                (4, delivered_third_check.id, None)],
        })

        self.env['account.journal'].with_context(
            force_company_id=company.id)._enable_issue_check_on_bank_journals()
        return res
