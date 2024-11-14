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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Ruta 201-1170");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Remesa 1", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Nodo19");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Nodo18", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Nodo20");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Nodo12", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Nodo13");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Remesa 2", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("201-Capital", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Ruta 202-1200");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Nodo1-0-1");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Nodo1-0-2");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Remesa 1", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Nodo1-1-0");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Ruta 202-1200", new System.Windows.Forms.TreeNode[] {
            treeNode14});
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Remesa 3");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("202 - Goya", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode15,
            treeNode16});
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Nodo9");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Nodo2", new System.Windows.Forms.TreeNode[] {
            treeNode18});
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Nodo10");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Nodo3", new System.Windows.Forms.TreeNode[] {
            treeNode20});
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Ru = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Loc = new System.Windows.Forms.TextBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.BtonProcesCarg = new System.Windows.Forms.Button();
            this.tvwCargas = new System.Windows.Forms.TreeView();
            this.imgList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.BtonNodos = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.RestNod = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.btnProcesarCarga = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.shellView1 = new GongSolutions.Shell.ShellView();
            this.dataGridConceptosDat = new System.Windows.Forms.DataGridView();
            this.dataGridConex = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridMed = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridPers = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labelCantReg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.shellNotificationListener1 = new GongSolutions.Shell.ShellNotificationListener(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConceptosDat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.Ru);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.Loc);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 565);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1259, 52);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(258, 25);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(79, 20);
            this.textBox3.TabIndex = 4;
            this.textBox3.Visible = false;
            // 
            // Ru
            // 
            this.Ru.Location = new System.Drawing.Point(80, 25);
            this.Ru.Name = "Ru";
            this.Ru.Size = new System.Drawing.Size(60, 20);
            this.Ru.TabIndex = 11;
            this.Ru.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(360, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(113, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(158, 25);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(79, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Visible = false;
            // 
            // Loc
            // 
            this.Loc.Location = new System.Drawing.Point(6, 25);
            this.Loc.Name = "Loc";
            this.Loc.Size = new System.Drawing.Size(61, 20);
            this.Loc.TabIndex = 7;
            this.Loc.Visible = false;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1141, 10);
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
            this.BtonProcesCarg.Location = new System.Drawing.Point(130, 3);
            this.BtonProcesCarg.Name = "BtonProcesCarg";
            this.BtonProcesCarg.Size = new System.Drawing.Size(143, 25);
            this.BtonProcesCarg.TabIndex = 5;
            this.BtonProcesCarg.Text = "Procesar Carga (Enzo)";
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
            treeNode1.Name = "Nodo11";
            treeNode1.Text = "Ruta 201-1170";
            treeNode2.Name = "Nodo4";
            treeNode2.Text = "Remesa 1";
            treeNode3.Name = "Nodo19";
            treeNode3.Text = "Nodo19";
            treeNode4.Name = "Nodo18";
            treeNode4.Text = "Nodo18";
            treeNode5.Name = "Nodo20";
            treeNode5.Text = "Nodo20";
            treeNode6.Name = "Nodo12";
            treeNode6.Text = "Nodo12";
            treeNode7.Name = "Nodo13";
            treeNode7.Text = "Nodo13";
            treeNode8.Name = "Nodo5";
            treeNode8.Text = "Remesa 2";
            treeNode9.Name = "Nodo0";
            treeNode9.Text = "201-Capital";
            treeNode10.Name = "Nodo14";
            treeNode10.Text = "Ruta 202-1200";
            treeNode11.Name = "Nodo15";
            treeNode11.Text = "Nodo1-0-1";
            treeNode12.Name = "Nodo16";
            treeNode12.Text = "Nodo1-0-2";
            treeNode13.Name = "Nodo6";
            treeNode13.Text = "Remesa 1";
            treeNode14.Name = "Nodo17";
            treeNode14.Text = "Nodo1-1-0";
            treeNode15.Name = "Nodo7";
            treeNode15.Text = "Ruta 202-1200";
            treeNode16.Name = "Nodo8";
            treeNode16.Text = "Remesa 3";
            treeNode17.Name = "Nodo1";
            treeNode17.Text = "202 - Goya";
            treeNode18.Name = "Nodo9";
            treeNode18.Text = "Nodo9";
            treeNode19.Name = "Nodo2";
            treeNode19.Text = "Nodo2";
            treeNode20.Name = "Nodo10";
            treeNode20.Text = "Nodo10";
            treeNode21.Name = "Nodo3";
            treeNode21.Text = "Nodo3";
            this.tvwCargas.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode17,
            treeNode19,
            treeNode21});
            this.tvwCargas.SelectedImageIndex = 0;
            this.tvwCargas.ShowNodeToolTips = true;
            this.tvwCargas.Size = new System.Drawing.Size(489, 510);
            this.tvwCargas.TabIndex = 0;
            this.tvwCargas.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCargas_BeforeCollapse);
            this.tvwCargas.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCargas_BeforeExpand);
            this.tvwCargas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCargas_AfterSelect);
            this.tvwCargas.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwCargas_NodeClick);
            this.tvwCargas.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwCargas_NodeMouseDoubleClick);
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1259, 53);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rutas Disponibles";
            // 
            // BtonNodos
            // 
            this.BtonNodos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtonNodos.Location = new System.Drawing.Point(11, 37);
            this.BtonNodos.Name = "BtonNodos";
            this.BtonNodos.Size = new System.Drawing.Size(70, 43);
            this.BtonNodos.TabIndex = 33;
            this.BtonNodos.Text = "Boton Nodos";
            this.BtonNodos.UseVisualStyleBackColor = true;
            this.BtonNodos.Visible = false;
            this.BtonNodos.Click += new System.EventHandler(this.button2_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 53);
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
            this.splitContainer1.Panel2.Controls.Add(this.dataGridConceptosDat);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridConex);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridMed);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridPers);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel2.Controls.Add(this.labelCantReg);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1259, 512);
            this.splitContainer1.SplitterDistance = 491;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.RestNod);
            this.panel3.Controls.Add(this.BtonNodos);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.btnProcesarCarga);
            this.panel3.Controls.Add(this.BtonProcesCarg);
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 427);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(489, 83);
            this.panel3.TabIndex = 1;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(312, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 25);
            this.button1.TabIndex = 35;
            this.button1.Text = "Volver";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RestNod
            // 
            this.RestNod.AutoSize = true;
            this.RestNod.ForeColor = System.Drawing.Color.Red;
            this.RestNod.Location = new System.Drawing.Point(130, 31);
            this.RestNod.Name = "RestNod";
            this.RestNod.Size = new System.Drawing.Size(35, 13);
            this.RestNod.TabIndex = 34;
            this.RestNod.Text = "label8";
            this.RestNod.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(402, 55);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 25);
            this.button3.TabIndex = 13;
            this.button3.TabStop = false;
            this.button3.Text = "Limpiar tabla";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnProcesarCarga
            // 
            this.btnProcesarCarga.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesarCarga.Location = new System.Drawing.Point(11, 3);
            this.btnProcesarCarga.Name = "btnProcesarCarga";
            this.btnProcesarCarga.Size = new System.Drawing.Size(103, 25);
            this.btnProcesarCarga.TabIndex = 1;
            this.btnProcesarCarga.Text = "Procesar Carga";
            this.btnProcesarCarga.UseVisualStyleBackColor = true;
            this.btnProcesarCarga.Click += new System.EventHandler(this.btnProcesarCarga_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(276, 8);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(208, 16);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 13;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel4.Controls.Add(this.shellView1);
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(756, 366);
            this.panel4.TabIndex = 28;
            // 
            // shellView1
            // 
            this.shellView1.CurrentFolder = new GongSolutions.Shell.ShellItem("shell:///MyComputerFolder");
            this.shellView1.Location = new System.Drawing.Point(3, 3);
            this.shellView1.Name = "shellView1";
            this.shellView1.Size = new System.Drawing.Size(745, 388);
            this.shellView1.StatusBar = null;
            this.shellView1.TabIndex = 0;
            this.shellView1.Text = "shellView1";
            // 
            // dataGridConceptosDat
            // 
            this.dataGridConceptosDat.AllowUserToAddRows = false;
            this.dataGridConceptosDat.AllowUserToDeleteRows = false;
            this.dataGridConceptosDat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridConceptosDat.Location = new System.Drawing.Point(443, 248);
            this.dataGridConceptosDat.Name = "dataGridConceptosDat";
            this.dataGridConceptosDat.ReadOnly = true;
            this.dataGridConceptosDat.Size = new System.Drawing.Size(107, 88);
            this.dataGridConceptosDat.TabIndex = 27;
            // 
            // dataGridConex
            // 
            this.dataGridConex.AllowUserToAddRows = false;
            this.dataGridConex.AllowUserToDeleteRows = false;
            this.dataGridConex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridConex.Location = new System.Drawing.Point(21, 248);
            this.dataGridConex.Name = "dataGridConex";
            this.dataGridConex.ReadOnly = true;
            this.dataGridConex.Size = new System.Drawing.Size(107, 88);
            this.dataGridConex.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(464, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "ConceptosDatos";
            // 
            // dataGridMed
            // 
            this.dataGridMed.AllowUserToAddRows = false;
            this.dataGridMed.AllowUserToDeleteRows = false;
            this.dataGridMed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMed.Location = new System.Drawing.Point(159, 248);
            this.dataGridMed.Name = "dataGridMed";
            this.dataGridMed.ReadOnly = true;
            this.dataGridMed.Size = new System.Drawing.Size(107, 88);
            this.dataGridMed.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(332, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Personas";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Medidores";
            // 
            // dataGridPers
            // 
            this.dataGridPers.AllowUserToAddRows = false;
            this.dataGridPers.AllowUserToDeleteRows = false;
            this.dataGridPers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPers.Location = new System.Drawing.Point(300, 248);
            this.dataGridPers.Name = "dataGridPers";
            this.dataGridPers.ReadOnly = true;
            this.dataGridPers.Size = new System.Drawing.Size(107, 88);
            this.dataGridPers.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Conexiones";
            this.label4.Click += new System.EventHandler(this.labelCarga_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(747, 183);
            this.dataGridView1.TabIndex = 12;
            // 
            // labelCantReg
            // 
            this.labelCantReg.AutoSize = true;
            this.labelCantReg.Location = new System.Drawing.Point(739, 198);
            this.labelCantReg.Name = "labelCantReg";
            this.labelCantReg.Size = new System.Drawing.Size(13, 13);
            this.labelCantReg.TabIndex = 9;
            this.labelCantReg.Text = "0";
            this.labelCantReg.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(608, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cantidad de Conexiones:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
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
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form4Cargas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 617);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConceptosDat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private System.Windows.Forms.ImageList imgList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnProcesarCarga;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private GongSolutions.Shell.ShellNotificationListener shellNotificationListener1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button BtonProcesCarg;
        private System.Windows.Forms.TextBox Loc;
        private System.Windows.Forms.Label labelCantReg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Ru;
        private System.Windows.Forms.Button BtonNodos;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.DataGridView dataGridMed;
        private System.Windows.Forms.DataGridView dataGridConex;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridView dataGridPers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridConceptosDat;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label RestNod;
        private GongSolutions.Shell.ShellView shellView1;
        private System.Windows.Forms.Button button1;
    }
}
