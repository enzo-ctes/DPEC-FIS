using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace gagFIS_Interfase {
    public partial class FormStart : Form {
        private int cnt = 0;
        Form1Inicio Inicio = new Form1Inicio();
        public FormStart() {
            InitializeComponent();
            lstLogin.Items.Clear();
            cnt = 0;
        }

        public string TextoLabel {
            get { return lstLogin.Items.ToString(); }
            set { lstLogin.Items.Add(value); }
        }

        /// <summary>
        /// Inicia la verificación de contraseña, y trata de abrir la base de 
        /// datos, si lo consigue, pasa al siguiente paso, si no lo consigue, 
        /// presenta un mensaje y vuelve a presentar la pantalla de usuario y constraseña.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e) {
            //DB.sDbKey = txtKey.Text;
            //DB.sDbUsu = txtUsuario.Text;
            DB.sDbKey = textKey.Text;
            DB.sDbUsu = textUsuario.Text;
            DB.Entorno = "";


            if (DB.sDbUsu == "admin" & DB.sDbKey == "Micc4001")
            {
                GBEntorno.Visible = true;
            }
            else if (DB.sDbUsu.ToUpper() == "SUPERVISOR" & DB.sDbKey == "Macro2020" || DB.sDbUsu.ToUpper() == "AUDITORIA" & DB.sDbKey == "Macro2020")
            {
                RBPRD.Visible = false;
                RBQAS.Visible = false;
                RBPrueba.Visible = false;
                RBSUP.Visible = true;
                DB.Entorno = "SUP";
                RBSUP.Checked = true;
                IniciarSesion();
            }
            else
            {
                DB.Entorno = "PRD";
                RBPRD.Checked = true;
                IniciarSesion();
            }

          

        }  //final btnAceptar_Clik


        /// <summary>
        /// Cierra el formulario de inicio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e) {
            int cit = lstLogin.Items.Count - 1;
            cnt++;
            lstLogin.Items[cit] = lstLogin.Items[cit] + " -";
            if(cnt > 6)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                
            }
        }

        

        private void grpUsu_Enter(object sender, EventArgs e)
        {

        }

        private void lstLogin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormStart_Load(object sender, EventArgs e)
        {
            //RBPRD.Checked = true;
        }

        private void RBPRD_CheckedChanged(object sender, EventArgs e)
        {
            if (RBPRD.Checked == true)
            {
                RBPRD.Checked = true;
                RBQAS.Checked = false;
                RBPrueba.Checked = false;

                IniciarSesion();
                //RBPRD.Checked = false;

            }
        }

        private void RBQAS_CheckedChanged(object sender, EventArgs e)
        {
            if (RBQAS.Checked == true)
            {
                RBQAS.Checked = true;
                RBPRD.Checked = false;
                RBPrueba.Checked = false;

                IniciarSesion();
            }
        }

        public async void IniciarSesion()
        {
            if (RBPRD.Checked == true)
            {
                DB.Entorno = "PRD";
            }
            else if (RBQAS.Checked == true)
            {
                DB.Entorno = "QAS";
            }
            else if (RBSUP.Checked == true)
            {
                DB.Entorno = "SUP";
            }
            else
            {
                DB.Entorno = "NO";
            }
            //MessageBox.Show(Ctte.CarpetaRecursos.ToString());
            grpUsu.Visible = false;
            lstLogin.Items.Add("----- ----- Abriendo base de datos... ");

            Ctte.ArchivoLog.EscribirLog("Abriendo base de datos. Usuario: " + DB.sDbUsu);
            if (DB.Entorno != "NO")
            {

                if (DB.AbrirBaseDatos(DB.Entorno))
                {
                    lstLogin.Items.Add("Conexión establecida normalmente!! - Usuario: " + DB.sDbUsu);
                    Ctte.ArchivoLog.EscribirLog("Conexión establecida normalmente!! Base: " + DB.NombreBD +
                        ". Usuario: " + DB.sDbUsu);
                    lstLogin.Items.Add("Cargando Datos Generales...");
                    Ctte.ArchivoLog.EscribirLog("Cargando datos Generales");
                    // RECORDARME FALTA CARGAR DATOS GENERALES
                    //cada medio segundos envia tick para cerrar el ormulario de arranque
                    lstLogin.Items.Add("Aguarde -");
                    timer1.Interval = 100;
                    timer1.Enabled = true;
                    
                    Task oTask = new Task(TaskCarBaseFijSP);
                    oTask.Start();
                   
                    await oTask;
                }
                else
                {
                    lstLogin.Items.Add("ERROR AL ESTABLECER LA CONEXIÓN CON LA BASE DE DATOS");
                    Ctte.ArchivoLog.EscribirLog("Error al abrir base de datos: " + DB.NombreBD +
                        "; Usuario: " + DB.sDbUsu);
                    grpUsu.Visible = true;
                    txtKey.Text = "";
                    textUsuario.Focus();
                    //txtUsuario.Focus();
                }

            }
            else
            {
                lstLogin.Items.Add("Debe seleccionar un entorno de Inicio para la aplicacion PRD o QAS");
                Ctte.ArchivoLog.EscribirLog("Error al abrir aplicacion: " + DB.NombreBD +
                    "; Usuario: " + DB.sDbUsu);
                grpUsu.Visible = true;
                txtKey.Text = "";
                textUsuario.Focus();
            }

        }

        private void TaskCarBaseFijSP()
        {
            Inicio.btnImportar.Enabled = false;
            //CarBaseFijSP.RunWorkerAsync();
            MetodoCargarTablasFijas();
            
        }

        private void CarBaseFijSP_DoWork(object sender, DoWorkEventArgs e)
        {

            MetodoCargarTablasFijas();
        }

        private void MetodoCargarTablasFijas()
        {
            try
            {
                //simulateHeavyWork(Vble.CantRegistros);
                MySqlDataAdapter da;
                MySqlCommandBuilder comandoSQL;
                string txSQL;
                DataTable Tabla;
                SQLiteCommand command;


                DB.conSQLiteFija.Open();
                #region Carga de Parametros
                txSQL = "select * From Parametros";
                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

                da.Dispose();
                comandoSQL.Dispose();

                //

                string DeleteParametros = "DELETE FROM Parametros";
                //preparamos la cadena pra insercion
                SQLiteCommand commandParametros = new SQLiteCommand(DeleteParametros, DB.conSQLiteFija);
                //y la ejecutamos
                commandParametros.CommandTimeout = 300;
                commandParametros.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                commandParametros.Dispose();
                //MessageBox.Show("Se eliminaron todos los registros");


                foreach (DataRow RegLect in Tabla.Rows)
                {
                    string InsertParametro = "INSERT INTO Parametros ([Parametro], [Tipo], [Valor]) " +
                       "VALUES ('" + RegLect["Parametro"] + "', '" + RegLect["Tipo"].ToString() + "', '" +
                               RegLect["Valor"].ToString() + "')";

                    //preparamos la cadena pra insercion
                    command = new SQLiteCommand(InsertParametro, DB.conSQLiteFija);
                    //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();


                }


                #endregion

                #region NovedadesTabla
                txSQL = "select * From NovedadesTabla";
                Tabla = new DataTable();
                da = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(da);
                da.Fill(Tabla);

                da.Dispose();
                comandoSQL.Dispose();
                //

                string DeleteNovTabla = "DELETE FROM NovedadesTabla";
                //preparamos la cadena pra insercion
                SQLiteCommand commandNovTabla = new SQLiteCommand(DeleteNovTabla, DB.conSQLiteFija);
                //y la ejecutamos
                commandNovTabla.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                commandNovTabla.Dispose();
                //MessageBox.Show("Se eliminaron todos los registros");


                foreach (DataRow RegLect in Tabla.Rows)
                {
                    if ((int)RegLect["CodigoNov"] >= -1)
                    {
                        string InsertNovedadTabla = "INSERT INTO NovedadesTabla (codigoNov, DetalleNov, Sector, Estimacion, Prioridad) " +
                            "VALUES (" + (int)RegLect["CodigoNov"] + ", '" + RegLect["DetalleNov"].ToString() + "', '" +
                            RegLect["Sector"].ToString() + "', '" + RegLect["Estimacion"].ToString() + "', " + (int)RegLect["Prioridad"] + ")";

                        //preparamos la cadena pra insercion
                        command = new SQLiteCommand(InsertNovedadTabla, DB.conSQLiteFija);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                    }


                }

                #endregion

                DB.conSQLiteFija.Close();
                Inicio.btnImportar.Enabled = true;
            }
            catch (Exception er)
            {

                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + er.Message +
                                                              " Error en el metodo que carga las tablas de la base Fija. Entorno: " + DB.Entorno);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

              
                    var bounds = Screen.FromControl(this).Bounds;
                    //this.Width = bounds.Width - 100;
                    //this.Height = bounds.Height - 100;

                

                MessageBox.Show("Ancho = " + bounds.Width.ToString() + "; Alto = " + bounds.Height.ToString());


            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
            }

        }
    }  //final FormStart
} //final espacio de nombres gagFIS_Interfase
