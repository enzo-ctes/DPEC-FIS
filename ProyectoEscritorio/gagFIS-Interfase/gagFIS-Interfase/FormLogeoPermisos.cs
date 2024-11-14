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

namespace gagFIS_Interfase
{

   

    public partial class FormLogeoPermisos : Form
    {
        public static bool banderaPermisoBorrado { get; set; }

        public  FormLogeoPermisos()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
          
        }

        private async void FormLogeoPermisos_Load(object sender, EventArgs e)
        {
            await VerificarPermiso();
        }


        public async Task<bool> VerificarPermiso()
        {
            bool dev = false;
            MySqlCommand cmd = new MySqlCommand();
            //Lee la tabla conexiones del periodo y sin leer
            string txSQL = "SELECT Count(*)" +
                    " FROM Lecturistas" +
                    " WHERE Codigo = " + txtUsuario.Text + " AND Clave = '" + txtKey.Text +"'";

            cmd = new MySqlCommand(txSQL, DB.conexBD);
            var count = await cmd.ExecuteScalarAsync();
            ///Tiempo que tardara en esperar la respuesta desde la base de datos
            cmd.Dispose();

            if ((Int32)count==1)
            {
                if (banderaPermisoBorrado)
                {
                    Vble.dniBorrado = txtUsuario.Text;
                    Vble.passBorrado = txtKey.Text;
                    dev =true;
                    return dev;
                }
                else
                {
                    FormEstadosRutas estados = new FormEstadosRutas();
                }

            }
            else
            {
                MessageBox.Show("No tiene privilegios para borrar lo que contiene la colectora");

            }

            return dev;
         
        }

        private void grpUsu_Enter(object sender, EventArgs e)
        {

        }
    }
}
