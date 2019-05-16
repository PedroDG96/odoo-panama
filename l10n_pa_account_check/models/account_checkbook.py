# coding: utf-8
from odoo import _, api, fields, models
from odoo.exceptions import ValidationError


class AccountCheckbook(models.Model):

    _name = 'account.checkbook'
    _description = 'Account Checkbook'

    name = fields.Char('Nombre', compute='_compute_name')
    sequence_id = fields.Many2one(
        'ir.sequence',
        'Secuencia',
        readonly=True,
        copy=False,
        domain=[('code', '=', 'issue_check')],
        help="Checks numbering sequence.",
        context={'default_code': 'issue_check'},
        states={'draft': [('readonly', False)]},
    )
    next_number = fields.Integer(
        'Próximo Número',
        related='sequence_id.number_next_actual',
    )
    issue_check_subtype = fields.Selection(
        [('deferred', 'Diferidos'),
         ('currents', 'Corrientes')],
        string='Subtipo Cheques',
        readonly=True,
        required=True,
        default='deferred',
        help='* Con cheques corrientes el asiento generado por el pago '
        'descontará directamente de la cuenta de banco y además la fecha de '
        'pago no es obligatoria.\n'
        '* Con cheques diferidos el asiento generado por el pago se hará '
        'contra la cuenta definida para tal fin en la compañía, luego será '
        'necesario el asiento de débito que se puede generar desde el extracto'
        'o desde el cheque.',
        states={'draft': [('readonly', False)]}
    )

    journal_id = fields.Many2one(
        'account.journal', 'Diario',
        help='Journal where it is going to be used',
        readonly=True,
        required=True,
        domain=[('type', '=', 'bank')],
        ondelete='cascade',
        context={'default_type': 'bank'},
        states={'draft': [('readonly', False)]}
    )
    range_to = fields.Integer(
        'Ultimo Número',
        readonly=True,
        states={'draft': [('readonly', False)]},
        help='Si establece un número aquí, esta chequera será automáticamente'
        'utilizada hasta alcanzar este número.'
    )
    issue_check_ids = fields.One2many(
        'account.check',
        'checkbook_id',
        string='Cheques Propios',  # cheques emitidos - Issue Checks
        readonly=True,
    )
    state = fields.Selection(
        [('draft', 'Borrador'),
         ('active', 'En Uso'),
         ('used', 'Usado')],
        string='Status',
        # readonly=True,
        default='draft',
        copy=False
    )
    block_manual_number = fields.Boolean(
        readonly=True,
        default=True,
        string='Bloquear Número Manual?',
        states={'draft': [('readonly', False)]},
        help='Bloquea al Usuario para introducir manualmente otro número que no sea el sugerido'
    )

    @api.model
    def create(self, vals):
        rec = super(AccountCheckbook, self).create(vals)
        if not rec.sequence_id:
            rec._create_sequence()
        return rec

    @api.one
    def _create_sequence(self):
        """ Create a check sequence for the checkbook """
        self.sequence_id = self.env['ir.sequence'].sudo().create({
            'name': '%s - %s' % (self.journal_id.name, self.name),
            'implementation': 'no_gap',
            'padding': 8,
            'number_increment': 1,
            'code': 'issue_check',
            'company_id': self.journal_id.company_id.id,
        })

    @api.multi
    def _compute_name(self):
        for rec in self:
            if rec.issue_check_subtype == 'deferred':
                name = _('Cheques Diferidos')
            else:
                name = _('Cheques Corrientes')
            if rec.range_to:
                name += _(' hasta %s') % rec.range_to
            rec.name = name

    @api.one
    def unlink(self):
        if self.issue_check_ids:
            raise ValidationError(
                _('No puede eliminar una Chequera si ya ha sido usado algún cheque.!'))
        return super(AccountCheckbook, self).unlink()
