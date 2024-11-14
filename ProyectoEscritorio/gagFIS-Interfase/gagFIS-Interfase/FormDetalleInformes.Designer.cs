namespace gagFIS_Interfase
{
    partial class FormDetalleInformes
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lpbexcelCircular = new System.Windows.Forms.Label();
            this.GroupBoxResumenGral = new iTalk.iTalk_GroupBox();
            this.LabCargandoInformes = new iTalk.iTalk_Label();
            this.PBExcelCircular = new iTalk.iTalk_ProgressBar();
            this.LblDatosInforme = new System.Windows.Forms.Label();
            this.lblAvanceExportacion = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarAExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.MiLoadingInformes = new System.Windows.Forms.PictureBox();
            this.dgResumen = new System.Windows.Forms.DataGridView();
            this.dFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dLecturista = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dRuta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dHoraInicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dHoraFin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dDuracion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dPorcentaje_Hora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dTotalRuta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dLeidos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dImpr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dPorcenajte_Impr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dFueraRango = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dIndicacionNoImprimir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dMarcadoXLote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dApagados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.iTalk_GroupBox1 = new iTalk.iTalk_GroupBox();
            this.PBLLecDias = new System.Windows.Forms.PictureBox();
            this.LVLectDias = new System.Windows.Forms.ListView();
            this.Fecha = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CantTomados = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CantLeidos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CantImpresos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VerLects = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.iTalk_GroupBox2 = new iTalk.iTalk_GroupBox();
            this.PBLOprs = new System.Windows.Forms.PictureBox();
            this.LVLectOper = new System.Windows.Forms.ListView();
            this.fechaLect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.operario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CantLect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bgwDetalleExport = new System.ComponentModel.BackgroundWorker();
            this.BGWInfSuperv = new System.ComponentModel.BackgroundWorker();
            this.bgwLectXOp = new System.ComponentModel.BackgroundWorker();
            this.BGWInfAltas = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.GroupBoxResumenGral.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MiLoadingInformes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgResumen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.iTalk_GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBLLecDias)).BeginInit();
            this.iTalk_GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBLOprs)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lpbexcelCircular);
            this.splitContainer1.Panel1.Controls.Add(this.GroupBoxResumenGral);
            this.splitContainer1.Panel1.Controls.Add(this.PBExcelCircular);
            this.splitContainer1.Panel1.Controls.Add(this.LblDatosInforme);
            this.splitContainer1.Panel1.Controls.Add(this.lblAvanceExportacion);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar);
            this.splitContainer1.Panel1.Controls.Add(this.BtnExcel);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1385, 731);
            this.splitContainer1.SplitterDistance = 162;
            this.splitContainer1.TabIndex = 0;
            // 
            // lpbexcelCircular
            // 
            this.lpbexcelCircular.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lpbexcelCircular.AutoSize = true;
            this.lpbexcelCircular.BackColor = System.Drawing.SystemColors.Control;
            this.lpbexcelCircular.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpbexcelCircular.Location = new System.Drawing.Point(794, 70);
            this.lpbexcelCircular.Name = "lpbexcelCircular";
            this.lpbexcelCircular.Size = new System.Drawing.Size(141, 16);
            this.lpbexcelCircular.TabIndex = 52;
            this.lpbexcelCircular.Text = "Exportando a Excel";
            this.lpbexcelCircular.Visible = false;
            // 
            // GroupBoxResumenGral
            // 
            this.GroupBoxResumenGral.BackColor = System.Drawing.Color.Transparent;
            this.GroupBoxResumenGral.Controls.Add(this.LabCargandoInformes);
            this.GroupBoxResumenGral.Location = new System.Drawing.Point(1222, 12);
            this.GroupBoxResumenGral.MinimumSize = new System.Drawing.Size(136, 50);
            this.GroupBoxResumenGral.Name = "GroupBoxResumenGral";
            this.GroupBoxResumenGral.Padding = new System.Windows.Forms.Padding(5, 28, 5, 5);
            this.GroupBoxResumenGral.Size = new System.Drawing.Size(151, 69);
            this.GroupBoxResumenGral.TabIndex = 4;
            this.GroupBoxResumenGral.Text = "Detalle";
            this.GroupBoxResumenGral.Visible = false;
            this.GroupBoxResumenGral.Click += new System.EventHandler(this.GroupBoxResumenGral_Click);
            // 
            // LabCargandoInformes
            // 
            this.LabCargandoInformes.AutoSize = true;
            this.LabCargandoInformes.BackColor = System.Drawing.SystemColors.Control;
            this.LabCargandoInformes.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabCargandoInformes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.LabCargandoInformes.Location = new System.Drawing.Point(497, 419);
            this.LabCargandoInformes.Name = "LabCargandoInformes";
            this.LabCargandoInformes.Size = new System.Drawing.Size(0, 30);
            this.LabCargandoInformes.TabIndex = 7;
            this.LabCargandoInformes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PBExcelCircular
            // 
            this.PBExcelCircular.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PBExcelCircular.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PBExcelCircular.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.PBExcelCircular.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PBExcelCircular.Location = new System.Drawing.Point(661, 27);
            this.PBExcelCircular.Maximum = ((long)(100));
            this.PBExcelCircular.MinimumSize = new System.Drawing.Size(100, 100);
            this.PBExcelCircular.Name = "PBExcelCircular";
            this.PBExcelCircular.ProgressColor1 = System.Drawing.Color.Silver;
            this.PBExcelCircular.ProgressColor2 = System.Drawing.Color.Silver;
            this.PBExcelCircular.ProgressShape = iTalk.iTalk_ProgressBar._ProgressShape.Round;
            this.PBExcelCircular.Size = new System.Drawing.Size(105, 105);
            this.PBExcelCircular.TabIndex = 9;
            this.PBExcelCircular.Text = "Exportando a Excel";
            this.PBExcelCircular.Value = ((long)(0));
            this.PBExcelCircular.Visible = false;
            // 
            // LblDatosInforme
            // 
            this.LblDatosInforme.AutoSize = true;
            this.LblDatosInforme.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDatosInforme.Location = new System.Drawing.Point(21, 71);
            this.LblDatosInforme.Name = "LblDatosInforme";
            this.LblDatosInforme.Size = new System.Drawing.Size(70, 25);
            this.LblDatosInforme.TabIndex = 50;
            this.LblDatosInforme.Text = "label1";
            this.LblDatosInforme.Visible = false;
            // 
            // lblAvanceExportacion
            // 
            this.lblAvanceExportacion.AutoSize = true;
            this.lblAvanceExportacion.Location = new System.Drawing.Point(269, 52);
            this.lblAvanceExportacion.Name = "lblAvanceExportacion";
            this.lblAvanceExportacion.Size = new System.Drawing.Size(35, 13);
            this.lblAvanceExportacion.TabIndex = 49;
            this.lblAvanceExportacion.Text = "label1";
            this.lblAvanceExportacion.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(11, 43);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(252, 23);
            this.progressBar.TabIndex = 48;
            this.progressBar.Visible = false;
            // 
            // BtnExcel
            // 
            this.BtnExcel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnExcel.Image = global::gagFIS_Interfase.Properties.Resources.iconoexcel;
            this.BtnExcel.Location = new System.Drawing.Point(305, 70);
            this.BtnExcel.Name = "BtnExcel";
            this.BtnExcel.Size = new System.Drawing.Size(33, 30);
            this.BtnExcel.TabIndex = 47;
            this.BtnExcel.UseVisualStyleBackColor = true;
            this.BtnExcel.Visible = false;
            this.BtnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1383, 24);
            this.menuStrip1.TabIndex = 51;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportarAExcelToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // exportarAExcelToolStripMenuItem
            // 
            this.exportarAExcelToolStripMenuItem.Name = "exportarAExcelToolStripMenuItem";
            this.exportarAExcelToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exportarAExcelToolStripMenuItem.Text = "Exportar a Excel";
            this.exportarAExcelToolStripMenuItem.Click += new System.EventHandler(this.exportarAExcelToolStripMenuItem_Click);
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
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.splitContainer2.Panel1.Controls.Add(this.MiLoadingInformes);
            this.splitContainer2.Panel1.Controls.Add(this.dgResumen);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1383, 563);
            this.splitContainer2.SplitterDistance = 230;
            this.splitContainer2.TabIndex = 5;
            // 
            // MiLoadingInformes
            // 
            this.MiLoadingInformes.BackColor = System.Drawing.SystemColors.Control;
            this.MiLoadingInformes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MiLoadingInformes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MiLoadingInformes.Image = global::gagFIS_Interfase.Properties.Resources.GIF_circular_loading1;
            this.MiLoadingInformes.Location = new System.Drawing.Point(0, 0);
            this.MiLoadingInformes.Name = "MiLoadingInformes";
            this.MiLoadingInformes.Size = new System.Drawing.Size(1383, 230);
            this.MiLoadingInformes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MiLoadingInformes.TabIndex = 8;
            this.MiLoadingInformes.TabStop = false;
            // 
            // dgResumen
            // 
            this.dgResumen.AllowUserToAddRows = false;
            this.dgResumen.AllowUserToDeleteRows = false;
            this.dgResumen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResumen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dFecha,
            this.dLecturista,
            this.dRuta,
            this.dHoraInicio,
            this.dHoraFin,
            this.dDuracion,
            this.dPorcentaje_Hora,
            this.dTotalRuta,
            this.dLeidos,
            this.dImpr,
            this.dPorcenajte_Impr,
            this.dFueraRango,
            this.dIndicacionNoImprimir,
            this.dMarcadoXLote,
            this.dApagados});
            this.dgResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgResumen.Location = new System.Drawing.Point(0, 0);
            this.dgResumen.Name = "dgResumen";
            this.dgResumen.ReadOnly = true;
            this.dgResumen.Size = new System.Drawing.Size(1383, 230);
            this.dgResumen.TabIndex = 1;
            this.dgResumen.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dgResumen_CellContextMenuStripNeeded);
            // 
            // dFecha
            // 
            this.dFecha.HeaderText = "Fecha";
            this.dFecha.Name = "dFecha";
            this.dFecha.ReadOnly = true;
            this.dFecha.Width = 80;
            // 
            // dLecturista
            // 
            this.dLecturista.HeaderText = "Lecturista";
            this.dLecturista.Name = "dLecturista";
            this.dLecturista.ReadOnly = true;
            this.dLecturista.Width = 50;
            // 
            // dRuta
            // 
            this.dRuta.HeaderText = "Ruta";
            this.dRuta.Name = "dRuta";
            this.dRuta.ReadOnly = true;
            this.dRuta.Width = 65;
            // 
            // dHoraInicio
            // 
            this.dHoraInicio.HeaderText = "HoraInicio";
            this.dHoraInicio.Name = "dHoraInicio";
            this.dHoraInicio.ReadOnly = true;
            this.dHoraInicio.Width = 80;
            // 
            // dHoraFin
            // 
            this.dHoraFin.HeaderText = "HoraFin";
            this.dHoraFin.Name = "dHoraFin";
            this.dHoraFin.ReadOnly = true;
            this.dHoraFin.Width = 80;
            // 
            // dDuracion
            // 
            this.dDuracion.HeaderText = "Duracion";
            this.dDuracion.Name = "dDuracion";
            this.dDuracion.ReadOnly = true;
            this.dDuracion.Width = 80;
            // 
            // dPorcentaje_Hora
            // 
            this.dPorcentaje_Hora.HeaderText = "% Hora";
            this.dPorcentaje_Hora.Name = "dPorcentaje_Hora";
            this.dPorcentaje_Hora.ReadOnly = true;
            this.dPorcentaje_Hora.Width = 80;
            // 
            // dTotalRuta
            // 
            this.dTotalRuta.HeaderText = "Total Usuarios";
            this.dTotalRuta.Name = "dTotalRuta";
            this.dTotalRuta.ReadOnly = true;
            this.dTotalRuta.Width = 80;
            // 
            // dLeidos
            // 
            this.dLeidos.HeaderText = "Leidos";
            this.dLeidos.Name = "dLeidos";
            this.dLeidos.ReadOnly = true;
            this.dLeidos.Width = 80;
            // 
            // dImpr
            // 
            this.dImpr.HeaderText = "Impresos";
            this.dImpr.Name = "dImpr";
            this.dImpr.ReadOnly = true;
            this.dImpr.Width = 80;
            // 
            // dPorcenajte_Impr
            // 
            this.dPorcenajte_Impr.HeaderText = "% Impr";
            this.dPorcenajte_Impr.Name = "dPorcenajte_Impr";
            this.dPorcenajte_Impr.ReadOnly = true;
            this.dPorcenajte_Impr.Width = 80;
            // 
            // dFueraRango
            // 
            this.dFueraRango.HeaderText = "Rango";
            this.dFueraRango.Name = "dFueraRango";
            this.dFueraRango.ReadOnly = true;
            this.dFueraRango.Width = 80;
            // 
            // dIndicacionNoImprimir
            // 
            this.dIndicacionNoImprimir.HeaderText = "Indicados";
            this.dIndicacionNoImprimir.Name = "dIndicacionNoImprimir";
            this.dIndicacionNoImprimir.ReadOnly = true;
            this.dIndicacionNoImprimir.Width = 80;
            // 
            // dMarcadoXLote
            // 
            this.dMarcadoXLote.HeaderText = "Marcados por Lote";
            this.dMarcadoXLote.Name = "dMarcadoXLote";
            this.dMarcadoXLote.ReadOnly = true;
            // 
            // dApagados
            // 
            this.dApagados.HeaderText = "Apagados";
            this.dApagados.Name = "dApagados";
            this.dApagados.ReadOnly = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.iTalk_GroupBox1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.iTalk_GroupBox2);
            this.splitContainer3.Size = new System.Drawing.Size(1383, 329);
            this.splitContainer3.SplitterDistance = 673;
            this.splitContainer3.TabIndex = 0;
            // 
            // iTalk_GroupBox1
            // 
            this.iTalk_GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_GroupBox1.Controls.Add(this.PBLLecDias);
            this.iTalk_GroupBox1.Controls.Add(this.LVLectDias);
            this.iTalk_GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTalk_GroupBox1.Location = new System.Drawing.Point(0, 0);
            this.iTalk_GroupBox1.MinimumSize = new System.Drawing.Size(136, 50);
            this.iTalk_GroupBox1.Name = "iTalk_GroupBox1";
            this.iTalk_GroupBox1.Padding = new System.Windows.Forms.Padding(5, 28, 5, 5);
            this.iTalk_GroupBox1.Size = new System.Drawing.Size(673, 329);
            this.iTalk_GroupBox1.TabIndex = 0;
            this.iTalk_GroupBox1.Text = "Lecturas por día";
            // 
            // PBLLecDias
            // 
            this.PBLLecDias.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PBLLecDias.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PBLLecDias.Image = global::gagFIS_Interfase.Properties.Resources.gif_loading_circular_rayas;
            this.PBLLecDias.Location = new System.Drawing.Point(5, 82);
            this.PBLLecDias.Name = "PBLLecDias";
            this.PBLLecDias.Size = new System.Drawing.Size(609, 242);
            this.PBLLecDias.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBLLecDias.TabIndex = 3;
            this.PBLLecDias.TabStop = false;
            // 
            // LVLectDias
            // 
            this.LVLectDias.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Fecha,
            this.CantTomados,
            this.CantLeidos,
            this.CantImpresos,
            this.VerLects});
            this.LVLectDias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVLectDias.HideSelection = false;
            this.LVLectDias.Location = new System.Drawing.Point(5, 28);
            this.LVLectDias.Name = "LVLectDias";
            this.LVLectDias.Size = new System.Drawing.Size(663, 296);
            this.LVLectDias.TabIndex = 0;
            this.LVLectDias.UseCompatibleStateImageBehavior = false;
            this.LVLectDias.View = System.Windows.Forms.View.Details;
            this.LVLectDias.SelectedIndexChanged += new System.EventHandler(this.LVLectDias_SelectedIndexChanged);
            this.LVLectDias.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LVLectDias_MouseDoubleClick);
            // 
            // Fecha
            // 
            this.Fecha.Text = "Fecha";
            this.Fecha.Width = 102;
            // 
            // CantTomados
            // 
            this.CantTomados.Text = "Tomados";
            // 
            // CantLeidos
            // 
            this.CantLeidos.Text = "Solo Leidos";
            this.CantLeidos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CantLeidos.Width = 122;
            // 
            // CantImpresos
            // 
            this.CantImpresos.Text = "Leidos e Impresos";
            this.CantImpresos.Width = 122;
            // 
            // VerLects
            // 
            this.VerLects.Text = "VER lecturas por Lecturista";
            this.VerLects.Width = 150;
            // 
            // iTalk_GroupBox2
            // 
            this.iTalk_GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_GroupBox2.Controls.Add(this.PBLOprs);
            this.iTalk_GroupBox2.Controls.Add(this.LVLectOper);
            this.iTalk_GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTalk_GroupBox2.Location = new System.Drawing.Point(0, 0);
            this.iTalk_GroupBox2.MinimumSize = new System.Drawing.Size(136, 50);
            this.iTalk_GroupBox2.Name = "iTalk_GroupBox2";
            this.iTalk_GroupBox2.Padding = new System.Windows.Forms.Padding(5, 28, 5, 5);
            this.iTalk_GroupBox2.Size = new System.Drawing.Size(706, 329);
            this.iTalk_GroupBox2.TabIndex = 1;
            this.iTalk_GroupBox2.Text = "Lecturas por Lecturista";
            // 
            // PBLOprs
            // 
            this.PBLOprs.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PBLOprs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PBLOprs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBLOprs.Image = global::gagFIS_Interfase.Properties.Resources.gif_loading_circular_rayas;
            this.PBLOprs.Location = new System.Drawing.Point(5, 28);
            this.PBLOprs.Name = "PBLOprs";
            this.PBLOprs.Size = new System.Drawing.Size(696, 296);
            this.PBLOprs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBLOprs.TabIndex = 2;
            this.PBLOprs.TabStop = false;
            this.PBLOprs.Visible = false;
            // 
            // LVLectOper
            // 
            this.LVLectOper.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fechaLect,
            this.operario,
            this.CantLect});
            this.LVLectOper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVLectOper.HideSelection = false;
            this.LVLectOper.Location = new System.Drawing.Point(5, 28);
            this.LVLectOper.Name = "LVLectOper";
            this.LVLectOper.Size = new System.Drawing.Size(696, 296);
            this.LVLectOper.TabIndex = 1;
            this.LVLectOper.UseCompatibleStateImageBehavior = false;
            this.LVLectOper.View = System.Windows.Forms.View.Details;
            // 
            // fechaLect
            // 
            this.fechaLect.Text = "Fecha";
            this.fechaLect.Width = 111;
            // 
            // operario
            // 
            this.operario.Text = "Operario";
            // 
            // CantLect
            // 
            this.CantLect.Text = "Tomados";
            // 
            // bgwDetalleExport
            // 
            this.bgwDetalleExport.WorkerReportsProgress = true;
            this.bgwDetalleExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwDetalleExport_DoWork);
            this.bgwDetalleExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwDetalleExport_ProgressChanged);
            this.bgwDetalleExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwDetalleExport_RunWorkerCompleted);
            // 
            // BGWInfSuperv
            // 
            this.BGWInfSuperv.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGWInfSuperv_DoWork);
            this.BGWInfSuperv.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BGWInfSuperv_ProgressChanged);
            this.BGWInfSuperv.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGWInfSuperv_RunWorkerCompleted);
            // 
            // bgwLectXOp
            // 
            this.bgwLectXOp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwLectXOp_DoWork);
            this.bgwLectXOp.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwLectXOp_RunWorkerCompleted);
            // 
            // BGWInfAltas
            // 
            this.BGWInfAltas.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGWInfAltas_DoWork);
            // 
            // FormDetalleInformes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 731);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDetalleInformes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DetalleInformes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormDetalleInformes_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.GroupBoxResumenGral.ResumeLayout(false);
            this.GroupBoxResumenGral.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MiLoadingInformes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgResumen)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.iTalk_GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBLLecDias)).EndInit();
            this.iTalk_GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBLOprs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button BtnExcel;
        private System.Windows.Forms.DataGridView dgResumen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn dLecturista;
        private System.Windows.Forms.DataGridViewTextBoxColumn dRuta;
        private System.Windows.Forms.DataGridViewTextBoxColumn dHoraInicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn dHoraFin;
        private System.Windows.Forms.DataGridViewTextBoxColumn dDuracion;
        private System.Windows.Forms.DataGridViewTextBoxColumn dPorcentaje_Hora;
        private System.Windows.Forms.DataGridViewTextBoxColumn dTotalRuta;
        private System.Windows.Forms.DataGridViewTextBoxColumn dLeidos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dImpr;
        private System.Windows.Forms.DataGridViewTextBoxColumn dPorcenajte_Impr;
        private System.Windows.Forms.DataGridViewTextBoxColumn dFueraRango;
        private System.Windows.Forms.DataGridViewTextBoxColumn dIndicacionNoImprimir;
        private System.Windows.Forms.DataGridViewTextBoxColumn dMarcadoXLote;
        private System.Windows.Forms.DataGridViewTextBoxColumn dApagados;
        private System.ComponentModel.BackgroundWorker bgwDetalleExport;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker BGWInfSuperv;
        private System.Windows.Forms.PictureBox MiLoadingInformes;
        private System.Windows.Forms.Label lblAvanceExportacion;
        private System.Windows.Forms.Label LblDatosInforme;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private iTalk.iTalk_GroupBox iTalk_GroupBox1;
        private iTalk.iTalk_GroupBox iTalk_GroupBox2;
        private System.Windows.Forms.ListView LVLectDias;
        private System.Windows.Forms.ListView LVLectOper;
        private System.Windows.Forms.ColumnHeader Fecha;
        private System.Windows.Forms.ColumnHeader CantImpresos;
        private System.Windows.Forms.ColumnHeader CantLeidos;
        private System.Windows.Forms.ColumnHeader CantTomados;
        private System.Windows.Forms.ColumnHeader fechaLect;
        private iTalk.iTalk_GroupBox GroupBoxResumenGral;
        private iTalk.iTalk_Label LabCargandoInformes;
        private System.Windows.Forms.PictureBox PBLOprs;
        private System.Windows.Forms.PictureBox PBLLecDias;
        private System.Windows.Forms.ColumnHeader CantLect;
        private System.Windows.Forms.ColumnHeader operario;
        public System.ComponentModel.BackgroundWorker bgwLectXOp;
        private System.Windows.Forms.ColumnHeader VerLects;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarAExcelToolStripMenuItem;
        private iTalk.iTalk_ProgressBar PBExcelCircular;
        private System.Windows.Forms.Label lpbexcelCircular;
        private System.ComponentModel.BackgroundWorker BGWInfAltas;
    }
}