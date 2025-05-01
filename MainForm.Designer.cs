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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lReferencia = new System.Windows.Forms.Label();
            this.lCantidadTotal = new System.Windows.Forms.Label();
            this.lDescripcion = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbLotes = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.finLote = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.epcs = new System.Windows.Forms.DataGridView();
            this.label25 = new System.Windows.Forms.Label();
            this.btnScanEPC = new System.Windows.Forms.Button();
            this.totalesOrden = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.pTotal = new System.Windows.Forms.Panel();
            this.pLinea = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pLotes = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pReferencia = new System.Windows.Forms.Panel();
            this.tbLinea = new System.Windows.Forms.TextBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.pPelPda = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.finLote.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epcs)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // lReferencia
            // 
            this.lReferencia.AutoSize = true;
            this.lReferencia.BackColor = System.Drawing.Color.Transparent;
            this.lReferencia.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.lReferencia.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lReferencia.Location = new System.Drawing.Point(243, 25);
            this.lReferencia.Name = "lReferencia";
            this.lReferencia.Size = new System.Drawing.Size(0, 27);
            this.lReferencia.TabIndex = 13;
            this.lReferencia.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lCantidadTotal
            // 
            this.lCantidadTotal.AutoSize = true;
            this.lCantidadTotal.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.lCantidadTotal.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lCantidadTotal.Location = new System.Drawing.Point(243, 111);
            this.lCantidadTotal.Name = "lCantidadTotal";
            this.lCantidadTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lCantidadTotal.Size = new System.Drawing.Size(0, 27);
            this.lCantidadTotal.TabIndex = 15;
            // 
            // lDescripcion
            // 
            this.lDescripcion.AutoSize = true;
            this.lDescripcion.Font = new System.Drawing.Font("Microsoft YaHei", 16F, System.Drawing.FontStyle.Bold);
            this.lDescripcion.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lDescripcion.Location = new System.Drawing.Point(38, 185);
            this.lDescripcion.Name = "lDescripcion";
            this.lDescripcion.Size = new System.Drawing.Size(0, 36);
            this.lDescripcion.TabIndex = 14;
            this.lDescripcion.Click += new System.EventHandler(this.lUnidades_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.textBox1.Location = new System.Drawing.Point(294, 171);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(180, 34);
            this.textBox1.TabIndex = 8;
            // 
            // tbLotes
            // 
            this.tbLotes.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.tbLotes.Location = new System.Drawing.Point(691, 80);
            this.tbLotes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLotes.Name = "tbLotes";
            this.tbLotes.Size = new System.Drawing.Size(88, 34);
            this.tbLotes.TabIndex = 16;
            this.tbLotes.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // button2
            // 
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(509, 168);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(201, 39);
            this.button2.TabIndex = 9;
            this.button2.Text = "Buscar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // finLote
            // 
            this.finLote.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("finLote.BackgroundImage")));
            this.finLote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.finLote.Controls.Add(this.panel7);
            this.finLote.Location = new System.Drawing.Point(274, 427);
            this.finLote.Margin = new System.Windows.Forms.Padding(4);
            this.finLote.Name = "finLote";
            this.finLote.Size = new System.Drawing.Size(452, 118);
            this.finLote.TabIndex = 35;
            this.finLote.Visible = false;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Red;
            this.panel7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel7.BackgroundImage")));
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel7.Location = new System.Drawing.Point(20, 17);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(100, 89);
            this.panel7.TabIndex = 37;
            // 
            // panel6
            // 
            this.panel6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel6.BackgroundImage")));
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Location = new System.Drawing.Point(274, 655);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(452, 118);
            this.panel6.TabIndex = 36;
            this.panel6.Visible = false;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Red;
            this.panel8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel8.BackgroundImage")));
            this.panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel8.Location = new System.Drawing.Point(43, 17);
            this.panel8.Margin = new System.Windows.Forms.Padding(4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(100, 89);
            this.panel8.TabIndex = 38;
            // 
            // epcs
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epcs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.epcs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epcs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.epcs.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.epcs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.epcs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.epcs.DefaultCellStyle = dataGridViewCellStyle3;
            this.epcs.GridColor = System.Drawing.Color.White;
            this.epcs.Location = new System.Drawing.Point(861, 475);
            this.epcs.Name = "epcs";
            this.epcs.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.NullValue = null;
            this.epcs.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.epcs.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epcs.RowTemplate.Height = 24;
            this.epcs.RowTemplate.ReadOnly = true;
            this.epcs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.epcs.Size = new System.Drawing.Size(1408, 599);
            this.epcs.TabIndex = 37;
            this.epcs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.epcs_CellContentClick);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label25.Font = new System.Drawing.Font("Microsoft YaHei", 20F);
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label25.Location = new System.Drawing.Point(413, 134);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(0, 45);
            this.label25.TabIndex = 19;
            // 
            // btnScanEPC
            // 
            this.btnScanEPC.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnScanEPC.Location = new System.Drawing.Point(408, 842);
            this.btnScanEPC.Margin = new System.Windows.Forms.Padding(4);
            this.btnScanEPC.Name = "btnScanEPC";
            this.btnScanEPC.Size = new System.Drawing.Size(240, 54);
            this.btnScanEPC.TabIndex = 34;
            this.btnScanEPC.Text = "  Iniciar lectura  ";
            this.btnScanEPC.UseCompatibleTextRendering = true;
            this.btnScanEPC.UseVisualStyleBackColor = false;
            this.btnScanEPC.Visible = false;
            this.btnScanEPC.Click += new System.EventHandler(this.button3_Click);
            // 
            // totalesOrden
            // 
            this.totalesOrden.AutoSize = true;
            this.totalesOrden.BackColor = System.Drawing.Color.White;
            this.totalesOrden.Font = new System.Drawing.Font("Microsoft YaHei", 20F);
            this.totalesOrden.ForeColor = System.Drawing.Color.Black;
            this.totalesOrden.Location = new System.Drawing.Point(1760, 420);
            this.totalesOrden.Name = "totalesOrden";
            this.totalesOrden.Size = new System.Drawing.Size(0, 45);
            this.totalesOrden.TabIndex = 29;
            this.totalesOrden.Click += new System.EventHandler(this.totalesOrden_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 20F);
            this.label9.ForeColor = System.Drawing.Color.Olive;
            this.label9.Location = new System.Drawing.Point(1036, 420);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 45);
            this.label9.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(233, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 89);
            this.panel1.TabIndex = 38;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.button1.Location = new System.Drawing.Point(22, 74);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 40);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Controls.Add(this.pTotal);
            this.panel9.Controls.Add(this.pLinea);
            this.panel9.Controls.Add(this.panel4);
            this.panel9.Controls.Add(this.panel3);
            this.panel9.Controls.Add(this.btnScanEPC);
            this.panel9.Controls.Add(this.tbLinea);
            this.panel9.Controls.Add(this.label9);
            this.panel9.Controls.Add(this.totalesOrden);
            this.panel9.Controls.Add(this.epcs);
            this.panel9.Controls.Add(this.panel6);
            this.panel9.Controls.Add(this.finLote);
            this.panel9.Controls.Add(this.button2);
            this.panel9.Controls.Add(this.textBox1);
            this.panel9.Location = new System.Drawing.Point(-30, -12);
            this.panel9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(2519, 914);
            this.panel9.TabIndex = 37;
            // 
            // pTotal
            // 
            this.pTotal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pTotal.BackgroundImage")));
            this.pTotal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pTotal.Location = new System.Drawing.Point(1491, 420);
            this.pTotal.Name = "pTotal";
            this.pTotal.Size = new System.Drawing.Size(220, 49);
            this.pTotal.TabIndex = 45;
            this.pTotal.Visible = false;
            // 
            // pLinea
            // 
            this.pLinea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pLinea.BackgroundImage")));
            this.pLinea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pLinea.Location = new System.Drawing.Point(71, 281);
            this.pLinea.Name = "pLinea";
            this.pLinea.Size = new System.Drawing.Size(190, 58);
            this.pLinea.TabIndex = 44;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(71, 161);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(190, 54);
            this.panel4.TabIndex = 43;
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
            this.panel3.Controls.Add(this.label25);
            this.panel3.Location = new System.Drawing.Point(861, 136);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(850, 253);
            this.panel3.TabIndex = 42;
            // 
            // pLotes
            // 
            this.pLotes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pLotes.BackgroundImage")));
            this.pLotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pLotes.Location = new System.Drawing.Point(421, 73);
            this.pLotes.Name = "pLotes";
            this.pLotes.Size = new System.Drawing.Size(239, 41);
            this.pLotes.TabIndex = 19;
            // 
            // panel5
            // 
            this.panel5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel5.BackgroundImage")));
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel5.Location = new System.Drawing.Point(19, 98);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(196, 47);
            this.panel5.TabIndex = 18;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
            // 
            // pReferencia
            // 
            this.pReferencia.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pReferencia.BackgroundImage")));
            this.pReferencia.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pReferencia.Location = new System.Drawing.Point(19, 15);
            this.pReferencia.Name = "pReferencia";
            this.pReferencia.Size = new System.Drawing.Size(196, 47);
            this.pReferencia.TabIndex = 17;
            this.pReferencia.Paint += new System.Windows.Forms.PaintEventHandler(this.pReferencia_Paint);
            // 
            // tbLinea
            // 
            this.tbLinea.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.tbLinea.Location = new System.Drawing.Point(317, 294);
            this.tbLinea.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLinea.Name = "tbLinea";
            this.tbLinea.Size = new System.Drawing.Size(62, 34);
            this.tbLinea.TabIndex = 41;
            this.tbLinea.Text = "1";
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.LightGray;
            this.panel11.Controls.Add(this.pPelPda);
            this.panel11.Controls.Add(this.panel2);
            this.panel11.Controls.Add(this.button1);
            this.panel11.Controls.Add(this.panel1);
            this.panel11.Location = new System.Drawing.Point(41, 20);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(2355, 110);
            this.panel11.TabIndex = 47;
            // 
            // pPelPda
            // 
            this.pPelPda.BackColor = System.Drawing.Color.LightGray;
            this.pPelPda.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pPelPda.BackgroundImage")));
            this.pPelPda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pPelPda.Location = new System.Drawing.Point(30, 4);
            this.pPelPda.Name = "pPelPda";
            this.pPelPda.Size = new System.Drawing.Size(119, 110);
            this.pPelPda.TabIndex = 46;
            this.pPelPda.TabStop = true;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(684, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(281, 73);
            this.panel2.TabIndex = 39;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(2316, 1129);
            this.Controls.Add(this.panel9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "UHF(1.3.2)";
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.ScanEPCForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.finLote.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.epcs)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Label lReferencia;
        private System.Windows.Forms.Label lCantidadTotal;
        private System.Windows.Forms.Label lDescripcion;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tbLotes;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel finLote;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.DataGridView epcs;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnScanEPC;
        private System.Windows.Forms.Label totalesOrden;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbLinea;
        private System.Windows.Forms.Panel panel3;

        #endregion

        private System.Windows.Forms.Panel pLinea;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pReferencia;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pLotes;
        private System.Windows.Forms.Panel pTotal;
        private System.Windows.Forms.Panel pPelPda;
        private System.Windows.Forms.Panel panel11;
    }
}