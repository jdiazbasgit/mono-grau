using System.Drawing;
using System.Windows.Forms;

namespace UHFAPP
{
    partial class MainFormBajas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormBajas));
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
            this.tDescripcion = new System.Windows.Forms.TextBox();
            this.descripcion = new System.Windows.Forms.Label();
            this.buttonConsulta = new System.Windows.Forms.Button();
            this.panelCaja = new System.Windows.Forms.Panel();
            this.botonCaja = new System.Windows.Forms.Button();
            this.textoCaja = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.pPelPda = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCalidad = new System.Windows.Forms.Button();
            this.buttonInversas = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.epcs)).BeginInit();
            this.panel9.SuspendLayout();
            this.panelCaja.SuspendLayout();
            this.panel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // epcs
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.NullValue = null;
            this.epcs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.epcs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epcs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.epcs.BackgroundColor = System.Drawing.Color.White;
            this.epcs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.NullValue = "\"\"";
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.epcs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.epcs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.epcs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.epcs.DefaultCellStyle = dataGridViewCellStyle7;
            this.epcs.GridColor = System.Drawing.Color.White;
            this.epcs.Location = new System.Drawing.Point(0, 321);
            this.epcs.Margin = new System.Windows.Forms.Padding(2);
            this.epcs.Name = "epcs";
            this.epcs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.NullValue = null;
            this.epcs.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.epcs.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epcs.RowTemplate.Height = 24;
            this.epcs.RowTemplate.ReadOnly = true;
            this.epcs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.epcs.Size = new System.Drawing.Size(0, 0);
            this.epcs.TabIndex = 37;
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
            this.Column2.HeaderText = "LOTE";
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
            this.Column4.FillWeight = 120.738F;
            this.Column4.HeaderText = "DESCRIPCION";
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
            this.btnScanEPC.Location = new System.Drawing.Point(860, 722);
            this.btnScanEPC.Name = "btnScanEPC";
            this.btnScanEPC.Size = new System.Drawing.Size(180, 44);
            this.btnScanEPC.TabIndex = 34;
            this.btnScanEPC.Text = "  Iniciar lectura  ";
            this.btnScanEPC.UseCompatibleTextRendering = true;
            this.btnScanEPC.UseVisualStyleBackColor = false;
            this.btnScanEPC.Visible = false;
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
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Transparent;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel9.Controls.Add(this.tDescripcion);
            this.panel9.Controls.Add(this.descripcion);
            this.panel9.Controls.Add(this.btnScanEPC);
            this.panel9.Controls.Add(this.buttonConsulta);
            this.panel9.Controls.Add(this.panelCaja);
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Controls.Add(this.epcs);
            this.panel9.Location = new System.Drawing.Point(-22, -10);
            this.panel9.Margin = new System.Windows.Forms.Padding(2);
            this.panel9.MinimumSize = new System.Drawing.Size(1360, 768);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1360, 768);
            this.panel9.TabIndex = 37;
            // 
            // tDescripcion
            // 
            this.tDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tDescripcion.Location = new System.Drawing.Point(693, 123);
            this.tDescripcion.Name = "tDescripcion";
            this.tDescripcion.Size = new System.Drawing.Size(544, 30);
            this.tDescripcion.TabIndex = 52;
            // 
            // descripcion
            // 
            this.descripcion.AutoSize = true;
            this.descripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descripcion.Location = new System.Drawing.Point(480, 118);
            this.descripcion.Name = "descripcion";
            this.descripcion.Size = new System.Drawing.Size(167, 25);
            this.descripcion.TabIndex = 51;
            this.descripcion.Text = "DESCRIPCION:";
            // 
            // buttonConsulta
            // 
            this.buttonConsulta.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonConsulta.Location = new System.Drawing.Point(135, 717);
            this.buttonConsulta.Name = "buttonConsulta";
            this.buttonConsulta.Size = new System.Drawing.Size(180, 44);
            this.buttonConsulta.TabIndex = 38;
            this.buttonConsulta.Text = "Iniciar bajas";
            this.buttonConsulta.UseCompatibleTextRendering = true;
            this.buttonConsulta.UseVisualStyleBackColor = false;
            this.buttonConsulta.Click += new System.EventHandler(this.buttonConsulta_Click);
            // 
            // panelCaja
            // 
            this.panelCaja.BackColor = System.Drawing.Color.Orange;
            this.panelCaja.Controls.Add(this.botonCaja);
            this.panelCaja.Controls.Add(this.textoCaja);
            this.panelCaja.Location = new System.Drawing.Point(660, 370);
            this.panelCaja.Name = "panelCaja";
            this.panelCaja.Size = new System.Drawing.Size(590, 353);
            this.panelCaja.TabIndex = 50;
            this.panelCaja.Visible = false;
            // 
            // botonCaja
            // 
            this.botonCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonCaja.Location = new System.Drawing.Point(243, 275);
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
            this.textoCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoCaja.Location = new System.Drawing.Point(28, 19);
            this.textoCaja.Name = "textoCaja";
            this.textoCaja.Size = new System.Drawing.Size(533, 253);
            this.textoCaja.TabIndex = 0;
            this.textoCaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.LightGray;
            this.panel11.Controls.Add(this.buttonInversas);
            this.panel11.Controls.Add(this.buttonCalidad);
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
            this.dataGridViewTextBoxColumn2.HeaderText = "LOTE";
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
            // buttonCalidad
            // 
            this.buttonCalidad.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonCalidad.Location = new System.Drawing.Point(748, 31);
            this.buttonCalidad.Name = "buttonCalidad";
            this.buttonCalidad.Size = new System.Drawing.Size(149, 44);
            this.buttonCalidad.TabIndex = 53;
            this.buttonCalidad.Text = "Calidad";
            this.buttonCalidad.UseCompatibleTextRendering = true;
            this.buttonCalidad.UseVisualStyleBackColor = false;
            this.buttonCalidad.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonInversas
            // 
            this.buttonInversas.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.buttonInversas.Location = new System.Drawing.Point(925, 31);
            this.buttonInversas.Name = "buttonInversas";
            this.buttonInversas.Size = new System.Drawing.Size(149, 44);
            this.buttonInversas.TabIndex = 54;
            this.buttonInversas.Text = "Orden Inversa";
            this.buttonInversas.UseCompatibleTextRendering = true;
            this.buttonInversas.UseVisualStyleBackColor = false;
            this.buttonInversas.Click += new System.EventHandler(this.buttonInversas_Click);
            // 
            // MainFormBajas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1426, 718);
            this.Controls.Add(this.panel9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainFormBajas";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UHF(1.3.2)";
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ScanEPCForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.epcs)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panelCaja.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button btnScanEPC;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;

        #endregion
        private System.Windows.Forms.Panel pPelPda;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Button buttonConsulta;
        private System.Windows.Forms.Panel panelCaja;
        private System.Windows.Forms.Button botonCaja;
        private System.Windows.Forms.Label textoCaja;
        private System.Windows.Forms.Panel panel9;
        public DataGridView epcs;
        private TextBox tDescripcion;
        private Label descripcion;
        private Button buttonCalidad;
        private Button buttonInversas;
    }
}