/*
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 16:45
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form5ConfigCarp.
    /// </summary>
    public partial class Form5ConfigCarp : Form {
        public Form5ConfigCarp(){
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        
        
        
        void Form5_Load(object sender, System.EventArgs e) {
            
            CargarCarpetas();
            
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;

        }
        
        
        /// <summary>
        /// Lee desde el archivo ini las carpetas y las carga en los
        /// cuadros de textos respectivos
        /// </summary>
        void CargarCarpetas() {
            var stB = new StringBuilder(500);
            Int16 VerTipoArchivoImp;


            Inis.GetPrivateProfileString(
               "Datos", "VerExtArchImportacion", "No", stB, 250, Ctte.ArchivoIniName);
            VerTipoArchivoImp = Convert.ToInt16(stB.ToString());


            if (VerTipoArchivoImp == 1)
            {
                RBSITipoArchImp.Checked = true;
            }
            else
            {
                RBNOTipoArchImp.Checked = true;
            }

            Inis.GetPrivateProfileString(
                "Datos", "Base", "No", stB, 250, Ctte.ArchivoIniName);
            textDirBasesSQLite.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Empresa Servidor", "No", stB, 250, Ctte.ArchivoIniName);
            txDirEmpresaServidor.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Trabajo local", "No", stB, 250, Ctte.ArchivoIniName);
            txDirTrabajoLocal.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Respaldo Local", "No", stB, 250, Ctte.ArchivoIniName);
            txDirRespaldoLocal.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Importacion", "No", stB, 250, Ctte.ArchivoIniName);
            txDirImportacion.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Exportacion", "No", stB, 250, Ctte.ArchivoIniName);
            txDirExportacion.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Informes", "No", stB, 250, Ctte.ArchivoIniName);
            txDirInformes.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Descargas Sin Proceso", "No", stB, 250, Ctte.ArchivoIniName);
            txDirDescargaNoProcesada.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Descargas Procesadas", "No", stB, 250, Ctte.ArchivoIniName);
            txDirDescargaProcesada.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Temporal", "No", stB, 250, Ctte.ArchivoIniName);
            txDirTempo.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Cargas Enviar", "No", stB, 250, Ctte.ArchivoIniName);
            txDirCargasNoEnviadas.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
                "Carpetas", "Dir Cargas Enviadas", "No", stB, 250, Ctte.ArchivoIniName);
            txDirCargasEnviadas.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
               "Carpetas", "Dir Directorio Colectora en PC", "No", stB, 250, Ctte.ArchivoIniName);
            txCarpColecEnPc.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
               "Carpetas", "Dir CarpetaColectora", "No", stB, 250, Ctte.ArchivoIniName);
            txCarpCompColectoras.Text = stB.ToString().Trim();

            Inis.GetPrivateProfileString(
               "Carpetas", "Dir CarpetaDestinoColectora", "No", stB, 250, Ctte.ArchivoIniName);
            txCarpColEnCol.Text = stB.ToString().Trim();

        }

        /// <summary>
        /// Guarda las carpetas cargadas en los cuadros de texto en el
        /// archivo ini de configuración
        /// </summary>
        void GuardarCarpetas() {
            //Inis.WritePrivateProfileString(

            if (RBSITipoArchImp.Checked == true)
            {
                Inis.WritePrivateProfileString("Datos", "VerExtArchImportacion", 1.ToString(), Ctte.ArchivoIniName);
            }
            else if (RBNOTipoArchImp.Checked == true)
            {
                Inis.WritePrivateProfileString("Datos", "VerExtArchImportacion", 0.ToString(), Ctte.ArchivoIniName);
            }



            Inis.WritePrivateProfileString(
                "Datos", "Base", textDirBasesSQLite.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Empresa Servidor", txDirEmpresaServidor.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Trabajo Local", txDirTrabajoLocal.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Respaldo Local", txDirRespaldoLocal.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Importacion", txDirImportacion.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Exportacion", txDirExportacion.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Informes", txDirInformes.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Descargas Sin Proceso", txDirDescargaNoProcesada.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Descargas Procesadas", txDirDescargaProcesada.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Temporal", txDirTempo.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Cargas Enviar", txDirCargasNoEnviadas.Text, Ctte.ArchivoIniName);

            Inis.WritePrivateProfileString(
                "Carpetas", "Dir Cargas Enviadas", txDirCargasEnviadas.Text, Ctte.ArchivoIniName);


        }
        
        
        
        
        void btnCancelar_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void Form5_Resize(object sender, System.EventArgs e) {
            //this.WindowState = FormWindowState.Maximized;
            panel1.Left = (this.Width - panel1.Width) / 2;
            panel1.Top = (this.ClientSize.Height - panel1.Height) / 2;
        }

        private void btnAceptar_Click(object sender, EventArgs e) {
            GuardarCarpetas();
            
            this.Close();
        }
    }
}
