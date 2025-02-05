/*
 * Creado por SharpDevelop.
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 14:02
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;

namespace gagFIS_Interfase
{
    partial class Form4Cargas
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4Cargas));
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Ruta 201-1170");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Remesa 1", new System.Windows.Forms.TreeNode[] {
            treeNode22});
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Nodo19");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Nodo18", new System.Windows.Forms.TreeNode[] {
            treeNode24});
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Nodo20");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Nodo12", new System.Windows.Forms.TreeNode[] {
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Nodo13");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Remesa 2", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28});
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("201-Capital", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode29});
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Ruta 202-1200");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Nodo1-0-1");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Nodo1-0-2");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Remesa 1", new System.Windows.Forms.TreeNode[] {
            treeNode31,
            treeNode32,
            treeNode33});
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Nodo1-1-0");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Ruta 202-1200", new System.Windows.Forms.TreeNode[] {
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Remesa 3");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("202 - Goya", new System.Windows.Forms.TreeNode[] {
            treeNode34,
            treeNode36,
            treeNode37});
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Nodo9");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Nodo2", new System.Windows.Forms.TreeNode[] {
            treeNode39});
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Nodo10");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Nodo3", new System.Windows.Forms.TreeNode[] {
            treeNode41});
            this.panel2 = new System.Windows.Forms.Panel();
            this.BotActPanPC = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Ru = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.shellView2 = new GongSolutions.Shell.ShellView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.BtonNodos = new System.Windows.Forms.Button();
            this.Loc = new System.Windows.Forms.TextBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.BotProcesCarg = new System.Windows.Forms.Button();
            this.tvwCargas = new System.Windows.Forms.TreeView();
            this.imgList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.LabNomColect = new System.Windows.Forms.Label();
            this.cmbDevicesWifi = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RBEnvWifi = new System.Windows.Forms.RadioButton();
            this.RBEnvCable = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.botExpulsaColec = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verEstadosDeRutasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.PickBoxLoading = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelProcesando = new System.Windows.Forms.Label();
            this.BtnCerrarUsuarios = new System.Windows.Forms.Button();
            this.BotDetenerProcCarga = new System.Windows.Forms.Button();
            this.groupBoxProrrateo = new System.Windows.Forms.GroupBox();
            this.InfoTipoPro = new System.Windows.Forms.Label();
            this.LabelInfoPro = new System.Windows.Forms.Label();
            this.radioButProBasYfec = new System.Windows.Forms.RadioButton();
            this.radioButProrLim = new System.Windows.Forms.RadioButton();
            this.radioButSinPro = new System.Windows.Forms.RadioButton();
            this.labelCantReg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PorcLabel = new System.Windows.Forms.Label();
            this.RestNod = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.BotDevCarga = new System.Windows.Forms.Button();
            this.listViewCargasProcesadas = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.ListViewCargados = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BotEnviarCarga = new System.Windows.Forms.Button();
            this.LabRestEnvArc = new System.Windows.Forms.Label();
            this.labelEnviando = new System.Windows.Forms.Label();
            this.LabRestDevArc = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.ListViewColectora = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.LisViewDescargados = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.shellNotificationListener1 = new GongSolutions.Shell.ShellNotificationListener(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.backgroundProcesarCarga = new System.ComponentModel.BackgroundWorker();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.backgroundEnviarArchivo = new System.ComponentModel.BackgroundWorker();
            this.toolTip5 = new System.Windows.Forms.ToolTip(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.shellViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.shellItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.windowsPortableDeviceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PickBoxLoading)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBoxProrrateo.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shellViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shellItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsPortableDeviceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.BotActPanPC);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.Ru);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.shellView2);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.BtonNodos);
            this.panel2.Controls.Add(this.Loc);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 498);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1284, 89);
            this.panel2.TabIndex = 3;
            this.toolTip1.SetToolTip(this.panel2, "Este es un mensaje ");
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            // 
            // BotActPanPC
            // 
            this.BotActPanPC.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BotActPanPC.Location = new System.Drawing.Point(554, 10);
            this.BotActPanPC.Name = "BotActPanPC";
            this.BotActPanPC.Size = new System.Drawing.Size(77, 28);
            this.BotActPanPC.TabIndex = 44;
            this.BotActPanPC.Text = "Actualizar";
            this.BotActPanPC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BotActPanPC.UseVisualStyleBackColor = true;
            this.BotActPanPC.Visible = false;
            this.BotActPanPC.Click += new System.EventHandler(this.BotActPanPC_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button3);
            this.panel5.Location = new System.Drawing.Point(3, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(485, 93);
            this.panel5.TabIndex = 45;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(234, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(71, 25);
            this.button3.TabIndex = 13;
            this.button3.TabStop = false;
            this.button3.Text = "Boton Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(258, 3);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(79, 20);
            this.textBox3.TabIndex = 4;
            this.textBox3.Visible = false;
            // 
            // Ru
            // 
            this.Ru.Location = new System.Drawing.Point(80, 3);
            this.Ru.Name = "Ru";
            this.Ru.Size = new System.Drawing.Size(60, 20);
            this.Ru.TabIndex = 11;
            this.Ru.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(343, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(92, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.Visible = false;
            // 
            // shellView2
            // 
            this.shellView2.Enabled = false;
            this.shellView2.Location = new System.Drawing.Point(313, 37);
            this.shellView2.Name = "shellView2";
            this.shellView2.Size = new System.Drawing.Size(79, 44);
            this.shellView2.StatusBar = null;
            this.shellView2.TabIndex = 47;
            this.shellView2.Text = "shellView2";
            this.shellView2.View = GongSolutions.Shell.ShellViewStyle.Details;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(158, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(79, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Visible = false;
            // 
            // BtonNodos
            // 
            this.BtonNodos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtonNodos.Location = new System.Drawing.Point(-27, 25);
            this.BtonNodos.Name = "BtonNodos";
            this.BtonNodos.Size = new System.Drawing.Size(49, 41);
            this.BtonNodos.TabIndex = 33;
            this.BtonNodos.Text = "Boton Nodos";
            this.BtonNodos.UseVisualStyleBackColor = true;
            this.BtonNodos.Visible = false;
            this.BtonNodos.Click += new System.EventHandler(this.button2_Click);
            // 
            // Loc
            // 
            this.Loc.Location = new System.Drawing.Point(6, 3);
            this.Loc.Name = "Loc";
            this.Loc.Size = new System.Drawing.Size(61, 20);
            this.Loc.TabIndex = 7;
            this.Loc.Visible = false;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(1157, 3);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(87, 48);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // BotProcesCarg
            // 
            this.BotProcesCarg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotProcesCarg.Location = new System.Drawing.Point(5, 16);
            this.BotProcesCarg.Name = "BotProcesCarg";
            this.BotProcesCarg.Size = new System.Drawing.Size(143, 38);
            this.BotProcesCarg.TabIndex = 5;
            this.BotProcesCarg.Text = "Procesar Carga";
            this.BotProcesCarg.UseVisualStyleBackColor = true;
            this.BotProcesCarg.Click += new System.EventHandler(this.button4_Click);
            // 
            // tvwCargas
            // 
            this.tvwCargas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvwCargas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwCargas.HotTracking = true;
            this.tvwCargas.ImageIndex = 0;
            this.tvwCargas.ImageList = this.imgList1;
            this.tvwCargas.Location = new System.Drawing.Point(0, 0);
            this.tvwCargas.Name = "tvwCargas";
            treeNode22.Name = "Nodo11";
            treeNode22.Text = "Ruta 201-1170";
            treeNode23.Name = "Nodo4";
            treeNode23.Text = "Remesa 1";
            treeNode24.Name = "Nodo19";
            treeNode24.Text = "Nodo19";
            treeNode25.Name = "Nodo18";
            treeNode25.Text = "Nodo18";
            treeNode26.Name = "Nodo20";
            treeNode26.Text = "Nodo20";
            treeNode27.Name = "Nodo12";
            treeNode27.Text = "Nodo12";
            treeNode28.Name = "Nodo13";
            treeNode28.Text = "Nodo13";
            treeNode29.Name = "Nodo5";
            treeNode29.Text = "Remesa 2";
            treeNode30.Name = "Nodo0";
            treeNode30.Text = "201-Capital";
            treeNode31.Name = "Nodo14";
            treeNode31.Text = "Ruta 202-1200";
            treeNode32.Name = "Nodo15";
            treeNode32.Text = "Nodo1-0-1";
            treeNode33.Name = "Nodo16";
            treeNode33.Text = "Nodo1-0-2";
            treeNode34.Name = "Nodo6";
            treeNode34.Text = "Remesa 1";
            treeNode35.Name = "Nodo17";
            treeNode35.Text = "Nodo1-1-0";
            treeNode36.Name = "Nodo7";
            treeNode36.Text = "Ruta 202-1200";
            treeNode37.Name = "Nodo8";
            treeNode37.Text = "Remesa 3";
            treeNode38.Name = "Nodo1";
            treeNode38.Text = "202 - Goya";
            treeNode39.Name = "Nodo9";
            treeNode39.Text = "Nodo9";
            treeNode40.Name = "Nodo2";
            treeNode40.Text = "Nodo2";
            treeNode41.Name = "Nodo10";
            treeNode41.Text = "Nodo10";
            treeNode42.Name = "Nodo3";
            treeNode42.Text = "Nodo3";
            this.tvwCargas.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode38,
            treeNode40,
            treeNode42});
            this.tvwCargas.SelectedImageIndex = 0;
            this.tvwCargas.ShowNodeToolTips = true;
            this.tvwCargas.Size = new System.Drawing.Size(480, 295);
            this.tvwCargas.TabIndex = 0;
            this.tvwCargas.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCargas_BeforeCollapse);
            this.tvwCargas.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCargas_BeforeExpand);
            this.tvwCargas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCargas_AfterSelect);
            this.tvwCargas.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwCargas_NodeClick);
            this.tvwCargas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // imgList1
            // 
            this.imgList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imgList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.LabNomColect);
            this.panel1.Controls.Add(this.cmbDevicesWifi);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbDevices);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.botExpulsaColec);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 63);
            this.panel1.TabIndex = 2;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button1
            // 
            this.button1.Image = global::gagFIS_Interfase.Properties.Resources.bin_closed;
            this.button1.Location = new System.Drawing.Point(1232, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 28);
            this.button1.TabIndex = 52;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // LabNomColect
            // 
            this.LabNomColect.AutoSize = true;
            this.LabNomColect.Location = new System.Drawing.Point(623, 32);
            this.LabNomColect.Name = "LabNomColect";
            this.LabNomColect.Size = new System.Drawing.Size(54, 13);
            this.LabNomColect.TabIndex = 51;
            this.LabNomColect.Text = "MICx-xxxx";
            this.LabNomColect.Visible = false;
            // 
            // cmbDevicesWifi
            // 
            this.cmbDevicesWifi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevicesWifi.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDevicesWifi.FormattingEnabled = true;
            this.cmbDevicesWifi.Location = new System.Drawing.Point(1058, 31);
            this.cmbDevicesWifi.Name = "cmbDevicesWifi";
            this.cmbDevicesWifi.Size = new System.Drawing.Size(133, 26);
            this.cmbDevicesWifi.TabIndex = 49;
            this.cmbDevicesWifi.Visible = false;
            this.cmbDevicesWifi.SelectionChangeCommitted += new System.EventHandler(this.cmbDevicesWifi_SelectionChangeCommitted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RBEnvWifi);
            this.groupBox1.Controls.Add(this.RBEnvCable);
            this.groupBox1.Location = new System.Drawing.Point(842, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 38);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enviar por:";
            // 
            // RBEnvWifi
            // 
            this.RBEnvWifi.AutoSize = true;
            this.RBEnvWifi.Location = new System.Drawing.Point(8, 15);
            this.RBEnvWifi.Name = "RBEnvWifi";
            this.RBEnvWifi.Size = new System.Drawing.Size(79, 17);
            this.RBEnvWifi.TabIndex = 46;
            this.RBEnvWifi.TabStop = true;
            this.RBEnvWifi.Text = "Inalámbrica";
            this.RBEnvWifi.UseVisualStyleBackColor = true;
            this.RBEnvWifi.CheckedChanged += new System.EventHandler(this.RBEnvWifi_CheckedChanged);
            // 
            // RBEnvCable
            // 
            this.RBEnvCable.AutoSize = true;
            this.RBEnvCable.Location = new System.Drawing.Point(96, 15);
            this.RBEnvCable.Name = "RBEnvCable";
            this.RBEnvCable.Size = new System.Drawing.Size(52, 17);
            this.RBEnvCable.TabIndex = 47;
            this.RBEnvCable.TabStop = true;
            this.RBEnvCable.Text = "Cable";
            this.RBEnvCable.UseVisualStyleBackColor = true;
            this.RBEnvCable.CheckedChanged += new System.EventHandler(this.RBEnvCable_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rutas Disponibles";
            // 
            // cmbDevices
            // 
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.FormattingEnabled = true;
            this.cmbDevices.Location = new System.Drawing.Point(1066, 32);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(126, 21);
            this.cmbDevices.TabIndex = 45;
            this.cmbDevices.Visible = false;
            this.cmbDevices.SelectedIndexChanged += new System.EventHandler(this.cmbDevices_SelectedIndexChanged_1);
            this.cmbDevices.SelectionChangeCommitted += new System.EventHandler(this.cmbDevices_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(994, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Dispositivos:";
            // 
            // botExpulsaColec
            // 
            this.botExpulsaColec.Image = global::gagFIS_Interfase.Properties.Resources.desconectar_usb;
            this.botExpulsaColec.Location = new System.Drawing.Point(1197, 28);
            this.botExpulsaColec.Name = "botExpulsaColec";
            this.botExpulsaColec.Size = new System.Drawing.Size(29, 28);
            this.botExpulsaColec.TabIndex = 43;
            this.botExpulsaColec.UseVisualStyleBackColor = true;
            this.botExpulsaColec.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1284, 24);
            this.menuStrip1.TabIndex = 50;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verEstadosDeRutasToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // verEstadosDeRutasToolStripMenuItem
            // 
            this.verEstadosDeRutasToolStripMenuItem.Name = "verEstadosDeRutasToolStripMenuItem";
            this.verEstadosDeRutasToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.verEstadosDeRutasToolStripMenuItem.Text = "Ver estados de Rutas";
            this.verEstadosDeRutasToolStripMenuItem.Click += new System.EventHandler(this.verEstadosDeRutasToolStripMenuItem_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(100, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 19);
            this.label4.TabIndex = 46;
            this.label4.Text = "Rutas para Cargar";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 63);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 435);
            this.splitContainer1.SplitterDistance = 482;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.PickBoxLoading);
            this.splitContainer2.Panel1.Controls.Add(this.tvwCargas);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel3);
            this.splitContainer2.Size = new System.Drawing.Size(480, 433);
            this.splitContainer2.SplitterDistance = 295;
            this.splitContainer2.TabIndex = 0;
            // 
            // PickBoxLoading
            // 
            this.PickBoxLoading.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PickBoxLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PickBoxLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PickBoxLoading.Image = global::gagFIS_Interfase.Properties.Resources.gif_loading_circular_rayas;
            this.PickBoxLoading.Location = new System.Drawing.Point(0, 0);
            this.PickBoxLoading.Name = "PickBoxLoading";
            this.PickBoxLoading.Size = new System.Drawing.Size(480, 295);
            this.PickBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PickBoxLoading.TabIndex = 3;
            this.PickBoxLoading.TabStop = false;
            this.PickBoxLoading.Visible = false;
            this.PickBoxLoading.Click += new System.EventHandler(this.PickBoxLoading_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelProcesando);
            this.panel3.Controls.Add(this.BtnCerrarUsuarios);
            this.panel3.Controls.Add(this.BotDetenerProcCarga);
            this.panel3.Controls.Add(this.groupBoxProrrateo);
            this.panel3.Controls.Add(this.labelCantReg);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.PorcLabel);
            this.panel3.Controls.Add(this.RestNod);
            this.panel3.Controls.Add(this.BotProcesCarg);
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(480, 134);
            this.panel3.TabIndex = 1;
            // 
            // labelProcesando
            // 
            this.labelProcesando.AutoSize = true;
            this.labelProcesando.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcesando.ForeColor = System.Drawing.Color.Black;
            this.labelProcesando.Location = new System.Drawing.Point(225, 9);
            this.labelProcesando.Name = "labelProcesando";
            this.labelProcesando.Size = new System.Drawing.Size(80, 18);
            this.labelProcesando.TabIndex = 55;
            this.labelProcesando.Text = "Procesando.";
            this.labelProcesando.Visible = false;
            // 
            // BtnCerrarUsuarios
            // 
            this.BtnCerrarUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCerrarUsuarios.Location = new System.Drawing.Point(5, 65);
            this.BtnCerrarUsuarios.Name = "BtnCerrarUsuarios";
            this.BtnCerrarUsuarios.Size = new System.Drawing.Size(143, 35);
            this.BtnCerrarUsuarios.TabIndex = 42;
            this.BtnCerrarUsuarios.Text = "Cerrar Saldos";
            this.BtnCerrarUsuarios.UseVisualStyleBackColor = true;
            this.BtnCerrarUsuarios.Click += new System.EventHandler(this.BtnCerrarUsuarios_Click);
            // 
            // BotDetenerProcCarga
            // 
            this.BotDetenerProcCarga.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotDetenerProcCarga.Location = new System.Drawing.Point(426, 35);
            this.BotDetenerProcCarga.Name = "BotDetenerProcCarga";
            this.BotDetenerProcCarga.Size = new System.Drawing.Size(61, 32);
            this.BotDetenerProcCarga.TabIndex = 41;
            this.BotDetenerProcCarga.Text = "Detener";
            this.BotDetenerProcCarga.UseVisualStyleBackColor = true;
            this.BotDetenerProcCarga.Visible = false;
            this.BotDetenerProcCarga.Click += new System.EventHandler(this.BotDetenerProcCarga_Click);
            // 
            // groupBoxProrrateo
            // 
            this.groupBoxProrrateo.Controls.Add(this.InfoTipoPro);
            this.groupBoxProrrateo.Controls.Add(this.LabelInfoPro);
            this.groupBoxProrrateo.Controls.Add(this.radioButProBasYfec);
            this.groupBoxProrrateo.Controls.Add(this.radioButProrLim);
            this.groupBoxProrrateo.Controls.Add(this.radioButSinPro);
            this.groupBoxProrrateo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxProrrateo.Location = new System.Drawing.Point(0, 109);
            this.groupBoxProrrateo.Name = "groupBoxProrrateo";
            this.groupBoxProrrateo.Size = new System.Drawing.Size(480, 25);
            this.groupBoxProrrateo.TabIndex = 40;
            this.groupBoxProrrateo.TabStop = false;
            this.groupBoxProrrateo.Text = "Prorrateo";
            this.groupBoxProrrateo.Visible = false;
            // 
            // InfoTipoPro
            // 
            this.InfoTipoPro.AutoSize = true;
            this.InfoTipoPro.Location = new System.Drawing.Point(40, 98);
            this.InfoTipoPro.Name = "InfoTipoPro";
            this.InfoTipoPro.Size = new System.Drawing.Size(35, 13);
            this.InfoTipoPro.TabIndex = 0;
            this.InfoTipoPro.Text = "label6";
            this.InfoTipoPro.Visible = false;
            // 
            // LabelInfoPro
            // 
            this.LabelInfoPro.AutoSize = true;
            this.LabelInfoPro.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfoPro.Location = new System.Drawing.Point(13, 16);
            this.LabelInfoPro.Name = "LabelInfoPro";
            this.LabelInfoPro.Size = new System.Drawing.Size(458, 42);
            this.LabelInfoPro.TabIndex = 3;
            this.LabelInfoPro.Text = resources.GetString("LabelInfoPro.Text");
            // 
            // radioButProBasYfec
            // 
            this.radioButProBasYfec.AutoSize = true;
            this.radioButProBasYfec.Location = new System.Drawing.Point(312, 72);
            this.radioButProBasYfec.Name = "radioButProBasYfec";
            this.radioButProBasYfec.Size = new System.Drawing.Size(131, 17);
            this.radioButProBasYfec.TabIndex = 2;
            this.radioButProBasYfec.TabStop = true;
            this.radioButProBasYfec.Text = "Sobre Bases y Fechas";
            this.radioButProBasYfec.UseVisualStyleBackColor = true;
            this.radioButProBasYfec.CheckedChanged += new System.EventHandler(this.radioButProBasYfec_CheckedChanged);
            // 
            // radioButProrLim
            // 
            this.radioButProrLim.AutoSize = true;
            this.radioButProrLim.Location = new System.Drawing.Point(177, 72);
            this.radioButProrLim.Name = "radioButProrLim";
            this.radioButProrLim.Size = new System.Drawing.Size(88, 17);
            this.radioButProrLim.TabIndex = 1;
            this.radioButProrLim.TabStop = true;
            this.radioButProrLim.Text = "Sobre Limites";
            this.radioButProrLim.UseVisualStyleBackColor = true;
            this.radioButProrLim.CheckedChanged += new System.EventHandler(this.radioButProrLim_CheckedChanged);
            // 
            // radioButSinPro
            // 
            this.radioButSinPro.AutoSize = true;
            this.radioButSinPro.Location = new System.Drawing.Point(33, 72);
            this.radioButSinPro.Name = "radioButSinPro";
            this.radioButSinPro.Size = new System.Drawing.Size(86, 17);
            this.radioButSinPro.TabIndex = 0;
            this.radioButSinPro.TabStop = true;
            this.radioButSinPro.Text = "Sin Prorrateo";
            this.radioButSinPro.UseVisualStyleBackColor = true;
            this.radioButSinPro.CheckedChanged += new System.EventHandler(this.radioButSinPro_CheckedChanged);
            // 
            // labelCantReg
            // 
            this.labelCantReg.AutoSize = true;
            this.labelCantReg.Location = new System.Drawing.Point(311, 66);
            this.labelCantReg.Name = "labelCantReg";
            this.labelCantReg.Size = new System.Drawing.Size(13, 13);
            this.labelCantReg.TabIndex = 39;
            this.labelCantReg.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Nº de Conexiones:";
            // 
            // PorcLabel
            // 
            this.PorcLabel.AutoSize = true;
            this.PorcLabel.BackColor = System.Drawing.Color.Transparent;
            this.PorcLabel.ForeColor = System.Drawing.Color.Black;
            this.PorcLabel.Location = new System.Drawing.Point(374, 41);
            this.PorcLabel.Name = "PorcLabel";
            this.PorcLabel.Size = new System.Drawing.Size(35, 13);
            this.PorcLabel.TabIndex = 37;
            this.PorcLabel.Text = "label8";
            this.PorcLabel.Visible = false;
            // 
            // RestNod
            // 
            this.RestNod.AutoSize = true;
            this.RestNod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestNod.ForeColor = System.Drawing.Color.Red;
            this.RestNod.Location = new System.Drawing.Point(19, 103);
            this.RestNod.Name = "RestNod";
            this.RestNod.Size = new System.Drawing.Size(41, 15);
            this.RestNod.TabIndex = 34;
            this.RestNod.Text = "label8";
            this.RestNod.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(161, 37);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(205, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 13;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel4.Controls.Add(this.splitContainer3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(796, 433);
            this.panel4.TabIndex = 29;
            this.panel4.Click += new System.EventHandler(this.panel4_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(796, 433);
            this.splitContainer3.SplitterDistance = 528;
            this.splitContainer3.TabIndex = 55;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.splitContainer7);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.BotEnviarCarga);
            this.splitContainer6.Panel2.Controls.Add(this.LabRestEnvArc);
            this.splitContainer6.Panel2.Controls.Add(this.labelEnviando);
            this.splitContainer6.Panel2.Controls.Add(this.LabRestDevArc);
            this.splitContainer6.Size = new System.Drawing.Size(528, 433);
            this.splitContainer6.SplitterDistance = 362;
            this.splitContainer6.TabIndex = 55;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.splitContainer8);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer9);
            this.splitContainer7.Size = new System.Drawing.Size(362, 433);
            this.splitContainer7.SplitterDistance = 232;
            this.splitContainer7.TabIndex = 54;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.button2);
            this.splitContainer8.Panel1.Controls.Add(this.label4);
            this.splitContainer8.Panel1.Controls.Add(this.BotDevCarga);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.listViewCargasProcesadas);
            this.splitContainer8.Size = new System.Drawing.Size(362, 232);
            this.splitContainer8.SplitterDistance = 48;
            this.splitContainer8.TabIndex = 54;
            // 
            // BotDevCarga
            // 
            this.BotDevCarga.BackColor = System.Drawing.Color.Transparent;
            this.BotDevCarga.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotDevCarga.FlatAppearance.BorderSize = 2;
            this.BotDevCarga.ForeColor = System.Drawing.Color.Transparent;
            this.BotDevCarga.Image = ((System.Drawing.Image)(resources.GetObject("BotDevCarga.Image")));
            this.BotDevCarga.Location = new System.Drawing.Point(12, 2);
            this.BotDevCarga.Name = "BotDevCarga";
            this.BotDevCarga.Size = new System.Drawing.Size(31, 29);
            this.BotDevCarga.TabIndex = 53;
            this.BotDevCarga.UseVisualStyleBackColor = false;
            this.BotDevCarga.Click += new System.EventHandler(this.BotDevCarga_Click);
            // 
            // listViewCargasProcesadas
            // 
            this.listViewCargasProcesadas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewCargasProcesadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCargasProcesadas.HideSelection = false;
            this.listViewCargasProcesadas.Location = new System.Drawing.Point(0, 0);
            this.listViewCargasProcesadas.MultiSelect = false;
            this.listViewCargasProcesadas.Name = "listViewCargasProcesadas";
            this.listViewCargasProcesadas.Size = new System.Drawing.Size(362, 180);
            this.listViewCargasProcesadas.TabIndex = 49;
            this.listViewCargasProcesadas.UseCompatibleStateImageBehavior = false;
            this.listViewCargasProcesadas.View = System.Windows.Forms.View.Details;
            this.listViewCargasProcesadas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "  Periodo   Distrito  NºCarga  Fecha  Hora         Rutas";
            this.columnHeader1.Width = 292;
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(0, 0);
            this.splitContainer9.Name = "splitContainer9";
            this.splitContainer9.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.label5);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.ListViewCargados);
            this.splitContainer9.Size = new System.Drawing.Size(362, 197);
            this.splitContainer9.SplitterDistance = 32;
            this.splitContainer9.TabIndex = 52;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(97, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 19);
            this.label5.TabIndex = 51;
            this.label5.Text = "Rutas Cargadas ";
            // 
            // ListViewCargados
            // 
            this.ListViewCargados.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.ListViewCargados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewCargados.HideSelection = false;
            this.ListViewCargados.Location = new System.Drawing.Point(0, 0);
            this.ListViewCargados.MultiSelect = false;
            this.ListViewCargados.Name = "ListViewCargados";
            this.ListViewCargados.Size = new System.Drawing.Size(362, 161);
            this.ListViewCargados.TabIndex = 50;
            this.ListViewCargados.UseCompatibleStateImageBehavior = false;
            this.ListViewCargados.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "  Periodo   Distrito  NºCarga  Fecha  Hora          Rutas               Colectora" +
    "";
            this.columnHeader2.Width = 366;
            // 
            // BotEnviarCarga
            // 
            this.BotEnviarCarga.Image = ((System.Drawing.Image)(resources.GetObject("BotEnviarCarga.Image")));
            this.BotEnviarCarga.Location = new System.Drawing.Point(35, 146);
            this.BotEnviarCarga.Name = "BotEnviarCarga";
            this.BotEnviarCarga.Size = new System.Drawing.Size(47, 44);
            this.BotEnviarCarga.TabIndex = 42;
            this.BotEnviarCarga.UseVisualStyleBackColor = true;
            this.BotEnviarCarga.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // LabRestEnvArc
            // 
            this.LabRestEnvArc.AutoSize = true;
            this.LabRestEnvArc.ForeColor = System.Drawing.Color.Red;
            this.LabRestEnvArc.Location = new System.Drawing.Point(6, 243);
            this.LabRestEnvArc.Name = "LabRestEnvArc";
            this.LabRestEnvArc.Size = new System.Drawing.Size(22, 13);
            this.LabRestEnvArc.TabIndex = 48;
            this.LabRestEnvArc.Text = ".....";
            this.LabRestEnvArc.Visible = false;
            // 
            // labelEnviando
            // 
            this.labelEnviando.AutoSize = true;
            this.labelEnviando.ForeColor = System.Drawing.Color.Green;
            this.labelEnviando.Location = new System.Drawing.Point(32, 130);
            this.labelEnviando.Name = "labelEnviando";
            this.labelEnviando.Size = new System.Drawing.Size(35, 13);
            this.labelEnviando.TabIndex = 54;
            this.labelEnviando.Text = "label6";
            this.labelEnviando.Visible = false;
            // 
            // LabRestDevArc
            // 
            this.LabRestDevArc.AutoSize = true;
            this.LabRestDevArc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabRestDevArc.ForeColor = System.Drawing.Color.Red;
            this.LabRestDevArc.Location = new System.Drawing.Point(5, 35);
            this.LabRestDevArc.Name = "LabRestDevArc";
            this.LabRestDevArc.Size = new System.Drawing.Size(22, 13);
            this.LabRestDevArc.TabIndex = 49;
            this.LabRestDevArc.Text = ".....";
            this.LabRestDevArc.Visible = false;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.ListViewColectora);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(264, 433);
            this.splitContainer4.SplitterDistance = 232;
            this.splitContainer4.TabIndex = 0;
            // 
            // ListViewColectora
            // 
            this.ListViewColectora.BackColor = System.Drawing.Color.Gainsboro;
            this.ListViewColectora.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.ListViewColectora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewColectora.Enabled = false;
            this.ListViewColectora.HideSelection = false;
            this.ListViewColectora.Location = new System.Drawing.Point(0, 0);
            this.ListViewColectora.MultiSelect = false;
            this.ListViewColectora.Name = "ListViewColectora";
            this.ListViewColectora.Size = new System.Drawing.Size(264, 232);
            this.ListViewColectora.TabIndex = 52;
            this.ListViewColectora.UseCompatibleStateImageBehavior = false;
            this.ListViewColectora.View = System.Windows.Forms.View.Details;
            this.ListViewColectora.Visible = false;
            this.ListViewColectora.SelectedIndexChanged += new System.EventHandler(this.ListViewColectora_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Datos de Rutas en Colectora";
            this.columnHeader3.Width = 304;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.label6);
            this.splitContainer5.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer5_Panel1_Paint);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.LisViewDescargados);
            this.splitContainer5.Size = new System.Drawing.Size(264, 197);
            this.splitContainer5.SplitterDistance = 33;
            this.splitContainer5.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(82, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 19);
            this.label6.TabIndex = 47;
            this.label6.Text = "Rutas Descargadas";
            // 
            // LisViewDescargados
            // 
            this.LisViewDescargados.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.LisViewDescargados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LisViewDescargados.HideSelection = false;
            this.LisViewDescargados.Location = new System.Drawing.Point(0, 0);
            this.LisViewDescargados.MultiSelect = false;
            this.LisViewDescargados.Name = "LisViewDescargados";
            this.LisViewDescargados.Size = new System.Drawing.Size(264, 160);
            this.LisViewDescargados.TabIndex = 51;
            this.LisViewDescargados.UseCompatibleStateImageBehavior = false;
            this.LisViewDescargados.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "  Periodo   Distrito  NºCarga  Fecha  Hora          Rutas";
            this.columnHeader4.Width = 290;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(747, 140);
            this.dataGridView1.TabIndex = 12;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // backgroundProcesarCarga
            // 
            this.backgroundProcesarCarga.WorkerReportsProgress = true;
            this.backgroundProcesarCarga.WorkerSupportsCancellation = true;
            this.backgroundProcesarCarga.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundProcesarCarga.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundProcesarCarga.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // backgroundEnviarArchivo
            // 
            this.backgroundEnviarArchivo.WorkerReportsProgress = true;
            this.backgroundEnviarArchivo.WorkerSupportsCancellation = true;
            this.backgroundEnviarArchivo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundEnviarArchivo_DoWork);
            this.backgroundEnviarArchivo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundEnviarArchivo_RunWorkerCompleted);
            // 
            // toolTip5
            // 
            this.toolTip5.AutoPopDelay = 3000;
            this.toolTip5.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.toolTip5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.toolTip5.InitialDelay = 500;
            this.toolTip5.ReshowDelay = 100;
            // 
            // timer4
            // 
            this.timer4.Interval = 1000;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // shellViewBindingSource
            // 
            this.shellViewBindingSource.DataSource = typeof(GongSolutions.Shell.ShellView);
            // 
            // shellItemBindingSource
            // 
            this.shellItemBindingSource.DataSource = typeof(GongSolutions.Shell.ShellItem);
            // 
            // button2
            // 
            this.button2.Image = global::gagFIS_Interfase.Properties.Resources.bin_closed;
            this.button2.Location = new System.Drawing.Point(49, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(29, 28);
            this.button2.TabIndex = 53;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form4Cargas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 587);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form4Cargas";
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.form4_Load);
            this.Resize += new System.EventHandler(this.Form4_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PickBoxLoading)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBoxProrrateo.ResumeLayout(false);
            this.groupBoxProrrateo.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel1.PerformLayout();
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel1.PerformLayout();
            this.splitContainer9.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shellViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shellItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsPortableDeviceBindingSource)).EndInit();
            this.ResumeLayout(false);

        }







        private void groupBox1_Enter(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private System.Windows.Forms.ImageList imgList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private GongSolutions.Shell.ShellNotificationListener shellNotificationListener1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button BotProcesCarg;
        private System.Windows.Forms.TextBox Loc;
        private System.Windows.Forms.TextBox Ru;
        private System.Windows.Forms.Button BtonNodos;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer2;
        private System.ComponentModel.BackgroundWorker backgroundProcesarCarga;
        private System.Windows.Forms.Label RestNod;
        private System.Windows.Forms.Label PorcLabel;
        private System.Windows.Forms.Label labelCantReg;
        private System.Windows.Forms.Label label2;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.BindingSource shellViewBindingSource;
        private System.Windows.Forms.BindingSource shellItemBindingSource;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button botExpulsaColec;
        private System.Windows.Forms.Button BotEnviarCarga;
        private System.Windows.Forms.ComboBox cmbDevices;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource windowsPortableDeviceBindingSource;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label4;
        private GongSolutions.Shell.ShellView shellView2;
        private System.Windows.Forms.Label LabRestEnvArc;
        private System.Windows.Forms.ListView listViewCargasProcesadas;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button BotActPanPC;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ListView ListViewColectora;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button BotDevCarga;
        private System.Windows.Forms.ToolTip toolTip4;
        private System.Windows.Forms.Label LabRestDevArc;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Label labelEnviando;
        private System.ComponentModel.BackgroundWorker backgroundEnviarArchivo;
        private System.Windows.Forms.ToolTip toolTip5;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBoxProrrateo;
        private System.Windows.Forms.Label LabelInfoPro;
        private System.Windows.Forms.RadioButton radioButProBasYfec;
        private System.Windows.Forms.RadioButton radioButProrLim;
        private System.Windows.Forms.RadioButton radioButSinPro;
        private System.Windows.Forms.Label InfoTipoPro;
        private System.Windows.Forms.Button BotDetenerProcCarga;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.Button BtnCerrarUsuarios;
        public System.Windows.Forms.ListView ListViewCargados;
        public System.Windows.Forms.ListView LisViewDescargados;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RBEnvWifi;
        private System.Windows.Forms.RadioButton RBEnvCable;
        private System.Windows.Forms.ComboBox cmbDevicesWifi;
        private System.Windows.Forms.Timer timer4;
        public System.Windows.Forms.Label labelProcesando;
        public System.Windows.Forms.TreeView tvwCargas;
        private System.Windows.Forms.PictureBox PickBoxLoading;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verEstadosDeRutasToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label LabNomColect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
