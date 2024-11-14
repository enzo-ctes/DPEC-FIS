namespace gagFIS_Interfase
{
    partial class FormLogeoPermisos
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
            this.grpUsu = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textKey = new iTalk.iTalk_TextBox_Small();
            this.textUsuario = new iTalk.iTalk_TextBox_Small();
            this.grpUsu.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpUsu
            // 
            this.grpUsu.BackColor = System.Drawing.SystemColors.Control;
            this.grpUsu.Controls.Add(this.textKey);
            this.grpUsu.Controls.Add(this.textUsuario);
            this.grpUsu.Controls.Add(this.btnCancel);
            this.grpUsu.Controls.Add(this.btnAceptar);
            this.grpUsu.Controls.Add(this.txtKey);
            this.grpUsu.Controls.Add(this.txtUsuario);
            this.grpUsu.Controls.Add(this.label4);
            this.grpUsu.Controls.Add(this.label3);
            this.grpUsu.Location = new System.Drawing.Point(12, 12);
            this.grpUsu.Name = "grpUsu";
            this.grpUsu.Size = new System.Drawing.Size(390, 155);
            this.grpUsu.TabIndex = 3;
            this.grpUsu.TabStop = false;
            this.grpUsu.Text = "Credenciales";
            this.grpUsu.Enter += new System.EventHandler(this.grpUsu_Enter);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(56, 126);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(266, 126);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(307, 92);
            this.txtKey.Name = "txtKey";
            this.txtKey.PasswordChar = '*';
            this.txtKey.Size = new System.Drawing.Size(40, 20);
            this.txtKey.TabIndex = 3;
            this.txtKey.Text = "Micc4001";
            this.txtKey.Visible = false;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(311, 36);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(40, 20);
            this.txtUsuario.TabIndex = 2;
            this.txtUsuario.Text = "admin";
            this.txtUsuario.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Contraseña";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "DNI";
            // 
            // textKey
            // 
            this.textKey.BackColor = System.Drawing.Color.Transparent;
            this.textKey.Font = new System.Drawing.Font("Tahoma", 11F);
            this.textKey.ForeColor = System.Drawing.Color.DimGray;
            this.textKey.Location = new System.Drawing.Point(41, 84);
            this.textKey.MaxLength = 32767;
            this.textKey.Multiline = false;
            this.textKey.Name = "textKey";
            this.textKey.ReadOnly = false;
            this.textKey.Size = new System.Drawing.Size(310, 28);
            this.textKey.TabIndex = 7;
            this.textKey.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.textKey.UseSystemPasswordChar = true;
            // 
            // textUsuario
            // 
            this.textUsuario.BackColor = System.Drawing.Color.Transparent;
            this.textUsuario.Font = new System.Drawing.Font("Tahoma", 11F);
            this.textUsuario.ForeColor = System.Drawing.Color.DimGray;
            this.textUsuario.Location = new System.Drawing.Point(41, 36);
            this.textUsuario.MaxLength = 32767;
            this.textUsuario.Multiline = false;
            this.textUsuario.Name = "textUsuario";
            this.textUsuario.ReadOnly = false;
            this.textUsuario.Size = new System.Drawing.Size(310, 28);
            this.textUsuario.TabIndex = 6;
            this.textUsuario.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.textUsuario.UseSystemPasswordChar = false;
            // 
            // FormLogeoPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 181);
            this.ControlBox = false;
            this.Controls.Add(this.grpUsu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogeoPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLogeoPermisos";
            this.Load += new System.EventHandler(this.FormLogeoPermisos_Load);
            this.grpUsu.ResumeLayout(false);
            this.grpUsu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpUsu;
        private iTalk.iTalk_TextBox_Small textKey;
        private iTalk.iTalk_TextBox_Small textUsuario;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}