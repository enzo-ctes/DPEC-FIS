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
using MySql.Data;
using System.Security.Permissions;
using System.Linq;
using System.Globalization;
using System.Text;
using System.IO;
using GongSolutions.Shell;
using System.Web;
using System.Web.Security;
//using System.Web.SessionState;
//using System.IO.Ports;
//using System.Windows.Devices.Portable;
using System.Collections.ObjectModel;



namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form4Cargas.
    /// </summary>
    public partial class Form4Cargas : Form {
        private int QueTimer;
        private Dictionary<string, clInfoNodos> dcNodos = new Dictionary<string, clInfoNodos>();
        private bool BloqueoClick = false;

        //declaracion de arrays que contendran informacion de las Secuencias
        //Rutas y Localidades que se seleccionan del treeview para realizar las consultas
        ArrayList ArrayDesde = new ArrayList();
        ArrayList ArrayHasta = new ArrayList();
        ArrayList ArrayRuta = new ArrayList();
        ArrayList ArrayLocalidad = new ArrayList();
        


      



        public Form4Cargas()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();


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

            


            toolTip1.SetToolTip(Loc, "Codigo de Localidad");
            toolTip2.SetToolTip(Ru, "Ruta de la Localidad");
            toolTip3.SetToolTip(textBox2, "Nº Secuencia DESDE");
            toolTip4.SetToolTip(textBox2, "Nº Secuencia HASTA");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
                        
            //LLama a cargar los nodos de rutas disponibles
            QueTimer = 1;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            //CargarCarpetas();
            //CargarColectoras();
            //Verdatoscolectora();
            

        }

       
        //************Metodo que devuelve la ruta o memoria flash de la colectora
        //************
        // private void Verdatoscolectora()
        // {
        //     lstDiscos.Items.Clear();
        //     DriveInfo[] d = DriveInfo.GetDrives();
        // System.IO.DriveInfo di = new System.IO.DriveInfo(@"C:\");
        //     if (System.IO.Directory.Exists(@"C:\Users\usuario\AppData\Local\Microsoft\Windows\INetCache\IE\CVHM6R7O\"))
        //     {
        //         try
        //         {
        //             lstDiscos.Items.Add("C:/Users/usuario/AppData/Local/Microsoft/Windows/INetCache/IE/CVHM6R7O/");
        //         }
        //         catch (Exception ex)
        //         {
        //             MessageBox.Show("Error leyendo los discos: " + ex.Message);
        //         }
        //         System.IO.Directory.SetCurrentDirectory(@"C:\Users\usuario\AppData\Local\Microsoft\Windows\INetCache\IE\CVHM6R7O\");
        //     }
        //     else
        //     {
        //         MessageBox.Show("La carpeta no existe");
        //     }           
        //}




        /// <summary>
        /// carga listview con Descripción de Colectoras ubicada en archivo de configuración ini
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void CargarColectoras()
        //{
        //    int i = 0;
        //    string sC, sK, sI;
        //    StringBuilder stB = new StringBuilder();
        //    //lsColectoras.Items.Clear();
        //    treColectoras.Nodes.Clear();

        //    do
        //    {
        //        i++;
        //        sC = "Carpeta" + i.ToString("00");
        //        Inis.GetPrivateProfileString
        //            ("Carpetas Colectoras", sC, "NO", stB, 250, Ctte.ArchivoIniName);
        //        sI = stB.ToString().Trim();
        //        sK = sI.ToUpper();
        //        if ((sI.Length > 0) && (sK != "NO"))
        //        {
        //            if (!treColectoras.Nodes.ContainsKey(sK))
        //                //if (!lsColectoras.Items.ContainsKey(sK))
        //                //lsColectoras.Items.Add(sK, sI, 0);                        
        //                treColectoras.Nodes.Add(sK, sI, 0);
        //        }
        //    } while (stB.ToString().Trim().ToUpper() != "NO");


        //}
  
      

     

       //Boton que cierra el formulario actual
        void btnCerrar_Click(object sender, EventArgs e) {
            this.Close();
            for (int f = 0; f < vectorSecDESDE.secuencianodos.Length; f++)
            {
                vectorSecDESDE.secuencianodos[f] = 0;
                vectorSecHASTA.secuencianodos[f] = 0;
            }
            
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
                        " WHERE conexiones.conexionid=infoconex.conexionid " +
                        " AND conexiones.periodo = " + Vble.Periodo +
                        " AND conexiones.impresioncod = " + 0 +
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
            



            //toma unicamente los valores de secuencia que esten habilitados como "todos""
            if (Nodo != null) { 
                if (e.Button == MouseButtons.Left && Nodo.Level == 4)
                {


                    if (tn.ImageKey == "todo")
                    {
                        vectorSecDESDE.secuencianodos[f - 1] = 0;
                        vectorSecHASTA.secuencianodos[f - 1] = 0;
                    }

                    else
                    {
                      
                        versecuencia(Nodo);
                    }
                }
                else 
                {                  
                    
                }
            }


            //Para evitar cambio de estado al expandir o contraer nodo
            if (BloqueoClick) {
                BloqueoClick = false;
                return;
            }

            
            //Debe cambiar el estado de selección del nodo
            if (e.Button == MouseButtons.Left) {
                //vector que contendra los numeros de secuencias para consulta en caso que se seleccione mas de un nodo
                

                //Si está todo pasa a nada, caso contrario pasa a todo
                if (tn.ImageKey == "todo")
                    tn.ImageKey = "nada";
                else
                    tn.ImageKey = "todo";
                Nodo.ImageKey = tn.ImageKey;
                Nodo.SelectedImageKey = tn.ImageKey;
                AplicarEstadoAHijos(Nodo);
                TomarEstadoDeHijos(tvwCargas.Nodes["dpec"]);
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

        /// <summary>
        /// Genera la carpeta y archivos que serán enviados a la colectora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesarCarga_Click(object sender, EventArgs e) {
            //Hay que buscar los nodos seleccionados

            
            {//RECORDARME sacar esto es solo prueba
                Vble.Distrito = 201;
                Vble.Lote = 53;
                Vble.Remesa = 3;
                Vble.Ruta = 1256;                
            } // ////////// hasta aca

           

            GenerarCarga();



        }


        private bool GenerarCarga() {


            LeerEstructurasArchivosTablas();
            GenerarTablaArchivo("conexiones");
           
            

            return true;
        }


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

        private bool GenerarTablaArchivo(string Tabla) {
            string ArchivoTabla;
            string Carp;
            int Carga = 0;
            //Obtiene el nombre del archivo donde se almacena la tabla
            StringBuilder stb = new StringBuilder();
            Inis.GetPrivateProfileString("Varios", Tabla, Tabla +".btx", stb, 50, Ctte.ArchivoEstructuraColectora);
            ArchivoTabla = stb.ToString();

            //Leer y actualizar el número de carga
            Inis.GetPrivateProfileString("Numeros Cargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
            Carga = int.Parse(stb.ToString()) + 1;
            Inis.WritePrivateProfileString("Numeros Carga", Vble.Distrito.ToString(), 
                Carga.ToString().Trim(), Ctte.ArchivoIniName);

            //La carpeta donde estará el archivo tendrá la forma
            //          EP201602_D201_C00021.aaMMdd_HHmm
            //          EP{Periodo:yyyyMM}_D{Distrito:000}_C{Carga:00000}.{DateTime.Now:yyMMdd_HHmm}
            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM", 
                CultureInfo.CurrentCulture);
            Carp = string.Format("EP{0:yyyyMM}_D{1:000}_C{2:00000}.{3:yyMMdd_HHmm}", Per, 
                Vble.Distrito, Carga, DateTime.Now);          
            ArchivoTabla = Vble.CarpetaTrabajo  + "\\" + Vble.CarpetaCargasNoEnviadas + "\\" + Carp + "\\" + ArchivoTabla;


            ObtenerValoresDeVariablesSistema();
            ArchivoTabla = Vble.ValorarUnNombreRuta(ArchivoTabla);
            //Generarlos datos y cargarlos al archivo.



            return true;
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

       



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
            try
            {

                
                TreeNode Nodo = tvwCargas.SelectedNode;
                int idx = Nodo.Index;
                string sKy = Nodo.Tag.ToString();
                clInfoNodos tn = new clInfoNodos();
                tn = dcNodos[Nodo.Tag.ToString()];
                int n = 0;
                //this.button2_Click(sender, e);

                //toma unicamente los valores de secuencia que esten habilitados como "todos"
                if (Nodo != null)
                {
                    //cargarArrays();
                    if (Nodo.Level == 4)
                       
                     {

                        for (int f = 0; f < vectorSecDESDE.secuencianodos.Length; f++)
                        {
                            if (vectorSecDESDE.secuencianodos[f] > 0)
                            {
                                n++;
                            }
                        }
                        for (int j = 0; j < n; j++)
                        {
                            if (n == 1)
                            {
                                dataGridView1.DataSource = CargarRegistrosSecuencia();
                                label3.Text = dataGridView1.RowCount.ToString();
                                dataGridView1.Columns[1].HeaderText = "Conexión Nº";
                                dataGridView1.Columns[2].HeaderText = "Codigo Localidad";
                                dataGridView1.Columns[3].HeaderText = "Ruta";
                                dataGridView1.Columns[4].HeaderText = "ID Usuario";
                                dataGridView1.Columns[5].HeaderText = "Usuario";
                                dataGridView1.Columns[6].HeaderText = "ID Titular";
                                dataGridView1.Columns[7].HeaderText = "Nombre Titular";
                                dataGridView1.Columns[8].HeaderText = "ID Propietario";
                                dataGridView1.Columns[9].HeaderText = "Nombre Propietario";
                            }
                            if (n == 2)
                            {
                                dataGridView1.DataSource = CargarRegistrosSecuencia2();
                                label3.Text = dataGridView1.RowCount.ToString();
                                dataGridView1.Columns[1].HeaderText = "Conexión Nº";
                                dataGridView1.Columns[2].HeaderText = "Codigo Localidad";
                                dataGridView1.Columns[3].HeaderText = "Ruta";
                                dataGridView1.Columns[4].HeaderText = "ID Usuario";
                                dataGridView1.Columns[5].HeaderText = "Usuario";
                                dataGridView1.Columns[6].HeaderText = "ID Titular";
                                dataGridView1.Columns[7].HeaderText = "Nombre Titular";
                                dataGridView1.Columns[8].HeaderText = "ID Propietario";
                                dataGridView1.Columns[9].HeaderText = "Nombre Propietario";
                            }
                        }                        
                     }  
                    else

                    {

                    }
                }

                dataGridView1.AutoSize = true;
            }
            catch (Exception)
            {
                //retorno = false;
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
                txSQL = "select C.Secuencia, C.conexionID, L.CodigoInt, C.Ruta, C.usuarioID, P.Apellido, C.titularID, P.Apellido, C.propietarioID, P.apellido " +
                        "From conexiones C " +
                        "INNER JOIN personas P ON C.usuarioID = P.personaID and C.titularID = P.personaID and C.propietarioID = P.personaID " +
                        "INNER JOIN localidades L ON L.CodigoPostal = C.CodPostalSumin " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (C.Secuencia >= " + this.textBox2.Text + " and C.Secuencia <= " + this.textBox3.Text + " and C.Ruta = " + Ru.Text + " and L.CodigoInt = " + Loc.Text + " and C.ImpresionOBS = " + 0 + ")"; //" or C.Secuencia >= " +  445513 + " and C.Secuencia <= " + 446720 + " and C.Ruta = " + 1169 + " and C.ImpresionOBS = " + 0 + ")";               

                Tabla = new DataTable();
                        da = new MySqlDataAdapter(txSQL, DB.conexBD);
                        comandoSQL = new MySqlCommandBuilder(da);
                        da.Fill(Tabla);
                //this.Loc.Text = Tabla.Compute("MIN(Conexión Nº)", "").ToString();
                //this.Ru.Text = Tabla.Compute("MAX(Conexión Nº)", "").ToString();
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
        private DataTable CargarRegistrosSecuencia2()
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select C.Secuencia, C.conexionID, L.CodigoInt, C.Ruta, C.usuarioID, P.Apellido, C.titularID, P.Apellido, C.propietarioID, P.apellido " +
                        "From conexiones C " +
                        "INNER JOIN personas P ON C.usuarioID = P.personaID and C.titularID = P.personaID and C.propietarioID = P.personaID " +
                        "INNER JOIN localidades L ON L.CodigoPostal = C.CodPostalSumin " +
                        //"INNER JOIN conceptosdatos D on C.conexionID = D.conexionID " +
                        "Where (C.Secuencia >= " + vectorSecDESDE.secuencianodos[0] + " and C.Secuencia <= " + vectorSecDESDE.secuencianodos[1] + " and C.Ruta = " + Ru.Text + " and L.CodigoInt = " + Loc.Text + " and C.ImpresionOBS = " + 0 + ")"; // or C.Secuencia >= " +  445513 + " and C.Secuencia <= " + 446720 + " and C.Ruta = " + 1169 + " and C.ImpresionOBS = " + 0 + ")";               

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);
                //this.Loc.Text = Tabla.Compute("MIN(Conexión Nº)", "").ToString();
                //this.Ru.Text = Tabla.Compute("MAX(Conexión Nº)", "").ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return Tabla;

        }

        int f = 0;
        /// <summary>
        /// //declaracion global del vector que contendrá los numeros de secuencia DESDE del treview 
        /// //para la muestra de los registros 
        /// </summary>
        public static class vectorSecDESDE
        {
            public static int []
                secuencianodos  = new int [50];
        };
        public static class vectorSecHASTA
        {
            public static int[]
                secuencianodos = new int[50];
        };
       
        

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
            textBox1.Text = tn.Desde.ToString();
                      
            vectorSecDESDE.secuencianodos[f] = int.Parse(tn.Desde.ToString());
            vectorSecHASTA.secuencianodos[f] = int.Parse(tn.Hasta.ToString());
           
            
            //MessageBox.Show(vectorSecDesde.secuencianodos[f].ToString());
            f++;


        }
        private SQLiteConnection con;
        
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
                con = new SQLiteConnection("Data Source=F:/Google Drive/MACRO INTELL - Software/gagFIS-Interfase/gagFIS-Interfase/dbFIS-DPEC.db");
                con.Open();


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
                SQLiteCommand command = new SQLiteCommand(txSQL, con);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();


                //-----PARA CONSULTA SELECT
                //Tabla = new DataTable();
                //da = new SQLiteDataAdapter(txSQL, con);
                //comandoSQL = new SQLiteCommandBuilder(da);
                //da.Fill(Tabla);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return Tabla;

        }

        /// <summary>
        /// //metodo que me muestra todos los datos de cada conexionID en el datagridview2
        /// </summary>
        /// <param name="numeroconexion"></param>
        /// <returns></returns>
        private DataTable VerRegistrosConexion(int numeroconexion)
        {
            MySqlDataAdapter da;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            try
            {
                txSQL = "select * From conexiones Where conexionID = " + numeroconexion;

                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);                
            }
            catch (Exception)
            {
            }
            return Tabla;
        }


        public void cargartablaconexiones()
        {
            int conexionid, periodo, usuarioid, titularid, propietarioid, impresionCod, impresionOBS, impresionCant,
                Operario, lote, zona, ruta, secuencia, remesa, ConsumoPromedio, ConsumoResidual, ConsumoFacturado, ConsumoTipo, OrdenTomado,
                PuntoVenta, FacturaNro1, DocumPago1, FacturaNro2, DocumPago2, HistoPeriodo01, HistoConsumo01, HistoPeriodo02, HistoConsumo02,
                HistoPeriodo03, HistoConsumo03, HistoPeriodo04, HistoConsumo04, HistoPeriodo05, HistoConsumo05, HistoPeriodo06,
                HistoConsumo06, HistoPeriodo07, HistoConsumo07, HistoPeriodo08, HistoConsumo08, HistoPeriodo09, HistoConsumo09, HistoPeriodo10,
                HistoConsumo10, HistoPeriodo11, HistoConsumo11, HistoPeriodo12, HistoConsumo12;
            string DomicSumin, BarrioSumin, CodPostalSumin, CuentaDebito, Categoria, SubCategoria, TipoProrrateo, CESPnumero, CESPvencimiento,
                FacturaLetra, Vencimiento1, Vencimiento2, VencimientoProx;
            float PromedioDiario, Importe1, Importe2;
            

            conexionid = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
            periodo = Convert.ToInt32(dataGridView2.CurrentRow.Cells[1].Value);
            usuarioid = Convert.ToInt32(dataGridView2.CurrentRow.Cells[2].Value);
            titularid = Convert.ToInt32(dataGridView2.CurrentRow.Cells[3].Value);
            propietarioid = Convert.ToInt32(dataGridView2.CurrentRow.Cells[4].Value);
            DomicSumin = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            
            BarrioSumin = dataGridView2.CurrentRow.Cells[6].Value.ToString();
            CodPostalSumin = dataGridView2.CurrentRow.Cells[7].Value.ToString();
            CuentaDebito = dataGridView2.CurrentRow.Cells[8].Value.ToString();            
            impresionCod = Convert.ToInt32(dataGridView2.CurrentRow.Cells[9].Value);
            impresionOBS = Convert.ToInt32(dataGridView2.CurrentRow.Cells[10].Value);
            impresionCant = Convert.ToInt32(dataGridView2.CurrentRow.Cells[11].Value);
            Operario = Convert.ToInt32(dataGridView2.CurrentRow.Cells[12].Value);
            lote  = Convert.ToInt32(dataGridView2.CurrentRow.Cells[13].Value);
            zona = Convert.ToInt32(dataGridView2.CurrentRow.Cells[14].Value);            
            ruta = Convert.ToInt32(dataGridView2.CurrentRow.Cells[15].Value);
            secuencia = Convert.ToInt32(dataGridView2.CurrentRow.Cells[16].Value);
            remesa = Convert.ToInt32(dataGridView2.CurrentRow.Cells[17].Value);
            Categoria = dataGridView2.CurrentRow.Cells[18].Value.ToString();
            //TipoProrrateo = dataGridView2.CurrentRow.Cells[20].Value.ToString();
            SubCategoria = dataGridView2.CurrentRow.Cells[19].Value.ToString();
            ConsumoPromedio = Convert.ToInt32(dataGridView2.CurrentRow.Cells[20].Value);
            ConsumoResidual = Convert.ToInt32(dataGridView2.CurrentRow.Cells[21].Value);
            ConsumoFacturado = Convert.ToInt32(dataGridView2.CurrentRow.Cells[22].Value);
            ConsumoTipo = Convert.ToInt32(dataGridView2.CurrentRow.Cells[23].Value);
            OrdenTomado = Convert.ToInt32(dataGridView2.CurrentRow.Cells[24].Value);
            CESPnumero = dataGridView2.CurrentRow.Cells[25].Value.ToString();
            CESPvencimiento = dataGridView2.CurrentRow.Cells[26].Value.ToString();
            FacturaLetra = dataGridView2.CurrentRow.Cells[27].Value.ToString();
            //PromedioDiario = Convert.ToInt32(dataGridView2.CurrentRow.Cells[22].Value);            
            PuntoVenta = Convert.ToInt32(dataGridView2.CurrentRow.Cells[28].Value);
            FacturaNro1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[29].Value);
            DocumPago1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[30].Value);
            Vencimiento1 = dataGridView2.CurrentRow.Cells[31].Value.ToString();
            Importe1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[32].Value.ToString());
            FacturaNro2 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[33].Value);
            DocumPago2 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[34].Value);
            Vencimiento2 = dataGridView2.CurrentRow.Cells[35].Value.ToString();
            Importe2 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[36].Value);
            VencimientoProx = dataGridView2.CurrentRow.Cells[37].Value.ToString();
            HistoPeriodo01 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[38].Value);
            HistoConsumo01 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[39].Value);
            HistoPeriodo02 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[40].Value);
            HistoConsumo02 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[41].Value);
            HistoPeriodo03 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[42].Value);
            HistoConsumo03 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[43].Value);
            HistoPeriodo04 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[44].Value);
            HistoConsumo04 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[45].Value);
            HistoPeriodo05 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[46].Value);
            HistoConsumo05 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[47].Value);
            HistoPeriodo06 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[48].Value);
            HistoConsumo06 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[49].Value);
            HistoPeriodo07 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[50].Value);
            HistoConsumo07 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[51].Value);
            HistoPeriodo08 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[52].Value);
            HistoConsumo08 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[53].Value);
            HistoPeriodo09 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[54].Value);
            HistoConsumo09 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[55].Value);
            HistoPeriodo10 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[56].Value);
            HistoConsumo10 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[57].Value);
            HistoPeriodo11 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[58].Value);
            HistoConsumo11 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[59].Value);
            HistoPeriodo12 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[60].Value);
            HistoConsumo12 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[61].Value);
            
           
            //parametros de sqlite
            SQLiteDataAdapter da;
            SQLiteCommandBuilder comandoSQL;
            string txSQL;

            try
            {
                con = new SQLiteConnection("Data Source=F:/Google Drive/MACRO INTELL - Software/gagFIS-Interfase/gagFIS-Interfase/dbFIS-DPEC.db");
                con.Open();

                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //txSQL = "select * From conexiones Where conexionID
                txSQL = "INSERT INTO conexiones([conexionID], [Periodo], [usuarioID], [titularID], [propietarioID], DomicSumin, [BarrioSumin], [CodPostalSumin], [CuentaDebito], [ImpresionCOD], [ImpresionOBS], [ImpresionCANT], [Operario]," +
                    " [Lote], [Zona], [Ruta], [Secuencia], [Remesa], [Categoria], [SubCategoria], [ConsumoPromedio]," +
                    " [ConsumoResidual], [ConsumoFacturado], [ConsumoTipo], [OrdenTomado], [CESPnumero], [CESPvencimiento]," +
                    " [FacturaLetra], [PuntoVenta], [FacturaNro1], [DocumPago1], [Vencimiento1], [Importe1], [FacturaNro2], [DocumPago2]," +
                    " [Vencimiento2], [Importe2], [VencimientoProx], [HistoPeriodo01], [HistoConsumo01], [HistoPeriodo02], [HistoConsumo02]," +
                    " [HistoPeriodo03], [HistoConsumo03], [HistoPeriodo04], [HistoConsumo04], [HistoPeriodo05], [HistoConsumo05], [HistoPeriodo06]," +
                    " [HistoConsumo06], [HistoPeriodo07], [HistoConsumo07], [HistoPeriodo08], [HistoConsumo08], [HistoPeriodo09], [HistoConsumo09]," +
                    " [HistoPeriodo10], [HistoConsumo10], [HistoPeriodo11], [HistoConsumo11], [HistoPeriodo12],[HistoConsumo12]) " +
                    "VALUES ("+ conexionid +", " + periodo + ", " + usuarioid + ", " + titularid + ", " + propietarioid + ", "  + DomicSumin + ","  +  BarrioSumin + ", " + CodPostalSumin + ", " + CuentaDebito + ", " + impresionCod + ", " + impresionOBS + ", " + impresionCant + 
                    ", " + Operario + ", " + lote + ", " + zona + ", " + ruta + ", " + secuencia + ", " + remesa + ", " + Categoria +
                    ", " + SubCategoria + ", " + ConsumoPromedio + ", " + ConsumoResidual + ", " + ConsumoFacturado + ", " + ConsumoTipo +
                    ", " + OrdenTomado + ", " + CESPnumero + ", " + CESPvencimiento + ", " + FacturaLetra + ", " + PuntoVenta + ", " + FacturaNro1 + ", " + DocumPago1 + ", " + Vencimiento1 +
                    ", " + Importe1 + ", " + FacturaNro2 + ", " + DocumPago2 + ", " + Vencimiento2 + ", " + Importe2 + ", " + VencimientoProx + ", " + HistoPeriodo01 +
                    ", " + HistoConsumo01 + ", " + HistoPeriodo02 + ", " + HistoConsumo02 + ", " + HistoPeriodo03 + ", " + HistoConsumo03 + ", " + HistoPeriodo04 + ", " + HistoConsumo04 +
                    ", " + HistoPeriodo05 + ", " + HistoConsumo05 + ", " + HistoPeriodo06 + ", " + HistoConsumo06 + ", " + HistoPeriodo07 + ", " + HistoConsumo07 + 
                    ", " + HistoPeriodo08 + ", " + HistoConsumo08 + ", " + HistoPeriodo09 + ", " + HistoConsumo09 + ", " + HistoPeriodo10 + ", " + HistoConsumo10 +
                    ", " + HistoPeriodo11 + ", " + HistoConsumo11 + ", " + HistoPeriodo12 + ", " + HistoConsumo12 +") from conexiones";

                //preparamos la cadena pra insercion
                SQLiteCommand command = new SQLiteCommand(txSQL, con);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();
            }
            
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    


        /// <summary>
        /// //Boton 4..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            int codconex;

            try {                
                if (this.dataGridView1.RowCount > 0)
                {
                    //cargaGrid();
                    foreach (DataGridViewRow Fila in dataGridView1.Rows)
                    {
                        codconex = Convert.ToInt32(Fila.Cells["conexionID"].Value);
                        dataGridView2.DataSource = VerRegistrosConexion(codconex);
                        cargartablaconexiones();                      


                        //foreach (DataGridViewRow row in dataGridView2.Rows)
                        //{

                        //    dataGridConex.Rows.Add(row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value, row.Cells[4].Value,
                        //        row.Cells[5].Value, row.Cells[6].Value, row.Cells[7].Value, row.Cells[8].Value, row.Cells[9].Value, row.Cells[10].Value,
                        //        row.Cells[11].Value, row.Cells[12].Value, row.Cells[13].Value, row.Cells[14].Value, row.Cells[15].Value, row.Cells[16].Value,
                        //        row.Cells[17].Value, row.Cells[18].Value, row.Cells[19].Value, row.Cells[20].Value, row.Cells[21].Value, row.Cells[22].Value,
                        //        row.Cells[23].Value, row.Cells[24].Value, row.Cells[25].Value, row.Cells[26].Value, row.Cells[27].Value, row.Cells[28].Value,
                        //        row.Cells[29].Value, row.Cells[30].Value, row.Cells[31].Value, row.Cells[32].Value, row.Cells[33].Value, row.Cells[34].Value,
                        //        row.Cells[35].Value, row.Cells[36].Value, row.Cells[37].Value, row.Cells[38].Value, row.Cells[39].Value, row.Cells[40].Value,
                        //        row.Cells[41].Value, row.Cells[42].Value, row.Cells[43].Value, row.Cells[44].Value, row.Cells[45].Value, row.Cells[46].Value,
                        //        row.Cells[47].Value, row.Cells[48].Value, row.Cells[49].Value, row.Cells[50].Value, row.Cells[51].Value, row.Cells[52].Value,
                        //        row.Cells[53].Value, row.Cells[54].Value, row.Cells[55].Value, row.Cells[56].Value, row.Cells[57].Value, row.Cells[58].Value,
                        //        row.Cells[59].Value, row.Cells[60].Value, row.Cells[61].Value, row.Cells[62].Value, row.Cells[63].Value);
                        //    //dataGridConex.DataSource = CargarRegistrosConexiones();
                        //}



                    }
                }
                else
                {
                    MessageBox.Show("error en el boton 4 ");
                }
            }
            catch (Exception)
            { 
            }            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tvwCargas_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string clave = "";
            
            int j = 0;

            //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
            foreach (TreeNode tNd1 in tvwCargas.Nodes[0].Nodes)
            {
                if (tNd1.ImageKey != "nada")
                {
                    clave = tNd1.Tag.ToString();
                    Vble.Distrito = dcNodos[clave].Distrito;
                    MessageBox.Show(Vble.Distrito.ToString(), "Distrito");
                    ArrayLocalidad.Add(Vble.Distrito.ToString());
                    
                    //Obtener Remesa, se busca en nivel 2, debajo del nodo clave.
                    foreach (TreeNode tNd2 in tNd1.Nodes)
                    {
                        if (tNd2.ImageKey != "nada")
                        {
                            clave = tNd2.Tag.ToString();
                            Vble.Remesa = dcNodos[clave].Remesa;
                            //MessageBox.Show(Vble.Remesa.ToString(), "Remesa");
                            //Obtener Ruta, se busca en nivel 3, debajo del nodo clave.
                            foreach (TreeNode tNd3 in tNd2.Nodes)
                            {
                                if (tNd3.ImageKey == "todo")
                                {
                                    clave = tNd3.Tag.ToString();
                                    Vble.Ruta = dcNodos[clave].Ruta;
                                    
                                    ArrayRuta.Add(Vble.Ruta.ToString());
                                    MessageBox.Show(ArrayRuta[j].ToString(), "RUTA");
                                }
                                foreach (TreeNode tNd4 in tNd3.Nodes)
                                {
                                    if (tNd4.ImageKey == "todo")
                                    {
                                        
                                        clave = tNd4.Tag.ToString();
                                        Vble.desde = dcNodos[clave].Desde;
                                        Vble.hasta = dcNodos[clave].Hasta;
                                       
                                        ArrayDesde.Add(Vble.desde.ToString());
                                        ArrayHasta.Add(Vble.hasta.ToString());
                                        MessageBox.Show("DESDE: (" + ArrayDesde[j].ToString() + ")  HASTA: (" + ArrayHasta[j].ToString() + ")");
                                        j++;
                                    }
                                 
                                }

                            }
                            break;
                        }
                    }
                    break;
                }

            }




            //try
            //{

            //    for (int f = 0; f < vectorSecDESDE.secuencianodos.Length; f++)
            //    {

            //        if (vectorSecDESDE.secuencianodos[f] > 0)
            //        {
            //            MessageBox.Show(vectorSecDESDE.secuencianodos[f].ToString(), "DESDE");
            //            MessageBox.Show(vectorSecHASTA.secuencianodos[f].ToString(), "HASTA");

            //        }


            //    }


            //}
            //catch
            //{

            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //string Valor = "";
            string clave = "";

            int j = 0;

            try
            {

           
            //Obtener Distrito (Zona), se busca en el nivel 1, es decir debajo de "dpec"
            foreach (TreeNode tNd1 in tvwCargas.Nodes[0].Nodes)
            {
                if (tNd1.ImageKey != "nada")
                {
                    clave = tNd1.Tag.ToString();
                    Vble.Distrito = dcNodos[clave].Distrito;
                    MessageBox.Show(Vble.Distrito.ToString(), "Distrito"); 
                    //Obtener Remesa, se busca en nivel 2, debajo del nodo clave.
                    foreach (TreeNode tNd2 in tNd1.Nodes)
                    {
                        if (tNd2.ImageKey != "nada")
                        {
                           
                            //Obtener Ruta, se busca en nivel 3, debajo del nodo clave.
                            foreach (TreeNode tNd3 in tNd2.Nodes)
                            {
                                if (tNd3.ImageKey == "todo")
                                {
                                  MessageBox.Show(ArrayRuta[j].ToString(), "RUTA");
                                }
                                foreach (TreeNode tNd4 in tNd3.Nodes)
                                {
                                    if (tNd4.ImageKey == "todo")
                                    {                                      
                                        MessageBox.Show("DESDE: (" + ArrayDesde[j].ToString() + ")  HASTA: (" + ArrayHasta[j].ToString() + ")");
                                        j++;
                                    }

                                }

                            }
                            break;
                        }
                    }
                    break;
                }
                
            }
            }
            catch (Exception)
            {               
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



}
