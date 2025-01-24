/*
 * Creado por SharpDevelop.
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 13:53
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Ionic.Zip;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.IO;


using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SQLite;
using System.Globalization;
using Microsoft.VisualBasic.Devices;
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
using WPDSpLib;


using static gagFIS_Interfase.Form4Cargas;
using System.Threading.Tasks;

namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form2Exportar.
    /// </summary>
    public partial class Form2Exportar : Form
    {
        //---------------------------------------------------------------------------------------
        //Declaracion de Variables locales de la Clase Exportar o crear Upload
        //---------------------------------------------------------------------------------------
        ArrayList ArrayLotes = new ArrayList();
        //private static int QueTimer;
        private Dictionary<string, clInfoNodos> dcNodos = new Dictionary<string, clInfoNodos>();
        public static DataTable TablaUpload1 = new DataTable();
        public static DataTable TablaUploadCantidadRutas = new DataTable();
        public static DataTable TablaUploadNoved = new DataTable();
        /// <summary>
        /// Contendra datos de la consulta a las tablas conexiones y medidores teniendo en cuenta los parametros
        /// lote, periodo y distrito
        /// </summary>
        public static DataTable TablaUpload2 = new DataTable();
        public static string CONSULTA = "";
        public static string REMESA = "";

        /// <summary>
        /// Contendra el usuarioID o TitularID de la tabla conexiones del cual fueron Leidos NO Impresos
        /// </summary>
        public static DataTable TablaUploadLeidosNoImpresos = new DataTable();
        public static DataTable TablaConecptosFacturados = new DataTable();
        public static ArrayList ArrayCodNoved = new ArrayList();
        
        public static string[] ArrayCodNovedades = new string[6];
        public static string[] ArrayDescrNovedades = new string[6];
        /// <summary>
        /// Variable que contendrá el nombre del archivo que se está exportando al momento de hacer clic en generar exportación
        /// el mismo nombre tambien se guarda en el archivo Log Exportaciones
        /// </summary>
        public static string ArchivoPLANO;
        public static string ruta = "";
        public static string FiltroDESDE = "";
        public static string FiltroHASTA = "";

        ArrayList ArrayLocalidad = new ArrayList();
        ArrayList ArrayRemesa = new ArrayList();
        ArrayList ArrayRuta = new ArrayList();
        ArrayList ArrayRemesaRuta = new ArrayList();
        //ArrayList ArrayRuta = new ArrayList();

        public int ñ = 0;

        public static DataTable Tabla = new DataTable();


        public Form2Exportar()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private async void Form2_Load(object sender, System.EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.tvlotes.Nodes.Clear();
            UpPorRuta.Checked = true;
            CheckForIllegalCrossThreadCalls = false;
            Image Im1;
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Todo.gif");
            imgList1.Images.Add("Todo", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Nada.gif");
            imgList1.Images.Add("Nada", Im1);
            Im1 = null;

            toolTip1.SetToolTip(cbLote, "Muestra las conexiones clasificadas por lote para su seleccion y Exportación");
            toolTip1.SetToolTip(cbTodo, "Exportará todas las Rutas, teniendo en cuenta la opcion seleccionada de Saldos");
            
            //QueTimer = 1;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            RadioButtonNO.Checked = true;
            
            //CargarTVRutasExportadas();
            //BGWEsperar.RunWorkerAsync();
            PickBoxLoading.Visible = true;
            Task oTask = new Task(ResumenExportadas);
            oTask.Start();
            await oTask;

            RBRuta.Checked = true;
            StringBuilder stb = new StringBuilder("", 250);
            Inis.GetPrivateProfileString("Datos", "CambiarFecha", "", stb, 250, Ctte.ArchivoIniName);
            Inis.GetPrivateProfileString("Datos", "VerLogExportacon", "0", Vble.PanelLogExp, 10, Ctte.ArchivoIniName);


            if (stb.ToString() == "1")
            {
                CheckCambiarFechaLectura.Visible = true;
            }
            else
            {
                CheckCambiarFechaLectura.Visible = false;
            }
            
        }

        /// <summary>
        /// El metodo resumen exportadas llama al metodo CargarTVRutasExportadas el cual mostraria en el Treeview TVRutasExportadas
        /// las rutas que contengan algun usuario de su totalidad que ya se encuentran exportadas.
        /// ****en desarrollo la posibilidad de ver por fecha en caso de que la ruta haya salido en mas de un dia a leer.
        /// </summary>
        private void ResumenExportadas()
        {
            CargarTVRutasExportadas();
            CBFiltroZona.Items.Clear();
            foreach (var item in Vble.ArrayZona)
            {
                CBFiltroZona.Items.Add(item.ToString());
            }
            CBFiltroZona.Text = CBFiltroZona.Items[0].ToString();
            //PickBoxLoading.Visible = false;
        }

        /// <summary>
        /// metodo que contiene los metodos que no se pueden ejecutar en segundo plano porque no fueron creados
        /// dentro del mismo proceso de Segundo Plano
        /// </summary>
        public void InvokeMethod1()
        {
            AgregarNodoEmpresaExportadas(Vble.Empresa, "logo");


            //Recorre la tabla y carga las ramas del arbol
            foreach (DataRow Fila in Tabla.Rows)
            {           
                Vble.Distrito = Fila.Field<int>("Zona");
                if (Vble.Distrito < 100) Vble.Distrito += 200;
                Vble.Lote = Fila.Field<int>("Lote");
                Vble.Remesa = Fila.Field<int>("Remesa");
                Vble.Ruta = Fila.Field<int>("Ruta");


                if (AgregarNodoDistritoExportadas(Vble.Empresa, Vble.Distrito))
                    if (AgregarNodoRemesaExportadas(Vble.Empresa, Vble.Distrito, Vble.Lote, Vble.Remesa))
                        AgregarNodoRutaExportadas(Vble.Empresa, Vble.Distrito, Vble.Lote, Vble.Ruta, Vble.Remesa);
                //AgregarNodoParticionA(Vble.Empresa, Distr, Rem, Rut, Par, Sec);

                //BeginInvoke(new InvokeDelegate(InvokeMethod2));

            }

            tvExportadas.Nodes[Vble.Empresa.ToLower()].ExpandAll();
            //TomarEstadoDeHijos(tvlotes.Nodes["dpec"]);                
            tvExportadas.Sort();

            //BeginInvoke(new InvokeDelegate(InvokeMethod3));
            ////tvExportadas.Nodes[Vble.Empresa.ToLower()].ExpandAll();
            //////TomarEstadoDeHijos(tvlotes.Nodes["dpec"]);                
            ////tvExportadas.Sort();

        }

        /// <summary>
        /// metodo que contiene los metodos que no se pueden ejecutar en segundo plano porque no fueron creados
        /// dentro del mismo proceso de Segundo Plano
        /// </summary>
        public void InvokeMethod2()
        {

            if (AgregarNodoDistritoExportadas(Vble.Empresa, Vble.Distrito))
                if (AgregarNodoRemesaExportadas(Vble.Empresa, Vble.Distrito, Vble.Lote, Vble.Remesa))
                    AgregarNodoRutaExportadas(Vble.Empresa, Vble.Distrito, Vble.Lote, Vble.Ruta, Vble.Remesa);

        }

        /// <summary>
        /// metodo que contiene los metodos que no se pueden ejecutar en segundo plano porque no fueron creados
        /// dentro del mismo proceso de Segundo Plano
        /// </summary>
        public void InvokeMethod3()
        {
            tvExportadas.Nodes[Vble.Empresa.ToLower()].ExpandAll();
            //TomarEstadoDeHijos(tvlotes.Nodes["dpec"]);                
            tvExportadas.Sort();
        }


        /// <summary>
        /// metodo que contiene los metodos que no se pueden ejecutar en segundo plano porque no fueron creados
        /// dentro del mismo proceso de Segundo Plano
        /// </summary>
        public void InvokeMethod4()
        {
            
        }



        private void CargarTreeviewXLotes()
        {
            TablaUpload1.Clear();
            TablaUpload2.Clear();
            //TablaUploadLeidosNoImpresos.Clear();
            //CargarListaRutas();
            CargarRutasParaExportar();
        }





        #region Metodos



        /// <summary>
        /// Método para comprimir una lista de archivos, enviando solo la ruta donde se encuentran, no importa que no haya solo imagenes, el metodo le aplica un filtro.
        /// </summary>
        /// <param name="RutaDeArchivos">Ruta de donde se encuentran las imagenes.
        /// <param name="RutaGuardar">Ruta donde se guardara el archivo Zip
        /// <returns></returns>
        static public Boolean ComprimirListaDeArchivos(string RutaDeArchivos, string RutaGuardar)
        {
            try
            {
                String[] NombreArchivos = Directory.GetFiles(RutaDeArchivos, "*.btx*");

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(NombreArchivos, "");
                    zip.Save(RutaGuardar);
                    zip.Password = "1a1b1c";
                }
                return true;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
                return false;
            }
        }

        /// <summary>
        /// Método utilizado para comprimir archivos
        /// </summary>
        /// <param name="Ruta">Ruta del folder que se desea comprimir. (Server.MapPath(...))
        /// <param name="Nombre">el nombre con el que se desea que se guarde el archivo generado. Sin la extension
        /// <returns></returns>
        static public Boolean ComprimirArchivo(string Ruta, string Nombre)
        {

            try
            {
                String[] NombreArchivos = Directory.GetFiles(Ruta, "*.btx*");

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(NombreArchivos, "");

                    zip.Save(Nombre + ".zip");

                }

                return true;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
                return false;

            }

        }

        /// <summary> Devuelve el nombre de ruta cuando se le pasa una cadena que contiene nombres
        /// de Variables de Sistema, tales como 'Distrito', 'Zona', 'Periodo', etc. El parámetro debe 
        /// seguir normas en su sintaxis, y cada variable es remplazada por su valor vigente al
        /// momento de procesar, y de acuerdo con el formato solicitado dentro del parametro
        /// </summary>
        /// <param name="Ruta">Nombre de la ruta con las variables y formatos incluidos.</param>
        /// <returns>Devuelve el nombre de la ruta con las variables remplazadas por su valor.</returns>
        public static string ValorarUnNombreRuta(string Ruta)
        {
            string VarNom = "";
            List<string> VarVal = new List<string>();


            //ArchivoTabla = Vble.CarpetaTrabajo + "\\" + Vble.CarpetaCargasNoEnviadas + "\\" + SubCarp + "\\";

            //remplaza las variables dentro de la cadena
            int i1, i2, i3;  // i1:'{'  -  i2:';'  -  i3:'}'
            i1 = Ruta.IndexOf("{");
            while (i1 >= 0)
            {
                i3 = Ruta.IndexOf("}", i1);       //Busca cierre llave
                if (i3 > i1)
                {
                    i2 = Ruta.IndexOf(":", i1, i3 - i1);  //Busca dos puntos
                    if (i2 < i1) i2 = i3;
                    VarNom = Ruta.Substring(i1 + 1, i2 - i1 - 1);
                    VarVal.Add(VarNom);

                    Ruta = Ruta.Substring(0, i1 + 1) +
                            (VarVal.Count - 1).ToString().Trim() +
                            Ruta.Substring(i2);
                }
                i1 = Ruta.IndexOf("{", i1 + 1);
            }  //Hasta aca se tiene la cadena de formato
               // MessageBox.Show(VarVal[0]);
               //Generar el arreglo con los datos de remplazo
            object[] VarDat = new object[VarVal.Count];
            #region For_Variable
            for (int i = 0; i < VarVal.Count; i++)
            {
                switch (VarVal[i].Trim().ToUpper())
                {
                    case "EMPRESA":
                        VarDat[i] = Vble.Empresa;
                        break;
                    case "PERIODO":
                        VarDat[i] = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM", CultureInfo.CurrentCulture);
                        break;
                    case "AHORA":
                        VarDat[i] = DateTime.Now;
                        break;
                    case "LOTE":
                        VarDat[i] = Vble.Lote.ToString("00000000");
                        break;
                    case "ZONA":
                        //VarDat[i] = Convert.ToInt16(Vble.ArrayZona[0]).ToString("000");
                        VarDat[i] = Vble.Distrito.ToString("000");
                        break;
                    case "DISTRITO":
                        VarDat[i] = Vble.Distrito.ToString("000");
                        break;
                    case "REMESA":
                        VarDat[i] = Vble.Remesa;
                        break;
                    case "RUTA":
                        VarDat[i] = Vble.Ruta.ToString("D");
                        break;
                    case "DOCUMENTOS EN MICC-":
                        VarDat[i] = Vble.Colectora;
                        break;
                    case "FECHA":
                        VarDat[i] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        break;
                    default:
                        VarDat[i] = "";
                        break;

                }
            }
            #endregion For_Variable

            return string.Format(Ruta, VarDat);
        }


        #endregion Métodos



        ///**********************
        ///Region de Funciones
        //*********************
        #region Funciones



        


        /// <summary>
        /// Carga Treeview tvRutasExportadas para saber que rutas contienen de importacion osea 6xx pertenecientes 
        /// al periodo en le que se esta posicionado.
        /// </summary>
        /// <returns></returns>
        public bool CargarTVRutasExportadas()
        {            
            bool retorno = true;
            //DataTable Tabla;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            int Distr, Lote, Rut, Sec, Rem;
            string Par;

            try
            {
                ///Tomo las localidades pertenecientes a la interfaz que se esta trabajando para ver a la hora de cargar las conexiones a colectora
                LeerArchivoZonaFIS();

                //Lee la tabla conexiones del periodo y sin leer
                //txSQL = "SELECT C.ConexionID, C.Zona, C.Lote, " +
                //            "C.Ruta, C.Secuencia, I.Particion, C.ImpresionOBS, C.Remesa" +
                //        " FROM Conexiones C" +
                //        " JOIN infoconex I using (ConexionID, Periodo) " +
                //        " WHERE (C.ConexionID = I.ConexionID" +
                //        " AND C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND ((C.ImpresionOBS >= 600) AND (C.ImpresionOBS < 800)) " +
                //        ") ORDER BY C.Zona, C.Remesa, C.Ruta, C.Secuencia";

                txSQL = "SELECT DISTINCT C.Zona, C.Periodo, C.Lote, " +
                           "C.Ruta, I.Particion, C.Remesa" +
                       " FROM Conexiones C" +
                       " JOIN infoconex I using (ConexionID, Periodo) " +
                       " WHERE (C.ConexionID = I.ConexionID" +
                       " AND C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND ((C.ImpresionOBS >= 600) AND (C.ImpresionOBS < 800)) " +
                       ") ORDER BY C.Zona, C.Remesa, C.Ruta, C.Secuencia";


                Tabla = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                tvExportadas.Nodes.Clear();
                dcNodos.Clear();

                BeginInvoke(new InvokeDelegate(InvokeMethod1));

                //AgregarNodoEmpresaExportadas(Vble.Empresa, "logo");


                ////Recorre la tabla y carga las ramas del arbol
                //foreach (DataRow Fila in Tabla.Rows)
                //{
                //    Distr = Fila.Field<int>("Zona");
                //    if (Distr < 100) Distr += 200;
                //    Lote = Fila.Field<int>("Lote");
                //    Rut = Fila.Field<int>("Ruta");
                //    //Sec = Fila.Field<int>("Secuencia");
                //    //Par = Fila.Field<string>("particion");
                //    Rem = Fila.Field<int>("Remesa");

                //    Vble.Distrito = Distr;
                //    Vble.Lote = Lote;
                //    Vble.Remesa = Rem;
                //    Vble.Ruta = Rut;


                //    //if (AgregarNodoDistritoExportadas(Vble.Empresa, Distr))
                //    //    if (AgregarNodoRemesaExportadas(Vble.Empresa, Distr, Lote, Rem))
                //    //        AgregarNodoRutaExportadas(Vble.Empresa, Distr, Lote, Rut, Rem);
                //    ////AgregarNodoParticionA(Vble.Empresa, Distr, Rem, Rut, Par, Sec);

                //    BeginInvoke(new InvokeDelegate(InvokeMethod2));
                 
                //}

                BeginInvoke(new InvokeDelegate(InvokeMethod3));
                //tvExportadas.Nodes[Vble.Empresa.ToLower()].ExpandAll();
                ////TomarEstadoDeHijos(tvlotes.Nodes["dpec"]);                
                //tvExportadas.Sort();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "- en: " + e.TargetSite.Name);
                retorno = false;
            }

            //BloqueoClick = false;
            PickBoxLoading.Visible = false;
            cbLote.Enabled = true;
            return retorno;
        }



        /// <summary>
        /// Carga Treeview al seleccionar Exportación por Lotes para poder seleccionar algunas conexiones y exportar solo las seleccionadas.
        /// </summary>
        /// <returns></returns>
        private bool CargarListaRutas()
        {
            bool retorno = true;
            //DataTable Tabla;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            int Distr, Lote, Rut, Sec, Rem;
            string Par;

            try
            {
                ///Tomo las localidades pertenecientes a la interfaz que se esta trabajando para ver a la hora de cargar las conexiones a colectora
                LeerArchivoZonaFIS();

                //Lee la tabla conexiones del periodo y sin leer
                txSQL = "SELECT C.ConexionID, C.Zona, C.Lote, " +
                            "C.Ruta, C.Secuencia, I.Particion, C.ImpresionOBS, C.Remesa" +
                        " FROM Conexiones C, infoconex I" +
                        " WHERE (C.ConexionID = I.ConexionID " +
                        " AND C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND ((C.ImpresionOBS > 500 AND C.ImpresionOBS < 600)) " +// OR(C.ImpresionOBS = 800)) " +
                        ") ORDER BY C.Zona, C.Remesa, C.Ruta, C.Secuencia";

                DataTable Tabla = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                tvlotes.Nodes.Clear();
                dcNodos.Clear();
                AgregarNodoEmpresa(Vble.Empresa, "logo");
                

                //Recorre la tabla y carga las ramas del arbol
                foreach (DataRow Fila in Tabla.Rows)
                {
                    Distr = Fila.Field<int>("Zona");
                    if (Distr < 100) Distr += 200;
                    Lote = Fila.Field<int>("Lote");
                    Rut = Fila.Field<int>("Ruta");
                    Sec = Fila.Field<int>("Secuencia");
                    Par = Fila.Field<string>("Particion");
                    Rem = Fila.Field<int>("Remesa");

                    //MessageBox.Show(Vble.Empresa);
                    if (AgregarNodoDistrito(Vble.Empresa, Distr))
                        //if (AgregarNodoRemesa(Vble.Empresa, Distr, Lote))
                        if (AgregarNodoRemesa(Vble.Empresa, Distr, Rem))
                            AgregarNodoRuta(Vble.Empresa, Distr, Rem, Rut);
                    //AgregarNodoRuta(Vble.Empresa, Distr, Lote, Rut);
                    //            AgregarNodoParticionA(Vble.Empresa, Distr, Rem, Rut, Par, Sec);


                }
                tvlotes.Nodes[Vble.Empresa.ToLower()].ExpandAll();
                //TomarEstadoDeHijos(tvlotes.Nodes["dpec"]);                
                tvlotes.Sort();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "- en: " + e.TargetSite.Name);
                retorno = false;
            }

            //BloqueoClick = false;
            return retorno;
        }


        /// <summary>
        /// Carga Treeview al seleccionar Exportación por Lotes para poder seleccionar algunas conexiones y exportar solo las seleccionadas.
        /// </summary>
        /// <returns></returns>
        private bool CargarRutasParaExportar()
        {
            bool retorno = true;
            //DataTable Tabla;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            int Distr, Lote, Rut, Sec, Rem;
            string Par;

            try
            {
                ///Tomo las localidades pertenecientes a la interfaz que se esta trabajando para ver a la hora de cargar las conexiones a colectora
                LeerArchivoZonaFIS();

                //Lee la tabla conexiones del periodo y sin leer
                txSQL = "SELECT C.ConexionID, C.Zona, C.Lote," +
                        " C.Ruta, C.Secuencia, C.ImpresionOBS, C.Remesa" +
                        " FROM Conexiones C" +
                        //" WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND (C.ImpresionOBS > 500 AND C.ImpresionOBS < 600 AND C.ImpresionOBS <> 517) " +// OR(C.ImpresionOBS = 800)) " +
                        " WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND (C.ImpresionOBS > 500 AND C.ImpresionOBS < 600) " +// OR(C.ImpresionOBS = 800)) " +
                        " ORDER BY C.Zona, C.Remesa, C.Ruta, C.Secuencia";

                DataTable Tabla = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                tvlotes.Nodes.Clear();
                dcNodos.Clear();
                AgregarNodoEmpresa(Vble.Empresa, "logo");

                //Recorre la tabla y carga las ramas del arbol
                foreach (DataRow Fila in Tabla.Rows)
                {
                    Distr = Fila.Field<int>("Zona");
                    if (Distr < 100) Distr += 200;
                    Lote = Fila.Field<int>("Lote");
                    Rut = Fila.Field<int>("Ruta");
                    Sec = Fila.Field<int>("Secuencia");
                    //Par = Fila.Field<string>("Particion");
                    Rem = Fila.Field<int>("Remesa");

                    //MessageBox.Show(Vble.Empresa);
                    if (AgregarNodoDistrito(Vble.Empresa, Distr))
                        //if (AgregarNodoRemesa(Vble.Empresa, Distr, Lote))
                        if (AgregarNodoRemesa(Vble.Empresa, Distr, Rem))
                            AgregarNodoRuta(Vble.Empresa, Distr, Rem, Rut);
                    //AgregarNodoRuta(Vble.Empresa, Distr, Lote, Rut);
                    //            AgregarNodoParticionA(Vble.Empresa, Distr, Rem, Rut, Par, Sec);


                }
                tvlotes.Nodes[Vble.Empresa.ToLower()].ExpandAll();
                //TomarEstadoDeHijos(tvlotes.Nodes["dpec"]);                
                tvlotes.Sort();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "- en: " + e.TargetSite.Name);
                retorno = false;
            }

            //BloqueoClick = false;
            return retorno;
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
        private bool AgregarNodoEmpresa(string NombreEmpresa, string keyLogo)
        {
            string sClave = NombreEmpresa.ToLower().Trim();
            clInfoNodos tiN = new clInfoNodos();

            // Verificar si ya está el nodo
            if (!tvlotes.Nodes.ContainsKey(sClave))
            {
                //No está el nodo, lo agrega
                tvlotes.Nodes.Add(sClave, NombreEmpresa, keyLogo);
                tvlotes.Nodes[sClave].Expand();
                tvlotes.Nodes[sClave].Tag = sClave;
                tiN.Texto = NombreEmpresa;
                tiN.Key = sClave;
                tiN.ImageKey = "nada";
                dcNodos.Add(sClave, tiN);

                tvlotes.Nodes[sClave].Expand();
            }

            // Verifica que se haya añadido correctamente
            return tvlotes.Nodes.ContainsKey(sClave);
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
        private bool AgregarNodoEmpresaExportadas(string NombreEmpresa, string keyLogo)
        {
            string sClave = NombreEmpresa.ToLower().Trim();
            clInfoNodos tiN = new clInfoNodos();
         
            if (!tvExportadas.Nodes.ContainsKey(sClave))
            {
                //No está el nodo, lo agrega
                tvExportadas.Nodes.Add(sClave, NombreEmpresa, keyLogo);
                tvExportadas.Nodes[sClave].Expand();
                tvExportadas.Nodes[sClave].Tag = sClave;
                tiN.Texto = NombreEmpresa;
                tiN.Key = sClave;
                tiN.ImageKey = "nada";
                if (dcNodos.ContainsKey(sClave))
                {

                }
                else
                {
                    dcNodos.Add(sClave, tiN);
                }

                tvExportadas.Nodes[sClave].Expand();
            }
            // Verifica que se haya añadido correctamente
            return tvExportadas.Nodes.ContainsKey(sClave);
        }

        public class Delegado1
        {           
           
        }


        /// <summary>Verifica que el nodo distrito, según el número de zona, que se  
        /// corresponde con el código interno de localidad, esté cargado, si no está 
        /// cargado, lo agrega.
        /// </summary>
        /// <param name="sEmpresa">Empresa para la que se buscarán los nodos (DPEC siempre)</param>
        /// <param name="Distrito">Número de zona, a la quew corresponde la localidad o distrito</param>
        /// <returns>Devuelve la cantidad de nodos agregados</returns>
        private bool AgregarNodoDistrito(string sEmpresa, int Distrito)
        {
            string sKey, sD, Loc;
            clInfoNodos tn = new clInfoNodos();
            int i;
            DataTable tabZona;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;

            try
            {
                //Obtener la Key de la rama superior (padre)


                if (Distrito < 100) Distrito += 200;
                sD = Distrito.ToString().Trim();

                sKey = Vble.Empresa.ToLower() + sD;

                if (!tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey))
                {
                    //No está el distrito, debe agregarlo
                    txSQL = "SELECT  * FROM Localidades " +
                       "WHERE codigoInt=" + sD;
                    tabZona = new DataTable();
                    datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(tabZona);
                    Loc = "-";
                    if (tabZona.Rows.Count > 0)
                        Loc = tabZona.Rows[0].Field<string>("localidad");
                    tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes.Add(sKey, sD + " - " + Vble.LetraCapital(Loc.Trim()), "nada");
                    tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Tag = sKey;

                    tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Expand();
                    i = tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Index;
                    tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].BackColor = Color.AliceBlue;

                    tn.Texto = sD.ToUpperInvariant();
                    tn.Key = sKey.ToLowerInvariant();
                    tn.Distrito = Distrito;

                    tn.ImageKey = "todo";
                    dcNodos.Add(sKey, tn);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return tvlotes.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey);
        }


        /// <summary>Verifica que el nodo distrito, según el número de zona, que se  
        /// corresponde con el código interno de localidad, esté cargado, si no está 
        /// cargado, lo agrega.
        /// </summary>
        /// <param name="sEmpresa">Empresa para la que se buscarán los nodos (DPEC siempre)</param>
        /// <param name="Distrito">Número de zona, a la quew corresponde la localidad o distrito</param>
        /// <returns>Devuelve la cantidad de nodos agregados</returns>
        private bool AgregarNodoDistritoExportadas(string sEmpresa, int Distrito)
        {
            string sKey, sD, Loc;
            clInfoNodos tn = new clInfoNodos();
            int i;
            DataTable tabZona;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;

            try
            {
                //Obtener la Key de la rama superior (padre)
                if (Distrito < 100) Distrito += 200;
                sD = Distrito.ToString().Trim();

                sKey = Vble.Empresa.ToLower() + sD;

                if (!tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey))
                {
                    //No está el distrito, debe agregarlo
                    txSQL = "SELECT  * FROM Localidades " +
                       "WHERE CodigoInt=" + sD;
                    tabZona = new DataTable();
                    datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(tabZona);
                    Loc = "-";
                    if (tabZona.Rows.Count > 0)
                        Loc = tabZona.Rows[0].Field<string>("localidad");
                    tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes.Add(sKey, sD + " - " + Vble.LetraCapital(Loc.Trim()), "nada");
                    tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Tag = sKey;

                    tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Expand();
                    i = tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].Index;
                    tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes[sKey].BackColor = Color.AliceBlue;

                    tn.Texto = sD.ToUpperInvariant();
                    tn.Key = sKey.ToLowerInvariant();
                    tn.Distrito = Distrito;

                    tn.ImageKey = "todo";

                    if (!dcNodos.ContainsKey(sKey))
                    {
                        dcNodos.Add(sKey, tn);
                    }
                   

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return tvExportadas.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey);
        }

        /// <summary>
        /// Por cada localidad busca las remesas que tienen datos para cargar, y carga 
        /// los nodos respectivos
        /// </summary>
        /// <param name="sLote">Remesa a verificar/cargar</param>
        /// <param name="sDistrito">Distrito al que pertenece la zona</param>
        /// <param name="sLetrZon">Si corresponde, letra que tiene la zona</param>
        /// <returns>Retorna True si el nodo de zona existe o fué añadico con éxito</returns>
        private bool AgregarNodoRemesa(string Empresa, int Distrito, int Lote)
        {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            string sLote = Lote.ToString().Trim();

            //string sKRm = sDtr + "Lo" + sLote;
            string sKRm = sDtr + "Rem" + sLote;

            //MessageBox.Show(sEmp + " " + sDtr + " " + sKRm + " " + sKRt);

            clInfoNodos tn = new clInfoNodos();

            try
            {
                if (!tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm))
                {
                    ///Agrega el nodo Lote
                    //tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes.Add(sKRm, "Lote " + sLote, "nada");
                    tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes.Add(sKRm, "Remesa " + sLote, "nada");
                    tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes[sKRm].Tag = sKRm;

                    //Carga info del nodo
                    //tn.Texto = "Lote " + sLote;
                    tn.Texto = "Remesa " + sLote;
                    tn.Key = sKRm;
                    tn.Distrito = Distrito;
                    tn.Remesa = Lote;
                    //tn.Ruta = Ruta;
                    tn.ImageKey = "nada";
                    dcNodos.Add(sKRm, tn);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
            }

            //return tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm);
            return true;

        }


        /// <summary>
        /// Por cada localidad busca las remesas que tienen datos para cargar, y carga 
        /// los nodos respectivos en tvExportadas
        /// </summary>
        /// <param name="sLote">Remesa a verificar/cargar</param>
        /// <param name="sDistrito">Distrito al que pertenece la zona</param>
        /// <param name="sLetrZon">Si corresponde, letra que tiene la zona</param>
        /// <returns>Retorna True si el nodo de zona existe o fué añadico con éxito</returns>
        private bool AgregarNodoRemesaExportadas(string Empresa, int Distrito, int Lote, int Remesa)
        {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            //string sLote = Lote.ToString().Trim();
            string sRemesa = Remesa.ToString().Trim();

            //string sKRm = sDtr + "Lo" + sLote;
            string sKRm = sDtr + "Rem" + sRemesa;
            //string sKRt = sKRm + "ru" + sRuta;

            //MessageBox.Show(sEmp + " " + sDtr + " " + sKRm + " " + sKRt);

            clInfoNodos tn = new clInfoNodos();

            try
            {
                if (!tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm))
                {
                    /////Agrega el nodo Lote
                    //tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes.Add(sKRm, "Lote " + sLote, "nada");
                    //tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sKRm].Tag = sKRm;

                    ///Agrega el nodo Remesa
                    tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes.Add(sKRm, "Remesa " + sRemesa, "nada");
                    tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sKRm].Tag = sKRm;

                    ////Carga info del nodo
                    //tn.Texto = "Lote " + sLote;
                    //tn.Key = sKRm;
                    //tn.Distrito = Distrito;
                    //tn.Remesa = Lote;
                    ////tn.Ruta = Ruta;
                    //tn.ImageKey = "nada";
                    //dcNodos.Add(sKRm, tn);

                    //Carga info del nodo
                    tn.Texto = "Remesa " + sRemesa;
                    tn.Key = sKRm;
                    tn.Distrito = Distrito;
                    tn.Remesa = Lote;
                    //tn.Ruta = Ruta;
                    tn.ImageKey = "nada";
                    //dcNodos.Add(sKRm, tn);

                    if (!dcNodos.ContainsKey(sKRm))
                    {
                        dcNodos.Add(sKRm, tn);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
            }

            //return tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm);
            return true;

        }




        /// <summary> Agrega la ruta, con todas
        /// </summary>
        /// <param name="Empresa"></param>
        /// <param name="Distrito"></param>
        /// <param name="Remesa"></param>
        /// <param name="Ruta"></param>
        /// <returns>true si consigue agregar el nodo o sumar al existente</returns>
        private bool AgregarNodoRuta(string Empresa, int Distrito, int Remesa, int Ruta)
        {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            //string sRem = sDtr + "Lo" + Remesa.ToString().Trim();//el nodo remesa en realidad es Lote en este caso
            string sRem = sDtr + "Rem" + Remesa.ToString().Trim();//el nodo remesa en realidad es Lote en este caso
            string sKRt = sRem + "ru" + Ruta.ToString().Trim();
            clInfoNodos tn = new clInfoNodos();

            try
            {
                if (!tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.ContainsKey(sKRt))
                {
                    //Agrega el nodo remesa
                    tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.Add(sKRt, "Ruta:" + Ruta.ToString().Trim(), "nada");
                    tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].Tag = sKRt;
                    tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].BackColor = Color.FloralWhite;

                    //Carga info del nodo
                    tn.Texto = "Ruta: " + Ruta.ToString().Trim();
                    tn.Key = sKRt;
                    tn.Distrito = Distrito;
                    tn.Remesa = Remesa;
                    tn.Ruta = Ruta;
                    //tn.Hasta = 0;
                    //tn.Desde = int.MaxValue;
                    //tn.CnxSelected = tn.CnxTotal = 0;
                    tn.ImageKey = "nada";
                    dcNodos.Add(sKRt, tn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
                return false;
            }
            return tvlotes.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.ContainsKey(sKRt);
            //return true;

        }


        /// <summary> Agrega la ruta, con todas
        /// </summary>
        /// <param name="Empresa"></param>
        /// <param name="Distrito"></param>
        /// <param name="Remesa"></param>
        /// <param name="Ruta"></param>
        /// <returns>true si consigue agregar el nodo o sumar al existente</returns>
        private bool AgregarNodoRutaExportadas(string Empresa, int Distrito, int Lote, int Ruta, int Rem)
        {
            string sEmp = Empresa.ToLower().Trim();
            string sDtr = sEmp + Distrito.ToString().Trim();
            //string sRem = sDtr + "Lo" + Lote.ToString().Trim();//el nodo remesa en realidad es Lote en este caso
            string sRem = sDtr + "Rem" + Rem.ToString().Trim();//el nodo remesa en realidad es Lote en este caso
            string sKRt = sRem + "ru" + Ruta.ToString().Trim();
            clInfoNodos tn = new clInfoNodos();

            try
            {
                if (!tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.ContainsKey(sKRt))
                {
                    //Agrega el nodo remesa
                    tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.Add(sKRt, "Ruta:" + Ruta.ToString().Trim(), "nada");
                    tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].Tag = sKRt;
                    tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].BackColor = Color.FloralWhite;

                    //Carga info del nodo
                    tn.Texto = "Ruta: " + Ruta.ToString().Trim();
                    tn.Key = sKRt;
                    tn.Distrito = Distrito;
                    //tn.Remesa = Remesa;
                    tn.Remesa = Rem;
                    tn.Ruta = Ruta;
                    tn.ImageKey = "nada";
                    dcNodos.Add(sKRt, tn);

                    if (!dcNodos.ContainsKey(sKRt))
                    {
                        dcNodos.Add(sKRt, tn);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
                return false;
            }
            return tvExportadas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.ContainsKey(sKRt);
            //return true;

        }


        /// <summary>
        /// Cuenta la cantidad de conexiones que contiene el distrito seleccionado de acuerdo al lote en el que se esta trajando
        /// </summary>
        /// <param name="Lote"></param>
        /// <param name="Distr"></param>
        /// <returns></returns>
        private int CantidadConexPorLoteyDistr(int Lote, int Distr, int Ruta, int Remesa)
        {
            int Cant = 0;
            MySqlCommand da;

            string txSQL;
            DataTable Tabla = new DataTable();
            try
            {
                txSQL = "select Count(*) " +
                        "From Conexiones " +
                //"Where (Lote = " + Lote + " and Zona = " + Distr + " and Ruta = " + Ruta +" and Periodo = " + Vble.Periodo + " and Remesa = " + Remesa + ")";
                "Where (Zona = " + Distr + " and Ruta = " + Ruta + " and Periodo = " + Vble.Periodo + " and Remesa = " + Remesa + ") AND (ImpresionOBS > 500 AND ImpresionOBS < 600) ORDER BY Ruta ASC ";
                //"Where (Lote = " + Lote + " and Zona = " + Distr + " and Ruta = " + Ruta + " and Periodo = " + Vble.Periodo + ") AND ((ImpresionOBS >= 500 AND ImpresionOBS <= 600) OR (ImpresionOBS = 800))";
                da = new MySqlCommand(txSQL, DB.conexBD);
                Cant = Convert.ToInt32(da.ExecuteScalar());

                da.Dispose();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            return Cant;
        }

        /// <summary>
        /// Cuenta la cantidad de conexiones total del lote en el que se esta trabajando
        /// </summary>
        /// <param name="Lote"></param>
        /// <returns></returns>
        private int CantidadConexPorLote(int Lote)
        {
            int Cant = 0;
            MySqlCommand da;

            string txSQL;
            DataTable Tabla = new DataTable();
            try
            {
                txSQL = "select Count(*) " +
                        "From Conexiones " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (Lote = " + Lote + ")";
                da = new MySqlCommand(txSQL, DB.conexBD);
                Cant = Convert.ToInt32(da.ExecuteScalar());

                da.Dispose();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            return Cant;
        }




        /// <summary>
        /// Devuelve la cantidad de novedades que contiene la conexion que se pasa como parametro
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private int CantidadNovedadesPorConex(int ConexionID)
        {
            int Cant = 0;
            MySqlCommand da;

            string txSQL;
            DataTable Tabla = new DataTable();
            try
            {
                txSQL = "select Count(*) " +
                        "From novedadesconex " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (ConexionID = " + ConexionID + ")";
                da = new MySqlCommand(txSQL, DB.conexBD);
                Cant = Convert.ToInt32(da.ExecuteScalar());

                da.Dispose();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            return Cant;
        }

        /// <summary>
        /// Devuelve TRUE la conexion pasada como parametro contiene novedades,
        /// caso contrario FALSE
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private bool ExisteNovedades(Int32 ConexionID, int Periodo)
        {
            bool bandera = false;
            try
            {
                MySqlDataReader dr;
                string txSQL;
                txSQL = "select C.Lote, C.ConexionID, N.Codigo, N.Observ " +
                           "From Conexiones C " +
                           "INNER JOIN NovedadesConex N ON C.ConexionID = N.ConexionID " +
                           "Where (N.ConexionID = " + ConexionID + " AND N.Periodo = " + Periodo + ")";
                MySqlCommand command = new MySqlCommand(txSQL, DB.conexBD);
                dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    bandera = true;
                }
                dr.Close();
                return bandera;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error en Novedades de Conexion");
            }
            return bandera;
        }

        /// <summary>
        /// Devuelve TRUE la conexion pasada como parametro contiene novedades,
        /// caso contrario FALSE
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private bool ExistenRegistrosLogErrores(Int32 ConexionID, int Periodo)
        {
            bool bandera = false;
            try
            {
                MySqlDataReader dr;
                string txSQL;
                txSQL = "select Count(*) From LogErrores " +                           
                           "Where ConexionID = " + ConexionID + " AND Periodo = " + Periodo;
                MySqlCommand command = new MySqlCommand(txSQL, DB.conexBD);
                dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    bandera = true;
                }
                dr.Close();
                command.Dispose();
                return bandera;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error al verificar existencia de registro en Log Errores");
            }
            return bandera;
        }

        /// <summary>
        /// Funcion que creará la linea con los Importes de cada Concepto Facturado por Conexion
        /// si tiene n + 1 conceptos facturados, habrá n+1 lineas HCF con el mismo numero de conexionID
        /// pero correspondiente a cada concepto Facturado.
        /// </summary>
        private void CargarConceptosFacturados(Int32 ConexionID)
        {
            DataTable Tabla2 = new DataTable();
            int contador = 0;
            try
            {
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;

                //txSQL = "select conexionID, Periodo, CodigoConcepto, CodigoDpec, CodigoEscalon, Importe, ImportePA " +
                //           "From conceptosfacturados " +                           
                //           "Where (conexionID = " + ConexionID + ")";

                txSQL = "select * From conceptosfacturados " +
                          "Where (conexionID = " + ConexionID + " and Periodo = " + Vble.Periodo + ")";


                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla2);


                string consultacount;
                MySqlCommand de;
                int count = 0;

                ///consulta para saber la cantidad de conceptos facturados tiene cada conexion
                consultacount = "SELECT Count(*) FROM conceptosfacturados WHERE conexionID = " + ConexionID + " and Periodo = " + Vble.Periodo;
                de = new MySqlCommand(consultacount, DB.conexBD);
                //de.Parameters.AddWithValue("ConexionID", DB.conexBD);
                count = Convert.ToInt32(de.ExecuteScalar());

                de.Dispose();


                //          **********----  Formato de linea de Conceptos Facturados   ----************
                //Encabezado|ConexionID|Periodo|CodigoConcepto|CodigoDpec|CodigoEscalon|ImportePA|ImportePB
                //    HCF   | 00000000 |000000 |     0000     |  aaaaaaa |     00      |  00.00  |   00.00


                foreach (DataRow fi in Tabla2.Rows)
                {
                    contador++;
                    double ImportePB;
                    Int64 CodigoGrupo;
                    string CodigoDpec = "", CodigoConcepto = "";
                    ImportePB = (fi.Field<double>("Importe") - fi.Field<double>("ImportePA"));
                    CodigoGrupo = fi.Field<int>("CodigoGrupo");


                    //// Campo CodigoConcepto:
                    //// 	si < 100 --> NO DEVOLVER
                    //// 	
                    ////	si está entre 100 y 10.000	DEVUELVE: 
                    //// 										CodigoDpec = Dato de tabla
                    //// 										CodigoConcepto = ""   (vacio)
                    ////   
                    ////    si > 10.000 DEVUELVE: 
                    ////						CodigoDpec = ""       (vacio)
                    ////						CodigoConcepto = Dato de tabla
                    //// 
                    //// ANTES DE DEVOLVER VERIFICAR
                    //// 
                    //// 	Si CodigoGrupo = 0
                    // 							DEVOLVER
                    //// 	Si CodigoGrupo > 0 Y Agrupador Contiene "SI"
                    //// 							DEVOLVER


                    if (fi.Field<int>("CodigoConcepto") > 100 && CodigoGrupo == 0 || (fi.Field<string>("Agrupador").Contains("SI")))
                    {
                        // DEVOLVER
                        if (fi.Field<int>("CodigoConcepto") < 10000)
                        {
                            CodigoConcepto = "";           //Vacio
                            CodigoDpec = fi.Field<string>("CodigoDpec");
                        }
                        else
                        {
                            CodigoConcepto = fi.Field<int>("CodigoConcepto").ToString();
                            CodigoDpec = "";            //Vacio
                        }


                        //Verifica el Campo Codigo Auxiliar que se tomo como bandera para indicar si el CodigoConcepto que vino 
                        //en el download se deberá devolver de la misma forma en el Upload.
                        if (fi.Field<int>("CodigoAux") == 1)
                        {
                            CodigoConcepto = CodigoDpec;
                            CodigoDpec = "";
                        }


                        ///éste if sirve para que a la hora de armar la linea HCF, la ultima linea no haga un salto de linea
                        ///y no quede separado de la linea de la proxima conexionID                   
                        if (count == contador)
                        {
                            Vble.LineaHCF += "HCF|" + fi.Field<int>("conexionID").ToString() + "|" + fi.Field<int>("Periodo").ToString() +
                                             "|" + CodigoConcepto + "|" + CodigoDpec +
                                             "|" + fi.Field<int>("CodigoEscalon").ToString() + "|" + fi.Field<double>("ImportePA").ToString() +
                                             "|" + ImportePB.ToString();

                            Vble.LineaHCX += "\n" + Vble.LineaHCF;
                            Vble.LineaHCF = "";

                        }
                        else
                        {
                            Vble.LineaHCF += "HCF|" + fi.Field<int>("conexionID").ToString() + "|" + fi.Field<int>("Periodo").ToString() +
                                             "|" + CodigoConcepto + "|" + CodigoDpec +
                                             "|" + fi.Field<int>("CodigoEscalon").ToString() + "|" + fi.Field<double>("ImportePA").ToString() +
                                             "|" + ImportePB.ToString();

                            Vble.LineaHCX += "\n" + Vble.LineaHCF;
                            Vble.LineaHCF = "";

                        }

                    }

                }



                ///Cierro la consulta
                comandoSQL.Dispose();
                da.Dispose();

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error al Crear Linea HCF");
            }
            //return Tabla2;


        }



        /// <summary>
        /// Función que carga las novedades en caso de que exista para la conexion que se pasa por parametro 
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private void CargarNovedadesConex(Int32 ConexionID, int periodo, int j, string Operario, string ActualEstado, string Numero)
        {
            DataTable Tabla2 = new DataTable();
            string obsernov = "";
            Vble.LineaHNC = "";
            Vble.EstadoCorregido = "";
            Vble.EnergiaInyectada = "";
            try
            {
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;

                txSQL = "select C.Lote, C.ConexionID, N.Codigo, N.Observ, N.Periodo, C.ImpresionCOD " +
                           "From Conexiones C " +
                           "INNER JOIN NovedadesConex N ON C.ConexionID = N.ConexionID AND C.Periodo = N.Periodo " +
                           "Where (C.ConexionID = " + ConexionID + " and N.Periodo = " + Vble.Periodo + ") ";


                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla2);
                int indice = 0;

               
                Vble.EnergiaInyectada = Funciones.EnergiaInyectada(ConexionID, Vble.Periodo, Numero);
                

                obsernov = Vble.EnergiaInyectada;

                foreach (DataRow fi in Tabla2.Rows)
                {
                    if ((fi.Field<int>("ConexionID") == ConexionID))
                    {
                        if (fi.Field<int>("Codigo").ToString().Trim() == "00" || fi.Field<int>("Codigo").ToString().Trim() == "0")
                        {
                            ArrayCodNovedades[indice] = "";
                        }
                        else
                        {
                            ArrayCodNovedades[indice] = fi.Field<int>("Codigo").ToString().Trim();
                        }
                    }                

                        if (ArrayCodNovedades[indice] == "243")
                    {
                        Vble.EstadoCorregido = fi.Field<string>("Observ");
                        ArrayCodNovedades[indice] = "";
                        ArrayDescrNovedades[indice] = "";
                        obsernov += " " + ArrayDescrNovedades[indice].Replace("|", "");
                    }
                    else if (ArrayCodNovedades[indice] == "242")
                    {
                        ArrayCodNovedades[indice] = "";
                        ArrayDescrNovedades[indice] = fi.Field<string>("Observ");
                        obsernov += " Erroneo: " + ArrayDescrNovedades[indice].Replace("|", "");
                    }
                    else

                    {
                        if (!String.IsNullOrWhiteSpace(fi.Field<string>("Observ")))
                        {
                            
                            ArrayDescrNovedades[indice] = (fi.Field<string>("Observ").Replace("'", "").Trim());
                            if (ArrayDescrNovedades[indice].Length > 10)
                            {
                                ArrayDescrNovedades[indice] = ArrayDescrNovedades[indice].Substring(0, 9);
                            }
                            obsernov += " " + ArrayDescrNovedades[indice].Replace("|","");
                        }
                    }

                    indice++;
                }

                /////Almaceno la cadena con el formatmo ConexionID|Periodo|0|0|0|0|0|Observaciones en la variable Vble.LineaHCN  que luego se concatena con la variable
                /////general que contiene los demas datos del upload.
                if (ArrayCodNovedades[0].ToString() == "")
                {
                    //if (!String.IsNullOrWhiteSpace(ArrayCodNovedades[1].ToString()) || ArrayCodNovedades[1].ToString() == "")
                    if (ArrayCodNovedades[1] != null)
                    {
                        ArrayCodNovedades[0] = ArrayCodNovedades[1].ToString();
                        ArrayCodNovedades[1] = "";
                       
                    }
                    
                    else if (ArrayCodNovedades[2] != null)
                    {
                        ArrayCodNovedades[0] = ArrayCodNovedades[2].ToString();
                        ArrayCodNovedades[2] = "";
                    }
                    else if (ArrayCodNovedades[3] != null)
                    {
                        ArrayCodNovedades[0] = ArrayCodNovedades[3].ToString();
                        ArrayCodNovedades[3] = "";
                    }
                    else if (ArrayCodNovedades[4]  != null)
                    {
                        ArrayCodNovedades[0] = ArrayCodNovedades[4].ToString();
                        ArrayCodNovedades[4] = "";
                    }
                }

                
                if (Vble.EstadoCorregido != "")
                {
                    Vble.LineaHNC += $"{Vble.EstadoCorregido}|{ArrayCodNovedades[0]}|{ArrayCodNovedades[1]}|{ArrayCodNovedades[2]}|" +
                                $"{ArrayCodNovedades[3]}|{ArrayCodNovedades[4]}|{obsernov}|{Operario}";
                }
                else
                {
                    Vble.LineaHNC += $"{ActualEstado}|{ArrayCodNovedades[0]}|{ArrayCodNovedades[1]}|{ArrayCodNovedades[2]}|" +
                             $"{ArrayCodNovedades[3]}|{ArrayCodNovedades[4]}|{obsernov}|{Operario}";
                }

                if (j == 0)
                {
                    //Vble.LineaHCX += "\n" + Vble.LineaHNC;
                    Vble.LineaHFSindiv += $"{Vble.LineaHNC}\n";
                    obsernov = "";
                    Vble.LineaHNC = "";
                }
                else
                {
                    //Vble.LineaHCX += "\n" + Vble.LineaHNC;
                    Vble.LineaHFSindiv += $"{Vble.LineaHNC}\n";
                    obsernov = "";
                    Vble.LineaHNC = "";
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error en Novedades de Conexion");
            }
            //return Tabla2;
        }



        /// <summary>
        /// Función que carga las novedades en caso de que exista para la conexion que se pasa por parametro 
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private string ArmarLineaLog(Int32 ConexionID, int periodo)
        {
            DataTable Tabla2 = new DataTable();
            string Linea = "";            
            try
            {
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;

                txSQL = "SELECT * From LogErrores " +                           
                           "WHERE ConexionID = " + ConexionID + " and Periodo = " + periodo;


                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla2);
                
                foreach (DataRow fi in Tabla2.Rows)
                {
                    Linea = fi.Field<DateTime>("Fecha").ToString("dd/MM/yyyy").PadRight(11) + fi.Field<string>("Hora").ToString().PadRight(9) + "E" +
                                            fi.Field<string>("Entorno").ToString().PadRight(2) + "- Op:" + fi.Field<Int32>("Lecturista").ToString().PadRight(5) + "- R:" +
                                            fi.Field<Int32>("Ruta").ToString().PadRight(4) + "- Cnx:" + fi.Field<Int32>("ConexionID").ToString().PadRight(8) + " - Err: " +
                                            fi.Field<string>("CodigoError").ToString().PadRight(6) + " - " + fi.Field<string>("TextoError").ToString() + " " +
                                            fi.Field<string>("Mensaje").ToString() + "\n";
                    Vble.CANTLog++;
                }
                comandoSQL.Dispose();
                da.Dispose();

                
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error en Novedades de Conexion");
            }
            return Linea;
        }


        /// <summary>
        /// Al seleccionar el checbok Todos, se seleccionan todas las conexiones que se encuentran en la base de datos
        /// para su exportación
        /// </summary>
        /// <returns></returns>
        private DataTable CargarConexionesTodo()
        {

            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL = "", txSQL2 = "";
            DataTable Tabla1 = new DataTable();
            DataTable Tabla2 = new DataTable();

            ///Tomo las localidades pertenecientes a la interfaz que se esta trabajando para ver a la hora de cargar las conexiones a colectora
            LeerArchivoZonaFIS();

            int Distr;
            try
            {

                txSQL = "select Distinct C.Lote, C.Zona, C.Ruta, C.Periodo, C.Remesa" +
                        " From Conexiones C WHERE C.Periodo = " + Vble.Periodo + " AND ((C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") " +
                                                                                        "AND (C.Remesa = 1 OR C.Remesa = 2 OR C.Remesa = 3 OR " +
                                                                                              "C.Remesa = 4 OR C.Remesa = 5 OR C.Remesa = 6 OR " +
                                                                                              "C.Remesa = 7 OR C.Remesa = 8)) AND (C.ImpresionOBS > 500 and C.ImpresionOBS < 600)" +
                        " GROUP BY C.Remesa ORDER BY C.ConexionID ASC ";

                txSQL2 = "select Distinct C.Lote, C.Zona, C.Ruta, C.Periodo, C.Remesa" +
                          " From Conexiones C WHERE C.Periodo = " + Vble.Periodo + " AND ((C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") " +
                                                                                          "AND (C.Remesa = 1 OR C.Remesa = 2 OR C.Remesa = 3 OR " +
                                                                                                "C.Remesa = 4 OR C.Remesa = 5 OR C.Remesa = 6 OR " +
                                                                                                "C.Remesa = 7 OR C.Remesa = 8)) AND (C.ImpresionOBS > 500 and C.ImpresionOBS < 600)" +
                          " GROUP BY C.Zona  ORDER BY C.ConexionID ASC ";

                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla1);
                TablaUpload1 = Tabla1;
                comandoSQL.Dispose();
                da.Dispose();


                da = new MySqlDataAdapter(txSQL2, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla2);
                TablaUploadCantidadRutas = Tabla2;
                comandoSQL.Dispose();
                da.Dispose();


                //Almaceno en vble.Lote y Vble.Distrito los datos que serán utilizados para generar el nombre del archivo Upload Correspondiente
                foreach (DataRow fi in TablaUpload1.Rows)
                {
                    //Vble.Lote = (int)fi.Field<int>("Lote");
                    Vble.Remesa = (int)fi.Field<int>("Remesa");
                    Distr = fi.Field<int>("Zona");
                    if (Distr < 100) Vble.Distrito = Distr + 200;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Tabla1;
        }



        /// <summary>
        /// Obtiene las rutas que pertencen al periodo y remesa que se pasan por parametros para informar en el panel de 
        /// Rutas a exporar.
        /// </summary>
        /// <returns></returns>
        private DataTable RutasPoRemesaYPeriodo(int Periodo, string Remesa)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL = "";
            DataTable Tabla1 = new DataTable();

            try
            {

                txSQL = $"SELECT DISTINCT Ruta FROM Conexiones WHERE Periodo = {Periodo} AND Remesa = {Remesa}";

                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla1);
                TablaUploadCantidadRutas = Tabla1;
                comandoSQL.Dispose();
                da.Dispose();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Tabla1;
        }

        /// <summary>
        /// //Metodo que contiene la consulta SELECT para obtener los registros que esten 
        /// //dentro de la secuencia seleccionada del treeview con un solo valor de secuencia DESDE y HASTA
        /// </summary>
        /// <returns></returns>
        //private DataTable CargarConexionesXLote(int Lote, int Ruta)
        private DataTable ExportarRutasSeleccionadas()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            DataTable Tabla1 = new DataTable();

            int Distr;

            //foreach (var item in ArrayRuta)
            //{
            //    MessageBox.Show(item.ToString());
            //}

            try
            {

                txSQL = "select Distinct C.Lote, C.Zona, C.Ruta, C.Periodo, C.Remesa " +
                        "From Conexiones C " +
                        ////"Where (C.Lote = " + Lote + " and C.Ruta = " + Ruta + ") " +
                        //"Where (C.Lote = " + ArrayLotes[0] + " and C.Ruta = " + ArrayRuta[0] + " and Periodo = " + Vble.Periodo + ") " +
                        //"Where (C.Ruta = " + ArrayRuta[0] + " and Periodo = " + Vble.Periodo + " and Remesa = " + ArrayLotes[0] + ") " +
                        "Where (C.Ruta = " + ArrayRemesaRuta[0].ToString().Substring(2) + " and Periodo = " + Vble.Periodo + " and Remesa = " + ArrayRemesaRuta[0].ToString().Substring(0, 1) + ") " +
                         //"Where (C.Remesa = " + ArrayLotes[0] + " and C.Ruta = " + ArrayRuta[0] + " and Periodo = " + Vble.Periodo + ") " +
                         iteracion() + "ORDER BY C.Ruta ASC";


                //txSQL = "select C.Lote, C.Zona, C.ConexionID, C.Periodo, C.Instalacion, C.FechaCalP, C.ImpresionOBS, C.ImpresionCANt, C.Operario, C.ConsumoFacturado, " +
                //        "C.FacturaLetra, C.PuntoVenta, C.FacturaNro1, C.Importe1, C.ImporteBasico1, C.ImporteImpuesto1, C.FacturaNro2, "+
                //        "C.ImporteBasico2, C.ImporteImpuesto2, M.ActualFecha, M.ActualHora, M.ActualEstado, M.TipoLectura " +
                //        "From conexiones C " +                        
                //        //"INNER JOIN localidades L ON L.CodigoPostal = C.CodPostalSumin " +
                //        "INNER JOIN medidores M on C.conexionID = M.conexionID " +                      
                //        "Where (C.Lote = " + Lote + ") " +  
                //         iteracion() + "ORDER BY C.ConexionID ASC";

                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.SelectCommand.CommandTimeout = 900;
                da.Fill(Tabla1);
                TablaUpload1 = Tabla1;

                dataGridView1.DataSource = TablaUpload1;
                Vble.CantRegistros = TablaUpload1.Rows.Count;

                //TablaUpload2 = CargarNovedadesConex(Lote);
                //Almaceno en vble.Lote y Vble.Distrito los datos que serán utilizados para generar el nombre del archivo Upload Correspondiente
                foreach (DataRow fi in TablaUpload1.Rows)
                {
                    Vble.Lote = fi.Field<int>("Lote");
                    Vble.Remesa = fi.Field<int>("Remesa");
                    Distr = fi.Field<int>("Zona");
                    if (Distr < 150) Vble.Distrito = Distr + 200;
                    //int i = 0;
                    //    if (fi.Field<int>("ImpresionOBS") > 500)
                    //    {
                    //        Vble.CantAExportar++;
                    //    }
                    //    i++;
                }
                comandoSQL.Dispose();
                da.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Tabla1;
        }


        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracion()
        {
            string where = "";
            try
            {
                for (int i = 0; i < ArrayLotes.Count; i++)
                {

                    //where += "OR (C.Lote = " + ArrayLotes[i] + " and C.Ruta = " + ArrayRuta[i] + " and C.Periodo = " + Vble.Periodo +")";
                    where += "OR (C.Remesa = " + ArrayLotes[i] + " and C.Ruta = " + ArrayRuta[i] + " and C.Periodo = " + Vble.Periodo + ") ";
                }

            }
            catch (Exception)
            {

            }
            return where;

        }

        ///// <summary>
        ///// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        ///// </summary>
        ///// <returns></returns>
        //private string iteracion2()
        //{
        //    string where = "";
        //    try
        //    {
        //        for (int i = 0; i < ArrayLotes.Count; i++)
        //        {
        //            where += "OR (C.ConexionID = " + ArrayLotes[i] + ")";
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return where;

        //}


        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// L=LOTE
        /// D=DISTRITO
        /// R=RUTA
        /// P=PERIODO
        /// </summary>
        /// <returns></returns>
        private string iteracionLDRP(int Distr, int Periodo)
        {
            string where = "";
            try
            {
                for (int i = 0; i < ArrayRuta.Count; i++)
                {
                    //where += "OR (C.Lote = " + ArrayLotes[i] + ")";
                    where += " OR (C.Lote = " + ArrayLotes[i] +
                             " and C.Zona = " + Distr +
                             " and C.Ruta = " + ArrayRuta[i] +
                             " and C.Periodo = " + Periodo + ")";
                }

            }
            catch (Exception)
            {

            }
            return where;

        }

        /// <summary>
        /// Consulta las conexiones que pertenezcan al mismo periodo y remesa segun las conexiones existentes y disponibles a 
        /// exportar (con ImpresionOBS correspondiente) para generar los archivos UPLOAD individualizados por remesas, 
        /// independientemente de las rutas involucradas
        /// en el periodo que se está procesando.
        /// El parametro AllRemesas hace referencia a la condición y en que momento se utiliza el metodo, es decir,
        /// Cuando AllRemesas 1, haria la consulta que involucra todas las remesas del 1 al 8 para obtener la cantidad
        /// de conexiones que se exportarian y estan habilitadas con ImpresionOBS entre 500 y 600
        /// </summary>
        /// <param name="Lote"></param>
        /// <param name="Distr"></param>
        /// <param name="Ruta"></param>
        /// <param name="Periodo"></param>
        /// <param name="Remesa"></param>
        /// <param name="AllRemesas"></param>
        private void ObtenerConexionesXloteYDistr(int Lote, int Distr, int Ruta, Int32 Periodo, int Remesa, int AllRemesas)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL="";

            try
            {
                //if (AllRemesas == 0)
                //{
                    if (UpPorRuta.Checked == true)
                    {
                        //    txSQL = "select distinct P.*, C.*, M.* " +
                        //            "From Conexiones C " +
                        //            "INNER JOIN Medidores M on C.ConexionID = M.ConexionID and C.Periodo = M.Periodo " +
                        //            "INNER JOIN Personas P ON C.titularID = P.PersonaID and C.Periodo = P.Periodo " +
                        //            "Where (C.Periodo = " + Periodo +
                        //            " and C.Remesa = " + Remesa /*+ iteracionRemesa() + ")+ " AND C.Ruta = " + Ruta + " ) AND " +
                        //            "(C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND " +
                        //            "(C.ImpresionOBS > 500 AND C.ImpresionOBS < 600) " +
                        //            "ORDER BY C.Ruta ASC";
                        //}
                        //else
                        //{
                        //    txSQL = "select distinct P.*, C.*, M.* " +
                        //            "From Conexiones C " +
                        //            "INNER JOIN Medidores M on C.ConexionID = M.ConexionID and C.Periodo = M.Periodo " +
                        //            "INNER JOIN Personas P ON C.titularID = P.PersonaID and C.Periodo = P.Periodo " +
                        //            "Where (C.Periodo = " + Periodo +
                        //            " and C.Remesa = " + Remesa /*+ iteracionRemesa() + ")*/+ " and C.Ruta = " + ArrayRuta[0] + iteracionRuta() + ") AND " +
                        //            "(C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND " +
                        //            "(C.ImpresionOBS > 500 AND C.ImpresionOBS < 600) " +
                        //            "ORDER BY C.Ruta ASC";// OR (C.ImpresionOBS = 800))";
                        //                    //ULTIMA VERSION ---> "Where 
                        //                    //(C.Zona = " + Distr + " and C.Ruta = " + Ruta + " and C.Periodo = " + Periodo + " and 
                        //                    //C.Remesa = " + Remesa + ") AND ((C.ImpresionOBS >= 500 AND C.ImpresionOBS <= 600) OR 
                        //                    //(C.ImpresionOBS = 800))";



                        txSQL = "select distinct P.*, C.*, M.*, A.* " +
                                    "From Conexiones C " +
                                    "INNER JOIN Medidores M on C.ConexionID = M.ConexionID and C.Periodo = M.Periodo " +
                                    "INNER JOIN Personas P ON C.titularID = P.PersonaID and C.Periodo = P.Periodo " +
                                    "LEFT JOIN Altas A ON C.ConexionID = A.ConexionID AND C.Periodo = A.Periodo " +
                                    "Where (C.Periodo = " + Periodo +
                                    " and C.Remesa = " + Remesa + " and C.Ruta = " + Ruta + ") AND " +
                                    "(C.Zona = " + Distr + ") AND " +
                                    "(C.ImpresionOBS > 500 AND C.ImpresionOBS < 600)  " +
                                    "ORDER BY C.Ruta ASC";


                    }
                //}
                //else
                //{
                else
                {
                    if (UpPorRuta.Checked == false)
                    {

                        txSQL = "select distinct P.*, C.*, M.*, A.* " +
                                "From Conexiones C " +
                                "INNER JOIN Medidores M on C.ConexionID = M.ConexionID and C.Periodo = M.Periodo " +
                                "INNER JOIN Personas P ON C.titularID = P.PersonaID and C.Periodo = P.Periodo " +
                                "LEFT JOIN Altas A ON C.ConexionID = A.ConexionID AND C.Periodo = A.Periodo " +
                                "Where (C.Periodo = " + Periodo +
                                " and C.Remesa = " + Remesa /*+ iteracionRemesa() + ")*/+ " and (C.Ruta = " + ArrayRuta[0] + iteracionRuta() + ")) AND " +
                                "(C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND " +
                                "(C.ImpresionOBS > 500 AND C.ImpresionOBS < 600) " +
                                "ORDER BY C.Ruta ASC";// OR (C.ImpresionOBS = 800))";
                                                      //ULTIMA VERSION ---> "Where 
                                                      //(C.Zona = " + Distr + " and C.Ruta = " + Ruta + " and C.Periodo = " + Periodo + " and 
                                                      //C.Remesa = " + Remesa + ") AND ((C.ImpresionOBS >= 500 AND C.ImpresionOBS <= 600) OR 
                                                      //(C.ImpresionOBS = 800))";



                        //txSQL = "select distinct P.*, C.*, M.* " +
                        //        "From Conexiones C " +
                        //        "INNER JOIN Medidores M on C.ConexionID = M.ConexionID and C.Periodo = M.Periodo " +
                        //        "INNER JOIN Personas P ON C.titularID = P.PersonaID and C.Periodo = P.Periodo " +
                        //        "Where (C.Periodo = " + Periodo +
                        //        " and (C.Remesa = " + Remesa + iteracionRemesa() + ") AND C.Ruta = " + Ruta + ") AND " +
                        //        "(C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND " +
                        //        "(C.ImpresionOBS > 500 AND C.ImpresionOBS < 600) " +
                        //        "ORDER BY C.Ruta ASC";// OR (C.ImpresionOBS = 800))";
                        //                       //ULTIMA VERSION ---> "Where (C.Zona = " + Distr + " and C.Ruta = " + Ruta + " and C.Periodo = " 
                        //                       //+ Periodo + " and C.Remesa = " + Remesa + ") AND 
                        //                       //((C.ImpresionOBS >= 500 AND C.ImpresionOBS <= 600) OR (C.ImpresionOBS = 800))"; 
                    }
                    //else
                    //{
                    //    txSQL = "select distinct P.*, C.*, M.* " +
                    //    "From Conexiones C " +
                    //    "INNER JOIN Medidores M on C.ConexionID = M.ConexionID and C.Periodo = M.Periodo " +
                    //    "INNER JOIN Personas P ON C.titularID = P.PersonaID and C.Periodo = P.Periodo " +
                    //    "Where (C.Periodo = " + Periodo +
                    //    " and (C.Remesa = " + Remesa + iteracionRemesa() + ")  and C.Ruta = " + ArrayRuta[0] + iteracionRuta() +") AND " +
                    //    "(C.Zona = " + Vble.ArrayZona[0] + iteracionZona() + ") AND " +
                    //    "(C.ImpresionOBS > 500 AND C.ImpresionOBS < 600) " +
                    //    "ORDER BY C.Ruta ASC";
                    //}
                }
                //}

                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                da.SelectCommand.CommandTimeout = 300;
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(TablaUpload2);
                //dataGridView2.DataSource = TablaUpload2;
                Vble.Lote = Convert.ToInt16(TablaUpload2.Rows[0]["Lote"].ToString());



            }
            catch (Exception r)
            {
                if (r.Message.Contains("No hay ninguna fila en la posición 0."))
                {

                }
                else
                {
                    MessageBox.Show(r.Message + " Erro al generar Archivo Upload");
                }
            }

        }


        /// <summary>
        /// Se encuentran las consultas que copian los datos a la base HistorialDatosDPEC cada vez que se realiza una exportación para que 
        /// quede registro en una base Historial en caso de querer hacer consultas de estados anteriores.
        /// </summary>
        private void CopiarDatosEnHistorialDatosDPEC(int Periodo, int Localidad, int Ruta, Int32 conexionID, Int32 personaID)
        {
            //Insertamos los datos en la tabla conexiones
            string InsertConexiones = "INSERT INTO HistorialDatosDPEC.conexiones SELECT * FROM datosdpec.conexiones WHERE datosdpec.conexiones.Periodo = " + Periodo +
                                      " AND datosdpec.conexiones.Zona = " + Localidad + " AND datosdpec.conexiones.Ruta = " + Ruta + " AND datosdpec.conexiones.ConexionID = " + conexionID;
            //preparamos la cadena pra insercion
            MySqlCommand commandConex = new MySqlCommand(InsertConexiones, DB.conexBDHistorial);
            //y la ejecutamos
            commandConex.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            commandConex.Dispose();

            //Insertamos los datos en la tabla medidores
            string InsertMedidores = "INSERT INTO HistorialDatosDPEC.medidores SELECT * FROM datosdpec.medidores WHERE datosdpec.medidores.Periodo = " + Periodo +
                                      " AND datosdpec.medidores.ConexionID = " + conexionID;
            //preparamos la cadena pra insercion
            MySqlCommand commandMed = new MySqlCommand(InsertMedidores, DB.conexBDHistorial);
            //y la ejecutamos
            commandMed.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            commandMed.Dispose();

            //Insertamos los datos en la tabla personas
            string InsertPersonas = "INSERT INTO HistorialDatosDPEC.personas SELECT * FROM datosdpec.personas WHERE datosdpec.personas.Periodo = " + Periodo +
                                      " AND datosdpec.personas.PersonaID = " + personaID; ;
            //preparamos la cadena pra insercion
            MySqlCommand commandPersonas = new MySqlCommand(InsertPersonas, DB.conexBDHistorial);
            //y la ejecutamos
            commandPersonas.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            commandPersonas.Dispose();

            //Insertamos los datos en la tabla ConceptosFacturados
            string InsertConFacturados = "INSERT INTO HistorialDatosDPEC.conceptosfacturados SELECT * FROM datosdpec.conceptosfacturados WHERE datosdpec.conceptosfacturados.Periodo = " + Periodo +
                                         " AND datosdpec.conceptosfacturados.ConexionID = " + conexionID;
            //preparamos la cadena pra insercion
            MySqlCommand commandConFacturados = new MySqlCommand(InsertConFacturados, DB.conexBDHistorial);
            //y la ejecutamos
            commandConFacturados.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            commandConFacturados.Dispose();

            //Insertamos los datos en la tabla infoconex
            string InsertInfoConex = "INSERT INTO HistorialDatosDPEC.infoconex SELECT * FROM datosdpec.infoconex WHERE datosdpec.infoconex.Periodo = " + Periodo +
                                         " AND datosdpec.infoconex.ConexionID = " + conexionID;
            //preparamos la cadena pra insercion
            MySqlCommand commandInfoConex = new MySqlCommand(InsertInfoConex, DB.conexBDHistorial);
            //y la ejecutamos
            commandInfoConex.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            commandInfoConex.Dispose();

            //Insertamos los datos en la tabla novedadesconex
            string InsertNovConex = "INSERT INTO HistorialDatosDPEC.novedadesconex SELECT * FROM datosdpec.novedadesconex WHERE datosdpec.novedadesconex.Periodo = " + Periodo +
                                         " AND datosdpec.novedadesconex.ConexionID = " + conexionID;
            //preparamos la cadena pra insercion
            MySqlCommand commandNovConex = new MySqlCommand(InsertNovConex, DB.conexBDHistorial);
            //y la ejecutamos
            commandNovConex.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            commandNovConex.Dispose();
        }

        /// <summary>
        /// Proceso que lee el archivo ZonaFIS.txt que contiene las localidades de la interfaz en el cual se esta trabajando 
        /// la misma esta ubicada en el directorio C:\Windows\ZonaFIS.txt ubicación común para todas las interfaces de GagFIS-Interface 
        /// </summary>
        private static void LeerArchivoZonaFIS()
        {
            try
            {
                Vble.ArrayZona.Clear();
                Vble.ArrayZona.RemoveRange(0, Vble.ArrayZona.Count);
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
                    where += " OR C.Zona = " + Vble.ArrayZona[i];
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
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracionRuta()
        {
            string where = "";
            try
            {
                for (int i = 0; i < ArrayRuta.Count; i++)
                {

                    where += " OR C.Ruta = " + ArrayRuta[i];
                                
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración en Rutas de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }

        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        private string iteracionRemesa()
        {
            string where = "";
            try
            {
                for (int i = 1; i <= 8; i++)
                {

                    where += " OR C.Remesa = " + i;

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
        #endregion Funciones

        void btnCerrar_Click(object sender, EventArgs e)
        {
            tvExportadas.Nodes.Clear();            
            //CargarTVRutasExportadas();
            this.Close();
            Form1Inicio Inicio = new Form1Inicio();
            //Inicio.timer2.Start();
        }

        private void Form2_Resize(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public void cbLote_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbLote.Checked == true)
            {
                cbTodo.Checked = false;
                tvlotes.Visible = true;
                labelPanExpXRuta.Visible = true;
                tvConexUpdload.Items.Clear();
                Vble.CantConex = 0;
                CargarTreeviewXLotes();
            }
        }

        private void cbTodo_CheckStateChanged(object sender, EventArgs e)
        {
            int Distr, Ruta, Remesa;
            Int32 Periodo;

            if (cbTodo.Checked == true)
            {
                cbLote.Checked = false;
                tvlotes.Visible = false;
                labelPanExpXRuta.Visible = false;
                TablaUpload1.Clear();
                TablaUpload2.Clear();
                dataGridView1.DataSource = CargarConexionesTodo();
                if (dataGridView1.RowCount > 0)
                {
                    foreach (DataRow fi in TablaUpload1.Rows)
                    {
                        Vble.Lote = fi.Field<Int32>("Lote");
                        Vble.Distrito = Distr = fi.Field<Int32>("Zona");
                        Vble.Ruta = Ruta = fi.Field<int>("Ruta");
                        Vble.Periodo = Periodo = fi.Field<Int32>("Periodo");
                        Vble.Remesa = Remesa = fi.Field<int>("Remesa");
                        Vble.ArrayRemesas.Add(Vble.Remesa);
                        if (Distr < 150) Vble.Distrito = Distr + 200;
                        //ObtenerConexionesXloteYDistr(Vble.Lote, Distr, Ruta, Periodo, Remesa);
                        //Vble.CantConex += CantidadConexPorLote(Vble.Lote);comentado porque no se estaba usando el todo
                    }
                    ObtenerConexionesXloteYDistr(Vble.Lote, Vble.Distrito, Vble.Ruta, Vble.Periodo, Vble.Remesa, 1);
                    Vble.CantRegistros = TablaUpload1.Rows.Count;
                    //Vble.CantConex = CantidadConexPorLote(Vble.Lote);
                    //MessageBox.Show(Vble.CantConex.ToString());
                    //if (TablaUpload1.Rows.Count >  0)
                    //{
                    tvConexUpdload.Items.Clear();
                    //tvConexUpdload.Items.Add("Estas son las rutas a considerar a la de generar el UPLOA,  las cuales pertenecen a éste centro de interfaz: " + Vble.ArrayZona[0].ToString() + " \n");
                    this.Cursor = Cursors.WaitCursor;
                    for (int i = 0; i < TablaUpload1.Rows.Count; i++)
                    {
                        tvConexUpdload.Items.Add("Distrito: " + TablaUpload1.Rows[i][1].ToString() + " Remesa: " + TablaUpload1.Rows[i][4].ToString() + "\n");
                        for (int j = 0; j < RutasPoRemesaYPeriodo(Vble.Periodo, TablaUpload1.Rows[i][4].ToString()).Rows.Count; j++)
                        {
                            //if (TablaUploadCantidadRutas.Rows[j][2].ToString() == TablaUpload1.Rows[i][2].ToString())
                            //{
                            tvConexUpdload.Items.Add("                             Ruta: " + RutasPoRemesaYPeriodo(Vble.Periodo, TablaUpload1.Rows[i][4].ToString()).Rows[j][0].ToString());
                            //}
                        }
                    }
                    tvConexUpdload.Items.Add("Total de Conexiones: " + TablaUpload2.Rows.Count);
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Disulpe no existen Conexiones en la Base de Datos para exportar", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnIniciarExpor_Click(object sender, EventArgs e)
        {
            try
            {
                //Cierro toda conexion por las dudas que este abierta sino, no me va a dejar exportar
                this.Cursor = Cursors.WaitCursor;
                Vble.CerrarUnidadDeRed();
                ñ = 0;
                if (cbLote.Checked == true || cbTodo.Checked == true)
                {
                    if (Vble.CantConex > 0)
                    //if (TablaUpload1.Rows.Count > 0)
                    {
                        if (GBCambioFecha.Visible == true)
                        {
                            if (MessageBox.Show("ATENCIÓN ¿ESTÁ SEGURO QUE DESEA REALIZAR LA EXPORTACION " +
                                "CON LA FECHA CAMBIADA REAL DE LECTURA?", "Exportar Datos CON FECHA MODIFICADA", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                ExportarRutasSeleccionadas();
                                TareaSegundoPlano1.RunWorkerAsync();
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                            }
                        }
                        else if (GBCambioFecha.Visible == false)
                        {                            
                            if (MessageBox.Show("¿Está seguro que desea realizar la exportación?", "Exportar Datos", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                    ExportarRutasSeleccionadas();
                                    TareaSegundoPlano1.RunWorkerAsync();
                            }
                            else
                            {                             
                              this.Cursor = Cursors.Default;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Primero seleccione lo que desea exportar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                    }

                }
                else
                {
                    MessageBox.Show("Primero seleccione lo que desea exportar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        /// <summary>
        /// Aca se ejecuta Procesar Carga en segundo plano 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TareaSegundoPlano1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Change mouse cursor to busy
                CheckForIllegalCrossThreadCalls = false;
                simulateHeavyWork();                
                progressBarExpor.Maximum = Vble.CantConex;
                GenerarArchivoUpload();
               
            }
            catch (Exception r)
            {

                MessageBox.Show(r.Message + " Error en Comienzo de Tarea de Exportacin en Segundo Plano");
            }
        }



        /// <summary>
        /// Agrega informacion de Carga y descarga de las conexiones en la tabla infoconex de la base
        /// MySql que se utiliza luego para la importacion o exportacion de cada conexion       
        /// </summary>
        /// 
        public static void ModificarInfoConex(Int32 conexionID, int Periodo, string Fecha, string hora, string operario, int Cod_Impresion, string Operacion)
        {
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter();
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder();
            MySqlCommand command = new MySqlCommand();
            Int32 Oper;
            try
            {
                DataTable Tabla1 = new DataTable();
                string txSQL;

                //Int32 conexionID;
                //Int32 Periodo;
                if (Operacion == "Upload")
                {

                    txSQL = "SELECT ImpresionOBS, Ruta FROM conexiones WHERE ConexionID = " + conexionID + " AND Periodo = " + Periodo;
                    datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(Tabla1);

                    Oper = operario == "admin" ? 9003 : Convert.ToInt32(operario);

                    foreach (DataRow fi in Tabla1.Rows)
                    {
                        //si es una carga modifica los campos de carga FechaCarga, Hora Carga, OperCarga

                        //asignación a variables locales para manejar en el UPDATE
                        //conexionID = Convert.ToInt32(fi[0]);
                        //Periodo = Convert.ToInt32(fi["Periodo"]);
                        if (Convert.ToInt32(fi["ImpresionOBS"]) >= 500 & Convert.ToInt32(fi["ImpresionOBS"]) < 600)
                        {
                            //conexionID = fi.Field<Int32>("ConexionID");
                            //Cod_Impresion = fi.Field<int>("ImpresionCOD");                        
                            string update;//Declaración de string que contendra la consulta UPDATE               
                            update = "UPDATE infoconex SET CodigoImpresion = CodigoImpresion + 100 WHERE ConexionID = " + conexionID +
                                                           " AND Periodo = " + Periodo;
                            //preparamos la cadena pra insercion
                            command = new MySqlCommand(update, DB.conexBD);
                            //y la ejecutamos
                            command.ExecuteNonQuery();
                            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                            command.Dispose();
                        }

                    }
                    comandoSQL.Dispose();
                    datosAdapter.Dispose();


                }

            }

            catch (Exception r)
            {
                command.Dispose();
                comandoSQL.Dispose();
                datosAdapter.Dispose();

                MessageBox.Show(r.Message);
            }

        }


        private void simulateHeavyWork()
        {
            //Thread.Sleep(Vble.CantConex);

        }

        private void AbrirUnidadDeRed()
        {
            try
            {
                //Se establece la conexion con la unidad de red donde estará disponible el archivo encriptado que provee SAP para la importación
                IWshRuntimeLibrary.IWshNetwork2 network = new IWshRuntimeLibrary.WshNetwork();
                String localname = @"P:";
                String remotename = @"\\10.1.3.125\UPLOAD";
                Object updateprofile = Type.Missing;
                Object username = Vble.DominioYUsuarioRed;
                Object pass = Vble.ContraseñaRed;
                network.MapNetworkDrive(localname, remotename, ref updateprofile, ref username, ref pass);

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al conectar con la Unidad de Red");
            }
        }

        private void CerrarUnidadDeRed()
        {
            try
            {


                IWshRuntimeLibrary.IWshNetwork2 network = new IWshRuntimeLibrary.WshNetwork();
                Object updateprofile = Type.Missing;
                //buscamos todas las unidades de red para desconectar y no quede abierto el acceso a cualquier usuario
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
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al cerrar Unidad de Red");
            }
        }


        /// <summary>
        /// Cambia el estado impresionOBS de cada conexion a 06xx exportando a SAP de la base
        /// MySQL general donde estan todas las conexiones, respetando los codigos de impresion que se editaron en la lectura
        /// </summary>
        /// 
        public void CambiarEstadoExportadoMySql(Int32 conexionID, int StatusChange)
        {
            DataTable Tabla = new DataTable();

            //DataTable tablamedidores = new DataTable();
            try
            {
                string txSQL;
                MySqlDataAdapter datosAdapter;
                MySqlCommandBuilder comandoSQL;

                Int32 ImpresionOBS;

               
               txSQL = "SELECT * FROM Conexiones C " +
              "WHERE C.ConexionID = " + conexionID + " and C.Periodo = " + Vble.Periodo;

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    ////////asignación a variables locales para manejar en el UPDATE                   
                    ImpresionOBS = Convert.ToInt32(fi["ImpresionOBS"]);
                    string update = "";
                    if (ImpresionOBS.ToString().Length == 3)
                    {
                        if (ImpresionOBS.ToString().IndexOf('5',0,1) == 0)
                        //if (ImpresionOBS.ToString().Contains("5"))
                        {

                            ImpresionOBS = ImpresionOBS + 100;
                            //Declaración de string que contendra la consulta UPDATE               
                            update = "UPDATE Conexiones SET ImpresionOBS = " +
                                    //(ImpresionOBS.ToString().Replace("5", StatusChange.ToString())) +
                                    //(ImpresionOBS.ToString().Replace(ImpresionOBS.ToString().Substring(0, 1), StatusChange.ToString())) +
                                    ImpresionOBS +
                                     " WHERE conexionID = " + conexionID + " AND Periodo = " + Vble.Periodo;
                        }
                        //else if (ImpresionOBS.ToString().Contains("8"))
                        else if (ImpresionOBS.ToString().IndexOf('8', 0, 1) == 0)
                        {
                            //Declaración de string que contendra la consulta UPDATE               
                            update = "UPDATE Conexiones SET ImpresionOBS = " + 
                                     (ImpresionOBS.ToString().Replace(ImpresionOBS.ToString().Substring(0,1), StatusChange.ToString())) +
                                     " WHERE conexionID = " + conexionID + " AND Periodo = " + Vble.Periodo;
                        }
                        else
                        {
                            //Declaración de string que contendra la consulta UPDATE               
                            update = "UPDATE Conexiones SET ImpresionOBS = " + 600 +
                                                            " WHERE conexionID = " + conexionID + " AND Periodo = " + Vble.Periodo;
                        }
                    }


                    //preparamos la cadena pra insercion
                    MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //comandoSQL.Dispose();
                   
                    string updateinfoconex;//Declaración de string que contendra la consulta UPDATE               
                    updateinfoconex = "UPDATE infoconex SET CodigoImpresion = CodigoImpresion + 100 WHERE ConexionID = " + conexionID +
                                                   " AND Periodo = " + Vble.Periodo;
                    //preparamos la cadena pra insercion
                    command = new MySqlCommand(updateinfoconex, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();


                }

                comandoSQL.Dispose();
                datosAdapter.Dispose();


            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message);
            }
        }



        /// <summary>
        /// Metodo que genera el directorio Uploads en Respaldo y el archivo Uploads con los datos necesarios 
        /// UAAAAPP_LLLLLLLL_ULULULUL.aaaammdd_HHmmss.btx
        /// </summary>
        private void GenerarArchivoUpload()
        {
            string FechaUpload = DateTime.Now.ToString("yyyyMMdd");
            Int32 Lote, Distr, Ruta, Remesa;
            StringBuilder stb = new StringBuilder("", 250);
            Inis.GetPrivateProfileString("Archivos", "ExportCompr", "", stb, 250, Ctte.ArchivoIniName);
            string Comprimido = stb.ToString();
            LeerArchivoZonaFIS();


            try
            {
                //Vble.AbrirUnidadDeRed(@"Y:", @"\\10.1.3.125\UPLOAD");
                string RutaArchivoUpload = Vble.CarpetaRespaldo + "\\UPLOAD";
                ////Se establece la conexion con la unidad de red donde estará disponible el 
                ////archivo encriptado que provee SAP para la importación
                if (DB.Entorno == "PRD")
                {
                    Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaUpload);
                    RutaArchivoUpload = Vble.CarpetaRespaldo + "\\UPLOAD";
                    Vble.CarpetaExportacion = RutaArchivoUpload;
                }
                else if (DB.Entorno == "QAS")
                {
                    Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaUploadPrueba);
                    RutaArchivoUpload = Vble.CarpetaRespaldoQAS + "\\UPLOAD";
                    Vble.CarpetaExportacion = RutaArchivoUpload;
                }

                if (!Directory.Exists(RutaArchivoUpload))
                {
                    Directory.CreateDirectory(RutaArchivoUpload);
                }

                //RutaArchivoUpload = Vble.CarpetaRespaldo + "\\UPLOAD";
                //Vble.CarpetaExportacion = RutaArchivoUpload;
                // Comprimido = Vble.CarpetaExportacion + "\\" + Comprimido;

                //--INICIO DE FOR QUE GENERA UN ARCHIVO POR ZONA
                foreach (DataRow fi in TablaUpload1.Rows)
                {
                    Lote = fi.Field<int>("Lote");
                    Distr = fi.Field<int>("Zona");
                    Ruta = fi.Field<int>("Ruta");
                    Remesa = fi.Field<int>("Remesa");
                    Vble.Lote = Lote;
                    Vble.Distrito = Distr;
                    Vble.Ruta = Ruta;
                    Vble.Remesa = Remesa;

                    Distr = fi.Field<int>("zona");

                    if (Distr < 150) Vble.Distrito = Distr + 200;
                    TablaUpload2.Clear();

                    //ObtenerConexionesXloteYDistr(Vble.Lote, Distr, Vble.Ruta, Vble.Periodo, Vble.Remesa);
                    ObtenerConexionesXloteYDistr(Vble.Lote, Vble.Distrito, Vble.Ruta, Vble.Periodo, Vble.Remesa, 0);

                    //CopiarDatosEnHistorialDatosDPEC();

                    //BeginInvoke(new InvokeDelegate(InvoketerminarProgressBar));

                    //Obtengo el nombre que tomara el archivo de exportación desde el .ini (FACTURAS_RLLLrrrr_AAAAMMDDHHMMSS)
                    string nombrearchivoFACTURAS = ""; 
                    string nombrearchivoLECTURAS = "";
                    string nombreArchivoLOG = "";
                    string nombreArchivoGPS = "";


                    ///Verifica si esta seleccionado el check Upload por ruta, si esta tildado y 
                    ///selecciona mas de una ruta para exportar a la vez, generará un archivo de lectura, factura, y log
                    ///por ruta seleccionada. Si no esta tildada la opcion check upload por ruta y se selecciona mas de una
                    ///ruta a axportar a la vez, generará un solo archivo de lectura, facturas y log en el cual estarian
                    ///la/s rutas seleccionadas. con la particularidad que en el nombre del archivo no se informa las/s
                    ///rutas exportadas, se presentara seguido del numero de localidad la numeracion 0000 (espacio, para
                    ///informar la ruta exportada en caso de que se tilde el check Upload por ruta)
                    ///
                    if (UpPorRuta.Checked == true)
                    {
                        StringBuilder stb4 = new StringBuilder("", 250);
                        Inis.GetPrivateProfileString("Archivos", "ArchivoUploadFacIndiv", "", stb4, 250, Ctte.ArchivoIniName);
                        nombrearchivoFACTURAS = stb4.ToString();
                        StringBuilder stb1 = new StringBuilder("", 250);
                        Inis.GetPrivateProfileString("Archivos", "ArchivoUploadLecIndiv", "", stb1, 250, Ctte.ArchivoIniName);
                        nombrearchivoLECTURAS = stb1.ToString();
                        StringBuilder stb5 = new StringBuilder("", 250);
                        Inis.GetPrivateProfileString("Archivos", "ArchivoLogLecturasIndiv", "", stb5, 250, Ctte.ArchivoIniName);
                        nombreArchivoLOG = stb5.ToString();
                        StringBuilder stb6 = new StringBuilder("", 250);
                        Inis.GetPrivateProfileString("Archivos", "ArchivoGPSIndiv", "", stb6, 250, Ctte.ArchivoIniName);
                        nombreArchivoGPS = stb6.ToString();
                    }
                    else
                    {
                        if (TablaUpload1.Rows.Count == 1)
                        {
                            StringBuilder stb4 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoUploadFacIndiv", "", stb4, 250, Ctte.ArchivoIniName);
                            nombrearchivoFACTURAS = stb4.ToString();
                            StringBuilder stb1 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoUploadLecIndiv", "", stb1, 250, Ctte.ArchivoIniName);
                            nombrearchivoLECTURAS = stb1.ToString();
                            StringBuilder stb5 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoLogLecturasIndiv", "", stb5, 250, Ctte.ArchivoIniName);
                            nombreArchivoLOG = stb5.ToString();
                            StringBuilder stb6 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoGPSIndiv", "", stb6, 250, Ctte.ArchivoIniName);
                            nombreArchivoGPS = stb6.ToString();
                        }
                        else
                        {
                            StringBuilder stb4 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoUploadFac", "", stb4, 250, Ctte.ArchivoIniName);
                            nombrearchivoFACTURAS = stb4.ToString();
                            StringBuilder stb1 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoUploadLec", "", stb1, 250, Ctte.ArchivoIniName);
                            nombrearchivoLECTURAS = stb1.ToString();
                            StringBuilder stb5 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoLogLecturas", "", stb5, 250, Ctte.ArchivoIniName);
                            nombreArchivoLOG = stb5.ToString();
                            StringBuilder stb6 = new StringBuilder("", 250);
                            Inis.GetPrivateProfileString("Archivos", "ArchivoGPSIndiv", "", stb6, 250, Ctte.ArchivoIniName);
                            nombreArchivoGPS = stb6.ToString();
                        }
                       
                    }


                    ////Obtengo el nombre que tomara el archivo de exportación desde el .ini (UyyyyMM_00000000_00000000_AAAAMMDDHHMMSS)
                    //StringBuilder stb0 = new StringBuilder("", 250);
                    //Inis.GetPrivateProfileString("Archivos", "ArchivoImpresos", "", stb0, 250, Ctte.ArchivoIniName);
                    //string archivoImpresos = stb0.ToString();

                    //Obtengo la extension con la que se generara el archivo(.btx, .txt) modificable en el .ini
                    StringBuilder stb2 = new StringBuilder("", 250);
                    Inis.GetPrivateProfileString("Datos", "ExtencionArchExpor", "", stb2, 250, Ctte.ArchivoIniName);
                    string extension = stb2.ToString();

                    //Obtengo la extension con la que se generara el archivo encriptado (.gpg)  modificable en el .ini
                    StringBuilder stb3 = new StringBuilder("", 250);
                    Inis.GetPrivateProfileString("Datos", "ExtensionArchiEncrip", "", stb3, 250, Ctte.ArchivoIniName);
                    string extensionGPG = stb3.ToString();

                    //string archivozip = RutaArchivoUpload + "\\" + ValorarUnNombreRuta(nombrearchivo);
                    string rutadelarhivo = RutaArchivoUpload + "\\";

                    nombrearchivoLECTURAS = ValorarUnNombreRuta(nombrearchivoLECTURAS);
                    nombrearchivoFACTURAS = ValorarUnNombreRuta(nombrearchivoFACTURAS);
                    nombreArchivoLOG = ValorarUnNombreRuta(nombreArchivoLOG);
                    nombreArchivoGPS = ValorarUnNombreRuta(nombreArchivoGPS);

                    //Asigno el nombre de los directorios donde quedarán los archivos UPLOAD de exportación
                    string RutaArchivoExcel = Vble.CarpetaUploadSap + FechaUpload + "\\" + nombrearchivoLECTURAS + ".xlsx";
                    string RutaArchivoImpresos = RutaArchivoUpload + "\\" + nombrearchivoFACTURAS + extension;
                    string RutaArchivoLog = RutaArchivoUpload + "\\" + nombreArchivoLOG + extension;
                    string RutaArchivoGPS = Vble.CarpetaGPSPRD;
                    
                    RutaArchivoUpload = RutaArchivoUpload + "\\" + nombrearchivoLECTURAS + extension;

                    if (DB.Entorno == "PRD")
                    {
                        //////Copio los archivos .BTX Y .GPG en las carpetas de UPLOAD de la NAS
                        //if (!Directory.Exists(Vble.CarpetaUpload + "\\" + Vble.ArrayZona[0].ToString()))
                        //{
                        //    Directory.CreateDirectory(Vble.CarpetaUpload + "\\" + Vble.ArrayZona[0].ToString());
                        //}
                        if (!Directory.Exists(Vble.CarpetaUpload + "\\" + Vble.Distrito.ToString()))
                        {
                            Directory.CreateDirectory(Vble.CarpetaUpload + "\\" + Vble.Distrito.ToString());
                        }

                        //if (!Directory.Exists(Vble.CarpetaUploadSap + Vble.ArrayZona[0].ToString()))
                        //{
                        //    Directory.CreateDirectory(Vble.CarpetaUploadSap + Vble.ArrayZona[0].ToString());
                        //}
                        if (!Directory.Exists(Vble.CarpetaUploadSap + Vble.Distrito.ToString()))
                        {
                            Directory.CreateDirectory(Vble.CarpetaUploadSap + Vble.Distrito.ToString());
                        }

                        ///Si no existe la carpeta GPS en PRD lo crea
                        if (!Directory.Exists(Vble.CarpetaGPSPRD))
                        {
                            Directory.CreateDirectory(Vble.CarpetaGPSPRD);
                        }

                        RutaArchivoGPS = Vble.CarpetaGPSPRD + nombreArchivoGPS + ".btx";
                    }
                    else if (DB.Entorno == "QAS")
                    {
                        //////Copio los archivos .BTX Y .GPG en las carpetas de UPLOAD de la NAS
                        //if (!Directory.Exists(Vble.CarpetaUploadPrueba + "\\" + Vble.ArrayZona[0].ToString()))
                        //{
                        //    Directory.CreateDirectory(Vble.CarpetaUploadPrueba + "\\" + Vble.ArrayZona[0].ToString());
                        //}
                        if (!Directory.Exists(Vble.CarpetaUploadPrueba + "\\" + Vble.Distrito.ToString()))
                        {
                            Directory.CreateDirectory(Vble.CarpetaUploadPrueba + "\\" + Vble.Distrito.ToString());
                        }

                        //if (!Directory.Exists(Vble.CarpetaUploadProcesados + Vble.ArrayZona[0].ToString()))
                        //{
                        //    Directory.CreateDirectory(Vble.CarpetaUploadProcesados + Vble.ArrayZona[0].ToString());
                        //}
                        if (!Directory.Exists(Vble.CarpetaUploadProcesados + Vble.Distrito.ToString()))
                        {
                            Directory.CreateDirectory(Vble.CarpetaUploadProcesados + Vble.Distrito.ToString());
                        }

                        ///Si no existe la carpeta GPS en QAS lo crea
                        if (!Directory.Exists(Vble.CarpetaGPSQAS))
                        {
                            Directory.CreateDirectory(Vble.CarpetaGPSQAS);
                        }

                        RutaArchivoGPS = Vble.CarpetaGPSQAS + nombreArchivoGPS + ".btx";
                    }
                    

                    //*******Llamo al metodo que crea los archivos de exportacion .btx*********
                    CrearArchivoUpload(RutaArchivoUpload, nombrearchivoLECTURAS + extension, "", RutaArchivoExcel, RutaArchivoImpresos, RutaArchivoLog, RutaArchivoGPS);

                    //*******Llamo al metodo que encriptará el archivo .btx de exportación********
                    //Vble.EncriptarArchivo(ArchivoPLANO + extension, rutadelarhivo);

                    ////Copia en la Carpeta \\10.1.3.125\upload\yyyyMMdd\"nombrearchivoupload.gpg" el archivo en formato encriptado
                    //File.Copy(RutaArchivoUpload + extensionGPG, Vble.CarpetaUpload + "\\" + FechaUpload + "\\" + ArchivoPLANO + extension + extensionGPG);

                    //Copia en la Carpeta \\10.1.3.125\upload\SAP\yyyyMMdd\"nombrearchivoupload.btx" el archivo btx con las conexiones que se exportaron
                    if (DB.Entorno == "PRD")
                    {
                        //if (File.Exists(RutaArchivoUpload))
                        //{
                        //    File.Copy(RutaArchivoUpload, Vble.CarpetaUploadSap + Vble.ArrayZona[0].ToString() + "\\" + nombrearchivoLECTURAS + extension);
                        //}
                        if (File.Exists(RutaArchivoUpload))
                        {
                            File.Copy(RutaArchivoUpload, Vble.CarpetaUploadSap + Vble.Distrito.ToString() + "\\" + nombrearchivoLECTURAS + extension);
                        }

                        //if (File.Exists(RutaArchivoImpresos))
                        //{
                        //    File.Copy(RutaArchivoImpresos, Vble.CarpetaUploadSap + Vble.ArrayZona[0].ToString() + "\\" + nombrearchivoFACTURAS + extension);
                        //}
                        if (File.Exists(RutaArchivoImpresos))
                        {
                            File.Copy(RutaArchivoImpresos, Vble.CarpetaUploadSap + Vble.Distrito.ToString() + "\\" + nombrearchivoFACTURAS + extension);
                        }


                        //if (File.Exists(RutaArchivoLog))
                        //{
                        //    File.Copy(RutaArchivoLog, Vble.CarpetaUploadSap + Vble.ArrayZona[0].ToString() + "\\" + nombreArchivoLOG + extension);
                        //}
                        if (File.Exists(RutaArchivoLog))
                        {
                            File.Copy(RutaArchivoLog, Vble.CarpetaUploadSap + Vble.Distrito.ToString() + "\\" + nombreArchivoLOG + extension);
                        }
                        //File.Copy(RutaArchivoImpresos, Vble.CarpetaUploadSap + Vble.ArrayZona[0].ToString() + "\\" + nombrearchivoFACTURAS + extension);
                    }
                    else if (DB.Entorno == "QAS")
                    {
                        //if (File.Exists(RutaArchivoUpload))
                        //{
                        //    File.Copy(RutaArchivoUpload, Vble.CarpetaUploadPrueba + "\\" + Vble.ArrayZona[0].ToString() + "\\" + nombrearchivoLECTURAS + extension);
                        //}
                        if (File.Exists(RutaArchivoUpload))
                        {
                            File.Copy(RutaArchivoUpload, Vble.CarpetaUploadPrueba + "\\" + Vble.Distrito.ToString() + "\\" + nombrearchivoLECTURAS + extension);
                        }

                        //if (File.Exists(RutaArchivoLog))
                        //{
                        //    File.Copy(RutaArchivoLog, Vble.CarpetaUploadPrueba + "\\" + Vble.ArrayZona[0].ToString() + "\\" + nombreArchivoLOG + extension);
                        //}
                        if (File.Exists(RutaArchivoLog))
                        {
                            File.Copy(RutaArchivoLog, Vble.CarpetaUploadPrueba + "\\" + Vble.Distrito.ToString() + "\\" + nombreArchivoLOG + extension);
                        }
                        //if (File.Exists(RutaArchivoImpresos))
                        //{
                        //    File.Copy(RutaArchivoImpresos, Vble.CarpetaUploadPrueba + "\\" + Vble.ArrayZona[0].ToString() + "\\" + nombrearchivoFACTURAS + extension);
                        //}
                        if (File.Exists(RutaArchivoImpresos))
                        {
                            File.Copy(RutaArchivoImpresos, Vble.CarpetaUploadPrueba + "\\" + Vble.Distrito.ToString() + "\\" + nombrearchivoFACTURAS + extension);
                        }
                    }
                    //if (!Directory.Exists(Vble.CarpetaUploadPrueba + FechaUpload))
                    //{
                    //    Directory.CreateDirectory(Vble.CarpetaUploadPrueba + FechaUpload);
                    //}
                    ////Copia en la Carpeta I:\UploadPrueba\yyyyMMdd\"nombrearchivoupload.btx"(Carpeta de prueba) el
                    ////archivo btx con las conexiones que se exportaron 
                    //File.Copy(RutaArchivoUpload, Vble.CarpetaUploadPrueba + FechaUpload + "\\" + ArchivoPLANO + extension);                    
                    //File.Copy(RutaArchivoUpload, )                      

                    Ctte.ArchivoLogEnzo.EscribirLog("Localidad: " + Vble.Distrito.ToString() + " Se Exportaron los archivos: | " + RutaArchivoUpload +  
                                                    " | " + RutaArchivoImpresos +
                                                    " | " + RutaArchivoLog + " |");
                    //una vez que Encripto elimino el archivo del temporal

                    Vble.Lote = Lote;
                    Vble.Distrito = Distr;
                    Vble.Ruta = Ruta;
                    Vble.Remesa = Remesa;


                    if (Vble.PanelLogExp.ToString() == "1")
                    {
                        AgregarRutaEnLogExportacion(Vble.Periodo, Vble.Distrito, Vble.Ruta, Vble.Remesa, Vble.CantConex, Vble.CANTImpresos, Vble.CANTLecturas, Vble.CANTLog);
                    }

                    File.Delete(RutaArchivoUpload);
                    File.Delete(RutaArchivoImpresos);
                    File.Delete(RutaArchivoLog);
                    //File.Delete(RutaArchivoUpload + extensionGPG);

                    RutaArchivoUpload = Vble.CarpetaRespaldo + "\\UPLOAD";
                    nombrearchivoFACTURAS = "";
                    nombrearchivoLECTURAS = "";
                    nombreArchivoLOG = "";
                }//_------------------------------CIERRE DE FOR QUE GENERA 1 ARCHIVO POR ZONA


                
                ////buscamos todas las unidades de red para desconectar y no quede abierto el acceso a cualquier usuario       
                Vble.CerrarUnidadDeRed();
                //ComprimirListaDeArchivos(Vble.CarpetaExportacion, Comprimido);

            }
            catch (Exception r)
            {
                if (r.Message.Contains("No hay ninguna fila en la posición 0."))
                {

                }
                else
                {
                    MessageBox.Show(r.Message + " Erro al generar Archivo Upload. -Método GenerarArchivoUpload()");
                }
                
            }
        }

        private void AgregarRutaEnLogExportacion(int periodo, int distrito, int ruta, int remesa, int cantConex, int cantImpresos,  int cantLecturas,  int cantLog)
        {


            string porcion = remesa.ToString() + ruta.ToString();

            int totalUsuariosDeRuta = ObtenerTotalRuta(periodo, distrito, ruta);

            int saldo = ObtenerSaldo(periodo, distrito, ruta);

            string InsertLogImp = "INSERT INTO LogExportacion(Periodo, Zona, Porcion, CantUsuarios, CantExportados, CantFact, " +
                               "CantLect, CantLog, Saldo, Operario, Fecha, Hora)" +
                               " VALUES (" + periodo + ", " + distrito + ", '" + porcion + "', " + totalUsuariosDeRuta + ", " + cantConex + ", " + cantImpresos + ", " +
                                           cantLecturas + ", " + cantLog + ", " + saldo + ", '" + DB.sDbUsu + "', '" +
                                           DateTime.Today.Date.ToString("yyyy-MM-dd") + "', '" + DateTime.Now.ToString("hh:mm:ss") + "')";

            //preparamos la cadena pra insercion
            MySqlCommand command = new MySqlCommand(InsertLogImp, DB.conexBD);
            //y la ejecutamos
            command.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            command.Dispose();
        }


        /// <summary>
        /// Consulta y devuelve la cantidad de usuarios que contiene la ruta leidos y no leidos,
        /// que se pasa por parametro, junto con el periodo y el distrito/zona
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="distrito"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private int ObtenerTotalRuta(int periodo, int distrito, int ruta)
        {
            int Cant = 0;
            MySqlCommand da;

            string txSQL;
            DataTable Tabla = new DataTable();
            try
            {
                txSQL = "select Count(*) From Conexiones Where (Zona = " + distrito + " and Ruta = " + ruta + " and Periodo = " + periodo + ")";
                da = new MySqlCommand(txSQL, DB.conexBD);
                Cant = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            return Cant;
        }


        /// <summary>
        /// Consulta y devuelve la cantidad de saldos de la ruta que se pasa por parametro, junto con el periodo y el distrito/zona
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="distrito"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private int ObtenerSaldo(int periodo, int distrito, int ruta)
        {
            int Cant = 0;
            MySqlCommand da;

            string txSQL;
            DataTable Tabla = new DataTable();
            try
            {
                txSQL = "select Count(*) From Conexiones Where (Zona = " + distrito + " and Ruta = " + ruta + " and Periodo = " + periodo + ") AND (ImpresionOBS <= 500 OR ImpresionOBS = 800)";                
                da = new MySqlCommand(txSQL, DB.conexBD);
                Cant = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();
                
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            return Cant;
        }

        /// <summary>
        /// metodo que realiza esas instrucciones fuera de la funcion donde se tendría que realizar,
        /// pertenecen a la funcion "Realizar Exportación"
        /// </summary>
        public void InvoketerminarProgressBar()
        {
            //ObtenerConexionesXloteYDistr(Vble.Lote, Vble.Distrito, Vble.Ruta, Vble.Periodo, Vble.Remesa, 0);
            //AgregarNodoEmpresaExportadas(Vble.Empresa, "logo");
        }


        /// <summary>
        /// Metodo que crea las lineas de cada ruta a exportar, y este a su vez contiene la funcion
        /// que crea el archivo .txt con esa linea armada segun formato de Exportación (Archivo Upload)
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        private void CrearArchivoUpload(string archivosecuencia, string filename, string colectora, string archivoexcel, 
                                        string archivoImpresos, string archivoLog, string archivoGPS)
        {

            int j = 0;
            int conSaldo = 0, sinSaldo = 0;
            int NroLinea = 0;
            //DataTable dtLeidosImpresos = new DataTable();
            //DataTable dtLeidosFueraDeRango = new DataTable();
            //dtLeidosFueraDeRango.Columns.Add("Usuarios que dieron FUERA DE RANGO");
            //dtLeidosImpresos.Columns.Add("Usuarios Leidos IMPRESOS");
            //DataRow NuevaFila = dtLeidosImpresos.NewRow();
            try
            {
                Vble.LineaHFS = "";
                Vble.LineaHNC = "";
                Vble.lineas = "";
                Vble.LineasGPS = "";
                Vble.CANTLecturas = 0;
                Vble.CANTLecturas = 0;
                Vble.CANTLog = 0;
                //Vble.LineaLeidosImpresos = "";
                //Vble.LineaLeidosFueraDeRango = "";
                Vble.NºInstalacionImpresos.Clear();
                Vble.ContratoImpresos.Clear();
                Vble.TitularImpresos.Clear();
                Vble.FacturaImpresos.Clear();
                Vble.NºInstalacionFueraDeRango.Clear();
                Vble.ContratoFueraDeRango.Clear();
                Vble.TitularFueraDeRango.Clear();
                Vble.ObservacionesFueraDeRango.Clear();
                string ImpresionOBS = "1", Operario = " ", ConsumoFacturado = " ", FechaCalp = "", FacturaLetra = " ", PuntoVenta = " ", FacturaNro1 = " ", Importe1 = " ",
                       ImporteBasico1 = " ", ImporteImpuesto1 = " ", FacturaNro2 = " ", Importe2 = " ", ImporteBasico2 = " ", ImporteImpuesto2 = " ",
                       ActualFecha = " ", ActualHora = " ", ActualEstado = " ", TipoLectura = " ", Observacion = "", Lote = "", Zona = "", Ruta = "", Remesa = "", OrdenTomado = "";
                int EstadoImpresion = 0, PuntoVentaImpresion = 0, FacturaN1Impresion = 0, Orden = 0;
                Int32 Modelo = 0;
                Int64 Numero = 0;
                decimal Latitud = 0, Longitud = 0;

                /// Verifica si esta seleccionado el checkbox "SI" que hace referencia a exportar todas las conexiones
                /// incluso las que no fueron leidas
                if (this.RadioButtonSI.Checked == true)
                {
                    //toma las lineas de a uno con la informacion de la carga que se va a procesar                                 
                    foreach (DataRow fi in TablaUpload2.Rows)
                    {
                        //Verifica si la conexion ya no ha sido exportada y contenga en ImpresionOBS el codigo 6xx 
                        //para no volver a enviarla dentro de un nuevo Upload
                        //if (!fi.Field<int>("ImpresionOBS").ToString().Contains("6"))
                        if (fi.Field<int>("ImpresionOBS") >= 500 & fi.Field<int>("ImpresionOBS") < 600 || fi.Field<int>("ImpresionOBS") < 800)
                        {

                            EstadoImpresion = fi.Field<int>("ImpresionOBS") - 500;

                            ////AGREGO CONEXIONES EXPORTADAS A LA BASE HISTORIALDATOSDPEC
                            ////-----------------------------------------------------------------------------------------------------------------------------------
                            //CopiarDatosEnHistorialDatosDPEC(Vble.Periodo, Vble.Distrito, Vble.Ruta, fi.Field<Int32>("ConexionID"), fi.Field<Int32>("PersonaID"));

                            ////-----------------------------------------------------------------------------------------------------------------
                            foreach (DataColumn col in TablaUpload2.Columns)
                            {
                                ////////Esta condición permite que solo se genere el Uploads con las conexiones que tienen
                                ////////ImpresionOBS distinto de 0, los que tienen ImpresionOBS = 0 quedan para volver a salir
                                //if (ImpresionOBS != "0")
                                if (col.ColumnName == "ConexionID")
                                {
                                    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                                    ////Este If transforma el dato ImpresionOBS que esta en la base general con extension de 3 digitos
                                    ////el cual significa que los datos han sido descargados de las colectoras y 
                                    ////los convierte a numeros de dos digitos para generar la linea Upload.
                                    if (fi.Field<int>("ImpresionOBS") == 500 || fi.Field<int>("ImpresionOBS") == 501 || fi.Field<int>("ImpresionOBS") == 502 || fi.Field<int>("ImpresionOBS") == 509)
                                    {
                                        ImpresionOBS = (fi.Field<int>("ImpresionOBS") - 500).ToString();
                                        EstadoImpresion = fi.Field<int>("ImpresionOBS") - 500;
                                    }
                                    else
                                    {
                                        ImpresionOBS = "2";
                                        EstadoImpresion = 2;
                                    }

                                    CambiarEstadoExportadoMySql(fi.Field<Int32>("ConexionID"), (int)cteCodEstado.Exportado);
                                    ConsumoFacturado = " ";

                                    if (EstadoImpresion > 0)
                                        ConsumoFacturado = fi.Field<int>("ConsumoFacturado").ToString();

                                    if (fi.Field<Int32>("ActualEstado") == 0 & fi.Field<string>("ActualHora") != "00:00")
                                    {
                                        ActualEstado = "0";
                                    }
                                    else if (fi.Field<Int32>("ActualEstado") == -1)//Si el estado del usuario es -1 quiere decir que es
                                                                                   // una lectura imposible y sap no toma valores negativos
                                                                                   // como estado, entonces se carga 1 el cual luego dentro de SAP 
                                                                                   // se corrige manualmente
                                    {
                                        ActualEstado = "1";
                                    }
                                    //else if (fi.Field<Int32>("ActualEstado") == -2)//Si el estado del usuario es -2 quiere decir que es
                                    //                                              // una lectura marcada como medidor apagado y sap no toma valores negativos
                                    //                                              // como estado, entonces se carga 2 el cual luego dentro de SAP 
                                    //                                              // se corrige manualmente
                                    //{
                                    //    ActualEstado = "2";
                                    //}
                                    else
                                    {
                                        ActualEstado = fi.Field<Int32>("ActualEstado").ToString();
                                    }

                                    ///Este inicio de if verifica primero si esta habilitado el cambio de fecha de lectura para la exportacion
                                    ///segundo: si el combobox de la fecha no esta vacio. Formatea la fecha a yyyyMMdd.
                                    ///por el Else toma la fecha real en el que se tomo la lectura y tambien la formatea a yyyyMMdd formato definido
                                    ///para enviar la fecha de lectura.
                                    if (GBCambioFecha.Visible == true)
                                    {
                                        if (TBFechaModificada.Text != "")
                                        {
                                            // MessageBox.Show(Convert.ToDateTime(TBFechaModificada.Text).ToString("yyyy-MM-dd"));
                                            FechaCalp = Convert.ToDateTime(TBFechaModificada.Value.Date, CultureInfo.CurrentCulture).ToString("yyyyMMdd");
                                        }
                                    }
                                    else
                                    {
                                        if ((fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd") != "2000-01-01" || fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd") != "0"
                                        ) & fi.Field<DateTime>("ActualFecha").ToString().Length == 10)
                                        {
                                            FechaCalp = $"{fi.Field<DateTime>("ActualFecha").ToString().Replace("/", "")}";
                                            FechaCalp = $"{fi.Field<DateTime>("ActualFecha").ToString().Replace("-", "")}";
                                            Vble.FechaCalp = FechaCalp;
                                        }
                                        else
                                        {
                                            FechaCalp = Vble.FechaCalp;
                                        }
                                    }                                   
                                    Operario = fi.Field<int>("Operario").ToString() == "0" ? " " : fi.Field<int>("Operario").ToString("000");

                                    if (NroLinea == 0)
                                    {
                                        if (EstadoImpresion == 1)
                                        {
                                            Vble.CANTImpresos++;
                                            Vble.LineaImpresos += $"{fi.Field<Int32>("OpBel").ToString()}|X";
                                            Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                        }
                                        else if (EstadoImpresion > 1)
                                        {

                                            if (EstadoImpresion == 17)
                                            {

                                            }
                                            else
                                            {
                                                Vble.CANTLecturas++;
                                            Vble.LineaHFSindiv +=
                                                            $"{fi.Field<string>("OrdenLectura")}|{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                                            $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                                            $"{FechaCalp}|";

                                            ////verifico si la conexion que se esta procesando tiene novedades
                                            ////de ser asi busca sus novedades y agrega a la linea sino no agrega la linea Novedades(HNC)
                                            if (ExisteNovedades(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo")) || (fi.Field<int>("ImpresionCOD").ToString().Trim() == "2"))
                                            {
                                                CargarNovedadesConex(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo"), j, Operario, ActualEstado, fi.Field<string>("Numero"));
                                            }
                                            else
                                            {
                                                if (Vble.LineaHFSindiv != "")
                                                {
                                                    Vble.LineaHNC = $"{ActualEstado}||||||";
                                                    Vble.LineaHFSindiv += $"{Vble.LineaHNC}|{Operario}\n";
                                                    Vble.LineaHNC = "";
                                                }
                                            }
                                            Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                            }
                                        }
                                        NroLinea++;
                                    }
                                    else
                                    {

                                        if (EstadoImpresion == 1)
                                        {
                                            Vble.CANTImpresos++;
                                            if (Vble.LineaImpresos == "")
                                            {
                                                Vble.LineaImpresos += $"{fi.Field<Int32>("OpBel").ToString()}|X";
                                            }
                                            else
                                            {
                                                Vble.LineaImpresos += $"\n{fi.Field<Int32>("OpBel").ToString()}|X";
                                            }
                                            Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                        }
                                        else if (EstadoImpresion > 1)
                                        {

                                            if (EstadoImpresion == 17)
                                            {

                                            }
                                            else
                                            {

                                                Vble.CANTLecturas++;
                                            Vble.LineaHFSindiv +=
                                            //$"{fi.Field<string>("OrdenLectura")}|{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                            // $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                            // $"{FechaCalp}|{ActualEstado}";
                                            $"{fi.Field<string>("OrdenLectura")}|{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                            $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                            $"{FechaCalp}|";

                                                ////verifico si la conexion que se esta procesando tiene novedades
                                                ////de ser asi busca sus novedades y agrega a la linea sino no agrega la linea Novedades(HNC)
                                                if (ExisteNovedades(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo")) || (fi.Field<int>("ImpresionCOD").ToString().Trim() == "2"))
                                                {
                                                CargarNovedadesConex(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo"), j, Operario, ActualEstado, fi.Field<string>("Numero"));
                                            }
                                            else
                                            {
                                                if (Vble.LineaHFSindiv != "")
                                                {
                                                    Vble.LineaHNC = $"{ActualEstado}||||||";
                                                    Vble.LineaHFSindiv += $"{Vble.LineaHNC}|{Operario}\n";
                                                    Vble.LineaHNC = "";
                                                }
                                            }
                                            Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                            }
                                        }

                                        if (ExistenRegistrosLogErrores(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo")))
                                        {
                                            Vble.LineaLogErrores += ArmarLineaLog(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo"));
                                        }
                                        NroLinea++;


                                    }
                                }
                            }

                            //vacia el Array que para cargar las novedades de la proxima Conexion en caso de que tenga
                            for (int i = 0; i < 5; i++)
                            {
                                ArrayCodNovedades[i] = "";

                            }
                            //Vble.LineaHFS = "";
                            if (Vble.LineaHFSindiv != "")
                            {
                                Vble.LineaHFS += Vble.LineaHFSindiv;
                            }
                            Vble.LineaHFSindiv = "";
                        }//cierre de if de checkbox Improtar todo                                               
                            TareaSegundoPlano1.ReportProgress(ñ);
                            ñ++;                       
                        sinSaldo++;
                        //ModificarInfoConex(fi.Field<Int32>("ConexionID"), fi.Field<int>("Periodo"), DateTime.Today.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), DB.sDbUsu, 6, "Upload");
                    }//cierra el if que controla que la conexion ya no ha sido exportada con codigo 6xx
                }

                ///Si no esta seleccionado el checkbox SI verifica por el NO para exportar 
                ///todas las conexiones excepto las que no fueron leidas, van a quedar en la base de datos
                ///para generar nuevamente la carga y que puedan salir al otro día
                else if (RadioButtonNO.Checked == true)
                {
                    //crea las lineas con la informacion de la carga que se va a procesar                                 
                    foreach (DataRow fi in TablaUpload2.Rows)
                    {
                        if (fi.Field<int>("ImpresionOBS") != 500 )
                        {
                            ////AGREGO CONEXIONES EXPORTADAS A LA BASE HISTORIALDATOSDPEC
                            ////-----------------------------------------------------------------------------------------------------------------------------------
                            //CopiarDatosEnHistorialDatosDPEC(Vble.Periodo, Vble.Distrito, Vble.Ruta, fi.Field<Int32>("ConexionID"), fi.Field<Int32>("PersonaID"));
                            ////-----------------------------------------------------------------------------------------------------------------                            
                            CambiarEstadoExportadoMySql(fi.Field<Int32>("ConexionID"), (int)cteCodEstado.Exportado);

                            foreach (DataColumn col in TablaUpload2.Columns)
                            {
                                if (col.ColumnName == "ConexionID")
                                {
                                    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                                    ////Este If transforma el dato ImpresionOBS que esta en la base general con extension de 3 digitos
                                    ////el cual significa que los datos han sido descargados de las colectoras y 
                                    ////los convierte a numeros de dos digitos para generar la linea Upload.
                                    if (fi.Field<int>("ImpresionOBS") == 501 || fi.Field<int>("ImpresionOBS") == 502 || fi.Field<int>("ImpresionOBS") == 509)
                                    {
                                        ImpresionOBS = (fi.Field<int>("ImpresionOBS") - 500).ToString();
                                        EstadoImpresion = fi.Field<int>("ImpresionOBS") - 500;
                                    }
                                    else
                                    {
                                        ImpresionOBS = "2";
                                        EstadoImpresion = 2;
                                    }

                                    if (fi.Field<Int32>("ActualEstado") == 0 & fi.Field<string>("ActualHora") != "00:00")
                                    {
                                        ActualEstado = "0";
                                    }
                                    else if (fi.Field<Int32>("ActualEstado") == -1)//Si el estado del usuario es -1 quiere decir que es
                                                                                   // una lectura imposible y sap no toma valores negativos
                                                                                   // como estado, entonces se carga 1 el cual luego dentro de SAP 
                                                                                   // se corrige manualmente
                                    {
                                        ActualEstado = "1";
                                    }
                                    //else if (fi.Field<Int32>("ActualEstado") == -2)//Si el estado del usuario es -2 quiere decir que es
                                    //                                              // una lectura marcada como medidor apagado y sap no toma valores negativos
                                    //                                              // como estado, entonces se carga 2 el cual luego dentro de SAP 
                                    //                                              // se corrige manualmente
                                    //{
                                    //    ActualEstado = "2";
                                    //}
                                    else
                                    {
                                        ActualEstado = fi.Field<Int32>("ActualEstado").ToString();
                                    }

                                    if (ActualEstado != "")
                                    {
                                        ///Este inicio de if verifica primero si esta habilitado el cambio de fecha de lectura para la exportacion
                                        ///segundo: si el combobox de la fecha no esta vacio. Formatea la fecha a yyyyMMdd.
                                        ///por el Else toma la fecha real en el que se tomo la lectura y tambien la formatea a yyyyMMdd formato definido
                                        ///para enviar la fecha de lectura.
                                        if (GBCambioFecha.Visible == true)
                                        {
                                            if (TBFechaModificada.Text != "")
                                            {                                                
                                                FechaCalp = Convert.ToDateTime(TBFechaModificada.Value.Date, CultureInfo.CurrentCulture).ToString("yyyyMMdd");
                                            }
                                        }
                                        else
                                        {
                                            if ((fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd") != "2000-01-01" || fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd") != "0")
                                                & fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd").Length == 10)
                                            {                                           
                                                FechaCalp = $"{fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd").Replace("/", "")}";
                                                FechaCalp = $"{fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd").Replace("-", "")}";
                                                Vble.FechaCalp = FechaCalp;
                                            }
                                            else
                                            {
                                                FechaCalp = Vble.FechaCalp;
                                            }
                                        }
                                        ///fin verificacion cambio de fecha

                                        ///asigno operario leido a variable Operario para usarlo en los proximos armados de cadenas.
                                        Operario = fi.Field<int>("Operario").ToString() == "0" ? "" : fi.Field<int>("Operario").ToString("000");

                                        ///Si el numero de linea es la primera, ingresa por el true del IF y arma sin salto de linea al inicio ambos 
                                        ///string, el de:
                                        ///Facturas(vble.LineaImpresos | EstadoImpresion = 1)
                                        ///Lecturas(vble.LineaHFSindiv | EstadoImpresion > 1)
                                        if (NroLinea == 0)
                                        {
                                            if (EstadoImpresion == 1)
                                            {
                                                Vble.CANTImpresos++;
                                                Vble.LineaImpresos += $"{fi.Field<Int32>("OpBel").ToString()}|X";
                                                Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                            }
                                            else if (EstadoImpresion > 1)
                                            {
                                                if (EstadoImpresion == 17)
                                                {

                                                }
                                                else
                                                {
                                                    Vble.CANTLecturas++;
                                                    Vble.LineaHFSindiv +=
                                                                    $"{fi.Field<string>("OrdenLectura")}|{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                                                    $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                                                    $"{FechaCalp}|";

                                                    ////verifico si la conexion que se esta procesando tiene novedades
                                                    ////de ser asi busca sus novedades y agrega a la linea sino no agrega la linea Novedades(HNC)
                                                    if (ExisteNovedades(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo")) || (fi.Field<int>("ImpresionCOD").ToString().Trim() == "2"))
                                                    {
                                                        CargarNovedadesConex(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo"), j, Operario, ActualEstado, fi.Field<string>("Numero"));
                                                    }
                                                    else
                                                    {
                                                        if (Vble.LineaHFSindiv != "")
                                                        {
                                                            Vble.LineaHNC = $"{ActualEstado}||||||";
                                                            Vble.LineaHFSindiv += $"{Vble.LineaHNC}|{Operario}\n";
                                                            Vble.LineaHNC = "";
                                                        }
                                                    }

                                                    if (Funciones.EnergiaReactivaGU(fi.Field<int>("ConexionID"), Vble.Periodo, fi.Field<string>("Numero")) != "")
                                                    {
                                                        Int64 OrdenLecGU = Convert.ToInt64(fi.Field<string>("OrdenLectura")) + 1;

                                                        Vble.LineaHFSindiv +=
                                                                            $"{OrdenLecGU.ToString("00000000000000000000")}|" +
                                                                            $"{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                                                            $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|" +
                                                                            $"{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                                                            $"{FechaCalp}|" +
                                                                            $"{Vble.EnergiaReactiva}||||||{Operario}\n";

                                                    }

                                                    Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                                  $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                                  $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                                  $"{ fi.Field<string>("Numero").ToString()}|" +
                                                                  $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                                  $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                                  $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                                  $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                                  $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                                }
                                            }
                                            NroLinea++;
                                        }///Fin IF que arma primer linea de cada archivo Facturas y Lecturas.
                                        else///Por el else arma lo mismo que por el true del if pero con saltos de linea para respetar la estructura del archivo.
                                        {
                                            if (EstadoImpresion == 1)
                                            {
                                                Vble.CANTImpresos++;
                                                if (Vble.LineaImpresos == "")
                                                {
                                                    Vble.LineaImpresos += $"{fi.Field<Int32>("OpBel").ToString()}|X";
                                                    Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                                }
                                                else
                                                {
                                                    Vble.LineaImpresos += $"\n{fi.Field<Int32>("OpBel").ToString()}|X";
                                                    Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                              $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                              $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                              $"{ fi.Field<string>("Numero").ToString()}|" +
                                                              $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                              $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                              $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                              $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                                }
                                            }
                                            else if (EstadoImpresion > 1)
                                            {

                                                if (EstadoImpresion == 17)
                                                {

                                                }
                                                else
                                                {
                                                    Vble.CANTLecturas++;
                                                    Vble.LineaHFSindiv +=
                                                    //$"{fi.Field<string>("OrdenLectura")}|{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                                    // $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                                    // $"{FechaCalp}|{ActualEstado}";
                                                    $"{fi.Field<string>("OrdenLectura")}|" +
                                                    $"{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                                    $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|" +
                                                    $"{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                                    $"{FechaCalp}|";

                                                    ////verifico si la conexion que se esta procesando tiene novedades
                                                    ////de ser asi busca sus novedades y agrega a la linea sino no agrega la linea Novedades(HNC)
                                                    if (ExisteNovedades(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo")) || (fi.Field<int>("ImpresionCOD").ToString().Trim() == "2"))
                                                    {
                                                        CargarNovedadesConex(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo"), j, Operario, ActualEstado, fi.Field<string>("Numero"));
                                                    }
                                                    else
                                                    {
                                                        if (Vble.LineaHFSindiv != "")
                                                        {
                                                            Vble.LineaHNC = $"{ActualEstado}||||||";
                                                            Vble.LineaHFSindiv += $"{Vble.LineaHNC}|{Operario}\n";
                                                            Vble.LineaHNC = "";
                                                        }
                                                    }

                                                    if (Funciones.EnergiaReactivaGU(fi.Field<int>("ConexionID"), Vble.Periodo, fi.Field<string>("Numero")) != "")
                                                    {
                                                        Int64 OrdenLecGU = Convert.ToInt64(fi.Field<string>("OrdenLectura")) + 1;

                                                        Vble.LineaHFSindiv +=
                                                                            $"{OrdenLecGU.ToString("00000000000000000000")}|" +
                                                                            $"{fi.Field<Int32>("titularID").ToString("0000000000")}|" +
                                                                            $"{fi.Field<Int32>("Contrato").ToString("000000000000")}|" +
                                                                            $"{Convert.ToInt32(fi.Field<string>("Instalacion")).ToString("0000000000")}|" +
                                                                            $"{FechaCalp}|" +
                                                                            $"{Vble.EnergiaReactiva}||||||{Operario}\n";

                                                    }



                                                    Vble.LineasGPS += $"{ fi.Field<Int32>("Ruta").ToString()}|" +
                                                                  $"{fi.Field<Int32>("Periodo").ToString()}|" +
                                                                  $"{fi.Field<Int32>("ConexionID").ToString()}|" +
                                                                  $"{ fi.Field<string>("Numero").ToString()}|" +
                                                                  $"{ fi.Field<string>("DomicSumin").ToString()}|" +
                                                                  $"{ fi.Field<decimal>("Latitud").ToString()}|" +
                                                                  $"{ fi.Field<decimal>("Longitud").ToString()}|" +
                                                                  $"{ fi.Field<DateTime>("ActualFecha").ToString("yyyy-MM-dd")}|" +
                                                                  $"{ fi.Field<string>("ActualHora").ToString()}\n";
                                                }
                                            }
                                            if (ExistenRegistrosLogErrores(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo")))
                                            {
                                                Vble.LineaLogErrores += ArmarLineaLog(fi.Field<int>("ConexionID"), fi.Field<int>("Periodo"));
                                            }
                                            NroLinea++;
                                        }//FIN else del armado de las lineas de los archivos Lecturas y Facturas.
                                    }                                    
                                }
                            }
                            
                            //vacia el Array que para cargar las novedades de la proxima Conexion en caso de que tenga
                            for (int i = 0; i < 5; i++)
                            {
                                ArrayCodNovedades[i] = "";
                            }                           
                            if (Vble.LineaHFSindiv != "")
                            {
                                Vble.LineaHFS += Vble.LineaHFSindiv;
                            }
                            Vble.LineaHFSindiv = "";

                        }//cierre de if de checkbox Improtar todo
                        TareaSegundoPlano1.ReportProgress(ñ);
                        ñ++;
                        sinSaldo++;
                    }
                }
                //Vble.lineas = Vble.LineaHCX;
                //Vble.CreateInfoCarga(archivosecuencia, filename, Vble.LineaHCX);
                Vble.lineas = Vble.LineaHFS;
                
                if (UpPorRuta.Checked == true)
                {
                    if (Vble.LineaHFS != "" || Vble.LineaHFS == "")
                    {
                        Vble.CreateInfoCarga(archivosecuencia, filename, Vble.LineaHFS);
                    }
                }
                else
                {
                    if (Vble.LineaHFS != "" )
                    {
                        Vble.CreateInfoCarga(archivosecuencia, filename, Vble.LineaHFS);
                    }
                }
                                             

                if (UpPorRuta.Checked == true)
                {
                    if (Vble.LineaImpresos != "" || Vble.LineaImpresos == "")
                    {
                        Vble.CreateInfoCarga(archivoImpresos, "Impresos.btx", Vble.LineaImpresos);
                    }
                }
                else
                {
                    if (Vble.LineaImpresos != "")
                    {
                        Vble.CreateInfoCarga(archivoImpresos, "Impresos.btx", Vble.LineaImpresos);
                    }
                }    
                if (UpPorRuta.Checked == true)
                {
                    if (Vble.LineaLogErrores != "" || Vble.LineaLogErrores == "")
                    {
                        Vble.CreateInfoCarga(archivoLog, "LogErrores", Vble.LineaLogErrores);
                    }
                }
                else
                {
                    if (Vble.LineaLogErrores != "" )
                    {
                        Vble.CreateInfoCarga(archivoLog, "LogErrores.btx", Vble.LineaLogErrores);
                    }
                }         
                if (Vble.LineasGPS != "" || Vble.LineasGPS == "")
                {
                    Vble.CreateInfoCarga(archivoGPS, "GPS.btx", Vble.LineasGPS);
                }

                ////Vble.CreateInfoCarga(archivosecuencia, filename, Vble.LineaHCX);
                ////Vble.ExportarExcel(dtLeidosImpresos, dtLeidosFueraDeRango, archivoexcel);
                //if (Vble.ExportarExcelImpresos() == "SI")
                //{
                //    Vble.ExportarExcel(Vble.NºInstalacionImpresos, Vble.ContratoImpresos, Vble.TitularImpresos, Vble.FacturaImpresos,
                //                  Vble.NºInstalacionFueraDeRango, Vble.ContratoFueraDeRango, Vble.TitularFueraDeRango,
                //                  Vble.ObservacionesFueraDeRango, archivoexcel);                   
                //}

                Vble.LineaHCX = "";
                Vble.LineaHMD = "";
                Vble.LineaHNC = "";
                Vble.LineaHFS = "";
                Vble.LineaImpresos = "";
                Vble.LineaLogErrores = "";
                Vble.LineasGPS = "";
                Vble.EnergiaReactiva = "";
            }
            catch (Exception r)
            {
                if (r.Message.Contains("No hay ninguna fila"))
                {

                }
                else
                {
                    MessageBox.Show(r.Message + " Erro al generar Archivo Upload");
                }

                
            }
        }
               



        /// <summary>
        /// Metodo que al seleccionar el checkbox Exportar todo(cbExportarTodo) deja preparado todas las conexiones disponibles para la exportación
        /// </summary>
        private void ExportarTodo()
        {
            try
            {
                TreeNode Nodo = tvlotes.SelectedNode;
                int idx = Nodo.Index;
                string sKy = Nodo.Tag.ToString();
                clInfoNodos tn = new clInfoNodos();
                tn = dcNodos[Nodo.Tag.ToString()];

                int Lote;
                Lote = tn.Remesa;
                ArrayLotes.Add(Lote);

                dataGridView1.DataSource = CargarConexionesTodo();
                //TablaUpload2 = CargarNovedadesConex(Lote);
                //dataGridView1.DataSource = CargarNovedadesConex(Lote);
                //CantidadConexPorLote(Lote);
                if (TablaUpload1.Rows.Count > 0)
                {
                    tvConexUpdload.Items.Add(tvlotes.SelectedNode.Text + " - Contiene: " + CantidadConexPorLote(Lote).ToString() + " conexiones");
                }
                tvlotes.SelectedNode.ForeColor = Color.Gray;




            }
            catch (Exception)
            {

            }
        }


        private void tvlotes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {

                TreeNode Nodo = tvlotes.SelectedNode;
                tvConexUpdload.Items.Clear();
                int idx = Nodo.Index;
                //int Distr;
                //string sKy = Nodo.Tag.ToString();
                clInfoNodos tn = new clInfoNodos();

                tn = dcNodos[Nodo.Tag.ToString()];
                //string clave = "";
                //int Lote, Ruta;

                if (Nodo != null)
                {
                    if (Nodo.Level == 3)
                    {
                        tn.ImageKey = "todo";
                        //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
                        if (Nodo.ForeColor == Color.Empty)
                        {
                            ArrayLotes.Clear();
                            ArrayRuta.Clear();
                            ArrayRemesa.Clear();
                            ArrayRemesaRuta.Clear();
                            ArrayLocalidad.Clear();

                            tvlotes.SelectedNode.ForeColor = Color.Gray;

                        }
                        else if (Nodo.ForeColor == Color.Gray)
                        {
                            tvlotes.SelectedNode.ForeColor = Color.Empty;
                            //Nodo.ForeColor = Color.Black;
                            dataGridView1.DataSource = "";
                            TablaUpload1.Clear();
                            TablaUpload2.Clear();
                            ArrayLotes.Clear();
                            ArrayRuta.Clear();
                            ArrayRemesa.Clear();
                            ArrayRemesaRuta.Clear();
                            ArrayLocalidad.Clear();
                            tvConexUpdload.Items.Clear();
                            Vble.CantRegistros = 0;
                            //aca iba el foreach

                        }

                        //Recorre los nodos y toma solo los que estan seleccionados (gris)
                        RecorrerNodos(sender, e);

                        //MessageBox.Show(Vble.CantConex.ToString());
                    }

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
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
        private void TomarEstadoDeHijos(TreeNode ndP)
        {
            clInfoNodos iNP = dcNodos[ndP.Tag.ToString()];
            int CxTot = 0;
            int CxSel = 0;
            //Recorre los hijos del nodo
            foreach (TreeNode ndH in ndP.Nodes)
            {
                //si tiene hijos, recursividad.
                if (ndH.Nodes.Count > 0)
                    TomarEstadoDeHijos(ndH);
                else
                {
                    ////Si no tiene hijos, toma seleccion según imagen
                    //if (dcNodos[ndH.Tag.ToString()].ImageKey == "Todo")
                    dcNodos[ndH.Tag.ToString()].CnxSelected = dcNodos[ndH.Tag.ToString()].CnxTotal;
                    //else
                    //    dcNodos[ndH.Tag.ToString()].CnxSelected = 0;
                }

                //acumula totales y seleccionados
                CxTot += dcNodos[ndH.Tag.ToString()].CnxTotal;
                CxSel += dcNodos[ndH.Tag.ToString()].CnxSelected;
            }

            //Aplica las cantidades al nodo
            dcNodos[ndP.Tag.ToString()].CnxSelected = CxSel;
            dcNodos[ndP.Tag.ToString()].CnxTotal = CxTot;

            //Aplica la imagen segun la cantidad seleccionada
            if (ndP.ForeColor == Color.Gray)
                ndP.ForeColor = Color.Gray;
            //dcNodos[ndP.Tag.ToString()].ImageKey = "nada";
            else /*if (CxTot == CxSel)*/
                 //dcNodos[ndP.Tag.ToString()].ImageKey = "todo";
                ndP.ForeColor = Color.Black;

            ////Muestra el estado
            //ndP.ImageKey = dcNodos[ndP.Tag.ToString()].ImageKey;
            //ndP.Text = dcNodos[ndP.Tag.ToString()].Texto +
            //    "  [ " + dcNodos[ndP.Tag.ToString()].CnxSelected.ToString() +
            //    " de " + dcNodos[ndP.Tag.ToString()].CnxTotal.ToString() + " ]";

            return;

        }

        /// <summary>
        /// Borra las tablas con las conexiones que tengan Codigo de Impresion 500 o solo aquellas que fueron leidas e Impresas
        /// es decir descargadas desde FIS segun seleccion,
        /// Si selecciono Todo del panel Opciones de Exportacion y SI del panel exportar Saldos, exportara todas las rutas
        /// y todas las conecciones que tengan codigo 5xx es decir descargadas desde FIS
        /// Si selecciono Rutas del panle de opciones de Exportacion limpiara las rutas seleccionadas segun lo que este seleccionado
        /// del panel Exportar Saldos
        /// </summary>
        /// <param name="txSQL"></param>
        private void LimpiarTablas(string txSQL)
        {
            string DeleteConexiones, DeletePersonas, DeleteMedidres, DeleteTextosVarios,
                   DeleteNovedadesConex, DeleteInfoConex, DeleteConceptosDatos, DeleteConceptosFacturados;

            DataTable Tabla = new DataTable();
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            foreach (DataRow item in Tabla.Rows)
            {
                ////Elimina Registros al Exportar de la tabla Personas
                //DeletePersonas = "delete from personas where personaID > 0";
                //MySqlCommand cmdSQL2 = new MySqlCommand(DeletePersonas, DB.conexBD);
                //cmdSQL2.ExecuteNonQuery();
                //cmdSQL2.Dispose();

                //Elimina Registros al Exportar de la tabla Personas
                DeletePersonas = "delete from personas where personaID = " + item.Field<Int32>("ConexionID") + " OR " + "personaID = " + item.Field<Int32>("TitularID");
                MySqlCommand cmdSQL2 = new MySqlCommand(DeletePersonas, DB.conexBD);
                cmdSQL2.ExecuteNonQuery();
                cmdSQL2.Dispose();

                ////Elimina Registros al Exportar de la tabla Medidores
                //DeleteMedidres = "delete from medidores where ConexionId > 0";
                //MySqlCommand cmdSQL3 = new MySqlCommand(DeleteMedidres, DB.conexBD);
                //cmdSQL3.ExecuteNonQuery();
                //cmdSQL3.Dispose();

                //Elimina Registros al Exportar de la tabla Medidores
                DeleteMedidres = "delete from medidores where ConexionId = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL3 = new MySqlCommand(DeleteMedidres, DB.conexBD);
                cmdSQL3.ExecuteNonQuery();
                cmdSQL3.Dispose();

                //Elimina Registros al Exportar de la tabla textosvarios
                //DeleteTextosVarios = "delete from textosvarios where ConexionId > 0";
                //MySqlCommand cmdSQL4 = new MySqlCommand(DeleteTextosVarios, DB.conexBD);
                //cmdSQL4.ExecuteNonQuery();
                //cmdSQL4.Dispose();

                //Elimina Registros al Exportar de la tabla textosvarios
                DeleteTextosVarios = "delete from textosvarios where ConexionId = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL4 = new MySqlCommand(DeleteTextosVarios, DB.conexBD);
                cmdSQL4.ExecuteNonQuery();
                cmdSQL4.Dispose();

                ////Elimina Registros al Exportar de la tabla Novedades Conexion
                //DeleteNovedadesConex = "delete from novedadesconex Where ConexionId > 0";
                //MySqlCommand cmdSQL5 = new MySqlCommand(DeleteNovedadesConex, DB.conexBD);
                //cmdSQL5.ExecuteNonQuery();
                //cmdSQL5.Dispose();

                //Elimina Registros al Exportar de la tabla Novedades Conexion
                DeleteNovedadesConex = "delete from novedadesconex Where ConexionId = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL5 = new MySqlCommand(DeleteNovedadesConex, DB.conexBD);
                cmdSQL5.ExecuteNonQuery();
                cmdSQL5.Dispose();

                ////Elimina Registros al Exportar de la tabla conceptosdatos
                //DeleteConceptosDatos = "delete from conceptosdatos Where ConexionId > 0";
                //MySqlCommand cmdSQL6 = new MySqlCommand(DeleteConceptosDatos, DB.conexBD);
                //cmdSQL6.ExecuteNonQuery();
                //cmdSQL6.Dispose();

                //Elimina Registros al Exportar de la tabla conceptosdatos
                DeleteConceptosDatos = "delete from conceptosdatos Where ConexionId = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL6 = new MySqlCommand(DeleteConceptosDatos, DB.conexBD);
                cmdSQL6.ExecuteNonQuery();
                cmdSQL6.Dispose();

                //Elimina Registros al Exportar de la tabla ConceptosFacturados
                DeleteConceptosFacturados = "delete from conceptosfacturados Where conexionID = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL8 = new MySqlCommand(DeleteConceptosFacturados, DB.conexBD);
                cmdSQL8.ExecuteNonQuery();
                cmdSQL8.Dispose();

                //Elimina Registros al Exportar de la tabla InfoConex
                DeleteInfoConex = "delete from infoconex Where ConexionId = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL7 = new MySqlCommand(DeleteInfoConex, DB.conexBD);
                cmdSQL7.ExecuteNonQuery();
                cmdSQL7.Dispose();

                //Elimina Registros al Exportar de la tabla Conexiones
                DeleteConexiones = "delete from conexiones where ConexionId = " + item.Field<Int32>("ConexionID");
                MySqlCommand cmdSQL1 = new MySqlCommand(DeleteConexiones, DB.conexBD);
                cmdSQL1.ExecuteNonQuery();
                cmdSQL1.Dispose();

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            ///Este inicio de if verifica primero si esta habilitado el cambio de fecha de lectura para la exportacion
            ///segundo: si el TextBox donde se colocaría 
            if (GBCambioFecha.Visible == true)
            {
                if (TBFechaModificada.Text != "")
                {                      
                        MessageBox.Show(Convert.ToDateTime(TBFechaModificada.Text).ToString("yyyy-MM-dd"));                  

                }
            }
            else
            {
                MessageBox.Show("El Cambio de Fecha está desactivado");
            }

            //if (textBox1.Text.IndexOf('5',0,1).ToString() == "5")
            //{
            //    MessageBox.Show("Contiene el 5");
            //}
            //else
            //{
            //    MessageBox.Show("Verificar");
            //}


            //Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaUpload);


            //DataTable oDataTableImpresos;
            //DataTable dtLeidosFueraDeRango;
            //string ruta = "C:\\Users\\operario\\Desktop\\prueba.xlsx";

            ////if ((oDataTableImpresos == null) || (String.IsNullOrEmpty(ruta)))
            ////{
            ////    throw new ArgumentNullException();
            ////}

            //Excel.Application excel = null;
            //Excel.Workbook book = null;
            //Excel.Worksheet HojaImpresos = null;
            //Excel.Worksheet HojaFueraDeRango = null;

            //try
            //{
            //    excel = new Microsoft.Office.Interop.Excel.Application();

            //    if (!File.Exists(ruta))
            //    {
            //        //Aqui se debe crear el archivo excel segun la ruta que se envia si no existe
            //        //ExcelLibrary.DataSetHelper.CreateWorkbook(ruta);
            //        //excel.Workbooks.Add(ruta);
            //        //File.Create(ruta);
            //        book = excel.Workbooks.Add();
            //        HojaImpresos = book.Worksheets.Add();
            //        HojaFueraDeRango = book.Worksheets.Add();
            //        HojaFueraDeRango.Name = "FUERAS DE RANGO";
            //        HojaImpresos.Name = "LEIDOS IMPRESOS";
            //        ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Sheets["Hoja1"]).Delete();
            //        book.SaveAs(ruta);
            //        book.Close();
            //        excel.Quit();

            //    }


            //    // Abrimos el libro de trabajo.
            //    book = excel.Workbooks.Open(ruta);

            //    Excel.Worksheet ws = (Excel.Worksheet)book.Worksheets[1];


            //    ws.Cells[1, 1].Value = "INSTALACION";
            //    ws.Cells[1, 2].Value = "CONTRATO";
            //    ws.Cells[1, 3].Value = "TITULAR";
            //    ws.Cells[1, 4].Value = "PUNTO DE VENTA";
            //    ws.Cells[1, 5].Value = "LETRA FACTURA";
            //    ws.Cells[1, 6].Value = "Nº FACTURA";




            //    //int indiceColumna = 0;
            //    ////Hoja Impresos
            //    //foreach (DataColumn col in oDataTableImpresos.Columns)  //Columnas
            //    //{
            //    //    indiceColumna++;
            //    //    excel.Cells[1, indiceColumna] = col.ColumnName;
            //    //    //HojaImpresos.Cells[1, indiceColumna] = col.ColumnName;
            //    //}


            //    //int indiceFila = 0;
            //    //foreach (DataRow row in oDataTableImpresos.Rows)  //Filas
            //    //{
            //    //    indiceFila++;
            //    //    indiceColumna = 0;

            //    //    foreach (DataColumn col in oDataTableImpresos.Columns)  //Columnas
            //    //    {
            //    //        indiceColumna++;
            //    //        excel.Cells[indiceFila + 1, indiceColumna] = row[col.ColumnName];
            //    //        //HojaImpresos.Cells[indiceFila + 1, indiceColumna].Value = row[col.ColumnName];
            //    //    }
            //    //    //HojaImpresos.Columns.AutoFit();
            //    //    excel.Columns.AutoFit();
            //    //}



            //    //indiceColumna = 0;
            //    // if ((dtLeidosFueraDeRango != null))
            //    // {
            //    //     //HojaFuera de rango
            //    //     foreach (DataColumn col in dtLeidosFueraDeRango.Columns)  //Columnas
            //    //     {
            //    //         indiceColumna++;
            //    //         //excel.Cells[1, indiceColumna] = col.ColumnName;
            //    //         HojaFueraDeRango.Cells[1, indiceColumna] = col.ColumnName;
            //    //     }


            //    //     int indiceFilaFR = 0;
            //    //     foreach (DataRow row in dtLeidosFueraDeRango.Rows)  //Filas
            //    //     {
            //    //         indiceFilaFR++;
            //    //         indiceColumna = 0;

            //    //         foreach (DataColumn col in dtLeidosFueraDeRango.Columns)  //Columnas
            //    //         {
            //    //             indiceColumna++;
            //    //             //excel.Cells[indiceFilaFR + 1, indiceColumna] = row[col.ColumnName];
            //    //             HojaFueraDeRango.Cells[indiceFila + 1, indiceColumna].Value = row[col.ColumnName];
            //    //         }
            //    //         HojaFueraDeRango.Columns.AutoFit();
            //    //     }
            //    // }


            //    //excel.Visible = true;
            //}
            //catch (Exception ex)
            //{
            //    if (book != null)
            //    {
            //        book.Saved = true;
            //    }

            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    if (book != null)
            //    {
            //        if (!book.Saved)
            //        {
            //            book.Save();
            //        }
            //        book.Close();
            //    }
            //    book = null;

            //    if (excel != null)
            //    {
            //        // Si procede, cerramos Excel y disminuimos el recuento de referencias al objeto Excel.Application.
            //        excel.Quit();

            //        while (System.Runtime.InteropServices.Marshal.ReleaseComObject(excel) > 0)
            //        {

            //        }
            //    }
            //    excel = null;
            //}

        }




        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void TareaSegundoPlano1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarExpor.Visible = true;
            LbPorcentaje.Visible = true;
            progressBarExpor.Value = e.ProgressPercentage;
            LbPorcentaje.Text = "% " + (e.ProgressPercentage * 100) / Vble.CantConex + " completado.";

        }


        /// <summary>
        ///Metodo que limpia las tablas que se utilizan al realizar para volver 
        ///a importar y se realice la importacion de la base completa 
        /// </summary>
        private void LimpiarTablasSegunSeleccion()
        {

            string txSQL;
            try
            {

                TreeNode Nodo = tvlotes.SelectedNode;
                int idx = Nodo.Index;
                int Distr;
                //string sKy = Nodo.Tag.ToString();
                clInfoNodos tn = new clInfoNodos();

                tn = dcNodos[Nodo.Tag.ToString()];
                //string clave = "";
                int Lote, Ruta;

                if (cbTodo.Checked == true)
                {
                    ////Elimina Registros al Exportar de la tabla Conexiones
                    //DeleteConexiones = "delete from conexiones where ConexionID > 0";
                    //MySqlCommand cmdSQL1 = new MySqlCommand(DeleteConexiones, DB.conexBD);           
                    //cmdSQL1.ExecuteNonQuery();
                    //cmdSQL1.Dispose(); 

                    //Lee las conexiones dependiendo de lo seleccionado en el RadioButton, si se exportaran todas 
                    //o quedaran las No leidas
                    if (RadioButtonSI.Checked == true)
                    {
                        txSQL = "SELECT ConexionID, TitularID " +
                                 " FROM conexiones";
                    }
                    else
                    {
                        txSQL = "SELECT ConexionID, TitularID " +
                                " FROM conexiones WHERE ImpresionOBS <> 500";
                    }

                    LimpiarTablas(txSQL);


                }
                else if (cbLote.Checked == true)
                {

                    foreach (TreeNode tNd1 in tvlotes.Nodes[0].Nodes)
                    {
                        foreach (TreeNode tNd2 in tNd1.Nodes)
                        {
                            if (tNd2.Nodes.Count > 0)
                            {
                                foreach (TreeNode tNd3Rutas in tNd2.Nodes)
                                {
                                    if (tNd3Rutas.ForeColor.Name == "Gray")
                                    {
                                        clInfoNodos tnn = new clInfoNodos();
                                        tnn = dcNodos[tNd3Rutas.Tag.ToString()];
                                        Lote = tnn.Remesa;
                                        Distr = tnn.Distrito;
                                        Ruta = tnn.Ruta;
                                        ArrayLotes.Add(Lote);

                                        //Lee las conexiones dependiendo de lo seleccionado en el RadioButton, si se exportaran todas 
                                        //o quedaran las No leidas
                                        if (RadioButtonSI.Checked == true)
                                        {
                                            txSQL = "SELECT ConexionID, TitularID " +
                                                     " FROM conexiones WHERE Ruta = " + Ruta.ToString();
                                        }
                                        else
                                        {
                                            txSQL = "SELECT ConexionID, TitularID " +
                                                    " FROM conexiones WHERE ImpresionOBS <> 500 And Ruta = " + Ruta.ToString();
                                        }

                                        LimpiarTablas(txSQL);

                                    }
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Limpiar las Tablas");
            }

        }

        private void TareaSegundoPlano1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //LimpiarTablasSegunSeleccion();////comentado momentaneamente para no cargar cada vez que realizo una exportación de prueba
            TablaUpload1.Clear();
            TablaUpload2.Clear();
            ArrayRuta.Clear();
            ArrayRemesa.Clear();
            ArrayRemesaRuta.Clear();
            ArrayLocalidad.Clear();
            TablaUploadNoved.Clear();
            tvConexUpdload.Items.Clear();
            LbPorcentaje.Visible = false;
            progressBarExpor.Visible = false;
            this.Cursor = Cursors.Default;
            cbLote_CheckStateChanged(sender, e);
            tvExportadas.Nodes.Clear();
            //tvExportadas.Nodes.RemoveAt(0);
            CargarTVRutasExportadas();
            this.Cursor = Cursors.Default;
            CheckCambiarFechaLectura.Checked = false;
            cbTodo.Checked = false;
            tvlotes.Visible = true;
            labelPanExpXRuta.Visible = true;
            tvConexUpdload.Items.Clear();
            Vble.CantConex = 0;
            CargarTreeviewXLotes();
            ñ = 0;
            progressBarExpor.Maximum = 100;

            MessageBox.Show("Upload terminado, se exportaron " + Vble.CantRegistros + " Rutas", "Exportación", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);



        }




        /// <summary>
        ///Metodo que contiene foreach anidados el cual recorre los nodos seleccionados para cargar los arraylist con
        ///los valores de         
        ///Ruta;
        ///Lotes;     
        ///para luego realizar la consulta que obtendra las conexiones las rutas seleccionadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RecorrerNodos(object sender, EventArgs e)
        {
            Vble.CantConex = 0;

            try
            {
           
                int Lote = 0, Ruta = 0, Distr = 0, Seleccionados = 0, Remesa = 0, Disponibles = 0;

                foreach (TreeNode tNd1 in tvlotes.Nodes[0].Nodes)
                {
                    foreach (TreeNode tNd2 in tNd1.Nodes)
                    {
                        if (tNd2.Nodes.Count > 0)
                        {
                            foreach (TreeNode tNd3Rutas in tNd2.Nodes)
                            {
                                if (tNd3Rutas.ForeColor.Name == "Gray")
                                {
                                    Seleccionados++;
                                    clInfoNodos tnn = new clInfoNodos();
                                    tnn = dcNodos[tNd3Rutas.Tag.ToString()];
                                    Lote = tnn.Remesa;
                                    Remesa = tnn.Remesa;
                                    Distr = tnn.Distrito;
                                    Ruta = tnn.Ruta;
                                    ArrayLotes.Add(Lote);
                                  
                                    ArrayRuta.Add(Ruta);
                                    ArrayLocalidad.Add(Distr);
                                    ArrayRemesa.Add(Remesa);

                                    ArrayRemesaRuta.Add(Remesa + "-" + Ruta);


                                    Disponibles = CantidadConexPorLoteyDistr(Vble.Lote, Distr, Ruta, Remesa);

                                    if (Disponibles > 0)
                                 
                                    {
                                       Vble.CantConex += Disponibles;
                                       tvConexUpdload.Items.Add("Distrito: " + Distr + " - Ruta:" + Ruta.ToString() + "  Contiene: " + Disponibles + " conexiones");
                                        
                                      
                                    }
                                }
                            }
                        }
                    }
                }

                //if (Seleccionados != 0)
                //{
                //    ObtenerConexionesXloteYDistr(Vble.Lote, Distr, Ruta, Vble.Periodo, Vble.Remesa, 0);
                //}
                //else
                //{
                //}

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al recorrer Nodos");

            }
        }




        private void tvlotes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //ArrayRuta.Clear();
            //RecorrerNodos(sender, e);
            //ArrayHasta.Clear();
            //ArrayRuta.Clear();
            //ArrayLocalidad.Clear();
            //ArrayCantConex.Clear();
        }

        public void ValidarTBFechas()
        {
            if (TextBoxDesde.Text.Length == 10)
            {
                if (TextBoxDesde.Text == "dd/MM/yyyy")
                {
                    FiltroDESDE = DateTime.Today.ToString("yyyy-MM-dd");
                }//TextBoxDesde.Text = DateTime.Today.ToString("yyyy-MM-dd");
                else
                {
                    FiltroDESDE = Convert.ToDateTime(TextBoxDesde.Text).ToString("yyyy-MM-dd");
                }
            }
            else if (TextBoxDesde.Text == "dd/mm/yyyy")
            {
                //TextBoxDesde.Text = DateTime.Today.ToString("yyyy-MM-dd");
                FiltroDESDE = DateTime.Today.ToString("yyyy-MM-dd");
            }

            if (TextBoxHasta.Text.Length == 10)
            {
                if (TextBoxHasta.Text == "dd/MM/yyyy")
                    { 
                   FiltroHASTA = DateTime.Today.ToString("yyyy-MM-dd");                    
                    }
                else
                {
                    FiltroHASTA = Convert.ToDateTime(TextBoxHasta.Text).ToString("yyyy-MM-dd");
                }
            }
            else if (TextBoxHasta.Text == "dd/MM/yyyy")
            {
                //TextBoxHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
                FiltroHASTA = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }


        //RESUMEN IMPRESAS
        private void button2_Click(object sender, EventArgs e)
        {            
           this.Cursor = Cursors.WaitCursor;
            //Por el true de RBRuta entra a la condicion de consulta por ruta remesa y fecha de toma de lectura
            if (RBRuta.Checked)
            {
                if (TextBoxRuta.Text != "")
                {
                    ruta = "AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }


                ValidarTBFechas();

                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                  "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, " +
                                  "M.AnteriorEstado, M.ActualEstado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, C.ConsumoFacturado, C.Operario, " +
                                  "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                 "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                 "CONCAT('Correccion: ', N62.Observ), " +
                                 "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                 "CONCAT('Correccion: ', N6.Observ), " +
                                     "IF(N6.Observ = N62.Observ, " +
                                         "N62.Observ, " +
                                             "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                 "CONCAT('Correccion: ', N62.Observ), " +
                                                     "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                        "CONCAT('Correccion: ', N6.Observ), " +
                                                             "IF(N6.Observ <> N62.Observ, " +
                                                                 "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                  "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                  "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                  "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                  "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                  "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                  "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                  "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                  "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                  "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                  "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                  "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                  "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                  "WHERE ((C.ImpresionOBS = 601) " +
                                  " AND M.ActualFecha BETWEEN  '" + Convert.ToDateTime(FiltroDESDE).ToString("yyyy-MM-dd") + "' " +
                                  "AND '" + Convert.ToDateTime(FiltroHASTA).ToString("yyyy-MM-dd") + "' " +
                                  "AND C.Remesa = " + CBRemesaRuta.Text + " AND C.Periodo = " + Vble.Periodo + " " + ruta + ") ";

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                //Funciones.VerDetallePreDescarga(LabImprPre.Text, "601", Vble.Periodo, CONSULTA);
                Funciones.VerDetallePreDescarga(LabImprPre.Text, Vble.Periodo, CONSULTA, true, "601", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
            }
            //Por el true de RBRemesaSola entra a la condicion de consulta por remesa de la localidad seleccionada.
            else if (RBRemesaSola.Checked)
            {
                ///Consulta la tabla altas para obtener las inyecciones
                ///
                CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, " +
                              "M.AnteriorEstado, M.ActualEstado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, C.ConsumoFacturado, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                             "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                             "CONCAT('Correccion: ', N62.Observ), " +
                             "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                             "CONCAT('Correccion: ', N6.Observ), " +
                                 "IF(N6.Observ = N62.Observ, " +
                                     "N62.Observ, " +
                                         "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                             "CONCAT('Correccion: ', N62.Observ), " +
                                                 "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                    "CONCAT('Correccion: ', N6.Observ), " +
                                                         "IF(N6.Observ <> N62.Observ, " +
                                                             "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                              "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                              "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                              "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                              "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                              "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                              "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                              "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                              "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                              "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                              "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                              "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                              "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                              "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                              "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                              "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                              "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                              "WHERE C.ImpresionOBS = 601 " +
                              " AND C.Zona = " + CBFiltroZona.Text +
                              " AND C.Remesa = " + CBRemesaSola.Text + " AND C.Periodo = " + Vble.Periodo + " ORDER BY C.Ruta";

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                REMESA = CBRemesaSola.Text;

                Funciones.VerDetallePreDescargaPorRemesa(LabImprPre.Text, Vble.Periodo, CONSULTA, true, "601", REMESA, "Resumen",
                                                TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);
            }
          this.Cursor = Cursors.Default;
        }
        ///RESUMEN LEIDAS NO IMPRESAS
        private void button3_Click(object sender, EventArgs e)
        {
            if (RBRuta.Checked)
            {
                this.Cursor = Cursors.WaitCursor;
                ValidarTBFechas();
                if (TextBoxRuta.Text != "")
                {
                    ruta = "AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }

                if (CBLeidosNOPrint.Text == "Leidos NO impresos")
                {
                  
                       string CONSULTA = "SELECT DISTINCT C.Periodo, " +
                                         "C.Remesa, " +
                                         "C.Ruta, " +
                                         "C.ConexionID AS NInstalacion, " +
                                         "C.Contrato, " +
                                         "C.titularID AS IC, " +
                                         "P.Apellido, " +
                                         "M.Numero as Medidor,  " +
                                         "C.DomicSumin as Domicilio, " +
                                         "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS HoraLect, E.Titulo AS Situacion, C.Operario, " +
                                         "if(N1.Codigo <> 0, N1.Codigo, NULL) as Ord1, if (N2.Codigo <> 0, N2.Codigo, NULL) as Ord2, if (N3.Codigo <> 0, N3.Codigo, NULL) as Ord3, " +
                                         "if (N4.Codigo <> 0, N4.Codigo, NULL) as Ord4, if (N5.Codigo <> 0, N5.Codigo, NULL) as Ord5,  " +
                                         "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                         "CONCAT('Correccion: ', N62.Observ), " +
                                         "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                         "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                      "FROM Conexiones C " +
                                      "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                      "INNER JOIN Errores E ON (C.ImpresionOBS - 600) = E.Codigo " +
                                      "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                      "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                      "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                      "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                      "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                      "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                      "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                      "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                      "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                      "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                      "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                      "WHERE (C.ImpresionOBS >= 602  AND C.ImpresionOBS <= 699 AND C.ImpresionOBS <> 609) " +
                                      "AND M.ActualFecha BETWEEN  '" + Convert.ToDateTime(FiltroDESDE).ToString("yyyy-MM-dd") + "' " +
                                      "AND '" + Convert.ToDateTime(FiltroHASTA).ToString("yyyy-MM-dd") + "' " +
                                      "AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                      ruta +
                                      " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo + " ";

                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    //Funciones.VerDetallePreDescarga(LabLeidNoImprePre.Text, "602", Vble.Periodo, CONSULTA);
                    Funciones.VerDetallePreDescarga(CBLeidosNOPrint.Text, Vble.Periodo, CONSULTA, true, "602", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
                    this.Cursor = Cursors.Default;
                }
                else if (CBLeidosNOPrint.Text == "Facturados NO impresos")
                {
                    CONSULTA = "SELECT DISTINCT " +
                                "C.Periodo, " +
                                "C.Remesa, " +
                                "C.Ruta, " +
                                "C.ConexionID AS NInstalacion," +
                                "C.Contrato, " +
                                "C.titularID AS IC, " +
                                "P.Apellido, " +
                                "M.Numero AS Medidor," +
                                "C.DomicSumin AS Domicilio, " +
                                "C.ImpresionCANT, " +
                                "M.AnteriorEstado, " +
                                "M.ActualEstado, " +
                                "C.ConsumoFacturado, " +
                                "M.ActualFecha AS Fecha, " +
                                "M.ActualHora AS Hora, " +
                                "if (ImpresionOBS = 400, 'EN CALLE', IF(ImpresionOBS = 500, 'NO LEIDO', IF(ImpresionOBS = 0, 'NO LEIDO', E.Titulo))) AS Situacion, " +
                                "C.Operario,    " +
                                "N1.Codigo AS Ord1, " +
                                "N2.Codigo AS Ord2, " +
                                "N3.Codigo AS Ord3, " +
                                "N4.Codigo AS Ord4, " +
                                "N5.Codigo AS Ord5, " +
                                 "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                    "CONCAT('Correccion: ', N62.Observ), " +
                                      "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                "FROM Conexiones C " +
                                "INNER JOIN " +
                                "Personas P " +
                                "ON C.TitularID = P.PersonaID " +
                                "AND C.Periodo = P.Periodo " +
                                "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
                                "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID " +
                                "AND C.Periodo = M.Periodo " +
                                "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                "WHERE ((C.ImpresionOBS MOD 100 >= 71 " +
                                      "AND C.ImpresionOBS MOD 100 < 78) " +
                                      "OR " +
                                      "(IF(LENGTH(C.ImpresionCANT) = 10, IF(LEFT(C.ImpresionCANT, 4) <> 0, 1, 0), 0)) " +
                                      "OR " +
                                      "(IF(LENGTH(C.ImpresionCANT) = 7, IF(LEFT(C.ImpresionCANT, 1) <> 0, 1, 0), 0))) " +
                                      " AND M.ActualFecha BETWEEN '" + Convert.ToDateTime(FiltroDESDE).ToString("yyyy-MM-dd") +
                                      "' AND '" + Convert.ToDateTime(FiltroHASTA).ToString("yyyy-MM-dd") +
                                      "' AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                      ruta +
                                      " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo + 
                                      " GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, Hora ASC, C.Secuencia";
                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    //Funciones.VerDetallePreDescarga(LabLeidNoImprePre.Text, "602", Vble.Periodo, CONSULTA);
                    Funciones.VerDetallePreDescarga(CBLeidosNOPrint.Text, Vble.Periodo, CONSULTA, true, "602", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
                    this.Cursor = Cursors.Default;
                }
            }
            else if (RBRemesaSola.Checked)
            {
                if (CBLeidosNOPrint.Text == "Leidos NO impresos")
                {
                    string CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, " +
                                      "C.Contrato, C.titularID AS IC, P.Apellido, M.Numero as Medidor,  C.DomicSumin as Domicilio, " +
                                      "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, E.Titulo AS Situacion, C.Operario, " +
                                      "if(N1.Codigo <> 0, N1.Codigo, NULL) as Ord1, if (N2.Codigo <> 0, N2.Codigo, NULL) as Ord2, if (N3.Codigo <> 0, N3.Codigo, NULL) as Ord3, " +
                                      "if (N4.Codigo <> 0, N4.Codigo, NULL) as Ord4, if (N5.Codigo <> 0, N5.Codigo, NULL) as Ord5,  " +
                                      "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                      "CONCAT('Correccion: ', N62.Observ), " +
                                      "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                      "FROM Conexiones C " +
                                      "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                      "INNER JOIN Errores E ON (C.ImpresionOBS - 600) = E.Codigo " +
                                      "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                      "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                      "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                      "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                      "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                      "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                      "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                      "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                      "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                      "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                      "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                      "WHERE (C.ImpresionOBS >= 602  AND C.ImpresionOBS <= 699 AND C.ImpresionOBS <> 609) " +                                     
                                      "AND C.Remesa = " + CBRemesaSola.Text + " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
                                      " GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, Hora ASC, C.Secuencia";

                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    //Funciones.VerDetallePreDescarga(LabLeidNoImprePre.Text, "602", Vble.Periodo, CONSULTA);
                    Funciones.VerDetallePreDescargaPorRemesa(CBLeidosNOPrint.Text, Vble.Periodo, CONSULTA, true, "602", CBRemesaSola.Text, "Resumen",
                                               TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);
                    this.Cursor = Cursors.Default;
                }
                else if (CBLeidosNOPrint.Text == "Facturados NO impresos")
                {
                    CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, " +
                                      "C.Contrato, C.titularID AS IC, P.Apellido, M.Numero as Medidor,  C.DomicSumin as Domicilio, " +
                                      "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, E.Titulo AS Situacion, C.Operario, " +
                                      "if(N1.Codigo <> 0, N1.Codigo, NULL) as Ord1, if (N2.Codigo <> 0, N2.Codigo, NULL) as Ord2, if (N3.Codigo <> 0, N3.Codigo, NULL) as Ord3, " +
                                      "if (N4.Codigo <> 0, N4.Codigo, NULL) as Ord4, if (N5.Codigo <> 0, N5.Codigo, NULL) as Ord5,  " +
                                      "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                      "CONCAT('Correccion: ', N62.Observ), " +
                                      "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                      "FROM Conexiones C " +
                                      "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                      "INNER JOIN Errores E ON (C.ImpresionOBS - 600) = E.Codigo " +
                                      "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                      "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                      "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                      "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                      "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                      "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                      "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                      "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                      "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                      "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                      "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                      "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                      "WHERE ((C.ImpresionOBS MOD 100 >= 71 " +
                                      "AND C.ImpresionOBS MOD 100 < 78) " +
                                      "OR " +
                                      "(IF(LENGTH(C.ImpresionCANT) = 10, IF(LEFT(C.ImpresionCANT, 4) <> 0, 1, 0), 0)) " +
                                      "OR " +
                                      "(IF(LENGTH(C.ImpresionCANT) = 7, IF(LEFT(C.ImpresionCANT, 1) <> 0, 1, 0), 0))) " +                                     
                                      " AND C.Remesa = " + CBRemesaSola.Text + " " +                                      
                                      " AND C.Periodo = " + Vble.Periodo +
                                      " GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, Hora ASC, C.Secuencia";
                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    //Funciones.VerDetallePreDescarga(LabLeidNoImprePre.Text, "602", Vble.Periodo, CONSULTA);
                    Funciones.VerDetallePreDescargaPorRemesa(CBLeidosNOPrint.Text, Vble.Periodo, CONSULTA, true, "602", CBRemesaSola.Text, "Resumen",
                                              TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);
                    this.Cursor = Cursors.Default;
                }
            }
        }


        ///RESUMEN FUERAS DE RANGO
        private void button4_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (RBRuta.Checked)
            {
                    ValidarTBFechas();

                if (TextBoxRuta.Text != "")
                {
                    ruta = "AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }

                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                  "C.TitularID AS IC, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, " +
                                  "M.ActualHora AS Hora, C.Operario, " +
                                  "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                        "CONCAT('Correccion: ', N62.Observ), " +
                                        "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                  "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                  "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                  "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                  "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                  "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                  "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                  "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                      "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                         "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                         "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                  " WHERE C.ImpresionOBS = 604 " +
                                   //" WHERE CONVERT(C.ImpresionOBS USING latin1) Like '%604' " +
                                  "AND M.ActualFecha BETWEEN  '" + FiltroDESDE + "' " +
                                  "AND '" + FiltroHASTA + "' " +
                                  "AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                  ruta + " ";

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                //Funciones.VerDetallePreDescarga(LabNoImprFueraRango.Text, "604", Vble.Periodo, CONSULTA);
                Funciones.VerDetallePreDescarga(LabNoImprFueraRango.Text, Vble.Periodo, CONSULTA, true, "604", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
                
            }
            else if (RBRemesaSola.Checked)
            {
                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                  "C.TitularID AS IC, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, " +
                                  "M.ActualHora AS Hora, C.Operario, " +
                                  "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                        "CONCAT('Correccion: ', N62.Observ), " +
                                        "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                  "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                  "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                  "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                  "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                  "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                  "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                  "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                      "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                         "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                         "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                  " WHERE C.ImpresionOBS = 604 " +
                                  " AND C.Periodo = " + Vble.Periodo +
                                  " AND C.Remesa = " + CBRemesaSola.Text;
                                  

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                //Funciones.VerDetallePreDescarga(LabNoImprFueraRango.Text, "604", Vble.Periodo, CONSULTA);
                Funciones.VerDetallePreDescargaPorRemesa(LabNoImprFueraRango.Text, Vble.Periodo, CONSULTA, true, "604", CBRemesaSola.Text, "Resumen",
                                              TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);

            }
            this.Cursor = Cursors.Default;
        }
        //SALDOS
        private void button5_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (RBRuta.Checked)
            {           
                ValidarTBFechas();
                if (TextBoxRuta.Text != "")
                {
                    ruta = "AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }

                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, C.titularID AS IC, " +
                                  "C.DomicSumin, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, " +
                                  "M.ActualFecha AS Fecha, M.ActualHora AS Hora, C.Operario, " +
                                  "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                        "CONCAT('Correccion: ', N62.Observ), " +
                                        "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                  "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                  "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                  "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                  "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                  "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                  "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                  "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                  "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                  "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                  "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                  "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                  "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +                                     
                                  "WHERE (C.ImpresionOBS MOD 100 = 0) " +
                                  " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo + " AND P.Periodo = " + Vble.Periodo +
                                  " AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                  ruta + " ";


                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                //Funciones.VerDetallePreDescarga(LabSaldos.Text, "", Vble.Periodo, CONSULTA);
                Funciones.VerDetallePreDescarga(LabSaldos.Text, Vble.Periodo, CONSULTA, true, "800", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
            }
            else if (RBRemesaSola.Checked)
            {
                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, C.titularID AS IC, " +
                                  "C.DomicSumin, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, " +
                                  "M.ActualFecha AS Fecha, M.ActualHora AS Hora, C.Operario, " +
                                  "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                        "CONCAT('Correccion: ', N62.Observ), " +
                                        "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                  "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                  "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                  "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                  "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                  "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                  "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                  "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                  "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                  "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                  "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                  "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                  "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                  "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                  "WHERE (C.ImpresionOBS MOD 100 = 0) " +
                                  " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo + " AND P.Periodo = " + Vble.Periodo +
                                  " AND C.Remesa = " + CBRemesaSola.Text;


                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                //Funciones.VerDetallePreDescarga(LabSaldos.Text, "", Vble.Periodo, CONSULTA);
                Funciones.VerDetallePreDescargaPorRemesa(LabSaldos.Text, Vble.Periodo, CONSULTA, true, "800", CBRemesaSola.Text, "Resumen",
                                             TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);
            }

            this.Cursor = Cursors.Default;

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        //Lecturas Imposibles
        private void button6_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (RBRuta.Checked)
            {
                    ValidarTBFechas();

                if (TextBoxRuta.Text != "")
                {
                    ruta = "AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }
                string CONSULTA = "";

                if (CBImposiblesApagados.Text == "Lecturas Imposibles")
                {

                        
                       CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                          "C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, " +
                                          "C.Operario, " +
                                          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                          "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                          "CONCAT('Correccion: ', N62.Observ), " +
                                          "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                          "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                         "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                         "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                         "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                         "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                         "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                         "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                         "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                         "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                         "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                         "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                          "WHERE C.ImpresionOBS = 609 " +
                                          " AND M.ActualFecha BETWEEN  '" + FiltroDESDE + "' " +
                                          " AND '" + FiltroHASTA + "' " +
                                          " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo =  " + Vble.Periodo +
                                          " AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                          ruta + " ";
                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    Funciones.VerDetallePreDescarga(label5.Text, Vble.Periodo, CONSULTA, true, "609", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");


                }
                else if (CBImposiblesApagados.Text == "Apagados")
                {
                   CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                        "C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, " +
                                        "C.Operario, " +
                                        "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                       "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                        "CONCAT('Correccion: ', N62.Observ), " +
                                        "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                        "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                        "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                        "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                        "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                        "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                        "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                        "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                        "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                        "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                        "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                        "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                        "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                        "WHERE C.ImpresionOBS MOD 100 = 17 " +
                                        "AND M.ActualFecha BETWEEN  '" + FiltroDESDE + "' " +
                                        "AND '" + FiltroHASTA + "' " +
                                        " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo =  " + Vble.Periodo +
                                        " AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                        ruta + " ";
                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    Funciones.VerDetallePreDescarga(label5.Text, Vble.Periodo, CONSULTA, true, "17", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
                }
               
               
            }
            else if (RBRemesaSola.Checked)
            {
                if (CBImposiblesApagados.Text == "Lecturas Imposibles")
                {
                    CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                         "C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, " +
                                         "C.Operario, " +
                                         "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                        "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                         "CONCAT('Correccion: ', N62.Observ), " +
                                         "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                         "CONCAT('Correccion: ', N6.Observ), " +
                                             "IF(N6.Observ = N62.Observ, " +
                                                 "N62.Observ, " +
                                                     "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                         "CONCAT('Correccion: ', N62.Observ), " +
                                                             "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                                "CONCAT('Correccion: ', N6.Observ), " +
                                                                     "IF(N6.Observ <> N62.Observ, " +
                                                                         "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                         "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                         "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                         "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                         "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                         "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                         "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                         "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                         "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                         "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                         "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                         "WHERE C.ImpresionOBS MOD 100 = 9 " +
                                         " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
                                         " AND C.Remesa = " + CBRemesaSola.Text;

                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    Funciones.VerDetallePreDescargaPorRemesa(CBImposiblesApagados.Text, Vble.Periodo, CONSULTA, true, "609", CBRemesaSola.Text, "Resumen",
                                             TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);



                }
                else if (CBImposiblesApagados.Text == "Apagados")
                {
                    CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                         "C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS Hora, " +
                                         "C.Operario, " +
                                         "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                        "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                         "CONCAT('Correccion: ', N62.Observ), " +
                                         "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                         "CONCAT('Correccion: ', N6.Observ), " +
                                             "IF(N6.Observ = N62.Observ, " +
                                                 "N62.Observ, " +
                                                     "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                         "CONCAT('Correccion: ', N62.Observ), " +
                                                             "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                                "CONCAT('Correccion: ', N6.Observ), " +
                                                                     "IF(N6.Observ <> N62.Observ, " +
                                                                         "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                         "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                         "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                         "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                         "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                         "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                         "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                         "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                         "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                         "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                         "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                         "WHERE C.ImpresionOBS MOD 100 = 17 " +
                                         " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo + 
                                         " AND C.Remesa = " + CBRemesaSola.Text;
                    Vble.TextDesdeInformes = FiltroDESDE;
                    Vble.TextHastaInformes = FiltroHASTA;
                    Funciones.VerDetallePreDescargaPorRemesa(CBImposiblesApagados.Text, Vble.Periodo, CONSULTA, true, "17", CBRemesaSola.Text, "Resumen",
                                                 TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);

                }
               

            }
            this.Cursor = Cursors.Default;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
            string txtRutas = "";

            if (tvExportadas.Nodes[0].Nodes.Count > 0)
            {
                foreach (TreeNode tNd1 in tvExportadas.Nodes[0].Nodes)
                {
                    txtRutas += $"{tNd1.Text}\n";
                    foreach (TreeNode tNd2 in tNd1.Nodes)
                    {
                        txtRutas += $"           {tNd2.Text} : \n";

                        foreach (TreeNode tNd3 in tNd2.Nodes)
                        {
                            txtRutas += "                   * " + tNd3.Text + "\n";
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("No existen rutas exportadas para generar el txt de rutas.", "Rutas exportadas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (txtRutas != "")
            {
                Vble.CrearArchivoTXT(txtRutas, "Rutas Exportadas " + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".txt");                
            }
           
        }

        private void impresasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS Nº_Instalacion, C.Contrato, C.TitularID AS Titular, P.Apellido, M.Numero, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, " +
            //              "C.Operario " +
            //              "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID " +
            //              "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo ";
            //Funciones.VerDetallePreDescarga(LabImprPre.Text, "601", Vble.Periodo, CONSULTA);
        }

        private void leidasNOImpresasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS Nº_Instalacion, C.Contrato, C.TitularID AS Titular, P.Apellido, M.Numero, M.AnteriorEstado, M.ActualEstado, C.Operario " +
            //            "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID " +
            //            "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo ";
            //Funciones.VerDetallePreDescarga(LabLeidNoImprePre.Text, "602", Vble.Periodo, CONSULTA);
        }

        private void noImpresasPorFueraDeRangoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS Nº_Instalacion, C.Contrato, C.TitularID AS Titular, P.Apellido, M.Numero, M.AnteriorEstado, M.ActualEstado, C.Operario " +
            //             "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID " +
            //             "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo ";

            //Funciones.VerDetallePreDescarga(LabNoImprFueraRango.Text, "604", Vble.Periodo, CONSULTA);

        }

        private void noImpresosPorOtrosMotivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, C.TitularID AS Titular, P.Apellido, M.Numero, M.AnteriorEstado, M.ActualEstado, C.Operario " +
                         "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID " +
                         "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                         "WHERE C.ImpresionOBS = 603 OR (C.ImpresionOBS > 604 AND C.ImpresionOBS <= 699)";
            Funciones.VerDetallePreDescarga(LabSaldos.Text, Vble.Periodo, CONSULTA, true, "603", CBRemesaRuta.Text, TextBoxDesde.Text, TextBoxHasta.Text, TextBoxRuta.Text, "Resumen");
        }

        private void saldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, C.TitularID AS Titular, P.Apellido, M.Numero, M.AnteriorEstado, M.ActualEstado, C.Operario " +
            //              "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID " +
            //              "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo ";
            //Funciones.VerDetallePreDescarga(LabSaldos.Text, "600", Vble.Periodo, CONSULTA);
        }

        

       
        //INDICADAS NO IMPRIMIR
        private void button8_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (RBRuta.Checked)
            {
                ValidarTBFechas();

                if (TextBoxRuta.Text != "")
                {
                    ruta = " AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }
                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                         "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, C.DomicSumin as Domicilio, M.Numero AS Medidor, M.AnteriorEstado, " +
                                         "M.ActualEstado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.ConsumoFacturado, " +
                                         "C.Operario, " +
                                         "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                         "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                         "CONCAT('Correccion: ', N62.Observ), " +
                                         "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                         "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                         "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                         "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                         "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                         "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                         "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                         "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                         "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                         "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                           "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                         "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                         "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                         "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                         "WHERE " +
                                         "C.ImpresionCOD = 1 AND M.ActualFecha BETWEEN '" + FiltroDESDE + "' AND '" +
                                         FiltroHASTA + "' AND C.Remesa = " + CBRemesaRuta.Text +
                                         " AND C.Periodo = " + Vble.Periodo +
                                         ruta;

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                Funciones.VerDetallePreDescarga(LabIndNoPrint.Text, Vble.Periodo, CONSULTA, true, "1", CBRemesaRuta.Text, FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
             
            }
            else if (RBRemesaSola.Checked)
            {
                string CONSULTA = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                        "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, C.DomicSumin as Domicilio, M.Numero AS Medidor, M.AnteriorEstado, " +
                                        "M.ActualEstado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.ConsumoFacturado, " +
                                        "C.Operario, " +
                                        "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, " +
                                        "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                        "CONCAT('Correccion: ', N62.Observ), " +
                                        "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                           "IF(N6.Observ = N62.Observ, " +
                                               "N62.Observ, " +
                                                   "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                       "CONCAT('Correccion: ', N62.Observ), " +
                                                           "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                              "CONCAT('Correccion: ', N6.Observ), " +
                                                                   "IF(N6.Observ <> N62.Observ, " +
                                                                       "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones " +
                                        "FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                        "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                        "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                        "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                        "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                        "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                        "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                          "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                        "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                        "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                        "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                        "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                        "WHERE " +
                                        "C.ImpresionCOD = 1 " +
                                        " AND C.Periodo = " + Vble.Periodo + 
                                        " AND C.Remesa = " + CBRemesaSola.Text;

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;
                Funciones.VerDetallePreDescargaPorRemesa(LabIndNoPrint.Text, Vble.Periodo, CONSULTA, true, "1", REMESA, "Resumen",
                                              TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);

            }
            this.Cursor = Cursors.Default;
        }


        private void informesToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            StringBuilder stb1 = new StringBuilder("", 250);
            Inis.GetPrivateProfileString("Carpetas", "Dir InfRutasExp", "", stb1, 250, Ctte.ArchivoIniName);
            string RutaCarpInformes = stb1.ToString();

            RutaCarpInformes = Vble.ValorarUnNombreRuta(RutaCarpInformes);
            if (!Directory.Exists(RutaCarpInformes))
            {
                Directory.CreateDirectory(RutaCarpInformes);
            }

            Process.Start(RutaCarpInformes);
        }

        private void LabLeidNoImprePre_Click(object sender, EventArgs e)
        {

        }

      
      

        private void LabIndNoPrint_Click(object sender, EventArgs e)
        {

        }

        private void CBRemesa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BGWEsperar_DoWork(object sender, DoWorkEventArgs e)
        {
            CargarTVRutasExportadas();
            //AgregarNodoEmpresaExportadas(Vble.Empresa, "logo");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (Char.IsDigit(e.KeyChar))
            {
              
                if (TextBoxDesde.TextLength == 2)
                {
                    TextBoxDesde.Text += "/";
                    TextBoxDesde.Select(TextBoxDesde.Text.Length, 0);
                }                
                else if (TextBoxDesde.TextLength == 5)
                {
                    TextBoxDesde.Text += "/";
                    TextBoxDesde.Select(TextBoxDesde.Text.Length, 0);
                }              
                else if (TextBoxDesde.TextLength == 10)
                {
                    MessageBox.Show("Formato de fecha invalido por favor reingrese.", "Fecha Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.None);
                    TextBoxDesde.Text = "";
                }
             
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Back))
            {
                e.Handled = false;
            }
            else          
            {
                TextBoxDesde.Text = "";
                e.Handled = false;
               
                //MessageBox.Show("Formato de fecha invalido por favor reingrese.", "Fecha Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.None);

            }

            //// Primero compruebo si es un signo de puntuación
            //if (char.IsPunctuation(e.KeyChar))
            //{
            //    // Referencio el control TextBox subyacente.
            //    //
            //    TextBox tb = (TextBox)sender;

            //    switch (e.KeyChar)
            //    {
            //        case '.':
            //        case ',':
            //            // Obtengo el carácter separador decimal existente
            //            // actualmente en la configuración regional de Windows.
            //            //
            //            string separadorDecimal = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //            e.KeyChar = Convert.ToChar(separadorDecimal[0]);

            //            if (tb.Text.Contains(separadorDecimal))
            //            {
            //                // Ya existe el separador decimal
            //                e.Handled = true;
            //            }
            //            break;
            //        case '-':
            //            if (!tb.Text.Contains('-'))
            //            {
            //                // Insertamos el carácter en la primera posición
            //                //
            //                tb.Text = tb.Text.Insert(0, Convert.ToString(e.KeyChar));
            //                // Envío la tecla final, para posicionarme
            //                // al final del contenido del texto.

            //                SendKeys.Send("{END}");
            //            }
            //            e.Handled = true;
            //            break;
            //    }
            //}
            //else if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Back))
            //{
            //}
            //// Tecla de retroceso; sin implementación.
            //else if (!char.IsNumber(e.KeyChar))
            //{
            //    // Sólo se aceptan números
            //    e.Handled = true;
            //}

        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (TextBoxDesde.Text == "dd/MM/yyyy")
            {
                TextBoxDesde.Clear();
                TextBoxDesde.ForeColor = Color.Black;

            }
        }

        private void TextBoxHasta_Enter(object sender, EventArgs e)
        {
            if (TextBoxHasta.Text == "dd/MM/yyyy")
            {
                TextBoxHasta.Text = "";
                TextBoxHasta.ForeColor = Color.Black;
            }
        }

        private void TextBoxDesde_Leave(object sender, EventArgs e)
        {
            if (TextBoxDesde.Text == "" || TextBoxDesde.TextLength != 10 ) 
            {
                TextBoxDesde.Text = "dd/MM/yyyy";
                TextBoxDesde.ForeColor = Color.Silver;
            }
        }

        private void TextBoxHasta_Leave(object sender, EventArgs e)
        {
            if (TextBoxHasta.Text == "")
            {
                TextBoxHasta.Text = "dd/MM/yyyy";
                TextBoxHasta.ForeColor = Color.Silver;
            }
        }

        private void TextBoxHasta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {               
                if (TextBoxHasta.TextLength == 2)
                {
                    TextBoxHasta.Text += "/";
                    TextBoxHasta.Select(TextBoxHasta.Text.Length, 0);
                }               
                else if (TextBoxHasta.TextLength == 5)
                {
                    TextBoxHasta.Text += "/";
                    TextBoxHasta.Select(TextBoxHasta.Text.Length, 0);
                }
                else if (TextBoxHasta.TextLength == 10)
                {
                    MessageBox.Show("Formato de fecha invalido por favor reingrese.", "Fecha Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.None);
                    TextBoxHasta.Text = "";
                }
            }
            else
            {
                TextBoxHasta.Text = "";
                e.Handled = false;
                //MessageBox.Show("Formato de fecha invalido por favor verifique.", "Fecha Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.None);
                
            }
        }

        private void TextBoxDesde_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxDesde_Validated(object sender, EventArgs e)
        {

        }

        private void LabNoImprFueraRango_Click(object sender, EventArgs e)
        {

        }

        private void informesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder stb1 = new StringBuilder("", 250);
            Inis.GetPrivateProfileString("Carpetas", "Dir InfRutasExp", "", stb1, 250, Ctte.ArchivoIniName);
            string RutaCarpInformes = stb1.ToString();

            RutaCarpInformes = Vble.ValorarUnNombreRuta(RutaCarpInformes);
            if (!Directory.Exists(RutaCarpInformes))
            {
                Directory.CreateDirectory(RutaCarpInformes);
            }

            Process.Start(RutaCarpInformes);
        }

        //ERRORES
        private void button10_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ValidarTBFechas();

            if (TextBoxRuta.Text != "")
            {
                ruta = "AND L.Ruta = " + TextBoxRuta.Text;
            }
            else
            {
                ruta = "";
            }

            CONSULTA = "SELECT L.ConexionID AS NInstalacion, L.Fecha, L.Hora, L.Periodo, L.Equipo, L.Ruta, L.Lecturista, C.titularID AS NUsuario, L.CodigoError, L.TextoError " +
                       "FROM LogErrores L JOIN Conexiones C USING (ConexionID, Periodo) WHERE L.Fecha BETWEEN '" + FiltroDESDE + "' AND '" + FiltroHASTA + "' " + ruta ;

            REMESA = CBRemesaRuta.Text;
            Funciones.VerDetallePreDescarga(LabelErrores.Text, Vble.Periodo, CONSULTA, true, "111", REMESA,
                                            FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");

            // Changes a specific shape of the cursor (here, the I-beam shape).
            //Input.SetCustomMouseCursor(beam, Input.CursorShape.Ibeam);
            this.Cursor = Cursors.Default;
        }

        private void cbLote_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Boton de Resumen TODOS: el mismo filtrará todos los casos en la fecha actual.
        //Se envia 999 como ImpresionOBS para hacer referencia a todos los casos de leidos a la hora de usarlo en el filtro 
        //por fecha
        public async void button9_Click_1(object sender, EventArgs e)
        {
            //Vble.ShowLoading();
            if (RBRuta.Checked)
            {
                ValidarTBFechas();
                if (TextBoxRuta.Text != "")
                {
                    ruta = "AND C.Ruta = " + TextBoxRuta.Text;
                }
                else
                {
                    ruta = "";
                }
                /////Consulta la tabla altas para obtener las inyecciones
                /////
                //CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //                 "C.titularID AS IC, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS HoraLect, " +
                //                 "if(ImpresionOBS = 400, '-', " +
                //                    "IF(ImpresionOBS = 500, '-', " +
                //                        "IF(ImpresionOBS = 0, 'NO LEIDO', " +
                //                            "IF(E.Titulo = 'Leído e Impreso', CONCAT(SUBSTRING(C.VencimientoProx, -4, 2), ':', SUBSTRING(C.VencimientoProx, -2)), IF(C.VencimientoProx <> 'Leído e Impreso', '-', '-'))))) AS HoraImp, " +
                //                 "if(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', IF (ImpresionOBS = 0,'NO LEIDO', E.Titulo))) AS Situacion, C.Operario, " +
                //                 "if(N1.Codigo <> 0, N1.Codigo, NULL) as Ord1, if (N2.Codigo <> 0, N2.Codigo, NULL) as Ord2, if (N3.Codigo <> 0, N3.Codigo, NULL) as Ord3, " +
                //                 "if (N4.Codigo <> 0, N4.Codigo, NULL) as Ord4, if (N5.Codigo <> 0, N5.Codigo, NULL) as Ord5,  " +
                //                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243), " +
                //                  "CONCAT('Correccion: ', N62.Observ), " +
                //                  "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242), " +
                //                  "CONCAT('Erroneo: ', N6.Observ), " +
                //                  "IF(N6.Observ = N62.Observ AND(N6.Codigo = 243), " +
                //                  "CONCAT('Correccion: ', N62.Observ), " +
                //                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 242), " +
                //                  "CONCAT('Erroneo: ', N6.Observ), " +
                //                  "IF(N6.Observ = N62.Observ, " +
                //                  "N62.Observ, " +
                //                  "IF(N6.Observ <> N62.Observ AND(N62.Codigo = 242), " +
                //                  "CONCAT('Erroneo: ', N6.Observ, ' ', N62.Observ), " +
                //                  "IF(N6.Observ <> N62.Observ AND(N62.Codigo = 243), " +
                //                  "CONCAT('Correccion ', N6.Observ, ' ', N62.Observ), " +
                //                 "N62.Observ))))))) as Observaciones,    " + 
                //                 "IF(Alt.Activa = 'Y',  Alt.Estado, '-') as Inyeccion " +
                //                 " FROM Conexiones C " +
                //                 "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                //                 "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
                //                 "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo  " +
                //                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                //                 "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                //                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                //                 "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                //                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                //                 "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                //                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                //                 "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                //                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                //                 "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                //                 "left JOIN(SELECT * FROM Altas WHERE Activa = 'Y' and Periodo = " + Vble.Periodo + ") Alt " +
                //                 "ON Alt.ConexionID = C.ConexionID AND Alt.Periodo = C.Periodo " +
                //                 "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                //                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                //                 "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                //                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                //                 "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                //                 "WHERE ((C.ImpresionOBS >= 0 AND M.ActualFecha BETWEEN '" + FiltroDESDE + "' AND '" + FiltroHASTA + "') " +
                //                 " OR (C.ImpresionOBS MOD 100 >= 0 and M.ActualFecha BETWEEN '2000-01-01' and '2000-01-01') AND C.Periodo = " + Vble.Periodo +
                //                 " AND C.Remesa = " + CBRemesaRuta.Text + " " +
                //                 ruta + ") " +
                //                 //"OR (C.ImpresionOBS = 800 OR C.ImpresionOBS = 500 or C.ImpresionOBS = 400 or C.ImpresionOBS = 0) " +
                //                 "AND C.Remesa = " + CBRemesaRuta.Text + " " +
                //                 ruta +
                //                 " AND C.Periodo = " + Vble.Periodo +
                //                 "  GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, HoraLect ASC, C.Secuencia";
                ///Consulta la tabla altas para obtener las inyecciones
                ///
                CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                 "C.titularID AS IC, " +
                                 "P.Apellido, " +
                                 "M.Numero AS Medidor, " +
                                 "C.DomicSumin as Domicilio, " +
                                 "M.AnteriorEstado, " +
                                 "M.ActualEstado, " +
                                 "C.ConsumoFacturado, " +
                                 "M.ActualFecha AS Fecha, " +
                                 "M.ActualHora AS HoraLect, " +
                                 "date_format((str_to_date(I.Fec_Actual, '%d/%m/%Y')), '%Y-%m-%d')  as FechaReg, " +
                                 "Replace(I.Lec_Actual, '.', '') as LectReg, " +
                                 "if(ImpresionOBS = 400, '-', " +
                                    "IF(ImpresionOBS = 500, '-', " +
                                        "IF(ImpresionOBS = 0, 'NO LEIDO', " +
                                            "IF(E.Titulo = 'Leído e Impreso', CONCAT(SUBSTRING(C.VencimientoProx, -4, 2), ':', SUBSTRING(C.VencimientoProx, -2)), IF(C.VencimientoProx <> 'Leído e Impreso', '-', '-'))))) AS HoraImp, " +
                                 "IF(C.ImpresionCOD = 0, 'NORMAL', IF(C.ImpresionCOD = 1, 'SOLO LECTURA N/F-N/I', IF(C.ImpresionCOD = 2, 'PRESUMIDOR N/F-N/I', IF(C.ImpresionCOD = 3, 'TELE LECT CONSUMIDOR', \r\n\tIF(C.ImpresionCOD = 4, 'TELE LECT PROSUMIDOR N/F-N/I', IF(C.ImpresionCOD = 5, 'CONSUMIDOR COMUN F-N/I', IF(C.ImpresionCOD = 6, 'PROSUMIDOR N/F-N/I', IF(C.ImpresionCOD = 7, 'TELE LECT CONSUMIDOR F-N/I', \r\n\tIF(C.ImpresionCOD = 8, 'TELE LECT PROSUMIDOR S/L', '-'))))))))) AS 'Condición', " +
                                 "IF(C.ImpresionOBS MOD 100 = 0, '-', IF((I.Estimado = 'T') OR (Replace(I.Lec_Actual, '.', '') < M.ActualEstado), Concat('T: ', I.Lec_Actual), IF(C.ImpresionOBS MOD 100 > 0, IF(M.ActualEstado = -1 , 'F: Estimado', IF(M.ActualEstado = -2, 'F: Apagado', CONCAT('F:', M.ActualEstado))), CONCAT('F:', I.Lec_Actual)))) as Tipo_Lectura, " +
                                 "IF(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', IF (ImpresionOBS = 0,'NO LEIDO', E.Titulo))) AS Situación, " +
                                 "C.Operario, " +
                                 "if(N1.Codigo <> 0, N1.Codigo, NULL) as Ord1, if (N2.Codigo <> 0, N2.Codigo, NULL) as Ord2, if (N3.Codigo <> 0, N3.Codigo, NULL) as Ord3, " +
                                 "if (N4.Codigo <> 0, N4.Codigo, NULL) as Ord4, if (N5.Codigo <> 0, N5.Codigo, NULL) as Ord5,  " +
                                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243), " +
                                  "CONCAT('Correccion: ', N62.Observ), " +
                                  "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242), " +
                                  "CONCAT('Erroneo: ', N6.Observ), " +
                                  "IF(N6.Observ = N62.Observ AND(N6.Codigo = 243), " +
                                  "CONCAT('Correccion: ', N62.Observ), " +
                                  "IF(N6.Observ = N62.Observ AND(N62.Codigo = 242), " +
                                  "CONCAT('Erroneo: ', N6.Observ), " +
                                  "IF(N6.Observ = N62.Observ, " +
                                  "N62.Observ, " +
                                  "IF(N6.Observ <> N62.Observ AND(N62.Codigo = 242), " +
                                  "CONCAT('Erroneo: ', N6.Observ, ' ', N62.Observ), " +
                                  "IF(N6.Observ <> N62.Observ AND(N62.Codigo = 243), " +
                                  "CONCAT('Correccion ', N6.Observ, ' ', N62.Observ), " +
                                 "N62.Observ))))))) as Observaciones,    " +
                                 "IF(Alt.Activa = 'Y',  Alt.Estado, '-') as Inyeccion, " +
                                 "IF(Alt.Activa = 'R',  Alt.Estado, '-') as Est_Reac_GU " +
                                 " FROM Conexiones C " +
                                 "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                 "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
                                 "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo  " +
                                 "LEFT JOIN Impresor I " +
                                 "ON C.ConexionID = I.ConexionID AND C.Periodo = I.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                 "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                 "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                 "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                 "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                 "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                 "left JOIN(SELECT * FROM Altas WHERE (Activa = 'Y' OR Activa = 'R') and Periodo = " + Vble.Periodo + ") Alt " +
                                 "ON Alt.ConexionID = C.ConexionID AND Alt.Periodo = C.Periodo " +
                                 "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                 "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                 "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                 "WHERE ((C.ImpresionOBS >= 0 AND M.ActualFecha BETWEEN '" + FiltroDESDE + "' AND '" + FiltroHASTA + "') " +
                                 " OR (C.ImpresionOBS MOD 100 >= 0 and M.ActualFecha BETWEEN '2000-01-01' and '2000-01-01') AND C.Periodo = " + Vble.Periodo +
                                 " AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                 ruta + ") " +
                                 //"OR (C.ImpresionOBS = 800 OR C.ImpresionOBS = 500 or C.ImpresionOBS = 400 or C.ImpresionOBS = 0) " +
                                 "AND C.Remesa = " + CBRemesaRuta.Text + " " +
                                 ruta +
                                 " AND C.Periodo = " + Vble.Periodo +
                                 "  GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, HoraLect ASC, C.Secuencia";

                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                REMESA = CBRemesaRuta.Text;
                Vble.lecturistas.Clear();
                Vble.lectOrd.Clear();
                Funciones.VerDetallePreDescarga(LabelTodos.Text, Vble.Periodo, CONSULTA, true, "999", REMESA,
                                                FiltroDESDE, FiltroHASTA, TextBoxRuta.Text, "Resumen");
            }
            else if (RBRemesaSola.Checked)
            {
                /// Consulta la tabla altas para obtener las inyecciones
                ///
                CONSULTA = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                 "C.titularID AS IC, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha AS Fecha, M.ActualHora AS HoraLect, " +
                                 "if(ImpresionOBS = 400, '-', " +
                                 "IF(ImpresionOBS = 500, '-', " +
                                       "IF(ImpresionOBS = 0, 'NO LEIDO', " +
                                           "IF(E.Titulo = 'Leído e Impreso', CONCAT(SUBSTRING(C.VencimientoProx, -4, 2), ':', SUBSTRING(C.VencimientoProx, -2)), IF(C.VencimientoProx <> 'Leído e Impreso', '-', '-'))))) AS HoraImp, " +
                                 "if(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', IF (ImpresionOBS = 0,'NO LEIDO', E.Titulo))) AS Situacion, " +
                                 "C.Operario, " +
                                 "if(N1.Codigo <> 0, N1.Codigo, NULL) as Ord1, if (N2.Codigo <> 0, N2.Codigo, NULL) as Ord2, if (N3.Codigo <> 0, N3.Codigo, NULL) as Ord3, " +
                                 "if (N4.Codigo <> 0, N4.Codigo, NULL) as Ord4, if (N5.Codigo <> 0, N5.Codigo, NULL) as Ord5,  " +
                                 "IF(N6.Observ = N62.Observ AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                    "CONCAT('Correccion: ', N62.Observ), " +
                                      "IF(N6.Observ = N62.Observ AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                        "CONCAT('Correccion: ', N6.Observ), " +
                                            "IF(N6.Observ = N62.Observ, " +
                                                "N62.Observ, " +
                                                    "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N62.Codigo = 243 OR N62.Codigo = 242), " +
                                                        "CONCAT('Correccion: ', N62.Observ), " +
                                                            "IF(N6.Observ <> '' OR N62.Observ <> '' AND(N6.Codigo = 242 OR N6.Codigo = 243), " +
                                                               "CONCAT('Correccion: ', N6.Observ), " +
                                                                    "IF(N6.Observ <> N62.Observ, " +
                                                                        "CONCAT(N6.Observ, ' ', N62.Observ), N62.Observ)))))) as Observaciones,  " +
                                 "IF(Alt.Activa = 'Y', CONCAT('(INY:', Alt.Estado, ')'), 'No contiene inyección') as Inyeccion " +
                                 " FROM Conexiones C " +
                                 "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                                 "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
                                 "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo  " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                                 "ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                                 "ON N2.ConexionID = C.ConexionID AND N2.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                                 "ON N3.ConexionID = C.ConexionID AND N3.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                                 "ON N4.ConexionID = C.ConexionID AND N4.Periodo = C.Periodo " +
                                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                                 "ON N5.ConexionID = C.ConexionID AND N5.Periodo = C.Periodo " +
                                 "left JOIN(SELECT * FROM Altas WHERE Activa = 'Y' and Periodo = " + Vble.Periodo + ") Alt " +
                                 "ON Alt.ConexionID = C.ConexionID AND Alt.Periodo = C.Periodo " +
                                 "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                 "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                 "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                 "WHERE ((C.ImpresionOBS >= 0) " +
                                 " OR (C.ImpresionOBS MOD 100 >= 0) AND C.Periodo = " + Vble.Periodo +
                                 " AND C.Remesa = " + CBRemesaSola.Text + ") " +
                                 //"OR (C.ImpresionOBS = 800 OR C.ImpresionOBS = 500 or C.ImpresionOBS = 400 or C.ImpresionOBS = 0) " +
                                 "AND C.Remesa = " + CBRemesaSola.Text + " " +
                                 " AND C.Periodo = " + Vble.Periodo +
                                 " AND C.Zona = " + CBFiltroZona.Text +
                                 "  GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, HoraLect ASC, C.Secuencia";


                Vble.TextDesdeInformes = FiltroDESDE;
                Vble.TextHastaInformes = FiltroHASTA;

                REMESA = CBRemesaSola.Text;

                Funciones.VerDetallePreDescargaPorRemesa(LabelTodos.Text, Vble.Periodo, CONSULTA, true, "999", REMESA, "Resumen",
                                                TextBoxRuta.Text, "Resumen", "SI", CBFiltroZona.Text);
            }
          

        }

        /// <summary>
        /// Metodo que contiene la consulta Todos a la que se llama desde el Task del boton TODOS.
        /// </summary>
        public void ConsultaTodos()
        {
          
        }

        public Action SolicitarInforme(string LeyendaImpresion, int Periodo, string CONSULTA,
                                                 bool NoImpresos, string ImpresionOBS, string Remesa,
                                                 string Desde, string Hasta, string Ruta)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();

            DetalleImpresos.IndicadorTipoInforme = "Resumen";
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            //DetalleImpresos.CONSULTANOIMPRESOS = "";
            DetalleImpresos.CONSULTANOIMPRESOS = CONSULTA;
            DetalleImpresos.Periodo = Vble.Periodo;

            DetalleImpresos.TextBoxRuta.Text = Ruta;
            DetalleImpresos.Ruta = Ruta;
            //DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.NoImpr = NoImpresos;
            DetalleImpresos.WindowState = FormWindowState.Maximized;
            DetalleImpresos.ImpresionOBS = ImpresionOBS;
            DetalleImpresos.Remesa = Remesa;
            DetalleImpresos.Desde = Desde;
            DetalleImpresos.Hasta = Hasta;
            DetalleImpresos.Visible = false;
            DetalleImpresos.CBRemesa.Text = Remesa;
            DetalleImpresos.DTPDesdeTomLect.Text = Convert.ToDateTime(Desde).ToString("dd/MM/yyyy");
            DetalleImpresos.DTPHastaTomLect.Text = Convert.ToDateTime(Hasta).ToString("dd/MM/yyyy");
            DetalleImpresos.Show();
            return null;
        }
    
        private void informesDeExportacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder stb1 = new StringBuilder("", 250);
            Inis.GetPrivateProfileString("Carpetas", "Dir InfRutasExp", "", stb1, 250, Ctte.ArchivoIniName);
            string RutaCarpInformes = stb1.ToString();

            RutaCarpInformes = Vble.ValorarUnNombreRuta(RutaCarpInformes);
            if (!Directory.Exists(RutaCarpInformes))
            {
                Directory.CreateDirectory(RutaCarpInformes);
            }
            Process.Start(RutaCarpInformes);
        }

        private void ChecCambiarFechaLectura_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckCambiarFechaLectura.Checked == true)
            {
                GBCambioFecha.Visible = true;
            }
            else if (CheckCambiarFechaLectura.Checked == false)
            {
                GBCambioFecha.Visible = false;
            }
            else
            {
                GBCambioFecha.Visible = false;
            }
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var Form7Inf = new Form7InformesAltas();
            Form7Inf.MdiParent = this.MdiParent;
            Form7Inf.WindowState = FormWindowState.Maximized;
            if (TextBoxDesde.Text == "dd/MM/yyyy")
            {
                TextBoxDesde.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            if (TextBoxHasta.Text == "dd/MM/yyyy")
            {
                TextBoxHasta.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            DateTime fechaDesde = Convert.ToDateTime(TextBoxDesde.Text);
            DateTime fechaHasta = Convert.ToDateTime(TextBoxHasta.Text);
            Form7InformesAltas.PantallaSolicitud = "Exportacion";
            Form7InformesAltas.RutaDesdeExportacion = TextBoxRuta.Text;
            Form7InformesAltas.FechaDesdeExportacion = fechaDesde.ToString("yyyy-MM-dd");
            Form7InformesAltas.FechaHastaExportacion = fechaHasta.ToString("yyyy-MM-dd");


            Form7Inf.Show();
        }

        private async void button12_Click(object sender, EventArgs e)
        {
        

        }

        private void tvExportadas_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {

                TreeNode Nodo = tvExportadas.SelectedNode;
                tvConexUpdload.Items.Clear();
                int idx = Nodo.Index;
                //int Distr;
                //string sKy = Nodo.Tag.ToString();
                clInfoNodos tn = new clInfoNodos();

                tn = dcNodos[Nodo.Tag.ToString()];
                //string clave = "";
                //int Lote, Ruta;

                if (Nodo != null)
                {
                    if (Nodo.Level == 3)
                    {
                        tn.ImageKey = "todo";
                        //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
                        if (Nodo.ForeColor == Color.Empty)
                        {
                            ArrayLotes.Clear();
                            ArrayRuta.Clear();
                            ArrayRemesa.Clear();
                            ArrayRemesaRuta.Clear();
                            ArrayLocalidad.Clear();

                            tvExportadas.SelectedNode.ForeColor = Color.Gray;

                        }
                        else if (Nodo.ForeColor == Color.Gray)
                        {
                            tvExportadas.SelectedNode.ForeColor = Color.Empty;
                            //Nodo.ForeColor = Color.Black;
                            dataGridView1.DataSource = "";
                            TablaUpload1.Clear();
                            TablaUpload2.Clear();
                            ArrayLotes.Clear();
                            ArrayRuta.Clear();
                            ArrayRemesa.Clear();
                            ArrayRemesaRuta.Clear();
                            ArrayLocalidad.Clear();
                            tvConexUpdload.Items.Clear();
                            Vble.CantRegistros = 0;
                            //aca iba el foreach

                        }

                        //Recorre los nodos y toma solo los que estan seleccionados (gris)
                        RecorrerNodosExportados(sender, e);

                        //MessageBox.Show(Vble.CantConex.ToString());
                    }

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }



        }

        /// <summary>
        ///Metodo que contiene foreach anidados el cual recorre los nodos seleccionados para cargar los arraylist con
        ///los valores de         
        ///Ruta;
        ///Lotes;     
        ///para luego realizar la consulta que obtendra las conexiones las rutas seleccionadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RecorrerNodosExportados(object sender, EventArgs e)
        {
            Vble.CantConex = 0;

            try
            {
                //string clave = "";
                int Lote = 0, Ruta = 0, Distr = 0, Seleccionados = 0, Remesa = 0, Disponibles = 0;

                foreach (TreeNode tNd1 in tvExportadas.Nodes[0].Nodes)
                {
                    foreach (TreeNode tNd2 in tNd1.Nodes)
                    {
                        if (tNd2.Nodes.Count > 0)
                        {
                            foreach (TreeNode tNd3Rutas in tNd2.Nodes)
                            {
                                if (tNd3Rutas.ForeColor.Name == "Gray")
                                {
                                    Seleccionados++;
                                    clInfoNodos tnn = new clInfoNodos();
                                    tnn = dcNodos[tNd3Rutas.Tag.ToString()];
                                    Lote = tnn.Remesa;
                                    Remesa = tnn.Remesa;
                                    Distr = tnn.Distrito;
                                    Ruta = tnn.Ruta;
                                    //MessageBox.Show(Remesa.ToString() + ' ' +  Ruta.ToString() );
                                    CBRemesaRuta.Text = Remesa.ToString();
                                    TextBoxRuta.Text = Ruta.ToString();
                                    tvExportadas.SelectedNode.ForeColor = Color.Empty;






                                }
                            }
                        }
                    }
                }

                //if (Seleccionados != 0)
                //{
                //    ObtenerConexionesXloteYDistr(Vble.Lote, Distr, Ruta, Vble.Periodo, Vble.Remesa, 0);
                //}
                //else
                //{
                //}

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al recorrer Nodos");

            }
        }

        private void LabelAltas_Click(object sender, EventArgs e)
        {

        }

        private void LabImprPre_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void RBRemesasTodas_CheckedChanged(object sender, EventArgs e)
        {
           

         
           
        }

        private void RBRemesasIndiv_CheckedChanged(object sender, EventArgs e)
        {
           
           
        }

        private void RBRemesasIndiv_CheckedChanged_1(object sender, EventArgs e)
        {
          
        }

        private void RBRemesasTodas_KeyDown(object sender, KeyEventArgs e)
        {
            if (RBRemesasTodas.Checked == false)
            {                
                RBRemesasTodas.Checked = true;              
                LabelRemesaRuta.Visible = false;
                LabelRuta.Visible = false;
                LabelDesde.Visible = false;
                LabelHasta.Visible = false;
                CBRemesaRuta.Visible = false;
                TextBoxRuta.Visible = false;
                TextBoxDesde.Visible = false;
                TextBoxHasta.Visible = false;
            }
        }

        private void RBRemesasIndiv_KeyDown(object sender, KeyEventArgs e)
        {
            if (RBRuta.Checked == false)
            {
                RBRuta.Checked = true;
                RBRemesasTodas.Checked = false;
                RBRemesaSola.Checked = false;
                LabelRemesaRuta.Visible = true;
                LabelRuta.Visible = true;
                LabelDesde.Visible = true;
                LabelHasta.Visible = true;
                CBRemesaRuta.Visible = true;
                TextBoxRuta.Visible = true;
                TextBoxDesde.Visible = true;
                TextBoxHasta.Visible = true;
            }
        }

        private void RBRemesasTodas_CheckedChanged_1(object sender, EventArgs e)
        {
            if (RBRemesasTodas.Checked == true)
            {              
                LabelRemesaRuta.Visible = false;
                LabelRuta.Visible = false;
                LabelDesde.Visible = false;
                LabelHasta.Visible = false;
                CBRemesaRuta.Visible = false;
                TextBoxRuta.Visible = false;
                TextBoxDesde.Visible = false;
                TextBoxHasta.Visible = false;
                //LabelRemesaSola.Visible = false;
                CBRemesaSola.Visible = false;
            }
        }

        private void RBRemesasIndiv_CheckedChanged_2(object sender, EventArgs e)
        {
            if (RBRuta.Checked == true)
            {             
                LabelRemesaRuta.Visible = true;
                LabelRuta.Visible = true;
                LabelDesde.Visible = true;
                LabelHasta.Visible = true;
                CBRemesaRuta.Visible = true;
                TextBoxRuta.Visible = true;
                TextBoxDesde.Visible = true;
                TextBoxHasta.Visible = true;
                //LabelRemesaSola.Visible = false;
                CBRemesaSola.Visible = false;
            }
        }

        private void RBRemesaSola_CheckedChanged(object sender, EventArgs e)
        {
            if (RBRemesaSola.Checked == true)
            {
                LabelRemesaRuta.Visible = false;
                LabelRuta.Visible = false;
                LabelDesde.Visible = false;
                LabelHasta.Visible = false;
                CBRemesaRuta.Visible = false;
                TextBoxRuta.Visible = false;
                TextBoxDesde.Visible = false;
                TextBoxHasta.Visible = false;
                //LabelRemesaSola.Visible = true;
                CBRemesaSola.Visible = true;
            }
        }

        private void RBRemesaSola_KeyDown(object sender, KeyEventArgs e)
        {
            if (RBRemesaSola.Checked == false)
            {
                RBRemesaSola.Checked = true;
                RBRuta.Checked = false;
                RBRemesasTodas.Checked = false;               
                LabelRemesaRuta.Visible = false;
                LabelRuta.Visible = false;
                LabelDesde.Visible = false;
                LabelHasta.Visible = false;
                CBRemesaRuta.Visible = false;
                TextBoxRuta.Visible = false;
                TextBoxDesde.Visible = false;
                TextBoxHasta.Visible = false;
                //LabelRemesaSola.Visible = true;
                CBRemesaSola.Visible = true;
            }
        }

        private void BGWLoading_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void BGWLoading_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void cbTodo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxDesde_Click(object sender, EventArgs e)
        {
            TextBoxDesde.Clear();
            TextBoxDesde.ForeColor = Color.Black;
        }
    }
}

