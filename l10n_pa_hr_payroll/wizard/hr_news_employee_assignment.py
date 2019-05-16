# -*- encoding: utf-8 -*-

from odoo import api, fields, models, _
from odoo.exceptions import UserError


class HrNewsEmployeeAssignment(models.TransientModel):
    _name = 'hr.news.employee.assignment'

    news_ids = fields.Many2one('hr.news', string='News')
    date_from = fields.Date(string='Date From', required=True)
    date_to = fields.Date(string='Date To')
    amount = fields.Float(string='Amount', required=True)
    employee_ids = fields.Many2many('hr.employee', 'hr_news_employee_rel', 'employee_id', 'wizard_id', 'Employee')

    @api.onchange('news_ids')
    def onchange_news(self):
        self.amount = self.news_ids.default_amount

    @api.one
    def assign(self):
        if self.amount == 0:
            raise UserError(_('Amount is Zero!'))
        for employee in self.employee_ids:
            employee_new = self.env['hr.employee.news'].search([('news_id','=',self.news_ids.id),
                                                                ('employee_id','=',employee.id)])

            if employee_new:
                employee_new.write({
                    'date_from':self.date_from,
                    'date_to': self.date_to,
                    'amount':self.amount,
                })
            else:
                employee_new.create({
                    'news_id':self.news_ids.id,
                    'employee_id':employee.id,
                    'date_from':self.date_from,
                    'date_to': self.date_to,
                    'amount':self.amount,
                })
