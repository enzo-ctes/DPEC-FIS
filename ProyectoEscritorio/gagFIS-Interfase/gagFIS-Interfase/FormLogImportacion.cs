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
    public partial class LogImpApartados : Form
    {
       
        public LogImpApartados()
        {
            InitializeComponent();
        }

        private void DownloadYUpload_Load(object sender, EventArgs e)
        {
      
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            string InformesPDF = Vble.ValorarUnNombreRuta(Vble.CarpetaInformesPDF);
            InformesPDF += Vble.Periodo + "\\" + DateTime.Today.ToString("yyyyMMdd");

            fichero.FileName = "Porcion_" + Vble.PorcionImp + " APARTADOS.txt";
            string txtLogImportacion = "";

          
                if (LVDetalle.Items.Count > 0)
                {
                    txtLogImportacion += LabelPorcion.Text + " " + TxtPorcion.Text + "\n" +
                                         LabelTotalUsuarios.Text + " " + TxtTotalUsuarios.Text + "\n" +
                                         LabelTotalImportados.Text + " " + TxtImportados.Text + "\n" +
                                         LabelApartados.Text + " " + TxtApartados.Text + "\n" +
                                         "Instalaciones:\n";
                    foreach (ListViewItem tNd1 in LVDetalle.Items)
                    {

                        string[] Instalaciones = tNd1.Text.Split(';');
                        for (int i = 0; i < Instalaciones.Length; i++)
                        {
                            txtLogImportacion += $"              {tNd1.Text}\n";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No existen usuarios apartados para generar el txt de rutas.", "Usuarios apartados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (txtLogImportacion != "")
                {
                    Vble.CrearArchivoTXT(txtLogImportacion, "Porcion_" + Vble.PorcionImp + " APARTADOS.txt");
                }
           
        }
    }
}
