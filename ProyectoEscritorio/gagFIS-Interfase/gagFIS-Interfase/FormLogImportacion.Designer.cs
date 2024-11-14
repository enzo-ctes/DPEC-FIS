namespace gagFIS_Interfase
{
    partial class LogImpApartados
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogImpApartados));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.TxtApartados = new System.Windows.Forms.Label();
            this.TxtImportados = new System.Windows.Forms.Label();
            this.TxtTotalUsuarios = new System.Windows.Forms.Label();
            this.TxtPorcion = new System.Windows.Forms.Label();
            this.LabelApartados = new System.Windows.Forms.Label();
            this.LabelTotalImportados = new System.Windows.Forms.Label();
            this.LabelTotalUsuarios = new System.Windows.Forms.Label();
            this.LabelPorcion = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LVDetalle = new System.Windows.Forms.ListView();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.btnCerrar);
            this.splitContainer1.Size = new System.Drawing.Size(411, 322);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TxtApartados);
            this.splitContainer2.Panel1.Controls.Add(this.TxtImportados);
            this.splitContainer2.Panel1.Controls.Add(this.TxtTotalUsuarios);
            this.splitContainer2.Panel1.Controls.Add(this.TxtPorcion);
            this.splitContainer2.Panel1.Controls.Add(this.LabelApartados);
            this.splitContainer2.Panel1.Controls.Add(this.LabelTotalImportados);
            this.splitContainer2.Panel1.Controls.Add(this.LabelTotalUsuarios);
            this.splitContainer2.Panel1.Controls.Add(this.LabelPorcion);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(411, 246);
            this.splitContainer2.SplitterDistance = 221;
            this.splitContainer2.TabIndex = 0;
            // 
            // TxtApartados
            // 
            this.TxtApartados.AutoSize = true;
            this.TxtApartados.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtApartados.ForeColor = System.Drawing.Color.Brown;
            this.TxtApartados.Location = new System.Drawing.Point(120, 140);
            this.TxtApartados.Name = "TxtApartados";
            this.TxtApartados.Size = new System.Drawing.Size(18, 18);
            this.TxtApartados.TabIndex = 7;
            this.TxtApartados.Text = "0";
            // 
            // TxtImportados
            // 
            this.TxtImportados.AutoSize = true;
            this.TxtImportados.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtImportados.ForeColor = System.Drawing.Color.DimGray;
            this.TxtImportados.Location = new System.Drawing.Point(120, 99);
            this.TxtImportados.Name = "TxtImportados";
            this.TxtImportados.Size = new System.Drawing.Size(18, 18);
            this.TxtImportados.TabIndex = 6;
            this.TxtImportados.Text = "0";
            // 
            // TxtTotalUsuarios
            // 
            this.TxtTotalUsuarios.AutoSize = true;
            this.TxtTotalUsuarios.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTotalUsuarios.ForeColor = System.Drawing.Color.DimGray;
            this.TxtTotalUsuarios.Location = new System.Drawing.Point(120, 58);
            this.TxtTotalUsuarios.Name = "TxtTotalUsuarios";
            this.TxtTotalUsuarios.Size = new System.Drawing.Size(18, 18);
            this.TxtTotalUsuarios.TabIndex = 5;
            this.TxtTotalUsuarios.Text = "0";
            // 
            // TxtPorcion
            // 
            this.TxtPorcion.AutoSize = true;
            this.TxtPorcion.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPorcion.ForeColor = System.Drawing.Color.DimGray;
            this.TxtPorcion.Location = new System.Drawing.Point(120, 22);
            this.TxtPorcion.Name = "TxtPorcion";
            this.TxtPorcion.Size = new System.Drawing.Size(18, 18);
            this.TxtPorcion.TabIndex = 4;
            this.TxtPorcion.Text = "0";
            // 
            // LabelApartados
            // 
            this.LabelApartados.AutoSize = true;
            this.LabelApartados.Location = new System.Drawing.Point(12, 143);
            this.LabelApartados.Name = "LabelApartados";
            this.LabelApartados.Size = new System.Drawing.Size(103, 13);
            this.LabelApartados.TabIndex = 3;
            this.LabelApartados.Text = "Total APARTADOS:";
            // 
            // LabelTotalImportados
            // 
            this.LabelTotalImportados.AutoSize = true;
            this.LabelTotalImportados.Location = new System.Drawing.Point(26, 102);
            this.LabelTotalImportados.Name = "LabelTotalImportados";
            this.LabelTotalImportados.Size = new System.Drawing.Size(89, 13);
            this.LabelTotalImportados.TabIndex = 2;
            this.LabelTotalImportados.Text = "Total Importados:";
            // 
            // LabelTotalUsuarios
            // 
            this.LabelTotalUsuarios.AutoSize = true;
            this.LabelTotalUsuarios.Location = new System.Drawing.Point(37, 61);
            this.LabelTotalUsuarios.Name = "LabelTotalUsuarios";
            this.LabelTotalUsuarios.Size = new System.Drawing.Size(78, 13);
            this.LabelTotalUsuarios.TabIndex = 1;
            this.LabelTotalUsuarios.Text = "Total Usuarios:";
            // 
            // LabelPorcion
            // 
            this.LabelPorcion.AutoSize = true;
            this.LabelPorcion.Location = new System.Drawing.Point(69, 25);
            this.LabelPorcion.Name = "LabelPorcion";
            this.LabelPorcion.Size = new System.Drawing.Size(46, 13);
            this.LabelPorcion.TabIndex = 0;
            this.LabelPorcion.Text = "Porcion:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LVDetalle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 246);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nº de Insalación de los Apartados";
            // 
            // LVDetalle
            // 
            this.LVDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVDetalle.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LVDetalle.HideSelection = false;
            this.LVDetalle.Location = new System.Drawing.Point(3, 16);
            this.LVDetalle.Name = "LVDetalle";
            this.LVDetalle.Size = new System.Drawing.Size(180, 227);
            this.LVDetalle.TabIndex = 0;
            this.LVDetalle.UseCompatibleStateImageBehavior = false;
            this.LVDetalle.View = System.Windows.Forms.View.List;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.Location = new System.Drawing.Point(282, 12);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(87, 48);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "CERRAR";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(97, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 48);
            this.button1.TabIndex = 6;
            this.button1.Text = "Guardar LOG";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LogImpApartados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 322);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogImpApartados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Importacion";
            this.Load += new System.EventHandler(this.DownloadYUpload_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label LabelTotalUsuarios;
        private System.Windows.Forms.Label LabelPorcion;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label LabelApartados;
        private System.Windows.Forms.Label LabelTotalImportados;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label TxtApartados;
        public System.Windows.Forms.Label TxtImportados;
        public System.Windows.Forms.Label TxtTotalUsuarios;
        public System.Windows.Forms.Label TxtPorcion;
        public System.Windows.Forms.ListView LVDetalle;
        private System.Windows.Forms.Button button1;
    }
}