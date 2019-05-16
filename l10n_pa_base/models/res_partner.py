# -*- coding: utf-8 -*-
# Part of Odoo. See LICENSE file for full copyright and licensing details.

from odoo import api, fields, models


class res_partner(models.Model):
    _inherit = 'res.partner'
   
    ruc = fields.Char(string="Identificador Fiscal")
    por_retencion = fields.Float(string="Retención %")
    act_economica = fields.Selection(
            [('B','Bienes'),
             ('S','Servicios')],
              'Actividad Económica', size=1)
