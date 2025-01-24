using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Org.BouncyCastle.Crypto.Macs;

namespace gagFIS_Interfase
{
    public partial class FormMaps : Form
    {

        public string latitud { get; set; }
        public string longitud { get; set; }

        public  string domicilioCD { get; set; }

        public FormMaps()
        {
            InitializeComponent();        
        }

        public void FormMaps_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.CacheOnly;
            // Inicializa el control de mapa
            gMap = new GMap.NET.WindowsForms.GMapControl
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(gMap);

            // Configura el mapa
            gMap.MapProvider = GMapProviders.GoogleMap; // Proveedor de mapas (Google, OpenStreetMap, etc.)
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMap.SetPositionByKeywords("Buenos Aires, Argentina");
            gMap.MinZoom = 2;
            gMap.MaxZoom = 18;
            gMap.Zoom = 10;

            // Centra el mapa en la posición especificada
            double lat = Convert.ToDouble(latitud.Replace(".", ","), CultureInfo.InvariantCulture);
            double lng = Convert.ToDouble(longitud.Replace(".", ","), CultureInfo.InvariantCulture);

            // Agrega un marcador
            AddMarker(lat, lng, domicilioCD);
        }

        private void AddMarker(double lat, double lng, string label)
        {
            var point = new PointLatLng(lat, lng);
            var markersOverlay = new GMapOverlay("markers");
            var marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot)
            {
                ToolTipText = label
            };
            markersOverlay.Markers.Add(marker);
            gMap.Overlays.Add(markersOverlay);
        }
    }
}
