
from odoo.report import report_sxw
from amount_to_text_es import amount_to_text_es
from odoo import models


class report_payslip(report_sxw.rml_parse):

    def __init__(self, cr, uid, name, context):
        super(report_payslip, self).__init__(cr, uid, name, context)
        self.localcontext.update({
        	'get_payslip_lines': self.get_payslip_lines,
            'convert': self.convert,
            'get_leave':self.get_leave,
        })


    def convert(self,amount, cur):
        amt_en = amount_to_text_es(amount, cur)
        return amt_en


    def get_payslip_lines(self, obj):
        payslip_line = self.pool.get('hr.payslip.line')
        res = []
        ids = []
        for id in range(len(obj)):
            if obj[id].appears_on_payslip is True:
                ids.append(obj[id].id)
        if ids:
            res = payslip_line.browse(self.cr, self.uid, ids)
        return res

    def get_leave(self,obj):
          res = []
          ids = []
          for id in range(len(obj)):
              if obj[id].type == 'leaves':
                 ids.append(obj[id].id)
          payslip_line = self.pool.get('hr.payslip.line')
          if len(ids):
              res = payslip_line.browse(self.cr, self.uid, ids)
          return res


class wrapped_report_payslip(models.TransientModel):
    _name = 'report.hr_payroll.report_payslip'
    _inherit = 'report.abstract_report'
    _template = 'hr_payroll.report_payslip'
    _wrapped_report_class = report_payslip


 


      






