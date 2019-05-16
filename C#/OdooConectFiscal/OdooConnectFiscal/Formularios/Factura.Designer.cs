namespace APP {
  partial class Factura {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Factura));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tsbBajarFacturas = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tsbImprimir = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.tsbVer = new System.Windows.Forms.ToolStripButton();
      this.dtgPrincipal = new System.Windows.Forms.DataGridView();
      this.spcPrincipal = new System.Windows.Forms.SplitContainer();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.dtgTodas = new System.Windows.Forms.DataGridView();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.dtpFiltroFecha = new System.Windows.Forms.DateTimePicker();
      this.label2 = new System.Windows.Forms.Label();
      this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
      this.ColTipoT = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColNumeroT = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColFechaT = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColClienteT = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColTotalT = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColImprimir = new System.Windows.Forms.DataGridViewImageColumn();
      this.ColMarca = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.ColPorImprimir = new System.Windows.Forms.DataGridViewImageColumn();
      this.ColTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dtgPrincipal)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.spcPrincipal)).BeginInit();
      this.spcPrincipal.Panel1.SuspendLayout();
      this.spcPrincipal.Panel2.SuspendLayout();
      this.spcPrincipal.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dtgTodas)).BeginInit();
      this.statusStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbBajarFacturas,
            this.toolStripSeparator1,
            this.tsbImprimir,
            this.toolStripSeparator2,
            this.tsbVer});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1178, 31);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tsbBajarFacturas
      // 
      this.tsbBajarFacturas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tsbBajarFacturas.Image = global::APP.Properties.Resources.Download;
      this.tsbBajarFacturas.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbBajarFacturas.Name = "tsbBajarFacturas";
      this.tsbBajarFacturas.Size = new System.Drawing.Size(28, 28);
      this.tsbBajarFacturas.Text = "Bajar facturas desde odoo";
      this.tsbBajarFacturas.Click += new System.EventHandler(this.tsbBajarFacturas_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
      // 
      // tsbImprimir
      // 
      this.tsbImprimir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tsbImprimir.Image = global::APP.Properties.Resources.Impresora;
      this.tsbImprimir.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbImprimir.Name = "tsbImprimir";
      this.tsbImprimir.Size = new System.Drawing.Size(28, 28);
      this.tsbImprimir.Text = "Imprimir facturas seleccionadas";
      this.tsbImprimir.Click += new System.EventHandler(this.tsbImprimir_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
      this.toolStripSeparator2.Visible = false;
      // 
      // tsbVer
      // 
      this.tsbVer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tsbVer.Image = global::APP.Properties.Resources.Ver;
      this.tsbVer.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbVer.Name = "tsbVer";
      this.tsbVer.Size = new System.Drawing.Size(28, 28);
      this.tsbVer.Text = "Ver todos los documentos";
      this.tsbVer.Click += new System.EventHandler(this.tsbVer_Click);
      // 
      // dtgPrincipal
      // 
      this.dtgPrincipal.AllowUserToAddRows = false;
      this.dtgPrincipal.AllowUserToDeleteRows = false;
      this.dtgPrincipal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dtgPrincipal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColMarca,
            this.ColPorImprimir,
            this.ColTipo,
            this.ColNumero,
            this.ColFecha,
            this.ColCliente,
            this.ColTotal});
      this.dtgPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dtgPrincipal.Location = new System.Drawing.Point(3, 16);
      this.dtgPrincipal.Name = "dtgPrincipal";
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
      this.dtgPrincipal.RowsDefaultCellStyle = dataGridViewCellStyle3;
      this.dtgPrincipal.Size = new System.Drawing.Size(655, 194);
      this.dtgPrincipal.TabIndex = 1;
      this.dtgPrincipal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgTodas_CellContentClick);
      this.dtgPrincipal.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dtgPrincipal_CellFormatting);
      this.dtgPrincipal.CurrentCellDirtyStateChanged += new System.EventHandler(this.dtgPrincipal_CurrentCellDirtyStateChanged);
      // 
      // spcPrincipal
      // 
      this.spcPrincipal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.spcPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.spcPrincipal.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.spcPrincipal.Location = new System.Drawing.Point(0, 64);
      this.spcPrincipal.Name = "spcPrincipal";
      // 
      // spcPrincipal.Panel1
      // 
      this.spcPrincipal.Panel1.Controls.Add(this.groupBox1);
      // 
      // spcPrincipal.Panel2
      // 
      this.spcPrincipal.Panel2.Controls.Add(this.groupBox2);
      this.spcPrincipal.Size = new System.Drawing.Size(1178, 217);
      this.spcPrincipal.SplitterDistance = 665;
      this.spcPrincipal.TabIndex = 1;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.dtgPrincipal);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(661, 213);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Por imprimir";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.dtgTodas);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(505, 213);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Todas";
      // 
      // dtgTodas
      // 
      this.dtgTodas.AllowUserToAddRows = false;
      this.dtgTodas.AllowUserToDeleteRows = false;
      this.dtgTodas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dtgTodas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColTipoT,
            this.ColNumeroT,
            this.ColFechaT,
            this.ColClienteT,
            this.ColTotalT,
            this.ColImprimir});
      this.dtgTodas.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dtgTodas.Location = new System.Drawing.Point(3, 16);
      this.dtgTodas.Name = "dtgTodas";
      dataGridViewCellStyle5.BackColor = System.Drawing.Color.Silver;
      this.dtgTodas.RowsDefaultCellStyle = dataGridViewCellStyle5;
      this.dtgTodas.Size = new System.Drawing.Size(499, 194);
      this.dtgTodas.TabIndex = 1;
      this.dtgTodas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgTodas_CellContentClick);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 281);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(1178, 22);
      this.statusStrip1.TabIndex = 3;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripSplitButton1
      // 
      this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
      this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripSplitButton1.Name = "toolStripSplitButton1";
      this.toolStripSplitButton1.Size = new System.Drawing.Size(146, 20);
      this.toolStripSplitButton1.Text = "www.falconsolutions.cl";
      this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.dtpFechaHasta);
      this.panel1.Controls.Add(this.dtpFiltroFecha);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 31);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1178, 33);
      this.panel1.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Fecha Desde";
      // 
      // dtpFiltroFecha
      // 
      this.dtpFiltroFecha.CustomFormat = "dd/MM/yyyy";
      this.dtpFiltroFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtpFiltroFecha.Location = new System.Drawing.Point(89, 7);
      this.dtpFiltroFecha.Name = "dtpFiltroFecha";
      this.dtpFiltroFecha.Size = new System.Drawing.Size(85, 20);
      this.dtpFiltroFecha.TabIndex = 0;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(219, 11);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(68, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Fecha Hasta";
      // 
      // dtpFechaHasta
      // 
      this.dtpFechaHasta.CustomFormat = "dd/MM/yyyy";
      this.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dtpFechaHasta.Location = new System.Drawing.Point(295, 7);
      this.dtpFechaHasta.Name = "dtpFechaHasta";
      this.dtpFechaHasta.Size = new System.Drawing.Size(85, 20);
      this.dtpFechaHasta.TabIndex = 1;
      // 
      // ColTipoT
      // 
      this.ColTipoT.DataPropertyName = "tipo_doc";
      this.ColTipoT.HeaderText = "Tipo";
      this.ColTipoT.Name = "ColTipoT";
      this.ColTipoT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // ColNumeroT
      // 
      this.ColNumeroT.DataPropertyName = "number";
      this.ColNumeroT.HeaderText = "Número";
      this.ColNumeroT.Name = "ColNumeroT";
      this.ColNumeroT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColNumeroT.Width = 120;
      // 
      // ColFechaT
      // 
      this.ColFechaT.DataPropertyName = "date_invoice";
      this.ColFechaT.HeaderText = "Fecha";
      this.ColFechaT.Name = "ColFechaT";
      this.ColFechaT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColFechaT.Width = 65;
      // 
      // ColClienteT
      // 
      this.ColClienteT.DataPropertyName = "partner_id";
      this.ColClienteT.HeaderText = "Cliente";
      this.ColClienteT.Name = "ColClienteT";
      this.ColClienteT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColClienteT.Width = 200;
      // 
      // ColTotalT
      // 
      this.ColTotalT.DataPropertyName = "amount_total";
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle4.Format = "#,##0.##";
      this.ColTotalT.DefaultCellStyle = dataGridViewCellStyle4;
      this.ColTotalT.HeaderText = "Total";
      this.ColTotalT.Name = "ColTotalT";
      this.ColTotalT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColTotalT.Width = 70;
      // 
      // ColImprimir
      // 
      this.ColImprimir.HeaderText = "Reimprimir";
      this.ColImprimir.Image = global::APP.Properties.Resources.Impresora;
      this.ColImprimir.Name = "ColImprimir";
      this.ColImprimir.Width = 60;
      // 
      // ColMarca
      // 
      this.ColMarca.DataPropertyName = "Marca";
      this.ColMarca.HeaderText = "";
      this.ColMarca.Name = "ColMarca";
      this.ColMarca.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.ColMarca.Width = 40;
      // 
      // ColPorImprimir
      // 
      this.ColPorImprimir.HeaderText = "";
      this.ColPorImprimir.Image = global::APP.Properties.Resources.Impresora;
      this.ColPorImprimir.Name = "ColPorImprimir";
      this.ColPorImprimir.Visible = false;
      this.ColPorImprimir.Width = 40;
      // 
      // ColTipo
      // 
      this.ColTipo.DataPropertyName = "tipo_doc";
      this.ColTipo.HeaderText = "Tipo";
      this.ColTipo.Name = "ColTipo";
      this.ColTipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // ColNumero
      // 
      this.ColNumero.DataPropertyName = "number";
      this.ColNumero.HeaderText = "Número";
      this.ColNumero.Name = "ColNumero";
      this.ColNumero.ReadOnly = true;
      this.ColNumero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColNumero.Width = 120;
      // 
      // ColFecha
      // 
      this.ColFecha.DataPropertyName = "date_invoice";
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.Format = "dd/MM/yyyy";
      this.ColFecha.DefaultCellStyle = dataGridViewCellStyle1;
      this.ColFecha.HeaderText = "Fecha";
      this.ColFecha.Name = "ColFecha";
      this.ColFecha.ReadOnly = true;
      this.ColFecha.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColFecha.Width = 65;
      // 
      // ColCliente
      // 
      this.ColCliente.DataPropertyName = "partner_id";
      this.ColCliente.HeaderText = "Cliente";
      this.ColCliente.Name = "ColCliente";
      this.ColCliente.ReadOnly = true;
      this.ColCliente.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColCliente.Width = 200;
      // 
      // ColTotal
      // 
      this.ColTotal.DataPropertyName = "amount_total";
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      dataGridViewCellStyle2.Format = "$#,##0.##";
      this.ColTotal.DefaultCellStyle = dataGridViewCellStyle2;
      this.ColTotal.HeaderText = "Total";
      this.ColTotal.Name = "ColTotal";
      this.ColTotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.ColTotal.Width = 70;
      // 
      // Factura
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1178, 303);
      this.Controls.Add(this.spcPrincipal);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.toolStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.KeyPreview = true;
      this.Name = "Factura";
      this.Text = "Programa Impresora Fiscal Panamá (Falcon Solutions SpA)";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Factura_FormClosing);
      this.Load += new System.EventHandler(this.Factura_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Factura_KeyDown);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dtgPrincipal)).EndInit();
      this.spcPrincipal.Panel1.ResumeLayout(false);
      this.spcPrincipal.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.spcPrincipal)).EndInit();
      this.spcPrincipal.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dtgTodas)).EndInit();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton tsbBajarFacturas;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton tsbImprimir;
    private System.Windows.Forms.DataGridView dtgPrincipal;
    private System.Windows.Forms.SplitContainer spcPrincipal;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.DataGridView dtgTodas;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton tsbVer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.DateTimePicker dtpFiltroFecha;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DateTimePicker dtpFechaHasta;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColTipoT;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColNumeroT;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColFechaT;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColClienteT;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColTotalT;
    private System.Windows.Forms.DataGridViewImageColumn ColImprimir;
    private System.Windows.Forms.DataGridViewCheckBoxColumn ColMarca;
    private System.Windows.Forms.DataGridViewImageColumn ColPorImprimir;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColTipo;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColNumero;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColFecha;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColCliente;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColTotal;
  }
}