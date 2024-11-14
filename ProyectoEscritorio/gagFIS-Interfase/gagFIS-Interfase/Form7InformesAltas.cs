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
using System.Threading;
using System.Data.SQLite;
using System.Globalization;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DGV2Printer;

//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;

namespace gagFIS_Interfase
{
    public partial class Form7InformesAltas : Form
    {
        DataView dtview = new DataView();
        DataTable TablaNovedades = new DataTable();
        DataTable TablaAltas = new DataTable();
        DataGridView TablaSituaciones = new DataGridView();
        
        DataTable Tabla = new DataTable();
        DataTable TablaConexDirec = new DataTable();
        DataTable TablaDetalleSituaciones = new DataTable();
        ContextMenuStrip ClickDerecho = new ContextMenuStrip();
        public int rowIndex { get; set; }
        public static string PantallaSolicitud { get; set; }
        public static string RutaDesdeExportacion { get; set; }


        public Form7InformesAltas()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Radio Button Fecha, al seleccionar se hacen visibles los datatimePicker Desde y Hasta
        /// para que el usuario pueda filtrar en un periodo de fecha las altas existentes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFecha.Checked == true)
            {                
                DTPDesdeAltas.Visible = true;
                DTPHastaAltas.Visible = true;
                CBPerDesdeAltas.Visible = false;
                CBPerHastaAltas.Visible = false;
                CBTipoAlta.Visible = false;
                label3.Visible = false;
                label1.Visible = true;
                label2.Visible = true;
                labelMotivo.Visible = false;
                if (CantidadAltas() > 0)
                {
                    CargarAltasPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
                }
            }
        }

        private void Form7InformesAltas_Load(object sender, EventArgs e)
        {
            var Form1 = new Form1Inicio();
            DTPDesdeAltas.Format = DateTimePickerFormat.Custom;
            DTPDesdeAltas.CustomFormat = "dd/MM/yyyy";
            DTPHastaAltas.Format = DateTimePickerFormat.Custom;
            DTPHastaAltas.CustomFormat = "dd/MM/yyyy";
            CargarPeriodosDesde();
            CargarPeriodosHasta();
            radioButtonFecha.Checked = false;
            radioButtonPeriodo.Checked = false;
            radioButtonTipoAlta.Checked = false;
            this.Cursor = Cursors.WaitCursor;

         
            if (CantidadAltas() > 0) CargarAltas();
            if (CantidadConexDirec() > 0) CargarConexDirectas();
            if (CantidadConOrdenativos() > 0) CargarOrdenativos();
            if (DB.sDbUsu.ToUpper() == "SUPERVISOR")
            {

            }
            else
            {
                if (CantidadUsuarios() > 0) CargarDetalleSituaciones();
            }

            //DGAlta hace referencia al datagridview de altas
            DGAlta.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            DGAlta.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            DGAlta.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGAlta.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DGAlta.Columns["Periodo"].Visible = false;
            DGAlta.Columns["Fecha"].Visible = true;
            DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //DGConDir hace referencia al datagridview de conexiones directas
            //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            DGConDir.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            DGConDir.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            DGConDir.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGConDir.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DGConDir.Columns["Periodo"].Visible = false;
            DGConDir.Columns["Fecha"].Visible = true;

            //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            //DGOrdenat.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //DGOrdenat.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;                
            DGOrdenat.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //DGOrdenat.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //DGOrdenat.Columns["Periodo"].Visible = false;
            //DGOrdenat.Columns["Fecha"].Visible = false;
            //bgwInicioPantalla.RunWorkerAsync();

            this.Cursor = Cursors.Default;
            lblCantAltas.Text = "Cantidad = " + DGAlta.Rows.Count.ToString();
            lblCantCxDir.Text = "Cantidad = " + DGConDir.Rows.Count.ToString();
            lblCantOrd.Text = "Cantidad = " + DGOrdenat.Rows.Count.ToString();


          


        }


        /// <summary>
        /// Funcion que carga DGConexionesDirectas por Periodo seleccionado de los combobox desde y hasta
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarConDirPorPeriodos(string desde, string hasta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario FROM Altas" +
                        " WHERE (Periodo >= '" + desde + "' AND Periodo <= '" + hasta + "') AND Numero < 0" ;
                //" ORDER BY Fecha ASC";

                TablaConexDirec = new DataTable();
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaConexDirec);
                DGConDir.DataSource = TablaConexDirec;
                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DGConDir.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGConDir.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                DGConDir.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGConDir.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DGConDir.Columns["Periodo"].Visible = false;
                DGConDir.Columns["Fecha"].Visible = false;
                labelPeriodoConDir.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                lblCantCxDir.Text = "Cantidad = " + DGConDir.RowCount.ToString();

                LblRutaConexDirc.Visible = true;
                TBBuscarRutaConexDirec.Visible = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Funcion que carga DGAlta por Periodo seleccionado de los combobox desde y hasta
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarAltasPorPeriodos(string desde, string hasta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                        " WHERE (Periodo >= '" + desde + "' AND Periodo <= '" + hasta + "') AND (Numero NOT LIKE '%CxDir%')";
                //" ORDER BY Fecha ASC";

                TablaAltas = new DataTable();
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaAltas);
                DGAlta.DataSource = TablaAltas;
                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DGAlta.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGAlta.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                DGAlta.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGAlta.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DGAlta.Columns["Periodo"].Visible = false;
                DGAlta.Columns["Fecha"].Visible = true;
                labelPeriodoAlt.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                lblCantAltas.Text = "Cantidad = " + DGAlta.RowCount.ToString();

                labelRuta.Visible = true;
                TBBuscarRuta.Visible = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Funcion que carga DGOrdenat Periodo seleccionado de los combobox desde y hasta
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarOrdenativosPorPeriodo(string desde, string hasta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla Novedades pertenecientes al periodo
                txSQL = "SELECT C.Ruta, C.ConexionID as Instalacion, M.Numero as Medidor, " +
                        "M.ActualFecha AS Fecha_de_Lectura, M.ActualHora as Hora_de_Lectura, " +
                        "M.ActualEstado, " +
                        "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                        "FROM Conexiones C " +
                        "JOIN Medidores M USING (ConexionID, Periodo) " +
                        //"JOIN NovedadesConex N USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                        //"ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                        "USING (ConexionID, Periodo) " +
                        "WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0].ToString() + iteracionZona() + ") " +
                        "AND N1.Codigo > 0 " +
                        "AND (C.Periodo >= '" + desde + "' AND C.Periodo <= '" + hasta + "')";
                //" ORDER BY Fecha ASC";

                TablaNovedades = new DataTable();
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaNovedades);
                DGOrdenat.DataSource = TablaNovedades;
                ////DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //DGConDir.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //DGConDir.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                //DGConDir.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGOrdenat.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //DGConDir.Columns["Periodo"].Visible = false;
                //DGConDir.Columns["Fecha"].Visible = false;
                LabelPeriodoOrden.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                TBBuscarRutaConexDirec.Text = "Cantidad = " + DGOrdenat.RowCount.ToString();

                LblRutaConexDirc.Visible = true;
                TBBuscarRutaConexDirec.Visible = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Funcion que carga DGAlta por Periodo seleccionado de los combobox desde y hasta
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarPorTipoAlta(string Tipo)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {

                if (Tipo == "A")
                {
                    //Lee la tabla ALTAS pertenecientes al periodo
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                            " WHERE ABM = '" + Tipo + "' AND Periodo = " + Vble.Periodo + " AND (Numero NOT LIKE '%CxDir%')";
                    //" ORDER BY Fecha ASC";
                }
                else if (Tipo == "M") 
                {
                    //Lee la tabla ALTAS pertenecientes al periodo
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario FROM Altas" +
                            " WHERE ABM = '" + Tipo + "' AND Periodo = " + Vble.Periodo + " AND (Numero NOT LIKE '%CxDir%')";
                    //" ORDER BY Fecha ASC";
                }


                    TablaAltas = new DataTable();
                    datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                    comandoSQL = new MySqlCommandBuilder(datosAdapter);
                    datosAdapter.Fill(TablaAltas);
                    DGAlta.DataSource = TablaAltas;
                    //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    DGAlta.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                    DGAlta.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                    DGAlta.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    DGAlta.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                    DGAlta.Columns["Fecha"].Visible = true;
                    DGAlta.Columns["Periodo"].Visible = false;
                    labelPeriodoAlt.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                    lblCantAltas.Text = "Cantidad = " + DGAlta.RowCount.ToString();

                    labelRuta.Visible = true;
                    TBBuscarRuta.Visible = true;

               

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Funcion que carga DGAlta por fechas considerando Desde y Hasta de los DataTimePiker
        /// /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarAltasPorFechas(string desde, string hasta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                        " WHERE (Fecha BETWEEN '" + desde + "' AND '" + hasta + "') AND (Numero NOT LIKE '%CxDir%')";
                        //" ORDER BY Fecha ASC";

                TablaAltas = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaAltas);
                DGAlta.DataSource = TablaAltas;
                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DGAlta.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGAlta.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                DGAlta.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGAlta.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DGAlta.Columns["Periodo"].Visible = false;
                DGAlta.Columns["Fecha"].Visible = true;
                labelPeriodoAlt.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                lblCantAltas.Text = "Cantidad = " + DGAlta.RowCount.ToString();

                labelRuta.Visible = true;
                TBBuscarRuta.Visible = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        /// <summary>
        /// Funcion que carga DGConexionesDirectas por fechas considerando Desde y Hasta de los DataTimePiker
        /// /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarConDirPorFechas(string desde, string hasta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario FROM Altas" +
                        " WHERE (Fecha BETWEEN '" + desde + "' AND '" + hasta + "') AND Numero like 'Cx%'";
                //" ORDER BY Fecha ASC";

                TablaConexDirec = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaConexDirec);
                DGConDir.DataSource = TablaConexDirec;
                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DGConDir.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGConDir.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                DGConDir.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGConDir.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DGConDir.Columns["Periodo"].Visible = false;
                DGConDir.Columns["Estado"].Visible = true;
                labelPeriodoConDir.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                lblCantCxDir.Text = "Cantidad = " + DGConDir.RowCount.ToString();

                LblRutaConexDirc.Visible = true;
                TBBuscarRutaConexDirec.Visible = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        
        /// <summary>
        /// Funcion que carga DGNovedades por fechas considerando Desde y Hasta de los DataTimePiker
        /// /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarOrdenativosPorFecha(string desde, string hasta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla Novedades pertenecientes al periodo
                txSQL = "SELECT C.Ruta, C.ConexionID as Instalacion, M.Numero as Medidor, " +
                        "M.ActualFecha AS Fecha_de_Lectura, M.ActualHora as Hora_de_Lectura, " +
                        "M.ActualEstado, " +
                        "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                        "FROM Conexiones C " +
                        "JOIN Medidores M USING (ConexionID, Periodo) " +
                        //"JOIN NovedadesConex N USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                        //"ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                        "USING (ConexionID, Periodo) " +
                        "WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0].ToString() + iteracionZona() + ") " +
                        "AND N1.Codigo > 0 " +
                        "AND (M.ActualFecha BETWEEN '" + desde + "' AND '" + hasta + "')";
                
                //" ORDER BY Fecha ASC";

                TablaConexDirec = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaConexDirec);
                DGConDir.DataSource = TablaConexDirec;
                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DGConDir.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGConDir.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                DGConDir.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGConDir.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DGConDir.Columns["Periodo"].Visible = false;
                DGConDir.Columns["Fecha"].Visible = false;
                labelPeriodoConDir.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                labelCantConDir.Text = "Cantidad = " + DGConDir.RowCount.ToString();

                LblRutaConexDirc.Visible = true;
                TBBuscarRutaConexDirec.Visible = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }


        /// <summary>
        /// Funcion que carga la tabla altas con las conexiones que pertenecen 
        /// al periodo actual en el que se encuentra el sistema.
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarAltas()
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
           
            string txSQL = "";

            try
            {

                //Lee la tabla ALTAS pertenecientes al periodo
                if (PantallaSolicitud == "Exportacion")
                {
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                        " WHERE Periodo = " + Vble.Periodo + " AND Numero NOT LIKE 'CxDir%' AND Ruta = " + RutaDesdeExportacion + 
                        " ORDER BY Fecha ASC";
                }
                else
                {
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                        " WHERE Periodo = " + Vble.Periodo + " AND Numero NOT LIKE 'CxDir%'" +
                        " ORDER BY Fecha ASC";
                }
                

                TablaAltas = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaAltas);
                DGAlta.DataSource = TablaAltas;


                DGAlta.DataSource = TablaAltas;
                DGAlta.DataBindingComplete += (s, e) =>
                {
                    if (DGAlta.Columns.Contains("Ruta"))
                    {                       

                        DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        DGAlta.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        DGAlta.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                        DGAlta.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        DGAlta.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        DGAlta.Columns["Periodo"].Visible = false;
                        DGAlta.Columns["Fecha"].Visible = true;
                    }
                };

            
              

               
                labelPeriodoAlt.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                LabCantidadAlt.Text = "Cantidad = " + DGAlta.RowCount.ToString();

                labelRuta.Visible = true;
                TBBuscarRuta.Visible = true;
                DGAlta.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        /// <summary>
        /// Funcion que carga la tabla altas con las conexiones que pertenecen 
        /// al periodo actual en el que se encuentra el sistema.
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarAltas(string Ruta)
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {

                //Lee la tabla ALTAS pertenecientes al periodo
                if (PantallaSolicitud == "Exportacion")
                {
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario FROM Altas" +
                        " WHERE Periodo = " + Vble.Periodo + " AND Numero <> 'CxDir' AND Ruta = " + RutaDesdeExportacion +
                        " ORDER BY Fecha ASC";
                }
                else
                {
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario FROM Altas" +
                        " WHERE Periodo = " + Vble.Periodo + " AND Numero <> 'CxDir'" +
                        " ORDER BY Fecha ASC";
                }


                TablaAltas = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaAltas);
                DGAlta.DataSource = TablaAltas;

                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
                DGAlta.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGAlta.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                DGAlta.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGAlta.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DGAlta.Columns["Periodo"].Visible = false;
                DGAlta.Columns["Fecha"].Visible = true;
                labelPeriodoAlt.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                LabCantidadAlt.Text = "Cantidad = " + DGAlta.RowCount.ToString();

                labelRuta.Visible = true;
                TBBuscarRuta.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }





        /// <summary>
        /// Funcion que carga la tabla altas con las conexiones directas que pertenecen 
        /// al periodo actual en el que se encuentra el sistema.
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarConexDirectas()
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {

                //Lee la tabla ALTAS pertenecientes al periodo

                //Lee la tabla ALTAS pertenecientes al periodo
                if (PantallaSolicitud == "Exportacion")
                {
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                       " WHERE Periodo = " + Vble.Periodo + " AND Numero like 'CxDir%' AND Ruta = " + RutaDesdeExportacion + 
                       " ORDER BY Fecha ASC";
                }
                else
                { 
                    txSQL = "SELECT Periodo, Ruta, Numero, Estado, Fecha, Hora, Domicilio, Observaciones, Operario, Latitud, Longitud FROM Altas" +
                        " WHERE Periodo = " + Vble.Periodo + " AND Numero like 'CxDir%'" +
                        " ORDER BY Fecha ASC";
                }

                TablaConexDirec = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaConexDirec);
                DGConDir.DataSource = TablaConexDirec;

                ////DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
                //DGConDir.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //DGConDir.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                //DGConDir.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //DGConDir.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //DGConDir.Columns["Periodo"].Visible = false;
                //DGConDir.Columns["Fecha"].Visible = true;
                labelPeriodoConDir.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                labelCantConDir.Text = "Cantidad = " + DGConDir.RowCount.ToString();

                LblRutaConexDirc.Visible = true;
                TBBuscarRutaConexDirec.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }


        /// <summary>
        /// Funcion que carga la tabla de usuarios con ordenativos que existen en 
        /// el periodo actual en el que se encuentra el sistema.
        /// ACLARACIÓN: en el caso de que se quite o agregue columnas en la consulta de altas, se tendrá que tener en cuenta
        /// en la parte del codigo donde se exporta a PDF ya que ahi está de manera estatica la cantidad de columnas que se tienen en cuenta 
        /// al momento de crear la tabla donde se mostraran en el archivo PDF
        /// </summary>
        private void CargarOrdenativos()
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            //DataTable Tabla;
            string txSQL = "";

            try
            {
                //Lee la tabla Novedades pertenecientes al periodo

                if (PantallaSolicitud == "Exportacion")
                {
                    txSQL = "SELECT C.Zona AS Localidad, C.Ruta, C.ConexionID as Instalacion, M.Numero as Medidor, " +
                       "M.ActualFecha AS Fecha_de_Lectura, M.ActualHora as Hora_de_Lectura, " +
                       "M.ActualEstado, " +
                       "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                       "FROM Conexiones C " +
                       "JOIN Medidores M USING (ConexionID, Periodo) " +
                       //"JOIN NovedadesConex N USING (ConexionID, Periodo) " +
                       "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                       //"ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                       "USING (ConexionID, Periodo) " +
                       "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                       "USING (ConexionID, Periodo) " +
                       "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                       "USING (ConexionID, Periodo) " +
                       "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                       "USING (ConexionID, Periodo) " +
                       "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                       "USING (ConexionID, Periodo) " +
                       "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                       "USING (ConexionID, Periodo) " +
                       "WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0].ToString() + iteracionZona() + ") AND C.Ruta = " + RutaDesdeExportacion +
                       " AND N1.Codigo > 0 " +
                       "ORDER BY C.Ruta, M.ActualFecha ASC, M.ActualHora ASC";
                }
                else
                {
                    txSQL = "SELECT C.Zona AS Localidad, C.Ruta, C.ConexionID as Instalacion, M.Numero as Medidor, " +
                        "M.ActualFecha AS Fecha_de_Lectura, M.ActualHora as Hora_de_Lectura, " +
                        "M.ActualEstado, " +
                        "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                        "FROM Conexiones C " +
                        "JOIN Medidores M USING (ConexionID, Periodo) " +
                        //"JOIN NovedadesConex N USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                        //"ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                        "USING (ConexionID, Periodo) " +
                        "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                        "USING (ConexionID, Periodo) " +
                        "WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0].ToString() + iteracionZona() + ") " +
                        "AND N1.Codigo > 0 " +
                        "ORDER BY C.Ruta, M.ActualFecha ASC, M.ActualHora ASC";
                }
                    TablaNovedades = new DataTable();
                
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                datosAdapter.SelectCommand.CommandTimeout = 300;
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaNovedades);
                DGOrdenat.DataSource = TablaNovedades;

                ////DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
                ////DGOrdenat.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                ////DGOrdenat.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;                
                //DGOrdenat.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ////DGOrdenat.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ////DGOrdenat.Columns["Periodo"].Visible = false;
                ////DGOrdenat.Columns["Fecha"].Visible = false;
                LabelPeriodoOrden.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                LabCanConexConOrd.Text = "Cantidad = " + DGOrdenat.RowCount.ToString();

                LblRutaOrdenativos.Visible = true;
                TBBuscarRutaOrdenativos.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

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
                         
                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }


        /// <summary>
        ///  Devuelve la cantidad de registros que existen en la tabla altas        
        /// </summary>
        /// <returns></returns>
        public static int CantidadAltas()
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM Altas WHERE Periodo = " + Vble.Periodo + " AND (Numero NOT LIKE '%CxDir%')";
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue("Periodo", Vble.Periodo);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }

        /// <summary>
        ///  Devuelve la cantidad de registros que existen en la tabla altas        
        /// </summary>
        /// <returns></returns>
        public static int CantidadConexDirec()
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM Altas WHERE Periodo = " + Vble.Periodo + " AND Numero like '%CxDir'";
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue("Periodo", Vble.Periodo);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }

        /// <summary>
        ///  Devuelve la cantidad de registros que contienen al menos un ordenativo cargado dentro del periodo
        ///  que se esta trabajando
        /// </summary>
        /// <returns></returns>
        public static int CantidadConOrdenativos()
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT DISTINCT Count(*) FROM NovedadesConex WHERE Periodo = " + Vble.Periodo;
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue("Periodo", Vble.Periodo);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }

        /// <summary>
        ///  Devuelve la cantidad de usuarios que tienen la ruta a filtrar para mostrar el detalle de situaciones que se observan
        ///  en el campo ImpresionCANT.
        /// </summary>
        /// <returns></returns>
        public static int CantidadUsuarios()
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;
            if (RutaDesdeExportacion == "")
            {

            }
            else
	            {
                    txSQL = "SELECT DISTINCT Count(*) FROM Conexiones WHERE Periodo = " + Vble.Periodo + " AND Ruta = " + RutaDesdeExportacion;
                    da = new MySqlCommand(txSQL, DB.conexBD);
                    da.Parameters.AddWithValue("Periodo", Vble.Periodo);
                    count = Convert.ToInt32(da.ExecuteScalar());
                }

          
             
            if (count == 0)
                return count;
            else
                return count;

        }

        /// <summary>
        /// quede aca comentar y continuar con la funcion
        /// </summary>
        public void CargarDetalleSituaciones()
        {

            int Total = 0, Leidos = 0, Impresos = 0;
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter();
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder();
            string txSQL, ImpresionCANT;
            string Estado = "", RangoConsumo = "", Impresora = "", Novedades = "", DiasPeriodo = "", IndicacionEnDato = "",
                   PostFacturacion = "", WebServer = "", CantImpresiones = "";
            MySqlCommand da;
            int count = 0;
            txSQL = "SELECT C.Periodo, C.Ruta, C.ConexionID, C.titularID, M.Numero AS Medidor, P.Apellido, C.DomicSumin as Domicilio, " +
                "M.ActualFecha, M.ActualHora, M.AnteriorEstado, M.ActualEstado, C.ConsumoFacturado,  " +
                "C.Operario, C.ImpresionCANT FROM Conexiones C " +
                "INNER JOIN Medidores M USING(ConexionID, Periodo) " +
                "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                "WHERE C.Ruta = " + RutaDesdeExportacion + " AND C.Periodo =  " + Vble.Periodo;

            TablaDetalleSituaciones = new DataTable();
            DataTable TblTempDetSit = new DataTable();

            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            datosAdapter.SelectCommand.CommandTimeout = 300;
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(TablaDetalleSituaciones);

            datosAdapter.Fill(Tabla);

            TablaDetalleSituaciones.Columns.Add("Estado");
            TablaDetalleSituaciones.Columns.Add("Rango Consumo");
            TablaDetalleSituaciones.Columns.Add("Impresora");
            TablaDetalleSituaciones.Columns.Add("Novedades");
            TablaDetalleSituaciones.Columns.Add("Días del Periodo");
            TablaDetalleSituaciones.Columns.Add("Indicación en Dato");
            TablaDetalleSituaciones.Columns.Add("Post Facturación");
            TablaDetalleSituaciones.Columns.Add("Web Server");
            TablaDetalleSituaciones.Columns.Add("Impresiones");

            TablaSituaciones.DataSource = TablaDetalleSituaciones;

            foreach (DataRow row in TablaDetalleSituaciones.Rows)
            {
                Total++;
                if (row["ImpresionCANT"].ToString().Length > 0)
                {
                    ImpresionCANT = row["ImpresionCANT"].ToString();

                    switch (ImpresionCANT.Length)
                    {
                        case 1:
                            Estado = IdentificaEstado(row["ImpresionCANT"].ToString());
                            if (ImpresionCANT == "1")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT == "2")
                            {
                                Leidos++;
                                
                            }
                            break;
                        case 2:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 3:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 4:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            Novedades = IdentificaNovedades(ImpresionCANT.Substring(ImpresionCANT.Length - 4, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 5:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            Novedades = IdentificaNovedades(ImpresionCANT.Substring(ImpresionCANT.Length - 4, 1));
                            DiasPeriodo = IdentificaDiasPeriodo(ImpresionCANT.Substring(ImpresionCANT.Length - 5, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 6:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            Novedades = IdentificaNovedades(ImpresionCANT.Substring(ImpresionCANT.Length - 4, 1));
                            DiasPeriodo = IdentificaDiasPeriodo(ImpresionCANT.Substring(ImpresionCANT.Length - 5, 1));
                            IndicacionEnDato = IdentificaIndicacionEnDato(ImpresionCANT.Substring(ImpresionCANT.Length - 6, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 7:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            Novedades = IdentificaNovedades(ImpresionCANT.Substring(ImpresionCANT.Length - 4, 1));
                            DiasPeriodo = IdentificaDiasPeriodo(ImpresionCANT.Substring(ImpresionCANT.Length - 5, 1));
                            IndicacionEnDato = IdentificaIndicacionEnDato(ImpresionCANT.Substring(ImpresionCANT.Length - 6, 1));
                            PostFacturacion = IdentificaPostFacturacion(ImpresionCANT.Substring(ImpresionCANT.Length - 7, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 8:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            Novedades = IdentificaNovedades(ImpresionCANT.Substring(ImpresionCANT.Length - 4, 1));
                            DiasPeriodo = IdentificaDiasPeriodo(ImpresionCANT.Substring(ImpresionCANT.Length - 5, 1));
                            IndicacionEnDato = IdentificaIndicacionEnDato(ImpresionCANT.Substring(ImpresionCANT.Length - 6, 1));
                            PostFacturacion = IdentificaPostFacturacion(ImpresionCANT.Substring(ImpresionCANT.Length - 7, 1));
                            WebServer = IdentificaErrorWebServer(ImpresionCANT.Substring(ImpresionCANT.Length - 8, 1));
                            if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            {
                                Impresos++;
                            }
                            else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            {
                                Leidos++;
                            }
                            break;
                        case 9:
                            Estado = IdentificaEstado(ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1));
                            RangoConsumo = IdentificaRangoConsumo(ImpresionCANT.Substring(ImpresionCANT.Length - 2, 1));
                            Impresora = IdentificaImpresora(ImpresionCANT.ToString().Substring(ImpresionCANT.Length - 3, 1));
                            Novedades = IdentificaNovedades(ImpresionCANT.Substring(ImpresionCANT.Length - 4, 1));
                            DiasPeriodo = IdentificaDiasPeriodo(ImpresionCANT.Substring(ImpresionCANT.Length - 5, 1));
                            IndicacionEnDato = IdentificaIndicacionEnDato(ImpresionCANT.Substring(ImpresionCANT.Length - 6, 1));
                            PostFacturacion = IdentificaPostFacturacion(ImpresionCANT.Substring(ImpresionCANT.Length - 7, 1));
                            WebServer = IdentificaErrorWebServer(ImpresionCANT.Substring(ImpresionCANT.Length - 8, 1));
                            //CantImpresiones = CantidadImpresiones(ImpresionCANT.Substring(ImpresionCANT.Length - 9, 1));
                            CantImpresiones = ImpresionCANT.Substring(ImpresionCANT.Length - 9, 1);
                            if (Convert.ToInt16(CantImpresiones) == 2)
                            {
                                CantImpresiones = "1";
                                Impresos++;
                            }
                            else if (Convert.ToInt16(CantImpresiones) == 1)
                            {
                                CantImpresiones = "0";
                                Leidos++;
                            }
                            else if (Convert.ToInt16(CantImpresiones) > 2)
                            {
                                CantImpresiones = (Convert.ToInt16(CantImpresiones) - 1).ToString();
                                Impresos++;
                            }
                            

                            //if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "1" || ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "9")
                            //{
                               
                            //    Impresos++;
                            //}
                            //else if (ImpresionCANT.Substring(ImpresionCANT.Length - 1, 1) == "2")
                            //{
                            //    Leidos++;
                               
                            //}
                            break;
                    }
                }


                row["Estado"] = Estado;
                row["Rango Consumo"] = RangoConsumo;
                row["Impresora"] = Impresora;
                row["Novedades"] = Novedades;
                row["Días del Periodo"] = DiasPeriodo;
                row["Indicación en Dato"] = IndicacionEnDato;
                row["Post Facturación"] = PostFacturacion;
                row["Web Server"] = WebServer;
                row["Impresiones"] = CantImpresiones;
            }

            DGDetalleSituaciones.DataSource = TablaDetalleSituaciones;
            DGDetalleSituaciones.Columns["ImpresionCANT"].Visible = false;            
            LblPeriodoDetSit.Text = DGDetalleSituaciones.Rows[0].Cells["Periodo"].Value.ToString();
            LblRutaDetSit.Text = DGDetalleSituaciones.Rows[0].Cells["Ruta"].Value.ToString();
            LblLecturista.Text = DGDetalleSituaciones.Rows[0].Cells["Operario"].Value.ToString();
            LblTotalUsers.Text = Total.ToString();
            LblLeidos.Text = Leidos.ToString();
            LblImpresos.Text = Impresos.ToString();
            DGDetalleSituaciones.Columns["Periodo"].Visible = false;
            DGDetalleSituaciones.Columns["Ruta"].Visible = false;
            DGDetalleSituaciones.Columns["Operario"].Visible = false;

            for (var i = 0; i < DGDetalleSituaciones.Columns.Count; i++)
            {
                DGDetalleSituaciones.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

        }

        /// <summary>
        /// Metodo que evalua el primer digito de la derecha, es decir el valor del estado en el que quedo el usuario leido o no;
        /// No Leido; Impreso; LeidoNoImpreso; Error durante la generación de la Gráfica 
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaEstado(string ImpresionCANT)
        {
           
                if (ImpresionCANT == ((int)ImpCntStatus.NoLeido).ToString())
                {
                    return "NO LEIDO";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoConCorrecionEstado).ToString())
                {
                    return "LEIDO Con Correción Estado";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoConTeleLectura).ToString())
                {
                    return "Tele-LEIDO";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoConCorrecionEstadoyTeleLectura).ToString())
                {
                    return "Tele-LEIDO con corrección estado, NO IMPRESO";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoImpreso).ToString())
                {
                    return "IMPRESO";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoImpresoConCorreccion).ToString())
                {
                    return "IMPRESO con correción";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoImpresoConTeleLectura).ToString())
                {
                    return "Tele-LEIDO, IMPRESO";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.LeidoImpresoConTeleLecturaCorrecionEstado).ToString())
                {
                    return "Tele-LEIDO, IMPRESO con Corrección";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.Leido).ToString())
                {
                    return "LEIDO";
                }
                else if (ImpresionCANT == ((int)ImpCntStatus.Apagado).ToString())
                {
                    return "APAGADO";
                }
            else
            {
                return "-";
            }
        }

        /// <summary>
        /// Metodo que evalua el segundo digito de la derecha, es decir el valor del Rango del consumo;
        /// 
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaRangoConsumo(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntRango.Indeterminado).ToString())
            {
                return "Indeterminado";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.DebajoMinimoImprimible).ToString())
            {
                return "Debajo del minimo Imprimible";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.ConsumoMuyBajo).ToString())
            {
                return "Consumo muy Bajo";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.ImpresionBajaConConfirmacion).ToString())
            {
                return "Impresión baja con confirmación";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.ImpresionBaja).ToString())
            {
                return "Impresión baja";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.DentroRangoLectura).ToString())
            {
                return "Dentro del rango de lectura";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.ImpresionAlta).ToString())
            {
                return "Impresión alta";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.ImpresionAltaConConfirmacion).ToString())
            {
                return "Impresión alta con confiurmación";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.ConsumoMuyAlto).ToString())
            {
                return "Consumo muy alto";
            }
            else if (ImpresionCANT == ((int)ImpCntRango.LecturaImposible).ToString())
            {
                return "Lectura Imposible/Apagado";
            }
            else
            {
                return "-";
            }
        }

        /// <summary>
        /// Metodo que evalua el tercer digito de la derecha, es decir el valor de error de impresora;
        ///
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaImpresora(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntPrinter.SinNovedad).ToString())
            {
                return "Sin novedad";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ImpresoraDeshabilitada).ToString())
            {
                return "Impresora deshabilitada";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ImpresoraApagada).ToString())
            {
                return "Impresora apagada";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ImpresoraNoVinculada).ToString())
            {
                return "Impresora NO vinculada";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ErrorDeImpresora).ToString())
            {
                return "Error de impresora";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ErrorComunicacionConImpresora).ToString())
            {
                return "Error de Comunicación con Impresora";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ErrorAlGererarGraficaDeFactura).ToString())
            {
                return "Error al generar Grafica de Factura";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.MarcadoParaImpEnLote).ToString())
            {
                return "Marcado para Imprimir en Lote";
            }
            else if (ImpresionCANT == ((int)ImpCntPrinter.ImpresoEnLote).ToString())
            {
                return "Impreso en Lote";
            }            
            return "-";
        }

        /// <summary>
        /// Metodo que evalua el cuarto digito de la derecha, es decir el valor de ingreso de novedades;
        ///
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaNovedades(string ImpresionCANT)
        {
            if (ImpresionCANT == ImpCntNoved.SinNovedad.ToString())
            {
                return "Sin novedad";
            }
            else if (ImpresionCANT == ((int)ImpCntNoved.OrdenativosNoImprimibles).ToString())
            {
                return "Ordenativos NO imprimibles";
            }
            else if (ImpresionCANT == ((int)ImpCntNoved.OrdenativoParaEstimacion).ToString())
            {
                return "Ordenativos para Estimación (98,99)";
            }
            else if (ImpresionCANT == ((int)ImpCntNoved.TarifasNoImprimibles).ToString())
            {
                return "Tarifa NO imprimible";
            }          
            return "-";
        }

        /// <summary>
        /// Metodo que evalua el quinto digito de la derecha, es decir el valor de ingreso de novedades;
        ///
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaDiasPeriodo(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntPeri.Normal).ToString())
            {
                return "Normal";
            }
            else if (ImpresionCANT == ((int)ImpCntPeri.ExcedeLimiteFacturacion).ToString())
            {
                return "Excede Limite 1: de Facturación";
            }
            else if (ImpresionCANT == ((int)ImpCntPeri.ExcedeLimiteLectura).ToString())
            {
                return "Excede Limite 2: de Lectura";
            }          

            return "-";
        }

        /// <summary>
        /// Metodo que evalua el sexto digito de la derecha, es decir el valor de ingreso de novedades;
        ///
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaIndicacionEnDato(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntIndDat.NoHayIndicacion).ToString())
            {
                return "No hay indicación";
            }
            else if (ImpresionCANT == ((int)ImpCntIndDat.Indefinido).ToString())
            {
                return "Indefinido";
            }
            else if (ImpresionCANT == ((int)ImpCntIndDat.DiferenciaDeDomicilioPostalYSuministro).ToString())
            {
                return "Diferencia de Domicilio Postal y de Suministro";
            }
            else if (ImpresionCANT == ((int)ImpCntIndDat.DiferenciaDeLocalidadPostalYSuministro).ToString())
            {
                return "Diferencia de Localidad Postal y de Suministro";
            }
            else
            {
                return "-";
            }
           
        }

        /// <summary>
        /// Metodo que evalua el septimo digito de la derecha, es decir el estado despues de facturar;
        /// 
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaPostFacturacion(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntPosFac.Normal).ToString())
            {
                return "Normal";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.ExcedeConsumoOErrorFormato).ToString())
            {
                return "Excede consumo, o error en su formato";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.ExcedeImporteOErrorFormato).ToString())
            {
                return "Excede Importe, o error en su formato";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.ImporteNegativoEnAlgunTalon).ToString())
            {
                return "Importe Negativo en alguno de los talones";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.ExcedeDiasPeriodoFacturacion).ToString())
            {
                return "Excede días de periodo de facturación";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.PeriodoNoCorresponde).ToString())
            {
                return "Periodo no corresponde";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.VencimientosMal).ToString())
            {
                return "Vencimientos Mal";
            }
            else if (ImpresionCANT == ((int)ImpCntPosFac.ExcesoDeRenglones).ToString())
            {
                return "Exceso de renglones";
            }         
            else
            {
                return "-";
            }

        }

        /// <summary>
        /// Metodo que evalua el octavo digito de la derecha, es decir el valor de error devuelvo por web service;
        ///
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string IdentificaErrorWebServer(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntWS.SinNovedad).ToString())
            {
                return "Sin Novedad";
            }
            else if (ImpresionCANT == ((int)ImpCntWS.ErrorInformadoPorSAP).ToString())
            {
                return "Error informado por SAP";
            }
            else if (ImpresionCANT == ((int)ImpCntWS.NoContestaWebServer).ToString())
            {
                return "No contesta el Web Server";
            }
            else if (ImpresionCANT == ((int)ImpCntWS.NoHayCoberturaRed).ToString())
            {
                return "No hay cobertura de Red";
            }
            else if (ImpresionCANT == ((int)ImpCntWS.NoHayRegistroImpresorFactura).ToString())
            {
                return "No hay registros Impresor y Factura";
            }
            else if (ImpresionCANT == ((int)ImpCntWS.TryCatchEnParteWS).ToString())
            {
                return "Try catch en parte Web Server";
            }
            return "-";
        }

        /// <summary>
        /// Metodo que evalua el octavo digito de la derecha, es decir el valor de error devuelvo por web service;
        ///
        /// </summary>
        /// <param name="ImpresionCANT"></param>
        /// <returns></returns>
        private string CantidadImpresiones(string ImpresionCANT)
        {
            if (ImpresionCANT == ((int)ImpCntCantCompImpr.SinNovedad).ToString())
            {
                return "Exceso de renglones";
            }
            return "-";
        }
          

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Lee desde el archivo de confiuguración el último periodo
        /// procesado, y lo muestra como por defecto, luego a partir de la fecha
        /// actual, agrega el periodo actual, uno anterior y uno posterior,
        /// siempre que no se repitan
        /// </summary>
        private void CargarPeriodosDesde()
        {
            var PerDef = new StringBuilder();
            int Anio, Per;
            var FormAltas = new Form7InformesAltas();

            //Periodos a partir del actual
            Anio = DateTime.Now.Year;
            Per = (DateTime.Now.Month + 1) / 2;

            //Anterior
            if (Per == 1)
            { 
                CBPerDesdeAltas.Items.Add((Anio - 1).ToString("0000") + "06");
                CBPerHastaAltas.Items.Add((Anio - 1).ToString("0000") + "06");
                CBPerDesdeConDir.Items.Add((Anio - 1).ToString("0000") + "06");
                CBPerHastaConDir.Items.Add((Anio - 1).ToString("0000") + "06");
            }
            else { 
                CBPerDesdeAltas.Items.Add(Anio.ToString("0000") +
                    (Per - 1).ToString("00"));
                CBPerHastaAltas.Items.Add(Anio.ToString("0000") +
                   (Per - 1).ToString("00"));
                CBPerDesdeConDir.Items.Add(Anio.ToString("0000") +
                    (Per - 1).ToString("00"));
                CBPerHastaConDir.Items.Add(Anio.ToString("0000") +
                    (Per - 1).ToString("00"));

                //Actual
                CBPerDesdeAltas.Items.Add(Anio.ToString("0000") +
                    Per.ToString("00"));
                CBPerHastaAltas.Items.Add(Anio.ToString("0000") +
                    Per.ToString("00"));
                CBPerDesdeConDir.Items.Add(Anio.ToString("0000") +
                    Per.ToString("00"));
                CBPerHastaConDir.Items.Add(Anio.ToString("0000") +
                   Per.ToString("00"));
            }

            //Siguiente
            if (Per == 6)
            {  
                CBPerDesdeAltas.Items.Add((Anio + 1).ToString("0000") + "01");
                CBPerHastaAltas.Items.Add((Anio + 1).ToString("0000") + "01");
                CBPerDesdeConDir.Items.Add((Anio + 1).ToString("0000") + "01");
                CBPerHastaConDir.Items.Add((Anio + 1).ToString("0000") + "01");
            }
            else
            { 
                CBPerDesdeAltas.Items.Add(Anio.ToString("0000")  +
                    (Per + 1).ToString("00"));
                CBPerHastaAltas.Items.Add(Anio.ToString("0000") +
                    (Per + 1).ToString("00"));
                CBPerDesdeConDir.Items.Add(Anio.ToString("0000") +
                    (Per + 1).ToString("00"));
                CBPerHastaConDir.Items.Add(Anio.ToString("0000") +
                    (Per + 1).ToString("00"));

                ////Si no está el por defecto lo agrega
                //Inis.GetPrivateProfileString(
                //    "Datos", "Periodo", comboBoxPerDesde.Items[0].ToString(), PerDef, 8, Ctte.ArchivoIniName);
                //if (!comboBoxPerDesde.Items.Contains(PerDef.ToString()))
                //    PerDef.Remove(4, 1);
                //    comboBoxPerDesde.Items.Add(PerDef.ToString());

                //Defecto
                CBPerDesdeAltas.Text = PerDef.ToString();
                CBPerHastaAltas.Text = PerDef.ToString();

                CBPerDesdeConDir.Text = PerDef.ToString();
                CBPerHastaConDir.Text = PerDef.ToString();
                //CBPerDesdeAltas.Items.Add("201503");
            }
        }

        /// <summary>
        /// Lee desde el archivo de confiuguración el último periodo
        /// procesado, y lo muestra como por defecto, luego a partir de la fecha
        /// actual, agrega el periodo actual, uno anterior y uno posterior,
        /// siempre que no se repitan
        /// </summary>
        private void CargarPeriodosHasta()
        {
            var PerDef = new StringBuilder();
            int Anio, Per;
            var FormAltas = new Form7InformesAltas();

            //Periodos a partir del actual
            Anio = DateTime.Now.Year;
            Per = (DateTime.Now.Month + 1) / 2;

            //Anterior
            if (Per == 1)
                CBPerHastaAltas.Items.Add((Anio - 1).ToString("0000") + "06");


            else
                CBPerHastaAltas.Items.Add(Anio.ToString("0000") +
                    (Per - 1).ToString("00"));


            //Actual
            CBPerHastaAltas.Items.Add(Anio.ToString("0000") +
                    Per.ToString("00"));


            //Siguiente
            if (Per == 6)
                CBPerHastaAltas.Items.Add((Anio + 1).ToString("0000") + "01");
            else
                CBPerHastaAltas.Items.Add(Anio.ToString("0000") +
                    (Per + 1).ToString("00"));

            ////Si no está el por defecto lo agrega
            //Inis.GetPrivateProfileString(
            //    "Datos", "Periodo", comboBoxPerHasta.Items[0].ToString(), PerDef, 8, Ctte.ArchivoIniName);
            //if (!comboBoxPerHasta.Items.Contains(PerDef.ToString()))
            //    comboBoxPerHasta.Items.Add(PerDef.ToString());

            //Defecto
            CBPerHastaAltas.Text = PerDef.ToString();
            CBPerHastaAltas.Items.Add("201503");

        }

        /// <summary>
        /// Se exporta la tabla en vista a un archivo pdf en la direccion que se especifica cuando se guarda el archivo.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="NombreArchivo"></param>
        private void ExportarAltasPDF(DataGridView grd, string NombreArchivo, string SavePath)
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
                if (tabPagConexDirect.Text == "Conexiones Directas")
                {
                    chunk = new Chunk("         Informe de Conexiones Directas \n\n         Periodo: " + labelPeriodoAlt.Text + " \n\n   " + " Ruta: " + RutaDesdeExportacion,
                                   FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                   new iTextSharp.text.BaseColor(0, 102, 0)));
                }
                else
                {
                    chunk = new Chunk("         Informe de Altas \n\n         Periodo: " + labelPeriodoAlt.Text + " \n\n   " + " Ruta: " + RutaDesdeExportacion,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
                }                
            }
            else
            {
                if (tabPagConexDirect.Text == "Conexiones Directas")
                {
                    chunk = new Chunk("         Informe de Conexiones Directas \n\n         Periodo: " + labelPeriodoAlt.Text,
                                   FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                   new iTextSharp.text.BaseColor(0, 102, 0)));
                }
                else if (tabPagAltasyMod.Text == "Altas y modificaciones")
                {
                    chunk = new Chunk("         Informe de Altas y Modificaciones \n\n         Periodo: " + labelPeriodoAlt.Text,
                                  FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                  new iTextSharp.text.BaseColor(0, 102, 0)));
                }
                else
                {
                    chunk = new Chunk("         Informe de Altas \n\n         Periodo: " + labelPeriodoAlt.Text,
                                  FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                  new iTextSharp.text.BaseColor(0, 102, 0)));
                }
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
            PdfPTable table = new PdfPTable(10);
            table.WidthPercentage = 180;
            table.TotalWidth = page.Width - 90;
            table.LockedWidth = true;
            //asigno el ancho de las columnas
            float[] widths = new float[] {0.8f, 1.3f, 1.0f, 1.5f, 1.5f, 4.5f, 3.0f, 1.5f, 1.9f, 1.9f };
            table.SetWidths(widths);

            ////Estructura de tabla:
            ////Periodo|Ruta|Fecha|Hora|Modelo|Numero|Estado|Domicilio|Observaciones|Lecturista
            //PdfPCell Periodo = (new PdfPCell(new Paragraph("Periodo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Periodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Periodo);
            PdfPCell Ruta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ruta);
            PdfPCell Fecha = (new PdfPCell(new Paragraph("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Fecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Fecha);
            PdfPCell Hora = (new PdfPCell(new Paragraph("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Hora.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Hora);
            //PdfPCell Modelo = (new PdfPCell(new Paragraph("Modelo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Modelo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Modelo);
            PdfPCell Numero = (new PdfPCell(new Phrase("Numero ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Numero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Numero);            
            PdfPCell SinLeer = (new PdfPCell(new Paragraph("Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            SinLeer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(SinLeer);
            PdfPCell Domicilio = (new PdfPCell(new Paragraph("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Domicilio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Domicilio);
            PdfPCell Observaciones = (new PdfPCell(new Paragraph("Observaciones", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Observaciones.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Observaciones);
            PdfPCell Activa = (new PdfPCell(new Phrase("Lecturista ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Activa.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Activa);
            PdfPCell Latitud = (new PdfPCell(new Phrase("Latitud ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Latitud.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Latitud);
            PdfPCell Longitud = (new PdfPCell(new Phrase("Longitud ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Longitud.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Longitud);



            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            foreach (DataGridViewRow fi in grd.Rows)
            {

                //fi.Cells["Observaciones"].DataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ////Agrega los datos de cada descarga a sus columnas correspondientes                                   
                //PdfPCell fi1 = (new PdfPCell(new Paragraph(fi.Cells["Periodo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi1);
                PdfPCell fi2 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi2);
                PdfPCell fi3 = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi.Cells["Fecha"].Value).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi3);
                PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Hora"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi4);
                //PdfPCell fi5 = (new PdfPCell(new Paragraph(fi.Cells["Modelo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi5);
                PdfPCell fi6 = (new PdfPCell(new Paragraph(fi.Cells["Numero"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi6);                
                PdfPCell fi7 = (new PdfPCell(new Paragraph(fi.Cells["Estado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi7);
                PdfPCell fi8 = (new PdfPCell(new Paragraph(fi.Cells["Domicilio"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                table.AddCell(fi8);
                PdfPCell fi9 = (new PdfPCell(new Paragraph(fi.Cells["Observaciones"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                table.AddCell(fi9);
                PdfPCell fi10 = (new PdfPCell(new Paragraph(fi.Cells["Operario"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi10);
                PdfPCell fi11 = (new PdfPCell(new Paragraph(fi.Cells["Latitud"].Value.ToString(), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));//Fila 11 hace referencia a la latitud
                fi11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi11);
                PdfPCell fi12 = (new PdfPCell(new Paragraph(fi.Cells["Longitud"].Value.ToString(), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));//Fila 12 hace referencia a la longitud 
                fi12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi12);
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


        /// <summary>
        /// Recive como parametro el datagridview de Altas y lo exporta a una planilla Excel
        /// para guardarlo como archivo o imprimir el mismo
        /// </summary>
        /// <param name="grd"></param>
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
                    hoja_trabajo.Cells[1, j]  = grd.Columns[j-1].HeaderText;
                    hoja_trabajo.Cells[1, j].Font.Bold = true;
                    hoja_trabajo.Cells[1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
                }


                for (int i = 0; i <= grd.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 2, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }

                libros_trabajo.SaveAs(fichero.FileName,  Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                string NombreArchivo = System.IO.Path.GetFileName(fichero.FileName);
                
                libros_trabajo.Close(true);
                aplicacion.Quit();

                //Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                ExportarAltasPDF(grd, NombreArchivo, fichero.FileName);


                if (!Directory.Exists(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas)))
                {
                    Directory.CreateDirectory(Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas));
                    File.Copy(fichero.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo);
                }
                else
                {
                    File.Copy(fichero.FileName, Vble.CarpetaRespaldo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesAltas) + NombreArchivo);
                }
                
            }
        }


        /// <summary>
        /// Se exporta la tabla en vista a un archivo pdf en la direccion que se especifica cuando se guarda el archivo.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="NombreArchivo"></param>
        private void ExportarOrdenativosPDF(DataGridView grd, string NombreArchivo, string SavePath)
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
                 chunk = new Chunk("         Informe de Ordenativos \n\n         Periodo: " + LabelPeriodoOrden.Text + "\n\n    Ruta: " + RutaDesdeExportacion,
                                   FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                   new iTextSharp.text.BaseColor(0, 102, 0)));

                if (tabPageOrdenativos.Text == "Ordenativos")
                {
                    chunk = new Chunk("         Informe de Ordenativos \n\n         Periodo: " + labelPeriodoAlt.Text + " \n\n   " + " Ruta: " + RutaDesdeExportacion,
                                   FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                   new iTextSharp.text.BaseColor(0, 102, 0)));
                }
            }
            else
            {
                chunk = new Chunk("         Informe de Ordenativos \n\n         Periodo: " + LabelPeriodoOrden.Text,
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
            PdfPTable table = new PdfPTable(13);
            table.WidthPercentage = 180;
            table.TotalWidth = page.Width - 90;
            table.LockedWidth = true;
            //asigno el ancho de las columnas
            float[] widths = new float[] { 1.0f, 1.3f, 2.0f, 2.0f, 4.0f, 4.0f, 1.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 4.5f };
            table.SetWidths(widths);


            ////Estructura de tabla:
            ////Ruta|Fecha|Hora|Modelo|Numero|Estado|Domicilio|Observaciones|Lecturista
            //PdfPCell Periodo = (new PdfPCell(new Paragraph("Periodo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Periodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Periodo);
            PdfPCell Localidad = (new PdfPCell(new Paragraph("Localidad", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Localidad.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Localidad);
            PdfPCell Ruta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Ruta);
            PdfPCell Instalacion = (new PdfPCell(new Paragraph("Instalacion", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Instalacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Instalacion);
            //PdfPCell Modelo = (new PdfPCell(new Paragraph("Modelo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Modelo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Modelo);
            PdfPCell Medidor = (new PdfPCell(new Phrase("Medidor", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Medidor.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Medidor);
            PdfPCell FechaLectura = (new PdfPCell(new Paragraph("Fecha_de_Lectura", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            FechaLectura.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(FechaLectura);
            PdfPCell HoraLectura = (new PdfPCell(new Paragraph("Hora_de_Lectura", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            HoraLectura.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(HoraLectura);
            PdfPCell Estado = (new PdfPCell(new Paragraph("Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Estado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Estado);
            PdfPCell Uno = (new PdfPCell(new Phrase("1", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Uno.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Uno);
            PdfPCell Dos = (new PdfPCell(new Phrase("2 ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Dos.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Dos);
            PdfPCell Tres = (new PdfPCell(new Phrase("3 ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Tres.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Tres);
            PdfPCell Cuatro = (new PdfPCell(new Phrase("4 ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Cuatro.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Cuatro);
            PdfPCell Cinco = (new PdfPCell(new Phrase("5 ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Cinco.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Cinco);
            PdfPCell Observaciones = (new PdfPCell(new Phrase("Observaciones ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Observaciones.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Observaciones);



            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            foreach (DataGridViewRow fi in grd.Rows)
            {

                //fi.Cells["Observaciones"].DataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ////Agrega los datos de cada descarga a sus columnas correspondientes                                   
                PdfPCell fi1 = (new PdfPCell(new Paragraph(fi.Cells["Localidad"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi1);
                PdfPCell fi2 = (new PdfPCell(new Paragraph(fi.Cells["Ruta"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi2);
                PdfPCell fi3 = (new PdfPCell(new Paragraph(fi.Cells["Instalacion"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi3);
                PdfPCell fi4 = (new PdfPCell(new Paragraph(fi.Cells["Medidor"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi4);
                //PdfPCell fi5 = (new PdfPCell(new Paragraph(fi.Cells["Modelo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi5);
                PdfPCell fi6 = (new PdfPCell(new Paragraph(fi.Cells["Fecha_de_Lectura"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi6);
                PdfPCell fi7 = (new PdfPCell(new Paragraph(fi.Cells["Hora_de_Lectura"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi7);
                PdfPCell fi8 = (new PdfPCell(new Paragraph(fi.Cells["ActualEstado"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi8);
                PdfPCell fi9 = (new PdfPCell(new Paragraph(fi.Cells["Ord1"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi9);
                PdfPCell fi10 = (new PdfPCell(new Paragraph(fi.Cells["Ord2"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi10);
                PdfPCell fi11 = (new PdfPCell(new Paragraph(fi.Cells["Ord3"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi11);
                PdfPCell fi12 = (new PdfPCell(new Paragraph(fi.Cells["Ord4"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi12);
                PdfPCell fi13 = (new PdfPCell(new Paragraph(fi.Cells["Ord5"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi13);
                PdfPCell fi14 = (new PdfPCell(new Paragraph(fi.Cells["Observaciones"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi14.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi14);

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

        /// <summary>
        /// Se exporta la tabla en vista a un archivo pdf en la direccion que se especifica cuando se guarda el archivo.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="NombreArchivo"></param>
        private void ExportarSituacionesPDF(DataTable grd, string NombreArchivo, string SavePath)
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
                chunk = new Chunk("         Informe de Situaciones \n\n         Periodo: " + labelPeriodoAlt.Text + " \n\n   " + " Ruta: " + RutaDesdeExportacion,
                                    FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                    new iTextSharp.text.BaseColor(0, 102, 0)));
            }
            else
            {
                chunk = new Chunk("         Informe de Situaciones \n\n         Periodo: " + labelPeriodoAlt.Text,
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
            PdfPTable table = new PdfPTable(17);
            table.WidthPercentage = 180;
            table.TotalWidth = page.Width - 90;
            table.LockedWidth = true;
            //asigno el ancho de las columnas
            //float[] widths = new float[] {1.5f, 1.3f, 2.0f, 4.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f};
            float[] widths = new float[] {1.6f, 1.6f, 1.5f, 1.5f, 1.5f, 2.0f, 1.5f, 1.7f, 2.2f, 2.1f, 2.1f, 1.5f, 1.5f, 1.5f, 1.5f, 1.0f, 0.8f };
            table.SetWidths(widths);


            ////Estructura de tabla:
            ///      1     |   2   |     3   |      4     |   5   |  6  |  7 |  8     | 9 | 10       | 11      | 12     | 13            | 14      | 15       | 16          |   17
            ///Instalacion|Medidor|EstadoAnt|EstadoActual|Consumo|Fecha|Hora| Estado  |rang| Impresora| Novedad | diasPer| IndicacionDato| PostFact| WebServer|Impresiones  |Lecturista


            //float[] widths = new float[] {1.5f, 1.3f, 2.0f, 4.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f};
            ////Estructura de tabla:
            ///  1  |     2     |   3   |    4    |     5   |      6     |   7   |  8  |  9 |  10     | 11 | 12       | 13      | 14     | 15            | 16      | 17       | 18          |   19
            ////Ruta|Instalacion|Medidor|Domicilio|EstadoAnt|EstadoActual|Consumo|Fecha|Hora| Estado  |rang| Impresora| Novedad | diasPer| IndicacionDato| PostFact| WebServer|Impresiones  |Lecturista
            ///



            //declaracion de tabla para resumen al pie de detalle
            iTextSharp.text.Rectangle page2 = document.PageSize;
            PdfPTable table2 = new PdfPTable(1);
            table2.WidthPercentage = 50;
            table2.TotalWidth =  160;
            table2.LockedWidth = true;
            //asigno el ancho de las columnas
            //float[] widths = new float[] {1.5f, 1.3f, 2.0f, 4.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f};
            float[] widths2 = new float[] { 5.0f};
            table2.SetWidths(widths2);

            //PdfPCell Periodo = (new PdfPCell(new Paragraph("Periodo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Periodo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Periodo);
            //PdfPCell Ruta = (new PdfPCell(new Paragraph("Ruta", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Ruta.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Ruta);
            PdfPCell Instalación = (new PdfPCell(new Phrase("IC ", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Instalación.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Instalación);
            //PdfPCell Titular = (new PdfPCell(new Paragraph("Titular", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Titular.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Titular);           
            PdfPCell Numero = (new PdfPCell(new Paragraph("N° Med", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Numero.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Numero);
            //PdfPCell Dom = (new PdfPCell(new Paragraph("Domicilio", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            //Dom.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(Dom);
            PdfPCell AnteriorEstado = (new PdfPCell(new Paragraph("Ant Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            AnteriorEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(AnteriorEstado);
            PdfPCell ActualEstado = (new PdfPCell(new Phrase("Actual Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ActualEstado);
            PdfPCell ConsumoFacturado = (new PdfPCell(new Phrase("Cons", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            ConsumoFacturado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ConsumoFacturado);
            PdfPCell Fecha = (new PdfPCell(new Phrase("Fecha", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Fecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Fecha);
            PdfPCell Hora = (new PdfPCell(new Phrase("Hora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            ActualEstado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Hora);
            PdfPCell Estado = (new PdfPCell(new Phrase("Estado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Estado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Estado);
            PdfPCell Rango = (new PdfPCell(new Phrase("Rango", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Rango.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Rango);
            PdfPCell Impresora = (new PdfPCell(new Phrase("Impresora", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Impresora.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Impresora);
            PdfPCell Novedad = (new PdfPCell(new Phrase("Nov", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Novedad.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Novedad);
            PdfPCell diasPer = (new PdfPCell(new Phrase("Dias Periodo", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            diasPer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(diasPer);
            PdfPCell IndicadoDato = (new PdfPCell(new Phrase("Indicado", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            IndicadoDato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(IndicadoDato);
            PdfPCell PostFacturacion = (new PdfPCell(new Phrase("Post Fac", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            PostFacturacion.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(PostFacturacion);
            PdfPCell WebServer = (new PdfPCell(new Phrase("WebServer", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            WebServer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(WebServer);
            PdfPCell Impresiones = (new PdfPCell(new Phrase("N° Imp", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Impresiones.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Impresiones);
            PdfPCell Lecturista = (new PdfPCell(new Phrase("Oper", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Lecturista.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Lecturista);


            //Agrego los datos de cada registro de Alta a sus columnas Correspondientes
            ////Estructura de tabla:
            ///  1  |     2     |   3   |    4    |     5   |      6     |   7   |  8  |  9 |  10     | 11 | 12       | 13      | 14     | 15            | 16      | 17       | 18          |   19
            ////Ruta|Instalacion|Medidor|Domicilio|EstadoAnt|EstadoActual|Consumo|Fecha|Hora| Estado  |rang| Impresora| Novedad | diasPer| IndicacionDato| PostFact| WebServer|Impresiones  |Lecturista
            foreach (DataRow fi in grd.Rows)
            {

                //fi.Cells["Observaciones"].DataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ////Agrega los datos de cada descarga a sus columnas correspondientes                                   
                //PdfPCell fi1 = (new PdfPCell(new Paragraph(fi.Cells["Periodo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi1);
                //PdfPCell fi2 = (new PdfPCell(new Paragraph(fi["Ruta"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi2);
                PdfPCell fi3 = (new PdfPCell(new Paragraph(Convert.ToInt32(fi["ConexionID"]).ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi3);
                PdfPCell fi4 = (new PdfPCell(new Paragraph(fi["Medidor"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi4);
                //PdfPCell fi5 = (new PdfPCell(new Paragraph(fi.Cells["Modelo"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi5);
                //PdfPCell fi6 = (new PdfPCell(new Paragraph(fi["Domicilio"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //table.AddCell(fi6);
                PdfPCell fi7 = (new PdfPCell(new Paragraph(fi["AnteriorEstado"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi7);
                //PdfPCell fi8 = (new PdfPCell(new Paragraph(fi.Cells["Domicilio"].Value.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                //fi8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //table.AddCell(fi8);
                PdfPCell fi9 = (new PdfPCell(new Paragraph(fi["ActualEstado"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                table.AddCell(fi9);
                PdfPCell fi10 = (new PdfPCell(new Paragraph(fi["ConsumoFacturado"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi10);
                PdfPCell fi10bis = (new PdfPCell(new Paragraph(Convert.ToDateTime(fi["ActualFecha"]).ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi10bis.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi10bis);
                PdfPCell fi10bis2 = (new PdfPCell(new Paragraph(fi["ActualHora"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi10bis2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi10bis2);
                PdfPCell fi11 = (new PdfPCell(new Paragraph(fi["Estado"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi11);
                PdfPCell fi12 = (new PdfPCell(new Paragraph(fi["Rango Consumo"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi12);
                PdfPCell fi13 = (new PdfPCell(new Paragraph(fi["Impresora"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi13);
                PdfPCell fi14 = (new PdfPCell(new Paragraph(fi["Novedades"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi14.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi14);
                PdfPCell fi15 = (new PdfPCell(new Paragraph(fi["Días del Periodo"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi15.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi15);
                PdfPCell fi16 = (new PdfPCell(new Paragraph(fi["Indicación en Dato"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi16.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi16);
                PdfPCell fi17 = (new PdfPCell(new Paragraph(fi["Post Facturación"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi17.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi17);
                PdfPCell fi18 = (new PdfPCell(new Paragraph(fi["Web Server"].ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL))));
                fi18.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi18);
                PdfPCell fi19 = (new PdfPCell(new Paragraph(fi["Impresiones"].ToString(), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));
                fi19.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi19);
                PdfPCell fi20 = (new PdfPCell(new Paragraph(fi["Operario"].ToString(), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL))));
                fi20.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(fi20);
            }



            PdfPCell ResumenTitulo = (new PdfPCell(new Phrase("Resumen de Ruta " + LblRutaDetSit.Text, FontFactory.GetFont("Arial", 15, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            ResumenTitulo.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            table2.AddCell(ResumenTitulo);
            PdfPCell TotalUsuarios = (new PdfPCell(new Phrase("Total Usuarios: " + LblTotalUsers.Text, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            TotalUsuarios.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            table2.AddCell(TotalUsuarios);
            PdfPCell Leidos = (new PdfPCell(new Phrase("Total Solo Leidos: " + LblLeidos.Text, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Leidos.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            table2.AddCell(Leidos);
            PdfPCell Impresos = (new PdfPCell(new Phrase("Total Impresos: " + LblImpresos.Text, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD))) { Rowspan = 1 });
            Impresos.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            table2.AddCell(Impresos);

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
            document.Add(new Paragraph(" "));
            document.Add(table2);
            wri.Add(Total);
            //wri.Close();
            document.Add(Total);
            document.Add(new Paragraph(" "));
            document.Close();

        }

        /// <summary>
        /// Boton que exporta la tabla altas a un Excel para guardarlo como archivo segun criterio del operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (DGAlta.RowCount > 0)
            {
                //metodo de exportación con datagridview de altas como parametro
                ExportarAltasExcel(DGAlta);
               
            }
            else
            {
                MessageBox.Show("Disculpe No existen Altas para realizar la Exportación, verifique la busqueda " +
                    "o tal vez aún no existen altas en la base de datos", "Exportación", MessageBoxButtons.OK , MessageBoxIcon.Asterisk);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void dateTimePickerDesde_ValueChanged(object sender, EventArgs e)
        {
            CargarAltasPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
        }

        private void dateTimePickerHasta_ValueChanged(object sender, EventArgs e)
        {
            CargarAltasPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));

        }

        /// <summary>
        /// RadioButtonPeriodo, al seleccionar se muestra el comboboxPeriodo el cual contiene
        /// los periodos vigentes para poder filtrar las altas por periodos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonPeriodo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPeriodo.Checked == true)
            {
                DTPDesdeAltas.Visible = false;
                DTPHastaAltas.Visible = false;
                labelMotivo.Visible = false;
                CBTipoAlta.Visible = false;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = false;
                CBPerDesdeAltas.Visible = true;
                CBPerHastaAltas.Visible = true;
                if (CantidadAltas() > 0)
                {
                    CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);
                }
            }
        }

        private void comboBoxPerDesde_SelectedIndexChanged(object sender, EventArgs e)
        {            
            CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);            
        }

        private void comboBoxPerHasta_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButtonTipoAlta.Checked == true)
            {
                DTPDesdeAltas.Visible = false;
                DTPHastaAltas.Visible = false;
                CBTipoAlta.Visible = true;
                labelMotivo.Visible = true;
                label3.Visible = true;
                label1.Visible = false;
                label2.Visible = false;
                CBPerDesdeAltas.Visible = false;
                CBPerHastaAltas.Visible = false;

                if (CantidadAltas() > 0)
                {
                    //aca hiria consulta por tipo de alta (Modificación o Alta nueva)
                }
            }
        }

        private void ComboBoxTipoAlta_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPorTipoAlta(this.CBTipoAlta.Text);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //DGAlta.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            //DGAlta.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            string LeyendaCabecera = "";

            if (DGAlta.RowCount > 0)
            {
            if (radioButtonFecha.Checked == true)
            {
                LeyendaCabecera = "Resumen de Altas por fecha \n\r Desde: " + DTPDesdeAltas.Value.ToString("dd/MM/yyyy") + " Hasta: " + DTPHastaAltas.Value.ToString("dd/MM/yyyy") +
                      "\n\r Periodo: " + labelPeriodoAlt.Text;
            }
            else if (radioButtonPeriodo.Checked == true)
            {
                LeyendaCabecera = "Resumen de Altas por Periodo \n\r Desde: " + DTPDesdeAltas.Value.ToString("dd/MM/yyyy") + " Hasta: " + DTPHastaAltas.Value.ToString("dd/MM/yyyy") +
                      "\n\r Periodo: " + labelPeriodoAlt.Text;
            }
            else if (radioButtonTipoAlta.Checked == true)
            {
                if (CBTipoAlta.Text == "A")
                {
                    LeyendaCabecera = "Resumen de Altas \n\r Periodo: " + labelPeriodoAlt.Text;
                }
                else
                {
                    LeyendaCabecera = "Resumen de Modificaciones \n\r Periodo: " + labelPeriodoAlt.Text;


                }
            }

            DGAlta.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            PrintDataGridView pr = new PrintDataGridView(DGAlta)
            {
                ReportHeader = LeyendaCabecera,
                ReportFooter = "Macro Intell - DPEC",
                MargenDerecho = 10,
                MargenIzquierdo = 30,
                MargenInferior = 20,
                MargenSuperior = 30
            };

            pr.Print(this);
            }
            else
            {
                MessageBox.Show("NO existen datos para imprimir", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "PDF (*.pdf)|.*pdf";
            if (fichero.ShowDialog() == DialogResult.OK)
            {                
                //Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                ExportarAltasPDF(DGAlta, System.IO.Path.GetFileName(fichero.FileName), System.IO.Path.GetFullPath(fichero.FileName));

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.DGAlta.RowCount > 0)
            {
                if (TBBuscarRuta.Text.Any(x => char.IsNumber(x)))
                {                  
                    string fieldName = string.Concat("[", TablaAltas.Columns[4].ColumnName, "]");
                    //string fieldName = "conexionID";
                    TablaAltas.DefaultView.Sort = fieldName;
                    DataView view = TablaAltas.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TBBuscarRuta.Text != string.Empty)
                        //view.RowFilter = String.Format("Convert(Nº_Conexion, 'System.String') like '%{0}%'", TBBuscar.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                        view.RowFilter = String.Format("Convert(Ruta, 'System.String') like '%{0}%'", TBBuscarRuta.Text);
                    DGAlta.DataSource = view;
                    LabCantidadAlt.Text = "Cantidad: " + view.Count.ToString();
                }
                else
                {
                    if (radioButtonFecha.Checked == true)
                    {
                        DTPDesdeAltas.Visible = true;
                        DTPHastaAltas.Visible = true;
                        CBPerDesdeAltas.Visible = false;
                        CBPerHastaAltas.Visible = false;
                        CBTipoAlta.Visible = false;
                        label3.Visible = false;
                        label1.Visible = true;
                        label2.Visible = true;
                        labelMotivo.Visible = false;
                        if (CantidadAltas() > 0)
                        {
                            CargarAltasPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
                        }
                       
                    }
                    else if (radioButtonPeriodo.Checked == true)
                    {
                        DTPDesdeAltas.Visible = false;
                        DTPHastaAltas.Visible = false;
                        labelMotivo.Visible = false;
                        CBTipoAlta.Visible = false;
                        label1.Visible = true;
                        label2.Visible = true;
                        label3.Visible = false;
                        CBPerDesdeAltas.Visible = true;
                        CBPerHastaAltas.Visible = true;
                        if (CantidadAltas() > 0)
                        {
                            CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);
                        }
                       
                    }
                    else if (radioButtonTipoAlta.Checked == true)
                    {
                        DTPDesdeAltas.Visible = false;
                        DTPHastaAltas.Visible = false;
                        CBTipoAlta.Visible = true;
                        labelMotivo.Visible = true;
                        label3.Visible = true;
                        label1.Visible = false;
                        label2.Visible = false;
                        CBPerDesdeAltas.Visible = false;
                        CBPerHastaAltas.Visible = false;

                        if (CantidadAltas() > 0)
                        {
                            CargarPorTipoAlta(this.CBTipoAlta.Text);
                        }
                        
                    }
                    else
                    {
                        DTPDesdeAltas.Visible = false;
                        DTPHastaAltas.Visible = false;
                        labelMotivo.Visible = false;
                        CBTipoAlta.Visible = false;
                        label1.Visible = true;
                        label2.Visible = true;
                        label3.Visible = false;
                        CBPerDesdeAltas.Visible = true;
                        CBPerHastaAltas.Visible = true;
                        if (CantidadAltas() > 0)
                        {
                            CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);
                        }
                       
                    }
                }
            }
            else
            {
                 
                if (radioButtonFecha.Checked == true)
                {
                    DTPDesdeAltas.Visible = true;
                    DTPHastaAltas.Visible = true;
                    CBPerDesdeAltas.Visible = false;
                    CBPerHastaAltas.Visible = false;
                    CBTipoAlta.Visible = false;
                    label3.Visible = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    labelMotivo.Visible = false;
                    if (CantidadAltas() > 0)
                    {
                        CargarAltasPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
                    }
                    
                }
                else if (radioButtonPeriodo.Checked == true)
                {
                    DTPDesdeAltas.Visible = false;
                    DTPHastaAltas.Visible = false;
                    labelMotivo.Visible = false;
                    CBTipoAlta.Visible = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = false;
                    CBPerDesdeAltas.Visible = true;
                    CBPerHastaAltas.Visible = true;
                    if (CantidadAltas() > 0)
                    {
                        CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);
                    }
                   
                }
                else if (radioButtonTipoAlta.Checked == true)
                {
                    DTPDesdeAltas.Visible = false;
                    DTPHastaAltas.Visible = false;
                    CBTipoAlta.Visible = true;
                    labelMotivo.Visible = true;
                    label3.Visible = true;
                    label1.Visible = false;
                    label2.Visible = false;
                    CBPerDesdeAltas.Visible = false;
                    CBPerHastaAltas.Visible = false;

                    if (CantidadAltas() > 0)
                    {
                        CargarPorTipoAlta(this.CBTipoAlta.Text);
                    }
                    
                }
             
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //private void DGAlta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
           
        //}

        //private void DGAlta_MouseClick(object sender, MouseEventArgs e)
        //{
           
        //}

        //private void DGAlta_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{

        //    contextMenuStrip1.Items.Clear();
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        contextMenuStrip1.Items.Add("Ocultar Registro").Name = "OCULTAR REGISTRO";
        //        //Obtienes las coordenadas de la celda seleccionada. 
        //        System.Drawing.Rectangle coordenada = DGAlta.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
        //        int anchoCelda = coordenada.Location.X; //Ancho de la localizacion de la celda
        //        int altoCelda = coordenada.Location.Y;  //Alto de la localizacion de la celda
        //        //Y para mostrar el menú lo haces de esta forma:  
        //        int X = anchoCelda + DGAlta.Location.X;
        //        int Y = altoCelda + DGAlta.Location.Y + 15;

        //        contextMenuStrip1.Show(DGAlta, new Point(X, Y));               
               
        //    }

           
              
           
        //    //if (e.Button == MouseButtons.Right)
        //    //{
        //    //    this.DGAlta.Rows[e.RowIndex].Selected = true;
        //    //    this.rowIndex = e.RowIndex;
        //    //    this.DGAlta.CurrentCell = this.DGAlta.Rows[e.RowIndex].Cells[1];
        //    //    this.contextMenuStrip1.Show(this.DGAlta, e.Location);
        //    //    contextMenuStrip1.Show(Cursor.Position);
        //    //}

        //    //if (e.Button == MouseButtons.Right)
        //    //{
        //    //    if (MessageBox.Show("¿Desea eliminar el registro seleccionado?", "¡Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        //    //    {
        //    //        //string _codigo_barra = DGAlta.Rows[rowIndex].Cells[1].Value.ToString();
        //    //        ////_bdVentas.DeleteProduct(_codigo_barra);
        //    //        this.DGAlta.Rows.Remove(this.DGAlta.CurrentRow);
        //    //        this.DGAlta.Refresh();

        //    //    }
        //    //}

        //}

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "OCULTAR REGISTRO")
            {
                this.DGAlta.Rows.Remove(this.DGAlta.CurrentRow);
                this.DGAlta.Refresh();
            }

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "PDF (*.pdf)|.*pdf";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                //Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                ExportarAltasPDF(DGConDir, System.IO.Path.GetFileName(fichero.FileName), System.IO.Path.GetFullPath(fichero.FileName));

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //DGAlta.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            //DGAlta.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            string LeyendaCabecera = "";

            if (DGConDir.RowCount > 0)
            {
                if (RBFechaConDir.Checked == true)
                {
                    LeyendaCabecera = "Resumen de Conexiones Directas por fecha \n\r Desde: " + DTPDesdeConDir.Value.ToString("dd/MM/yyyy") + " Hasta: " + DTPHastaAltas.Value.ToString("dd/MM/yyyy") +
                          "\n\r Periodo: " + labelPeriodoConDir.Text;
                }
                else if (RBPeriodoConDir.Checked == true)
                {
                    LeyendaCabecera = "Resumen de Conexiones Directas por Periodo \n\r Desde: " + DTPDesdeConDir.Value.ToString("dd/MM/yyyy") + " Hasta: " + DTPHastaAltas.Value.ToString("dd/MM/yyyy") +
                          "\n\r Periodo: " + labelPeriodoConDir.Text;
                }              
                

                DGConDir.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                PrintDataGridView pr = new PrintDataGridView(DGConDir)
                {
                    ReportHeader = LeyendaCabecera,
                    ReportFooter = "Macro Intell - DPEC",
                    MargenDerecho = 10,
                    MargenIzquierdo = 30,
                    MargenInferior = 20,
                    MargenSuperior = 30
                };

                pr.Print(this);
            }
            else
            {
                MessageBox.Show("NO existen datos para imprimir", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
           


        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (RBFechaConDir.Checked == true)
            {
                DTPDesdeConDir.Visible = true;
                DTPHastaConDir.Visible = true;
                CBPerDesdeConDir.Visible = false;
                CBPerHastaConDir.Visible = false;
                DTPDesdeConDir.Visible = true;
                DTPHastaConDir.Visible = true;
                labelPerDesdeConDir.Visible = true;
                labelPerHastaConDir.Visible = true;
                labelMotivo.Visible = false;
                if (CantidadConexDirec() > 0)
                {
                    CargarConDirPorFechas(DTPDesdeConDir.Value.ToString("yyyy/MM/dd"), DTPHastaConDir.Value.ToString("yyyy/MM/dd"));
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void RBPeriodoConDir_CheckedChanged(object sender, EventArgs e)
        {
            if (RBPeriodoConDir.Checked == true)
            {
                DTPDesdeConDir.Visible = false;
                DTPHastaConDir.Visible = false;
                labelPerDesdeConDir.Visible = true;
                labelPerHastaConDir.Visible = true;                
                CBPerDesdeConDir.Visible = true;
                CBPerHastaConDir.Visible = true;
                if (CantidadConexDirec() > 0)
                {
                    CargarConDirPorPeriodos(CBPerDesdeConDir.Text, CBPerHastaConDir.Text);
                }
            }
        }

        private void LabCantidadAlt_Click(object sender, EventArgs e)
        {

        }

        private void CBPerDesdeConDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarConDirPorPeriodos(CBPerDesdeConDir.Text, CBPerHastaConDir.Text);
        }

        private void CBPerHastaConDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarConDirPorPeriodos(CBPerDesdeConDir.Text, CBPerHastaConDir.Text);
        }

        private void DTPDesdeConDir_ValueChanged(object sender, EventArgs e)
        {
            CargarConDirPorFechas(DTPDesdeConDir.Value.ToString("yyyy-MM-dd"), DTPHastaConDir.Value.ToString("yyyy-MM-dd"));
        }

        private void DTPHastaConDir_ValueChanged(object sender, EventArgs e)
        {
            CargarConDirPorFechas(DTPDesdeConDir.Value.ToString("yyyy/MM/dd"), DTPHastaConDir.Value.ToString("yyyy/MM/dd"));
        }

        private void TBBuscarRutaConexDirec_TextChanged(object sender, EventArgs e)
        {
            if (this.DGConDir.RowCount > 0)
            {
                if (TBBuscarRutaConexDirec.Text.Any(x => char.IsNumber(x)))
                {
                    string fieldName = string.Concat("[", TablaConexDirec.Columns[4].ColumnName, "]");
                    //string fieldName = "conexionID";
                    TablaConexDirec.DefaultView.Sort = fieldName;
                    DataView view = TablaConexDirec.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TBBuscarRutaConexDirec.Text != string.Empty)
                        //view.RowFilter = String.Format("Convert(Nº_Conexion, 'System.String') like '%{0}%'", TBBuscar.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                        view.RowFilter = String.Format("Convert(Ruta, 'System.String') like '%{0}%'", TBBuscarRutaConexDirec.Text);
                    DGConDir.DataSource = view;
                    labelCantConDir.Text = "Cantidad: " + view.Count.ToString();
                }
                else
                {
                    if (RBFechaConDir.Checked == true)
                    {
                        DTPDesdeConDir.Visible = true;
                        DTPHastaConDir.Visible = true;
                        CBPerDesdeConDir.Visible = false;
                        CBPerHastaConDir.Visible = false;
                        labelPerDesdeConDir.Visible = true;
                        labelPerHastaConDir.Visible = true;

                        if (CantidadConexDirec() > 0)
                        {
                            CargarConDirPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
                        }
                    }
                    else if (RBPeriodoConDir.Checked == true)
                    {
                        DTPDesdeConDir.Visible = false;
                        DTPHastaConDir.Visible = false;
                        labelPerDesdeConDir.Visible = true;
                        labelPerHastaConDir.Visible = true;
                        labelPerDesdeConDir.Visible = true;
                        labelPerHastaConDir.Visible = true;
                        if (CantidadConexDirec() > 0)
                        {
                            CargarConDirPorPeriodos(CBPerDesdeConDir.Text, CBPerHastaConDir.Text);
                        }
                    }
                }
            }
            else
            {

                if (radioButtonFecha.Checked == true)
                {
                    DTPDesdeAltas.Visible = true;
                    DTPHastaAltas.Visible = true;
                    CBPerDesdeAltas.Visible = false;
                    CBPerHastaAltas.Visible = false;
                    CBTipoAlta.Visible = false;
                    label3.Visible = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    labelMotivo.Visible = false;
                    if (CantidadConexDirec() > 0)
                    {
                        CargarConDirPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
                    }

                }
                else if (radioButtonPeriodo.Checked == true)
                {
                    DTPDesdeAltas.Visible = false;
                    DTPHastaAltas.Visible = false;
                    labelMotivo.Visible = false;
                    CBTipoAlta.Visible = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = false;
                    CBPerDesdeConDir.Visible = true;
                    CBPerHastaConDir.Visible = true;
                    if (CantidadConexDirec() > 0)
                    {
                        CargarConDirPorPeriodos(CBPerDesdeConDir.Text, CBPerHastaConDir.Text);
                    }
                }               
            }            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TBBuscarRutaOrdenativos_TextChanged(object sender, EventArgs e)
        {
            if (this.DGOrdenat.RowCount > 0)
            {
                if (TBBuscarRutaOrdenativos.Text.Any(x => char.IsNumber(x)))
                {
                    string fieldName = string.Concat("[", TablaNovedades.Columns[4].ColumnName, "]");
                    //string fieldName = "conexionID";
                    TablaNovedades.DefaultView.Sort = fieldName;
                    DataView view = TablaNovedades.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TBBuscarRutaOrdenativos.Text != string.Empty)
                        //view.RowFilter = String.Format("Convert(Nº_Conexion, 'System.String') like '%{0}%'", TBBuscar.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                        view.RowFilter = String.Format("Convert(Ruta, 'System.String') like '%{0}%'", TBBuscarRutaOrdenativos.Text);
                    DGOrdenat.DataSource = view;
                    LabCanConexConOrd.Text = "Cantidad: " + view.Count.ToString();
                }
                else
                {
                    if (RBFechaNov.Checked == true)
                    {
                        DTPDesdeConDir.Visible = true;
                        DTPHastaConDir.Visible = true;
                        CBPerDesdeConDir.Visible = false;
                        CBPerHastaConDir.Visible = false;
                        labelPerDesdeConDir.Visible = true;
                        labelPerHastaConDir.Visible = true;

                        if (CantidadConOrdenativos() > 0)
                        {
                            CargarOrdenativosPorFecha(DTPFechaNovDesde.Value.ToString("yyyy/MM/dd"), DTPFechaNovHasta.Value.ToString("yyyy/MM/dd"));
                        }
                    }
                    else if (RBPeriodoNov.Checked == true)
                    {
                        DTPDesdeConDir.Visible = false;
                        DTPHastaConDir.Visible = false;
                        labelPerDesdeConDir.Visible = true;
                        labelPerHastaConDir.Visible = true;
                        labelPerDesdeConDir.Visible = true;
                        labelPerHastaConDir.Visible = true;
                        if (CantidadConOrdenativos() > 0)
                        {
                            CargarOrdenativosPorPeriodo(CBPeriodoNovDesde.Text, CBPeriodoNovHasta.Text);
                        }
                    }
                }
            }
            else
            {

                if (radioButtonFecha.Checked == true)
                {
                    DTPDesdeAltas.Visible = true;
                    DTPHastaAltas.Visible = true;
                    CBPerDesdeAltas.Visible = false;
                    CBPerHastaAltas.Visible = false;
                    CBTipoAlta.Visible = false;
                    label3.Visible = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    labelMotivo.Visible = false;
                    if (CantidadAltas() > 0)
                    {
                        CargarAltasPorFechas(DTPDesdeAltas.Value.ToString("yyyy/MM/dd"), DTPHastaAltas.Value.ToString("yyyy/MM/dd"));
                    }

                }
                else if (radioButtonPeriodo.Checked == true)
                {
                    DTPDesdeAltas.Visible = false;
                    DTPHastaAltas.Visible = false;
                    labelMotivo.Visible = false;
                    CBTipoAlta.Visible = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = false;
                    CBPerDesdeAltas.Visible = true;
                    CBPerHastaAltas.Visible = true;
                    if (CantidadAltas() > 0)
                    {
                        CargarAltasPorPeriodos(CBPerDesdeAltas.Text, CBPerHastaAltas.Text);
                    }

                }
                else if (radioButtonTipoAlta.Checked == true)
                {
                    DTPDesdeAltas.Visible = false;
                    DTPHastaAltas.Visible = false;
                    CBTipoAlta.Visible = true;
                    labelMotivo.Visible = true;
                    label3.Visible = true;
                    label1.Visible = false;
                    label2.Visible = false;
                    CBPerDesdeAltas.Visible = false;
                    CBPerHastaAltas.Visible = false;

                    if (CantidadAltas() > 0)
                    {
                        CargarPorTipoAlta(this.CBTipoAlta.Text);
                    }

                }

            }

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "PDF (*.pdf)|.*pdf";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                //Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                ExportarOrdenativosPDF(DGOrdenat, System.IO.Path.GetFileName(fichero.FileName), System.IO.Path.GetFullPath(fichero.FileName));

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TextFiltro_TextChanged(object sender, EventArgs e)
        {
            if (this.DGDetalleSituaciones.RowCount > 0)
            {
                if (RBRuta.Checked)
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Ruta"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = TablaDetalleSituaciones.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Ruta, 'System.String') like '%{0}%' AND Periodo = " + LblPeriodoDetSit.Text, TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";                    
                    DGDetalleSituaciones.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (RBInstalacion.Checked)
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["ConexionID"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = TablaDetalleSituaciones.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(ConexionID, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGDetalleSituaciones.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (RBContrato.Checked)
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Contrato"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = TablaDetalleSituaciones.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Contrato, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGDetalleSituaciones.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (RBTitular.Checked)
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["titularID"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = TablaDetalleSituaciones.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(titularID, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGDetalleSituaciones.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                else if (RBNumMed.Checked)
                {
                    string NombreCampo = string.Concat("[", Tabla.Columns["Medidor"].ColumnName, "]");
                    //string fieldName = "conexionID";
                    Tabla.DefaultView.Sort = NombreCampo;
                    DataView view = TablaDetalleSituaciones.DefaultView;
                    view.RowFilter = string.Empty;
                    if (TextFiltro.Text != string.Empty)
                        view.RowFilter = String.Format("Convert(Medidor, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                    DGDetalleSituaciones.DataSource = view;
                    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                    //ConsiderarOrdenativosAlImprimir();
                }
                //else if (TipoInforme == "R" || TipoInforme == "T")
                //{
                //    string NombreCampo = string.Concat("[", Tabla.Columns["Interfaz"].ColumnName, "]");
                //    //string fieldName = "conexionID";
                //    Tabla.DefaultView.Sort = NombreCampo;
                //    DataView view = Tabla.DefaultView;
                //    view.RowFilter = string.Empty;
                //    if (TextFiltro.Text != string.Empty)
                //        view.RowFilter = String.Format("Convert(Interfaz, 'System.String') like '%{0}%'", TextFiltro.Text);  // + " LIKE '%" + TextNºInstalacion.Text + "%'";
                //    DGDetalleSituaciones.DataSource = view;
                //    LabCantidad.Text = "Cantidad: " + view.Count.ToString();
                //    //ConsiderarOrdenativosAlImprimir();

                //    ArmarGraficos(DGResumenExp, TipoInforme);
                //}

            }

            else
            {
                CargarDetalleSituaciones();
                //this.TextNºInstalacion_TextChanged(sender, e);
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "PDF (*.pdf)|.*pdf";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                //Exporto lo mismo a un archivo PDF que queda como respaldo de FIS aparte del archivo .xls 
                ExportarSituacionesPDF(TablaDetalleSituaciones, System.IO.Path.GetFileName(fichero.FileName), System.IO.Path.GetFullPath(fichero.FileName));

            }
        }

        private void DGAlta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGAlta_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items.Add("Ocultar Registro").Name = "OCULTAR REGISTRO";
                //Obtienes las coordenadas de la celda seleccionada. 
                System.Drawing.Rectangle coordenada = DGAlta.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int anchoCelda = coordenada.Location.X; //Ancho de la localizacion de la celda
                int altoCelda = coordenada.Location.Y;  //Alto de la localizacion de la celda
                //Y para mostrar el menú lo haces de esta forma:  
                int X = anchoCelda + DGAlta.Location.X;
                int Y = altoCelda + DGAlta.Location.Y + 15;

                contextMenuStrip1.Show(DGAlta, new Point(X, Y));

            }




            //if (e.Button == MouseButtons.Right)
            //{
            //    this.DGAlta.Rows[e.RowIndex].Selected = true;
            //    this.rowIndex = e.RowIndex;
            //    this.DGAlta.CurrentCell = this.DGAlta.Rows[e.RowIndex].Cells[1];
            //    this.contextMenuStrip1.Show(this.DGAlta, e.Location);
            //    contextMenuStrip1.Show(Cursor.Position);
            //}

            //if (e.Button == MouseButtons.Right)
            //{
            //    if (MessageBox.Show("¿Desea eliminar el registro seleccionado?", "¡Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //    {
            //        //string _codigo_barra = DGAlta.Rows[rowIndex].Cells[1].Value.ToString();
            //        ////_bdVentas.DeleteProduct(_codigo_barra);
            //        this.DGAlta.Rows.Remove(this.DGAlta.CurrentRow);
            //        this.DGAlta.Refresh();

            //    }
            //}

        }

        private void DGAlta_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged_2(object sender, EventArgs e)
        {
            if (RBSin98_98.Checked == true)
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                string txSQL = "SELECT C.Zona AS Localidad, C.Ruta, C.ConexionID as Instalacion, M.Numero as Medidor, " +
                 "M.ActualFecha AS Fecha_de_Lectura, M.ActualHora as Hora_de_Lectura, " +
                 "M.ActualEstado, " +
                 "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                 "FROM Conexiones C " +
                 "JOIN Medidores M USING (ConexionID, Periodo) " +
                 //"JOIN NovedadesConex N USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                 //"ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                 "USING (ConexionID, Periodo) " +
                 "WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0].ToString() + iteracionZona() + ") AND C.Ruta = " + RutaDesdeExportacion +
                 " AND (N1.Codigo < 98 and N1.Codigo > 0)" +
                 "ORDER BY C.Ruta, M.ActualFecha ASC, M.ActualHora ASC";
                //" ORDER BY Fecha ASC";

                TablaNovedades = new DataTable();

                MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                datosAdapter.SelectCommand.CommandTimeout = 300;
                MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaNovedades);
                DGOrdenat.DataSource = TablaNovedades;

                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
                //DGOrdenat.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //DGOrdenat.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;


                DGOrdenat.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //DGOrdenat.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //DGOrdenat.Columns["Periodo"].Visible = false;
                //DGOrdenat.Columns["Fecha"].Visible = false;
                LabelPeriodoOrden.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                LabCanConexConOrd.Text = "Cantidad = " + DGOrdenat.RowCount.ToString();

                LblRutaOrdenativos.Visible = true;
                TBBuscarRutaOrdenativos.Visible = true;
                lblCantOrd.Text =  "Cantidad = " + DGOrdenat.RowCount.ToString();
            }
         
        }

        private void RBTodosOrd_CheckedChanged(object sender, EventArgs e)
        {
            if (RBTodosOrd.Checked == true)
            {
                //Lee la tabla ALTAS pertenecientes al periodo
                string txSQL = "SELECT C.Zona AS Localidad, C.Ruta, C.ConexionID as Instalacion, M.Numero as Medidor, " +
                 "M.ActualFecha AS Fecha_de_Lectura, M.ActualHora as Hora_de_Lectura, " +
                 "M.ActualEstado, " +
                 "N1.Codigo as Ord1, N2.Codigo as Ord2, N3.Codigo as Ord3, N4.Codigo as Ord4, N5.Codigo as Ord5, N6.Observ as Observaciones " +
                 "FROM Conexiones C " +
                 "JOIN Medidores M USING (ConexionID, Periodo) " +
                 //"JOIN NovedadesConex N USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 1 and Periodo = " + Vble.Periodo + ") N1 " +
                 //"ON N1.ConexionID = C.ConexionID AND N1.Periodo = C.Periodo " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 2 and Periodo = " + Vble.Periodo + ") N2 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 3 and Periodo = " + Vble.Periodo + ") N3 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 4 and Periodo = " + Vble.Periodo + ") N4 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN(SELECT ConexionID, Periodo, Codigo FROM NovedadesConex WHERE Orden = 5 and Periodo = " + Vble.Periodo + ") N5 " +
                 "USING (ConexionID, Periodo) " +
                 "left JOIN (SELECT ConexionID, Periodo, Codigo, Observ FROM NovedadesConex WHERE (Orden = 0 OR Observ <> '') and Periodo = " + Vble.Periodo + ") N6 " +
                 "USING (ConexionID, Periodo) " +
                 "WHERE C.Periodo = " + Vble.Periodo + " AND (C.Zona = " + Vble.ArrayZona[0].ToString() + iteracionZona() + ") AND C.Ruta = " + RutaDesdeExportacion +
                 " AND N1.Codigo <> '' " +
                 "ORDER BY C.Ruta, M.ActualFecha ASC, M.ActualHora ASC";
                //" ORDER BY Fecha ASC";

                TablaNovedades = new DataTable();

                MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                datosAdapter.SelectCommand.CommandTimeout = 300;
                MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaNovedades);
                DGOrdenat.DataSource = TablaNovedades;

                //DGAlta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
                //DGOrdenat.Columns["Ruta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //DGOrdenat.Columns["Operario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;


                DGOrdenat.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //DGOrdenat.Columns["Observaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //DGOrdenat.Columns["Periodo"].Visible = false;
                //DGOrdenat.Columns["Fecha"].Visible = false;
                LabelPeriodoOrden.Text = Vble.Periodo.ToString().Substring(4, 2) + "-" + Vble.Periodo.ToString().Substring(0, 4);
                LabCanConexConOrd.Text = "Cantidad = " + DGOrdenat.RowCount.ToString();

                LblRutaOrdenativos.Visible = true;
                TBBuscarRutaOrdenativos.Visible = true;
                lblCantOrd.Text =  "Cantidad = " + DGOrdenat.RowCount.ToString();
            }
        }

        private void Form7InformesAltas_Shown(object sender, EventArgs e)
        {
           
        }

        private void bgwInicioPantalla_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }

        private void bgwInicioPantalla_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Termino el proceso");
        }

        private void Form7InformesAltas_Resize(object sender, EventArgs e)
        {
            ///centro el label y textbox de la pestaña Altas
            // Centrar el Label
            labelRuta.Left = (this.ClientSize.Width - labelRuta.Width) /2;
            labelRuta.Top = (this.ClientSize.Height - labelRuta.Height) / 4; // Ajusta la posición superior del Label si es necesario
            // Centrar el TextBox debajo del Label
            TBBuscarRuta.Left = (this.ClientSize.Width - TBBuscarRuta.Width) / 2;
            TBBuscarRuta.Top = labelRuta.Bottom + 10; // Ajusta la distancia entre el Label y el TextBox

            ///centro el label y textbox de la pestaña Conexiones Directas
            // Centrar el Label
            LblRutaConexDirc.Left = (this.ClientSize.Width - LblRutaConexDirc.Width) / 2;
            LblRutaConexDirc.Top = (this.ClientSize.Height - LblRutaConexDirc.Height) / 4; // Ajusta la posición superior del Label si es necesario
            // Centrar el TextBox debajo del Label
            TBBuscarRutaConexDirec.Left = (this.ClientSize.Width - TBBuscarRutaConexDirec.Width) / 2;
            TBBuscarRutaConexDirec.Top = LblRutaConexDirc.Bottom + 10; // Ajusta la distancia entre el Label y el TextBox

            ///centro el label y textbox de la pestaña Ordenativos
            // Centrar el Label
            LblRutaOrdenativos.Left = (this.ClientSize.Width - LblRutaOrdenativos.Width) / 2;
            LblRutaOrdenativos.Top = (this.ClientSize.Height - LblRutaOrdenativos.Height) / 4; // Ajusta la posición superior del Label si es necesario
            // Centrar el TextBox debajo del Label
            TBBuscarRutaOrdenativos.Left = (this.ClientSize.Width - TBBuscarRutaOrdenativos.Width) / 2;
            TBBuscarRutaOrdenativos.Top = LblRutaOrdenativos.Bottom + 10; // Ajusta la distancia entre el Label y el TextBox
        }
    }
}
