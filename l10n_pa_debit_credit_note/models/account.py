# -*- coding: utf-8 -*-

from odoo import models, api, fields, _


class AccountJournal(models.Model):
    _inherit = "account.journal"

    debit_note_sequence_id = fields.Many2one('ir.sequence', string='Debit Note Entry Sequence',
                                         help="This field contains the information related to the numbering of the debit note entries of this journal.",
                                         copy=False)

    debit_note_sequence = fields.Boolean(string='Dedicated Debit Note Sequence',
                                     help="Check this box if you don't want to share the same sequence for invoices and refunds and debit notes made from this journal",
                                     default=False)

    @api.multi
    def write(self, vals):
        for journal in self.filtered(lambda j: j.type in ('sale', 'purchase')):
            if not journal.refund_sequence:
                if not journal.refund_sequence_id:
                    journal_vals = {
                        'name': 'Nota De Credito - ' + journal.name,
                        'company_id': journal.company_id.id,
                        'code': 'NC'
                    }
                    vals['refund_sequence'] = True
                    vals['refund_sequence_id'] = self.sudo()._create_sequence(journal_vals, refund=False).id

                if not journal.debit_note_sequence:
                    if not journal.debit_note_sequence_id:
                        journal_vals = {
                            'name': 'Nota De Debito - ' + journal.name,
                            'company_id': journal.company_id.id,
                            'code': 'ND'
                        }
                        vals['debit_note_sequence'] = True
                        vals['debit_note_sequence_id'] = self.sudo()._create_sequence(journal_vals, refund=False).id

        result = super(AccountJournal, self).write(vals)
        return result
