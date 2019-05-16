# -*- coding: utf-8 -*-
# Part of Odoo. See LICENSE file for full copyright and licensing details.

from odoo import fields, models


class HrNews(models.Model):
    _name = 'hr.news'

    name = fields.Char(string='Name', required=True)
    currency_id = fields.Many2one("res.currency", string="Currency", required=True, default=3)
    default_amount = fields.Float('Amount', default=0.0)
    inputs_id = fields.Many2one("hr.rule.input", string="Entrada", required=True)
