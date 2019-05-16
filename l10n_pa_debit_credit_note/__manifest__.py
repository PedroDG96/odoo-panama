# -*- encoding: utf-8 -*-
##############################################################################
#
#    OpenERP, Open Source Management Solution
#    This module copyright (C) 2017 Marlon Falcón Hernandez
#    (<http://www.falconsolutions.cl>).
#
#    This program is free software: you can redistribute it and/or modify
#    it under the terms of the GNU Affero General Public License as
#    published by the Free Software Foundation, either version 3 of the
#    License, or (at your option) any later version.
#
#    This program is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU Affero General Public License for more details.
#
#    You should have received a copy of the GNU Affero General Public License
#    along with this program.  If not, see <http://www.gnu.org/licenses/>.
#
##############################################################################

{
    'name': 'Credit and Debit Notes For Panama MFH',
    'version': '10.0.0.1.0',
    'author': "Falcón Solutions SpA",
    'maintainer': 'Falcon Solutions',
    'website': 'http://www.falconsolutions.cl',
    'license': 'AGPL-3',
    'category': 'Account',
    'summary': 'Invoice refund...',
    'depends': ['sale','purchase','account'],
    'description': """
Contabilidad: Nota de Debito y Credito
=============================================================================
- Adds the menus of "credit and debit note" in customer and supplier invoices.
- Add buttons in form view to generate the credit and debit notes.
        """,
    'data': [
        'data/journal_data.xml',
        'wizards/account_invoice_refund_view.xml',
        'wizards/account_invoice_debit_view.xml',
        'views/account_invoice_view.xml',
        'views/account_view.xml',
    ],
    'installable': True,
}
