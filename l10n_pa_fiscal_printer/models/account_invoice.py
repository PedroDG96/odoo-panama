# -*- coding: utf-8 -*-
# Part of Odoo. See LICENSE file for full copyright and licensing details.

from odoo import api, fields, models
from datetime import datetime, timedelta, date

class L10nPAAccountInvoice(models.Model):
    _inherit = "account.invoice"
    printer_list_ids = fields.One2many('l.printer', 'account_id', string='Impresora')
    testa = fields.Char("hello")
    numero_comprobante = fields.Char("N° Comprobante Fiscal", compute='calc_numero_comprobante')

    @api.one
    @api.depends('printer_list_ids')
    def calc_numero_comprobante(self):
        numero_comprobante = ''
        if self.partner_id:
            if self.printer_list_ids:
                for invoice in self.printer_list_ids:
                    if not invoice.reimpresion:
                        if invoice.serial_equipo_fiscal:
                            serial_equipo_fiscal = str(invoice.serial_equipo_fiscal)
                            num = str(invoice.numero_comprobante)
                            numero_comprobante = serial_equipo_fiscal + "-" + num.zfill(8)
                            self.numero_comprobante = numero_comprobante


class List_FiscalPrinter(models.Model):
    _name = 'l.printer'
    id_fiscal = fields.Integer("Documento Fiscal")
    serial_equipo_fiscal = fields.Char("Serie Equipo Fiscal")
    reimpresion = fields.Boolean("Reimpresión")
    numero_comprobante = fields.Integer("N° Comprobante Fiscal")
    obs = fields.Char("Observaciones")
    account_id = fields.Many2one('account.invoice',"Factura")
    print_date = fields.Date('Fecha')
    print_user = fields.Char("Usuario")
