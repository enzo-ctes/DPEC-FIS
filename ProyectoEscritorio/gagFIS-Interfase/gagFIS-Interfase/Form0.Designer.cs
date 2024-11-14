namespace gagFIS_Interfase {
    partial class Form0 {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form0));
            this.imgList1 = new System.Windows.Forms.ImageList(this.components);
            this.imgSelec = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.periodoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPeriodoActual = new System.Windows.Forms.ToolStripMenuItem();
            this.usuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarContraseñaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.periodoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCboPeriodo = new System.Windows.Forms.ToolStripComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LblCentroInterfaz = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgList1
            // 
            this.imgList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imgList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imgSelec
            // 
            this.imgSelec.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSelec.ImageStream")));
            this.imgSelec.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSelec.Images.SetKeyName(0, "Nada");
            this.imgSelec.Images.SetKeyName(1, "Algo");
            this.imgSelec.Images.SetKeyName(2, "Todo");
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.periodoToolStripMenuItem1,
            this.mnuPeriodoActual,
            this.usuarioToolStripMenuItem,
            this.userToolStripMenuItem,
            this.cambiarContraseñaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(694, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // periodoToolStripMenuItem1
            // 
            this.periodoToolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.periodoToolStripMenuItem1.Name = "periodoToolStripMenuItem1";
            this.periodoToolStripMenuItem1.Size = new System.Drawing.Size(63, 20);
            this.periodoToolStripMenuItem1.Text = "Periodo:";
            this.periodoToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mnuPeriodoActual
            // 
            this.mnuPeriodoActual.Font = new System.Drawing.Font("Arial", 13F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuPeriodoActual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.mnuPeriodoActual.Name = "mnuPeriodoActual";
            this.mnuPeriodoActual.Size = new System.Drawing.Size(12, 20);
            this.mnuPeriodoActual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mnuPeriodoActual.Click += new System.EventHandler(this.mnuPeriodoActual_Click);
            // 
            // usuarioToolStripMenuItem
            // 
            this.usuarioToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
            this.usuarioToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.usuarioToolStripMenuItem.Text = "Usuario:";
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Font = new System.Drawing.Font("Arial", 13F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.userToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(62, 25);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Visible = false;
            // 
            // cambiarContraseñaToolStripMenuItem
            // 
            this.cambiarContraseñaToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.cambiarContraseñaToolStripMenuItem.Name = "cambiarContraseñaToolStripMenuItem";
            this.cambiarContraseñaToolStripMenuItem.Size = new System.Drawing.Size(127, 20);
            this.cambiarContraseñaToolStripMenuItem.Text = "Cambiar Contraseña";
            this.cambiarContraseñaToolStripMenuItem.Visible = false;
            this.cambiarContraseñaToolStripMenuItem.Click += new System.EventHandler(this.cambiarContraseñaToolStripMenuItem_Click);
            // 
            // periodoToolStripMenuItem
            // 
            this.periodoToolStripMenuItem.Name = "periodoToolStripMenuItem";
            this.periodoToolStripMenuItem.Size = new System.Drawing.Size(60, 23);
            this.periodoToolStripMenuItem.Text = "Periodo";
            // 
            // mnuCboPeriodo
            // 
            this.mnuCboPeriodo.BackColor = System.Drawing.SystemColors.Menu;
            this.mnuCboPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mnuCboPeriodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mnuCboPeriodo.Name = "mnuCboPeriodo";
            this.mnuCboPeriodo.Size = new System.Drawing.Size(75, 23);
            this.mnuCboPeriodo.Sorted = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LblCentroInterfaz
            // 
            this.LblCentroInterfaz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblCentroInterfaz.AutoSize = true;
            this.LblCentroInterfaz.BackColor = System.Drawing.SystemColors.Control;
            this.LblCentroInterfaz.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCentroInterfaz.ForeColor = System.Drawing.Color.Blue;
            this.LblCentroInterfaz.Location = new System.Drawing.Point(381, 3);
            this.LblCentroInterfaz.Name = "LblCentroInterfaz";
            this.LblCentroInterfaz.Size = new System.Drawing.Size(51, 18);
            this.LblCentroInterfaz.TabIndex = 3;
            this.LblCentroInterfaz.Text = "label1";
            this.LblCentroInterfaz.Visible = false;
            // 
            // Form0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 682);
            this.Controls.Add(this.LblCentroInterfaz);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form0";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "gagFIS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form0_FormClosing);
            this.Load += new System.EventHandler(this.Form0_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        public System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem periodoToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox mnuCboPeriodo;
        private System.Windows.Forms.ImageList imgList1;
        private System.Windows.Forms.ImageList imgSelec;
        private System.Windows.Forms.ToolStripMenuItem periodoToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem mnuPeriodoActual;
        private System.Windows.Forms.ToolStripMenuItem usuarioToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem cambiarContraseñaToolStripMenuItem;
        public System.Windows.Forms.Label LblCentroInterfaz;
    }
}

