# -*- coding: utf-8 -*-
{
    'name': 'Panamá Check Printing',
    'version': '1.0',
    'author': 'Falcón Solutions',
    'maintainer': 'Falcon Solutions',
    'category': 'Localization',
    'summary': 'Impresión de Cheques para localización de Panamá',
    'website': 'http://www.falconsolutions.cl',
    'depends': ['account_accountant'],
    'data': [
        'data/payment_method_data.xml',
        'data/res_bank.xml',
        'data/account_data.xml',
        'wizard/account_check_action_wizard_view.xml',
        'views/res_company_view.xml',
        'views/account_check_view.xml',
        'views/account_journal_view.xml',
        'views/account_checkbook_view.xml',
        'views/account_payment_view.xml',
        'views/account_chart_template_view.xml',
        'views/account_register_payment_view.xml'
    ],
    'installable': True,
    'auto_install': False,
    'license': 'AGPL-3',
}
