# -*- coding: utf-8 -*-
# Part of Odoo. See LICENSE file for full copyright and licensing details.

from odoo import fields, models


class HREmployee(models.Model):
    _inherit = 'hr.employee'

    news_ids = fields.One2many('hr.employee.news', 'employee_id', string='News')


class HREmployeeNews(models.Model):
    _name = 'hr.employee.news'

    news_id = fields.Many2one('hr.news', string='News')
    date_from = fields.Date('From', default=fields.Date.today)
    date_to = fields.Date('To')
    currency_id = fields.Many2one("res.currency", string="Currency", required=True, related='news_id.currency_id')
    amount = fields.Float('Amount')
    employee_id = fields.Many2one('hr.employee', "Employee", ondelete='cascade')
