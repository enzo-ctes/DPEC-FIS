using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using gagLogs;


namespace gagFIS_Interfase {
    public partial class Form0 : Form {
        public int QueTimer = 0;       
        
        FormStart FS = new FormStart();


        public Form0() {
            InitializeComponent();
        }

        private void Form0_Load(object sender, EventArgs e)
        {
            // se hace esto para que cargue todo el formulario inicial y luego pida clave
            QueTimer = 1;
            timer1.Interval = 100;
            timer1.Enabled = true;
        }


        public void Iniciar() {
           

            StringBuilder stb3 = new StringBuilder();
            StringBuilder stb4 = new StringBuilder();
            StringBuilder stb5 = new StringBuilder();
            StringBuilder stb6 = new StringBuilder("1000");//contendra el directorio informes que debera tener por defecto el programa para que funcione
                                                     //el panel de informes


            Inis.GetPrivateProfileString("Datos", "VerAddPeriodo", "", stb3, 100, Ctte.ArchivoIniName);           
            int VerAddPeriodo = Convert.ToInt16(stb3.ToString());

            Inis.GetPrivateProfileString("Datos", "VerExtArchImportacion", "", stb4, 100, Ctte.ArchivoIniName);            
            Int16 VerExtensionArchivoImportacion = Convert.ToInt16(stb4.ToString());

            Inis.GetPrivateProfileString("Datos", "VerCBModuloEjec", "", stb5, 100, Ctte.ArchivoIniName);
            Int16 VerCBEntorno = Convert.ToInt16(stb5.ToString());

            //Presentar el formulario inicial de selección
            Form1Inicio Form1 = new Form1Inicio();

            Form1.WindowState = FormWindowState.Maximized;

            if (DB.Entorno == "QAS")
            {
                Form1.BackColor = Color.LightSalmon;
                Form1.frCmd.BackColor = Color.LightSalmon;
               Form1.PBExistArchImpNO.BackColor = Color.LightSalmon;
                Form1.RBQAS.Checked = true;
                Form1.RButBTX.Checked = true;
                Form1.RBPRD.Checked = false;
            }
            else if(DB.Entorno == "PRD")
            {
                Form1.BackColor = Color.AntiqueWhite;
                Form1.frCmd.BackColor = Color.AntiqueWhite;
                Form1.PBExistArchImpNO.BackColor = Color.AntiqueWhite;
                Form1.RBPRD.Checked = true;
                Form1.RButBTX.Checked = true;
                Form1.RBQAS.Checked = false;

            }
            else if (DB.Entorno == "SUP")
            {
                Form1.BackColor = Color.AntiqueWhite;
                Form1.frCmd.BackColor = Color.AntiqueWhite;
                Form1.PBExistArchImpNO.BackColor = Color.AntiqueWhite;
                Form1.RBPRD.Checked = true;
                Form1.RButBTX.Checked = true;
                Form1.RBQAS.Checked = false;
               
                Form1.btnImportar.Visible = false;
                Form1.PBExistArchImpSI.Visible = false;
                Form1.btnCargas.Visible = false;
                Form1.btnInfDesc.Visible = false;
                Form1.PBExistArchImpNO.Visible = false;
                Form1.PBExistArchImpSI.Visible = false;
            }

            Vble.LeerNombresCarpetas();

            if (!Directory.Exists(Vble.CarpetaDefectoInformes))
            {
                Directory.CreateDirectory(Vble.CarpetaDefectoInformes);
            }

            if ((DB.sDbUsu != Vble.UserAdmin() && DB.sDbKey != Vble.PassAdmin()) || DB.sDbUsu == Vble.OperAdmin())
            {
                if (DB.sDbUsu.ToUpper() == "SUPERVISOR" && DB.sDbKey == "Macro2020" || DB.sDbUsu.ToUpper() == "AUDITORIA" && DB.sDbKey == "Macro2020")
                {
                    Form1.btnImportar.Visible = false;
                    Form1.btnInfDesc.Visible = false;
                    Form1.btnExportacion.Text = "Resumenes";
                    Form1.btnExportacion.Visible = true;
                    Form1.btnConfigCarp.Visible = false;
                    Form1.button2.Visible = false;///boton para eliminar los registros de la base de datos segun el periodo que se desee
                    Form1.button3.Visible = false;
                    Form1.btnHistorial.Visible = false;
                    Form1.GBExtArch.Visible = false;
                    Form1.GBEntorno.Visible = false;
                    Form1.PBExistArchImpNO.Visible = false;
                    Form1.PBExistArchImpSI.Visible = false;
                    Form1.btnInfAltas.Visible = true;
                    Form1.btnDescargas.Visible = false;
                    Form1.btnExportacion.Size = new Size(313, 70);
                    Form1.btnInfAltas.Size = new Size(313, 70);
                    Form1.btnInfAltas.Location = new Point(11, 280);
                    Form1.btnExportacion.Location = new Point(11, 172);
                   
                }

                else
                {
                    //cambio visibilidad
                    Form1.btnImportar.Visible = true;
                    Form1.btnExportacion.Visible = true;
                    Form1.btnConfigCarp.Visible = false;
                    Form1.button2.Visible = false;///boton para eliminar los registros de la base de datos segun el periodo que se desee
                    Form1.button3.Visible = false;
                    Form1.btnHistorial.Visible = false;

                    ////cambio posicion de botones para que se ubique mejor al esconder botones de Importar y Exportar
                    //Form1.btnCargas.Location = new Point(24, 161);
                    //Form1.btnDescargas.Location = new Point(24, 195);
                    //Form1.btnInfDesc.Location = new Point(24, 230);
                    //Form1.btnInfAltas.Location = new Point(24, 294);


                    ////Cambio tamanaño de botones para que se adapte mejor a la interfaz
                    //Form1.btnExportacion.Size = new Size(300, 40);
                    //Form1.btnImportar.Size = new Size(300, 40);
                    //Form1.btnCargas.Size = new Size(300, 40);
                    //Form1.btnDescargas.Size = new Size(300, 40);
                    //Form1.btnInfDesc.Size = new Size(300, 40);
                    //Form1.btnInfAltas.Size = new Size(300, 40);

                    //Form1.RBPRD.Visible = false;
                    //Form1.RBQAS.Visible = false;
                    //Form1.RBPrueba.Visible = false;

                    //Form1.RB.Checked = true;
                    //Form1.RButGPG.Checked = true;
                    Form1.GBExtArch.Visible = false;
                    Form1.GBEntorno.Visible = false;
                }

            }

            //Form1.RBPRD.Checked = true;
            //Form1.RButBTX.Checked = true;


            if (VerAddPeriodo == 1)
            {
                Form1.TextNewPeriodo.Visible = true;
                Form1.BtnAddPeriodo.Visible = true;
            }
            else
            {
                Form1.TextNewPeriodo.Visible = false;
                Form1.BtnAddPeriodo.Visible = false;
            }

            if (VerExtensionArchivoImportacion == 1)
            {
                Form1.GBExtArch.Visible = true;
                //Form1.RBPRD.Visible = true;
                //Form1.RBQAS.Visible = true;
                //Form1.RBPrueba.Visible = true;
            }
            else
            {
                Form1.GBExtArch.Visible = false;
                //Form1.RBPRD.Visible = false;
                //Form1.RBQAS.Visible = false;
                //Form1.RBPrueba.Visible = false;
            }
            if (VerCBEntorno == 1)
            {
                Form1.GBEntorno.Visible = true;
            }
            else
            {
                Form1.GBEntorno.Visible = false;
            }
            LblCentroInterfaz.Text = Vble.centroInterfaz;
            LblCentroInterfaz.Visible = true;
            Form1.MdiParent = this;
            Form1.Show();
            Form1.WindowState = FormWindowState.Maximized;          
        }

        /// <summary> El temporizador llegó a su fin. 
        /// Hay una variable que indica qué se debe hacer, segun el valor de la misma
        /// <para>= 0: Al inicio del programa, dispara la carga de datos y el formulario de presentación. </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e) {
            switch(QueTimer)
                    {
                    case 1:
                    timer1.Enabled = false;
                    bool que = true;
                    while(que) {
                        que = false;
                        if(FS.ShowDialog(this) == DialogResult.OK)
                        {
                            Iniciar();                            
                        }
                        else {
                                if(MessageBox.Show("Debe ingresar un usuario. \n 'Aceptar': reintenta. \n 'Cancelar': Sale del programa",
                                "Error de usuario", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                que = true;
                                else
                                Application.Exit();
                        }
                    }
                    timer1.Enabled = false;
                    break;                    
                case 2:
                    break;                
                default:
                    break;
            }
        }




        void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            // TODO: Implement menuStrip1_ItemClicked
        }
        void mnuPeriodoActual_Click(object sender, EventArgs e) {
            // TODO: Implement mnuPeriodoActual_Click
        }
       

        /// <summary>
        /// Acciones a llevarse a cabo al cerrar el formulario principal:
        /// <para> -- 1. Cierre de base de datos.-</para> 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form0_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                Ctte.ArchivoLog.EscribirLog("Cerrando base de datos: " + DB.NombreBD +
                    " - Usuario: " + DB.sDbUsu);
                DB.conexBD.Close();
                Ctte.ArchivoLog.EscribirLog("Base de datos: " + DB.NombreBD +
                "  Cerrada! - Usuario: " + DB.sDbUsu);

            }
            catch {
                Ctte.ArchivoLog.EscribirLog("Error al cerrar base de datos: " + DB.NombreBD +
                " - Usuario: " + DB.sDbUsu);

            }

        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCambioContraseña CambContr = new FormCambioContraseña();
            CambContr.LabUser.Text = DB.sDbUsu;
            CambContr.Show();
        }
    }



   
}

