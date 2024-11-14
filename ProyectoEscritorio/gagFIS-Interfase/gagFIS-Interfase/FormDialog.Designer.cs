namespace gagFIS_Interfase {
    partial class FormDialog {
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
            this.grpPartir = new System.Windows.Forms.GroupBox();
            this.lbRuta = new System.Windows.Forms.Label();
            this.lbAyuda = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCantV = new System.Windows.Forms.TextBox();
            this.txtHastaV = new System.Windows.Forms.TextBox();
            this.txtDesdeV = new System.Windows.Forms.TextBox();
            this.txtCant = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHasta = new System.Windows.Forms.TextBox();
            this.txtDesde = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpPartir.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPartir
            // 
            this.grpPartir.Controls.Add(this.lbRuta);
            this.grpPartir.Controls.Add(this.lbAyuda);
            this.grpPartir.Controls.Add(this.button1);
            this.grpPartir.Controls.Add(this.btnOK);
            this.grpPartir.Controls.Add(this.btnCancel);
            this.grpPartir.Controls.Add(this.groupBox2);
            this.grpPartir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPartir.Location = new System.Drawing.Point(12, 12);
            this.grpPartir.Name = "grpPartir";
            this.grpPartir.Size = new System.Drawing.Size(302, 404);
            this.grpPartir.TabIndex = 0;
            this.grpPartir.TabStop = false;
            this.grpPartir.Text = "DialogoParticion Ruta";
            // 
            // lbRuta
            // 
            this.lbRuta.AutoSize = true;
            this.lbRuta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRuta.Location = new System.Drawing.Point(6, 24);
            this.lbRuta.Name = "lbRuta";
            this.lbRuta.Size = new System.Drawing.Size(47, 15);
            this.lbRuta.TabIndex = 6;
            this.lbRuta.Text = "label6";
            // 
            // lbAyuda
            // 
            this.lbAyuda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAyuda.Location = new System.Drawing.Point(6, 173);
            this.lbAyuda.Name = "lbAyuda";
            this.lbAyuda.Size = new System.Drawing.Size(290, 165);
            this.lbAyuda.TabIndex = 2;
            this.lbAyuda.Text = "Solo considera dos campos:";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DarkRed;
            this.button1.Location = new System.Drawing.Point(34, 341);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(237, 24);
            this.button1.TabIndex = 5;
            this.button1.Text = "Eliminar Todas las particiones";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(185, 369);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(86, 27);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Aceptar";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(34, 369);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtCantV);
            this.groupBox2.Controls.Add(this.txtHastaV);
            this.groupBox2.Controls.Add(this.txtDesdeV);
            this.groupBox2.Controls.Add(this.txtCant);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtHasta);
            this.groupBox2.Controls.Add(this.txtDesde);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 128);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(199, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Nuevo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(100, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Actual";
            // 
            // txtCantV
            // 
            this.txtCantV.Location = new System.Drawing.Point(71, 90);
            this.txtCantV.Name = "txtCantV";
            this.txtCantV.Size = new System.Drawing.Size(100, 23);
            this.txtCantV.TabIndex = 8;
            // 
            // txtHastaV
            // 
            this.txtHastaV.Location = new System.Drawing.Point(71, 61);
            this.txtHastaV.Name = "txtHastaV";
            this.txtHastaV.Size = new System.Drawing.Size(100, 23);
            this.txtHastaV.TabIndex = 7;
            // 
            // txtDesdeV
            // 
            this.txtDesdeV.Location = new System.Drawing.Point(71, 32);
            this.txtDesdeV.Name = "txtDesdeV";
            this.txtDesdeV.Size = new System.Drawing.Size(100, 23);
            this.txtDesdeV.TabIndex = 6;
            // 
            // txtCant
            // 
            this.txtCant.Location = new System.Drawing.Point(177, 91);
            this.txtCant.Name = "txtCant";
            this.txtCant.Size = new System.Drawing.Size(100, 23);
            this.txtCant.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Cantidad";
            // 
            // txtHasta
            // 
            this.txtHasta.Location = new System.Drawing.Point(177, 62);
            this.txtHasta.Name = "txtHasta";
            this.txtHasta.Size = new System.Drawing.Size(100, 23);
            this.txtHasta.TabIndex = 3;
            // 
            // txtDesde
            // 
            this.txtDesde.Location = new System.Drawing.Point(177, 33);
            this.txtDesde.Name = "txtDesde";
            this.txtDesde.Size = new System.Drawing.Size(100, 23);
            this.txtDesde.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hasta";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Desde";
            // 
            // FormDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(326, 428);
            this.Controls.Add(this.grpPartir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDialog";
            this.Text = "FormDialog";
            this.grpPartir.ResumeLayout(false);
            this.grpPartir.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPartir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbAyuda;
        private System.Windows.Forms.TextBox txtHasta;
        private System.Windows.Forms.TextBox txtDesde;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtCant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCantV;
        private System.Windows.Forms.TextBox txtHastaV;
        private System.Windows.Forms.TextBox txtDesdeV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbRuta;
    }
}