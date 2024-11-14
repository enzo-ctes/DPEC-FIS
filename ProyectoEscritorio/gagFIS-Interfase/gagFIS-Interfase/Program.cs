using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Microsoft.SmartDevice.DeviceAgentTransport;
using PortableDeviceConnectApiLib;
using PortableDeviceTypesLib;



namespace gagFIS_Interfase {
    

    
    static class Program {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main() {

           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form0());

           

            //var devices = new PortableDeviceCollection();
            //devices.Refresh();
            //var Colectora = devices.First();
            //Colectora.Connect();

            //Colectora.TransferContentToDevice(@"C:\A_DPEC\_Pruebas\EmpresaLocal\201503\Envios_Cargas\201", @"Este equipo\MICC-4032\\\Datos DPEC");

            //Colectora.Disconnect();



        }

      
    }    
}
