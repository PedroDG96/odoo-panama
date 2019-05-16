using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;
using ITD.Log;
using System.Text;
using System.Globalization;

namespace APP {
  public static class Global {
    /// <summary>
    /// Punto de entrada principal para la aplicación.
    /// </summary>
    /// 

    #region Propiedades
    public static string URL { get; private set; }
    public static string DB { get; private set; }
    public static string PuertoImpresora { get; private set; }
    public static string User { get; set; }
    public static string Pass { get; set; }

    public static DataTable TablaDB { get; set; }

    public const string STR_ERROR = "ERROR";
    public const string SEGURIDAD = "Seguridad.xml";
    public const string CONEXION = "Conexion.xml";

    public static System.Windows.Forms.NotifyIcon Icono { get; private set; }    
    private static System.Windows.Forms.ContextMenuStrip mnuPrincipal;
    private static System.Windows.Forms.ToolStripMenuItem mniSalir;
    static Factura objFactura;

    public static FiscalPrinter.Fiscal Impresora { get; set; }

    #endregion
    [STAThread]
    static void Main() {
      var objProceso = System.Diagnostics.Process.GetProcessesByName("OdooConnectFiscal");
      var otro = objProceso;

      if (objProceso.Count() > 1) {
        MessageBox.Show("Ya se encuentra ejecutando la aplicación.", STR_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      ManejadorLog.InicializarLog("OdooConnectFiscal");
      ManejadorLog.Log.Info("Iniciando OdooConnectFiscal");
      CargarConexionXML();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      InicializarIcono();

      objFactura = new Factura();

      if (objFactura.IsDisposed)
        Application.Exit();
      else {
        Icono_DoubleClick(null, null);

        Application.Run();
      }
    }

    private static void CargarConexionXML() {
      string strArchivo = Path.Combine(Directory.GetCurrentDirectory(), CONEXION);
      DataSet dts = new DataSet();
      dts.ReadXml(strArchivo);

      if (dts.Tables.Count < 2) {
        MessageBox.Show("El archivo Conexion.xml no está bien configurado.", STR_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
      }
      try {
        DataRow dtr = dts.Tables[0].Rows[0];
        string strUrl = string.Format("{0}:{1}", dtr["Url"], dtr["Puerto"]);
        URL = strUrl;
        DB = dts.Tables[1].Rows[0]["DB"].ToString();
        PuertoImpresora = dtr["PuertoImpresora"].ToString();
        Impresora = new FiscalPrinter.Fiscal(PuertoImpresora);
        Impresora.AbrirPuerto();

        TablaDB = new DataTable();
        TablaDB.Columns.Add("DB");
        TablaDB.Merge(dts.Tables[1]);
        

      } catch (Exception ex) {
        MessageBox.Show("El archivo Conexion.xml no está bien configurado.", STR_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
      }
    }
    
    private static void InicializarIcono() {
      Icono = new System.Windows.Forms.NotifyIcon();
      mnuPrincipal = new System.Windows.Forms.ContextMenuStrip();
      mniSalir = new System.Windows.Forms.ToolStripMenuItem();

      Icono.Icon = Properties.Resources.odoo;
      Icono.ContextMenuStrip = mnuPrincipal;
      Icono.Visible = true;
      Icono.DoubleClick += new System.EventHandler(Icono_DoubleClick);
      Icono.BalloonTipClicked += new System.EventHandler(Icono_BalloonTipClicked);

      // 
      // mniSalir
      // 
      mniSalir.Name = "mniSalir";
      mniSalir.Size = new System.Drawing.Size(177, 22);
      mniSalir.Text = "Salir";
      mniSalir.Click += new System.EventHandler(mniSalir_Click);

      mnuPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {  mniSalir });
      mnuPrincipal.Name = "mnuPrincipal";
      mnuPrincipal.Size = new System.Drawing.Size(178, 148);
    

    }

    #region Icono de notificacion


    private static void Icono_DoubleClick(object sender, EventArgs e) {
      if (objFactura == null || objFactura.IsDisposed) {
        objFactura = new Factura();
        objFactura.Show();
      } else {
        objFactura.Visible = true;
        objFactura.Focus();
      }
    }

    private static void Icono_BalloonTipClicked(object sender, EventArgs e) {    
        Icono_DoubleClick(null, null);
    }

    private static void mniSalir_Click(object sender, EventArgs e) {
      Impresora.CerrarPuerto();
      Icono.Dispose();
      Application.Exit();
    }
    #endregion

  }
}
