using System.Drawing;
using System.Windows.Forms;

namespace UHFAPP
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

     
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 


        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.epcs = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnScanEPC = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.buttonBiocam = new System.Windows.Forms.Button();
            this.buttonConsulta = new System.Windows.Forms.Button();
            this.panelCaja = new System.Windows.Forms.Panel();
            this.botonCaja = new System.Windows.Forms.Button();
            this.textoCaja = new System.Windows.Forms.Label();
            this.cantidadComprobacion = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel11 = new System.Windows.Forms.Panel();
            this.buttonInversa = new System.Windows.Forms.Button();
            this.buttonBajas = new System.Windows.Forms.Button();
            this.pPelPda = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pLinea = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pLotes = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pReferencia = new System.Windows.Forms.Panel();
            this.tbLotes = new System.Windows.Forms.TextBox();
            this.lDescripcion = new System.Windows.Forms.Label();
            this.lCantidadTotal = new System.Windows.Forms.Label();
            this.lReferencia = new System.Windows.Forms.Label();
            this.tbLinea = new System.Windows.Forms.TextBox();
            this.buttonExportar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.epcs)).BeginInit();
            this.panel9.SuspendLayout();
            this.panelCaja.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel11.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.textBox1.Location = new System.Drawing.Point(220, 139);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(136, 29);
            this.textBox1.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(382, 136);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 32);
            this.button2.TabIndex = 9;
            this.button2.Text = "Buscar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // epcs
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.NullValue = null;
            this.epcs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.epcs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epcs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.epcs.BackgroundColor = System.Drawing.Color.White;
            this.epcs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.NullValue = "\"\"";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.epcs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.epcs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.epcs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.epcs.DefaultCellStyle = dataGridViewCellStyle3;
            this.epcs.GridColor = System.Drawing.Color.White;
            this.epcs.Location = new System.Drawing.Point(0, 321);
            this.epcs.Margin = new System.Windows.Forms.Padding(2);
            this.epcs.Name = "epcs";
            this.epcs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.NullValue = null;
            this.epcs.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.epcs.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epcs.RowTemplate.Height = 24;
            this.epcs.RowTemplate.ReadOnly = true;
            this.epcs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.epcs.Size = new System.Drawing.Size(0, 0);
            this.epcs.TabIndex = 37;
            this.epcs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.epcs_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 146.1281F;
            this.Column1.HeaderText = "PRODUCTO";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 109.0447F;
            this.Column2.HeaderText = "ID NUMBER";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.FillWeight = 109.0447F;
            this.Column3.HeaderText = "NUMERO DE SERIE";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.FillWeight = 26.73797F;
            this.Column4.HeaderText = "LINEA";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.FillWeight = 109.0447F;
            this.Column5.HeaderText = "FECHA";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // btnScanEPC
            // 
            this.btnScanEPC.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnScanEPC.Location = new System.Drawing.Point(660, 722);
            this.btnScanEPC.Name = "btnScanEPC";
            this.btnScanEPC.Size = new System.Drawing.Size(180, 44);
            this.btnScanEPC.TabIndex = 34;
            this.btnScanEPC.Text = "  Iniciar lectura  ";
            this.btnScanEPC.UseCompatibleTextRendering = true;
            this.btnScanEPC.UseVisualStyleBackColor = false;
            this.btnScanEPC.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(175, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 72);
            this.panel1.TabIndex = 38;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.button1.Location = new System.Drawing.Point(16, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 32);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Transparent;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel9.Controls.Add(this.button3);
            this.panel9.Controls.Add(this.buttonExcel);
            this.panel9.Controls.Add(this.buttonBiocam);
            this.panel9.Controls.Add(this.btnScanEPC);
            this.panel9.Controls.Add(this.buttonConsulta);
            this.panel9.Controls.Add(this.panelCaja);
            this.panel9.Controls.Add(this.cantidadComprobacion);
            this.panel9.Controls.Add(this.dataGridView1);
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Controls.Add(this.pLinea);
            this.panel9.Controls.Add(this.panel4);
            this.panel9.Controls.Add(this.panel3);
            this.panel9.Controls.Add(this.tbLinea);
            this.panel9.Controls.Add(this.epcs);
            this.panel9.Controls.Add(this.button2);
            this.panel9.Controls.Add(this.textBox1);
            this.panel9.Location = new System.Drawing.Point(-22, -10);
            this.panel9.Margin = new System.Windows.Forms.Padding(2);
            this.panel9.MinimumSize = new System.Drawing.Size(1360, 768);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1360, 768);
            this.panel9.TabIndex = 37;
            this.panel9.Paint += new System.Windows.Forms.PaintEventHandler(this.panel9_Paint);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(341, 717);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(180, 44);
            this.button3.TabIndex = 54;
            this.button3.Text = "Exportar EXCEL";
            this.button3.UseCompatibleTextRendering = true;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_3);
            // 
            // buttonExcel
            // 
            this.buttonExcel.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonExcel.Location = new System.Drawing.Point(849, 722);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(180, 44);
            this.buttonExcel.TabIndex = 53;
            this.buttonExcel.Text = "Exportar EXCEL";
            this.buttonExcel.UseCompatibleTextRendering = true;
            this.buttonExcel.UseVisualStyleBackColor = false;
            this.buttonExcel.Click += new System.EventHandler(this.buttonExcel_Click);
            // 
            // buttonBiocam
            // 
            this.buttonBiocam.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonBiocam.Location = new System.Drawing.Point(1041, 722);
            this.buttonBiocam.Name = "buttonBiocam";
            this.buttonBiocam.Size = new System.Drawing.Size(180, 44);
            this.buttonBiocam.TabIndex = 51;
            this.buttonBiocam.Text = "Iniciar Biocam";
            this.buttonBiocam.UseCompatibleTextRendering = true;
            this.buttonBiocam.UseVisualStyleBackColor = false;
            this.buttonBiocam.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // buttonConsulta
            // 
            this.buttonConsulta.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonConsulta.Location = new System.Drawing.Point(135, 717);
            this.buttonConsulta.Name = "buttonConsulta";
            this.buttonConsulta.Size = new System.Drawing.Size(180, 44);
            this.buttonConsulta.TabIndex = 38;
            this.buttonConsulta.Text = "Iniciar comprobacion";
            this.buttonConsulta.UseCompatibleTextRendering = true;
            this.buttonConsulta.UseVisualStyleBackColor = false;
            this.buttonConsulta.Click += new System.EventHandler(this.buttonConsulta_Click);
            // 
            // panelCaja
            // 
            this.panelCaja.BackColor = System.Drawing.Color.Orange;
            this.panelCaja.Controls.Add(this.botonCaja);
            this.panelCaja.Controls.Add(this.textoCaja);
            this.panelCaja.Location = new System.Drawing.Point(470, 155);
            this.panelCaja.Name = "panelCaja";
            this.panelCaja.Size = new System.Drawing.Size(700, 500);
            this.panelCaja.TabIndex = 50;
            this.panelCaja.Visible = false;
            // 
            // botonCaja
            // 
            this.botonCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonCaja.Location = new System.Drawing.Point(308, 416);
            this.botonCaja.Name = "botonCaja";
            this.botonCaja.Size = new System.Drawing.Size(116, 39);
            this.botonCaja.TabIndex = 1;
            this.botonCaja.Text = "CERRAR";
            this.botonCaja.UseVisualStyleBackColor = true;
            this.botonCaja.Click += new System.EventHandler(this.botonCaja_Click);
            // 
            // textoCaja
            // 
            this.textoCaja.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textoCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoCaja.Location = new System.Drawing.Point(28, 15);
            this.textoCaja.Name = "textoCaja";
            this.textoCaja.Size = new System.Drawing.Size(643, 398);
            this.textoCaja.TabIndex = 0;
            this.textoCaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cantidadComprobacion
            // 
            this.cantidadComprobacion.AutoSize = true;
            this.cantidadComprobacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cantidadComprobacion.Location = new System.Drawing.Point(234, 296);
            this.cantidadComprobacion.Name = "cantidadComprobacion";
            this.cantidadComprobacion.Size = new System.Drawing.Size(90, 20);
            this.cantidadComprobacion.TabIndex = 49;
            this.cantidadComprobacion.Text = "Cantidad: 0";
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.NullValue = null;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.NullValue = "\"\"";
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(0, 321);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.NullValue = null;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(0, 0);
            this.dataGridView1.TabIndex = 37;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 146.1281F;
            this.dataGridViewTextBoxColumn1.HeaderText = "PRODUCTO";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 109.0447F;
            this.dataGridViewTextBoxColumn2.HeaderText = "ID NUMBER";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 109.0447F;
            this.dataGridViewTextBoxColumn3.HeaderText = "NUMERO DE SERIE";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.LightGray;
            this.panel11.Controls.Add(this.buttonInversa);
            this.panel11.Controls.Add(this.buttonBajas);
            this.panel11.Controls.Add(this.pPelPda);
            this.panel11.Controls.Add(this.panel2);
            this.panel11.Controls.Add(this.button1);
            this.panel11.Controls.Add(this.panel1);
            this.panel11.Location = new System.Drawing.Point(31, 16);
            this.panel11.Margin = new System.Windows.Forms.Padding(2);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1766, 89);
            this.panel11.TabIndex = 47;
            // 
            // buttonInversa
            // 
            this.buttonInversa.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonInversa.Location = new System.Drawing.Point(959, 31);
            this.buttonInversa.Name = "buttonInversa";
            this.buttonInversa.Size = new System.Drawing.Size(180, 44);
            this.buttonInversa.TabIndex = 54;
            this.buttonInversa.Text = "Orden Inversa";
            this.buttonInversa.UseCompatibleTextRendering = true;
            this.buttonInversa.UseVisualStyleBackColor = false;
            this.buttonInversa.Click += new System.EventHandler(this.buttonInversa_Click);
            // 
            // buttonBajas
            // 
            this.buttonBajas.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonBajas.Location = new System.Drawing.Point(747, 31);
            this.buttonBajas.Name = "buttonBajas";
            this.buttonBajas.Size = new System.Drawing.Size(180, 44);
            this.buttonBajas.TabIndex = 53;
            this.buttonBajas.Text = "Bajas";
            this.buttonBajas.UseCompatibleTextRendering = true;
            this.buttonBajas.UseVisualStyleBackColor = false;
            this.buttonBajas.Click += new System.EventHandler(this.buttonBajas_Click_1);
            // 
            // pPelPda
            // 
            this.pPelPda.BackColor = System.Drawing.Color.LightGray;
            this.pPelPda.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pPelPda.BackgroundImage")));
            this.pPelPda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pPelPda.Location = new System.Drawing.Point(22, 3);
            this.pPelPda.Margin = new System.Windows.Forms.Padding(2);
            this.pPelPda.Name = "pPelPda";
            this.pPelPda.Size = new System.Drawing.Size(89, 89);
            this.pPelPda.TabIndex = 46;
            this.pPelPda.TabStop = true;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(513, 16);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(211, 59);
            this.panel2.TabIndex = 39;
            // 
            // pLinea
            // 
            this.pLinea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pLinea.BackgroundImage")));
            this.pLinea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pLinea.Location = new System.Drawing.Point(53, 228);
            this.pLinea.Margin = new System.Windows.Forms.Padding(2);
            this.pLinea.Name = "pLinea";
            this.pLinea.Size = new System.Drawing.Size(142, 47);
            this.pLinea.TabIndex = 44;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(53, 131);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(142, 44);
            this.panel4.TabIndex = 43;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.pLotes);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.pReferencia);
            this.panel3.Controls.Add(this.tbLotes);
            this.panel3.Controls.Add(this.lDescripcion);
            this.panel3.Controls.Add(this.lCantidadTotal);
            this.panel3.Controls.Add(this.lReferencia);
            this.panel3.Location = new System.Drawing.Point(646, 110);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(638, 206);
            this.panel3.TabIndex = 42;
            // 
            // pLotes
            // 
            this.pLotes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pLotes.BackgroundImage")));
            this.pLotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pLotes.Location = new System.Drawing.Point(316, 59);
            this.pLotes.Margin = new System.Windows.Forms.Padding(2);
            this.pLotes.Name = "pLotes";
            this.pLotes.Size = new System.Drawing.Size(179, 33);
            this.pLotes.TabIndex = 19;
            this.pLotes.Visible = false;
            // 
            // panel5
            // 
            this.panel5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel5.BackgroundImage")));
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel5.Location = new System.Drawing.Point(14, 80);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(147, 38);
            this.panel5.TabIndex = 18;
            // 
            // pReferencia
            // 
            this.pReferencia.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pReferencia.BackgroundImage")));
            this.pReferencia.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pReferencia.Location = new System.Drawing.Point(14, 12);
            this.pReferencia.Margin = new System.Windows.Forms.Padding(2);
            this.pReferencia.Name = "pReferencia";
            this.pReferencia.Size = new System.Drawing.Size(147, 38);
            this.pReferencia.TabIndex = 17;
            // 
            // tbLotes
            // 
            this.tbLotes.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.tbLotes.Location = new System.Drawing.Point(518, 65);
            this.tbLotes.Margin = new System.Windows.Forms.Padding(2);
            this.tbLotes.Name = "tbLotes";
            this.tbLotes.Size = new System.Drawing.Size(67, 29);
            this.tbLotes.TabIndex = 16;
            this.tbLotes.Visible = false;
            // 
            // lDescripcion
            // 
            this.lDescripcion.AutoSize = true;
            this.lDescripcion.Font = new System.Drawing.Font("Microsoft YaHei", 16F, System.Drawing.FontStyle.Bold);
            this.lDescripcion.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lDescripcion.Location = new System.Drawing.Point(28, 150);
            this.lDescripcion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lDescripcion.Name = "lDescripcion";
            this.lDescripcion.Size = new System.Drawing.Size(0, 30);
            this.lDescripcion.TabIndex = 14;
            // 
            // lCantidadTotal
            // 
            this.lCantidadTotal.AutoSize = true;
            this.lCantidadTotal.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.lCantidadTotal.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lCantidadTotal.Location = new System.Drawing.Point(182, 90);
            this.lCantidadTotal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lCantidadTotal.Name = "lCantidadTotal";
            this.lCantidadTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lCantidadTotal.Size = new System.Drawing.Size(0, 22);
            this.lCantidadTotal.TabIndex = 15;
            // 
            // lReferencia
            // 
            this.lReferencia.AutoSize = true;
            this.lReferencia.BackColor = System.Drawing.Color.Transparent;
            this.lReferencia.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.lReferencia.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lReferencia.Location = new System.Drawing.Point(182, 20);
            this.lReferencia.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lReferencia.Name = "lReferencia";
            this.lReferencia.Size = new System.Drawing.Size(0, 22);
            this.lReferencia.TabIndex = 13;
            this.lReferencia.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbLinea
            // 
            this.tbLinea.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.tbLinea.Location = new System.Drawing.Point(238, 239);
            this.tbLinea.Margin = new System.Windows.Forms.Padding(2);
            this.tbLinea.Name = "tbLinea";
            this.tbLinea.Size = new System.Drawing.Size(48, 29);
            this.tbLinea.TabIndex = 41;
            this.tbLinea.Text = "1";
            // 
            // buttonExportar
            // 
            this.buttonExportar.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonExportar.Location = new System.Drawing.Point(1343, 108);
            this.buttonExportar.Name = "buttonExportar";
            this.buttonExportar.Size = new System.Drawing.Size(180, 44);
            this.buttonExportar.TabIndex = 52;
            this.buttonExportar.Text = "Exportar TXT";
            this.buttonExportar.UseCompatibleTextRendering = true;
            this.buttonExportar.UseVisualStyleBackColor = false;
            this.buttonExportar.Visible = false;
            this.buttonExportar.Click += new System.EventHandler(this.button3_Click_2);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1530, 735);
            this.Controls.Add(this.buttonExportar);
            this.Controls.Add(this.panel9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UHF(1.3.2)";
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing_1);
            this.Load += new System.EventHandler(this.ScanEPCForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.epcs)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panelCaja.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnScanEPC;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbLinea;

        #endregion

        private System.Windows.Forms.Panel pLinea;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pPelPda;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pReferencia;
        private System.Windows.Forms.TextBox tbLotes;
        private System.Windows.Forms.Label lDescripcion;
        private System.Windows.Forms.Label lCantidadTotal;
        private System.Windows.Forms.Label lReferencia;
        private System.Windows.Forms.Panel pLotes;
        private System.Windows.Forms.Button buttonConsulta;
        private System.Windows.Forms.Label cantidadComprobacion;
        private System.Windows.Forms.Panel panelCaja;
        private System.Windows.Forms.Button botonCaja;
        private System.Windows.Forms.Label textoCaja;
        private System.Windows.Forms.Panel panel9;
        public DataGridView epcs;
        public DataGridView dataGridView1;
        private Button buttonInversa;
        private Button buttonBajas;
        private Button buttonBiocam;
        private Button buttonExportar;
        private Button buttonExcel;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private Button button3;
    }
}