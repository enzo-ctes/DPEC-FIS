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
    public partial class FormLoading : Form
    {
        public FormLoading()
        {
            InitializeComponent();
        }

        public void FormLoading_Load(object sender, EventArgs e)
        {
            MiLoading.Location = new Point(this.Width / 2 - MiLoading.Width / 2, this.Height / 2 - MiLoading.Height / 2);
            LabCargando.Location = new Point(this.Width / 2 - LabCargando.Width / 2, this.Height / 2 + (LabCargando.Height * 2));

        }
        
    }
}
