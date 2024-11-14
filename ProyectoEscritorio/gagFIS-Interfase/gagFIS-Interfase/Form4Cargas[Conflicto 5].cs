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

using PortableDeviceApiLib;
using WPDSpLib;


namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form4Cargas.
    /// </summary>
    public partial class Form4Cargas : Form {
        private int QueTimer;
        private Dictionary<string, clInfoNodos> dcNodos = new Dictionary<string, clInfoNodos>();
        Computer mycomputer = new Computer();


        /// <summary>
        ///agregado para control de colectora
        /// </summary>
        StandardWindowsPortableDeviceService service = new StandardWindowsPortableDeviceService();
        WindowsPortableDevicesLib.Domain.PortableDeviceFolder currentFolder = null;
        //IList<WindowsPortableDevice> contenido = null;
        IList<WindowsPortableDevicesLib.Domain.PortableDeviceObject> currentContent = null;


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
        public static DataTable TabRegSelec = new DataTable();
        
        

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

        



        private void form4_Load(object sender, System.EventArgs e) {
            
            //Carga las imagenes para nodos
            Image Im1;
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Todo.gif");
            imgList1.Images.Add("Todo", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Algo.gif");
            imgList1.Images.Add("Algo", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Nada.gif");
            imgList1.Images.Add("Nada", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Symbol-Check-2.gif");
            imgList1.Images.Add("GenOk", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Symbol-Error-3.gif");
            imgList1.Images.Add("Error", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\ICONOS\\Regular\\Favorites.gif");
            imgList1.Images.Add("EnPro", Im1);
            Im1 = new System.Drawing.Bitmap(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
            imgList1.Images.Add("Logo", Im1);
            Im1 = null; 
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.progressBar1.Visible = false;           
            //this.WindowState = FormWindowState.Maximized;
            this.toolTip1.SetToolTip(this.BotEnviarCarga, "Envia los archivos seleccionados de la PC a la Colectora");
            this.toolTip2.SetToolTip(this.BotActPanPC, "Actualiza el Panel de Rutas para Cargar y Rutas Cargadas");
            this.toolTip3.SetToolTip(this.BotProcesCarg, "Genera el archivo con las conexiones seleccionadas");
            this.toolTip4.SetToolTip(this.BotDevCarga, "Devuelve la Carga Generada al panel de Rutas Disponibles");


            //-----------------------------------------------------------------------------------------------------------------

            QueTimer = 1;
            timer1.Interval = 1000;
            timer1.Enabled = true;

            ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
            shellView2.CurrentFolder = folder;

            BotActPanPC_Click(sender, e);
            //CargasProcesadas();
        }
        

       //Boton que cierra el formulario actual
        void btnCerrar_Click(object sender, EventArgs e) {
            //DB.con.Close();
            this.Close(); 
        }

        private void Form4_Resize(object sender, System.EventArgs e) {
            //this.WindowState = FormWindowState.Maximized;
        }

        
        /// <summary>
        /// Ejecuta algo, segun el valor de QueTimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e) {
            switch(QueTimer) {
                case 1:
                    //Carga las rutas disponibles en la lista
                    timer1.Enabled = false;
                    CargarListaRutas();
                                       
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
            string txSQL;
            int Distr, Rem, Rut, Sec;
            string Par;
            
            try {
                //Lee la tabla conexiones del periodo y sin leer
                txSQL = "SELECT conexiones.conexionid,conexiones.zona,conexiones.remesa," +
                            "conexiones.ruta, conexiones.secuencia,infoconex.particion" +
                        " FROM conexiones, infoconex" +
                        " WHERE (conexiones.conexionid=infoconex.conexionid " +
                        " AND conexiones.periodo = " + Vble.Periodo +
                        " AND conexiones.ImpresionOBS = " + 0 + ") OR (conexiones.conexionid=infoconex.conexionid " +
                        " AND conexiones.periodo = " + Vble.Periodo +
                        " AND conexiones.ImpresionOBS = " + 500 + ")" +
                        " ORDER BY conexiones.zona,conexiones.remesa,conexiones.ruta,conexiones.secuencia";

                Tabla = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                tvwCargas.Nodes.Clear();
                dcNodos.Clear();
                AgregarNodoEmpresa(Vble.Empresa, "logo");
                
                //Recorre la tabla y carga las ramas del arbol
                foreach(DataRow Fila in Tabla.Rows) {
                    Distr = Fila.Field<int>("zona");
                    if(Distr < 100) Distr += 200;
                    Rem = Fila.Field<int>("remesa");
                    Rut = Fila.Field<int>("ruta");
                    Sec = Fila.Field<int>("secuencia");
                    Par = Fila.Field<string>("particion");

                    if(AgregarNodoDistrito(Vble.Empresa, Distr))
                        if(AgregarNodoRemesa(Vble.Empresa, Distr, Rem))
                            if(AgregarNodoRuta(Vble.Empresa, Distr, Rem, Rut))
                                AgregarNodoParticionA(Vble.Empresa, Distr, Rem, Rut, Par, Sec);
                               

                }
                tvwCargas.Nodes[Vble.Empresa.ToLower()].ExpandAll();
                TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
                tvwCargas.Sort();

            }
            catch (Exception e) {
                MessageBox.Show(e.Message + "- en: " + e.TargetSite.Name);
                retorno = false;
            }

            BloqueoClick = false;
            return retorno;
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
                if(!tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes.ContainsKey(sKPt)) {
                    //Agrega el nodo remesa
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes.Add(sKPt, sPar,"nada");
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
                    Application.DoEvents();

                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
                return false;                
            }


            if(tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes.ContainsKey(sKPt)) {
                tn = dcNodos[sKPt];
                
                tn.CnxSelected++;
                tn.CnxTotal++;
                if(tn.Hasta < Secuencia) tn.Hasta = Secuencia;
                if(tn.Desde > Secuencia) tn.Desde = Secuencia;
                
                //Actualiza el texto del nodo
                tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sRut].Nodes[sKPt].Text = sPar +" (" +
                    tn.Desde.ToString().Trim()+ " a "+ tn.Hasta.ToString().Trim() +")["+
                    tn.CnxTotal.ToString().Trim() + "]";
                
                tvwCargas.ExpandAll();
                
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
                if(!tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.ContainsKey(sKRt)) {
                    //Agrega el nodo remesa
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes.Add(sKRt, "Ruta:"+ Ruta.ToString().Trim(), "nada");
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].Tag = sKRt;
                    tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes[sRem].Nodes[sKRt].BackColor = Color.FloralWhite;

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
            catch(Exception ex) {
                MessageBox.Show(ex.Message + " - en: " + ex.TargetSite.Name);
                return false;
            }
            return true ;

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
                if(!tvwCargas.Nodes[sEmp].Nodes[sDtr].Nodes.ContainsKey(sKRm)) {
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
                
                
                if(Distrito < 100) Distrito += 200;
                sD = Distrito.ToString().Trim();
                sKey = Vble.Empresa.ToLower() + sD;

                if(!tvwCargas.Nodes[Vble.Empresa.ToLower()].Nodes.ContainsKey(sKey)) {
                    //No está el distrito, debe agregarlo
                    txSQL = "SELECT  * FROM localidades " +
                       "WHERE codigoint=" + sD;
                    tabZona = new DataTable();
                    datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(tabZona);
                    Loc = "-";
                    if(tabZona.Rows.Count > 0)
                        Loc = tabZona.Rows[0].Field<string>("localidad");
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

            catch(Exception e) {
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
            if(!tvwCargas.Nodes.ContainsKey(sClave)) {
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
                    
            if(ndX.Nodes.Count > 0) {
                tIN.CnxSelected = 0;
                tIN.CnxTotal = 0;
                ndH = ndX.FirstNode;
                while(ndH != null) {
                    //if(pCancelar) Err.Raise 20015, "Aplicar Estado a Hijos ", " Proceso Cancelado por el Usuario "
                    //ndH.Tag = ndH.Tag ?? tIN.Key;   //= infNdX[ndH.Index];

                    ndH.ImageKey = ndX.ImageKey;
                    dcNodos[ndH.Tag.ToString()].ImageKey = ndX.ImageKey;

                    AplicarEstadoAHijos(ndH);
                    if(ndX.ImageKey.ToLower() == "todo") tIN.CnxSelected += dcNodos[ndH.Tag.ToString()].CnxTotal;
                    tIN.CnxTotal += dcNodos[ndH.Tag.ToString()].CnxTotal;
                    if(ndX == ndH.LastNode) break;
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
            foreach(TreeNode ndH in ndP.Nodes) {
                //si tiene hijos, recursividad.
                if(ndH.Nodes.Count > 0)
                    TomarEstadoDeHijos(ndH);
                else {
                    //Si no tiene hijos, toma seleccion según imagen
                    if(dcNodos[ndH.Tag.ToString()].ImageKey == "todo")
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
            if(CxSel == 0)
                dcNodos[ndP.Tag.ToString()].ImageKey = "nada";
            else if(CxTot==CxSel )
                dcNodos[ndP.Tag.ToString()].ImageKey = "todo";
            else
                dcNodos[ndP.Tag.ToString()].ImageKey = "algo";

            //Muestra el estado
            ndP.ImageKey = dcNodos[ndP.Tag.ToString()].ImageKey;
            ndP.Text = dcNodos[ndP.Tag.ToString()].Texto + 
                "  [ " + dcNodos[ndP.Tag.ToString()].CnxSelected.ToString() +
                " de " + dcNodos[ndP.Tag.ToString()].CnxTotal.ToString() + " ]";

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

            switch(fD.ShowDialog(this)) {
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

            if(Desde == 0 && Hasta > 0 && Cantidad > 0)
                AscDesc = "DESC";

            Tabla = new DataTable();

            //Adecuación de límites.
            iDesde = Desde < inN.Desde ? inN.Desde : Desde;
            iHasta = (Hasta == 0) || (Hasta > inN.Hasta) ? inN.Hasta : Hasta;
            iCnt = Cantidad == 0 ? inN.CnxTotal : Cantidad;

            Tabla = new DataTable();

            //Seleccionar las conexiones que integrarán la nueva partición
            txSQL = "SELECT C.conexionid, I.particion, C.secuencia, C.ruta " +
                " FROM  conexiones C JOIN infoconex I ON C.conexionid=I.conexionid" +
                " WHERE I.particion = '" + inN.Particion + "'" +
                " AND I.codigoimpresion = 0" +
                " AND C.ruta = " + inN.Ruta.ToString().Trim() +
                " AND C.secuencia >= " + iDesde.ToString().Trim() +
                " AND C.secuencia <=" + iHasta.ToString().Trim() +
                " ORDER BY C.secuencia " + AscDesc +
                " LIMIT " + iCnt; 


            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            //Si la cantidad de registros es igual al total de la partición se informa
            if(Tabla.Rows.Count == inN.CnxTotal) {
                MessageBox.Show(" Está seleccionando una forma de particionar que incluye "
                             + "\nla totalidad de las conexiones de la partición."
                             + "\nNo tiene sentido, se desestima la nueva partición!!. ",
                             "Partición Mal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Buscar la última letra usada en las particiones de la ruta.
            txSQL = "SELECT DISTINCT(particion) " +
                " FROM  conexiones C JOIN infoconex I ON C.conexionid=I.conexionid" +
                " AND I.codigoimpresion = 0" +
                " AND C.ruta = " + inN.Ruta.ToString().Trim() +
                " AND C.secuencia >= " + iDesde.ToString().Trim() +
                " AND C.secuencia <=" + iHasta.ToString().Trim() +
                " ORDER BY particion";

           
            DataTable TabPart = new DataTable();
            MySqlDataAdapter dtAdap = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder cmdSql = new MySqlCommandBuilder(dtAdap);
            dtAdap.Fill(TabPart);

            int cnt = TabPart.Rows.Count;
            if(cnt > 0) {
                DataRow Fila = TabPart.Rows[cnt - 1];
                viejaLetra = Fila[0].ToString().ToUpper();
            }

            List<string> Nuevas = new List<string> {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
                                  "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            //Si la letra vieja no es "Z" pone la que le sigue
            if(viejaLetra != "Z") {
                nuevaLetra = Nuevas[Nuevas.IndexOf(viejaLetra) + 1];
            }
            else {
                //si la vija es 'Z' busca la primera libre.
                List<string> Viejas = new List<string>();
                foreach(DataRow Fila in TabPart.Rows)
                    Viejas.Add(Fila[0].ToString());

                //Busca primera de Nuevas que no esté en Viejas
                nuevaLetra = viejaLetra;
                foreach(string Ltr in Nuevas)
                    if(!Viejas.Contains(Ltr)) {
                        nuevaLetra = Ltr;
                        break;
                    }
            }

            //Si las letras nuevas y viejas son iguales, NO hace la partición
            if(nuevaLetra == viejaLetra)
                return;
           
            List<int> Fla = new List<int>();
            for(int i = 0; i < iCnt && i < Tabla.Rows.Count; i++)
                Fla.Add(Tabla.Rows[i].Field<int>("conexionid"));

            txSQL = "UPDATE infoconex " +
                    "SET particion = '" + nuevaLetra + "'" +
                    " WHERE conexionid IN( " + string.Join(",", Fla.ToArray()) + ")";

            MySqlCommand cmdSQL = new MySqlCommand(txSQL,DB.conexBD );            
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
            if(MessageBox.Show("Está seguro de ELIMINAR  las particiones" +
                "\nde la RUTA: " + inN.Ruta.ToString() + "????", "Eliminar particiones", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;

            //Confirmado, le pone una "A" en partición de todas las conexiones de la ruta.
            Tabla = new DataTable();

            //Seleccionar las conexiones que integrarán la nueva partición
            txSQL = "SELECT C.conexionid, I.particion, C.secuencia, C.ruta " +
                " FROM  conexiones C JOIN infoconex I ON C.conexionid=I.conexionid" +
                " WHERE I.codigoimpresion = 0" +
                " AND C.ruta = " + inN.Ruta.ToString().Trim();

            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            foreach(DataRow Fila in Tabla.Rows)
                cnxs.Add(Fila.Field<int>("conexionid"));

            txSQL = "UPDATE infoconex " +
                    "SET particion = 'A'" +
                    " WHERE conexionid IN( " + string.Join(",", cnxs.ToArray()) + ")";

            MySqlCommand cmdSQL = new MySqlCommand(txSQL, DB.conexBD);
            MySqlDataAdapter dtAdap = new MySqlDataAdapter(txSQL, DB.conexBD);
            dtAdap.UpdateCommand = cmdSQL;
            dtAdap.AcceptChangesDuringFill = true;
            cmdSQL.ExecuteNonQuery();

            CargarListaRutas();

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


                //toma unicamente los valores de secuencia que esten habilitados como "todos""
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
                        //Nodo.ImageKey = tn.ImageKey;
                        //Nodo.SelectedImageKey = tn.ImageKey;
                        //AplicarEstadoAHijos(Nodo);
                        //TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
                        //button2_Click(sender, e);
                    }                   
                else
                      
                    tn.ImageKey = "todo";
                    Nodo.ImageKey = tn.ImageKey;
                    Nodo.SelectedImageKey = tn.ImageKey;
                    AplicarEstadoAHijos(Nodo);
                    TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
                    button2_Click(sender, e);

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
            BloqueoClick = true;
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
            if(Nodo != null)
                if(e.Button == MouseButtons.Right && Nodo.Level == 4)
                    DialogoParticion(Nodo);
                                                           
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
            catch(Exception e) {
                MessageBox.Show(e.Message, "Leyendo estructura de Tablas desde Archivos");
                return false;
            }
            
            return true;
        }

        /// <summary>
        ///Metodo que genera la carpeta de Secuencias Seleccionadas procesadas de acuerdo a parametros de creacion como Periodo,
        ///Distrito, Carga Nº, Fecha de generación del procesamiento.
        /// </summary>
        private void GenerarCarpetaArchivo() {
            string ArchivoTabla;
            string archivosecuencia;
            string Carp;
            int Carga = 0;
            StringBuilder stb = new StringBuilder();
 
            //Lee y obtiene el nombre de la base Sqlite                        
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string archivo = stb1.ToString();

            //Leer y actualizar el número de carga
            Inis.GetPrivateProfileString("Numeros Cargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
            Carga = int.Parse(stb.ToString()) + 1;
            Inis.WritePrivateProfileString("Numeros Cargas", Vble.Distrito.ToString(), 
                Carga.ToString().Trim(), Ctte.ArchivoIniName);

            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM", 
                CultureInfo.CurrentCulture);
            Carp = string.Format("EP{0:yyyyMM}_D{1:000}_C{2:00000}.{3:yyMMdd_HHmm}", Per, 
                Vble.Distrito, Carga, DateTime.Now);
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

            string destino = ArchivoTabla + "\\" + archivo;
            //copia el archivo de base de datos Sqlite al directorio generado anteriormente.
            CopiaArchivos(ObtenerOrigen(), destino);


            //A PARTIR DE ACA GENERO ARCHIVO CON DATOS DE LA CARGA QUE SE PROCESO 
            //COMO SER RUTA, SECUENCIA, CANTIDAD DE REGISTROS CARGADOS, ETC
                                   
            string filename = "InfoCarga.txt";
            archivosecuencia = System.IO.Path.Combine(ArchivoTabla, filename);    
            //Llamo al metodo que crea el archivo InfoCarga.txt que contiene informacion de la carga procesada
            CrearArchivoInfoCarga(archivosecuencia, filename, "");

        }

        /// <summary>
        /// Funcion que crea el archivo vacio, con la secuencia como nombre para utilizar como como información a la hora de mostrar en el listview1
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        private void CrearArchivoInfoCarga(string archivosecuencia, string filename, string colectora)
        {
            try
            {
                int CantRutas = ArrayRuta.Count;
                //Vble.desde = Convert.ToInt32(this.dataGridView1.Rows[0].Cells["Secuencia"].Value);
                //Vble.hasta = Convert.ToInt32(this.dataGridView1.Rows[this.dataGridView1.RowCount - 1].Cells["Secuencia"].Value);
                Vble.desde = Convert.ToInt32(Vble.TablaConexSelec.Rows[0][0]);
                Vble.hasta = Convert.ToInt32(Vble.TablaConexSelec.Rows[Vble.TablaConexSelec.Rows.Count-1][0]);

                Vble.lineas = "";

                //crea las lineas con la informacion de la carga que se va a procesar
                for (int i = 0; i < ArrayDesde.Count; i++)
                {
                    Vble.lineas += ArrayLocalidad[i] + "-" + ArrayRuta[i] + " (" + ArrayDesde[i] + "-" + ArrayHasta[i] + ") " + ArrayCantConex[i] + colectora + "\n";
                }

                CreateInfoCarga(archivosecuencia, filename, Vble.lineas);
         
            }
            catch (Exception)
            {

                throw;
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

                throw;
            }
        }

        /// <summary> Recorre el tree view y obtiene los valores para las variables de sistema
        /// según los nodos seleccionados
        /// </summary>
        private void ObtenerValoresDeVariablesSistema() {
            //string Valor = "";
            string clave = "";

            //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
            foreach(TreeNode tNd1 in tvwCargas.Nodes[0].Nodes ) {
                if(tNd1.ImageKey!= "nada") {
                    clave = tNd1.Tag.ToString();
                    Vble.Distrito = dcNodos[clave].Distrito;
                    //Obtener Remesa, se busca en nivel 2, debajo del nodo clave.
                    foreach(TreeNode tNd2 in tNd1.Nodes) {
                        if(tNd2.ImageKey != "nada") {
                            clave = tNd2.Tag.ToString();
                            Vble.Remesa = dcNodos[clave].Remesa;
                            //Obtener Ruta, se busca en nivel 3, debajo del nodo clave.
                            foreach(TreeNode tNd3 in tNd2.Nodes) {
                                if(tNd3.ImageKey != "nada") {
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
                    button2_Click(sender, e);//invoca al boton 2 que contiene metodo de recorrer los nodos del treview


                    if (Nodo.Level == 4 )        
                     {
                        RestNod.Visible = false;                        
                        Vble.CantNodosDesde = ArrayDesde.Count;//variable que utilizo para saber cuantas Secuencias va a contener mi consulta                         
                        //versecuencia(Nodo);

                        // }
                        if (Vble.CantNodosDesde > 0)
                        {
                            Vble.TablaConexSelec = CargarRegistrosSecuenciaP();
                            //labelCantReg.Text = Vble.TablaConexSelec.Rows.Count.ToString();
                            dataGridView1.DataSource = CargarRegistrosSecuenciaP();
                            labelCantReg.Text = dataGridView1.RowCount.ToString();

                        }    
                        

                    }
                    else if(Nodo.Level == 3)
                    {

                        RestNod.Text = " *Por favor seleccione solo los Nodos \n que contiene las secuencias";
                        RestNod.Visible = true;
                        tn.ImageKey = "nada";
                        Nodo.ImageKey = tn.ImageKey;
                        Nodo.SelectedImageKey = tn.ImageKey;
                        AplicarEstadoAHijos(Nodo);
                        TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
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
        private string iteracion()
        {
            string where = "";
            try
            {                                 
                for (int i = 0; i < ArrayDesde.Count; i++)
                    {
                        where += "OR ((C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                        " and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 0 + ") "+
                        "OR (C.Secuencia >= " + ArrayDesde[i] + " and C.Secuencia <= " + ArrayHasta[i] + " and C.Ruta = " + ArrayRuta[i] +
                        " and L.CodigoInt = " + ArrayLocalidad[i] + " and C.ImpresionOBS = " + 500 + "))";                
                    }
                
            }
            catch (Exception)
            {
                
            }
            return where;
            
        }

        /// <summary>
        /// //Metodo que contiene la consulta SELECT para obtener los registros que esten 
        /// //dentro de la secuencia seleccionada del treeview con un solo valor de secuencia DESDE y HASTA
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosSecuenciaP()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;

            try
            {
                txSQL = "select C.Secuencia, C.ConexionID, C.Lote, L.CodigoInt as Cod_Localidad, C.Ruta, C.usuarioID, C.titularID, " +
                        "C.propietarioID, C.ImpresionOBS " +
                        "From conexiones C " +
                        "INNER JOIN personas P ON C.usuarioID = P.personaID " +//and C.titularID = P.personaID "  and C.propietarioID = P.personaID " +
                        "INNER JOIN localidades L ON L.CodigoPostal = C.CodPostalSumin " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where ((C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                        " and L.CodigoInt = " + ArrayLocalidad[0] + " and C.ImpresionOBS = " + 0 + ") OR " +
                        "(C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                        " and L.CodigoInt = " + ArrayLocalidad[0] + " and C.ImpresionOBS = " + 500 + ")" +
                        ") " + iteracion() + "ORDER BY C.Secuencia ASC";  

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }

            return Tabla;

        }

        /// <summary>
        /// //Metodo que contiene la consulta SELECT para obtener los registros que esten 
        /// //dentro de la secuencia seleccionada del treeview con un solo valor de secuencia DESDE y HASTA
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosSecuencia()
        {                   
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;                     
            string txSQL;           
            try
            {
                txSQL = "select C.Secuencia, C.ConexionID, C.Lote, L.CodigoInt as Cod_Localidad, C.Ruta, C.usuarioID, C.titularID, " +
                        "C.propietarioID, C.ImpresionOBS " +
                        "From conexiones C " +
                        "INNER JOIN personas P ON C.usuarioID = P.personaID " +//and C.titularID = P.personaID "  and C.propietarioID = P.personaID " +
                        "INNER JOIN localidades L ON L.CodigoPostal = C.CodPostalSumin " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] + 
                        " and L.CodigoInt = " + ArrayLocalidad[0] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 + 
                        ") ORDER BY C.Secuencia ASC";

                        Tabla = new DataTable();
                        da = new MySqlDataAdapter(txSQL, DB.conexBD);
                        comandoSQL = new MySqlCommandBuilder(da);
                        da.Fill(Tabla);
              
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
             
            }

            return Tabla;

        }
        
        /// <summary>
        /// //Metodo que contiene la consulta SELECT para obtener los registros que esten 
        /// //dentro de la secuencia seleccionada del treeview con DOS  valores de secuencia DESDE y HASTA
        /// almacenados en el ArrayDesde y ArrayHasta al igual que la Ruta que le corresponde 
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosSecuencia2()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
             txSQL = "select C.Secuencia, C.ConexionID, C.Lote, L.CodigoInt as Cod_Localidad, C.Ruta, C.usuarioID, C.titularID, " +
                     "C.propietarioID, C.ImpresionOBS " +
                     "From conexiones C " +
                     "INNER JOIN Personas P ON C.usuarioID = P.personaID " +//and C.titularID = P.personaID and C.propietarioID = P.personaID " +
                     "INNER JOIN localidades L ON C.CodPostalSumin = L.CodigoPostal " +
                     //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                     "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                     " and L.CodigoInt = " + ArrayLocalidad[0] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +                         
                     ") OR (C.Secuencia >= " + ArrayDesde[1] + " and C.Secuencia <= " + ArrayHasta[1] + " and C.Ruta = " + ArrayRuta[1] + 
                     " and L.CodigoInt = " + ArrayLocalidad[1] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                     ") ORDER BY C.Secuencia ASC";               


                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
               
            }

            return Tabla;

        }
        /// <summary>
        /// //Metodo que contiene la consulta SELECT para obtener los registros que esten 
        /// //dentro de la secuencia seleccionada del treeview con TRES  valores de secuencia DESDE y HASTA
        /// almacenados en el ArrayDesde y ArrayHasta al igual que la Ruta que le corresponde almacenado en el ArrayRuta 
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosSecuencia3()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "SELECT C.Secuencia, C.ConexionID, C.Lote, L.CodigoInt as Cod_Localidad, C.Ruta, C.usuarioID, C.titularID, " +
                        "C.propietarioID, C.ImpresionOBS " +
                        "From conexiones C " +
                        "INNER JOIN Personas P ON C.usuarioID = P.personaID " +// and C.titularID = P.personaID and C.propietarioID = P.personaID " +
                        "INNER JOIN localidades L ON C.CodPostalSumin = L.CodigoPostal " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                        " and L.CodigoInt = " + ArrayLocalidad[0] + " and C.ImpresionOBS = " + 0 + " OR C.ImpresionOBS = " + 500 +
                        ") OR (C.Secuencia >= " + ArrayDesde[1] + " and C.Secuencia <= " + ArrayHasta[1] + " and C.Ruta = " + ArrayRuta[1] +
                        " and L.CodigoInt = " + ArrayLocalidad[1] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                        ") OR (C.Secuencia >= " + ArrayDesde[2] + " and C.Secuencia <= " + ArrayHasta[2] + " and C.Ruta = " + ArrayRuta[2] + 
                        " and L.CodigoInt = " + ArrayLocalidad[2] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                     ") ORDER BY C.Secuencia ASC";               

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }

            return Tabla;

        }


        /// <summary>
        /// //Metodo que contiene la consulta SELECT para obtener los registros que esten 
        /// //dentro de la secuencia seleccionada del treeview con TRES  valores de secuencia DESDE y HASTA
        /// almacenados en el ArrayDesde y ArrayHasta al igual que la Ruta que le corresponde almacenado en el ArrayRuta 
        /// </summary>
        /// <returns></returns>
        private DataTable CargarRegistrosSecuencia4()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "SELECT C.Secuencia, C.ConexionID, C.Lote, L.CodigoInt as Cod_Localidad, C.Ruta, C.usuarioID, C.titularID, " +
                        "C.propietarioID, C.ImpresionOBS " +
                        "From conexiones C " +
                        "INNER JOIN Personas P ON C.usuarioID = P.personaID " +//and C.titularID = P.personaID and C.propietarioID = P.personaID " +
                        "INNER JOIN localidades L ON C.CodPostalSumin = L.CodigoPostal " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (C.Secuencia >= " + ArrayDesde[0] + " and C.Secuencia <= " + ArrayHasta[0] + " and C.Ruta = " + ArrayRuta[0] +
                        " and L.CodigoInt = " + ArrayLocalidad[0] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                        ") OR (C.Secuencia >= " + ArrayDesde[1] + " and C.Secuencia <= " + ArrayHasta[1] + " and C.Ruta = " + ArrayRuta[1] +
                        " and L.CodigoInt = " + ArrayLocalidad[1] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                        ") OR (C.Secuencia >= " + ArrayDesde[2] + " and C.Secuencia <= " + ArrayHasta[2] + " and C.Ruta = " + ArrayRuta[2] +
                        " and L.CodigoInt = " + ArrayLocalidad[2] + " and C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                        ") OR (C.Secuencia >= " + ArrayDesde[3] + " AND C.Secuencia <= " + ArrayHasta[3] + " AND C.Ruta = " + ArrayRuta[3] + 
                        " AND L.CodigoInt = " + ArrayLocalidad[3] + " AND C.ImpresionOBS = " + 0 + " OR  C.ImpresionOBS = " + 500 +
                        ") ORDER BY C.Secuencia ASC";

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

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
        /// //metodo que carga la tabla conexiones de la base SQLite
        /// </summary>
        /// <param name="numeroconexion"></param>
        /// <returns></returns>
        private DataTable CargarRegistrosConexion(int codconex)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From conexiones Where ConexionID = " + codconex;

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

               
                //asignación a variables locales para manejar en el INSERT
                foreach (DataRow fi in Tabla.Rows)
                {
                    //declaracióno de variables
                    int conexionid, periodo, usuarioid, titularid, propietarioid, impresionCod, impresionOBS, impresionCant,
                        Operario, lote, zona, ruta, secuencia, remesa, ConsumoPromedio, ConsumoResidual, ConsumoFacturado, ConsumoTipo, OrdenTomado,
                        PuntoVenta, FacturaNro1, DocumPago1, FacturaNro2, DocumPago2, HistoPeriodo01, HistoConsumo01, HistoPeriodo02, HistoConsumo02,
                        HistoPeriodo03, HistoConsumo03, HistoPeriodo04, HistoConsumo04, HistoPeriodo05, HistoConsumo05, HistoPeriodo06,
                        HistoConsumo06, HistoPeriodo07, HistoConsumo07, HistoPeriodo08, HistoConsumo08, HistoPeriodo09, HistoConsumo09, HistoPeriodo10,
                        HistoConsumo10, HistoPeriodo11, HistoConsumo11, HistoPeriodo12, HistoConsumo12;
                    string DomicSumin, BarrioSumin, CodPostalSumin, CuentaDebito, Categoria, SubCategoria, CESPnumero, CESPvencimiento,
                        FacturaLetra, Vencimiento1, Vencimiento2, VencimientoProx;
                    double Importe1, Importe2;
                    try
                    {

                     //Asignacion de variables
                     conexionid = (int)fi[0]; periodo = (int)fi[1]; usuarioid = (int)fi[2]; titularid = (int)fi[3]; propietarioid = (int)fi[4];
                     DomicSumin = fi[5].ToString(); BarrioSumin = fi[6].ToString();  CodPostalSumin = fi[7].ToString(); CuentaDebito = fi[8].ToString();
                     impresionCod = (int)fi[9]; impresionOBS = (int)fi[10]; impresionCant = (int)fi[11]; Operario = (int)fi[12]; lote = (int)fi[13];
                     zona = (int)fi[14]; ruta = (int)fi[15]; secuencia = (int)fi[16]; remesa = (int)fi[17]; Categoria = fi[18].ToString();
                     SubCategoria = fi[19].ToString(); ConsumoPromedio = (int)fi[20]; ConsumoResidual = (int)fi[21]; ConsumoFacturado = (int)fi[22];
                     ConsumoTipo = (int)fi[23]; OrdenTomado = (int)fi[24]; CESPnumero = fi[25].ToString(); CESPvencimiento = fi[26].ToString();
                     FacturaLetra = fi[27].ToString(); PuntoVenta = (int)fi[28]; FacturaNro1 = (int)fi[29]; DocumPago1 = (int)fi[30];
                     Vencimiento1 = fi[31].ToString(); Importe1 = (double)fi[32]; FacturaNro2 = (int)fi[33]; DocumPago2 = (int)fi[34];
                     Vencimiento2 = fi[35].ToString(); Importe2 = (double)fi[36]; VencimientoProx = fi[37].ToString(); HistoPeriodo01 = (int)fi[38];
                     HistoConsumo01 = (int)fi[39]; HistoPeriodo02 = (int)fi[40]; HistoConsumo02 = (int)fi[41]; HistoPeriodo03 = (int)fi[42]; HistoConsumo03 = (int)fi[43];
                     HistoPeriodo04 = (int)fi[44]; HistoConsumo04 = (int)fi[45]; HistoPeriodo05 = (int)fi[46]; HistoConsumo05 = (int)fi[47]; HistoPeriodo06 = (int)fi[48];
                     HistoConsumo06 = (int)fi[49]; HistoPeriodo07 = (int)fi[50]; HistoConsumo07 = (int)fi[51]; HistoPeriodo08 = (int)fi[52]; HistoConsumo08 = (int)fi[53];
                     HistoPeriodo09 = (int)fi[54]; HistoConsumo09 = (int)fi[55]; HistoPeriodo10 = (int)fi[56]; HistoConsumo10 = (int)fi[57]; HistoPeriodo11 = (int)fi[58];
                     HistoConsumo11 = (int)fi[59]; HistoPeriodo12 = (int)fi[60]; HistoConsumo12 = (int)fi[61];

                    string insert;//Declaración de insert que contendra la consulta INSERT  
                    insert = "INSERT INTO conexiones ([conexionID], [Periodo], [usuarioID], [titularID], [propietarioID], [DomicSumin], [BarrioSumin]," +
                        " [CodPostalSumin], [CuentaDebito], [ImpresionCOD], [ImpresionOBS], [ImpresionCANT], [Operario]," +
                        " [Lote], [Zona], [Ruta], [Secuencia], [Remesa], [Categoria], [SubCategoria], [ConsumoPromedio]," +
                        " [ConsumoResidual], [ConsumoFacturado], [ConsumoTipo], [OrdenTomado], [CESPnumero], [CESPvencimiento]," +
                        " [FacturaLetra], [PuntoVenta], [FacturaNro1], [DocumPago1], [Vencimiento1], [Importe1], [FacturaNro2], [DocumPago2]," +
                        " [Vencimiento2], [Importe2], [VencimientoProx], [HistoPeriodo01], [HistoConsumo01], [HistoPeriodo02], [HistoConsumo02]," +
                        " [HistoPeriodo03], [HistoConsumo03], [HistoPeriodo04], [HistoConsumo04], [HistoPeriodo05], [HistoConsumo05], [HistoPeriodo06]," +
                        " [HistoConsumo06], [HistoPeriodo07], [HistoConsumo07], [HistoPeriodo08], [HistoConsumo08], [HistoPeriodo09], [HistoConsumo09]," +
                        " [HistoPeriodo10], [HistoConsumo10], [HistoPeriodo11], [HistoConsumo11], [HistoPeriodo12], [HistoConsumo12]) " +
                        "VALUES ('" + conexionid + "', '" + periodo + "', '" + usuarioid + "', '" + titularid + "', '" + propietarioid + "', '" + DomicSumin + "', '" + BarrioSumin + "', '" + CodPostalSumin + "', '" + CuentaDebito + "', '" + impresionCod + "', '" + impresionOBS + "', '" + impresionCant +
                        "', '" + Operario + "', '" + lote + "', '" + zona + "', '" + ruta + "', '" + secuencia + "', '" + remesa + "', '" + Categoria +
                        "', '" + SubCategoria + "', '" + ConsumoPromedio + "', '" + ConsumoResidual + "', '" + ConsumoFacturado + "', '" + ConsumoTipo +
                        "', '" + OrdenTomado + "', '" + CESPnumero + "', '" + CESPvencimiento + "', '" + FacturaLetra + "', '" + PuntoVenta + "', '" + FacturaNro1 + "', '" + DocumPago1 + "', '" + Vencimiento1 +
                        "', '" + Importe1 + "', '" + FacturaNro2 + "', '" + DocumPago2 + "', '" + Vencimiento2 + "', '" + Importe2 + "', '" + VencimientoProx + "', '" + HistoPeriodo01 +
                        "', '" + HistoConsumo01 + "', '" + HistoPeriodo02 + "', '" + HistoConsumo02 + "', '" + HistoPeriodo03 + "', '" + HistoConsumo03 + "', '" + HistoPeriodo04 + "', '" + HistoConsumo04 +
                        "', '" + HistoPeriodo05 + "', '" + HistoConsumo05 + "', '" + HistoPeriodo06 + "', '" + HistoConsumo06 + "', '" + HistoPeriodo07 + "', '" + HistoConsumo07 +
                        "', '" + HistoPeriodo08 + "', '" + HistoConsumo08 + "', '" + HistoPeriodo09 + "', '" + HistoConsumo09 + "', '" + HistoPeriodo10 + "', '" + HistoConsumo10 +
                        "', '" + HistoPeriodo11 + "', '" + HistoConsumo11 + "', '" + HistoPeriodo12 + "', '" + HistoConsumo12 + "')";

                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    }
                    catch (Exception)
                    {

                    }

                }

            }
            catch (Exception)
            {
            }
            return Tabla;
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
                else               
                    return true;
        }

            catch (Exception )
            {
                //MessageBox.Show(e.Message);
            }
            return false;
        }

        /// <summary>
        ///  muestra en el datagridMed los registros que se seleccionar del treview y se cargaron
        /// </summary>
        /// <returns></returns>
        public static bool ExisteConceptoDatos(int conexionID)
        {
            string txSQL;
            MySqlCommand da;            
            try
            {
                txSQL = "SELECT * FROM conceptosdatos WHERE ConexionID = " + conexionID;

                da = new MySqlCommand(txSQL, DB.conexBD);
                da.Parameters.AddWithValue("ConexionID", conexionID);
                //DB.conexBD.Open();

               int count = Convert.ToInt32(da.ExecuteScalar());               
                if (count == 0)
                    return false; 
                else
                     return true ;
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
                txSQL = "SELECT * FROM textosvarios WHERE ConexionID = " + conexionID;

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
                txSQL = "SELECT * FROM excepciones WHERE ConexionID = " + conexionID;

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
        private DataTable CargarRegistrosMedidores(int codconex)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From medidores Where ConexionID = " + codconex;
                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                foreach (DataRow fi in Tabla.Rows)
                {
                                    
                //declaracióno de variables
                int conexionid, periodo, Orden, Multiplicador, Digitos, AnteriorEstado, ActualEstado, TipoLectura;
                string Modelo, Numero, AnteriorFecha, ActualFecha, ActualHora;

                //asignación a variables locales para manejar en el INSERT
                    conexionid = (int)fi[0];
                    periodo = (int)fi[1];
                    Orden = (int)fi[2];
                    Modelo = fi[3].ToString();
                    Numero = fi[4].ToString();
                    Multiplicador = (int)fi[5];
                    Digitos = (int)fi[6];
                    AnteriorFecha = fi[7].ToString();
                    AnteriorEstado = (int)fi[8];
                    ActualFecha = fi[9].ToString();
                    ActualHora = fi[10].ToString();
                    ActualEstado = (int)fi[11];
                    TipoLectura = (int)fi[12];

                    string insert;//Declaración de string que contendra la consulta INSERT
                    insert = "INSERT INTO medidores ([conexionID], [Periodo], [Orden], [Modelo], [Numero], [Multiplicador], [Digitos]," +
                        " [AnteriorFecha], [AnteriorEstado], [ActualFecha], [ActualHora], [ActualEstado], [TipoLectura]) " +
                        "VALUES ('" + conexionid + "', '" + periodo + "', '" + Orden + "', '" + Modelo + "', '" + Numero + "', '" + Multiplicador + "', '" + Digitos +
                        "', '" + AnteriorFecha + "', '" + AnteriorEstado + "', '" + ActualFecha + "', '" + ActualHora + "', '" + ActualEstado + "', '" + TipoLectura + "')";

                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }
            }
            catch (Exception)
            {
            }
            return Tabla;
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
                txSQL = "select * From conceptosdatos Where ConexionID = " + codconex;
                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                
                
                foreach (DataRow fi in Tabla.Rows)
                {
                    int conexionid, periodo, CodigoConcepto, CodigoEscalon, CodigoAux, CodigoGrupo, CalcularBase,  CuotaUno, AplicarBase;
                    double Unitario, Cantidad, CalcularDesde, CalcularHasta, AplicarDesde, AplicarHasta, CantMinima, CantMaxima;
                    string TextoEscalon, TextoUnidades,  Subtotales, ImprimeSiCero, ImprimeSubtotal;
                   
                    //MessageBox.Show(fi["conexionID"].ToString() + " " + fi["CodigoConcepto"].ToString());
                    conexionid = (int)fi[0];//columna que contiene la conexionID
                    periodo = (int)fi[4];// columna que contiene el Periodo
                    CodigoConcepto = (int)fi[1];//columna que contiene CodigoConcepto
                    CodigoEscalon = (int)fi[2];//columna que contiene CodigoEscalon
                    CodigoAux = (int)fi[3];// columna que contiene CodigoAux                    
                    CodigoGrupo = (int)fi[5];// columna que contiene CodigoGrupo
                    TextoEscalon = fi[6].ToString();//columna que contiene TextoEscalon
                    TextoUnidades = fi[7].ToString();//columna que contiene TextoUnidades                    
                    CalcularBase = (int)fi[8];                   
                    CalcularDesde = (double)(fi[9]);                   
                    CalcularHasta = (double)(fi[10]);                  
                    AplicarBase = (int)fi[11];
                    AplicarDesde = (double)fi[12];
                    AplicarHasta = (double)fi[13];
                    Subtotales = fi[14].ToString();                    
                    CantMinima = (double)fi[15];                    
                    CantMaxima = (double)fi[16];                   
                    ImprimeSiCero = fi[17].ToString();
                    ImprimeSubtotal = fi[18].ToString();                    
                    CuotaUno = (int)fi[19];
                    Cantidad = (double)fi[20];
                    Unitario = (double)fi[21];                    
                    //llamo al procedimiento que contiene el insert de sqlite de los datos conceptosdatos que se pasan por parametros obtenidos del datatable(Tabla)
                    cargartablaConceptosDatos(conexionid, periodo, CodigoConcepto, CodigoEscalon, CodigoAux, CodigoGrupo, TextoEscalon, TextoUnidades, 
                                              CalcularBase, CalcularDesde, CalcularHasta, AplicarBase, AplicarDesde, AplicarHasta, Subtotales, 
                                              CantMinima, CantMaxima, ImprimeSiCero, ImprimeSubtotal, CuotaUno, Cantidad, Unitario);
                }

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
                txSQL = "select * From textosvarios Where ConexionID = " + codconex;

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
                int ValidaLecturaBaja, ValidaLecturaAlta, ValidaImpresionBaja, ValidaImpresionAlta, ValidaImpresionConfirma, nroLote, nroSecuencia,
                        puntoVenta, FacturaA, FacturaB, FacturaX, PeriodoFacturacion, codigoLecturista, localidadEmision, DiasPeriodo;
                    string FechaCarga, HoraCarga, resolucionTarifa, flgSubTotGravados, codigoEquipo;

                    //asignación a variables locales para manejar en el INSERT
                    ValidaLecturaBaja = 40;
                    ValidaLecturaAlta = 40;
                    ValidaImpresionBaja = 80;
                    ValidaImpresionAlta = 80;
                    ValidaImpresionConfirma = 120;
                    nroLote = lote;
                    nroSecuencia = 0;
                    puntoVenta = 1;
                    FacturaA = 0;
                    FacturaB = 0;
                    FacturaX = 0;                    
                    FechaCarga = DateTime.Now.ToString("dd/MM/yyyy");
                    HoraCarga = DateTime.Now.ToString("hh:mm:ss");                                     
                    PeriodoFacturacion = 0;
                    resolucionTarifa = "";
                    codigoLecturista = 0;
                    flgSubTotGravados = "";
                    codigoEquipo = "";
                    localidadEmision = 0;
                    DiasPeriodo = 61;                

                    string insert;//Declaración de string que contendra la consulta INSERT
                    insert = "INSERT INTO Varios ([ValidaLecturaBaja], [ValidaLecturaAlta], [ValidaImpresionBaja], [ValidaImpresionAlta]," +
                        " [ValidaImpresionConfirma], [nroLote], [nroSecuencia], [puntoVenta], [FacturaA], [FacturaB], [FacturaX], [FechaCarga]," +
                        " [HoraCarga], [PeriodoFacturacion], [resolucionTarifa], [codigoLecturista], [flgSubTotGravados], [codigoEquipo]," +
                        " [localidadEmision], [DiasPeriodo]) " +
                        "VALUES ('" + ValidaLecturaBaja + "', '" + ValidaLecturaAlta + "', '" + ValidaImpresionBaja + "', '" + ValidaImpresionAlta + 
                        "', '" + ValidaImpresionConfirma + "', '" + nroLote + "', '" + nroSecuencia + "', '" + puntoVenta + "', '" + FacturaA + "', '" + FacturaB + 
                        "', '" + FacturaX + "', '" + FechaCarga + "', '" + HoraCarga + "', '" + PeriodoFacturacion + "', '" + resolucionTarifa + 
                        "', '" + codigoLecturista + "', '" + flgSubTotGravados + "', '" + codigoEquipo + "', '" + localidadEmision + "', '" + DiasPeriodo + "')";

                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return Tabla;
        }

        /// <summary>
        /// //Metodo que carga usuarioID, titularID o PropietarioID  
        /// </summary>
        /// <param name="codconex"></param>
        /// <returns></returns>
        private DataTable CargaRegistrosPersonas(int codconex)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From personas Where PersonaID = " + codconex;
                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                //declaracióno de variables

                int personaID, periodo, CondIVA;
                string Apellido, Nombre, DocTipo, Domicilio, Barrio, CodigoPostal, DocNro;
                
                foreach (DataRow fi in Tabla.Rows)
                {              
                //asignación a variables locales para manejar en el INSERT
                personaID = (int)(fi[0]);
                periodo = (int)(fi[1]);
                Apellido = fi[2].ToString();
                Nombre = fi[3].ToString();
                DocTipo = fi[4].ToString();
                DocNro = fi[5].ToString();
                CondIVA = (int)(fi[6]);
                Domicilio = fi[7].ToString(); 
                Barrio = fi[8].ToString();
                CodigoPostal = fi[9].ToString();

                    string insert;//Declaración de string que contendra la consulta INSERT               
                    insert = "INSERT INTO Personas ([personaID], [Periodo], [Apellido], [Nombre], [DocTipo], [DocNro], [CondIVA]," +
                        " [Domicilio], [Barrio], [CodigoPostal]) " +
                        "VALUES ('" + personaID + "', '" + periodo + "', '" + Apellido + "', '" + Nombre + "', '" + DocTipo + "', '" + DocNro + "', '" + CondIVA +
                        "', '" + Domicilio + "', '" + Barrio + "', '" + CodigoPostal + "')";
                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(insert, DB.con);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }

            }
            catch (Exception)
            {
            }
            return Tabla;
        }

        /// <summary>
        /// //Funcion que contiene la consulta INSERT de SQLite para cargar los registros seleccionados a la tabla ConceptosDatos 
        /// </summary>
        ///  //public void cargartablaConceptosDatos()
        public void cargartablaConceptosDatos(int conexionid, int periodo, int CodigoConcepto, int CodigoEscalon, int CodigoAux, int CodigoGrupo, 
                                              string TextoEscalon, string TextoUnidades, int CalcularBase, double CalcularDesde, double CalcularHasta,
                                              int AplicarBase, double AplicarDesde, double AplicarHasta, string Subtotales, double CantMinima, 
                                              double CantMaxima, string ImprimeSiCero, string ImprimeSubtotal, int CuotaUno, double Cantidad,
                                              double Unitario)
       
        {
            string txSQL;//Declaración de string que contendra la consulta INSERT
            try
            {               

      txSQL = "INSERT INTO ConceptosDatos ([conexionID], [Periodo], [CodigoConcepto], [CodigoEscalon], [CodigoAux], [CodigoGrupo], [TextoEscalon]," +
              " [TextoUnidades], [CalcularBase], [CalcularDesde], [CalcularHasta], [AplicarBase], [AplicarDesde]," +
              " [AplicarHasta], [Subtotales], [CantMinima], [CantMaxima], [ImprimeSiCero], [ImprimeSubtotal], [CuotaUno], [Cantidad]," + " [Unitario]) " +
              "VALUES ('" + conexionid + "', '" + periodo + "', '" + CodigoConcepto + "', '" + CodigoEscalon + "', '" + CodigoAux + "', '" + CodigoGrupo + 
              "', '" + TextoEscalon + "', '" + TextoUnidades + "', '" + CalcularBase + "', '" + CalcularDesde + "', '" + CalcularHasta + "', '" + AplicarBase +
              "', '" + AplicarDesde + "', '" + AplicarHasta + "', '" + Subtotales + "', '" + CantMinima + "', '" + CantMaxima + "', '" + ImprimeSiCero + "', '" + ImprimeSubtotal +
              "', '" + CuotaUno + "', " + Cantidad.ToString(CultureInfo.CreateSpecificCulture("en-US")) + 
              ", " + Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ")";

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
                MessageBox.Show(e.Message);
            }
                 
        }


 

        //metodo que contiene string de tablas sqlite para limpiar antes de realizar carga de registros
        public void EliminarContenidosTablas()
        {
            ArrayTablasSQLite.Clear();
            ArrayTablasSQLite.Add("DELETE FROM Conexiones");
            ArrayTablasSQLite.Add("DELETE FROM Medidores");
            ArrayTablasSQLite.Add("DELETE FROM Personas");
            ArrayTablasSQLite.Add("DELETE FROM ConceptosDatos");
            ArrayTablasSQLite.Add("DELETE FROM TextosVarios");
            ArrayTablasSQLite.Add("DELETE FROM ConceptosFacturados");          
            ArrayTablasSQLite.Add("DELETE FROM NovedadesConex");
            ArrayTablasSQLite.Add("DELETE FROM Altas");
            ArrayTablasSQLite.Add("DELETE FROM Excepciones");
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
        /// Método que contiene las cargas de registros a las tablas Variables de SQLite
        /// </summary>
        /// <param name="codconex"></param>
        /// <param name="ArrayPersonas"></param>
        public void CargaTablasVariables(int codconex, ArrayList ArrayPersonas)
        {
            try
            {
            //Carga registros en tabla conexiones de SQLite
            CargarRegistrosConexion(codconex);
            //Carga registros en tabla Medidores de SQLite
            CargarRegistrosMedidores(codconex);   
            //Recorre el array de personas que contiene usuarioID, titularID, PropietarioID, en caso de que sean distintas la carga a tabla Personas de SQLite 
            foreach (Int32 i in ArrayPersonas)
              {
                  //Verifica si usuarioID, propietarioID, titularID no esta cargado y carga a Tabla personas de SQLite  
                  if (!ExistePersona(i))
                {                                          
                  CargaRegistrosPersonas(i);
                }
              }
            //verifica si existen registros en la tabla ConceptosDatos que pertenezca a alguna de las conexiones seleccionadas
            if (ExisteConceptoDatos(codconex))
            {               
                CargaRegistrosConceptosDatos(codconex);
            }
            
            //verifica si existen registros en la tabla TextosVarios que pertenezca a alguna de las conexiones seleccionadas
            if (ExisteTextoVario(codconex))
            {
                CargaTextosVariosSqlite(codconex);
            }
            //verifica si existen registros en la tabla Excepciones que pertenezca a alguna de las conexiones seleccionadas
            if (ExisteExepciones(codconex))
            {
                CargaExcepcionesSqlite(codconex);
            }

            }
            catch (Exception)
            {
                
            }
        }

        public void SECUENCIA()
        {

        }


        /// <summary>
        /// Metodo que contiene funciones de carga de tablas segun secuencia seleccionada y copia de archivo SQLite a la carpeta que contendra 
        /// registros de las cargas No enviadas
        /// </summary>
        public void EjecutarCarga()
        {
            DataGridView TablaConexSelec = new DataGridView();
            
            int codconex;
            int UsuarioID;
            int titularID;
            int propietarioID;            
            int i = 0;
            //if (this.dataGridView1.RowCount > 0)//verifica que en el datagridview1 existan registros para recorrerlos y generar la carga de las tablas
              if (Vble.TablaConexSelec.Rows.Count > 0)
                {
                //asigno a Vble.CantidadRegistros la cantidad de registros que corresponden a las secuencas seleccionadas
                //Vble.CantRegistros = dataGridView1.RowCount;
                Vble.CantRegistros = Vble.TablaConexSelec.Rows.Count;

                //recorre cada registro de los nodos seleccionados    
                //foreach (DataGridViewRow Fila in dataGridView1.Rows)
                foreach (DataRow Fila in Vble.TablaConexSelec.Rows)
                    //foreach (DataRow Fila in Tabla.Rows)
                    {
                    //codconex = Convert.ToInt32(Fila.Cells["ConexionID"].Value);//obtengo conexionID de cada registro para cargar a la tabla Conexiones SQLite
                    codconex = (int)Fila[1];
                    //UsuarioID = Convert.ToInt32(Fila.Cells["usuarioID"].Value);//  ""     suarioID  ""  ""     ""     ""    ""   "  "  ""   Personas     ""
                    UsuarioID = (int)Fila[5];
                    //titularID = Convert.ToInt32(Fila.Cells["titularID"].Value);// ""     titularID  ""  ""     ""     ""    ""   "  "  ""   Personas     ""
                    titularID = (int)Fila[6];
                    //propietarioID = Convert.ToInt32(Fila.Cells["propietarioID"].Value);//obtengo titularID de cada Registro para cargar a la tabla Personas de SQLite
                    propietarioID = (int)Fila[7];
                    //Vble.Lote = Convert.ToInt32(Fila.Cells["Lote"].Value);
                    Vble.Lote = (int)Fila[2];

                    ArrayPersonas.Add(UsuarioID);     //
                    ArrayPersonas.Add(titularID);     //Agrego Al Array personas "UsuarioID-TitularID-PropietarioID"
                    ArrayPersonas.Add(propietarioID); //

                    //----------------------Cambia el estado impresionOBS a Listo para Cargar en Colectora
                    Vble.CambiarEstadoConexionMySql(codconex, Convert.ToInt32(cteCodEstado.ParaCargar));

                    //----------------------Llamada al metodo CargaTablasVariables
                    CargaTablasVariables(codconex, ArrayPersonas);

                    

                    //----------------------Reporte de carga aumenta con cada registro cargado
                    backgroundWorker1.ReportProgress(i);
                    i++;
                }
                //////Actualiza tabla varios de acuerdo a la carga que se procesa
                CargarTablaVarios(Vble.Lote);
            }
            else
            {
                MessageBox.Show("Debe Seleccionar alguna secuencia del panel Izquierdo para ver los registros", "Atención", MessageBoxButtons.OK);
            }
        }

        public void LimpiarPanelCargasAenviar()
        {
            if (listViewCargasProcesadas.Items.Count > 0)
            {
                for (int i = 0; i < listViewCargasProcesadas.Items.Count; )
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
            

        }

        /// <summary>
        /// Metodo que Muestra en el ListView1 "Archivos para Cargar" 
        /// todas las cargas procesadas de las distintas localidades
        /// </summary>
        public void CargasProcesadas()
        {
            int indiceCarpetas;
            ArrayCarpetasCargas.Clear();
            listViewCargasProcesadas.ShowItemToolTips = true;

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
                                foreach (var files in sub.GetFiles())
                                  {                                
                                     if (files.Name.Contains("InfoCarga.txt"))
                                        {
                                            //Datos.SubItems.Add(files.Name);
                                            //Agrega el tooltip de información de las cargas generadas.
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
                                 listViewCargasProcesadas.Items.Add(Datos);                        
                                 ArrayCarpetasCargas.Add(sub.FullName);
                            }                   
                        }
                    }
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
                            if (fi.Name.Contains(Vble.CarpetaSeleccionada))
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
                                                Vble.lineas = "Fecha Envio: " + thisDay.ToString("d") + " - Colectora: " + cmbDevices.Text + "\n";
                                                while ((line = sr.ReadLine()) != null)
                                               {

                                                  Vble.lineas += line +"\n";
                                                    
                                               }
                                               Datos.ToolTipText = Vble.lineas;
                                             
                                            }
                                            //almaceno en variables temporales para crear los nuevos archivos con informacion de colectora
                                            string infocargarespaldo = Vble.CarpetaRespaldo + Vble.RespaldoEnviadas + "\\" + files.Name;
                                            string ruta = files.FullName;                                          
                                            string archivo = files.Name;                                          
                                            //elimino archivos para luevo volver a crearlos con info de colectora
                                            File.Delete(files.FullName);
                                            File.Delete(infocargarespaldo);
                                            //creo nuevos archivos InfoCarga.txt con informacion de colectora a la que se envio
                                            CreateInfoCarga(ruta, archivo, Vble.lineas);
                                            CreateInfoCarga(infocargarespaldo, archivo, Vble.lineas);
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
                //ArrayCarpetasCargas.Clear();
                ListViewCargados.ShowItemToolTips = true;

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
        /// //Boton que se encarga de la carga de los registros a las tablas vacias con formato SQLite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            RestNod.Visible = false;
            LabRestDevArc.Visible = false;
            LabRestEnvArc.Visible = false;
            try {
                //verifica que se haya seleccionado alguna secuencia que contenga conexiones a cargar
                //if (dataGridView1.RowCount > 0)
                if (Vble.TablaConexSelec.Rows.Count > 0)
                {
                    LimpiarPanelCargasAenviar();
                    listViewCargasProcesadas.Refresh();                          
                    ArrayPersonas.Clear();//limpia el array que contendra los usuarioID, titularID, propietarioID
                    EliminarContenidosTablas();//limpia las tablas variables sqlite antes de cargar los registros seleccionas                              

                    //El proceso de generar las conexiones seleccionadas se realiza en segundo plano por el tiempo excesivo 
                    //que toma el proceso de carga para no tildar la aplicación. Sigue en el metodo "backgroundWorker1_DoWork"
                    backgroundWorker1.RunWorkerAsync();             
                }
                else
                {
                    RestNod.Text = "* No existen archivos con la seleccion o no se selecciono una ruta correcta";
                    RestNod.Visible = true;                  
                }
              }
                catch (Exception R)
            {
                MessageBox.Show(R.Message);
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
                                        ////agregado recientemente
                                        //RecorrerNodosParticion(tNd3);                                     
                                        ///
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
                                        }
                                    else
                                    {

                                        dataGridView1.DataSource = "";
                                        Vble.TablaConexSelec.Clear();
                                        labelCantReg.Text = dataGridView1.RowCount.ToString();  
                                    }

                                    }
                            }
                          
                        }
                    }
                 
                }

            }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
              
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
            try
            {   //Leer Ruta Origen              
                StringBuilder stb1 = new StringBuilder("", 250);                
                Inis.GetPrivateProfileString("Datos", "Base", "", stb1, 250, Ctte.ArchivoIniName);
                //Inis.LeerCadenaPerfilPrivado("Datos", "RutaOrigen", "NO", out stb1, 97, Ctte.ArchivoIniName);
                string retorno = stb1.ToString();
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }     

        


        /// <summary>
        /// Metodo que obtendra el nombre del archivo SQLite que contiene los datos procesados para copiar a la carpeta 
        /// con el formato "EPyyyyMM_D000_C00000.aaMMdd_HHmm" generado anteriormente 
        /// </summary>
        public string ObtenerNombreArchivo()
        {
            try
            {   //Leer Ruta Origen              
                StringBuilder stb1 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);                
                string retorno = stb1.ToString();
                return retorno;
            }
            catch (Exception)
            {

                throw;
            }
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
            ListView.SelectedIndexCollection indexes = this.listViewCargasProcesadas.SelectedIndices;
            int indice;
            int indiceDistrito = 67;     

            List<string> VarVal = new List<string>();
            foreach (int index in indexes)
            {
                indice = listViewCargasProcesadas.Items[index].Index;
                Vble.CarpetaSeleccionada = listViewCargasProcesadas.Items[index].Text;
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
            return Vble.RutaCarpetaEnviadas;
        }



        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in ArrayDesde)
                {
                    MessageBox.Show(item.ToString());
                    MessageBox.Show(item.ToString());
                }
                
            }
                catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }


        private void timer2_Tick(object sender, EventArgs e)
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
                    shellView2.CurrentFolder = folder;
                }
                
            }
            catch (Exception)
            {
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Aca se ejecuta Procesar Carga en segundo plano 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {                          
                    simulateHeavyWork();              
                    EjecutarCarga();
                    //this.dataGridView1.DataSource = "";
                    GenerarCarpetaArchivo();//Crea Directorios Correspondientes y copia archivo SQLite para cargar a la colectora
                                            //backgroundWorker1.ReportProgress(i);
         
               

            }
            catch (Exception)
            {

           
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            progressBar1.Visible = true;
            PorcLabel.Visible = true;
            progressBar1.Value = (e.ProgressPercentage * 100) / Vble.CantRegistros;
            PorcLabel.Text = (e.ProgressPercentage * 100) / Vble.CantRegistros + " %";

        }
        /// <summary>
        /// Funcion que ejecuta proceso en segundo plano para que el sistema trabaje sin interrupción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            PorcLabel.Visible = false;
            //botNavAtras.Visible = true;
            MessageBox.Show("Se cargaron los " + Vble.CantRegistros +
                            " registros de la secuencia seleccionada");
            BotActPanPC_Click(sender, e);
            this.dataGridView1.DataSource = "";
            this.labelCantReg.Text = "0";
            Vble.TablaConexSelec.Clear();
            timer1.Start();

            //ShellItem path = new ShellItem(Vble.CarpetasGenerada);
            //shellView1.CurrentFolder = path;
            //shellView1.Visible = true;
            //fileFilterComboBox1.Visible = true;

        }

        private void simulateHeavyWork()
        {          
            Thread.Sleep(Vble.CantRegistros);
           
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
        private static void CopiarDirectorio(string sourceDirName, string destDirName, bool copySubDirs)
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
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
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
        /// Metodo que copia y envia archivos a carpetas Enviadas, Respaldo
        /// </summary>
        private void EnviarArchivos()
        {
            try
            {            
            string Dispositivo;
            string Ruta;

            Dispositivo = Funciones.BuscarNombreColectora(Vble.DirectorioColectoraenPC);
            Vble.Colectora = Dispositivo + cmbDevices.Text + "\\" +Vble.CarpetaDestinoColectora +"\\";
            Ruta = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;  

            LabRestEnvArc.Visible = false;
            //ruta del archivo .db creado al procesar la carga                                                   
            DirectoryInfo Origen = new DirectoryInfo(Vble.RutaCarpetaOrigen);
            string Origen1 = Vble.RutaCarpetaOrigen;

            //ruta del directorio donde se va a enviar el archivo seleccionado 
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string ArchivoBase = stb1.ToString();
            DirectoryInfo Destino1 = new DirectoryInfo(Ruta);
            
            string Destino = Ruta /*+ Vble.CarpetaSeleccionada*/;

            //verifica si existe la ruta "Envidas" dentro de cada distrito sino la crea y envia una copia allí
            if (!Directory.Exists(RutaEnviadas()))
                Vble.CrearDirectorioVacio(RutaEnviadas());
                CopiarDirectorio(Origen1, RutaEnviadas(), true);

            //verifica si existe la ruta "Respaldo" en Pruebas sino la crea y envia una copia allí
            if (!Directory.Exists(Vble.CarpetaRespaldo))
                    Vble.CrearDirectorioVacio(Vble.CarpetaRespaldo + Vble.RespaldoEnviadas);
                CopiarDirectorio(Origen1, Vble.CarpetaRespaldo + Vble.RespaldoEnviadas, true);
               
           
            //Por ultimo mueve el archivo seleccionado a la colectora y elimina el original que se muestra en el lisview
            //quedando dentro de carpetas enviadas y en respaldo.
            if (Directory.Exists(Destino))
                //Directory.Delete(Destino);
                foreach (var item in Origen.GetFiles())
                 {                                   
                    File.Copy(item.FullName, Destino1.FullName+item.Name);   
                 }
            //CopiarDirectorio(Origen1, Destino1, true);  
            //LeeCarpetaColectora(Destino1.FullName);
            AgregarColectoraAInfoCarga();            

            //cambia el estado de las conexiones enviadas de "Listo para enviar"(300) a "Enviados"(400) de la base MySql general. 
            Vble.CambiarEstadoEnviadasMySql(RutaEnviadas() ,Convert.ToInt32(cteCodEstado.Cargado));

            //Elimina el archivo SQLite generado dejando solo en la colectora para que no se produza redundancia de datos
            Directory.Delete(Origen1, true);
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }


        //Actualiza el listview de la colectora para corroborar la existencia 
        //o no de archivos en la carpeta compartida entre pc y colectora
        private void ActualizaListViewColectora()
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
            string Dispositivo;
            string Ruta;
            Dispositivo = Funciones.BuscarNombreColectora(Vble.DirectorioColectoraenPC);
            Vble.Colectora = Dispositivo + cmbDevices.Text + "\\" + Vble.CarpetaDestinoColectora + "\\";
            Ruta = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;
            DirectoryInfo Destino1 = new DirectoryInfo(Ruta);
            LeeCarpetaColectora(Destino1.FullName);
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
                      if ((shellView2.Visible == true))
                        {
                         if(ListViewColectora.Visible = true && ListViewColectora.Items.Count == 0)
                          { 
                            if (shellView2.Enabled == false)
                              {                            
                            ////Cambio el estado de las conexiones antes de ser enviadas a la colectora 
                            ////pasa de 300(Listo para Cargar) a 400(Cargados en Colectora)
                            Vble.CambiarEstadoConexionSqlite(Convert.ToInt32(cteCodEstado.NoCargado));
                            Vble.ModificarInfoConex(0, 0);   
                            //Metodo que envia a la colectora el directorio seleccionado del listivew
                            this.EnviarArchivos();
                            //Llama al boton que actualiza los listview de archivos sin enviar y enviados
                            BotActPanPC_Click(sender, e);
                            ActualizaListViewColectora();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se puede cargar mas de un archivo a la Colectora", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //ListViewColectora.Visible = true;                            
                            shellView2.Visible = true;
                            ListViewColectora.Visible = true;
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

                catch (Exception)
                    {
                MessageBox.Show("El archivo seleccionado ya se envió con anteriorida");   
                    }
            
            //button2_Click_2(sender, e);
        }




        public void button2_Click_2(object sender, EventArgs e)
        {
            cmbDevices.Items.Clear();           
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
            LabRestEnvArc.Visible = false;
            LabRestDevArc.Visible = false;
            //Lee y obtiene el nombre de la base Sqlite
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);            
            string ArchivoBase = stb1.ToString();
            //recorre los dispositivos conectados y va consultando si existe alguna colectora Con la denominación "MICC-0000"
            foreach (var item in shellView2.CurrentFolder)
            {                
                if (item.DisplayName == cmbDevices.Text)
                {
                    //shellView2.Visible = true;
                    //recorre las unidades que contiene la colectora y busca el directorio Raiz "\" para ingresar
                      foreach (var raiz in item)
                      {
                        if (raiz.DisplayName == "\\")
                        {                            
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
                                            string Dispositivo = Funciones.BuscarNombreColectora(Vble.DirectorioColectoraenPC);
                                            Vble.Colectora = Dispositivo + cmbDevices.Text + "\\" + Vble.CarpetaDestinoColectora + "\\";
                                            string Ruta = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;
                                            DirectoryInfo di = new DirectoryInfo(Ruta);
                                            if (File.Exists(Ruta+ArchivoBase))
                                            {                                                
                                                MessageBox.Show("La Colectora ya contiene la Carga: \n" + Funciones.LeerArchivostxt(Ruta + "InfoCarga.txt") +"Por favor primero realice la descarga para volver a enviar otro Archivo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                //shellView2.CurrentFolder = destino;
                                                shellView2.Visible = false;
                                                ListViewColectora.Visible = false;
                                            }
                                            else
                                            {
                                                shellView2.CurrentFolder = destino;                                                                                
                                                shellView2.Visible = true;
                                                ListViewColectora.Visible = true;
                                                ActualizaListViewColectora();
                                                //button2_Click_2(sender, e);
                                            }
                                        }
                                        else
                                        {
                                            //MessageBox.Show("Disculpe no se encuentra la carpeta destino", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            shellView2.Visible = false;
                                        }
                                    }                                   
                                }
                            }
                        }
                    }
                }        
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
        private void BotActPanPC_Click(object sender, EventArgs e)
        {           
            LimpiarPanelCargasAenviar();
            CargasProcesadas();
            LeeCargasEnviadas();
                       
            timer1_Tick(sender, e);
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
                //Vble.RutaCarpetaEnviadas = Vble.RutaCarpetaOrigen + Vble.ValorarUnNombreRuta(Vble.CarpetaCargasSiEnviadas);
            }        
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            LabRestEnvArc.Visible = false;
            LabRestDevArc.Visible = false;
        }

        private void BotDevCarga_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            StringBuilder stb = new StringBuilder();

            try
            {
                if (listViewCargasProcesadas.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show("¿Está seguro que desea devolver la ruta procesada al panel" +
                                        "\nde Rutas Disponibles?", "Devolver Rutas", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    { 
                        //cambia el estado de las conexiones enviadas de "Listo para enviar"(300) a "Enviados"(400) de la base MySql general. 
                        //MessageBox.Show(Vble.RutaCarpetaOrigen);
                        Vble.CambiarEstadoEnviadasMySql(Vble.RutaCarpetaOrigen, Convert.ToInt32(cteCodEstado.NoCargado));

                    ////Leer y actualizar el número de carga
                    //Inis.GetPrivateProfileString("Numeros Cargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
                    //Carga = int.Parse(stb.ToString()) - 1;
                    //Inis.WritePrivateProfileString("Numeros Cargas", Vble.Distrito.ToString(),
                    //Carga.ToString().Trim(), Ctte.ArchivoIniName);
                    ////Elimina la carpeta Generada y actualiza el listview de Rutas para Cargar ya que se devolvio las Cargas a Rutas Disponibles
                    Directory.Delete(Vble.RutaCarpetaOrigen, true);
                    BotActPanPC_Click(sender, e);
                    timer1_Tick(sender, e);
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
    }
    }
