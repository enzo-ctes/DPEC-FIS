using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.VisualBasic;
using System.IO;

namespace gagFIS_Interfase
{

    

    public partial class FormNuevoTicket : Form
    {
        public FormNuevoTicket()
        {
            InitializeComponent();
        }

        public static Dictionary<string, int> DictionaryEquipos = new Dictionary<string, int>();
        public static Dictionary<string, int> DictionaryIncidentes = new Dictionary<string, int>();
        DataTable Tabla = new DataTable();
        string rutaImagenSeleccionada = "";
        string nombreImagenSeleccionada = "";
        private const int EM_SETCUEBANNER = 0x1501;
        string correoSoporte = "";
        string correoEmisor = "";
        Int16 ticketNro = 0;
        string resumen = "";
        List<string> Archivo = new List<string>();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        private void FormTicket_Load(object sender, EventArgs e)
        {
            SendMessage(TBResponsable.Handle, EM_SETCUEBANNER, 0, "Apellido y Nombre");
            SendMessage(TBRemitente.Handle, EM_SETCUEBANNER, 0, "Apellido y Nombre");
            //SendMessage(TBCorreoEmisor.Handle, EM_SETCUEBANNER, 0, "correoemisor@dpec.com.ar");
            SendMessage(TBPlasholderResumen.Handle, EM_SETCUEBANNER, 0, "Aquí escriba un breve resumen del incidente");
            SendMessage(TBCC.Handle, EM_SETCUEBANNER, 0, "CC1@dpec.com.ar; CC2@dpec.com.ar");          


            StringBuilder stb3 = new StringBuilder("", 500);
            Inis.GetPrivateProfileString("Datos", "correoSoporte", "", stb3, 500, Ctte.ArchivoIniName);
            correoSoporte = stb3.ToString();///dbFIS-DPEC.db
            TBCorreoDestino.Text = correoSoporte;

            StringBuilder stb4 = new StringBuilder("", 500);
            Inis.GetPrivateProfileString("Datos", "correoEmisor", "", stb4, 500, Ctte.ArchivoIniName);
            correoEmisor = stb4.ToString();
            TBCorreoEmisor.Text = correoEmisor;

            DictionaryEquipos.Clear();
            CBEquipo.Items.Clear();
            CBIncidente.Items.Clear();
            cargarCBEquipos();
            cargarCBIncidentes();
            BuscarNombreLocalidad();
            //Vble.CarpetaRespaldo + Vble.RespaldoEnviadas;
            //TBPlasholderResumen.Visible = true;
        }

        private void cargarCBIncidentes()
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            Tabla.Clear();
            DictionaryIncidentes.Clear();
            CBIncidente.Items.Clear();
            string txSQL = "SELECT * FROM Incidente;";

            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            foreach (DataRow Fila in Tabla.Rows) { 
                DictionaryIncidentes.Add(Fila.Field<string>("Nombre"), Fila.Field<int>("idIncidente"));
                CBIncidente.Items.Add(Fila.Field<string>("Nombre"));
            }

            comandoSQL.Dispose();
            datosAdapter.Dispose();
        }

        private void cargarCBEquipos()
        {
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            Tabla.Clear();
            DictionaryEquipos.Clear();
            string txSQL = "SELECT IdEquipo, Nombre FROM equiposTicket;";

            datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            foreach (DataRow Fila in Tabla.Rows)
            {
                DictionaryEquipos.Add(Fila.Field<string>("Nombre"), Fila.Field<int>("IdEquipo"));
                CBEquipo.Items.Add(Fila.Field<string>("Nombre"));
            }

                    comandoSQL.Dispose();
            datosAdapter.Dispose();

        }

        private void TBResumen_TextChanged(object sender, EventArgs e)
        {
         
          

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuscarNombreLocalidad();
        }

        public void BuscarNombreLocalidad()
        {
            switch (CBLocIncid.Text)
            {
                case "201":
                    TBLocalidadIncidente.Text = "Capital";
                    break;
                case "202":
                    TBLocalidadIncidente.Text = "Goya";
                    break;
                case "203":
                    TBLocalidadIncidente.Text = "Itatí";
                    break;
                case "204":
                    TBLocalidadIncidente.Text = "Yapeyú";
                    break;
                case "205":
                    TBLocalidadIncidente.Text = "B. De Astrada";
                    break;
                case "206":
                    TBLocalidadIncidente.Text = "Bella Vista";
                    break;
                case "207":
                    TBLocalidadIncidente.Text = "Bonpland";
                    break;
                case "208":
                    TBLocalidadIncidente.Text = "Curuzú Cuatiá";
                    break;
                case "209":
                    TBLocalidadIncidente.Text = "Concepción";
                    break;
                case "210":
                    TBLocalidadIncidente.Text = "Chavarría";
                    break;
                case "211":
                    TBLocalidadIncidente.Text = "Esquina";
                    break;
                case "212":
                    TBLocalidadIncidente.Text = "P. Fernández";
                    break;
                case "213":
                    TBLocalidadIncidente.Text = "G. Martinez";
                    break;
                case "214":
                    TBLocalidadIncidente.Text = "Virasoro";
                    break;
                case "215":
                    TBLocalidadIncidente.Text = "Caá Catí";
                    break;
                case "216":
                    TBLocalidadIncidente.Text = "Guaviraví";
                    break;
                case "217":
                    TBLocalidadIncidente.Text = "Ituzaingó";
                    break;
                case "218":
                    TBLocalidadIncidente.Text = "Itá Ibaté";
                    break;
                case "219":
                    TBLocalidadIncidente.Text = "La Cruz";
                    break;
                case "220":
                    TBLocalidadIncidente.Text = "Lavalle";
                    break;
                case "221":
                    TBLocalidadIncidente.Text = "Loreto";
                    break;
                case "222":
                    TBLocalidadIncidente.Text = "M.I. Loza";
                    break;
                case "223":
                    TBLocalidadIncidente.Text = "Mburucuyá";
                    break;
                case "224":
                    TBLocalidadIncidente.Text = "Mercedes";
                    break;
                case "225":
                    TBLocalidadIncidente.Text = "Monte Caseros";
                    break;
                case "226":
                    TBLocalidadIncidente.Text = "Mocoretá";
                    break;
                case "227":
                    TBLocalidadIncidente.Text = "9 de Julio";
                    break;
                case "228":
                    TBLocalidadIncidente.Text = "P. de los Libres";
                    break;
                case "229":
                    TBLocalidadIncidente.Text = "P. de la Patria";
                    break;
                case "230":
                    TBLocalidadIncidente.Text = "Perugorría";
                    break;
                case "231":
                    TBLocalidadIncidente.Text = "San Roque";
                    break;
                case "232":
                    TBLocalidadIncidente.Text = "Santo Tomé";
                    break;
                case "233":
                    TBLocalidadIncidente.Text = "San Miguel";
                    break;
                case "234":
                    TBLocalidadIncidente.Text = "San Carlos";
                    break;
                case "235":
                    TBLocalidadIncidente.Text = "Yofre";
                    break;
                case "236":
                    TBLocalidadIncidente.Text = "Alvear";
                    break;
                case "237":
                    TBLocalidadIncidente.Text = "Saladas";
                    break;
                case "238":
                    TBLocalidadIncidente.Text = "El Sombrero";
                    break;
                case "239":
                    TBLocalidadIncidente.Text = "Empedrado";
                    break;
                case "240":
                    TBLocalidadIncidente.Text = "San Cosme";
                    break;
                case "241":
                    TBLocalidadIncidente.Text = "San Lorenzo";
                    break;
                case "242":
                    TBLocalidadIncidente.Text = "Santa Ana";
                    break;
                case "243":
                    TBLocalidadIncidente.Text = "Santa Lucia";
                    break;
                case "244":
                    TBLocalidadIncidente.Text = "Liebig";
                    break;
                case "245":
                    TBLocalidadIncidente.Text = "S.L del Palmar";
                    break;
                case "246":
                    TBLocalidadIncidente.Text = "Sauce";
                    break;
                case "247":
                    TBLocalidadIncidente.Text = "Pellegrini";
                    break;
                case "248":
                    TBLocalidadIncidente.Text = "Santa Rosa";
                    break;
                case "249":
                    TBLocalidadIncidente.Text = "Garruchos";
                    break;
                case "250":
                    TBLocalidadIncidente.Text = "Palmar Grande";
                    break;
                case "251":
                    TBLocalidadIncidente.Text = "E. Torrent";
                    break;
                case "252":
                    TBLocalidadIncidente.Text = "L. de Vallejos";
                    break;
                case "253":
                    TBLocalidadIncidente.Text = "Guayquiraró";
                    break;
                case "254":
                    TBLocalidadIncidente.Text = "Libertador";
                    break;
                case "256":
                    TBLocalidadIncidente.Text = "Riachuelo";
                    break;
                case "257":
                    TBLocalidadIncidente.Text = "San Cayetano";
                    break;
                case "258":
                    TBLocalidadIncidente.Text = "Tabay -Tatacuá";
                    break;
                default:
                    TBLocalidadIncidente.Text = "Tabay -Tatacuá";
                    break;
            }
        }

        private void BtnBuscarFoto_Click(object sender, EventArgs e)
        {
            openFileImage.Filter =  "Image files (*.jpg, *.gif, *.png) | *.jpg; *.gif; *.png";
            if (openFileImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<String> listaExtensiones = new List<string>() { ".jpg", ".gif", ".png" };

                if (listaExtensiones.Contains(Path.GetExtension(openFileImage.FileName)))
                {
                    this.nombreImagenSeleccionada = openFileImage.SafeFileName;
                    this.rutaImagenSeleccionada = openFileImage.FileName;
                    Bitmap bitImage = new Bitmap(this.rutaImagenSeleccionada);
                    pictureBox1.BackgroundImage = bitImage;
                }
                else
                    MessageBox.Show("Solo se permite adjuntar imagenes para enviar el ticket", "Archivo no permitido", MessageBoxButtons.OK, MessageBoxIcon.None);             

                //Archivo.Add(this.rutaImagenSeleccionada);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TBCorreoEmisorDPEC.Text != "")
            {
                if (ValidarCorreoElectronico(TBCC.Text))
                {
                    EnviarMail(DTPikerIncidente.Value.ToString("yyyy-MM-dd"), CBInterfazEmisor.Text, CBLocIncid.Text, TBLocalidadIncidente.Text,
                          CBEquipo.Text, CBIncidente.Text, TBResponsable.Text, TBCorreoEmisor.Text, TBRemitente.Text, TBDni.Text, TBCC.Text);
                }
                else
                {
                    MessageBox.Show("El correo electrónico ingresado en CC no es válido.", "Correo mal ingresado", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar un correo al cual quiere que se reciba la respuesta", "Incompleto", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
           
           

        }

        /// <summary>
        /// Metodo que contiene los parametros mencionados y envia a través del protocolo de mails con los datos correspondientes
        /// al servidor o cuenta desde el cual se envia a los correos que se coloca en el formulario 
        /// </summary>
        /// <param name="Fechaincidente"></param>
        /// <param name="CInterfazEmisor"></param>
        /// <param name="CodLocalidadEmisor"></param>
        /// <param name="NombLocalidadEmisor"></param>
        /// <param name="EquipoDañado"></param>
        /// <param name="Incidente"></param>
        /// <param name="Emisor"></param>
        /// <param name="correoEmisor"></param>
        /// <param name="ResponsableIncidente"></param>
        /// <param name="DniResponsable"></param>
        /// <param name="CorreosCC"></param>
        private void EnviarMail(string Fechaincidente, string CInterfazEmisor, string CodLocalidadEmisor, string NombLocalidadEmisor,
                                string EquipoDañado, string Incidente, string Emisor, string correoEmisor, string ResponsableIncidente, 
                                string DniResponsable, string CorreosCC)
        {
            try
            {
           
            int cantCorreosCC = 0;
            string[] correosCCArray = new string[0];
            MailMessage correo = new MailMessage();
            ticketNro = Convert.ToInt16(obtenenerNroTicket() + 1);
        
            //MessageBox.Show(ticketNro.ToString());

                correo.From = new MailAddress("pruebasticketfis@dpec.com.ar", "Ticket N° " + ticketNro +
                                          " - Localidad: " + CInterfazEmisor.ToString(), System.Text.Encoding.UTF8);//Correo de salida           


            string[] correosS = correoSoporte.Split(';');
            foreach (var item in correosS)
            {
                correo.To.Add(item); //Correo destino?
            }


            if (TBCC.Text != "")
            {
                if (TBCC.Text.Contains(";"))
                {
                    string[] correosCC = TBCC.Text.Split(';');
                    cantCorreosCC = correosCC.Length;
                    correosCCArray = new string[cantCorreosCC];
                    correosCCArray = TBCC.Text.Split(';');
                }
                else
                {
                    correo.To.Add(TBCC.Text.Trim());
                }
            }
            foreach (var item in correosCCArray)
            {
                correo.To.Add(item.Trim());
            }

                //si viene archivo a adjuntar
                //realizamos un recorrido por todos los adjuntos enviados en la lista
                //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
                if (rutaImagenSeleccionada != null)
                {                  
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(rutaImagenSeleccionada))
                            correo.Attachments.Add(new Attachment(rutaImagenSeleccionada));                    
                }

                correo.Subject = "Incidente: " + Incidente + " - Equipo: " + EquipoDañado; //Asunto
            resumen = TBResumen.Text;




                //TBResumen.Text = "Centro de interfaz emisor: " + CInterfazEmisor + Environment.NewLine + 
                //          "\n Localidad del incidente: " + CodLocalidadEmisor + "-" + NombLocalidadEmisor.ToUpper() + Environment.NewLine +
                //          "\n Equipo Afectado: " + EquipoDañado + Environment.NewLine +
                //          "\n Incidente: " + Incidente + Environment.NewLine  +
                //          "\n Descripcion del incidente: " + Environment.NewLine + 
                //          "                         \n-" + TBResumen.Text
                //          + Environment.NewLine + 
                //          TBCorreoEmisorDPEC.Text;

            correo.IsBodyHtml = true;
            string bodyhtml = @"<p>Centro de Interfaz emisor: {0}</p>
            <p>Localidad del Incidente: {1}</p>
            <p>Incidente: {2} </p>
            <p>Equipo Afectado: {3} </p>
            <p>Descripción del Incidente: {4} </p>";
            bodyhtml = string.Format(bodyhtml, CInterfazEmisor, CodLocalidadEmisor, Incidente, EquipoDañado, TBResumen.Text);
            correo.Body = bodyhtml;//Mensaje del correo         
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "maildpec.dpec.com.ar";//Host del servidor de correo
            smtp.Port = 587;//Puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential("pruebasticketfis@dpec.com.ar", "T1ck3tFIS");//Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = true;//True si el servidor de correo permite ssl
            smtp.Send(correo);
            TBResumen.Text = resumen;

            string idEquipo = " ";
            string idIncidente = " ";
           
            foreach (var item in DictionaryEquipos)
            {
                if (item.Key.ToString() == CBEquipo.Text)
                {
                    idEquipo = item.Value.ToString();
                }
            }

            foreach (var item in DictionaryIncidentes)
            {
                if (item.Key.ToString() == CBEquipo.Text)
                {
                    idEquipo = item.Value.ToString();
                }
            }

            guardarTicket(ticketNro, DateTime.Today.Date.ToString("yyyy-MM-dd"), Fechaincidente, CBLocIncid.Text, TBLocalidadIncidente.Text, idEquipo, 
                idIncidente, ResponsableIncidente, DniResponsable, resumen, TBRemitente.Text, "pruebasticketfis@dpec.com.ar", rutaImagenSeleccionada);

                TBResponsable.ResetText();
                TBResumen.ResetText();
                TBRemitente.ResetText();
                TBDni.ResetText();
                TBCC.ResetText();
                TBCorreoEmisorDPEC.ResetText();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al enviar el mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// metodo que contiene el guardado en la tabla ticket de la base de datos con los parametros que recibe el metodo 
        /// de envio del mail a soportefis
        /// </summary>
        /// <param name="ticketNro"></param>
        /// <param name="fechaTicket"></param>
        /// <param name="fechaincidente"></param>
        /// <param name="cBLocIncid"></param>
        /// <param name="tBLocalidadIncidente"></param>
        /// <param name="idEquipo"></param>
        /// <param name="idIncidente"></param>
        /// <param name="responsableIncidente"></param>
        /// <param name="dniResponsable"></param>
        /// <param name="resumen"></param>
        /// <param name="Remitente"></param>
        /// <param name="correo"></param>
        /// <param name="v3"></param>
        private void guardarTicket(Int32 ticketNro, string fechaTicket, string fechaincidente, string cBLocIncid, 
                                   string tBLocalidadIncidente, string idEquipo, string idIncidente, string responsableIncidente,
                                   string dniResponsable, string resumen, string Remitente, string correo, string v3)
        {
            try
            {
                byte[] avatar = null;
                if (v3 == "")
                {
                    avatar = null;
                }
                else
                {
                    avatar = convertirAvatarAByte(v3);
                }

            

            //txSQL = "select * From conexiones Where conexionID
            string txSQL = "INSERT INTO ticket(NroTicket, FechaRegistro, FechaIncidente, Zona, Interfaz, " +
                " IdEquipo, IdIncidente, Responsable, Dni, Resumen, Remitente, Correo," +
                " Foto, Estado) " +
                "VALUES ( " + ticketNro +  ", '" +  fechaTicket + "', '" +  fechaincidente + "', '" +  cBLocIncid  + "', '" +
                            tBLocalidadIncidente  + "', " +  idEquipo + ", '" +  idIncidente + "', '" +  
                            responsableIncidente + "', '" +  dniResponsable + "', '" +  resumen + "', '" +  Remitente + "', '" +
                            correo + "', '" + avatar + "', 'ABIERTO')";

            //preparamos la cadena pra insercion
            MySqlCommand command = new MySqlCommand(txSQL, DB.conexBD);
            //y la ejecutamos
            command.ExecuteNonQuery();
            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
            command.Dispose();


                // seteo las variables que contienen la direccion y el nombre de la imagen adjunta

                rutaImagenSeleccionada = "";
                nombreImagenSeleccionada = "";

            }
            catch (Exception)
            {
                MessageBox.Show("Error al guardar el ticket mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public static byte[] convertirAvatarAByte(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] avatar = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return avatar;
        }


        /// <summary>
        /// Metodo que obtiene el siguiente numero de ticket para armar el mail a enviar
        /// </summary>
        /// <returns></returns>
        public Int16 obtenenerNroTicket()
        {
            string resp = "0";
            Int16 siguienteTicket = 0;
            string query = "SELECT MAX(NroTicket) FROM ticket";
            MySqlCommand cmd = new MySqlCommand(query, DB.conexBD);

            if (cmd.ExecuteScalar() != null)
            {
                resp  = (cmd.ExecuteScalar().ToString() == "") ? "0" : cmd.ExecuteScalar().ToString();
            }

            siguienteTicket = Convert.ToInt16(resp);
            return siguienteTicket;
            
          
        }


        static bool ValidarCorreoElectronico(string correoElectronico)
        {
            // Expresión regular para validar el formato del correo electrónico
            string patron = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            Regex regex = new Regex(patron);
            return regex.IsMatch(correoElectronico);
        }

        private void TBCC_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void TBCC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar == 32)
            {
                if (TBCC.Text != " ")
                {
                    TBCC.Text += "; ";
                    TBCC.Select(TBCC.Text.Length, 0);
                }
            }
        }

        private void TBResumen_TextChanged_1(object sender, EventArgs e)
        {
            if (TBResumen.Text == "")
            {
                SendMessage(TBPlasholderResumen.Handle, EM_SETCUEBANNER, 0, "Aqui escriba un breve resumen del incidente");
                TBPlasholderResumen.Visible = true;
            }
            else
            {
                TBPlasholderResumen.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nuevoIncidente = Interaction.InputBox("Si el incidente no figura en la lista por favor agreguelo.", "Nuevo incidente");

            //Int32 siguienteIncidente = 0;
            //string query = "SELECT * FROM incidente";
            //MySqlCommand cmd = new MySqlCommand(query, DB.conexBD);
            //MySqlDataReader readerIncidentes = cmd.ExecuteReader();           
            //cmd.Dispose();
            if (nuevoIncidente != "")

            {
                MySqlConnection conection = new MySqlConnection(DB.connectionStringAdmin);
                conection.Open();
                MySqlCommand cmdAddIncidente = new MySqlCommand("agregarIncidente", conection);
                //cmdAddIncidente.CommandText = "agregarIncidente";                
                //MySqlDataAdapter adapter = new MySqlDataAdapter("agregarIncidente", DB.conexBD);
                cmdAddIncidente.CommandType = CommandType.StoredProcedure;
               

                cmdAddIncidente.Parameters.AddWithValue("@nuevoincidente", nuevoIncidente);
                cmdAddIncidente.Parameters.AddWithValue("@elim", 0);

                cmdAddIncidente.ExecuteNonQuery();
                MessageBox.Show("El incidente fue agregado correctamente", "Nuevo incidente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarCBIncidentes();
                cmdAddIncidente.Dispose();
                conection.Close();

            }
           
            ////txSQL = "select * From conexiones Where conexionID
            //string txSQL = "INSERT INTO incidente(idIncidente, nombre, eliminado, Zona, Interfaz, " +
            //    " IdEquipo, IdIncidente, Responsable, Dni, Resumen, Remitente, Correo," +
            //    " Foto) " +
            //    "VALUES ( " + ticketNro +  ", '" +  fechaTicket + "', '" +  fechaincidente + "', '" +  cBLocIncid  + "', '" +
            //                tBLocalidadIncidente  + "', " +  idEquipo + ", '" +  idIncidente + "', '" +
            //                responsableIncidente + "', '" +  dniResponsable + "', '" +  resumen + "', '" +  Remitente + "', '" +
            //                correo + "', '')";

            ////preparamos la cadena pra insercion
            //MySqlCommand command = new MySqlCommand(txSQL, DB.conexBD);
            ////y la ejecutamos
            //command.ExecuteNonQuery();
            ////finalmente cerramos la conexion ya que solo debe servir para una sola orden
            //command.Dispose();



        }

        private void TBDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void TBCC_MouseMove(object sender, MouseEventArgs e)
        {
            

            toolTip1.Show("Si va a ingresar mas de un mail destino, presione la barra espaciadora al finalizar el ingreso de cada correo",TBCC, e.X, e.Y, 1500);
         //   toolTip1.Show("Si va a ingresar mas de un mail destino, presione la barra espaciadora al finalizar el ingreso de cada correo", TBCC, p.X, p.Y);
        }
    }
}
