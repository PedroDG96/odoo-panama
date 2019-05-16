Public Enum TIPO_DOC_FISCAL
    Factura
    ND
    NC
End Enum


Public Class DatosImpresion

    Private _ruc As String
    Public Property RUC() As String
        Get
            Return _ruc
        End Get
        Set(ByVal value As String)
            _ruc = value
        End Set
    End Property

    Private _razonSocial As String
    Public Property RazonSocial() As String
        Get
            Return _razonSocial
        End Get
        Set(ByVal value As String)
            _razonSocial = value
        End Set
    End Property


    Private _tipoDoc As TIPO_DOC_FISCAL
    Public Property TipoDocFiscal() As TIPO_DOC_FISCAL
        Get
            Return _tipoDoc
        End Get
        Set(ByVal value As TIPO_DOC_FISCAL)
            _tipoDoc = value
        End Set
    End Property

    Private _listDetalle As List(Of DatosImpresionDetalle)
    Public Property ListaDetalles() As List(Of DatosImpresionDetalle)
        Get
            Return _listDetalle
        End Get
        Set(ByVal value As List(Of DatosImpresionDetalle))
            _listDetalle = value
        End Set
    End Property

    Private _reimpresion As Boolean
    Public Property Reimpresion() As Boolean
        Get
            Return _reimpresion
        End Get
        Set(ByVal value As Boolean)
            _reimpresion = value
        End Set
    End Property

    Private _Numero As String
    Public Property Numero() As String
        Get
            Return _Numero
        End Get
        Set(ByVal value As String)
            _Numero = value
        End Set
    End Property


    Private _numRef As String
    Public Property DocReferencia() As String
        Get
            Return _numRef
        End Get
        Set(ByVal value As String)
            _numRef = value
        End Set
    End Property

    Private _numeroOdoo As String
    Public Property NumeroOdoo() As String
        Get
            Return _numeroOdoo
        End Get
        Set(ByVal value As String)
            _numeroOdoo = value
        End Set
    End Property


End Class

Public Class DatosImpresionDetalle

    Enum IMPUESTO
        Exento
        ITBMS7
        ITBMS10
        ITBMS15
    End Enum

    Private _cantidad As Decimal
    Public Property Cantidad() As Decimal
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Decimal)
            _cantidad = value
        End Set
    End Property

    Private _precio As Decimal
    Public Property Precio() As Decimal
        Get
            Return _precio
        End Get
        Set(ByVal value As Decimal)
            _precio = value
        End Set
    End Property

    Private _impuesto As IMPUESTO
    Public Property TipoImpuesto() As IMPUESTO
        Get
            Return _impuesto
        End Get
        Set(ByVal value As IMPUESTO)
            _impuesto = value
        End Set
    End Property

    Private _descripcion As String
    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

End Class

Public Class RespuestaImpresion

    Private _documento As String
    Public Property Documento() As String
        Get
            Return _documento
        End Get
        Set(ByVal value As String)
            _documento = value
        End Set
    End Property

    Private _serie As String
    Public Property Serie() As String
        Get
            Return _serie
        End Get
        Set(ByVal value As String)
            _serie = value
        End Set
    End Property

    Private _numComprobante As String
    Public Property NumComprobanteFiscal() As String
        Get
            Return _numComprobante
        End Get
        Set(ByVal value As String)
            _numComprobante = value
        End Set
    End Property

    Private _error As String
    Public Property MensajeError() As String
        Get
            Return _error
        End Get
        Set(ByVal value As String)
            _error = value
        End Set
    End Property

    Private _fecha As DateTime
    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property


End Class
