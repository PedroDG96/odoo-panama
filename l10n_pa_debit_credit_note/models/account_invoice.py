# -*- coding: utf-8 -*-

from odoo import models,fields,api


class DebitCreditAccountInvoice(models.Model):
    _inherit = "account.invoice"

    @api.one
    @api.depends('invoice_line_ids.price_subtotal', 'tax_line_ids.amount', 'currency_id', 
                  'company_id', 'date_invoice', 'refund_type')
    def _compute_amount(self):
        self.amount_untaxed = sum(line.price_subtotal for line in self.invoice_line_ids)
        self.amount_tax = sum(line.amount for line in self.tax_line_ids)
        self.amount_total = self.amount_untaxed + self.amount_tax
        amount_total_company_signed = self.amount_total
        amount_untaxed_signed = self.amount_untaxed
        if self.currency_id and self.company_id and self.currency_id != self.company_id.currency_id:
            currency_id = self.currency_id.with_context(date=self.date_invoice)
            amount_total_company_signed = currency_id.compute(self.amount_total, self.company_id.currency_id)
            amount_untaxed_signed = currency_id.compute(self.amount_untaxed, self.company_id.currency_id)
        sign = (self.type in ['in_refund','out_refund'] and self.refund_type == 'credit') and -1 or 1
        self.amount_total_company_signed = amount_total_company_signed * sign
        self.amount_total_signed = self.amount_total * sign
        self.amount_untaxed_signed = amount_untaxed_signed * sign

    child_ids = fields.One2many(
        'account.invoice',
        'refund_invoice_id',
        'Debit and Credit Notes',
        readonly=True, copy=False,
        states={'draft': [('readonly', False)]},
        help='These are all credit and debit to this invoice')

    refund_type = fields.Selection(
        [('debit', 'Debit Note'),
         ('credit', 'Credit Note')], index=True,
        string='Refund type',
        track_visibility='always')

    @api.model
    def line_get_convert(self, line, part):
        res = super(DebitCreditAccountInvoice, self).line_get_convert(line, part)
        if self.type in ['in_refund','out_refund'] and self.refund_type == 'debit':
            debit = res.get('debit')
            credit = res.get('credit')
            res.update({'debit': credit , 'credit': debit})
        return res
