using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PortableDevices
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var devices = new PortableDeviceCollection();
            devices.Refresh();
            var kindle = devices.First();
            kindle.Connect();

            kindle.TransferContentToDevice(@"C:\Install.log", @"G:\Programas");

            kindle.Disconnect();
        }
    }
}
