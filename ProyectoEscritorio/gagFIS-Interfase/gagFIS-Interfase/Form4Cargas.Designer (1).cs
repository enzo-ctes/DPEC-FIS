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
        private System.Windows.Forms.TreeView tvwCargas;
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
            System.Windows.Forms.ImageList imageList1;
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.BotActPanPC = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Ru = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.BtonNodos = new System.Windows.Forms.Button();
            this.Loc = new System.Windows.Forms.TextBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.BtonProcesCarg = new System.Windows.Forms.Button();
            this.tvwCargas = new System.Windows.Forms.TreeView();
            this.imgList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.botActPanCol = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelCantReg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PorcLabel = new System.Windows.Forms.Label();
            this.RestNod = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listViewCargasProcesadas = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LabRestEnvArc = new System.Windows.Forms.Label();
            this.shellView2 = new GongSolutions.Shell.ShellView();
            this.BotEnviarCarga = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.shellNotificationListener1 = new GongSolutions.Shell.ShellNotificationListener(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.shellViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.shellItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.windowsPortableDeviceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.ListViewCargados = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shellViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shellItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsPortableDeviceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "Folder.gif");
            imageList1.Images.SetKeyName(1, "doc1.gif");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.BotActPanPC);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.Ru);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.BtonNodos);
            this.panel2.Controls.Add(this.Loc);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 410);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1323, 93);
            this.panel2.TabIndex = 3;
            this.toolTip1.SetToolTip(this.panel2, "Este es un mensaje ");
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(445, 90);
            this.panel5.TabIndex = 45;
            // 
            // BotActPanPC
            // 
            this.BotActPanPC.Image = global::gagFIS_Interfase.Properties.Resources.action_refresh;
            this.BotActPanPC.Location = new System.Drawing.Point(563, 6);
            this.BotActPanPC.Name = "BotActPanPC";
            this.BotActPanPC.Size = new System.Drawing.Size(43, 28);
            this.BotActPanPC.TabIndex = 44;
            this.BotActPanPC.UseVisualStyleBackColor = true;
            this.BotActPanPC.Click += new System.EventHandler(this.BotActPanPC_Click);
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
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(412, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 25);
            this.button3.TabIndex = 13;
            this.button3.TabStop = false;
            this.button3.Text = "Boton Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(343, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(92, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.Visible = false;
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
            this.BtonNodos.Location = new System.Drawing.Point(12, 29);
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
            this.btnCerrar.Location = new System.Drawing.Point(1192, 8);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(106, 30);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "&Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // BtonProcesCarg
            // 
            this.BtonProcesCarg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtonProcesCarg.Location = new System.Drawing.Point(5, 6);
            this.BtonProcesCarg.Name = "BtonProcesCarg";
            this.BtonProcesCarg.Size = new System.Drawing.Size(143, 25);
            this.BtonProcesCarg.TabIndex = 5;
            this.BtonProcesCarg.Text = "Procesar Carga";
            this.BtonProcesCarg.UseVisualStyleBackColor = true;
            this.BtonProcesCarg.Click += new System.EventHandler(this.button4_Click);
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
            this.tvwCargas.Size = new System.Drawing.Size(513, 358);
            this.tvwCargas.TabIndex = 0;
            this.tvwCargas.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCargas_BeforeCollapse);
            this.tvwCargas.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCargas_BeforeExpand);
            this.tvwCargas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCargas_AfterSelect);
            this.tvwCargas.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwCargas_NodeClick);
            this.tvwCargas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvwCargas_MouseDown);
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
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbDevices);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.botActPanCol);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1323, 50);
            this.panel1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(66, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 19);
            this.label4.TabIndex = 46;
            this.label4.Text = "Archivos para Cargar";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rutas Disponibles";
            // 
            // cmbDevices
            // 
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.FormattingEnabled = true;
            this.cmbDevices.Location = new System.Drawing.Point(1108, 22);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(126, 21);
            this.cmbDevices.TabIndex = 45;
            this.cmbDevices.SelectionChangeCommitted += new System.EventHandler(this.cmbDevices_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1036, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Dispositivos:";
            // 
            // botActPanCol
            // 
            this.botActPanCol.Image = global::gagFIS_Interfase.Properties.Resources.action_refresh;
            this.botActPanCol.Location = new System.Drawing.Point(1255, 17);
            this.botActPanCol.Name = "botActPanCol";
            this.botActPanCol.Size = new System.Drawing.Size(43, 28);
            this.botActPanCol.TabIndex = 43;
            this.botActPanCol.UseVisualStyleBackColor = true;
            this.botActPanCol.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.tvwCargas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1323, 360);
            this.splitContainer1.SplitterDistance = 515;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelCantReg);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.PorcLabel);
            this.panel3.Controls.Add(this.RestNod);
            this.panel3.Controls.Add(this.BtonProcesCarg);
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 275);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(513, 83);
            this.panel3.TabIndex = 1;
            // 
            // labelCantReg
            // 
            this.labelCantReg.AutoSize = true;
            this.labelCantReg.Location = new System.Drawing.Point(338, 37);
            this.labelCantReg.Name = "labelCantReg";
            this.labelCantReg.Size = new System.Drawing.Size(13, 13);
            this.labelCantReg.TabIndex = 39;
            this.labelCantReg.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 36);
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
            this.PorcLabel.Location = new System.Drawing.Point(439, 15);
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
            this.RestNod.Location = new System.Drawing.Point(17, 42);
            this.RestNod.Name = "RestNod";
            this.RestNod.Size = new System.Drawing.Size(41, 15);
            this.RestNod.TabIndex = 34;
            this.RestNod.Text = "label8";
            this.RestNod.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(161, 11);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(258, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 13;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.listViewCargasProcesadas);
            this.panel4.Controls.Add(this.ListViewCargados);
            this.panel4.Controls.Add(this.LabRestEnvArc);
            this.panel4.Controls.Add(this.shellView2);
            this.panel4.Controls.Add(this.BotEnviarCarga);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(802, 358);
            this.panel4.TabIndex = 29;
            // 
            // listViewCargasProcesadas
            // 
            this.listViewCargasProcesadas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewCargasProcesadas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewCargasProcesadas.Location = new System.Drawing.Point(8, 26);
            this.listViewCargasProcesadas.MultiSelect = false;
            this.listViewCargasProcesadas.Name = "listViewCargasProcesadas";
            this.listViewCargasProcesadas.Size = new System.Drawing.Size(296, 40);
            this.listViewCargasProcesadas.TabIndex = 49;
            this.listViewCargasProcesadas.UseCompatibleStateImageBehavior = false;
            this.listViewCargasProcesadas.View = System.Windows.Forms.View.Details;
            this.listViewCargasProcesadas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "  Periodo   Distrito  NºCarga  Fecha  Hora";
            this.columnHeader1.Width = 244;
            // 
            // LabRestEnvArc
            // 
            this.LabRestEnvArc.AutoSize = true;
            this.LabRestEnvArc.ForeColor = System.Drawing.Color.Red;
            this.LabRestEnvArc.Location = new System.Drawing.Point(325, 182);
            this.LabRestEnvArc.Name = "LabRestEnvArc";
            this.LabRestEnvArc.Size = new System.Drawing.Size(22, 13);
            this.LabRestEnvArc.TabIndex = 48;
            this.LabRestEnvArc.Text = ".....";
            this.LabRestEnvArc.Visible = false;
            // 
            // shellView2
            // 
            this.shellView2.Dock = System.Windows.Forms.DockStyle.Right;
            this.shellView2.Enabled = false;
            this.shellView2.Location = new System.Drawing.Point(457, 0);
            this.shellView2.Name = "shellView2";
            this.shellView2.Size = new System.Drawing.Size(345, 358);
            this.shellView2.StatusBar = null;
            this.shellView2.TabIndex = 47;
            this.shellView2.Text = "shellView2";
            this.shellView2.View = GongSolutions.Shell.ShellViewStyle.Details;
            this.shellView2.Visible = false;
            // 
            // BotEnviarCarga
            // 
            this.BotEnviarCarga.Image = global::gagFIS_Interfase.Properties.Resources.cargaarchivos;
            this.BotEnviarCarga.Location = new System.Drawing.Point(368, 138);
            this.BotEnviarCarga.Name = "BotEnviarCarga";
            this.BotEnviarCarga.Size = new System.Drawing.Size(47, 44);
            this.BotEnviarCarga.TabIndex = 42;
            this.BotEnviarCarga.UseVisualStyleBackColor = true;
            this.BotEnviarCarga.Click += new System.EventHandler(this.button1_Click_2);
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
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // shellViewBindingSource
            // 
            this.shellViewBindingSource.DataSource = typeof(GongSolutions.Shell.ShellView);
            // 
            // shellItemBindingSource
            // 
            this.shellItemBindingSource.DataSource = typeof(GongSolutions.Shell.ShellItem);
            // 
            // windowsPortableDeviceBindingSource
            // 
            this.windowsPortableDeviceBindingSource.DataSource = typeof(WindowsPortableDevicesLib.Domain.WindowsPortableDevice);
            // 
            // ListViewCargados
            // 
            this.ListViewCargados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ListViewCargados.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.ListViewCargados.Location = new System.Drawing.Point(8, 113);
            this.ListViewCargados.MultiSelect = false;
            this.ListViewCargados.Name = "ListViewCargados";
            this.ListViewCargados.Size = new System.Drawing.Size(296, 240);
            this.ListViewCargados.TabIndex = 50;
            this.ListViewCargados.UseCompatibleStateImageBehavior = false;
            this.ListViewCargados.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "  Periodo   Distrito  NºCarga  Fecha  Hora";
            this.columnHeader2.Width = 244;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(82, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 19);
            this.label5.TabIndex = 51;
            this.label5.Text = "Archivos Cargados ";
            // 
            // Form4Cargas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1323, 503);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form4Cargas";
            this.Text = "Form4Cargas";
            this.Load += new System.EventHandler(this.form4_Load);
            this.Resize += new System.EventHandler(this.Form4_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
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
        private System.Windows.Forms.Button BtonProcesCarg;
        private System.Windows.Forms.TextBox Loc;
        private System.Windows.Forms.TextBox Ru;
        private System.Windows.Forms.Button BtonNodos;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label RestNod;
        private System.Windows.Forms.Label PorcLabel;
        private System.Windows.Forms.Label labelCantReg;
        private System.Windows.Forms.Label label2;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.BindingSource shellViewBindingSource;
        private System.Windows.Forms.BindingSource shellItemBindingSource;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button botActPanCol;
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
        private System.Windows.Forms.ListView ListViewCargados;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label5;
    }
}
