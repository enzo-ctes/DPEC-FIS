/*
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 14:02
 * 
 */


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Collections;
using System.Globalization;
using System.Text;
using Microsoft.VisualBasic.Devices;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.IO;
using GongSolutions.Shell;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Collections.ObjectModel;
using WindowsPortableDevicesLib.Domain;
using WindowsPortableDevicesLib;
using System.Diagnostics;
using System.Management;

using PortableDeviceApiLib;
using WPDSpLib;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form4Cargas.
    /// </summary>
    public partial class Form4Cargas : Form {

        #region Variables globales dentro de la clase Cargas
        private int QueTimer;
        private Dictionary<string, clInfoNodos> dcNodos = new Dictionary<string, clInfoNodos>();
        Computer mycomputer = new Computer();
        Form0 form0 = new Form0();
        Form1Inicio Inicio = new Form1Inicio();
     

        /// <summary>
        ///agregado para control de colectora
        /// </summary>
        /// ///rootFolder seria la variable que contiene el directorio raiz de la colectora "\"
        public static PortableDeviceFolder currentFolder = null;
        public static PortableDeviceFolder MisDocumentos = null;
        public static PortableDeviceFolder CarpetaDatosDPEC = null;
        public static IList<PortableDeviceObject> currentContent = null;
        public static StandardWindowsPortableDeviceService service = new StandardWindowsPortableDeviceService();
        public static Dictionary<string, TreeNode> NodosSelected = new Dictionary<string, TreeNode>();
        public static ArrayList NodosSeleccionados = new ArrayList();
        IList<WindowsPortableDevice> devices;
        DataGridView dataGridViewPerAnt = new DataGridView();
        /// <summary>
        /// Variable banConexCol que se va a utilizar en el tiem2 para buscar o no la colectora conectada.
        /// </summary>
        public static bool bandConexCol { get; set; }
        //StandardWindowsPortableDeviceService service = new StandardWindowsPortableDeviceService();
        //WindowsPortableDevicesLib.Domain.PortableDeviceFolder currentFolder = null;
        //////IList<WindowsPortableDevice> contenido = null;
        //IList<WindowsPortableDevicesLib.Domain.PortableDeviceObject> currentContent = null;
        private bool BloqueoClick = false;
        //declaracion de arrays que contendran informacion de las Secuencias
        //Rutas y Localidades que se seleccionan del treeview para realizar las consultas
        ArrayList ArrayCantConex = new ArrayList();
        ArrayList ArrayDesde = new ArrayList();
        ArrayList ArrayHasta = new ArrayList();
        ArrayList ArrayRuta = new ArrayList();
        ArrayList ArrayLocalidad = new ArrayList();
        ArrayList ArrayTablasSQLite = new ArrayList();
        ArrayList ArrayPersonas = new ArrayList();
        ArrayList ArrayCarpetasCargas = new ArrayList();
        ArrayList ArrayZona = new ArrayList();
        IDictionary DirectorioRutasEnviadas = new Dictionary<int, string>();
        public static DataTable TabRegSelec = new DataTable();
        private int cnt = 0;
        public delegate void InvokeDelegate();

        public bool Continuar = true;
        public int Enviando = 0;
        public int Procesando = 0;
        #endregion
        

        public Form4Cargas()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            //DB.con.Open();    
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

        }

        private async void form4_Load(object sender, System.EventArgs e) {

            ////Carga las imagenes para nodos

            Image Im1;
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\todo.png");
            imgList1.Images.Add("Todo", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\algo.png");
            imgList1.Images.Add("Algo", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\nada.png");
            imgList1.Images.Add("Nada", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Symbol-Check-2.gif");
            imgList1.Images.Add("GenOk", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Symbol-Error-3.gif");
            imgList1.Images.Add("Error", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Favorites.gif");
            imgList1.Images.Add("EnPro", Im1);
            //Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
            //imgList1.Images.Add("Logo", Im1);
            Im1 = null;

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.progressBar1.Visible = false;
            //this.WindowState = FormWindowState.Maximized;
            this.toolTip1.IsBalloon = true;
            this.toolTip2.IsBalloon = true;
            this.toolTip3.IsBalloon = true;
            this.toolTip4.IsBalloon = true;
            this.toolTip5.IsBalloon = true;
            this.toolTip1.SetToolTip(this.BotEnviarCarga, "Envia los archivos seleccionados de la PC a la Colectora");
            this.toolTip2.SetToolTip(this.BotActPanPC, "Actualiza el Panel de Rutas para Cargar y Rutas Cargadas");
            this.toolTip3.SetToolTip(this.BotProcesCarg, "Genera el archivo con las conexiones seleccionadas");
            this.toolTip4.SetToolTip(this.BotDevCarga, "Devuelve la carga procesada seleccionada al panel de Rutas Disponibles");
            this.toolTip5.SetToolTip(this.botExpulsaColec, "Expulsa la colectora conectada");
            this.groupBoxProrrateo.Size = new Size(490, 190);
            this.radioButSinPro.Checked = true;
            this.RBEnvCable.Checked = true;

            //-----------------------------------------------------------------------------------------------------------------

            ////CargarTVRutasExportadas();
            ////BGWEsperar.RunWorkerAsync();
            PickBoxLoading.Visible = true;
            Task oTask = new Task(listarRutasDisponiblesTASK);
            oTask.Start();
            await oTask;


            cargarCBColeWifi();
            //QueTimer = 1;
            //timer1.Interval = 1000;
            //timer1.Enabled = true;

            //ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
            // shellView2.CurrentFolder = folder;

            BotActPanPC_Click(sender, e);
            
            if (RBEnvCable.Checked == true)
            {
                cmbDevices.Visible = true;
                cmbDevicesWifi.Visible = false;
            }
            else if (RBEnvWifi.Checked == true)
            {
                cmbDevices.Visible = false;
                cmbDevicesWifi.Visible = true;
            }

            //CargasProcesadas();

            timer4_Tick(sender, e);
            //IList<WindowsPortableDevice> devices = service.Devices;
        }

        /// <summary>
        /// Metodo que carga el ComboBox de Colectoras para conexion Inalambrica
        /// el cual lee del archivo .ini de datos las colectoras asociadas al centro de interfaz que esta operando
        /// </summary>
        private void cargarCBColeWifi()
        {
            StringBuilder stb = new StringBuilder(500);
            Inis.GetPrivateProfileString("Colectoras", Vble.locCentroInterfaz, "", stb, 500, Ctte.ArchivoIniName);
            Vble.colectorasCentroInterfaz = stb.ToString().Trim();
            List<string> ListaColectoras = new List<string>();

            string[] colectoras = Vble.colectorasCentroInterfaz.Split(',');
            cmbDevicesWifi.Items.Clear();
            ListaColectoras.Clear();
            foreach (var item in colectoras)
            {
                //cmbDevicesWifi.Items.Add(item.ToString());
                ListaColectoras.Add(item.ToString());
            }
            ///Ordeno la lista de forma ascendente y luego agrego al combobox
            ListaColectoras.Sort();
            foreach (var item in ListaColectoras)
            {
                cmbDevicesWifi.Items.Add(item);
            }
        }


        

      

        //Boton que cierra el formulario actual
        void btnCerrar_Click(object sender, EventArgs e)
        {
            //tvwCargas.Dispose();
            //ListViewCargados.Dispose();
            //listViewCargasProcesadas.Dispose();
            //LisViewDescargados.Dispose();
            this.Dispose();
            GC.SuppressFinalize(this);
            this.Close();
            Inicio.timer2.Start();

        }

        private void Form4_Resize(object sender, System.EventArgs e) {
            this.WindowState = FormWindowState.Maximized;
        }


        /// <summary>
        /// Ejecuta algo, segun el valor de QueTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e) {
            switch (QueTimer) {
                case 1:
                    //Carga las rutas disponibles en la lista
                    timer1.Enabled = false;
                    Vble.ArrayZona.Clear();
                    CargarListaRutas();
                
                    //CargarComboColectoras();
                    break;
                default:
                    break;
            }
        }


        #region PARTE_PARA_MANEJO_NODOS

        DataTable Tabla;
        public TreeNode tvwChild;
        public string txDPEC = "dpec";


        /// <summary>
        /// procesa la vista del listview de rutas disponibles para que lo haga de maner asyncrona asi el programa no espera que termine ese proceso
        /// </summary>
        public void listarRutasDisponiblesTASK()
        {
            ////QueTimer = 1;
            //////timer1.Interval = 1000;
            ////timer1.Start();
            CargarRutasDisponibles();
            


        }

       


        public void CargarRutasDisponibles() {
          
            Vble.ArrayZona.Clear();
            CargarListaRutas();
            
        }

        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracionZona()
        {
            string where = "";
            try
            {
                for (int i = 0; i < Vble.ArrayZona.Count; i++)
                {

                    where += " OR Conexiones.Zona = " + Vble.ArrayZona[i];
                    //where += " OR ((conexiones.conexionid=infoconex.conexionid " +
                    //" AND conexiones.periodo = " + Vble.Periodo +
                    //" AND conexiones.ImpresionOBS = " + 0 +
                    //" AND conexiones.Zona = " + ArrayZona[i] + ")" +
                    ////" AND" 
                    //" OR (conexiones.conexionid=infoconex.conexionid " +
                    //    " AND conexiones.periodo = " + Vble.Periodo +
                    //    " AND conexiones.ImpresionOBS = " + 500 +
                    //    " AND conexiones.Zona = " + ArrayZona[i] + "))";                
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }

        /// <summary>
        /// Proceso que lee el archivo ZonaFIS.txt que contiene las localidades de la interfaz en el cual se esta trabajando, 
        /// la misma esta ubicada en el directorio C:\Windows\ZonaFIS.txt ubicación común para todas las interfaces de GagFIS-Interface 
        /// </summary>
        private void LeerArchivoZonaFIS()
        {

            //Vble.ArrayZona.Clear();
            int cant = Vble.ArrayZona.Count;

            for (int i = 0; i < cant; i++)
            {
                Vble.ArrayZona.Remove(i);
            }
            
            try
            {
                using (StreamReader sr = new StreamReader(Ctte.ArchivoZonaFIS))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        String value = line;
                        Char delimiter = ',';
                        String[] substrings = value.Split(delimiter);

                        for (int i = 0; i < substrings.Length; i++)
                        {
                            Vble.ArrayZona.Add(substrings[i]);

                        }
                    }

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al leer el archivo que contiene informacion de localidades de la Interfaz.",
                                            "Error de Archivo Interfaz", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga el tree view con las rutas disponibles para ser asiganadas, 
        /// desde la tabla conexiones, tomando el periodo seleccionado.
        /// <para>Entre [] se indica el key de cada nodo, siempre es string sin ceros a izquierda</para>
        /// <para>El árbol tiene los siguientes niveles:</para>
        /// <para>Empresa: siempre Vble.Empresa ['dpec']</para>
        /// <para>- |_Distrito: equivale a la localidad (Codigo y Nombre) ['codigo interno']</para>
        /// <para>-     |_ Remesa: Número= 1 a 8 ['1' a '8'].</para>
        /// <para>-          |_ Ruta: Número: número de ruta [numero de ruta]</para>
        /// <para>-               |_ Partición: A ['A']... Desde-hasta parte de la ruta a ser cargada</para>
        /// La parte de Distito, Remesa y Ruta se carga al leer la tabla, la partición inicialmete
        /// es una sola que luego puede ser cortada en dos o mas, la primera partición es 'A' y
        /// se van agregando 'B', 'C', etc.
        /// <para>Por otro lado, hay un arreglo del tipo Dictionary (dcNodos), donde cada uno de sus elementos
        /// está asociado a cada uno de los nodos del Arbol, y la key es la misma que la del nodo
        /// a la que se corresponde.</para>
        /// <para>Para evitar posibles duplicaciones de key's se usará siempre para cada nodo 
        /// una cadena con las key's de los nodos superiores concatenadas, mas la key del nodo
        /// en particular. NO se usan espacios en las key's</para>
        /// </summary>
        /// <returns>true si no hubo error</returns>
        private bool CargarListaRutas() {
            bool retorno = true;
            //DataTable Tabla;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL = "";
            int Distr, Rem, Rut, Sec;
            string Par;
            try
            {
                //this.Cursor = Cursors.WaitCursor;
                //Tomo las localidades pertenecientes a la interfaz que se esta trabajando para ver a la hora de cargar las conexiones a colectora
                LeerArchivoZonaFIS();

                //Lee la tabla conexiones del periodo y sin leer
                txSQL = "SELECT Conexiones.ConexionID, Conexiones.Zona, Conexiones.Remesa," +
                            "Conexiones.Ruta, Conexiones.Secuencia, infoconex.Particion" +
                        " FROM Conexiones, infoconex" +
                        " WHERE (Conexiones.ConexionID = infoconex.ConexionID" +
                        " AND Conexiones.Periodo = infoconex.Periodo" +
                        " AND Conexiones.Periodo = " + Vble.Periodo +
                        //" AND (Conexiones.ImpresionOBS = 0 OR Conexiones.ImpresionOBS = 500 OR Conexiones.ImpresionOBS MOD 100 = 17 " +
                        " AND (Conexiones.ImpresionOBS = 0 OR Conexiones.ImpresionOBS = 500" +
                              //" OR Conexiones.ImpresionOBS = 509) " +
                              ") " +
                        " AND (Conexiones.Zona = " + Vble.ArrayZona[0] + iteracionZona() +
                        ")) ORDER BY Conexiones.Zona, Conexiones.Remesa, Conexiones.Ruta, Conexiones.Secuencia";

                //txSQL = "SELECT DISTINCT Conexiones.Zona, Conexiones.Remesa," +
                //          "Conexiones.Ruta, Conexiones.Secuencia, infoconex.Particion" +
                //      " FROM Conexiones, infoconex" +
                //      " WHERE (Conexiones.ConexionID = infoconex.ConexionID" +
                //      " AND Conexiones.Periodo = infoconex.Periodo" +
                //      " AND Conexiones.Periodo = " + Vble.Periodo +
                //      " AND (Conexiones.ImpresionOBS = " + 0 + " OR Conexiones.ImpresionOBS = " + 500 +
                //      ") AND (Conexiones.Zona = " + Vble.ArrayZona[0] + iteracionZona() +
                //      ")) ORDER BY Conexiones.Zona, Conexiones.Remesa, Conexiones.Ruta, Conexiones.Secuencia";


                Tabla = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                ///Tiempo que tardara en esperar la respuesta desde la base de datos
                datosAdapter.SelectCommand.CommandTimeout = 900;
                

                datosAdapter.Fill(Tabla);

                
                comandoSQL.Dispose();
                datosAdapter.Dispose();

                tvwCargas.Nodes.Clear();
                dcNodos.Clear();

                BeginInvoke(new InvokeDelegate(InvokeMethodCargarRutasDisponibles));

                
                
            }
            catch (Exception e) {
                MessageBox.Show(e.Message + "- en: " + e.TargetSite.Name);
                retorno = false;
            }
            //Application.DoEvents();
            //PickBoxLoading.Visible = false;
            BloqueoClick = false;
            GC.SuppressFinalize(this);
            return retorno;
        }

        public void InvokeMethodCargarRutasDisponibles()
        {
            AgregarNodoEmpresa(Vble.Empresa, "logo");
            Font FuenteNegrita = new Font(tvwCargas.Font, FontStyle.Bold);

            //Recorre la tabla y carga las ramas del arbol
            foreach (DataRow Fila in Tabla.Rows)
            {
                //Distr = Fila.Field<int>("Zona");
                //if (Distr < 100) Distr += 200;
                //Rem = Fila.Field<int>("Remesa");
                //Rut = Fila.Field<int>("Ruta");
                //Sec = Fila.Field<int>("Secuencia");
                //Par = Fila.Field<string>("Particion");
                Vble.Distrito = Fila.Field<int>("Zona");
                if (Vble.Distrito < 100) Vble.Distrito += 200;
                Vble.Remesa = Fila.Field<int>("Remesa");
                Vble.Ruta = Fila.Field<int>("Ruta");
                Vble.Secuencia = Fila.Field<int>("Secuencia");
                Vble.Particion = Fila.Field<string>("Particion");

                if (AgregarNodoDistrito(Vble.Empresa, Vble.Distrito))
                    if (AgregarNodoRemesa(Vble.Empresa, Vble.Distrito, Vble.Remesa))
                        if (AgregarNodoRuta(Vble.Empresa, Vble.Distrito, Vble.Remesa, Vble.Ruta))
                        {
                            AgregarNodoParticionA(Vble.Empresa, Vble.Distrito, Vble.Remesa, Vble.Ruta, Vble.Particion, Vble.Secuencia);
                        }
            }

            ///Recorre los nodos y coloca en negrita los nodos que contienen el numero de ruta para
            ///que se pueda identificar al momento de querer seleccionar la particion de la ruta 
            ///cuando se tienen muchos nodos abiertos.
            for (int i = 0; i < tvwCargas.Nodes.Count; i++)
            {
                for (int j = 0; j < tvwCargas.Nodes[i].Nodes.Count; j++)
                {
                    for (int k = 0; k < tvwCargas.Nodes[i].Nodes[j].Nodes.Count; k++)
                    {
                        tvwCargas.Nodes[i].Nodes[j].Nodes[k].NodeFont = FuenteNegrita;
                        tvwCargas.Nodes[i].Expand();
                        tvwCargas.Nodes[i].Nodes[j].Expand();
                        tvwCargas.Nodes[i].Nodes[j].Nodes[k].Expand();
                        for (int l = 0; l < tvwCargas.Nodes[i].Nodes[j].Nodes[k].Nodes.Count; l++)
                        {
                            tvwCargas.Nodes[i].Nodes[j].Nodes[k].Nodes[l].NodeFont = FuenteNegrita;
                        }
                    }
                }
            }
            //tvwCargas.Nodes[Vble.Empresa.ToLower()].ExpandAll();
            //tvwCargas.ExpandAll();
            TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
            tvwCargas.Sort();
            PickBoxLoading.Visible = false;
            //this.Cursor = Cursors.Default;
            //tvwCargas.Nodes[sEmp].Expand();
            //tvwCargas.Nodes[sEmp].Nodes[sDtr].Expand();
            //tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Expand();
            
        }




        private void CargarComboColectoras()
        {
            StringBuilder stb = new StringBuilder(650);
            if (Vble.ArrayZona[0].ToString() != "")
            {
                string Zona = Vble.ArrayZona[0].ToString();
                Inis.GetPrivateProfileString("Colectoras", Zona, "", stb, 650, Ctte.ArchivoIniName);
                string colectoras = stb.ToString();


                
                    string[] individual = colectoras.Split(',');
                  
                    foreach (string subDir in individual)
                    {
                    cmbDevices.Items.Add(subDir);
                    }
                
            }

        }

        /// <summary> Agrega la partición de ruta, con todas las 
        /// conexiones existentes en la ruta, y la llama "A", al ir agregando
        /// las conexiones las cuenta y carga en el tag del nodo particion
        /// </summary>
        /// <param name="Empresa"></param>
        /// <param name="Distrito"></param>
        /// <param name="Remesa"></param>
        /// <param name="Ruta"></param>
        /// <param name="Secuencia">Es el número de orden dentro de la ruta</param>
        /// <returns>true si consigue agregar el nodo o sumar al existente</returns>
        private bool AgregarNodoParticionA(string Empresa, int Distrito, int Remesa,
                                            int Ruta, string Particion, int Secuencia) {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            string sRem = sDtr + "re" + Remesa.ToString().Trim();
            string sRut = sRem + "ru" + Ruta.ToString().Trim();
            string sPar = Particion.ToUpper().Trim();
            string sKPt = sRut + sPar;
            clInfoNodos tn = new clInfoNodos();

            try {
                if (!tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes.ContainsKey(sKPt)) {
                    //Agrega el nodo remesa
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes.Add(sKPt, sPar, "nada");
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes[sKPt].Tag = sKPt;
                  

                    //Carga info del nodo
                    tn.Texto = sPar;
                    tn.Key = sKPt;
                    tn.Distrito = Distrito;
                    tn.Remesa = Remesa;
                    tn.Ruta = Ruta;
                    tn.Particion = sPar.Trim();
                    tn.Hasta = 0;
                    tn.Desde = int.MaxValue;

                    tn.CnxSelected = tn.CnxTotal = 0;
                    tn.ImageKey = "nada";
                    dcNodos.Add(sKPt, tn);
                    //Application.DoEvents();

                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
                return false;
            }


            if (tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes.ContainsKey(sKPt)) {
                tn = dcNodos[sKPt];

                tn.CnxSelected++;
                tn.CnxTotal++;
                if (tn.Hasta < Secuencia) tn.Hasta = Secuencia;
                if (tn.Desde > Secuencia) tn.Desde = Secuencia;

                //Actualiza el texto del nodo
                tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes[sKPt].Text = sPar + " (" +
                    tn.Desde.ToString().Trim() + " a " + tn.Hasta.ToString().Trim() + ")[" +
                    tn.CnxTotal.ToString().Trim() + "]";

                //MessageBox.Show(tn.Desde.ToString());
                //tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].NodeFont = Fuente;
                //tvwCargas.ExpandAll();
                //tvwCargas.Nodes[sEmp].Expand();
                //tvwCargas.Nodes[sEmp].Nodes[sDtr].Expand();
                //tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Expand();
                //tvwCargas.Sort();

                return true;
            }

            return false;


        }

        /// <summary> Agrega la ruta, con todas
        /// </summary>
        /// <param name="Empresa"></param>
        /// <param name="Distrito"></param>
        /// <param name="Remesa"></param>
        /// <param name="Ruta"></param>
        /// <returns>true si consigue agregar el nodo o sumar al existente</returns>
        private bool AgregarNodoRuta(string Empresa, int Distrito, int Remesa, int Ruta) {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            string sRem = sDtr + "re" + Remesa.ToString().Trim();
            string sKRt = sRem + "ru" + Ruta.ToString().Trim();
            clInfoNodos tn = new clInfoNodos();

            try {
                if (!tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.ContainsKey(sKRt)) {
                    //Agrega el nodo remesa
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.Add(sKRt, "Ruta:" + Ruta.ToString().Trim(), "nada");
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].Tag = sKRt;
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].BackColor = Color.White;
                    




                    //Carga info del nodo
                    tn.Texto = "Ruta:" + Ruta.ToString().Trim();
                    tn.Key = sKRt;
                    tn.Distrito = Distrito;
                    tn.Remesa = Remesa;
                    tn.Ruta = Ruta;
                    tn.Hasta = 0;
                    tn.Desde = int.MaxValue;
                    tn.CnxSelected = tn.CnxTotal = 0;
                    tn.ImageKey = "nada";
                    
                    dcNodos.Add(sKRt, tn);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
                return false;
            }
            return true;

        }

        /// <summary>
        /// Por cada localidad busca las remesas que tienen datos para cargar, y carga 
        /// los nodos respectivos
        /// </summary>
        /// <param name="sRemesa">Remesa a verificar/cargar</param>
        /// <param name="sDistrito">Distrito al que pertenece la zona</param>
        /// <param name="sLetrZon">Si corresponde, letra que tiene la zona</param>
        /// <returns>Retorna True si el nodo de zona existe o fué añadico con éxito</returns>
        private bool AgregarNodoRemesa(string Empresa, int Distrito, int Remesa) {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            string sRem = Remesa.ToString().Trim();
            string sKRm = sDtr + "re" + sRem;
            clInfoNodos tn = new clInfoNodos();

            try {
                if (!tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm)) {
                    //Agrega el nodo remesa
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes.Add(sKRm, "Rem." + sRem, "nada");
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sKRm].Tag = sKRm;
                    //Carga info del nodo
                    tn.Texto = "Rem." + sRem;
                    tn.Key = sKRm;
                    tn.Distrito = Distrito;
                    tn.Remesa = Remesa;
                    tn.ImageKey = "nada";
                    dcNodos.Add(sKRm, tn);

                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
            }

            return tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm);

        }

        /// <summary>Verifica que el nodo distrito, según el número de zona, que se  
        /// corresponde con el código interno de localidad, esté cargado, si no está 
        /// cargado, lo agrega.
        /// </summary>
        /// <param name="sEmpresa">Empresa para la que se buscarán los nodos (DPEC siempre)</param>
        /// <param name="Distrito">Número de zona, a la quew corresponde la localidad o distrito</param>
        /// <returns>Devuelve la cantidad de nodos agregados</returns>
        private bool AgregarNodoDistrito(string sEmpresa, int Distrito) {
            string sKey, sD, Loc;
            clInfoNodos tn = new clInfoNodos();
            int i;
            DataTable tabZona;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;

            try {
                //Obtener la Key de la rama superior (padre)


                if (Distrito < 100) Distrito += 200;
                sD = Distrito.ToString().Trim();
                sKey = Vble.Empresa.ToLower() + sD;

                if (!tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey)) {
                    //No está el distrito, debe agregarlo
                    txSQL = "SELECT  * FROM Localidades " +
                       "WHERE CodigoInt=" + sD;
                    tabZona = new DataTable();
                    datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(tabZona);
                    Loc = "-";
                    if (tabZona.Rows.Count > 0)
                        Loc = tabZona.Rows[0].Field<string>("Localidad");
                    tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes.Add(sKey, sD + " - " + Vble.LetraCapital(Loc.Trim()), "nada");
                    tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Tag = sKey;

                    tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Expand();
                    i = tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Index;
                    tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].BackColor = Color.AliceBlue;

                    tn.Texto = sD.ToUpperInvariant();
                    tn.Key = sKey.ToLowerInvariant();
                    tn.Distrito = Distrito;
                    tn.ImageKey = "nada";
                    dcNodos.Add(sKey, tn);

                }
            }

            catch (Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }

            return tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey);


        }

        /// <summary>
        /// El nodo de Empresa está en el nivel cero, y debería ser el único
        /// nodo en ese nivel, salvo que hubiera mas de una empresa en simultáneo
        /// cosa que no debería pasar. Verifica si está presente el nodo, si no
        /// está lo agrega
        /// </summary>
        /// <param name="NombreEmpresa">Nombre de la empresa </param>
        /// <param name="keyLogo">Key del logo que se va a mostrar en el NODO</param>
        /// <returns>Retorna True si el nodo de la Empresa está presente o
        /// fue añadido con éxito.</returns>
        private bool AgregarNodoEmpresa(string NombreEmpresa, string keyLogo) {
            string sClave = NombreEmpresa.ToLower().Trim();
            clInfoNodos tiN = new clInfoNodos();

            // Verificar si ya está el nodo
            if (!tvwCargas.Nodes.ContainsKey(sClave)) {
                //No está el nodo, lo agrega
                tvwCargas.Nodes.Add(sClave, NombreEmpresa, keyLogo);
                tvwCargas.Nodes[sClave].Expand();
                tvwCargas.Nodes[sClave].Tag = sClave;
                tiN.Texto = NombreEmpresa;
                tiN.Key = sClave;
                tiN.ImageKey = "nada";
                dcNodos.Add(sClave, tiN);

                tvwCargas.Nodes[sClave].Expand();
            }

            // Verifica que se haya añadido correctamente
            return tvwCargas.Nodes.ContainsKey(sClave);
        }

        /// <summary>
        /// Aplica el estado del nodo a todos los hijos
        /// </summary>
        /// <param name="ndX">Nodo padre</param>
        private void AplicarEstadoAHijos(TreeNode ndX) {   //(ByVal ndX As Node)
            TreeNode ndH;
            clInfoNodos tIN = new clInfoNodos();
            tIN = dcNodos[ndX.Tag.ToString()];

            if (ndX.Nodes.Count > 0) {
                tIN.CnxSelected = 0;
                tIN.CnxTotal = 0;
                ndH = ndX.FirstNode;
                while (ndH != null) {
                    //if(pCancelar) Err.Raise 20015, "Aplicar Estado a Hijos ", " Proceso Cancelado por el Usuario "
                    //ndH.Tag = ndH.Tag ?? tIN.Key;   //= infNdX[ndH.Index];

                    ndH.ImageKey = ndX.ImageKey;
                    dcNodos[ndH.Tag.ToString()].ImageKey = ndX.ImageKey;

                    AplicarEstadoAHijos(ndH);
                    if (ndX.ImageKey.ToLower() == "todo") tIN.CnxSelected += dcNodos[ndH.Tag.ToString()].CnxTotal;
                    tIN.CnxTotal += dcNodos[ndH.Tag.ToString()].CnxTotal;
                    if (ndX == ndH.LastNode) break;
                    ndH = ndH.NextNode;

                }
            }
            // ndX.Text = tIN.Texto + " [" + tIN.CnxSelected.ToString() + " de " + tIN.CnxTotal.ToString() + "]";
        }

        /// <summary>
        /// Desde el nodo ndP se baja por su gajo leyendo los estados de todos
        /// los hijos, el estado final del nodo es tal que si:
        /// <list type="bullet">
        /// <item><description>Seleccionado = 0: "Nada"</description></item>
        /// <item><description>Seleccionado = Total: "Todo"</description></item>
        /// <item><description>Otros casos: "Algo"</description></item>
        /// </list>
        /// </summary>
        /// <param name="ndP">Nodo cuyo estado se va a actualizar.</param>
        /// <returns>Devuelve la cantidad seleccionada en el nodo</returns>
        private void TomarEstadoDeHijos(TreeNode ndP) {
            clInfoNodos iNP = dcNodos[ndP.Tag.ToString()];         
            int CxTot = 0;
            int CxSel = 0;
            //Recorre los hijos del nodo
            foreach (TreeNode ndH in ndP.Nodes) {
                //si tiene hijos, recursividad.
                if (ndH.Nodes.Count > 0)
                    TomarEstadoDeHijos(ndH);
                else {
                    //Si no tiene hijos, toma seleccion según imagen
                    if (dcNodos[ndH.Tag.ToString()].ImageKey == "todo")
                        dcNodos[ndH.Tag.ToString()].CnxSelected = dcNodos[ndH.Tag.ToString()].CnxTotal;
                    else
                        dcNodos[ndH.Tag.ToString()].CnxSelected = 0;
                }

                //acumula totales y seleccionados
                CxTot += dcNodos[ndH.Tag.ToString()].CnxTotal;
                CxSel += dcNodos[ndH.Tag.ToString()].CnxSelected;
            }

            //Aplica las cantidades al nodo
            dcNodos[ndP.Tag.ToString()].CnxSelected = CxSel;
            dcNodos[ndP.Tag.ToString()].CnxTotal = CxTot;

            //Aplica la imagen segun la cantidad seleccionada
            if (CxSel == 0)
                dcNodos[ndP.Tag.ToString()].ImageKey = "nada";
            else if (CxTot == CxSel)
                dcNodos[ndP.Tag.ToString()].ImageKey = "todo";
            else
                dcNodos[ndP.Tag.ToString()].ImageKey = "algo";

            //Muestra el estado
           
            ndP.ImageKey = dcNodos[ndP.Tag.ToString()].ImageKey;
            //ndP.Text =  dcNodos[ndP.Tag.ToString()].Texto +
            //    "  [ " + dcNodos[ndP.Tag.ToString()].CnxSelected.ToString() +
            //    " de " + dcNodos[ndP.Tag.ToString()].CnxTotal.ToString() + " ]";   
            ndP.ExpandAll();
            
            return;

        }

        /// <summary>Presenta el cuadro de diálogo para particionar 
        /// una partición dada
        /// </summary>
        /// <param name="Nodo">Nodo partición, que se va a particionar</param>
        private void DialogoParticion(TreeNode Nodo) {
            int idx = Nodo.Index;
            string sKy = Nodo.Tag.ToString();
            clInfoNodos tn = new clInfoNodos();
            tn = dcNodos[Nodo.Tag.ToString()];

            //Click derecho en nodo partición, opcion de dividir nodo
            int dsd, hst, cnt;
            dsd = tn.Desde;
            hst = tn.Hasta;
            cnt = tn.CnxTotal;

            FormDialog fD = new FormDialog(tn.Ruta, tn.Particion, dsd, hst, cnt);
            fD.StartPosition = FormStartPosition.CenterParent;

            switch (fD.ShowDialog(this)) {
                case DialogResult.OK:
                    dsd = fD.PartDesde;
                    hst = fD.PartHasta;
                    cnt = fD.PartCantidad;
                    Particionar(Nodo, dsd, hst, cnt);
                    break;
                case DialogResult.Abort:
                    dsd = fD.PartDesde;
                    EliminarParticiones(Nodo);
                    break;
            }
        }

        /// <summary> Particiona el nodo de acuerdo con los datos suministrados.
        /// Un valor '0' en alguno de los parámetros significa que ese dato
        /// no fue suminstrado, y se actúa en función de los demas
        /// </summary>
        /// <param name="Nodo">Nodo que se va a particionar, debe ser un nodo particion</param>
        /// <param name="Desde">Valor de secuencia inicial, si=0, desde el principio</param>
        /// <param name="Hasta">Valor de secuencia superior, si=0, no hay limite</param>
        /// <param name="Cantidad">Cantidad de conexiones en particion nueva, si=0, no se considera</param>
        private void Particionar(TreeNode Nodo, int Desde, int Hasta, int Cantidad) {
            int iDesde, iHasta, iCnt;
            clInfoNodos inN = dcNodos[Nodo.Tag.ToString()];
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL, nuevaLetra, viejaLetra = "";
            string AscDesc = "ASC";

            if (Desde == 0 && Hasta > 0 && Cantidad > 0)
                AscDesc = "DESC";

            Tabla = new DataTable();

            //Adecuación de límites.
            iDesde = Desde < inN.Desde ? inN.Desde : Desde;
            iHasta = (Hasta == 0) || (Hasta > inN.Hasta) ? inN.Hasta : Hasta;
            iCnt = Cantidad == 0 ? inN.CnxTotal : Cantidad;

            Tabla = new DataTable();

            //Seleccionar las conexiones que integrarán la nueva partición
            txSQL = "SELECT C.ConexionID, I.Particion, C.Secuencia, C.Ruta " +
                " FROM  Conexiones C JOIN infoconex I ON C.Conexionid=I.Conexionid" +
                " WHERE I.Particion = '" + inN.Particion + "'" +
                " AND (I.CodigoImpresion = 0 OR I.CodigoImpresion = 500)" +
                " AND C.Ruta = " + inN.Ruta.ToString().Trim() +
                " AND C.Secuencia >= " + iDesde.ToString().Trim() +
                " AND C.Secuencia <=" + iHasta.ToString().Trim() +
                " ORDER BY C.Secuencia " + AscDesc +
                " LIMIT " + iCnt;

            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            dataGridView1.DataSource = Tabla;

            //Si la cantidad de registros es igual al total de la partición se informa
            if (Tabla.Rows.Count == inN.CnxTotal) {
                MessageBox.Show(" Está seleccionando una forma de particionar que incluye "
                             + "\nla totalidad de las conexiones de la partición."
                             + "\nNo tiene sentido, se desestima la nueva partición!!. ",
                             "Partición Mal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Buscar la última letra usada en las particiones de la ruta.
            txSQL = "SELECT DISTINCT(particion) " +
                " FROM  Conexiones C JOIN infoconex I ON C.Conexionid=I.Conexionid" +
                " AND (I.Codigoimpresion = 0 OR I.Codigoimpresion = 500)" +
                " AND C.Ruta = " + inN.Ruta.ToString().Trim() +
                " AND C.Secuencia >= " + iDesde.ToString().Trim() +
                " AND C.Secuencia <=" + iHasta.ToString().Trim() +
                " ORDER BY Particion";


            DataTable TabPart = new DataTable();
            MySqlDataAdapter dtAdap = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder cmdSql = new MySqlCommandBuilder(dtAdap);
            dtAdap.Fill(TabPart);



            int cnt = TabPart.Rows.Count;
            if (cnt > 0) {
                DataRow Fila = TabPart.Rows[cnt - 1];
                viejaLetra = Fila[0].ToString().ToUpper();
            }

            List<string> Nuevas = new List<string> {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
                                  "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            //Si la letra vieja no es "Z" pone la que le sigue
            if (viejaLetra != "Z") {
                nuevaLetra = Nuevas[Nuevas.IndexOf(viejaLetra) + 1];
            }
            else {
                //si la vija es 'Z' busca la primera libre.
                List<string> Viejas = new List<string>();
                foreach (DataRow Fila in TabPart.Rows)
                    Viejas.Add(Fila[0].ToString());

                //Busca primera de Nuevas que no esté en Viejas
                nuevaLetra = viejaLetra;
                foreach (string Ltr in Nuevas)
                    if (!Viejas.Contains(Ltr)) {
                        nuevaLetra = Ltr;
                        break;
                    }
            }

            //Si las letras nuevas y viejas son iguales, NO hace la partición
            if (nuevaLetra == viejaLetra)
                return;

            List<int> Fla = new List<int>();
            for (int i = 0; i < iCnt && i < Tabla.Rows.Count; i++)

                Fla.Add(Tabla.Rows[i].Field<int>("conexionid"));


            txSQL = "UPDATE infoconex " +
                    "SET Particion = '" + nuevaLetra + "'" +
                    " WHERE ConexionID IN(" + string.Join(", ", Fla.ToArray()) + ")";

            MySqlCommand cmdSQL = new MySqlCommand(txSQL, DB.conexBD);
            dtAdap.UpdateCommand = cmdSQL;
            dtAdap.AcceptChangesDuringFill = true;
            cmdSQL.ExecuteNonQuery();

            CargarListaRutas();
        }

        /// <summary> Elimina todas las particiones del nodo, lo que significa que
        /// se le asigna a todas las conexiones de la ruta la particion 'A'
        /// </summary>
        /// <param name="Nodo"></param>
        private void EliminarParticiones(TreeNode Nodo) {
            clInfoNodos inN = dcNodos[Nodo.Tag.ToString()];
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            List<int> cnxs = new List<int>();
            string txSQL;

            //Antes de eliminarlas pregunta para confirmar
            if (MessageBox.Show("Está seguro de ELIMINAR  las particiones" +
                "\nde la RUTA: " + inN.Ruta.ToString() + "????", "Eliminar particiones", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;

            //Confirmado, le pone una "A" en partición de todas las conexiones de la ruta.
            Tabla = new DataTable();

            //Seleccionar las conexiones que integrarán la nueva partición
            txSQL = "SELECT C.ConexionID, I.Particion, C.Secuencia, C.Ruta " +
                " FROM  Conexiones C JOIN infoconex I ON C.ConexionID = I.ConexionID" +
                " WHERE I.CodigoImpresion = 0 OR I.CodigoImpresion = 500" +
                " AND C.Ruta = " + inN.Ruta.ToString().Trim();

            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            ///Agregado para prueba de datos
            //FormDetallePreDescarga Prueba = new FormDetallePreDescarga();
            //Prueba.dataGridView1.DataSource = Tabla;
            //Prueba.Show();
            /////---------------------------------------------
            
            foreach (DataRow Fila in Tabla.Rows)
                cnxs.Add(Fila.Field<int>("ConexionID"));

            txSQL = "UPDATE infoconex " +
                    "SET Particion = 'A'" +
                    " WHERE ConexionID IN('" + string.Join("', '", cnxs.ToArray()) + "')";

            MySqlCommand cmdSQL = new MySqlCommand(txSQL, DB.conexBD);
            MySqlDataAdapter dtAdap = new MySqlDataAdapter(txSQL, DB.conexBD);
            dtAdap.UpdateCommand = cmdSQL;
            dtAdap.AcceptChangesDuringFill = true;
            cmdSQL.ExecuteNonQuery();
            CargarListaRutas();
        }


        public void cargaDescargaListNodosSelected(TreeNode Nodo, string Tarea)
        {
            bool existe = false;
            try
            {            
            if (Tarea == "Eliminar")
            {            
                if (Nodo.ImageKey == "nada" && Nodo.Level == 4)
                {
                    foreach (TreeNode item in NodosSeleccionados)
                    {
                        if (item.Name == Nodo.Name)
                        {
                           //NodosSeleccionados.Remove(Nodo);
                           NodosSelected.Remove(Nodo.Name);
                         }
                    }
                }
            }
            else if (Tarea == "Agregar")
            {
                    foreach (TreeNode item in NodosSeleccionados)
                    {
                        if (item.Name == Nodo.Name)
                        {
                            existe = true;
                        }
                    }

                    if (existe == false)
                    {
                        if (!NodosSelected.ContainsKey(Nodo.ImageKey))
                        {
                            NodosSelected.Add(Nodo.ImageKey, Nodo);
                            NodosSeleccionados.Add(Nodo);
                        }                        
                    }                  
                }
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
            }
        }


        /// <summary>
        /// Al hacer click izquierdo en un nodo se cambia el estado de selección del mismo, y en
        /// el caso de estar en modo "todo" pasa a "nada", y cualquier otro caso pasa a "todo".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwCargas_NodeClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e) {
            TreeNode Nodo = tvwCargas.GetNodeAt(e.X, e.Y);
            int idx = Nodo.Index;
            string sKy = Nodo.Tag.ToString();
            clInfoNodos tn = new clInfoNodos();
            tn = dcNodos[Nodo.Tag.ToString()];
            
            try
            {


                ////toma unicamente los valores de secuencia que esten habilitados como "todos""
                if (Nodo != null)
                {
                    button2_Click(sender, e);
                }

                //Para evitar cambio de estado al expandir o contraer nodo
                if (BloqueoClick) {
                    BloqueoClick = false;
                    return;
                }
                //Debe cambiar el estado de selección del nodo
                if (e.Button == MouseButtons.Left & Nodo.Level == 4)
                {
                   
                    //Si está todo pasa a nada, caso contrario pasa a todo
                    if (tn.ImageKey == "todo")
                    {
                        tn.ImageKey = "nada";
                        //dataGridView1.DataSource = "";                        //
                        //this.labelCantReg.Text = "0";                         //
                        Nodo.ImageKey = tn.ImageKey;                          //probar con varias rutas cargadas
                        Nodo.SelectedImageKey = tn.ImageKey;                  //al seleccionar las distintas particiones
                        AplicarEstadoAHijos(Nodo);                            //funciona, si hay problemas con varias rutas
                        TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);          //comentar esta seccion
                        button2_Click(sender, e);        
                        RestNod.Visible = false;
                        Vble.CantNodosDesde = ArrayDesde.Count;//variable que utilizo para saber cuantas Secuencias va a contener mi consulta                         
                                                               //versecuencia(Nodo);
                                                               //MessageBox.Show("Localidad: " + tn.Distrito + " Remesa: " + tn.Remesa + " Ruta: " + tn.Ruta + " Desde: " + tn.Desde + " Hasta: " + tn.Hasta);
                                                               // }
                        //cargaDescargaListNodosSelected(Nodo, "Eliminar");

                        if (Vble.CantNodosDesde > 0)
                        {
                            Vble.TablaConexSelec.Reset();
                            //Vble.TablaConexSelec = CargarRegistrosSecuenciaP();
                            Vble.TablaConexSelec = CargarRegistrosDisponibles();
                            labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                        }
                        else
                        {
                            Vble.TablaConexSelec.Reset();                            
                            labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                        }

                        this.Cursor = Cursors.Default;//pongo el cursor en estado normal al finalizar la seleccion 
                    }
                    else

                    tn.ImageKey = "todo";
                    Nodo.ImageKey = tn.ImageKey;
                    Nodo.SelectedImageKey = tn.ImageKey;
                    AplicarEstadoAHijos(Nodo);
                    TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
                    button2_Click(sender, e);    
                    RestNod.Visible = false;
                    Vble.CantNodosDesde = ArrayDesde.Count;//variable que utilizo para saber cuantas Secuencias va a contener mi consulta                         
                                                           //versecuencia(Nodo);
                                                           //MessageBox.Show("Localidad: " + tn.Distrito + " Remesa: " + tn.Remesa + " Ruta: " + tn.Ruta + " Desde: " + tn.Desde + " Hasta: " + tn.Hasta);
                                                           // }
                    //cargaDescargaListNodosSelected(Nodo, "Agregar");

                    if (Vble.CantNodosDesde > 0)
                    {
                        Vble.TablaConexSelec.Reset();
                        //Vble.TablaConexSelec = CargarRegistrosSecuenciaP();
                        Vble.TablaConexSelec = CargarRegistrosDisponibles();

                        labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                    }
                    else
                    {
                        Vble.TablaConexSelec.Reset();
                        labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                    }
                    this.Cursor = Cursors.Default;//pongo el cursor en estado normal al finalizar la seleccion 
                }
                else
                {
                  
                }
             

            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// Bloquea el click para que no dispare la opción de selección.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwCargas_BeforeCollapse(object sender, TreeViewCancelEventArgs e) {
            //BloqueoClick = true;
        }

        /// <summary>
        /// Bloquea el click para que no dispare la opción de selección.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwCargas_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            BloqueoClick = true;
        }

        private void tvwCargas_MouseDown(object sender, MouseEventArgs e) {
             
            TreeNode Nodo = tvwCargas.GetNodeAt(e.X, e.Y);

            //BOTON DERECHO
            //Presenta el cuadro de division de una partición
            if (Nodo != null)
                if (e.Button == MouseButtons.Right && Nodo.Level == 4)
                { 
                    DialogoParticion(Nodo);
                }
            else if (e.Button == MouseButtons.Left && Nodo.Level == 4)
                {                 
                    //this.Cursor = Cursors.WaitCursor;//pongo el cursor en estado cargando
                }
        }



        #endregion    Manejo de Nodos ////////////////////////////////////////////////////



        /// <summary> Lee las estructuras de los archivos de tablas que 
        /// que deberan generarse para la carga
        /// </summary>
        /// <returns>Si no hay errores devuelve true</returns>
        private bool LeerEstructurasArchivosTablas() {
            try {
                Vble.cposAltas = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Altas");
                Vble.cposAlumbrado = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Alumbrado");
                Vble.cposComprobantes = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Comprobantes");
                Vble.cposConceptosDatos = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "ConceptosDatos");
                Vble.cposConceptosFacturados = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "ConceptosFacturados");
                Vble.cposConceptosFijos = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "ConceptosFijos");
                Vble.cposConceptosTarifa = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "ConceptosTarifa");
                Vble.cposCondicionIVA = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "CondicionIVA");
                Vble.cposConexiones = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Conexiones");
                Vble.cposExcepciones = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Excepciones");
                Vble.cposGeneral = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "General");
                Vble.cposLecturistas = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Lecturistas");
                Vble.cposLocalidades = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Localidades");
                Vble.cposMedidores = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Medidores");
                Vble.cposNovedades = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Novedades");
                Vble.cposNovedadesConex = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "NovedadesConex");
                Vble.cposPersonas = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Personas");
                Vble.cposTextosVarios = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "TextosVarios");
                Vble.cposVencimientos = Inis.LeerArchivoEstructura(Ctte.ArchivoEstructuraColectora, "Vencimientos");
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Leyendo estructura de Tablas desde Archivos");
                return false;
            }

            return true;
        }


        #region GENERA_ARCHIVOS_CARGAS

        /// <summary>
        ///Metodo que genera la carpeta de Secuencias Seleccionadas procesadas de acuerdo a parametros de creacion como Periodo,
        ///Distrito, Carga Nº, Fecha de generación del procesamiento.
        /// </summary>
        private void GenerarCarpetaArchivo() {
            string ArchivoTabla;
            string archivosecuencia, distrito;
            string Carp;
            int Carga = 0;

            StringBuilder stb = new StringBuilder();

            //Lee y obtiene el nombre de la base Sqlite                        
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string NombreBaseSqliteGeneral = stb1.ToString();


            ////Leer y actualizar el número de carga
            //Inis.GetPrivateProfileString("Cargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
            //Carga = int.Parse(stb.ToString()) + 1;

            //Leer y actualizar el número de carga
            Inis.GetPrivateProfileString("Cargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
            Carga = int.Parse(stb.ToString()) + 1;

    
            //LeerarchivoZonaFIS leo este archivo para tomar el distrito de cabecera el cual va a 
            // ir como nombre del archivo que contendra la carga procesada
            //LeerArchivoZonaFIS();

            distrito = Vble.ArrayZona[0].ToString() == null ? "0" : Vble.ArrayZona[0].ToString();

            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
                CultureInfo.CurrentCulture);
            Carp = string.Format("EP{0:yyyyMM}_D{1:000}_C{2:00000}.{3:yyMMdd_HHmmss}", Per,
                distrito, Carga, DateTime.Now);
            ArchivoTabla = Vble.CarpetaTrabajo + "\\" + Vble.CarpetaCargasNoEnviadas + "\\" + Carp;

            ObtenerValoresDeVariablesSistema();//obtiene valores de los nodos del treeview actuales

            ArchivoTabla = Vble.ValorarUnNombreRuta(ArchivoTabla);


            //Creo variable que contendra el directorio para mostrar en el shellview al finalizar el proceso de Carga
            string DestArchivo = Vble.CarpetaTrabajo + "\\" + Vble.CarpetaCargasNoEnviadas;

            Vble.CarpetasGenerada = Vble.ValorarUnNombreRuta(DestArchivo);

            //Generar los datos y cargarlos al archivo.
            //Crea la ruta que pasa como parametro
            //en caso de que no exista tal directorio como parametro crea uno nuevo con sus datos correspondientes
            Vble.CrearDirectorioVacio(ArchivoTabla);

            string destino = ArchivoTabla + "\\" + NombreBaseSqliteGeneral;
            string destinoBaseFija = ArchivoTabla + "\\" + Vble.NombreArchivoBaseFijaSqlite();

            //ObtenerDatosBaseFija(ObtenerOrigen());

            //copia el archivo de base de datos Sqlite al directorio generado anteriormente.
            Vble.CopiaArchivos(ObtenerOrigen(), destino);
            Vble.CopiaArchivos(ObtenerBaseFija(), destinoBaseFija);

            //File.Copy(Vble.BaseChicaFIS, ArchivoTabla + "\\" + NombreBaseSqliteChica);

            Inis.WritePrivateProfileString("Cargas", Vble.Distrito.ToString(), Carga.ToString(), Ctte.ArchivoIniName);
            //A PARTIR DE ACA GENERO ARCHIVO CON DATOS DE LA CARGA QUE SE PROCESO 
            //COMO SER RUTA, SECUENCIA, CANTIDAD DE REGISTROS CARGADOS, ETC

            string filename = "InfoCarga.txt";
            archivosecuencia = System.IO.Path.Combine(ArchivoTabla, filename);
            //Llamo al metodo que crea el archivo InfoCarga.txt que contiene informacion de la carga procesada
            CrearArchivoInfoCarga(archivosecuencia, filename, "");

            
        }

      

        /// <summary>
        /// Funcion que crea el archivo vacio, con la secuencia como nombre para utilizar como información 
        /// a la hora de mostrar en el "Rutas para cargar"
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        private void CrearArchivoInfoCarga(string archivosecuencia, string filename, string colectora)
        {
            try
            {
                int CantRutas = ArrayRuta.Count;
                StringBuilder stb = new StringBuilder();
                int Carga = 0;

                //Vble.desde = Convert.ToInt32(Vble.TablaConexSelec.Rows[0]["Secuencia"]); //segundo indice indica el nombre de la columna "Secuencia"
                //Vble.hasta = Convert.ToInt32(Vble.TablaConexSelec.Rows[Vble.TablaConexSelec.Rows.Count - 1]["Secuencia"]);//segundo indice indica el nombre de la columna "Secuencia"
                Vble.desde = Convert.ToInt32(Vble.TablaConexSelec.Rows[0][0]); //segundo indice indica el nombre de la columna "Secuencia"
                Vble.hasta = Convert.ToInt32(Vble.TablaConexSelec.Rows[Vble.TablaConexSelec.Rows.Count - 1][0]);//segundo indice indica el nombre de la columna "Secuencia"
                Vble.lineas = "";

                //crea las lineas con la informacion de la carga que se va a procesar
                for (int i = 0; i < ArrayDesde.Count; i++)
                {
                    if (i == (ArrayDesde.Count - 1))
                    {
                        Vble.lineas += ArrayLocalidad[i] + "-" + ArrayRuta[i] + " (" + ArrayDesde[i] + "-" + ArrayHasta[i] + ") " +
                                  ArrayCantConex[i] + colectora;
                    }
                    else
                    {
                        Vble.lineas += ArrayLocalidad[i] + "-" + ArrayRuta[i] + " (" + ArrayDesde[i] + "-" + ArrayHasta[i] + ") " +
                                      ArrayCantConex[i] + colectora + "\n";
                    }

                }

                //Leer y actualizar el número de carga
                Inis.GetPrivateProfileString("Cargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
                Carga = int.Parse(stb.ToString());

                Vble.lineas += "C" + Carga.ToString("00000");

                Vble.CreateInfoCarga(archivosecuencia, filename, Vble.lineas);

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }



        /// <summary>
        /// Funcion que crea el archivo vacio, con la secuencia como nombre para utilizar como como información a la hora de mostrar en el listview1
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        private void CrearArchivoInfoCargaEnviadas(string archivosecuencia, string filename, string colectora)
        {
            String line;
            try
            {


                StreamReader sr = new StreamReader(archivosecuencia);
                //StreamWriter sw = new StreamWriter(archivosecuencia);
                line = sr.ReadLine();
                while (line != null)
                {
                    Vble.lineas += line + colectora;
                    MessageBox.Show(Vble.lineas);
                    line = sr.ReadLine();
                }

                //Close the file
                //sw.Close();
                sr.Close();

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
                    Console.WriteLine("Archivo \"{0}\" inexistente.", filename);
                    return;
                }
                System.IO.File.WriteAllText(archivosecuencia, Vble.lineas);
            }
            catch (Exception)
            {
              
            }
        }

        /// <summary> Recorre el tree view y obtiene los valores para las variables de sistema
        /// según los nodos seleccionados
        /// </summary>
        private void ObtenerValoresDeVariablesSistema() {
            //string Valor = "";
            string clave = "";

            //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
            foreach (TreeNode tNd1 in tvwCargas.Nodes[0].Nodes) {
                if (tNd1.ImageKey != "nada") {
                    clave = tNd1.Tag.ToString();
                    Vble.Distrito = dcNodos[clave].Distrito;
                    //Obtener Remesa, se busca en nivel 2, debajo del nodo clave.
                    foreach (TreeNode tNd2 in tNd1.Nodes) {
                        if (tNd2.ImageKey != "nada") {
                            clave = tNd2.Tag.ToString();
                            Vble.Remesa = dcNodos[clave].Remesa;
                            //Obtener Ruta, se busca en nivel 3, debajo del nodo clave.
                            foreach (TreeNode tNd3 in tNd2.Nodes) {
                                if (tNd3.ImageKey != "nada") {
                                    clave = tNd3.Tag.ToString();
                                    Vble.Ruta = dcNodos[clave].Ruta;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// //Boton que llama al método Cargar productos el cual tiene la sentencia SELECT para ver 
        /// //los registros de la ruta seleccionada en el treeview, si no se selecciona una secuencia 
        /// //no se ejecutara la consulta.
        ///</summary>
        private void tvwCargas_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LabRestDevArc.Visible = false;
            LabRestEnvArc.Visible = false;
            try
            {
               
                TreeNode Nodo = tvwCargas.SelectedNode;
                int idx = Nodo.Index;
                string sKy = Nodo.Tag.ToString();
                clInfoNodos tn = new clInfoNodos();
                tn = dcNodos[Nodo.Tag.ToString()];
                
                if (Nodo != null)
                {
                    //button2_Click(sender, e);//invoca al boton 2 que contiene metodo de recorrer los nodos del treview

                    if (Nodo.Level == 4)
                    {
                        //if (Nodo.ImageKey == "nada" && Nodo.Level == 4)
                        //{
                        //    foreach (var item in NodosSeleccionados)
                        //    {
                        //        if (item.ToString() == Nodo.Parent.Tag.ToString())
                        //        {
                        //            NodosSeleccionados.Remove(item.ToString());
                        //        }
                        //    }
                        //}

                        RestNod.Visible = false;
                        Vble.CantNodosDesde = ArrayDesde.Count;//variable que utilizo para saber cuantas Secuencias va a contener mi consulta                         
                                                               //versecuencia(Nodo);
                        //MessageBox.Show("Localidad: " + tn.Distrito + " Remesa: " + tn.Remesa + " Ruta: " + tn.Ruta + " Desde: " + tn.Desde + " Hasta: " + tn.Hasta);

                        // }
                        if (Vble.CantNodosDesde > 0)
                        {                           
                            Vble.TablaConexSelec.Reset();
                            //Vble.TablaConexSelec = CargarRegistrosSecuenciaP();
                            Vble.TablaConexSelec = CargarRegistrosDisponibles();

                            labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                        }
                        else
                        {
                            Vble.TablaConexSelec.Reset();                          
                            labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                        }

                       

                    }
                    else if (Nodo.Level == 3 || Nodo.Level == 2 || Nodo.Level == 1 || Nodo.Level == 0)
                    {
                        RestNod.Text = " *Por favor seleccione solo los Nodos que contiene las secuencias \n                                        Ej: A(1 a 100)[100]";
                        this.dataGridView1.DataSource = "";
                        this.labelCantReg.Text = "0";
                        RestNod.Visible = true;
                        //tn.ImageKey = "nada";
                        Nodo.ImageKey = tn.ImageKey;
                        Nodo.SelectedImageKey = tn.ImageKey;
                        AplicarEstadoAHijos(Nodo);
                        TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
                        button2_Click(sender, e);
                        //RestNod.Visible = false;
                        Vble.CantNodosDesde = ArrayDesde.Count;//variable que utilizo para saber cuantas Secuencias va a contener mi consulta                         
                                                               //versecuencia(Nodo);
                        //MessageBox.Show("Localidad: " + tn.Distrito + " Remesa: " + tn.Remesa + " Ruta: " + tn.Ruta + " Desde: " + tn.Desde + " Hasta: " + tn.Hasta);
                        // }
                        if (Vble.CantNodosDesde > 0)
                        {
                            Vble.TablaConexSelec.Clear();
                            //Vble.TablaConexSelec = CargarRegistrosSecuenciaP();
                            Vble.TablaConexSelec = CargarRegistrosDisponibles();
                            labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                        }
                        else
                        {
                            Vble.TablaConexSelec.Reset();
                            labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                        }

                    }
                }
                
            }
            catch (Exception)
            {

            }
        }


        private void shellView1_Click(object sender, EventArgs e)
        {

        }


        public void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracion(string Periodo)
        {
            string where = "";
            try
            {
                for (int i = 0; i < ArrayDesde.Count; i++)
                {

                    //where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //" and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 0 + " AND C.Periodo =  " + Periodo + ") " +
                    //"OR (C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //" and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 500 + " AND C.Periodo =  " + Periodo + ")) ";

                    where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                           " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500) AND (C.Periodo = " + Periodo + " AND P.Periodo = " + Periodo +
                            " AND M.Periodo = " + Periodo + " AND H.Periodo = " + Periodo + "))) ";


                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }
        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracionGPS(string Periodo)
        {
            string where = "";
            try
            {
                for (int i = 0; i < ArrayDesde.Count; i++)
                {

                    //where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //" and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 0 + " AND C.Periodo =  " + Periodo + ") " +
                    //"OR (C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //" and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 500 + " AND C.Periodo =  " + Periodo + ")) ";

                    //where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //       " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500) AND (C.Periodo = " + Periodo + " AND P.Periodo = " + Periodo +
                    //        " AND M.Periodo = " + Periodo + " AND H.Periodo = " + Periodo + "))) ";

                    where += " OR (C.Ruta = " + ArrayRuta[i] +  ")";
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }
        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracionDisponibles(string Periodo)
        {
            string where = "";
            try
            {
                for (int i = 0; i < ArrayDesde.Count; i++)
                {

                    //where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //" and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 0 + " AND C.Periodo =  " + Periodo + ") " +
                    //"OR (C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                    //" and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 500 + " AND C.Periodo =  " + Periodo + ")) ";

                    where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                           " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500) AND (C.Periodo = " + Periodo + "))) ";
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }

        /// <summary>
        /// Metodo que contiene la consulta SELECT para obtener los registros que esten de las tablas Conexiones, Personas y Medidores
        /// dentro de la secuencia seleccionada del treeview con un solo valor de secuencia DESDE y HASTA, Numero de Ruta
        /// y Periodo
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosSecuenciaP()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            string Periodo = "";
            Periodo = Vble.PeriodoFORM0;
            try
            {              
                if (ArrayDesde.Count > 1)
                {
                    txSQL = "select DISTINCT C.*, P.*, M.*, H.* " +
                            "From Conexiones C " +
                            "INNER JOIN Personas P ON C.titularID = P.personaID AND C.Periodo = P.Periodo " +
                            "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                            "INNER JOIN Historial H ON C.ConexionID = H.ConexionID AND C.Periodo = H.Periodo " +                            
                            "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                            " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500)  AND (C.Periodo = " + Periodo + " AND P.Periodo = " + Periodo +
                            " AND M.Periodo = " + Periodo + " AND H.Periodo = " + Periodo + ") " + iteracion(Periodo) + ") ORDER BY C.Secuencia ASC";
                }
                else
                {
                    txSQL = "select distinct C.*, P.*, M.*, H.* " +
                            "From Conexiones C " +
                            "INNER JOIN Personas P ON C.titularID = P.PersonaID AND C.Periodo = P.Periodo " +
                            "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                            "INNER JOIN Historial H ON C.ConexionID = H.ConexionID AND C.Periodo = H.Periodo " +                            
                            "Where ((C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + ") and C.Ruta = " + ArrayRuta[0] +
                            " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500) AND (C.Periodo = " + Periodo + " AND P.Periodo = " + Periodo +
                            " AND M.Periodo = " + Periodo + " AND H.Periodo = " + Periodo + ")) ORDER BY C.Secuencia ASC";
                    //"GROUP BY C.ConexionID, C.Periodo, C.Ruta " +
                    //"HAVING C.Periodo = " + Periodo + " AND (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500 " +
                    //"OR C.ImpresionOBS = 800) AND C.Ruta = " + ArrayRuta[0] + 
                    //" AND (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + ")";
                }                
                Tabla = new DataTable();                
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                da.SelectCommand.CommandTimeout = 900;
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                dataGridView1.DataSource = Tabla;
                buscarPuntosGPSPerAnterior(dataGridView1, Periodo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Tabla;
        }

        /// <summary>
        /// Metodo que recorrera la lista de usuarios seleccionados antes de ser procesados y cargados
        /// a la base SQLite para su posterior carga y compara con la misma
        /// selección pero del periodo anterior para buscar la posición GPS en caso de que tenga y asigna a su correspondiente usuario.
        /// En caso de que no encuentre posición en el periodo anterior lo carga en 0 al campo Latitud y Longitud en la tabla Medidores.
        /// EN PRINCIPIO SE COLOCA CERO SI NO ENCUENTRA
        /// </summary>
        /// <param name="dataGridView1"></param>
        /// <param name="Periodo"></param>
        private void buscarPuntosGPSPerAnterior(DataGridView dataGridView1, string Periodo)
        {
            Int32 PerAnterior = Convert.ToInt32(Periodo);
            Int32 AñoPer = Convert.ToInt32(Periodo.Substring(0, 4));
            MySqlDataAdapter daPerAnt;
            MySqlCommandBuilder comandoSQLPerAnt;            
            string txSQL = "";
            try
            {
                int numUltPer = Convert.ToInt16(Periodo.Substring(5,1));
                if (numUltPer == 1)
                {
                    string PerAnt = (String.Concat(AñoPer - 1) + "06");
                    PerAnterior = Convert.ToInt32(PerAnt);
                }
                else
                {
                    PerAnterior = Convert.ToInt32(Periodo) - 1;
                }

                if (ArrayDesde.Count > 1)
                {

                    txSQL = "SELECT DISTINCT C.*, M.* FROM Conexiones C INNER JOIN Medidores M USING (ConexionId, Periodo) " +
                            "WHERE ( Ruta = "  + ArrayRuta[0] + iteracionGPS(PerAnterior.ToString()) + ") AND Periodo = " + PerAnterior + " ORDER BY C.Secuencia ASC";
                    //txSQL = "select DISTINCT C.*, P.*, M.*, H.* " +
                    //        "From Conexiones C " +
                    //        "INNER JOIN Personas P ON C.titularID = P.personaID AND C.Periodo = P.Periodo " +
                    //        "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                    //        "INNER JOIN Historial H ON C.ConexionID = H.ConexionID AND C.Periodo = H.Periodo " +
                    //        "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                    //        " and  (C.Periodo = " + PerAnterior + " AND P.Periodo = " + PerAnterior +
                    //        " AND M.Periodo = " + PerAnterior + " AND H.Periodo = " + PerAnterior + ") " + iteracion(PerAnterior.ToString()) + ") ORDER BY C.Secuencia ASC";
                }
                else
                {
                    //txSQL = "select distinct C.*, P.*, M.*, H.* " +
                    //        "From Conexiones C " +
                    //        "INNER JOIN Personas P ON C.titularID = P.PersonaID AND C.Periodo = P.Periodo " +
                    //        "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                    //        "INNER JOIN Historial H ON C.ConexionID = H.ConexionID AND C.Periodo = H.Periodo " +
                    //        "Where ((C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + ") and C.Ruta = " + ArrayRuta[0] +
                    //        " and (C.Periodo = " + PerAnterior + " AND P.Periodo = " + PerAnterior +
                    //        " AND M.Periodo = " + PerAnterior + " AND H.Periodo = " + PerAnterior + ")) ORDER BY C.Secuencia ASC";
                    txSQL = "SELECT DISTINCT  C.ConexionID,  M.Latitud, M.Longitud " +
                       "FROM Conexiones C " +
                       "INNER JOIN Medidores M USING(ConexionID, Periodo) WHERE " +
                       "((C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + ") and C.Ruta = " + ArrayRuta[0] +
                       " and C.Periodo = " + PerAnterior + ") ORDER BY C.Secuencia ASC";
                }

                DataTable TablaPerAnt = new DataTable();
                daPerAnt = new MySqlDataAdapter(txSQL, DB.conexBD);
                daPerAnt.SelectCommand.CommandTimeout = 900;
                comandoSQLPerAnt = new MySqlCommandBuilder(daPerAnt);
                daPerAnt.Fill(TablaPerAnt);
                dataGridViewPerAnt.DataSource = TablaPerAnt;
                decimal LatPerAnt = 0;
                decimal LongPerAnt = 0;
                ;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToDecimal(row.Cells["Latitud"].Value) == 0)
                    {
                        for (int i = 0; i < TablaPerAnt.Rows.Count; i++)
                        {
                            if (row.Cells["ConexionID"].Value.ToString() == TablaPerAnt.Rows[i]["ConexionID"].ToString())
                            {
                                if (Convert.ToDecimal(TablaPerAnt.Rows[i]["Latitud"].ToString()) != 0)
                                {
                                    row.Cells["Latitud"].Value = Convert.ToDecimal(TablaPerAnt.Rows[i]["Latitud"].ToString());
                                    row.Cells["Longitud"].Value = Convert.ToDecimal(TablaPerAnt.Rows[i]["Longitud"].ToString());
                                }
                                
                            }
                        }
                    }
                }
            }
               catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
                  
         

        /// <summary>
        /// Metodo que contiene la consulta SELECT para obtener los registros que esten de las tablas Conexiones, Personas y Medidores
        /// dentro de la secuencia seleccionada del treeview con un solo valor de secuencia DESDE y HASTA, Numero de Ruta
        /// y Periodo
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosDisponibles()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            string Periodo = "";
            Periodo = Vble.PeriodoFORM0;

            try
            {
                
                if (ArrayDesde.Count > 1)
                {
                    txSQL = "select DISTINCT C.* " +
                            "From Conexiones C " +                           
                            "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                            " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500)  AND (C.Periodo = " + Periodo + ") " + iteracionDisponibles(Periodo) + ") ORDER BY C.Secuencia ASC";
                }
                else
                {
                    txSQL = "select distinct C.* " +
                            "From Conexiones C " +                            
                            "Where ((C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + ") and C.Ruta = " + ArrayRuta[0] +
                            " and (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500) AND (C.Periodo = " + Periodo + " )) ORDER BY C.Secuencia ASC";
                    //"GROUP BY C.ConexionID, C.Periodo, C.Ruta " +
                    //"HAVING C.Periodo = " + Periodo + " AND (C.ImpresionOBS = 0 OR C.ImpresionOBS = 500 " +
                    //"OR C.ImpresionOBS = 800) AND C.Ruta = " + ArrayRuta[0] + 
                    //" AND (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + ")";
                }

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                da.SelectCommand.CommandTimeout = 900;
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                dataGridView1.DataSource = Tabla;


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }

            return Tabla;

        }



        /// <summary>
        /// //metodo que me carga los textbox el from4 para control de programación
        /// </summary>
        /// <param name="Nodo"></param>
        private void versecuencia(TreeNode Nodo)
        {

            int idx = Nodo.Index;
            string sKy = Nodo.Tag.ToString();
            clInfoNodos tn = new clInfoNodos();
            tn = dcNodos[Nodo.Tag.ToString()];
            Loc.Text = tn.Distrito.ToString();
            Ru.Text = tn.Ruta.ToString();
            textBox2.Text = tn.Desde.ToString();
            textBox3.Text = tn.Hasta.ToString();
            textBox1.Text = tn.Particion.ToString();
            //MessageBox.Show(vectorSecDesde.secuencianodos[f].ToString());
        }


        /// <summary>
        /// //Metodo que contiene la consulta SELECT para cargar los registros  
        /// //a la tabla conexiones de la base de datos SQLITE el cual contiente la misma estructura que la base
        /// //de datos MySQL
        /// </summary>
        /// <returns></returns>
        /// 
        private DataTable CargarRegistrosConexiones()
        {
            string txSQL;
            try
            {
                //txSQL = "select * From conexiones Where conexionID
                txSQL = "INSERT INTO conexiones([conexionID], [Periodo], [usuarioID], [titularID], [propietarioID], [DomicSumin]," +
                    " [BarrioSumin], [CodPostalSumin], [CuentaDebito], [ImpresionCOD], [ImpresionOBS], [ImpresionCANT], [Operario]," +
                    " [Lote], [Zona], [Ruta], [Secuencia], [Remesa], [Categoria], [SubCategoria], [TipoProrrateo], [ConsumoPromedio]," +
                    " [PromedioDiario], [ConsumoResidual], [ConsumoFacturado], [ConsumoTipo], [OrdenTomado], [CESPnumero], [CESPvencimiento]," +
                    " [FacturaLetra], [PuntoVenta], [FacturaNro1], [DocumPago1], [Vencimiento1], [Importe1], [FacturaNro2], [DocumPago2]," +
                    " [Vencimiento2], [Importe2], [VencimientoProx], [HistoPeriodo01], [HistoConsumo01], [HistoPeriodo02], [HistoConsumo02]," +
                    " [HistoPeriodo03], [HistoConsumo03], [HistoPeriodo04], [HistoConsumo04], [HistoPeriodo05], [HistoConsumo05], [HistoPeriodo06]," +
                    " [HistoConsumo06], [HistoPeriodo07], [HistoConsumo07], [HistoPeriodo08], [HistoConsumo08], [HistoPeriodo09], [HistoConsumo09]," +
                    " [HistoPeriodo10], [HistoConsumo10], [HistoPeriodo11], [HistoConsumo11], [HistoPeriodo12],[HistoConsumo12]) " +
                    "VALUES () from conexiones";

                //preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(txSQL, DB.con);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return Tabla;

        }

        /// <summary>
        /// //metodo que carga la tabla conexiones de la base SQLite,
        /// </summary>
        /// <param name="codconex"></param>Numero de conexionID que identifica al registro
        /// <param name="TipoProrrateo"></param>Tipo de prorrateo a aplicar segun se seleccionó antes de procesar la carga
        ///                                     por defecto queda seleccionado sin prorrateo
        /// <returns></returns>
         //private void CargarRegistrosConexion(int codconex, int periodo, string FechaCalP, string Contrato, string Instalacion,
                                         //int usuarioid, int titularid, int propietarioid, string DomicSumin, string BarrioSumin,
                                         //string CodPostalSumin, string CuentaDebito, int impresionCod, int impresionOBS, int impresionCant,
                                         //int Operario, int lote, int zona, int ruta, int secuencia, int remesa, string TarifaCod,
                                         //string TarifaText, int ConsumoControl, int ConsumoResidual, int ConsumoFacturado,
                                         //int OrdenTomado, string VencimientoProx)
        private void CargarRegistrosConexion(string insert)        
        {
            //MySqlDataAdapter da;
            //MySqlCommandBuilder comandoSQL;
            //string txSQL;
            try
            {
                //txSQL = "select * From Conexiones Where ConexionID = " + codconex + " AND Periodo = " + Vble.PeriodoFORM0;
                //Tabla = new DataTable();
                //da = new MySqlDataAdapter(txSQL, DB.conexBD);
                //comandoSQL = new MySqlCommandBuilder(da);
                //da.Fill(Tabla);
                ////asignación a variables locales para manejar en el INSERT
                //foreach (DataRow fi in Tabla.Rows)
                //{

                    //////declaracióno de variables
                    ////int conexionid, periodo, usuarioid, titularid, propietarioid, impresionCod, impresionOBS, impresionCant,
                    ////    Operario, lote, zona, ruta, secuencia, remesa, ConsumoResidual, ConsumoFacturado, OrdenTomado,
                    ////     ConsumoControl;
                    ////string DomicSumin, Instalacion, BarrioSumin, CodPostalSumin, CuentaDebito,
                    ////     FechaCalP, Contrato, TarifaCod, TarifaText;
                    ////string VencimientoProx;

                    ////Asignacion de variables
                    //conexionid = (int)fi["ConexionID"];
                    //periodo = (int)fi["Periodo"];
                    //FechaCalP = fi["FechaCalP"].ToString();
                    //Contrato = fi["Contrato"].ToString();
                    //Instalacion = fi["Instalacion"].ToString();
                    //usuarioid = (int)fi["usuarioID"];
                    //titularid = (int)fi["titularID"];
                    //propietarioid = (int)fi["propietarioID"];
                    //DomicSumin = fi["DomicSumin"].ToString();
                    //BarrioSumin = fi["LocalidadSumin"].ToString();
                    //CodPostalSumin = fi["CodPostalSumin"].ToString();
                    //CuentaDebito = fi["CuentaDebito"].ToString();
                    //impresionCod = (int)fi["ImpresionCOD"];
                    //impresionOBS = (int)fi["ImpresionOBS"];
                    //impresionCant = (int)fi["ImpresionCANT"];
                    //Operario = (int)fi["Operario"];
                    //lote = (int)fi["Lote"];
                    //zona = (int)fi["Zona"]; ruta = (int)fi["Ruta"];
                    //secuencia = (int)fi["Secuencia"];
                    //remesa = (int)fi["Remesa"];
                    //TarifaCod = fi["TarifaCod"].ToString();
                    //TarifaText = fi["TarifaTex"].ToString();
                    //ConsumoControl = (int)fi["ConsumoControl"];
                    //ConsumoResidual = (int)fi["ConsumoResidual"];
                    //ConsumoFacturado = (int)fi["ConsumoFacturado"];
                    //OrdenTomado = (int)fi["OrdenTomado"];
                    //VencimientoProx = fi["VencimientoProx"].ToString();

                    //string insert;//Declaración de insert que contendra la consulta INSERT  
                    //insert = "INSERT INTO Conexiones ([conexionID], [Periodo], [FechaCalP], [Contrato], [Instalacion], [usuarioID], [titularID], [propietarioID], [DomicSumin], [LocalidadSumin]," +
                    //    " [CodPostalSumin], [CuentaDebito], [ImpresionCOD], [ImpresionOBS], [ImpresionCANT], [Operario]," +
                    //    " [Lote], [Zona], [Ruta], [Secuencia], [Remesa], [TarifaCod], [TarifaTex], [ConsumoControl], [ConsumoResidual],[ConsumoFacturado], [OrdenTomado]," +
                    //    " [VencimientoProx]) " +
                    //    "VALUES ('" + codconex + "', '" + periodo + "', '" + FechaCalP + "', '" + Contrato + "', '" + Instalacion + "', '" + usuarioid +
                    //    "', '" + titularid + "', '" + propietarioid + "', '" + DomicSumin + "', '" + BarrioSumin + "', '" + CodPostalSumin +
                    //    "', '" + CuentaDebito + "', '" + impresionCod + "', '" + impresionOBS + "', '" + impresionCant + "', '" + Operario +
                    //    "', '" + lote + "', '" + zona + "', '" + ruta + "', '" + secuencia + "', '" + remesa + "', '" + TarifaCod +
                    //    "', '" + TarifaText + "', " + ConsumoControl + ", " + ConsumoResidual + ", " + ConsumoFacturado +
                    //    ", " + OrdenTomado + ", '" + VencimientoProx + "')";

                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();


                //}
                //comandoSQL.Dispose();
                //da.Dispose();


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error en el metodo CargarRegistrosConexion");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                                " Error en el metodo CargarRegistrosConexion al momento en el que se estuvo " +
                                                "procesando la carga del registros de conexiones. \n");
            }
            //return Tabla;
        }

        /// <summary>
        ///  devuelve true si existe algun concetoDatos correspondiente a la conexion id pasada por parametro
        /// </summary>
        /// <returns></returns>
        public static bool ExisteConceptosFacturados(Int32 conexionID)
        {
            string txSQL;
            MySqlCommand da;
            try
            {
                txSQL = "SELECT * FROM conceptosfacturados WHERE ConexionID = " + conexionID + " and Periodo = " + Vble.Periodo;

                da = new MySqlCommand(txSQL, DB.conexBD);
                da.Parameters.AddWithValue("ConexionID", conexionID);
                //DB.conexBD.Open();

                int count = Convert.ToInt32(da.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }

            catch (Exception)
            {
                //MessageBox.Show(e.Message);
            }
            return false;
        }



        /// <summary>
        ///  Metodo que verifica si existe la personas en tabla Personas segun su ID"dni"
        /// </summary>
        /// <returns></returns>
        public static bool ExistePersona(int personaID)
        {
            string txSQL;
            SQLiteCommand da;

            try
            {
                txSQL = "SELECT * FROM Personas WHERE personaID = " + personaID;

                da = new SQLiteCommand(txSQL, DB.con);
                da.Parameters.AddWithValue("personaID", personaID);
                DB.con.Open();

                int count = Convert.ToInt32(da.ExecuteScalar());

                if (count == 0)
                    return false;
                //retorno = false;
                else
                    return true;
                //retorno = true;
            }

            catch (Exception)
            {
                //MessageBox.Show(e.Message + " Erro al verificar Persona.");
            }
            return false;
        }

        /// <summary>
        ///  devuelve true si existe algun concetoDatos correspondiente a la conexion id pasada por parametro
        /// </summary>
        /// <returns></returns>
        public static bool ExisteConceptoDatos(int conexionID)
        {
            string txSQL;
            MySqlCommand da;
            try
            {
                txSQL = "SELECT * FROM conceptosdatos WHERE ConexionID = " + conexionID + " AND Periodo = " + Vble.Periodo;

                da = new MySqlCommand(txSQL, DB.conexBD);
                da.Parameters.AddWithValue("ConexionID", conexionID);
                //DB.conexBD.Open();

                int count = Convert.ToInt32(da.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }

            catch (Exception)
            {
                //MessageBox.Show(e.Message);
            }
            return false;
        }



        /// <summary>
        ///  Verifica que exista registros con esa conexionID en la tabla TextosVarios
        /// </summary>
        /// <returns></returns>
        public static bool ExisteTextoVario(int conexionID)
        {
            string txSQL;
            MySqlCommand da;
            try
            {
                txSQL = "SELECT * FROM textosvarios WHERE ConexionID = " + conexionID + " AND Periodo = " + Vble.Periodo;

                da = new MySqlCommand(txSQL, DB.conexBD);
                da.Parameters.AddWithValue("conexionID", conexionID);
                int count = Convert.ToInt32(da.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        ///  Verifica que exista registros con esa conexionID en la tabla Excepciones
        /// </summary>
        /// <returns></returns>
        public static bool ExisteExepciones(int conexionID)
        {
            string txSQL;
            MySqlCommand da;
            try
            {
                txSQL = "SELECT * FROM excepciones WHERE ConexionID = " + conexionID + " AND Periodo = " + Vble.Periodo;

                da = new MySqlCommand(txSQL, DB.conexBD);
                da.Parameters.AddWithValue("conexionID", conexionID);
                int count = Convert.ToInt32(da.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
        /// <summary>
        ///  Devuelve la cantidad de registros que existen en la tabla ConceptosDatos
        ///  ya que puede ocurrir que haya mas de una conexion con el mismo ID pero distintos conceptos
        /// </summary>
        /// <returns></returns>
        public static int CantidadConceptoDatos(int conexionID)
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM conceptosdatos WHERE ConexionID = " + conexionID;
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue("ConexionID", conexionID);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }

        /// <summary>
        /// //Método que carga la tabla medidores de la base SQLite
        /// </summary>
        /// <param name="codconex"></param>
        /// <returns></returns>
        private void CargarRegistrosMedidores(string insert)
        //private void CargarRegistrosMedidores(int codconex)
        //private void CargarRegistrosMedidores(int codconex, int periodo, int Orden, int Multiplicador, int Digitos, int AnteriorEstado,
        //                                      int ActualEstado, int TipoLectura, Int64 Numero, string Modelo, string ActualHora,
        //                                     string ActualFecha, DateTime AnteriorFecha)
        {
            //MySqlDataAdapter da;
            //MySqlCommandBuilder comandoSQL;
            //string txSQL;
            try
            {
                //txSQL = "select * From Medidores Where ConexionID = " + codconex + " AND Periodo = " + Vble.Periodo;

                //Tabla = new DataTable();
                //da = new MySqlDataAdapter(txSQL, DB.conexBD);
                //comandoSQL = new MySqlCommandBuilder(da);
                //da.Fill(Tabla);

                //foreach (DataRow fi in Tabla.Rows)
                //{
                    ////declaracióno de variables
                    //int conexionid, periodo, Orden, Multiplicador, Digitos, AnteriorEstado, ActualEstado, TipoLectura;
                    //Int64 Numero;
                    //string Modelo, ActualHora, ActualFecha;
                    //DateTime AnteriorFecha;
                    ////asignación a variables locales para manejar en el INSERT
                    //conexionid = (int)fi["ConexionID"];
                    //periodo = (int)fi["Periodo"];
                    //Orden = fi["Orden", DataRowVersion.Original].ToString() == "" ? 0 : (int)fi["Orden"];
                    ////Orden = (int)fi["Orden"];
                    //Modelo = fi["Modelo"].ToString();
                    //Numero = (Int64)fi["Numero"];
                    //Multiplicador = (int)fi["Multiplicador"];
                    //Digitos = (int)fi["Digitos"];
                    //AnteriorFecha = fi["AnteriorFecha", DataRowVersion.Original].ToString() == "" ? DateTime.Parse("01/01/2000") : DateTime.Parse(fi["AnteriorFecha"].ToString());
                    ////AnteriorFecha = fi["AnteriorFecha"].ToString();
                    //AnteriorEstado = fi["AnteriorEstado", DataRowVersion.Original].ToString() == "" ? 0 : (int)fi["AnteriorEstado"];
                    ////AnteriorEstado = (int)fi["AnteriorEstado"];
                    ////ActualFecha = fi["ActualFecha", DataRowVersion.Original].ToString() == "" ? DateTime.Parse("01/01/2000") : DateTime.Parse(fi["ActualFecha"].ToString());
                    //ActualFecha = fi["ActualFecha", DataRowVersion.Original].ToString() == "" || fi["ActualFecha", DataRowVersion.Original].ToString() == "1/1/2000 12:00:00 a. m."
                    //              || fi["ActualFecha", DataRowVersion.Original].ToString() == "0" ? "0" : DateTime.Parse(fi["ActualFecha"].ToString()).ToString();
                    ////ActualFecha = fi["ActualFecha", DataRowVersion.Original].ToString() == "" ? DateTime.Parse("01/01/2000") : "0");
                    ////ActualFecha = fi["ActualFecha"].ToString();
                    ////ActualHora = fi["ActualHora", DataRowVersion.Original].ToString() == "" ? "0" : fi["ActualHora"].ToString();
                    //ActualHora = fi["ActualHora", DataRowVersion.Original].ToString() == "" || fi["ActualHora", DataRowVersion.Original].ToString() == "00:00:00"
                    //             || fi["ActualHora", DataRowVersion.Original].ToString() == "0" ? "0" : fi["ActualHora"].ToString();
                    ////ActualHora = fi["ActualHora"].ToString();
                    //ActualEstado = fi["ActualEstado", DataRowVersion.Original].ToString() == "" ||
                    //               fi["ActualEstado", DataRowVersion.Original].ToString() == "0" ? 0 : (int)fi["ActualEstado"];
                    ////ActualEstado = (int)fi["ActualEstado"];
                    //TipoLectura = fi["TipoLectura", DataRowVersion.Original].ToString() == "" ||
                    //              fi["TipoLectura", DataRowVersion.Original].ToString() == "0" ? 0 : (int)fi["TipoLectura"];
                    ////TipoLectura = (int)fi["TipoLectura"];

                    //string insert;//Declaración de string que contendra la consulta INSERT
                    //insert = "INSERT INTO Medidores ([conexionID], [Periodo], [Orden], [Modelo], [Numero], [Multiplicador], [Digitos]," +
                    //    " [AnteriorFecha], [AnteriorEstado], [ActualFecha], [ActualHora], [ActualEstado], [TipoLectura]) " +
                    //    "VALUES ('" + codconex + "', '" + periodo + "', '" + Orden + "', '" + Modelo + "', " + Numero + ", '" + Multiplicador + "', '" + Digitos +
                    //    "', '" + AnteriorFecha.ToString("dd/MM/yyyy") + "', '" + AnteriorEstado + "', '" + ActualFecha.ToString() + "', '" + ActualHora + "', '" + ActualEstado + "', '" + TipoLectura + "')";

                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                //}
                //comandoSQL.Dispose();
                //da.Dispose();

            }
            catch (Exception r)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(r.Message);
            }
            //return Tabla;
        }

        /// <summary>
        /// //Método que obtiene y carga los datos de cada conexionID que contiene ConceptosDatos de la base MySQL en Tabla ConceptosDatos de SQLite
        /// </summary>
        /// <param name="codconex"></param>
        /// <returns></returns>
        private DataTable CargaRegistrosConceptosDatos(int codconex)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From Conceptosdatos Where ConexionID = " + codconex + " AND Periodo = " + Vble.Periodo;
                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);


                foreach (DataRow fi in Tabla.Rows)
                {
                    int conexionid, periodo, CodigoConcepto, CodigoEscalon, CodigoAux, CodigoGrupo, CalcularBase, CuotaUno, AplicarBase;
                    double Unitario, Cantidad, CalcularDesde, CalcularHasta, AplicarDesde, AplicarHasta, CantMinima, CantMaxima;
                    string CodigoDpec, TextoEscalon, TextoUnidades, Subtotales, ImprimeSiCero, ImprimeSubtotal, Prorrateo, VigenciaDesde, VigenciaHasta;

                    //MessageBox.Show(fi["conexionID"].ToString() + " " + fi["CodigoConcepto"].ToString());
                    conexionid = (int)fi["ConexionID"];//columna que contiene la conexionID
                    periodo = (int)fi["Periodo"];// columna que contiene el Periodo
                    CodigoConcepto = (int)fi["CodigoConcepto"];//columna que contiene CodigoConcepto
                    CodigoDpec = fi["CodigoDpec"].ToString();
                    CodigoEscalon = (int)fi["CodigoEscalon"];//columna que contiene CodigoEscalon
                    CodigoAux = (int)fi["CodigoAux"];// columna que contiene CodigoAux                    
                    CodigoGrupo = (int)fi["CodigoGrupo"];// columna que contiene CodigoGrupo
                    TextoEscalon = fi["TextoEscalon"].ToString();//columna que contiene TextoEscalon
                    TextoUnidades = fi["TextoUnidades"].ToString();//columna que contiene TextoUnidades                    
                    CalcularBase = (int)fi["CalcularBase"];
                    CalcularDesde = (double)(fi["CalcularDesde"]);
                    CalcularHasta = (double)(fi["CalcularHasta"]);
                    AplicarBase = (int)fi["AplicarBase"];
                    AplicarDesde = (double)fi["AplicarDesde"];
                    AplicarHasta = (double)fi["AplicarHasta"];
                    Subtotales = fi["Subtotales", DataRowVersion.Original].ToString() == "" ? "0" : fi["Subtotales"].ToString();
                    //Subtotales = fi["Subtotales"].ToString();    
                    CantMinima = fi["CantMinima", DataRowVersion.Original].ToString() == "" ? 0 : (double)fi["CantMinima"];
                    //CantMinima = (double)fi["CantMinima"];                    
                    CantMaxima = (double)fi["CantMaxima"];
                    ImprimeSiCero = fi["ImprimeSiCero"].ToString();
                    ImprimeSubtotal = fi["ImprimeSubtotal", DataRowVersion.Original].ToString() == "" ? "0" : fi["ImprimeSubtotal"].ToString();
                    //ImprimeSubtotal = fi["ImprimeSubtotal"].ToString();
                    Prorrateo = fi["Prorrateo"].ToString();
                    CuotaUno = (int)fi["CuotaUno"];
                    Cantidad = (double)fi["Cantidad"];
                    Unitario = (double)fi["Unitario"];
                    VigenciaDesde = fi["VigenciaDesde"].ToString();
                    VigenciaHasta = fi["VigenciaHasta"].ToString();
                    //llamo al procedimiento que contiene el insert de sqlite de los datos conceptosdatos que se pasan por parametros obtenidos del datatable(Tabla)
                    cargartablaConceptosDatos(conexionid, periodo, CodigoConcepto, CodigoDpec, CodigoEscalon, CodigoAux, CodigoGrupo, TextoEscalon, TextoUnidades,
                                              CalcularBase, CalcularDesde, CalcularHasta, AplicarBase, AplicarDesde, AplicarHasta, Subtotales,
                                              CantMinima, CantMaxima, ImprimeSiCero, ImprimeSubtotal, Prorrateo, CuotaUno, Cantidad, Unitario, VigenciaDesde, VigenciaHasta);
                }
                comandoSQL.Dispose();
                da.Dispose();

            }
            catch (Exception)
            {

            }

            return Tabla;
        }

        /// <summary>
        /// //Método que obtiene y carga los datos de cada conexionID que contiene TextosVarios de la base MySQL en Tabla TextosVarios de SQLite
        /// </summary>
        /// <param name="codconex"></param>
        /// <returns></returns>
        private DataTable CargaTextosVariosSqlite(int codconex)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From TextosVarios Where ConexionID = " + codconex + " AND Periodo = " + Vble.Periodo;

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                foreach (DataRow fi in Tabla.Rows)
                {
                    int conexionid, periodo, Renglon;
                    string Texto;
                    conexionid = (int)fi[0];//columna que contiene la conexionID
                    periodo = (int)fi[1];// columna que contiene el Periodo
                    Renglon = (int)fi[2];
                    Texto = fi[3].ToString();

                    string insert;//Declaración de string que contendra la consulta INSERT
                    insert = "INSERT INTO TextosVarios ([conexionID], [Periodo], [Renglon], [Texto]) " +
                           "VALUES ('" + conexionid + "', '" + periodo + "', '" + Renglon + "', '" + Texto + "')";
                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //asignación a variables locales para manejar en el INSERT de la tabla Conceptos Datos
                }
                comandoSQL.Dispose();
                da.Dispose();
            }
            catch (Exception)
            {

            }
            //this.dataGridView5.DataSource = Tabla;
            return Tabla;
        }

        /// <summary>
        /// //Método que obtiene y carga los datos de cada conexionID que contiene Excepciones de la base MySQL en Tabla Excepciones de SQLite
        /// </summary>
        /// <param name="codconex"></param>
        /// <returns></returns>
        private DataTable CargaExcepcionesSqlite(int codconex)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From excepciones Where ConexionID = " + codconex;

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);


                foreach (DataRow fi in Tabla.Rows)
                {
                    int conexionid, periodo;
                    string Excepciones;
                    conexionid = (int)fi[0];//columna que contiene la conexionID
                    periodo = (int)fi[1];// columna que contiene el Periodo
                    Excepciones = fi[2].ToString();

                    if (Excepciones == "0")
                    {
                        string update;//Declaración de string que contendra la consulta UPDATE               
                        update = "UPDATE Conexiones SET ImpresionCOD = " + 1 + " WHERE conexionID = " + conexionid + " AND Periodo = " + Vble.PeriodoFORM0;
                        //preparamos la cadena pra insercion
                        SQLiteCommand command = new SQLiteCommand(update, DB.con);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                    }
                    else
                    {
                        string insert;//Declaración de string que contendra la consulta INSERT
                        insert = "INSERT INTO Excepciones ([conexionID], [Periodo], [Excepciones]) " +
                               "VALUES ('" + conexionid + "', '" + periodo + "', '" + Excepciones + "')";
                        //preparamos la cadena pra insercion
                        SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                        //asignación a variables locales para manejar en el INSERT de la tabla Conceptos Datos
                    }


                }
                comandoSQL.Dispose();
                da.Dispose();
            }
            catch (Exception)
            {

            }
            //this.dataGridView5.DataSource = Tabla;
            return Tabla;
        }


        /// <summary>
        /// //Método que carga la tabla varios de la base SQLite para cada ProcesarCarga
        /// </summary>
        /// <param name="lote"></param>
        /// <returns></returns>
        private DataTable CargarTablaVarios(int lote)
        {
            try
            {
                //asignación a variables locales para manejar en el INSERT
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;
                Tabla = new DataTable();
                txSQL = "select * From Varios";
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

                string Parametro, Tipo, Valor;//variables que contendran los datos de cada fila del select anterior.

                foreach (DataRow fi in Tabla.Rows)
                {
                    Parametro = fi["Parametro"].ToString();
                    Tipo = fi["Tipo"].ToString();
                    Valor = fi["Valor"].ToString();
                    //actualiza la fecha actual de carga
                    if (Parametro == "FechaCarga")
                    {
                        Valor = DateTime.Now.ToString("dd-MM-yyyy");
                    }
                    //actualiza la Hora actual de carga
                    if (Parametro == "HoraCarga")
                    {
                        Valor = DateTime.Now.ToString("HH:mm");
                    }
                    //actualiza con el periodo de facturacion actual
                    if (Parametro == "PeriodoFacturacion")
                    {
                        Valor = Vble.Periodo.ToString();
                    }
                    //actualiza con el lote actual de carga
                    if (Parametro == "nroLote")
                    {
                        Valor = lote.ToString();

                    }
                    string insert;//Declaración de string que contendra la consulta INSERT
                    insert = "INSERT INTO Varios ([Parametro], [Tipo], [Valor]) " +
                               "VALUES ('" + Parametro + "', '" + Tipo + "', '" + Valor + "')";

                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }
                comandoSQL.Dispose();
                da.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Tabla;
        }

        /// <summary>
        /// //Método que carga la tabla Comprobantes con los ultimos datos actualizados para cada punto de venta es decir el ultimo numero de factura
        /// impresa.
        /// </summary>
        /// <param name="lote"></param>
        /// <returns></returns>
        private void CargarTablaComprobantes()
        {
            try
            {
                //asignación a variables locales para manejar en el INSERT
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;
                Tabla = new DataTable();
                txSQL = "select * From comprobantes";
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

                string Letra;//variables que contendran los datos de cada fila del select anterior.
                Int32 Numero, PuntoVenta;
                //SQLiteConnection basesqlite = new SQLiteConnection("Data Source=" + Vble.CarpetaSqlite);                
                //basesqlite.Open();

                foreach (DataRow fi in Tabla.Rows)
                {
                    PuntoVenta = Convert.ToInt32(fi["PuntoVenta"].ToString());
                    Letra = fi["Letra"].ToString();
                    Numero = Convert.ToInt32(fi["Numero"].ToString());                   
                    /////actualiza la fecha actual de carga
                    string update;//Declaración de string que contendra la consulta UPDATE               
                    update = "UPDATE Comprobantes SET Numero = " + Numero + " WHERE PuntoVenta = " + PuntoVenta + " AND Letra = '" + Letra + "'";
                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(update, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }
                comandoSQL.Dispose();
                da.Dispose();
                //basesqlite.Close();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ". Error al cargar Comprobantes");
            }

        }

        /// <summary>
        /// //Metodo que ejecuta el insert a la tabla Persona de la ruta seleccionada a la base SQLite 
        /// que se va a enviar a la colectora.
        /// </summary>
        /// <param name="codconex"></param>
        /// <returns></returns> //
         //private DataTable CargaRegistrosPersonas(Int32 personaID, int periodo, int CondIVA,
         //                                       string Apellido, string Nombre, string DocTipo, string Domicilio,
         //                                       string Barrio, string CodigoPostal, string DocNro)
        //private DataTable CargaRegistrosPersonas(int codconex)  
        private DataTable CargaRegistrosPersonas(string insert)
        {
            ////MySqlDataAdapter da;
            ////MySqlCommandBuilder comandoSQL;
            ////string txSQL;
            try
            {
                //txSQL = "select * From Personas Where PersonaID = " + codconex + " AND Periodo = " + Vble.Periodo;
                //Tabla = new DataTable();
                //da = new MySqlDataAdapter(txSQL, DB.conexBD);
                //comandoSQL = new MySqlCommandBuilder(da);
                //da.Fill(Tabla);
                //int personaID, periodo, CondIVA;
                //string Apellido, Nombre, DocTipo, Domicilio, Barrio, CodigoPostal, DocNro;

                //foreach (DataRow fi in Tabla.Rows)
                //{
                //    //asignación a variables locales para manejar en el INSERT
                //    personaID = (int)(fi["PersonaID"]);
                //    periodo = (int)(fi["Periodo"]);
                //    Apellido = fi["Apellido"].ToString();
                //    Nombre = fi["Nombre"].ToString();
                //    DocTipo = fi["DocTipo"].ToString();
                //    DocNro = fi["DocNro"].ToString();
                //    CondIVA = (int)(fi["CondIVA"]);
                //    Domicilio = fi["Domicilio"].ToString();
                //    Barrio = fi["Barrio"].ToString();
                //    CodigoPostal = fi["CodigoPostal"].ToString();

                //string insert;//Declaración de string que contendra la consulta INSERT               
                //insert = "INSERT INTO Personas ([personaID], [Periodo], [Apellido], [Nombre], [DocTipo], [DocNro], [CondIVA]," +
                //    " [Domicilio], [Barrio], [CodigoPostal]) " +
                //    "VALUES ('" + personaID + "', '" + periodo + "', '" + Apellido + "', '" + Nombre + "', '" + DocTipo + "', '" + DocNro + "', '" + CondIVA +
                //    "', '" + Domicilio + "', '" + Barrio + "', '" + CodigoPostal + "')";
                ////preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                //y la ejecutamos
                command.CommandTimeout = 300;
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //}
                //comandoSQL.Dispose();
                //da.Dispose();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + ". Error al cargar registro Personas");
            }
            return Tabla;
        }

        /// <summary>
        /// Metodo que ejecuta el insert en la tabla Historial SQLite de la ruta que se proceso para ser
        /// cargada a la colectora.
        /// 
        /// </summary>
        /// <param name="insert"></param>
        private void CargarRegistrosHistorial(string insert)
        {
           
            try
            {
               
                //preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                //y la ejecutamos
                command.CommandTimeout = 300;
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();

                //}
                //comandoSQL.Dispose();
                //da.Dispose();


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error en el metodo CargarRegistrosConexion");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                                " Error en el metodo CargarRegistrosConexion al momento en el que se estuvo " +
                                                "procesando la carga del registros de conexiones. \n");
            }
            //return Tabla;
        }


        /// <summary>
        /// Metodo que ejecuta el insert en la tabla Historial SQLite de la ruta que se proceso para ser
        /// cargada a la colectora.
        /// 
        /// </summary>
        /// <param name="insert"></param>
        private void CargarLecturistasABaseFija()
        {
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter();
                MySqlCommandBuilder comandoSQL;
                string txSQL;
                if (DB.conSQLiteFija.State == ConnectionState.Closed)
                {
                    DB.conSQLiteFija.Open();
                }
                

                #region Carga de Lecturistas

                txSQL = "select * From Lecturistas WHERE Eliminado = false ";
                Tabla = new DataTable();
                //da.SelectCommand.CommandTimeout = 300;
                da = new MySqlDataAdapter(txSQL, DB.conexBD);                
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

                //
                string DeleteLecturistas = "DELETE FROM Lecturistas";
                //preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(DeleteLecturistas, DB.conSQLiteFija);
                command.CommandTimeout = 300;
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //MessageBox.Show("Se eliminaron todos los registros");


                foreach (DataRow RegLect in Tabla.Rows)
                {
                    string InsertLecturis = "INSERT INTO Lecturistas ([Codigo], [Legajo], [Apellido], [Nombre]," +
                                            " [Clave], [FechaClave], [Privilegio]) " +
                       "VALUES (" + Convert.ToInt32(RegLect["Codigo"]) + ", '" + RegLect["Legajo"].ToString() + "', '" +
                               RegLect["Apellido"].ToString() + "', '" + RegLect["Nombre"].ToString() + "', '" +
                               RegLect["Clave"].ToString() + "', '" + RegLect["FechaClave"].ToString() + "', '" +
                               RegLect["Privilegio"].ToString() + "')";

                    //preparamos la cadena pra insercion
                    command = new SQLiteCommand(InsertLecturis, DB.conSQLiteFija);
                    //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();


                }

                da.Dispose();
                comandoSQL.Dispose();

                #endregion
                if (DB.conSQLiteFija.State == ConnectionState.Open)
                {
                    DB.conSQLiteFija.Close();
                }
               
             }
            catch (Exception r)
            {
                //MessageBox.Show(r.Message + " Error en el cargar tabla Lecturistas/Parametros/NovedadesTabla");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                                " Error en el metodo cargar tabla Lecturistas/Parametros/NovedadesTabla. Proceso - procesar carga." +
                                                "\n");
            }
            //return Tabla;
        }


        /// <summary>
        /// //Funcion que contiene la consulta INSERT de SQLite para cargar los registros seleccionados a la tabla ConceptosDatos 
        /// </summary>
        ///  //public void cargartablaConceptosDatos()
        public void cargartablaConceptosDatos(int conexionid, int periodo, int CodigoConcepto, string CodigoDpec, int CodigoEscalon, int CodigoAux, int CodigoGrupo,
                                              string TextoEscalon, string TextoUnidades, int CalcularBase, double CalcularDesde, double CalcularHasta,
                                              int AplicarBase, double AplicarDesde, double AplicarHasta, string Subtotales, double CantMinima,
                                              double CantMaxima, string ImprimeSiCero, string ImprimeSubtotal, string Prorrateo, int CuotaUno, double Cantidad,
                                              double Unitario, string VigenciaDesde, string VigenciaHasta)

        {
            string txSQL;//Declaración de string que contendra la consulta INSERT
            try
            {

                //txSQL = "INSERT INTO ConceptosDatos ([conexionID], [Periodo], [CodigoConcepto], [CodigoEscalon], [CodigoAux], [CodigoGrupo], [TextoEscalon]," +
                //        " [TextoUnidades], [CalcularBase], [CalcularDesde], [CalcularHasta], [AplicarBase], [AplicarDesde]," +
                //        " [AplicarHasta], [Subtotales], [CantMinima], [CantMaxima], [ImprimeSiCero], [ImprimeSubtotal], [Prorrateo], [CuotaUno], [Cantidad]," + " [Unitario]," +
                //        " [VigenciaDesde], [VigenciaHasta]) " +
                //        "VALUES ('" + conexionid + "', '" + periodo + "', '" + CodigoConcepto + "', '" + CodigoEscalon + "', '" + CodigoAux + "', '" + CodigoGrupo + 
                //        "', '" + TextoEscalon + "', '" + TextoUnidades + "', '" + CalcularBase + "', '" + CalcularDesde + "', '" + CalcularHasta + "', '" + AplicarBase +
                //        "', '" + AplicarDesde + "', '" + AplicarHasta + "', '" + Subtotales + "', '" + CantMinima + "', '" + CantMaxima + "', '" + ImprimeSiCero + "', '" + ImprimeSubtotal +
                //        "', '" + Prorrateo + "', '" + CuotaUno + "', '" + Cantidad.ToString(CultureInfo.CreateSpecificCulture("en-US")) + 
                //        "', '" + Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + "', '" + VigenciaDesde + "', '" + VigenciaHasta + "')";

                txSQL = "INSERT INTO ConceptosDatos ([conexionID], [Periodo], [CodigoConcepto], [CodigoDpec], [CodigoEscalon], [CodigoAux], [CodigoGrupo], [TextoEscalon]," +
                       " [TextoUnidades], [CalcularBase], [CalcularDesde], [CalcularHasta], [AplicarBase], [AplicarDesde]," +
                       " [AplicarHasta], [Subtotales], [CantMinima], [CantMaxima], [ImprimeSiCero], [ImprimeSubtotal], [Prorrateo], [CuotaUno], [Cantidad]," + " [Unitario]," +
                       " [VigenciaDesde], [VigenciaHasta]) " +
                       "VALUES ('" + conexionid + "', '" + periodo + "', '" + CodigoConcepto + "', '" + CodigoDpec + "', '" + CodigoEscalon + "', '" + CodigoAux + "', '" + CodigoGrupo +
                       "', '" + TextoEscalon + "', '" + TextoUnidades + "', '" + CalcularBase + "', '" + CalcularDesde + "', '" + CalcularHasta + "', '" + AplicarBase +
                       "', '" + AplicarDesde + "', '" + AplicarHasta + "', '" + Subtotales + "', '" + CantMinima + "', '" + CantMaxima + "', '" + ImprimeSiCero + "', '" + ImprimeSubtotal +
                       "', '" + Prorrateo + "', '" + CuotaUno + "', '" + Cantidad.ToString(CultureInfo.CreateSpecificCulture("en-US")) +
                       "', '" + Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + "', '" + VigenciaDesde + "', '" + VigenciaHasta + "')";

                //preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(txSQL, DB.con);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //asignación a variables locales para manejar en el INSERT de la tabla Conceptos Datos

            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message + ". Error al cargar registos Conceptos Datos");
            }

        }




        //metodo que contiene string de tablas sqlite para limpiar antes de realizar carga de registros
        public void EliminarContenidosTablas()
        {
            ArrayTablasSQLite.Clear();
            ArrayTablasSQLite.Add("DELETE FROM Conexiones");
            ArrayTablasSQLite.Add("DELETE FROM Medidores");
            ArrayTablasSQLite.Add("DELETE FROM Personas");
            ArrayTablasSQLite.Add("DELETE FROM Facturas");
            ArrayTablasSQLite.Add("DELETE FROM Impresor");
            ArrayTablasSQLite.Add("DELETE FROM Historial");
            //ArrayTablasSQLite.Add("DELETE FROM ConceptosDatos");
            //ArrayTablasSQLite.Add("DELETE FROM TextosVarios");
            //ArrayTablasSQLite.Add("DELETE FROM ConceptosFacturados");
            ArrayTablasSQLite.Add("DELETE FROM NovedadesConex");
            ArrayTablasSQLite.Add("DELETE FROM Altas");
            //ArrayTablasSQLite.Add("DELETE FROM Excepciones");
            ArrayTablasSQLite.Add("DELETE FROM Varios");
            foreach (var i in ArrayTablasSQLite)
            {
                //preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(i.ToString(), DB.con);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
                //MessageBox.Show("Se eliminaron todos los registros");
            }

        }

        /// <summary>
        /// Método que contiene las cargas de registros a las tablas Variables de SQLite: Conexiones, Medidores, Conceptos Datos,
        /// TextoVario, Excepciones
        /// </summary>
        /// <param name="codconex"></param>
        /// <param name="ArrayPersonas"></param> 
         public void CargaTablasVariables(int codconex, int Periodo, string FechaCalP, string Contrato, string Instalacion,
                                         int usuarioid, int titularid, int propietarioid, string DomicSumin, string BarrioSumin,
                                         string CodPostalSumin,
                                         string CuentaDebito, int impresionCod, int impresionOBS, int impresionCant,
                                         int Operario, int lote, int zona, int ruta, int secuencia, int remesa, string TarifaCod,
                                         string TarifaText, int ConsumoControl, int ConsumoResidual, int ConsumoFacturado,
                                         int OrdenTomado, string VencimientoProx)
        //public void CargaTablasVariables(int codconex)     
        {            
            StringBuilder stb1 = new StringBuilder("", 50);
            string TipoProrrateo;
            try
            {
                //Carga registros en tabla conexiones de SQLite
                if (radioButSinPro.Checked == true)
                {
                    //Lee y obtiene la descripcion del tipo de prorrateo que le corresponde si RadioButSinPro fue seleccionado                    
                    Inis.GetPrivateProfileString("Prorrateo", "SinProrrateo", "", stb1, 100, Ctte.ArchivoIniName);
                    TipoProrrateo = stb1.ToString();
                    //CargarRegistrosConexion(codconex);
                    //CargarRegistrosConexion(codconex,  Periodo,  FechaCalP,  Contrato,  Instalacion,
                    //                      usuarioid,  titularid,  propietarioid,  DomicSumin,  BarrioSumin,
                    //                      CodPostalSumin,  CuentaDebito,  impresionCod,  impresionOBS,  impresionCant,
                    //                      Operario,  lote,  zona,  ruta,  secuencia,  remesa,  TarifaCod,
                    //                      TarifaText,  ConsumoControl,  ConsumoResidual,  ConsumoFacturado,
                    //                      OrdenTomado,  VencimientoProx);
                }
                else if (radioButProrLim.Checked == true)
                {
                    //Lee y obtiene la descripcion del tipo de prorrateo que le corresponde si radioButProrLim fue seleccionado                    
                    Inis.GetPrivateProfileString("Prorrateo", "ProrrateoLimites", "", stb1, 100, Ctte.ArchivoIniName);
                    TipoProrrateo = stb1.ToString();
                    //CargarRegistrosConexion(codconex);
                    //CargarRegistrosConexion(codconex, Periodo, FechaCalP, Contrato, Instalacion,
                    //                      usuarioid, titularid, propietarioid, DomicSumin, BarrioSumin,
                    //                      CodPostalSumin, CuentaDebito, impresionCod, impresionOBS, impresionCant,
                    //                      Operario, lote, zona, ruta, secuencia, remesa, TarifaCod,
                    //                      TarifaText, ConsumoControl, ConsumoResidual, ConsumoFacturado,
                    //                      OrdenTomado, VencimientoProx);

                }
                else if (radioButProBasYfec.Checked == true)
                {
                    //Lee y obtiene la descripcion del tipo de prorrateo que le corresponde si radioButProrLim fue seleccionado                    
                    Inis.GetPrivateProfileString("Prorrateo", "ProrrateoBases", "", stb1, 100, Ctte.ArchivoIniName);
                    TipoProrrateo = stb1.ToString();
                    //CargarRegistrosConexion(codconex);
                    //CargarRegistrosConexion(codconex, Periodo, FechaCalP, Contrato, Instalacion,
                    //                      usuarioid, titularid, propietarioid, DomicSumin, BarrioSumin,
                    //                      CodPostalSumin, CuentaDebito, impresionCod, impresionOBS, impresionCant,
                    //                      Operario, lote, zona, ruta, secuencia, remesa, TarifaCod,
                    //                      TarifaText, ConsumoControl, ConsumoResidual, ConsumoFacturado,
                    //                      OrdenTomado, VencimientoProx);
                }


                //Carga registros en tabla Medidores de SQLite
                //CargarRegistrosMedidores(codconex);
                //CargarRegistrosMedidores(codconex, Periodo, Orden, Multiplicador, Digitos, AnteriorEstado,
                  //                           ActualEstado, TipoLectura, Numero, Modelo, ActualHora,
                    //                         ActualFecha, AnteriorFecha);


                //////verifica si existen registros en la tabla ConceptosDatos que pertenezca a alguna de las conexiones seleccionadas
                ////if (ExisteConceptoDatos(codconex))
                ////{
                ////    CargaRegistrosConceptosDatos(codconex);
                ////}

                //verifica si existen registros en la tabla TextosVarios que pertenezca a alguna de las conexiones seleccionadas
                if (ExisteTextoVario(codconex))
                {
                    CargaTextosVariosSqlite(codconex);
                }
                ////verifica si existen registros en la tabla Excepciones que pertenezca a alguna de las conexiones seleccionadas
                //if (ExisteExepciones(codconex))
                //{
                //    CargaExcepcionesSqlite(codconex);
                //}

            }
            catch (Exception r)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(r.Message + " Error en el metodo CargaTablaVariables");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                                " Error en el metodo CargaTablaVariables al momento en el que se estuvo " +
                                                "procesando la carga seleccionada \n");

            }
        }


        /// <summary>
        /// Metodo que contiene funciones de carga de tablas(Conexiones, Medidores, Personas, Conceptos Datos, Textos Varios,
        /// Excepciones, Varios, Comprobantes) segun secuencia seleccionada
        /// </summary>
        public void EjecutarCarga()
        {

            DataGridView TablaConexSelec = new DataGridView();
            Int32 codconex;
            Int32 UsuarioID;
            Int32 titularID;
            Int32 propietarioID;
            Int32 Periodo;
            string lineaconex = "";
            string FeLecProg = "";
            int i = 0;
            //if (this.dataGridView1.RowCount > 0)//verifica que en el datagridview1 existan registros para recorrerlos y generar la carga de las tablas
            if (Vble.TablaConexSelec.Rows.Count > 0)
            {        
               

                Vble.TablaConexSelec.Reset();
                Vble.TablaConexSelec = CargarRegistrosSecuenciaP();


                Vble.CantRegistros = (Vble.TablaConexSelec.Rows.Count);
                         

                int contador = 0;
                string insertPersonas = "INSERT INTO Personas ([personaID], [Periodo], [Apellido], [Nombre], [DocTipo], [DocNro], [CondIVA]," +
                       " [Domicilio], [Barrio], [CodigoPostal]) " +
                       "VALUES ";

                string insertConexiones = "INSERT INTO Conexiones ([conexionID], [Periodo], [FechaCalP], [OrdenLectura]," +
                       " [Contrato], [Instalacion], [usuarioID], [titularID], [propietarioID], [DomicSumin], [LocalidadSumin]," +
                       " [CodPostalSumin], [CuentaDebito], [ImpresionCOD], [ImpresionOBS], [ImpresionCANT], [Operario]," +
                       " [Lote], [Zona], [Ruta], [Secuencia], [Remesa], [TarifaCod], [TarifaTex], [ConsumoControl], [ConsumoResidual],[ConsumoFacturado], [OrdenTomado]," +
                       " [VencimientoProx]) " +
                       "VALUES ";

               
                string insertMedidores = "INSERT INTO Medidores ([conexionID], [Periodo], [Orden], [Modelo], [Numero], [Multiplicador], [Digitos]," +
                    " [AnteriorFecha], [AnteriorEstado], [ActualFecha], [ActualHora], [ActualEstado], [LecturaControl], [TipoLectura], [Latitud], [Longitud])" +
                    "VALUES ";

                string insertHistoria = "INSERT INTO Historial (ConexionID, Periodo, Periodo00," +
                        " Consumo00, Periodo01, Consumo01, Periodo02, Consumo02, Periodo03, Consumo03," +
                        " Periodo04, Consumo04, Periodo05, Consumo05, Periodo06, Consumo06, Periodo07," +
                        " Consumo07, Periodo08, Consumo08, Periodo09, Consumo09, Periodo10, Consumo10," +
                        " Periodo11, Consumo11) " +
                   "VALUES ";

                foreach (DataRow Fila in Vble.TablaConexSelec.Rows)
                //foreach (DataRow Fila in Tabla.Rows)
                {
                    contador++;

                    if (contador % 499 == 0)
                    {
                        insertConexiones += "(" + Fila["ConexionID"] + ", " + Fila["Periodo"] + ", '" + FeLecProg +
                                          "', '" + Fila["OrdenLectura"] + "', " + Fila["Contrato"] + ", " + Fila["Instalacion"] +
                                          ", " + Fila["usuarioID"] + ", " + Fila["titularID"] + ", " + Fila["propietarioID"] +
                                          ", '" + Fila["DomicSumin"].ToString().Replace("'", "") + "', '" + Fila["LocalidadSumin"] + "', '" + Fila["CodPostalSumin"] +
                                          "', '" + Fila["CuentaDebito"] + "', " + Fila["ImpresionCOD"] + ", " + Fila["ImpresionOBS"] +
                                          ", " + Fila["ImpresionCANT"] + ", " + Fila["Operario"] + ", " + Fila["Lote"] +
                                          ", " + Fila["Zona"] + ", " + Fila["Ruta"] + ", " + Fila["Secuencia"] +
                                          ", " + Fila["Remesa"] + ", '" + Fila["TarifaCod"] + "', '" + Fila["TarifaTex"] +
                                          "', " + Fila["ConsumoControl"] + ", " + Fila["ConsumoResidual"] + ", " + Fila["ConsumoFacturado"] +
                                          ", " + Fila["OrdenTomado"] + ", '" + Fila["VencimientoProx"] + "')";

                        CargarRegistrosConexion(insertConexiones);


                        insertMedidores += "('" + Fila["ConexionID"] + "', '" + Fila["Periodo"] + "', '" + Fila["Orden"] +
                                         "', '" + Fila["Modelo"] + "', " + Fila["Numero"] + ", '" + Fila["Multiplicador"] +
                                         "', '" + Fila["Digitos"] + "', '" + Convert.ToDateTime(Fila["AnteriorFecha"]).ToString("dd/MM/yyyy") +
                                         "', '" + Fila["AnteriorEstado"] + "', '" + Fila["ActualFecha"].ToString() + "', '" + Fila["ActualHora"] +
                                         "', '" + Fila["ActualEstado"] + "', '" + Fila["LecturaControl"] + "', '" + Fila["TipoLectura"] + "', " + Fila["Latitud"].ToString().Replace(",", ".") + ", " + Fila["Longitud"].ToString().Replace(",", ".") + ")";

                        CargarRegistrosConexion(insertMedidores);


                        insertPersonas += "('" + Fila["PersonaID"] + "', '" + Fila["Periodo"] + "', '" + Fila["Apellido"].ToString().Replace("'", "") +
                                          "', '" + Fila["Nombre"].ToString().Replace("'", "") + "', '" + Fila["DocTipo"] + "', '" + Fila["DocNro"] +
                                          "', '" + Fila["CondIVA"] + "', '" + Fila["Domicilio"].ToString().Replace("'", "") + "', '" + Fila["Barrio"].ToString().Replace("'", "") +
                                          "', '" + Fila["CodigoPostal"] + "')";

                        CargarRegistrosConexion(insertPersonas);

                        insertHistoria += "(" + Fila["ConexionID"] + ", " + Fila["Periodo"] + ", '" + Fila["HistoPeriodo00"] + "', '" + Fila["HistoConsumo00"] + "', '" + Fila["HistoPeriodo01"] + "', '" + Fila["HistoConsumo01"] + "', '" + Fila["HistoPeriodo02"] + "', '" + Fila["HistoConsumo02"] +
                        "', '" + Fila["HistoPeriodo03"] + "', '" + Fila["HistoConsumo03"] + "', '" + Fila["HistoPeriodo04"] + "', '" + Fila["HistoConsumo04"] + "', '" + Fila["HistoPeriodo05"] + "', '" + Fila["HistoConsumo05"] +
                        "', '" + Fila["HistoPeriodo06"] + "', '" + Fila["HistoConsumo06"] + "', '" + Fila["HistoPeriodo07"] + "', '" + Fila["HistoConsumo07"] + "', '" + Fila["HistoPeriodo08"] + "', '" + Fila["HistoConsumo08"] +
                        "', '" + Fila["HistoPeriodo09"] + "', '" + Fila["HistoConsumo09"] + "', '" + Fila["HistoPeriodo10"] + "', '" + Fila["HistoConsumo10"] + "', '" + Fila["HistoPeriodo11"] + "', '" + Fila["HistoConsumo11"] + "')";

                        CargarRegistrosConexion(insertHistoria);

                        insertConexiones = "";
                        insertHistoria = "";
                        insertMedidores = "";
                        insertPersonas = "";
                        //contador = 0;
                        //contador++;
                        insertPersonas = "INSERT INTO Personas ([personaID], [Periodo], [Apellido], [Nombre], [DocTipo], [DocNro], [CondIVA]," +
                       " [Domicilio], [Barrio], [CodigoPostal]) " +
                       "VALUES ";

                        insertConexiones = "INSERT INTO Conexiones ([conexionID], [Periodo], [FechaCalP], [OrdenLectura]," +
                               " [Contrato], [Instalacion], [usuarioID], [titularID], [propietarioID], [DomicSumin], [LocalidadSumin]," +
                               " [CodPostalSumin], [CuentaDebito], [ImpresionCOD], [ImpresionOBS], [ImpresionCANT], [Operario]," +
                               " [Lote], [Zona], [Ruta], [Secuencia], [Remesa], [TarifaCod], [TarifaTex], [ConsumoControl], [ConsumoResidual],[ConsumoFacturado], [OrdenTomado]," +
                               " [VencimientoProx]) " +
                               "VALUES ";


                        insertMedidores = "INSERT INTO Medidores ([conexionID], [Periodo], [Orden], [Modelo], [Numero], [Multiplicador], [Digitos]," +
                            " [AnteriorFecha], [AnteriorEstado], [ActualFecha], [ActualHora], [ActualEstado], [LecturaControl], [TipoLectura], [Latitud], [Longitud]) " +
                            "VALUES ";

                        insertHistoria = "INSERT INTO Historial (ConexionID, Periodo, Periodo00," +
                                " Consumo00, Periodo01, Consumo01, Periodo02, Consumo02, Periodo03, Consumo03," +
                                " Periodo04, Consumo04, Periodo05, Consumo05, Periodo06, Consumo06, Periodo07," +
                                " Consumo07, Periodo08, Consumo08, Periodo09, Consumo09, Periodo10, Consumo10," +
                                " Periodo11, Consumo11) " +
                           "VALUES ";

                    }
                    else if (contador == Vble.TablaConexSelec.Rows.Count)
                    {

                        insertPersonas += "('" + Fila["PersonaID"] + "', '" + Fila["Periodo"] + "', '" + Fila["Apellido"].ToString().Replace("'", "") +
                              "', '" + Fila["Nombre"].ToString().Replace("'", "") + "', '" + Fila["DocTipo"] + "', '" + Fila["DocNro"] + "', '" + Fila["CondIVA"] +
                       "', '" + Fila["Domicilio"].ToString().Replace("'", "") + "', '" + Fila["Barrio"].ToString().Replace("'", "") + "', '" + Fila["CodigoPostal"] + "')";


                        FeLecProg = Fila["FechaCalP"].ToString().Substring(0, 4) + "-" + Fila["FechaCalP"].ToString().Substring(4, 2) + "-" + Fila["FechaCalP"].ToString().Substring(6, 2);
                        FeLecProg = FeLecProg == "" ? "0000-00-00" : FeLecProg;
                       insertConexiones += "(" + Fila["ConexionID"] + ", " + Fila["Periodo"] + ", '" + FeLecProg +
                                          "', '" + Fila["OrdenLectura"] + "', " + Fila["Contrato"] + ", " + Fila["Instalacion"] + 
                                          ", " + Fila["usuarioID"] + ", " + Fila["titularID"] + ", " + Fila["propietarioID"] + 
                                          ", '" + Fila["DomicSumin"].ToString().Replace("'", "") + "', '" + Fila["LocalidadSumin"] + "', '" + Fila["CodPostalSumin"] +
                                          "', '" + Fila["CuentaDebito"] + "', " + Fila["ImpresionCOD"] + ", " + Fila["ImpresionOBS"] + 
                                          ", " + Fila["ImpresionCANT"] +", " + Fila["Operario"] + ", " + Fila["Lote"] + 
                                          ", " + Fila["Zona"] + ", " + Fila["Ruta"] + ", " + Fila["Secuencia"] + 
                                          ", " + Fila["Remesa"] + ", '" + Fila["TarifaCod"] + "', '" + Fila["TarifaTex"] + 
                                          "', " + Fila["ConsumoControl"] + ", " + Fila["ConsumoResidual"] + ", " + Fila["ConsumoFacturado"] + 
                                          ", " + Fila["OrdenTomado"] + ", '" + Fila["VencimientoProx"] + "')";


                        insertMedidores += "('" + Fila["ConexionID"] + "', '" + Fila["Periodo"] + "', '" + Fila["Orden"] +
                                          "', '" + Fila["Modelo"] + "', " + Fila["Numero"] + ", '" + Fila["Multiplicador"] +
                                          "', '" + Fila["Digitos"] + "', '" + Convert.ToDateTime(Fila["AnteriorFecha"]).ToString("dd/MM/yyyy") +
                                          "', '" + Fila["AnteriorEstado"] + "', '" + Fila["ActualFecha"].ToString() + "', '" + Fila["ActualHora"] +
                                          "', '" + Fila["ActualEstado"] + "', '" + Fila["LecturaControl"] + "', '" + Fila["TipoLectura"] + "', " + Fila["Latitud"].ToString().Replace(",", ".") + ", " + Fila["Longitud"].ToString().Replace(",", ".") + ")";


                        insertHistoria += "(" + Fila["ConexionID"] + ", " + Fila["Periodo"] + ", '" + Fila["HistoPeriodo00"] + "', '" + Fila["HistoConsumo00"] + "', '" + Fila["HistoPeriodo01"] + "', '" + Fila["HistoConsumo01"] + "', '" + Fila["HistoPeriodo02"] + "', '" + Fila["HistoConsumo02"] +
                        "', '" + Fila["HistoPeriodo03"] + "', '" + Fila["HistoConsumo03"] + "', '" + Fila["HistoPeriodo04"] + "', '" + Fila["HistoConsumo04"] + "', '" + Fila["HistoPeriodo05"] + "', '" + Fila["HistoConsumo05"] +
                        "', '" + Fila["HistoPeriodo06"] + "', '" + Fila["HistoConsumo06"] + "', '" + Fila["HistoPeriodo07"] + "', '" + Fila["HistoConsumo07"] + "', '" + Fila["HistoPeriodo08"] + "', '" + Fila["HistoConsumo08"] +
                        "', '" + Fila["HistoPeriodo09"] + "', '" + Fila["HistoConsumo09"] + "', '" + Fila["HistoPeriodo10"] + "', '" + Fila["HistoConsumo10"] + "', '" + Fila["HistoPeriodo11"] + "', '" + Fila["HistoConsumo11"] + "')";

                    }
                    else if (contador < Vble.TablaConexSelec.Rows.Count)
                    {
                        FeLecProg = Fila["FechaCalP"].ToString().Substring(0, 4) + "-" + Fila["FechaCalP"].ToString().Substring(4, 2) + "-" + Fila["FechaCalP"].ToString().Substring(6, 2);
                        FeLecProg = FeLecProg == "" ? "0000-00-00" : FeLecProg;

                        insertPersonas += "('" + Fila["PersonaID"] + "', '" + Fila["Periodo"] + "', '" + Fila["Apellido"].ToString().Replace("'", "") +
                                          "', '" + Fila["Nombre"].ToString().Replace("'", "") + "', '" + Fila["DocTipo"] + "', '" + Fila["DocNro"] + 
                                          "', '" + Fila["CondIVA"] + "', '" + Fila["Domicilio"].ToString().Replace("'", "") + "', '" + Fila["Barrio"].ToString().Replace("'", "") + 
                                          "', '" + Fila["CodigoPostal"] + "'), ";
                        lineaconex = "(" + Fila["ConexionID"] + ", " + Fila["Periodo"] + ", '" + FeLecProg +
                                          "', '" + Fila["OrdenLectura"] + "', " + Fila["Contrato"] + ", " + Fila["Instalacion"] +
                                          ", " + Fila["usuarioID"] + ", " + Fila["titularID"] + ", " + Fila["propietarioID"] +
                                          ", '" + Fila["DomicSumin"] + "', '" + Fila["LocalidadSumin"] + "', '" + Fila["CodPostalSumin"] +
                                          "', '" + Fila["CuentaDebito"] + "', " + Fila["ImpresionCOD"] + ", " + Fila["ImpresionOBS"] +
                                          ", " + Fila["ImpresionCANT"] + ", " + Fila["Operario"] + ", " + Fila["Lote"] +
                                          ", " + Fila["Zona"] + ", " + Fila["Ruta"] + ", " + Fila["Secuencia"] +
                                          ", " + Fila["Remesa"] + ", '" + Fila["TarifaCod"] + "', '" + Fila["TarifaTex"] +
                                          "', " + Fila["ConsumoControl"] + ", " + Fila["ConsumoResidual"] + ", " + Fila["ConsumoFacturado"] +
                                          ", " + Fila["OrdenTomado"] + ", '" + Fila["VencimientoProx"] + "'),";
                        insertConexiones += lineaconex;

                        insertMedidores += "('" + Fila["ConexionID"] + "', '" + Fila["Periodo"] + "', '" + Fila["Orden"] +
                                         "', '" + Fila["Modelo"] + "', " + Fila["Numero"] + ", '" + Fila["Multiplicador"] +
                                         "', '" + Fila["Digitos"] + "', '" + Convert.ToDateTime(Fila["AnteriorFecha"]).ToString("dd/MM/yyyy") +
                                         "', '" + Fila["AnteriorEstado"] + "', '" + Fila["ActualFecha"].ToString() + "', '" + Fila["ActualHora"] +
                                         "', '" + Fila["ActualEstado"] + "', '" + Fila["LecturaControl"] + "', '" + Fila["TipoLectura"] + "', " + Fila["Latitud"].ToString().Replace(",", ".") + ", " + Fila["Longitud"].ToString().Replace(",", ".") + "), ";


                        insertHistoria += "(" + Fila["ConexionID"] + ", " + Fila["Periodo"] + ", '" + Fila["HistoPeriodo00"] + "', '" + Fila["HistoConsumo00"] + "', '" + Fila["HistoPeriodo01"] + "', '" + Fila["HistoConsumo01"] + "', '" + Fila["HistoPeriodo02"] + "', '" + Fila["HistoConsumo02"] +
                        "', '" + Fila["HistoPeriodo03"] + "', '" + Fila["HistoConsumo03"] + "', '" + Fila["HistoPeriodo04"] + "', '" + Fila["HistoConsumo04"] + "', '" + Fila["HistoPeriodo05"] + "', '" + Fila["HistoConsumo05"] +
                        "', '" + Fila["HistoPeriodo06"] + "', '" + Fila["HistoConsumo06"] + "', '" + Fila["HistoPeriodo07"] + "', '" + Fila["HistoConsumo07"] + "', '" + Fila["HistoPeriodo08"] + "', '" + Fila["HistoConsumo08"] +
                        "', '" + Fila["HistoPeriodo09"] + "', '" + Fila["HistoConsumo09"] + "', '" + Fila["HistoPeriodo10"] + "', '" + Fila["HistoConsumo10"] + "', '" + Fila["HistoPeriodo11"] + "', '" + Fila["HistoConsumo11"] + "'),";
                    }

                    //----------------------Cambia el estado impresionOBS a Listo para Cargar en Colectora
                    Vble.CambiarEstadoConexionMySql((Int32)Fila["ConexionID"], Convert.ToInt32(cteCodEstado.ParaCargar), (Int32)Fila["Periodo"]);
                    backgroundProcesarCarga.ReportProgress(i);
                    i++;
                }

                ///Se realiza los insert luego del armado de la cadena con los registros a agregar en las siguientes tablas
                ///Conexiones
                ///Medidores
                ///Personas
                ///Historial
                ///Lecturistas: esta tabla inserta todos los lecturistas registrados en la base general que son de toda la provincia.                                
                    CargarRegistrosConexion(insertConexiones);
                    CargarRegistrosMedidores(insertMedidores);
                    CargaRegistrosPersonas(insertPersonas);
                    CargarRegistrosHistorial(insertHistoria);
                    CargarLecturistasABaseFija();
                
            }
            else
            {
                MessageBox.Show("Debe Seleccionar alguna secuencia del panel Izquierdo para ver los registros", "Atención", MessageBoxButtons.OK);
            }
        }


        /// <summary>
        /// Elimina los items del panel Cargas a enviar para que al finalizar el proceso de la nueva carga, se agregue nuevamente
        /// las que estaban mas la nueva que se proceso
        /// </summary>
        public void LimpiarPanelCargasAenviar()
        {
            try
            {            
                    if (listViewCargasProcesadas.Items.Count > 0)
                    {
                        for (int i = 0; i < listViewCargasProcesadas.Items.Count;)
                        {
                            listViewCargasProcesadas.Items[i].Remove();
                        }
                    }

                    if (ListViewCargados.Items.Count > 0)
                    {
                        for (int i = 0; i < ListViewCargados.Items.Count;)
                        {
                            ListViewCargados.Items[i].Remove();
                        }
                    }

                    if (LisViewDescargados.Items.Count > 0)
                    {
                        for (int i = 0; i < LisViewDescargados.Items.Count;)
                        {
                            LisViewDescargados.Items[i].Remove();
                        }
                    }
            }
            catch (Exception)
            {
                
            }

        }

        /// <summary>
        /// Metodo que Muestra en el ListView1 "Archivos para Cargar" 
        /// todas las cargas procesadas de las distintas localidades
        /// </summary>
        public void CargasProcesadas()
        {
            try
            {

                int indiceCarpetas;
                ArrayCarpetasCargas.Clear();
                listViewCargasProcesadas.ShowItemToolTips = true;
                string LocRuta = "";

                DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
                CultureInfo.CurrentCulture);
                //armo la carpeta donde estan los archivos tanto sin enviar como enviadas, a partir de ahi escalo las subcarpetas que me interesan
                string ArchivosSinEnviar = string.Format(Vble.CarpetasCargasAcargar, Per);

                DirectoryInfo di = new DirectoryInfo(ArchivosSinEnviar);
                if (Directory.Exists(di.ToString()))
                {
                    foreach (var dis in di.GetDirectories())
                    {
                        foreach (var sub in dis.GetDirectories())
                        {
                            if (sub.Name.Contains("EP2"))
                            {
                                indiceCarpetas = sub.Name.IndexOf("EP2");
                                ListViewItem Datos = new ListViewItem(sub.Name.Substring(indiceCarpetas));
                                ListViewItem DatosAMostrar = new ListViewItem();
                                foreach (var files in sub.GetFiles())
                                {
                                    if (files.Name.Contains("InfoCarga.txt"))
                                    {
                                        //Datos.SubItems.Add(files.Name);
                                        //Agrega el tooltip de información de las cargas generadas.
                                        using (StreamReader sr = new StreamReader(files.DirectoryName + "\\InfoCarga.txt", false))
                                        {
                                            string line, linTool;
                                            int longruta;
                                            linTool = "";

                                            while ((line = sr.ReadLine()) != null)
                                            {
                                                linTool += line + "\n";
                                                longruta = line.IndexOf('(') - line.IndexOf('-') - 1;
                                                LocRuta += line.Substring(line.IndexOf('-') + 1, longruta);
                                            }
                                            //Datos.ToolTipText = linTool ;
                                            DatosAMostrar.ToolTipText = linTool;
                                        }
                                    }
                                }

                                DatosAMostrar.Text = (sub.Name.Substring(indiceCarpetas) + "        " + LocRuta);
                                listViewCargasProcesadas.Items.Add(DatosAMostrar);
                                ArrayCarpetasCargas.Add(sub.FullName);
                            }
                            LocRuta = "";
                        }
                    }
                }
            }
            catch (Exception)
            {

                
            }
        }

        /// <summary>
        /// Metodo que Muestra en el ListView1 "Archivos para Cargar" 
        /// todas las cargas procesadas de las distintas localidades
        /// </summary>
        public void AgregarColectoraAInfoCarga()
        {
            //ArrayCarpetasCargas.Clear();
            ListViewCargados.ShowItemToolTips = true;

            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
            CultureInfo.CurrentCulture);
            //armo la carpeta donde estan los archivos tanto Sin Enviar como Enviadas, a partir de ahi escalo las subcarpetas que me interesan
            string ArchivosSinEnviar = string.Format(Vble.CarpetasCargasAcargar, Per);

            DirectoryInfo di = new DirectoryInfo(ArchivosSinEnviar);
            //Verifica si Existe la carpeta "Envios_Carga",
            //si existe comienza a escalar las subcarpetas indicadas en los if
            if (Directory.Exists(di.ToString()))
            {
                foreach (var dis in di.GetDirectories())
                {
                    //recorre cada carpeta distrito que haya sido generado y busca la carpeta "Enviadas"
                    foreach (var sub in dis.GetDirectories())
                    {
                        if (sub.Name.Contains("Enviadas"))
                        {
                            foreach (var fi in sub.GetDirectories())
                            {
                                if (fi.Name == (Vble.CarpetaSeleccionada.TrimEnd()))
                                {
                                    ListViewItem Datos = new ListViewItem(fi.Name);
                                    foreach (var files in fi.GetFiles())
                                    {
                                        if (files.Name.Contains("InfoCarga.txt"))
                                        {
                                            using (StreamReader sr = new StreamReader(files.DirectoryName + "\\InfoCarga.txt", false))
                                            {
                                                string line /*linTool*/;
                                                //linTool = "";
                                                Vble.lineas = "";
                                                DateTime thisDay = DateTime.Today;
                                                Vble.lineas = "Fecha Envio: " + thisDay.ToString("d") + "  Colectora: " + cmbDevices.Text.Replace("-", " ") + "  |  ";
                                                while ((line = sr.ReadLine()) != null)
                                                {
                                                    Vble.lineas += line + "\n";
                                                }
                                                Datos.ToolTipText = Vble.lineas;
                                            }
                                            //almaceno en variables temporales para crear los nuevos archivos con informacion de colectora
                                            string infocargarespaldo = Vble.CarpetaRespaldo + Vble.RespaldoEnviadas.TrimEnd(' ') + "\\" + files.Name;
                                            //string infocargarespaldo = Vble.CarpetaRespaldo + "\\" + files.Name;
                                            string ruta = files.FullName;
                                            string archivo = files.Name;
                                            //elimino archivos para luevo volver a crearlos con info de colectora
                                            //File.Delete(files.FullName);
                                            //File.Delete(infocargarespaldo);
                                            //creo nuevos archivos InfoCarga.txt con informacion de colectora a la que se envio
                                            Vble.CreateInfoCarga(ruta.TrimEnd(' '), archivo, Vble.lineas);
                                            Vble.CreateInfoCarga(infocargarespaldo, archivo, Vble.lineas);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Metodo que Busca y escribe en listviewCargadas las conexiones que se enviaron de la PC a la colectora
        /// a modo de información para el operador.
        /// </summary>
        public void LeeCargasEnviadas()
        {
            try
            {
                //ArrayCarpetasCargas.Clear();
                ListViewCargados.ShowItemToolTips = true;
                Int32 IndiceDictionary = 0;

                DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
                CultureInfo.CurrentCulture);
                //armo la carpeta donde estan los archivos tanto sin enviar como enviadas, a partir de ahi escalo las subcarpetas que me interesan
                string ArchivosSinEnviar = string.Format(Vble.CarpetasCargasAcargar, Per);

                DirectoryInfo di = new DirectoryInfo(ArchivosSinEnviar);

                if (Directory.Exists(di.ToString()))
                {
                    foreach (var dis in di.GetDirectories())
                    {
                        foreach (var sub in dis.GetDirectories())
                        {
                            if (sub.Name.Contains("Enviadas"))
                            {
                                foreach (var fi in sub.GetDirectories())
                                {
                                    if (fi.Name.Contains("EP2"))
                                    {
                                        ListViewItem Datos = new ListViewItem(fi.Name);
                                        foreach (var files in fi.GetFiles())
                                        {

                                            if (files.Name.Contains("InfoCarga.txt"))
                                            {
                                                using (StreamReader sr = new StreamReader(files.DirectoryName + "\\InfoCarga.txt", false))
                                                {

                                                    string line, linTool;
                                                    linTool = "";
                                                    while ((line = sr.ReadLine()) != null)
                                                    {
                                                        linTool += line + "\n";
                                                    }
                                                    Datos.ToolTipText = linTool;


                                                }
                                            }
                                        }
                                        ListViewCargados.Items.Add(Datos);
                                        ListViewCargados.ForeColor = System.Drawing.Color.Gray;

                                    }
                                }
                            }
                        }
                    }
                    IndiceDictionary = 0;
                }
            }
            catch (Exception)
            {
             
            }
        }

        public void LeerCargasRecibidas()
        {
            try
            {            
                //ArrayCarpetasCargas.Clear();            
                LisViewDescargados.ShowItemToolTips = true;
                Int32 IndiceDictionary = 0;
                DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
                CultureInfo.CurrentCulture);
                //armo la carpeta donde estan los archivos tanto sin enviar como enviadas, a partir de ahi escalo las subcarpetas que me interesan
                string ArchivosRecibidos = string.Format(Vble.CarpetasCargasRecibidas, Per);

                DirectoryInfo di = new DirectoryInfo(ArchivosRecibidos);

                if (Directory.Exists(di.ToString()))
                {
                    foreach (var dis in di.GetDirectories())
                    {
                        foreach (var sub in dis.GetDirectories())
                        {
                            if (sub.Name.Contains("Recibidas"))
                            {
                                foreach (var fi in sub.GetDirectories())
                                {
                                    if (fi.Name.Contains("EP2"))
                                    {
                                        ListViewItem Datos = new ListViewItem(fi.Name);
                                        foreach (var files in fi.GetFiles())
                                        {

                                            if (files.Name.Contains("InfoDescarga.txt"))
                                            {
                                                using (StreamReader sr = new StreamReader(files.DirectoryName + "\\InfoCarga.txt", false))
                                                {

                                                    string line, linTool;
                                                    linTool = "";
                                                    while ((line = sr.ReadLine()) != null)
                                                    {
                                                        linTool += line + "\n";
                                                    }
                                                    Datos.ToolTipText = linTool;


                                                }
                                            }
                                        }
                                        LisViewDescargados.Items.Add(Datos);
                                        LisViewDescargados.ForeColor = System.Drawing.Color.Gray;

                                    }
                                }
                            }
                        }
                    }
                    IndiceDictionary = 0;
                }
            }
            catch (Exception)
            {
                
            }
        }
    

        /// <summary>
        /// Metodo que Busca y escribe en listviewCargadas las conexiones que se enviaron de la PC a la colectora
        /// a modo de información para el operador.
        /// </summary>
        public void LeeCarpetaColectora(string carpetacolectora)
        {
            //ArrayCarpetasCargas.Clear();
            ListViewColectora.ShowItemToolTips = true;
            StringBuilder stb1 = new StringBuilder("", 250);
            Inis.GetPrivateProfileString("Archivos", "NombreArchivoInfo", "", stb1, 250, Ctte.ArchivoIniName);
            string nombrearchivo = stb1.ToString();
            

            DirectoryInfo di = new DirectoryInfo(carpetacolectora);

            if (Directory.Exists(di.ToString()))
            {                
                foreach (var files in di.GetFiles())
                  {
                    if (files.Name.Contains(nombrearchivo))
                      {
                        ListViewItem Datos = new ListViewItem(files.Name);
                       
                        using (StreamReader sr = new StreamReader(files.DirectoryName + "\\" + nombrearchivo, false))
                          {
                            string line;
                           
                             while ((line = sr.ReadLine()) != null)
                              {
                                 //linTool += line + "\n";
                                ListViewColectora.Items.Add(line);
                              }
                             
                        }
                       }
                  }
                    
            }
            else
            {
                MessageBox.Show("no existe la carpeta");
            }
        }


        /// <summary>
        /// Metodo que detiene la tarea en segundo plano (backgroundworker) que se esta ejecuando,
        /// recibe como parametro dicha proceso.
        /// </summary>
        /// <param name="tareasegundoplano"></param>
        private void DetenerTareaSegundoPlano(BackgroundWorker tareasegundoplano)
        {
            // Cancel the asynchronous operation.
            tareasegundoplano.CancelAsync();
            tareasegundoplano.Dispose();
            Int32 codconex;
            //Habilito nuevamente los botones que podian haber ocacionado interrupcion al proceso de descarga                     
            this.btnCerrar.Enabled = true;
            this.ControlBox = true;
            this.cmbDevices.Enabled = true;
            this.BotEnviarCarga.Enabled = true;
            this.BotDevCarga.Visible = true;
            this.BotProcesCarg.Enabled = true;
            this.BotActPanPC.Enabled = true;
            this.btnCerrar.Enabled = true;


          




            this.BotDetenerProcCarga.Visible = false;

        }



        /// <summary>
        /// //Boton que se encarga de la carga de los registros a las tablas vacias con formato SQLite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            RestNod.Visible = false;
            LabRestDevArc.Visible = false;
            LabRestEnvArc.Visible = false;
            try
            {
               
                    //DB.con.Close()
               
                DB.con.Open();
                
                //verifica que se haya seleccionado alguna secuencia que contenga conexiones a cargar
                //if (dataGridView1.RowCount > 0)
                if (Vble.TablaConexSelec.Rows.Count > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    LimpiarPanelCargasAenviar();
                    listViewCargasProcesadas.Refresh();                          
                    ArrayPersonas.Clear();//limpia el array que contendra los usuarioID, titularID, propietarioID
                    EliminarContenidosTablas();//limpia las tablas variables sqlite antes de cargar los registros seleccionados                            

                    //El proceso de generar las conexiones seleccionadas se realiza en segundo plano por el tiempo excesivo 
                    //que toma el proceso de carga para no tildar la aplicación. Sigue en el metodo "backgroundWorker1_DoWork"
                    BotProcesCarg.Enabled = false;
                    BotActPanPC.Enabled = false;
                    btnCerrar.Enabled = false;
                    BotEnviarCarga.Enabled = false;
                    BotDetenerProcCarga.Enabled = true;
                    BotDetenerProcCarga.Visible = true;
                    //-------------- Muestra el label de procesando Carga Procesando....
                    Procesando = 1;
                    Enviando = 0;
                    timer3.Interval = 100;
                    timer3.Enabled = true;
                    labelProcesando.Text = "Procesando";
                    labelProcesando.Visible = true;
                    //--------------
                    backgroundProcesarCarga.RunWorkerAsync();             
                }
                else
                {
                    RestNod.Text = "* No existen archivos con la seleccion o no se selecciono una ruta correcta";
                    DB.con.Close();
                  
                    RestNod.Visible = true;                  
                }
              }
                catch (Exception R)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(R.Message + " Error al seleccionar Ruta para generar la carga.");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + R.Message +
                                              " Error al Presionar Boton de procesar Carga \n");
            }
        }       
     

        /// <summary>
        ///Metodo que contiene foreach anidados el cual recorre los nodos seleccionados para cargar los arraylist con
        ///los valores de 
        ///ArrayDesde;
        ///ArrayHasta;
        ///ArrayRuta;
        ///ArrayLocalidad;      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RecorrerNodos(object sender, EventArgs e)
        {

            bool existe = false;

            try
            {                        
            string clave = "";
            //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
            foreach (TreeNode tNd1 in tvwCargas.Nodes[0].Nodes)
            {
                if (tNd1.ImageKey != "nada")
                {
                    clave = tNd1.Tag.ToString();
                    Vble.Distrito = dcNodos[clave].Distrito;                   
                    foreach (TreeNode tNd2 in tNd1.Nodes)
                    {
                        if (tNd2.ImageKey != "nada")
                        {
                            clave = tNd2.Tag.ToString();
                            Vble.Remesa = dcNodos[clave].Remesa;                        
                            foreach (TreeNode tNd3 in tNd2.Nodes)
                            {
                                if (tNd3.ImageKey != "nada" /*&& tNd3.Level == 3*/)
                                {
                                    clave = tNd3.Tag.ToString();
                                    Vble.Ruta = dcNodos[clave].Ruta;
                                       
                                }
                                    foreach (TreeNode tNd4 in tNd3.Nodes)
                                    {
                                        if (tNd4.ImageKey != "nada" && tNd4.Level == 4)
                                        {
                                            clave = tNd4.Tag.ToString();
                                            clInfoNodos tn = new clInfoNodos();
                                            tn = dcNodos[tNd4.Tag.ToString()];                                                              
                                            Vble.desde = dcNodos[clave].Desde;
                                            Vble.hasta = dcNodos[clave].Hasta;                                        
                                            Vble.CantConex = dcNodos[clave].CnxSelected;
                                            ArrayCantConex.Add(Vble.CantConex.ToString());
                                            ArrayDesde.Add(Vble.desde.ToString());
                                            ArrayHasta.Add(Vble.hasta.ToString());
                                            ArrayRuta.Add(Vble.Ruta.ToString());
                                            ArrayLocalidad.Add(Vble.Distrito.ToString());
                                            //foreach (var item in NodosSeleccionados)
                                            //{
                                            //    if (item.ToString() == tNd3.Name)
                                            //    {                                                   
                                            //        existe = true;                                                    
                                            //    }                                                
                                            //}

                                            //if (existe == false)
                                            //{
                                            //    NodosSeleccionados.Add(tNd3.Name);                                                
                                            //}                                            
                                        }
                                    else if (tNd4.ImageKey == "nada" && tNd4.Level == 4)
                                    {
                                        //    tNd4.ImageKey = "nada";
                                        //dataGridView1.DataSource = "";
                                        Vble.TablaConexSelec.Clear();
                                        dataGridView1.DataSource = "";
                                        labelCantReg.Text = "0";
                                        //ArrayRuta.Clear();    
                                            //foreach (var item in NodosSeleccionados)
                                            //{
                                            //   if (item.ToString() == tNd3.Name)
                                            //   {
                                            //       NodosSeleccionados.Remove(tNd3.Name);
                                            //   }
                                            //}                                            

                                        }
                                        //else
                                        //{                                          
                                        //  foreach (var item in NodosSeleccionados)
                                        //  {
                                        //      if (item.ToString() == tNd3.Name)
                                        //      {
                                        //          NodosSeleccionados.Remove(tNd3.Name);
                                        //      }
                                        //  }
                                        //}                                      
                                    }
                                    //existe = false;                                    
                                }                          
                        }
                    }
                 
                }

            }
            }
            catch (Exception r)
            {
                if (r.Message.Contains("Colección modificada;"))
                {

                }
                else
                {
                    MessageBox.Show(r.Message + " Error al recorrer Nodos");
                }
                
              
            }
        }

        private void RecorrerNodosParticion(TreeNode tNd3 )
        {
            string clave = "";
            if (dcNodos[clave].Particion == "A")
            {

            }
            clave = tNd3.Tag.ToString();
            clInfoNodos tn = new clInfoNodos();
            tn = dcNodos[tNd3.Tag.ToString()];
            Vble.desde = dcNodos[clave].Desde;
            Vble.hasta = dcNodos[clave].Hasta;
            Vble.CantConex = dcNodos[clave].CnxSelected;
            ArrayCantConex.Add(Vble.CantConex.ToString());
            ArrayDesde.Add(Vble.desde.ToString());
            ArrayHasta.Add(Vble.hasta.ToString());
            ArrayRuta.Add(Vble.Ruta.ToString());
            ArrayLocalidad.Add(Vble.Distrito.ToString());
        }

        /// <summary>
        /// //Boton que contiene el recorrido de los nodos del treview para cargar los 
        /// //arraylist que se van a utilizar para la consulta MySQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ArrayDesde.Clear();
            ArrayHasta.Clear();
            ArrayRuta.Clear();
            //ArrayRuta.RemoveRange(0, ArrayRuta.Count);
            ArrayLocalidad.Clear();
            ArrayCantConex.Clear();
            RecorrerNodos(sender, e);
         
        }        

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        /// <summary>
        /// Metodo que obtendra la ruta Origen para la copia de archivos (Ubicación de la base sqlite) que se pasaran a la subcarpeta 
        /// con el formato "EPyyyyMM_D000_C00000.aaMMdd_HHmm" generado anteriormente 
        /// </summary>
        public string ObtenerOrigen() {
            string retorno = "";
            try
            {   //Leer Ruta Origen              
                //    StringBuilder stb1 = new StringBuilder("", 250);                
                //    Inis.GetPrivateProfileString("Datos", "Base", "", stb1, 250, Ctte.ArchivoIniName);
                //    //Inis.LeerCadenaPerfilPrivado("Datos", "RutaOrigen", "NO", out stb1, 97, Ctte.ArchivoIniName);
                //    string retorno = stb1.ToString();
                retorno = Ctte.CarpetaRecursos + "\\" + Vble.NombreArchivoBaseSqlite();
               
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al obtener la base variable al generar la carga");

            }
            return retorno;
        }

        /// <summary>
        /// Metodo que obtendra la ruta Origen para la copia de archivos (Ubicación de la base sqlite) que se pasaran a la subcarpeta 
        /// con el formato "EPyyyyMM_D000_C00000.aaMMdd_HHmm" generado anteriormente 
        /// </summary>
        public string ObtenerBaseFija()
        {
            string retorno = "";
            try
            {   //Leer Ruta Origen              
                //StringBuilder stb1 = new StringBuilder("", 250);
                //Inis.GetPrivateProfileString("Datos", "BaseFija", "", stb1, 250, Ctte.ArchivoIniName);
                ////Inis.LeerCadenaPerfilPrivado("Datos", "RutaOrigen", "NO", out stb1, 97, Ctte.ArchivoIniName);
                //string retorno = stb1.ToString();
                retorno = Ctte.CarpetaRecursos + "\\" + Vble.NombreArchivoBaseFijaSqlite();
               
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al obtener la base variable al generar la carga");                

            }
            return retorno;
        }




        /// <summary>
        /// Metodo que obtendra el nombre del archivo SQLite que contiene los datos procesados para copiar a la carpeta 
        /// con el formato "EPyyyyMM_D000_C00000.aaMMdd_HHmm" generado anteriormente 
        /// </summary>
        public string ObtenerNombreArchivo()
        {
            string retorno = "";
            try
            {   //Leer Ruta Origen              
                StringBuilder stb1 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                retorno = stb1.ToString();
                
            }
            catch (Exception)
            {

                
            }
            return retorno;
        }

        /// <summary>
        /// Borrar un archivo
        /// </summary>
        public static void EliminaArchivo(string RutaDestino)
        {
            if (System.IO.File.Exists(RutaDestino))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(RutaDestino);
                info.Attributes = System.IO.FileAttributes.Normal;
                System.IO.File.Delete(RutaDestino);
            }
        }


        //public bool BuscarColectora(string Ruta)
        //{
        //    //bool existe = false;
        //    //string VarNom = "";
        //    //List<string> VarVal = new List<string>();

        //    ////remplaza las variables dentro de la cadena
        //    //int i1, i2, i3;  // i1:'{'  -  i2:';'  -  i3:'}'
        //    //i1 = Ruta.IndexOf("M");
        //    //while (i1 >= 0)
        //    //{
        //    //    i3 = Ruta.IndexOf("-", i1);       //Busca cierre llave
        //    //    if (i3 > i1)
        //    //    {
        //    //        i2 = Ruta.IndexOf("C", i1, i3 - i1);  //Busca dos puntos
        //    //        if (i2 < i1) i2 = i3;
        //    //        VarNom = Ruta.Substring(i1 + 1, i2 - i1 - 1);
        //    //        VarVal.Add(VarNom);
        //    //        Ruta = Ruta.Substring(0, i1 + 1) +
        //    //                (VarVal.Count - 1).ToString().Trim() +
        //    //                Ruta.Substring(i2);
        //    //        existe = true;

        //    //        return existe;
        //    //    }
        //    //    i1 = Ruta.IndexOf("{", i1 + 1);
        //    //}  //Hasta aca se tiene la cadena de formato                       
        //    //return existe;      
        //}

        //public string BuscarNombreColectora(string Ruta)
        //{
        //    string existe = "";
        //    string VarNom = "";
        //    List<string> VarVal = new List<string>();

        //    //remplaza las variables dentro de la cadena
        //    int i1, i2, i3;  // i1:'{'  -  i2:';'  -  i3:'}'
        //    i1 = Ruta.IndexOf("{");
           
        //    while (i1 >= 0)
        //    {
        //        i3 = Ruta.IndexOf("}", i1);       //Busca cierre llave
             
        //        if (i3 > i1)
        //        {
        //            i2 = Ruta.IndexOf(":", i1, i3 - i1);  //Busca dos puntos
        //            if (i2 < i1) i2 = i3;
        //            VarNom = Ruta.Substring(i1 + 1, i2 - i1 - 1);
        //            VarVal.Add(VarNom);
        //            existe = VarNom;
        //            return existe;
        //        }
                
        //    }  //Hasta aca se tiene la cadena de formato                        
        //    return existe;
        //}

      
           
        /// <summary>
        /// Metodo que arma la ruta de Cargas enviadas y Respaldo para guardar dentro de cada distrito correspondiente al 
        /// que se selecciono del listview1
        /// </summary>
        public string RutaEnviadas()
        {
            try
            {           
            ListView.SelectedIndexCollection indexes = this.listViewCargasProcesadas.SelectedIndices;
            
            int indice;
            int indiceDistrito = 67;     

            List<string> VarVal = new List<string>();
            foreach (int index in indexes)
            {
                indice = listViewCargasProcesadas.Items[index].Index;
                    if (cmbDevices.Text != "")
                    {
                        Vble.CarpetaSeleccionada = listViewCargasProcesadas.Items[index].Text + "           " + cmbDevices.Text;
                    }
                    else if (cmbDevicesWifi.Text != "") 
                    {
                        Vble.CarpetaSeleccionada = listViewCargasProcesadas.Items[index].Text + "           " + cmbDevicesWifi.Text;
                    }
                Vble.CarpetaSeleccionada = listViewCargasProcesadas.Items[index].Text + "           " + cmbDevices.Text;
                Vble.RutaCarpetaOrigen = ArrayCarpetasCargas[indice].ToString();
                if (Vble.RutaCarpetaOrigen.Contains("D2"))
                {
                    indiceDistrito = Vble.RutaCarpetaOrigen.IndexOf("D2");                    
                    Vble.Localidad = Vble.RutaCarpetaOrigen.Substring(indiceDistrito + 1, 3);
                }

            }
            //Se crea Ruta de carpeta respaldo de la carga procesada seleccionada para guardar lo que se envia a la colectora
            Vble.RespaldoEnviadas = Vble.ValorarUnNombreRuta(Vble.CarpetaCargasSiEnviadas) + 
                                    Vble.Localidad + Vble.CarpetaEnviadas + Vble.CarpetaSeleccionada;
           

            //Se crea la carpeta Enviadas dentro de la Carpeta de la localidad a la que pertenece La Carga Procesada.
            Vble.RutaCarpetaEnviadas = Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaCargasSiEnviadas) +
                                              Vble.Localidad + Vble.CarpetaEnviadas + Vble.CarpetaSeleccionada;
            }
            catch (Exception)
            {
                MessageBox.Show("Excepción en metodo Ruta Enviadas o Panel Cargas procesadas, verificar Directorio que" +
                                " contienen a los mismos, comunicarse con el administrador.", "Exepcion al Leer directorio Rutas para cargar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return Vble.RutaCarpetaEnviadas;

            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                int[] array = {1, 2, 3};
                                
                array.Reverse();

                MessageBox.Show(array.ToString());
          
            

            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
            }


            //if ("123df3".Any(x => !char.IsNumber(x)))
            //{
            //    MessageBox.Show("Es alfanumerico");
            //}
            //else
            //{
            //    MessageBox.Show("Es numero");
            //}

        }


        private static void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            //Dentro del evento watcher_EventArrived
            e.NewEvent.Properties["MCC-"].Value.ToString();
            //timer2.Start();
        }


        /// <summary>
        /// Constantemente esta verificando cuando se conecta un dispositivo y lo carga al combobox, 
        /// en caso de las colectoras lo carga con su nombre configurado (MICC-xxxx)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {            
                var deviceIds = new Dictionary<string, string>();
                var dispositvos = new StandardWindowsPortableDeviceService();
                /// Localidades que reconocen la colectora con dispositivos.Count = 1: (Instalador Capital)
                /// Esquina
                /// Santo Tome
                ///Localidad que reconoce la colectora con dispositivo.Count = 0: (Instalador Interior)
                /// Monte Caseros.
                /// Curuzu Cuatia.                
                /// Goya         
                /// Capital PC ENZO
                /// Capital PC Macro Intell
                /// Localidad que reconoce la colectora con dispositivo.Count = 0 EN CARGAS Y EN DESCARGAS: (Instalador Interior)
                /// Santa Rosa
                if (dispositvos.Devices.Count() == 0)
                {
                    deviceIds.Clear();
                    cmbDevices.Items.Clear();
                    timer1.Interval = 2000;
                    ListViewColectora.Visible = false;

                    if (Directory.Exists(Vble.RutaTemporal))
                    {
                        Directory.Delete(Vble.RutaTemporal, true);
                    }
                }
                else
                {
                    foreach (var device in dispositvos.Devices)
                {               
                    device.Connect();
                    if (device.FriendlyName.Contains("MIC"))
                    {
                        LabNomColect.Text = device.FriendlyName;
                        if (deviceIds.Count() == 0)
                        {
                            deviceIds.Add(device.FriendlyName, device.DeviceID);
                            var rootFolder = device.GetContents().Files;
                            currentFolder = device.GetContents();
                            var carpetas = currentFolder.Files;
                            foreach (var CarpetaAlmacIntComp in carpetas)
                            {
                                if (cmbDevices.Items.Count == 0)
                                {
                                    if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
                                    {
                                        var contents = device.GetContents();
                                        cmbDevices.Items.Add(device.ToString());
                                        cmbDevicesWifi.Items.Add(device.ToString());
                                        Vble.dispositivo = device.ToString();
                                        cmbDevices.SelectedIndex = 0;
                                        timer2.Interval = 3000;                                            
                                        device.Disconnect();
                                        break;
                                    }
                                }
                            }
                        }          
                        device.Disconnect();                           
                    }
                    else
                    {                        
                        device.Disconnect();
                        //cmbDevices.Items.Clear();
                        //timer2.Interval = 2000;                     
                        //ListViewColectora.Visible = false;                      
                        ////ListViewColectora.Visible = false;
                    }

                }
                GC.SuppressFinalize(this);
                }
            }
            catch (Exception)
            {
                
            }


            ////ManagementEventWatcher watcher = new ManagementEventWatcher();
            ////WqlEventQuery consulta = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            ////watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            ////watcher.Query = consulta;
            ////watcher.Start();
            //////watcher.WaitForNextEvent();
            //try
            //{
            //    //cmbDevices.Items.Clear();
            //    cmbDevicesWifi.Items.Clear();
            //    //shellView2.RefreshContents();
            //    //service.Devices.Clear();
            //    IList<WindowsPortableDevice> devices = service.Devices;
            //    devices.ToList().ForEach(device =>
            //    {
            //        device.Connect();
            //        if (Funciones.BuscarColectora(device.ToString()) || device.ToString() == "WindowsPortableDevicesLib.Domain.WindowsPortableDevice")
            //        {
            //            var rootFolder = device.GetContents().Files;
            //            currentFolder = device.GetContents();
            //            var carpetas = currentFolder.Files;

            //            foreach (var CarpetaAlmacIntComp in carpetas)
            //            {
            //                if (cmbDevices.Items.Count == 0)
            //                {
            //                    if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
            //                    {
            //                        var contents = device.GetContents();
            //                        cmbDevices.Items.Add(device.ToString());
            //                        //cmbDevicesWifi.Items.Add(device.ToString());
            //                        Vble.dispositivo = device.ToString();
            //                        device.Disconnect();
            //                    }
            //                }
            //            }
            //            //if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
            //            //    {   
            //            //            var contents = device.GetContents();
            //            //            cmbDevices.Items.Add(device.ToString());
            //            //            //cmbDevicesWifi.Items.Add(device.ToString());
            //            //            Vble.dispositivo = device.ToString();
            //            //            device.Disconnect();                                    
            //            //    }
            //            //}                           
            //            //ListViewColectora.Visible = false;                      
            //        }
            //    });

            //    if (cmbDevices.Items.Count > 0)
            //    {
            //        cmbDevices.SelectedIndex = 0;
            //        //cmbDevicesWifi.SelectedIndex = 0;
            //    }
            //    else
            //    {
            //        //ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
            //        //shellView2.CurrentFolder = folder;
            //        ////BotActPanPC_Click(sender, e);
            //        //shellView2.Visible = false;
            //        ListViewColectora.Visible = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    timer2.Stop();
            //    shellView2.Visible = false;
            //    ListViewColectora.Visible = false;
            //    cmbDevices.Items.Clear();
            //    //MessageBox.Show(r.Message + " Erro al leer colectora conectada");
            //}
        }


        private void button1_Click(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Aca se ejecuta Procesar Carga en segundo plano donde ingresa los datos de las conexiones seleccionadas 
        /// a las tablas Conexiones Medidores y Personas dentro del archivo de base de datos SQLite el cual luego es
        /// enviado a la colectora, haciendose una copia de respaldo en la pc con los datos del equipo al que se mando, fecha
        /// y hora
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {                          
                simulateHeavyWork(Vble.CantRegistros);
                form0.ControlBox = false;
              
                //Carga los datos a la base de datos sqlite a enviar.
                EjecutarCarga();

                //Crea Directorios Correspondientes y copia archivo SQLite para cargar a la colectora
                //backgroundWorker1.ReportProgress(i);
                GenerarCarpetaArchivo();
         
               

            }
            catch (Exception r)
             {
                //this.Cursor = Cursors.Default;
                MessageBox.Show(r.Message + " Error al comenzar con el proceso de carga en Metodo backgroundprocesarCarga");
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + r.Message +
                                                              " Error al comenzar con el proceso de carga en Carga de Colectoras \n");

            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            progressBar1.Visible = true;
            PorcLabel.Visible = true;
            progressBar1.Value = (e.ProgressPercentage * 100) / (Vble.CantRegistros);
            PorcLabel.Text = (e.ProgressPercentage * 100) / Vble.CantRegistros + " %";

        }
        /// <summary>
        /// Finalizacion del proceso en Segundo Plano
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

            
            progressBar1.Visible = false;
            PorcLabel.Visible = false;
            //botNavAtras.Visible = true;
            MessageBox.Show("Se generó la carga con las " + labelCantReg.Text +
                            " conexiones de la secuencia seleccionada", "Carga Procesada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BotActPanPC_Click(sender, e);
            //this.dataGridView1.DataSource = "";
            this.labelCantReg.Text = "0";
            Vble.TablaConexSelec.Clear();
            timer1.Start();
            BotProcesCarg.Enabled = true;
            BotActPanPC.Enabled = true;
            btnCerrar.Enabled = true;
            form0.ControlBox = true;
            BotEnviarCarga.Enabled = true;
            this.Cursor = Cursors.Default;
            DB.con.Close();
            Procesando = 0;
            Enviando = 0;
            labelProcesando.Visible = false;
            labelEnviando.Visible = false;
            //ShellItem path = new ShellItem(Vble.CarpetasGenerada);
            //shellView1.CurrentFolder = path;
            //shellView1.Visible = true;
            //fileFilterComboBox1.Visible = true;
            PickBoxLoading.Visible = true;
            Task oTask = new Task(listarRutasDisponiblesTASK);
            oTask.Start();
            await oTask;

            }
            catch (Exception R)
            {
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + R.Message +
                                                              " Error al Terminar Proceso en Segundo Plano de procesar Carga en Cargas de colectora \n");
            }
            

        }

        private void simulateHeavyWork(int cantidad)
        {          
            Thread.Sleep(cantidad);
           
        }    
             

        private void shellView1_Click_1(object sender, EventArgs e)
        {

        } 
        
    
        private void fileFilterComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Copia el directorio especificado de la ruta origen (SourceDirName) a la ruta destino(DestDirName),
        /// si se envia con true copia todos los subdirectorios y archivos
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public static void CopiarDirectorio(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {

                
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                

                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException("No existe la carpeta que desea enviar"  + sourceDirName);
                }

                DirectoryInfo[] dirs = dir.GetDirectories();

                //If the destination directory doesn't exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {

                    //string temppath = Path.Combine(destDirName, file.Name);
                    string temppath = destDirName + "\\" + file.Name;
                    //var rut = "C:/A_DPEC/_Pruebas/EmpresaLocal/202402/Envios_Cargas/201/Enviadas/EP202402_D201_C00270.240325_123412        666/" + file.Name;
                    //File.Create(temppath);
                    file.CopyTo(temppath, true);
                    //File.Copy(file.FullName, temppath, true);
                }

                //If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        //MessageBox.Show(subdir.Name);
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        CopiarDirectorio(sourceDirName, temppath, copySubDirs);
                    }
                }
            }
            catch (Exception)
            {

               
            }
        }

        /// <summary>
        ///  Metodo que me cambia el estado de "Listo para Cargar" a "Cargados" todos las conexiones de la 
        /// </summary>
        /// <returns></returns>
        private DataTable CambiarEstadoConexionesSqlite(int StatusChange)
        {
            string txSQL;
            DataTable Tab;
            SQLiteDataAdapter da;
            SQLiteCommandBuilder comandoSQL;
            Tab = new DataTable();

            //SQLiteConnection conexion = new SQLiteConnection("Data Source=F:/Google Drive/MACRO INTELL -Software/gagFIS - Interfase/gagFIS - Interfase/Resources/dbFIS - DPEC.db");
           

            try
            {

                txSQL = "SELECT * FROM Conexiones ORDER BY ConexionID ASC";

                da = new SQLiteDataAdapter(txSQL, DB.con);
                comandoSQL = new SQLiteCommandBuilder(da);
                da.Fill(Tab);

                //declaracióno de variables
              

            }
            catch (Exception)
            {
                //MessageBox.Show(e.Message);
            }
            return Tabla;
        }

     
        /// <summary>
        /// Metodo que copia y envia archivos a carpetas Enviadas, Respaldo, y Colectora
        /// </summary>
        private void EnviarArchivos()
        {
            try
            {
                string colectora = cmbDevices.Text;
                string ColectoraWifi = cmbDevicesWifi.Text;
                LabRestEnvArc.Visible = false;
                //ruta del archivo .db creado al procesar la carga                                                   
                DirectoryInfo Origen = new DirectoryInfo(Vble.RutaCarpetaOrigen);
                string comprimido;
                comprimido = Origen + ".zip";
                string Origen1 = Vble.RutaCarpetaOrigen;
               
                //verifica si existe la ruta "Enviadas" dentro de cada distrito sino la crea y envia una copia allí
                if (!Directory.Exists(RutaEnviadas() ))
                    Vble.CrearDirectorioVacio(RutaEnviadas());

                CopiarDirectorio(Origen1, RutaEnviadas().TrimEnd(' '), true);

                //Vble.CarpetaRespaldo = Vble.CarpetaRespaldo +  Vble.RespaldoEnviadas + "       " + Vble.Colectora;

                //verifica si existe la ruta "Respaldo" en Pruebas sino la crea y envia una copia allí
                if (!Directory.Exists(Vble.CarpetaRespaldo))
                    Vble.CrearDirectorioVacio(Vble.CarpetaRespaldo + Vble.RespaldoEnviadas);

                CopiarDirectorio(Origen1, (Vble.CarpetaRespaldo + Vble.RespaldoEnviadas).TrimEnd(' '), true);


                //Por ultimo mueve el archivo seleccionado a la colectora y elimina el original que se muestra en el lisview
                //quedando dentro de carpetas enviadas y en respaldo.
                if (Directory.Exists(Vble.RutaCarpetaOrigen))
                    ///Esta Linea comprime el directorio que contiene los tres archivos para enviar al servidor un solo documento
                    ZipFile.CreateFromDirectory(Origen.FullName, Origen.FullName + ".zip");


                //Consulta que opcion está seleccionada para asi enviar por la opción correcta
                //Wifi o Cable
                if (RBEnvCable.Checked == true)
                {
                    /////llamo al metodo que envia los archivos generados que estan en la pc como rutas procesadas y los envia
                    /////a la colectora
                    string CarpetaCarga;

                    if (EnviarArchivosAColectora(Origen))
                    {
                    //    string CarpetaCargaNAS_PRD = "";
                    //    string CarpetaCargaNAS_QAS = "";
                    //    ///Creo la direccion del directorio en partes porque al ser muy extensos el parametro para crear con 
                    //    ///el metodo Directory.CreateDirectory().
                    //    ///
                    //    if (DB.Entorno == "PRD")
                    //    {
                    //        CarpetaCargaNAS_PRD = Vble.CarpetaCar_Desc_ColectorasNAS_PRD +
                    //                                                 Vble.CarpetaCargasSiEnviadas +
                    //                                                 Vble.ArrayZona[0].ToString() + "\\";
                    //        CarpetaCargaNAS_PRD = Vble.ValorarUnNombreRuta(Vble.CarpetaCar_Desc_ColectorasNAS_PRD);

                    //        if (!Directory.Exists(CarpetaCargaNAS_PRD))
                    //        {
                    //            Directory.CreateDirectory(CarpetaCargaNAS_PRD);
                    //        }
                    //        CarpetaCarga = DateTime.Today.ToString("yyyyMMdd") + "\\" +
                    //                                                Vble.CarpetaSeleccionada.Substring(Vble.CarpetaSeleccionada.IndexOf("C"));
                    //        ///---------------------------------------------------------------------------------------------------------
                    //        CarpetaCargaNAS_PRD = CarpetaCargaNAS_PRD + CarpetaCarga;

                    //        CopiarDirectorio(Origen1, CarpetaCargaNAS_PRD, true);
                    //    }
                    //    else
                    //    {
                    //        CarpetaCargaNAS_QAS = Vble.CarpetaCar_Desc_ColectorasNAS_QAS +
                    //                                                 Vble.CarpetaCargasSiEnviadas +
                    //                                                 Vble.ArrayZona[0].ToString() + "\\";
                    //        CarpetaCargaNAS_QAS = Vble.ValorarUnNombreRuta(CarpetaCargaNAS_QAS);

                    //        if (!Directory.Exists(CarpetaCargaNAS_QAS))
                    //        {
                    //            Directory.CreateDirectory(CarpetaCargaNAS_QAS);
                    //        }
                    //        CarpetaCarga = DateTime.Today.ToString("yyyyMMdd") + "\\" +
                    //                                                Vble.CarpetaSeleccionada.Substring(Vble.CarpetaSeleccionada.IndexOf("C"));
                    //        ///---------------------------------------------------------------------------------------------------------
                    //        CarpetaCargaNAS_QAS = CarpetaCargaNAS_QAS + CarpetaCarga;

                    //        CopiarDirectorio(Origen1, CarpetaCargaNAS_QAS, true);
                    //    }
                    }
                }
                else if (RBEnvWifi.Checked == true)
                {
                    if (Vble.ExistenArchEnServCargas(Vble.ArrayZona[0].ToString(), ColectoraWifi, "Cargas") == "NO")
                    {
                        Continuar = Vble.EnviarArchivosAServidor(Origen.FullName, ColectoraWifi, Vble.ArrayZona[0].ToString()) == "SI" ? true : false;
                    }
                    else
                    {
                        MessageBox.Show("La Colectora ya tiene una ruta asignada en espera de que acepte la carga", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        Directory.Delete(Vble.CarpetaRespaldo + Vble.RespaldoEnviadas, true);
                        Directory.Delete(RutaEnviadas(), true);
                        File.Delete(Origen.FullName + ".zip");
                        Continuar = false;
                    }                    
                }


                if (Continuar)
                {
                    ActualizaListViewColectora(Vble.RutaCarpetaOrigen);
                    AgregarColectoraAInfoCarga();

                    //cambia el estado de las conexiones enviadas de "Listo para enviar"(300) a "Enviados"(400) de la base MySql general. 
                    Vble.CambiarEstadoEnviadasMySql(RutaEnviadas(), Convert.ToInt32(cteCodEstado.Cargado));

                    //Elimina el archivo SQLite generado dejando solo en la colectora para que no se produza redundancia de datos
                    Directory.Delete(Origen1, true);
                    //Directory.Delete(Origen.FullName);
                }



            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
                Continuar = false;
            }
        }



        /// <summary>
        /// Metodo que envia los archivos que se procesaron para la carga a la colectora conectada
        /// </summary>
        public bool EnviarArchivosAColectora(DirectoryInfo RutaCarpetaOrigen)
        {
            bool retorno = false;

            try
            {
                StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
                IList<WindowsPortableDevice> devices = services.Devices;
                devices.ToList().ForEach(device =>
                {
                    device.Connect();

                    if (Funciones.BuscarColectora(device.ToString()))
                    {
                        var rootFolder = device.GetContents().Files;
                        currentFolder = device.GetContents();
                        var carpetas = currentFolder.Files;

                        foreach (var CarpetaAlmacIntComp in carpetas)
                        {
                            if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
                            {
                                currentFolder = device.GetContents((PortableDeviceFolder)CarpetaAlmacIntComp);
                                carpetas = currentFolder.Files;

                                foreach (var CarpetaDatosDpec in carpetas)
                                {
                                    if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                                    {
                                        CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                                        var Archivos = CarpetaDatosDPEC.Files;
                                        //Pregunto si existen archivos antes de enviar la nueva carga, 
                                        //si existe elimina lo que habia para poder copiar la nueva carga
                                        //si no existen archivos pasa el if y no elimina nada.
                                        if (Archivos.Count > 0)
                                        {
                                            foreach (PortableDeviceFile arc in Archivos)
                                            {
                                                device.DeleteFile(arc);
                                            }
                                        }
                                        //Este For se encarga de cargar los archivos procesados con la nueva carga
                                        //que se encuentran almacenados en un arraylist, en donde cada elemento
                                        //contiene el nombre del archivo a enviar con la ubicación del mismo
                                        foreach (var item in RutaCarpetaOrigen.GetFiles())
                                        {
                                            device.TransferContentToDevice(item.FullName, CarpetaDatosDpec.Id);
                                        }

                                        retorno = true;
                                        //for (int i = 0; i < ArchivosProcesados.Count; i++)
                                        //{
                                        //    device.TransferContentToDevice(ArchivosProcesados[i].ToString(), CarpetaDatosDpec.Id);
                                        //}
                                }
                            }
                        }                    
                       
                        }
                        //}
                        device.Disconnect();
                    }
                });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return retorno;
        }


        //Actualiza el listview de la colectora para corroborar la existencia 
        //o no de archivos en la carpeta compartida entre pc y colectora
        private void ActualizaListViewColectora(string temporal)
        {
            if (ListViewColectora.Items.Count > 0)
            {
                for (int i = 0; i < ListViewColectora.Items.Count;)
                {
                    ListViewColectora.Items[i].Remove();
                }
            }
            ListViewColectora.Items.Clear();
            shellView2.RefreshContents();
            //string Dispositivo;
            //string Ruta;
            //Dispositivo = Funciones.BuscarNombreColectora(Vble.DirectorioColectoraenPC);
            //Vble.Colectora = Dispositivo + cmbDevices.Text + "\\" + Vble.CarpetaDestinoColectora + "\\";
            //Ruta = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;
            //DirectoryInfo Destino1 = new DirectoryInfo(Ruta);
            //LeeCarpetaColectora(Destino1.FullName);
            LeeCarpetaColectora(temporal);
        }
        


        //Botono que se encarga de pasar los archivos seleccionados del panel Rutas para cargar
        //a la colectora y carga el panel de datos de Rutas en Colectora
        private void button1_Click_2(object sender, EventArgs e)
        {
            
            RestNod.Visible = false;
            try
            {
                if (listViewCargasProcesadas.SelectedItems.Count > 0) 
                    {
                    if (shellView2.Visible == true)
                    {
                        if (ListViewColectora.Visible == true)
                            { 
                                if (ListViewColectora.Items.Count == 0)
                                {
                                    if (shellView2.Enabled == false)
                                    {
                                        Procesando = 0;
                                        Enviando = 1;
                                        timer3.Interval = 100;
                                        timer3.Enabled = true;
                                        labelEnviando.Text = "Enviando";
                                        labelEnviando.Visible = true;
                                        //envia el proceso de enviar archivo de la pc a colectora a un proceso en segundo plano. 
                                        //llamado backgroundProcesarCarga
                                        backgroundEnviarArchivo.RunWorkerAsync();
                                    }
                                }
                                else
                                {
                                        MessageBox.Show("No se puede cargar mas de un archivo a la Colectora", "Atención",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //ListViewColectora.Visible = true;                            
                                        shellView2.Visible = true;
                                        ListViewColectora.Visible = true;
                                }
                            }
                            else
                            {
                            LabRestEnvArc.Text = "  Debe Seleccionar el dispositivo\n conectado para realizar el envío";
                            LabRestEnvArc.ForeColor = Color.Red;
                            LabRestEnvArc.Visible = true;
                            LabRestDevArc.Visible = false;
                        }
                    }    
                        else
                          {
                            LabRestEnvArc.Text = "  Debe Seleccionar el dispositivo\n conectado para realizar el envio";
                            LabRestEnvArc.ForeColor = Color.Red;
                            LabRestEnvArc.Visible = true;
                            LabRestDevArc.Visible = false;
                          }
                    
                    }
                    else
                    {
                         LabRestEnvArc.Text = "  Debe Seleccionar el Archivo \n del panel Rutas para Cargar \n que desea Enviar a la colectora";
                         LabRestEnvArc.ForeColor = Color.Red;
                         LabRestEnvArc.Visible = true;
                         LabRestDevArc.Visible = false;
                }   
                }

                catch (Exception r)
                    {
                //MessageBox.Show("El archivo seleccionado ya se envió con anteriorida");   
                       MessageBox.Show(r.Message);
                    }
            
            //button2_Click_2(sender, e);
        }


        public void button2_Click_2(object sender, EventArgs e)
        {
            timer2.Start();
            #region Codigo comentario que no se usa
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            //ManagementObjectCollection items = searcher.Get();
            //foreach (ManagementObject item in items)
            //{
            //    var variable = item.Path;
            //}          
            //foreach (var item in res)
            //{
            //    MessageBox.Show("ID: " + item.Value.Id.ToString() + " Model: " + item.Value.Model.ToString()
            //        + " Tipo: " + item.Value.Type.ToString());
            //}                                           
            //ManagementEventWatcher watcher = new ManagementEventWatcher();
            //WqlEventQuery consulta = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            //watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            //watcher.Query = consulta;
            //watcher.Start();
            //watcher.WaitForNextEvent();
            //IList<WindowsPortableDevice> devices = service.Devices;
            //devices.ToList().ForEach(device =>
            //{
            //    device.Connect();
            //    if (Funciones.BuscarColectora(device.ToString()))
            //    {
            //        device.Disconnect();
            //        cmbDevices.Items.Clear();
            //        ListViewColectora.Visible = false;
            //        timer2.Stop();
            //        Thread.Sleep(700);
            //        toolTip5.Show("Es seguro quitar el dispositivo", cmbDevices, 3000);
            //    }
            //});
            //MessageBox.Show("Es seguro quitar el dipositivo", "Expulsión de Dispositovo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion
        }

        /// <summary>
        /// Cuando selecciono el dispositivo que esta conectado(Colectora de datos), automaticamente me posiciona en la carpeta 
        /// "Datos DPEC" (especificado en archivo.ini) 
        /// que se encuentra dentro de la colectora y muestra el contenido en el listbox. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDevices_SelectionChangeCommitted(object sender, EventArgs e)
        {


            string Temporal = @"C:/Users/" + Environment.UserName + "/Documents/Temporal";
            //string Temporal = @"C:\Users\Usuario\Documents\Informes PDF";
            DirectoryInfo ArchivosEnTemporal = new DirectoryInfo(Temporal);
            string direcColec = "";
            try
            {

                //Pregunto si existe la carpeta temporal, si existe borra los archivos que hay en el para no prestar confusion
                //con otra ruta, etc. Si no existe crea la carpeta temporal vacia para trabajar con los archivos que
                //que se encuentran en la colectora
                if (!Directory.Exists(Temporal))
                {
                    Directory.CreateDirectory(Temporal);
                }
                else
                {
                    foreach (var item in ArchivosEnTemporal.GetFiles())
                    {
                        try
                        {
                            File.Delete(item.FullName);
                        }
                        catch (Exception)
                        {
                            SQLiteConnection Base = new SQLiteConnection("Data Source=" + item.FullName);
                            if (Base.State == ConnectionState.Open)
                            {
                                Base.Dispose();
                                Base.Close();
                            }
                        }
                    }
                }
                //----------------------------------------------------------------------------------------------------------------
                             
                //Conecto el dispositivo para verificar si existen archivos o Ruta cargada, asi podrá mandar una nueva carga 
                //a la colectora o no.
                StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
                IList<WindowsPortableDevice> devices = services.Devices;
                devices.ToList().ForEach(device =>
                {
                    device.Connect();

                    if (Funciones.BuscarColectora(device.ToString()))
                    {

                        var rootFolder = device.GetContents().Files;

                        foreach (var item in rootFolder)
                        {
                            if (item.Name == "Almacenamiento interno compartido")
                            {
                                currentFolder = device.GetContents((PortableDeviceFolder)item);
                                var carpetas = currentFolder.Files;
                                direcColec = "Este equipo/" + device.FriendlyName + "/" + item.Name;
                                foreach (var CarpetaDatosDpec in carpetas)
                                {
                                    if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                                    {

                                        CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                                        var Archivos = CarpetaDatosDPEC.Files;
                                        direcColec +=  "/" + CarpetaDatosDPEC.Name;
                                        //Pregunto si existen archivos antes de enviar la nueva carga, 
                                        //si existe elimina lo que habia para poder copiar la nueva carga
                                        //si no existen archivos pasa el if y no elimina nada.
                                        if (Archivos.Count > 0)
                                        {
                                            foreach (PortableDeviceFile arc in Archivos)
                                            {
                                                //direcColec += "/" + arc.Name.Remove(arc.Name.IndexOf("."));
                                                //device.DeleteFile(arc);

                                                device.GetFile(arc, Temporal + "/");
                                                //device.DownloadFile(arc, Temporal);
                                                //File.Copy(direcColec, Temporal);
                                                //device.TransferContentToDevice(arc.ToString(), Temporal + "\\");
                                            }
                                        }
                                        ////Este For se encarga de cargar los archivos procesados con la nueva carga
                                        ////que se encuentran almacenados en un arraylist, en donde cada elemento
                                        ////contiene el nombre del archivo a enviar con la ubicación del mismo
                                        //for (int i = 0; i < ArchivosProcesados.Count; i++)
                                        //{
                                        //    device.TransferContentToDevice(ArchivosProcesados[i].ToString(), CarpetaDatosDpec.Id);
                                        //}
                                    }
                                }
                            }
                        }

                                             
                        device.Disconnect();
                    }
                });
              

            ////Pregunto si es igual a 3 porque si no hay 3 archivos que son las dos bases y el archivo InforCarga.txt es porque
            ////la colectora no esta cargada correctamente y no podré hacer la consulta sobre que ruta esta cargada en la colectora
            if (ArchivosEnTemporal.GetFiles().Length == 3)
                {
                    foreach (var ArchivoBase in ArchivosEnTemporal.GetFiles())
                    {
                        if (ArchivoBase.Name == Vble.NombreArchivoBaseSqlite())
                        {
                            foreach (var ArchivoInfo in ArchivosEnTemporal.GetFiles())
                            {
                                if (ArchivoInfo.Name == "InfoCarga.txt")
                                {
                                    MessageBox.Show("La Colectora ya contiene la Carga: \n" + Funciones.LeerArchivostxt(ArchivoInfo.FullName) + " Por favor primero realice la descarga para volver a enviar otro Archivo",
                                                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ActualizaListViewColectora(Temporal);
                                    shellView2.Visible = true;
                                    ListViewColectora.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            ActualizaListViewColectora(Temporal);
                            shellView2.Visible = true;
                            ListViewColectora.Visible = true;
                        }
                    }
                }

                else if (ArchivosEnTemporal.GetFiles().Length == 1 || ArchivosEnTemporal.GetFiles().Length == 0)
                {
                    ActualizaListViewColectora(Temporal);
                    shellView2.Visible = true;
                    ListViewColectora.Visible = true;
                    foreach (var Archivos in ArchivosEnTemporal.GetFiles())
                    {
                        if (Archivos.Name == Vble.NombreArchivoBaseFijaSqlite())
                        {
                            ActualizaListViewColectora(Temporal);
                            shellView2.Visible = true;
                            ListViewColectora.Visible = true;
                        }
                    }
                }
              

                if (Directory.Exists(Temporal))
                {
                    Directory.Delete(Temporal, true);
                }

                
            }

            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al seleccionar Colectora");
            }

        }


    //------------------------------------------------------------------------
    // - - - - -  clase clInfoNodos - - - - - - - - - - - - - - - - - -

    /// <summary>
    /// Esta clase se usa para guardar información de los nodos del TreeView
    /// y poder llevar la cuenta de las conexiones seleccionadas y totales
    /// <para>Lo que aqui se llama Remesa, DPEC lo llama Remesa</para>
    /// </summary>
    public class clInfoNodos : ICloneable {
        public string Key { get; set; }
        public string Texto { get; set; }
        public int CnxTotal { get; set; }
        public int CnxSelected { get; set; }
        public int RutasTotal { get; set; }
        public int RutasSelected { get; set; }
        public int RemesasTotal { get; set; }
        public int RemesasSelected { get; set; }
        public int Distrito { get; set; }
        public int Ruta { get; set; }
        public int Remesa { get; set; }
        public string Particion { get; set; }
        public string ImageKey { get; set; }
        
        /// <summary> Obtiene o establece el número de secuencia del primer elemento
        /// </summary>
        public int Desde { get; set; }
        /// <summary>Obtiene o establece el número de secuencia del último elemento
        /// </summary>
        public int Hasta { get; set; }
        /// <summary>Permite compiar una copia como nueva instancia
        /// </summary>
        /// <returns></returns>
        public object Clone() {
            return MemberwiseClone();
        }
        }
        
        /// <summary>
        ///  Boton que actualiza el listview1 de las carpetas procesadas 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BotActPanPC_Click(object sender, EventArgs e)
        {           
            LimpiarPanelCargasAenviar();
            CargasProcesadas();
            LeeCargasEnviadas();
            LeerCargasRecibidas();
            //timer1_Tick(sender, e);
            //timer2.Start();
        }  
        
        private void ActualizarListview ()
        {
            LimpiarPanelCargasAenviar();
            CargasProcesadas();
            LeeCargasEnviadas();
            LeerCargasRecibidas();
            //timer1_Tick(sender, e);
        } 

        /// <summary>
        /// Al hacer click sobre el items del lisview arma la direccion completa a la que pertenece
        /// para utilizarla luego en el envio a la colectora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            LabRestEnvArc.Visible = false;
            LabRestDevArc.Visible = false;
            
            ListView.SelectedIndexCollection indexes = this.listViewCargasProcesadas.SelectedIndices;
            int indice;
            foreach (int index in indexes)
            {                
                indice = listViewCargasProcesadas.Items[index].Index;
                Vble.CarpetaSeleccionada = listViewCargasProcesadas.Items[index].Text;                                
                Vble.RutaCarpetaOrigen = ArrayCarpetasCargas[indice].ToString();
                //MessageBox.Show(RutaEnviadas() + " = " + Vble.CarpetaSeleccionada);
                //MessageBox.Show(RutaEnviadas() + " = " + Vble.RutaCarpetaOrigen);
                //Vble.RutaCarpetaEnviadas = Vble.RutaCarpetaOrigen + Vble.ValorarUnNombreRuta(Vble.CarpetaCargasSiEnviadas);
            }

            if (e.Button == MouseButtons.Right)
            {
                //DGResumenExp.CurrentCell = DGResumenExp.Rows[e.RowIndex].Cells[e.ColumnIndex];
                //obtenemos las coordenadas de la celda seleccionada.
                //Rectangle coordenada = DGResumenExp.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                var coordenada = tvwCargas.GetNodeAt(e.X, e.Y);           
                //mostramos el menu             
                MessageBox.Show(coordenada.ToString());
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            LabRestEnvArc.Visible = false;
            LabRestDevArc.Visible = false;
        }


        /// <summary>
        /// Devuelve las conexiones que fueron procesadas en esa Carga al panel de Rutas Disponibles, esto se puede hacer
        /// antes de enviar la Carga a la colectora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotDevCarga_Click(object sender, EventArgs e)
        {
            
            StringBuilder stb = new StringBuilder();
            int Carga = 0;

            try
            {
                if (listViewCargasProcesadas.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show("¿Está seguro que desea devolver la ruta procesada al panel" +
                                        "\nde Rutas Disponibles?", "Devolver Rutas", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    { 
                        //cambia el estado de las conexiones enviadas de "Listo para enviar"(300) a "No Procesados"(000) de la base MySql general. 
                        //MessageBox.Show(Vble.RutaCarpetaOrigen);
                        Vble.CambiarEstadoEnviadasMySql(Vble.RutaCarpetaOrigen, Convert.ToInt32(cteCodEstado.NoCargado));

                   
                    ////Elimina la carpeta Generada y actualiza el listview de Rutas para Cargar ya que se devolvio las Cargas a Rutas Disponibles
                    Directory.Delete(Vble.RutaCarpetaOrigen, true);
                    BotActPanPC_Click(sender, e);

                        listarRutasDisponiblesTASK();

                        //timer1_Tick(sender, e);
                    }
                }
                else
                {
                    LabRestDevArc.Text = "Debe Seleccionar el Archivo \n del panel 'Rutas para Cargar' \n que desea devolver al panel \n de 'Rutas Disponibles'";
                    LabRestDevArc.ForeColor = Color.Red;
                    LabRestDevArc.Visible = true;
                    LabRestEnvArc.Visible = false;
                }
            }

            catch(Exception r)
            {             
                MessageBox.Show(r.Message);
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString() + "---> " + r.Message +
                                                " Error al devolver ruta procesada al panel de rutas disponibles \n");
            }
            
        }




        private void panel1_Click(object sender, EventArgs e)
        {
            LabRestEnvArc.Visible = false;
            LabRestDevArc.Visible = false;
        }
        private void panel2_Click(object sender, EventArgs e)
        {
            LabRestEnvArc.Visible = false;
            LabRestDevArc.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            cnt++;
            if (Procesando == 1)
            {
                labelProcesando.Text += " .";
                if (cnt > 4)
                {
                    labelProcesando.Text = "Procesando";
                    cnt = 0;
                }
            }
            else if (Enviando == 1)
            {
                labelEnviando.Text += " >";
                if (cnt > 4)
                {
                    labelEnviando.Text = "Enviando";
                    cnt = 0;
                }
            }
          
        }


        /// <summary>
        /// Obtiene la cantidad De conexiones que contiene la base a enviar para utilizar como tiempo para hacer dormir al proceso de
        /// envio en segundo plano de BackgroundEnviarAchivo.
        /// </summary>
        /// <returns></returns>
        private int CantidadRegistrosEnviar()
        { 
            //Lee y obtiene el nombre de la base Sqlite
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();            
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
            //BaseACargar.Open();
            int count = 0;

            string txSQL;
            SQLiteCommand da;
           
            DataTable Tabla = new DataTable();
            try
            {
                BaseACargar.Open();
                txSQL = "select Count(*) From conexiones";
                da = new SQLiteCommand(txSQL, BaseACargar);
                count = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();
            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message + " Error al contar la cantidad de conexiones a enviar");
            }           
            BaseACargar.Close();
            return count;
        }

        /// <summary>
        /// Proceso en segundo plano de Enviar Archivo de PC a colectora backgroundEnviarArchivo()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundEnviarArchivo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                            
            //duerme al proceso de acuerdo a la cantidad de registros que se envian
            simulateHeavyWork(CantidadRegistrosEnviar());
            CheckForIllegalCrossThreadCalls = false;
            BotProcesCarg.Enabled = false;
            BotEnviarCarga.Enabled = false;
            BotActPanPC.Enabled = false;
            btnCerrar.Enabled = false;
            string NombreColectora = "";
            NombreColectora = cmbDevices.Text == "" ? " " : cmbDevices.Text;
            Vble.Colectora = NombreColectora;
                
                ////Cambio el estado de las conexiones antes de ser enviadas a la colectora 
                ////pasa de 300(Listo para Cargar) a 400(Cargados en Colectora)
                Vble.CambiarEstadoConexionSqlite(Convert.ToInt32(cteCodEstado.NoCargado), NombreColectora);

                //Metodo que envia a la colectora el directorio seleccionado del listivew
                this.EnviarArchivos();

                if (Continuar)
                {
                    Vble.ModificarInfoConex(DateTime.Today.Date.ToString("yyyyMMdd"),
                                            DateTime.Now.ToString("HHmmss"),
                                            DB.sDbUsu, 0, "Carga", RutaEnviadas());

                    BeginInvoke(new InvokeDelegate(InvokeMethod));

                    //ActualizarListview();
                    //ActualizaListViewColectora();
                    timer1.Interval = 100;
                    timer1.Enabled = false;
                    //labelEnviando.Visible = false;
                }
                //Vble.ActualizaTablaVarios(DateTime.Today.Date.ToString("dd-MM-yyyy"), 
                //                        DateTime.Now.ToString("HH:mm"), 
                //                      RutaEnviadas());
                //Llama al boton que actualiza los listview de archivos sin enviar y enviados

            }
            catch (Exception R)
            {
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + R.Message +
                                                              " Error al comenzar Proceso en Segundo Plano al enviar ruta seleccionada a colectora \n");
            }
            
        }

        /// <summary>
        /// metodo que contiene los metodos que no se pueden ejecutar en segundo plano porque no fueron creados
        /// dentro del mismo proceso de Segundo Plano
        /// </summary>
        public void InvokeMethod()
        {
            try
            {           
                LimpiarPanelCargasAenviar();
                CargasProcesadas();
                LeeCargasEnviadas();
                LeerCargasRecibidas();
            }
            catch (Exception)
            {   
                
            }
        }

        private void backgroundEnviarArchivo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Continuar)
            {
                timer3.Interval = 100;
                timer3.Enabled = false;
                labelEnviando.Text = "Enviando";
                timer2.Start();
                labelEnviando.Visible = false;
                BotProcesCarg.Enabled = true;
                BotEnviarCarga.Enabled = true;
                BotActPanPC.Enabled = true;
                btnCerrar.Enabled = true;
                ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
                shellView2.CurrentFolder = folder;
                MessageBox.Show("Colectora cargada y lista para salir", "Carga exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                shellView2.Visible = false;
                ListViewColectora.Visible = false;
                cmbDevices.Text = "";
            }
            else
            {
                timer3.Interval = 100;
                timer3.Enabled = false;
                labelEnviando.Text = "Enviando";
                labelEnviando.Visible = false;
                BotProcesCarg.Enabled = true;
                BotEnviarCarga.Enabled = true;
                BotActPanPC.Enabled = true;
                btnCerrar.Enabled = true;
                ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
                shellView2.CurrentFolder = folder;
            }
            
            
        }

        private void BotExpCol_Click(object sender, EventArgs e)
        {
           
        }

        private void radioButSinPro_CheckedChanged(object sender, EventArgs e)
        {


            if (radioButSinPro.Checked == true)
            {
                InfoTipoPro.Visible = true;
                InfoTipoPro.Text = "No tendrá en cuenta ningun acontecimiento extraño en las fechas de toma de lectura, \n"+ 
                                    "y no se aplica ningun cambio a la hora de calcular la factura";
            }
        }

        private void radioButProrLim_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButProrLim.Checked == true)
            {
                InfoTipoPro.Visible = true;
                InfoTipoPro.Text = "Se aplica cuando se reprograma la fecha de lectura, ya sea por cambio de remesa \n " +
                                   "u otro motivo. Esto hará que se facture el total del consumo, se tome la fecha \n" +
                                   "actual como fecha de lectura, pero se modificarán los rangos de precios en \n" +
                                   "proporción a la variación de la cantidad de días. \n";
            }
        }

        private void radioButProBasYfec_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButProBasYfec.Checked == true)
            {
                InfoTipoPro.Visible = true;
                InfoTipoPro.Text = "Se aplica cuando la fecha de lectura real es distinta a la programada, debido a \n" +
                                   "acontecimientos fortuitos(mal tiempo, licencias), y se mantiene la fecha programada \n" +
                                   "de lectura para periodos posteriores. Esto hará que se prorratee el consumo a la \n" +
                                   "cantidad de días que debe tener el periodo, y se considerará como fecha de lectura\n" +                                   
                                   "la fecha programada para cierre de periodo. ";
            }
        }

        private void BotDetenerProcCarga_Click(object sender, EventArgs e)
        {
            //backgroundProcesarCarga.CancelAsync();
            DetenerTareaSegundoPlano(backgroundProcesarCarga);
            //BotDetenerProcCarga.Visible = false;
        }

        private void BtnCerrarUsuarios_Click(object sender, EventArgs e)
        {
            if (Vble.TablaConexSelec.Rows.Count > 0)
            {

            

            Int32 codconex, Periodo, ImpresionOBS;
            //recorre cada registro de los nodos seleccionados    
            //foreach (DataGridViewRow Fila in dataGridView1.Rows)
            foreach (DataRow Fila in Vble.TablaConexSelec.Rows)
            //foreach (DataRow Fila in Tabla.Rows)
            {

                codconex = (Int32)Fila["ConexionID"];
                Periodo = (Int32)Fila["Periodo"];
                ImpresionOBS = (Int32)Fila["ImpresionOBS"];

                if (Math.Floor(Math.Log10(ImpresionOBS) + 1) == 3)
                {
                    if (ImpresionOBS.ToString().Substring(0,1) == "5")
                    {
                        //envio 5 porque en la funcion se le suma los 300 para que quede con la marca de 800 que es devuelto de FIS (FIS Y almacen)
                        Vble.CambiarEstadoConexionMySql(codconex, 5, Periodo);
                    }
                   
                    //----------------------Cambia el estado impresionOBS a Devuelto por FIS para que se considere a la hora de hacer la exportacion
                  
                }
                else if (ImpresionOBS.ToString() == "0")
                    {
                        Vble.CambiarEstadoConexionMySql(codconex, 0, Periodo);
                    }
            }

            LimpiarPanelCargasAenviar();
            CargasProcesadas();
            CargarRutasDisponibles();
            LeeCargasEnviadas();
            LeerCargasRecibidas();

            timer1_Tick(sender, e);
            timer2.Start();
                

            MessageBox.Show("Se han cerrado los saldos para poder exportar", "Cierre de saldos", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se seleccionó ninguna ruta para cerrar saldos" , "Cierre de saldos", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //timer1_Tick(sender, e);
        }

        private void cmbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void RBEnvWifi_CheckedChanged(object sender, EventArgs e)
        {
            if (RBEnvCable.Checked == true)
            {
                cmbDevices.Visible = true;
                cmbDevicesWifi.Visible = false;
            }
            else if (RBEnvWifi.Checked == true)
            {
                cmbDevices.Visible = false;
                cmbDevicesWifi.Visible = true;
            }
            
        }

        private void RBEnvCable_CheckedChanged(object sender, EventArgs e)
        {
            if (RBEnvCable.Checked == true)
            {
                cmbDevices.Visible = true;
                cmbDevicesWifi.Visible = false;
            }
            else if (RBEnvWifi.Checked == true)
            {
                cmbDevices.Visible = false;
                cmbDevicesWifi.Visible = true;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (LabNomColect.Text == "")
            {
                ListViewColectora.Visible = false;
            }
            timer4.Stop();
        }

        private void splitContainer5_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ListViewColectora_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbDevicesWifi_SelectionChangeCommitted(object sender, EventArgs e)
        {
            shellView2.Visible = true;
            ListViewColectora.Visible = true;
        }

        private void PickBoxLoading_Click(object sender, EventArgs e)
        {

        }

        private void verEstadosDeRutasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEstadosRutas EstadoRutas = new FormEstadosRutas();           
            string input = Interaction.InputBox("Ingrese la remesa que está trabajando", "REMESA", "0", 550, 300);
            if (input != "")
            {
                EstadoRutas.Remesa = input;
                EstadoRutas.Show();
            }
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cmbDevices_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private async void button1_Click_3(object sender, EventArgs e)
        {
            bool habilitado = false;
            string dni = "";
            string pass = "";

            

            if (cmbDevices.Text == "")
            {
                MessageBox.Show("Discupe no hay colectoras conectadas para efectuar el borrado de ruta", "Sin Colectora", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //FormLogeoPermisos credenciales = new FormLogeoPermisos();
                //credenciales.Show();
                //await credenciales.VerificarPermiso();
                dni = Interaction.InputBox("Por favor ingrese el DNI para autorizar el borrado de la ruta que se encuentra en la colectora", "DNI", "0", 550, 300);
                //pass = Interaction.InputBox("Por favor ingrese la CONTRASEÑA para autorizar el borrado de la ruta que se encuentra en la colectora", "CONTRASEÑA", "0", 550, 300);
               
                if (Vble.dniBorrado != "" && Vble.passBorrado != "")
                {            
                    try
                    { 
                        string txSQL = "SELECT * FROM lecturistas WHERE Codigo = " + dni + " AND (Privilegio = 'RUTISTA' OR Privilegio = 'DESARROLLADOR')";

                        MySqlCommand da = new MySqlCommand(txSQL, DB.conexBD);
                        
                        int count = Convert.ToInt32(da.ExecuteScalar());

                        if (count == 0)
                            habilitado = false;
                        //retorno = false;
                        else
                            habilitado = true;
                        //retorno = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " Erro al verificar Persona en consulta a la base de datos.");
                    }


                    if (habilitado == true)
                    {
                        //string Temporal = @"C:/Users/" + Environment.UserName + "/Documents/Temporal";
                        string RespaldoBeforClear = @""+Vble.CarpetaBorradoRutas + "\\"+ DateTime.Today.Date.ToString("yyyyMMdd") + "\\" + cmbDevices.Text;
                        //string Temporal = @"C:\Users\Usuario\Documents\Informes PDF";
                        if (!Directory.Exists(RespaldoBeforClear))
                        {
                            Directory.CreateDirectory(RespaldoBeforClear);
                        }
                        DirectoryInfo ArchivosEnTemporal = new DirectoryInfo(RespaldoBeforClear);
                        string direcColec = "";
                        try
                        {
                            //Conecto el dispositivo para verificar si existen archivos o Ruta cargada, asi podrá mandar una nueva carga 
                            //a la colectora o no.
                            StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
                            IList<WindowsPortableDevice> devices = services.Devices;
                            devices.ToList().ForEach(device =>
                            {
                                device.Connect();

                                if (Funciones.BuscarColectora(device.ToString()))
                                {
                                    var rootFolder = device.GetContents().Files;
                                    foreach (var item in rootFolder)
                                    {
                                        if (item.Name == "Almacenamiento interno compartido")
                                        {
                                            currentFolder = device.GetContents((PortableDeviceFolder)item);
                                            var carpetas = currentFolder.Files;
                                            direcColec = "Este equipo/" + device.FriendlyName + "/" + item.Name;
                                            foreach (var CarpetaDatosDpec in carpetas)
                                            {
                                                if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                                                {
                                                    CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                                                    var Archivos = CarpetaDatosDPEC.Files;
                                                    direcColec +=  "/" + CarpetaDatosDPEC.Name;
                                                    //Pregunto si existen archivos antes de enviar la nueva carga, 
                                                    //si existe elimina lo que habia para poder copiar la nueva carga
                                                    //si no existen archivos pasa el if y no elimina nada.
                                                    if (Archivos.Count > 0)
                                                    {
                                                        foreach (PortableDeviceFile arc in Archivos)
                                                        {
                                                            device.GetFile(arc, RespaldoBeforClear + "/");
                                                            //device.TransferContentToDevice(arc.ToString(), Temporal + "\\");
                                                            //direcColec += "/" + arc.Name.Remove(arc.Name.IndexOf("."));
                                                            device.DeleteFile(arc);
                                                            //                                                          
                                                            //File.Copy(direcColec, Temporal);                                                            
                                                        }
                                                    }
                                                    ////Este For se encarga de cargar los archivos procesados con la nueva carga
                                                    ////que se encuentran almacenados en un arraylist, en donde cada elemento
                                                    ////contiene el nombre del archivo a enviar con la ubicación del mismo
                                                    //for (int i = 0; i < ArchivosProcesados.Count; i++)
                                                    //{
                                                    //    device.TransferContentToDevice(ArchivosProcesados[i].ToString(), CarpetaDatosDpec.Id);
                                                    //}
                                                }
                                            }
                                        }
                                    }

                                    MessageBox.Show("Se borraron los archivos de FIS que se encontraban en la colectora", "Borrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Ctte.ArchivoLog.EscribirLog("Borrado de Archivos en Colectora, realizado por el DNI: " + dni);
                                    device.Disconnect();

                                }
                            });

                            //////Pregunto si es igual a 3 porque si no hay 3 archivos que son las dos bases y el archivo InforCarga.txt es porque
                            //////la colectora no esta cargada correctamente y no podré hacer la consulta sobre que ruta esta cargada en la colectora
                            //if (ArchivosEnTemporal.GetFiles().Length == 3)
                            //{
                            //    foreach (var ArchivoBase in ArchivosEnTemporal.GetFiles())
                            //    {
                            //        if (ArchivoBase.Name == Vble.NombreArchivoBaseSqlite())
                            //        {
                            //            foreach (var ArchivoInfo in ArchivosEnTemporal.GetFiles())
                            //            {
                            //                if (ArchivoInfo.Name == "InfoCarga.txt")
                            //                {
                            //                    MessageBox.Show("La Colectora ya contiene la Carga: \n" + Funciones.LeerArchivostxt(ArchivoInfo.FullName) + " Por favor primero realice la descarga para volver a enviar otro Archivo",
                            //                                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //                    ActualizaListViewColectora(Temporal);
                            //                    shellView2.Visible = true;
                            //                    ListViewColectora.Visible = true;
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            ActualizaListViewColectora(Temporal);
                            //            shellView2.Visible = true;
                            //            ListViewColectora.Visible = true;
                            //        }
                            //    }
                            //}
                            //else if (ArchivosEnTemporal.GetFiles().Length == 1 || ArchivosEnTemporal.GetFiles().Length == 0)
                            //{
                            //    ActualizaListViewColectora(Temporal);
                            //    shellView2.Visible = true;
                            //    ListViewColectora.Visible = true;
                            //    foreach (var Archivos in ArchivosEnTemporal.GetFiles())
                            //    {
                            //        if (Archivos.Name == Vble.NombreArchivoBaseFijaSqlite())
                            //        {
                            //            ActualizaListViewColectora(Temporal);
                            //            shellView2.Visible = true;
                            //            ListViewColectora.Visible = true;
                            //        }
                            //    }
                            //}
                            //if (Directory.Exists(Temporal))
                            //{
                            //    Directory.Delete(Temporal, true);
                            //}
                        }
                        catch (Exception r)
                        {
                            MessageBox.Show(r.Message + " Error al seleccionar Colectora");
                        }
                    }
                    else
                    {
                        MessageBox.Show("La crendencial introducida no es correcta", "AUTORIZACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }            
      }
    }
