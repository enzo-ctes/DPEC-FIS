using DGV2Printer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
using Rectangle = System.Drawing.Rectangle;
using System.Text.RegularExpressions;

namespace gagFIS_Interfase
{
    public partial class FormDetallePreDescarga : Form
    {

        public string ImpresionOBS { get; set; }
        public string RutaDatos { get; set; }
        public string Ruta { get; set; }
        public string RutaNº { get; set; }
        public string Remesa { get; set; }
        public int Periodo { get; set; }
        public string CONSULTA { get; set; }
        public string TipoInforme { get; set; }
        public string CONSULTANOIMPRESOS { get; set; }
        public string CONSULTAREMESA { get; set; }
        public string ultimaConsultaReg { get; set; }
        public bool NoImpr { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public new Boolean Visible { get; set; }
        DataView dtview = new DataView();
        DataTable Tabla = new DataTable();
        public string IndicadorTipoInforme { get; set; }
        ///Variable que identifica cuando se solicita el informe de toda la remesa de la localidad seleccionada del periodo vigente
        public string RBSelectionRemesa { get; set; } = "NO";
        ///Variable que identifica cuando se solicita el informe de toda localidad (Zona) seleccionada del periodo
        ///vigente, incluye todas las remesas cargadas en la base de FIS hasta el momento.
        public string RBSelectionZona { get; set; }
        public string ResZona { get; set; }
        public string[] ArrayCodNovedades = new string[5];
        public string[] ArrayDescrNovedades = new string[5];
        public string Observaciones = "";
        public delegate void InvokeDelegate();
        ListViewItem items;
        public string VerResumenGral { get; set; }
        StringBuilder stbResumen = new StringBuilder("", 2);
        public static string PantallaSolicitud { get; set; }
        public string PantallaEstRuta { get; set; } = "NO";
        List<Datos> result;

        ArrayList zonasGrafico = new ArrayList();
        ArrayList TotalZonasGrafico = new ArrayList();
        ArrayList leidosGrafico = new ArrayList();
        ArrayList impresosGrafico = new ArrayList();
        ArrayList teleLecturaGrafico = new ArrayList();

        ArrayList PerGrafTot = new ArrayList();
        ArrayList PerImprTot = new ArrayList();
        //DataGridView dataGridViewAltas = new DataGridView();

        DataTable TablaAltas;
        DataTable TablaModificaciones;


        /// <summary>
        /// Variables de No impresion por impresora
        /// </summary>
        int totalErrorIndefImpresora = 0;
        int totalImpresoraDeshab = 0;
        int totalImpresoraApagada = 0;
        int totalImpresoraNoVinc = 0;
        int totalImpresoraVinculacion = 0;
        int totalImpresoraComunicacion = 0;
        int totalMarcadosPorLote = 0;
        int totalNoImprPorWS = 0;
        int totalFueraRango = 0;
        int totalIndicadoDato = 0;
        int totalCorreccionEstado = 0;
        int totalNovedadNoImpri = 0;
        int totalTarifaNoImpri = 0;
        int totalExcesoConsumoSap = 0;
        int totalExcesoImporteSAP = 0;
        int totalExcesoDiasPeriodo = 0;
        int totalExcesoRenglones = 0;
        int totalMedidorApagado = 0;

        //Variables cargadas al hacer click derecho sobre el DGResumen
        ContextMenuStrip menu = new ContextMenuStrip();
        string ZonaDetRes = "";
        string RemesaDetRes = "";

        public FormDetallePreDescarga()
        {
            InitializeComponent();
        }


        private void FormDetallePreDescarga_Load(object sender, EventArgs e)
        {
            if (RBSelectionRemesa.ToString() == "SI")
            {
                pantallaLoading(BGWCargaTablas);
            }
            else
            {
                //var bounds = Screen.FromControl(this).Bounds;
                //MiLoadingInformes.Size = new Size(bounds.Width, bounds.Height);               
                //PanelHistorial.Visible = false;
                //BGWProcessing.RunWorkerAsync();

                MiLoadingInformes.Visible = true;




                this.DTPDesdeTomLect.Text = Vble.TextDesdeInformes;
                this.DTPHastaTomLect.Text = Vble.TextHastaInformes;
                Visible = true;
                RutaNº = TextBoxRuta.Text;
                CheckForIllegalCrossThreadCalls = false;
                lbOrdenativos.Size = new Size(250, 124);
                LabCargandoInformes.Location = new Point(MiLoadingInformes.Width / 2 - 180, MiLoadingInformes.Height / 2 + 200);

                //int anchoScreen = Screen.PrimaryScreen.Bounds.Width;
                //int altoScreen = Screen.PrimaryScreen.Bounds.Height;
                //LabCargandoInformes.Location = new Point(anchoScreen / 2, altoScreen / 2);

                Int16 contRelev = 0;

                //MiLoadingInformes.Visible = true;
                //LabCargandoInformes.Visible = true;
                ////CBRemesa.Text = Remesa;
                Inis.GetPrivateProfileString("Datos", "ResumenGralRuta", "", stbResumen, 8, Ctte.ArchivoIniName);

                if (DB.sDbUsu.ToUpper() == "SUPERVISOR" || DB.sDbUsu.ToUpper() == "AUDITORIA")
                {
                    PanelChart.Visible = true;
                    lblModRelev.Visible = false;
                    lblTotModRel.Visible = false;
                    panelSuperior.Visible = false;
                    ButPrint.Visible = false;
                    button3.Visible = false;
                    BtnPDF.Visible = true;
                    lbOrdenativos.Visible = false;
                    lblTotalUsConOrd.Visible = false;
                    LblBuscar.Location = new Point(333, 76);
                    TextFiltro.Location = new Point(390, 67);
                    BtnExcel.Location = new Point(720, 70);
                    BtnPDF.Location = new Point(760, 70);
                    BtnTxt.Location = new Point(800, 70);
                    LabelLeyenda.Location = new Point(910, 30);
                    LabelPeriodo.Location = new Point(910, 55);
                    splitContainer1.SplitterDistance = 100;
                    splitContainer2.SplitterDistance = 400;
                    PanelChart.Visible = false;

                    if (IndicadorTipoInforme == "Resumen")
                    {
                        CargarTablaPreDescarga();

                        if (LabelLeyenda.Text == "TodosR")
                        {
                            if (stbResumen.ToString() == "1")
                            {
                                if (PantallaEstRuta == "NO")
                                {
                                    GroupBoxResumenGral.Visible = true;
                                    //ContarOrdenativos();
                                    CargarResumenGeneral();
                                }
                                else
                                {
                                    BtnPDF.Visible = false;
                                    ButPrint.Visible = false;
                                    button3.Visible = false;
                                    TextFiltro.Enabled = true;
                                }
                            }
                        }
                        LabelLeyenda.Visible = true;

                        if (DB.sDbUsu.ToUpper() == "AUDITORIA")
                        {
                            TextFiltro.Enabled = true;
                            panelSuperior.Size = new Size(1100, 53);
                            panelSuperior.Visible = true;
                            LabelRemesa.Visible = false;
                            CBRemesa.Visible = false;
                            label6.Visible = false;
                            TextBoxRuta.Visible = false;
                            groupBox2.Visible = false;
                            groupBox3.Visible = false;
                            splitContainer1.SplitterDistance = 170;
                            LabelLeyenda.Location = new Point(910, 70);
                            LabelPeriodo.Location = new Point(910, 95);
                        }
                        BtnTxt.Visible = true;
                        //button4.Location = new Point(452, 126);
                        PanelTotOrd.Visible = false;
                        PanelHistorial.Visible = false;
                    }
                    else if (IndicadorTipoInforme == "Historial")
                    {
                        CargarPeriodos();
                        PanelHistorial.Visible = true;
                        //CargarTablaHistorial();
                    }
                    /// Bandera cuando se ingresa con el usuario Supervisor y se pide resumen por remesa individual.
                    else if (IndicadorTipoInforme == "ResumenRemesa")
                    {

                        CargarTablaPreDescarga();


                        if (LabelLeyenda.Text == "Todos")
                        {
                            if (stbResumen.ToString() == "1")
                            {
                                if (PantallaEstRuta == "NO")
                                {
                                    GroupBoxResumenGral.Visible = true;
                                    //ContarOrdenativos();
                                    CargarResumenGeneral();
                                }
                                else
                                {
                                    BtnPDF.Visible = false;
                                    ButPrint.Visible = false;
                                    button3.Visible = false;
                                    TextFiltro.Enabled = true;
                                }
                            }
                        }

                        if (ImpresionOBS == "999" || ImpresionOBS == "9999")
                        {
                            //BeginInvoke(new InvokeDelegate(InvokeMethod));
                            ArmarGraficos(DGResumenExp, TipoInforme);
                            TabCtrlGraficos.TabPages["TabPagPeriodos"].Text = "";
                            //GraficarComparacionPeriodos();
                            BeginInvoke(new InvokeDelegate(InvokeMethod2));
                        }
                        else
                        {
                            PanelChart.Visible = false;
                            BtnExcel.Visible = true;
                            GroupBoxResumenGral.Visible = false;
                        }
                        MiLoadingInformes.Visible = false;
                        LabCargandoInformes.Visible = false;
                        //bool resultado = await task;
                        //if (resultado)
                        //{
                        //    MessageBox.Show("termino");
                        //}
                    }

                    if (TipoInforme == "T")
                    {
                        LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " Remesa 1-8 ";
                        LabelPeriodo.Visible = true;
                    }
                    else if (TipoInforme == "R")
                    {
                        LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " Remesa " + Remesa + " - " + LabelLeyenda.Text;
                        LabelPeriodo.Visible = true;
                    }
                    else
                    {
                        LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " " + leyenda.Text;
                        LabelPeriodo.Visible = true;
                    }
                    leyenda.Visible = false;

                }
                //Inicio bloque de informes para usuarios operario de DPEC
                else
                {
                    PanelChart.Visible = false;
                    if (IndicadorTipoInforme == "Resumen")
                    {
                        CargarTablaPreDescarga();
                        ContarRelevM();
                        //IdentificarEstadoContMenorFact();

                        if (LabelLeyenda.Text == "Todos")
                        {
                            if (stbResumen.ToString() == "1")
                            {
                                if (PantallaEstRuta == "NO")
                                {
                                    if (RBSelectionRemesa.ToString() == "SI")
                                    {

                                    }
                                    else
                                    {
                                        GroupBoxResumenGral.Visible = true;
                                        //ContarOrdenativos();
                                        /* ContarRelevM()*/
                                        ;
                                        CargarResumenGeneral();
                                    }
                                }
                                else
                                {
                                    BtnPDF.Visible = false;
                                    ButPrint.Visible = false;
                                    button3.Visible = false;
                                    TextFiltro.Enabled = true;
                                }
                            }
                        }
                        BtnTxt.Visible = false;
                        //button4.Location = new Point(566, 126);
                        PanelTotOrd.Visible = false;
                        LabelLeyenda.Visible = true;
                        BtnExcel.Visible = true;
                        LabelLeyenda.Location = new Point(870, 88);
                        LabelPeriodo.Location = new Point(870, 110);
                        LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " " + leyenda.Text;
                        LabelPeriodo.Visible = true;
                        PanelHistorial.Visible = false;
                        splitContainer2.SplitterDistance = 400;
                        TextFiltro.Enabled = true;
                    }
                    else if (IndicadorTipoInforme == "Historial")
                    {
                        CargarPeriodos();
                        PanelHistorial.Visible = true;
                        //CargarTablaHistorial();
                    }
                    else if (IndicadorTipoInforme == "ResumenRemesa")
                    {
                        CargarTablaPreDescarga();



                        panelSuperior.Visible = false;
                        ButPrint.Visible = false;
                        button3.Visible = false;
                        BtnPDF.Visible = false;
                        lbOrdenativos.Visible = false;
                        lblTotalUsConOrd.Visible = false;
                        GroupBoxResumenGral.Visible = true;
                        GroupBoxResumenGral.Text = "Resumen Gráfico";
                        //GroupBoxResumenGral.Size = new Size(1200, 184);
                        DGResumenExp.ScrollBars = ScrollBars.Both;

                        if (LabelLeyenda.Text == "Todos")
                        {
                            if (stbResumen.ToString() == "1")
                            {
                                if (PantallaEstRuta == "NO")
                                {
                                    GroupBoxResumenGral.Visible = true;
                                    //ContarOrdenativos();
                                    CargarResumenGeneral();
                                }
                                else
                                {
                                    CargarTablaPreDescarga();
                                    BtnPDF.Visible = false;
                                    ButPrint.Visible = false;
                                    button3.Visible = false;
                                    TextFiltro.Enabled = true;
                                }
                            }
                        }

                        if (ImpresionOBS == "999" || ImpresionOBS == "9999")
                        {
                            ArmarGraficos(DGResumenExp, TipoInforme);
                            PanelChart.Visible = true;
                            //LabelLeyenda.Visible = false;
                            //LabelPeriodo.Text = "Periodo: " + Vble.Periodo.ToString();
                            LabelPeriodo.Visible = true;
                            PanelHistorial.Visible = false;
                            PanelTotOrd.Visible = false;
                            LblBuscar.Location = new Point(333, 76);
                            TextFiltro.Location = new Point(390, 67);
                            BtnExcel.Location = new Point(720, 70);
                            BtnPDF.Location = new Point(760, 70);
                            BtnTxt.Location = new Point(800, 70);
                            LabelLeyenda.Location = new Point(910, 30);
                            LabelPeriodo.Location = new Point(910, 35);
                            splitContainer1.SplitterDistance = 100;
                            //splitContainer3.SplitterDistance = 500;
                            splitContainer2.SplitterDistance = 200;
                            TextFiltro.Enabled = true;
                            GraficarComparacionPeriodos();
                        }
                        else
                        {
                            PanelChart.Visible = false;
                            BtnExcel.Visible = true;
                            //LblBuscar.Location = new Point(333, 76);
                            //TextFiltro.Location = new Point(390, 67);
                            //BtnExcel.Location = new Point(720, 70);
                            //BtnPDF.Location = new Point(760, 70);
                            //BtnTxt.Location = new Point(800, 70);
                            //LabelLeyenda.Location = new Point(910, 30);
                            //LabelPeriodo.Location = new Point(910, 35);
                            //splitContainer1.SplitterDistance = 100;
                            ////splitContainer3.SplitterDistance = 500;
                            //splitContainer2.SplitterDistance = 400;
                            GroupBoxResumenGral.Visible = false;
                        }
                        //PanelChart.Visible = true;
                        //splitContainer3.Dock = DockStyle.Top;
                        //splitContainer3.Size = new Size(1129, 100);
                    }
                    LabelLeyenda.Visible = false;
                    leyenda.Visible = false;
                    LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " " + leyenda.Text;
                    LabelPeriodo.Visible = true;
                    PanelHistorial.Visible = false;
                    TextFiltro.Enabled = true;
                }
                //Vble.ShowLoading();
                //Task oTask = new Task(Algo);
                //oTask.Start();
                //await oTask;


                MiLoadingInformes.Visible = false;
                LabCargandoInformes.Visible = false;


                //// poner en marcha la operacion en segundo plano
                //// se va a disparar el evento [worker_DoWork] y se va a ejecutar su contenido
                //// alli es donde hay que poner la opercion que debe realizarse
                //BGWProcessing.RunWorkerAsync();        





            }          

        }

        private void IdentificarEstadoContMenorFact()
        {
            foreach (DataGridViewRow item in DGResumenExp.Rows)
            {
                if (item.Cells["Situacion"].Value.ToString() == "Estado Contador < Facturado")
                {
                    item.DefaultCellStyle.ForeColor = Color.Red;
                }
                
            }
          
        }

        private void InvokeMethod2()
        {
            panelSuperior.Visible = false;
            ButPrint.Visible = false;
            button3.Visible = false;
            BtnPDF.Visible = false;
            lbOrdenativos.Visible = false;
            lblTotalUsConOrd.Visible = false;
            GroupBoxResumenGral.Visible = true;
            GroupBoxResumenGral.Text = "Resumen Gráfico";
            //GroupBoxResumenGral.Size = new Size(1200, 184);
            DGResumenExp.ScrollBars = ScrollBars.Both;

            PanelChart.Visible = true;
            //LabelLeyenda.Visible = false;
            //LabelPeriodo.Text = "Periodo: " + Vble.Periodo.ToString();
            LabelPeriodo.Visible = true;
            PanelHistorial.Visible = false;
            PanelTotOrd.Visible = false;
            LblBuscar.Location = new Point(333, 76);
            TextFiltro.Location = new Point(390, 67);
            BtnExcel.Location = new Point(720, 70);
            BtnPDF.Location = new Point(760, 70);
            BtnTxt.Location = new Point(800, 70);
            LabelLeyenda.Location = new Point(910, 30);
            LabelPeriodo.Location = new Point(910, 35);
            //splitContainer1.SplitterDistance = 100;
            //splitContainer3.SplitterDistance = 500;
            //splitContainer2.SplitterDistance = 200;
            TextFiltro.Enabled = true;

        }

        private void InvokeMethod()
        {
            PanelChart.Visible = true;
            //LabelLeyenda.Visible = false;
            //LabelPeriodo.Text = "Periodo: " + Vble.Periodo.ToString();
            LabelPeriodo.Visible = true;
            PanelHistorial.Visible = false;
            PanelTotOrd.Visible = false;
            LblBuscar.Location = new Point(333, 76);
            TextFiltro.Location = new Point(390, 67);
            BtnExcel.Location = new Point(720, 70);
            BtnPDF.Location = new Point(760, 70);
            BtnTxt.Location = new Point(800, 70);
            LabelLeyenda.Location = new Point(910, 30);
            LabelPeriodo.Location = new Point(910, 35);
            splitContainer1.SplitterDistance = 100;
            //splitContainer3.SplitterDistance = 500;
            splitContainer2.SplitterDistance = 200;
            TextFiltro.Enabled = true;
        }

        private bool PantallaWait()
        {
            CargarTablaPreDescarga();
           
            return true;
        }

        private void pantallaLoading(BackgroundWorker bgw)
        {
            MiLoadingInformes.Visible = true;
            if (bgw.IsBusy != true)
            {
                // poner en marcha la operacion en segundo plano
                // se va a disparar el evento [worker_DoWork] y se va a ejecutar su contenido
                // alli es donde hay que poner la opercion que debe realizarse


                bgw.RunWorkerAsync();

            }

        }

        public void Algo()
        {

            
        

        }

        /// <summary>
        /// ContarOrdenativos va a recorrer el datagridview obtenido del detalle de la ruta y va a contar cuantos ordenativos existen
        /// en el mismo, considera cada ordenativo por separado, es decir que, si un usuario tiene tres ordenativos, en el conteo
        /// son 3 ordenativos disntintos para el total que existe en la ruta.
        /// </summary>
        private void ContarOrdenativos()
        {
            int ord1 = 0;
            int ord2 = 0;
            int ord3 = 0;
            int ord4 = 0;
            int ord5 = 0;

            if (DGResumenExp.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in DGResumenExp.Rows)
                {
                                    
                        if (item.Cells["Ord1"].Value.ToString() != "")
                        {
                            ord1++;
                            if (item.Cells["Ord2"].Value.ToString() != "")
                            {
                                ord2++;
                                if (item.Cells["Ord3"].Value.ToString() != "")
                                {
                                    ord3++;
                                    if (item.Cells["Ord4"].Value.ToString() != "")
                                    {
                                        ord4++;
                                        if (item.Cells["Ord5"].Value.ToString() != "")
                                        {
                                            ord5++;
                                        }
                                    }
                                }
                            }
                        }
                        else if (item.Cells["Ord2"].Value.ToString() != "")
                        {
                            ord2++;
                        }
                        else if (item.Cells["Ord3"].Value.ToString() != "")
                        {
                            ord3++;
                        }
                        else if (item.Cells["Ord4"].Value.ToString() != "")
                        {
                            ord4++;
                        }
                        else if (item.Cells["Ord5"].Value.ToString() != "")
                        {
                            ord5++;
                        }
                     }               
            }

            int TotalConOrd = ord1;
            int TotalOrd = ord1 + ord2 + ord3 + ord4 + ord5;

            
            LblTotOrdenativos.Text = "Total Ordenativos: " + TotalOrd.ToString();
        }


        /// <summary>
        /// ContarOrdenativos va a recorrer el datagridview obtenido del detalle de la ruta y va a contar cuantos ordenativos existen
        /// en el mismo, considera cada ordenativo por separado, es decir que, si un usuario tiene tres ordenativos, en el conteo
        /// son 3 ordenativos disntintos para el total que existe en la ruta.
        /// </summary>
        private void ContarRelevM()
        {
            
        }



        #region Metodos


        /// <summary>
        /// Función que carga las novedades en caso de que exista para la conexion que se pasa por parametro 
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private void CargarNovedadesConex(string ConexionID, string periodo)
        {
            DataTable Tabla2 = new DataTable();
            //string obsernov = "";            

            for (int i = 0; i < ArrayCodNovedades.Length; i++)
            {
                ArrayCodNovedades[i] = "";
            }



            try
            {
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;

                txSQL = "select N.* " +
                           "From NovedadesConex N " +
                           "INNER JOIN Conexiones C ON C.ConexionID = N.ConexionID AND C.Periodo = N.Periodo " +
                           "Where (N.ConexionID = " + ConexionID + " and N.Periodo = " + Vble.Periodo + ") ";


                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla2);
                int indice = 0;
                foreach (DataRow fi in Tabla2.Rows)
                {
                    if ((fi.Field<int>("ConexionID").ToString() == ConexionID))
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
                        //Vble.EstadoCorregido = fi.Field<string>("Observ");

                        ArrayCodNovedades[indice] = "";
                        ArrayDescrNovedades[indice] = "";
                        Observaciones += ArrayDescrNovedades[indice].Replace("|", "");
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(fi.Field<string>("Observ")))
                        {
                            ArrayDescrNovedades[indice] = (fi.Field<string>("Observ").Replace("'", "").Trim());
                            Observaciones += ArrayDescrNovedades[indice].Replace("|", "");
                        }
                    }

                    indice++;
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error en Novedades de Conexion");
            }
            //return Tabla2;
        }

        /// <summary>
        /// Devuelve TRUE la conexion pasada como parametro contiene novedades,
        /// caso contrario FALSE
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <returns></returns>
        private bool ExisteNovedades(string ConexionID, string Periodo)
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
                dr.Dispose();
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
        /// Se exporta la tabla en vista a un archivo pdf en la direccion que se especifica cuando se guarda el archivo.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="NombreArchivo"></param>
        private void ExportarAltasPDF(DataGridView grd, string NombreArchivo, string SavePath, bool Domicilio)
        {
            DataTable Tabla = new DataTable();
            Vble.LeerNombresCarpetas();
            //Creo el docuemento .pdf con el formato especificado
            Document document = new Document(PageSize.A4);
            //Gira la hoja en posicion horizontal
            ArrayList Ordenativos = new ArrayList();
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
          

            string PathInformesAltas = Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo;

            //Creating iTextSharp Table from the DataTable data
            //PdfPTable pdfTable = new PdfPTable(LVResumenGral.Columns.Count);
            //pdfTable.DefaultCell.Padding = LVResumenGral.Width - 100;
            //pdfTable.WidthPercentage = 90;
            //pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //pdfTable.DefaultCell.BorderWidth = 1;

            //pdftable que contendra los datos del resumen general
            PdfPTable pdfTable = new PdfPTable(LVResumenGral.Columns.Count);
            float[] widthsResumen = new float[0];
            //asigno el ancho de las columnas 
            iTextSharp.text.Rectangle page0 = document.PageSize;
            pdfTable = new PdfPTable(13);
            pdfTable.WidthPercentage = 180;
            pdfTable.TotalWidth = page0.Width - 90;
            pdfTable.DefaultCell.Padding = LVResumenGral.Width - 100;
            pdfTable.LockedWidth = true;
            widthsResumen = new float[] { 1.0f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f };
            pdfTable.SetWidths(widthsResumen);

            //LabelLeyenda.Text = "Todos";
            //pdftableAltas que contendra los datos de las altas en caso de que tenga la ruta
            PdfPTable pdfTableAltas = new PdfPTable(8);
            float[] widthsAltas = new float[0];
            //asigno el ancho de las columnas 
            iTextSharp.text.Rectangle page1 = document.PageSize;
            pdfTableAltas = new PdfPTable(10);
            pdfTableAltas.WidthPercentage = 180;
            pdfTableAltas.TotalWidth = page1.Width - 90;
            //pdfTable.DefaultCell.Padding = LVResumenGral.Width - 100;
            pdfTableAltas.LockedWidth = true;
            widthsAltas = new float[] { 0.6f, 0.8f, 0.6f, 0.8f, 0.6f, 2.0f, 2.0f, 0.8f, 1.1f, 1.1f };
            pdfTableAltas.SetWidths(widthsAltas);


            //pdftableModificaciones que contendra los datos de las Modificaciones en caso de que tenga la ruta
            PdfPTable pdfTableModifica = new PdfPTable(9);
            float[] widthsModifica = new float[0];
            //asigno el ancho de las columnas 
            iTextSharp.text.Rectangle page2 = document.PageSize;
            pdfTableModifica = new PdfPTable(10);
            pdfTableModifica.WidthPercentage = 180;
            pdfTableModifica.TotalWidth = page1.Width - 90;
            //pdfTable.DefaultCell.Padding = LVResumenGral.Width - 100;
            pdfTableModifica.LockedWidth = true;
            widthsModifica = new float[] { 0.5f, 0.7f, 0.5f, 1.0f, 1.0f, 0.3f, 0.3f, 2.0f, 2.0f, 0.5f};
            pdfTableModifica.SetWidths(widthsModifica);


            if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
            {
                Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
            }

            PdfWriter wri = PdfWriter.GetInstance(document, new FileStream(SavePath + ".pdf", FileMode.OpenOrCreate));
            //PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
            wri.PageEvent = new PageEventHelper();
            wri.Open();
            document.Open();

            /// ********************Creamos la imagen de DPEC y le ajustamos el tamaño
            //iTextSharp.text.Image imagenDPEC = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
            Ctte.imagenDPEC.BorderWidth = 0;
            ///imagenDPEC.Alignment = Element.ALIGN_RIGHT;

            ///imagenDPEC.SetAbsolutePosition(40f, 790f);  posicion de imagen para hoja vertical
            Ctte.imagenDPEC.SetAbsolutePosition(40f, 510f);//  posicion de imagen para hoja horizontal
            float percentage1 = 0.0f;                 //  
            percentage1 = 70 / Ctte.imagenDPEC.Width;      //  Edito tamaño de imagen
            Ctte.imagenDPEC.ScalePercent(percentage1 * 100);//

            ///*******************Creamos la imagen de MacroIntell y le ajustamos el tamaño
            //iTextSharp.text.Image imagenMINTELL = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\MacroIntell Isologo.jpg");
            Ctte.imagenMINTELL.BorderWidth = 0;
            ///imagenMINTELL.Alignment = Element.ALIGN_LEFT;
            ///imagenMINTELL.SetAbsolutePosition(500f, 790f);  posicion de imagen para hoja vertical
            Ctte.imagenMINTELL.SetAbsolutePosition(750f, 530f);// posicion de imagen para hoja horizontal
            float percentage2 = 0.0f;                     //
            percentage2 = 50 / Ctte.imagenMINTELL.Width;       //edito Tamaño de imagen
            Ctte.imagenMINTELL.ScalePercent(percentage2 * 100);//
                                                          //*************************************************************************************************************************

            //datos del informe                       
            document.Add(Ctte.imagenMINTELL);
            document.Add(Ctte.imagenDPEC);         
            Vble.leyenda = leyenda.Text;
            Vble.rutas = TextBoxRuta.Text;

            Ctte.chunkLeyenda = new Chunk("         Periodo: " + Vble.Periodo + " \n\n       " + Vble.leyenda,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            //chunk.SetUnderline(0.9f, -1.8f);
            Paragraph titulo = new Paragraph();
            titulo.Add(Ctte.chunkLeyenda);
            titulo.Alignment = Element.ALIGN_CENTER;
            document.Add(titulo);

            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(NombreArchivo, FileMode.Create));
            //wri.PageEvent = new HeaderFooter();
            //wri.PageEvent.OnStartPage(wri, document);


            document.Add(new Paragraph("  "));
            Paragraph infoinforme = new Paragraph("Fecha: " + DateTime.Today.ToString("dd/MM/yyyy") + "\n Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
            infoinforme.Alignment = Element.ALIGN_RIGHT;
            document.Add(new Paragraph(infoinforme));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            //declaracion de tabla para volcar datos de descargas
            iTextSharp.text.Rectangle page = document.PageSize;
            //PdfPTable table = new PdfPTable(15);
         
            float[] widths = new float[0];
            //asigno el ancho de las columnas
            if (LabelLeyenda.Text == "Indicados para NO imprimir")
            {
                Ctte.table = new PdfPTable(17);
                Ctte.table.WidthPercentage = 180;
                Ctte.table.TotalWidth = page.Width - 90;
                Ctte.table.LockedWidth = true;
                widths = new float[] { 0.6f, 0.9f, 2.0f, 1.2f, 2.0f, 1.0f, 1.0f, 1.2f, 1.0f, 0.6f, 0.6f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 1.0f};
            }
            else if (LabelLeyenda.Text == "Leidas NO impresas" || LabelLeyenda.Text == "Todos")
            {
                Ctte.table = new PdfPTable(20);
                Ctte.table.WidthPercentage = 180;
                Ctte.table.TotalWidth = page.Width - 10;
                Ctte.table.LockedWidth = true;
                                     //IC   Med  Dom  EstAnt EstAct Cons  Fech HLec  HIm  Cond   Situ  TipLec  Ope   1    2     3    4     5    Obser  Iny
                widths = new float[] {0.9f, 0.8f, 2.0f, 0.65f, 0.65f, 0.8f, 0.8f, 0.5f, 0.5f, 1.1f, 1.1f, 0.7f, 0.4f, 0.3f, 0.3f, 0.3f, 0.3f, 0.3f, 0.8f, 0.5f };
            }
            else if (LabelLeyenda.Text == "Errores")
            {
                Ctte.table = new PdfPTable(8);
                Ctte.table.WidthPercentage = 180;
                Ctte.table.TotalWidth = page.Width - 90;
                Ctte.table.LockedWidth = true;
                widths = new float[] { 0.7f, 0.6f, 1.0f, 0.6f, 0.8f, 0.8f, 1.0f, 3.5f };
            }
            else if (LabelLeyenda.Text == "DispSaldos")
            {
                Ctte.table = new PdfPTable(9);
                Ctte.table.WidthPercentage = 180;
                Ctte.table.TotalWidth = page.Width - 90;
                Ctte.table.LockedWidth = true;
                widths = new float[] { 0.7f, 0.9f, 0.7f, 0.7f, 2.5f, 0.7f, 2.5f, 0.9f, 0.9f };
            }
            else
            {
                Ctte.table = new PdfPTable(16);
                Ctte.table.WidthPercentage = 180;
                Ctte.table.TotalWidth = page.Width - 90;
                Ctte.table.LockedWidth = true;
                widths = new float[] { 0.5f, 0.8f, 0.8f, 2.0f, 0.8f, 0.8f, 1.0f, 1.0f, 0.6f, 1.2f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 1.0f };
            }
            Ctte.table.SetWidths(widths);


            ////Estructura de tabla:
            ////Periodo|Ruta|Fecha|Hora|Modelo|Numero|Estado|Domicilio|Observaciones|Lecturista

            if (LabelLeyenda.Text == "Errores")
            {
                PdfPCell FechaErr = (new PdfPCell(new Paragraph("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                FechaErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(FechaErr);
                PdfPCell HoraErr = (new PdfPCell(new Phrase("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                HoraErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(HoraErr);
                PdfPCell EquipoErr = (new PdfPCell(new Phrase("Equipo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                EquipoErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(EquipoErr);
                PdfPCell RutaErr = (new PdfPCell(new Phrase("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                RutaErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(RutaErr);
                PdfPCell NInstalacion = (new PdfPCell(new Phrase("Usuario", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                NInstalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(NInstalacion);
                PdfPCell LectErr = (new PdfPCell(new Phrase("Lecturista", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                LectErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(LectErr);
                PdfPCell CodigoError = (new PdfPCell(new Phrase("CodigoError", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                CodigoError.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(CodigoError);
                PdfPCell TextoError = (new PdfPCell(new Phrase("TextoError", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                TextoError.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(TextoError);
            }
            else if (LabelLeyenda.Text == "DispSaldos")
            {
                PdfPCell Periodo = (new PdfPCell(new Paragraph("Periodo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Periodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Periodo);
                PdfPCell Instalacion = (new PdfPCell(new Phrase("Instalacíón", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Instalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Instalacion);
                PdfPCell Contrato = (new PdfPCell(new Phrase("Contrato", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Contrato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Contrato);
                PdfPCell IC = (new PdfPCell(new Phrase("IC", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                IC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(IC);
                PdfPCell Apellido = (new PdfPCell(new Phrase("Apellido", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Apellido.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Apellido);
                PdfPCell Medidor = (new PdfPCell(new Phrase("Medidor", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Medidor.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Medidor);
                PdfPCell Dom = (new PdfPCell(new Phrase("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Dom.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Dom);
                PdfPCell AnteriorEstado = (new PdfPCell(new Phrase("AnteriorEstado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                AnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(AnteriorEstado);
                PdfPCell SituacionAct = (new PdfPCell(new Phrase("Situación Actual", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                SituacionAct.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(SituacionAct);
            }
            else
            {
                ///Se quita la columna Ruta porque se informara solo en el cabezal de cada Hoja ya que seria un dato iterativo
                //PdfPCell Ruta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                //Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(Ruta);
                PdfPCell Instalación = (new PdfPCell(new Phrase("IC ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Instalación.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Instalación);
                if (LabelLeyenda.Text == "Indicados para NO imprimir")
                {
                    PdfPCell Titular = (new PdfPCell(new Paragraph("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    Titular.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(Titular);
                }
                PdfPCell Numero = (new PdfPCell(new Paragraph("NºMed", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Numero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Numero);
                PdfPCell Dom = (new PdfPCell(new Paragraph("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Dom.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Dom);
                PdfPCell AnteriorEstado = (new PdfPCell(new Paragraph("Estado ANT", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                AnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(AnteriorEstado);
                PdfPCell ActualEstado = (new PdfPCell(new Phrase("Estado ACT", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(ActualEstado);
                PdfPCell ConsumoFacturado = (new PdfPCell(new Phrase("Consumo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ConsumoFacturado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(ConsumoFacturado);
                PdfPCell Fecha = (new PdfPCell(new Phrase("Fecha", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Fecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Fecha);
                PdfPCell Hora = (new PdfPCell(new Phrase("HoraLect", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Hora);
                if (LabelLeyenda.Text == "Leidas NO impresas" || LabelLeyenda.Text == "Todos")
                {
                    PdfPCell HoraImpr = (new PdfPCell(new Phrase("HoraImp", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    HoraImpr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(HoraImpr);
                    PdfPCell Condicion = (new PdfPCell(new Phrase("Condición", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    Condicion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(Condicion);
                    PdfPCell MotivoNoImprimir = (new PdfPCell(new Phrase("Situación", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    MotivoNoImprimir.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(MotivoNoImprimir);
                    PdfPCell TipoLectura = (new PdfPCell(new Phrase("TipLec", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    TipoLectura.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(TipoLectura);
                }
                PdfPCell Operario = (new PdfPCell(new Phrase("Op. ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Operario.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Operario);
                PdfPCell Ord1 = (new PdfPCell(new Phrase("1", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Ord1);
                PdfPCell Ord2 = (new PdfPCell(new Phrase("2", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Ord2);
                PdfPCell Ord3 = (new PdfPCell(new Phrase("3", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Ord3);
                PdfPCell Ord4 = (new PdfPCell(new Phrase("4", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Ord4);
                PdfPCell Ord5 = (new PdfPCell(new Phrase("5", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Ord5);
                PdfPCell Observ = (new PdfPCell(new Phrase("Observ ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Observ.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Ctte.table.AddCell(Observ);
                if (LabelLeyenda.Text == "Leidas NO impresas" || LabelLeyenda.Text == "Todos")
                {
                    PdfPCell Iny = (new PdfPCell(new Phrase("INY ", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    Iny.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(Iny);
                }
            }


            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            foreach (DataGridViewRow fi in grd.Rows)
            {
                if (LabelLeyenda.Text == "Errores")
                {
                    PdfPCell fi201 = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi201.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi201);
                    PdfPCell fi202 = (new PdfPCell(new Paragraph(fi.Cells["Hora"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi202.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi202);
                    PdfPCell fi203 = (new PdfPCell(new Paragraph(fi.Cells["Equipo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi203.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi203);
                    PdfPCell fi204 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi204.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi204);
                    PdfPCell fi205 = (new PdfPCell(new Paragraph(fi.Cells["NUsuario"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi205.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi205);
                    PdfPCell fi206 = (new PdfPCell(new Paragraph(fi.Cells["Lecturista"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi206.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi206);
                    PdfPCell fi207 = (new PdfPCell(new Paragraph(fi.Cells["CodigoError"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi207.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi207);
                    PdfPCell fi208 = (new PdfPCell(new Paragraph(fi.Cells["TextoError"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi208.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi208);
                }
                else if (LabelLeyenda.Text == "DispSaldos")
                {
                    PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Periodo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi4);
                    //PdfPCell fi5 = (new PdfPCell(new Paragraph(fi.Cells["Modelo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    //fi5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //table.AddCell(fi5);
                    PdfPCell fi6 = (new PdfPCell(new Paragraph(fi.Cells["NInstalacion"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi6);    
                    PdfPCell fi8 = (new PdfPCell(new Paragraph(fi.Cells["Contrato"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi8);
                    PdfPCell fiDomicilio = (new PdfPCell(new Paragraph(fi.Cells["IC"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fiDomicilio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fiDomicilio);
                    PdfPCell fi9 = (new PdfPCell(new Paragraph(fi.Cells["Apellido"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi9);
                    PdfPCell fi10 = (new PdfPCell(new Paragraph(fi.Cells["Medidor"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi10);
                    PdfPCell fi1010 = (new PdfPCell(new Paragraph(fi.Cells["Domicilio"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi1010.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi1010);
                    PdfPCell fi111 = (new PdfPCell(new Paragraph(fi.Cells["AnteriorEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi111.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi111);
                    PdfPCell fi102 = (new PdfPCell(new Paragraph(fi.Cells["Situacion Actual"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi102.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi102);     
                }
                else
                {
                    ///Se quita la columna Ruta porque se informara solo en el cabezal de cada Hoja ya que seria un dato iterativo
                    //PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    //fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //table.AddCell(fi4);
                    //PdfPCell fi5 = (new PdfPCell(new Paragraph(fi.Cells["Modelo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    //fi5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //table.AddCell(fi5);
                    PdfPCell fi6 = (new PdfPCell(new Paragraph(fi.Cells["IC"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi6);
                    if (LabelLeyenda.Text == "Indicados para NO imprimir")
                    {
                        PdfPCell fi7 = (new PdfPCell(new Paragraph(fi.Cells["Domicilio"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                        fi7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Ctte.table.AddCell(fi7);
                    }
                    PdfPCell fi8 = (new PdfPCell(new Paragraph(fi.Cells["Medidor"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi8);
                    PdfPCell fiDomicilio = (new PdfPCell(new Paragraph(fi.Cells["Domicilio"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fiDomicilio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fiDomicilio);
                    PdfPCell fi9 = (new PdfPCell(new Paragraph(fi.Cells["AnteriorEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi9);
                    PdfPCell fi10 = (new PdfPCell(new Paragraph(fi.Cells["ActualEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi10);
                    PdfPCell fi1010 = (new PdfPCell(new Paragraph(fi.Cells["ConsumoFacturado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi1010.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi1010);
                    PdfPCell fi111 = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));
                    fi111.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi111);
                    PdfPCell fi102 = (new PdfPCell(new Paragraph(fi.Cells["HoraLect"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi102.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi102);
                    if (LabelLeyenda.Text == "Leidas NO impresas" || LabelLeyenda.Text == "Todos")
                    {
                        PdfPCell fHoraImp = (new PdfPCell(new Paragraph(fi.Cells["HoraImp"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                        fHoraImp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Ctte.table.AddCell(fHoraImp);
                        ///Comento esta parte porque se encargaba de colorear el informe en pantalla cuando la leyenda del campo Situacion era "Contador Facturado > al ingresado"
                        //if (fi.Cells["Situacion"].Value.ToString() == "Estado Contador < Facturado")
                        //{
                        //    PdfPCell FMotivo = (new PdfPCell(new Paragraph(fi.Cells["Situacion"].Value.ToString(), FontFactory.GetFont("Arial", 7, iTextSharp.text.BaseColor.RED))));
                        //    FMotivo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //    table.AddCell(FMotivo);
                        //}
                        //else
                        //{
                        PdfPCell Condicion = (new PdfPCell(new Paragraph(fi.Cells["Condición"].Value.ToString(), FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL))));
                        Condicion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Ctte.table.AddCell(Condicion);
                        PdfPCell FMotivo = (new PdfPCell(new Paragraph(fi.Cells["Situación"].Value.ToString(), FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL))));
                        FMotivo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Ctte.table.AddCell(FMotivo);                      
                        //}                                   
                        PdfPCell TipoLectura = (new PdfPCell(new Paragraph(fi.Cells["Tipo_Lectura"].Value.ToString(), FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL))));
                        TipoLectura.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Ctte.table.AddCell(TipoLectura);
                    }
                    PdfPCell fi11 = (new PdfPCell(new Paragraph(fi.Cells["Operario"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi11);
                    ///Novedades del 1 al 5 seguido del campo observaciones
                    PdfPCell fi12 = (new PdfPCell(new Paragraph(fi.Cells["Ord1"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi12);
                    PdfPCell fi13 = (new PdfPCell(new Paragraph(fi.Cells["Ord2"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi13);
                    PdfPCell fi14 = (new PdfPCell(new Paragraph(fi.Cells["Ord3"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi14.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi14);
                    PdfPCell fi15 = (new PdfPCell(new Paragraph(fi.Cells["Ord4"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi15.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi15);
                    PdfPCell fi16 = (new PdfPCell(new Paragraph(fi.Cells["Ord5"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi16.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi16);
                    PdfPCell fi17 = (new PdfPCell(new Paragraph(fi.Cells["Observaciones"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi17.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    Ctte.table.AddCell(fi17);
                    if (LabelLeyenda.Text == "Leidas NO impresas" || LabelLeyenda.Text == "Todos")
                    {
                        PdfPCell fi18 = (new PdfPCell(new Paragraph(fi.Cells["Inyeccion"].Value.ToString(), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));
                        fi18.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        Ctte.table.AddCell(fi18);
                    }
                }
                //for (int i = 0; i < ArrayCodNovedades.Length; i++)
                //{
                //    ArrayCodNovedades[i] = "";
                //}
                //Observaciones = "";

            }
                      
            document.Add(new Paragraph(" "));
           
            document.Add(Ctte.table);
            document.Add(new Paragraph(" "));


            Chunk CantidadRegistros = new Chunk("Total = " + grd.Rows.Count.ToString(),
                                                FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD,
                                                new iTextSharp.text.BaseColor(0, 0, 0)));
            Paragraph Total = new Paragraph(CantidadRegistros);

            Total.Alignment = Element.ALIGN_RIGHT;

            ///Obtengo las altas asociada a la ruta si existen
            AltasDeRuta();



            if (LabelLeyenda.Text == "Todos")
            {

                //Agregmos los encabezados del List View Resumen
                foreach (ColumnHeader column in LVResumenGral.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.Text));
                    pdfTable.AddCell(cell);
                }
                //Agregamos las lineas de datos del List View Resumen              
                foreach (ListViewItem itemRow in LVResumenGral.Items)
                {
                    int i = 0;
                    for (i = 0; i < itemRow.SubItems.Count; i++)
                    {
                        //Thread.Sleep(100);
                        string valor = itemRow.SubItems[i].Text;
                        PdfPCell valorcelda = (new PdfPCell(new Phrase(valor)));
                        pdfTable.AddCell(valorcelda);
                    }
                }


                PdfPCell ColumRuta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumRuta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumRuta);
                PdfPCell ColumFecha = (new PdfPCell(new Paragraph("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumFecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumFecha);
                PdfPCell ColumHora = (new PdfPCell(new Paragraph("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumHora.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumHora);
                PdfPCell ColumNumero = (new PdfPCell(new Paragraph("Numero", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumNumero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumNumero);
                PdfPCell ColumEstado = (new PdfPCell(new Paragraph("Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumEstado);
                PdfPCell ColumDomicilio = (new PdfPCell(new Paragraph("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumDomicilio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumDomicilio);
                PdfPCell ColumObservaciones = (new PdfPCell(new Paragraph("Observaciones", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumObservaciones.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumObservaciones);
                PdfPCell ColumLecturista = (new PdfPCell(new Paragraph("Lecturista", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumLecturista.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(ColumLecturista);
                PdfPCell LatitudAlta = (new PdfPCell(new Paragraph("Latitud", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                LatitudAlta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(LatitudAlta);
                PdfPCell LongitudAlta = (new PdfPCell(new Paragraph("Longitud", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                LongitudAlta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableAltas.AddCell(LongitudAlta);


                //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
                foreach (DataGridViewRow fiAltas in dataGridViewAltas.Rows)
                {

                    if (fiAltas.Cells["Ruta"].Value != null)
                    {
                        string Ruta = fiAltas.Cells["Ruta"].Value.ToString();
                        PdfPCell Ru = (new PdfPCell(new Phrase(Ruta)));
                        pdfTableAltas.AddCell(Ru);
                    }

                    if (fiAltas.Cells["Fecha"].Value != null)
                    {
                        string Fecha = Convert.ToDateTime(fiAltas.Cells["Fecha"].Value).ToString("dd/MM/yyyy");
                        PdfPCell Fec = (new PdfPCell(new Phrase(Fecha, FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                        pdfTableAltas.AddCell(Fec);
                    }

                    if (fiAltas.Cells["Hora"].Value != null)
                    {
                        string Hora = fiAltas.Cells["Hora"].Value.ToString();
                        PdfPCell Hor = (new PdfPCell(new Phrase(Hora)));
                        pdfTableAltas.AddCell(Hor);
                    }

                    if (fiAltas.Cells["Numero"].Value != null)
                    {
                        string Numero = fiAltas.Cells["Numero"].Value.ToString();
                        PdfPCell Num = (new PdfPCell(new Phrase(Numero)));
                        pdfTableAltas.AddCell(Num);
                    }
                    if (fiAltas.Cells["Estado"].Value != null)
                    {
                        string Estado = fiAltas.Cells["Estado"].Value.ToString();
                        PdfPCell Est = (new PdfPCell(new Phrase(Estado)));
                        pdfTableAltas.AddCell(Est);
                    }

                    if (fiAltas.Cells["Domicilio"].Value != null)
                    {
                        string Domi = fiAltas.Cells["Domicilio"].Value.ToString();
                        PdfPCell Dom = (new PdfPCell(new Phrase(Domi)));
                        pdfTableAltas.AddCell(Dom);
                    }

                    if (fiAltas.Cells["Observaciones"].Value != null)
                    {
                        string Observaciones = fiAltas.Cells["Observaciones"].Value.ToString();
                        PdfPCell Observ = (new PdfPCell(new Phrase(Observaciones)));
                        pdfTableAltas.AddCell(Observ);
                    }

                    if (fiAltas.Cells["Operario"].Value != null)
                    {
                        string Lecturista = fiAltas.Cells["Operario"].Value.ToString();
                        PdfPCell lect = (new PdfPCell(new Phrase(Lecturista)));
                        pdfTableAltas.AddCell(lect);
                    }
                    if (fiAltas.Cells["Latitud"].Value != null)
                    {
                        string Latitud = fiAltas.Cells["Latitud"].Value.ToString();
                        PdfPCell lat = (new PdfPCell(new Phrase(Latitud, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));
                        pdfTableAltas.AddCell(lat);
                    }
                    if (fiAltas.Cells["Longitud"].Value != null)
                    {
                        string Longitud = fiAltas.Cells["Longitud"].Value.ToString();
                        PdfPCell longit = (new PdfPCell(new Phrase(Longitud, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));
                        pdfTableAltas.AddCell(longit);
                    }
                }


                ///Obtengo las modificaciones asociada a la ruta si existen
                ModificacionesDeRuta();
                //Agrego las columnas para la tablita de Modificaciones si existen
                pdfTableModifica.AddCell(ColumRuta);
                pdfTableModifica.AddCell(ColumFecha);
                pdfTableModifica.AddCell(ColumHora);
                PdfPCell ColumnInstalacion = (new PdfPCell(new Paragraph("Interlocutor", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumnInstalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableModifica.AddCell(ColumnInstalacion);
                pdfTableModifica.AddCell(ColumNumero);
                PdfPCell ColumDigitos = (new PdfPCell(new Paragraph("Digitos", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumDigitos.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableModifica.AddCell(ColumDigitos);
                PdfPCell ColumFacturado = (new PdfPCell(new Paragraph("FactorMult", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ColumFacturado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTableModifica.AddCell(ColumFacturado);
                pdfTableModifica.AddCell(ColumDomicilio);
                pdfTableModifica.AddCell(ColumObservaciones);
                pdfTableModifica.AddCell(ColumLecturista);
                //pdfTableModifica.AddCell(LatitudAlta);
                //pdfTableModifica.AddCell(LongitudAlta);





                foreach (DataRow fiModificaciones in TablaModificaciones.Rows)
                {
                    if (fiModificaciones["Ruta"].ToString() != null)
                    {
                        string Ruta = fiModificaciones["Ruta"].ToString();
                        PdfPCell Ru = (new PdfPCell(new Phrase(Ruta)));
                        pdfTableModifica.AddCell(Ru);
                    }
                    if (fiModificaciones["Fecha"].ToString() != null)
                    {
                        string Fecha = fiModificaciones["Fecha"].ToString();
                        PdfPCell Fec = (new PdfPCell(new Phrase(Fecha)));
                        pdfTableModifica.AddCell(Fec);
                    }
                    if (fiModificaciones["Hora"].ToString() != null)
                    {
                        string Hora = fiModificaciones["Hora"].ToString();
                        PdfPCell Hor = (new PdfPCell(new Phrase(Hora)));
                        pdfTableModifica.AddCell(Hor);
                    }
                    if (fiModificaciones["titularID"].ToString() != null)
                    {
                        string ConexID = fiModificaciones["titularID"].ToString();
                        PdfPCell Con = (new PdfPCell(new Phrase(ConexID)));
                        pdfTableModifica.AddCell(Con);
                    }
                    if (fiModificaciones["Numero"].ToString() != null)
                    {
                        string Numero = fiModificaciones["Numero"].ToString();
                        PdfPCell Num = (new PdfPCell(new Phrase(Numero)));
                        pdfTableModifica.AddCell(Num);
                    }
                    if (fiModificaciones["Digitos"].ToString() != null)
                    {
                        string Digitos = fiModificaciones["Digitos"].ToString();
                        PdfPCell Dig = (new PdfPCell(new Phrase(Digitos)));
                        pdfTableModifica.AddCell(Dig);
                    }
                    if (fiModificaciones["FactorMult"].ToString() != null)
                    {
                        string FactorMult = fiModificaciones["FactorMult"].ToString();
                        PdfPCell Factor = (new PdfPCell(new Phrase(FactorMult)));
                        pdfTableModifica.AddCell(Factor);
                    }
                    if (fiModificaciones["Domicilio"].ToString() != null)
                    {
                        string Domi = fiModificaciones["Domicilio"].ToString();
                        PdfPCell Dom = (new PdfPCell(new Phrase(Domi)));
                        pdfTableModifica.AddCell(Dom);
                    }
                    if (fiModificaciones["Observaciones"].ToString() != null)
                    {
                        string Observaciones = fiModificaciones["Observaciones"].ToString();
                        PdfPCell Observ = (new PdfPCell(new Phrase(Observaciones)));
                        pdfTableModifica.AddCell(Observ);
                    }
                    if (fiModificaciones["Operario"].ToString() != null)
                    {
                        string Lecturista = fiModificaciones["Operario"].ToString();
                        PdfPCell lect = (new PdfPCell(new Phrase(Lecturista)));
                        pdfTableModifica.AddCell(lect);
                    }
                    //if (fiModificaciones["Latitud"].ToString() != null)
                    //{
                    //    string Latitud = fiModificaciones["Latitud"].ToString();
                    //    PdfPCell lat = (new PdfPCell(new Phrase(Latitud)));
                    //    pdfTableModifica.AddCell(lat);
                    //}
                    //if (fiModificaciones["Longitud"].ToString() != null)
                    //{
                    //    string Longitud = fiModificaciones["Longitud"].ToString();
                    //    PdfPCell longit = (new PdfPCell(new Phrase(Longitud)));
                    //    pdfTableModifica.AddCell(longit);
                    //}
                }

            }

            wri.Add(Ctte.imagenMINTELL);
            wri.Add(Ctte.imagenDPEC);
            wri.Add(new Paragraph("  "));
            wri.Add(titulo);
            wri.Add(new Paragraph(""));
            wri.Add(new Paragraph(infoinforme));
            wri.Add(new Paragraph(""));
            //
            
            wri.Add(Ctte.table);
            wri.Add(new Paragraph(""));
            if (LabelLeyenda.Text == "Todos")
            {
                wri.Add(pdfTable);
                wri.Add(pdfTableAltas);
                wri.Add(pdfTableModifica);
            }
            wri.Add(Total);
            document.Add(Total);
            document.Add(new Paragraph(" Resumen cuantitativo por Fecha ", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph(" "));
            document.Add(pdfTable);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("ALTAS ", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph(" "));
            document.Add(pdfTableAltas);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("MODIFICACIONES ", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph(" "));
            document.Add(pdfTableModifica);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Usuarios con Ordenativos", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph(lblTotalUsConOrd.Text, FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Modulos Registrados", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph(lblTotModRel.Text, FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD)));
            document.Close();

        }


        /// <summary>
        /// Consulta si existen Altas de la ruta que se va a generar el PDF para agregar debajo del informe
        /// de "TODOS" con el siguiente formato.
        /// Ruta | Fecha | Hora | Numero | Estado | Domicilio | Observaciones | Lecturista
        /// </summary>
        private void AltasDeRuta()
        {

            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL = "";

            try
            {
                //Numero <> 'CxDir' AND
                //Lee la tabla ALTAS pertenecientes al periodo
                txSQL = "SELECT Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                        " WHERE Periodo = " + Vble.Periodo + " AND Ruta = " + TextBoxRuta.Text + " AND ABM = 'A'" +
                        " ORDER BY Fecha ASC, Hora ASC";

                TablaAltas = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaAltas);
                dataGridViewAltas.DataSource = TablaAltas;

                comandoSQL.Dispose();
                datosAdapter.Dispose();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Consulta si existen Modificaciones de la ruta que se va a generar el PDF para agregar debajo del informe
        /// de "TODOS" con el siguiente formato.
        /// Ruta | Fecha | Hora | Numero| Digitos | FactorMult | Domicilio | Observaciones | Lecturista
        /// </summary>
        private void ModificacionesDeRuta()
        {

            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL = "";

            try
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                txSQL = "SELECT A.Ruta, A.Fecha, A.Hora, C.titularID, A.Numero, A.Digitos, A.FactorMult, A.Domicilio, A.Observaciones, A.Operario, A.Latitud, A.Longitud" +
                        " FROM Altas A" +
                        " INNER JOIN Conexiones C" +
                        " USING (ConexionID, Periodo) " +
                        " WHERE Periodo = " + Vble.Periodo + " AND A.Numero <> 'CxDir' AND A.Ruta = " + TextBoxRuta.Text + " AND A.ABM = 'M'" +
                        " ORDER BY A.Fecha ASC, A.Hora ASC";

                TablaModificaciones = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaModificaciones);

                comandoSQL.Dispose();
                datosAdapter.Dispose();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Se exporta la tabla en vista a un archivo pdf en la direccion que se especifica cuando se guarda el archivo.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="NombreArchivo"></param>
        private void ExportarAltasPDF(DataGridView grd, bool Domicilio)
        {
            DataTable Tabla = new DataTable();
            Vble.LeerNombresCarpetas();
            //Creo el docuemento .pdf con el formato especificado
            Document document = new Document(PageSize.A4);
            //Gira la hoja en posicion horizontal
            ArrayList Ordenativos = new ArrayList();

            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            //string PathInformesAltas = Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo;
            //string SavePath = "D:\\Informes\\InformeImpreso";
            Vble.NombreArchivoParaImprimirResumen = Vble.ValorarUnNombreRuta(Vble.NombreArchivoParaImprimirResumen);
            if (!Directory.Exists(Vble.NombreArchivoParaImprimirResumen))
            {

                Directory.CreateDirectory(Vble.NombreArchivoParaImprimirResumen);
            }
            Vble.NombreArchivoParaImprimirResumen += "Impresion.pdf";
            string SavePath = Vble.NombreArchivoParaImprimirResumen;


            if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
            {
                Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
            }

            PdfWriter wri = PdfWriter.GetInstance(document, new FileStream(SavePath, FileMode.OpenOrCreate));
            //PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
            wri.PageEvent = new PageEventHelper();
            wri.Open();
            document.Open();

            // ********************Creamos la imagen de DPEC y le ajustamos el tamaño
            iTextSharp.text.Image imagenDPEC = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
            imagenDPEC.BorderWidth = 0;
            //imagenDPEC.Alignment = Element.ALIGN_RIGHT;

            //imagenDPEC.SetAbsolutePosition(40f, 790f);  posicion de imagen para hoja vertical
            imagenDPEC.SetAbsolutePosition(40f, 510f);//  posicion de imagen para hoja horizontal
            float percentage1 = 0.0f;                 //  
            percentage1 = 70 / imagenDPEC.Width;      //  Edito tamaño de imagen
            imagenDPEC.ScalePercent(percentage1 * 100);//

            //*******************Creamos la imagen de MacroIntell y le ajustamos el tamaño
            iTextSharp.text.Image imagenMINTELL = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\MacroIntell Isologo.jpg");
            imagenMINTELL.BorderWidth = 0;
            //imagenMINTELL.Alignment = Element.ALIGN_LEFT;
            //imagenMINTELL.SetAbsolutePosition(500f, 790f);  posicion de imagen para hoja vertical
            imagenMINTELL.SetAbsolutePosition(750f, 530f);// posicion de imagen para hoja horizontal
            float percentage2 = 0.0f;                     //
            percentage2 = 50 / imagenMINTELL.Width;       //edito Tamaño de imagen
            imagenMINTELL.ScalePercent(percentage2 * 100);//
                                                          //*************************************************************************************************************************

            //datos del informe                       
            document.Add(imagenMINTELL);
            document.Add(imagenDPEC);
            document.Add(new Paragraph("  "));
            Chunk chunk = new Chunk("         Informe " + LabelLeyenda.Text + " \n\n         Periodo: " + Vble.Periodo,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            //chunk.SetUnderline(0.9f, -1.8f);
            Paragraph titulo = new Paragraph();
            titulo.Add(chunk);
            titulo.Alignment = Element.ALIGN_CENTER;
            document.Add(new Paragraph(titulo));


            Paragraph infoinforme = new Paragraph("Fecha: " + DateTime.Today.ToString("dd/MM/yyyy") + "\n Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
            infoinforme.Alignment = Element.ALIGN_RIGHT;
            document.Add(new Paragraph(infoinforme));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            //declaracion de tabla para volcar datos de descargas
            iTextSharp.text.Rectangle page = document.PageSize;
            PdfPTable table = new PdfPTable(15);
            float[] widths = new float[0];
            //asigno el ancho de las columnas
            if (LabelLeyenda.Text == "Indicados para NO imprimir")
            {
                table = new PdfPTable(16);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                widths = new float[] { 0.5f, 0.8f, 1.5f, 0.8f, 1.0f, 1.0f, 0.9f, 1.0f, 0.6f, 0.6f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 1.0f };
            }
            else if (LabelLeyenda.Text == "Leidas NO impresas")
            {
                table = new PdfPTable(16);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                widths = new float[] { 0.5f, 0.8f, 0.8f, 0.6f, 0.8f, 0.9f, 1.0f, 0.5f, 1.8f, 0.8f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 1.0f };
            }
            else if (LabelLeyenda.Text == "Errores")
            {
                table = new PdfPTable(7);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                widths = new float[] { 0.7f, 0.6f, 1.0f, 0.6f, 0.8f, 1.0f, 3.5f };
            }
            else
            {
                table = new PdfPTable(15);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                widths = new float[] { 0.5f, 0.8f, 0.8f, 1.0f, 0.8f, 0.9f, 1.0f, 0.6f, 1.2f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 1.0f };
            }
            table.SetWidths(widths);


            ////Estructura de tabla:
            ////Periodo|Ruta|Fecha|Hora|Modelo|Numero|Estado|Domicilio|Observaciones|Lecturista
            if (LabelLeyenda.Text == "Errores")
            {
                PdfPCell FechaErr = (new PdfPCell(new Paragraph("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                FechaErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(FechaErr);
                PdfPCell HoraErr = (new PdfPCell(new Phrase("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                HoraErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(HoraErr);
                PdfPCell EquipoErr = (new PdfPCell(new Phrase("Equipo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                EquipoErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(EquipoErr);
                PdfPCell RutaErr = (new PdfPCell(new Phrase("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                RutaErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(RutaErr);
                PdfPCell NInstalacion = (new PdfPCell(new Phrase("Usuario", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                NInstalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(NInstalacion);
                PdfPCell LectErr = (new PdfPCell(new Phrase("Lecturista", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                LectErr.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(LectErr);
                PdfPCell CodigoError = (new PdfPCell(new Phrase("CodigoError", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                CodigoError.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(CodigoError);
                PdfPCell TextoError = (new PdfPCell(new Phrase("TextoError", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                TextoError.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(TextoError);
            }
            else
            {
                PdfPCell Ruta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Ruta);
                PdfPCell Instalación = (new PdfPCell(new Phrase("IC ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Instalación.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Instalación);
                if (LabelLeyenda.Text == "Indicados para NO imprimir")
                {
                    PdfPCell Titular = (new PdfPCell(new Paragraph("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    Titular.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(Titular);
                }
                PdfPCell Numero = (new PdfPCell(new Paragraph("Medidor", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Numero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Numero);
                PdfPCell AnteriorEstado = (new PdfPCell(new Paragraph("Anterior Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                AnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(AnteriorEstado);
                PdfPCell ActualEstado = (new PdfPCell(new Phrase("Actual Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(ActualEstado);
                PdfPCell ConsumoFacturado = (new PdfPCell(new Phrase("Consumo Facturado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ConsumoFacturado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(ConsumoFacturado);
                PdfPCell Fecha = (new PdfPCell(new Phrase("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Fecha);
                PdfPCell Hora = (new PdfPCell(new Phrase("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Hora);
                if (LabelLeyenda.Text == "Leidas NO impresas")
                {
                    PdfPCell MotivoNoImprimir = (new PdfPCell(new Phrase("Motivo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                    MotivoNoImprimir.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(MotivoNoImprimir);
                }
                PdfPCell Operario = (new PdfPCell(new Phrase("Operario ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Operario.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Operario);


                PdfPCell Ord1 = (new PdfPCell(new Phrase("Ord1", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Ord1);
                PdfPCell Ord2 = (new PdfPCell(new Phrase("Ord2", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Ord2);
                PdfPCell Ord3 = (new PdfPCell(new Phrase("Ord3", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Ord3);
                PdfPCell Ord4 = (new PdfPCell(new Phrase("Ord4", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Ord4);
                PdfPCell Ord5 = (new PdfPCell(new Phrase("Ord5", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Ord5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Ord5);
                PdfPCell Observ = (new PdfPCell(new Phrase("Observ ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
                Observ.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(Observ);
            }


            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            foreach (DataGridViewRow fi in grd.Rows)
            {
                if (LabelLeyenda.Text == "Errores")
                {
                    PdfPCell fi201 = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi201.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi201);
                    PdfPCell fi202 = (new PdfPCell(new Paragraph(fi.Cells["Hora"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi202.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi202);
                    PdfPCell fi203 = (new PdfPCell(new Paragraph(fi.Cells["Equipo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi203.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi203);
                    PdfPCell fi204 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi204.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi204);
                    PdfPCell fi205 = (new PdfPCell(new Paragraph(fi.Cells["NUsuario"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi205.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi205);
                    PdfPCell fi206 = (new PdfPCell(new Paragraph(fi.Cells["Lecturista"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi206.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi206);
                    PdfPCell fi207 = (new PdfPCell(new Paragraph(fi.Cells["CodigoError"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi207.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi207);
                    PdfPCell fi208 = (new PdfPCell(new Paragraph(fi.Cells["TextoError"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi208.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi208);
                }
                else
                {

                    PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi4);
                    //PdfPCell fi5 = (new PdfPCell(new Paragraph(fi.Cells["Modelo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    //fi5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //table.AddCell(fi5);
                    PdfPCell fi6 = (new PdfPCell(new Paragraph(fi.Cells["IC"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi6);
                    if (LabelLeyenda.Text == "Indicados para NO imprimir")
                    {
                        PdfPCell fi7 = (new PdfPCell(new Paragraph(fi.Cells["Domicilio"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                        fi7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table.AddCell(fi7);
                    }

                    PdfPCell fi8 = (new PdfPCell(new Paragraph(fi.Cells["Medidor"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi8);
                    PdfPCell fi9 = (new PdfPCell(new Paragraph(fi.Cells["AnteriorEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi9);
                    PdfPCell fi10 = (new PdfPCell(new Paragraph(fi.Cells["ActualEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi10);
                    PdfPCell fi1010 = (new PdfPCell(new Paragraph(fi.Cells["ConsumoFacturado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi1010.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi1010);
                    PdfPCell fi111 = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi111.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi111);
                    PdfPCell fi102 = (new PdfPCell(new Paragraph(fi.Cells["Hora"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi102.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi102);
                    if (LabelLeyenda.Text == "Leidas NO impresas")
                    {
                        PdfPCell FMotivo = (new PdfPCell(new Paragraph(fi.Cells["Situacion"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                        FMotivo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table.AddCell(FMotivo);
                    }

                    PdfPCell fi11 = (new PdfPCell(new Paragraph(fi.Cells["Operario"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi11);



                    PdfPCell fi12 = (new PdfPCell(new Paragraph(fi.Cells["Ord1"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi12);
                    PdfPCell fi13 = (new PdfPCell(new Paragraph(fi.Cells["Ord2"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi13);
                    PdfPCell fi14 = (new PdfPCell(new Paragraph(fi.Cells["Ord3"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi14.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi14);
                    PdfPCell fi15 = (new PdfPCell(new Paragraph(fi.Cells["Ord4"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi15.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi15);
                    PdfPCell fi16 = (new PdfPCell(new Paragraph(fi.Cells["Ord5"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi16.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi16);
                    PdfPCell fi17 = (new PdfPCell(new Paragraph(fi.Cells["Observaciones"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                    fi17.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(fi17);
                }
                //for (int i = 0; i < ArrayCodNovedades.Length; i++)
                //{
                //    ArrayCodNovedades[i] = "";
                //}
                //Observaciones = "";

            }

            document.Add(new Paragraph(" "));
            document.Add(table);

            Chunk CantidadRegistros = new Chunk("Total = " + grd.Rows.Count.ToString(),
                                                FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD,
                                                new iTextSharp.text.BaseColor(0, 0, 0)));
            Paragraph Total = new Paragraph(CantidadRegistros);
            Total.Alignment = Element.ALIGN_RIGHT;


            wri.Add(imagenMINTELL);
            wri.Add(imagenDPEC);
            wri.Add(new Paragraph("  "));
            wri.Add(titulo);
            wri.Add(new Paragraph(""));
            wri.Add(new Paragraph(infoinforme));
            wri.Add(new Paragraph(""));
            wri.Add(table);
            wri.Add(Total);
            //wri.Close();
            document.Add(Total);

            document.Add(new Paragraph(" "));


            document.Close();
            //Thread.Sleep(10000);
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            doc.LoadFromFile(Vble.NombreArchivoParaImprimirResumen);
            doc.PrintSettings.DocumentName = doc.PrintSettings.PrinterName;
            doc.Print();


            if (File.Exists(Vble.NombreArchivoParaImprimirResumen))
            {
                File.Delete(Vble.NombreArchivoParaImprimirResumen);
            }

        }

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
                cboPeriodoHistorial.Items.Add((Anio - 1).ToString("0000") + "-06");

            else
                cboPeriodoHistorial.Items.Add(Anio.ToString("0000") + "-" +
                    (Per - 1).ToString("00"));
            FormAltas.CBPerDesdeAltas.Items.Add((Anio - 1).ToString("0000") + "-06");

            //Actual
            cboPeriodoHistorial.Items.Add(Anio.ToString("0000") + "-" +
                    Per.ToString("00"));


            //Siguiente
            if (Per == 6)
                cboPeriodoHistorial.Items.Add((Anio + 1).ToString("0000") + "-01");
            else
                cboPeriodoHistorial.Items.Add(Anio.ToString("0000") + "-" +
                    (Per + 1).ToString("00"));

            //Si no está el por defecto lo agrega
            Inis.GetPrivateProfileString("Datos", "Periodo", cboPeriodoHistorial.Items[0].ToString(), PerDef, 8, Ctte.ArchivoIniName);
            if (!cboPeriodoHistorial.Items.Contains(PerDef.ToString()))
                cboPeriodoHistorial.Items.Add(PerDef.ToString());

            //Defecto
            cboPeriodoHistorial.Text = PerDef.ToString();
            cboPeriodoHistorial.Items.Add("2015-03");
            //cboPeriodo.Items.Add("2017-05");
            //cboPeriodo.Items.Add("2017-06");

            //Actualiza el periodo indicado en la barra de menús principal           
            //((Form0)this.MdiParent).mnuPeriodoActual.Text = cboPeriodoHistorial.Text;

        }

        /// <summary>
        /// Carga el datagridview para ver el detalle segun el estado de ImpresionOBS
        /// </summary>
        public void CargarTablaPreDescarga()
        {
            try
            {

           
            string txSQL;
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter();
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder();
            MySqlDataAdapter datosAdapterLecturistas = new MySqlDataAdapter();
            MySqlCommandBuilder comandoSQLLecturistas = new MySqlCommandBuilder();
            decimal porcsaldo = 0;
            


            this.DGResumenExp.DataSource = "";
            this.DGResumenExp.Columns.Clear();
            lblTotalUsConOrd.Text = "";
            Vble.contOrdenativos = 0;
            Vble.lecturistas.Clear();
            Vble.lectOrd.Clear();


            Tabla.Clear();
            Tabla.Dispose();

           
            Vble.TablaLecturistas.Dispose();
            Vble.TablaLecturistas.Clear();
            this.DGResumenExp.Refresh();

            if (LabelLeyenda.Text == "LabelErrores")
            {
                txSQL = CONSULTANOIMPRESOS + " AND Periodo = " + Periodo + " ORDER BY Fecha Asc, Hora ASC";
                
                LabelRemesa.Visible = false;
                CBRemesa.Visible = false;
                
            }
            else if (LabelLeyenda.Text == "LabelTodosRem" || LabelLeyenda.Text == "LabImprPreR" || LabelLeyenda.Text == "LabIndNoPrintR" || LabelLeyenda.Text == "LabLeidNoImprePreR" || LabelLeyenda.Text == "lblOrdenativosR" || LabelLeyenda.Text == "LabelTeleLect")
            {
                txSQL = CONSULTANOIMPRESOS;
                
                LabelRemesa.Visible = false;
                CBRemesa.Visible = false;
            }
            else
            {
                //txSQL = CONSULTANOIMPRESOS + " AND C.Periodo = " + Periodo + " ORDER BY Fecha Asc, Hora ASC";
                txSQL = CONSULTANOIMPRESOS + "";
               
                LabelRemesa.Visible = true;
                CBRemesa.Visible = true;
            }

            //MessageBox.Show("Conection String = " + DB.conexBD.ConnectionString);
            //MessageBox.Show("Conexion Time Out = " + DB.conexBD.ConnectionTimeout.ToString());
            
                 //----aca iba codigo consulta


            if (DB.sDbUsu.ToUpper() == "SUPERVISOR")
            {
                ///Consulta para el caso del usuario supervisor que es distinta a la del usuario operario tradicional
                datosAdapterLecturistas = new MySqlDataAdapter(txSQL, DB.conexBD);
                datosAdapterLecturistas.SelectCommand.CommandTimeout = 300;  
                datosAdapterLecturistas.Fill(Vble.TablaLecturistas);
            }
            else
            {
                //--codigo consulta
                txSQL = txSQL == "" ? ultimaConsultaReg : txSQL;
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                datosAdapter.SelectCommand.CommandTimeout = 300;
                //------



                if (txSQL != "")
                { 
                    ///Aca voy a usar la misma consulta para obtener los lecturistas (si hay mas de uno) que operaron sobre la misma ruta
                    string queryLecturistas = "SELECT DISTINCT R.Operario as Lecturistas FROM ( " + txSQL + " ) AS R";
                    datosAdapterLecturistas = new MySqlDataAdapter(queryLecturistas, DB.conexBD);
                    datosAdapterLecturistas.SelectCommand.CommandTimeout = 300;
                    datosAdapterLecturistas.Fill(Vble.TablaLecturistas);
                }
                else
                {

                    //MessageBox.Show("Por favor verifique los datos ingresados", "No incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ///Aca voy a usar la misma consulta para obtener los lecturistas (si hay mas de uno) que operaron sobre la misma ruta
                    //string queryLecturistas = "SELECT DISTINCT R.Operario as Lecturistas FROM ( " + Vble.queryInicialExpor + " ) AS R";
                    //datosAdapterLecturistas = new MySqlDataAdapter(queryLecturistas, DB.conexBD);
                    //datosAdapterLecturistas.SelectCommand.CommandTimeout = 300;
                    //datosAdapterLecturistas.Fill(Vble.TablaLecturistas);
                    Vble.lecturistas.Clear();
                    Vble.lectOrd.Clear();
                    LVResumenGral.Items.Clear();
                    ConsultaPorFechaTomaLect();
                }
               
            }
           

        

            int count = 0;
            bool bandera = false;
           

            if (IndicadorTipoInforme == "ResumenRemesa")
            {


                this.DGResumenExp.DataSource = Vble.TablaLecturistas; 
                this.LabCantidad.Text = "Cantidad: " + (this.DGResumenExp.RowCount).ToString();

              
            }
            else
            {
                if (RBSelectionRemesa == "SI")
                {
                    string SelectCount = "select Count(*) From Conexiones WHERE Zona = " + ResZona + " AND Remesa = " + CBRemesa.Text + " AND Periodo = " + Vble.Periodo;
                    MySqlCommand daCount = new MySqlCommand(SelectCount, DB.conexBD);
                    daCount.CommandTimeout = 300;
                    count = Convert.ToInt32(daCount.ExecuteScalar());
                    bandera = true;

                    datosAdapter.Fill(Tabla);
                    this.DGResumenExp.DataSource = Tabla;
                    this.LabCantidad.Text = "Cantidad: " + (this.DGResumenExp.RowCount).ToString() + " de " + count.ToString();
                }
                else
                {
                    string SelectCount = "select Count(*) From Conexiones WHERE Ruta = " + TextBoxRuta.Text + " AND Remesa = " + CBRemesa.Text + " AND Periodo = " + Vble.Periodo;
                    MySqlCommand daCount = new MySqlCommand(SelectCount, DB.conexBD);
                    daCount.CommandTimeout = 300;
                    count = Convert.ToInt32(daCount.ExecuteScalar());


                    Tabla = new DataTable();

                    datosAdapter.Fill(Tabla);
                    this.DGResumenExp.DataSource = Tabla;
                    this.LabCantidad.Text = "Cantidad: " + (this.DGResumenExp.RowCount).ToString() + " de " + count.ToString();
                    //daCount.Dispose();
                    //datosAdapter.Dispose();

                }
            }

            if (DB.sDbUsu.ToUpper() == "SUPERVISOR" || DB.sDbUsu.ToUpper() == "AUDITORIA"){

                DGResumenExp.Columns.Add("%Saldo", "%Saldo");
                DGResumenExp.Columns["%Saldo"].DisplayIndex = 8;
                DGResumenExp.Columns["Rem"].Visible = false;
            }
            else{
                DGResumenExp.Columns["Remesa"].Visible = false;
                DGResumenExp.Columns["Ruta"].Visible = true;
             }
            
            int Total = 0;

            if (DGResumenExp.Columns.Contains("Total"))
            {               
                foreach (DataGridViewRow item in DGResumenExp.Rows)
                {       
                    Total = Convert.ToInt32(item.Cells["Total"].Value);
                    if (Total == 0)
                    {
                        item.Cells["%Saldo"].Value = " ";
                    }
                    else
                    {
                        porcsaldo = ((Convert.ToInt32(item.Cells["Saldo"].Value) * 100) / (decimal)Total);
                        item.Cells["%Saldo"].Value = porcsaldo.ToString("N3");
                    }
                }
            }
            if (DGResumenExp.Columns.Contains("ImpresionCANT"))
            {
                DGResumenExp.Columns["ImpresionCANT"].Visible = false;
            }
            if (DGResumenExp.Columns.Contains("Inyeccion"))
            {
                DGResumenExp.Columns["Inyeccion"].Visible = true;
            }
            if (DGResumenExp.Columns.Contains("FechaReg"))
            {
                DGResumenExp.Columns["FechaReg"].Visible = false;
            }
            if (DGResumenExp.Columns.Contains("LectReg"))
            {
                DGResumenExp.Columns["LectReg"].Visible = false;
            }
            DGResumenExp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            if (LabelLeyenda.Text == "LabelErrores")
            {
                DGResumenExp.Columns["TextoError"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGResumenExp.Columns["NUsuario"].Visible = true;
            }
            else if (LabelLeyenda.Text == "LabelTodosRem")
            {
                if (ImpresionOBS == "999" || ImpresionOBS == "9999")
                {
                    int totalLocalidades = 0;
                    int totalTotal = 0;
                    int totalInfoNoImprimir = 0;
                    int totalLeidos = 0;
                    int totalSaldos = 0;
                    decimal totalPorcSaldo = 0;
                    int totalImpresos = 0;
                    int totalLeidosNOImp = 0;
                    int totalFueraRango = 0;
                    int totalOrdenativo = 0;
                    int totalImpresora = 0;

                    int totalIndicados = 0;
                    int totaImposible = 0;
                    //int totalIndicadosEImposibles = 0;
                    int totalSAP = 0;
                    int totalWS = 0;
                    ///comento esta columna hasta que se confirme lo de inyeccion de consumo

                    foreach (DataGridViewRow item in DGResumenExp.Rows)
                    {
                        totalLocalidades++;
                        totalTotal = totalTotal + Convert.ToInt32(item.Cells["Total"].Value);
                        totalInfoNoImprimir = totalInfoNoImprimir + Convert.ToInt32(item.Cells["InfoNoImprimir"].Value);
                        totalLeidos = totalLeidos + Convert.ToInt32(item.Cells["Leidos"].Value);
                        totalSaldos = totalSaldos + Convert.ToInt32(item.Cells["Saldo"].Value);
                        if (item.Cells["%Saldo"].Value.ToString() != " ")
                        {
                            totalPorcSaldo = totalSaldos + Convert.ToDecimal(item.Cells["%Saldo"].Value);
                        }
                        totalImpresos = totalImpresos + Convert.ToInt32(item.Cells["Impresos"].Value);
                        totalLeidosNOImp = totalLeidosNOImp + Convert.ToInt32(item.Cells["LeidoNoImpresos"].Value);
                        totalFueraRango = totalFueraRango + Convert.ToInt32(item.Cells["FueraRango"].Value);
                        totalOrdenativo = totalOrdenativo + Convert.ToInt32(item.Cells["Ordenativo"].Value);
                        totalImpresora = totalImpresora + Convert.ToInt32(item.Cells["Impresora"].Value);
                        totalIndicados = totalIndicados + Convert.ToInt32(item.Cells["Indicado"].Value);
                        totaImposible = totaImposible + Convert.ToInt32(item.Cells["Imposible"].Value);
                    
                        //totalIndicadosEImposibles = totalIndicadosEImposibles + Convert.ToInt32(item.Cells["Indicado E Imposible"].Value);
                        totalSAP = totalSAP + Convert.ToInt32(item.Cells["SAP"].Value);
                        totalWS = totalWS + Convert.ToInt32(item.Cells["WS"].Value);
                    }

                    toolStripTotalizador.Text = "Localidades = " + totalLocalidades.ToString() + " Total = " + totalTotal.ToString() +
                                                " InfoNoImprimir = " + totalInfoNoImprimir.ToString() + " Leidos = " + totalLeidos.ToString() +
                                                " Saldos = " + totalSaldos.ToString() + " %Saldo = " + totalPorcSaldo +
                                                " Impresos = " + totalImpresos.ToString() + " Leidos NO Impresos = " + totalLeidosNOImp.ToString() +
                                                " Fuera De Rango = " + totalFueraRango.ToString() + " Ordenativos = " + totalOrdenativo.ToString() +
                                                " Impresora = " + totalImpresora + " Indicados = " + totalIndicados.ToString() + " Imposible = " + totaImposible.ToString() +
                                                " SAP = " + totalSAP.ToString() + " WS = " + totalWS.ToString();
                    statusStrip1.Visible = true;
                }
            }
            else if (LabelLeyenda.Text == "lblOrdenativosR")
            {
                int cont201 = 0, cont202 = 0, cont203 = 0, cont204 = 0, cont205 = 0, cont206 = 0, cont207 = 0, cont208 = 0, cont209 = 0, cont210 = 0,
                    cont211 = 0, cont212 = 0, cont213 = 0, cont214 = 0, cont215 = 0, cont216 = 0, cont217 = 0, cont218 = 0, cont219 = 0, cont220 = 0,
                    cont221 = 0, cont222 = 0, cont223 = 0, cont224 = 0, cont225 = 0, cont226 = 0, cont227 = 0, cont228 = 0, cont229 = 0, cont230 = 0,
                    cont231 = 0, cont232 = 0, cont233 = 0, cont234 = 0, cont235 = 0, cont236 = 0, cont237 = 0, cont238 = 0, cont239 = 0, cont240 = 0,
                    cont241 = 0, cont242 = 0, cont243 = 0, cont244 = 0, cont245 = 0, cont246 = 0, cont247 = 0, cont248 = 0, cont249 = 0, cont250 = 0,
                    cont251 = 0, cont252 = 0, cont253 = 0, cont254 = 0, cont255 = 0, cont256 = 0, cont257 = 0, cont258 = 0;

                string[] localidades = new string[58] { "201", "202", "203", "204", "205",
                                                        "206", "207", "208", "209", "210",
                                                        "211", "212", "213", "214", "215",
                                                        "216", "217", "218", "219", "220",
                                                        "221", "222", "223", "224", "225",
                                                        "226", "227", "228", "229", "230",
                                                        "231", "232", "233", "234", "235",
                                                        "236", "237", "238", "239", "240",
                                                        "241", "242", "243", "244", "246",
                                                        "247", "248", "249", "250", "251",
                                                        "252", "253", "254", "255", "256",
                                                        "257", "258", "259"};



                foreach (DataGridViewRow item in DGResumenExp.Rows)
                {
                    if (item.Cells["Zona"].Value.ToString() == "201")
                    {
                        cont201++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "202")
                    {
                        cont202++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "203")
                    {
                        cont203++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "204")
                    {
                        cont204++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "205")
                    {
                        cont205++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "206")
                    {
                        cont206++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "207")
                    {
                        cont207++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "208")
                    {
                        cont208++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "209")
                    {
                        cont209++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "210")
                    {
                        cont210++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "211")
                    {
                        cont211++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "212")
                    {
                        cont212++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "213")
                    {
                        cont213++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "214")
                    {
                        cont214++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "215")
                    {
                        cont215++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "216")
                    {
                        cont216++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "217")
                    {
                        cont217++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "218")
                    {
                        cont218++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "219")
                    {
                        cont219++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "220")
                    {
                        cont220++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "221")
                    {
                        cont221++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "222")
                    {
                        cont222++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "223")
                    {
                        cont223++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "224")
                    {
                        cont224++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "225")
                    {
                        cont225++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "226")
                    {
                        cont226++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "227")
                    {
                        cont227++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "228")
                    {
                        cont228++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "229")
                    {
                        cont229++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "230")
                    {
                        cont230++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "231")
                    {
                        cont231++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "232")
                    {
                        cont232++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "233")
                    {
                        cont233++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "234")
                    {
                        cont234++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "235")
                    {
                        cont235++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "236")
                    {
                        cont236++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "237")
                    {
                        cont237++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "238")
                    {
                        cont238++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "239")
                    {
                        cont239++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "240")
                    {
                        cont240++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "241")
                    {
                        cont241++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "242")
                    {
                        cont242++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "243")
                    {
                        cont243++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "244")
                    {
                        cont244++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "245")
                    {
                        cont245++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "246")
                    {
                        cont246++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "247")
                    {
                        cont247++;

                    }
                    else if (item.Cells["Zona"].Value.ToString() == "248")
                    {
                        cont248++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "249")
                    {
                        cont249++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "250")
                    {
                        cont250++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "251")
                    {
                        cont251++;

                    }
                    else if (item.Cells["Zona"].Value.ToString() == "252")
                    {
                        cont252++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "253")
                    {
                        cont253++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "254")
                    {
                        cont254++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "255")
                    {
                        cont255++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "256")
                    {
                        cont256++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "257")
                    {
                        cont257++;
                    }
                    else if (item.Cells["Zona"].Value.ToString() == "258")
                    {
                        cont258++;
                    }

                }

                int[] cantidades = new int[58] {cont201, cont202, cont203, cont204, cont205, cont206, cont207, cont208, cont209, cont210,
                              cont211, cont212, cont213, cont214, cont215, cont216, cont217, cont218, cont219, cont220,
                              cont221, cont222, cont223, cont224, cont225, cont226, cont227, cont228, cont229, cont230,
                              cont231, cont232, cont233, cont234, cont235, cont236, cont237, cont238, cont239, cont240,
                              cont241, cont242, cont243, cont244, cont245, cont246, cont247, cont248, cont249, cont250,
                              cont251, cont252, cont253, cont254, cont255, cont256, cont257, cont258};

                listBoxOrd.Items.Add("Localidad  |  Cantidad");
                for (int i = 0; i < localidades.Length; i++)
                {
                    //ListViewItem itemAgregar = new ListViewItem();
                    //itemAgregar.SubItems.Add(localidades[i].ToString() + ": " + cantidades[i].ToString());
                    listBoxOrd.Items.Add(localidades[i].ToString() + ":         " + cantidades[i].ToString());
                }
                //listBoxOrd.Items.Add(itemAgregar);
                listBoxOrd.Visible = true;
                PanelTotOrd.Visible = true;
            }
            else if (LabelLeyenda.Text == "Leidos NO impresos")
            {
                listBoxOrd.Items.Add("                NO IMPRESOS POR IMPRESORA ");
              

                ///Recorro los registros devueltos como NO Impresos para contar las cantidades por cada motivo de no impresion.
                ///compara el campo Situación correspondiente a la columna Titulo de la tabla Errores y cada caso almacena en su variable
                ///correspondiente para luego mostarlo.
                foreach (DataGridViewRow item in DGResumenExp.Rows)
                {
                    if (item.Cells["Situacion"].Value.ToString() == "Impresora error - NO Impreso ")
                    {
                        totalErrorIndefImpresora++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Impresora deshabilitada - NO Impreso ")
                    {
                        totalImpresoraDeshab++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Impresora apagada - NO Impreso ")
                    {
                        totalImpresoraApagada++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Impresora NO vinculada - NO Impreso")
                    {
                        totalImpresoraNoVinc++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Error de comunicacion con Impresora. No Impreso")
                    {
                        totalImpresoraComunicacion++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Impresion Marcada por Lote. No Impreso")
                    {
                        totalMarcadosPorLote++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "No Hay Respuesta WS - NO Impreso")
                    {
                        totalNoImprPorWS++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Fuera de Rango - NO Impreso")
                    {
                        totalFueraRango++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Indicado Dato - NO Impreso ")
                    {
                        totalIndicadoDato++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Ingresó CORRECCION Estado - NO Impreso ")
                    {
                        totalCorreccionEstado++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Tarifa No Imprimible - NO Impreso")
                    {
                        totalTarifaNoImpri++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Exceso Consumo SAP - NO Impreso")
                    {
                        totalExcesoConsumoSap++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Exceso Importes SAP - NO Impreso ")
                    {
                        totalExcesoImporteSAP++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Exceso Dias Periodo SAP - NO Impreso ")
                    {
                        totalExcesoDiasPeriodo++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Medidor Apagado. NO Impreso")
                    {
                        totalMedidorApagado++;
                    }
                    if (item.Cells["Situacion"].Value.ToString() == "Ingresó Novedad NO Imprimible - NO Impreso ")
                    {
                        totalNovedadNoImpri++;
                    }
                 
                }
                toolStripTotalizador.Text = "Impresora Error = " + totalErrorIndefImpresora + " | Impresora deshabilitada = " + totalImpresoraDeshab
                + " | Impresora apagada = " + totalImpresoraApagada
                + " | Impresora NO vinculada = " + totalImpresoraNoVinc
                + " | Error de comunicacion con Impresora = " + totalImpresoraComunicacion
                + " | Impresion Marcada por Lote = " + totalMarcadosPorLote;

                listBoxOrd.Items.Add("Impresora error = " + totalErrorIndefImpresora);
                listBoxOrd.Items.Add("Impresora deshabilitada = " + totalImpresoraDeshab);
                listBoxOrd.Items.Add("Impresora apagada = " + totalImpresoraApagada);
                listBoxOrd.Items.Add("Impresora NO vinculada = " + totalImpresoraNoVinc);
                listBoxOrd.Items.Add("Error de comunicacion con Impresora = " + totalImpresoraComunicacion);
                listBoxOrd.Items.Add("Impresion Marcada por Lote = " + totalMarcadosPorLote);
                listBoxOrd.Items.Add("                NO IMPRESOS POR WS ");
                listBoxOrd.Items.Add("WS = " + totalNoImprPorWS);
                listBoxOrd.Items.Add("                NO IMPRESOS POR FUERA DE RANGO ");
                listBoxOrd.Items.Add("Fuera de Rango = " + totalFueraRango);
                listBoxOrd.Items.Add("                NO IMPRESOS POR INDICADO EN DATO ");
                listBoxOrd.Items.Add("Indicado en Dato = " + totalIndicadoDato);
                listBoxOrd.Items.Add("                NO IMPRESOS POR CORRECCION DE ESTADO ");
                listBoxOrd.Items.Add("Correccion de estado = " + totalCorreccionEstado);
                listBoxOrd.Items.Add("                NO IMPRESOS POR NOVEDAD NO IMPRIMIBLE");
                listBoxOrd.Items.Add("Novedad NO Imprimible = " + totalNovedadNoImpri);
                listBoxOrd.Items.Add("                NO IMPRESOS POR TARIFA ");
                listBoxOrd.Items.Add("Tarifa NO Imprimibles = " + totalTarifaNoImpri);
                listBoxOrd.Items.Add("                NO IMPRESOS EXCESO CONSUMO DESDE SAP ");
                listBoxOrd.Items.Add("Exceso de Consumo = " + totalExcesoConsumoSap);
                listBoxOrd.Items.Add("                NO IMPRESOS EXCESO IMPORTE DESDE SAP ");
                listBoxOrd.Items.Add("Exceso de Importe = " + totalExcesoImporteSAP);
                listBoxOrd.Items.Add("                NO IMPRESOS EXCESO DIAS DEL PERIODO ");
                listBoxOrd.Items.Add("Exceso de Importe = " + totalExcesoDiasPeriodo);
                listBoxOrd.Items.Add("           NO IMPRESOS EXCESO DE RENGLONES EN FACTURA ");
                listBoxOrd.Items.Add("Exceso de Renglones = " + totalExcesoDiasPeriodo);
                listBoxOrd.Items.Add("           NO IMPRESOS POR MEDIDOR APAGADO ");
                listBoxOrd.Items.Add("Medidor Apagado = " + totalMedidorApagado);
                statusStrip1.Visible = true;
                listBoxOrd.Visible = true;
            }
            else if (LabelLeyenda.Text == "LabelTeleLect")
            {
                DataGridViewColumn column = DGResumenExp.Columns["Localidad"];
                //DGResumenExp.Columns[0].Visible = false;            
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else if (LabelLeyenda.Text == "Todos")
            {
                DGResumenExp.Columns["Periodo"].Visible = false;
                DGResumenExp.Columns["Remesa"].Visible = false;
                DGResumenExp.Columns["Ruta"].Visible = false;
                Vble.contRelev = 0;
                Vble.contOrdenativos = 0;
                lblTotalUsConOrd.Text="";
                int ord1 = 0;
                int ord2 = 0;
                int ord3 = 0;
                int ord4 = 0;
                int ord5 = 0;

                string patron = @"[a-zA-Z]\d+";

                Regex regex = new Regex(patron);

               
                try
                {
                    foreach (DataRow item in Vble.TablaLecturistas.Rows)
                    {
                        if (item["Lecturistas"].ToString() != "0")
                        {
                            Vble.lecturistas.Add((item["Lecturistas"].ToString()));
                            Vble.lectOrd.Add(item["Lecturistas"].ToString(), 0);
                        }
                       
                    }
                }
                catch (Exception)
                {

                    //throw;
                }

                #region Seccion Contador de Ordenativos
                foreach (DataGridViewRow item in DGResumenExp.Rows)
                {
                    if (item.Cells["Observaciones"].Value.ToString().ToUpper().Contains("|R") || item.Cells["Observaciones"].Value.ToString().ToUpper().Contains("R0")
                        || item.Cells["Observaciones"].Value.ToString().ToUpper().Contains("|0") || regex.IsMatch(item.Cells["Observaciones"].Value.ToString()))
                    {
                        Vble.contRelev++;
                    }                  
                    if (item.Cells["Ord1"].Value.ToString() != "")
                        {
                        if (item.Cells["Ord1"].Value.ToString() != "99")
                        {
                            ord1++;

                            foreach (var lect in Vble.lecturistas)
                            {
                                if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                {
                                    Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                }
                            }
                            if (item.Cells["Ord2"].Value.ToString() != "")
                            {
                                ord2++;
                                foreach (var lect in Vble.lecturistas)
                                {
                                    if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                    {
                                        Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                    }
                                }
                                if (item.Cells["Ord3"].Value.ToString() != "")
                                {
                                    ord3++;
                                    foreach (var lect in Vble.lecturistas)
                                    {
                                        if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                        {
                                            Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                        }
                                    }
                                    if (item.Cells["Ord4"].Value.ToString() != "")
                                    {
                                        ord4++;
                                        foreach (var lect in Vble.lecturistas)
                                        {
                                            if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                            {
                                                Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                            }
                                        }
                                        if (item.Cells["Ord5"].Value.ToString() != "")
                                        {
                                            ord5++;
                                            foreach (var lect in Vble.lecturistas)
                                            {
                                                if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                                {
                                                    Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (item.Cells["Ord1"].Value.ToString() == "99")
                        {
                            if (item.Cells["Ord2"].Value.ToString() != "")
                            {
                                ord2++;
                                foreach (var lect in Vble.lecturistas)
                                {
                                    if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                    {
                                        Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                    }
                                }
                                if (item.Cells["Ord3"].Value.ToString() != "")
                                {
                                    ord3++;
                                    foreach (var lect in Vble.lecturistas)
                                    {
                                        if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                        {
                                            Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                        }
                                    }
                                    if (item.Cells["Ord4"].Value.ToString() != "")
                                    {
                                        ord4++;
                                        foreach (var lect in Vble.lecturistas)
                                        {
                                            if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                            {
                                                Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                            }
                                        }
                                        if (item.Cells["Ord5"].Value.ToString() != "")
                                        {
                                            ord5++;
                                            foreach (var lect in Vble.lecturistas)
                                            {
                                                if (item.Cells["Operario"].Value.ToString() == lect.ToString())
                                                {
                                                    Vble.lectOrd[item.Cells["Operario"].Value.ToString()] = Vble.lectOrd[item.Cells["Operario"].Value.ToString()] + 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }   
                }
             

                Vble.contOrdenativos = ord1 + ord2 + ord3 + ord4 + ord5;

                lblTotalUsConOrd.Text = "Total de Ordenativos: " + Vble.contOrdenativos.ToString();
                for (int i = 0; i < Vble.lectOrd.Count; i++)
                {
                    lblTotalUsConOrd.Text += "\n Lecturista: " + Vble.lectOrd.ElementAt(i).Key + "  =  " + Vble.lectOrd.ElementAt(i).Value;
                }
                //ContarOrdenativos();
                lblTotModRel.Text = "Total: " + Vble.contRelev.ToString();
                #endregion

            }
            else if (LabelLeyenda.Text == "DispSaldos")
            {
                DGResumenExp.Columns["Secuencia"].Visible = false;
                DGResumenExp.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGResumenExp.Columns["Apellido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            
            else
            {
                DGResumenExp.Columns["Localidad"].Visible = false;
                DGResumenExp.Columns["NInstalacion"].Visible = false;
            }            
            leyenda.Text = "Informe seleccionado: " + LabelLeyenda.Text;
            leyenda.Visible = true;
            //Vble.HideLoading();
            CONSULTANOIMPRESOS = "";
            txSQL = "";
                //MiLoadingInformes.Visible = false;
                //LabCargandoInformes.Visible = false;
            }
            catch (Exception)
            {

                
            }
        }

        public void EjecutarInforme()
        {
          
        }

        public void ResumenGeneral()
        {
            //CargarTablaPreDescarga();
            ArrayList ListaFechas = new ArrayList();
            DataTable TablaFechas = new DataTable();
            ArrayList CantUsurios = new ArrayList();
            ArrayList Tomados = new ArrayList();
            ArrayList Impresos = new ArrayList();
            ArrayList PorcentajeImpresos = new ArrayList();
            ArrayList LeidosSinImprimir = new ArrayList();
            ArrayList FueraDeRango = new ArrayList();
            ArrayList IndicacionNoImpresion = new ArrayList();
            ArrayList HoraInicio = new ArrayList();
            ArrayList HoraFin = new ArrayList();
            ArrayList Inicio = new ArrayList();
            ArrayList Fin = new ArrayList();
            Dictionary<string, int> DictionaryTL = new Dictionary<string, int>();
            ArrayList PorcenajePorHora = new ArrayList();


            PLoadingResGral.Visible = true;
            TextBoxRuta.Enabled = false;
            DTPDesdeTomLect.Enabled = false;
            DTPHastaTomLect.Enabled = false;

            if (DB.sDbUsu.ToUpper() == "SUPERVISOR")
            {
                Desde = DateTime.Now.Year + "-01-01";
                Hasta = DateTime.Now.Year + "-12-31";
            }
            else
            {
                Desde = DTPDesdeTomLect.Value.ToString("yyyy-MM-dd");
                Hasta = DTPHastaTomLect.Value.ToString("yyyy-MM-dd");
            }

            string SelectFechas = "";

            if (RutaNº != "")
            {
                SelectFechas = "SELECT DISTINCT M.ActualFecha AS Fecha FROM Conexiones C " +
               "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
               "WHERE C.Ruta = " + RutaNº + " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
               " AND M.ActualFecha BETWEEN '" + Desde + "' AND '" + Hasta + "' AND M.ActualFecha <> '01-01-2000' ORDER BY Fecha ASC";
            }
            else
            {
                SelectFechas = "SELECT DISTINCT M.ActualFecha AS Fecha FROM Conexiones C " +
               "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
               "WHERE C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
               " AND M.ActualFecha BETWEEN '" + Desde + "' AND '" + Hasta + "' AND M.ActualFecha <> '01-01-2000' ORDER BY Fecha ASC";
            }

            //txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario FROM Conexiones C WHERE C.ImpresionOBS = " + ImpresionOBS;

            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(SelectFechas, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);

            TablaFechas = new DataTable();
            datosAdapter.Fill(TablaFechas);

            datosAdapter.Dispose();
            comandoSQL.Dispose();

            ListaFechas.RemoveRange(0, ListaFechas.Count);

            foreach (DataRow item in TablaFechas.Rows)
            {
                ListaFechas.Add(item.Field<DateTime>("Fecha").ToString("dd/MM/yyyy"));
                DictionaryTL.Add(item.Field<DateTime>("Fecha").ToString("dd/MM/yyyy"), 0);
            }

            


            for (int i = 0; i < ListaFechas.Count; i++)
            {
                //string SelectHoraMin = "SELECT MIN(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
                //                      " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                //                      //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                //                      " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                //                      " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                //MySqlCommand command = new MySqlCommand(SelectHoraMin, DB.conexBD);
                //command.CommandTimeout = 300;
                //HoraInicio.Add(command.ExecuteScalar().ToString());
                //command.Dispose();

                string SelectHoraMin = "SELECT MIN(M.ActualHora) as HoraInicio, MAX(M.ActualHora) as HoraFin, Count(C.ConexionID) as Total FROM Conexiones C INNER JOIN Medidores M" +
                                      " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                                      //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                      " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                                      " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                MySqlCommand command = new MySqlCommand(SelectHoraMin, DB.conexBD);
                command.CommandTimeout = 300;
                MySqlDataReader reader = command.ExecuteReader();
                command.Dispose();

                // Lee los datos mientras haya filas disponibles
                while (reader.Read())
                {
                    // Accede a los valores por índice de columna o por nombre de columna
                    HoraInicio.Add(reader["HoraInicio"].ToString());
                    HoraFin.Add(reader["HoraFin"].ToString());
                    CantUsurios.Add(reader["Total"].ToString());                  
                }
                reader.Dispose();

                //string SelectHoraMax = "SELECT MAX(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
                //                      " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                //                      //" WHERE C.Ruta = " + RutaNº + " AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                //                      " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                //                      " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                //command = new MySqlCommand(SelectHoraMax, DB.conexBD);
                //command.CommandTimeout = 300;
                //HoraFin.Add(command.ExecuteScalar().ToString());
                //command.Dispose();

                //string TotalUsuarios = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                //                       " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                //                       //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                //                       " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                //                       " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                //command = new MySqlCommand(TotalUsuarios, DB.conexBD);
                //command.CommandTimeout = 300;
                //CantUsurios.Add(command.ExecuteScalar().ToString());
                //command.Dispose();

                string SelectTomados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                       " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                       //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                       " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                                       " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                       " AND C.ImpresionOBS > 600";

                command = new MySqlCommand(SelectTomados, DB.conexBD);
                command.CommandTimeout = 300;
                Tomados.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectImpresos = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionOBS = 601";

                command = new MySqlCommand(SelectImpresos, DB.conexBD);
                command.CommandTimeout = 300;
                Impresos.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectLeidosSinImprimir = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionOBS > 601";

                command = new MySqlCommand(SelectLeidosSinImprimir, DB.conexBD);
                command.CommandTimeout = 300;
                LeidosSinImprimir.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectFueraRango = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionOBS = 604";

                command = new MySqlCommand(SelectFueraRango, DB.conexBD);
                command.CommandTimeout = 300;
                FueraDeRango.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectIndicados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " WHERE C.Ruta = " + RutaNº + " and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionCOD = 1";

                command = new MySqlCommand(SelectIndicados, DB.conexBD);
                command.CommandTimeout = 300;
                string indicados = command.ExecuteScalar() == null ? "0" : command.ExecuteScalar().ToString();
                IndicacionNoImpresion.Add(indicados);
                command.Dispose();


                Inicio.Add(DateTime.ParseExact(Convert.ToDateTime(HoraInicio[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));
                Fin.Add(DateTime.ParseExact(Convert.ToDateTime(HoraFin[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));

            }


            foreach (DataGridViewRow fi in DGResumenExp.Rows)
            {
                if (fi.Cells["Tipo_Lectura"].Value.ToString().Contains("T:"))
                {
                    string fechaCelda = Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy");
                    if (DictionaryTL.ContainsKey(fechaCelda))
                    {
                        DictionaryTL[fechaCelda]++;
                    }
                }

                //if (fi.Cells["Tipo_Lectura"].Value.ToString().Contains("T:"))
                //{
                //    //foreach (var item in DictionaryTL.Keys)
                //    //{
                //        for (int index = 0; index < DictionaryTL.Count; index++)
                //        {                              

                //        string FechaLectura = Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy");
                //        if (FechaLectura == DictionaryTL.ElementAt(index).ToString())
                //        {
                //            int contTL = DictionaryTL.TryGetValue(DictionaryTL[DictionaryTL.ElementAt(index).Key], out contTL) ? contTL : DictionaryTL.ElementAt(index);
                //            DictionaryTL[DictionaryTL.ElementAt(index).Key] = contTL + 1;
                //            //break;
                //        }

                //    }

                //}

                //}
            }

            //ListView Resumen;

            for (int i = 0; i < ListaFechas.Count; i++)
            {
                var dif = (Convert.ToDateTime(Fin[i]) - Convert.ToDateTime(Inicio[i])).TotalHours;
                ///agrega el item fecha
                items = new ListViewItem(Convert.ToDateTime(ListaFechas[i]).ToString("dd/MM/yyyy"));
                ///agrega el item hora inicio
                items.SubItems.Add(HoraInicio[i].ToString());
                ///agrega el item hora fin
                items.SubItems.Add(HoraFin[i].ToString());

                ///agrega el item Duracion
                if (dif.ToString().Length > 3)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 3));
                }
                else if (dif.ToString().Length == 3)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 3));
                }
                else if (dif.ToString().Length == 2)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 1));
                }
                else if (dif.ToString().Length == 1)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 1));
                }

                if (Convert.ToDecimal(dif) != 0)
                {
                    if ((Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Length > 3)
                    {
                        items.SubItems.Add((Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Substring(0, 4) + " % ");
                    }
                    else
                    {
                        int dividendo = Convert.ToInt32(Tomados[i]);
                        decimal divisor = Convert.ToDecimal(dif);
                        decimal resul = (dividendo / divisor);
                        if (resul.ToString().Length <= 3)
                        {
                            if (resul.ToString().Contains(","))
                            {
                                items.SubItems.Add((resul).ToString().Substring(0, 2) + " % ");
                            }
                            else
                            {
                                items.SubItems.Add((resul).ToString());
                            }

                        }

                    }

                }
                else if (dif == 0)
                {
                    items.SubItems.Add("0");
                }

                ///agrega el item Cantidad Usuarios
                items.SubItems.Add(CantUsurios[i].ToString());
                ///agrega el item Cantidad Tomados
                items.SubItems.Add(Tomados[i].ToString());
                ///agrega el item Cantidad Impresos
                items.SubItems.Add(Impresos[i].ToString());

                if (Convert.ToDecimal(Tomados[i]) != 0)
                {
                    if (((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Length > 2)
                    {
                        items.SubItems.Add(((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 2) + " %");
                    }
                    else
                    {
                        items.SubItems.Add(((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 1) + " %");
                    }

                }

                items.SubItems.Add(LeidosSinImprimir[i].ToString());
                items.SubItems.Add(FueraDeRango[i].ToString());
                items.SubItems.Add(IndicacionNoImpresion[i].ToString());
                foreach (var item in DictionaryTL.Keys)
                {
                    if (ListaFechas[i].ToString() == item)
                    {
                        items.SubItems.Add(DictionaryTL[item].ToString());
                    }
                }
              
                
                //LVResumenGral.BeginInvoke(new InvokeDelegate(InvoketerminatorProgress));
                LVResumenGral.Items.Add(items);


                TextBoxRuta.Enabled = true;
                DTPDesdeTomLect.Enabled = true;
                DTPHastaTomLect.Enabled = true;
                TextFiltro.Enabled = true;
                PLoadingResGral.Visible = false;
                LVResumenGral.Visible = true;
            }
        }

        public void ResumenGeneralDetalleZona(string Zona)
        {
            //CargarTablaPreDescarga();
            ArrayList ListaFechas = new ArrayList();
            DataTable TablaFechas = new DataTable();
            ArrayList CantUsurios = new ArrayList();
            ArrayList Tomados = new ArrayList();
            ArrayList Impresos = new ArrayList();
            ArrayList PorcentajeImpresos = new ArrayList();
            ArrayList LeidosSinImprimir = new ArrayList();
            ArrayList FueraDeRango = new ArrayList();
            ArrayList IndicacionNoImpresion = new ArrayList();
            ArrayList HoraInicio = new ArrayList();
            ArrayList TeleLecturas = new ArrayList();
            ArrayList HoraFin = new ArrayList();
            ArrayList Inicio = new ArrayList();
            ArrayList Fin = new ArrayList();
            ArrayList PorcenajePorHora = new ArrayList();

            PLoadingResGral.Visible = true;
            TextBoxRuta.Enabled = false;
            DTPDesdeTomLect.Enabled = false;
            DTPHastaTomLect.Enabled = false;

            if (DB.sDbUsu.ToUpper() == "SUPERVISOR")
            {
                Desde = DateTime.Now.Year + "-01-01";
                Hasta = DateTime.Now.Year + "-12-31";
            }
            else
            {
                Desde = DTPDesdeTomLect.Value.ToString("yyyy-MM-dd");
                Hasta = DTPHastaTomLect.Value.ToString("yyyy-MM-dd");
            }

            string SelectFechas = "";

            if (RutaNº != "")
            {
                SelectFechas = "SELECT DISTINCT M.ActualFecha AS Fecha FROM Conexiones C " +
               "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
               "WHERE C.Ruta = " + RutaNº + " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
               " AND M.ActualFecha BETWEEN '" + Desde + "' AND '" + Hasta + "' AND M.ActualFecha <> '01-01-2000' ORDER BY Fecha ASC";
            }
            else
            {
                SelectFechas = "SELECT DISTINCT M.ActualFecha AS Fecha FROM Conexiones C " +
               "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
               "WHERE C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
               " AND M.ActualFecha BETWEEN '" + Desde + "' AND '" + Hasta + "' AND M.ActualFecha <> '01-01-2000' ORDER BY Fecha ASC";
            }

            //txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario FROM Conexiones C WHERE C.ImpresionOBS = " + ImpresionOBS;

            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(SelectFechas, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);

            TablaFechas = new DataTable();
            datosAdapter.Fill(TablaFechas);

            datosAdapter.Dispose();
            comandoSQL.Dispose();

            ListaFechas.RemoveRange(0, ListaFechas.Count);

            foreach (DataRow item in TablaFechas.Rows)
            {
                ListaFechas.Add(item.Field<DateTime>("Fecha").ToString());
            }


            for (int i = 0; i < ListaFechas.Count; i++)
            {
                string SelectHoraMin = "SELECT MIN(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
                                      " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                                      " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                      " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                MySqlCommand command = new MySqlCommand(SelectHoraMin, DB.conexBD);
                command.CommandTimeout = 300;
                HoraInicio.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectHoraMax = "SELECT MAX(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
                                      " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                                      " WHERE C.Ruta = " + RutaNº + " AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                      " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                command = new MySqlCommand(SelectHoraMax, DB.conexBD);
                command.CommandTimeout = 300;
                HoraFin.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string TotalUsuarios = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                       " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                       " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                       " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'";

                command = new MySqlCommand(TotalUsuarios, DB.conexBD);
                command.CommandTimeout = 300;
                CantUsurios.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectTomados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                       " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                       " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                       " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                       " AND C.ImpresionOBS > 600";

                command = new MySqlCommand(SelectTomados, DB.conexBD);
                command.CommandTimeout = 300;
                Tomados.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectImpresos = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionOBS = 601";

                command = new MySqlCommand(SelectImpresos, DB.conexBD);
                command.CommandTimeout = 300;
                Impresos.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectLeidosSinImprimir = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionOBS > 601";

                command = new MySqlCommand(SelectLeidosSinImprimir, DB.conexBD);
                command.CommandTimeout = 300;
                LeidosSinImprimir.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectFueraRango = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionOBS = 604";

                command = new MySqlCommand(SelectFueraRango, DB.conexBD);
                command.CommandTimeout = 300;
                FueraDeRango.Add(command.ExecuteScalar().ToString());
                command.Dispose();

                string SelectIndicados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                        " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                        " WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                        " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                        " AND C.ImpresionCOD = 1";

                command = new MySqlCommand(SelectIndicados, DB.conexBD);
                command.CommandTimeout = 300;
                IndicacionNoImpresion.Add(command.ExecuteScalar().ToString());
                command.Dispose();


                Inicio.Add(DateTime.ParseExact(Convert.ToDateTime(HoraInicio[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));
                Fin.Add(DateTime.ParseExact(Convert.ToDateTime(HoraFin[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));

            }

            //ListView Resumen;

            for (int i = 0; i < ListaFechas.Count; i++)
            {
                var dif = (Convert.ToDateTime(Fin[i]) - Convert.ToDateTime(Inicio[i])).TotalHours;
                ///agrega el item fecha
                items = new ListViewItem(Convert.ToDateTime(ListaFechas[i]).ToString("dd/MM/yyyy"));
                ///agrega el item hora inicio
                items.SubItems.Add(HoraInicio[i].ToString());
                ///agrega el item hora fin
                items.SubItems.Add(HoraFin[i].ToString());


                ///agrega el item Duracion
                if (dif.ToString().Length > 3)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 3));
                }
                else if (dif.ToString().Length == 3)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 3));
                }
                else if (dif.ToString().Length == 2)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 1));
                }
                else if (dif.ToString().Length == 1)
                {
                    items.SubItems.Add(dif.ToString().Substring(0, 1));
                }

                if (Convert.ToDecimal(dif) != 0)
                {
                    if ((Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Length > 3)
                    {
                        items.SubItems.Add((Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Substring(0, 4) + " % ");
                    }
                    else
                    {
                        int dividendo = Convert.ToInt32(Tomados[i]);
                        decimal divisor = Convert.ToDecimal(dif);
                        decimal resul = (dividendo / divisor);
                        if (resul.ToString().Length <= 3)
                        {
                            if (resul.ToString().Contains(","))
                            {
                                items.SubItems.Add((resul).ToString().Substring(0, 2) + " % ");
                            }
                            else
                            {
                                items.SubItems.Add((resul).ToString());
                            }

                        }

                    }

                }
                else if (dif == 0)
                {
                    items.SubItems.Add("0");
                }

                ///agrega el item Cantidad Usuarios
                items.SubItems.Add(CantUsurios[i].ToString());
                ///agrega el item Cantidad Tomados
                items.SubItems.Add(Tomados[i].ToString());
                ///agrega el item Cantidad Impresos
                items.SubItems.Add(Impresos[i].ToString());

                if (Convert.ToDecimal(Tomados[i]) != 0)
                {
                    if (((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Length > 2)
                    {
                        items.SubItems.Add(((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 2) + " %");
                    }
                    else
                    {
                        items.SubItems.Add(((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 1) + " %");
                    }

                }

                items.SubItems.Add(LeidosSinImprimir[i].ToString());
                items.SubItems.Add(FueraDeRango[i].ToString());
                items.SubItems.Add(IndicacionNoImpresion[i].ToString());
                //LVResumenGral.BeginInvoke(new InvokeDelegate(InvoketerminatorProgress));
                LVResumenGral.Items.Add(items);


                TextBoxRuta.Enabled = true;
                DTPDesdeTomLect.Enabled = true;
                DTPHastaTomLect.Enabled = true;
                TextFiltro.Enabled = true;
                PLoadingResGral.Visible = false;
                LVResumenGral.Visible = true;
            }
        }


        public void InvoketerminatorProgress()
        {
            //LVResumenGral.Items.Add(items);

            
        }

        public async void CargarResumenGeneral()
        {
            //Vble.ShowLoading();

            PickBoxLoading.Location = new Point(PLoadingResGral.Width / 2 - PickBoxLoading.Width / 2, PLoadingResGral.Height / 2 - PickBoxLoading.Height / 2);
            //PickBoxLoading.Location = new Point(PickBoxLoading.Width / 2, PickBoxLoading.Height / 2);
            Task oTask = new Task(ResumenGeneral);
            oTask.Start();         

            await oTask;
            //Vble.HideLoading();

        }

        /// <summary>
        /// Carga el datagridview para ver el detalle segun el estado de ImpresionOBS
        /// </summary>
        public void CargarTablaHistorial(string Ruta, string Periodo)
        {

            //Tabla.Clear();
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);

            txSQL = "SELECT DISTINCT C.ConexionID AS Nº_Conexion, P.Apellido AS Titular, M.AnteriorEstado AS Estado Anterior, " +
                    "M.ActualEstado AS Estado Actual, C.ConsumoFacturado AS Consumo Actual Facturado, C.Importe1 AS Importe Cuota 1, " +
                    "C.Importe2 AS Importe_Cuota2 FROM conexiones C " +
                    "INNER JOIN personas P ON C.ConexionID = P.PersonaID " +
                    "INNER JOIN medidores M ON C.ConexionID = M.ConexionID " +
                    "WHERE C.Ruta = " + Ruta + " AND C.Periodo = " + Periodo + " AND M.Periodo = " + Periodo + " AND P.Periodo = " + Periodo;
            //txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario FROM Conexiones C WHERE C.ImpresionOBS = " + ImpresionOBS;
            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBDHistorial);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            this.DGResumenExp.DataSource = Tabla;
            this.DGResumenExp.Refresh();

           



            this.LabCantidad.Text = "Cantidad: " + (this.DGResumenExp.RowCount).ToString();
            comandoSQL.Dispose();
            datosAdapter.Dispose();

        }


        /// <summary>
        /// Metodo que imprime el datagridview como reporte
        /// </summary>
        public void Imprimir(string Titulo, String Subtitulo, int Periodo)
        {
            DGResumenExp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            DGResumenExp.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridViewColumn column = DGResumenExp.Columns[5];
            DGResumenExp.Columns[0].Visible = false;
            DGResumenExp.Columns[2].Visible = false;
            DGResumenExp.Columns[3].Visible = false;
            DGResumenExp.Columns[5].Visible = false;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;



            PrintDataGridView pr = new PrintDataGridView(DGResumenExp)
            { 
                ReportHeader = Titulo + "\n Periodo: " + Periodo,
                ReportFooter = Subtitulo,
                MargenDerecho = 10,
                MargenIzquierdo = 30,
                MargenInferior = 20,
                MargenSuperior = 30,            
                
            };
            
            pr.Print(this);
            DGResumenExp.Columns[0].Visible = true;
            DGResumenExp.Columns[2].Visible = true;
            DGResumenExp.Columns[3].Visible = true;
            DGResumenExp.Columns[5].Visible = true;

        }

        /// <summary>
        /// Se ejecuta la consulta sobre la base mysql general considerando la fecha ingresada desde hasta del filtro 
        /// Fecha de toma de lectura
        /// </summary>
        public void ConsultaPorFechaTomaLect()
        {
            this.Cursor = Cursors.WaitCursor;

            ///Seteo las variables que muestran totales y contadores de cada ruta para asignar con la nueva ruta que se busca nuevamente.
            this.LabCantidad.Text = "0";
            lblTotalUsConOrd.Text = "0";
            LblTotOrdenativos.Text = "0";
            Vble.lectOrd = new Dictionary<string, int>();
            this.DGResumenExp.DataSource = "";
            Vble.TablaLecturistas.Clear();
            ///


            if (TextBoxRuta.Text != "")
            {
                if (ImpresionOBS == "111")
                {
                    Ruta = " AND Ruta = " + TextBoxRuta.Text;
                    RutaNº = TextBoxRuta.Text;
                }
                else
                {
                    Ruta = " AND C.Ruta = " + TextBoxRuta.Text;
                    RutaNº = TextBoxRuta.Text;
                }
                
            }
            else
            {
                Ruta = "";
                RutaNº = "0";
            }

            if (ImpresionOBS.Length == 3)
            {
                if (ImpresionOBS == "602")
                {
                    CONSULTANOIMPRESOS = " SELECT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, " +
                                               "C.Contrato, C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, " +
                                               "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha,  M.ActualHora AS Hora, E.Titulo as Situacion, C.Operario, " +
                                               "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                         "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                         "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                         "WHERE (C.ImpresionOBS >= 602  AND C.ImpresionOBS <= 699 AND ImpresionOBS <> 609) AND M.ActualFecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                         DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text  + Ruta + " and C.Periodo = " + Vble.Periodo;
                }
                else if (ImpresionOBS == "601")
                {
                    CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, " +
                              "M.AnteriorEstado, M.ActualEstado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.ConsumoFacturado, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                              "WHERE (C.ImpresionOBS = " + ImpresionOBS + ") AND (M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                              DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "') AND C.Remesa = " + CBRemesa.Text + Ruta + " and C.Periodo = " + Vble.Periodo;
                }
                else if ( ImpresionOBS == "604")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.TitularID AS IC, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                              "C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "WHERE C.ImpresionOBS = " + ImpresionOBS + " AND M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                               DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + Ruta + " and C.Periodo = " + Vble.Periodo;
                }
                else if (ImpresionOBS == "609")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "WHERE C.ImpresionOBS = " + ImpresionOBS + " AND M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                               DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text  + Ruta + " and C.Periodo = " + Vble.Periodo;
                }
                else if (ImpresionOBS == "17")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, P.Apellido, M.Numero as Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "WHERE C.ImpresionOBS = " + ImpresionOBS + " AND M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                               DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + Ruta + " and C.Periodo = " + Vble.Periodo;
                }
                else if (ImpresionOBS == "800")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                        "C.titularID AS IC, C.DomicSumin, P.Apellido, M.Numero AS Medidor, C.DomicSumin as Domicilio, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
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
                        "WHERE C.ImpresionOBS = " + ImpresionOBS + " AND C.Remesa = " + CBRemesa.Text + Ruta + " and C.Periodo = " + Vble.Periodo; 
                }
                else if (ImpresionOBS == "999")
                {               
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
                                 "WHERE ((C.ImpresionOBS >= 0 AND M.ActualFecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" + DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "') " +
                                 " OR (C.ImpresionOBS MOD 100 >= 0 and M.ActualFecha BETWEEN '2000-01-01' and '2000-01-01') AND C.Periodo = " + Vble.Periodo +
                                 " AND C.Remesa = " + CBRemesa.Text + " AND C.Ruta =  " +
                                 TextBoxRuta.Text + ") " +  
                                 "AND C.Remesa = " + CBRemesa.Text + " AND C.Ruta =  " +
                                 TextBoxRuta.Text +
                                 " AND C.Periodo = " + Vble.Periodo +
                                 "  GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, HoraLect ASC, C.Secuencia";

                    CONSULTANOIMPRESOS =  CONSULTA;
                }
                else if (ImpresionOBS == "111")
                {
                    CONSULTANOIMPRESOS = "SELECT Fecha, Hora, Periodo, Equipo, Ruta, Lecturista, ConexionID AS NInstalacion, CodigoError, TextoError " +
                       "FROM LogErrores WHERE Periodo = " + Vble.Periodo + " AND (Fecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" + DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "') " + Ruta;
                }
                else if (ImpresionOBS == "000")//Cuando la variable dentro del programa ImpresionOBS = 000 
                {
                    CONSULTANOIMPRESOS = "SELECT DISTINCT C.Secuencia, C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, C.titularID AS IC, P.Apellido, M.Numero AS Medidor, " +
              "C.DomicSumin as Domicilio, M.AnteriorEstado, " +
              "if (ImpresionOBS = 400, 'EN CALLE', IF(ImpresionOBS = 500, 'NO LEIDO', IF(ImpresionOBS = 0, 'NO LEIDO', E.Titulo))) AS 'Situacion Actual' " +
              "FROM Conexiones C " +
              "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
              "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
              "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
              "WHERE((C.ImpresionOBS = 0 OR C.ImpresionOBS = 500)  and C.Periodo = " + Vble.Periodo +
              " AND C.Remesa = " +  CBRemesa.Text + " AND C.Ruta = " + TextBoxRuta.Text + ")" +
              " GROUP BY C.ConexionID, M.ConexionID order by C.Secuencia ";
                }

            }
            else if (ImpresionOBS.Length == 1)//Indicados para no Imprimir ImpresionCOD = 1
            {
                CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                     "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, C.DomicSumin as Domicilio, M.Numero AS Medidor, M.AnteriorEstado, " +
                                     "M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                                     "C.Operario, " +
                                     "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                                     "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                                     "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                     "WHERE " +
                                     "C.ImpresionCOD = " + ImpresionOBS + " AND M.ActualFecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                                     DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + " " + Ruta;
            }
          
       
           
            NoImpr = true;
            LabelLeyenda.Visible = true;
            LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString();
            LabelPeriodo.Visible = true;

            CargarTablaPreDescarga();

            if (LabelLeyenda.Text == "Todos")
            {
                if (stbResumen.ToString() == "1")
                {
                    GroupBoxResumenGral.Visible = true;
                    CargarResumenGeneral();

                }
            }

            this.Cursor = Cursors.Default;

        }

        public void ConsiderarOrdenativosAlImprimir()
        {
            if (LabelLeyenda.Text != "Indicados para NO imprimir")
            {
               
                //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
                foreach (DataGridViewRow fi in DGResumenExp.Rows)
                {

                    if (ExisteNovedades(fi.Cells["NInstalacion"].Value.ToString(), fi.Cells["Periodo"].Value.ToString()))
                    {
                        CargarNovedadesConex(fi.Cells["NInstalacion"].Value.ToString(), fi.Cells["Periodo"].Value.ToString());

                        for (int i = 0; i < ArrayCodNovedades.Length; i++)
                        {
                            fi.Cells["Ord" + (i + 1)].Value = ArrayCodNovedades[i];
                        }
                        fi.Cells["Observ"].Value = Observaciones;

                    }
                    else
                    {
                        for (int i = 0; i < ArrayCodNovedades.Length; i++)
                        {
                            ArrayCodNovedades[i] = "";
                        }

                    }
                    Observaciones = "";
                }
            }
        }



        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            // if (e.Button == MouseButtons.Right)
            //{
            //    ContextMenu m = new ContextMenu();
            //    m.MenuItems.Add(new MenuItem("Eliminar"));
            //    m.MenuItems.Add(new MenuItem("Copiar"));
            //    m.MenuItems.Add(new MenuItem("Pegar"));
            //    m.Show(dataGridView1, new Point(e.X, e.Y));
            //}

        }

        private void ArmarGraficos(DataGridView DGResumenExportación, string TipoInforme)
        {
            ArrayList Loc = new ArrayList();
            ArrayList Impresiones = new ArrayList(); 
            ArrayList Total = new ArrayList();

            ///limpio los chart en caso de que se vuelvan a cargar o a filtrar el dtgridviewResumen para su posterior dibujo nuevamnete
            if (ChartImpresos.Series["Total"].Points.Count > 0)
            {               
                ChartImpresos.Series["Total"].Points.Clear();               
            }
            if (ChartImpresos.Series["Impresos"].Points.Count > 0)
            {
                ChartImpresos.Series["Impresos"].Points.Clear();
            }
            if (ChartLeidos.Series["Total"].Points.Count > 0)
            {
                ChartLeidos.Series["Total"].Points.Clear();
            }
            if (ChartLeidos.Series["Leidos"].Points.Count > 0)
            {
                ChartLeidos.Series["Leidos"].Points.Clear();
            }

            ChartImpresos.Titles.Clear();
            ChartLeidos.Titles.Clear();
            zonasGrafico.Clear();
            TotalZonasGrafico.Clear();
            leidosGrafico.Clear();
            impresosGrafico.Clear();
            teleLecturaGrafico.Clear();

            if (TipoInforme == "R")
            {            
                ChartImpresos.Titles.Add("Cantidad IMPRESOS sobre total Importados"); 
                ChartImpresos.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                ChartImpresos.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                ChartLeidos.Titles.Add("Cantidad LEIDOS sobre total Importados");
                ChartLeidos.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                ChartLeidos.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                
                foreach (DataGridViewRow item in DGResumenExp.Rows)
                {
                    ChartImpresos.Series["Total"].Points.AddXY(item.Cells["Interfaz"].Value.ToString(), item.Cells["Total"].Value.ToString());                    
                    ChartImpresos.Series["Impresos"].Points.AddXY(item.Cells["Interfaz"].Value.ToString(), item.Cells["Impresos"].Value.ToString());
                    if (LabelLeyenda.Text == "LabelTeleLect")
                    {
                        ChartImpresos.Series["TeleLectura"].Points.AddXY(item.Cells["TeleLectura"].Value.ToString(), item.Cells["TeleLectura"].Value.ToString());
                    }


                    ChartLeidos.Series["Total"].Points.AddXY(item.Cells["Interfaz"].Value.ToString(), item.Cells["Total"].Value.ToString());
                    ChartLeidos.Series["Leidos"].Points.AddXY(item.Cells["Interfaz"].Value.ToString(), item.Cells["Leidos"].Value.ToString());


                    zonasGrafico.Add(item.Cells["Interfaz"].Value.ToString());
                    TotalZonasGrafico.Add(item.Cells["Total"].Value.ToString());
                    leidosGrafico.Add(item.Cells["Leidos"].Value.ToString());
                    impresosGrafico.Add(item.Cells["Impresos"].Value.ToString());
                   

                    if (LabelLeyenda.Text == "LabelTeleLect")
                    {
                     
                        teleLecturaGrafico.Add(item.Cells["TeleLectura"].Value.ToString());
                    }
                  
                }

            }
            else if (TipoInforme == "T")
            {
                ChartImpresos.Titles.Add("Cantidad IMPRESOS sobre total Importados");
                ChartImpresos.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                ChartImpresos.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

                result = (from item in DGResumenExp.Rows.Cast<DataGridViewRow>()
                                      group item by item.Cells["Interfaz"].Value into grupo
                                      select new Datos()
                                      {
                                          Localidad = Convert.ToInt32(grupo.Key),
                                          Total = grupo.Sum(x => Convert.ToInt32(x.Cells["Total"].Value)),
                                          Leidos = grupo.Sum(x => Convert.ToInt32(x.Cells["Leidos"].Value)),
                                          Impresos = grupo.Sum(x=>Convert.ToInt32(x.Cells["Impresos"].Value)),
                                          TeleLectura = grupo.Sum(x => Convert.ToInt32(x.Cells["TeleLectura"].Value))


                                      }).ToList();
                ChartLeidos.Titles.Add("Cantidad LEIDOS sobre total Importados");
                ChartLeidos.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                ChartLeidos.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                foreach (var item in result)
                {
                    ChartImpresos.Series["Total"].Points.AddXY(item.Localidad, item.Total);
                    //chart1.Series["Leidos"].Points.AddXY(item.Localidad, item.Leidos);                    
                    ChartImpresos.Series["Impresos"].Points.AddXY(item.Localidad, item.Impresos);
                    ChartLeidos.Series["Total"].Points.AddXY(item.Localidad, item.Total);
                    ChartLeidos.Series["Leidos"].Points.AddXY(item.Localidad, item.Leidos);
                    if (LabelLeyenda.Text == "LabelTeleLect")
                    {
                        ChartImpresos.Series["TeleLectura"].Points.AddXY(item.Localidad, item.TeleLectura);
                    }


                    zonasGrafico.Add(item.Localidad);
                    TotalZonasGrafico.Add(item.Total);
                    leidosGrafico.Add(item.Leidos);
                    impresosGrafico.Add(item.Impresos);
                    if (LabelLeyenda.Text == "LabelTeleLect")
                    {
                        teleLecturaGrafico.Add(item.TeleLectura);
                    }
                }

            }

        }

        /// <summary>
        /// Se realiza una consulta que engloba todos los periodos y obtiene la cantidad de empresiones por cada periodo
        /// para graficarlo de forma lineal y ver el mejoramiento en la impresion por periodo.
        /// </summary>
        private void GraficarComparacionPeriodos()
        {
            
            string consulta = "";
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter();
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder();
            DataTable TablaPeriodos = new DataTable();


            consulta = "SELECT Periodo, COUNT(ImpresionOBS) AS Total FROM Conexiones WHERE ImpresionOBS MOD 100 = 1 GROUP BY Periodo ORDER BY Periodo ASC";
            datosAdapter = new MySqlDataAdapter(consulta, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(TablaPeriodos);

            datosAdapter.Dispose();
            comandoSQL.Dispose();
            PerGrafTot.Clear();
            PerImprTot.Clear();

            if (ChartPeriodos.Series["Impresos"].Points.Count > 0)
            {
                ChartPeriodos.Series["Impresos"].Points.Clear();
            }
            ChartPeriodos.Titles.Clear();
            ChartPeriodos.Titles.Add("Cantidad de IMPRESIONES por Periodo");
            ChartPeriodos.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;

            foreach (DataRow item in TablaPeriodos.Rows)
            {                
                ChartPeriodos.Series["Impresos"].Points.AddXY(item["Periodo"].ToString(), item["Total"].ToString());
                PerGrafTot.Add(item["Periodo"].ToString());
                PerImprTot.Add(item["Total"].ToString());
            }

            TablaPeriodos.Clear();

            //consulta = "SELECT Periodo, COUNT(ImpresionOBS) AS Total FROM Conexiones WHERE ImpresionOBS MOD 100 = 1 GROUP BY Periodo ORDER BY Periodo ASC";
            //datosAdapter = new MySqlDataAdapter(consulta, DB.conexBD);
            //comandoSQL = new MySqlCommandBuilder(datosAdapter);
            //datosAdapter.Fill(TablaPeriodos);

        }

        #endregion





        private void button1_Click(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
        
            ////se localiza el formulario buscandolo entre los forms abiertos 
            //Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is FormBusDetallePreDescarga);

            //if (frm != null)
            //{
            //    //si la instancia existe la pongo en primer plano
            //    frm.BringToFront();
            //    return;
            //}

            ////sino existe la instancia se crea una nueva
            //FormBusDetallePreDescarga FormBusqueda = new FormBusDetallePreDescarga();
            //FormBusqueda.ImpresionOBS = ImpresionOBS;
            //FormBusqueda.RutaDatos = RutaDatos; 
            //FormBusqueda.Show();
            ////this.Close();
            


        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();           
            string txSQL;
            SQLiteDataAdapter datosAdapter;
            SQLiteCommandBuilder comandoSQL;
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
            BaseACargar.Open();

            //int cant = 0;
            this.DGResumenExp.DataSource = "";
            Tabla.Clear();
            txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, P.Apellido AS Persona, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario FROM Conexiones C INNER JOIN Personas P ON C.TitularID = P.PersonaID WHERE C.ImpresionOBS = " + ImpresionOBS;
            datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
            comandoSQL = new SQLiteCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);            

            this.DGResumenExp.DataSource = Tabla;
            this.DGResumenExp.Refresh();
            this.LabCantidad.Text = "Cantidad: " + (this.DGResumenExp.RowCount).ToString();
            this.TextFiltro.Text = "";          
            comandoSQL.Dispose();
            datosAdapter.Dispose();
            BaseACargar.Close();
        }

       public void TextNºInstalacion_TextChanged(object sender, EventArgs e)
        {
            if (this.DGResumenExp.RowCount > 0)
            {
                    if (RBRuta.Checked)
                    {
                        string NombreCampo = string.Concat("[", Tabla.Columns["Ruta"].ColumnName, "]");
                        //string fieldName = "conexionID";
                        Tabla.DefaultView.Sort = NombreCampo;
                        DataView view = Tabla.DefaultView;
                        view.RowFilter = string.Empty;
                        if (TextFiltro.Text != string.Empty)
                            view.RowFilter = String.Format("Convert(Ruta, 'System.String') like '%{0}%' AND Periodo = "+ Periodo.ToString(), TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";                    
                        DGResumenExp.DataSource = view;
                        LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                        //ConsiderarOrdenativosAlImprimir();
                    }
                    else if (RBInstalacion.Checked)
                    {
                        string NombreCampo = string.Concat("[", Tabla.Columns["NInstalacion"].ColumnName, "]");
                        //string fieldName = "conexionID";
                        Tabla.DefaultView.Sort = NombreCampo;
                        DataView view = Tabla.DefaultView;
                        view.RowFilter = string.Empty;
                        if (TextFiltro.Text != string.Empty)
                            view.RowFilter = String.Format("Convert(NInstalacion, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                        DGResumenExp.DataSource = view;
                        LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                    else if (RBContrato.Checked)
                    {
                        string NombreCampo = string.Concat("[", Tabla.Columns["Contrato"].ColumnName, "]");
                        //string fieldName = "conexionID";
                        Tabla.DefaultView.Sort = NombreCampo;
                        DataView view = Tabla.DefaultView;
                        view.RowFilter = string.Empty;
                        if (TextFiltro.Text != string.Empty)
                            view.RowFilter = String.Format("Convert(Contrato, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                        DGResumenExp.DataSource = view;
                        LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                    else if (RBTitular.Checked)
                    {
                        string NombreCampo = string.Concat("[", Tabla.Columns["IC"].ColumnName, "]");
                        //string fieldName = "conexionID";
                        Tabla.DefaultView.Sort = NombreCampo;
                        DataView view = Tabla.DefaultView;
                        view.RowFilter = string.Empty;
                        if (TextFiltro.Text != string.Empty)
                            view.RowFilter = String.Format("Convert(IC, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                        DGResumenExp.DataSource = view;
                        LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (RBNumMed.Checked)
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Medidor"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = Tabla.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Medidor, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGResumenExp.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (TipoInforme == "R" || TipoInforme == "T")
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Interfaz"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = Tabla.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Interfaz, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGResumenExp.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                    
                    ArmarGraficos(DGResumenExp, TipoInforme);
                }

            }
            
            else
            {
                //if (DGResumenExp.RowCount == 0)
                //{
                //    MessageBox.Show("Por favor verifique los datos ingresados", "No incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    TextFiltro.Text = "";
                //}
                
                CargarTablaPreDescarga();
                //this.TextNºInstalacion_TextChanged(sender, e);
            }

        }

        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IndicadorTipoInforme == "Resumen")
            {
                Imprimir("Resumen de instalaciones \n\r " + LabelLeyenda.Text, "Macro Intell - DPEC", Vble.Periodo);
            }
            else if (IndicadorTipoInforme == "Historial")
            {
                Imprimir("Resumen Historial", "Ruta:_______ \n\rPeriodo:____", Vble.Periodo);
            }
        }

        private void iTalk_Button_11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.DGResumenExp.DataSource = "";
            this.DGResumenExp.Refresh();
            CargarTablaHistorial(TXTRutaHistorial.Text, cboPeriodoHistorial.Text.Replace("-", ""));
        }

        private void BtnAddPeriodo_Click(object sender, EventArgs e)
        {
            //Expresion Regular para que acepte el periodo con el formato 0000-00
            string sPattern = "^\\d{4}-\\d{2}$";
            bool existeperiodo = false;

            if (System.Text.RegularExpressions.Regex.IsMatch(this.TextNewPeriodo.Text, sPattern))
            {
                foreach (var item in cboPeriodoHistorial.Items)
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
                    cboPeriodoHistorial.Items.Add(this.TextNewPeriodo.Text);
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

        private void TXTRutaHistorial_KeyPress(object sender, KeyPressEventArgs e)
        {
          
          

        }

        private void TXTRutaHistorial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.TXTRutaHistorial.Text.Replace("\r", "");
                this.TXTRutaHistorial.Text.Replace("\n", "");
                CargarTablaHistorial(this.TXTRutaHistorial.Text, cboPeriodoHistorial.Text.Replace("-", ""));
                e.SuppressKeyPress = true;
            }

        }

        private void TXTRutaHistorial_KeyUp(object sender, KeyEventArgs e)
        {
          
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
                    foreach (var item in cboPeriodoHistorial.Items)
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
                        cboPeriodoHistorial.Items.Add(this.TextNewPeriodo.Text);
                        this.toolTip1.Show("Periodo agregado Correctamente", this.TextNewPeriodo, 2000);
                        this.TextNewPeriodo.Text = "";
                    }

                }
                else
                {
                    this.toolTip1.Show("Formato del Periodo Invalido", this.TextNewPeriodo, 2000);
                    this.TextNewPeriodo.Text = "";
                }
                e.SuppressKeyPress = true;

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CargarTablaPreDescarga();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            toolTip1.RemoveAll();
            if (DGResumenExp.RowCount > 0)
            {              

                if (IndicadorTipoInforme == "Resumen")
            {

                    ExportarAltasPDF(DGResumenExp, false);

                    //Imprimir("Resumen de instalaciones \n\r " + LabelLeyenda.Text, "Macro Intell - DPEC", Vble.Periodo);
            }
            else if (IndicadorTipoInforme == "Historial")
            {
                Imprimir("Resumen Historial", "Ruta:_______ \n\rPeriodo:____", Vble.Periodo);
            }

            }
            else
            {
                toolTip1.Show("No existen datos para imprimir", ButPrint, 2500);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            string InformesPDF = Vble.ValorarUnNombreRuta(Vble.CarpetaInformesPDF);
            InformesPDF += Vble.Periodo + "\\" + DateTime.Today.ToString("yyyyMMdd");

            if (!Directory.Exists(InformesPDF))
            {
                Directory.CreateDirectory(InformesPDF);
            }

            fichero.InitialDirectory = InformesPDF;

            fichero.Filter = "PDF (*.pdf)|.*pdf";
            if (DGResumenExp.RowCount > 0)
            {
                fichero.FileName = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text + "-Ruta_" + TextBoxRuta.Text;
                if (fichero.ShowDialog() == DialogResult.OK)
                {
                    if (LabelLeyenda.Text != "TodosR")
                    {
                        //fichero.FileName = LabelLeyenda.Text + "-" + Vble.Periodo.ToString() ;
                        //Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                        ExportarAltasPDF(DGResumenExp, "Resumen " + LabelLeyenda.Text, System.IO.Path.GetFullPath(fichero.FileName), false);
                    }
                    else if (LabelLeyenda.Text == "TodosR")
                    {
                        //fichero.FileName = LabelLeyenda.Text + "-" + Vble.Periodo.ToString();
                        ////Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                        //ExportarToExcel(DGResumenExp, "Resumen " + LabelLeyenda, System.IO.Path.GetFullPath(fichero.FileName), false);
                        //ExportarToExcel(DGResumenExp, "Resumen " + LabelLeyenda.Text, fichero.FileName);
                        ExportarAltasPDF(DGResumenExp, "Resumen " + LabelLeyenda.Text, System.IO.Path.GetFullPath(fichero.FileName), false);
                    }
                }

               
            }
            else
            {
                MessageBox.Show("No existen datos en la tabla para generar el archivo PDF", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            //fichero.Title = LabelLeyenda + "_" + Vble.Periodo.ToString();

          
        }

     

     
        private void LabelRemesa_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void CBRemesa_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void TextBoxRuta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                
                if (Convert.ToInt16(e.KeyChar) == 13)   
                {
                    Vble.lecturistas.Clear();
                    Vble.lectOrd.Clear();
                    LVResumenGral.Items.Clear();
                    ultimaConsultaReg = CONSULTANOIMPRESOS;
                    ConsultaPorFechaTomaLect();
                    

                }
                else
                { 
                    e.Handled = false;
                }
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }          
               
            else
            {
                e.Handled = true;
            }
        }

        private void CBRemesa_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (TextBoxRuta.Text != "")
            {
                Ruta = " AND C.Ruta = " + TextBoxRuta.Text;
                RutaNº = TextBoxRuta.Text;
            }
            else
            {
                Ruta = "";
                RutaNº = "0";
            }


            if (ImpresionOBS.Length == 3)
            {
                if (LabelLeyenda.Text == "Leidas NO impresas")
                {
                    CONSULTANOIMPRESOS = " SELECT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, " +
                                               "C.Contrato, C.titularID AS IC, P.Apellido, M.Numero as Medidor, " +
                                               "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, E.Titulo, C.Operario, " +
                                               "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                         "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                         "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                          "WHERE (C.ImpresionOBS >= 602  AND C.ImpresionOBS <= 699 AND ImpresionOBS <> 609) AND M.ActualFecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                        DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + Ruta;
                }
                else if (LabelLeyenda.Text == "Impresas")
                {
                    CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, " +
                              "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                              "WHERE (C.ImpresionOBS = 601) AND (M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                          DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "') AND C.Remesa = " + CBRemesa.Text + Ruta;
                }
                else if (LabelLeyenda.Text == "NO impresos Fuera de Rango")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.TitularID AS IC, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                              "C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "WHERE C.ImpresionOBS = 604 AND M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                               DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + Ruta;
                }
                else if (LabelLeyenda.Text == "Lecturas Imposibles")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, P.Apellido, M.Numero as Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "WHERE C.ImpresionOBS = 609 AND M.ActualFecha" +
                              " BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                               DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + Ruta;
                }
                else if (LabelLeyenda.Text == "Saldos")
                {
                    CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                        "C.titularID AS IC, C.DomicSumin, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                        "C.Operario, " +
                        "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                        "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                        "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                        "WHERE C.ImpresionOBS = 800 AND C.Remesa = " + CBRemesa.Text + Ruta;
                }
                else if (LabelLeyenda.Text == "Todos")
                {
                    CONSULTANOIMPRESOS = "SELECT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                              "C.titularID AS IC, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                              "if(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', E.Titulo)) Situacion, C.Operario, " +
                              "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                              "FROM Conexiones C " +
                              "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                              "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
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
                              "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                              "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                              "WHERE((C.ImpresionOBS >= 601) " +
                              "AND M.ActualFecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" + DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' " +
                               Ruta + " AND C.Remesa = " + CBRemesa.Text + ") " +
                              "OR (C.ImpresionOBS = 800 OR C.ImpresionOBS = 500 or C.ImpresionOBS = 400) AND C.Remesa = " + CBRemesa.Text + Ruta;
                }
                else if (ImpresionOBS == "Errores")
                {
                    CONSULTANOIMPRESOS = "SELECT Fecha, Hora, Periodo, Equipo, Ruta, Lecturista, ConexionID AS NInstalacion, CodigoError, TextoError " +
                       "FROM LogErrores WHERE Fecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" + DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' " + Ruta + " ORDER BY Fecha ASC, Hora ASC";
                }

            }
            else if (ImpresionOBS.Length == 1)//Indicados para no Imprimir ImpresionCOD = 1
            {
                CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                                     "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, M.AnteriorEstado, " +
                                     "M.ActualEstado, C.ConsumoFacturado,  M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                                     "C.Operario, " +
                                     "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
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
                                     "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                                     "ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                     "WHERE " +
                                     "C.ImpresionCOD = " + ImpresionOBS + " AND M.ActualFecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" +
                                     DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' AND C.Remesa = " + CBRemesa.Text + Ruta;
            }



            NoImpr = true;
            LabelLeyenda.Visible = true;
            LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString();
            LabelPeriodo.Visible = true;

            CargarTablaPreDescarga();

            this.Cursor = Cursors.Default;
        }

        private void DTPDesdeTomLect_KeyPress(object sender, KeyPressEventArgs e)
        {
           
                //ConsultaPorFechaTomaLect();
           
        }

        private void DTPHastaTomLect_KeyPress(object sender, KeyPressEventArgs e)
        {
            //ConsultaPorFechaTomaLect();
        }

        private void DTPDesdeTomLect_ValueChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
              
                LVResumenGral.Items.Clear();
                ConsultaPorFechaTomaLect();
               
            }
            
        }

        private void DTPHastaTomLect_ValueChanged(object sender, EventArgs e)
        {           
            
            if (Visible == true)
            {               
                LVResumenGral.Items.Clear();
                ConsultaPorFechaTomaLect();
               
            }
        }

        private void contarOrdenativosToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (DGResumenExp.RowCount > 0)
            {
                this.Cursor = Cursors.WaitCursor;
                SaveFileDialog fichero = new SaveFileDialog();
                //fichero.Filter = "Excel (*.xls)|*.xls";
                fichero.Filter = "Texto (*.txt)|*.txt";
                Vble.TipoInforme = "TXT";
                if (fichero.ShowDialog() == DialogResult.OK)
                {
                    Vble.FileName = fichero.FileName;
                    
                    bgwExpExcel.RunWorkerAsync();

                }

                this.Cursor = Cursors.Default;

            }
            else
            {
                MessageBox.Show("Disculpe No existen datos en la tabla para realizar la Exportación, verifique la busqueda " +
                    "o tal vez aún no existen usuarios para los datos filtrados", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// Exporta la tabla del resumen por remesa a excel y a pdf simultaneamente.
        /// </summary>
        /// <param name="grd"></param>
        private void ExportarExcelPorRemesa(DataGridView grd)
        {
            int k = 0;
           
                string[,] sArray;
                sArray = new string[grd.Rows.Count, grd.Columns.Count];          

                //Microsoft.Office.Interop.Excel.Application aplicacion;
                //Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                //Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                //aplicacion = new Microsoft.Office.Interop.Excel.Application();
                //libros_trabajo = aplicacion.Workbooks.Add();
                //hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);


            ////Agregamos los encabezados de la tabla altas en negrita y con fondo gris
            //for (int j = 1; j <= grd.Columns.Count; j++)
            //{
            //    hoja_trabajo.Cells[1, j] = grd.Columns[j - 1].HeaderText;
            //    hoja_trabajo.Cells[1, j].Font.Bold = true;
            //    hoja_trabajo.Cells[1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
            //}

                //Agrego el encabezado
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    sArray[1, j] = grd.Columns[j ].HeaderText;
                    //hoja_trabajo.Cells[1, j].Font.Bold = true;
                    //hoja_trabajo.Cells[1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
                    Vble.LineaExportartxt += grd.Columns[j].HeaderText + "|";
            }
            Vble.LineaExportartxt += "\n";
            //armo el array con el contenido del grid
            for (int i = 0; i <= grd.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                    if (grd.Rows[i].Cells[j].Value != null)
                    {
                        sArray[i, j] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                      
                    

                }
                   
            }
                //for (int i = 0; i <= grd.Rows.Count - 1; i++)
                //{
                //    for (int j = 0; j < grd.Columns.Count; j++)
                //    {
                //        hoja_trabajo.Cells[i + 2, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                //    }
                //}

                //Inserto los datos del array a las celdas del excel
                for (int f = 0; f < grd.Rows.Count; f++)
                {
                    
                    for (int n = 0; n < grd.Columns.Count; n++)
                        {
                            //hoja_trabajo.Cells[f, n] = sArray[f - 1, n - 1];
                            Vble.LineaExportartxt += sArray[f, n] + "|";
                        }
                    Vble.LineaExportartxt += "\n";
                bgwExpExcel.ReportProgress(k);
                k++;
            }

               Vble.LineaExportartxt += "\n\n" + toolStripTotalizador;

            Vble.LineaExportartxt += "\n";
            if (LabelLeyenda.Text == "lblOrdenativosR")
            {
                for (int i = 0; i < listBoxOrd.Items.Count; i++)
                {
                    Vble.LineaExportartxt += "\n" + listBoxOrd.Items[i].ToString() ;
                }
                
            }
            


            Vble.CreateInfoCarga(Vble.FileName, Vble.FileName, Vble.LineaExportartxt);


                //libros_trabajo.SaveAs(Vble.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                //string NombreArchivo = System.IO.Path.GetFileName(Vble.FileName);
                //libros_trabajo.Close(true);
                //aplicacion.Quit();




                ////Exporto a un archivo Excel que queda como respaldo de FIS 
                //ExportarToPDF(grd, NombreArchivo, fichero.FileName);


                //if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
                //{
                //    Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
                //    File.Copy(Vble.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo, true);
                //}
                //else
                //{
                //    File.Copy(Vble.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo, true);
                //}

            //}
        }



        /// <summary>
        /// Exporta la tabla del resumen por remesa a excel y a pdf simultaneamente.
        /// </summary>
        /// <param name="grd"></param>
        private void ExportarExcel(DataGridView grd)
        {
            int k = 0;
            int nroFilaExcel = 0; 
            string[,] sArray;
            string[,] sArrayResGral;
            int filasResGral = 0;
            int columnasResGral = 0;
            sArray = new string[grd.Rows.Count, grd.Columns.Count];
            nroFilaExcel = grd.Rows.Count + 2 ;           

            ///Almaceno en variables locales la cantidad de filas y columnas que tiene el resumen general para luego recorrer
            ///y generar en el excel su cuadro correspondiente.
            foreach (ListViewItem item in LVResumenGral.Items)
            {
                filasResGral++;
            }

            filasResGral++;
            columnasResGral = LVResumenGral.Columns.Count;
            sArrayResGral = new string[filasResGral, columnasResGral];
            Microsoft.Office.Interop.Excel.Application aplicacion;
            Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
            Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo1;
            Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo2;
            Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo3;
            aplicacion = new Microsoft.Office.Interop.Excel.Application();
            libros_trabajo = aplicacion.Workbooks.Add();
            libros_trabajo.Sheets.Add();
            libros_trabajo.Sheets.Add();


            hoja_trabajo1 = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
            hoja_trabajo2 = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(2);
            hoja_trabajo3 = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(3);

            //Agrego el encabezado
            for (int j = 0; j < grd.Columns.Count; j++)
            {
                sArray[0, j] = grd.Columns[j].HeaderText;
                hoja_trabajo1.Cells[1, j+1].Font.Bold = true;
                hoja_trabajo1.Cells[1, j+1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);              
             
            }
            //Vble.LineaExportartxt += "\n";
            //armo el array con el contenido del grid
            for (int i = 1; i <= grd.Rows.Count - 1; i++)
            {
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    if (grd.Rows[i].Cells[j].Value != null)
                    {
                        sArray[i, j] = grd.Rows[i - 1].Cells[j].Value.ToString();
                    }
                                 
                }
            }

            for (int i = 0; i <= grd.Rows.Count - 1; i++)
            {
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    if (grd.Rows[i].Cells[j].Value != null)
                    {
                        hoja_trabajo1.Cells[i + 2, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }

            //Inserto los datos del array a las celdas del excel
            for (int f = 1; f < grd.Rows.Count; f++)
            {

                for (int n = 1; n <= grd.Columns.Count; n++)
                {
                    hoja_trabajo1.Cells[f, n] = sArray[f - 1, n - 1];
                    //Vble.LineaExportartxt += sArray[f, n] + "|";
                }
                //Vble.LineaExportartxt += "\n";
                bgwExpExcel.ReportProgress(k);
                k++;
            }

            ///Se agregan las tablas de los graficos en hojas seperadas
            ///
             //Agrego el encabezado de la tabla Para los Leidos e Impresos
            for (int j = 0; j <= zonasGrafico.Count; j++)
            {
                //sArray[0, j] = grd.Columns[j].HeaderText;
                hoja_trabajo2.Cells[1, j+2].Font.Bold = true;
                hoja_trabajo2.Cells[1, j+2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);

                //Vble.LineaExportartxt += grd.Columns[j].HeaderText + "|";
            }
            hoja_trabajo2.Cells[2, 1] = "TOTAL";
            hoja_trabajo2.Cells[3, 1] = "LEIDOS";
            hoja_trabajo2.Cells[4, 1] = "IMPRESOS";
            for (int j = 0; j < leidosGrafico.Count; j++)
                {
                    hoja_trabajo2.Cells[1, j+2] = zonasGrafico[j].ToString();
                    hoja_trabajo2.Cells[2, j+2] = TotalZonasGrafico[j].ToString();
                    hoja_trabajo2.Cells[3, j+2] = leidosGrafico[j].ToString();
                    hoja_trabajo2.Cells[4, j+2] = impresosGrafico[j].ToString();
            }

            //Agrego el encabezado de la tabla Total Impresos por Periodo
            for (int j = 0; j <= PerGrafTot.Count; j++)
            {
                //sArray[0, j] = grd.Columns[j].HeaderText;
                hoja_trabajo3.Cells[1, j + 2].Font.Bold = true;
                hoja_trabajo3.Cells[1, j + 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);

                //Vble.LineaExportartxt += grd.Columns[j].HeaderText + "|";
            }
            
            hoja_trabajo3.Cells[2, 1] = "IMPRESOS";
            for (int j = 0; j < PerImprTot.Count; j++)
            {
                hoja_trabajo3.Cells[1, j + 2] = PerGrafTot[j].ToString();
                hoja_trabajo3.Cells[2, j + 2] = PerImprTot[j].ToString();
               
            }

            ////Parte que agrega el resumen general cuantitativo debajo del resumen detallado.         
            int Cant = LVResumenGral.Columns.Count;           
            //Agrego el encabezado
            foreach (ListViewItem item in LVResumenGral.Items)
            {
                for (int j = 0; j < Cant; j++)
                {
                    sArrayResGral[0, j] = LVResumenGral.Columns[j].Text;
                    
                    hoja_trabajo1.Cells[nroFilaExcel, j + 1].Font.Bold = true;
                    hoja_trabajo1.Cells[nroFilaExcel, j + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);                    
                }            
            }
          
            int y = 1;
            //armo el array con el contenido del grid
            foreach (ListViewItem item in LVResumenGral.Items)
            {              
                    for (int j = 0; j < LVResumenGral.Columns.Count; j++)
                    {
                        sArrayResGral[y, j] = item.SubItems[j].Text;
                    }                
                y++;
            }
            
            foreach (ListViewItem item in LVResumenGral.Items)
            {              
                 for (int j = 0; j < LVResumenGral.Columns.Count; j++)
                {
                 //hoja_trabajo1.Cells[nroFilaExcel  + 2, j + 1] = item.SubItems[j].Text;
                   hoja_trabajo1.Cells[nroFilaExcel + 2, j + 1] = sArrayResGral[0, j];
                }
               
            }

            int l = nroFilaExcel;
            int p = 1;
            //Inserto los datos del array a las celdas del excel
            for (int n = 1; n <= LVResumenGral.Columns.Count; n++)
            {
                hoja_trabajo1.Cells[l, n] = sArrayResGral[0, n - 1];
                //Vble.LineaExportartxt += sArray[f, n] + "|";
            }

            for (int f = 0; f < filasResGral; f++)
            {
                for (int n = 1; n <= LVResumenGral.Columns.Count; n++)
                    {
                        hoja_trabajo1.Cells[l, n] = sArrayResGral[f, n - 1];
                        //Vble.LineaExportartxt += sArray[f, n] + "|";
                    }               
                l++;
                p++;
            }

            object misValue = System.Reflection.Missing.Value;
            object misValue2 = System.Reflection.Missing.Value;
            object misValue3 = System.Reflection.Missing.Value;

            if (TipoInforme == "R")
            {            
                ///Generación del Chart grafico TODOS donde se muestran los impresos, leidos y motivos de no impresion
                Excel.Range chartRange;
                Excel.ChartObjects xlChart = (Excel.ChartObjects)hoja_trabajo1.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlChart.Add(100, 225, 900, 400);            
                Excel.Chart chartPage = myChart.Chart;
               
                int filas = DGResumenExp.Rows.Count + 1;
                string CeldaHastaExcel = "U" + filas.ToString();
                chartRange = hoja_trabajo1.get_Range("C1", CeldaHastaExcel);
                chartPage.SetSourceData(chartRange, misValue);
                chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
                chartPage.HasTitle = true;
                chartPage.ChartTitle.Font.Size = 10;
                chartPage.ChartTitle.Text = "Detalle total \n " + LabelPeriodo.Text + " - " + leyenda.Text;
                //chartPage.Export(@"C:\Users\operario\Desktop\Excel.bmp", "BMP", misValue);
            }

            ///Generacion del Chart grafico Todos, Leidos e impresos solamente por Localidad
            Excel.Range chartRange2;
            Excel.ChartObjects xlChart2 = (Excel.ChartObjects)hoja_trabajo2.ChartObjects(Type.Missing);
            Excel.ChartObject myChart2 = (Excel.ChartObject)xlChart2.Add(50, 100, 900, 400);
            Excel.Chart chartPage2 = myChart2.Chart;
           
            string CeldaHastaExcel2 = "";
            int filas2 = zonasGrafico.Count;
            switch (filas2)
            {
                case 1:
                    CeldaHastaExcel2 = "B";
                    break;
                case 2:
                    CeldaHastaExcel2 = "C";
                    break;
                case 3:
                    CeldaHastaExcel2 = "D";
                    break;
                case 4:
                    CeldaHastaExcel2 = "E";
                    break;
                case 5:
                    CeldaHastaExcel2 = "F";
                    break;
                case 6:
                    CeldaHastaExcel2 = "G";
                    break;
                case 7:
                    CeldaHastaExcel2 = "H";
                    break;
                case 8:
                    CeldaHastaExcel2 = "I";
                    break;
                case 9:
                    CeldaHastaExcel2 = "J";
                    break;
                case 10:
                    CeldaHastaExcel2 = "K";
                    break;
                case 11:
                    CeldaHastaExcel2 = "L";
                    break;
                case 12:
                    CeldaHastaExcel2 = "M";
                    break;
                case 13:
                    CeldaHastaExcel2 = "N";
                    break;
                case 14:
                    CeldaHastaExcel2 = "O";
                    break;
                case 15:
                    CeldaHastaExcel2 = "P";
                    break;
                case 16:
                    CeldaHastaExcel2 = "Q";
                    break;
                case 17:
                    CeldaHastaExcel2 = "R";
                    break;
                case 18:
                    CeldaHastaExcel2 = "S";
                    break;
                case 19:
                    CeldaHastaExcel2 = "T";
                    break;
                case 20:
                    CeldaHastaExcel2 = "U";
                    break;
                case 21:
                    CeldaHastaExcel2 = "V";
                    break;
                case 22:
                    CeldaHastaExcel2 = "W";
                    break;
                case 23:
                    CeldaHastaExcel2 = "X";
                    break;
                case 24:
                    CeldaHastaExcel2 = "Y";
                    break;
                case 25:
                    CeldaHastaExcel2 = "Z";
                    break;
                default:
                    CeldaHastaExcel2 = "AK";
                    break;
            }            
            CeldaHastaExcel2 = CeldaHastaExcel2 + "4";
            chartRange2 = hoja_trabajo2.get_Range("A1", CeldaHastaExcel2);
            chartPage2.SetSourceData(chartRange2, misValue2);
            chartPage2.ChartType = Excel.XlChartType.xlColumnClustered;
            chartPage2.HasTitle = true;
            chartPage2.ChartTitle.Font.Size = 10;
            chartPage2.ChartTitle.Text = "Total, Leidos e Impresos por Localidad \n " + LabelPeriodo.Text + " - " + leyenda.Text;
            //chartPage2.Export(@"C:\Users\operario\Desktop\Excel2.bmp", "BMP", misValue2);

            ///Generacion del Chart grafico CANTIDAD IMPRESOS por Periodo, cada periodo contiene todas las remesas
            Excel.Range chartRange3;
            Excel.ChartObjects xlChart3 = (Excel.ChartObjects)hoja_trabajo3.ChartObjects(Type.Missing);
            Excel.ChartObject myChart3 = (Excel.ChartObject)xlChart3.Add(50, 100, 900, 400);
            Excel.Chart chartPage3 = myChart3.Chart;
            
            string CeldaHastaExcel3 = "";
            int filas3 = PerGrafTot.Count;
            switch (filas3)
            {
                case 1:
                    CeldaHastaExcel3 = "B";
                    break;
                case 2:
                    CeldaHastaExcel3 = "C";
                    break;
                case 3:
                    CeldaHastaExcel3 = "D";
                    break;
                case 4:
                    CeldaHastaExcel3 = "E";
                    break;
                case 5:
                    CeldaHastaExcel3 = "F";
                    break;
                case 6:
                    CeldaHastaExcel3 = "G";
                    break;
                case 7:
                    CeldaHastaExcel3 = "H";
                    break;
                case 8:
                    CeldaHastaExcel3 = "I";
                    break;
                case 9:
                    CeldaHastaExcel3 = "J";
                    break;
                case 10:
                    CeldaHastaExcel3 = "K";
                    break;
                case 11:
                    CeldaHastaExcel3 = "L";
                    break;
                case 12:
                    CeldaHastaExcel3 = "M";
                    break;
                case 13:
                    CeldaHastaExcel3 = "N";
                    break;
                case 14:
                    CeldaHastaExcel3 = "O";
                    break;
                case 15:
                    CeldaHastaExcel3 = "P";
                    break;
                case 16:
                    CeldaHastaExcel3 = "Q";
                    break;
                case 17:
                    CeldaHastaExcel3 = "R";
                    break;
                case 18:
                    CeldaHastaExcel3 = "S";
                    break;
                case 19:
                    CeldaHastaExcel3 = "T";
                    break;
                case 20:
                    CeldaHastaExcel3 = "U";
                    break;
                case 21:
                    CeldaHastaExcel3 = "V";
                    break;
                case 22:
                    CeldaHastaExcel3 = "W";
                    break;
                case 23:
                    CeldaHastaExcel3 = "X";
                    break;
                case 24:
                    CeldaHastaExcel3 = "Y";
                    break;
                case 25:
                    CeldaHastaExcel3 = "Z";
                    break;
                default:
                    CeldaHastaExcel3 = "AK";
                    break;
            }

            CeldaHastaExcel3 = CeldaHastaExcel3 + "2";
            chartRange3 = hoja_trabajo3.get_Range("A1", CeldaHastaExcel3);
            chartPage3.SetSourceData(chartRange3, misValue3);
            chartPage3.ChartType = Excel.XlChartType.xlLineMarkers;
            chartPage3.HasTitle = true;
            chartPage3.ChartTitle.Font.Size = 10;
            chartPage3.ChartTitle.Text = "IMPRESOS POR Periodo";
            //chartPage2.Export(@"C:\Users\operario\Desktop\Excel2.bmp", "BMP", misValue2);


            libros_trabajo.SaveAs(Vble.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);        
            string NombreArchivo = System.IO.Path.GetFileName(Vble.FileName);
            libros_trabajo.Close(true);
            aplicacion.Quit();

            ////Exporto a un archivo Excel que queda como respaldo de FIS 
            //ExportarToPDF(grd, NombreArchivo, fichero.FileName);
            //if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
            //{
            //    Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
            //    File.Copy(Vble.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo, true);
            //}
            //else
            //{
            //    File.Copy(Vble.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo, true);
            //}

            
        }


      


        private void ExportarAltasExcel(DataGridView grd)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                //Agregamos los encabezados de la tabla altas en negrita y con fondo gris
                for (int j = 1; j <= grd.Columns.Count; j++)
                {
                    hoja_trabajo.Cells[1, j] = grd.Columns[j - 1].HeaderText;
                    hoja_trabajo.Cells[1, j].Font.Bold = true;
                    hoja_trabajo.Cells[1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
                }


                for (int i = 0; i <= grd.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        if (grd.Rows[i].Cells[j].Value != null)
                        {
                            hoja_trabajo.Cells[i + 2, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                }

                libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                string NombreArchivo = System.IO.Path.GetFileName(fichero.FileName);

                libros_trabajo.Close(true);
                aplicacion.Quit();

                //Exporto a un archivo Excel que queda como respaldo de FIS 
                ExportarToExcel(grd, NombreArchivo, fichero.FileName);


                if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
                {
                    Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));                   
                    File.Copy(fichero.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo,true);
                }
                else
                {
                    File.Copy(fichero.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo, true);
                }

            }
        }

        private void ExportarToExcel(DataGridView grd, string NombreArchivo, string SavePath)
        {
            DataTable Tabla = new DataTable();

            //Creo el docuemento .pdf con el formato especificado
            Document document = new Document(PageSize.A4);
            //Gira la hoja en posicion horizontal
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            string PathInformesAltas = Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo;

            if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
            {
                Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
            }
            //PdfWriter.GetInstance(document, new FileStream(SavePath + ".pdf", FileMode.OpenOrCreate));
            PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
            document.Open();

            PdfWriter wri = PdfWriter.GetInstance(document, new FileStream(SavePath + ".pdf", FileMode.OpenOrCreate));
            //PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
            wri.PageEvent = new PageEventHelper();
            wri.Open();
            document.Open();

            // ********************Creamos la imagen de DPEC y le ajustamos el tamaño
            iTextSharp.text.Image imagenDPEC = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
            imagenDPEC.BorderWidth = 0;
            //imagenDPEC.Alignment = Element.ALIGN_RIGHT;

            //imagenDPEC.SetAbsolutePosition(40f, 790f);  posicion de imagen para hoja vertical
            imagenDPEC.SetAbsolutePosition(40f, 510f);//  posicion de imagen para hoja horizontal
            float percentage1 = 0.0f;                 //  
            percentage1 = 70 / imagenDPEC.Width;      //  Edito tamaño de imagen
            imagenDPEC.ScalePercent(percentage1 * 100);//

            //*******************Creamos la imagen de MacroIntell y le ajustamos el tamaño
            iTextSharp.text.Image imagenMINTELL = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\MacroIntell Isologo.jpg");
            imagenMINTELL.BorderWidth = 0;
            //imagenMINTELL.Alignment = Element.ALIGN_LEFT;
            //imagenMINTELL.SetAbsolutePosition(500f, 790f);  posicion de imagen para hoja vertical
            imagenMINTELL.SetAbsolutePosition(750f, 530f);// posicion de imagen para hoja horizontal
            float percentage2 = 0.0f;                     //
            percentage2 = 50 / imagenMINTELL.Width;       //edito Tamaño de imagen
            imagenMINTELL.ScalePercent(percentage2 * 100);//
                                                          //*************************************************************************************************************************

            //datos del informe                       
            document.Add(imagenMINTELL);
            document.Add(imagenDPEC);
            document.Add(new Paragraph("  "));
            Chunk chunk = new Chunk();
            if (PantallaSolicitud == "Exportacion")
            {
                chunk = new Chunk("         Informe en Excel \n\n         Periodo " + LabelPeriodo.Text + " \n\n   " + " Ruta: " + TextBoxRuta.Text,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            }
            else
            {
                chunk = new Chunk("         Informe en Excel \n\n         Periodo " + LabelPeriodo.Text,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            }
            //chunk.SetUnderline(0.9f, -1.8f);
            Paragraph titulo = new Paragraph();
            titulo.Add(chunk);
            titulo.Alignment = Element.ALIGN_CENTER;
            document.Add(new Paragraph(titulo));


            Paragraph infoinforme = new Paragraph("Fecha: " + DateTime.Today.ToString("dd/MM/yyyy") + "\n Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
            infoinforme.Alignment = Element.ALIGN_RIGHT;
            document.Add(new Paragraph(infoinforme));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            //declaracion de tabla para volcar datos de descargas
            iTextSharp.text.Rectangle page = document.PageSize;
            PdfPTable table = new PdfPTable(20);
            //table.WidthPercentage = 180;
            //table.TotalWidth = page.Width - 90;
            //table.LockedWidth = true;


            if (LabelLeyenda.Text == "Leidas NO impresas")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f};
                table.SetWidths(widths);
                //Periodo|Ruta|NInstalacion|Contrato|IC|Apellido|Medidor|AnteriorEstado|ActualEstado|ConsumoFacturado|Fecha|Hora|Titulo|Operario|Ord1|Ord2|Ord3|Ord4|Ord5|Observaciones               
            }
            else if (LabelLeyenda.Text == "Impresas")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, " +
                //          "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
              
            }
            else if (LabelLeyenda.Text == "NO impresos Fuera de Rango")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.TitularID AS IC, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //          "C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
              
            }
            else if (LabelLeyenda.Text == "Lecturas Imposibles")
            {
                table = new PdfPTable(19);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 2.5f};
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.titularID AS IC, P.Apellido, M.Numero as Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
              
            }
            else if (LabelLeyenda.Text == "Saldos")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //    "C.titularID AS IC, C.DomicSumin, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //    "C.Operario, " +
                //    "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
              
            }
            else if (LabelLeyenda.Text == "TodosR")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.8f, 1.2f, 2.0f, 2.0f, 2.0f, 4.0f, 1.8f, 2.0f, 2.0f, 1.4f, 1.6f, 2.0f, 2.2f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 2.3f, 1.3f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.titularID AS IC, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //          "if(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', E.Titulo)) Situacion, C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                //          "FROM Conexiones C " +
              
            }
            else if (ImpresionOBS == "Errores")
            {
                table = new PdfPTable(9);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f};
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT Fecha, Hora, Periodo, Equipo, Ruta, Lecturista, ConexionID AS NInstalacion, CodigoError, TextoError " +
                //   "FROM LogErrores WHERE Fecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" + DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' " + Ruta + " ORDER BY Fecha ASC, Hora ASC";
            }        
            else if (ImpresionOBS.Length == 1)//Indicados para no Imprimir ImpresionCOD = 1
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.8f, 1.2f, 2.0f, 2.0f, 2.0f, 4.0f, 1.8f, 2.0f, 2.0f, 1.4f, 1.6f, 2.0f, 2.2f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 2.3f, 1.3f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //                     "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, M.AnteriorEstado, " +
                //                     "M.ActualEstado, C.ConsumoFacturado,  M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //                     "C.Operario, " +
                //                     "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                
            }

            ////Estructura de tabla:
            ////Periodo|Ruta|Instalacion|Contrato|IC|Apellido|Numero|AnteriorEstado|Fecha|Hora|Estado Actual|ConsumoFacturado|Situacion|Ord1|Ord2|Ord3|Ord4|Ord5|Observaciones|Lecturista
            PdfPCell Periodo = (new PdfPCell(new Paragraph("Periodo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Periodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Periodo);
            PdfPCell Ruta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ruta);
            PdfPCell Instalacion = (new PdfPCell(new Paragraph("Instalacion", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Instalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Instalacion);
            PdfPCell Contrato = (new PdfPCell(new Paragraph("Contrato", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Contrato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Contrato);
            PdfPCell IC = (new PdfPCell(new Paragraph("IC", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            IC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(IC);
            PdfPCell Apellido = (new PdfPCell(new Paragraph("Apellido", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Apellido.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Apellido);
            PdfPCell Numero = (new PdfPCell(new Phrase("Numero ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Numero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Numero);
            PdfPCell AnteriorEstado = (new PdfPCell(new Phrase("AnteriorEstado ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            AnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(AnteriorEstado);
            PdfPCell Fecha = (new PdfPCell(new Paragraph("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Fecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Fecha);
            PdfPCell Hora = (new PdfPCell(new Paragraph("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Hora.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Hora);           
            PdfPCell SinLeer = (new PdfPCell(new Paragraph("Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            SinLeer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(SinLeer);
            PdfPCell Consumo = (new PdfPCell(new Paragraph("Consumo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Consumo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Consumo);
            PdfPCell Situacion = (new PdfPCell(new Paragraph("Situacion", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Situacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Situacion);
            PdfPCell Ord1 = (new PdfPCell(new Paragraph("1", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord1);
            PdfPCell Ord2 = (new PdfPCell(new Paragraph("2", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord2);
            PdfPCell Ord3 = (new PdfPCell(new Paragraph("3", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord3);
            PdfPCell Ord4 = (new PdfPCell(new Paragraph("4", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord4);
            PdfPCell Ord5 = (new PdfPCell(new Paragraph("5", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord5);
            PdfPCell Observaciones = (new PdfPCell(new Paragraph("Obs", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Observaciones.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Observaciones);
            PdfPCell Activa = (new PdfPCell(new Phrase("Lect", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Activa.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Activa);

            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            foreach (DataGridViewRow fi in grd.Rows)
            {
                ////Estructura de tabla:
                ////Periodo|Ruta|Instalacion|Contrato|IC|Apellido|Numero|AnteriorEstado|Fecha|Hora|Estado Actual|Ord1|Ord2|Ord3|Ord4|Ord5|Observaciones|Lecturista
                PdfPCell fiPeriodo = (new PdfPCell(new Paragraph(fi.Cells["Periodo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiPeriodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiPeriodo);
                PdfPCell fi2 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi2);
                PdfPCell fiInstalacion = (new PdfPCell(new Paragraph(fi.Cells["NInstalacion"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiInstalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiInstalacion);
                PdfPCell fiContrato = (new PdfPCell(new Paragraph(fi.Cells["Contrato"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiContrato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiContrato);
                PdfPCell fiIC = (new PdfPCell(new Paragraph(fi.Cells["IC"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiIC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiIC);
                PdfPCell fiApellido = (new PdfPCell(new Paragraph(fi.Cells["Apellido"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiApellido.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiApellido);
                PdfPCell fiNumero = (new PdfPCell(new Paragraph(fi.Cells["Medidor"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiNumero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiNumero);
                PdfPCell fiAnteriorEstado = (new PdfPCell(new Paragraph(fi.Cells["AnteriorEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiAnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiAnteriorEstado);
                PdfPCell fi3 = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi3);
                PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Hora"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi4);
                PdfPCell fEstadoActual = (new PdfPCell(new Paragraph(fi.Cells["ActualEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fEstadoActual.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fEstadoActual);
                PdfPCell fConsumoFacturado = (new PdfPCell(new Paragraph(fi.Cells["ConsumoFacturado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fConsumoFacturado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fConsumoFacturado);
                PdfPCell fSituacion = (new PdfPCell(new Paragraph(fi.Cells["Situacion"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fSituacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fSituacion);
                PdfPCell fOrd1 = (new PdfPCell(new Paragraph(fi.Cells["Ord1"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd1);
                PdfPCell fOrd2 = (new PdfPCell(new Paragraph(fi.Cells["Ord2"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd2);
                PdfPCell fOrd3 = (new PdfPCell(new Paragraph(fi.Cells["Ord3"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd3);
                PdfPCell fOrd4 = (new PdfPCell(new Paragraph(fi.Cells["Ord4"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd4);
                PdfPCell fOrd5 = (new PdfPCell(new Paragraph(fi.Cells["Ord5"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd5);   
                PdfPCell fi9 = (new PdfPCell(new Paragraph(fi.Cells["Observaciones"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                table.AddCell(fi9);
                PdfPCell fi10 = (new PdfPCell(new Paragraph(fi.Cells["Operario"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi10);
            }

            document.Add(new Paragraph(" "));
            document.Add(table);

            Chunk CantidadRegistros = new Chunk("Total = " + grd.Rows.Count.ToString(),
                                               FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD,
                                               new iTextSharp.text.BaseColor(0, 0, 0)));
            Paragraph Total = new Paragraph(CantidadRegistros);
            Total.Alignment = Element.ALIGN_RIGHT;


            wri.Add(imagenMINTELL);
            wri.Add(imagenDPEC);
            wri.Add(new Paragraph("  "));
            wri.Add(titulo);
            wri.Add(new Paragraph(""));
            wri.Add(new Paragraph(infoinforme));
            wri.Add(new Paragraph(""));
            wri.Add(table);
            wri.Add(Total);
            //wri.Close();
            document.Add(Total);

            document.Add(new Paragraph(" "));
            document.Close();

        }


        private void ExportarToPDF(DataGridView grd, string NombreArchivo, string SavePath)
        {
            DataTable Tabla = new DataTable();

            //Creo el docuemento .pdf con el formato especificado
            Document document = new Document(PageSize.A4);
            //Gira la hoja en posicion horizontal
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            string PathInformesAltas = Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo;

            if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
            {
                Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
            }
            //PdfWriter.GetInstance(document, new FileStream(SavePath + ".pdf", FileMode.OpenOrCreate));
            PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
            document.Open();

            PdfWriter wri = PdfWriter.GetInstance(document, new FileStream(SavePath + ".pdf", FileMode.OpenOrCreate));
            //PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
            wri.PageEvent = new PageEventHelper();
            wri.Open();
            document.Open();

            // ********************Creamos la imagen de DPEC y le ajustamos el tamaño
            iTextSharp.text.Image imagenDPEC = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
            imagenDPEC.BorderWidth = 0;
            //imagenDPEC.Alignment = Element.ALIGN_RIGHT;

            //imagenDPEC.SetAbsolutePosition(40f, 790f);  posicion de imagen para hoja vertical
            imagenDPEC.SetAbsolutePosition(40f, 510f);//  posicion de imagen para hoja horizontal
            float percentage1 = 0.0f;                 //  
            percentage1 = 70 / imagenDPEC.Width;      //  Edito tamaño de imagen
            imagenDPEC.ScalePercent(percentage1 * 100);//

            //*******************Creamos la imagen de MacroIntell y le ajustamos el tamaño
            iTextSharp.text.Image imagenMINTELL = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\MacroIntell Isologo.jpg");
            imagenMINTELL.BorderWidth = 0;
            //imagenMINTELL.Alignment = Element.ALIGN_LEFT;
            //imagenMINTELL.SetAbsolutePosition(500f, 790f);  posicion de imagen para hoja vertical
            imagenMINTELL.SetAbsolutePosition(750f, 530f);// posicion de imagen para hoja horizontal
            float percentage2 = 0.0f;                     //
            percentage2 = 50 / imagenMINTELL.Width;       //edito Tamaño de imagen
            imagenMINTELL.ScalePercent(percentage2 * 100);//
                                                          //*************************************************************************************************************************

            //datos del informe                       
            document.Add(imagenMINTELL);
            document.Add(imagenDPEC);
            document.Add(new Paragraph("  "));
            Chunk chunk = new Chunk();
            if (PantallaSolicitud == "Exportacion")
            {
                chunk = new Chunk("         Informe en Excel \n\n         " + LabelPeriodo.Text + " \n\n   " + " Ruta " + TextBoxRuta.Text,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            }
            else
            {
                chunk = new Chunk("         Informe en Excel \n\n         " + LabelPeriodo.Text,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            }
            //chunk.SetUnderline(0.9f, -1.8f);
            Paragraph titulo = new Paragraph();
            titulo.Add(chunk);
            titulo.Alignment = Element.ALIGN_CENTER;
            document.Add(new Paragraph(titulo));


            Paragraph infoinforme = new Paragraph("Fecha: " + DateTime.Today.ToString("dd/MM/yyyy") + "\n Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
            infoinforme.Alignment = Element.ALIGN_RIGHT;
            document.Add(new Paragraph(infoinforme));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            //declaracion de tabla para volcar datos de descargas
            iTextSharp.text.Rectangle page = document.PageSize;
            PdfPTable table = new PdfPTable(20);
            //table.WidthPercentage = 180;
            //table.TotalWidth = page.Width - 90;
            //table.LockedWidth = true;


            if (LabelLeyenda.Text == "Leidas NO impresas")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //Periodo|Ruta|NInstalacion|Contrato|IC|Apellido|Medidor|AnteriorEstado|ActualEstado|ConsumoFacturado|Fecha|Hora|Titulo|Operario|Ord1|Ord2|Ord3|Ord4|Ord5|Observaciones               
            }
            else if (LabelLeyenda.Text == "Impresas")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, " +
                //          "M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +

            }
            else if (LabelLeyenda.Text == "NO impresos Fuera de Rango")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.TitularID AS IC, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //          "C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +

            }
            else if (LabelLeyenda.Text == "Lecturas Imposibles")
            {
                table = new PdfPTable(19);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 2.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.titularID AS IC, P.Apellido, M.Numero as Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +

            }
            else if (LabelLeyenda.Text == "Saldos")
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.3f, 1.3f, 2.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 4.5f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = " SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //    "C.titularID AS IC, C.DomicSumin, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //    "C.Operario, " +
                //    "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +

            }
            else if (LabelLeyenda.Text == "Todos")
            {
                table = new PdfPTable(15);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.5f, 1.5f, 4.0f, 1.5f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.2f, 2.0f, 2.0f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //          "C.titularID AS IC, P.Apellido, M.Numero AS Medidor, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado, M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //          "if(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', E.Titulo)) Situacion, C.Operario, " +
                //          "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                //          "FROM Conexiones C " +

            }
            else if (ImpresionOBS == "Errores")
            {
                table = new PdfPTable(9);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.2f, 1.0f, 1.5f, 1.5f, 1.5f, 4.5f, 1.5f, 1.0f, 1.0f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT Fecha, Hora, Periodo, Equipo, Ruta, Lecturista, ConexionID AS NInstalacion, CodigoError, TextoError " +
                //   "FROM LogErrores WHERE Fecha BETWEEN '" + DTPDesdeTomLect.Value.ToString("yyyy-MM-dd") + "' AND '" + DTPHastaTomLect.Value.ToString("yyyy-MM-dd") + "' " + Ruta + " ORDER BY Fecha ASC, Hora ASC";
            }
            else if (ImpresionOBS.Length == 1)//Indicados para no Imprimir ImpresionCOD = 1
            {
                table = new PdfPTable(20);
                table.WidthPercentage = 180;
                table.TotalWidth = page.Width - 90;
                table.LockedWidth = true;
                float[] widths = new float[] { 1.8f, 1.2f, 2.0f, 2.0f, 2.0f, 4.0f, 1.8f, 2.0f, 2.0f, 1.4f, 1.6f, 2.0f, 2.2f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 2.3f, 1.3f };
                table.SetWidths(widths);
                //CONSULTANOIMPRESOS = "SELECT DISTINCT C.Periodo, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
                //                     "C.titularID AS IC, CONCAT(P.Apellido, ' ', P.Nombre) as Apellido, M.Numero AS Medidor, M.AnteriorEstado, " +
                //                     "M.ActualEstado, C.ConsumoFacturado,  M.ActualFecha as Fecha, M.ActualHora AS Hora, " +
                //                     "C.Operario, " +
                //                     "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +

            }

            ////Estructura de tabla:
            ////Periodo|Ruta|Instalacion|Contrato|IC|Apellido|Numero|AnteriorEstado|Fecha|Hora|Estado Actual|ConsumoFacturado|Situacion|Ord1|Ord2|Ord3|Ord4|Ord5|Observaciones|Lecturista
            PdfPCell Periodo = (new PdfPCell(new Paragraph("Rem", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Periodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Periodo);
            PdfPCell Ruta = (new PdfPCell(new Paragraph("Interfaz", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ruta);
            PdfPCell Instalacion = (new PdfPCell(new Paragraph("Localidad", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Instalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Instalacion);
            PdfPCell Contrato = (new PdfPCell(new Paragraph("Total", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Contrato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Contrato);
            PdfPCell IC = (new PdfPCell(new Paragraph("InfoNoImprimir", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            IC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(IC);
            PdfPCell Apellido = (new PdfPCell(new Paragraph("Leidos", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Apellido.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Apellido);
            PdfPCell Numero = (new PdfPCell(new Phrase("Impresas ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Numero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Numero);
            PdfPCell AnteriorEstado = (new PdfPCell(new Phrase("LeidasNoImpresas ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            AnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(AnteriorEstado);
            PdfPCell Fecha = (new PdfPCell(new Paragraph("FueraDeRango", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Fecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Fecha);
            PdfPCell Hora = (new PdfPCell(new Paragraph("PorOrdenativo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Hora.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Hora);
            PdfPCell SinLeer = (new PdfPCell(new Paragraph("Impresora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            SinLeer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(SinLeer);
            PdfPCell Consumo = (new PdfPCell(new Paragraph("Indicado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Consumo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Consumo);
            PdfPCell Situacion = (new PdfPCell(new Paragraph("Imposible", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Situacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Situacion);
            PdfPCell Ord1 = (new PdfPCell(new Paragraph("SAP", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord1);
            PdfPCell Ord2 = (new PdfPCell(new Paragraph("WS", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ord2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ord2);           

            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            foreach (DataGridViewRow fi in grd.Rows)
            {
                ////Estructura de tabla:
                //Rem|Interfaz|Localidad|Total|InfoNoImprimir|Leidos|Impresos|LeidoNoImpresos|FueraRango|Ordenativo|Impresora|Indicado|Imposible|SAP|WS
                PdfPCell fiPeriodo = (new PdfPCell(new Paragraph(fi.Cells["Rem"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiPeriodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiPeriodo);
                PdfPCell fi2 = (new PdfPCell(new Paragraph(fi.Cells["Interfaz"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi2);
                PdfPCell fiInstalacion = (new PdfPCell(new Paragraph(fi.Cells["Localidad"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiInstalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiInstalacion);
                PdfPCell fiContrato = (new PdfPCell(new Paragraph(fi.Cells["Total"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiContrato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiContrato);
                PdfPCell fiIC = (new PdfPCell(new Paragraph(fi.Cells["InfoNoImprimir"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiIC.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiIC);
                PdfPCell fiApellido = (new PdfPCell(new Paragraph(fi.Cells["Leidos"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiApellido.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiApellido);
                PdfPCell fiNumero = (new PdfPCell(new Paragraph(fi.Cells["Impresos"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiNumero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiNumero);
                PdfPCell fiAnteriorEstado = (new PdfPCell(new Paragraph(fi.Cells["LeidoNoImpresos"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fiAnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fiAnteriorEstado);
                PdfPCell fi3 = (new PdfPCell(new Paragraph(fi.Cells["FueraRango"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi3);
                PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Ordenativo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi4);
                PdfPCell fEstadoActual = (new PdfPCell(new Paragraph(fi.Cells["Impresora"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fEstadoActual.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fEstadoActual);
                PdfPCell fConsumoFacturado = (new PdfPCell(new Paragraph(fi.Cells["Indicado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fConsumoFacturado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fConsumoFacturado);
                PdfPCell fSituacion = (new PdfPCell(new Paragraph(fi.Cells["Imposible"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fSituacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fSituacion);
                PdfPCell fOrd1 = (new PdfPCell(new Paragraph(fi.Cells["SAP"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd1);
                PdfPCell fOrd2 = (new PdfPCell(new Paragraph(fi.Cells["WS"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fOrd2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fOrd2);                
            }

            document.Add(new Paragraph(" "));
            document.Add(table);
            Chunk CantidadRegistros = new Chunk("Total = " + grd.Rows.Count.ToString(),
                                               FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD,
                                               new iTextSharp.text.BaseColor(0, 0, 0)));
            Paragraph Total = new Paragraph(CantidadRegistros);
            Total.Alignment = Element.ALIGN_RIGHT;

            wri.Add(imagenMINTELL);
            wri.Add(imagenDPEC);
            wri.Add(new Paragraph("  "));
            wri.Add(titulo);
            wri.Add(new Paragraph(""));
            wri.Add(new Paragraph(infoinforme));
            wri.Add(new Paragraph(""));
            wri.Add(table);
            wri.Add(Total);
            //wri.Close();
            document.Add(Total);
            document.Add(new Paragraph(" "));
            document.Close();

        }

        private void bgwExpExcel_DoWork(object sender, DoWorkEventArgs e)
        {

            simulateHeavyWork();           

            if (LabelLeyenda.Text != "TodosR")
            {
                this.Cursor = Cursors.WaitCursor;
                //metodo de exportación con datagridview de altas como parametro
                //this.progressBar.Visible = true;
                //lblAvanceExportacion.Visible = true;
                //ExportarExcelPorRemesa(DGResumenExp);
                ExportarExcelDetalleXRuta(DGResumenExp);
                //ExportarExcel(DGResumenExp);                
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                //metodo de exportación con datagridview de altas como parametro
                //ExportarAltasExcel(DGResumenExp);               
                if (Vble.TipoInforme == "EXLS")
                {
                    //ExportarExcel(DGResumenExp);
                    ExportarExcelDetalleXRuta(DGResumenExp);
                }
                else if (Vble.TipoInforme == "TXT")
                {
                    //ExportarToExcel(DGResumenExp, Vble.NombreArchivo, Vble.NombreArchivo);
                    //formInformesSup.ExportarExcelDetalleXRuta(DGResumenExp);
                    ExportarExcelPorRemesa(DGResumenExp);
                }
            }
        }


        /// <summary>
        /// Exporta la tabla del detalle que se obtiene por ruta y lecturista con porcentaje de lecturas
        /// para el usuario supervisor.
        /// </summary>
        /// <param name="grd"></param>
        public void ExportarExcelDetalleXRuta(DataGridView grd)
        {
            int k = 0;
            int nroFilaExcel = 0;
            string[,] sArray;
            string[,] sArrayResGral;
            int filasResGral = 0;
            int columnasResGral = 0;
            sArray = new string[grd.Rows.Count, grd.Columns.Count];
            nroFilaExcel = grd.Rows.Count + 2;         

            filasResGral++;         
            Microsoft.Office.Interop.Excel.Application aplicacion;
            Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
            //Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo1;
            aplicacion = new Microsoft.Office.Interop.Excel.Application();
            libros_trabajo = aplicacion.Workbooks.Add();
            //libros_trabajo.Sheets.Add();
            //libros_trabajo.Sheets.Add();
                       

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);
            libros_trabajo = excel.Workbooks.Add();

            object[,] datos = new object[grd.Rows.Count + 1, grd.Columns.Count]; // +1 por la cabecera
            for (int j = 0; j < grd.Columns.Count; j++) //cabeceras
            {
                datos[0, j] = grd.Columns[j].Name;
            }


            for (int i = 0; i < grd.Rows.Count; i++)
            {
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    datos[i + 1, j] = grd.Rows[i].Cells[j].Value;
                }
                bgwExpExcel.ReportProgress(i);
            }

           
            excel.Range[excel.Cells[1, 1], excel.Cells[datos.GetLength(0), datos.GetLength(1)]].Value = datos;
            //excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo1 = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;
            




            object misValue = System.Reflection.Missing.Value;
            object misValue2 = System.Reflection.Missing.Value;
            object misValue3 = System.Reflection.Missing.Value;



            //libros_trabajo.Worksheets.Add(hoja_trabajo1);
            
            string NombreArchivo = System.IO.Path.GetFileName(Vble.FileName);
            libros_trabajo.SaveAs(Vble.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //excel.GetSaveAsFilename(Vble.NombreArchivo, misValue, misValue, misValue, misValue);

           
            hoja_trabajo1.Activate();
            libros_trabajo.Close(true);
            excel.Quit();
            aplicacion.Quit();

        }


        private void simulateHeavyWork()
        {
            Thread.Sleep(DGResumenExp.Rows.Count);
        }

        private void bgwExpExcel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            this.progressBar.Visible = true;           
            lblAvanceExportacion.Visible = true; 
            this.progressBar.Value = (e.ProgressPercentage * 100) / DGResumenExp.Rows.Count;
            this.lblAvanceExportacion.Text = (e.ProgressPercentage * 100) / DGResumenExp.Rows.Count + " % completado";
        }

        private void bgwExpExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("La exportacion a finalizado", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.progressBar.Value = 0;
            this.progressBar.Visible = false;
            lblAvanceExportacion.Visible = false;
            Vble.LineaExportartxt = "";
            this.Cursor = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            string InformesPDF = Vble.ValorarUnNombreRuta(Vble.CarpetaInformesPDF);
            InformesPDF += Vble.Periodo + "\\" + DateTime.Today.ToString("yyyyMMdd");

            if (!Directory.Exists(InformesPDF))
            {
                Directory.CreateDirectory(InformesPDF);
            }
            fichero.InitialDirectory = InformesPDF;
            fichero.Filter = "Excel (*.xlsx)|.*xls";
            Vble.TipoInforme = "EXLS";
            if (DGResumenExp.RowCount > 0)
            {
                if (TipoInforme == "R")
                {
                    fichero.FileName = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text;
                    Vble.NombreArchivo = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text;
                }
                else if (TipoInforme == "T")
                {
                    fichero.FileName = "Resumen_" + LabelPeriodo.Text + "-" + leyenda.Text;
                    Vble.NombreArchivo = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text + "-Ruta_" + TextBoxRuta.Text;
                }
                else if (TipoInforme == "ER")
                {
                    fichero.FileName = "Disponibles-Saldos-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text + "-Ruta_" + TextBoxRuta.Text;
                }
                else
                {
                    fichero.FileName = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text + "-Ruta_" + TextBoxRuta.Text;
                    Vble.NombreArchivo = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text + "-Ruta_" + TextBoxRuta.Text;
                }
                if (fichero.ShowDialog() == DialogResult.OK)
                { 
                    Vble.FileName = fichero.FileName;
                    Vble.NombreArchivo = "Resumen_" + LabelLeyenda.Text + "-Periodo_" + Vble.Periodo.ToString() + "-Remesa_" + CBRemesa.Text + "-Ruta_" + TextBoxRuta.Text;                  
                    bgwExpExcel.RunWorkerAsync();                 
                }
            }
            else
            {
                MessageBox.Show("No existen datos en la tabla para generar el archivo PDF", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //fichero.Title = LabelLeyenda + "_" + Vble.Periodo.ToString();
        }

        private void TextBoxRuta_TextChanged(object sender, EventArgs e)
        {

        }

        private void detalleDeSituacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Form7Inf = new Form7InformesAltas();
            Form7Inf.MdiParent = this.MdiParent;
            Form7Inf.WindowState = FormWindowState.Maximized;
            Form7Inf.TPDetalleSituaciones.Show();
            Form7Inf.Show();
        }

        public class Datos
        {
            public int Localidad { get; set;}
            public int Total { get; set; }
            public int Leidos { get; set; }
            public int Impresos { get; set; }
            public int TeleLectura { get; set; }
        }

        private void BGWCargaTablas_DoWork(object sender, DoWorkEventArgs e)
        {
          
            this.DTPDesdeTomLect.Text = Vble.TextDesdeInformes;
            this.DTPHastaTomLect.Text = Vble.TextHastaInformes;
            Visible = true;
            RutaNº = TextBoxRuta.Text;
            CheckForIllegalCrossThreadCalls = false;
            lbOrdenativos.Size = new Size(250, 124);
            LabCargandoInformes.Location = new Point(MiLoadingInformes.Width / 2 - 120, MiLoadingInformes.Height / 2 + 300);
            MiLoadingInformes.Visible = true;
            LabCargandoInformes.Visible = true;
            int contRelev = 0;
            //CBRemesa.Text = Remesa;

            Inis.GetPrivateProfileString("Datos", "ResumenGralRuta", "", stbResumen, 8, Ctte.ArchivoIniName);


            if (DB.sDbUsu.ToUpper() == "SUPERVISOR" || DB.sDbUsu.ToUpper() == "AUDITORIA")
            {
                PanelChart.Visible = true;
                if (IndicadorTipoInforme == "Resumen")
                {
                    CargarTablaPreDescarga();

                 

                    lblTotModRel.Text = contRelev.ToString();

                    panelSuperior.Visible = false;
                    ButPrint.Visible = false;
                    button3.Visible = false;
                    BtnPDF.Visible = true;
                    lbOrdenativos.Visible = false;
                    lblTotalUsConOrd.Visible = false;
                    LblBuscar.Location = new Point(333, 76);
                    TextFiltro.Location = new Point(390, 67);
                    BtnExcel.Location = new Point(720, 70);
                    BtnPDF.Location = new Point(760, 70);
                    BtnTxt.Location = new Point(800, 70);
                    LabelLeyenda.Location = new Point(910, 30);
                    LabelPeriodo.Location = new Point(910, 55);
                    splitContainer1.SplitterDistance = 100;
                    splitContainer2.SplitterDistance = 400;
                    PanelChart.Visible = false;

                    if (LabelLeyenda.Text == "TodosR")
                    {
                        if (stbResumen.ToString() == "1")
                        {
                            if (PantallaEstRuta == "NO")
                            {
                                GroupBoxResumenGral.Visible = true;
                              
                               
                                CargarResumenGeneral();
                            }
                            else
                            {
                                BtnPDF.Visible = false;
                                ButPrint.Visible = false;
                                button3.Visible = false;
                                TextFiltro.Enabled = true;
                            }
                        }
                    }
                    LabelLeyenda.Visible = true;

                    if (DB.sDbUsu.ToUpper() == "AUDITORIA")
                    {
                        TextFiltro.Enabled = true;
                        panelSuperior.Size = new Size(1100, 53);
                        panelSuperior.Visible = true;
                        LabelRemesa.Visible = false;
                        CBRemesa.Visible = false;
                        label6.Visible = false;
                        TextBoxRuta.Visible = false;
                        groupBox2.Visible = false;
                        groupBox3.Visible = false;
                        splitContainer1.SplitterDistance = 170;
                        LabelLeyenda.Location = new Point(910, 70);
                        LabelPeriodo.Location = new Point(910, 95);
                    }
                    BtnTxt.Visible = true;
                    //button4.Location = new Point(452, 126);
                    PanelTotOrd.Visible = false;
                    PanelHistorial.Visible = false;
                }
                else if (IndicadorTipoInforme == "Historial")
                {
                    CargarPeriodos();
                    PanelHistorial.Visible = true;
                    //CargarTablaHistorial();
                }
                else if (IndicadorTipoInforme == "ResumenRemesa")
                {
                    CargarTablaPreDescarga();
                    panelSuperior.Visible = false;
                    ButPrint.Visible = false;
                    button3.Visible = false;
                    BtnPDF.Visible = false;
                    lbOrdenativos.Visible = false;
                    lblTotalUsConOrd.Visible = false;
                    GroupBoxResumenGral.Visible = true;
                    GroupBoxResumenGral.Text = "Resumen Gráfico";
                    GroupBoxResumenGral.Size = new Size(1200, 184);
                    DGResumenExp.ScrollBars = ScrollBars.Both;

                    if (LabelLeyenda.Text == "Todos")
                    {
                        if (stbResumen.ToString() == "1")
                        {
                            if (PantallaEstRuta == "NO")
                            {
                                GroupBoxResumenGral.Visible = true;
                                //ContarOrdenativos();
                                //ContarRelevM();
                                CargarResumenGeneral();
                            }
                            else
                            {
                                BtnPDF.Visible = false;
                                ButPrint.Visible = false;
                                button3.Visible = false;
                                TextFiltro.Enabled = true;
                            }
                        }
                    }

                    if (ImpresionOBS == "999" || ImpresionOBS == "9999")
                    {
                        ArmarGraficos(DGResumenExp, TipoInforme);
                        PanelChart.Visible = true;
                        LabelLeyenda.Visible = false;
                        //LabelPeriodo.Text = "Periodo: " + Vble.Periodo.ToString();
                        LabelPeriodo.Visible = true;
                        PanelHistorial.Visible = false;
                        PanelTotOrd.Visible = false;
                        LblBuscar.Location = new Point(333, 76);
                        TextFiltro.Location = new Point(390, 67);
                        BtnExcel.Location = new Point(720, 70);
                        BtnPDF.Location = new Point(760, 70);
                        BtnTxt.Location = new Point(800, 70);
                        LabelLeyenda.Location = new Point(910, 30);
                        LabelPeriodo.Location = new Point(910, 35);
                        splitContainer1.SplitterDistance = 100;
                        //splitContainer3.SplitterDistance = 500;
                        splitContainer2.SplitterDistance = 200;
                        TextFiltro.Enabled = true;
                        GraficarComparacionPeriodos();
                    }
                    else
                    {
                        PanelChart.Visible = false;
                        BtnExcel.Visible = true;
                        //LblBuscar.Location = new Point(333, 76);
                        //TextFiltro.Location = new Point(390, 67);
                        //BtnExcel.Location = new Point(720, 70);
                        //BtnPDF.Location = new Point(760, 70);
                        //BtnTxt.Location = new Point(800, 70);
                        //LabelLeyenda.Location = new Point(910, 30);
                        //LabelPeriodo.Location = new Point(910, 35);
                        //splitContainer1.SplitterDistance = 100;
                        ////splitContainer3.SplitterDistance = 500;
                        //splitContainer2.SplitterDistance = 400;
                        GroupBoxResumenGral.Visible = true;
                    }

                    //PanelChart.Visible = true;
                    //splitContainer3.Dock = DockStyle.Top;
                    //splitContainer3.Size = new Size(1129, 100);
                }
                if (TipoInforme == "T")
                {
                    LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " Remesa 1-8 ";
                    LabelPeriodo.Visible = true;

                }
                else if (TipoInforme == "R")
                {
                    LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " Remesa " + Remesa;
                    LabelPeriodo.Visible = true;

                }
                else
                {
                    LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " " + leyenda.Text;
                    LabelPeriodo.Visible = true;
                }
                MiLoadingInformes.Visible = false;
                LabCargandoInformes.Visible = false;

            }
            //Inicio bloque de informes para usuarios operario de DPEC
            else
            {
                PanelChart.Visible = false;
                if (IndicadorTipoInforme == "Resumen")
                {
                    CargarTablaPreDescarga();

                
                    if (LabelLeyenda.Text == "Todos")
                    {
                        if (stbResumen.ToString() == "1")
                        {
                            if (PantallaEstRuta == "NO")
                            {
                                
                            }
                            else
                            {
                                BtnPDF.Visible = false;
                                ButPrint.Visible = false;
                                button3.Visible = false;
                                TextFiltro.Enabled = true;
                            }
                        }
                    }
                    BtnTxt.Visible = false;
                    //button4.Location = new Point(566, 126);
                    PanelTotOrd.Visible = false;
                    LabelLeyenda.Visible = true;
                    BtnExcel.Visible = true;
                    LabelLeyenda.Location = new Point(870, 88);
                    LabelPeriodo.Location = new Point(870, 110);
                    LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " " + leyenda.Text;
                    LabelPeriodo.Visible = true;
                    PanelHistorial.Visible = false;
                    splitContainer2.SplitterDistance = 400;
                    TextFiltro.Enabled = true;
                }
                else if (IndicadorTipoInforme == "Historial")
                {
                    CargarPeriodos();
                    PanelHistorial.Visible = true;
                    //CargarTablaHistorial();
                }
                else if (IndicadorTipoInforme == "ResumenRemesa")
                {
                    CargarTablaPreDescarga();
                    panelSuperior.Visible = false;
                    ButPrint.Visible = false;
                    button3.Visible = false;
                    BtnPDF.Visible = false;
                    lbOrdenativos.Visible = false;
                    lblTotalUsConOrd.Visible = false;
                    GroupBoxResumenGral.Visible = true;
                    GroupBoxResumenGral.Text = "Resumen Gráfico";
                    GroupBoxResumenGral.Size = new Size(1200, 184);
                    DGResumenExp.ScrollBars = ScrollBars.Both;

                    if (LabelLeyenda.Text == "Todos")
                    {
                        if (stbResumen.ToString() == "1")
                        {
                            if (PantallaEstRuta == "NO")
                            {
                                GroupBoxResumenGral.Visible = true;
                                //ContarOrdenativos();
                                //ContarRelevM();
                                CargarResumenGeneral();
                            }
                            else
                            {
                                CargarTablaPreDescarga();
                                BtnPDF.Visible = false;
                                ButPrint.Visible = false;
                                button3.Visible = false;
                                TextFiltro.Enabled = true;
                            }
                        }
                    }

                    if (ImpresionOBS == "999" || ImpresionOBS == "9999")
                    {
                        ArmarGraficos(DGResumenExp, TipoInforme);
                        PanelChart.Visible = true;
                        LabelLeyenda.Visible = false;
                        //LabelPeriodo.Text = "Periodo: " + Vble.Periodo.ToString();
                        LabelPeriodo.Visible = true;
                        PanelHistorial.Visible = false;
                        PanelTotOrd.Visible = false;
                        LblBuscar.Location = new Point(333, 76);
                        TextFiltro.Location = new Point(390, 67);
                        BtnExcel.Location = new Point(720, 70);
                        BtnPDF.Location = new Point(760, 70);
                        BtnTxt.Location = new Point(800, 70);
                        LabelLeyenda.Location = new Point(910, 30);
                        LabelPeriodo.Location = new Point(910, 35);
                        splitContainer1.SplitterDistance = 100;
                        //splitContainer3.SplitterDistance = 500;
                        splitContainer2.SplitterDistance = 200;
                        TextFiltro.Enabled = true;
                        GraficarComparacionPeriodos();
                    }
                    else
                    {
                        PanelChart.Visible = false;
                        BtnExcel.Visible = true;
                        //LblBuscar.Location = new Point(333, 76);
                        //TextFiltro.Location = new Point(390, 67);
                        //BtnExcel.Location = new Point(720, 70);
                        //BtnPDF.Location = new Point(760, 70);
                        //BtnTxt.Location = new Point(800, 70);
                        //LabelLeyenda.Location = new Point(910, 30);
                        //LabelPeriodo.Location = new Point(910, 35);
                        //splitContainer1.SplitterDistance = 100;
                        ////splitContainer3.SplitterDistance = 500;
                        //splitContainer2.SplitterDistance = 400;
                        GroupBoxResumenGral.Visible = true;
                    }
                    //PanelChart.Visible = true;
                    //splitContainer3.Dock = DockStyle.Top;
                    //splitContainer3.Size = new Size(1129, 100);
                }
                LabelLeyenda.Visible = true;
                LabelPeriodo.Text = "Periodo " + Vble.Periodo.ToString() + " " + leyenda.Text;
                LabelPeriodo.Visible = true;
                PanelHistorial.Visible = false;
                TextFiltro.Enabled = true;
            }
            //Vble.ShowLoading();
            //Task oTask = new Task(Algo);
            //oTask.Start();
            //await oTask;
            //MiLoadingInformes.Visible = false;
            //LabCargandoInformes.Visible = false;

        }

        private void BGWCargaTablas_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            MiLoadingInformes.Visible = false;
            LabCargandoInformes.Visible = false;
            //MessageBox.Show("Terminado");
        }

        private void BGWCargaTablas_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
          
            LabCargandoInformes.Text = (e.ProgressPercentage * 100).ToString();
            
        }

        private void RBNumMed_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DGResumenExp_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (menu.Items.Count > 0)
            //{
            //    menu.Items.Clear();
            //}
            if (e.RowIndex != -1 && e.ColumnIndex != -1) // Verificar que se haya hecho clic dentro de una celda válida
            {              
            
                if (e.Button == MouseButtons.Right)
                {
                    DGResumenExp.CurrentCell = DGResumenExp.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    
                    //obtenemos las coordenadas de la celda seleccionada.
                    Rectangle coordenada = DGResumenExp.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    int anchoCelda = coordenada.Location.X;
                    int altoCelda = coordenada.Location.Y;

                    if (LabelLeyenda.Text == "LabelTeleLect")
                    {
                        RemesaDetRes = DGResumenExp.Rows[e.RowIndex].Cells[1].Value.ToString();
                        ZonaDetRes = DGResumenExp.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                    else
                    {
                        RemesaDetRes = DGResumenExp.Rows[e.RowIndex].Cells[0].Value.ToString();
                        ZonaDetRes = DGResumenExp.Rows[e.RowIndex].Cells[1].Value.ToString();
                    }             

                    //mostramos el menu
                    int X = anchoCelda + DGResumenExp.Location.X;
                    int Y = altoCelda + DGResumenExp.Location.Y;              
                    CMSDetalleInformes.Show(DGResumenExp, new Point(X, Y));
                    //mostramos el menu
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
     
        }

        /// <summary>
        /// Evento que ocurre cuando se selecciona uno de los items del ContextMenuItem CMSItemDetalleInformes
        /// items:
        /// Ver detalle Zona
        /// Ver impresos por FIS
        /// Ver Leidos no impresos
        /// Ver Saldos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            FormDetalleInformes pantallaDetalle = new FormDetalleInformes();
            CMSDetalleInformes.Close();
            //string Zona = DGResumenExp.

            if (DB.sDbUsu.ToUpper() == "SUPERVISOR" || DB.sDbUsu.ToUpper() == "AUDITORIA")
            {
                if (e.ClickedItem.Name == "ItemDetalleZona")
                {
                    pantallaDetalle.tipoResumen = "DZ";//Abreviación para obtener informe detalle Zona
                    pantallaDetalle.ZonaDetResumen = ZonaDetRes;
                    pantallaDetalle.RemesaDetResumen = RemesaDetRes;
                    pantallaDetalle.ResumenDetTeleLectura = LabelLeyenda.Text;
                    pantallaDetalle.zonaTeleLect = ZonaDetRes;
                    pantallaDetalle.remTeleLect = RemesaDetRes;
                    pantallaDetalle.Show();
                }
                else if (e.ClickedItem.Name == "ItemAltasZona")
                    {
                        pantallaDetalle.tipoResumen = "AZ"; //Abreviación para obtener inform de Altas de la zona (donde trae la ubicacion- Latitud y Longitud)
                        pantallaDetalle.ZonaDetResumen = ZonaDetRes;
                        pantallaDetalle.RemesaDetResumen = RemesaDetRes;
                        pantallaDetalle.ResumenDetTeleLectura = LabelLeyenda.Text;
                        pantallaDetalle.zonaTeleLect = ZonaDetRes;
                        pantallaDetalle.remTeleLect = RemesaDetRes;
                        pantallaDetalle.Show();
                    }
                {

                }
            }
        }

        private void CMSDetalleInformes_Opening(object sender, CancelEventArgs e)
        {

        }

        private void DGResumenExp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BGWInfSuperv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MiLoadingInformes.Visible = false;
            LabCargandoInformes.Visible = false;
        }

        private void BGWInfSuperv_DoWork(object sender, DoWorkEventArgs e)
        {
            MiLoadingInformes.Visible = true;
            LabCargandoInformes.Visible = true;
        }

        private void BGWInfSuperv_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }

        private void BGWProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
          

        }

        private void BGWProcessing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            LabCargandoInformes.Location = new Point(MiLoadingInformes.Width / 2 - 120, MiLoadingInformes.Height / 2 + 50);       
            LabCargandoInformes.Visible = true;
        }

        private void BGWProcessing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //groupBox1.Visible = true;
            //panel1.Visible = true;
            //MiLoadingInformes.Visible = false;
            //LabCargandoInformes.Visible = false;
            //groupBox1.Visible = false;
            //panel1.Visible = false;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void RBInstalacion_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MiLoadingInformes_Click(object sender, EventArgs e)
        {

        }

        private void FormDetallePreDescarga_LocationChanged(object sender, EventArgs e)
        {
            LabCargandoInformes.Location = new Point(MiLoadingInformes.Width / 2, MiLoadingInformes.Height / 2 );
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DGResumenExp.RowCount > 0)
            {
                if (CBTipoLect.Text == "FIS")
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Tipo_Lectura"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = Tabla.DefaultView;
                    view.RowFilter = string.Empty;
                    if (CBTipoLect.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Tipo_Lectura, 'System.String') like '%{0}%' OR Tipo_Lectura = null ", "F:");  // + " LIKE '%" + TextNºInstalacion.Text + "%'";                    
                    DGResumenExp.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (CBTipoLect.Text == "TELELECTURA")
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Tipo_Lectura"].ColumnName, "]");                    
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = Tabla.DefaultView;
                    view.RowFilter = string.Empty;
                    if (CBTipoLect.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Tipo_Lectura, 'System.String') like '%{0}%'","T:");  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGResumenExp.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (CBTipoLect.Text == "TODOS")
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Condición"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = Tabla.DefaultView;
                    view.RowFilter = string.Empty;
                    if (CBTipoLect.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Condición, 'System.String') <> '%{0}%' ", "");  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGResumenExp.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
            }
            else
            {
                //if (DGResumenExp.RowCount == 0)
                //{
                //    MessageBox.Show("Por favor verifique los datos ingresados", "No incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    TextFiltro.Text = "";
                //}

                CargarTablaPreDescarga();
                //this.TextNºInstalacion_TextChanged(sender, e);
            }
        }

        private void DGResumenExp_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1) // Verificar que se haya hecho clic dentro de una celda válida
            {
                e.ContextMenuStrip = null; // Deshabilitar el menú contextual en esa celda
            }
        }
    }
}


