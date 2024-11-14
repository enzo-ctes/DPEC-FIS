namespace gagFIS_Interfase
{
    partial class FormEstadosRutas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEstadosRutas));
            this.iTalk_GroupBox1 = new iTalk.iTalk_GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LVEstados = new iTalk.iTalk_Listview();
            this.columnZona = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRemesa = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnRuta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCantidad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDisponibles = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProcesados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEnColect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDescargados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnExportados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCerrados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCerrar = new System.Windows.Forms.Button();
            this.BtnDisponibles = new iTalk.iTalk_Button_1();
            this.BtnHabCarga = new iTalk.iTalk_Button_1();
            this.BtnCerrarSaldos = new iTalk.iTalk_Button_1();
            this.BtnHabExp = new iTalk.iTalk_Button_1();
            this.iTalk_GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // iTalk_GroupBox1
            // 
            this.iTalk_GroupBox1.BackColor = System.Drawing.Color.DarkGray;
            this.iTalk_GroupBox1.Controls.Add(this.splitContainer1);
            this.iTalk_GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTalk_GroupBox1.Location = new System.Drawing.Point(0, 0);
            this.iTalk_GroupBox1.MinimumSize = new System.Drawing.Size(136, 50);
            this.iTalk_GroupBox1.Name = "iTalk_GroupBox1";
            this.iTalk_GroupBox1.Padding = new System.Windows.Forms.Padding(5, 28, 5, 5);
            this.iTalk_GroupBox1.Size = new System.Drawing.Size(780, 376);
            this.iTalk_GroupBox1.TabIndex = 0;
            this.iTalk_GroupBox1.Text = "Estados de Rutas";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(5, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LVEstados);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnCerrar);
            this.splitContainer1.Panel2.Controls.Add(this.BtnDisponibles);
            this.splitContainer1.Panel2.Controls.Add(this.BtnHabCarga);
            this.splitContainer1.Panel2.Controls.Add(this.BtnCerrarSaldos);
            this.splitContainer1.Panel2.Controls.Add(this.BtnHabExp);
            this.splitContainer1.Size = new System.Drawing.Size(770, 343);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 1;
            // 
            // LVEstados
            // 
            this.LVEstados.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.LVEstados.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LVEstados.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnZona,
            this.columnRemesa,
            this.columnRuta,
            this.columnCantidad,
            this.columnDisponibles,
            this.columnProcesados,
            this.columnEnColect,
            this.columnDescargados,
            this.columnExportados,
            this.columnCerrados});
            this.LVEstados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVEstados.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LVEstados.HideSelection = false;
            this.LVEstados.Location = new System.Drawing.Point(0, 0);
            this.LVEstados.Name = "LVEstados";
            this.LVEstados.Size = new System.Drawing.Size(770, 236);
            this.LVEstados.TabIndex = 0;
            this.LVEstados.UseCompatibleStateImageBehavior = false;
            this.LVEstados.View = System.Windows.Forms.View.Details;
            // 
            // columnZona
            // 
            this.columnZona.Text = "Zona";
            // 
            // columnRemesa
            // 
            this.columnRemesa.Text = "Remesa";
            this.columnRemesa.Width = 69;
            // 
            // columnRuta
            // 
            this.columnRuta.Text = "Ruta";
            this.columnRuta.Width = 72;
            // 
            // columnCantidad
            // 
            this.columnCantidad.Text = "Cantidad";
            this.columnCantidad.Width = 72;
            // 
            // columnDisponibles
            // 
            this.columnDisponibles.Text = "Disponibles/Saldos";
            this.columnDisponibles.Width = 111;
            // 
            // columnProcesados
            // 
            this.columnProcesados.Text = "Procesados";
            this.columnProcesados.Width = 72;
            // 
            // columnEnColect
            // 
            this.columnEnColect.Text = "En Colectora";
            this.columnEnColect.Width = 78;
            // 
            // columnDescargados
            // 
            this.columnDescargados.Text = "Descargados";
            this.columnDescargados.Width = 77;
            // 
            // columnExportados
            // 
            this.columnExportados.Text = "Exportados";
            this.columnExportados.Width = 93;
            // 
            // columnCerrados
            // 
            this.columnCerrados.Text = "Cerrados";
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(680, 55);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(87, 48);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // BtnDisponibles
            // 
            this.BtnDisponibles.BackColor = System.Drawing.Color.Transparent;
            this.BtnDisponibles.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDisponibles.Image = null;
            this.BtnDisponibles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDisponibles.Location = new System.Drawing.Point(24, 26);
            this.BtnDisponibles.Name = "BtnDisponibles";
            this.BtnDisponibles.Size = new System.Drawing.Size(129, 47);
            this.BtnDisponibles.TabIndex = 3;
            this.BtnDisponibles.Text = "VER Disponibles/Saldos";
            this.BtnDisponibles.TextAlignment = System.Drawing.StringAlignment.Center;
            this.BtnDisponibles.Click += new System.EventHandler(this.BtnDisponibles_Click);
            // 
            // BtnHabCarga
            // 
            this.BtnHabCarga.BackColor = System.Drawing.Color.Transparent;
            this.BtnHabCarga.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnHabCarga.Image = null;
            this.BtnHabCarga.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnHabCarga.Location = new System.Drawing.Point(159, 26);
            this.BtnHabCarga.Name = "BtnHabCarga";
            this.BtnHabCarga.Size = new System.Drawing.Size(131, 47);
            this.BtnHabCarga.TabIndex = 2;
            this.BtnHabCarga.Text = "Habilitar Carga";
            this.BtnHabCarga.TextAlignment = System.Drawing.StringAlignment.Center;
            this.BtnHabCarga.Click += new System.EventHandler(this.BtnHabCarga_Click);
            // 
            // BtnCerrarSaldos
            // 
            this.BtnCerrarSaldos.BackColor = System.Drawing.Color.Transparent;
            this.BtnCerrarSaldos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCerrarSaldos.Image = null;
            this.BtnCerrarSaldos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCerrarSaldos.Location = new System.Drawing.Point(296, 26);
            this.BtnCerrarSaldos.Name = "BtnCerrarSaldos";
            this.BtnCerrarSaldos.Size = new System.Drawing.Size(131, 47);
            this.BtnCerrarSaldos.TabIndex = 1;
            this.BtnCerrarSaldos.Text = "Cerrar Saldos";
            this.BtnCerrarSaldos.TextAlignment = System.Drawing.StringAlignment.Center;
            this.BtnCerrarSaldos.Click += new System.EventHandler(this.BtnCerrarSaldos_Click);
            // 
            // BtnHabExp
            // 
            this.BtnHabExp.BackColor = System.Drawing.Color.Transparent;
            this.BtnHabExp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnHabExp.Image = null;
            this.BtnHabExp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnHabExp.Location = new System.Drawing.Point(433, 26);
            this.BtnHabExp.Name = "BtnHabExp";
            this.BtnHabExp.Size = new System.Drawing.Size(128, 47);
            this.BtnHabExp.TabIndex = 0;
            this.BtnHabExp.Text = "Habilitar EXPORTACION";
            this.BtnHabExp.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // FormEstadosRutas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 376);
            this.ControlBox = false;
            this.Controls.Add(this.iTalk_GroupBox1);
            this.Name = "FormEstadosRutas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estados de Rutas";
            this.Load += new System.EventHandler(this.FormEstadosRutas_Load);
            this.iTalk_GroupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private iTalk.iTalk_GroupBox iTalk_GroupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private iTalk.iTalk_Listview LVEstados;
        private System.Windows.Forms.ColumnHeader columnZona;
        private System.Windows.Forms.ColumnHeader columnRuta;
        private System.Windows.Forms.ColumnHeader columnCantidad;
        private System.Windows.Forms.ColumnHeader columnDisponibles;
        private System.Windows.Forms.ColumnHeader columnProcesados;
        private System.Windows.Forms.ColumnHeader columnEnColect;
        private System.Windows.Forms.ColumnHeader columnDescargados;
        private System.Windows.Forms.ColumnHeader columnExportados;
        private iTalk.iTalk_Button_1 BtnDisponibles;
        private iTalk.iTalk_Button_1 BtnHabCarga;
        private iTalk.iTalk_Button_1 BtnCerrarSaldos;
        private iTalk.iTalk_Button_1 BtnHabExp;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.ColumnHeader columnRemesa;
        private System.Windows.Forms.ColumnHeader columnCerrados;
    }
}