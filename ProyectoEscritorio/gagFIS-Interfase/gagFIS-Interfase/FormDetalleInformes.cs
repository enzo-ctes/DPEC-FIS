using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using static Mysqlx.Datatypes.Scalar.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Rectangle = System.Drawing.Rectangle;

namespace gagFIS_Interfase
{
    public partial class FormDetalleInformes : Form
    {
        public string RemesaDetResumen { get; set; }
        public string ZonaDetResumen { get; set; }
        public string ResumenDetTeleLectura { get; set; }
        public string remTeleLect { get; set; }
        public string zonaTeleLect { get; set; }
        public string detalle { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public string tipoResumen { get; set; } 

        ArrayList ListaFechas = new ArrayList();
        ArrayList ListaCodigosLect = new ArrayList();
        ArrayList ListaRutas = new ArrayList();
        ArrayList CantUsurios = new ArrayList();
        ArrayList Tomados = new ArrayList();
        ArrayList Impresos = new ArrayList();
        ArrayList PorcentajeImpresos = new ArrayList();
        ArrayList LeidosSinImprimir = new ArrayList();
        ArrayList FueraDeRango = new ArrayList();
        ArrayList IndicacionNoImpresion = new ArrayList();
        ArrayList MarcadosPorLoteArrayList = new ArrayList();
        ArrayList ApagadosArrayList = new ArrayList();
        ArrayList HoraInicio = new ArrayList();
        ArrayList HoraFin = new ArrayList();
        ArrayList Inicio = new ArrayList();
        ArrayList Fin = new ArrayList();
        ArrayList PorcenajePorHora = new ArrayList();
        ListViewItem items;
        DataTable Tabla = new DataTable();
        DataTable TablaFechas = new DataTable();
        public List<object> lecturistas;
        public List<DateTime> fechasDistintas;


        public string FechaLec { get; set; }

        public FormDetalleInformes()
        {
            InitializeComponent();
        }

        private void FormDetalleInformes_Load(object sender, EventArgs e)
        {
            if (tipoResumen == "DZ")
            {
                BGWInfSuperv.RunWorkerAsync();
                //dgResumen.ScrollBars = ScrollBars.Both;
            }
            else if (tipoResumen == "AZ")
            {
                BGWInfAltas.RunWorkerAsync();
            }
            

        }

     

        private void LVResumenGral_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GroupBoxResumenGral_Click(object sender, EventArgs e)
        {

        }

        private void panelSuperior_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simulateHeavyWork()
        {
            Thread.Sleep(dgResumen.Rows.Count);
        }

        /// <summary>
        /// Boton que contendra la funcion en segundo plano bgwDetalleExport 
        /// de exportar la tabla datagridview que muestra el detalle del resumen a un excel.
        /// para su posterior analisis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();           
            fichero.InitialDirectory = "C:\\Desktop";
            fichero.Filter = "Excel (*.xlsx)|.*xls";
            Vble.TipoInforme = "EXLS";

            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Vble.FileName = fichero.FileName;
                Vble.NombreArchivo = "Detalle de Lecturas Remesa: " + RemesaDetResumen + " Periodo = " + Vble.Periodo;
                bgwDetalleExport.RunWorkerAsync();
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
            PBExcelCircular.Visible = true;
            lpbexcelCircular.Visible = true;
            

            /////Almaceno en variables locales la cantidad de filas y columnas que tiene el resumen general para luego recorrer
            /////y generar en el excel su cuadro correspondiente.
            //foreach (ListViewItem item in LVResumenGral.Items)
            //{
            //    filasResGral++;
            //}

            filasResGral++;
            //columnasResGral = LVResumenGral.Columns.Count;
            //sArrayResGral = new string[filasResGral, columnasResGral];
            Microsoft.Office.Interop.Excel.Application aplicacion;
            Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
            //Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo1;
            aplicacion = new Microsoft.Office.Interop.Excel.Application();
            libros_trabajo = aplicacion.Workbooks.Add();
            libros_trabajo.Sheets.Add();
            libros_trabajo.Sheets.Add();


            //hoja_trabajo1 = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

            ////armo el array con el contenido del grid
            //for (int i = 1; i <= grd.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < grd.Columns.Count; j++)
            //    {
            //        if (grd.Rows[i].Cells[j].Value != null)
            //        {
            //            sArray[i, j] = grd.Rows[i - 1].Cells[j].Value.ToString();
            //        }

            //    }
            //}
            //for (int i = 0; i <= grd.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < grd.Columns.Count; j++)
            //    {
            //        if (grd.Rows[i].Cells[j].Value != null)
            //        {
            //            hoja_trabajo1.Cells[i + 2, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
            //            //Vble.LineaExportartxt += "\n";
                        
            //        }
                    
            //    }
            //    bgwDetalleExport.ReportProgress(k);
            //    k++;
            //}



            ////Inserto los datos del array a las celdas del excel
            //for (int f = 1; f < grd.Rows.Count; f++)
            //{

            //    for (int n = 1; n <= grd.Columns.Count; n++)
            //    {
            //        hoja_trabajo1.Cells[f, n] = sArray[f - 1, n - 1];
            //        //Vble.LineaExportartxt += sArray[f, n] + "|";
            //    }
            //    //Vble.LineaExportartxt += "\n";
            //    bgwDetalleExport.ReportProgress(k);
            //    k++;
            //}



            ////armo el array con el contenido del grid
            //for (int i = 1; i <= grd.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < grd.Columns.Count; j++)
            //    {
            //        if (grd.Rows[i].Cells[j].Value != null)
            //        {
            //            sArray[i, j] = grd.Rows[i-1].Cells[j].Value.ToString();
            //            bgwDetalleExport.ReportProgress(i);
            //            i++;
            //        }
            //    }
            //}



            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

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
                bgwDetalleExport.ReportProgress(i);
            }

            excel.Range[excel.Cells[1, 1], excel.Cells[datos.GetLength(0), datos.GetLength(1)]].Value = datos;
            excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo1 = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;
            hoja_trabajo1.Activate();
           
           
         

            object misValue = System.Reflection.Missing.Value;
            object misValue2 = System.Reflection.Missing.Value;
            object misValue3 = System.Reflection.Missing.Value;

            libros_trabajo.SaveAs(Vble.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            string NombreArchivo = System.IO.Path.GetFileName(Vble.FileName);
            libros_trabajo.Close(true);
            aplicacion.Quit();
                    
        }

        private void bgwDetalleExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // this.progressBar.Visible = true;
            //lblAvanceExportacion.Visible = true;   
            PBExcelCircular.Visible = true;
            PBExcelCircular.Value = ((e.ProgressPercentage * 100) / dgResumen.Rows.Count);
            // this.progressBar.Value = ((e.ProgressPercentage * 100) / dgResumen.Rows.Count);
            lpbexcelCircular.Text = "Exportando... " + ((e.ProgressPercentage * 100) / dgResumen.Rows.Count) + " % completado";
            //this.lblAvanceExportacion.Text = "Exportando... " + ((e.ProgressPercentage * 100) / dgResumen.Rows.Count) + " % completado";
            //this.progressBar.Value = e.ProgressPercentage;
            //this.lblAvanceExportacion.Text = "Exportando usuario " + e.ProgressPercentage + " ";
        }

        private void bgwDetalleExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Visible = false;
            PBExcelCircular.Visible = false;
            lpbexcelCircular.Visible = false;
            PBExcelCircular.Value = 0;
            MiLoadingInformes.Visible = false;
            lblAvanceExportacion.Visible = false;
            MessageBox.Show("La exportacion a finalizado", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.progressBar.Value = 0;           
            Vble.LineaExportartxt = "";
            this.Cursor = Cursors.Default;
        }

        private void bgwDetalleExport_DoWork(object sender, DoWorkEventArgs e)
        {
            ExportarExcelDetalleXRuta(dgResumen);
        }

        private void BGWInfSuperv_DoWork(object sender, DoWorkEventArgs e)
        {  
            if (ResumenDetTeleLectura == "LabelTeleLect")
            {
                CargaDetalleTeleLect();
                CargarResumenLectDias();
                //CargarResumenLectOperarios();
            }
            else if (ResumenDetTeleLectura == "LabelTodosRem")
            {
                CargaDetalleZonaSelec();
                CargarResumenLectDias();
            }
            else
            {
                CargaDetalle();
                CargarResumenLectDias();
            }
            this.WindowState = FormWindowState.Maximized;
        }

       

        private void CargarResumenLectDias()
        {
            //CargarTablaPreDescarga();
            ArrayList ListaFechas = new ArrayList();
            DataTable TablaFechas = new DataTable();
            ArrayList CantUsuriosXFecha = new ArrayList();
            ArrayList Operarios = new ArrayList();
            ArrayList tomadosXLect = new ArrayList();
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
            ArrayList PorcenajePorHora = new ArrayList();

            //PLoadingResGral.Visible = true;
            //TextBoxRuta.Enabled = false;
            //DTPDesdeTomLect.Enabled = false;
            //DTPHastaTomLect.Enabled = false;

            ObtenerPeriodoRemesa();

            string SelectFechas = "";

            ////if (RutaNº != "")
            ////{
            ////    SelectFechas = "SELECT DISTINCT M.ActualFecha AS Fecha FROM Conexiones C " +
            ////   "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
            ////   "WHERE C.Ruta = " + RutaNº + " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
            ////   " AND M.ActualFecha BETWEEN '" + Desde + "' AND '" + Hasta + "' AND M.ActualFecha <> '01-01-2000' ORDER BY Fecha ASC";
            ////}
            ////else
            ////{
            //    SelectFechas = "SELECT DISTINCT M.ActualFecha AS Fecha FROM Conexiones C " +
            //   "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
            //   "WHERE C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo +
            //   " AND M.ActualFecha BETWEEN '" + Desde + "' AND '" + DateTime.Today.ToString("yyyy-MM-dd") + "' AND M.ActualFecha <> '01-01-2000' AND " +
            //   " C.Zona = " + ZonaDetResumen + " ORDER BY Fecha ASC";
            ////}            

             fechasDistintas = dgResumen.Rows.Cast<DataGridViewRow>()
                        .Select (row => Convert.ToDateTime(row.Cells["Fecha"].Value))
                        .Distinct()
                        .ToList();

            foreach (var item in fechasDistintas)
            {
                string fechaLecturas = Convert.ToDateTime(item).ToString("yyyy-MM-dd");
                if (fechaLecturas != "2000-01-01")
                {          
                      //string fechaLecturas =item.ToString();
                      ListaFechas.Add(item.ToString());
                      //int cantidadMayoresQue5 = numeros.Where(n => n > 5).Count();

                      string SelectTomados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                           " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                           //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                           " WHERE C.Periodo = " + Vble.Periodo +
                                           " AND M.ActualFecha = '" + fechaLecturas + "'" +
                                           " AND C.Zona = " + ZonaDetResumen + "" +
                                           " AND C.ImpresionOBS MOD 100 > 0";

                      MySqlCommand command = new MySqlCommand(SelectTomados, DB.conexBD);
                      command.CommandTimeout = 300;
                      Tomados.Add(command.ExecuteScalar().ToString());
                      command.Dispose();
                                    


                      string SelectLeidosSinImprimir = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                              " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                              " WHERE C.Periodo = " + Vble.Periodo +
                                             " AND M.ActualFecha = '" + fechaLecturas + "'" +
                                             " AND C.Zona = " + ZonaDetResumen + "" +
                                             " AND C.ImpresionOBS MOD 100 > 1";

                      command = new MySqlCommand(SelectLeidosSinImprimir, DB.conexBD);
                      command.CommandTimeout = 300;
                      LeidosSinImprimir.Add(command.ExecuteScalar().ToString());
                      command.Dispose();

                      string SelectImpresos = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                              " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                              //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                              " WHERE C.Periodo = " + Vble.Periodo +
                                             " AND M.ActualFecha = '" + fechaLecturas + "'" +
                                             " AND C.Zona = " + ZonaDetResumen + "" +
                                             " AND C.ImpresionOBS MOD 100 = 1";

                      command = new MySqlCommand(SelectImpresos, DB.conexBD);
                      command.CommandTimeout = 300;
                      Impresos.Add(command.ExecuteScalar().ToString());
                      command.Dispose();                  
                  }
            }

            //InitializeComponent(); // you need to add a listView named listView1 with the designer
            //LVLectDias.FullRowSelect = true;
            //LVLectDias extender = new ListViewExtender(listView1);
            //// extend 2nd column
            //ListViewButtonColumn buttonAction = new ListViewButtonColumn(1);
            //buttonAction.Click += OnButtonActionClick;
            //buttonAction.FixedWidth = true;

            //extender.AddColumn(buttonAction);

            //for (int i = 0; i < 10000; i++)
            //{
            //    ListViewItem item = listView1.Items.Add("item" + i);
            //    item.SubItems.Add("button " + i);
            //}


            for (int i = 0; i < ListaFechas.Count; i++)
                {
                //var dif = (Convert.ToDateTime(Fin[i]) - Convert.ToDateTime(Inicio[i])).TotalHours;
                ///agrega el item fecha

                //items.UseItemStyleForSubItems = false;
                //items.SubItems.Add(new ListViewItem.ListViewSubItem(items, ""));
                items = new ListViewItem(Convert.ToDateTime(ListaFechas[i]).ToString("dd/MM/yyyy"));
                //items = new ListViewItem(Convert.ToDateTime(ListaFechas[i]).ToString("dd/MM/yyyy"));
                //items.SubItems.Add(CantUsuriosXFecha[i].ToString());
                items.SubItems.Add(Tomados[i].ToString());
                items.SubItems.Add(LeidosSinImprimir[i].ToString());                             
                items.SubItems.Add(Impresos[i].ToString());
                //items.SubItems.Add("Ver");
                // Agregar el botón como control al subitem correspondiente
                //Rectangle bounds = items.SubItems["Ver"].Bounds;  // Cambiar el índice por la posición correcta del subitem donde quieres mostrar el botón.
                //btnEditar.Size = new Size(bounds.Width - 10, bounds.Height - 10);
                //btnEditar.Location = new Point(bounds.Left + 5, bounds.Top + 5);
                //LVLectDias.Controls.Add(btnEditar);



                LVLectDias.Items.Add(items);


            }
            PBLLecDias.Visible = false;
            PBLOprs.Visible = false;

        }


   

private void ObtenerPeriodoRemesa()
        {
            string Periodo = Vble.Periodo.ToString().Substring(4);
            switch (Periodo)
            {
                case "01":
                    ObtenerFechaPer1(RemesaDetResumen);
                    break;
                case "02":
                    ObtenerFechaPer2(RemesaDetResumen);
                    break;
                case "03":
                    ObtenerFechaPer3(RemesaDetResumen);
                    break;
                case "04":
                    ObtenerFechaPer4(RemesaDetResumen);
                    break;
                case "05":
                    ObtenerFechaPer5(RemesaDetResumen);
                    break;
                case "06":
                    ObtenerFechaPer6(RemesaDetResumen);
                    break;
            }
           
        }
        private void ObtenerFechaPer1(string remesa)
        {
            switch (remesa)
            {
            case "1":
                 Desde = DateTime.Today.Year + "-12-22";
                 Hasta = DateTime.Today.Year + "-12-29";
                break;
            case "2":
                Desde = DateTime.Today.Year + "-01-02";
                Hasta = DateTime.Today.Year + "-01-09";
                break;
            case "3":
                Desde = DateTime.Today.Year + "-01-09";
                Hasta = DateTime.Today.Year + "-01-16";
                break;
            case "4":
                Desde = DateTime.Today.Year + "-01-16";
                Hasta = DateTime.Today.Year + "-01-23";
                break;
            case "5":
                Desde = DateTime.Today.Year + "-01-23";
                Hasta = DateTime.Today.Year + "-01-30";
                break;
            case "6":
                Desde = DateTime.Today.Year + "-02-01";
                Hasta = DateTime.Today.Year + "-02-08";
                break;
            case "7":
                Desde = DateTime.Today.Year + "-02-08";
                Hasta = DateTime.Today.Year + "-02-15";
                break;
            case "8":
                Desde = DateTime.Today.Year + "-02-15";
                Hasta = DateTime.Today.Year + "-02-22";
                break;
            }
        }
        private void ObtenerFechaPer2(string remesa)
        {
            switch (remesa)
            {
                case "01":
                    Desde = DateTime.Today.Year + "-02-21";
                    Hasta = DateTime.Today.Year + "-02-29";
                    break;
                case "2":
                    Desde = DateTime.Today.Year + "-03-02";
                    Hasta = DateTime.Today.Year + "-03-09";
                    break;
                case "3":
                    Desde = DateTime.Today.Year + "-03-09";
                    Hasta = DateTime.Today.Year + "-03-16";
                    break;
                case "4":
                    Desde = DateTime.Today.Year + "-03-16";
                    Hasta = DateTime.Today.Year + "-03-23";
                    break;
                case "5":
                    Desde = DateTime.Today.Year + "-03-23";
                    Hasta = DateTime.Today.Year + "-03-30";
                    break;
                case "6":
                    Desde = DateTime.Today.Year + "-04-01";
                    Hasta = DateTime.Today.Year + "-04-08";
                    break;
                case "7":
                    Desde = DateTime.Today.Year + "-04-08";
                    Hasta = DateTime.Today.Year + "-04-15";
                    break;
                case "8":
                    Desde = DateTime.Today.Year + "-04-15";
                    Hasta = DateTime.Today.Year + "-04-22";
                    break;
            }
        }
        private void ObtenerFechaPer3(string remesa)
        {
            switch (remesa)
            {
                case "01":
                    Desde = DateTime.Today.Year + "-04-22";
                    Hasta = DateTime.Today.Year + "-04-29";
                    break;
                case "2":
                    Desde = DateTime.Today.Year + "-05-02";
                    Hasta = DateTime.Today.Year + "-05-09";
                    break;
                case "3":
                    Desde = DateTime.Today.Year + "-05-09";
                    Hasta = DateTime.Today.Year + "-05-16";
                    break;
                case "4":
                    Desde = DateTime.Today.Year + "-05-16";
                    Hasta = DateTime.Today.Year + "-05-23";
                    break;
                case "5":
                    Desde = DateTime.Today.Year + "-05-23";
                    Hasta = DateTime.Today.Year + "-05-30";
                    break;
                case "6":
                    Desde = DateTime.Today.Year + "-06-01";
                    Hasta = DateTime.Today.Year + "-06-08";
                    break;
                case "7":
                    Desde = DateTime.Today.Year + "-06-08";
                    Hasta = DateTime.Today.Year + "-06-15";
                    break;
                case "8":
                    Desde = DateTime.Today.Year + "-06-15";
                    Hasta = DateTime.Today.Year + "-06-22";
                    break;
            }
        }
        private void ObtenerFechaPer4(string remesa)
        {
            switch (remesa)
            {
                case "01":
                    Desde = DateTime.Today.Year + "-06-22";
                    Hasta = DateTime.Today.Year + "-06-29";
                    break;
                case "2":
                    Desde = DateTime.Today.Year + "-07-02";
                    Hasta = DateTime.Today.Year + "-07-09";
                    break;
                case "3":
                    Desde = DateTime.Today.Year + "-07-09";
                    Hasta = DateTime.Today.Year + "-07-16";
                    break;
                case "4":
                    Desde = DateTime.Today.Year + "-07-16";
                    Hasta = DateTime.Today.Year + "-07-23";
                    break;
                case "5":
                    Desde = DateTime.Today.Year + "-07-23";
                    Hasta = DateTime.Today.Year + "-07-30";
                    break;
                case "6":
                    Desde = DateTime.Today.Year + "-08-01";
                    Hasta = DateTime.Today.Year + "-08-08";
                    break;
                case "7":
                    Desde = DateTime.Today.Year + "-08-08";
                    Hasta = DateTime.Today.Year + "-08-15";
                    break;
                case "8":
                    Desde = DateTime.Today.Year + "-08-15";
                    Hasta = DateTime.Today.Year + "-08-22";
                    break;
            }
        }
        private void ObtenerFechaPer5(string remesa)
        {
            switch (remesa)
            {
                case "01":
                    Desde = DateTime.Today.Year + "-08-22";
                    Hasta = DateTime.Today.Year + "-08-29";
                    break;
                case "2":
                    Desde = DateTime.Today.Year + "-09-02";
                    Hasta = DateTime.Today.Year + "-09-09";
                    break;
                case "3":
                    Desde = DateTime.Today.Year + "-09-09";
                    Hasta = DateTime.Today.Year + "-09-16";
                    break;
                case "4":
                    Desde = DateTime.Today.Year + "-09-16";
                    Hasta = DateTime.Today.Year + "-09-23";
                    break;
                case "5":
                    Desde = DateTime.Today.Year + "-09-23";
                    Hasta = DateTime.Today.Year + "-09-30";
                    break;
                case "6":
                    Desde = DateTime.Today.Year + "-10-01";
                    Hasta = DateTime.Today.Year + "-10-08";
                    break;
                case "7":
                    Desde = DateTime.Today.Year + "-10-08";
                    Hasta = DateTime.Today.Year + "-10-15";
                    break;
                case "8":
                    Desde = DateTime.Today.Year + "-10-15";
                    Hasta = DateTime.Today.Year + "-10-22";
                    break;
            }
        }
        private void ObtenerFechaPer6(string remesa)
        {
            switch (remesa)
            {
                case "01":
                    Desde = DateTime.Today.Year + "-10-22";
                    Hasta = DateTime.Today.Year + "-10-29";
                    break;
                case "2":
                    Desde = DateTime.Today.Year + "-11-02";
                    Hasta = DateTime.Today.Year + "-11-09";
                    break;
                case "3":
                    Desde = DateTime.Today.Year + "-11-09";
                    Hasta = DateTime.Today.Year + "-11-16";
                    break;
                case "4":
                    Desde = DateTime.Today.Year + "-11-16";
                    Hasta = DateTime.Today.Year + "-11-23";
                    break;
                case "5":
                    Desde = DateTime.Today.Year + "-11-23";
                    Hasta = DateTime.Today.Year + "-30-11";
                    break;
                case "6":
                    Desde = DateTime.Today.Year + "-12-01";
                    Hasta = DateTime.Today.Year + "-12-08";
                    break;
                case "7":
                    Desde = DateTime.Today.Year + "-12-08";
                    Hasta = DateTime.Today.Year + "-12-15";
                    break;
                case "8":
                    Desde = DateTime.Today.Year + "-12-15";
                    Hasta = DateTime.Today.Year + "-12-22";
                    break;
            }
        }

        private void CargaDetalleZonaSelec()
        {
           string CONSULTA = "SELECT DISTINCT C.Periodo, C.Zona, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
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
                                   "I.Lec_Actual as LectReg, " +
                                   "if(ImpresionOBS = 400, '-', " +
                                      "IF(ImpresionOBS = 500, '-', " +
                                          "IF(ImpresionOBS = 0, 'NO LEIDO', " +
                                              "IF(E.Titulo = 'Leído e Impreso', CONCAT(SUBSTRING(C.VencimientoProx, -4, 2), ':', SUBSTRING(C.VencimientoProx, -2)), IF(C.VencimientoProx <> 'Leído e Impreso', '-', '-'))))) AS HoraImp, " +
                                   "IF(C.ImpresionCOD = 0, 'NORMAL', IF(C.ImpresionCOD = 1, 'SOLO LECTURA N/F-N/I', IF(C.ImpresionCOD = 2, 'PRESUMIDOR N/F-N/I', IF(C.ImpresionCOD = 3, 'TELE LECT CONSUMIDOR', \r\n\tIF(C.ImpresionCOD = 4, 'TELE LECT PROSUMIDOR N/F-N/I', IF(C.ImpresionCOD = 5, 'CONSUMIDOR COMUN F-N/I', IF(C.ImpresionCOD = 6, 'PROSUMIDOR N/F-N/I', IF(C.ImpresionCOD = 7, 'TELE LECT CONSUMIDOR F-N/I', \r\n\tIF(C.ImpresionCOD = 8, 'TELE LECT PROSUMIDOR S/L', '-'))))))))) AS 'Condición', " +
                                   "IF(C.ImpresionOBS MOD 100 = 0, '-', IF((date_format((str_to_date(I.Fec_Actual, '%d/%m/%Y')), '%Y-%m-%d') < date_format(M.ActualFecha, '%Y-%m-%d')), Concat('T: ', I.Lec_Actual), IF(Replace(I.Lec_Actual, '.', '') < M.ActualEstado, Concat('T: ', Replace(I.Lec_Actual, '.', '')), IF(M.ActualEstado < 0, 'F', Concat('F: ', M.ActualEstado))))) as Tipo_Lectura, " +
                                   "IF(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', IF (ImpresionOBS = 0,'NO LEIDO', E.Titulo))) AS Situacion, " +
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
                                   "IF(Alt.Activa = 'Y',  Alt.Estado, '-') as Inyeccion " +
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
                                   "left JOIN(SELECT * FROM Altas WHERE Activa = 'Y' and Periodo = " + Vble.Periodo + ") Alt " +
                                   "ON Alt.ConexionID = C.ConexionID AND Alt.Periodo = C.Periodo " +
                                   "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                   "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                   "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                   "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                   "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                   "WHERE ((C.ImpresionOBS >= 0) " +
                                   " OR (C.ImpresionOBS MOD 100 >= 0 and M.ActualFecha BETWEEN '2000-01-01' and '2000-01-01') AND C.Periodo = " + Vble.Periodo +
                                   " AND C.Remesa = " + remTeleLect + ") " +
                                   //"OR (C.ImpresionOBS = 800 OR C.ImpresionOBS = 500 or C.ImpresionOBS = 400 or C.ImpresionOBS = 0) " +
                                   "AND C.Remesa = " + remTeleLect +
                                   " AND C.Periodo = " + Vble.Periodo +
                                   " AND C.Zona = " + zonaTeleLect +
                                   "  GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, HoraLect ASC, C.Secuencia";

            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(CONSULTA, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);

            TablaFechas = new DataTable();
            datosAdapter.Fill(TablaFechas);
            //datosAdapter.Fill(Tabla);
            dgResumen.Columns.Clear();
            dgResumen.DataSource = TablaFechas;

            datosAdapter.Dispose();
            comandoSQL.Dispose();
            GroupBoxResumenGral.Text = "Resumen detalle de lectura Remesa " + RemesaDetResumen;

        }

        /// <summary>
        /// Ejecuta la consulta que carga el detalle de resumen con cantidades de usuarios con teleLectura
        /// </summary>
        private void CargaDetalleTeleLect()
        {
            string CONSULTA = "SELECT DISTINCT C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, " +
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
                                 "I.Lec_Actual as LectReg, " +
                                 "if(ImpresionOBS = 400, '-', " +
                                    "IF(ImpresionOBS = 500, '-', " +
                                        "IF(ImpresionOBS = 0, 'NO LEIDO', " +
                                            "IF(E.Titulo = 'Leído e Impreso', CONCAT(SUBSTRING(C.VencimientoProx, -4, 2), ':', SUBSTRING(C.VencimientoProx, -2)), IF(C.VencimientoProx <> 'Leído e Impreso', '-', '-'))))) AS HoraImp, " +
                                 "IF(C.ImpresionOBS MOD 100 = 0, '-', IF((date_format((str_to_date(I.Fec_Actual, '%d/%m/%Y')), '%Y-%m-%d') < date_format(M.ActualFecha, '%Y-%m-%d')), Concat('T: ', I.Lec_Actual), IF(Replace(I.Lec_Actual, '.', '') < M.ActualEstado, Concat('T: ', Replace(I.Lec_Actual, '.', '')), IF(M.ActualEstado < 0, 'F', Concat('F: ', M.ActualEstado))))) as Tipo_Lectura, " +
                                 "IF(ImpresionOBS = 400, 'EN CALLE', IF (ImpresionOBS = 500, 'NO LEIDO', IF (ImpresionOBS = 0,'NO LEIDO', E.Titulo))) AS Situacion, " +
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
                                 "IF(Alt.Activa = 'Y',  Alt.Estado, '-') as Inyeccion " +
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
                                 "left JOIN(SELECT * FROM Altas WHERE Activa = 'Y' and Periodo = " + Vble.Periodo + ") Alt " +
                                 "ON Alt.ConexionID = C.ConexionID AND Alt.Periodo = C.Periodo " +
                                 "LEFT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER(PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo < 243 and Periodo = " + Vble.Periodo + ") AS N6 " +
                                 "RIGHT JOIN(SELECT ConexionID, Periodo, Codigo, Observ, ROW_NUMBER() OVER (PARTITION BY ConexionID  ORDER BY Codigo) as Renglon " +
                                 "FROM NovedadesConex WHERE ((Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") OR Codigo >= 243 and Periodo = " + Vble.Periodo + " ) as N62 " +
                                 "ON N6.ConexionID = N62.ConexionID  and N6.Renglon = N62.Renglon ON N6.ConexionID = C.ConexionID AND N6.Periodo = C.Periodo " +
                                 " WHERE C.Remesa = " + remTeleLect + " AND  (date_format((str_to_date(I.Fec_Actual, '%d/%m/%Y')), '%Y-%m-%d') < date_format(M.ActualFecha, '%Y-%m-%d') " +
                                 " AND C.Periodo = " + Vble.Periodo + " AND Zona = " + zonaTeleLect + 
                                 ")  GROUP BY C.ConexionID, M.Numero ORDER BY Fecha Asc, HoraLect ASC, C.Secuencia, C.Ruta";

            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(CONSULTA, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);

            TablaFechas = new DataTable();
            datosAdapter.Fill(TablaFechas);
            //datosAdapter.Fill(Tabla);
            dgResumen.Columns.Clear();
            dgResumen.DataSource = TablaFechas;

            datosAdapter.Dispose();
            comandoSQL.Dispose();

            //ListaFechas.RemoveRange(0, ListaFechas.Count);

            //foreach (DataRow item in TablaFechas.Rows)
            //{
            //    ListaFechas.Add(item.Field<DateTime>("Fecha").ToString());
            //    ListaCodigosLect.Add(item.Field<Int32>("Operario").ToString());
            //    ListaRutas.Add(item.Field<Int32>("Ruta").ToString());

            //}

            //for (int i = 0; i < ListaFechas.Count; i++)
            //{

            //    if (ListaCodigosLect[i].ToString() != "0")
            //    {

            //        string SelectHoraMin = "SELECT MIN(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
            //                              " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
            //                              " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                              " AND C.Operario = " + ListaCodigosLect[i].ToString() +
            //                              " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                               " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        MySqlCommand command = new MySqlCommand(SelectHoraMin, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            HoraInicio.Add("0");
            //        }
            //        else
            //        {
            //            HoraInicio.Add(Convert.ToDateTime(command.ExecuteScalar().ToString()));
            //        }

            //        command.Dispose();

            //        string SelectHoraMax = "SELECT MAX(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
            //                              " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
            //                              " WHERE C.Ruta = " + ListaRutas[i].ToString() + " AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                              " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                              " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(SelectHoraMax, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            HoraFin.Add("0");
            //        }
            //        else
            //        {
            //            HoraFin.Add(Convert.ToDateTime(command.ExecuteScalar().ToString()));
            //        }

            //        command.Dispose();

            //        string TotalUsuarios = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                               " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                               " WHERE C.Ruta = " +  ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                               " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                               " AND C.ImpresionOBS MOD 100 >= 0" +
            //                                " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(TotalUsuarios, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            CantUsurios.Add("0");
            //        }
            //        else
            //        {
            //            CantUsurios.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();

            //        string SelectTomados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                               " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                               " WHERE C.Ruta = " + ListaRutas[i].ToString() + " and C.Periodo = " + Vble.Periodo +
            //                               " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                               " AND C.ImpresionOBS MOD 100 > 0" +
            //                               " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(SelectTomados, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            Tomados.Add("0");
            //        }
            //        else
            //        {
            //            Tomados.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();

            //        string SelectImpresos = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                                " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                                " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                                " AND M.ActualFecha = '" +  Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                                " AND C.ImpresionOBS MOD 100 = 1" +
            //                                " GROUP BY M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(SelectImpresos, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            Impresos.Add("0");
            //        }
            //        else
            //        {
            //            Impresos.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();

            //        string SelectLeidosSinImprimir = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                                " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                                " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                                " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                                " AND C.ImpresionOBS MOD 100 > 1" +
            //                                " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(SelectLeidosSinImprimir, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            LeidosSinImprimir.Add("0");
            //        }
            //        else
            //        {
            //            LeidosSinImprimir.Add(command.ExecuteScalar().ToString());
            //        }


            //        command.Dispose();

            //        string SelectFueraRango = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                                " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                                " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                                " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                                " AND C.ImpresionOBS MOD 100 = 4" +
            //                                " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(SelectFueraRango, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            FueraDeRango.Add("0");
            //        }
            //        else
            //        {
            //            FueraDeRango.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();

            //        string SelectIndicados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                                " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                                " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                                " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                                " AND C.ImpresionCOD = 1" +
            //                               " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(SelectIndicados, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            IndicacionNoImpresion.Add("0");
            //        }
            //        else
            //        {
            //            IndicacionNoImpresion.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();


            //        string MarcadosPorLote = " SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                              " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                              " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                              " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                              " AND C.ImpresionOBS MOD 100 = 46" +
            //                               " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(MarcadosPorLote, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            MarcadosPorLoteArrayList.Add("0");
            //        }
            //        else
            //        {
            //            MarcadosPorLoteArrayList.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();


            //        string Apagados = " SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
            //                              " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
            //                              " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
            //                              " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
            //                              " AND C.ImpresionOBS MOD 100 = 17" +
            //                              " GROUP BY M.ActualFecha, C.Ruta, C.Operario" +
            //                               " ORDER BY M.ActualHora ASC";

            //        command = new MySqlCommand(Apagados, DB.conexBD);
            //        command.CommandTimeout = 300;
            //        if (command.ExecuteScalar() == null)
            //        {
            //            ApagadosArrayList.Add("0");
            //        }
            //        else
            //        {
            //            ApagadosArrayList.Add(command.ExecuteScalar().ToString());
            //        }

            //        command.Dispose();


            //        Inicio.Add(DateTime.ParseExact(Convert.ToDateTime(HoraInicio[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));
            //        Fin.Add(DateTime.ParseExact(Convert.ToDateTime(HoraFin[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));

            //    }

            //}



            GroupBoxResumenGral.Text = "Resumen detalle de lectura Remesa " + RemesaDetResumen;

            //LabCargandoInformes.Visible = false;
            //MiLoadingInformes.Visible = false;
        }

        /// <summary>
        /// Ejecuta la consulta que carga el detalle de resumen por operario y ruta tomada
        /// </summary>
        private void CargaDetalle()
        {
            string SelectFechas = "SELECT C.Operario, C.Ruta, M.ActualFecha AS Fecha FROM Conexiones C" +
             " INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
             "WHERE C.Remesa = " + RemesaDetResumen + " AND C.Periodo = " + Vble.Periodo + " AND M.Periodo = " + Vble.Periodo + " AND C.Zona = " + ZonaDetResumen +
             " AND M.ActualFecha <> '2000-01-01'  GROUP BY M.ActualFecha, C.Ruta, C.Operario ORDER BY Fecha ASC";

            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(SelectFechas, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);

            TablaFechas = new DataTable();
            datosAdapter.Fill(TablaFechas);
            //datosAdapter.Fill(Tabla);

            datosAdapter.Dispose();
            comandoSQL.Dispose();

            ListaFechas.RemoveRange(0, ListaFechas.Count);

            foreach (DataRow item in TablaFechas.Rows)
            {
                ListaFechas.Add(item.Field<DateTime>("Fecha").ToString());
                ListaCodigosLect.Add(item.Field<Int32>("Operario").ToString());
                ListaRutas.Add(item.Field<Int32>("Ruta").ToString());

            }

            for (int i = 0; i < ListaFechas.Count; i++)
            {

                if (ListaCodigosLect[i].ToString() != "0")
                {

                    string SelectHoraMin = "SELECT MIN(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
                                          " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                                          " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                          " AND C.Operario = " + ListaCodigosLect[i].ToString() +
                                          " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                           " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    MySqlCommand command = new MySqlCommand(SelectHoraMin, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        HoraInicio.Add("0");
                    }
                    else
                    {
                        HoraInicio.Add(Convert.ToDateTime(command.ExecuteScalar().ToString()));
                    }

                    command.Dispose();

                    string SelectHoraMax = "SELECT MAX(M.ActualHora) FROM Conexiones C INNER JOIN Medidores M" +
                                          " ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo" +
                                          " WHERE C.Ruta = " + ListaRutas[i].ToString() + " AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                          " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                          " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(SelectHoraMax, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        HoraFin.Add("0");
                    }
                    else
                    {
                        HoraFin.Add(Convert.ToDateTime(command.ExecuteScalar().ToString()));
                    }

                    command.Dispose();

                    string TotalUsuarios = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                           " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                           " WHERE C.Ruta = " +  ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                           " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                           " AND C.ImpresionOBS MOD 100 >= 0" +
                                            " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(TotalUsuarios, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        CantUsurios.Add("0");
                    }
                    else
                    {
                        CantUsurios.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();

                    string SelectTomados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                           " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                           " WHERE C.Ruta = " + ListaRutas[i].ToString() + " and C.Periodo = " + Vble.Periodo +
                                           " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                           " AND C.ImpresionOBS MOD 100 > 0" +
                                           " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(SelectTomados, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        Tomados.Add("0");
                    }
                    else
                    {
                        Tomados.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();

                    string SelectImpresos = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                            " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                            " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                            " AND M.ActualFecha = '" +  Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                            " AND C.ImpresionOBS MOD 100 = 1" +
                                            " GROUP BY M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(SelectImpresos, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        Impresos.Add("0");
                    }
                    else
                    {
                        Impresos.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();

                    string SelectLeidosSinImprimir = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                            " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                            " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                            " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                            " AND C.ImpresionOBS MOD 100 > 1" +
                                            " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(SelectLeidosSinImprimir, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        LeidosSinImprimir.Add("0");
                    }
                    else
                    {
                        LeidosSinImprimir.Add(command.ExecuteScalar().ToString());
                    }


                    command.Dispose();

                    string SelectFueraRango = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                            " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                            " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                            " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                            " AND C.ImpresionOBS MOD 100 = 4" +
                                            " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(SelectFueraRango, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        FueraDeRango.Add("0");
                    }
                    else
                    {
                        FueraDeRango.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();

                    string SelectIndicados = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                            " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                            " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                            " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                            " AND C.ImpresionCOD = 1" +
                                           " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(SelectIndicados, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        IndicacionNoImpresion.Add("0");
                    }
                    else
                    {
                        IndicacionNoImpresion.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();


                    string MarcadosPorLote = " SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                          " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                          " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                          " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                          " AND C.ImpresionOBS MOD 100 = 46" +
                                           " GROUP BY  M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(MarcadosPorLote, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        MarcadosPorLoteArrayList.Add("0");
                    }
                    else
                    {
                        MarcadosPorLoteArrayList.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();


                    string Apagados = " SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                          " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                          " WHERE C.Ruta = " + ListaRutas[i].ToString() + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                          " AND M.ActualFecha = '" + Convert.ToDateTime(ListaFechas[i]).ToString("yyyy-MM-dd") + "'" +
                                          " AND C.ImpresionOBS MOD 100 = 17" +
                                          " GROUP BY M.ActualFecha, C.Ruta, C.Operario" +
                                           " ORDER BY M.ActualHora ASC";

                    command = new MySqlCommand(Apagados, DB.conexBD);
                    command.CommandTimeout = 300;
                    if (command.ExecuteScalar() == null)
                    {
                        ApagadosArrayList.Add("0");
                    }
                    else
                    {
                        ApagadosArrayList.Add(command.ExecuteScalar().ToString());
                    }

                    command.Dispose();


                    Inicio.Add(DateTime.ParseExact(Convert.ToDateTime(HoraInicio[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));
                    Fin.Add(DateTime.ParseExact(Convert.ToDateTime(HoraFin[i]).ToString("HHmm"), "HHmm", System.Globalization.CultureInfo.InvariantCulture));

                }

            }

           
            
            GroupBoxResumenGral.Text = "Resumen detalle de lectura Remesa " + RemesaDetResumen;

            //LabCargandoInformes.Visible = false;
            //MiLoadingInformes.Visible = false;
        }

        private void BGWInfSuperv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 0; i < ListaFechas.Count; i++)
            {
                var dif = (Convert.ToDateTime(Fin[i]) - Convert.ToDateTime(Inicio[i])).TotalHours;
                ///agrega el item fecha
                //items = new ListViewItem(Convert.ToDateTime(ListaFechas[i]).ToString("dd/MM/yyyy"));
                dgResumen.Rows.Add();
                dgResumen.Rows[i].Cells[0].Value = Convert.ToDateTime(ListaFechas[i]).ToString("dd/MM/yyyy"); //Fecha

                ///agrega el item Cod Lecturista
                //items.SubItems.Add(ListaCodigosLect[i].ToString());
                dgResumen.Rows[i].Cells[1].Value = ListaCodigosLect[i].ToString(); //Lecturista
                ///agrega el item Ruta
                //items.SubItems.Add(ListaRutas[i].ToString());
                dgResumen.Rows[i].Cells[2].Value = ListaRutas[i].ToString(); //Ruta
                ///agrega el item hora inicio
                //items.SubItems.Add(Convert.ToDateTime(HoraInicio[i]).ToString("HH:mm:ss"));
                dgResumen.Rows[i].Cells[3].Value = Convert.ToDateTime(HoraInicio[i]).ToString("HH:mm:ss"); //HoraInicio
                ///agrega el item hora fin
                //items.SubItems.Add(Convert.ToDateTime(HoraFin[i]).ToString("HH:mm:ss"));
                dgResumen.Rows[i].Cells[4].Value = Convert.ToDateTime(HoraFin[i]).ToString("HH:mm:ss"); //HoraFin


                ///agrega el item Duracion
                if (dif.ToString().Length > 3)
                {
                    //items.SubItems.Add(dif.ToString().Substring(0, 3));
                    dgResumen.Rows[i].Cells[5].Value = dif.ToString().Substring(0, 3); //diferencia
                }
                else if (dif.ToString().Length == 3)
                {
                    //items.SubItems.Add(dif.ToString().Substring(0, 3));
                    dgResumen.Rows[i].Cells[5].Value = dif.ToString().Substring(0, 3); //diferencia
                }
                else if (dif.ToString().Length == 2)
                {
                    //items.SubItems.Add(dif.ToString().Substring(0, 1));
                    dgResumen.Rows[i].Cells[5].Value = dif.ToString().Substring(0, 1); //diferencia
                }
                else if (dif.ToString().Length == 1)
                {
                    //items.SubItems.Add(dif.ToString().Substring(0, 1));
                    dgResumen.Rows[i].Cells[5].Value = dif.ToString().Substring(0, 1); //diferencia
                }

                if (Convert.ToDecimal(dif) != 0)
                {
                    if ((Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Length > 3)
                    {
                        //items.SubItems.Add((Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Substring(0, 4) + " % ");
                        dgResumen.Rows[i].Cells[6].Value = (Convert.ToInt32(Tomados[i]) / Convert.ToDecimal(dif)).ToString().Substring(0, 4) + " % "; //diferencia
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
                                //items.SubItems.Add((resul).ToString().Substring(0, 2) + " % ");
                                dgResumen.Rows[i].Cells[6].Value = (resul).ToString().Substring(0, 2) + " % "; //PorcentajeHora
                            }
                            else
                            {
                                //items.SubItems.Add((resul).ToString());
                                dgResumen.Rows[i].Cells[6].Value = (resul).ToString() + " % "; //PorcentajeHora
                            }

                        }

                    }

                }
                else if (dif == 0)
                {
                    //items.SubItems.Add("0");
                }

                ///agrega el item Cantidad Usuarios
                //items.SubItems.Add(CantUsurios[i].ToString());
                dgResumen.Rows[i].Cells[7].Value = CantUsurios[i].ToString(); //CantidadUsuarios
                ///agrega el item Cantidad Tomados/Leidos
                //items.SubItems.Add(Tomados[i].ToString());
                dgResumen.Rows[i].Cells[8].Value = Tomados[i].ToString();
                ///agrega el item Cantidad Impresos
                //items.SubItems.Add(Impresos[i].ToString());
                dgResumen.Rows[i].Cells[9].Value = Impresos[i].ToString();

                if (Convert.ToDecimal(Tomados[i]) != 0)
                {
                    if (((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Length > 2)
                    {
                        //items.SubItems.Add(((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 2) + " %");
                        dgResumen.Rows[i].Cells[10].Value = ((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 2) + " %";
                    }
                    else
                    {
                        //items.SubItems.Add(((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 1) + " %");
                        dgResumen.Rows[i].Cells[10].Value = ((Convert.ToInt32(Impresos[i]) / Convert.ToDecimal(Tomados[i])) * 100).ToString().Substring(0, 1) + " %";
                    }

                }

                //items.SubItems.Add(LeidosSinImprimir[i].ToString());
                //dgResumen.Rows[i].Cells[11].Value = LeidosSinImprimir[i].ToString();//LeidosSinImprimir

                //items.SubItems.Add(FueraDeRango[i].ToString());
                dgResumen.Rows[i].Cells[11].Value = FueraDeRango[i].ToString();//FueraRango

                //items.SubItems.Add(IndicacionNoImpresion[i].ToString());
                dgResumen.Rows[i].Cells[12].Value = IndicacionNoImpresion[i].ToString();//Indicados

                //items.SubItems.Add(IndicacionNoImpresion[i].ToString());
                dgResumen.Rows[i].Cells[13].Value = MarcadosPorLoteArrayList[i].ToString();//Marcados Por Lote


                //items.SubItems.Add(IndicacionNoImpresion[i].ToString());
                dgResumen.Rows[i].Cells[14].Value = ApagadosArrayList[i].ToString();//Indicados
                                                                                    //LVResumenGral.Items.Add(items);
                //PLoadingResGral.Visible = false;
                dgResumen.Visible = true;
                //LVResumenGral.Visible = true;
                GroupBoxResumenGral.Visible = true;
            }

            MiLoadingInformes.Visible = false;
            LabCargandoInformes.Visible = false;
        }

        private void BGWInfSuperv_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            LabCargandoInformes.Location = new Point(MiLoadingInformes.Width / 2 - 120, MiLoadingInformes.Height / 2 + 50);
            MiLoadingInformes.Visible = true;
            LabCargandoInformes.Visible = true;
        }

        private void TextFiltro_TextChanged(object sender, EventArgs e)
        {

        }

        private void LVLectDias_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void LVLectDias_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lecturistas = dgResumen.Rows.Cast<DataGridViewRow>()
                      .Select(row => (row.Cells["Operario"].Value))
                      .Distinct()
                      .ToList();
            PBLOprs.WaitOnLoad = false;
            PBLOprs.Visible = true;
           

            bgwLectXOp_DoWork();

            //if (LVLectDias.SelectedItems.Count == 1)
            //{
            //    LogImpApartados logImp = new LogImpApartados();
            //    logImp.TxtPorcion.Text = Vble.PorcionImp;
            //    logImp.TxtTotalUsuarios.Text = Vble.TotalUsuariosImp;
            //    logImp.TxtImportados.Text = Vble.TotalImportados;
            //    logImp.TxtApartados.Text = Vble.TotalApartados;

            //    if (Convert.ToInt16(Vble.TotalApartados) > 0)
            //    {
            //        DataTable TableLogImportado = new DataTable();

            //        string txSQL = "SELECT * FROM LogImportacion WHERE IDLogImportacion = " + Vble.IDLogImportacion;
            //        MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            //        MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
            //        datosAdapter.Fill(TableLogImportado);
            //        datosAdapter.Dispose();
            //        comandoSQL.Dispose();

            //        InfoImportacion.Visible = true;
            //        ListViewItem ResumenImportacion;
            //        ResumenImportacion = new ListViewItem();

            //        if (TableLogImportado.Rows.Count >= 1)
            //        {
            //            foreach (DataRow item in TableLogImportado.Rows)
            //            {
            //                string Detalle = item["DetalleApartados"].ToString();
            //                string[] Instalaciones = Detalle.Split(';');
            //                for (int i = 0; i < Instalaciones.Length; i++)
            //                {
            //                    logImp.LVDetalle.Items.Add(new ListViewItem(Instalaciones[i]));
            //                }

            //            }
            //        }
            //        logImp.Show();
            //    }
            //    else
            //    {
            //        MessageBox.Show("La ruta que selecciono se importo correctamente en su totalidad, " +
            //                        "no contiene usuarios apartados", "Ruta sin usuarios apartados",
            //                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }


            //}
           
         
        }

        private void bgwLectXOp_DoWork()
        {
            PBLOprs.Visible = true;
            
            LVLectOper.Items.Clear();
            ThreadStart lecturasPorOperarios = new ThreadStart(LectxOperario);
            Thread thread = new Thread(lecturasPorOperarios);

            thread.Start();
        }

        public void LectxOperario()
        {
            foreach (ListViewItem item in LVLectDias.SelectedItems)
            {
                FechaLec = item.Text;
            }

            string cantLecturas = "0";
          
            foreach (var ope in lecturistas)
            {
                //var lecturas = dgResumen.Rows.Cast<DataGridViewRow>()
                //               .Where(row => (Convert.ToInt32(row.Cells["Periodo"].Value) == Vble.Periodo) && row.Cells["Fecha"].Value.ToString() == Convert.ToDateTime(item).ToString("dd-MM-yyyy").ToString()
                //                && row.Cells["Zona"].Value.ToString() == ZonaDetResumen
                //                && row.Cells["Operario"].Value == ope)
                //               .Count();
                if (ope.ToString() != "0")
                {
                    string SelectTomadosxLect = "SELECT Count(C.ConexionID) FROM Conexiones C INNER JOIN Medidores M" +
                                       " ON C.ConexionID = M.ConexionID and C.Periodo = M.Periodo" +
                                       //" WHERE C.Ruta = " + RutaNº + "  AND M.ActualHora <> '00:00' and C.Periodo = " + Vble.Periodo +
                                       " WHERE C.Periodo = " + Vble.Periodo +
                                       " AND M.ActualFecha = '" + Convert.ToDateTime(FechaLec).ToString("yyyy-MM-dd") + "'" +
                                       " AND C.Zona = " + ZonaDetResumen + "" +
                                       " AND C.Operario = " + ope +
                                       " AND C.ImpresionOBS MOD 100 > 0";



                    MySqlCommand command = new MySqlCommand(SelectTomadosxLect, DB.conexBD);
                    command.CommandTimeout = 300;
                    cantLecturas = command.ExecuteScalar().ToString();
                    command.Dispose();

                    if (cantLecturas != "0")
                    {
                        items = new ListViewItem(LVLectDias.SelectedItems[0].Text);
                        items.SubItems.Add(ope.ToString());
                        items.SubItems.Add(cantLecturas);
                        LVLectOper.Items.Add(items);
                        cantLecturas = "0";
                    }

                   
                }
            }
            PBLOprs.Visible = false;
        }

        private void bgwLectXOp_DoWork(object sender, DoWorkEventArgs e)
        {
            //bgwLectXOp_DoWork();
           
            LectxOperario();
        }

        private void bgwLectXOp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PBLOprs.Visible = false;
        }

        private void dgResumen_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
           
        }

        private void exportarAExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.InitialDirectory = "C:\\Desktop";
            fichero.Filter = "Excel (*.xlsx)|.*xls";
            Vble.TipoInforme = "EXLS";

            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Vble.FileName = fichero.FileName;
                Vble.NombreArchivo = "Detalle de Lecturas Remesa: " + RemesaDetResumen + " Periodo = " + Vble.Periodo;
                bgwDetalleExport.RunWorkerAsync();
            }

        }

        private void BGWInfAltas_DoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoResumen == "AZ")
            {
                ObtenerAltasZona();
            }
        }

        private void ObtenerAltasZona()
        {
            //MySqlDataAdapter datosAdapter;
            //MySqlCommandBuilder comandoSQL;
            //string txSQL = "";

            //    //Numero <> 'CxDir' AND
            //    //Lee la tabla ALTAS pertenecientes al periodo
            //    string CONSULTA = "SELECT Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
            //            " WHERE Periodo = " + Vble.Periodo + " AND Ruta = " + TextBoxRuta.Text + " AND ABM = 'A'" +
            //            " ORDER BY Fecha ASC, Hora ASC";

            //    TablaFechas = new DataTable();

            //    datosAdapter = new MySqlDataAdapter(CONSULTA, DB.conexBD);
            //    comandoSQL = new MySqlCommandBuilder(datosAdapter);
            //    datosAdapter.Fill(TablaFechas);
            //    dgResumen.Columns.Clear();
            //    dgResumen.DataSource = TablaFechas;

            //    comandoSQL.Dispose();
            //    datosAdapter.Dispose();       

            //GroupBoxResumenGral.Text = "Resumen detalle de lectura Remesa " + RemesaDetResumen;
           
        }
    } 
}
