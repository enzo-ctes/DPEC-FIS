using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GongSolutions.Shell;
using System.Diagnostics;
using System.IO;

namespace gagFIS_Interfase
{
    public partial class Form6Informes : Form
    {
        public Form6Informes()
        {
            InitializeComponent();
        }

        private void shellView1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void Form6Informes_Load(object sender, EventArgs e)
        {
           
            //Vble.pathInformes = new ShellItem(Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas));
            if (!Directory.Exists(Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas)))
            {
                Directory.CreateDirectory(Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas));
                ShellItem path = new ShellItem(Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas));
                shellViewInformes.CurrentFolder = path;
                shellViewInformes.ShowWebView = false;
            }
            else
            {
                Vble.pathInformes = new ShellItem(Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas));
                //MessageBox.Show(Vble.pathInformes.ParsingName);
                shellViewInformes.CurrentFolder = Vble.pathInformes;
                //MessageBox.Show("no existe el directorio de informes");
            }
            
        }

        private void BtonAtras_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(Vble.Periodo.ToString());
                ShellItem path = new ShellItem(Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas));
            
                if (shellViewInformes.CurrentFolder != path)
                {
                    shellViewInformes.NavigateParent();//navega un nodo hacia atras en la raiz
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(r.Message);
            }
        }

        private void shellViewInformes_DoubleClick(object sender, EventArgs e)
        {

            //MessageBox.Show(shellViewInformes.SelectedItems[0].ToString());
            Process.Start(shellViewInformes.SelectedItems[0].ToString());
            //Process.Start();
        }
    }
}
