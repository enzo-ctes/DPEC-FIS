namespace gagFIS_Interfase
{
    partial class Form6Informes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6Informes));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.shellViewInformes = new GongSolutions.Shell.ShellView();
            this.BtonAtras = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.BtonAtras);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(597, 256);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.shellViewInformes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(597, 227);
            this.panel1.TabIndex = 2;
            // 
            // shellViewInformes
            // 
            this.shellViewInformes.CurrentFolder = new GongSolutions.Shell.ShellItem("file:///C:/A_DPEC/_Pruebas/EmpresaLocal/201604/Informes");
            this.shellViewInformes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellViewInformes.Location = new System.Drawing.Point(0, 0);
            this.shellViewInformes.MultiSelect = false;
            this.shellViewInformes.Name = "shellViewInformes";
            this.shellViewInformes.Size = new System.Drawing.Size(597, 227);
            this.shellViewInformes.StatusBar = null;
            this.shellViewInformes.TabIndex = 0;
            this.shellViewInformes.Text = "shellView1";
            this.shellViewInformes.View = GongSolutions.Shell.ShellViewStyle.Details;
            this.shellViewInformes.DoubleClick += new System.EventHandler(this.shellViewInformes_DoubleClick);
            // 
            // BtonAtras
            // 
            this.BtonAtras.Image = global::gagFIS_Interfase.Properties.Resources.action_back1;
            this.BtonAtras.Location = new System.Drawing.Point(12, 4);
            this.BtonAtras.Name = "BtonAtras";
            this.BtonAtras.Size = new System.Drawing.Size(51, 24);
            this.BtonAtras.TabIndex = 0;
            this.BtonAtras.UseVisualStyleBackColor = true;
            this.BtonAtras.Click += new System.EventHandler(this.BtonAtras_Click);
            // 
            // Form6Informes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 256);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form6Informes";
            this.Text = "Informes de Descargas";
            this.Load += new System.EventHandler(this.Form6Informes_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button BtonAtras;
        private System.Windows.Forms.Panel panel1;
        private GongSolutions.Shell.ShellView shellViewInformes;
    }
}