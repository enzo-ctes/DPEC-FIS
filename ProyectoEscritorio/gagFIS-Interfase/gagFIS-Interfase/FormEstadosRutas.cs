﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;



namespace gagFIS_Interfase
{
    public partial class FormEstadosRutas : Form
    {
        DataTable Tabla = new DataTable();
        public string Remesa { get; set; }

        public FormEstadosRutas()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormEstadosRutas_Load(object sender, EventArgs e)
        {
            CargarEstadoRutas(Remesa);
            BtnDisponibles.ForeColor = Color.Black;
            BtnCerrarSaldos.ForeColor = Color.Black;
            BtnHabCarga.ForeColor = Color.Black;
            BtnHabExp.ForeColor = Color.Black;
        }

        #region Metodos y Funciones
        public void CargarEstadoRutas(string rem)
        {
            try
            {
                LVEstados.Items.Clear();
                DataTable TableImportados = new DataTable();
                string PeriodoImportadas = Vble.Periodo.ToString().Replace("-", "");

                string txSQL = "SELECT * FROM LogImportacion WHERE Periodo = " + Vble.Periodo +
                                " AND (Zona = " + Vble.ArrayZona[0] + iteracionzona() + ") AND Porcion LIKE '" + rem + "%'";
                MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
                MySqlCommand comandCant = new MySqlCommand();
                datosAdapter.Fill(TableImportados);

                datosAdapter.Dispose();
                comandoSQL.Dispose();
                //string Remesa;
                string Zona;
                string Ruta;
                int CantDisponibles, CantProcesados, CantColectora, CantDescargados, CantExportados, CantCerrados;
                string txSQLCantidades;               


                ListViewItem ResumenImportacion;
                ResumenImportacion = new ListViewItem();

                string QueryGrupal = "SELECT " +
                                     "Zona, " +
                                     "Remesa, " +
                                     "Ruta, " +
                                     "Count(*) AS CantUsuarios, " +
                                     "SUM(IF ((ImpresionOBS = 0 OR ImpresionOBS = 300 OR ImpresionOBS = 500), 1, 0)) AS Disponibles, " +
                                     "SUM(IF (ImpresionOBS = 300, 1, 0)) AS ProcParaCargar, " +
                                     "SUM(IF (ImpresionOBS = 400, 1, 0)) AS EnColectora, " +
                                     "SUM(IF ((ImpresionOBS > 500 AND ImpresionOBS < 600), 1, 0)) AS Descagardos, " +
                                     "SUM(IF ((ImpresionOBS > 600 AND ImpresionOBS < 700), 1, 0)) AS Exportados, " +
                                     "SUM(IF (ImpresionOBS = 800, 1, 0)) AS Cerrados " +
                                     "FROM Conexiones " +
                                     "WHERE " +
                                     "Periodo = " + Vble.Periodo + " " +
                                     "AND Remesa = " + rem + " " +
                                     "AND (Ruta = " + TableImportados.Rows[0]["Porcion"].ToString().Substring(5) + " " + iteracionRutas(TableImportados) + ") " +
                                     "GROUP BY Ruta " +
                                     "ORDER BY Ruta ";


               
                     

                        using (MySqlCommand command = new MySqlCommand(QueryGrupal, DB.conexBD))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                // Leer los registros
                                while (reader.Read())
                                {
                                   
                                    ResumenImportacion = new ListViewItem(reader.GetString("Zona"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("Remesa"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("Ruta"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("CantUsuarios"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("Disponibles"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("ProcParaCargar"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("EnColectora"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("Descagardos"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("Exportados"));
                                    ResumenImportacion.SubItems.Add(reader.GetString("Cerrados"));
                                    LVEstados.Items.Add(ResumenImportacion);

                        }
                        
                        reader.Dispose();
                    }
                    command.Dispose();
                            
                }


                //if (TableImportados.Rows.Count >= 1)
                //{
                //    foreach (DataRow item in TableImportados.Rows)
                //    {
                        ////Remesa = item["Porcion"].ToString().Substring(0,1);
                        //Zona = item["Porcion"].ToString().Substring(1, 3);
                        //Ruta = item["Porcion"].ToString().Substring(5);
                        //ResumenImportacion = new ListViewItem(Zona);
                        //ResumenImportacion.SubItems.Add(item["Porcion"].ToString().Substring(0,1));
                        //ResumenImportacion.SubItems.Add(Ruta);
                        //ResumenImportacion.SubItems.Add(item["CantUsuarios"].ToString());
                        /////Por cada Ruta:
                        /////Obtengo la cantidad de usuarios que estan aun disponibles para cargar ser procesados
                        //txSQLCantidades = "SELECT Count(ConexionID) as Disponible From Conexiones WHERE Zona = " + Zona + " and Periodo = " + PeriodoImportadas + " and Remesa = " + rem + " and Ruta = " + Ruta +
                        //                          " AND (ImpresionOBS = 0 OR ImpresionOBS = 500) AND(ImpresionOBS <> 800)";                       
                        //comandCant = new MySqlCommand(txSQLCantidades, DB.conexBD);
                        //CantDisponibles = Convert.ToInt32(comandCant.ExecuteScalar());                       
                        //comandCant.Dispose();
                        //ResumenImportacion.SubItems.Add(CantDisponibles.ToString());
                        /////Obtengo la cantidad de usuarios por cada ruta se encuentran procesadas y listas para ser cargadas a colectoras.
                        //txSQLCantidades = "SELECT Count(ConexionID) as Disponible From Conexiones WHERE Zona = " + Zona + " and Periodo = "+ PeriodoImportadas + " and Remesa = " + rem + " and Ruta = " + Ruta  +
                        //                  " AND ImpresionOBS = 300";

                        //comandCant = new MySqlCommand(txSQLCantidades, DB.conexBD);
                        //CantProcesados = Convert.ToInt32(comandCant.ExecuteScalar());
                        //comandCant.Dispose();
                        //ResumenImportacion.SubItems.Add(CantProcesados.ToString());

                        /////Obtengo la cantidad de usuarios por cada ruta que estan asignada a alguna colectora (cargadas)
                        //txSQLCantidades = "SELECT Count(ConexionID) as Disponible From Conexiones WHERE Zona = " + Zona + " and Periodo = " + PeriodoImportadas + " and Remesa = " + rem + " and Ruta = " + Ruta +
                        //                  " AND ImpresionOBS = 400";
                        //comandCant = new MySqlCommand(txSQLCantidades, DB.conexBD);
                        //CantColectora = Convert.ToInt32(comandCant.ExecuteScalar());
                        //comandCant.Dispose();
                        //ResumenImportacion.SubItems.Add(CantColectora.ToString());

                        /////Obtengo la cantidad de usuarios por cada ruta que hayan sido descargadas con alguna lectura y sin haber sido exportados
                        //txSQLCantidades = "SELECT Count(ConexionID) as Disponible From Conexiones WHERE Zona = " + Zona + " and Periodo = " + PeriodoImportadas + " and Remesa = " + rem + " and Ruta = " + Ruta +
                        //                  " AND ImpresionOBS > 500 and ImpresionOBS < 600";
                        //comandCant = new MySqlCommand(txSQLCantidades, DB.conexBD);
                        //CantDescargados = Convert.ToInt32(comandCant.ExecuteScalar());
                        //comandCant.Dispose();
                        //ResumenImportacion.SubItems.Add(CantDescargados.ToString());

                        /////Obtengo la cantidad de usuarios por cada ruta que hayan sido exportados
                        //txSQLCantidades = "SELECT Count(ConexionID) as Disponible From Conexiones WHERE Zona = " + Zona + " and Periodo = " + PeriodoImportadas + " and Remesa = " + rem + " and Ruta = " + Ruta +
                        //                  " AND ImpresionOBS > 600 and ImpresionOBS < 700";
                        //comandCant = new MySqlCommand(txSQLCantidades, DB.conexBD);
                        //CantExportados = Convert.ToInt32(comandCant.ExecuteScalar());
                        //comandCant.Dispose();
                        //ResumenImportacion.SubItems.Add(CantExportados.ToString());
                        ///Obtengo la cantidad de usuarios por cada ruta que hayan quedado como saldo en FIS y fueron cerrados 
                        ///para que no aparezcan mas como disponibles para ser procesadas nuevamente.
                        //txSQLCantidades = "SELECT Count(ConexionID) as Disponible From Conexiones WHERE Zona = " + Zona + " and Periodo = " + PeriodoImportadas + " and Remesa = " + rem + " and Ruta = " + Ruta +
                        //                  " AND ImpresionOBS = 800";
                        //comandCant = new MySqlCommand(txSQLCantidades, DB.conexBD);
                        //CantCerrados = Convert.ToInt32(comandCant.ExecuteScalar());
                        //comandCant.Dispose();
                        //ResumenImportacion.SubItems.Add(CantCerrados.ToString());
                        //LVEstados.Items.Add(ResumenImportacion);
                //    }
                //}
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        /// <summary>
        /// Recorre el ArrayList que contiene los codigos de localidades almacenados al leer el archivo
        /// ZonaFIS.txt de cada centro de localidad. Si el archivo no contine nignun codigo devuelve vacio.
        /// </summary>
        /// <returns></returns>
        private string iteracionRutas(DataTable tablaImportacion)
        {
            string iteracion = "";
        

            if (tablaImportacion.Rows.Count > 1)
            {
                for (int i = 1; i < tablaImportacion.Rows.Count; i++)
                {
                    iteracion += " OR Ruta = " + tablaImportacion.Rows[i]["Porcion"].ToString().Substring(5);
                }
            }

            return iteracion;

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

        /// <summary>
        /// Este Metodo, ejectura una consulta Mysql donde dejara disponible nuevamente la ruta que se encuentra con estado de ImpresionOBS = 300 (Procesado)
        /// para que se vuelva a generar la carga en caso de que se haya perdido.
        /// 
        /// <param name="Zona"></param>
        /// <param name="Remesa"></param>
        /// <param name="Ruta"></param>
        private void DejarDisponibleProcesados(string Zona, string remesa, string Ruta, int CantProcesados)
        {
            
            DataTable TableProcesados = new DataTable();
            string PeriodoImportadas = Vble.Periodo.ToString().Replace("-", "");
            string txSQLCantProcesados = "";
            string txSQLUpdateProcesados = "";
            Int32 CantProcesadosEnBase = 0;
            MySqlCommand comandCant = new MySqlCommand();
            MySqlCommand comandUpdateProcesados = new MySqlCommand();

            //txSQLCantProcesados = "SELECT Count(*) FROM Conexiones WHERE Periodo = " + PeriodoImportadas +
            //                    " AND Zona = " + Zona + iteracionzona() + " AND Remesa = " + Remesa + " AND Ruta = " + Ruta +
            //                    " AND ImpresionOBS = 300";
            txSQLCantProcesados = "SELECT Count(*) FROM Conexiones WHERE Periodo = " + PeriodoImportadas +
                               " AND Zona = " + Zona + " AND Remesa = " + Remesa + " AND Ruta = " + Ruta +
                               " AND ImpresionOBS = 300";
            comandCant = new MySqlCommand(txSQLCantProcesados, DB.conexBD);
            CantProcesadosEnBase = Convert.ToInt32(comandCant.ExecuteScalar());

            comandCant.Dispose();


            if (CantProcesadosEnBase == CantProcesados)
            {
                txSQLUpdateProcesados = "UPDATE Conexiones SET ImpresionOBS = 0 WHERE Periodo = " + PeriodoImportadas +
                               " AND Zona = " + Zona + " AND Remesa = " + Remesa + " AND Ruta = " + Ruta +
                               " AND ImpresionOBS = 300";
                comandUpdateProcesados = new MySqlCommand(txSQLUpdateProcesados, DB.conexBD);
                comandUpdateProcesados.ExecuteNonQuery();
                comandUpdateProcesados.Dispose();
                CargarEstadoRutas(remesa);

                Form4Cargas PantallaCargas = Application.OpenForms.OfType<Form4Cargas>().FirstOrDefault();

                if (PantallaCargas != null)
                {
                    PantallaCargas.listarRutasDisponiblesTASK();
                    PantallaCargas.LimpiarPanelCargasAenviar();
                    PantallaCargas.CargasProcesadas();
                    PantallaCargas.LeeCargasEnviadas();
                    PantallaCargas.LeerCargasRecibidas();
                    PantallaCargas.Refresh();
                }



                Ctte.ArchivoLog.EscribirLog("Se volvió a dejar disponible la ruta " + Ruta +", que tenia " + CantProcesados + " usuarios marcados como procesados ImpresionOBS = 300");
                MessageBox.Show("Se cambió el estado de los usuarios de PROCESADOS a DISPONIBLES de la porción seleccionada",
                    "Estado de porción cambiada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            
        }





        /// <summary>
        /// Este Metodo, ejectura una consulta Mysql donde dejara disponible nuevamente la ruta que se encuentra con estado de ImpresionOBS = 300 (Procesado)
        /// para que se vuelva a generar la carga en caso de que se haya perdido.
        /// 
        /// <param name="Zona"></param>
        /// <param name="Remesa"></param>
        /// <param name="Ruta"></param>
        private void CerrarSaldos(string Zona, string remesa, string Ruta, int CantProcesados)
        {

            DataTable TableProcesados = new DataTable();
            string PeriodoImportadas = Vble.Periodo.ToString().Replace("-", "");
            string txSQLCantDisSaldos = "";
            string txSQLUpdateDisSaldos = "";
            Int32 CantProcesadosEnBase = 0;
            MySqlCommand comandCant = new MySqlCommand();
            MySqlCommand comandUpdateProcesados = new MySqlCommand();

            //txSQLCantProcesados = "SELECT Count(*) FROM Conexiones WHERE Periodo = " + PeriodoImportadas +
            //                    " AND Zona = " + Zona + iteracionzona() + " AND Remesa = " + Remesa + " AND Ruta = " + Ruta +
            //                    " AND ImpresionOBS = 300";
            txSQLCantDisSaldos = "SELECT Count(*) FROM Conexiones WHERE Periodo = " + PeriodoImportadas +
                               " AND Zona = " + Zona + " AND Remesa = " + Remesa + " AND Ruta = " + Ruta +
                               " AND (ImpresionOBS = 500 OR ImpresionOBS = 0)";
            comandCant = new MySqlCommand(txSQLCantDisSaldos, DB.conexBD);
            CantProcesadosEnBase = Convert.ToInt32(comandCant.ExecuteScalar());

            comandCant.Dispose();


            if (CantProcesadosEnBase == CantProcesados)
            {
                txSQLUpdateDisSaldos = "UPDATE Conexiones SET ImpresionOBS = 800 WHERE Periodo = " + PeriodoImportadas +
                               " AND Zona = " + Zona + " AND Remesa = " + Remesa + " AND Ruta = " + Ruta +
                               " AND (ImpresionOBS = 500 OR ImpresionOBS = 0)";
                comandUpdateProcesados = new MySqlCommand(txSQLUpdateDisSaldos, DB.conexBD);
                comandUpdateProcesados.ExecuteNonQuery();
                comandUpdateProcesados.Dispose();
                CargarEstadoRutas(remesa);

                Form4Cargas PantallaCargas = Application.OpenForms.OfType<Form4Cargas>().FirstOrDefault();

                if (PantallaCargas != null)
                {
                    PantallaCargas.listarRutasDisponiblesTASK();
                    PantallaCargas.LimpiarPanelCargasAenviar();
                    PantallaCargas.CargasProcesadas();
                    PantallaCargas.LeeCargasEnviadas();
                    PantallaCargas.LeerCargasRecibidas();
                    PantallaCargas.Refresh();
                }



                Ctte.ArchivoLog.EscribirLog("Se volvió a dejar disponible la ruta " + Ruta + ", que tenia " + CantProcesados + " usuarios marcados como procesados ImpresionOBS = 300");
                MessageBox.Show("Se cambió el estado de los usuarios de PROCESADOS a DISPONIBLES de la porción seleccionada",
                    "Estado de porción cambiada", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        /// que se recibe como parametro.
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="LeyendaImpresion"></param>
        /// <param name="ImpresionOBS"></param>                    
        public static void VerDetallePreDescarga(string LeyendaImpresion, int Periodo, string CONSULTA,
                                                 bool NoImpresos, string ImpresionOBS, string Remesa,
                                                 string Ruta, string IndicadorTipoInforme, string EstadoRuta)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();

            DetalleImpresos.IndicadorTipoInforme = IndicadorTipoInforme;
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            //DetalleImpresos.leyenda.Text = Leyenda;
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
            DetalleImpresos.Visible = false;
            DetalleImpresos.CBRemesa.Text = Remesa;
            DetalleImpresos.PantallaEstRuta = "SI";
            DetalleImpresos.TipoInforme = "ER";
            DetalleImpresos.Show();
        }

        private void DejarDisponibleEnColectora(string zona, string remesa, string ruta, int cantidadEnColectra)
        {
            DataTable TableProcesados = new DataTable();
            string PeriodoImportadas = Vble.Periodo.ToString().Replace("-", "");
            string txSQLCantColectora = "";
            string txSQLUpdateColectora = "";
            Int32 CantColectoraEnBase = 0;
            MySqlCommand comandCant = new MySqlCommand();
            MySqlCommand comandUpdateColectora = new MySqlCommand();

            txSQLCantColectora = "SELECT Count(*) FROM Conexiones WHERE Periodo = " + PeriodoImportadas +
                                " AND Zona = " + zona + " AND Remesa = " + remesa + " AND Ruta = " + ruta +
                                " AND ImpresionOBS = 400";
            comandCant = new MySqlCommand(txSQLCantColectora, DB.conexBD);
            CantColectoraEnBase = Convert.ToInt32(comandCant.ExecuteScalar());

            comandCant.Dispose();


            if (CantColectoraEnBase == cantidadEnColectra)
            {
                txSQLUpdateColectora = "UPDATE Conexiones SET ImpresionOBS = 0 WHERE Periodo = " + PeriodoImportadas +
                               " AND Zona = " + zona + " AND Remesa = " + remesa + " AND Ruta = " + ruta +
                               " AND ImpresionOBS = 400";
                comandUpdateColectora = new MySqlCommand(txSQLUpdateColectora, DB.conexBD);
                comandUpdateColectora.ExecuteNonQuery();
                comandUpdateColectora.Dispose();
                CargarEstadoRutas(remesa);

                Form4Cargas PantallaCargas = Application.OpenForms.OfType<Form4Cargas>().FirstOrDefault();

                if (PantallaCargas != null)
                {
                    PantallaCargas.listarRutasDisponiblesTASK();
                    PantallaCargas.LimpiarPanelCargasAenviar();
                    PantallaCargas.CargasProcesadas();
                    PantallaCargas.LeeCargasEnviadas();
                    PantallaCargas.LeerCargasRecibidas();
                    PantallaCargas.Refresh();
                }

                Ctte.ArchivoLog.EscribirLog("Se volvió a dejar disponible la ruta " + ruta +", que tenía " + cantidadEnColectra + " usuarios marcados como 'EN COLECTORA' ImpresionOBS = 400");
                MessageBox.Show("Se cambió el estado de los usuarios EN COLECTORA a DISPONIBLES de la porción seleccionada",
                    "Estado de porción cambiada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        private void BtnDisponibles_Click(object sender, EventArgs e)
        {
            if (LVEstados.SelectedItems != null)
            {
                string Zona = LVEstados.SelectedItems[0].SubItems[0].Text;
                string Remesa = LVEstados.SelectedItems[0].SubItems[1].Text;
                string Ruta = LVEstados.SelectedItems[0].SubItems[2].Text;

                string CONSULTA = "SELECT DISTINCT C.Secuencia, C.Periodo, C.Remesa, C.Ruta, C.ConexionID AS NInstalacion, C.Contrato, C.titularID AS IC, P.Apellido, M.Numero AS Medidor, " +
                "C.DomicSumin as Domicilio, " +
                "if (ImpresionOBS = 400, 'EN CALLE', IF(ImpresionOBS = 500, 'NO LEIDO', IF(ImpresionOBS = 0, 'NO LEIDO', E.Titulo))) AS 'Situacion Actual', C.Operario " +
                "FROM Conexiones C " +
                "INNER JOIN Personas P ON C.TitularID = P.PersonaID AND C.Periodo = P.Periodo " +
                "INNER JOIN Errores E ON C.ImpresionOBS MOD 100 = E.Codigo " +
                "INNER JOIN Medidores M ON C.ConexionID = M.ConexionID AND C.Periodo = M.Periodo " +
                "WHERE((C.ImpresionOBS MOD 100 = 0 AND C.ImpresionOBS <> 800 )  and C.Periodo = " + Vble.Periodo +
                " AND C.Remesa = " + Remesa + " AND C.Ruta = " + Ruta + ")  and Zona = " + Zona +  
                " GROUP BY C.ConexionID, M.ConexionID order by C.Secuencia ";

                VerDetallePreDescarga("DispSaldos", Vble.Periodo, CONSULTA, true, "000", Remesa,
                                     Ruta, "Resumen", "ER");
            }
        }

       

        private void BtnHabCarga_Click(object sender, EventArgs e)
        {
            int CantidadProc = 0;
            int CantEnColectora = 0;
            string Zona = "0";
            string Remesa = "0";
            string Ruta = "0";
            DialogResult mjeConfirmacion = new DialogResult();

            if (LVEstados.SelectedItems != null)
            {
                Zona = LVEstados.SelectedItems[0].SubItems[0].Text;
                Remesa = LVEstados.SelectedItems[0].SubItems[1].Text;
                Ruta = LVEstados.SelectedItems[0].SubItems[2].Text;
                CantEnColectora = Convert.ToInt32(LVEstados.SelectedItems[0].SubItems[6].Text);
                CantidadProc = Convert.ToInt32(LVEstados.SelectedItems[0].SubItems[5].Text);
                //CantEnColectora = Convert.ToInt32(LVEstados.SelectedItems[0].SubItems[6].Text);

                if (CantidadProc > 0 && CantEnColectora > 0)
                {
                    if (MessageBox.Show("La porción seleccionada contiene usuarios EN COLECTORA y PROCESADOS, si continua devolverá el total de usuarios en ambos estados a DISPONIBLES", "Devolver Rutas", MessageBoxButtons.YesNo,
                      MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        DejarDisponibleProcesados(Zona, Remesa, Ruta, CantidadProc);
                        DejarDisponibleEnColectora(Zona, Remesa, Ruta, CantEnColectora);
                    }
                }
                else if (CantidadProc > 0)
                {
                    if (MessageBox.Show("¿Desea devolver los " + CantidadProc.ToString() + " usuarios PROCESADOS de la ruta " + Ruta +  ", al estado DISPONIBLES para volver a procesarlos?", "Devolver Rutas", MessageBoxButtons.YesNo,
                      MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        DejarDisponibleProcesados(Zona, Remesa, Ruta, CantidadProc);
                    }
                }
                else if (CantEnColectora > 0)
                {
                    if (MessageBox.Show("¿Desea devolver los " + CantEnColectora.ToString() + " usuarios EN COLECTORA de la ruta " + Ruta + ", al estado DISPONIBLES para volver a procesarlos?", "Devolver Rutas", MessageBoxButtons.YesNo,
                      MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        DejarDisponibleEnColectora(Zona, Remesa, Ruta, CantEnColectora);
                    }
                }
                else
                {
                    MessageBox.Show("No existen usarios PROCESADOS o EN COLECTORA para cambiar al estado DISPONIBLE",
                        "Función invalida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnCerrarSaldos_Click(object sender, EventArgs e)
        {
            int CantidadProc = 0;
            int CantEnColectora = 0;
            int DisSaldos = 0;
            string Zona = "0";
            string Remesa = "0";
            string Ruta = "0";
            DialogResult mjeConfirmacion = new DialogResult();

            if (LVEstados.SelectedItems != null)
            {
                Zona = LVEstados.SelectedItems[0].SubItems[0].Text;
                Remesa = LVEstados.SelectedItems[0].SubItems[1].Text;
                Ruta = LVEstados.SelectedItems[0].SubItems[2].Text;
                DisSaldos = Convert.ToInt32(LVEstados.SelectedItems[0].SubItems[4].Text);
                CantidadProc = Convert.ToInt32(LVEstados.SelectedItems[0].SubItems[5].Text);
                CantEnColectora = Convert.ToInt32(LVEstados.SelectedItems[0].SubItems[6].Text);

                if (DisSaldos > 0)
                {
                    CerrarSaldos(Zona, Remesa, Ruta, DisSaldos);

                    
                }
                
            }
        }
    }
}
