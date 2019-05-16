using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Connect;
using ITD.Log;

namespace APP {
  public partial class Factura : Form {

    #region Declaraciones

    public OdooAPI API { get; private set; }
    public OdooModel FacturaModel { get; private set; }
    public OdooModel ImpresoraModel { get; private set; }
    public OdooModel DetalleModel { get; private set; }
    public OdooModel ImpuestosModel { get; private set; }
    public OdooModel PartnerModel { get; private set; }

    public DataTable TablaGrilla { get; private set; }
    public DataTable TablaImpresora { get; private set; }
    public DataTable TablaDetalles { get; private set; }
    public DataTable TablaImpuestos { get; private set; }

    #endregion

    #region Inicial

    public Factura() {
      InitializeComponent();
      dtpFiltroFecha.Value = dtpFechaHasta.Value = DateTime.Now.Date;

      Configuracion objConf = new Configuracion();
      if (!objConf.IsDisposed && objConf.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
        this.Close();
        return;
      }

      OdooConnectionCredentials creds = new OdooConnectionCredentials(Global.URL, Global.DB, Global.User, Global.Pass);

      API = new OdooAPI(creds);
      FacturaModel = API.GetModel("account.invoice");
      ImpresoraModel = API.GetModel("l.printer");
      DetalleModel = API.GetModel("account.invoice.line");
      ImpuestosModel = API.GetModel("account.tax");
      PartnerModel = API.GetModel("res.partner");
      dtgPrincipal.AutoGenerateColumns = dtgTodas.AutoGenerateColumns = false;

    }

    private void Factura_Load(object sender, EventArgs e) {

      string[] str = Properties.Resources.Campos_Factura.Replace("\r", "").Split('\n');

      TablaGrilla = new DataTable();
      TablaGrilla.Columns.Add(new DataColumn() { ColumnName = "Marca", DataType = typeof(bool) });
      foreach (string valor in str) {
        string strValor = valor.Split('|')[0];
        FacturaModel.AddField(strValor);
        TablaGrilla.Columns.Add(new DataColumn() { ColumnName = strValor });
      }

      TablaGrilla.Columns.Add(new DataColumn() { ColumnName = "RUC", DataType = typeof(string) });
      TablaGrilla.Columns.Add(new DataColumn() { ColumnName = "data", DataType = typeof(OdooRecord) });
      TablaGrilla.Columns.Add(new DataColumn() { ColumnName = "tipo_doc", DataType = typeof(string) });


      str = Properties.Resources.Campos_Linea_Impresora.Replace("\r", "").Split('\n');

      TablaImpresora = new DataTable();
      foreach (string valor in str) {
        string strCampo = valor.Split('|')[0];
        ImpresoraModel.AddField(strCampo);
        TablaImpresora.Columns.Add(new DataColumn() { ColumnName = strCampo });
      }
      TablaImpresora.Columns.Add(new DataColumn() { ColumnName = "data", DataType = typeof(OdooRecord) });


      str = Properties.Resources.Campos_Detalles.Replace("\r", "").Split('\n');
      TablaDetalles = new DataTable();
      foreach (string valor in str) {
        string strCampo = valor.Split('|')[0];
        DetalleModel.AddField(strCampo);
        TablaDetalles.Columns.Add(new DataColumn() { ColumnName = strCampo });
      }


      str = Properties.Resources.Campos_Impuestos.Replace("\r", "").Split('\n');
      TablaImpuestos = new DataTable();
      foreach (string valor in str) {
        string strCampo = valor.Split('|')[0];
        ImpuestosModel.AddField(strCampo);
        TablaImpuestos.Columns.Add(new DataColumn() { ColumnName = strCampo });
      }


      spcPrincipal.Panel2Collapsed = true;
      this.Size = new Size(676, this.Size.Height);

      tsbBajarFacturas_Click(null, null);
    }

    #endregion

    #region Actualizar

    #endregion

    #region Interfaz

    #endregion

    #region Acciones

    private void InicializarTablas() {

      Cursor = Cursors.WaitCursor;

      TablaGrilla.Clear();
      TablaDetalles.Clear();
      TablaImpresora.Clear();
      TablaImpuestos.Clear();

      #region Facturas
      object[] filter = new object[]{
        new object[]{ "state","!=","proforma" },
        new object[]{ "state","!=","proforma2"},
        new object[]{ "state","!=","cancel"},
        new object[]{ "state","!=","draft"},
        new object[]{ "type","like","out" },
        new object[]{ "date_invoice",">=",dtpFiltroFecha.Value.ToString("yyyy-MM-dd HH:mm:ss") },
        new object[]{ "date_invoice","<",dtpFechaHasta.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") }
      };

      //Inicializar Tablas


      /*****************************************/
      string[] str = Properties.Resources.Campos_Factura.Replace("\r", "").Split('\n');
      List<OdooRecord> records = FacturaModel.Search(filter);

      List<List<object>> lista = new List<List<object>>();

      foreach (OdooRecord record in records) {
        int intIdPartner = 0;
        List<object> listaInterna = new List<object>();
        listaInterna.Add(false);

        foreach (string strDato in str) {
          string[] strValores = strDato.Split('|');
          string strValor = strValores[0];

          object objDato = record.GetValue(strValor);

          if (objDato is object[]) {
            object[] objValArr = objDato as object[];

            if (objValArr.Length == 0)
              listaInterna.Add(DBNull.Value);
            else {
              int intIndex = Convert.ToInt32(strValores[1]);
              listaInterna.Add(objValArr[intIndex]);

              if (strValor == "partner_id") {
                intIdPartner = Convert.ToInt32(objValArr[0]);
              }

            }
          } else {

            if (strDato == "date_invoice") {

              DateTime dt = DateTime.MinValue;
              DateTime.TryParse(objDato.ToString(), out dt);
              objDato = dt.ToString("dd/MM/yyyy");
            } else if (strDato == "amount_total") {
              decimal dec = decimal.MinValue;
              decimal.TryParse(objDato.ToString(), out dec);
              objDato = dec.ToString("$#,##0.00");
            }

            listaInterna.Add(objDato);
          }

        }

        listaInterna.Add(TomarRUCPartner(intIdPartner));

        listaInterna.Add(record);

        lista.Add(listaInterna);

      }

      //dtgPrincipal.Columns.Add(new DataGridViewCheckBoxColumn() { HeaderText = "Marca", DataPropertyName = "Marca", Width = 50 });
      lista.ForEach(s => TablaGrilla.Rows.Add(s.ToArray()));




      TablaGrilla.AsEnumerable().ToList().ForEach(s => s["tipo_doc"] = s["refund_type"].ToString() == "credit" || s["type"].ToString().Contains("refund") ? "Nota de Crédito" : s["refund_type"].ToString() == "debit" ? "Nota de Débito" : "Factura");

      dtgPrincipal.DataSource = new DataView(TablaGrilla, "printer_list_ids is  null", "Id", DataViewRowState.CurrentRows);
      ColFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
      dtgTodas.DataSource = new DataView(TablaGrilla, "printer_list_ids is not null", "Id Desc", DataViewRowState.CurrentRows);

      tsbImprimir.Enabled = dtgPrincipal.Rows.Count > 0;

      #endregion

      #region Referencias de la factura



      foreach (DataRow dtr in TablaGrilla.Rows) {
        str = Properties.Resources.Campos_Linea_Impresora.Replace("\r", "").Split('\n');
        filter = new object[]{
          new object[]{ "account_id.id","=",dtr["Id"].ToString() },
        };

        records = ImpresoraModel.Search(filter);
        List<List<object>> listaImpresora = new List<List<object>>();
        List<List<object>> listaDetalle = new List<List<object>>();

        foreach (OdooRecord record in records) {

          List<object> listaInterna = new List<object>();

          foreach (string strDato in str) {
            string[] strValores = strDato.Split('|');
            string strValor = strValores[0];

            object objDato = record.GetValue(strValor);

            if (objDato is object[]) {
              object[] objValArr = objDato as object[];

              if (objValArr.Length == 0)
                listaInterna.Add(DBNull.Value);
              else {
                int intIndex = Convert.ToInt32(strValores[1]);
                listaInterna.Add(objValArr[intIndex]);
              }
            } else
              listaInterna.Add(objDato);
          }

          listaInterna.Add(record);
          listaImpresora.Add(listaInterna);

        }

        listaImpresora.ForEach(s => TablaImpresora.Rows.Add(s.ToArray()));


        str = Properties.Resources.Campos_Detalles.Replace("\r", "").Split('\n');

        filter = new object[]{
          new object[]{ "invoice_id.id","=",dtr["Id"].ToString() },
        };
        records = DetalleModel.Search(filter);

        foreach (OdooRecord record in records) {

          List<object> listaInterna = new List<object>();

          foreach (string strDato in str) {
            string[] strValores = strDato.Split('|');
            string strValor = strValores[0];

            object objDato = record.GetValue(strValor);

            if (objDato is object[] || objDato is int[]) {
              dynamic objValArr = objDato;

              if (objValArr.Length == 0)
                listaInterna.Add(DBNull.Value);
              else {
                if (strValores.Length > 1) {
                  int intIndex = Convert.ToInt32(strValores[1]);
                  listaInterna.Add(objValArr[intIndex]);
                } else
                  listaInterna.Add(objValArr[0]);
              }
            } else
              listaInterna.Add(objDato);
          }


          listaDetalle.Add(listaInterna);

        }

        listaDetalle.ForEach(s => TablaDetalles.Rows.Add(s.ToArray()));
      }

      #endregion

      #region Impuestos

      str = Properties.Resources.Campos_Impuestos.Replace("\r", "").Split('\n');

      foreach (DataRow dtrDet in TablaDetalles.Rows) {

        if (string.IsNullOrEmpty(dtrDet["invoice_line_tax_ids"].ToString()))
          continue;

        int intIdTax = Convert.ToInt32(dtrDet["invoice_line_tax_ids"]);

        if (TablaImpuestos.AsEnumerable().ToList().Exists(s => Convert.ToInt32(s["id"]) == intIdTax))
          continue;

        filter = new object[]{
            new object[]{ "id","=", intIdTax.ToString() }
          };

        records = ImpuestosModel.Search(filter);

        lista = new List<List<object>>();

        foreach (OdooRecord record in records) {

          List<object> listaInterna = new List<object>();

          foreach (string strDato in str) {
            string[] strValores = strDato.Split('|');
            string strValor = strValores[0];

            object objDato = record.GetValue(strValor);

            if (objDato is object[]) {
              object[] objValArr = objDato as object[];

              if (objValArr.Length == 0)
                listaInterna.Add(DBNull.Value);
              else {
                int intIndex = Convert.ToInt32(strValores[1]);
                listaInterna.Add(objValArr[intIndex]);
              }
            } else
              listaInterna.Add(objDato);
          }


          lista.Add(listaInterna);

        }

        //dtgPrincipal.Columns.Add(new DataGridViewCheckBoxColumn() { HeaderText = "Marca", DataPropertyName = "Marca", Width = 50 });
        lista.ForEach(s => TablaImpuestos.Rows.Add(s.ToArray()));


      }


      //Inicializar Tablas


      /*****************************************/





      #endregion

      Cursor = Cursors.Default;
    }

    private void Imprimir(DataRow dtr) {
      dtr.ClearErrors();

      FiscalPrinter.DatosImpresion objDatos = new FiscalPrinter.DatosImpresion();

      var lista = TablaDetalles.AsEnumerable().Where(s => Convert.ToInt32(s["invoice_id"]) == Convert.ToInt32(dtr["id"])).ToList();

      string tipo = dtr["refund_type"].ToString().Trim();
      objDatos.TipoDocFiscal = tipo == "credit" || dtr["type"].ToString().Contains("refund") ? FiscalPrinter.TIPO_DOC_FISCAL.NC : tipo == "debit" ? FiscalPrinter.TIPO_DOC_FISCAL.ND : FiscalPrinter.TIPO_DOC_FISCAL.Factura;
      objDatos.ListaDetalles = new List<FiscalPrinter.DatosImpresionDetalle>();
      objDatos.RUC = dtr["ruc"].ToString();
      objDatos.RazonSocial = dtr["partner_id"].ToString();
      objDatos.NumeroOdoo = dtr["number"].ToString();

      if (objDatos.TipoDocFiscal == FiscalPrinter.TIPO_DOC_FISCAL.NC || objDatos.TipoDocFiscal == FiscalPrinter.TIPO_DOC_FISCAL.ND) {
        DataRow dtrRef = TablaGrilla.AsEnumerable().Where(s => s["number"].ToString() == dtr["origin"].ToString()).FirstOrDefault();

        if (dtrRef != null) {
          DataRow dtrNumeroRef = TablaImpresora.AsEnumerable().Where(s => !s.IsNull("account_id") && Convert.ToInt32(s["account_id"]) == Convert.ToInt32(dtrRef["id"])).OrderBy(s => Convert.ToInt32(s["id"])).ToList().FirstOrDefault();

          if (dtrNumeroRef != null)
            objDatos.DocReferencia = string.Format("{0}-{1}", dtrNumeroRef["serial_equipo_fiscal"], dtrNumeroRef["numero_comprobante"].ToString().PadLeft(22 - (dtrNumeroRef["serial_equipo_fiscal"].ToString().Length + 1), '0'));
        }
      }


      DataRow dtrReimpresion = TablaImpresora.AsEnumerable().Where(s => !s.IsNull("account_id") && Convert.ToInt32(s["account_id"]) == Convert.ToInt32(dtr["id"])).OrderBy(s => Convert.ToInt32(s["id"])).ToList().FirstOrDefault();
      ManejadorLog.Log.Info("*************************************************");
      ManejadorLog.Log.InfoFormat("Se imprimiran los siguientes datos:");
      ManejadorLog.Log.InfoFormat("RUC: {0}", objDatos.RUC);
      ManejadorLog.Log.InfoFormat("RazonSocial: {0}", objDatos.RazonSocial);
      ManejadorLog.Log.InfoFormat("TipoDocFiscal: {0}", objDatos.TipoDocFiscal);


      if (dtrReimpresion != null) {
        objDatos.Reimpresion = true;
        objDatos.Numero = dtrReimpresion["numero_comprobante"].ToString();
      } else {

        foreach (DataRow item in lista) {
          FiscalPrinter.DatosImpresionDetalle det = new FiscalPrinter.DatosImpresionDetalle();
          det.Cantidad = Convert.ToDecimal(item["quantity"]);
          det.Precio = Convert.ToDecimal(item["price_unit"]);
          det.Descripcion = item["name"].ToString();

          if (item.IsNull("invoice_line_tax_ids")) {
            det.TipoImpuesto = FiscalPrinter.DatosImpresionDetalle.IMPUESTO.Exento;
          } else {

            DataRow dtrImp = TablaImpuestos.AsEnumerable().Where(s => Convert.ToInt32(s["id"]) == Convert.ToInt32(item["invoice_line_tax_ids"])).SingleOrDefault();
            if (dtrImp == null)
              det.TipoImpuesto = FiscalPrinter.DatosImpresionDetalle.IMPUESTO.Exento;
            else {
              if (dtrImp["name"].ToString().Contains("7"))
                det.TipoImpuesto = FiscalPrinter.DatosImpresionDetalle.IMPUESTO.ITBMS7;
              else if (dtrImp["name"].ToString().Contains("10"))
                det.TipoImpuesto = FiscalPrinter.DatosImpresionDetalle.IMPUESTO.ITBMS10;
              else if (dtrImp["name"].ToString().Contains("15"))
                det.TipoImpuesto = FiscalPrinter.DatosImpresionDetalle.IMPUESTO.ITBMS15;
            }

          }

          objDatos.ListaDetalles.Add(det);
        }
      }

      ManejadorLog.Log.InfoFormat("Cantidad Detalles : {0}", objDatos.ListaDetalles.Count);
      var res = Global.Impresora.Imprimir(objDatos);

      dtr.RowError = res.MensajeError;

      if (!dtr.HasErrors) {
        res.Fecha = DateTime.Now;
        MarcarComoImpresa(dtr, res);
        dtr["printer_list_ids"] = 1;
        dtr["Marca"] = false;
        dtr.AcceptChanges();
      }
    }
    
    private void MarcarComoImpresa(DataRow dtr, FiscalPrinter.RespuestaImpresion resultado) {
      DataRow dtrImp = TablaImpresora.AsEnumerable().Where(s => !s.IsNull("account_id") && Convert.ToInt32(s["account_id"]) == Convert.ToInt32(dtr["id"])).FirstOrDefault();
      OdooRecord data = null;
      bool bolReimpresion = true;
      data = ImpresoraModel.CreateNew();
      if (dtrImp == null)
        bolReimpresion = false;

      data.SetValue("account_id", dtr["id"]);
      data.SetValue("numero_comprobante", resultado.NumComprobanteFiscal);
      data.SetValue("reimpresion", bolReimpresion);
      data.SetValue("serial_equipo_fiscal", resultado.Serie);
      data.SetValue("print_date", resultado.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
      data.SetValue("print_user", Global.User);
      data.Save();

      dtrImp = TablaImpresora.NewRow();
      dtrImp["id"] = data.Id;
      dtrImp["account_id"] = dtr["id"];
      dtrImp["numero_comprobante"] = resultado.NumComprobanteFiscal;
      TablaImpresora.Rows.Add(dtrImp);
      TablaImpresora.AcceptChanges();
    }

    private string TomarRUCPartner(int intId) {
      object[] filter = new object[]{
        new object[]{ "id","=",intId.ToString() },
      };
      PartnerModel.AddField("ruc");
      List<OdooRecord> records = PartnerModel.Search(filter);

      if (records.Count == 0)
        return "0";

      return records[0].GetValue("ruc").ToString();

    }

    #endregion

    #region Eventos de Barra

    private void tsbBajarFacturas_Click(object sender, EventArgs e) {

      InicializarTablas();

    }

    private void tsbImprimir_Click(object sender, EventArgs e) {

      if (MessageBox.Show("Va a imprimir los documentos seleccionados. ¿Confirma?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
        return;

      Cursor = Cursors.WaitCursor;

      var lista = TablaGrilla.AsEnumerable().Where(s => (s["Marca"] as bool?) ?? false).ToList();
      lista = lista.OrderBy(s => Convert.ToInt32( s["id"])).ToList();
      foreach (DataRow item in lista) {
        Imprimir(item);
      }

      dtgPrincipal.Refresh();
      dtgTodas.Refresh();

      Cursor = Cursors.Default;
      MessageBox.Show("Proceso finalizado.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void tsbVer_Click(object sender, EventArgs e) {
      spcPrincipal.Panel2Collapsed = !spcPrincipal.Panel2Collapsed;

      if (spcPrincipal.Panel2Collapsed) {
        tsbVer.Image = Properties.Resources.Ver;
        tsbVer.ToolTipText = "Ver todos los documentos";
        this.Size = new Size(676, this.Size.Height);
      } else {
        tsbVer.Image = Properties.Resources.NoVer;
        tsbVer.ToolTipText = "Ocultar todos los documentos";
        this.Size = new Size(1356, this.Size.Height);
      }
    }

    #endregion

    #region Eventos de Controles

    private void Factura_KeyDown(object sender, KeyEventArgs e) {

    }

    private void Factura_FormClosing(object sender, FormClosingEventArgs e) {
      if (e.CloseReason == CloseReason.UserClosing) {
        e.Cancel = true;
        Visible = false;
      }
    }

    #endregion

    #region Eventos de Grillas

    private void dtgTodas_CellContentClick(object sender, DataGridViewCellEventArgs e) {
      if (e.RowIndex > -1) {
        DataGridView grilla = (DataGridView)sender;
        string strMensaje = "Va a {0} el documento seleccionado. ¿Confirma?";

        if (grilla.Name == dtgTodas.Name)
          strMensaje = string.Format(strMensaje, "reimprimir");
        else
          strMensaje = string.Format(strMensaje, "imprimir");

        if ((grilla.Name == dtgTodas.Name && e.ColumnIndex == ColImprimir.Index) || (grilla.Name == dtgPrincipal.Name && e.ColumnIndex == ColPorImprimir.Index) ) {
          if (MessageBox.Show(strMensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            return;

          Cursor = Cursors.WaitCursor;
          Imprimir(((DataRowView)grilla.Rows[e.RowIndex].DataBoundItem).Row);
          Cursor = Cursors.Default;
          MessageBox.Show("Proceso finalizado.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }

    private void dtgPrincipal_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
      DataGridView dgv = sender as DataGridView;
      if (null == dgv || null == dgv.CurrentCell || !dgv.IsCurrentCellDirty) {
        return;
      }

      if ((dgv.CurrentCell is DataGridViewComboBoxCell || dgv.CurrentCell is DataGridViewCheckBoxCell)) {
        dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
      }
    }

    #endregion

    private void toolStripStatusLabel1_Click(object sender, EventArgs e) {

    }

    private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) {
      System.Diagnostics.Process.Start("http://" + toolStripSplitButton1.Text);
    }

    private void dtgPrincipal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
      if (e.RowIndex > -1) {

      }
    }
  }
}
