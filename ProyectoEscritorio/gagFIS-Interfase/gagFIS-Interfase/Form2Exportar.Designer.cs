/*
 * Creado por SharpDevelop.
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 13:53
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace gagFIS_Interfase
{
    partial class Form2Exportar
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCerrar;
        
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2Exportar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.CheckCambiarFechaLectura = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.UpPorRuta = new System.Windows.Forms.CheckBox();
            this.labelPanExpXRuta = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RadioButtonNO = new System.Windows.Forms.RadioButton();
            this.RadioButtonSI = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbLote = new System.Windows.Forms.CheckBox();
            this.cbTodo = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button12 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tvlotes = new System.Windows.Forms.TreeView();
            this.tvConexUpdload = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.GBCambioFecha = new iTalk.iTalk_GroupBox();
            this.TBFechaModificada = new System.Windows.Forms.DateTimePicker();
            this.LabFechaLectura = new iTalk.iTalk_Label();
            this.iTalk_Label1 = new iTalk.iTalk_Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LbPorcentaje = new System.Windows.Forms.Label();
            this.progressBarExpor = new System.Windows.Forms.ProgressBar();
            this.BtnIniciarExpor = new System.Windows.Forms.Button();
            this.TareaSegundoPlano1 = new System.ComponentModel.BackgroundWorker();
            this.imgList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CBImposiblesApagados = new System.Windows.Forms.ComboBox();
            this.CBLeidosNOPrint = new System.Windows.Forms.ComboBox();
            this.LabelAltas = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.LabelErrores = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.LabelTodos = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CBFiltroZona = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CBRemesaSola = new System.Windows.Forms.ComboBox();
            this.RBRemesaSola = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.RBRuta = new System.Windows.Forms.RadioButton();
            this.RBRemesasTodas = new System.Windows.Forms.RadioButton();
            this.TextBoxHasta = new System.Windows.Forms.TextBox();
            this.TextBoxDesde = new System.Windows.Forms.TextBox();
            this.LabelHasta = new System.Windows.Forms.Label();
            this.LabelDesde = new System.Windows.Forms.Label();
            this.TextBoxRuta = new System.Windows.Forms.TextBox();
            this.LabelRuta = new System.Windows.Forms.Label();
            this.LabelRemesaRuta = new System.Windows.Forms.Label();
            this.CBRemesaRuta = new System.Windows.Forms.ComboBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.LabIndNoPrint = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.LabSaldos = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.LabNoImprFueraRango = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.LabLeidNoImprePre = new System.Windows.Forms.Label();
            this.LabImprPre = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.PickBoxLoading = new System.Windows.Forms.PictureBox();
            this.tvExportadas = new System.Windows.Forms.TreeView();
            this.informesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.BGWEsperar = new System.ComponentModel.BackgroundWorker();
            this.resumenesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.impresasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leidasNOImpresasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noImpresasPorFueraDeRangoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noImpresosPorOtrosMotivosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saldosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.informesDeExportacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BGWLoading = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.GBCambioFecha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PickBoxLoading)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CheckCambiarFechaLectura);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.labelPanExpXRuta);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1370, 96);
            this.panel1.TabIndex = 0;
            // 
            // CheckCambiarFechaLectura
            // 
            this.CheckCambiarFechaLectura.AutoSize = true;
            this.CheckCambiarFechaLectura.Location = new System.Drawing.Point(713, 29);
            this.CheckCambiarFechaLectura.Name = "CheckCambiarFechaLectura";
            this.CheckCambiarFechaLectura.Size = new System.Drawing.Size(173, 17);
            this.CheckCambiarFechaLectura.TabIndex = 6;
            this.CheckCambiarFechaLectura.Text = "Cambiar FECHA DE LECTURA";
            this.CheckCambiarFechaLectura.UseVisualStyleBackColor = true;
            this.CheckCambiarFechaLectura.Visible = false;
            this.CheckCambiarFechaLectura.CheckedChanged += new System.EventHandler(this.ChecCambiarFechaLectura_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.UpPorRuta);
            this.groupBox6.Location = new System.Drawing.Point(169, -1);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(523, 87);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Cantidad de Archivos";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(103, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(414, 44);
            this.label9.TabIndex = 5;
            this.label9.Text = resources.GetString("label9.Text");
            // 
            // UpPorRuta
            // 
            this.UpPorRuta.AutoSize = true;
            this.UpPorRuta.Location = new System.Drawing.Point(6, 37);
            this.UpPorRuta.Name = "UpPorRuta";
            this.UpPorRuta.Size = new System.Drawing.Size(99, 17);
            this.UpPorRuta.TabIndex = 0;
            this.UpPorRuta.Text = "Upload por ruta";
            this.UpPorRuta.UseVisualStyleBackColor = true;
            // 
            // labelPanExpXRuta
            // 
            this.labelPanExpXRuta.AutoSize = true;
            this.labelPanExpXRuta.Font = new System.Drawing.Font("Sitka Small", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPanExpXRuta.Location = new System.Drawing.Point(3, 65);
            this.labelPanExpXRuta.Name = "labelPanExpXRuta";
            this.labelPanExpXRuta.Size = new System.Drawing.Size(150, 19);
            this.labelPanExpXRuta.TabIndex = 5;
            this.labelPanExpXRuta.Text = "Rutas para exportar";
            this.labelPanExpXRuta.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RadioButtonNO);
            this.groupBox2.Controls.Add(this.RadioButtonSI);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(970, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(78, 70);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exportar Saldos";
            this.groupBox2.Visible = false;
            // 
            // RadioButtonNO
            // 
            this.RadioButtonNO.AutoSize = true;
            this.RadioButtonNO.Location = new System.Drawing.Point(338, 48);
            this.RadioButtonNO.Name = "RadioButtonNO";
            this.RadioButtonNO.Size = new System.Drawing.Size(41, 17);
            this.RadioButtonNO.TabIndex = 4;
            this.RadioButtonNO.Text = "NO";
            this.RadioButtonNO.UseVisualStyleBackColor = true;
            // 
            // RadioButtonSI
            // 
            this.RadioButtonSI.AutoSize = true;
            this.RadioButtonSI.Checked = true;
            this.RadioButtonSI.ForeColor = System.Drawing.Color.Black;
            this.RadioButtonSI.Location = new System.Drawing.Point(263, 48);
            this.RadioButtonSI.Name = "RadioButtonSI";
            this.RadioButtonSI.Size = new System.Drawing.Size(35, 17);
            this.RadioButtonSI.TabIndex = 3;
            this.RadioButtonSI.TabStop = true;
            this.RadioButtonSI.Text = "SI";
            this.RadioButtonSI.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(84, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "NO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(87, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "SI";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(587, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Si selecciona         se exportarán  todos las conexiones Leidas y No Leidas.\r\nSi" +
    " selecciona          se exportaran las conexiones Leidas y quedan las no Leidas " +
    "para su carga nuevamente.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbLote);
            this.groupBox1.Controls.Add(this.cbTodo);
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 70);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opciones de Exportación";
            // 
            // cbLote
            // 
            this.cbLote.AutoSize = true;
            this.cbLote.Enabled = false;
            this.cbLote.Location = new System.Drawing.Point(29, 43);
            this.cbLote.Name = "cbLote";
            this.cbLote.Size = new System.Drawing.Size(114, 17);
            this.cbLote.TabIndex = 0;
            this.cbLote.Text = "Seleccion de Ruta";
            this.cbLote.UseVisualStyleBackColor = true;
            this.cbLote.CheckedChanged += new System.EventHandler(this.cbLote_CheckedChanged);
            this.cbLote.CheckStateChanged += new System.EventHandler(this.cbLote_CheckStateChanged);
            // 
            // cbTodo
            // 
            this.cbTodo.AutoSize = true;
            this.cbTodo.Location = new System.Drawing.Point(29, 18);
            this.cbTodo.Name = "cbTodo";
            this.cbTodo.Size = new System.Drawing.Size(93, 17);
            this.cbTodo.TabIndex = 1;
            this.cbTodo.Text = "Exportar Todo";
            this.cbTodo.UseVisualStyleBackColor = true;
            this.cbTodo.Visible = false;
            this.cbTodo.CheckedChanged += new System.EventHandler(this.cbTodo_CheckedChanged);
            this.cbTodo.CheckStateChanged += new System.EventHandler(this.cbTodo_CheckStateChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button12);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.button7);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 582);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1370, 93);
            this.panel2.TabIndex = 1;
            // 
            // button12
            // 
            this.button12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(74, 22);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(127, 34);
            this.button12.TabIndex = 31;
            this.button12.Text = "Ver Rutas Exportadas";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Visible = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(375, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 30;
            this.textBox1.Visible = false;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.button7.Image = ((System.Drawing.Image)(resources.GetObject("button7.Image")));
            this.button7.Location = new System.Drawing.Point(856, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(32, 35);
            this.button7.TabIndex = 29;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(251, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 49);
            this.button1.TabIndex = 16;
            this.button1.Text = "Prueba";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(1265, 7);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(93, 46);
            this.btnCerrar.TabIndex = 3;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tvlotes);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 120);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 462);
            this.panel3.TabIndex = 2;
            // 
            // tvlotes
            // 
            this.tvlotes.Location = new System.Drawing.Point(0, 0);
            this.tvlotes.Name = "tvlotes";
            this.tvlotes.Size = new System.Drawing.Size(239, 467);
            this.tvlotes.TabIndex = 0;
            this.tvlotes.Visible = false;
            this.tvlotes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvlotes_AfterSelect);
            // 
            // tvConexUpdload
            // 
            this.tvConexUpdload.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.tvConexUpdload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvConexUpdload.HideSelection = false;
            this.tvConexUpdload.Location = new System.Drawing.Point(0, 0);
            this.tvConexUpdload.Name = "tvConexUpdload";
            this.tvConexUpdload.Size = new System.Drawing.Size(462, 208);
            this.tvConexUpdload.TabIndex = 1;
            this.tvConexUpdload.UseCompatibleStateImageBehavior = false;
            this.tvConexUpdload.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Datos de Exportación";
            this.columnHeader1.Width = 500;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvConexUpdload);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView2);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GBCambioFecha);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.LbPorcentaje);
            this.splitContainer1.Panel2.Controls.Add(this.progressBarExpor);
            this.splitContainer1.Panel2.Controls.Add(this.BtnIniciarExpor);
            this.splitContainer1.Size = new System.Drawing.Size(466, 462);
            this.splitContainer1.SplitterDistance = 212;
            this.splitContainer1.TabIndex = 3;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(27, 4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(379, 189);
            this.dataGridView2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(204, 188);
            this.dataGridView1.TabIndex = 0;
            // 
            // GBCambioFecha
            // 
            this.GBCambioFecha.BackColor = System.Drawing.Color.Transparent;
            this.GBCambioFecha.Controls.Add(this.TBFechaModificada);
            this.GBCambioFecha.Controls.Add(this.LabFechaLectura);
            this.GBCambioFecha.Controls.Add(this.iTalk_Label1);
            this.GBCambioFecha.Location = new System.Drawing.Point(235, 68);
            this.GBCambioFecha.MinimumSize = new System.Drawing.Size(136, 50);
            this.GBCambioFecha.Name = "GBCambioFecha";
            this.GBCambioFecha.Padding = new System.Windows.Forms.Padding(5, 28, 5, 5);
            this.GBCambioFecha.Size = new System.Drawing.Size(171, 102);
            this.GBCambioFecha.TabIndex = 19;
            this.GBCambioFecha.Text = "Establecer Fecha de Lectura";
            this.GBCambioFecha.Visible = false;
            // 
            // TBFechaModificada
            // 
            this.TBFechaModificada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBFechaModificada.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TBFechaModificada.Location = new System.Drawing.Point(161, 114);
            this.TBFechaModificada.Name = "TBFechaModificada";
            this.TBFechaModificada.Size = new System.Drawing.Size(130, 26);
            this.TBFechaModificada.TabIndex = 20;
            // 
            // LabFechaLectura
            // 
            this.LabFechaLectura.AutoSize = true;
            this.LabFechaLectura.BackColor = System.Drawing.Color.Transparent;
            this.LabFechaLectura.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.LabFechaLectura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.LabFechaLectura.Location = new System.Drawing.Point(55, 120);
            this.LabFechaLectura.Name = "LabFechaLectura";
            this.LabFechaLectura.Size = new System.Drawing.Size(94, 13);
            this.LabFechaLectura.TabIndex = 19;
            this.LabFechaLectura.Text = "Fecha de lectura:";
            // 
            // iTalk_Label1
            // 
            this.iTalk_Label1.AutoSize = true;
            this.iTalk_Label1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTalk_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_Label1.Location = new System.Drawing.Point(1, 26);
            this.iTalk_Label1.Name = "iTalk_Label1";
            this.iTalk_Label1.Size = new System.Drawing.Size(353, 78);
            this.iTalk_Label1.TabIndex = 18;
            this.iTalk_Label1.Text = resources.GetString("iTalk_Label1.Text");
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Cantidad de Usuarios: ";
            // 
            // LbPorcentaje
            // 
            this.LbPorcentaje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LbPorcentaje.AutoSize = true;
            this.LbPorcentaje.Location = new System.Drawing.Point(232, 195);
            this.LbPorcentaje.Name = "LbPorcentaje";
            this.LbPorcentaje.Size = new System.Drawing.Size(35, 13);
            this.LbPorcentaje.TabIndex = 15;
            this.LbPorcentaje.Text = "label1";
            this.LbPorcentaje.Visible = false;
            // 
            // progressBarExpor
            // 
            this.progressBarExpor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarExpor.Location = new System.Drawing.Point(44, 191);
            this.progressBarExpor.Name = "progressBarExpor";
            this.progressBarExpor.Size = new System.Drawing.Size(179, 22);
            this.progressBarExpor.Step = 1;
            this.progressBarExpor.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarExpor.TabIndex = 14;
            this.progressBarExpor.Visible = false;
            // 
            // BtnIniciarExpor
            // 
            this.BtnIniciarExpor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnIniciarExpor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnIniciarExpor.Location = new System.Drawing.Point(33, 124);
            this.BtnIniciarExpor.Name = "BtnIniciarExpor";
            this.BtnIniciarExpor.Size = new System.Drawing.Size(143, 35);
            this.BtnIniciarExpor.TabIndex = 6;
            this.BtnIniciarExpor.Text = "Iniciar Exportación";
            this.BtnIniciarExpor.UseVisualStyleBackColor = true;
            this.BtnIniciarExpor.Click += new System.EventHandler(this.BtnIniciarExpor_Click);
            // 
            // TareaSegundoPlano1
            // 
            this.TareaSegundoPlano1.WorkerReportsProgress = true;
            this.TareaSegundoPlano1.WorkerSupportsCancellation = true;
            this.TareaSegundoPlano1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TareaSegundoPlano1_DoWork_1);
            this.TareaSegundoPlano1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TareaSegundoPlano1_ProgressChanged);
            this.TareaSegundoPlano1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TareaSegundoPlano1_RunWorkerCompleted);
            // 
            // imgList1
            // 
            this.imgList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imgList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(239, 120);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1131, 462);
            this.splitContainer2.SplitterDistance = 466;
            this.splitContainer2.TabIndex = 4;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer3.Size = new System.Drawing.Size(661, 462);
            this.splitContainer3.SplitterDistance = 249;
            this.splitContainer3.TabIndex = 31;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CBImposiblesApagados);
            this.groupBox3.Controls.Add(this.CBLeidosNOPrint);
            this.groupBox3.Controls.Add(this.LabelAltas);
            this.groupBox3.Controls.Add(this.button11);
            this.groupBox3.Controls.Add(this.LabelErrores);
            this.groupBox3.Controls.Add(this.button10);
            this.groupBox3.Controls.Add(this.LabelTodos);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.button9);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.LabIndNoPrint);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.LabSaldos);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.LabNoImprFueraRango);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.LabLeidNoImprePre);
            this.groupBox3.Controls.Add(this.LabImprPre);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Sitka Small", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(661, 249);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Resumen de exportación";
            // 
            // CBImposiblesApagados
            // 
            this.CBImposiblesApagados.BackColor = System.Drawing.SystemColors.Menu;
            this.CBImposiblesApagados.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBImposiblesApagados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.CBImposiblesApagados.FormattingEnabled = true;
            this.CBImposiblesApagados.Items.AddRange(new object[] {
            "Lecuturas Imposibles",
            "Apagados"});
            this.CBImposiblesApagados.Location = new System.Drawing.Point(272, 158);
            this.CBImposiblesApagados.Name = "CBImposiblesApagados";
            this.CBImposiblesApagados.Size = new System.Drawing.Size(167, 27);
            this.CBImposiblesApagados.TabIndex = 41;
            this.CBImposiblesApagados.Text = "Lecturas Imposibles";
            // 
            // CBLeidosNOPrint
            // 
            this.CBLeidosNOPrint.BackColor = System.Drawing.SystemColors.Menu;
            this.CBLeidosNOPrint.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBLeidosNOPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.CBLeidosNOPrint.FormattingEnabled = true;
            this.CBLeidosNOPrint.Items.AddRange(new object[] {
            "Leidos NO impresos",
            "Facturados NO impresos"});
            this.CBLeidosNOPrint.Location = new System.Drawing.Point(57, 186);
            this.CBLeidosNOPrint.Name = "CBLeidosNOPrint";
            this.CBLeidosNOPrint.Size = new System.Drawing.Size(167, 27);
            this.CBLeidosNOPrint.TabIndex = 40;
            this.CBLeidosNOPrint.Text = "Leidos NO impresos";
            // 
            // LabelAltas
            // 
            this.LabelAltas.BackColor = System.Drawing.SystemColors.Control;
            this.LabelAltas.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabelAltas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelAltas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabelAltas.Location = new System.Drawing.Point(485, 219);
            this.LabelAltas.Name = "LabelAltas";
            this.LabelAltas.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelAltas.Size = new System.Drawing.Size(51, 23);
            this.LabelAltas.TabIndex = 38;
            this.LabelAltas.Text = "Otros";
            this.LabelAltas.Click += new System.EventHandler(this.LabelAltas_Click);
            // 
            // button11
            // 
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Image = ((System.Drawing.Image)(resources.GetObject("button11.Image")));
            this.button11.Location = new System.Drawing.Point(461, 219);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(24, 23);
            this.button11.TabIndex = 39;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // LabelErrores
            // 
            this.LabelErrores.BackColor = System.Drawing.SystemColors.Control;
            this.LabelErrores.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabelErrores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelErrores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabelErrores.Location = new System.Drawing.Point(486, 190);
            this.LabelErrores.Name = "LabelErrores";
            this.LabelErrores.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelErrores.Size = new System.Drawing.Size(51, 23);
            this.LabelErrores.TabIndex = 36;
            this.LabelErrores.Text = "Errores";
            // 
            // button10
            // 
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Image = ((System.Drawing.Image)(resources.GetObject("button10.Image")));
            this.button10.Location = new System.Drawing.Point(462, 190);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(24, 23);
            this.button10.TabIndex = 37;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // LabelTodos
            // 
            this.LabelTodos.BackColor = System.Drawing.SystemColors.Control;
            this.LabelTodos.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabelTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTodos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabelTodos.Location = new System.Drawing.Point(488, 159);
            this.LabelTodos.Name = "LabelTodos";
            this.LabelTodos.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelTodos.Size = new System.Drawing.Size(41, 23);
            this.LabelTodos.TabIndex = 31;
            this.LabelTodos.Text = "Todos";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.CBFiltroZona);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.CBRemesaSola);
            this.groupBox5.Controls.Add(this.RBRemesaSola);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.RBRuta);
            this.groupBox5.Controls.Add(this.RBRemesasTodas);
            this.groupBox5.Controls.Add(this.TextBoxHasta);
            this.groupBox5.Controls.Add(this.TextBoxDesde);
            this.groupBox5.Controls.Add(this.LabelHasta);
            this.groupBox5.Controls.Add(this.LabelDesde);
            this.groupBox5.Controls.Add(this.TextBoxRuta);
            this.groupBox5.Controls.Add(this.LabelRuta);
            this.groupBox5.Controls.Add(this.LabelRemesaRuta);
            this.groupBox5.Controls.Add(this.CBRemesaRuta);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(3, 20);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(655, 131);
            this.groupBox5.TabIndex = 35;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Filtros";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(9, 72);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(87, 23);
            this.label8.TabIndex = 42;
            this.label8.Text = "Individual";
            // 
            // CBFiltroZona
            // 
            this.CBFiltroZona.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBFiltroZona.FormattingEnabled = true;
            this.CBFiltroZona.Location = new System.Drawing.Point(54, 35);
            this.CBFiltroZona.Name = "CBFiltroZona";
            this.CBFiltroZona.Size = new System.Drawing.Size(73, 30);
            this.CBFiltroZona.TabIndex = 49;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 19);
            this.label7.TabIndex = 48;
            this.label7.Text = "Zona:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(257, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(379, 22);
            this.label6.TabIndex = 47;
            this.label6.Text = "Si tilda el check \"Remesa\", estará filtrando todas las rutas de la  Zona (Localid" +
    "ad) \r\ny Remesa seleccionada.";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // CBRemesaSola
            // 
            this.CBRemesaSola.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBRemesaSola.FormattingEnabled = true;
            this.CBRemesaSola.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.CBRemesaSola.Location = new System.Drawing.Point(215, 33);
            this.CBRemesaSola.Name = "CBRemesaSola";
            this.CBRemesaSola.Size = new System.Drawing.Size(37, 30);
            this.CBRemesaSola.TabIndex = 46;
            this.CBRemesaSola.Text = "1";
            this.CBRemesaSola.Visible = false;
            // 
            // RBRemesaSola
            // 
            this.RBRemesaSola.AutoSize = true;
            this.RBRemesaSola.Location = new System.Drawing.Point(134, 37);
            this.RBRemesaSola.Name = "RBRemesaSola";
            this.RBRemesaSola.Size = new System.Drawing.Size(81, 23);
            this.RBRemesaSola.TabIndex = 45;
            this.RBRemesaSola.TabStop = true;
            this.RBRemesaSola.Text = "Remesa";
            this.RBRemesaSola.UseVisualStyleBackColor = true;
            this.RBRemesaSola.CheckedChanged += new System.EventHandler(this.RBRemesaSola_CheckedChanged);
            this.RBRemesaSola.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RBRemesaSola_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(257, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(373, 22);
            this.label10.TabIndex = 44;
            this.label10.Text = "Si tilda el check \"Todo\", estará filtrando todas las rutas de todas las remesas d" +
    "el \r\nperiodo vigente que se encuentra seleccionado desde la pantalla de inicio.";
            this.label10.Visible = false;
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // RBRuta
            // 
            this.RBRuta.AutoSize = true;
            this.RBRuta.Location = new System.Drawing.Point(6, 94);
            this.RBRuta.Name = "RBRuta";
            this.RBRuta.Size = new System.Drawing.Size(60, 23);
            this.RBRuta.TabIndex = 43;
            this.RBRuta.TabStop = true;
            this.RBRuta.Text = "Ruta";
            this.RBRuta.UseVisualStyleBackColor = true;
            this.RBRuta.CheckedChanged += new System.EventHandler(this.RBRemesasIndiv_CheckedChanged_2);
            this.RBRuta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RBRemesasIndiv_KeyDown);
            // 
            // RBRemesasTodas
            // 
            this.RBRemesasTodas.AutoSize = true;
            this.RBRemesasTodas.Location = new System.Drawing.Point(135, 5);
            this.RBRemesasTodas.Name = "RBRemesasTodas";
            this.RBRemesasTodas.Size = new System.Drawing.Size(60, 23);
            this.RBRemesasTodas.TabIndex = 42;
            this.RBRemesasTodas.TabStop = true;
            this.RBRemesasTodas.Text = "Todo";
            this.RBRemesasTodas.UseVisualStyleBackColor = true;
            this.RBRemesasTodas.Visible = false;
            this.RBRemesasTodas.CheckedChanged += new System.EventHandler(this.RBRemesasTodas_CheckedChanged_1);
            this.RBRemesasTodas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RBRemesasTodas_KeyDown);
            // 
            // TextBoxHasta
            // 
            this.TextBoxHasta.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxHasta.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxHasta.Location = new System.Drawing.Point(531, 95);
            this.TextBoxHasta.Name = "TextBoxHasta";
            this.TextBoxHasta.Size = new System.Drawing.Size(82, 23);
            this.TextBoxHasta.TabIndex = 40;
            this.TextBoxHasta.Text = "dd/MM/yyyy";
            this.TextBoxHasta.Visible = false;
            this.TextBoxHasta.Enter += new System.EventHandler(this.TextBoxHasta_Enter);
            this.TextBoxHasta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxHasta_KeyPress);
            this.TextBoxHasta.Leave += new System.EventHandler(this.TextBoxHasta_Leave);
            // 
            // TextBoxDesde
            // 
            this.TextBoxDesde.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxDesde.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxDesde.Location = new System.Drawing.Point(385, 95);
            this.TextBoxDesde.Name = "TextBoxDesde";
            this.TextBoxDesde.Size = new System.Drawing.Size(82, 23);
            this.TextBoxDesde.TabIndex = 39;
            this.TextBoxDesde.Text = "dd/mm/yyyy";
            this.TextBoxDesde.Visible = false;
            this.TextBoxDesde.Click += new System.EventHandler(this.TextBoxDesde_Click);
            this.TextBoxDesde.Enter += new System.EventHandler(this.textBox3_Enter);
            this.TextBoxDesde.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox3_KeyPress);
            this.TextBoxDesde.Leave += new System.EventHandler(this.TextBoxDesde_Leave);
            // 
            // LabelHasta
            // 
            this.LabelHasta.AutoSize = true;
            this.LabelHasta.Location = new System.Drawing.Point(471, 96);
            this.LabelHasta.Name = "LabelHasta";
            this.LabelHasta.Size = new System.Drawing.Size(54, 19);
            this.LabelHasta.TabIndex = 38;
            this.LabelHasta.Text = "Hasta:";
            this.LabelHasta.Visible = false;
            // 
            // LabelDesde
            // 
            this.LabelDesde.AutoSize = true;
            this.LabelDesde.Location = new System.Drawing.Point(329, 96);
            this.LabelDesde.Name = "LabelDesde";
            this.LabelDesde.Size = new System.Drawing.Size(56, 19);
            this.LabelDesde.TabIndex = 37;
            this.LabelDesde.Text = "Desde:";
            this.LabelDesde.Visible = false;
            // 
            // TextBoxRuta
            // 
            this.TextBoxRuta.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxRuta.Location = new System.Drawing.Point(248, 91);
            this.TextBoxRuta.Name = "TextBoxRuta";
            this.TextBoxRuta.Size = new System.Drawing.Size(70, 30);
            this.TextBoxRuta.TabIndex = 36;
            this.TextBoxRuta.Visible = false;
            // 
            // LabelRuta
            // 
            this.LabelRuta.AutoSize = true;
            this.LabelRuta.Location = new System.Drawing.Point(198, 96);
            this.LabelRuta.Name = "LabelRuta";
            this.LabelRuta.Size = new System.Drawing.Size(47, 19);
            this.LabelRuta.TabIndex = 35;
            this.LabelRuta.Text = "Ruta:";
            this.LabelRuta.Visible = false;
            // 
            // LabelRemesaRuta
            // 
            this.LabelRemesaRuta.AutoSize = true;
            this.LabelRemesaRuta.Location = new System.Drawing.Point(85, 96);
            this.LabelRemesaRuta.Name = "LabelRemesaRuta";
            this.LabelRemesaRuta.Size = new System.Drawing.Size(68, 19);
            this.LabelRemesaRuta.TabIndex = 34;
            this.LabelRemesaRuta.Text = "Remesa:";
            this.LabelRemesaRuta.Visible = false;
            // 
            // CBRemesaRuta
            // 
            this.CBRemesaRuta.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBRemesaRuta.FormattingEnabled = true;
            this.CBRemesaRuta.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.CBRemesaRuta.Location = new System.Drawing.Point(157, 91);
            this.CBRemesaRuta.Name = "CBRemesaRuta";
            this.CBRemesaRuta.Size = new System.Drawing.Size(37, 30);
            this.CBRemesaRuta.TabIndex = 33;
            this.CBRemesaRuta.Text = "1";
            this.CBRemesaRuta.Visible = false;
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Image = ((System.Drawing.Image)(resources.GetObject("button9.Image")));
            this.button9.Location = new System.Drawing.Point(462, 159);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(24, 23);
            this.button9.TabIndex = 32;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Image = ((System.Drawing.Image)(resources.GetObject("button8.Image")));
            this.button8.Location = new System.Drawing.Point(247, 190);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(24, 23);
            this.button8.TabIndex = 30;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // LabIndNoPrint
            // 
            this.LabIndNoPrint.BackColor = System.Drawing.SystemColors.Control;
            this.LabIndNoPrint.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabIndNoPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabIndNoPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabIndNoPrint.Location = new System.Drawing.Point(270, 190);
            this.LabIndNoPrint.Name = "LabIndNoPrint";
            this.LabIndNoPrint.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabIndNoPrint.Size = new System.Drawing.Size(159, 23);
            this.LabIndNoPrint.TabIndex = 29;
            this.LabIndNoPrint.Text = "Indicados para NO imprimir";
            this.LabIndNoPrint.Click += new System.EventHandler(this.LabIndNoPrint_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Image = ((System.Drawing.Image)(resources.GetObject("button6.Image")));
            this.button6.Location = new System.Drawing.Point(247, 159);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 23);
            this.button6.TabIndex = 28;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(269, 159);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(124, 23);
            this.label5.TabIndex = 27;
            this.label5.Text = "Lecturas Imposibles";
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.Location = new System.Drawing.Point(247, 219);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(24, 23);
            this.button5.TabIndex = 26;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // LabSaldos
            // 
            this.LabSaldos.BackColor = System.Drawing.SystemColors.Control;
            this.LabSaldos.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabSaldos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabSaldos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabSaldos.Location = new System.Drawing.Point(272, 219);
            this.LabSaldos.Name = "LabSaldos";
            this.LabSaldos.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabSaldos.Size = new System.Drawing.Size(66, 23);
            this.LabSaldos.TabIndex = 25;
            this.LabSaldos.Text = "Saldos";
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(31, 219);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 24;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // LabNoImprFueraRango
            // 
            this.LabNoImprFueraRango.BackColor = System.Drawing.SystemColors.Control;
            this.LabNoImprFueraRango.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabNoImprFueraRango.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabNoImprFueraRango.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabNoImprFueraRango.Location = new System.Drawing.Point(54, 219);
            this.LabNoImprFueraRango.Name = "LabNoImprFueraRango";
            this.LabNoImprFueraRango.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabNoImprFueraRango.Size = new System.Drawing.Size(213, 23);
            this.LabNoImprFueraRango.TabIndex = 23;
            this.LabNoImprFueraRango.Text = "NO impresos Fuera de Rango";
            this.LabNoImprFueraRango.Click += new System.EventHandler(this.LabNoImprFueraRango_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(31, 190);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 23);
            this.button3.TabIndex = 22;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(31, 159);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 23);
            this.button2.TabIndex = 21;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LabLeidNoImprePre
            // 
            this.LabLeidNoImprePre.BackColor = System.Drawing.SystemColors.Control;
            this.LabLeidNoImprePre.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabLeidNoImprePre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabLeidNoImprePre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabLeidNoImprePre.Location = new System.Drawing.Point(54, 190);
            this.LabLeidNoImprePre.Name = "LabLeidNoImprePre";
            this.LabLeidNoImprePre.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabLeidNoImprePre.Size = new System.Drawing.Size(147, 23);
            this.LabLeidNoImprePre.TabIndex = 20;
            this.LabLeidNoImprePre.Text = "Leidas NO impresas";
            this.LabLeidNoImprePre.Click += new System.EventHandler(this.LabLeidNoImprePre_Click);
            // 
            // LabImprPre
            // 
            this.LabImprPre.BackColor = System.Drawing.SystemColors.Control;
            this.LabImprPre.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabImprPre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabImprPre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabImprPre.Location = new System.Drawing.Point(56, 159);
            this.LabImprPre.Name = "LabImprPre";
            this.LabImprPre.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabImprPre.Size = new System.Drawing.Size(87, 23);
            this.LabImprPre.TabIndex = 19;
            this.LabImprPre.Text = "Impresas";
            this.LabImprPre.Click += new System.EventHandler(this.LabImprPre_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.splitContainer4);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Font = new System.Drawing.Font("Sitka Small", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(661, 209);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rutas exportadas";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 20);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.PickBoxLoading);
            this.splitContainer4.Panel2.Controls.Add(this.tvExportadas);
            this.splitContainer4.Size = new System.Drawing.Size(655, 186);
            this.splitContainer4.SplitterDistance = 31;
            this.splitContainer4.TabIndex = 2;
            // 
            // PickBoxLoading
            // 
            this.PickBoxLoading.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PickBoxLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PickBoxLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PickBoxLoading.Image = global::gagFIS_Interfase.Properties.Resources.gif_loading_circular_rayas;
            this.PickBoxLoading.Location = new System.Drawing.Point(0, 0);
            this.PickBoxLoading.Name = "PickBoxLoading";
            this.PickBoxLoading.Size = new System.Drawing.Size(655, 151);
            this.PickBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PickBoxLoading.TabIndex = 2;
            this.PickBoxLoading.TabStop = false;
            this.PickBoxLoading.Visible = false;
            // 
            // tvExportadas
            // 
            this.tvExportadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvExportadas.Font = new System.Drawing.Font("Sitka Small", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvExportadas.Location = new System.Drawing.Point(0, 0);
            this.tvExportadas.Name = "tvExportadas";
            this.tvExportadas.Size = new System.Drawing.Size(655, 151);
            this.tvExportadas.TabIndex = 1;
            this.tvExportadas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvExportadas_AfterSelect);
            // 
            // informesToolStripMenuItem1
            // 
            this.informesToolStripMenuItem1.Name = "informesToolStripMenuItem1";
            this.informesToolStripMenuItem1.Size = new System.Drawing.Size(66, 35);
            this.informesToolStripMenuItem1.Text = "Informes";
            this.informesToolStripMenuItem1.Click += new System.EventHandler(this.informesToolStripMenuItem1_Click_1);
            // 
            // BGWEsperar
            // 
            this.BGWEsperar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGWEsperar_DoWork);
            // 
            // resumenesToolStripMenuItem
            // 
            this.resumenesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.impresasToolStripMenuItem,
            this.leidasNOImpresasToolStripMenuItem,
            this.noImpresasPorFueraDeRangoToolStripMenuItem,
            this.noImpresosPorOtrosMotivosToolStripMenuItem,
            this.saldosToolStripMenuItem});
            this.resumenesToolStripMenuItem.Name = "resumenesToolStripMenuItem";
            this.resumenesToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.resumenesToolStripMenuItem.Text = "Resumenes";
            this.resumenesToolStripMenuItem.Visible = false;
            // 
            // impresasToolStripMenuItem
            // 
            this.impresasToolStripMenuItem.Name = "impresasToolStripMenuItem";
            this.impresasToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.impresasToolStripMenuItem.Text = "Impresas";
            this.impresasToolStripMenuItem.Click += new System.EventHandler(this.impresasToolStripMenuItem_Click);
            // 
            // leidasNOImpresasToolStripMenuItem
            // 
            this.leidasNOImpresasToolStripMenuItem.Name = "leidasNOImpresasToolStripMenuItem";
            this.leidasNOImpresasToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.leidasNOImpresasToolStripMenuItem.Text = "Leidas NO impresas";
            this.leidasNOImpresasToolStripMenuItem.Click += new System.EventHandler(this.leidasNOImpresasToolStripMenuItem_Click);
            // 
            // noImpresasPorFueraDeRangoToolStripMenuItem
            // 
            this.noImpresasPorFueraDeRangoToolStripMenuItem.Name = "noImpresasPorFueraDeRangoToolStripMenuItem";
            this.noImpresasPorFueraDeRangoToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.noImpresasPorFueraDeRangoToolStripMenuItem.Text = "No impresas por Fuera de Rango";
            this.noImpresasPorFueraDeRangoToolStripMenuItem.Click += new System.EventHandler(this.noImpresasPorFueraDeRangoToolStripMenuItem_Click);
            // 
            // noImpresosPorOtrosMotivosToolStripMenuItem
            // 
            this.noImpresosPorOtrosMotivosToolStripMenuItem.Name = "noImpresosPorOtrosMotivosToolStripMenuItem";
            this.noImpresosPorOtrosMotivosToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.noImpresosPorOtrosMotivosToolStripMenuItem.Text = "No impresos por otros motivos";
            this.noImpresosPorOtrosMotivosToolStripMenuItem.Click += new System.EventHandler(this.noImpresosPorOtrosMotivosToolStripMenuItem_Click);
            // 
            // saldosToolStripMenuItem
            // 
            this.saldosToolStripMenuItem.Name = "saldosToolStripMenuItem";
            this.saldosToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.saldosToolStripMenuItem.Text = "Saldos";
            this.saldosToolStripMenuItem.Click += new System.EventHandler(this.saldosToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resumenesToolStripMenuItem,
            this.informesDeExportacionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1370, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // informesDeExportacionToolStripMenuItem
            // 
            this.informesDeExportacionToolStripMenuItem.Name = "informesDeExportacionToolStripMenuItem";
            this.informesDeExportacionToolStripMenuItem.Size = new System.Drawing.Size(148, 20);
            this.informesDeExportacionToolStripMenuItem.Text = "Informes de Exportacion";
            this.informesDeExportacionToolStripMenuItem.Click += new System.EventHandler(this.informesDeExportacionToolStripMenuItem_Click);
            // 
            // BGWLoading
            // 
            this.BGWLoading.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGWLoading_DoWork);
            this.BGWLoading.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGWLoading_RunWorkerCompleted);
            // 
            // Form2Exportar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 675);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2Exportar";
            this.Text = "Exportar Datos";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Resize += new System.EventHandler(this.Form2_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.GBCambioFecha.ResumeLayout(false);
            this.GBCambioFecha.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PickBoxLoading)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvlotes;
        private System.Windows.Forms.CheckBox cbTodo;
        private System.Windows.Forms.Button BtnIniciarExpor;
        private System.Windows.Forms.ProgressBar progressBarExpor;
        private System.ComponentModel.BackgroundWorker TareaSegundoPlano1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label LbPorcentaje;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView tvConexUpdload;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imgList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip2;
        public System.Windows.Forms.CheckBox cbLote;
        private System.Windows.Forms.RadioButton RadioButtonNO;
        private System.Windows.Forms.RadioButton RadioButtonSI;
        private System.Windows.Forms.Label labelPanExpXRuta;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button5;
        public System.Windows.Forms.Label LabSaldos;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.Label LabNoImprFueraRango;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label LabLeidNoImprePre;
        public System.Windows.Forms.Label LabImprPre;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button6;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        public System.Windows.Forms.Label LabIndNoPrint;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ToolStripMenuItem informesToolStripMenuItem1;
        private System.Windows.Forms.Button button9;
        public System.Windows.Forms.Label LabelTodos;
        private System.Windows.Forms.Label LabelRemesaRuta;
        private System.Windows.Forms.ComboBox CBRemesaRuta;
        private System.Windows.Forms.TextBox textBox1;
        private System.ComponentModel.BackgroundWorker BGWEsperar;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox TextBoxHasta;
        private System.Windows.Forms.TextBox TextBoxDesde;
        private System.Windows.Forms.Label LabelHasta;
        private System.Windows.Forms.Label LabelDesde;
        private System.Windows.Forms.TextBox TextBoxRuta;
        private System.Windows.Forms.Label LabelRuta;
        public System.Windows.Forms.Label LabelErrores;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.CheckBox UpPorRuta;
        private iTalk.iTalk_Label LabFechaLectura;
        private iTalk.iTalk_Label iTalk_Label1;
        private System.Windows.Forms.DateTimePicker TBFechaModificada;
        private System.Windows.Forms.ToolStripMenuItem resumenesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem impresasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leidasNOImpresasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noImpresasPorFueraDeRangoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noImpresosPorOtrosMotivosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saldosToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem informesDeExportacionToolStripMenuItem;
        public System.Windows.Forms.CheckBox CheckCambiarFechaLectura;
        public iTalk.iTalk_GroupBox GBCambioFecha;
        public System.Windows.Forms.Label LabelAltas;
        private System.Windows.Forms.Button button11;
        public  System.Windows.Forms.TreeView tvExportadas;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.PictureBox PickBoxLoading;
        private System.Windows.Forms.ComboBox CBLeidosNOPrint;
        private System.Windows.Forms.ComboBox CBImposiblesApagados;
        private System.Windows.Forms.RadioButton RBRuta;
        private System.Windows.Forms.RadioButton RBRemesasTodas;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton RBRemesaSola;
        private System.Windows.Forms.ComboBox CBRemesaSola;
        private System.ComponentModel.BackgroundWorker BGWLoading;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CBFiltroZona;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label8;
    }
}
