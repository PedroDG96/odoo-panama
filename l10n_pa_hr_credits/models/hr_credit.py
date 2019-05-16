# -*- coding: utf-8 -*-
# Part of Odoo. See LICENSE file for full copyright and licensing details.

from odoo import api, fields, models, _
from datetime import datetime


class HRCredit(models.Model):
    _name = "hr.credit"

    name = fields.Char(string='Credit Reference', required=True, copy=False, readonly=True,
                       index=True, default=lambda self: _('New'))
    date = fields.Date(string='Date', default=fields.Date.today,required=True)
    init_date = fields.Date(string='Init Date',required=True)
    employee_id = fields.Many2one('hr.employee',string='Employee',required=True)
    amount = fields.Float(string='Amount',default=0.0,required=True)
    dues = fields.Integer(string='Dues',default=1,required=True)
    discount_type = fields.Selection([('biweekly','Biweekly'),
                                      ('monthly','Monthly'),], string='Discount Type',required=True)
    hr_credit_line_id = fields.One2many('hr.credit.line', 'credit_id', string='HR Credit Lines',copy=False)
    state = fields.Selection([
        ('draft', 'Draft'),
        ('done', 'Done'),
        ('cancel', 'Cancelled'),
    ], string='Status', readonly=True, copy=False, index=True, track_visibility='onchange', default='draft')

    description = fields.Text('Notes')
    amount_total = fields.Float(string='Amount Total', store=True, readonly=True, compute='_amount_total',
                                 track_visibility='always')
    paid_amount = fields.Float(string='Paid Amount', store=True, readonly=True, compute='_amount_total',
                                 track_visibility='always')
    due_amount = fields.Float(string='Due Amount', store=True, readonly=True, compute='_amount_total',
                                 track_visibility='always')

    inputs_id = fields.Many2one("hr.rule.input", string="Entrada", required=True)

    @api.multi
    @api.depends('hr_credit_line_id.amount','hr_credit_line_id.ispaid')
    def _amount_total(self):
        for credit in self:
            credit.amount_total = sum(fee.amount for fee in self.hr_credit_line_id)
            credit.paid_amount = sum(fee.amount for fee in self.hr_credit_line_id if fee.ispaid)
            credit.due_amount = sum(fee.amount for fee in self.hr_credit_line_id if not fee.ispaid)



    @api.multi
    def action_done(self):
        discount_date = self.init_date
        for line in self.hr_credit_line_id:
            line.unlink()

        if self.amount <= 0:
            raise _('Amount is Zero.')

        if self.discount_type == 'biweekly':
            dues = self.dues * 2
        else:
            dues = self.dues

        due_amount = self.amount / dues

        date = datetime.strptime(discount_date,"%Y-%m-%d")
        year = date.year
        month = date.month

        day = 28

        if self.discount_type == 'biweekly':
            if date.day < 14:
                day = 14
            elif date.day >= 28:
                day = 14
                if month == 12:
                    month = 1
                    year = year + 1
                else:
                    month = month + 1
        else:
            if month == 12:
                month = 1
                year = year + 1
            else:
                month = month + 1

        x = 1
        while x <= dues:
            discount_date = datetime(
                year=year,
                month=month,
                day=day,
            )

            creditline_v = self.env['hr.credit.line'].new({
                'credit_id': self.id,
                'sequence': x,
                'employee_id': self.employee_id,
                'discount_date': discount_date,
                'amount': due_amount,
            })

            creditline_vals = creditline_v._convert_to_write(creditline_v._cache)

            self.env['hr.credit.line'].create(creditline_vals)

            if day == 28:
                if month == 12:
                    month = 1
                    year = year + 1
                else:
                    month = month + 1

            if self.discount_type == 'biweekly':
                if day == 14:
                    day = 28
                else:
                    day = 14

            x = x + 1

        if self.amount_total != self.amount:
            raise _('Amount Incorrect.')

        self.write({'state': 'done'})
        if self.name == _('New'):
            sequence = self.env['ir.sequence'].next_by_code('hr.credit') or _('New')
            self.write({'name': sequence})


    @api.multi
    def action_draft(self):
        self.write({'state': 'draft'})

    @api.multi
    def action_cancel(self):
        self.write({'state': 'cancel'})


class HRCreditLine(models.Model):
    _name = "hr.credit.line"

    credit_id = fields.Many2one('hr.credit', string='HR Credit', required=True, ondelete='cascade',
                               index=True,copy=False)
    sequence = fields.Integer('sequence', default=1)
    discount_date = fields.Date(string='Discount Date')
    employee_id = fields.Many2one('hr.employee',string='Employee')
    amount = fields.Float(string='Amount',default=0.0)
    ispaid = fields.Boolean(string='Is Paid', default=False)
