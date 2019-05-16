# -*- encoding: utf-8 -*-
from odoo import api, fields, models, tools, _
from odoo.exceptions import ValidationError
from odoo.tools import float_is_zero, float_compare

class DetailsAccountPayment(models.Model):
    _inherit = "account.payment"

    payment_number = fields.Char('Número',
        help="Número del medio de pago utilizado (cheque, transferencia, depósito, etc.)")
    bank_id = fields.Many2one('res.bank','Banco origen')
    journal_type = fields.Selection(related='journal_id.type',string='Tipo Diario') 
