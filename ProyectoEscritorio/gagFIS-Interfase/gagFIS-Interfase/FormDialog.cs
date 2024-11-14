using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gagFIS_Interfase {
    public partial class FormDialog : Form {
        int desde = 0;
        int hasta = 0;
        int cant = 0;
        int desdeV, hastaV, cantV;  //Valores 'viejos'
        int i;
           
        public FormDialog(int Ruta, string Particion, int Desde, int Hasta, int Cantidad) {
            InitializeComponent();
            desdeV = Desde;
            hastaV = Hasta;
            cantV = Cantidad;
            txtDesdeV.Text = desdeV.ToString().Trim();
            txtHastaV.Text = hastaV.ToString().Trim();
            txtCantV.Text = cantV.ToString().Trim();
            lbRuta.Text = "Ruta: " + Ruta.ToString() + "   Partición: " + Particion;

            lbAyuda.Text = "Solo se consideran campos con datos:" +
                "\n1) Si se cargan los tres datos, inicia en 'Desde' termina en 'Hasta' " +
                "o una vez que alcance la cantidad indicada" +
                "\n2) Si NO se carga 'Desde' se busca desde el inicio." +
                "\n3) Si NO se carga 'Hasta' se busca hasta el final." +
                "\n4) Si NO se carga 'Cantidad' busca todo dentro de los límites" +
                "\n5) Si se carga 'Cantidad' y uno de los límites busca esa cantidad " +
                "más próxima al límite indicado";
                

        }

        /// <summary>
        /// Obtiene el número de secuencia para el extremo inferior de la partición,
        /// devuelve el valor ingresado en el cuadro de texto Desde (Nuevo).
        /// </summary>
        public int PartDesde {
            get {
                if(int.TryParse(txtDesde.Text, out i))
                    return i;
                else return desde;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Obtiene el número de secuencia para el extremo superior de la partición,
        /// devuelve el valor ingresado en el cuadro de texto Hasta (Nuevo).
        /// </summary>
        public int PartHasta {
            get {
                if(int.TryParse(txtHasta.Text, out i))
                    return i;
                else return hasta;
            }
        }

        /// <summary>
        /// Obtiene la cantidad de unidades a incluir en la partición,
        /// devuelve el valor ingresado en el cuadro de texto Cantidad (Nuevo).
        /// </summary>
        public int PartCantidad {
            get {
                if(int.TryParse(txtCant.Text, out i))
                    return i;
                else return cant;
            }
        }



    }
}
