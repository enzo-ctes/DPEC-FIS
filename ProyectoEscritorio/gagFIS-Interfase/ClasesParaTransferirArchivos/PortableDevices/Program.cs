using System;
using System.Linq;
using System.Windows.Forms;

namespace PortableDevices
{
    class Program
    {
        [STAThread]
        static void Main()
        {             

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        

    }

    
        public static void DisplayResourceContents(PortableDeviceObject portableDeviceObject)
        {
            Console.WriteLine(portableDeviceObject.Name);
            if (portableDeviceObject is PortableDeviceFolder)
            {
                DisplayFolderContents((PortableDeviceFolder) portableDeviceObject);
            }
        }

        public static void DisplayFolderContents(PortableDeviceFolder folder)
        {
            foreach (var item in folder.Files)
            {
                Console.WriteLine(item.Id);

                if (item is PortableDeviceFolder)
                {
                    DisplayFolderContents((PortableDeviceFolder) item);
                }
            }
        }
    }
}
