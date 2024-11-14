/*
 * Creado por SharpDevelop.
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 13:57
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Management;
using Microsoft.SmartDevice.DeviceAgentTransport;
using WindowsPortableDevicesLib.Domain;
using WindowsPortableDevicesLib;
using GongSolutions.Shell;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualBasic.Devices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading;
using System.Xml.Linq;

namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form2.
    /// </summary>
    /// 
   
    public partial class Form3Descargas : Form {

        //private DriveDetector driveDetector = null;
        
        StandardWindowsPortableDeviceService service = new StandardWindowsPortableDeviceService();
        

        public Form3Descargas() {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //CrearArchivoInfoDescarga
            InitializeComponent();
            


            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void Form3_Load(object sender, System.EventArgs e) {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            toolTip1.SetToolTip(btnDescargar, "Realiza la descarga de las conexiones de la Colectora a la PC ");
            toolTipTodos.SetToolTip(btnResumTodo, "Muestra resumenes de descarga pertenecientes al periodo en el que se esta trabajando");

            //Mensaje de entrada para seleccionar datos de la colectora
            //DialogResult resul = new DialogResult();
            //resul = MessageBox.Show("¿Desea realizar la descarga de la colectora al sistema?", "Buscar Colectora", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            //if (resul == DialogResult.OK)
            //{
            //    mensaje();
            //}
            //----------------------------------------------------------------


            //CargarCarpetas();
            ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
            shellViewDescargas.CurrentFolder = folder;
            timer1_Tick(sender, e);

        }

        



        //abre cuadro de seleccion de archivos si se opto por
        //elegir carga de archivos a colectoras o viceversa al abrir el form
        public void mensaje()
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                System.IO.StreamReader(openFileDialog1.FileName);
                MessageBox.Show(sr.ReadToEnd());
                sr.Close();
            }
        }


        private void Form3_Resize(object sender, System.EventArgs e) {
            //this.WindowState = FormWindowState.Maximized;
        }
        
        /// <summary>
        /// Carga la lista de carpetas con las existentes en el archivo
        /// ini de configuración
        /// </summary>
        private void CargarCarpetas() {
            int i = 0;
            string sC, sK, sI, localidad;
            StringBuilder stB = new StringBuilder();
            lsCarpetas.Items.Clear();

            do {
                i++;
                sC = "Carpeta" + i.ToString("00");
                Inis.GetPrivateProfileString
                    ("Carpetas Descargas", sC, "NO", stB, 250, Ctte.ArchivoIniName);
                sI = stB.ToString().Trim();
                sK = sI.ToUpper();
                localidad = Vble.ValorarUnNombreRuta(sK);
                if ((sI.Length > 0) && (sK != "NO")) {
                    if (!lsCarpetas.Items.ContainsKey(sK))
                        //lsCarpetas.Items.Add(sK, sI, 0);
                        lsCarpetas.Items.Add(localidad);
                    //lsCarpetas.Items.Add(localidad, sI, 0);
                }
            } while (stB.ToString().Trim().ToUpper() != "NO");


        }

        /// <summary>
        /// Guarda en el archivo ini de configuración, las carpetas que están 
        /// cargadas en la lista de carpetas de descargas, en el orden en
        /// que están en la lista
        /// </summary>
        private void GuardarCarpetas() {
            int i;
            string sC;
            //Antes de guardar las carpetas, limpa la seccion en el archivo ini
            BorraCarpetasEnIni();

            for (i = 0; i < lsCarpetas.Items.Count; i++) {
                sC = "Carpeta"+(i+1).ToString("00");
                Inis.WritePrivateProfileString("Carpetas Descargas", sC,
                    lsCarpetas.Items[i].Text.Trim(), Ctte.ArchivoIniName);
            }

        }

        /// <summary>
        /// Borra todas las carpetas cargadas en el ini, para evitar posibilidad
        /// de que queden elementos repetidos al actualizar.
        /// </summary>
        private void BorraCarpetasEnIni() {
            int i = 0;
            string sC;
            StringBuilder stB = new StringBuilder();
            
            do {
                i++;
                sC = "Carpeta" + i.ToString("00");
                Inis.GetPrivateProfileString(
                   "Carpetas Descargas", sC, "NO", stB, 250, Ctte.ArchivoIniName);
                if  (stB.ToString().Trim().ToUpper() != "NO")
                    //Si existe key, limpia el valor
                    Inis.WritePrivateProfileString(
                        "Carpetas Descargas", sC, "", Ctte.ArchivoIniName);
            } while (stB.ToString().Trim().ToUpper() != "NO");

        }

        /// <summary>
        /// Lee de la tabla "descargas" aquellas que pertenecen al periodo en el cual se esta trabajando cuando se selecciona el periodo
        /// 
        /// </summary>
        private void LeerInformes(int periodo)
        {
            tvInformes.Nodes.Clear();
            DataTable Tabla = new DataTable();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string rutaInformes = Vble.CarpetaTrabajo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas);
            DirectoryInfo RutaDescargas = new DirectoryInfo(rutaInformes);
            try
            {    
                txSQL = "SELECT * FROM descargas";
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                if (Tabla.Rows.Count > 0)
                {               
                    foreach (DataRow fi in Tabla.Rows)
                    {                    
                        if (fi[1].ToString() == periodo.ToString())
                        {                        
                        TreeNode nodoraiz = tvInformes.Nodes.Add(fi[3].ToString());
                        nodoraiz.Nodes.Add("Equipo: " + fi[4].ToString());
                        nodoraiz.Nodes.Add("Lecturista: " + fi[5].ToString());
                        string infoconexion = "Informacion de Conexiones: ";
                        nodoraiz.Nodes.Add(infoconexion);
                        nodoraiz.LastNode.Nodes.Add(fi[6].ToString());
                        string cantidadConexiones = "Cantidad de Conexiones: " + fi[7].ToString();
                        nodoraiz.Nodes.Add(cantidadConexiones);
                        nodoraiz.LastNode.Nodes.Add("No Leidos: " + fi[8].ToString());
                        nodoraiz.LastNode.Nodes.Add("Leidas NO Impresas: " + fi[9].ToString());
                        nodoraiz.LastNode.Nodes.Add("Leidas Impresas: " + fi[10].ToString());
                        nodoraiz.LastNode.Nodes.Add("NO Impresas Impresora Desconectada: " + fi[11].ToString());
                        nodoraiz.LastNode.Nodes.Add("NO Impresas Fuera de Rango: " + fi[12].ToString());
                        nodoraiz.LastNode.Nodes.Add("NO Impresas Estado Negativo: " + fi[13].ToString());
                        nodoraiz.LastNode.Nodes.Add("NO Impresas Error Dato: " + fi[14].ToString());
                        nodoraiz.LastNode.Nodes.Add("NO Impresas Domicilio Postal: " + fi[15].ToString());
                        nodoraiz.LastNode.Nodes.Add("NO Impresas Indicado Dato: " + fi[16].ToString());
                        nodoraiz.LastNode.Nodes.Add("Imposible Leer: " + fi[17].ToString());
                        nodoraiz.LastNode.Nodes.Add("Sub Total Negativo: " + fi[18].ToString());
                        nodoraiz.LastNode.Nodes.Add("Error al Archivar Datos: " + fi[19].ToString());
                        nodoraiz.LastNode.Nodes.Add("Error en Nº de Factura: " + fi[20].ToString());
                        nodoraiz.LastNode.Nodes.Add("Sin Conceptos a Facturar: " + fi[21].ToString());
                        nodoraiz.LastNode.Nodes.Add("Error al Facturar: " + fi[22].ToString());
                        nodoraiz.LastNode.Nodes.Add("Periodo Excedido en Días: " + fi[23].ToString());
                        tvInformes.Nodes.Add("--------------------------------------------------------------------------------");

                         }
                      }
                  }
              }
                catch(Exception)
                {

                }
            }

        /// <summary>
        /// Genera un documento .pdf con el informe correspondiente al periodo que se esta trabajando
        /// </summary>
        /// <param name="periodo"></param>
        private void GenerarInformeDescarga(int periodo)
        {
            tvInformes.Nodes.Clear();
            DataTable Tabla = new DataTable();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string rutaInformes = Vble.CarpetaTrabajo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas);
            DirectoryInfo RutaDescargas = new DirectoryInfo(rutaInformes);
            try
            {
                txSQL = "SELECT * FROM descargas";
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                //Variable que contendra el total de Conexiones descargadas pertenecientes al periodo en el que se esta trabajando
                int TotalGeneral = 0;

                string informe;
                

                if (Tabla.Rows.Count > 0)
                {
                    
                    foreach (DataRow fi in Tabla.Rows)
                    {
                        if (fi[1].ToString() == periodo.ToString())
                        {
                            TotalGeneral = TotalGeneral + Convert.ToInt32(fi[7]);
                            Vble.lineas += "------------------------------------------------------------------------------\n";
                            Vble.lineas += "Descarga: " + fi[3].ToString() + "\n" + "Información de la descarga: \n" + fi[6].ToString() +
                                           "Cantidad Conexiones: " + fi[7].ToString() + "\n" +
                                           "Estado de Conexiones luego de terminar el recorrido: \n" +
                                           "    Leidas e Impresas: " + fi[10].ToString() + "\n" +
                                           "    No Leidas: " + fi[8].ToString() + "\n" +
                                           "    Leidas NO Impresas: " + fi[8].ToString() + "\n" +
                                           "    NO Impresas Impresora Desconectada: " + fi[11].ToString() + "\n" +
                                           "    NO Impresas Fuera de Rango: " + fi[12].ToString() + "\n" +
                                           "    NO Impresas Estado Negativo: " + fi[13].ToString() + "\n" +
                                           "    NO Impresas Error Dato: " + fi[14].ToString() + "\n" +
                                           "    NO Impresas Domicilio Postal: " + fi[15].ToString() + "\n" +
                                           "    NO Impresas Indicado Dato: " + fi[16].ToString() + "\n" +
                                           "    Imposible Leer: " + fi[17].ToString() + "\n" +
                                           "    Sub Total Negativo: " + fi[18].ToString() + "\n" +
                                           "    Error al Archivar Datos: " + fi[19].ToString() + "\n" +
                                           "    Error en Nº de Factura: " + fi[20].ToString() + "\n" +
                                           "    Sin Conceptos a Facturar: " + fi[21].ToString() + "\n" +
                                           "    Error al Facturar: " + fi[22].ToString() + "\n" +
                                           "    Periodo Excedido en Días: " + fi[23].ToString() + "\n";    
                        }
                    }
                }

                Document document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document,
                              new FileStream("Informe Periodo "+Vble.Periodo+".pdf",
                                     FileMode.OpenOrCreate));
                document.Open();
                document.Add(new Paragraph("Informe de Descargas pertenecientes al Periodo: "+ periodo + "\n\n\n" +
                                           "Total de Conexiones Descargadas del periodo: " +TotalGeneral +"\n"+ Vble.lineas));
                document.Close();

                
            }
            catch (Exception)
            {

            }
        }

            #region REGION BOTONES 

            void btnCerrar_Click(object sender, EventArgs e) {
            this.Close();
        }

        /// <summary>
        /// Tilda todas las carpetas de descargas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarpTodo_Click(object sender, EventArgs e) {
            foreach (ListViewItem Itm in lsCarpetas.Items)
                Itm.Checked = true;

        }

        /// <summary>
        /// Saca el tilde de todas las carpetas de descargas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarpNada_Click(object sender, EventArgs e) {
            foreach (ListViewItem Itm in lsCarpetas.Items)
                Itm.Checked = false;
        }
        
        /// <summary>
        /// Elimina la carpeta seleccionada de la lista de carpeta de descargas
        /// Pide confirmación, lo que hace en realidad es eliminarla del 
        /// archivo ini de configuración
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitar_Click(object sender, EventArgs e) {
            ListViewItem Itm = new ListViewItem();
            if (lsCarpetas.SelectedItems.Count > 0) {
                lsCarpetas.HideSelection = false;
                Itm = lsCarpetas.SelectedItems[0];
                int idx = Itm.Index;
                if (MessageBox.Show("Está seguro que desea quitar la carpeta \n" +
                    Itm.Text.Trim() + "\nde la lista de carpetas de descargas??",
                    "Quitar Carpeta de Descarga", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes) {
                    //Confirmado la quita
                    lsCarpetas.Items.Remove(Itm);
                    GuardarCarpetas();
                    CargarCarpetas();
                }
            }
            else
                MessageBox.Show("No hay carpeta de descarga seleccionada",
                    "Quitar carpeta de descarga", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            lsCarpetas.HideSelection = true;
        }

        /// <summary>
        /// Agregar una nueva carpeta a la lista de descargas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e) {
            FolderBrowserDialog opDir = new FolderBrowserDialog();
            if (lsCarpetas.Items.Count > 0)
                opDir.SelectedPath = Directory.GetParent(lsCarpetas.Items[0].Text).ToString();
            opDir.Description = "Carpeta de Decargas de Colectoras";
            opDir.ShowNewFolderButton = true;
            

            if (opDir.ShowDialog(this) == DialogResult.OK) {
                string sK, sI;
                //Si no existe, agregar
                sI = opDir.SelectedPath.Trim();
                sK = sI.ToUpper();
                
                if (!lsCarpetas.Items.ContainsKey(sK)) {
                    lsCarpetas.Items.Add(sK, sI, 0);
                    GuardarCarpetas();
                }
            }
        }









        #endregion  //Fin Region BOTONES

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e) {

        }
        /// <summary>
        ///Lee y muestra en cmbDevice Dispositivos la colectora que esta conectada en caso de que lo esté
        /// </summary>        
        public void TomaColectoraConectada()
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {       
            try
            {
                cmbDevices.Items.Clear();
                //shellView2.RefreshContents();
                IList<WindowsPortableDevice> devices = service.Devices;
                devices.ToList().ForEach(device =>
                {
                    device.Connect();
                    if (Funciones.BuscarColectora(device.ToString()))
                    {
                        var contents = device.GetContents();
                        cmbDevices.Items.Add(device.ToString());
                        device.Disconnect();
                    }
                });
                if (devices.Count > 0)
                {
                    cmbDevices.SelectedIndex = 0;
                }
                else
                {
                    ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
                    shellViewDescargas.CurrentFolder = folder;
                }

            }
            catch (Exception)
            {
            }
            //TomaColectoraConectada();
            //LeerInformes();
            
        }

        private void lsCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

           
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void cmbDevices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {           
            //Lee y obtiene el nombre de la base Sqlite
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string ArchivoBase = stb1.ToString();
            //recorre los dispositivos conectados y va consultando si existe alguna colectora Con la denominación "MICC-0000"
            foreach (var item in shellViewDescargas.CurrentFolder)
            {
                if (item.DisplayName == cmbDevices.Text)
                {
                    //shellView2.Visible = true;
                    //recorre las unidades que contiene la colectora y busca el directorio Raiz "\" para ingresar
                    foreach (var raiz in item)
                    {
                        if (raiz.DisplayName == "\\")                        {
                            //recorre las subcarpetas del directorio Raiz y busca la carpeta a la cual se van a enviar los archivos desde la PC
                            //vble.DestinoArchivoColectora = "Datos DPEC" en archivo.ini
                            foreach (var carpeta in raiz)
                            {
                                if (carpeta.DisplayName == Vble.DestinoArchivosColectora)
                                {
                                    //shellView2.CurrentFolder = carpeta;                                    
                                    foreach (var destino in carpeta)
                                    {
                                        //////determina como directorio del shellview a mostrar el que se especifico buscar en el archivo.ini
                                        if (destino.DisplayName == Vble.CarpetaDestinoColectora)
                                        {
                                                //declaro algunas variables que se utilizan para la DESCARGA de conexiones 
                                            string Dispositivo = Funciones.BuscarNombreColectora(Vble.DirectorioColectoraenPC);                                                
                                            Vble.Colectora = Dispositivo + cmbDevices.Text + "\\" + Vble.CarpetaDestinoColectora + "\\";
                                            Vble.RutaColectoraConectada  = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora; 
                                            DirectoryInfo di = new DirectoryInfo(Vble.RutaColectoraConectada); 
                                            Vble.ArchivoInfoCargaColectora = Vble.RutaColectoraConectada + "InfoCarga.txt";                                            
                                            Vble.RutaBaseSQLiteColectora = Vble.RutaColectoraConectada + ArchivoBase;
                                            Vble.ColectoraConectada = cmbDevices.Text;

                                            if (File.Exists(Vble.RutaBaseSQLiteColectora))
                                            {
                                                DialogResult = MessageBox.Show("La Colectora contiene la Carga: \n" + Funciones.LeerArchivostxt(Vble.RutaColectoraConectada + "InfoCarga.txt") 
                                                                                + "Desea comenzar la descarga de los Rutas?", "Descarga de Rutas", 
                                                                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                                if (DialogResult == DialogResult.OK)
                                                {
                                                    lsDesc.Items.Clear();
                                                    Vble.InfoDescarga = Funciones.LeerArchivostxt(Vble.RutaColectoraConectada + "InfoCarga.txt");
                                                    MostrarRutasDeColectoras(Vble.RutaBaseSQLiteColectora);
                                                    btnDescargar.Visible = true;
                                                }                                              
                                                
                                            }
                                            else
                                            {
                                                    MessageBox.Show("La colectora no contiene Conexiones para descargar", "Colectora Vacia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        else
                                        {
                                            //MessageBox.Show("Disculpe no se encuentra la carpeta destino", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            shellViewDescargas.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            }
            catch (Exception R)
            {
                MessageBox.Show(R.Message.Substring(0,31) + " de informacion de las conexiones", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Rellena la lista de Conexiones que se encuentra en la colectora para visualizar los datos y comenzar con la descarga de 
        /// las conexiones tanto leidas como no leidas
        /// </summary>
        /// <param name="RutaDatos"></param>
        private void MostrarRutasDeColectoras(string RutaDatos)
        {
            DataTable Tabla = new DataTable();

            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;              
                SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
                BaseACargar.Open();

                txSQL = "SELECT * FROM Conexiones";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);                

                foreach (DataRow fi in Tabla.Rows)
                {
                    ListViewItem Datos = new ListViewItem(fi[1].ToString());//Columna Periodo                    
                    foreach (DataRow colum in Tabla.Rows)
                    {
                        Datos.SubItems.Add(fi[0].ToString());//Columna Conex   
                        Datos.SubItems.Add(fi[15].ToString());//Columna Ruta
                        Datos.SubItems.Add(fi[10].ToString());//Columna Estado
                        Datos.SubItems.Add(cmbDevices.Text);//Columna Dispositivo
                        Datos.SubItems.Add(fi[17].ToString());//Columna Remesa
                        Datos.SubItems.Add(SubconsultaDistrito(fi[7].ToString())[0].ToString() + 
                                           " - " + SubconsultaDistrito(fi[7].ToString())[2].ToString());//Columna Distrito y Localidad   
                        Vble.Distrito = Convert.ToInt32(SubconsultaDistrito(fi[7].ToString())[0].ToString());
                        Datos.SubItems.Add(SubconsultaFechaLectura(BaseACargar, fi[0].ToString())[0].ToString() + " - Hora: " +
                                           SubconsultaFechaLectura(BaseACargar, fi[0].ToString())[1].ToString()); //Columna Fecha y Hora
                        Datos.SubItems.Add(fi[12].ToString());//Columna Operario                        
                        
                        //Datos.SubItems.Add(fi[10].ToString());
                    }
                    lsDesc.Items.Add(Datos);                    
                }
                comandoSQL.Dispose();
                datosAdapter.Dispose();
                BaseACargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        /// <summary>
        /// Devuelve datos del Distrito como ser Codigo Interno, localidad y provincia recibiendo como parametro el codigo postal
        /// y la ruta donde se encuentra la base de datos que contiene la colectora
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="conex"></param>
        /// <returns></returns>
        private ArrayList SubconsultaDistrito(string CodigoPostal)
        {
            DataTable Tabla = new DataTable();
            ArrayList DatosDistrito = new ArrayList();      

            try
            {
                string txSQL;
                MySqlDataAdapter datosAdapter;
                MySqlCommandBuilder comandoSQL;              

                txSQL = "SELECT CodigoInt, Provincia, Localidad FROM Localidades WHERE CodigoPostal = " + CodigoPostal;
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);              

                foreach (DataRow fi in Tabla.Rows)
                {
                    DatosDistrito.Add(fi[0].ToString());//CodigoInt
                    DatosDistrito.Add(fi[1].ToString());//Provincia
                    DatosDistrito.Add(fi[2].ToString());//Localidad
                    //CodigoInt = fi[0].ToString();
                }
                return DatosDistrito;
            }

            catch (Exception r )
            {
                MessageBox.Show(r.Message);
            }
          
            return DatosDistrito;
        }


        /// <summary>
        /// Devuelve la fecha de lectura de cada conexion y la muestra en la lista 
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="conex"></param>
        /// <returns></returns>
        private ArrayList SubconsultaFechaLectura(SQLiteConnection BaseACargar, string conex)
        {
            DataTable Tabla = new DataTable();
            ArrayList Fechas = new ArrayList();
            //SQLiteConnection BaseACargar = new SQLiteConnection(RutaDatos);
            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;

                txSQL = "SELECT ActualFecha, ActualHora FROM Medidores WHERE conexionID = " + conex;
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    Fechas.Add((fi[0]));//FechaLectura
                    Fechas.Add(fi[1]);//HoraLectura   
                }                
                //comandoSQL.Dispose();
                datosAdapter.Dispose();
                BaseACargar.Close();
                return Fechas;
            }

            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            
            BaseACargar.Close();
            return Fechas;
        }

        /// <summary>
        /// Cambia el estado impresionOBS de cada conexion a 05xx que se esta descargando a la colectora de la base
        /// MySQL general donde estan todas las conexiones, respetando los codigos de impresion que se editaron en la lectura
        /// </summary>
        /// 
        public void CambiarEstadoRecibidoMySql(string RutaRecibir, int StatusChange)
        {
            DataTable Tabla = new DataTable();
            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;
                Int32 conexionID;
                Int32 ImpresionOBS;
               
                SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseACargar.Open();

                txSQL = "SELECT conexionID, ImpresionOBS, Operario, Periodo FROM Conexiones";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {                   
                    //asignación a variables locales para manejar en el UPDATE
                    conexionID = Convert.ToInt32(fi[0]);
                    ImpresionOBS = Convert.ToInt32(fi[1]);
                    Vble.Operario = Convert.ToInt32(fi[2]);
                    Vble.PeriodoEnColectora = Convert.ToInt32(fi[3]);
                    Vble.CantConex = Tabla.Rows.Count;

                    
                        string update;//Declaración de string que contendra la consulta UPDATE               
                        update = "UPDATE conexiones SET ImpresionOBS = " + (ImpresionOBS + (StatusChange * 100)) + " WHERE conexionID = " + conexionID;
                        //preparamos la cadena pra insercion
                        MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                    //comandoSQL.Dispose();


                }
                MessageBox.Show("La descarga se realizo con exito", "Descarga de Conexiones", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comandoSQL.Dispose();
                datosAdapter.Dispose();                
                BaseACargar.Close();
            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message);
            }
        }

        /// <summary>
        ///Metodo que genera la carpeta de Secuencias Seleccionadas Descargadas de acuerdo a parametros de creacion como Periodo,
        ///Distrito, Carga Nº, Fecha de generación del procesamiento.
        /// </summary>
        private void GenerarCarpetaDescarga()
        {       
        
            string ArchivoTabla;
            string archivosecuencia;
            string Carp;
            int Carga = 0;
            StringBuilder stb = new StringBuilder();

            //Lee y obtiene el nombre de la base Sqlite                        
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string archivo = stb1.ToString();

            //Leer y actualizar el número de Descarga
            Inis.GetPrivateProfileString("Numeros Descargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
            Carga = int.Parse(stb.ToString()) + 1;
            Inis.WritePrivateProfileString("Numeros Descargas", Vble.Distrito.ToString(),
            Carga.ToString().Trim(), Ctte.ArchivoIniName);

            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
            CultureInfo.CurrentCulture);
            Vble.NomCarpDescarga = string.Format("DP{0:yyyyMM}_{1}_D{2:00000}.{3:yyMMdd_HHmm}", Per,
                cmbDevices.Text, Carga, DateTime.Now);                        
           
            //Genero Ruta donde se va a almacenar datos de la descarga
            ArchivoTabla = Vble.CarpetaTrabajo  + "\\" +Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasRecibidas) + Vble.NomCarpDescarga;

            //Genero la carpeta que contendra el archivo de base sqlite de la descarga realizada.
            Vble.CrearDirectorioVacio(ArchivoTabla);
            string destino = ArchivoTabla + "\\" + archivo;
            
            //copia el archivo de base de datos Sqlite al directorio generado anteriormente.
            CopiaArchivos(Vble.RutaBaseSQLiteColectora, destino);

            ////A PARTIR DE ACA GENERO ARCHIVO CON DATOS DE LA CARGA QUE SE PROCESO 
            ////COMO SER RUTA, SECUENCIA, CANTIDAD DE REGISTROS CARGADOS, ETC

            string filename = "InfoDescarga.txt";
            archivosecuencia = System.IO.Path.Combine(ArchivoTabla, filename);
            ////Llamo al metodo que crea el archivo InfoCarga.txt que contiene informacion de la carga procesada
            CrearArchivoInfoDescarga(Vble.RutaColectoraConectada, cmbDevices.Text, Vble.NomCarpDescarga, filename, ArchivoTabla);      

    }
        /// <summary>
        /// Copiar archivo
        /// </summary>
        public static void CopiaArchivos(string RutaOrigen, string RutaDestino)
        {
            Computer mycomputer = new Computer();
            try
            {
                if (RutaOrigen != "" && RutaDestino != "")
                {
                    mycomputer.FileSystem.CopyFile(RutaOrigen, RutaDestino);
                }
                else
                {
                    MessageBox.Show("No se genero correctamente el Archivo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Funcion que crea el archivo vacio, con la secuencia como nombre para utilizar como información a la hora de mostrar en el listview1
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        private void CrearArchivoInfoDescarga(string archivosecuencia, string colectora, string carp, string filename, string rutadescargada)
        {
            try
            {
                StringBuilder stb1 = new StringBuilder("", 250);
                Inis.GetPrivateProfileString("Archivos", "NombreArchivoInfoDescarga", "", stb1, 250, Ctte.ArchivoIniName);
                string nombrearchivo = stb1.ToString();
                string Descarga;
                int i = 0;               
                StringBuilder stB = new StringBuilder();
                lsCarpetas.Items.Clear();

                DirectoryInfo directorio = new DirectoryInfo(archivosecuencia);

                Descarga = carp;
                TreeNode nodoraiz = tvInformes.Nodes.Add(Descarga);
                nodoraiz.Nodes.Add("Equipo: " + colectora);
                nodoraiz.Nodes.Add("Lecturista: " + Vble.Operario);
                string infoconexion = "Informacion de Conexiones: ";
                nodoraiz.Nodes.Add(infoconexion);
                nodoraiz.LastNode.Nodes.Add(Vble.InfoDescarga);
                string cantidadConexiones = "Cantidad de Conexiones: " + Vble.CantConex;                
                nodoraiz.Nodes.Add(cantidadConexiones);
                nodoraiz.LastNode.Nodes.Add("No Leidos: " + Vble.ConexNoLeidas);
                nodoraiz.LastNode.Nodes.Add("Leidas NO Impresas: " + Vble.ConexLeidasNoImpresas);
                nodoraiz.LastNode.Nodes.Add("Leidas Impresas: " + Vble.ConexLeidasImpresas);
                nodoraiz.LastNode.Nodes.Add("NO Impresas Impresora Desconectada: " + Vble.ConexNoImpresasImpresoraDesc);
                nodoraiz.LastNode.Nodes.Add("NO Impresas Fuera de Rango: " + Vble.ConexNoImpresasFueradeRango);
                nodoraiz.LastNode.Nodes.Add("NO Impresas Estado Negativo: " + Vble.ConexNoImpresasEstadoNegativo);
                nodoraiz.LastNode.Nodes.Add("NO Impresas Error Dato: " + Vble.ConexNoImpresasErrorDato);
                nodoraiz.LastNode.Nodes.Add("NO Impresas Domicilio Postal: " + Vble.ConexNoImpresasDomicilioPostal);
                nodoraiz.LastNode.Nodes.Add("NO Impresas Indicado Dato: " + Vble.ConexNoImpresasIndicadoDato);
                nodoraiz.LastNode.Nodes.Add("Imposible Leer: " + Vble.ConexImposibleLeer);
                nodoraiz.LastNode.Nodes.Add("Sub Total Negativo: " + Vble.ConexSubtNeg);
                nodoraiz.LastNode.Nodes.Add("Error al Archivar Datos: " + Vble.ConexErrorArchDatos);
                nodoraiz.LastNode.Nodes.Add("Error en Nº de Factura: " + Vble.ConexErrorNFact);
                nodoraiz.LastNode.Nodes.Add("Sin Conceptos a Facturar: " + Vble.ConexSinConcepFacturar);
                nodoraiz.LastNode.Nodes.Add("Error al Facturar: " + Vble.ConexErrorFacturando);
                nodoraiz.LastNode.Nodes.Add("Periodo Excedido en Días: " + Vble.ConexPerExcDias);
                nodoraiz.LastNode.Nodes.Add("--------------------------------------------------------------------------------");

                PrintRecursive(nodoraiz);                
                CreateInfoCarga(rutadescargada + "\\" + filename, filename, Vble.lineas);

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// recorre recursivamente los nodos de la descarga que se esta generando para almacenar datos en Vble.Lineas que luego
        /// se utiliza para generar archivo txt con datos
        /// </summary>
        /// <param name="treeNode"></param>
        private void PrintRecursive(TreeNode treeNode)
        {
            // Print the node.
            
            Vble.lineas += treeNode.Text +"\n";
            // Print each node recursively.
            foreach (TreeNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn);
            }
        }
        //Crea archivo en la ruta que se pasa como parametro en variable 
        //archivosecuencia, con el nombre y los datos dentro
        public void CreateInfoCarga(string archivosecuencia, string filename, string lineas)
        {
            //Creo el archivo que contiene la información de la carga que se proceso y esta lista para enviar
            if (!System.IO.File.Exists(archivosecuencia))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(archivosecuencia))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
            else
            {
                MessageBox.Show("Archivo \"{0}\" no exite.", filename);

                return;
            }

            System.IO.File.WriteAllText(archivosecuencia, Vble.lineas);
        }


        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnCarpTodo_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < lsDesc.Items.Count; i++)
            {
                lsDesc.Items[i].Selected = true;
            }

        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lsDesc.Items.Count > 0)
                {
                    if (MessageBox.Show("¿Está seguro que desea realizar la descarga de las conexiones" +
                                        "\nde la colectora a la PC?", "Devolver Conexiones", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                        lsDesc.Items.Clear();
                        CambiarEstadoRecibidoMySql(Vble.RutaBaseSQLiteColectora, Convert.ToInt32(cteCodEstado.Descargado));                        
                        ObtenerDatosDescarga(Vble.RutaBaseSQLiteColectora);                        
                        GenerarCarpetaDescarga();
                        CargarTablaDescargas(Vble.RutaBaseSQLiteColectora);
                        File.Delete(Vble.RutaBaseSQLiteColectora);
                        File.Delete(Vble.ArchivoInfoCargaColectora);


                        //lsDesc.Items.Clear();                


                        //File.Move(Vble.RutaBaseSQLiteColectora, Vble.RutaBaseSQLiteColectora+"_Descargado");
                        //File.Move(Vble.ArchivoInfoCargaColectora, Vble.ArchivoInfoCargaColectora  + "_Descargado");



                    }
                }
            }
            catch (Exception r)
            {               
                MessageBox.Show(r.Message);
            }
        }


        /// <summary>
        /// Realizo los select para obtener la cantidad de conexiones segun codigos de Impresion con los que vuelven de
        /// de la colectora a la PC
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void ObtenerDatosDescarga(string RutaRecibir)
        {
            DataTable Tabla = new DataTable();

            try
            {
                string txSQL;                
                SQLiteCommand da;
                SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseACargar.Open();

                //ConsultaCantidadConexionesNoLeidas
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoLeido);
                da = new SQLiteCommand(txSQL, BaseACargar);               
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoLeido));
                Vble.ConexNoLeidas = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesLeidasNoImpresas
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.LeidoNoImpreso);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.LeidoNoImpreso));
                Vble.ConexLeidasNoImpresas = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesLeidasImpresas
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.LeidoImpreso);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.LeidoImpreso));
                Vble.ConexLeidasImpresas = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresasImpresoraDesc
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoImpresoraDes);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoImpresoraDes));
                Vble.ConexNoImpresasImpresoraDesc = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresasFueraDeRango
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoFueraRango);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoFueraRango));
                Vble.ConexNoImpresasFueradeRango = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresasEstadoNegativo
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoEstadoNegativo);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoEstadoNegativo));
                Vble.ConexNoImpresasEstadoNegativo = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresoErrorDato
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoErrorDato);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoErrorDato));
                Vble.ConexNoImpresasErrorDato = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresoDomicilioPostal
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoDomicilioPostal);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoDomicilioPostal));
                Vble.ConexNoImpresasDomicilioPostal = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresoIndicadoDato
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoIndicadoDato);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoIndicadoDato));
                Vble.ConexNoImpresasIndicadoDato = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesImposibleLeer
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ImposibleLeer);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ImposibleLeer));
                Vble.ConexImposibleLeer = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesSubtotalNegativo
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.subNegativo);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.subNegativo));
                Vble.ConexSubtNeg = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesError al archivar Datos
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ErrorArchDatos);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ErrorArchDatos));
                Vble.ConexErrorArchDatos = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesError numero de factura
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ErrorNFact);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ErrorNFact));
                Vble.ConexErrorNFact = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex Sin Concepto que Facturar
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ConexSinConcepFacturar);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ConexSinConcepFacturar));
                Vble.ConexSinConcepFacturar = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex Error al Facturar
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ConexErrorFacturando);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ConexErrorFacturando));
                Vble.ConexErrorFacturando = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex Periodo Excedido en Dias
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ConexPerExcDias);
                da = new SQLiteCommand(txSQL, BaseACargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ConexPerExcDias));
                Vble.ConexPerExcDias = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                BaseACargar.Close();

            }
                catch (Exception)
                {

                    throw;
                }
            }

        private void CargarTablaDescargas(string RutaRecibir)
        {
            DataTable Tabla = new DataTable();
            try
            {
                //string fechadescarga = string.Format("{0:yyMMdd_HHmm}", DateTime.Now);
                
                string insert;//Declaración de string que contendra la consulta INSERT
                insert = "INSERT INTO descargas (Periodo, FechaDescarga, Carpeta, Dispositivo, Lecturista, InfConexiones, CantidadConex," +
                    " Noleidas, LeidasNoImpresas, LeidasImpresas, NoImpresasImpDesconec, NoImpresasFueraDeRango, NoImpresasEstadoNeg," +
                    " NoImpresasErrorDato, NoImpresasDomPostal, NoImpresasIndicDato, ImposibleLeer, SubTotalNeg, ErrorArchivarDatos," +
                    " ErroNumFactura, SinConceptosFacturar, ErrorAlFacturar, PeriodoExcEnDias) " +
                    "VALUES ('" + Vble.PeriodoEnColectora + "', '" + DateTime.Now.ToString("dd/MM/yyyy_HH:mm")  + "', '" + Vble.NomCarpDescarga + "', '" + Vble.ColectoraConectada + "', '" + Vble.Operario + 
                    "', '" + Vble.InfoDescarga + "', '" + Vble.CantConex + "', '" + Vble.ConexNoLeidas + "', '" + Vble.ConexLeidasNoImpresas + 
                    "', '" + Vble.ConexLeidasImpresas + "', '" + Vble.ConexNoImpresasImpresoraDesc + "', '" + Vble.ConexNoImpresasFueradeRango +
                    "', '" + Vble.ConexNoImpresasEstadoNegativo + "', '" + Vble.ConexNoImpresasErrorDato + "', '" + Vble.ConexNoImpresasDomicilioPostal +
                    "', '" + Vble.ConexNoImpresasIndicadoDato + "', '" + Vble.ConexImposibleLeer + "', '" + Vble.ConexSubtNeg +
                    "', '" + Vble.ConexErrorArchDatos + "', '" + Vble.ConexErrorNFact + "', '" + Vble.ConexSinConcepFacturar + "', '" + Vble.ConexErrorFacturando +
                    "', '" + Vble.ConexPerExcDias + "')";
                

                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message, "Error al generar archivo de descarga");
            }


            }

        private void splitContainer4_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAgregar_Click_2(object sender, EventArgs e)
        {
            GenerarInformeDescarga(Vble.Periodo);
            
        }

        private void btnCarpTodo_Click_2(object sender, EventArgs e)
        {
            LeerInformes(Vble.Periodo);
        }

        private void btnCarpNada_Click_1(object sender, EventArgs e)
        {
            tvInformes.Nodes.Clear();
        }
    }
}
