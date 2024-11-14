namespace gagFIS_Interfase
{
    partial class FormLoading
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
            this.MiLoading = new System.Windows.Forms.PictureBox();
            this.LabCargando = new iTalk.iTalk_Label();
            ((System.ComponentModel.ISupportInitialize)(this.MiLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // MiLoading
            // 
            this.MiLoading.Image = global::gagFIS_Interfase.Properties.Resources.GIF_circular_loading1;
            this.MiLoading.Location = new System.Drawing.Point(75, 68);
            this.MiLoading.Name = "MiLoading";
            this.MiLoading.Size = new System.Drawing.Size(336, 237);
            this.MiLoading.TabIndex = 0;
            this.MiLoading.TabStop = false;
            // 
            // LabCargando
            // 
            this.LabCargando.AutoSize = true;
            this.LabCargando.BackColor = System.Drawing.Color.Transparent;
            this.LabCargando.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabCargando.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.LabCargando.Location = new System.Drawing.Point(187, 308);
            this.LabCargando.Name = "LabCargando";
            this.LabCargando.Size = new System.Drawing.Size(118, 30);
            this.LabCargando.TabIndex = 1;
            this.LabCargando.Text = "Cargando...";
            this.LabCargando.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 450);
            this.Controls.Add(this.LabCargando);
            this.Controls.Add(this.MiLoading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLoading";
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLoading";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormLoading_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MiLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MiLoading;
        private iTalk.iTalk_Label LabCargando;
    }
}