# -*- coding: utf-8 -*-
import time
from datetime import datetime

import babel

from odoo import api, fields, models, tools, _


class HrPayslip(models.Model):
    _inherit = 'hr.payslip'
    _description = 'Pay Slip'

    @api.model
    def get_payroll_total(self):
        return {
            'perceptions': sum([x.total for x in self.line_ids if (x.salary_rule_id.appears_on_payslip and x.total > 0)]),
            'deductions': sum([-1 * x.total for x in self.line_ids if (x.salary_rule_id.appears_on_payslip and x.total < 0)]),
            'totals': sum([x.total for x in self.line_ids if x.salary_rule_id.appears_on_payslip]),
        }

    # YTI TODO To rename. This method is not really an onchange, as it is not in any view
    # employee_id and contract_id could be browse records
    @api.multi
    def onchange_employee_id(self, date_from, date_to, employee_id=False, contract_id=False):
        # defaults
        res = {
            'value': {
                'line_ids': [],
                # delete old input lines
                'input_line_ids': map(lambda x: (2, x,), self.input_line_ids.ids),
                # delete old worked days lines
                'worked_days_line_ids': map(lambda x: (2, x,), self.worked_days_line_ids.ids),
                # 'details_by_salary_head':[], TODO put me back
                'name': '',
                'contract_id': False,
                'struct_id': False,
            }
        }
        if (not employee_id) or (not date_from) or (not date_to):
            return res
        ttyme = datetime.fromtimestamp(time.mktime(time.strptime(date_from, "%Y-%m-%d")))
        employee = self.env['hr.employee'].browse(employee_id)
        locale = self.env.context.get('lang', 'en_US')
        res['value'].update({
            'name': _('Salary Slip of %s for %s') % (
            employee.name, tools.ustr(babel.dates.format_date(date=ttyme, format='MMMM-y', locale=locale))),
            'company_id': employee.company_id.id,
        })

        if not self.env.context.get('contract'):
            # fill with the first contract of the employee
            contract_ids = self.get_contract(employee, date_from, date_to)
        else:
            if contract_id:
                # set the list of contract for which the input have to be filled
                contract_ids = [contract_id]
            else:
                # if we don't give the contract, then the input to fill should be for all current contracts of the employee
                contract_ids = self.get_contract(employee, date_from, date_to)

        if not contract_ids:
            return res
        contract = self.env['hr.contract'].browse(contract_ids[0])
        res['value'].update({
            'contract_id': contract.id
        })
        struct = contract.struct_id
        if not struct:
            return res
        res['value'].update({
            'struct_id': struct.id,
        })
        # computation of the salary input
        worked_days_line_ids = self.get_worked_day_lines(contract_ids, date_from, date_to)
        input_line_ids = self.get_inputs(employee_id, contract_ids, date_from, date_to)
        res['value'].update({
            'worked_days_line_ids': worked_days_line_ids,
            'input_line_ids': input_line_ids,
        })
        return res

    @api.model
    def get_inputs(self, employee_id, contract_ids, date_from, date_to):
        res = []

        contracts = self.env['hr.contract'].browse(contract_ids)
        structure_ids = contracts.get_all_structures()
        rule_ids = self.env['hr.payroll.structure'].browse(structure_ids).get_all_rules()
        sorted_rule_ids = [id for id, sequence in sorted(rule_ids, key=lambda x: x[1])]
        inputs = self.env['hr.salary.rule'].browse(sorted_rule_ids).mapped('input_ids')

        for contract in contracts:
            for input in inputs:
                news = self.env['hr.news'].search([('inputs_id','=',input.id)])
                amount = 0.0

                for new_id in news:
                    employee_news = self.env['hr.employee.news'].search([('employee_id', '=', employee_id), ('news_id', '=', new_id.id),
                                                            ('date_from', '<=', date_from),
                                                            ('date_to', '=', False)])
                    for new_emp in employee_news:
                        amount += new_emp.amount

                    employee_news = self.env['hr.employee.news'].search([('employee_id', '=', employee_id),
                                                                       ('news_id', '=', new_id.id),
                                                                        ('date_from', '<=', date_from),
                                                                        ('date_to', '>=', date_to)])
                    for new_emp in employee_news:
                        amount += new_emp.amount

                    employee_news = self.env['hr.employee.news'].search([('employee_id', '=', employee_id),
                                                                         ('news_id', '=', new_id.id),
                                                                         ('date_from', '>=', date_from),
                                                                         ('date_to', '<=', date_to)])
                    for new_emp in employee_news:
                        amount += new_emp.amount

                credits = self.env['hr.credit'].search([('inputs_id', '=', input.id),('employee_id', '=', employee_id),
                                                        ('state', '=', 'done')])

                for credit in credits:
                    fees = self.env['hr.credit.line'].search([('credit_id', '=', credit.id),
                                                            ('discount_date', '<=', date_to),
                                                            ('ispaid', '=', False)])
                    for fee in fees:
                        amount += fee.amount

                input_data = {
                    'name': input.name,
                    'code': input.code,
                    'amount': amount,
                    'contract_id': contract.id,
                }
                res += [input_data]
        return res

    @api.multi
    def action_payslip_done(self):
        inputs = (input for input in self.input_line_ids if input.amount > 0)

        for input in inputs:
            rules = self.env['hr.rule.input'].search([('code','=',input.code)])

            for rule in rules:
                credits = self.env['hr.credit'].search([('inputs_id', '=', rule.id), ('employee_id', '=', self.employee_id.id)])

                for credit in credits:
                    amount = input.amount
                    fees = self.env['hr.credit.line'].search([('credit_id', '=', credit.id),
                                                           ('discount_date', '<=', self.date_to),
                                                           ('ispaid', '=', False)])
                    paid_amount = 0.0
                    for fee in fees:
                        paid_amount -= fee.amount

                        if amount >= 0:
                            paid_amount += fee.amount
                            paid_amount.write({'ispaid': True})

                            credit.write({'paid_amount': credit.paid_amount + paid_amount})
                    credit._amount_total()

        return super(HrPayslip, self).action_payslip_done()