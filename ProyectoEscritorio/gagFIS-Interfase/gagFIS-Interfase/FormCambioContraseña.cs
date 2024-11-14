using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gagFIS_Interfase
{
    public partial class FormCambioContraseña : Form
    {
        public FormCambioContraseña()
        {
            InitializeComponent();
        }

        private void iTalk_Button_12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iTalk_Label4_Click(object sender, EventArgs e)
        {

        }

        private void iTalk_Button_11_Click(object sender, EventArgs e)
        {
            try
            {
                if (true)
                {

                }
                string Consulta = "SET PASSWORD FOR '" + DB.sDbUsu + "'@'" + Vble.ServidorBD+ "' = " + TextBoxConfirmaContr.Text;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
