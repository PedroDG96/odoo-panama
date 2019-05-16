# coding: utf-8
from odoo import _, api, fields, models
from odoo.exceptions import UserError


class ResCompany(models.Model):
    _inherit = 'res.company'

    check_rejected_account_id = fields.Many2one('account.account', 'Cuenta de Cheques Rechazados',
                                                help='Cuenta de cheques de rechazo, por ejemplo: "Cheques Rechazados"')
    check_deferred_account_id = fields.Many2one('account.account', 'Cuenta de Cheques Diferidos',
                                                help='Cuenta de cheques diferidos, por ejemplo: "Cheques Diferidos"')
    check_holding_account_id = fields.Many2one('account.account', 'Cuenta de Cheques Retenidos',
                                               help=u'Retencion de cheques cuenta para terceros cheques, por ejemplo: "Retención de Cheques"')

    check_main_bank_id = fields.Many2one('res.bank', 'Banco Principal', domain=[('bic', 'in', ['BANSPAPA', 'MCTBPAPA', 'MIDLPAPA'])],
                                         help=u'Banco por defecto para los cheques propios, para fines de impresión')

    @api.multi
    def _get_check_account(self, ctype):
        self.ensure_one()
        if ctype == 'holding':
            account = self.check_holding_account_id
        elif ctype == 'rejected':
            account = self.check_rejected_account_id
        elif ctype == 'deferred':
            account = self.check_deferred_account_id
        else:
            raise UserError(_("Type %s not implemented!"))
        if not account:
            raise UserError(_(u'No hay una cuenta de cheques %s definida para la compañía %s') % (ctype, self.name))
        return account
