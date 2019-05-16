# -*- encoding: utf-8 -*-
##############################################################################
#
#    Odoo, Open Source Management Solution Chilean Payroll
#
#    Copyright (c) 2015 Blanco Martin y Asociados - Nelson Ramírez Sánchez
#    Daniel Blanco
#    http://blancomartin.cl
#
#    This program is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    This program is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU General Public License for more details.
#
#    You should have received a copy of the GNU General Public License
#    along with this program.  If not, see <http://www.gnu.org/licenses/>.
#
##############################################################################

import time
from odoo import models
from odoo.exceptions import UserError
from odoo.report import report_sxw


class report_hr_salary_employee_bymonth(report_sxw.rml_parse):

    def __init__(self, cr, uid, name, context):
        super(report_hr_salary_employee_bymonth, self).__init__(
            cr, uid, name, context=context)

        self.localcontext.update({
            'time': time,
            'get_employee': self.get_employee,
            'get_total_salary': self.get_total_salary,
            'get_total_wage': self.get_total_wage,
            'get_total_salary_daily': self.get_total_salary_daily,
        })

        self.context = context
        self.total = 0.0

    def get_worked_days(self, emp_salary, emp_id, date_from, date_to):

        self.cr.execute(
            '''select sum(number_of_days) from hr_payslip_worked_days as p
        left join hr_payslip as r on r.id = p.payslip_id
        where r.employee_id = %s  and (date_from >= %s)
        and (date_to <= %s) and ('WORK100' = p.code)''', (emp_id, date_from, date_to,))

        max = self.cr.fetchone()

        if max is None:
            emp_salary.append(0.00)

        emp_salary.append(max[0])

        return emp_salary

    def get_total_wage(self, date_from, date_to):

        self.cr.execute(
            '''select COALESCE(SUM(r.wage),0)
        from hr_payslip as p left join hr_employee as emp on emp.id = p.employee_id
        left join hr_contract as r on r.id = p.contract_id
        where (date_from >= %s)and (date_to <= %s)''', (date_from, date_to,))

        max = self.cr.fetchone()

        if max is None:
            return 0.0

        return max[0]

    def get_salary(self, emp_id, emp_salary, cod_id, date_from, date_to):

        self.cr.execute(
            '''select COALESCE(sum(pl.total),0) from hr_payslip_line as pl
        left join hr_payslip as p on pl.slip_id = p.id
        left join hr_employee as emp on emp.id = p.employee_id
        left join resource_resource as r on r.id = emp.resource_id
        where p.employee_id = %s and (pl.code like %s)
        and (date_from >= %s) and (date_to <= %s)
        group by r.name, p.date_to,emp.id''', (emp_id, cod_id, date_from, date_to,))

        max = self.cr.fetchone()

        if max is None:
            emp_salary.append(0.00)
        else:
            emp_salary.append(max[0])

        return emp_salary

    def get_total_salary(self, cod_id, date_from, date_to):

        self.cr.execute(
                '''select COALESCE(SUM(pl.total),0) from hr_payslip_line as pl
        left join hr_payslip as p on pl.slip_id = p.id
        left join hr_employee as emp on emp.id = p.employee_id
        left join resource_resource as r on r.id = emp.resource_id
        where (pl.code like %s)
        and (date_from >= %s) and (date_to <= %s)
        group by r.name, p.date_to,emp.id''', (cod_id, date_from, date_to,))

        max = self.cr.fetchone()

        if max is None:
            return 0.0

        return max[0]

    def get_total_salary_daily(self, date_from, date_to):

        if not date_from or not date_to:
            raise UserError('No Existe Nomina Para el Periodo.')

        self.cr.execute(
            '''select sum(number_of_days) from hr_payslip_worked_days as p
        left join hr_payslip as r on r.id = p.payslip_id
        where (date_from >= %s) and (date_to <= %s) and ('WORK100' = p.code)''',
            (date_from, date_to,))

        max = self.cr.fetchone()

        if max[0] is None:
            return 0.0

        salary = self.get_total_salary('SUELDO',date_from, date_to)

        salary = salary / max[0]

        return round(salary,2)

    def get_employee(self, form):
        emp_salary = []
        salary_list = []
        cont = 0

        self.cr.execute(
            '''select emp.id, emp.identification_id, emp.name_related, r.wage
        from hr_payslip as p left join hr_employee as emp on emp.id = p.employee_id
        left join hr_contract as r on r.id = p.contract_id
        where (date_from >= %s)
        and (date_to <= %s)
        group by emp.id, emp.name_related, emp.identification_id, r.wage
        order by name_related''', (form['first_date'], form['end_date'],))

        id_data = self.cr.fetchall()
        if id_data is None:
            emp_salary.append(0.00)
            emp_salary.append(0.00)
            emp_salary.append(0.00)
            emp_salary.append(0.00)
            emp_salary.append(0.00)
        else:
            for index in id_data:
                emp_salary.append(id_data[cont][0])
                emp_salary.append(id_data[cont][1])
                emp_salary.append(id_data[cont][2])
                emp_salary.append(id_data[cont][3])
                emp_salary = self.get_worked_days(emp_salary, id_data[cont][0], form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'SUELDO', form['first_date'],form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'BONO', form['first_date'],form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'VIATICO', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'HOREXT', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'DECIMO', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'TOTOTRING', form['first_date'],form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'TOTING', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'SEGSOC', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'SEGEDU', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'ISLR', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'BAC', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'CABANI', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'CREDCORP', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'DESCCEL', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'PRESTISLR', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'PRESTDESC', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'TOTDESC', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'APTEMP', form['first_date'], form['end_date'])
                emp_salary = self.get_salary(id_data[cont][0], emp_salary, 'TOTPAGO', form['first_date'], form['end_date'])

                cont = cont + 1
                salary_list.append(emp_salary)

                emp_salary = []

        return salary_list

    def get_company(self,form):
        company = self.env['res.company']._company_default_get()

        return company


class wrapped_report_employee_salary_bymonth(models.TransientModel):
    _name = 'report.l10n_pa_hr_payroll.report_hrsalarybymonth'
    _inherit = 'report.abstract_report'
    _template = 'l10n_pa_hr_payroll.report_hrsalarybymonth'
    _wrapped_report_class = report_hr_salary_employee_bymonth
