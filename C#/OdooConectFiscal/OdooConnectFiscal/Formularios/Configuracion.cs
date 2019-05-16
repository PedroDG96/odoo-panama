using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APP.Formularios;
using Connect;
using System.IO;

namespace APP {
  public partial class Configuracion : Form {
    public Configuracion() {
      InitializeComponent();
      ddlDB.DataSource = Global.TablaDB;
      ddlDB.SelectedValue = Global.DB;

      string strArchivo = Path.Combine(Directory.GetCurrentDirectory(), Global.SEGURIDAD);
      DataSet dts = new DataSet();
      dts.ReadXml(strArchivo);

      if (dts.Tables.Count == 0) {
        MessageBox.Show("El archivo Seguridad.xml no está bien configurado.", Global.STR_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
      }

      DataRow dtr = dts.Tables[0].Rows[0];

      txtUser.Text = dtr["Usuario"].ToString();
      txtPass.Text = dtr["Pass"].ToString(); 
      btnAceptar_Click(null, null);

    }
  
    private bool Validar() {
      StringBuilder stbError = new StringBuilder();

      if (string.IsNullOrWhiteSpace(txtUser.Text))
        stbError.AppendLine("Usuario es requerido");
      if (string.IsNullOrWhiteSpace(txtPass.Text))
        stbError.AppendLine("Clave es requerida");


      try {

        OdooConnectionCredentials creds = new OdooConnectionCredentials(Global.URL, Global.DB, txtUser.Text, txtPass.Text);

        OdooAPI API = new OdooAPI(creds);

      } catch (Exception ex) {
        stbError.AppendLine("Usuario y clave no válido");
      }

      if (stbError.Length > 0) {
        stbError.Insert(0, "Existen campos con errores: \n");
        MessageBox.Show(stbError.ToString(), Global.STR_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    private void btnCancelar_Click(object sender, EventArgs e) {
      DialogResult = System.Windows.Forms.DialogResult.Cancel;
      Close();
    }

    private void btnAceptar_Click(object sender, EventArgs e) {
      if (Validar()) {
        Global.User = txtUser.Text;
        Global.Pass = txtPass.Text;
        DialogResult = System.Windows.Forms.DialogResult.OK;
        Close();

      }
    }



  }
}
