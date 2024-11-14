/*
 * Creado por SharpDevelop.
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 13:39
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace gagFIS_Interfase
{
    partial class Form1Inicio
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public System.Windows.Forms.GroupBox frCmd;
        internal System.Windows.Forms.Button btnSalir;
        internal System.Windows.Forms.Button btnConfigCarp;
        internal System.Windows.Forms.Button btnCargas;
        internal System.Windows.Forms.Button btnDescargas;
        public System.Windows.Forms.Button btnExportacion;
        public System.Windows.Forms.ComboBox cboPeriodo;
        public System.Windows.Forms.Label lbPeriodo;
        private System.Windows.Forms.Button btnTest;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1Inicio));
            iTalk.ControlRenderer controlRenderer1 = new iTalk.ControlRenderer();
            iTalk.MSColorTable msColorTable1 = new iTalk.MSColorTable();
            this.frCmd = new System.Windows.Forms.GroupBox();
            this.PBExistArchImpSI = new System.Windows.Forms.PictureBox();
            this.PBExistArchImpNO = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.GBEntorno = new System.Windows.Forms.GroupBox();
            this.RBQAS = new System.Windows.Forms.CheckBox();
            this.RBPRD = new System.Windows.Forms.CheckBox();
            this.RBPrueba = new System.Windows.Forms.CheckBox();
            this.GBExtArch = new System.Windows.Forms.GroupBox();
            this.RButGPG = new System.Windows.Forms.RadioButton();
            this.RButBTX = new System.Windows.Forms.RadioButton();
            this.btnHistorial = new System.Windows.Forms.Button();
            this.BtnAddPeriodo = new System.Windows.Forms.Button();
            this.TextNewPeriodo = new System.Windows.Forms.TextBox();
            this.btnInfAltas = new System.Windows.Forms.Button();
            this.btnInfDesc = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnExportacion = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnConfigCarp = new System.Windows.Forms.Button();
            this.btnCargas = new System.Windows.Forms.Button();
            this.btnDescargas = new System.Windows.Forms.Button();
            this.cboPeriodo = new System.Windows.Forms.ComboBox();
            this.lbPeriodo = new System.Windows.Forms.Label();
            this.InfoImportacion = new System.Windows.Forms.Panel();
            this.LVResImpor = new System.Windows.Forms.ListView();
            this.Porcion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cantidad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Importados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Apartados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDLogImportacion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LblRutas = new iTalk.iTalk_Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.Download2plano = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.PorcLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxArchivoAImpor = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.TextBoxAgreArcImport = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.button5 = new System.Windows.Forms.Button();
            this.ImportarFacturasBGW = new System.ComponentModel.BackgroundWorker();
            this.ProgressBarImportarFact = new iTalk.iTalk_ProgressBar();
            this.iTalk_MenuStrip1 = new iTalk.iTalk_MenuStrip();
            this.soporteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verTicketsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.frCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBExistArchImpSI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBExistArchImpNO)).BeginInit();
            this.GBEntorno.SuspendLayout();
            this.GBExtArch.SuspendLayout();
            this.InfoImportacion.SuspendLayout();
            this.groupBoxArchivoAImpor.SuspendLayout();
            this.iTalk_MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // frCmd
            // 
            this.frCmd.AutoSize = true;
            this.frCmd.BackColor = System.Drawing.SystemColors.Control;
            this.frCmd.Controls.Add(this.PBExistArchImpSI);
            this.frCmd.Controls.Add(this.PBExistArchImpNO);
            this.frCmd.Controls.Add(this.button4);
            this.frCmd.Controls.Add(this.GBEntorno);
            this.frCmd.Controls.Add(this.GBExtArch);
            this.frCmd.Controls.Add(this.btnHistorial);
            this.frCmd.Controls.Add(this.BtnAddPeriodo);
            this.frCmd.Controls.Add(this.TextNewPeriodo);
            this.frCmd.Controls.Add(this.btnInfAltas);
            this.frCmd.Controls.Add(this.btnInfDesc);
            this.frCmd.Controls.Add(this.btnImportar);
            this.frCmd.Controls.Add(this.btnExportacion);
            this.frCmd.Controls.Add(this.btnSalir);
            this.frCmd.Controls.Add(this.btnConfigCarp);
            this.frCmd.Controls.Add(this.btnCargas);
            this.frCmd.Controls.Add(this.btnDescargas);
            this.frCmd.Controls.Add(this.cboPeriodo);
            this.frCmd.Controls.Add(this.lbPeriodo);
            this.frCmd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.frCmd.Location = new System.Drawing.Point(97, 168);
            this.frCmd.Name = "frCmd";
            this.frCmd.Padding = new System.Windows.Forms.Padding(0);
            this.frCmd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.frCmd.Size = new System.Drawing.Size(391, 456);
            this.frCmd.TabIndex = 1;
            this.frCmd.TabStop = false;
            this.frCmd.Text = " ";
            // 
            // PBExistArchImpSI
            // 
            this.PBExistArchImpSI.Image = ((System.Drawing.Image)(resources.GetObject("PBExistArchImpSI.Image")));
            this.PBExistArchImpSI.Location = new System.Drawing.Point(330, 77);
            this.PBExistArchImpSI.Name = "PBExistArchImpSI";
            this.PBExistArchImpSI.Size = new System.Drawing.Size(34, 30);
            this.PBExistArchImpSI.TabIndex = 42;
            this.PBExistArchImpSI.TabStop = false;
            this.PBExistArchImpSI.Visible = false;
            // 
            // PBExistArchImpNO
            // 
            this.PBExistArchImpNO.Image = ((System.Drawing.Image)(resources.GetObject("PBExistArchImpNO.Image")));
            this.PBExistArchImpNO.Location = new System.Drawing.Point(330, 77);
            this.PBExistArchImpNO.Name = "PBExistArchImpNO";
            this.PBExistArchImpNO.Size = new System.Drawing.Size(34, 30);
            this.PBExistArchImpNO.TabIndex = 43;
            this.PBExistArchImpNO.TabStop = false;
            this.PBExistArchImpNO.Visible = false;
            this.PBExistArchImpNO.Click += new System.EventHandler(this.PBExistArchImpNO_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Font = new System.Drawing.Font("Arial Narrow", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button4.Location = new System.Drawing.Point(18, 371);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(126, 31);
            this.button4.TabIndex = 41;
            this.button4.Text = "&Cerrar Sesion";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // GBEntorno
            // 
            this.GBEntorno.Controls.Add(this.RBQAS);
            this.GBEntorno.Controls.Add(this.RBPRD);
            this.GBEntorno.Controls.Add(this.RBPrueba);
            this.GBEntorno.Location = new System.Drawing.Point(326, 251);
            this.GBEntorno.Name = "GBEntorno";
            this.GBEntorno.Size = new System.Drawing.Size(60, 95);
            this.GBEntorno.TabIndex = 40;
            this.GBEntorno.TabStop = false;
            // 
            // RBQAS
            // 
            this.RBQAS.AutoSize = true;
            this.RBQAS.Location = new System.Drawing.Point(3, 45);
            this.RBQAS.Name = "RBQAS";
            this.RBQAS.Size = new System.Drawing.Size(48, 17);
            this.RBQAS.TabIndex = 37;
            this.RBQAS.Text = "QAS";
            this.RBQAS.UseVisualStyleBackColor = true;
            this.RBQAS.CheckedChanged += new System.EventHandler(this.RBQAS_CheckedChanged);
            // 
            // RBPRD
            // 
            this.RBPRD.AutoSize = true;
            this.RBPRD.Location = new System.Drawing.Point(3, 22);
            this.RBPRD.Name = "RBPRD";
            this.RBPRD.Size = new System.Drawing.Size(49, 17);
            this.RBPRD.TabIndex = 36;
            this.RBPRD.Text = "PRD";
            this.RBPRD.UseVisualStyleBackColor = true;
            this.RBPRD.CheckedChanged += new System.EventHandler(this.RBPRD_CheckedChanged);
            // 
            // RBPrueba
            // 
            this.RBPrueba.AutoSize = true;
            this.RBPrueba.Location = new System.Drawing.Point(3, 68);
            this.RBPrueba.Name = "RBPrueba";
            this.RBPrueba.Size = new System.Drawing.Size(60, 17);
            this.RBPrueba.TabIndex = 38;
            this.RBPrueba.Text = "Prueba";
            this.RBPrueba.UseVisualStyleBackColor = true;
            this.RBPrueba.CheckedChanged += new System.EventHandler(this.RBPrueba_CheckedChanged);
            // 
            // GBExtArch
            // 
            this.GBExtArch.Controls.Add(this.RButGPG);
            this.GBExtArch.Controls.Add(this.RButBTX);
            this.GBExtArch.Location = new System.Drawing.Point(326, 202);
            this.GBExtArch.Name = "GBExtArch";
            this.GBExtArch.Size = new System.Drawing.Size(60, 53);
            this.GBExtArch.TabIndex = 39;
            this.GBExtArch.TabStop = false;
            this.GBExtArch.Visible = false;
            // 
            // RButGPG
            // 
            this.RButGPG.AutoSize = true;
            this.RButGPG.Location = new System.Drawing.Point(5, 9);
            this.RButGPG.Name = "RButGPG";
            this.RButGPG.Size = new System.Drawing.Size(48, 17);
            this.RButGPG.TabIndex = 28;
            this.RButGPG.TabStop = true;
            this.RButGPG.Text = "GPG";
            this.RButGPG.UseVisualStyleBackColor = true;
            this.RButGPG.Visible = false;
            this.RButGPG.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RButGPG_MouseMove);
            // 
            // RButBTX
            // 
            this.RButBTX.AutoSize = true;
            this.RButBTX.Location = new System.Drawing.Point(5, 28);
            this.RButBTX.Name = "RButBTX";
            this.RButBTX.Size = new System.Drawing.Size(46, 17);
            this.RButBTX.TabIndex = 27;
            this.RButBTX.TabStop = true;
            this.RButBTX.Text = "BTX";
            this.RButBTX.UseVisualStyleBackColor = true;
            this.RButBTX.Visible = false;
            this.RButBTX.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RButBTX_MouseMove);
            // 
            // btnHistorial
            // 
            this.btnHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnHistorial.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnHistorial.Location = new System.Drawing.Point(24, 407);
            this.btnHistorial.Name = "btnHistorial";
            this.btnHistorial.Size = new System.Drawing.Size(46, 33);
            this.btnHistorial.TabIndex = 29;
            this.btnHistorial.Text = "Historial";
            this.btnHistorial.UseVisualStyleBackColor = false;
            this.btnHistorial.Visible = false;
            this.btnHistorial.Click += new System.EventHandler(this.btnHistorial_Click);
            // 
            // BtnAddPeriodo
            // 
            this.BtnAddPeriodo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnAddPeriodo.Location = new System.Drawing.Point(325, 26);
            this.BtnAddPeriodo.Name = "BtnAddPeriodo";
            this.BtnAddPeriodo.Size = new System.Drawing.Size(22, 22);
            this.BtnAddPeriodo.TabIndex = 19;
            this.BtnAddPeriodo.UseVisualStyleBackColor = true;
            this.BtnAddPeriodo.Click += new System.EventHandler(this.BtnAddPeriodo_Click);
            // 
            // TextNewPeriodo
            // 
            this.TextNewPeriodo.Location = new System.Drawing.Point(254, 24);
            this.TextNewPeriodo.Multiline = true;
            this.TextNewPeriodo.Name = "TextNewPeriodo";
            this.TextNewPeriodo.Size = new System.Drawing.Size(65, 24);
            this.TextNewPeriodo.TabIndex = 18;
            this.TextNewPeriodo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextNewPeriodo_KeyDown);
            // 
            // btnInfAltas
            // 
            this.btnInfAltas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnInfAltas.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfAltas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnInfAltas.Location = new System.Drawing.Point(11, 281);
            this.btnInfAltas.Name = "btnInfAltas";
            this.btnInfAltas.Size = new System.Drawing.Size(313, 43);
            this.btnInfAltas.TabIndex = 17;
            this.btnInfAltas.Text = "Informes de Altas";
            this.btnInfAltas.UseVisualStyleBackColor = false;
            this.btnInfAltas.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnInfDesc
            // 
            this.btnInfDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnInfDesc.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnInfDesc.Location = new System.Drawing.Point(193, 402);
            this.btnInfDesc.Name = "btnInfDesc";
            this.btnInfDesc.Size = new System.Drawing.Size(108, 24);
            this.btnInfDesc.TabIndex = 16;
            this.btnInfDesc.Text = "Estadisticas y Comparaciones";
            this.btnInfDesc.UseVisualStyleBackColor = false;
            this.btnInfDesc.Visible = false;
            this.btnInfDesc.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnImportar.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnImportar.Location = new System.Drawing.Point(11, 64);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(313, 43);
            this.btnImportar.TabIndex = 15;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = false;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnExportacion
            // 
            this.btnExportacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnExportacion.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnExportacion.Location = new System.Drawing.Point(11, 118);
            this.btnExportacion.Name = "btnExportacion";
            this.btnExportacion.Size = new System.Drawing.Size(313, 43);
            this.btnExportacion.TabIndex = 10;
            this.btnExportacion.Text = "Exportar";
            this.btnExportacion.UseVisualStyleBackColor = false;
            this.btnExportacion.Click += new System.EventHandler(this.btnExportacion_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSalir.Font = new System.Drawing.Font("Arial Narrow", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnSalir.Location = new System.Drawing.Point(219, 372);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(122, 31);
            this.btnSalir.TabIndex = 14;
            this.btnSalir.Text = "&Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnConfigCarp
            // 
            this.btnConfigCarp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(230)))));
            this.btnConfigCarp.Font = new System.Drawing.Font("Arial Narrow", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigCarp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnConfigCarp.Location = new System.Drawing.Point(24, 402);
            this.btnConfigCarp.Name = "btnConfigCarp";
            this.btnConfigCarp.Size = new System.Drawing.Size(125, 29);
            this.btnConfigCarp.TabIndex = 13;
            this.btnConfigCarp.Text = "Configurar Carpetas";
            this.btnConfigCarp.UseVisualStyleBackColor = false;
            this.btnConfigCarp.Visible = false;
            this.btnConfigCarp.Click += new System.EventHandler(this.btnConfigCarp_Click);
            // 
            // btnCargas
            // 
            this.btnCargas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnCargas.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnCargas.Location = new System.Drawing.Point(11, 172);
            this.btnCargas.Name = "btnCargas";
            this.btnCargas.Size = new System.Drawing.Size(313, 43);
            this.btnCargas.TabIndex = 12;
            this.btnCargas.Text = "CARGAS de Colectoras";
            this.btnCargas.UseVisualStyleBackColor = false;
            this.btnCargas.Click += new System.EventHandler(this.btnCargas_Click);
            this.btnCargas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnCargas_KeyPress);
            // 
            // btnDescargas
            // 
            this.btnDescargas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(210)))));
            this.btnDescargas.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDescargas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnDescargas.Location = new System.Drawing.Point(11, 227);
            this.btnDescargas.Name = "btnDescargas";
            this.btnDescargas.Size = new System.Drawing.Size(313, 43);
            this.btnDescargas.TabIndex = 11;
            this.btnDescargas.Text = "DESCARGAS Colectoras";
            this.btnDescargas.UseVisualStyleBackColor = false;
            this.btnDescargas.Click += new System.EventHandler(this.btnDescargas_Click);
            // 
            // cboPeriodo
            // 
            this.cboPeriodo.BackColor = System.Drawing.SystemColors.Window;
            this.cboPeriodo.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.cboPeriodo.FormatString = "\"0000-00\"";
            this.cboPeriodo.FormattingEnabled = true;
            this.cboPeriodo.Location = new System.Drawing.Point(148, 24);
            this.cboPeriodo.Name = "cboPeriodo";
            this.cboPeriodo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cboPeriodo.Size = new System.Drawing.Size(100, 24);
            this.cboPeriodo.TabIndex = 1;
            this.cboPeriodo.SelectedIndexChanged += new System.EventHandler(this.cboPeriodo_SelectedIndexChanged);
            // 
            // lbPeriodo
            // 
            this.lbPeriodo.AutoSize = true;
            this.lbPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.lbPeriodo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbPeriodo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPeriodo.ForeColor = System.Drawing.Color.Blue;
            this.lbPeriodo.Location = new System.Drawing.Point(20, 28);
            this.lbPeriodo.Name = "lbPeriodo";
            this.lbPeriodo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbPeriodo.Size = new System.Drawing.Size(130, 19);
            this.lbPeriodo.TabIndex = 4;
            this.lbPeriodo.Text = "Seleccione Periodo ";
            // 
            // InfoImportacion
            // 
            this.InfoImportacion.Controls.Add(this.LVResImpor);
            this.InfoImportacion.Controls.Add(this.LblRutas);
            this.InfoImportacion.Location = new System.Drawing.Point(494, 82);
            this.InfoImportacion.Name = "InfoImportacion";
            this.InfoImportacion.Size = new System.Drawing.Size(343, 432);
            this.InfoImportacion.TabIndex = 44;
            // 
            // LVResImpor
            // 
            this.LVResImpor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Porcion,
            this.Cantidad,
            this.Importados,
            this.Apartados,
            this.IDLogImportacion});
            this.LVResImpor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LVResImpor.HideSelection = false;
            this.LVResImpor.Location = new System.Drawing.Point(3, 37);
            this.LVResImpor.Name = "LVResImpor";
            this.LVResImpor.Size = new System.Drawing.Size(337, 382);
            this.LVResImpor.TabIndex = 3;
            this.LVResImpor.UseCompatibleStateImageBehavior = false;
            this.LVResImpor.View = System.Windows.Forms.View.Details;
            this.LVResImpor.SelectedIndexChanged += new System.EventHandler(this.LVResImpor_SelectedIndexChanged);
            this.LVResImpor.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LVResImpor_MouseDoubleClick);
            // 
            // Porcion
            // 
            this.Porcion.Text = "Porcion";
            this.Porcion.Width = 102;
            // 
            // Cantidad
            // 
            this.Cantidad.Text = "Cantidad";
            this.Cantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Cantidad.Width = 64;
            // 
            // Importados
            // 
            this.Importados.Text = "Importados";
            this.Importados.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Importados.Width = 78;
            // 
            // Apartados
            // 
            this.Apartados.Text = "Apartados";
            this.Apartados.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Apartados.Width = 88;
            // 
            // IDLogImportacion
            // 
            this.IDLogImportacion.Text = "IDLogImportacion";
            // 
            // LblRutas
            // 
            this.LblRutas.AutoSize = true;
            this.LblRutas.BackColor = System.Drawing.Color.Transparent;
            this.LblRutas.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblRutas.ForeColor = System.Drawing.Color.DimGray;
            this.LblRutas.Location = new System.Drawing.Point(98, 7);
            this.LblRutas.Name = "LblRutas";
            this.LblRutas.Size = new System.Drawing.Size(128, 20);
            this.LblRutas.TabIndex = 2;
            this.LblRutas.Text = "Rutas Importadas:";
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.SeaGreen;
            this.btnTest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnTest.Location = new System.Drawing.Point(12, 222);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(79, 69);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Cambiar Formato Fecha";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Download2plano
            // 
            this.Download2plano.WorkerReportsProgress = true;
            this.Download2plano.WorkerSupportsCancellation = true;
            this.Download2plano.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Download2plano_DoWork_1);
            this.Download2plano.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Download2plano_ProgressChanged);
            this.Download2plano.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Download2plano_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(423, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(267, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "Por favor no interrumpa este proceso.";
            this.label2.Visible = false;
            // 
            // PorcLabel
            // 
            this.PorcLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PorcLabel.AutoSize = true;
            this.PorcLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PorcLabel.Location = new System.Drawing.Point(667, 90);
            this.PorcLabel.Name = "PorcLabel";
            this.PorcLabel.Size = new System.Drawing.Size(50, 16);
            this.PorcLabel.TabIndex = 23;
            this.PorcLabel.Text = "label2";
            this.PorcLabel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.progressBar.Location = new System.Drawing.Point(455, 82);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(204, 31);
            this.progressBar.TabIndex = 22;
            this.progressBar.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(470, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "Realizando Importación...";
            this.label1.Visible = false;
            // 
            // groupBoxArchivoAImpor
            // 
            this.groupBoxArchivoAImpor.Controls.Add(this.button1);
            this.groupBoxArchivoAImpor.Controls.Add(this.TextBoxAgreArcImport);
            this.groupBoxArchivoAImpor.Location = new System.Drawing.Point(97, 60);
            this.groupBoxArchivoAImpor.Name = "groupBoxArchivoAImpor";
            this.groupBoxArchivoAImpor.Size = new System.Drawing.Size(186, 46);
            this.groupBoxArchivoAImpor.TabIndex = 26;
            this.groupBoxArchivoAImpor.TabStop = false;
            this.groupBoxArchivoAImpor.Text = "Agregar Archivo de Importación";
            this.groupBoxArchivoAImpor.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 23);
            this.button1.TabIndex = 27;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // TextBoxAgreArcImport
            // 
            this.TextBoxAgreArcImport.Location = new System.Drawing.Point(6, 19);
            this.TextBoxAgreArcImport.Name = "TextBoxAgreArcImport";
            this.TextBoxAgreArcImport.Size = new System.Drawing.Size(126, 20);
            this.TextBoxAgreArcImport.TabIndex = 27;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.IndianRed;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(12, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 75);
            this.button2.TabIndex = 29;
            this.button2.Text = "Limpiar Base General";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.IndianRed;
            this.button3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button3.Location = new System.Drawing.Point(12, 139);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(79, 75);
            this.button3.TabIndex = 30;
            this.button3.Text = "Limpia Tabla Conceptos Facturados";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // LabelVersion
            // 
            this.LabelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelVersion.AutoSize = true;
            this.LabelVersion.Location = new System.Drawing.Point(796, 35);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(98, 13);
            this.LabelVersion.TabIndex = 31;
            this.LabelVersion.Text = "V: 20240508.01.04";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 408);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 39);
            this.button5.TabIndex = 34;
            this.button5.Text = "Importar Facturas";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // ImportarFacturasBGW
            // 
            this.ImportarFacturasBGW.WorkerReportsProgress = true;
            this.ImportarFacturasBGW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ImportarFacturasBGW_DoWork);
            this.ImportarFacturasBGW.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ImportarFacturasBGW_ProgressChanged);
            this.ImportarFacturasBGW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ImportarFacturasBGW_RunWorkerCompleted);
            // 
            // ProgressBarImportarFact
            // 
            this.ProgressBarImportarFact.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.ProgressBarImportarFact.Location = new System.Drawing.Point(0, 302);
            this.ProgressBarImportarFact.Maximum = ((long)(100));
            this.ProgressBarImportarFact.MinimumSize = new System.Drawing.Size(100, 100);
            this.ProgressBarImportarFact.Name = "ProgressBarImportarFact";
            this.ProgressBarImportarFact.ProgressColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.ProgressBarImportarFact.ProgressColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.ProgressBarImportarFact.ProgressShape = iTalk.iTalk_ProgressBar._ProgressShape.Round;
            this.ProgressBarImportarFact.Size = new System.Drawing.Size(100, 100);
            this.ProgressBarImportarFact.TabIndex = 33;
            this.ProgressBarImportarFact.Text = "iTalk_ProgressBar1";
            this.ProgressBarImportarFact.Value = ((long)(0));
            this.ProgressBarImportarFact.Visible = false;
            // 
            // iTalk_MenuStrip1
            // 
            this.iTalk_MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.soporteToolStripMenuItem});
            this.iTalk_MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.iTalk_MenuStrip1.Name = "iTalk_MenuStrip1";
            controlRenderer1.ColorTable = msColorTable1;
            controlRenderer1.RoundedEdges = true;
            this.iTalk_MenuStrip1.Renderer = controlRenderer1;
            this.iTalk_MenuStrip1.Size = new System.Drawing.Size(906, 24);
            this.iTalk_MenuStrip1.TabIndex = 45;
            this.iTalk_MenuStrip1.Text = "iTalk_MenuStrip1";
            // 
            // soporteToolStripMenuItem
            // 
            this.soporteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarTicketToolStripMenuItem,
            this.verTicketsToolStripMenuItem});
            this.soporteToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.soporteToolStripMenuItem.Name = "soporteToolStripMenuItem";
            this.soporteToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.soporteToolStripMenuItem.Text = "Soporte";
            // 
            // generarTicketToolStripMenuItem
            // 
            this.generarTicketToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.generarTicketToolStripMenuItem.Name = "generarTicketToolStripMenuItem";
            this.generarTicketToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.generarTicketToolStripMenuItem.Text = "NuevoTicket";
            this.generarTicketToolStripMenuItem.Click += new System.EventHandler(this.generarTicketToolStripMenuItem_Click);
            // 
            // verTicketsToolStripMenuItem
            // 
            this.verTicketsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.verTicketsToolStripMenuItem.Name = "verTicketsToolStripMenuItem";
            this.verTicketsToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.verTicketsToolStripMenuItem.Text = "Ver Tickets";
            this.verTicketsToolStripMenuItem.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(796, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "V: 20240508.01.03";
            this.label3.Visible = false;
            // 
            // Form1Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnSalir;
            this.ClientSize = new System.Drawing.Size(906, 519);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.InfoImportacion);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.ProgressBarImportarFact);
            this.Controls.Add(this.LabelVersion);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBoxArchivoAImpor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PorcLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.frCmd);
            this.Controls.Add(this.iTalk_MenuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(100, 100);
            this.MainMenuStrip = this.iTalk_MenuStrip1;
            this.Name = "Form1Inicio";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.frCmd.ResumeLayout(false);
            this.frCmd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBExistArchImpSI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBExistArchImpNO)).EndInit();
            this.GBEntorno.ResumeLayout(false);
            this.GBEntorno.PerformLayout();
            this.GBExtArch.ResumeLayout(false);
            this.GBExtArch.PerformLayout();
            this.InfoImportacion.ResumeLayout(false);
            this.InfoImportacion.PerformLayout();
            this.groupBoxArchivoAImpor.ResumeLayout(false);
            this.groupBoxArchivoAImpor.PerformLayout();
            this.iTalk_MenuStrip1.ResumeLayout(false);
            this.iTalk_MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public System.Windows.Forms.Button btnImportar;
        public System.Windows.Forms.Button btnInfDesc;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Button btnInfAltas;
        public System.Windows.Forms.TextBox TextNewPeriodo;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Button BtnAddPeriodo;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label PorcLabel;
        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label label1;
        //private iTalk.iTalk_ProgressBar iTalk_ProgressBar1;
        private System.Windows.Forms.GroupBox groupBoxArchivoAImpor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TextBoxAgreArcImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.RadioButton RButGPG;
        public System.Windows.Forms.RadioButton RButBTX;
        public System.ComponentModel.BackgroundWorker Download2plano;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button btnHistorial;
        public System.Windows.Forms.CheckBox RBPrueba;
        public System.Windows.Forms.CheckBox RBQAS;
        public System.Windows.Forms.CheckBox RBPRD;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.GroupBox GBEntorno;
        public System.Windows.Forms.GroupBox GBExtArch;
        public System.Windows.Forms.Timer timer2;
        internal System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label LabelVersion;
        public System.Windows.Forms.PictureBox PBExistArchImpNO;
        private System.Windows.Forms.ToolTip toolTip2;
        private iTalk.iTalk_ProgressBar ProgressBarImportarFact;
        private System.Windows.Forms.Button button5;
        public System.ComponentModel.BackgroundWorker ImportarFacturasBGW;
        private iTalk.iTalk_Label LblRutas;
        private System.Windows.Forms.ListView LVResImpor;
        public System.Windows.Forms.ColumnHeader Porcion;
        public System.Windows.Forms.ColumnHeader Cantidad;
        public System.Windows.Forms.ColumnHeader Importados;
        public System.Windows.Forms.ColumnHeader Apartados;
        public System.Windows.Forms.Panel InfoImportacion;
        public System.Windows.Forms.PictureBox PBExistArchImpSI;
        private System.Windows.Forms.ColumnHeader IDLogImportacion;
        private iTalk.iTalk_MenuStrip iTalk_MenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem soporteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generarTicketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verTicketsToolStripMenuItem;
        private System.Windows.Forms.Label label3;
    }
}
