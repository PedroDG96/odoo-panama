# -*- encoding: utf-8 -*-
##############################################################################
#
#    OpenERP, Open Source Management Solution
#    Copyright (C) 2004-2010 Tiny SPRL (<http://tiny.be>).
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
    'name': 'Panama Payroll Base',
    'category': 'Localization',
    'author': u'''Marlon Falcon Hernandez''',
    'website': 'http://falconsolutions.cl',
    'depends': ['hr_payroll'],
    'license': 'AGPL-3',
    'version': '10.0.0',
    'description': """HR Payroll""",
    'active': True,
    'data': [#'views/hr_news_views.xml',
             #'views/hr_employee_views.xml',
             #'wizard/hr_news_employee_wizard.xml',
             #'views/hr_salary_books.xml',
             #'views/report_hrsalarybymonth.xml',
             #'report/hr_payroll_report.xml',
             #'report/report_payslip_templates.xml',
             'data/l10n_pa_hr_payroll_data.xml',
             ],
    'demo': ['demo/l10n_pa_hr_payroll_demo.xml',],
    'installable': True,
}

