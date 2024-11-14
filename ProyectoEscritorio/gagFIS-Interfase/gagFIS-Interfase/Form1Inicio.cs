/*
 * Creado por 
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 13:39
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */




using System.ComponentModel;
using System.Threading;
using System.Linq;
using GongSolutions.Shell;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Collections.ObjectModel;
using WindowsPortableDevicesLib.Domain;
using WindowsPortableDevicesLib;
using System.Diagnostics;
using PortableDeviceApiLib;
using System;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Globalization;
using System.Configuration;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using static gagFIS_Interfase.Form4Cargas;
using System.Windows;
using System.Net;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
//using System.Net.NetworkInformation.IPv4InterfaceStatistics;


namespace gagFIS_Interfase
{


    /// <summary>
    /// Description of FormInicio.
    /// </summary>
    public partial class Form1Inicio : Form
    {
        //public static  DownloadYUpload Download = new DownloadYUpload();
        public static int k = 0;
        private int cnt = 0;
        //private int QueTimer = 0;
        [DllImport("user32.dll")]
        public static extern int GetKeyState(byte nVirtKey);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        //private DriveDetector driveDetector = null;
        StandardWindowsPortableDeviceService service = new StandardWindowsPortableDeviceService();
        byte NUMLOCK_KEY = 0X90;
        byte CAPS_KEY = 0x14;

        public string RutaRecibir = "";


        public int CantidadFacturas = 0;

        FormLoading loading;
        //Retornar el estado de una tecla 
        private bool KeyStatus(byte KeyCode)
        {
            return (GetKeyState(KeyCode) == 1);
        }
        string ArchivoConRuta;
        string NombreArchivo;
        //Enviar una senal de presion para (teclado extendido) 
        private void PresionarTecla(byte KeyCode)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            keybd_event(KeyCode, 0x45, KEYEVENTF_KEYUP, (UIntPtr)0);
            keybd_event(KeyCode, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
        }


        public Form1Inicio()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Load del Form1Inicio aca se llama a los procesos que se ejecutan cuando se abre la pantalla incio despues de realizarse 
        /// el control previo al logeo y determinar entorno y usuarios.
        /// Se carga la lista de periodos, desde el vigente, anterior y posterior (esto va avanzando teniendo en cuenta la fecha actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_Load(object sender, System.EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.WindowState = FormWindowState.Maximized;
            ((Form0)this.MdiParent).userToolStripMenuItem.Text = DB.sDbUsu;
            ((Form0)this.MdiParent).userToolStripMenuItem.Visible = true;

            //ParaQueGenerar(enumGeneraPara.CD);
            CargarPeriodos();

            //btnTest.Location = frCmd.Location;
            //btnTest.SendToBack();
            Vble.LeerNombresCarpetas();
            RButGPG.Checked = false;
            RButBTX.Checked = true;

            toolTip1.SetToolTip(RButBTX, "Realiza la importación si en el Servidor se subio el archivo con extension .BTX");
            toolTip1.SetToolTip(RButGPG, "Realiza la importación si en el Servidor se subio el archivo con extension .GPG");

            if (DB.sDbUsu == Vble.UserAdmin())
            {
                ///cambiar a true cuando vuelva de vacas
                this.groupBoxArchivoAImpor.Visible = false;
                this.RButBTX.Visible = true;
                this.RButGPG.Visible = true;
                this.btnConfigCarp.Visible = false;
                this.btnTest.Visible = true;
            }

            Vble.LeerNombresCarpetas();
            Vble.ArrayZona.Clear();
            Vble.LeerArchivoZonaFIS();

            Inis.GetPrivateProfileString("Datos", "VerLogImportacion", "", Vble.PanelLogImp, 10, Ctte.ArchivoIniName);

            string valor = Vble.PanelLogImp.ToString();

            if (Vble.PanelLogImp.ToString() == "1")
            {
                InfoImportacion.Visible = true;
                CargarRutasImportadas();
            }
            else
            {
                InfoImportacion.Visible = false;
            }

            timer2.Start();
        }

        /// <summary>
        /// Metodo que consultara la tabla LogImportacion y mostrará en el listview al costado de los accesos del menu las
        /// Rutas que ya han sido importadas del periodo vigente que se encuentra en la pantalla
        /// en la misma tambien se informa si hay usuarios apartados, es decir aquellos que no fueron cargados a la base de FIS
        /// por alguna inconsistencia de datos.
        /// </summary>
        private void CargarRutasImportadas()
        {
            try
            {
                LVResImpor.Items.Clear();
                DataTable TableImportados = new DataTable();
                string PeriodoImportadas = cboPeriodo.Text.Replace("-", "");

                string txSQL = "SELECT * FROM LogImportacion WHERE Periodo = " + PeriodoImportadas +
                                " AND (Zona = " + Vble.ArrayZona[0] + iteracionzona() + ")";
                MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TableImportados);

                datosAdapter.Dispose();
                comandoSQL.Dispose();

                InfoImportacion.Visible = true;
                ListViewItem ResumenImportacion;
                ResumenImportacion = new ListViewItem();

                if (TableImportados.Rows.Count >= 1)
                {
                    foreach (DataRow item in TableImportados.Rows)
                    {
                        ResumenImportacion = new ListViewItem(item["Porcion"].ToString());
                        ResumenImportacion.SubItems.Add(item["CantUsuarios"].ToString());
                        ResumenImportacion.SubItems.Add(item["CantImportados"].ToString());
                        ResumenImportacion.SubItems.Add(item["CantApartados"].ToString());
                        ResumenImportacion.SubItems.Add(item["IDLogImportacion"].ToString());
                        LVResImpor.Items.Add(ResumenImportacion);

                    }
                }
            }
            catch (Exception)
            {
                CargarRutasImportadas();
            }

        }

        /// <summary>
        /// Recorre el ArrayList que contiene los codigos de localidades almacenados al leer el archivo
        /// ZonaFIS.txt de cada centro de localidad. Si el archivo no contine nignun codigo devuelve vacio.
        /// </summary>
        /// <returns></returns>
        private string iteracionzona()
        {
            string iteracion = "";

            if (Vble.ArrayZona.Count > 1)
            {
                for (int i = 1; i < Vble.ArrayZona.Count; i++)
                {
                    iteracion += " OR Zona = " + Vble.ArrayZona[i].ToString() + " ";
                }
            }

            return iteracion;

        }

        private void Form1_Activated(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        /*
        /// <summary>
        /// Activa las opciones de generar para servidor o para CD
        /// y deja resaltado el boton que corresponda
        /// </summary>
        /// <param name="ip"></param>
        private void ParaQueGenerar(enumGeneraPara ip)
        {   
        }*/


        #region Funciones

        /// <summary>
        /// Lee desde el archivo de confiuguración el último periodo
        /// procesado, y lo muestra como por defecto, luego a partir de la fecha
        /// actual, agrega el periodo actual, uno anterior y uno posterior,
        /// siempre que no se repitan
        /// </summary>
        private void CargarPeriodos()
        {
            var PerDef = new StringBuilder();
            int Anio, Per;
            var FormAltas = new Form7InformesAltas();

            //Periodos a partir del actual
            Anio = DateTime.Now.Year;
            Per = (DateTime.Now.Month + 1) / 2;

            //Anterior
            if (Per == 1)
                cboPeriodo.Items.Add((Anio - 1).ToString("0000") + "-06");

            else
                cboPeriodo.Items.Add(Anio.ToString("0000") + "-" +
                    (Per - 1).ToString("00"));
            FormAltas.CBPerDesdeAltas.Items.Add((Anio - 1).ToString("0000") + "-06");

            //Actual
            cboPeriodo.Items.Add(Anio.ToString("0000") + "-" +
                    Per.ToString("00"));


            //Siguiente
            if (Per == 6)
                cboPeriodo.Items.Add((Anio + 1).ToString("0000") + "-01");
            else
                cboPeriodo.Items.Add(Anio.ToString("0000") + "-" +
                    (Per + 1).ToString("00"));

            //Si no está el por defecto lo agrega
            Inis.GetPrivateProfileString(
                "Datos", "Periodo", cboPeriodo.Items[0].ToString(), PerDef, 8, Ctte.ArchivoIniName);
            if (!cboPeriodo.Items.Contains(PerDef.ToString()))
                cboPeriodo.Items.Add(PerDef.ToString());

            //Defecto
            cboPeriodo.Text = PerDef.ToString();
            cboPeriodo.Items.Add("2015-03");
            //cboPeriodo.Items.Add("2017-05");
            //cboPeriodo.Items.Add("2017-06");

            //Actualiza el periodo indicado en la barra de menús principal
            ((Form0)this.MdiParent).mnuPeriodoActual.Text = cboPeriodo.Text;



        }



        /// <summary>
        /// Renombra el archivo Importación que fue compartido por SAP modificando la descripción del Nro de Lote al correspondiente secuencialmente a la descarga
        /// </summary>
        public void ActualizarNroLoteNombreArchivoImportación(string NombreArchivo)
        {
            try
            {
                Vble.NombreArchivoImportacion = "";
                string[] lectura = NombreArchivo.Split('_', '.');
                int contadguion = 0;
                foreach (var item in lectura)
                {
                    contadguion++;
                    string dato = item;
                    if (contadguion == 2)
                    {
                        StringBuilder stb = new StringBuilder();
                        Inis.GetPrivateProfileString("NdeLote", "Lote", "0", stb, 50, Ctte.ArchivoIniName);
                        int lote = Convert.ToInt32(stb.ToString()) + 1;
                        dato = lote.ToString("00000000");
                    }
                    if (dato == "btx")
                    {
                        Vble.NombreArchivoImportacion += "." + dato;
                    }
                    else
                    {
                        Vble.NombreArchivoImportacion += dato + "_";
                    }
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al crear nombre de archivo download.");
            }

        }

        /// <summary>
        /// Envio el archivo encriptado a pc local para poder descencriptar mediante archivo.bat y ejecuto la importación,
        /// esto se hace debido a que no se puede descencriptar el archivo compartido por SAP que se encuentra en la NAS
        /// siendo la ubicación de este NO local.
        /// </summary>
        /// <param name="compartidoconSAP"></param>
        /// <param name="archivoimportacion"></param>
        public void EnviarAPC(string compartidoconSAP, string archivoimportacion)
        {
            int cantarchivo;
            //var Download = new DownloadYUpload();
            string FechaDownload = DateTime.Now.ToString("yyyyMMdd");

            //Vble.CarpetaImportacion.Replace('operario', Environment.UserName);
            Vble.CarpetaImportacion = Vble.ValorarUnNombreRuta(Vble.CarpetaImportacion);
            string CarpetaImport = Vble.ValorarUnNombreRuta(Vble.CarpetaImportacion);

            try
            {
                if (!Directory.Exists(CarpetaImport))
                {
                    Directory.CreateDirectory(CarpetaImport);
                }

                List<string> strFiles = Directory.GetFiles(CarpetaImport, "*", SearchOption.AllDirectories).ToList();

                foreach (string fichero in strFiles)
                {
                    File.Delete(fichero);
                }

                if (File.Exists(CarpetaImport + archivoimportacion))
                {
                    File.Delete(CarpetaImport + archivoimportacion);
                }

                File.Copy(compartidoconSAP, CarpetaImport + archivoimportacion);
                DirectoryInfo ImportacionPC = new DirectoryInfo(Vble.ValorarUnNombreRuta(Vble.CarpetaImportacion));

                //////Reinicio el contador de archivos para verificar que unicamente 
                //////tengo el archivo encriptado que se va a leer
                cantarchivo = 0;
                foreach (var fi in ImportacionPC.GetFiles()) { cantarchivo++; }


                //Verifica si esta seleccionado el RadioButon para importacion de archivos encriptados GPG, llama al proceso
                //de desencriptar, si no, no hace nada y toma el archivo BTX
                if (RButGPG.Checked == true)
                {
                    //Si contiene el unico archivo encriptado, llamo al metodo Descencriptar el cual
                    //lo descencripta para poder realizar la importación 
                    if (cantarchivo == 1)
                    {
                        foreach (var fi in ImportacionPC.GetFiles())
                        {
                            if (fi.Extension == "gpg")
                            {
                                DescencriptarArchivo(fi.Name, fi.FullName, fi.DirectoryName);
                            }
                            else
                            {
                                MessageBox.Show("El archivo que se compartió para Importar no es un archivo encriptado, " +
                                                "por favor verifique con el administrador si el archivo compartido es el correcto", "Archivo sin encriptar",
                                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                Vble.CancelarImportacion = true;
                                return;
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("directorio sin archivos al descencriptar", "Error de Archivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                //////Reinicio el contador de archivos para verificar que unicamente tengo el archivo descencriptado 
                cantarchivo = 0;
                foreach (var fil in ImportacionPC.GetFiles())
                {
                    cantarchivo++;
                    //Cuento la cantidad de conexiones que se van a descargar para tomar como maximo del progress bar                  
                    Vble.TotalConexiones = CantidadConexiones(fil.FullName);
                }


                //Si existe unicamente el archivo descencriptado lo envio al directorio Importación donde se realizará la descarga del archivo
                if (cantarchivo == 1)
                {
                    foreach (var fil in ImportacionPC.GetFiles())
                    {
                        if (File.Exists(fil.FullName))
                        {
                            //string CarpImportacion = "C:\\Users\\Enzo\\Documents\\Visual Studio 2015\\Projects\\MACRO INTELL - Software\\Datos DPEC\\Importacion\\";                                                        
                            ActualizarNroLoteNombreArchivoImportación(fil.Name);
                            Vble.ArchivoImportación = "";
                            Vble.ArchivoImportación = CarpetaImport + Vble.NombreArchivoImportacion;
                            Vble.ArchivoImportación = Vble.ValorarUnNombreRuta(Vble.ArchivoImportación);

                            if (RBPRD.Checked == true)
                            {
                                if (!Directory.Exists(Vble.DownloadEntregadas + "\\" + FechaDownload))
                                {
                                    Directory.CreateDirectory(Vble.DownloadEntregadas + "\\" + FechaDownload);
                                }

                                if (!Directory.Exists(Vble.DownloadsHechas + "\\" + FechaDownload))
                                {
                                    Directory.CreateDirectory(Vble.DownloadsHechas + "\\" + FechaDownload);
                                }

                                File.Copy(fil.FullName, Vble.ArchivoImportación);

                                if (!File.Exists(Vble.DownloadsHechas + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion))
                                {
                                    File.Move(fil.FullName, Vble.DownloadsHechas + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                }
                                else
                                {
                                    File.Delete(Vble.DownloadsHechas + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                    File.Move(fil.FullName, Vble.DownloadsHechas + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                }


                                File.Delete(fil.FullName);
                            }
                            else if (RBQAS.Checked == true)
                            {
                                if (!Directory.Exists(Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload))
                                {
                                    Directory.CreateDirectory(Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload);
                                }
                                if (!Directory.Exists(Vble.DownloadsHechasPRUEBA + "\\" + FechaDownload))
                                {
                                    Directory.CreateDirectory(Vble.DownloadsHechasPRUEBA + "\\" + FechaDownload);
                                }

                                //Vble.DownloadEntregadas = Vble.DownloadEntregadas + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion;
                                File.Copy(fil.FullName, Vble.ArchivoImportación);

                                if (!File.Exists(Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion))
                                {
                                    //File.Copy(fil.FullName, Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                    File.Move(fil.FullName, Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                }
                                else
                                {
                                    File.Delete(Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                    File.Move(fil.FullName, Vble.DownloadEntregadasPRUEBA + "\\" + FechaDownload + "\\" + Vble.NombreArchivoImportacion);
                                }
                                File.Delete(fil.FullName);
                            }


                        }
                        else
                        {
                            MessageBox.Show("directorio sin archivos", "Error de Archivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    //llama al procedimiento en 2º Plano donde esta el metodo de Downloads 
                    if (File.Exists(Vble.ArchivoImportación))
                    {
                        //evita que el proceso pare porque se llama a un metodo que se declaro fuera
                        //BeginInvoke(new InvokeDelegate(InvokeMethodActivarProgressBar));

                        ///Aca se llama al proceso en segundo plano(que no funciona con mas de un archivo de Imporación)
                        ///Llamo al proceso de leer el archivo directamente( si funciona con mas de un archivo de Importación)
                        //Download2plano.RunWorkerAsync();
                        LeerArchivoImportacion(Vble.ArchivoImportación);

                        Vble.CantImportados = 0;
                        Vble.CantApartados = 0;
                        Vble.ConexBloImp = false;
                        Vble.ImporConex.Clear();
                        Vble.ImporMed.Clear();
                        Vble.ImporPers.Clear();
                        Vble.ImporOrdenesDeLecturas.Clear();
                        //Vble.ImporOrdenesDeLecturas.Clear();

                        ////evita que el proceso pare porque se llama a un metodo que se declaro fuera
                        //BeginInvoke(new InvokeDelegate(InvokeMethodDesactivarProgressBar));
                        ////RespaldaDowload();
                    }
                    else
                    {
                        MessageBox.Show("Disculpe el archivo Download no se encuentra Disponible", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Disculpe existe un problema con el archivo download o el mismo no se encuentra disponible aún, por favor comuniquese con un administrador.",
                                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                File.Delete(Vble.ArchivoImportación);
                if (Vble.CancelarImportacion == true)
                {
                }
                else
                {
                    File.Delete(compartidoconSAP);
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Enviar archivo de importacion al directorio Local", "Error al copiar archivo de Importacion",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists(Vble.ArchivoImportación))
                {
                    File.Delete(Vble.ArchivoImportación);
                }
            }
        }

        //ejecuta esas instrucciones fuera del metodo que le corresponde, por estar dentro de un background
        //no dejaba ejecutarlos, pertenece a la funcion "EnviaraPC"
        public void InvokeMethodDesactivarProgressBar()
        {
            //this.progressBar.Value = 0;
            //this.iTalk_ProgressBar1.Value = 0;
            this.timer1.Enabled = false;
            this.timer1.Stop();
        }


        /// <summary>
        /// Método que desencripta archivo Compartido por SAP para poder contar la cantidad de conexiones 
        /// </summary>
        public void DescencriptarParaContar(string ArchivoGPG, string ArchivoConRuta, string CarpetaContenedora)
        {
            string archivoBTX = "", archivoBAT = "";

            try
            {
                Vble.NombreArchivoImportacion = "";
                string[] lectura = ArchivoGPG.Split('.');

                foreach (var item in lectura)
                {
                    string dato = item;

                    if (dato == "gpg" || dato == "btx")
                    {
                        Vble.NombreArchivoImportacion += "";
                    }
                    else
                    {
                        Vble.NombreArchivoImportacion += dato;
                    }
                }

                archivoBTX = Vble.NombreArchivoImportacion + ".btx";
                archivoBAT = CarpetaContenedora + "\\Desencriptador.bat";

                Vble.lineas = "@echo off \n echo '" + Vble.PassDescrip + "' | gpg --passphrase-fd 0 -o " + archivoBTX + " -d " + ArchivoGPG;
                Vble.CreateInfoCarga(archivoBAT, "Desencriptador.bat", Vble.lineas);

                Process proc = null;
                string _batDir = string.Format(CarpetaContenedora);
                proc = new Process();
                proc.StartInfo.WorkingDirectory = _batDir;
                proc.StartInfo.FileName = "Desencriptador.bat";

                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                proc.Start();

                Thread.Sleep(6000);

                //Recorro los procesos activos hasta que encuentra el de gpgEncrypt e introduce la contraseña
                //para descencriptar el archivo.
                Process[] localAll = Process.GetProcesses();
                foreach (Process item in localAll)
                {
                    if (item.ProcessName == "pinentry")
                    {
                        //Esta condición verifica si la tecla BloqMayus está activada, si lo esta desactiva la mayuscula 
                        //para poder ingresar correctamente el password de descecriptación que se ve mas abajo
                        if (KeyStatus(CAPS_KEY) == true)
                        {
                            PresionarTecla(CAPS_KEY);
                        }
                        else { }
                        SendKeys.SendWait("{RIGHT}");
                        SendKeys.SendWait("9dejulio1718");//password de descencriptación
                        SendKeys.SendWait("{ENTER}");
                    }
                }

                Thread.Sleep(3000);
                proc.Close();

                //File.Delete(CarpetaContenedora + "\\"+ ArchivoGPG);
                //Elimino el archivo bat, el mismo es temporal para descencritar el archivo con extensión.gpg
                File.Delete(archivoBAT);

                //creo el objeto que va a contener la dirección de la carpeta compartida de SAP
                DirectoryInfo CompartidoSAP = new DirectoryInfo(CarpetaContenedora /*+"\\" + archivoBTX*/);

                //realica la suma de todas las conexiones que existen por Unidad de Lectura compartida para 
                //su descarga y almacena esa cantidad de conexiones en vble.TotalConexiones
                foreach (var fi in CompartidoSAP.GetFiles())
                {
                    Vble.TotalConexiones += CantidadConexiones(fi.FullName);
                    File.Delete(fi.FullName);
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + ". Erro al desencriptar el archivo compartido por SAP", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        /// <summary>
        /// Metodo que desencripta archivo Compartido por SAP para poder importar a la base de Datos Mysql Server
        /// </summary>        
        public void DescencriptarArchivo(string ArchivoGPG, string ArchivoConRuta, string CarpetaContenedora)
        {
            string archivoBTX = "", archivoBAT = "";
            try
            {
                Vble.NombreArchivoImportacion = "";
                string[] lectura = ArchivoGPG.Split('.');

                foreach (var item in lectura)
                {
                    string dato = item;
                    if (dato == "gpg" || dato == "btx")
                    {
                        Vble.NombreArchivoImportacion += "";
                    }
                    else
                    {
                        Vble.NombreArchivoImportacion += dato;
                    }
                }
                archivoBTX = Vble.NombreArchivoImportacion + ".btx";
                archivoBAT = CarpetaContenedora + "\\Desencriptador.bat";

                Vble.lineas = "@echo off \n echo " + Vble.PassDescrip + " | gpg --passphrase-fd 0 -o " + archivoBTX + " -d " + ArchivoGPG;
                Vble.CreateInfoCarga(archivoBAT, "Desencriptador.bat", Vble.lineas);

                Process proc = null;
                string _batDir = string.Format(CarpetaContenedora);
                proc = new Process();
                proc.StartInfo.WorkingDirectory = _batDir;
                proc.StartInfo.FileName = "Desencriptador.bat";

                //esconde la ventana de comando para que no interrumpa al usuario
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
                //SendKeys.SendWait("     9deJuli");
                //Thread.Sleep(2000);
                //SendKeys.SendWait("{ENTER}");
                proc.WaitForExit();
                proc.Close();

                if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasSiProcesadas)))
                {
                    Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasSiProcesadas));
                }
                //File.Copy(CarpetaContenedora+"\\"+ ArchivoGPG, Vble.DownloadsHechas + ArchivoGPG);
                File.Copy(CarpetaContenedora + "\\" + ArchivoGPG, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasSiProcesadas) + ArchivoGPG);
                File.Copy(CarpetaContenedora + "\\" + ArchivoGPG, Vble.DownloadEntregadas + ArchivoGPG);
                File.Delete(CarpetaContenedora + "\\" + ArchivoGPG);
                File.Delete(archivoBAT);
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + ". Erro al desencriptar el archivo compartido por SAP", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Copia el archivo compartido desde Servidor NAS de FIS a Carpeta de PC Local y llama a la Funcion 
        /// DescencriptarParaContar
        /// </summary>
        /// <param name="compartidoconSAP"></param>
        /// <param name="archivoimportacion"></param>
        public void EnviarAPCparaContar(string compartidoconSAP, string archivoimportacion)
        {
            int cantarchivo;
            //var Download = new DownloadYUpload();
            try
            {
                File.Copy(compartidoconSAP, Vble.CarpetaImportacion + archivoimportacion);
                DirectoryInfo ImportacionPC = new DirectoryInfo(Vble.CarpetaImportacion);

                //////Reinicio el contador de archivos para verificar que unicamente 
                //////tengo el archivo encriptado que se va a leer
                cantarchivo = 0;
                foreach (var fi in ImportacionPC.GetFiles()) { cantarchivo++; }

                //Si contiene el unico archivo encriptado, llamo al metodo Descencriptar el cual
                //lo descencripta para poder realizar la importación 
                if (cantarchivo == 1)
                {
                    foreach (var fi in ImportacionPC.GetFiles())
                    {
                        DescencriptarParaContar(fi.Name, fi.FullName, fi.DirectoryName);
                    }
                }
                else
                {
                    MessageBox.Show("directorio sin archivos al descencriptar", "Error de Archivos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Enviar archivo de importacion al directorio Local", "Error al copiar archivo de Importacion",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Metodo que contiene la funcion enviar a PC para contar, procedimientos para contar
        /// la cantidad de archivos
        /// </summary>
        private void RealizarImportacionParaContar()
        {
            int NroArchivos = 0;
            //var Download = new DownloadYUpload();
            try
            {
                //creo el objeto que va a contener la direccion de la carpeta compartida de SAP
                DirectoryInfo CompartidoSAP = new DirectoryInfo(Vble.CarpetaSAPImportacion);

                foreach (var fi in CompartidoSAP.GetFiles())
                {
                    NroArchivos++;
                }
                if (NroArchivos > 0)
                {
                    foreach (var fi in CompartidoSAP.GetFiles())
                    {
                        EnviarAPCparaContar(fi.FullName, fi.Name);
                    }

                }
                else
                {
                    MessageBox.Show("Disculpe, aún no se compartió ninguna Ruta para cargar al sistema.",
                                    "Error de existencia de Archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al intentar conectarse a la unidad de red.");
            }
        }

        /// <summary>
        /// Metodo que realiza la importacion, Desencripta el archivo .btx compartido por SAP o en directorio de PRUEBA de FIS,
        /// genera la carga de la base de Datos y vuelve a encriptar el archivo dejando como respaldo en la Carpeta DownloadsHechas 
        /// en la NAS de FIS
        /// </summary>
        private void RealizarImportacion()
        {
            int NroArchivos = 0;
            int ArchImportados = 0;
            try
            {
                //DEPENDE DEL RadioButtom Seleccionado se tomara el archivo de los distintos modulos ya sea:
                //RBPRD (PRODUCCIÓN) indica si se va a tomar el archivo de importación desde la carpeta en donde SAP dejará disponible las rutas
                //RBPRUEBA (PRUEBA) indica si se va a tomar el archivo de importación desde la carpeta PRUEBAS de FIS el cual se utilizara para
                //las distintas pruebas sin afectar al funcionamiento con SAP.
                //RBQAS todavia no está implementado pero sería el modulo de prueba para SAP.
                if (DB.Entorno == "PRD")
                {
                    //creo el objeto que va a contener la direccion de la carpeta compartida de SAP
                    DirectoryInfo CompartidoSAP = new DirectoryInfo(Vble.CarpetaSAPImportacion);
                    Vble.ArrayRutasImportadas.Clear();
                    foreach (var fi in CompartidoSAP.GetFiles())
                    {
                        NroArchivos++;
                    }
                    if (NroArchivos > 0)
                    {
                        foreach (var fi in CompartidoSAP.GetFiles())
                        {
                            if (Vble.IdentificarArchImport(fi.Name) == true)
                            {
                                //MessageBox.Show("PERTENECE");
                                EnviarAPC(fi.FullName, fi.Name);
                                ArchImportados++;
                                Vble.ArrayRutasImportadas.Add(fi.FullName);
                            }
                            else
                            {
                                NroArchivos = 0;
                            }
                        }

                        Vble.ExistenArchImportacion = Vble.ArrayRutasImportadas.Count > 0 ? true : false;
                        //cboPeriodo.Items.Add((Vble.Periodo).ToString("0000-00"));
                        //Download.progressBar.Visible = false;
                        //Download.progressBar.Value = 0;
                        //Download.timer1.Enabled = false;
                        //Download.Hide();
                        BeginInvoke(new InvokeDelegate(InvoketerminarProgressBar));
                        //MessageBox.Show("La importanción se realizo correctamente", "Importación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Disculpe, aún no se compartio ninguna Ruta para cargar al sistema.",
                                        "Error de existencia de Archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Vble.CancelarImportacion = true;
                    }
                }
                else if (DB.Entorno == "QAS")
                {
                    //creo el objeto que va a contener la direccion de la carpeta compartida de SAP pruebas que estara dentro de FIS-PRUEBAS
                    DirectoryInfo CompartidoSAP = new DirectoryInfo(Vble.CarpetaSAPImportacionPRUEBA);
                    foreach (var fi in CompartidoSAP.GetFiles())
                    {
                        NroArchivos++;
                    }
                    if (NroArchivos > 0)
                    {
                        foreach (var fi in CompartidoSAP.GetFiles())
                        {
                            if (Vble.IdentificarArchImport(fi.Name) == true)
                            {
                                //MessageBox.Show("PERTENECE");
                                EnviarAPC(fi.FullName, fi.Name);
                                ArchImportados++;
                            }
                            else
                            {
                                NroArchivos = 0;
                            }
                        }

                        Vble.ExistenArchImportacion = Vble.ArrayRutasImportadas.Count > 0 ? true : false;
                        //cboPeriodo.Items.Add((Vble.Periodo).ToString("0000-00"));
                        //Download.progressBar.Visible = false;
                        //Download.progressBar.Value = 0;
                        //Download.timer1.Enabled = false;
                        //Download.Hide();
                        BeginInvoke(new InvokeDelegate(InvoketerminarProgressBar));
                        //MessageBox.Show("La importanción se realizo correctamente", "Importación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Disculpe, aún no se compartio ninguna Ruta para cargar al sistema.",
                                        "Error de existencia de Archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Vble.CancelarImportacion = true;
                    }
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al realizar la importación, comuniquese con el administrador de la aplicación.");
            }

        }

        /// <summary>
        /// metodo que realiza esas instrucciones fuera de la funcion donde se tendría que realizar,
        /// pertenecen a la funcion "Realizar Importación"
        /// </summary>
        public void InvoketerminarProgressBar()
        {
            this.progressBar.Visible = false;
            this.PorcLabel.Visible = false;
            this.label1.Visible = false;
            this.label2.Visible = false;
            ////this.iTalk_ProgressBar1.Visible = false;
            ////this.iTalk_ProgressBar1.Value = 0;

        }



        /// <summary>
        /// Descencriptar el archivo compartido por SAP para contar cuantas conexiones existen 
        /// y almacena en una variable Vble.TotalConexiones para usarlo como propiedad de cantidad 
        /// del procedimiento BackGroundWorker en el que se realiza la Carga de Datos al MySQL general
        /// </summary>
        private void ContarCantidadDeConexiones()
        {

            #region Usar para Importar cuando este conectado a la NAS
            ///// Para importar desde la NAS accedo a traves de la red
            Vble.TotalConexiones = 0;
            Vble.AbrirUnidadDeRed(@"R:", @"\\10.1.3.125\DOWNLOAD");

            //UnidadRedUtil ConectarUnidadDeRed = new UnidadRedUtil();
            //ConectarUnidadDeRed.MapResource("K:", "\\\\10.1.3.125\\DOWNLOAD");

            RealizarImportacionParaContar();

            //ConectarUnidadDeRed.UnMapResource("K:");
            Vble.CerrarUnidadDeRed();
            #endregion

            #region Usar para Importar cuando no esta conectado a la NAS y el archivo esta en ubicacion local
            ///En caso de que no pueda importar desde la NAS
            ///creo el objeto que va a contener la direccion de la carpeta compartida de SAP

            //int NroArchivos = 0;
            //DirectoryInfo CompartidoSAP = new DirectoryInfo(Vble.CarpetaImportacion);
            //foreach (var fi in CompartidoSAP.GetFiles())
            //{
            //    NroArchivos++;
            //}

            //if (NroArchivos >= 1)
            //{
            //    foreach (var fi in CompartidoSAP.GetFiles())
            //    {
            //        Vble.TotalConexiones += CantidadConexiones(fi.FullName);
            //    }

            //}
            #endregion

        }



        /// <summary>
        /// Envio una copia del archivo Download a la carpeta Respaldos antes de eliminar del compartido con SAP
        /// </summary>
        private void RespaldaDowload()
        {
            try
            {
                //Lee y obtiene el nombre del archivo download                        
                StringBuilder stb1 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "ArchDownload", "", stb1, 100, Ctte.ArchivoIniName);
                string archivo = stb1.ToString();

                Char delimiter = '\\';

                String[] substrings = Vble.ArchivoImportación.Split(delimiter);

                for (int i = 0; i < substrings.Length; i++)
                {
                    if (substrings[i].Length > 2)
                    {
                        if (substrings[i].Substring(0, 3) == archivo)
                        {
                            Vble.CopiaArchivos(Vble.ArchivoImportación, Vble.CarpetaRespaldo + "\\Downloads\\" + substrings[i]);
                        }
                    }
                }
                //File.Delete(Vble.CarpetaImportacion);
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al realizar respaldo de download");
            }
        }


        /// <summary>
        /// Lee Linea por Linea el archivo Importacion que se recibe como parametro para determinar las cantidades de conexiones
        /// que contienen el archivo el cual se identifica con el comienzo de linea "HCX"
        /// </summary>
        /// <param name="Ruta"></param>
        /// <returns></returns>
        public static int CantidadConexiones(string registro)
        {
            int cantidad = 0;
            try
            {
                using (StreamReader sr = new StreamReader(registro))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        String value = line;
                        Char delimiter = '|';
                        String[] substrings = value.Split(delimiter);


                        for (int i = 0; i < substrings.Length; i++)
                        {

                            switch (substrings[i])
                            {
                                case "HCX":

                                    cantidad++;
                                    break;
                            }
                        }
                    }
                    return cantidad;
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al contar la cantidad de conexiones");
            }
            return cantidad;
        }


        /// <summary>
        /// verifica si la conexion con el periodo existen en la base de datos como exportados 6XX devuelve 0
        /// </summary>
        /// <param name="ValorID"></param>
        /// <param name="Periodo"></param>
        /// <param name="ColumnNameID"></param>
        /// <returns></returns>
        public static int ControlarImpresionOBS(int ValorID, int Periodo, string ColumnNameID)
        {
            DataTable Tabla = new DataTable();
            int valoradevolver = 0;
            int ImpresionOBS;
            string txSQL = "SELECT ConexionID, ImpresionOBS FROM conexiones WHERE " + ColumnNameID + " = " + ValorID + " AND Periodo = " + Periodo;
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            Form1Inicio Inicio = new Form1Inicio();
            if (Tabla.Rows.Count == 1)
            {
                foreach (DataRow item in Tabla.Rows)
                {

                    ImpresionOBS = item.Field<int>("ImpresionOBS");

                    //|| ImpresionOBS == 601 || ImpresionOBS == 603 || ImpresionOBS == 604 || ImpresionOBS == 605 ||
                    //    ImpresionOBS == 606 || ImpresionOBS == 607 || ImpresionOBS == 608 || ImpresionOBS == 609 || ImpresionOBS == 610 ||
                    //    ImpresionOBS == 611 || ImpresionOBS == 612 || ImpresionOBS == 613 || ImpresionOBS == 614 || ImpresionOBS == 619 ||
                    //    ImpresionOBS == 620 || ImpresionOBS == 621 || ImpresionOBS == 626 || ImpresionOBS == 699
                    // 
                    if (ImpresionOBS == 0 || ImpresionOBS == 600 || ImpresionOBS == 300 || ImpresionOBS == 400 || ImpresionOBS == 500)
                    {
                        valoradevolver = 0;
                    }
                    else
                    {
                        valoradevolver = 1;
                    }
                }

            }
            else
            {
                valoradevolver = 1;
            }
            datosAdapter.Dispose();
            comandoSQL.Dispose();
            Tabla.Dispose();
            return valoradevolver;
        }

        /// <summary>
        /// Lee Linea por Linea el archivo Importacion que se recibe como parametro y cuando identifica cada linea 
        /// por la denominacion a la que corresponde cada tabla, almacena en un string y lo pasa por parametro
        /// a cada funcion correspondiente en donde se agrega el registro recibido por parametro en la tabla correspondiente 
        /// </summary>
        /// <param name="Ruta"></param>
        /// <returns></returns>
        public void LeerArchivoImportacion(string registro)
        {
            string DistRuta = "";
            //int i = 0; //incrementara en 1 para almacenar datos en variableXX
            int k = 1;
            StringBuilder stb = new StringBuilder();
            //Leer y actualizar el número de Descarga
            Inis.GetPrivateProfileString("NdeLote", "Lote", "0", stb, 50, Ctte.ArchivoIniName);
            Vble.Lote = int.Parse(stb.ToString()) + 1;
            Inis.WritePrivateProfileString("NdeLote", "Lote", Vble.Lote.ToString().Trim(), Ctte.ArchivoIniName);

            using (StreamReader sr = new StreamReader(registro))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    String value = line;
                    Char delimiter = '|';
                    String[] substrings = value.Split(delimiter);
                    //recorre linea por linea separando cada dato del regitro el cual se encuentra dividido por "|" y lo almacena en un 
                    //vector string para luevo enviarlo al procedimiento correspondiente de cada tabla
                    for (int i = 0; i < substrings.Length; i++)
                    {
                        switch (substrings[i])
                        {
                            case "HCX":
                                //MessageBox.Show("carga tabla conexiones");
                                //MessageBox.Show("Conexiones: "+ substrings);                      
                                if (ImportarTablaConex(substrings, k) == 0)
                                {
                                    Download2plano.ReportProgress(k);
                                    k++;
                                    break;
                                }
                                else
                                {
                                    Vble.CancelarImportacion = true;
                                    sr.Dispose();
                                    File.Delete(Vble.ArchivoImportación);
                                    return;
                                }

                            case "HPS":
                                //MessageBox.Show("carga tabla Personas");                               
                                ImportarTablaPersonas(substrings, k);
                                //MessageBox.Show(substrings[i].IndexOf("'").ToString());
                                //if (substrings[i].IndexOf("'") == 1)
                                //{
                                //}
                                substrings[i].Replace("'", "");
                                break;
                            case "HMD":
                                //MessageBox.Show("carga tabla medidores");                               
                                ImportarTablaMedidores(substrings, k);
                                break;
                            //case "HCD":
                            //    ImportaTablaConceptosDatos(substrings);
                            //    break;
                            //case "HTX":
                            //    ImportarTablaTextosVarios(substrings);
                            //    break;
                            case "HFS": //Aca importara la tabla donde viene el numero de orden de factura y OPBEL 
                                ImportarOrdenesLectura(substrings, k);
                                break;
                        }
                    }
                }
                Vble.ConexBloImp = false;
                sr.Close();
                sr.Dispose();
            }

            FileInfo file = new FileInfo(registro);
            string porc = Vble.Remesa.ToString() + file.Name.Substring(17, file.Name.Substring(17).IndexOf("_"));
            //string zonaInNameFile = file.Name.Substring(17, file.Name.Substring(17).IndexOf("-"));
            int zona = 0;
            if (file.Name.ToString().Contains("-"))
            {
                zona =Convert.ToInt16(file.Name.Substring(17, file.Name.Substring(17).IndexOf("-")));
            }
            else
            {
                zona = 8888;
            }
            
        
            //if (Vble.PanelLogImp.ToString() == "1")
            //{
            //    InfoImportacion.Visible = true;                
            //    ListViewItem ResumenImportacion;
            //    ResumenImportacion = new ListViewItem(file.Name.Substring(17, file.Name.Substring(17).IndexOf("_")));
            //    ResumenImportacion.SubItems.Add(Vble.TotalConexiones.ToString());
            //    ResumenImportacion.SubItems.Add(Vble.CantImportados.ToString());
            //    ResumenImportacion.SubItems.Add(Vble.CantApartados.ToString());
            //    LVResImpor.Items.Add(ResumenImportacion);
            //}

            if (Vble.PanelLogImp.ToString() == "1")
            {
                AgregarApartadosALogImportacion(Vble.Periodo, zona, porc, Vble.TotalConexiones, Vble.CantImportados,
                                                Vble.CantApartados, Vble.ImporConex, Vble.ImporPers, Vble.ImporOrdenesDeLecturas, Vble.ImporMed);
            }
            Vble.ConexBloImp = false;
            Vble.ImporConex.Clear();
            Vble.ImporMed.Clear();
            Vble.ImporPers.Clear();
            Vble.ImporOrdenesDeLecturas.Clear();
        }

        /// <summary>
        /// Metodo que recibe los parametros necesarios para agregar en la tabla LogImportacion si la ruta que se importa
        /// tiene algunos usuarios que no cumplen con los campos necesarios y obligatorios para ser registrados en la base
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="porcion"></param>
        /// <param name="totalConexiones"></param>
        /// <param name="cantImportados"></param>
        /// <param name="cantApartados"></param>
        /// <param name="imporConex"></param>
        /// <param name="imporPers"></param>
        /// <param name="imporMed"></param>
        private void AgregarApartadosALogImportacion(int periodo, int zona, string porcion, int totalConexiones,
                                                     int cantImportados, int cantApartados, ArrayList imporConex,
                                                     ArrayList imporPers, ArrayList imporOrdLec, ArrayList imporMed)
        {
            try
            {
                string DetalleApartados = " ";

                if (imporConex.Count > 0 || imporPers.Count > 0 || imporMed.Count > 0 || imporOrdLec.Count > 0)
                {
                    //Recorro el arraylist de Conexiones que se apartaron y agrego al string detalleApartados
                    for (int i = 0; i < Vble.ImporConex.Count; i++)
                    {
                        if (Vble.ImporConex[i].ToString() != "")
                        {
                            DetalleApartados += Vble.ImporConex[i].ToString() + "; ";
                        }

                    }
                    //Recorro el arraylist de Personas que se apartaron y agrego al string detalleApartados
                    for (int i = 0; i < Vble.ImporPers.Count; i++)
                    {
                        if (Vble.ImporPers[i].ToString() != "")
                        {
                            DetalleApartados += Vble.ImporPers[i].ToString() + "; ";
                        }

                    }
                    //Recorro el arraylist de Medidores que se apartaron y agrego al string detalleApartados
                    for (int i = 0; i < Vble.ImporMed.Count; i++)
                    {
                        if (Vble.ImporMed[i].ToString() != "")
                        {
                            DetalleApartados += Vble.ImporMed[i].ToString() + "; ";
                        }

                    }
                    //Recorro el arraylist de Medidores que se apartaron y agrego al string detalleApartados
                    for (int i = 0; i < Vble.ImporOrdenesDeLecturas.Count; i++)
                    {
                        if (Vble.ImporOrdenesDeLecturas[i].ToString() != "")
                        {
                            DetalleApartados += Vble.ImporOrdenesDeLecturas[i].ToString() + "; ";
                        }

                    }
                }

                string InsertLogImp = "INSERT INTO logimportacion(Periodo, Zona, Porcion, CantUsuarios, CantImportados, CantApartados, " +
                                "DetalleApartados, Operario, FechaImportacion, HoraImportacion)" +
                                " VALUES (" + periodo + ", " + zona + ", '" + porcion + "', " + totalConexiones + ", " + cantImportados + ", " +
                                            cantApartados + ", '" + DetalleApartados + "', '" + DB.sDbUsu + "', '" +
                                            DateTime.Today.Date.ToString("yyyy-MM-dd") + "', '" + DateTime.Now.ToString("hh:mm:ss") + "')";

                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(InsertLogImp, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();

            }
            catch (Exception er)
            {
                //MessageBox.Show(r.Message + "Error al cargar registros de MEDIDORES");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + er.Message +
                                              " Error en el metodo AgregarApartadosALogImportacion. Proceso - Importar Ruta." +
                                              "\n");

            }


        }






        #region Importación de datos a tablas de BASE MySqlServer
        /// <summary>
        /// Recibo como parametro la linea HCX leida que contiene campos de la tabla conexiones y agrego el registro a la tabla
        /// </summary>
        /// <param name="substrings"></param>
        public static int ImportarTablaConex(string[] substrings, int k)
        {
            int i = 0;
            Int32 Conexionid = 0;

            try
            {
                //IFormatProvider culture = new System.Globalization.CultureInfo("es-AR", true);  
                Int32 Contrato, usuarioID, tituIarID, propietarioID, ImpresionCOD, Zona, Ruta, Secuencia, Remesa, CodigoAlumbrado,
                      ConsumoControl, DocumPago1, DocumPago2;
                string Instalacion, FechaCalP, CuentaDebito, TarifaCOD, TipoProrrateo, CESPnumero;
                double PromedioDiario;
                DateTime Vencimiento1, Vencimiento2, VencimientoProx, CESPvencimiento;
                Conexionid = Convert.ToInt32(substrings[1]);
                if (substrings.Length == 56)
                {
                    //cuando venga el txt correcto sin el campo Personas ID reordenar la asignación de indices eliminando PersonaID y corriendo 
                    //los indices de los demás
                    //agregar CodigoAlumbrado
                    Conexionid = Convert.ToInt32(substrings[1]);// ConexionID
                    Vble.ConexionID = Conexionid;
                    //PersonaID = Convert.ToInt32(substrings[2]);//
                    Vble.CondIVA = Convert.ToInt16(substrings[2]);//Condicion Iba
                    Vble.Periodo = Convert.ToInt32(substrings[3]);// Periodo
                    Contrato = Convert.ToInt32(substrings[4]);// Contrato
                    FechaCalP = substrings[5];// FechaCalP fecha calculo planificada

                    Instalacion = (substrings[1]);//

                    if (substrings[6] != "")
                    {
                        tituIarID = Convert.ToInt32(substrings[6]);
                        usuarioID = 0;
                        propietarioID = 0;
                    }
                    else
                    {
                        tituIarID = Convert.ToInt32(substrings[7]);
                        usuarioID = 0;
                        propietarioID = 0;
                    }

                    //substrings[6] = substrings[6] == "" ? "0" : substrings[6];
                    //usuarioID = Convert.ToInt32(substrings[6]);//

                    //substrings[7] = substrings[7] == "" ? "0" : substrings[7];
                    //tituIarID = Convert.ToInt32(substrings[7]);//

                    //substrings[8] = substrings[8] == "" ? "0" : substrings[8];
                    //propietarioID = Convert.ToInt32(substrings[8]);//

                    substrings[9] = substrings[9] == "" ? "0" : substrings[9];
                    Vble.DomicSumin = substrings[9].Replace("'", "");// Domicilio Suministro
                    Vble.DomicSumin.Replace(",", "");

                    substrings[10] = substrings[10] == "" ? "0" : substrings[10];
                    Vble.LocalidadSumin = substrings[10];// Localidad suministro (puede ser Codigo o Nombre)

                    substrings[11] = substrings[11] == "" ? "0" : substrings[11];
                    Vble.CodPostalSumin = substrings[11];// Codigo Postal Suministro

                    substrings[12] = substrings[12] == "" ? "0" : substrings[12];
                    CuentaDebito = substrings[12];// Cuenta Debito

                    substrings[13] = substrings[13] == "" ? "0" : substrings[13];
                    ImpresionCOD = Convert.ToInt32(substrings[13]);//ImpresionCOD = 0 "Indicado Impresion"
                                                                   //ImpresionCOD = 1 "Indicado NO Impresion"
                                                                   //ImpresionCOD = 3 "Indicado TELE LECTURA CONSUMIDOR con impresion"
                                                                   //ImpresionCOD = 3 "Indicado TELE LECTURA PROSUMIDOR con impresion"
                                                                   //ImpresionCOD = 5 "usuario bloqueado para impresion"
                                                                   //ImpresionCOD = 6 "Prosumidor sin impresion de factura"
                                                                   //ImpresionCOD = 7 "Tele lectura sin impresion defactura"



                    //substrings[14] = substrings[14] == ""? "0": substrings[14];
                    //Lote =  Convert.ToInt32(substrings[14]);//

                    substrings[15] = substrings[15] == "" ? "0" : substrings[15];
                    Zona = Convert.ToInt32(substrings[15]);// Zona, codigo de localidad

                    substrings[16] = substrings[16] == "" ? "0" : substrings[16];
                    i = substrings[16].IndexOf("-");
                    substrings[16] = i < 0 ? substrings[16] : substrings[16].Substring(i + 1);
                    Ruta = Convert.ToInt32(substrings[16]);// numero de ruta, con formato de unidad de lectura zona-ruta

                    substrings[17] = substrings[17] == "" ? "0" : substrings[17];
                    Secuencia = Convert.ToInt32(substrings[17]);// numero de aparicion en la ruta, generalmente es de forma continua 1, 2, 3.. etc

                    substrings[18] = substrings[18] == "" || Regex.IsMatch(substrings[18], "^[a-zA-Z]*$") ? "9" : substrings[18];
                    Remesa = Convert.ToInt32(substrings[18]);// numero de remesa en que se encuentra el usuario
                    Vble.Remesa = Remesa;

                    substrings[19] = substrings[19] == "" ? "0" : substrings[19];
                    TarifaCOD = substrings[19];// Codigo de tarifa

                    substrings[20] = substrings[20] == "" ? "0" : substrings[20];
                    CodigoAlumbrado = Convert.ToInt32(substrings[20]);//Codigo de alumbrado


                    substrings[21] = substrings[21] == "" ? "0" : substrings[21];
                    TipoProrrateo = substrings[21];//Tipo Prorrateo 

                    substrings[22] = substrings[22] == "" ? "0" : substrings[22];
                    ConsumoControl = Convert.ToInt32(substrings[22]);
                    //int num = i;
                    //int band = Convert.ToInt32(substrings[22].Substring(i + 1, 1)) >= 5 ? 1 : 0;
                    //ConsumoPromedio = band == 1 ? Convert.ToInt32(substrings[22].Substring(0, i)) + 1 : Convert.ToInt32(substrings[22].Substring(0, i));


                    substrings[23] = substrings[23] == "" ? "0" : substrings[23];
                    PromedioDiario = Convert.ToDouble(substrings[23], CultureInfo.CreateSpecificCulture("en-US"));//           

                    substrings[24] = substrings[24] == "" ? "0" : substrings[24];
                    Vble.ConsumoResidual = Convert.ToInt32(substrings[24]);//

                    substrings[25] = substrings[25] == "" ? "0" : substrings[25];
                    CESPnumero = substrings[25];//       

                    substrings[26] = substrings[26] == "" ? "01/01/2000" : substrings[26];
                    //substrings[26] = substrings[26] == "" ? "01/01/2000" : substrings[26];
                    //CESPvencimiento = DateTime.Parse(substrings[26], culture, System.Globalization.DateTimeStyles.AssumeLocal);//
                    CESPvencimiento = DateTime.Parse(substrings[26]);//

                    substrings[27] = substrings[27] == "" ? "0" : substrings[27];
                    DocumPago1 = Convert.ToInt32(substrings[27]);//Documento Pago1

                    substrings[28] = substrings[28] == "" ? "01/01/2000" : substrings[28];
                    //Vencimiento1 = DateTime.Parse(substrings[28], culture, System.Globalization.DateTimeStyles.AssumeLocal);//  
                    Vencimiento1 = DateTime.Parse(substrings[28]);//  

                    substrings[29] = substrings[29] == "" ? "0" : substrings[29];
                    DocumPago2 = Convert.ToInt32(substrings[29]);//Documento Pago2

                    substrings[30] = substrings[30] == "" ? "01/01/2000" : substrings[30];
                    //Vencimiento2 = DateTime.Parse(substrings[30], culture, System.Globalization.DateTimeStyles.AssumeLocal);//       
                    Vencimiento2 = DateTime.Parse(substrings[30]);// 

                    substrings[31] = substrings[31] == "" ? "01/01/2000" : substrings[31];
                    //VencimientoProx = DateTime.Parse(substrings[31], culture, System.Globalization.DateTimeStyles.AssumeLocal);//
                    VencimientoProx = DateTime.Parse(substrings[31]);//

                    substrings[32] = substrings[32] == "" ? "0" : substrings[32];
                    Vble.HistoPeriodo01 = Convert.ToInt32(substrings[32]);
                    substrings[33] = substrings[33] == "" ? "0" : substrings[33];
                    i = substrings[33].IndexOf(".");
                    substrings[33] = i < 0 ? substrings[33] : substrings[33].Substring(0, i);
                    Vble.HistoConsumo01 = Convert.ToInt32(substrings[33]);

                    substrings[34] = substrings[34] == "" ? "0" : substrings[34];
                    Vble.HistoPeriodo02 = Convert.ToInt32(substrings[34]);
                    substrings[35] = substrings[35] == "" ? "0" : substrings[35];
                    i = substrings[35].IndexOf(".");
                    substrings[35] = i < 0 ? substrings[35] : substrings[35].Substring(0, i);
                    Vble.HistoConsumo02 = Convert.ToInt32(substrings[35]);

                    substrings[36] = substrings[36] == "" ? "0" : substrings[36];
                    Vble.HistoPeriodo03 = Convert.ToInt32(substrings[36]);
                    substrings[37] = substrings[37] == "" ? "0" : substrings[37];
                    i = substrings[37].IndexOf(".");
                    substrings[37] = i < 0 ? substrings[37] : substrings[37].Substring(0, i);
                    Vble.HistoConsumo03 = Convert.ToInt32(substrings[37]);

                    substrings[38] = substrings[38] == "" ? "0" : substrings[38];
                    Vble.HistoPeriodo04 = Convert.ToInt32(substrings[38]);
                    substrings[39] = substrings[39] == "" ? "0" : substrings[39];
                    i = substrings[39].IndexOf(".");
                    substrings[39] = i < 0 ? substrings[39] : substrings[39].Substring(0, i);
                    Vble.HistoConsumo04 = Convert.ToInt32(substrings[39]);

                    substrings[40] = substrings[40] == "" ? "0" : substrings[40];
                    Vble.HistoPeriodo05 = Convert.ToInt32(substrings[40]);
                    substrings[41] = substrings[41] == "" ? "0" : substrings[41];
                    i = substrings[41].IndexOf(".");
                    substrings[41] = i < 0 ? substrings[41] : substrings[41].Substring(0, i);
                    Vble.HistoConsumo05 = Convert.ToInt32(substrings[41]);

                    substrings[42] = substrings[42] == "" ? "0" : substrings[42];
                    Vble.HistoPeriodo06 = Convert.ToInt32(substrings[42]);
                    substrings[43] = substrings[43] == "" ? "0" : substrings[43];
                    i = substrings[43].IndexOf(".");
                    substrings[43] = i < 0 ? substrings[43] : substrings[43].Substring(0, i);
                    Vble.HistoConsumo06 = Convert.ToInt32(substrings[43]);

                    substrings[44] = substrings[44] == "" ? "0" : substrings[44];
                    Vble.HistoPeriodo07 = Convert.ToInt32(substrings[44]);
                    substrings[45] = substrings[45] == "" ? "0" : substrings[45];
                    i = substrings[45].IndexOf(".");
                    substrings[45] = i < 0 ? substrings[45] : substrings[45].Substring(0, i);
                    Vble.HistoConsumo07 = Convert.ToInt32(substrings[45]);

                    substrings[46] = substrings[46] == "" ? "0" : substrings[46];
                    Vble.HistoPeriodo08 = Convert.ToInt32(substrings[46]);
                    substrings[47] = substrings[47] == "" ? "0" : substrings[47];
                    i = substrings[47].IndexOf(".");
                    substrings[47] = i < 0 ? substrings[47] : substrings[47].Substring(0, i);
                    Vble.HistoConsumo08 = Convert.ToInt32(substrings[47]);

                    substrings[48] = substrings[48] == "" ? "0" : substrings[48];
                    Vble.HistoPeriodo09 = Convert.ToInt32(substrings[48]);
                    substrings[49] = substrings[49] == "" ? "0" : substrings[49];
                    i = substrings[49].IndexOf(".");
                    substrings[49] = i < 0 ? substrings[49] : substrings[49].Substring(0, i);
                    Vble.HistoConsumo09 = Convert.ToInt32(substrings[49]);

                    substrings[50] = substrings[50] == "" ? "0" : substrings[50];
                    Vble.HistoPeriodo10 = Convert.ToInt32(substrings[50]);
                    substrings[51] = substrings[51] == "" ? "0" : substrings[51];
                    i = substrings[51].IndexOf(".");
                    substrings[51] = i < 0 ? substrings[51] : substrings[51].Substring(0, i);
                    Vble.HistoConsumo10 = Convert.ToInt32(substrings[51]);

                    substrings[52] = substrings[52] == "" ? "0" : substrings[52];
                    Vble.HistoPeriodo11 = Convert.ToInt32(substrings[52]);
                    substrings[53] = substrings[53] == "" ? "0" : substrings[53];
                    i = substrings[53].IndexOf(".");
                    substrings[53] = i < 0 ? substrings[53] : substrings[53].Substring(0, i);
                    Vble.HistoConsumo11 = Convert.ToInt32(substrings[53]);


                    substrings[54] = substrings[54] == "" ? "0" : substrings[54];
                    Vble.HistoPeriodo12 = Convert.ToInt32(substrings[54]);
                    substrings[55] = substrings[55] == "" ? "0" : substrings[55];
                    i = substrings[55].IndexOf(".");
                    substrings[55] = i < 0 ? substrings[55] : substrings[55].Substring(0, i);
                    Vble.HistoConsumo12 = Convert.ToInt32(substrings[55]);


                    //los campos que quedan por defecto al realizar la importacion son:
                    //ImpresionOBS; impresionCant; Operario; SubCategoria; || ConsumoFacturado; ConsumoTipo; OrdenTomado; CESPvencimiento
                    //FacturaLetra; PuntoVenta; FacturaNro1; Importe1, ImporteBasico1, ImporteImpuesto1, FacturaNro2,
                    //Importe2, ImporteBasico2, ImporteImpuesto2, 

                    //if (VerificarExistencia("Conexiones", "ConexionID", Conexionid, Vble.Periodo) == 0)
                    if (VerificarExistencia("conexiones", "ConexionID", Conexionid, Vble.Periodo) == 0)
                    {

                        string Insert = "INSERT INTO Conexiones(ConexionID, Periodo, FechaCalP, Contrato, Instalacion, titularID, usuarioID, propietarioID, DomicSumin, LocalidadSumin," +
                                " CodPostalSumin, CuentaDebito, ImpresionCOD," +
                                " Lote, Zona, Ruta, Secuencia, Remesa, TarifaCod, ConsumoControl, ConsumoResidual," +
                                " VencimientoProx)" +
                                " VALUES (" + Conexionid + ", " + Vble.Periodo + ", " + FechaCalP + ", " + Contrato + ", '" + Instalacion + "', " + tituIarID + ", " + usuarioID +
                                ", " + propietarioID + ", '" + Vble.DomicSumin + "', '" + Vble.LocalidadSumin + "', " + Vble.CodPostalSumin + ", " + CuentaDebito + ", " + ImpresionCOD +
                                ", " + Vble.Lote + ", " + Zona + ", " + Ruta + ", " + Secuencia + ", " + Remesa + ", '" + TarifaCOD + "', " + ConsumoControl + ", " + Vble.ConsumoResidual +
                                ", '" + VencimientoProx.ToString("dd/MM/yyyy") + "')";

                        //preparamos la cadena pra insercion
                        MySqlCommand command = new MySqlCommand(Insert, DB.conexBD);

                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();

                        //MessageBox.Show("Se agrego la Conexion Nº: " + Conexionid);
                        //string insertInfoConex = "INSERT INTO infoconex (ConexionID) VALUES (" + Conexionid + ")";
                        if (VerificarExistencia("infoconex", "ConexionID", Conexionid, Vble.Periodo) == 0)
                        {
                            string insertInfoConex = "INSERT INTO infoconex (ConexionID, Periodo) VALUES (" + Conexionid + ", " + Vble.Periodo + ")";
                            command = new MySqlCommand(insertInfoConex, DB.conexBD);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }



                        //if (VerificarExistencia("Historial", "ConexionID", Conexionid, Vble.Periodo) == 0)
                        if (VerificarExistencia("historial", "ConexionID", Conexionid, Vble.Periodo) == 0)
                        {
                            string insertHistorial = "INSERT INTO Historial (ConexionID, Periodo, HistoPeriodo00," +
                            " HistoConsumo00, HistoPeriodo01, HistoConsumo01, HistoPeriodo02, HistoConsumo02, HistoPeriodo03, HistoConsumo03," +
                            " HistoPeriodo04, HistoConsumo04, HistoPeriodo05, HistoConsumo05, HistoPeriodo06, HistoConsumo06, HistoPeriodo07," +
                            " HistoConsumo07, HistoPeriodo08, HistoConsumo08, HistoPeriodo09, HistoConsumo09, HistoPeriodo10, HistoConsumo10," +
                            " HistoPeriodo11, HistoConsumo11) VALUES (" + Conexionid + ", " + Vble.Periodo + ", '" + Vble.HistoPeriodo01 + "', '" + Vble.HistoConsumo01 + "', '" + Vble.HistoPeriodo02 + "', '" + Vble.HistoConsumo02 + "', '" + Vble.HistoPeriodo03 + "', '" + Vble.HistoConsumo03 +
                            "', '" + Vble.HistoPeriodo04 + "', '" + Vble.HistoConsumo04 + "', '" + Vble.HistoPeriodo05 + "', '" + Vble.HistoConsumo05 + "', '" + Vble.HistoPeriodo06 + "', '" + Vble.HistoConsumo06 +
                            "', '" + Vble.HistoPeriodo07 + "', '" + Vble.HistoConsumo07 + "', '" + Vble.HistoPeriodo08 + "', '" + Vble.HistoConsumo08 + "', '" + Vble.HistoPeriodo09 + "', '" + Vble.HistoConsumo09 +
                            "', '" + Vble.HistoPeriodo10 + "', '" + Vble.HistoConsumo10 + "', '" + Vble.HistoPeriodo11 + "', '" + Vble.HistoConsumo11 + "', '" + Vble.HistoPeriodo12 + "', '" + Vble.HistoConsumo12 + "')";
                            command = new MySqlCommand(insertHistorial, DB.conexBD);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }

                        //if (VerificarDomicNoPrinter("NoImprimir", "ConexionID", Conexionid, "Periodo", Vble.Periodo) == 1)
                        //{
                        //    ActualizarImpresionCOD(Conexionid);
                        //}

                        return 0;

                    }
                    else
                    {

                        if (ControlarImpresionOBS(Conexionid, Vble.Periodo, "ConexionID") == 0)
                        {
                            //ActualizarDatosEnConexiones(Conexionid, Contrato, Instalacion, usuarioID, tituIarID, propietarioID, ImpresionCOD, Zona, Ruta, Secuencia,
                            //                       Remesa, CodigoAlumbrado, ConsumoPromedio, ConsumoResidual, DocumPago1, DocumPago2, HistoPeriodo01, HistoConsumo01,
                            //                       HistoPeriodo02, HistoConsumo02, HistoPeriodo03, HistoConsumo03, HistoPeriodo04, HistoConsumo04, HistoPeriodo05,
                            //                       HistoConsumo05, HistoPeriodo06, HistoConsumo06, HistoPeriodo07, HistoConsumo07, HistoPeriodo08, HistoConsumo08,
                            //                       HistoPeriodo09, HistoConsumo09, HistoPeriodo10, HistoConsumo10, HistoPeriodo11, HistoConsumo11, HistoPeriodo12, HistoConsumo12,
                            //                       FechaCalP, DomicSumin, BarrioSumin, CodPostalSumin, CuentaDebito, Categoria, TipoProrrateo, CESPnumero, PromedioDiario,
                            //                       Vencimiento1, Vencimiento2, VencimientoProx, CESPvencimiento);
                            ActualizarDatosEnConexiones(Conexionid, FechaCalP, Contrato, Instalacion, usuarioID, tituIarID, propietarioID, ImpresionCOD, Zona, Ruta, Secuencia,
                                                 Remesa, ConsumoControl, Vble.ConsumoResidual, Vble.DomicSumin, Vble.LocalidadSumin, Vble.CodPostalSumin, CuentaDebito, TarifaCOD, PromedioDiario,
                                                 VencimientoProx);

                            //MessageBox.Show("Se agrego la Conexion Nº: " + Conexionid);
                            //string insertInfoConex = "INSERT INTO infoconex (ConexionID) VALUES (" + Conexionid + ")";
                            if (VerificarExistencia("infoconex", "ConexionID", Conexionid, Vble.Periodo) == 0)
                            {
                                string insertInfoConex = "INSERT INTO infoconex (ConexionID, Periodo) VALUES (" + Conexionid + ", " + Vble.Periodo + ")";
                                MySqlCommand commandInfoConex = new MySqlCommand(insertInfoConex, DB.conexBD);
                                commandInfoConex.ExecuteNonQuery();
                                commandInfoConex.Dispose();
                            }

                            if (VerificarExistencia("historial", "ConexionID", Conexionid, Vble.Periodo) == 0)
                            {
                                string insertHistorial = "INSERT INTO Historial (ConexionID, Periodo, HistoPeriodo00," +
                                " HistoConsumo00, HistoPeriodo01, HistoConsumo01, HistoPeriodo02, HistoConsumo02, HistoPeriodo03, HistoConsumo03," +
                                " HistoPeriodo04, HistoConsumo04, HistoPeriodo05, HistoConsumo05, HistoPeriodo06, HistoConsumo06, HistoPeriodo07," +
                                " HistoConsumo07, HistoPeriodo08, HistoConsumo08, HistoPeriodo09, HistoConsumo09, HistoPeriodo10, HistoConsumo10," +
                                " HistoPeriodo11, HistoConsumo11) VALUES (" + Conexionid + ", " + Vble.Periodo + ", '" + Vble.HistoPeriodo01 + "', '" + Vble.HistoConsumo01 + "', '" + Vble.HistoPeriodo02 + "', '" + Vble.HistoConsumo02 + "', '" + Vble.HistoPeriodo03 + "', '" + Vble.HistoConsumo03 +
                                "', '" + Vble.HistoPeriodo04 + "', '" + Vble.HistoConsumo04 + "', '" + Vble.HistoPeriodo05 + "', '" + Vble.HistoConsumo05 + "', '" + Vble.HistoPeriodo06 + "', '" + Vble.HistoConsumo06 +
                                "', '" + Vble.HistoPeriodo07 + "', '" + Vble.HistoConsumo07 + "', '" + Vble.HistoPeriodo08 + "', '" + Vble.HistoConsumo08 + "', '" + Vble.HistoPeriodo09 + "', '" + Vble.HistoConsumo09 +
                                "', '" + Vble.HistoPeriodo10 + "', '" + Vble.HistoConsumo10 + "', '" + Vble.HistoPeriodo11 + "', '" + Vble.HistoConsumo11 + "', '" + Vble.HistoPeriodo12 + "', '" + Vble.HistoConsumo12 + "')";
                                MySqlCommand command2 = new MySqlCommand(insertHistorial, DB.conexBD);
                                command2.ExecuteNonQuery();
                                command2.Dispose();
                            }

                            //if (VerificarDomicNoPrinter("NoImprimir", "ConexionID", Conexionid, "Periodo", Vble.Periodo) == 1)
                            //{
                            //    ActualizarImpresionCOD(Conexionid);
                            //}

                            return 0;



                            //return 0;
                        }

                        //if (VerificarDomicNoPrinter("NoImprimir", "ConexionID", Conexionid, "Periodo", Vble.Periodo) == 1)
                        //{
                        //    ActualizarImpresionCOD(Conexionid);
                        //}

                        return 0;
                    }
                }
                else
                {

                    //Conexionid = Convert.ToInt32(substrings[1]);//                   
                    Vble.ImporConex.Add(k + " - " + Conexionid.ToString());
                    Vble.ConexBloImp = true;

                }


            }
            catch (Exception r)
            {

                //MessageBox.Show(r.Message + r.InnerException + " Error al Cargar registro en Conexiones.");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                               " Error en el metodo Importar Tabla Conexiones. Proceso - Importar Ruta. - Registrado en Log Importacion: ConexionID = " + Conexionid.ToString() + " Periodo = " + Vble.Periodo.ToString() +
                                               "\n");
                Conexionid = Convert.ToInt32(substrings[1]);//                   
                Vble.ImporConex.Add(Conexionid.ToString());
                Vble.ConexBloImp = true;

                //sr.Dispose();
                //File.Delete(Vble.ArchivoImportación);               
            }
            return 0;
        }

        private static void ActualizarImpresionCOD(Int32 Conexionid)
        {
            string Select = $"SELECT ImpresionCOD FROM NoImprimir WHERE ConexionID = {Conexionid} AND Periodo = {Vble.Periodo}";
            string ImpresionCOD = "";
            DataTable TableImpCod = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(Select, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(da);
            da.Fill(TableImpCod);

            if (TableImpCod.Rows.Count == 1)
            {
                foreach (DataRow Fila in TableImpCod.Rows)
                {
                    ImpresionCOD = Fila["ImpresionCOD"].ToString();
                    string update;//Declaración de string que contendra la consulta UPDATE               
                    update = "UPDATE Conexiones SET ImpresionCOD = " + ImpresionCOD +
                             " WHERE ConexionID = " + Conexionid + " AND Periodo = " + Vble.Periodo;
                    //preparamos la cadena pra insercion
                    MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //comandoSQL.Dispose();
                }
            }

            da.Dispose();
            comandoSQL.Dispose();
            TableImpCod.Clear();

        }

        /// <summary>
        /// Metodo que va a realizar la actualización de los datos de la tabla conexiones en caso de que la conexion que se esta importando
        /// perteneciente al mismo periodo ya se encuentre y haya sido exportado (ImpresionOBS = 600) con anterioridad.
        /// </summary>
        /// <param name="Conexionid"></param><param name="CondIVA"></param> <param name="Contrato"></param><param name="usuarioID"></param>
        /// <param name="tituIarID"></param><param name="propietarioID"></param><param name="ImpresionCOD"></param><param name="Zona"></param>
        /// <param name="Ruta"></param><param name="Secuencia"></param><param name="Remesa"></param><param name="CodigoAlumbrado"></param>
        /// <param name="ConsumoPromedio"></param><param name="ConsumoResidual"></param><param name="DocumPago1"></param><param name="DocumPago2"></param>
        /// <param name="HistoPeriodo01"></param><param name="HistoConsumo01"></param><param name="HistoPeriodo02"></param><param name="HistoConsumo02"></param>
        /// <param name="HistoPeriodo03"></param><param name="HistoConsumo03"></param><param name="HistoPeriodo04"></param><param name="HistoConsumo04"></param>
        /// <param name="HistoPeriodo05"></param><param name="HistoConsumo05"></param><param name="HistoPeriodo06"></param><param name="HistoConsumo06"></param>
        /// <param name="HistoPeriodo07"></param><param name="HistoConsumo07"></param><param name="HistoPeriodo08"></param><param name="HistoConsumo08"></param>
        /// <param name="HistoPeriodo09"></param><param name="HistoConsumo09"></param><param name="HistoPeriodo10"></param><param name="HistoConsumo10"></param>
        /// <param name="HistoPeriodo11"></param><param name="HistoConsumo11"></param><param name="HistoPeriodo12"></param><param name="HistoConsumo12"></param>
        /// <param name="FechaCalP"></param><param name="DomicSumin"></param><param name="BarrioSumin"></param><param name="CodPostalSumin"></param>
        /// <param name="CuentaDebito"></param><param name="Categoria"></param><param name="TipoProrrateo"></param><param name="CESPnumero"></param>
        /// <param name="PromedioDiario"></param><param name="Vencimiento1"></param><param name="Vencimiento2"></param><param name="VencimientoProx"></param>
        /// <param name="CESPvencimiento"></param>
        public static void ActualizarDatosEnConexiones(Int32 Conexionid, string FechaCalP, Int32 Contrato, string Instalacion, Int32 usuarioID, Int32 tituIarID,
                                                       Int32 propietarioID, Int32 ImpresionCOD, Int32 Zona, Int32 Ruta, Int32 Secuencia, Int32 Remesa,
                                                       Int32 ConsumoControl, Int32 ConsumoResidual, string DomicSumin, string LocalidadSumin, string CodPostalSumin, string CuentaDebito,
                                                       string TarifaCOD, double PromedioDiario, DateTime VencimientoProx)
        {

            string update;//Declaración de string que contendra la consulta UPDATE               
            update = "UPDATE conexiones SET Periodo = " + Vble.Periodo + ", " +
                "FechaCalP = " + FechaCalP + ", " +
                "Contrato = " + Contrato + ", " +
                "Instalacion = " + Instalacion + ", " +
                "usuarioID = " + usuarioID + "," +
                "titularID = " + tituIarID + ", " +
                "propietarioID = " + propietarioID + ", " +
                "DomicSumin = '" + DomicSumin + "', " +
                "LocalidadSumin = '" + LocalidadSumin + "', " +
                "CodPostalSumin = " + CodPostalSumin + ", " +
                "CuentaDebito = " + CuentaDebito + ", " +
                "ImpresionCOD = " + ImpresionCOD + ", " +
                "ImpresionOBS = " + 0 + ", " +
                "Lote = " + Vble.Lote + ", " +
                "Zona = " + Zona + ", " +
                "Ruta = " + Ruta + ", " +
                "Secuencia = " + Secuencia + ", " +
                "Remesa = " + Remesa + ", " +
                "TarifaCOD = '" + TarifaCOD + "', " +
                "TarifaTex =  ' ', " +
                "ConsumoControl = " + ConsumoControl + ", " +
                "ConsumoResidual = " + ConsumoResidual + ", " +
                "ConsumoFacturado = " + 0 + ", " +// PromedioDiario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ", " +
                "OrdenTomado = " + 0 + ", " +
                "VencimientoProx = '" + VencimientoProx.ToString("dd/MM/yyyy") + "' " +
                //"HistoPeriodo01 = " + HistoPeriodo01 + ", " +
                //"HistoConsumo01 = " + HistoConsumo01 + ", " +
                //"HistoPeriodo02 = " + HistoPeriodo02 + ", " +
                //"HistoConsumo02 = " + HistoConsumo02 + ", " +
                //"HistoPeriodo03 = " + HistoPeriodo03 + ", " +
                //"HistoConsumo03 = " + HistoConsumo03 + ", " +
                //"HistoPeriodo04 = " + HistoPeriodo04 + ", " +
                //"HistoConsumo04 = " + HistoConsumo04 + ", " +
                //"HistoPeriodo05 = " + HistoPeriodo05 + ", " +
                //"HistoConsumo05 = " + HistoConsumo05 + ", " +
                //"HistoPeriodo06 = " + HistoPeriodo06 + ", " +
                //"HistoConsumo06 = " + HistoConsumo06 + ", " +
                //"HistoPeriodo07 = " + HistoPeriodo07 + ", " +
                //"HistoConsumo07 = " + HistoConsumo07 + ", " +
                //"HistoPeriodo08 = " + HistoPeriodo08 + ", " +
                //"HistoConsumo08 = " + HistoConsumo08 + ", " +
                //"HistoPeriodo09 = " + HistoPeriodo09 + ", " +
                //"HistoConsumo09 = " + HistoConsumo09 + ", " +
                //"HistoPeriodo10 = " + HistoPeriodo10 + ", " +
                //"HistoConsumo10 = " + HistoConsumo10 + ", " +
                //"HistoPeriodo11 = " + HistoPeriodo11 + ", " +
                //"HistoConsumo11 = " + HistoConsumo11 + ", " +
                //"HistoPeriodo12 = " + HistoPeriodo12 + ", " +
                //"HistoConsumo12 = " + HistoConsumo12 +
                " WHERE ConexionID = " + Conexionid + " AND Periodo = " + Vble.Periodo;
            //preparamos la cadena pra insercion
            MySqlCommand command = new MySqlCommand(update, DB.conexBD);
            //y la ejecutamos
            command.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            command.Dispose();
            //comandoSQL.Dispose();



            string UpdateHisto = "UPDATE historial SET HistoPeriodo00 = '" + Vble.HistoPeriodo01 + "', " +
                        "HistoConsumo00 = '" + Vble.HistoConsumo01 + "', " +
                        "HistoPeriodo01 = '" + Vble.HistoPeriodo02 + "', " +
                        "HistoConsumo01 = '" + Vble.HistoConsumo02 + "', " +
                        "HistoPeriodo02 = '" + Vble.HistoPeriodo03 + "', " +
                        "HistoConsumo02 = '" + Vble.HistoConsumo03 + "', " +
                        "HistoPeriodo03 = '" + Vble.HistoPeriodo04 + "', " +
                        "HistoConsumo03 = '" + Vble.HistoConsumo04 + "', " +
                        "HistoPeriodo04 = '" + Vble.HistoPeriodo05 + "', " +
                        "HistoConsumo04 = '" + Vble.HistoConsumo05 + "', " +
                        "HistoPeriodo05 = '" + Vble.HistoPeriodo06 + "', " +
                        "HistoConsumo05 = '" + Vble.HistoConsumo06 + "', " +
                        "HistoPeriodo06 = '" + Vble.HistoPeriodo07 + "', " +
                        "HistoConsumo06 = '" + Vble.HistoConsumo07 + "', " +
                        "HistoPeriodo07 = '" + Vble.HistoPeriodo08 + "', " +
                        "HistoConsumo07 = '" + Vble.HistoConsumo08 + "', " +
                        "HistoPeriodo08 = '" + Vble.HistoPeriodo09 + "', " +
                        "HistoConsumo08 = '" + Vble.HistoConsumo09 + "', " +
                        "HistoPeriodo09 = '" + Vble.HistoPeriodo10 + "', " +
                        "HistoConsumo09 = '" + Vble.HistoConsumo10 + "', " +
                        "HistoPeriodo10 = '" + Vble.HistoPeriodo11 + "', " +
                        "HistoConsumo10 = '" + Vble.HistoConsumo11 + "', " +
                        "HistoPeriodo11 = '" + Vble.HistoPeriodo12 + "', " +
                        "HistoConsumo11 = '" + Vble.HistoConsumo12 + "'" +
                        " WHERE ConexionID = " + Conexionid + " AND Periodo = " + Vble.Periodo;
            command = new MySqlCommand(UpdateHisto, DB.conexBD);
            command.CommandTimeout = 900;
            command.ExecuteNonQuery();
            command.Dispose();


        }

        /// <summary>
        /// Importa la linea HFS que contiene las ordenes de lectura asociada al usuario que se esta insertando a la base Mysql
        /// </summary>
        /// <param name="substrings"></param>
        public static void ImportarOrdenesLectura(string[] substrings, int k)
        {
            string OrdenLectura = substrings[1].Trim();//orden de lectura
            string Conexionid = substrings[4].Substring(2).Trim();// Instalacion

            if (Vble.ConexBloImp == false)
            {
                if (substrings.Length == 13)
                {

                    bool NoImprXExCons = false;

                    substrings[5] = substrings[5].Trim() == "" ? "00000000" : substrings[5].Trim();
                    Int32 FechaLecturaProgr = Convert.ToInt32(substrings[5].Trim());

                    substrings[6] = substrings[6].Contains('.') ? substrings[6].Substring(0, substrings[6].IndexOf('.')) : substrings[6];
                    substrings[12] = substrings[12].Contains('-') ? substrings[12].Replace("-", "") : substrings[12];

                    Int32 LecturaControl = int.TryParse(substrings[6].Trim(), NumberStyles.Any, CultureInfo.GetCultureInfo("es-AR"), out int Resul) ? Resul : -1;
                    Int32 ConsumoResidual = int.TryParse(substrings[12].Trim(), NumberStyles.Any, CultureInfo.GetCultureInfo("es-AR"), out int Residual) ? Residual : 0;

                    NoImprXExCons = ConsumoResidual != 0 ? true : false;

                    try
                    {
                        string update;//Declaración de string que contendra la consulta UPDATE     

                        ///Si NoImprXExCons es true, en la consulta cambia el campo ImpresionCOD a 1 que indica no imprimir por 
                        /// exceso de consumo al venir el consumo residual en el download distinto de 0.
                        if (NoImprXExCons)
                        {
                            update = $"UPDATE conexiones SET OrdenLectura = '{OrdenLectura}', FechaCalP = {FechaLecturaProgr}, " +
                                 $"ConsumoControl = {LecturaControl}, ImpresionCOD = 1 WHERE ConexionID = {Conexionid} AND Periodo = {Vble.Periodo}";
                            ////Habilitar la linea siguiente cuando se van a indicar los usuarios con exceso de consumo con ImpresionCOD = 2 para 
                            ///indicar que tambien se van a imprimir 
                                 //$"ConsumoControl = {LecturaControl}, ImpresionCOD = 2 WHERE ConexionID = {Conexionid} AND Periodo = {Vble.Periodo}";

                        }
                        else
                        {
                            update = $"UPDATE conexiones SET OrdenLectura = '{OrdenLectura}', FechaCalP = {FechaLecturaProgr}, " +
                                   $"ConsumoControl = {LecturaControl} WHERE ConexionID = {Conexionid} AND Periodo = {Vble.Periodo}";
                        }
                        //preparamos la cadena pra insercion
                        MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                        //comandoSQL.Dispose();


                        StringBuilder VerificarOrdLect = new StringBuilder();
                        Inis.GetPrivateProfileString("Datos", "VerificarOrdLect", "0", VerificarOrdLect, 2, Ctte.ArchivoIniName);

                        if (VerificarOrdLect.ToString() == "1")
                        {
                            ///Verifico si la instalacion perteneciente al periodo que se esta importando, está con ImpresionOBS 0 (No cargado, No Leido, No procesado) 
                            ///y se vuelve a importar 
                            ///la ruta, que actualice la Orden de Lectura para el caso de que se haya anulado y vuelto a generar la importación
                            ///y no existan dos ordenes de lecturas distintas.
                            int ImpresionOBS;
                            MySqlCommand da;
                            string txSQL = "SELECT ImpresionOBS FROM conexiones WHERE ConexionID = " + Conexionid + " AND Periodo = " + Vble.Periodo.ToString();
                            da = new MySqlCommand(txSQL, DB.conexBD);
                            da.Parameters.AddWithValue("ImpresionOBS", Conexionid);
                            ImpresionOBS = Convert.ToInt32(da.ExecuteScalar());
                            da.Dispose();
                            if (ImpresionOBS == 0)
                            {
                                string UpdateOrdenLect;//Declaración de string que contendra la consulta UPDATE               
                                UpdateOrdenLect = $"UPDATE conexiones SET OrdenLectura = '{OrdenLectura}' WHERE ConexionID = {Conexionid} AND Periodo = {Vble.Periodo}";
                                //preparamos la cadena pra insercion
                                MySqlCommand commandCxOrdLec = new MySqlCommand(UpdateOrdenLect, DB.conexBD);
                                //y la ejecutamos
                                commandCxOrdLec.ExecuteNonQuery();
                                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                                commandCxOrdLec.Dispose();
                                //comandoSQL.Dispose();
                            }
                        }

                        string updateLectCtrl;//Declaración de string que contendra la consulta UPDATE               
                        updateLectCtrl = $"UPDATE medidores SET LecturaControl = {LecturaControl} " +
                                         $"WHERE ConexionID = {Conexionid} AND Periodo = {Vble.Periodo}";

                        //preparamos la cadena pra insercion
                        MySqlCommand commandMed = new MySqlCommand(updateLectCtrl, DB.conexBD);
                        //y la ejecutamos
                        commandMed.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        commandMed.Dispose();
                        //comandoSQL.Dispose();

                        Vble.CantImportados++;
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message + ". Error al agregar Orden de Lectura y Nº de OpBel a la Instalación " + Conexionid.ToString());
                    }

                }
                else
                {
                    Conexionid = substrings[1];//
                    Vble.ImporOrdenesDeLecturas.Add(k + " - " + Conexionid.ToString());
                    Vble.ConexBloImp = true;
                    if (Conexionid != "")
                    {
                        DeleteUserPorBloq(Conexionid.ToString(), Vble.Periodo, "medidores", "ConexionID");
                        DeleteUserPorBloq(Conexionid.ToString(), Vble.Periodo, "personas", "PersonaID");
                        DeleteUserPorBloq(Conexionid.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                        Vble.ImporOrdenesDeLecturas.Add(k + " - " + Conexionid.ToString());
                        Vble.CantApartados++;
                    }
                }
            }
            else
            {
                Vble.CantApartados++;
                Vble.ImporOrdenesDeLecturas.Add(k);
                Vble.ConexBloImp = false;
            }
        }



        /// <summary>
        /// Metodo que va a realizar la actualización de los datos de la tabla PERSONA en caso de que la conexion que se esta importando
        /// perteneciente al mismo periodo ya se encuentre y haya sido exportado (ImpresionOBS = 600) con anterioridad.
        /// </summary>
        /// <param name="PersonaID"></param>
        /// <param name="Apellido"></param>
        /// <param name="Nombre"></param>
        /// <param name="DocTipo"></param>
        /// <param name="DocNro"></param>
        public static void ActualizarDatosEnPersonas(string PersonaID, string Apellido, string Nombre, string DocTipo, string DocNro,
                                                     int CondIVA, string Domicilio, string Barrio, string CodigoPostal)
        {
            try
            {
                string update;//Declaración de string que contendra la consulta UPDATE               
                update = "UPDATE personas SET Periodo = " + Vble.Periodo + ", " +
                    "Apellido = '" + Apellido + "', " +
                    "Nombre = '" + Nombre + "', " +
                    "DocTipo = '" + DocTipo + "', " +
                    "DocNro = '" + DocNro + "', " +
                    "CondIVA = " + CondIVA + ", " +
                    "Domicilio = '" + Domicilio + "', " +
                    "Barrio = '" + Barrio + "', " +
                    "CodigoPostal = '" + CodigoPostal + "' " +
                    "WHERE PersonaID = " + Convert.ToInt32(PersonaID) + " AND Periodo = " + Vble.Periodo;
                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //comandoSQL.Dispose();
            }
            catch (Exception Err)
            {
                MessageBox.Show("Excepción al actualizar el dato en la tabla Personas. " + Err.Message);
            }
        }


        /// <summary>
        ///  Metodo que va a realizar la actualización de los datos de la tabla MEDIDORES en caso de que la conexion que se esta importando
        /// perteneciente al mismo periodo ya se encuentre y haya sido exportado (ImpresionOBS = 600) con anterioridad.
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <param name="Periodo"></param>
        /// <param name="Numero"></param>
        /// <param name="Modelo"></param>
        /// <param name="Multiplicador"></param>
        /// <param name="Digitos"></param>
        /// <param name="AnteriorFecha"></param>
        /// <param name="AnteriorEstado"></param>
        public static void ActualizarDatosEnMedidores(Int32 ConexionID, int Periodo, string Numero, string Modelo, int Multiplicador, int Digitos, string AnteriorFecha, int AnteriorEstado)
        {
            try
            {

                string update;//Declaración de string que contendra la consulta UPDATE               
                update = "UPDATE medidores SET Numero = '" + Numero + "', " +
                    "Modelo = '" + Modelo + "', " +
                    "Multiplicador = " + Multiplicador + ", " +
                    "Digitos = " + Digitos + ", " +
                    "AnteriorFecha = '" + AnteriorFecha + "', " +
                    "AnteriorEstado = " + AnteriorEstado + ", " +
                    "ActualFecha = '2000/01/01', ActualHora = '00:00', ActualEstado = 0, TipoLectura = 0, " +
                    "Latitud = " + 0 + ", " +
                    "Longitud = " + 0 +
                    " WHERE ConexionID = " + ConexionID + " AND Periodo = " + Periodo;
                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //comandoSQL.Dispose();
            }
            catch (Exception Err)
            {
                MessageBox.Show("Excepción al actualizar el dato en la tabla Medidores. " + Err.Message);
            }
        }


        /// <summary>
        ///  Metodo que va a realizar la actualización de los datos de la tabla CONCETOS DATOS en caso de que la conexion que se esta importando
        /// perteneciente al mismo periodo ya se encuentre y haya sido exportado (ImpresionOBS = 600) con anterioridad.
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <param name="CodigoConcepto"></param>
        /// <param name="CodigoDpec"></param>
        /// <param name="CodigoEscalon"></param>
        /// <param name="Periodo"></param>
        /// <param name="CodigoGrupo"></param>
        /// <param name="TextoEscalon"></param>
        /// <param name="TextoUnidades"></param>
        /// <param name="ImprimeSiCero"></param>
        /// <param name="CuotaUno"></param>
        /// <param name="Unitario"></param>
        public static void ActualizarDatosEnConceptosDatos(Int32 ConexionID, string CodigoConcepto, string CodigoDpec, Int32 CodigoEscalon,
                                                            int Periodo, Int32 CodigoGrupo, string TextoEscalon, string TextoUnidades,
                                                            string ImprimeSiCero, Int32 CuotaUno, double Unitario)
        {
            try
            {
                //ConexionID, CodigoConcepto, CodigoDpec, CodigoEscalon, Periodo, CodigoGrupo, TextoEscalon, TextoUnidades, " +
                //                                      "ImprimeSiCero, CuotaUno, Unitario
                string update;//Declaración de string que contendra la consulta UPDATE               
                update = "UPDATE conceptosdatos SET CodigoConcepto = '" + CodigoConcepto + "', " +
                    "CodigoDpec = '" + CodigoDpec + "', " +
                    "CodigoEscalon = " + CodigoEscalon + ", " +
                    "Periodo = " + Periodo + ", " +
                    "CodigoGrupo = " + CodigoGrupo + ", " +
                    "TextoEscalon = '" + TextoEscalon + "', " +
                    "TextoUnidades = '" + TextoUnidades + "', " +
                    "ImprimeSiCero = '" + ImprimeSiCero + "', " +
                    "CuotaUno = " + CuotaUno + ", " +
                    "Unitario = " + Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) +
                    " WHERE ConexionID = " + ConexionID + " AND Periodo = " + Periodo;
                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //comandoSQL.Dispose();
            }
            catch (Exception Err)
            {
                MessageBox.Show("Excepción al actualizar el dato en la tabla Personas. " + Err.Message);
            }
        }

        /// <summary>
        ///  Metodo que va a realizar la actualización de los datos de la tabla TEXTOSVARIOS en caso de que la conexion que se esta importando
        /// perteneciente al mismo periodo ya se encuentre y haya sido exportado (ImpresionOBS = 600) con anterioridad.
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <param name="Periodo"></param>
        /// <param name="Renglon"></param>
        /// <param name="Texto"></param>
        public static void ActualizarDatosEnTextosVarios(Int32 ConexionID, int Periodo, int Renglon, string Texto)
        {


            try
            {
                //ConexionID, CodigoConcepto, CodigoDpec, CodigoEscalon, Periodo, CodigoGrupo, TextoEscalon, TextoUnidades, " +
                //                                      "ImprimeSiCero, CuotaUno, Unitario
                string update;//Declaración de string que contendra la consulta UPDATE               
                update = "UPDATE textosvarios SET Texto = '" + Texto + "' " +
                    "WHERE ConexionID = " + ConexionID + " AND Periodo = " + Periodo + " AND Renglon = " + Renglon;
                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //comandoSQL.Dispose();
            }
            catch (Exception Err)
            {
                MessageBox.Show("Excepción al actualizar el dato en la tabla Textos Varios. " + Err.Message);
            }
        }


        /// <summary>
        ///  Verifica si existe el registro que se pasa como parametro que es la ClavePrimaria
        /// </summary>
        /// <returns></returns>
        public static int VerificarConceptoDato(string tabla, string ClavePrimaria, Int32 ValorPK, string CodigoConcepto, int Escalon, int Periodo)
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM " + tabla + " WHERE " + ClavePrimaria + " = " + ValorPK + " and CodigoConcepto = '" + CodigoConcepto +
                    "' and CodigoEscalon = " + Escalon + " and Periodo = " + Periodo;
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue(ClavePrimaria, ValorPK);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }



        /// <summary>
        /// Verifica si existe ya el medidor con el numero de conexionID y el periodo que se esta cargando para que nose repita el registro
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="ClavePrimaria">Nombre de la Clave primaria enviada como parametro</param>
        /// <param name="ValorPK">El valor que se pasa a comparar con el nombre de la clave primaria</param>
        /// <param name="Periodo"></param>
        /// <returns></returns>
        public static int VerificarExistencia(string tabla, string ClavePrimaria, int ValorPK, int Periodo)
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM " + tabla + " WHERE " + ClavePrimaria + " = " + ValorPK + " and Periodo = " + Periodo;
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue(ClavePrimaria, ValorPK);
            count = Convert.ToInt32(da.ExecuteScalar());

            da.Dispose();

            if (count == 0)
                return count;
            else
                return count;




        }


        /// <summary>
        /// Verifica si se encuentra el usuario en la tabla NoImprimir que se encuentra cargada en MySQL para marcarlo para no imprimir si corresponde
        /// con el ImpresionCOD = 2
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="ClavePrimaria">Nombre de la Clave primaria enviada como parametro</param>
        /// <param name="ValorPK">El valor que se pasa a comparar con el nombre de la clave primaria</param>
        /// <param name="Periodo"></param>
        /// <returns></returns>
        public static int VerificarDomicNoPrinter(string tabla, string CampoPK1, int ValorPK1, string CampoPK2, int ValorPK2)
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM " + tabla + " WHERE " + CampoPK1 + " = " + ValorPK1 + " and " + CampoPK2 + " = " + ValorPK2;
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue(CampoPK1, ValorPK1);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }


        /// <summary>
        /// Verifica si existe ya el medidor con el numero de conexionID y el periodo que se esta cargando para que nose repita el registro
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="ClavePrimaria">Nombre de la Clave primaria enviada como parametro</param>
        /// <param name="ValorPK">El valor que se pasa a comparar con el nombre de la clave primaria</param>
        /// <param name="Periodo"></param>
        /// <returns></returns>
        public static int VerificarExistenciaTextosVarios(string tabla, string ClavePrimaria, int ValorPK, int Periodo, int Renglon)
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM " + tabla + " WHERE " + ClavePrimaria + " = " + ValorPK + " and Periodo = " + Periodo + " AND Renglon = " + Renglon;
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue(ClavePrimaria, ValorPK);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }


        /// <summary>
        /// Recibo como parametro la linea leida que contiene campos de la tabla PERSONAS y agrego el registro a la tabla
        /// </summary>
        /// <param name="substrings"></param>
        private static void ImportarTablaPersonas(string[] substrings, int k)
        {
            string PersonaID = "";
            try
            {
                //VARIABLE PARA EL INSERT          
                string Apellido, Nombre, DocTipo, DocNro;

                if (Vble.ConexBloImp == false)
                {
                    if (substrings.Length == 6)
                    {
                        PersonaID = substrings[1].ToString();
                        substrings[2] = substrings[2] == "" ? " " : substrings[2];
                        Apellido = substrings[2].Replace("'", "");
                        Apellido.Replace(",", "");
                        substrings[3] = substrings[3] == "" ? " " : substrings[3];
                        Nombre = substrings[3].Replace("'", "");
                        Nombre.Replace(",", "");
                        substrings[4] = substrings[4] == "" ? " " : substrings[4];
                        DocTipo = substrings[4];
                        substrings[5] = substrings[5] == "" ? " " : substrings[5];
                        DocNro = substrings[5];

                        if (VerificarExistencia("Personas", "PersonaID", Convert.ToInt32(PersonaID), Vble.Periodo) == 0)
                        {

                            string insert;//Declaración de string que contendra la consulta INSERT               
                            insert = "INSERT INTO personas (PersonaID, Periodo, Apellido, Nombre, DocTipo, DocNro, CondIVA, Domicilio, Barrio, CodigoPostal) " +
                                     "VALUES (" + Convert.ToInt32(PersonaID) + ", " + Vble.Periodo + ", '" + Apellido + "', '" +
                                               Nombre + "', '" + DocTipo + "', '" + DocNro + "', " + Vble.CondIVA + ", '" + Vble.DomicSumin + "', '" + Vble.LocalidadSumin + "', '" + Vble.CodPostalSumin + "')";
                            //preparamos la cadena pra insercion
                            MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                            //y la ejecutamos
                            command.ExecuteNonQuery();
                            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                            command.Dispose();
                            //MessageBox.Show("Se agrego la Persona: " + Apellido);
                        }
                        else
                        {

                            //    if (ControlarImpresionOBS(Convert.ToInt32(PersonaID), Vble.Periodo, "TitularID") == 0)
                            //    {
                            ActualizarDatosEnPersonas(PersonaID, Apellido, Nombre, DocTipo, DocNro, Vble.CondIVA, Vble.DomicSumin,
                                                      Vble.LocalidadSumin, Vble.CodPostalSumin);
                            //        return;
                            //    }


                            //return;

                        }
                    }
                    else
                    {
                        PersonaID = substrings[1];//  
                        Vble.ImporPers.Add(k + " - " + PersonaID.ToString());
                        if (PersonaID != "")
                        {
                            //Vble.ImporPers.Add(PersonaID.ToString());
                            DeleteUserPorBloq(PersonaID, Vble.Periodo, "conexiones", "ConexionID");
                        }
                        else
                        {
                            DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                        }
                        Vble.ConexBloImp = true;
                    }
                }
                else
                {
                    PersonaID = substrings[1];//                    
                    //Vble.ConexBloImp = true;
                    if (PersonaID != "")
                    {
                        //Vble.ImporPers.Add(PersonaID.ToString());
                        DeleteUserPorBloq(PersonaID, Vble.Periodo, "conexiones", "ConexionID");
                    }
                    else
                    {
                        DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                    }

                }
            }
            catch (Exception r)
            {
                //MessageBox.Show(r.Message + " Error al cargar registros de PERSONAS");

                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                               " Error en el metodo Importar Tabla Personas. Proceso - Importar Ruta. - Registrado en Log Importacion:  ConexionID = " + PersonaID.ToString() + " Periodo = " + Vble.Periodo.ToString() +
                                               "\n");
                PersonaID = substrings[1];//

                Vble.ConexBloImp = true;
                if (PersonaID != "")
                {
                    Vble.ImporPers.Add(k + " - " + PersonaID.ToString());
                    DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                }
                else
                {
                    DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                }

            }
        }

        public static void DeleteUserPorBloq(string ConexionID, Int32 Periodo, string Tabla, string ColumnID)
        {
            //Elimina Registros al Exportar de la tabla Personas
            string DeleteCF = "DELETE FROM " + Tabla +
                              " WHERE " + ColumnID + " = " + ConexionID +
                              " AND " + "Periodo = " + Periodo;
            MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
            cmdSQL2.ExecuteNonQuery();
            cmdSQL2.Dispose();
        }


        /// <summary>
        /// Recibo como parametro la linea leida que contiene campos de la tabla MEDIDORES y agrego el registro a la tabla
        /// 
        /// </summary>
        /// <param name="substrings"></param>
        private static void ImportarTablaMedidores(string[] substrings, int k)
        {
            string conexionID = "";
            try
            {
                //IFormatProvider culture = new System.Globalization.CultureInfo("es-AR", true);
                //VARIABLE PARA EL INSERT 
                Int32 Multiplicador, Digitos, AnteriorEstado;
                string Numero, Modelo;
                DateTime AnteriorFecha;
                int i;

                if (Vble.ConexBloImp == false)
                {
                    if (substrings.Length == 8)
                    {
                        conexionID = substrings[1];
                        substrings[2] = substrings[2] == "" ? "0" : substrings[2];
                        Numero = substrings[2].ToString();

                        substrings[3] = substrings[3] == "" ? "0" : substrings[3];
                        Modelo = substrings[3].ToString();

                        substrings[4] = substrings[4] == "" ? "0" : substrings[4];
                        i = substrings[4].IndexOf(".");
                        substrings[4] = i < 0 ? substrings[4] : substrings[4].Substring(0, i);
                        Multiplicador = Convert.ToInt32(substrings[4]);

                        substrings[5] = substrings[5] == "" ? "0" : substrings[5];
                        Digitos = Convert.ToInt32(substrings[5]);

                        substrings[6] = substrings[6] == "" ? "01/01/2000" : substrings[6];
                        AnteriorFecha = DateTime.Parse(substrings[6]);//

                        substrings[7] = substrings[7] == "" ? "0" : substrings[7];
                        AnteriorEstado = int.Parse(substrings[7], NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"));

                        if (VerificarExistencia("medidores", "ConexionID", Convert.ToInt32(conexionID), Vble.Periodo) == 0)
                        {

                            string insert;//Declaración de string que contendra la consulta INSERT               
                            insert = "INSERT INTO medidores (ConexionID, Periodo, Modelo, Numero, Multiplicador, Digitos, AnteriorFecha, " +
                                                            "AnteriorEstado, ActualFecha, ActualHora, TipoLectura, Latitud, Longitud) " +
                                            "VALUES (" + Convert.ToInt32(conexionID) + ", " + Vble.Periodo + ", '" + Modelo + "', '" + Numero + "', " +
                                                         Multiplicador + ", " + Digitos + ", '" + AnteriorFecha.ToString("yyyy/MM/dd") + "', " + AnteriorEstado + ", '2000/01/01', '00:00', 0, 0, 0)";
                            //preparamos la cadena pra insercion
                            MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                            //y la ejecutamos
                            command.ExecuteNonQuery();
                            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                            command.Dispose();
                            //MessageBox.Show("Se agrego el Medidor Nº: " + Numero + " y Modelo: " + Modelo);
                        }
                        else
                        {
                            if (ControlarImpresionOBS(Convert.ToInt32(conexionID), Vble.Periodo, "ConexionID") == 0)
                            {
                                ActualizarDatosEnMedidores(Convert.ToInt32(conexionID), Vble.Periodo, Numero, Modelo, Multiplicador, Digitos, AnteriorFecha.ToString("yyyy/MM/dd"), AnteriorEstado);
                                return;
                            }
                            //return;
                        }
                    }
                    else
                    {
                        conexionID = substrings[1];//                       
                        Vble.ConexBloImp = true;

                        if (conexionID != "")
                        {
                            Vble.ImporMed.Add(conexionID.ToString());
                            DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "personas", "PersonaID");
                            DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                        }
                        else
                        {
                            DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "personas", "PersonaID");
                            DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                        }

                    }
                }
                else
                {
                    conexionID = substrings[1];//
                    //Vble.ImporMed.Add(conexionID.ToString());
                    //Vble.ConexBloImp = true;
                    if (conexionID != "")
                    {
                        DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "personas", "PersonaID");
                        DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                    }
                }
            }
            catch (Exception r)
            {
                conexionID = substrings[1];//
                Vble.ImporMed.Add(k + " - " + conexionID.ToString());
                Vble.ConexBloImp = true;
                if (conexionID != "")
                {
                    DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "personas", "PersonaID");
                    DeleteUserPorBloq(Vble.ConexionID.ToString(), Vble.Periodo, "conexiones", "ConexionID");
                }
                //MessageBox.Show(r.Message + "Error al cargar registros de MEDIDORES");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                              " Error en el metodo Importar Tabla Medidores. Proceso - Importar Ruta. - Registrado en Log Importacion  ConexionID = " + conexionID + " Periodo = " + Vble.Periodo.ToString() +
                                              "\n");
            }

        }
        /// <summary>
        /// Recibo como parametro la linea leida que contiene campos de la tabla CONCEPTOS D y agrego el registro a la tabla
        /// 
        /// </summary>
        /// <param name="substrings"></param>
        private static void ImportaTablaConceptosDatos(string[] substrings)
        {
            try
            {
                //VARIABLE PARA EL INSERT    
                Int32 ConexionID, CodigoEscalon, CodigoGrupo, CuotaUno, CodigoAux = 0;
                string TextoEscalon, TextoUnidades, ImprimeSiCero, CodigoDpec, CodigoConcepto;
                double Unitario;

                ///habilitar cuando esta el codigo de dpec en el archivo Download
                //substrings[x] = substrings[x] == "" ? "z" : substrings[x];
                //CodigoDpec = substrings[x] == "" ? "z" : substrings[x];

                ConexionID = Convert.ToInt32(substrings[1]);


                ///habilitar cuando esta el codigo de dpec en el archivo Download
                //substrings[3] = substrings[3] == "" ? "z" : substrings[3];
                CodigoDpec = substrings[3] == "" ? "z" : substrings[3];
                if (CodigoDpec == "z")
                {
                    CodigoDpec = substrings[2];
                    CodigoAux = 1;
                }



                //substrings[2] = substrings[2] == "" ? "0" : substrings[2];
                //CodigoConcepto = Convert.ToInt32(substrings[2]);
                CodigoConcepto = substrings[2] == "" ? "0" : substrings[2];

                substrings[4] = substrings[4] == "" ? "0" : substrings[4];
                CodigoEscalon = Convert.ToInt32(substrings[4]);

                substrings[5] = substrings[5] == "" ? "0" : substrings[5];
                CodigoGrupo = Convert.ToInt32(substrings[5]);

                substrings[6] = substrings[6] == "" ? "-" : substrings[6];
                TextoEscalon = substrings[6];

                substrings[7] = substrings[7] == "" ? "-" : substrings[7];
                TextoUnidades = substrings[7];

                substrings[8] = substrings[8] == "" ? "NO" : substrings[8];
                ImprimeSiCero = substrings[8];

                substrings[9] = substrings[9] == "" ? "0" : substrings[9];
                CuotaUno = Convert.ToInt32(substrings[9]);

                substrings[10] = substrings[10] == "" ? "1" : substrings[10];
                Unitario = Convert.ToDouble(substrings[10], CultureInfo.CreateSpecificCulture("en-US"));
                //MessageBox.Show(Unitario.ToString());
                //Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US"));

                //string insert;//Declaración de string que contendra la consulta INSERT               
                //insert = "INSERT INTO conceptosdatos (ConexionID, CodigoConcepto, CodigoEscalon, Periodo, CodigoGrupo, TextoEscalon, TextoUnidades, " +
                //                                      "ImprimeSiCero, CuotaUno, Unitario)" +
                //                "VALUES (" + Convert.ToInt32(ConexionID) + ", " + Convert.ToInt32(CodigoConcepto) + ", " + Convert.ToInt32(CodigoEscalon) +
                //                         ", " + Vble.Periodo + ", " + Convert.ToInt32(CodigoGrupo) + ", '" + TextoEscalon + "', '" + TextoUnidades + "', '" +
                //                         ImprimeSiCero + "', " + Convert.ToInt32(CuotaUno) + ", " +
                //                         Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ")";


                if (VerificarConceptoDato("conceptosdatos", "ConexionID", ConexionID, CodigoConcepto, CodigoEscalon, Vble.Periodo) == 0)
                {

                    //insert con estructura modificada (columna CodigoDpec agregado)
                    string insert = "INSERT INTO conceptosdatos (ConexionID, CodigoConcepto, CodigoDpec, CodigoEscalon, CodigoAux, Periodo, CodigoGrupo, TextoEscalon, TextoUnidades, " +
                                                          "ImprimeSiCero, CuotaUno, Unitario)" +
                                    "VALUES (" + Convert.ToInt32(ConexionID) + ", '" + CodigoConcepto + "', '" + CodigoDpec + "', " + Convert.ToInt32(CodigoEscalon) + ", " + CodigoAux +
                                             ", " + Vble.Periodo + ", " + Convert.ToInt32(CodigoGrupo) + ", '" + TextoEscalon + "', '" + TextoUnidades + "', '" +
                                             ImprimeSiCero + "', " + Convert.ToInt32(CuotaUno) + ", " +
                                             Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ")";


                    //preparamos la cadena pra insercion
                    MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //MessageBox.Show("Se agrego el concepto Nº: " + TextoEscalon);
                }
                else
                {
                    //if (ControlarImpresionOBS(Convert.ToInt32(ConexionID), Vble.Periodo, "ConexionID") == 0)
                    //{
                    ActualizarDatosEnConceptosDatos(Convert.ToInt32(ConexionID), CodigoConcepto, CodigoDpec, CodigoEscalon, Vble.Periodo, CodigoGrupo,
                                                TextoEscalon, TextoUnidades, ImprimeSiCero, CuotaUno, Unitario);
                    return;
                    //}
                    //return;
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + ".Error al cargar registros de CONCEPTOS DATOS");
            }

        }
        /// <summary>
        /// Recibo como parametro la linea leida que contiene campos de la tabla PERSONAS y agrego el registro a la tabla
        /// </summary>
        /// <param name="substrings"></param>
        private static void ImportarTablaTextosVarios(string[] substrings)
        {
            try
            {
                //VARIABLE PARA EL INSERT          
                string Texto;
                Int32 ConexionID, Renglon;


                ConexionID = Convert.ToInt32(substrings[1]);
                substrings[2] = substrings[2] == "" ? " " : substrings[2];
                Renglon = Convert.ToInt32(substrings[2]);
                substrings[3] = substrings[3] == "" ? " " : substrings[3];
                Texto = substrings[3];

                if (VerificarExistenciaTextosVarios("Conexiones", "ConexionID", ConexionID, Vble.Periodo, Renglon) == 0)
                {

                    string insert;//Declaración de string que contendra la consulta INSERT               
                    insert = "INSERT INTO TextosVarios (ConexionID, Periodo, Renglon, Texto) " +
                             "VALUES (" + ConexionID + ", " + Vble.Periodo + ", " + Renglon + ", '" + Texto + "')";
                    //preparamos la cadena pra insercion
                    MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //MessageBox.Show("Se agrego el Texto Vario: " + Texto);

                }
                else
                {
                    //if (ControlarImpresionOBS(ConexionID, Vble.Periodo, "ConexionID") == 0)
                    //{
                    ActualizarDatosEnTextosVarios(ConexionID, Vble.Periodo, Renglon, Texto);
                    return;
                    //}
                    //return;
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al cargar registros de TEXTOS VARIOS");
            }
        }



        #endregion


        /// <summary>
        /// Para Realizar la Importación se necesita abrir una unidad de red, esto es para poder realizar la copia de la NAS
        /// a la PC (centro de interfaz) que va a realizar la importacion. Cuando termina la importacion, se cierra la Unidad de red
        /// para que no quede abierta al usuario final desde el método Cerrar ConexionRed().
        /// </summary>
        /// <param name="Directorio"></param>
        /// <param name="UnidadTemporal"></param>
        public void AbrirConexionRed(String Directorio, String UnidadTemporal)
        {
            IWshRuntimeLibrary.IWshNetwork2 network = new IWshRuntimeLibrary.WshNetwork();
            String localname = UnidadTemporal;//@"K:";
            String remotename = Directorio; //@"\\10.1.3.125\DOWNLOAD\";
            Object updateprofile = Type.Missing;
            Object username = Vble.DominioYUsuarioRed;
            Object pass = Vble.ContraseñaRed;
            network.MapNetworkDrive(localname, remotename, ref updateprofile, ref username, ref pass);
        }

        /// <summary>
        /// Una vez que se realizo toda la importacion, es decir no quedaron archivos de rutas para el centro de interfaz que ejecuto
        /// el proceso, la Unidad de red que se abrio temporalmente es cerrada.
        /// </summary>
        public void CerrarConexionRed()
        {
            IWshRuntimeLibrary.IWshNetwork2 network = new IWshRuntimeLibrary.WshNetwork();
            Object updateprofile = Type.Missing;
            DriveInfo[] drives;
            drives = System.IO.DriveInfo.GetDrives();

            foreach (DriveInfo strDrive in drives)
            {
                if (DriveType.Network == strDrive.DriveType)
                {
                    //borramos la unidad que sea de red
                    bool blnForce = true;
                    Object objblnForce = (object)blnForce;
                    //Object updateprofile = Type.Missing;
                    string strMapDriveLetter = strDrive.Name.Substring(0, 2);
                    network.RemoveNetworkDrive(strMapDriveLetter, ref objblnForce, ref updateprofile);
                }
            }
        }


        #endregion


        #region Botones y Herramientas del Formulario

        /// <summary>
        /// Presenta el formulario para generar los procesos de exportación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void btnExportacion_Click(object sender, EventArgs e)
        {
            if (DB.sDbUsu.ToUpper() == "SUPERVISOR" || DB.sDbUsu.ToUpper() == "AUDITORIA")
            {
                Vble.ShowLoading();

                Task oTask = new Task(ShowCargasColectoras);
                oTask.Start();

                var Form2 = new FormResumenSup();
                Form2.MdiParent = this.MdiParent;
                Form2.Show();
                Form2.WindowState = FormWindowState.Maximized;
                await oTask;
                Vble.HideLoading();
                timer2.Stop();
            }
            else
            {
                Vble.ShowLoading();

                Task oTask = new Task(ShowCargasColectoras);
                oTask.Start();

                var Form2 = new Form2Exportar();
                Form2.MdiParent = this.MdiParent;
                Form2.Show();
                Form2.WindowState = FormWindowState.Maximized;

                await oTask;
                Vble.HideLoading();
                timer2.Stop();
            }




        }

        /// <summary>
        /// Presenta el formulario para procxesar las descargas de colectoras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDescargas_Click(object sender, EventArgs e)
        {
            var Form3 = new Form3Descargas();
            Form3.MdiParent = this.MdiParent;
            Form3.Show();
            Form3.WindowState = FormWindowState.Maximized;
        }

        public void ShowLoading()
        {
            loading = new FormLoading();
            loading.Show();
        }

        public void HideLoading()
        {

        }



        /// <summary>
        /// Presenta el formulario para generar las cargas a las colectoras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void btnCargas_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            Vble.ShowLoading();
            Task oTask = new Task(ShowCargasColectoras);
            oTask.Start();
            Vble.PeriodoFORM0 = cboPeriodo.Text.ToString().Replace("-", "");
            Form4Cargas Form4 = new Form4Cargas();
            Form4.MdiParent = this.MdiParent;
            Form4.Show();
            Form4.WindowState = FormWindowState.Maximized;
            //this.Dispose();


            await oTask;
            Vble.HideLoading();
            timer2.Stop();

        }
        public void ShowCargasColectoras()
        {
            //Thread.Sleep(2500);
            //var Form2 = new Form2Exportar();
            //Form2.CargarTVRutasExportadas();
            //BeginInvoke(new InvokeDelegate(Mostrar));           
        }
        public void Mostrar()
        {

        }


        void Form1_Resize(object sender, System.EventArgs e)
        {
            InfoImportacion.Top = (this.Height - InfoImportacion.Height) / 2;
            InfoImportacion.Left = (this.Width - InfoImportacion.Width) / 1 - 50;
            //InfoImportacion.Width = 306;
            //InfoImportacion.Height = 327;
            //panel1.Top = (this.Height - InfoImportacion.Height) / 2;
            //panel1.Left = (this.Width - InfoImportacion.Width) / 8 - 50;
            //panel1.Width = 306;
            //panel1.Height = 327;
            frCmd.Top = (this.Height - frCmd.Height) / 2;
            frCmd.Left = (this.Width - frCmd.Width) / 2;
            btnTest.Location = frCmd.Location;
            btnTest.SendToBack();

        }

        void btnSalir_Click(object sender, EventArgs e)
        {
            FormStart ControlDeAcceso = new FormStart();
            Form0 Contenedor = new Form0();
            string Per;
            Per = ((Form0)this.MdiParent).mnuPeriodoActual.Text;
            Inis.WritePrivateProfileString("Datos", "Periodo", Per, Ctte.ArchivoIniName);


            //if (DB.conexBD != null || DB.conexBDHistorial != null)
            //DB.conexBD.Close();
            //DB.conexBDHistorial.Close();
            //Contenedor.timer1.Enabled = true;
            //this.Close();
            //ControlDeAcceso.Show();


            Application.Exit();
        }

        /// <summary>
        /// Presenta el formulario para establecer las carpetas donde
        /// se almacenan los datos, y configuracioines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConfigCarp_Click(object sender, EventArgs e)
        {
            var Form5 = new Form5ConfigCarp();
            //Form5.MdiParent = this.MdiParent;
            Form5.Show();
            //Form5.WindowState = FormWindowState.Maximized;
        }

        void btnParaServidor_Click(object sender, EventArgs e)
        {
            //ParaQueGenerar(enumGeneraPara.Servidor );
        }

        void btnParaCD_Click(object sender, EventArgs e)
        {
            //ParaQueGenerar(enumGeneraPara.CD);
        }

        void cboPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Actualiza el periodo indicado en la barra de menús principal y la variable periodo
            //Form6Informes form6 = new Form6Informes();
            ((Form0)this.MdiParent).mnuPeriodoActual.Text = cboPeriodo.Text;
            Vble.Periodo = int.Parse(cboPeriodo.Text.Substring(0, 4) + cboPeriodo.Text.Substring(5, 2));
            //MessageBox.Show(Vble.CarpetaTrabajo + "\\" + Vble.Periodo + "\\Informes");

            if (Vble.PanelLogImp.ToString() == "1")
            {
                InfoImportacion.Visible = true;
                CargarRutasImportadas();
            }
            else
            {
                InfoImportacion.Visible = false;
            }

        }



        public void InvokeMethodActivarProgressBar()
        {
            this.timer1.Interval = 100;
            this.timer1.Enabled = true;
            timer1.Start();
        }




        /// <summary>
        /// Importa los datos desde el repositorio de DPEC a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            //DownloadYUpload Download = new DownloadYUpload();

            try
            {

                this.Cursor = Cursors.WaitCursor;

                //ContarCantidadDeConexiones();  

                //if (Vble.TotalConexiones > 0)
                //{
                //this.progressBar.Maximum = Vble.TotalConexiones;
                ////this.iTalk_ProgressBar1.Maximum = Vble.TotalConexiones;
                //BeginInvoke(new InvokeDelegate(InvokeMethodActivarProgressBar));
                //InvokeMethodActivarProgressBar();
                Download2plano.RunWorkerAsync();
                //}
                //this.Cursor = Cursors.Default;


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Existe un problema con la unidad de red a la cual intenta conectarse", "Error de unidad de Red", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnTest_Click(object sender, EventArgs e)
        {




        }

        private void button1_Click(object sender, EventArgs e)
        {





            ///Esta porcion de codigo llama al Form6Inf que contiene los informes que se generan en la pantalla Descargas
            ///esta en comentario porque ese informe no se esta usando por el momento, solo se esta haciendo uso de los 
            ///informes que se generan desde la pantalla de exportación.
            //string DirecInf = $"C:\\A_DPEC\\_Pruebas\\EmpresaLocal\\{cboPeriodo.Text.Replace("-", "")}\\Informes";
            //if (!Directory.Exists(DirecInf))
            //{
            //    Directory.CreateDirectory(DirecInf);
            //}
            //Form Form6Inf = new Form6Informes();
            //Form6Inf.Show();


        }


        private void simulateHeavyWork()
        {
            //Thread.Sleep(CantidadConexiones(Vble.ArchivoImportación));
            Thread.Sleep(Vble.TotalConexiones);
        }




        public void Download2plano_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            //var Download = new DownloadYUpload();
            this.progressBar.Visible = true;
            //this.iTalk_ProgressBar1.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            this.PorcLabel.Visible = true;
            this.progressBar.Value = (e.ProgressPercentage * 100) / Vble.TotalConexiones;
            this.PorcLabel.Text = (e.ProgressPercentage * 100) / Vble.TotalConexiones + " % completado";



            //this.iTalk_ProgressBar1.Value = (e.ProgressPercentage * 100) / Vble.TotalConexiones;
            //this.iTalk_ProgressBar1.Value = e.ProgressPercentage;
            //this.iTalk_ProgressBar1.Text = this.iTalk_ProgressBar1.Value.ToString();
        }



        public void Download2plano_DoWork_1(object sender, DoWorkEventArgs e)
        {
            //Cierro toda conexion por las dudas que este abierta sino, no me va a dejar importar
            Vble.CerrarUnidadDeRed();

            #region Importar cuando se está conectado a la NAS 
            simulateHeavyWork();
            
            //Se establece la conexion con la unidad de red donde estará disponible el archivo encriptado que provee SAP para la importación            
            //Vble.AbrirUnidadDeRed(@"R:", @"\\10.1.3.125\DOWNLOAD");
            if (RBPRD.Checked == true)
            {
                Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaSAPImportacion);
            }
            else if (RBPrueba.Checked == true)
            {
                Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaSAPImportacionPRUEBA);
            }

            RealizarImportacion();
          
            ////buscamos todas las unidades de red para desconectar y no quede abierto el acceso a cualquier usuario
            Vble.CerrarUnidadDeRed();

            #endregion



            #region Usar cuando NO se está conectado a la NAS
            //LeerArchivoImportacion(Vble.ArchivoImportación);
            #endregion
        }

        public void Download2plano_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //var Download = new DownloadYUpload();
            try
            {
                if (Vble.CancelarImportacion == true)
                {
                    this.progressBar.Visible = false;
                    this.progressBar.Value = 0;
                    // this.timer1.Enabled = false;
                    this.PorcLabel.Visible = false;
                    Vble.TotalConexiones = 0;
                    Vble.NombreArchivoImportacion = "";
                    this.Cursor = Cursors.Default;
                    Vble.CancelarImportacion = false;
                    MessageBox.Show("La importacion ha sido cancelada", "Importación Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Vble.ArrayRutasImportadas.Clear();


                }
                else if (Vble.ExistenArchImportacion == false)
                {
                    this.progressBar.Visible = false;
                    this.progressBar.Value = 0;
                    // this.timer1.Enabled = false;
                    this.PorcLabel.Visible = false;
                    Vble.TotalConexiones = 0;
                    Vble.NombreArchivoImportacion = "";
                    this.Cursor = Cursors.Default;
                    Vble.CancelarImportacion = false;
                    MessageBox.Show("No existe ningun archivo de importación para este centro de interfaz", "Importación sin Archivos", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Vble.ArrayRutasImportadas.Clear();
                }
                else
                {
                    this.progressBar.Visible = false;
                    this.progressBar.Value = 0;
                    // this.timer1.Enabled = false;
                    this.PorcLabel.Visible = false;
                    //    this.label1.Visible = false;
                    //    this.label2.Visible = false;
                    //    this.iTalk_ProgressBar1.Visible = false;
                    //this.iTalk_ProgressBar1.Value = 0;

                    cboPeriodo.Items.Add((Vble.Periodo).ToString("0000-00"));
                    Vble.TotalConexiones = 0;
                    ////Download.label1.Visible = false;
                    //Download.Close();
                    //////comentado mientras esta en la NAS
                    if (File.Exists(Vble.ArchivoImportación))
                    {
                        //File.Copy(Vble.ArchivoImportación, Vble.DownloadsHechas + Vble.NombreArchivoImportacion);
                        File.Delete(Vble.ArchivoImportación);
                        Vble.NombreArchivoImportacion = "";
                    }

                    this.Cursor = Cursors.Default;
                    this.timer1.Stop();

                    if (Vble.PanelLogImp.ToString() == "1")
                    {
                        InfoImportacion.Visible = true;
                        CargarRutasImportadas();
                    }
                    else
                    {
                        InfoImportacion.Visible = false;
                    }

                    MessageBox.Show("La importanción se realizo correctamente", "Importación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Vble.ArrayRutasImportadas.Clear();

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Finalizar la Importación.");
            }

        }

        /// <summary>
        /// Timer que muestra el punto intermitente cuando se esta realizando la importación de las rutas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            cnt++;
            label1.Text += " .";

            if (cnt > 3)
            {
                label1.Text = "Realizando Importanción";
                cnt = 0;
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Form7Inf = new Form7InformesAltas();
            Form7Inf.MdiParent = this.MdiParent;
            Form7Inf.WindowState = FormWindowState.Maximized;
            if (DB.sDbUsu.ToUpper() == "SUPERVISOR")
            {
                TabPage pageDetalle = Form7Inf.tabControl1.TabPages["TPDetalleSituaciones"];
                Form7Inf.tabControl1.TabPages.Remove(pageDetalle);
                TabPage pagDetalleZona = Form7Inf.tabControl1.TabPages["TBDetalleZona"];
                Form7Inf.tabControl1.TabPages.Remove(pagDetalleZona);
            }
            Form7Inf.Show();
           
        }


        private void button1_Click_2(object sender, EventArgs e)
        {
            Process[] localAll = Process.GetProcesses();

            foreach (Process item in localAll)
            {
                if (item.ProcessName == "gpg-agent")
                {
                    MessageBox.Show("Nombre: " + item.ProcessName + "  ENCONTRO EL PROCESO GPG-AGENT");
                }
                else if (item.ProcessName == "pinentry")
                {
                    MessageBox.Show("Nombre: " + item.ProcessName + "  ENCONTRO EL PROCESO PINENTRY");
                }

            }
        }


        /// <summary>
        /// Agrega Periodo tomado del textobNewPeriodo al combobox cboPeriodo en caso de que se realice 
        /// alguna importación y el mismo no contenga el periodo descargado, verificando que cumpla con el formato
        /// que ya tienen los periodos por defecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddPeriodo_Click(object sender, EventArgs e)
        {
            //Expresion Regular para que acepte el periodo con el formato 0000-00
            string sPattern = "^\\d{4}-\\d{2}$";
            bool existeperiodo = false;

            if (System.Text.RegularExpressions.Regex.IsMatch(this.TextNewPeriodo.Text, sPattern))
            {
                foreach (var item in cboPeriodo.Items)
                {
                    if (item.ToString() == this.TextNewPeriodo.Text)
                    {
                        existeperiodo = true;
                    }
                }
                if (existeperiodo)
                {
                    this.toolTip1.Show("Ya existe el periodo", this.TextNewPeriodo, 2000);
                    this.TextNewPeriodo.Text = "";
                }
                else
                {
                    cboPeriodo.Items.Add(this.TextNewPeriodo.Text);
                    this.toolTip1.Show("Periodo agregado Correctamente", this.TextNewPeriodo, 2000);
                    this.TextNewPeriodo.Text = "";
                }

            }
            else
            {
                this.toolTip1.Show("Formato del Periodo Invalido", this.TextNewPeriodo, 2000);
                this.TextNewPeriodo.Text = "";
            }

        }

        private void RButBTX_MouseMove(object sender, MouseEventArgs e)
        {
            //toolTip1.Show("Realiza la importación si en el Servidor se subio el archivo con extension .BTX", this.RButBTX, 1500);
        }

        private void RButGPG_MouseMove(object sender, MouseEventArgs e)
        {
            //toolTip1.Show("Realiza la importación si en el Servidor se subio el archivo con extension .GPG", this.RButBTX, 1500);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            try
            {

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    this.TextBoxAgreArcImport.Text = openFileDialog1.SafeFileName;
                    Vble.ArchivoImportación = openFileDialog1.FileName;
                    File.Copy(Path.GetFullPath(openFileDialog1.FileName), Vble.CarpetaImportacion + "\\" + openFileDialog1.SafeFileName);


                    this.Cursor = Cursors.WaitCursor;

                    ContarCantidadDeConexiones();

                    if (Vble.TotalConexiones > 0)
                    {
                        this.progressBar.Maximum = Vble.TotalConexiones;
                        //this.iTalk_ProgressBar1.Maximum = Vble.TotalConexiones;

                        Download2plano.RunWorkerAsync();
                    }
                    this.Cursor = Cursors.Default;
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Existe un problema con la unidad de red a la cual intenta conectarse", "Error de unidad de Red", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                this.TextBoxAgreArcImport.Text = openFileDialog1.SafeFileName;
                Vble.ArchivoImportación = openFileDialog1.FileName;

                ////AbrirConexionRed(Vble.CarpetaSAPImportacion, @"R:");
                //Vble.AbrirUnidadDeRed(@"R:", Vble.CarpetaSAPImportacion);

                File.Copy(Path.GetFullPath(openFileDialog1.FileName), Vble.CarpetaImportacion + "\\" + openFileDialog1.SafeFileName);

                //Vble.CerrarUnidadDeRed();

            }
        }

        #region Conectar a Unidad de Red
        public struct NETRESOURCE
        {

            [MarshalAs(UnmanagedType.I4)]

            public int dwScope;

            [MarshalAs(UnmanagedType.I4)]

            public int dwType;

            [MarshalAs(UnmanagedType.I4)]

            public int dwDisplayType;

            [MarshalAs(UnmanagedType.I4)]

            public int dwUsage;

            [MarshalAs(UnmanagedType.LPWStr)]

            public string lpLocalName;

            [MarshalAs(UnmanagedType.LPWStr)]

            public string lpRemoteName;

            [MarshalAs(UnmanagedType.LPWStr)]

            public string lpComment;

            [MarshalAs(UnmanagedType.LPWStr)]

            public string lpProvider;

        }


        #endregion

        private void ContarSegundoPlano_DoWork(object sender, DoWorkEventArgs e)
        {


        }
        #endregion

        private void timer2_Tick(object sender, EventArgs e)
        {

            if (!Vble.CarpetasConfLeidas)
            {
                Vble.LeerNombresCarpetas();
            }

            string RutasDisponibles = "";
            //MessageBox.Show(Vble.CarpetaSAPImportacion);
            DirectoryInfo CompartidoSAP = new DirectoryInfo(Vble.CarpetaSAPImportacion);
            //creo el objeto que va a contener la direccion de la carpeta compartida de SAP
            if (DB.Entorno == "PRD")
            {
                CompartidoSAP = new DirectoryInfo(Vble.CarpetaSAPImportacion);
            }
            else if (DB.Entorno == "QAS")
            {
                CompartidoSAP = new DirectoryInfo(Vble.CarpetaSAPImportacionPRUEBA);
            }

            Vble.ArrayRutasImportadas.Clear();
            int NroArchivos = 0;
            foreach (var fi in CompartidoSAP.GetFiles())
            {
                if (Vble.IdentificarArchImport(fi.Name) == true)
                {
                    RutasDisponibles += fi.Name.Substring(17, fi.Name.Substring(17).IndexOf("_")) + "\n";
                    NroArchivos++;
                }

            }

            if (NroArchivos > 0)
            {
                PBExistArchImpSI.Visible = true;
                PBExistArchImpNO.Visible = false;
                toolTip2.SetToolTip(PBExistArchImpSI, RutasDisponibles);
            }
            else
            {
                PBExistArchImpSI.Visible = false;
                PBExistArchImpNO.Visible = true;
            }
            //Vble.HabilitarCentroDeDispositivo();
            //}}) ;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult Confirmacion = new DialogResult();

            Confirmacion = MessageBox.Show("¿Está seguro que desea limpiar las tablas que se usan al momento de Importar y Exportar los archivos a SAP?",
                "Borrar datos de tablas", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

            if (Confirmacion == DialogResult.OK)
            {
                string Periodo = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el PERIODO el cual pertenecen los datos que desea eliminar con el formato aaaaMM", "PERIODO A BORRAR ", "Periodo", 100, 0);
                int num;
                bool isnum = int.TryParse(Periodo, out num);

                string Ruta = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la RUTA al cual pertenecen los datos que desea eliminar", "RUTA A BORRAR", "RUTA", 100, 0);
                int num2;
                bool isnum2 = int.TryParse(Ruta, out num2);


                if (Periodo.Length > 0)
                {

                    if (Periodo != "" & isnum == true)
                    {
                        if (Periodo.Length == 6)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            string CONSULTA = "SELECT ConexionID, Periodo, TitularID FROM conexiones WHERE (Periodo = " + num + " OR Periodo = 0) AND Ruta = " + num2;
                            Vble.LimpiarTablas(CONSULTA);
                            this.Cursor = Cursors.Default;

                        }
                        else
                        {
                            MessageBox.Show("Controle que el formato del periodo ingresado sea aaaaMM, por ejemplo: 201701", "Dato mal ingresado",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Disculpe para limpiar la base de datos y poder realizar una nueva importación deberá ingresar el periodo correcto", "Periodo incorrecto",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Ctte.ArchivoIniName);
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DetalleImpresos.IndicadorTipoInforme = "Historial";
            //DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            //DetalleImpresos.ImpresionOBS = ImpresionOBS;
            //DetalleImpresos.RutaDatos = RutaDatos;          
            DetalleImpresos.MdiParent = this.MdiParent;
            DetalleImpresos.WindowState = FormWindowState.Maximized;
            DetalleImpresos.LabelLeyenda.Visible = false;
            DetalleImpresos.Show();
        }

        private void TextNewPeriodo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Expresion Regular para que acepte el periodo con el formato 0000-00
                string sPattern = "^\\d{4}-\\d{2}$";
                bool existeperiodo = false;

                if (System.Text.RegularExpressions.Regex.IsMatch(this.TextNewPeriodo.Text, sPattern))
                {
                    foreach (var item in cboPeriodo.Items)
                    {
                        if (item.ToString() == this.TextNewPeriodo.Text)
                        {
                            existeperiodo = true;
                        }
                    }
                    if (existeperiodo)
                    {
                        this.toolTip1.Show("Ya existe el periodo", this.TextNewPeriodo, 2000);
                        this.TextNewPeriodo.Text = "";
                    }
                    else
                    {
                        cboPeriodo.Items.Add(this.TextNewPeriodo.Text);
                        this.toolTip1.Show("Periodo agregado Correctamente", this.TextNewPeriodo, 2000);
                        this.TextNewPeriodo.Text = "";
                    }

                }
                else
                {
                    this.toolTip1.Show("Formato del Periodo Invalido", this.TextNewPeriodo, 2000);
                    this.TextNewPeriodo.Text = "";
                }
            }
        }

        private void RBPRD_CheckedChanged(object sender, EventArgs e)
        {
            if (RBPrueba.Checked == true)
            {
                RBQAS.Checked = false;
                RBPrueba.Checked = false;
            }
        }

        private void RBQAS_CheckedChanged(object sender, EventArgs e)
        {
            if (RBQAS.Checked == true)
            {
                RBPRD.Checked = false;
                RBPrueba.Checked = false;
            }
        }

        private void RBPrueba_CheckedChanged(object sender, EventArgs e)
        {
            if (RBPrueba.Checked == true)
            {
                RBPRD.Checked = false;
                RBQAS.Checked = false;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult Confirmacion = new DialogResult();

            Confirmacion = MessageBox.Show("¿Está seguro que desea limpiar las tablas que se usan al momento de Importar y Exportar los archivos a SAP?",
                "Borrar datos de tablas", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

            if (Confirmacion == DialogResult.OK)
            {
                string Periodo = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el PERIODO el cual pertenecen los datos que desea eliminar con el formato aaaaMM", "PERIODO A BORRAR ", "Periodo", 100, 0);
                int num;
                bool isnum = int.TryParse(Periodo, out num);

                string Ruta = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la RUTA al cual pertenecen los datos que desea eliminar", "RUTA A BORRAR", "RUTA", 100, 0);
                int num2;
                bool isnum2 = int.TryParse(Ruta, out num2);


                if (Periodo.Length > 0)
                {

                    if (Periodo != "" & isnum == true)
                    {
                        if (Periodo.Length == 6)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            string CONSULTA = "SELECT ConexionID, Periodo FROM conexiones WHERE (Periodo = " + num + " OR Periodo = 0) AND Ruta = " + num2;
                            Vble.LimpiarConceptosFacturados(CONSULTA);
                            this.Cursor = Cursors.Default;

                        }
                        else
                        {
                            MessageBox.Show("Controle que el formato del periodo ingresado sea aaaaMM, por ejemplo: 201701", "Dato mal ingresado",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Disculpe para limpiar la base de datos y poder realizar una nueva importación deberá ingresar el periodo correcto", "Periodo incorrecto",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {


            Form0 Contenedor = new Form0();
            // se hace esto para que cargue todo el formulario inicial y luego pida clave
            this.Close();


            Contenedor.QueTimer = 1;
            Contenedor.timer1.Interval = 100;
            Contenedor.timer1.Enabled = true;
            DB.conexBD.Close();
            DB.con.Close();
            Application.Restart();



        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            //string HoraInicio = "6:40";
            //string HoraFin = "12:30";

            //var diferencia = Convert.ToInt32(Convert.ToDateTime(HoraFin).ToString("HHmm")) - Convert.ToInt32(Convert.ToDateTime(HoraInicio).ToString("HHmm"));

            //var Inicio = DateTime.ParseExact(Convert.ToDateTime(HoraFin).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture);
            //var Fin = DateTime.ParseExact(Convert.ToDateTime(HoraInicio).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture);

            //var dif = (Inicio - Fin).TotalHours;

            //MessageBox.Show("Hora de Inicio = " + HoraInicio + "\n" +
            //                "Hora de Fin = " + HoraFin + "\n" +
            //                "Cantidad de Horas en Ruta = "+ dif.ToString());

            //MessageBox.Show(DB.conexBD.ConnectionTimeout.ToString());

            //MessageBox.Show(TextNewPeriodo.Text.Replace("'", ""));

            //string SELECTLog = "SELECT * FROM LogErrores";
            //MySqlDataAdapter adapter = new MySqlDataAdapter(SELECTLog, DB.conexBD);
            //DataTable LogErroresTable = new DataTable();
            //adapter.Fill(LogErroresTable);
            //string Fecha_ddMMyyyy = "";
            //string Fecha_yyyyMMdd = "";
            //string UPDATEFecha = "";
            //foreach (DataRow item in LogErroresTable.Rows)
            //{
            //    Fecha_ddMMyyyy = item["Fecha"].ToString();

            //    Fecha_yyyyMMdd = Fecha_ddMMyyyy.Substring(6) + "/" + Fecha_ddMMyyyy.Substring(3, 2) + "/" + Fecha_ddMMyyyy.Substring(0, 2);

            //    MessageBox.Show("Fecha Actual: " + Fecha_ddMMyyyy + "\n Fecha Modificada: " + Fecha_yyyyMMdd);


            //    UPDATEFecha = "UPDATE LogErrores SET Fecha = '" + Fecha_yyyyMMdd + "' WHERE ConexionID = " + item["ConexionID"].ToString();
            //    MySqlCommand ComandoUPDATE = new MySqlCommand(UPDATEFecha, DB.conexBD);
            //    ComandoUPDATE.ExecuteNonQuery();
            //    ComandoUPDATE.Dispose();



            //}

            //adapter.Dispose();
            //LogErroresTable.Dispose();

            //_______________________________________________________________________________

            //SELECTLog = "SELECT * FROM LogErrores";
            //adapter = new MySqlDataAdapter(SELECTLog, DB.conexBD);
            //LogErroresTable = new DataTable();
            //adapter.Fill(LogErroresTable);
            //string FechaActual = "";
            //foreach (DataRow item in LogErroresTable.Rows)
            //{
            //    FechaActual = item["Fecha"].ToString();


            //}

            //OpenFileDialog SeleccionArchivo = new OpenFileDialog();            

            //if (SeleccionArchivo.ShowDialog() == DialogResult.OK)
            //{
            //    RutaRecibir = @SeleccionArchivo.FileName;
            //}

            //ImportarFacturasBGW.RunWorkerAsync();
        }

        private void ImportarFacturasBGW_DoWork(object sender, DoWorkEventArgs e)
        {

            int CantCFDistinct = 0;
            try
            {
                ///Declaracion de variables 
                string txSQL;
                Int32 ConexionID, Periodo;

                var lstItemFac = new List<List<string>>();


                SQLiteDataAdapter da;
                SQLiteCommandBuilder comandoSQL;

                DataTable TablaFDistinct = new DataTable();
                DataTable TablaFDescargarDistinct = new DataTable();
                DataTable tablaFac = new DataTable();
                SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseADescargar.Open();
                ///----------------------------------------------------------------------------------------
                ///
                /// 
                ///Obtengo todas las conexiones/usuarios registrados en la tabla Impresor para descargar en la 
                ///base MySQL
                ///
                txSQL = "SELECT DISTINCT ConexionID, Periodo FROM Facturas";
                SQLiteDataAdapter daD = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQLD = new SQLiteCommandBuilder(daD);
                daD.Fill(TablaFDistinct);

                //comandoSQLD.Dispose();
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los conceptos facturados por cada conexionID 
                txSQL = "SELECT DISTINCT ConexionID, Periodo FROM Facturas";
                da = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(da);
                da.Fill(TablaFDescargarDistinct);
                comandoSQL.Dispose();
                da.Dispose();


                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los conceptos facturados por cada conexionID 
                string txSQLCOUNT = "SELECT Count(*) FROM Facturas";
                //SQLiteDataAdapter daCOUNT = new SQLiteDataAdapter(txSQLCOUNT, BaseADescargar);
                SQLiteCommand comandoSQLCOUNT = new SQLiteCommand(txSQLCOUNT, BaseADescargar);
                int CANTIDAD = Convert.ToInt32(comandoSQLCOUNT.ExecuteScalar());
                comandoSQLCOUNT.Dispose();
                //daCOUNT.Dispose();

                CantidadFacturas = CANTIDAD;
                //PesodeTrabajo(CANTIDAD);


                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                if (TablaFDistinct.Rows.Count > 0)
                {
                    CantCFDistinct = TablaFDistinct.Rows.Count;
                    foreach (DataRow fi in TablaFDistinct.Rows)
                    {
                        //Elimina Registros al Exportar de la tabla Personas
                        string DeleteCF = "delete from FacturasBIS " +
                                          "where (ConexionID = " + Convert.ToInt32(fi["ConexionID"]) +
                                          " AND " + "Periodo = " + Convert.ToInt32(fi["Periodo"]) + ")";
                        MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                        cmdSQL2.ExecuteNonQuery();
                        cmdSQL2.Dispose();
                        //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                        //AvanceDescarga++;
                    }
                    comandoSQLD.Dispose();
                }
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                int contador = 0;
                string insertFacturas = "INSERT INTO FacturasBIS " +
                            "(ConexionID, Periodo, Grupo, " +
                            "Detalle1, Importe1, Resaltar1, " +
                            "Detalle2, Importe2, Resaltar2, " +
                            "Detalle3, Importe3, Resaltar3, " +
                            "Detalle4, Importe4, Resaltar4, " +
                            "Detalle5, Importe5, Resaltar5, " +
                            "Detalle6, Importe6, Resaltar6, " +
                            "Detalle7, Importe7, Resaltar7, " +
                            "Detalle8, Importe8, Resaltar8, " +
                            "Detalle9, Importe9, Resaltar9, " +
                            "Detalle10, Importe10, Resaltar10) " +
                            "VALUES ";

                List<string> ListaRegisros = new List<string>();
                int t = 0;

                foreach (DataRow fi in TablaFDescargarDistinct.Rows)
                //foreach (DataRow Fila in Tabla.Rows)
                {
                    contador++;
                    ConexionID = Convert.ToInt32(fi["ConexionID"]);
                    Periodo = Convert.ToInt32(fi["Periodo"]);

                    tablaFac.Clear();


                    string txSQLReg = "SELECT * FROM Facturas WHERE ConexionID = " + ConexionID;
                    SQLiteDataAdapter daReg = new SQLiteDataAdapter(txSQLReg, BaseADescargar);
                    SQLiteCommandBuilder comandoSQLReg = new SQLiteCommandBuilder(daReg);
                    daReg.Fill(tablaFac);
                    comandoSQLReg.Dispose();
                    daReg.Dispose();
                    int group = 0;




                    //foreach (DataRow item in tablaFac.Rows)
                    //{
                    int i = 0;
                    while (i * 10 < tablaFac.Rows.Count)
                    {
                        lstItemFac = new List<List<string>>();
                        for (int j = 0; j < 10; j++)
                        {
                            if (j + i * 10 < tablaFac.Rows.Count)
                            {
                                lstItemFac.Add(new List<string>()
                                                                    {tablaFac.Rows[j+i*10]["Detalle"].ToString(),
                                                                     tablaFac.Rows[j+i*10]["Importe"].ToString(),
                                                                     tablaFac.Rows[j+i*10]["Resaltar"].ToString()});
                                ImportarFacturasBGW.ReportProgress(t);
                                t++;

                            }
                            else
                            {
                                lstItemFac.Add(new List<string>() { "", "", "0" });
                            }
                        }
                        group = i;
                        i++;
                        //Armar metodo del registro (i+1)= grupo
                        string s10ItemsFact = $"({ConexionID}, {Periodo}," +
                                                  $"{group.ToString()}";
                        for (int l = 0; l < lstItemFac.Count; l++)
                        {
                            s10ItemsFact += $",'{lstItemFac[l][0]}', '{lstItemFac[l][1]}', {lstItemFac[l][2]}";

                        }
                        s10ItemsFact += ")";
                        ListaRegisros.Add(s10ItemsFact);


                    }

                }

                float Prom = lstItemFac.Count / ListaRegisros.Count;
                //MessageBox.Show(ListaRegisros.Count.ToString());
                string sep = "";
                MySqlCommand command2 = new MySqlCommand();
                int CantImpor = 0;
                int Importadas = 0;
                int k = 499;
                int y = 0;
                for (y = Importadas; Importadas < ListaRegisros.Count; y = y + k)
                {
                    var insertTemp = insertFacturas;
                    for (int j = 0; j < k & j < ListaRegisros.Count; j++)
                    {
                        if (Importadas == ListaRegisros.Count)
                        {

                        }
                        else
                        {
                            insertTemp += $"{sep}{ListaRegisros[Importadas]}";
                            sep = ",";
                            CantImpor = j;
                            Importadas++;
                        }

                    }

                    CantImpor = (int)(CantImpor * Prom);

                    //preparamos la cadena pra insercion
                    command2 = new MySqlCommand(insertTemp, DB.conexBD);
                    //y la ejecutamos
                    command2.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command2.Dispose();
                    sep = "";
                }
                daD.Dispose();
                comandoSQLD.Dispose();
                BaseADescargar.Close();
                //                ProgressBarImportarFact.Maximum = 100;
                //---------------------------------------------------------------------------------------------------------

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Cargar la tabla Facturas");
            }
        }

        private void PesodeTrabajo(int count)
        {
            Thread.Sleep(count);
        }

        private void ImportarFacturasBGW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            ProgressBarImportarFact.Visible = true;
            //PorcLabel.Visible = true;
            ProgressBarImportarFact.Value = (e.ProgressPercentage * 100) / (CantidadFacturas);
            ProgressBarImportarFact.Text = ((e.ProgressPercentage * 100) / CantidadFacturas).ToString();

        }

        private void ImportarFacturasBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //ProgressBarImportarFact.Value = 100;
            ProgressBarImportarFact.Text = "100";
            MessageBox.Show("Se completo la Importacon", "Completo!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ProgressBarImportarFact.Visible = false;
            ProgressBarImportarFact.Value = 0;

        }

        private void ListRutas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PBExistArchImpNO_Click(object sender, EventArgs e)
        {

        }

        private void btnCargas_KeyPress(object sender, KeyPressEventArgs e)
        {
            Vble.ShowLoading();
        }



        private void LVResImpor_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LVResImpor.SelectedItems)
            {
                Vble.PorcionImp = item.Text;
                Vble.TotalUsuariosImp = item.SubItems[1].Text;
                Vble.TotalImportados = item.SubItems[2].Text;
                Vble.TotalApartados = item.SubItems[3].Text;
                Vble.IDLogImportacion = item.SubItems[4].Text;

            }
        }

        private void LVResImpor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LVResImpor.SelectedItems.Count == 1)
            {
                LogImpApartados logImp = new LogImpApartados();
                logImp.TxtPorcion.Text = Vble.PorcionImp;
                logImp.TxtTotalUsuarios.Text = Vble.TotalUsuariosImp;
                logImp.TxtImportados.Text = Vble.TotalImportados;
                logImp.TxtApartados.Text = Vble.TotalApartados;

                if (Convert.ToInt16(Vble.TotalApartados) > 0)
                {
                    DataTable TableLogImportado = new DataTable();

                    string txSQL = "SELECT * FROM LogImportacion WHERE IDLogImportacion = " + Vble.IDLogImportacion;
                    MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(TableLogImportado);
                    datosAdapter.Dispose();
                    comandoSQL.Dispose();

                    InfoImportacion.Visible = true;
                    ListViewItem ResumenImportacion;
                    ResumenImportacion = new ListViewItem();

                    if (TableLogImportado.Rows.Count >= 1)
                    {
                        foreach (DataRow item in TableLogImportado.Rows)
                        {
                            string Detalle = item["DetalleApartados"].ToString();
                            string[] Instalaciones = Detalle.Split(';');
                            for (int i = 0; i < Instalaciones.Length; i++)
                            {
                                logImp.LVDetalle.Items.Add(new ListViewItem(Instalaciones[i]));
                            }

                        }
                    }
                    logImp.Show();
                }
                else
                {
                    MessageBox.Show("La ruta que selecciono se importo correctamente en su totalidad, " +
                                    "no contiene usuarios apartados", "Ruta sin usuarios apartados",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
        }

        private void LabelVersion_Click(object sender, EventArgs e)
        {

        }

        private void generarTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Modulo en desarrollo, se habilitará cuando el administrador deje optimo para su uso.", "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            FormNuevoTicket PantallaTicket = new FormNuevoTicket();
            PantallaTicket.Show();
        }
    }
} 
