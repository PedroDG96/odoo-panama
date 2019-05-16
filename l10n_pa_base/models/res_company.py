# -*- coding: utf-8 -*-
# Part of Odoo. See LICENSE file for full copyright and licensing details.

from odoo import api, fields, models
from datetime import datetime

class ParaguayBaseCompany(models.Model):
    _inherit = "res.company"
    ruc = fields.Char('Identificador Fiscal', help="Identificador Fiscal")
    act_economica = fields.Selection(
            [('B','Bienes'),
             ('S','Servicios')],
              'Actividad Económica', size=1)


