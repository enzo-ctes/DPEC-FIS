namespace gagFIS_Interfase {
    partial class FormStart {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStart));
            this.label1 = new System.Windows.Forms.Label();
            this.grpUsu = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textKey = new iTalk.iTalk_TextBox_Small();
            this.textUsuario = new iTalk.iTalk_TextBox_Small();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lstLogin = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GBEntorno = new System.Windows.Forms.GroupBox();
            this.RBSUP = new System.Windows.Forms.CheckBox();
            this.RBQAS = new System.Windows.Forms.CheckBox();
            this.RBPRD = new System.Windows.Forms.CheckBox();
            this.RBPrueba = new System.Windows.Forms.CheckBox();
            this.CarBaseFijSP = new System.ComponentModel.BackgroundWorker();
            this.grpUsu.SuspendLayout();
            this.GBEntorno.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(493, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Iniciando...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpUsu
            // 
            this.grpUsu.BackColor = System.Drawing.Color.AliceBlue;
            this.grpUsu.Controls.Add(this.button1);
            this.grpUsu.Controls.Add(this.textKey);
            this.grpUsu.Controls.Add(this.textUsuario);
            this.grpUsu.Controls.Add(this.btnCancel);
            this.grpUsu.Controls.Add(this.btnAceptar);
            this.grpUsu.Controls.Add(this.txtKey);
            this.grpUsu.Controls.Add(this.txtUsuario);
            this.grpUsu.Controls.Add(this.label4);
            this.grpUsu.Controls.Add(this.label3);
            this.grpUsu.Location = new System.Drawing.Point(99, 82);
            this.grpUsu.Name = "grpUsu";
            this.grpUsu.Size = new System.Drawing.Size(390, 155);
            this.grpUsu.TabIndex = 2;
            this.grpUsu.TabStop = false;
            this.grpUsu.Text = "Ingrese Usuario y Contraseña";
            this.grpUsu.Enter += new System.EventHandler(this.grpUsu_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(155, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "TEST";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(56, 126);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Usuario";
            // 
            // lstLogin
            // 
            this.lstLogin.BackColor = System.Drawing.Color.RoyalBlue;
            this.lstLogin.ForeColor = System.Drawing.SystemColors.Info;
            this.lstLogin.FormattingEnabled = true;
            this.lstLogin.Location = new System.Drawing.Point(49, 40);
            this.lstLogin.Name = "lstLogin";
            this.lstLogin.Size = new System.Drawing.Size(504, 238);
            this.lstLogin.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GBEntorno
            // 
            this.GBEntorno.BackColor = System.Drawing.Color.AliceBlue;
            this.GBEntorno.Controls.Add(this.RBSUP);
            this.GBEntorno.Controls.Add(this.RBQAS);
            this.GBEntorno.Controls.Add(this.RBPRD);
            this.GBEntorno.Controls.Add(this.RBPrueba);
            this.GBEntorno.Location = new System.Drawing.Point(490, 103);
            this.GBEntorno.Name = "GBEntorno";
            this.GBEntorno.Size = new System.Drawing.Size(60, 112);
            this.GBEntorno.TabIndex = 41;
            this.GBEntorno.TabStop = false;
            this.GBEntorno.Text = "Entorno";
            this.GBEntorno.Visible = false;
            // 
            // RBSUP
            // 
            this.RBSUP.AutoSize = true;
            this.RBSUP.Location = new System.Drawing.Point(5, 59);
            this.RBSUP.Name = "RBSUP";
            this.RBSUP.Size = new System.Drawing.Size(48, 17);
            this.RBSUP.TabIndex = 39;
            this.RBSUP.Text = "SUP";
            this.RBSUP.UseVisualStyleBackColor = true;
            this.RBSUP.Visible = false;
            // 
            // RBQAS
            // 
            this.RBQAS.AutoSize = true;
            this.RBQAS.Location = new System.Drawing.Point(5, 45);
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
            this.RBPRD.Location = new System.Drawing.Point(5, 22);
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
            this.RBPrueba.Location = new System.Drawing.Point(5, 68);
            this.RBPrueba.Name = "RBPrueba";
            this.RBPrueba.Size = new System.Drawing.Size(60, 17);
            this.RBPrueba.TabIndex = 38;
            this.RBPrueba.Text = "Prueba";
            this.RBPrueba.UseVisualStyleBackColor = true;
            this.RBPrueba.Visible = false;
            // 
            // CarBaseFijSP
            // 
            this.CarBaseFijSP.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CarBaseFijSP_DoWork);
            // 
            // FormStart
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(608, 321);
            this.Controls.Add(this.GBEntorno);
            this.Controls.Add(this.grpUsu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormStart";
            this.Load += new System.EventHandler(this.FormStart_Load);
            this.grpUsu.ResumeLayout(false);
            this.grpUsu.PerformLayout();
            this.GBEntorno.ResumeLayout(false);
            this.GBEntorno.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpUsu;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstLogin;
        private iTalk.iTalk_TextBox_Small textUsuario;
        private iTalk.iTalk_TextBox_Small textKey;
        public System.Windows.Forms.GroupBox GBEntorno;
        public System.Windows.Forms.CheckBox RBQAS;
        public System.Windows.Forms.CheckBox RBPRD;
        public System.Windows.Forms.CheckBox RBPrueba;
        public System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker CarBaseFijSP;
        public System.Windows.Forms.CheckBox RBSUP;
        private System.Windows.Forms.Button button1;
    }
}