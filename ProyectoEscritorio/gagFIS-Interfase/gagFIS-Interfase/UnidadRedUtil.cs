using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static gagFIS_Interfase.Form1Inicio;

namespace gagFIS_Interfase
{
    class UnidadRedUtil
    {
        const int NO_ERROR = 0;

        const int CONNECT_UPDATE_PROFILE = 1;

        const int RESOURCETYPE_DISK = 1;



        // Constantes para NETRESOURCE

        const int RESOURCETYPE_PRINT = 2;

        const int RESOURCETYPE_ANY = 0;

        const int RESOURCE_CONNECTED = 1;

        const int RESOURCE_REMEMBERED = 3;

        const int RESOURCE_GLOBALNET = 2;

        const int RESOURCEDISPLAYTYPE_DOMAIN = 1;

        const int RESOURCEDISPLAYTYPE_GENERIC = 0;

        const int RESOURCEDISPLAYTYPE_SERVER = 2;

        const int RESOURCEDISPLAYTYPE_SHARE = 3;

        const int RESOURCEUSAGE_CONNECTABLE = 1;

        const int RESOURCEUSAGE_CONTAINER = 2;



        // Códigos de error:

        const int ERROR_ACCESS_DENIED = 5;

        const int ERROR_ALREADY_ASSIGNED = 85;

        const int ERROR_BAD_DEV_TYPE = 66;

        const int ERROR_BAD_DEVICE = 1200;

        const int ERROR_BAD_NET_NAME = 67;

        const int ERROR_BAD_PROFILE = 1206;

        const int ERROR_BAD_PROVIDER = 1204;

        const int ERROR_BUSY = 170;

        const int ERROR_CANCELLED = 1223;

        const int ERROR_CANNOT_OPEN_PROFILE = 1205;

        const int ERROR_DEVICE_ALREADY_REMEMBERED = 1202;

        const int ERROR_EXTENDED_ERROR = 1208;

        const int ERROR_INVALID_PASSWORD = 86;

        const int ERROR_NO_NET_OR_BAD_PATH = 1203;



        //Importar las API de Windows ...
        [DllImport("mpr.dll",

       EntryPoint = "WNetAddConnection2W",

       CharSet = CharSet.Unicode)]

        private static extern int

AddConnection(

        ref NETRESOURCE lpNetResource,

        [MarshalAs(UnmanagedType.LPWStr)]

        string Password,

        [MarshalAs(UnmanagedType.LPWStr)]

        string lpUserName,

        [MarshalAs(UnmanagedType.I4 )]

        int dwFlags);



        [DllImport("mpr.dll",

               EntryPoint = "WNetCancelConnection2W",

               CharSet = CharSet.Unicode)]

        private static extern int

        CancelConnection(

                [MarshalAs(UnmanagedType.LPWStr)]

        string lpName,

                [MarshalAs(UnmanagedType.I4)]

        int dwFlags,

                [MarshalAs(UnmanagedType.Bool)]

        bool fForce);


        //Codigo para añadir la unidad de red ...
        public void MapResource(string driveLetter, string path)

        {

            long result;

            NETRESOURCE netResource = new NETRESOURCE();

            netResource.dwScope = RESOURCE_GLOBALNET;

            netResource.dwType = RESOURCETYPE_DISK;

            netResource.dwDisplayType = RESOURCEDISPLAYTYPE_SHARE;

            netResource.dwUsage = RESOURCEUSAGE_CONNECTABLE;





            netResource.lpLocalName = driveLetter; // por ejemplo: "F:"

            netResource.lpRemoteName = path; /* por ejemplo:

					    "\\ServerName\ShareName" */



            // en este caso lo he puesto aqui, pero mejor deberiamos 

            // parametrizar estos datos!

            //string user = "admin";
            string user = Vble.UserAdmin();
            string pwd = Vble.PassAdmin();


            //string pwd = "Micc4001";



            result = AddConnection(ref netResource,

                           pwd, user, CONNECT_UPDATE_PROFILE);

            if (result != NO_ERROR)

            {

                // Sacar un mensaje mas amigable ya es tarea vuestra :-)

                throw new ApplicationException(

                String.Format(

                 "No se ha podido conectar, codigo de error {0}", result));

            }

        }


        //Codigo para desconectar la unidad de red ...
        public void UnMapResource(string driverLetter)

        {

            long result = NO_ERROR;

            result = CancelConnection(driverLetter,

                          CONNECT_UPDATE_PROFILE, true);

            if (result != NO_ERROR)

            {

                // Sacar un mensaje mas amigable ya es tarea vuestra :-)

                throw new ApplicationException(

                String.Format(

                 "No se ha podido desconectar, codigo de error {0}", result));

            }

        }

    }
}
