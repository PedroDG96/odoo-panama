Imports ITD.Log
Public Class Fiscal

    Public _conexionImpresora As TFHKADIR.Tfhka
    Public ReadOnly Property ConexionImpresora() As TFHKADIR.Tfhka
        Get
            Return _conexionImpresora
        End Get
    End Property

    Dim strPuerto As String

    Public Sub New(puerto As String)
        _conexionImpresora = New TFHKADIR.Tfhka(puerto)
        strPuerto = puerto
    End Sub

    Public Sub AbrirPuerto()
        _conexionImpresora.CloseFpctrl()
        _conexionImpresora.OpenFpctrl(strPuerto)
    End Sub

    Public Sub CerrarPuerto()
        _conexionImpresora.CloseFpctrl()
    End Sub

    Public Function Imprimir(datos As DatosImpresion) As RespuestaImpresion
        Dim objFinal As RespuestaImpresion = New RespuestaImpresion()


        ManejadorLog.Log.InfoFormat("Imprimiendo")
        If datos.Reimpresion Then
            Dim strModo As String = ""

            Select Case datos.TipoDocFiscal
                Case TIPO_DOC_FISCAL.Factura
                    strModo = "F"
                Case TIPO_DOC_FISCAL.NC
                    strModo = "C"
                Case TIPO_DOC_FISCAL.ND
                    strModo = "D"
            End Select

            Dim strComando As String = "R" & strModo & datos.Numero.ToString().PadLeft(7, "0") & datos.Numero.ToString().PadLeft(7, "0")

            objFinal = Comandos(strComando)

            If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
                Return objFinal
            End If
        Else

            ManejadorLog.Log.InfoFormat("Agregando los datos en comandos")
            objFinal = AgregarReglones(datos)

            If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
                Return objFinal
            End If

            objFinal = Comandos("3")

            If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
                ManejadorLog.Log.ErrorFormat("Error agregando el comando 3: {0}", objFinal.MensajeError)
                Return objFinal
            End If

            objFinal = Comandos("101")

            If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
                ManejadorLog.Log.ErrorFormat("Error agregando el comando 101: {0}", objFinal.MensajeError)
                Return objFinal
            End If
        End If


        Return DatosImpresora(datos)

    End Function

    Private Function AgregarReglones(doc As DatosImpresion) As RespuestaImpresion
        Dim objFinal As RespuestaImpresion = New RespuestaImpresion()
        Dim strImpuesto As String = Nothing
        Dim strImpuestoNota As String = Nothing
        Dim strInicial As String = Nothing
        Dim strDatos As String
        'Mandamos el RUC
        strDatos = "jR" & doc.RUC
        objFinal = Comandos(strDatos)

        If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
            ManejadorLog.Log.ErrorFormat("Error agregando el RUC: {1} {0}", objFinal.MensajeError, strDatos)
            Return objFinal
        End If

        If (doc.RazonSocial.Length > 40) Then
            doc.RazonSocial = doc.RazonSocial.Substring(0, 40)
        End If


        'Mandamos la razon social
        strDatos = "jS" & doc.RazonSocial.PadRight(40 - doc.RazonSocial.Length)
        objFinal = Comandos(strDatos)

        If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
            ManejadorLog.Log.ErrorFormat("Error agregando la razon social:{1} {0}", objFinal.MensajeError, strDatos)
            Return objFinal
        End If

        strDatos = "j1Num Odoo: " & doc.NumeroOdoo
        objFinal = Comandos(strDatos)

        If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
            ManejadorLog.Log.ErrorFormat("Error agregando el numero de Odoo:{1} {0}", objFinal.MensajeError, strDatos)
            Return objFinal
        End If

        'Falta el documento asociado cuando es una nota de credito o de debito
        If (Not String.IsNullOrWhiteSpace(doc.DocReferencia)) Then
            strDatos = "jF" & doc.DocReferencia
            objFinal = Comandos(strDatos)

            If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
                ManejadorLog.Log.ErrorFormat("Error agregando el documento de referencia:{1} {0}", objFinal.MensajeError, strDatos)
                Return objFinal
            End If
        End If

        For Each linea As DatosImpresionDetalle In doc.ListaDetalles
            Select Case linea.TipoImpuesto
                Case DatosImpresionDetalle.IMPUESTO.Exento
                    strImpuesto = " "

                Case DatosImpresionDetalle.IMPUESTO.ITBMS10
                    strImpuesto = """"
                Case DatosImpresionDetalle.IMPUESTO.ITBMS7
                    strImpuesto = "!"
                Case DatosImpresionDetalle.IMPUESTO.ITBMS15
                    strImpuesto = "#"
            End Select
            strImpuestoNota = CInt(linea.TipoImpuesto).ToString()
            Select Case doc.TipoDocFiscal
                Case TIPO_DOC_FISCAL.Factura
                    strInicial = strImpuesto
                Case TIPO_DOC_FISCAL.ND
                    strInicial = HexStringToString("60") & strImpuestoNota
                Case TIPO_DOC_FISCAL.NC
                    strInicial = "d" & strImpuestoNota
                Case Else

            End Select

            Dim strPrecio As String = linea.Precio.ToString()
            Dim strCantidad As String = linea.Cantidad.ToString()

            If strPrecio.IndexOf(".") < 0 Then
                strPrecio = strPrecio & "00"
            Else
                strPrecio = Replace(strPrecio, ".", "")
            End If

            If strCantidad.IndexOf(".") < 0 Then
                strCantidad = strCantidad & "000"
            Else
                strCantidad = Replace(strCantidad, ".", "")
            End If

            strPrecio = strPrecio.PadLeft(10, "0")
            strCantidad = strCantidad.PadLeft(8, "0")

            strDatos = strInicial & strPrecio & strCantidad & linea.Descripcion

            ManejadorLog.Log.InfoFormat("Detalle: {0}", strDatos)
            objFinal = Comandos(strDatos)

            If Not String.IsNullOrWhiteSpace(objFinal.MensajeError) Then
                ManejadorLog.Log.ErrorFormat("Error agregando una linea de detalle:{1} {0}", objFinal.MensajeError, strDatos)
                Exit For
            End If

        Next

        Return objFinal

    End Function

    Private Function Comandos(strValor As String) As RespuestaImpresion
        Dim objError As New RespuestaImpresion()
        Try
            If Not ConexionImpresora.SendCmd(strValor) Then
                Dim estaPrint As TFHKADIR.PrinterStatus
                estaPrint = ConexionImpresora.getPrinterStatus()
                objError.MensajeError = String.Format("Estado: {0} / Error: {1}", estado(estaPrint.PrinterStatusCode), estado(estaPrint.PrinterStatusCode))
            End If
        Catch ex As Exception
            objError.MensajeError = "Error no controlado: {0}" & ex.Message
        End Try

        Return objError
    End Function

    Private Function errores(ByVal codigo As Integer) As String

        Try
            Select Case codigo
                Case 0
                    Return "No hay Error"
                Case 1
                    Return "Fin en la entrega del papel"
                Case 2
                    Return "Error de indole mecanico en la entrega de papel"
                Case 3
                    Return "Fin en la entrega del papel y error mecanico"
                Case 80
                    Return "Comando Invalido Valor invalido"
                Case 84
                    Return "Tasa invalida"
                Case 88
                    Return "No hay directiva asignadas"
                Case 92
                    Return "Comando Invalido"
                Case 96
                    Return "Error Fiscal"
                Case 100
                    Return "Error en la memoria fiscal"
                Case 108
                    Return "Memoria llena"
                Case 112
                    Return "Buffer Completo (Reiniciar la impresora)"
                Case 128
                    Return "Error en la comunicacion"
                Case 137
                    Return "No hay Respuesta"
                Case 144
                    Return "Error RLC"
                Case 145
                    Return "Error interno de la API"
                Case 153
                    Return "Error en la apertura del archivo"
            End Select
            Return "No hay ese codigo en esta funcion"

        Catch ex As Exception

            Return ex.Message


        End Try
    End Function

    Private Function estado(ByVal codigo As Integer) As String

        Try
            Select Case codigo
                Case 0
                    Return "Estado desconocido"
                Case 1
                    Return "En modo de prueba y en espera"
                Case 2
                    Return "En modo de prueba y emision de documento fiscales"
                Case 3
                    Return "En modo de prueba y emision de documento no fiscales"
                Case 4
                    Return "En modo fiscal y en espera"
                Case 5
                    Return "En modo fiscal y emision de documento fiscales"
                Case 6
                    Return "En modo fiscal y emision de documento no fiscales"
                Case 7
                    Return "En modo fiscal y memoria casi llena y en espera"
                Case 8
                    Return "En modo fiscal y memoria casi llena y emision de documentos fiscales"
                Case 9
                    Return "En modo fiscal y memoria casi llena y emision de documentos no fiscales"
                Case 10
                    Return "En modo fiscal y memoria llena y en espera"
                Case 11
                    Return "En modo fiscal y memoria llena y emision de documentos fiscales"
                Case 12
                    Return "En modo fiscal y memoria llena y emision de documentos no fiscales"
            End Select
            Return "No hay ese codigo en esta funcion"

        Catch ex As Exception

            Return ex.Message


        End Try
    End Function

    Private Function DatosImpresora(objDatos As DatosImpresion) As RespuestaImpresion
        Dim objRes As RespuestaImpresion = New RespuestaImpresion()
        Try
            If (ConexionImpresora.CheckFprinter()) Then
                ConexionImpresora.getS1PrinterData()

                Select Case objDatos.TipoDocFiscal

                    Case TIPO_DOC_FISCAL.Factura
                        objRes.NumComprobanteFiscal = ConexionImpresora.S1Estado1.getLastInvoiceNumber().ToString()
                    Case TIPO_DOC_FISCAL.NC
                        objRes.NumComprobanteFiscal = ConexionImpresora.S1Estado1.getLastNCNumber().ToString()
                    Case TIPO_DOC_FISCAL.ND
                        objRes.NumComprobanteFiscal = ConexionImpresora.S1Estado1.getLastNDNumber().ToString()
                    Case Else

                End Select

                objRes.Serie = ConexionImpresora.S1Estado1.getRegisteredMachineNumber().ToString()
                objRes.Fecha = ConexionImpresora.S1Estado1.getCurrentPrinterDateTime()

                'cajero = tfhka.S1Estado1.getCashierNumber().ToString()
                'no_memoria_fiscal = tfhka.S1Estado1.getFiscalReportsCounter().ToString()
                'fecha_actual = tfhka.S1Estado1.getCurrentPrinterDateTime().ToString()
                'cortesZ = tfhka.S1Estado1.getDailyClosureCounter().ToString()
                'm_dv = tfhka.S1Estado1.getDV().ToString()
                'ult_factura = tfhka.S1Estado1.getLastInvoiceNumber().ToString()
                'ult_nota_credito = tfhka.S1Estado1.getLastNCNumber().ToString()
                'ult_nota_debito = tfhka.S1Estado1.getLastNDNumber().ToString()
                'no_dnf = tfhka.S1Estado1.getNumberNonFiscalDocuments().ToString()
                'ult_doc_no_fiscal = tfhka.S1Estado1.getQuantityNonFiscalDocuments().ToString()
                'no_fac = tfhka.S1Estado1.getQuantityOfInvoicesToday().ToString()
                'no_credito = tfhka.S1Estado1.getQuantityOfNCsToday().ToString()
                'no_debito = tfhka.S1Estado1.getQuantityOfNDsToday().ToString()
                'serial = tfhka.S1Estado1.getRegisteredMachineNumber().ToString()
                'm_ruc = tfhka.S1Estado1.getRUC().ToString()
                'ventas_dia = "Total de Ventas del dia = " + tfhka.S1Estado1.getTotalDailySales().ToString()


            End If
        Catch ex As Exception
            objRes.MensajeError = ex.Message
        End Try


        Return objRes
    End Function

    Private Function HexStringToString(HexString As String) As String
        Dim stringValue As String = ""
        For i As Integer = 0 To HexString.Length / 2 - 1
            Dim hexChar As String = HexString.Substring(i * 2, 2)
            Dim hexValue As Integer = Convert.ToInt32(hexChar, 16)
            stringValue += [Char].ConvertFromUtf32(hexValue)
        Next
        Return stringValue
    End Function

End Class
