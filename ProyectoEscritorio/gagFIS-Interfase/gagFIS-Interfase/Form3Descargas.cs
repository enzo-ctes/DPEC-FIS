/*
 * Creado por SharpDevelop.
 * Usuario: Gerardo
 * Fecha: 01/05/2015
 * Hora: 13:57
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Management;
using Microsoft.SmartDevice.DeviceAgentTransport;
using WindowsPortableDevicesLib.Domain;
using WindowsPortableDevicesLib;
using GongSolutions.Shell;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualBasic.Devices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Net;

//using System.Devices;

namespace gagFIS_Interfase
{
    /// <summary>
    /// Description of Form2.
    /// </summary>
    /// 

    public partial class Form3Descargas : Form
    {

        //private DriveDetector driveDetector = null;

        StandardWindowsPortableDeviceService service = new StandardWindowsPortableDeviceService();
        public static PortableDeviceFolder currentFolder = null;
        public TreeNode nodoraiz = new TreeNode();
        public delegate void InvokeDelegate();
        public SQLiteConnection BaseADescargar = new SQLiteConnection();
        public int AvanceDescarga = 0;
        public int CantCFDistinct = 0;
        bool Descargado = false;
        bool copiado = false;
        public int CantConex = 0;
        public int CantCFact = 0;
        public int CantImpresor = 0;
        public int CantLogErr = 0;
        public int TotalRegistros = 0;
        public int CantRegistros = 0;

        public Form3Descargas()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //CrearArchivoInfoDescarga
            InitializeComponent();
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void Form3_Load(object sender, System.EventArgs e)
        {
            //this.MaximizeBox = false;
            //this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            toolTip1.SetToolTip(btnDescargar, "Realiza la descarga de las conexiones de la Colectora a la PC ");
            toolTipTodos.SetToolTip(btnResumTodo, "Muestra resumenes de descarga pertenecientes al PERIODO en el que se esta trabajando");
            toolTipDia.SetToolTip(btnResDia, "Muestra resumenes de descarga pertenecientes a la FECHA en el que se esta trabajando");
            //toolTip3.SetToolTip(botExpulsaColec, "Expulsar Colectora de Datos");

            //----------------------------------------------------------------

            Funciones.LeerArchivoZonaFIS();
            //CargarCarpetas();
            ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
            shellViewDescargas.CurrentFolder = folder;
            //timer1.Enabled = true;
            //timer1_Tick(sender, e);
            //timer1.Start();
            cargarCBColeWifi();
            cargarCBLocZona();
            var ver = DB.conexBD;

            if (DB.sDbUsu == Vble.UserAdmin())
            {

                this.BotComenDesAdmin.Visible = true;
                this.BotCargaBase1.Visible = true;
                this.BotCargaBase2.Visible = true;
                this.BotCargaArchiInfo.Visible = true;
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
                this.textBox3.Visible = true;

            }
        }

        /// <summary>
        /// Este metodo lee el archivo de configuracion GAG_MoverDatos.ini y busca las colectoras asignadas al centro de interfaz para que solo muestre las que le corresponde
        /// </summary>
        private void cargarCBColeWifi()
        {
            StringBuilder stb = new StringBuilder(500);
            Inis.GetPrivateProfileString("Colectoras", Vble.locCentroInterfaz, "", stb, 500, Ctte.ArchivoIniName);
            Vble.colectorasCentroInterfaz = stb.ToString().Trim();
            List<string> ListaColectoras = new List<string>();

            string[] colectoras = Vble.colectorasCentroInterfaz.Split(',');
            cmbDevicesWifi.Items.Clear();
            ListaColectoras.Clear();

            foreach (var item in colectoras)
            {
                //cmbDevicesWifi.Items.Add(item.ToString());
                ListaColectoras.Add(item.ToString());
            }

            ///Ordeno la lista de forma ascendente y luego agrego al combobox
            ListaColectoras.Sort();

            foreach (var item in ListaColectoras)
            {
                cmbDevicesWifi.Items.Add(item);
            }

        }
        
       
        /// <summary>
        ///  Este metodo lee el archivo de configuracion GAG_MoverDatos.ini y busca las localidades asociadas al centro de interfaz para que a la hora de realizar
        ///  las descargas solo muestre el que corresponde a la Zona.
        /// </summary>
        private void cargarCBLocZona()
        {
         
            List<string> ListaLocalidades = new List<string>();
            string[] localidades = Vble.LocAsociadas.Split(',');
            CBZona.Items.Clear();
            ListaLocalidades.Clear();

            foreach (var item in localidades)
            {
                //cmbDevicesWifi.Items.Add(item.ToString());
                ListaLocalidades.Add(item.ToString().Trim(' '));
            }

            ///Ordeno la lista de forma ascendente y luego agrego al combobox
            ListaLocalidades.Sort();

            foreach (var item in ListaLocalidades)
            {
                CBZona.Items.Add(item);
            }

            CBZona.Text = Vble.centroInterfaz.Substring(Vble.centroInterfaz.IndexOf(":")+1);

        }

        //abre cuadro de seleccion de archivos si se opto por
        //elegir carga de archivos a colectoras o viceversa al abrir el form
        public void mensaje()
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                System.IO.StreamReader(openFileDialog1.FileName);
                MessageBox.Show(sr.ReadToEnd());
                sr.Close();
            }
        }
        
        private void Form3_Resize(object sender, System.EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Carga la lista de carpetas con las existentes en el archivo
        /// ini de configuración
        /// </summary>
        private void CargarCarpetas()
        {
            int i = 0;
            string sC, sK, sI, localidad;
            StringBuilder stB = new StringBuilder();
            lsCarpetas.Items.Clear();

            do
            {
                i++;
                sC = "Carpeta" + i.ToString("00");
                Inis.GetPrivateProfileString
                    ("Carpetas Descargas", sC, "NO", stB, 250, Ctte.ArchivoIniName);
                sI = stB.ToString().Trim();
                sK = sI.ToUpper();
                localidad = Vble.ValorarUnNombreRuta(sK);
                if ((sI.Length > 0) && (sK != "NO"))
                {
                    if (!lsCarpetas.Items.ContainsKey(sK))
                        //lsCarpetas.Items.Add(sK, sI, 0);
                        lsCarpetas.Items.Add(localidad);
                    //lsCarpetas.Items.Add(localidad, sI, 0);
                }
            } while (stB.ToString().Trim().ToUpper() != "NO");


        }

        /// <summary>
        /// Guarda en el archivo ini de configuración, las carpetas que están 
        /// cargadas en la lista de carpetas de descargas, en el orden en
        /// que están en la lista
        /// </summary>
        private void GuardarCarpetas()
        {
            int i;
            string sC;
            //Antes de guardar las carpetas, limpa la seccion en el archivo ini
            BorraCarpetasEnIni();

            for (i = 0; i < lsCarpetas.Items.Count; i++)
            {
                sC = "Carpeta" + (i + 1).ToString("00");
                Inis.WritePrivateProfileString("Carpetas Descargas", sC,
                    lsCarpetas.Items[i].Text.Trim(), Ctte.ArchivoIniName);
            }

        }

        /// <summary>
        /// Borra todas las carpetas cargadas en el ini, para evitar posibilidad
        /// de que queden elementos repetidos al actualizar.
        /// </summary>
        private void BorraCarpetasEnIni()
        {
            int i = 0;
            string sC;
            StringBuilder stB = new StringBuilder();

            do
            {
                i++;
                sC = "Carpeta" + i.ToString("00");
                Inis.GetPrivateProfileString(
                   "Carpetas Descargas", sC, "NO", stB, 250, Ctte.ArchivoIniName);
                if (stB.ToString().Trim().ToUpper() != "NO")
                    //Si existe key, limpia el valor
                    Inis.WritePrivateProfileString(
                        "Carpetas Descargas", sC, "", Ctte.ArchivoIniName);
            } while (stB.ToString().Trim().ToUpper() != "NO");

        }

        /// <summary>
        /// Lee de la tabla "descargas" aquellas que pertenecen al periodo/fecha que
        /// se pasa como parametro segun lo que se solicita ver, ya sea las descargas 
        /// correspondientes al periodo o a la fecha actual
        /// 
        /// </summary>
        private void LeerInformes(string periodo, int colum, string Tiempo)
        {
            tvInformes.Nodes.Clear();
            DataTable Tabla = new DataTable();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string rutaInformes = Vble.CarpetaTrabajo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas);
            DirectoryInfo RutaDescargas = new DirectoryInfo(rutaInformes);
            int cantidad = 0;
            int colectoras = 0, totimpresas = 0, totLeidasNOimpr = 0, totImposLeer = 0, totNOleidas = 0, totLeidasNoImprOtros = 0;
            ;
            try
            {
                txSQL = "SELECT * FROM descargas";
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);
                string acomparar;

                if (Tabla.Rows.Count > 0)
                {
                    foreach (DataRow fi in Tabla.Rows)
                    {

                        if (colum == 2)
                        {
                            acomparar = Convert.ToDateTime(fi["FechaDescarga"]).ToString("dd-MM-yyyy").ToString();
                        }
                        else
                        {
                            acomparar = fi["FechaDescarga"].ToString();
                        }
                        if (acomparar == periodo.ToString())
                        {
                            colectoras = colectoras + 1;  //acumulador total Colectoras descargadas                     
                            TreeNode nodoraiz = tvInformes.Nodes.Add(fi["Carpeta"].ToString());//3
                            nodoraiz.Nodes.Add("Equipo: " + fi["Dispositivo"].ToString());//4
                            //nodoraiz.Nodes.Add("Equipo: " + cmbDevices.Text);
                            nodoraiz.Nodes.Add("Lecturista: " + fi["Lecturista"].ToString());//5
                            string infoconexion = "Informacion de Conexiones: ";
                            nodoraiz.Nodes.Add(infoconexion);
                            nodoraiz.LastNode.Nodes.Add(fi["InfConexiones"].ToString());//6
                            string cantidadConexiones = "Cantidad de Conexiones: " + fi["CantidadConex"].ToString();//7
                            cantidad = (cantidad + Convert.ToInt32(fi["CantidadConex"]));//acumulador total conexiones
                            nodoraiz.Nodes.Add(cantidadConexiones);
                            nodoraiz.LastNode.Nodes.Add("No Leidos: " + fi["Noleidas"].ToString());//8
                            totNOleidas = (totNOleidas + Convert.ToInt32(fi["Noleidas"]));//acumulador total No Leidas
                            nodoraiz.LastNode.Nodes.Add("Leidas NO Impresas: " + fi["LeidasNoImpresas"].ToString());//9
                            totLeidasNOimpr = (totLeidasNOimpr + Convert.ToInt32(fi["LeidasNoImpresas"]));//acumulador total leidas no impresas
                            nodoraiz.LastNode.Nodes.Add("Leidas Impresas: " + fi["LeidasImpresas"].ToString());//10
                            totimpresas = (totimpresas + Convert.ToInt32(fi["LeidasImpresas"]));//acumulador total leidas Impresas
                            nodoraiz.LastNode.Nodes.Add("NO Impresas Impresora Desconectada: " + fi["NoImpresasImpDesconec"].ToString());//11
                            nodoraiz.LastNode.Nodes.Add("NO Impresas Fuera de Rango: " + fi["NoImpresasFueraDeRango"].ToString());//12
                            //nodoraiz.LastNode.Nodes.Add("NO Impresas Estado Negativo: " + fi["NoImpresasEstadoNeg"].ToString());//13
                            nodoraiz.LastNode.Nodes.Add("NO Impresas Error Dato: " + fi["NoImpresasErrorDato"].ToString());//14
                            nodoraiz.LastNode.Nodes.Add("NO Impresas Domicilio Postal: " + fi["NoImpresasDomPostal"].ToString());//15
                            nodoraiz.LastNode.Nodes.Add("NO Impresas Indicado Dato: " + fi["NoImpresasIndicDato"].ToString());//16
                            nodoraiz.LastNode.Nodes.Add("Imposible Leer: " + fi["ImposibleLeer"].ToString());//17
                            totImposLeer = (totImposLeer + Convert.ToInt32(fi["ImposibleLeer"]));//acumulador total Imposibles leer
                            //nodoraiz.LastNode.Nodes.Add("Sub Total Negativo: " + fi["SubTotalNeg"].ToString());//18
                            nodoraiz.LastNode.Nodes.Add("Error al Archivar Datos: " + fi["ErrorArchivarDatos"].ToString());//19
                            //nodoraiz.LastNode.Nodes.Add("Error en Nº de Factura: " + fi["ErroNumFactura"].ToString());//20
                            //nodoraiz.LastNode.Nodes.Add("Sin Conceptos a Facturar: " + fi["SinConceptosFacturar"].ToString());//21
                            nodoraiz.LastNode.Nodes.Add("Falta Titular: " + fi["FaltaTitular"].ToString());//22
                            nodoraiz.LastNode.Nodes.Add("Error al Facturar: " + fi["ErrorAlFacturar"].ToString());//23
                            nodoraiz.LastNode.Nodes.Add("Error en Memoria: " + fi["ErrorEnMemoria"].ToString());//24
                            nodoraiz.LastNode.Nodes.Add("Periodo Excedido en Días: " + fi["PeriodoExcEnDias"].ToString());//25
                            nodoraiz.LastNode.Nodes.Add("Error por otros Motivos:" + fi["ErrorIndeterminado"].ToString());//26

                            totLeidasNoImprOtros = (totLeidasNoImprOtros + (Convert.ToInt32(fi["NoImpresasImpDesconec"]) + Convert.ToInt32(fi["NoImpresasFueraDeRango"]) + Convert.ToInt32(fi["NoImpresasEstadoNeg"]) + Convert.ToInt32(fi["NoImpresasErrorDato"]) +
                                                   Convert.ToInt32(fi["NoImpresasDomPostal"]) + Convert.ToInt32(fi["NoImpresasIndicDato"]) + Convert.ToInt32(fi["SubTotalNeg"]) + Convert.ToInt32(fi["ErrorArchivarDatos"]) +
                                                   Convert.ToInt32(fi["ErroNumFactura"]) + Convert.ToInt32(fi["SinConceptosFacturar"]) + Convert.ToInt32(fi["FaltaTitular"]) + Convert.ToInt32(fi["ErrorAlFacturar"]) + Convert.ToInt32(fi["ErrorEnMemoria"]) +
                                                   Convert.ToInt32(fi["PeriodoExcEnDias"]) + Convert.ToInt32(fi["ErrorIndeterminado"])));

                            tvInformes.Nodes.Add("--------------------------------------------------------------------------------");
                        }
                    }
                }
                fraDescargas.Text = "Descargas " + Tiempo;
                lbColDesc.Text = "Total Colectoras Descargadas: " + colectoras;
                lbDescTotal.Text = "Total Conexiones Descargadas:   " + cantidad;
                lbTotImp.Text = "Total Impresas:   " + totimpresas;
                lbTotLeiNoImp.Text = "Total Leidas No Impresas: " + totLeidasNOimpr;
                lbTotImposLeer.Text = "Total Imposibles Leer: " + totImposLeer;
                lbNOLeidas.Text = "Total NO Leidas: " + totNOleidas;
                lbLeiNOimpOtros.Text = "Total Leidas No Impresas (otros motivos): " + totLeidasNoImprOtros;
                datosAdapter.Dispose();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        /// <summary>
        /// Agrega el encabezado del Informe 
        /// </summary>
        /// <param name="table"></param>
        public void AgregarEncabezado(PdfPTable table)
        {

            PdfPCell descarga = (new PdfPCell(new Paragraph("\nDescarga", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD))) { Rowspan = 2 });
            descarga.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(descarga);
            PdfPCell infoconexs = (new PdfPCell(new Paragraph("\nInfo. de Conexiones.", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD))) { Rowspan = 2 });
            infoconexs.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            infoconexs.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(infoconexs);
            PdfPCell cantconex = (new PdfPCell(new Paragraph("\nCant. Conex", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Rowspan = 2 });
            cantconex.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(cantconex);
            PdfPCell Impresas = (new PdfPCell(new Paragraph("\nImpre-sas", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Rowspan = 2 });
            Impresas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Impresas);
            PdfPCell NoLeidas = (new PdfPCell(new Phrase("NO LEIDAS ", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD))) { Colspan = 2 });
            NoLeidas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(NoLeidas);
            PdfPCell LeidasNoImpresas = (new PdfPCell(new Phrase("LEIDAS NO IMPRESAS ", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD))) { Colspan = 17 });
            LeidasNoImpresas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(LeidasNoImpresas);
            PdfPCell SinLeer = (new PdfPCell(new Paragraph("\nSin Leer", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            SinLeer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(SinLeer);
            PdfPCell ImposibleLeer = (new PdfPCell(new Paragraph("\nImposible Leer", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ImposibleLeer.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ImposibleLeer);
            PdfPCell Total = (new PdfPCell(new Paragraph("\nTotal", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            Total.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Total);
            PdfPCell Leidas = (new PdfPCell(new Paragraph("\nLeidas", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            Leidas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(Leidas);
            PdfPCell ImprDesc = (new PdfPCell(new Paragraph("\nImpre. Desc.", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ImprDesc.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ImprDesc);
            PdfPCell FueraRango = (new PdfPCell(new Paragraph("\nFuera Rango", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            FueraRango.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(FueraRango);
            //PdfPCell EstNeg = (new PdfPCell(new Paragraph("\nEst. Neg.", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            //EstNeg.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(EstNeg);
            PdfPCell ErrorDato = (new PdfPCell(new Paragraph("\nError Dato", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ErrorDato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ErrorDato);
            PdfPCell DomPostal = (new PdfPCell(new Paragraph("\nDom. Postal", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            DomPostal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(DomPostal);
            PdfPCell IndDato = (new PdfPCell(new Paragraph("\nIndicado. Dato", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            IndDato.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(IndDato);
            //PdfPCell SubTotalNeg = (new PdfPCell(new Paragraph("Sub Total Neg.", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            //SubTotalNeg.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(SubTotalNeg);
            PdfPCell ErrorArchDatos = (new PdfPCell(new Paragraph("Error Arch. Datos", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ErrorArchDatos.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ErrorArchDatos);
            //PdfPCell ErrorNºFactura = (new PdfPCell(new Paragraph("Error Nº de Factu-ra", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            //ErrorNºFactura.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(ErrorNºFactura);
            //PdfPCell SinCpto = (new PdfPCell(new Paragraph("Sin Cpto", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            //SinCpto.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(SinCpto);
            PdfPCell FaltaTitular = (new PdfPCell(new Paragraph("S/Titu-lar", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            FaltaTitular.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(FaltaTitular);
            PdfPCell ErroralFacturar = (new PdfPCell(new Paragraph("Error al Factu-rar", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ErroralFacturar.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ErroralFacturar);
            PdfPCell ErrorEnMemoria = (new PdfPCell(new Paragraph("Error en Memoria", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ErrorEnMemoria.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ErrorEnMemoria);
            PdfPCell PerExcDías = (new PdfPCell(new Paragraph("Per. Exc. en Días", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            PerExcDías.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(PerExcDías);
            PdfPCell ErrorIndeterminado = (new PdfPCell(new Paragraph("Error Indet", FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))) { Colspan = 1 });
            ErrorIndeterminado.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(ErrorIndeterminado);

        }

        /// <summary>
        /// Agrega datos de la tabla descarga a la tabla del PDF generado anteriormente
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="table"></param>
        /// <param name="TotalGeneral"></param>
        public void AgregaDatosdeTabla(DataRow fi, PdfPTable table, int TotalGeneral)
        {
            TotalGeneral = TotalGeneral + Convert.ToInt32(fi["CantidadConex"]);
            //Agrega los datos de cada descarga a sus columnas correspondientes                                   
            PdfPCell fi3 = (new PdfPCell(new Paragraph(fi["Carpeta"].ToString(), FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL))));
            fi3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi3);
            PdfPCell fi6 = (new PdfPCell(new Paragraph(fi["InfConexiones"].ToString(), FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL))));
            fi6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi6);
            PdfPCell fi7 = (new PdfPCell(new Paragraph(fi["CantidadConex"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi7);
            PdfPCell fi10 = (new PdfPCell(new Paragraph(fi["LeidasImpresas"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi10.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi10);
            PdfPCell fi8 = (new PdfPCell(new Paragraph(fi["Noleidas"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi8);
            PdfPCell fi17 = (new PdfPCell(new Paragraph(fi["ImposibleLeer"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi17.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi17);
            //columna contador de no impresas
            int TotalLeidasNoImpresas = (Convert.ToInt32(fi["LeidasNoImpresas"]) + Convert.ToInt32(fi["NoImpresasImpDesconec"]) + Convert.ToInt32(fi["NoImpresasFueraDeRango"]) /*+ Convert.ToInt32(fi["NoImpresasEstadoNeg"])*/ +
                                  Convert.ToInt32(fi["NoImpresasErrorDato"]) + Convert.ToInt32(fi["NoImpresasDomPostal"]) + Convert.ToInt32(fi["NoImpresasIndicDato"]) /*+ Convert.ToInt32(fi["SubTotalNeg"])*/ +
                                  Convert.ToInt32(fi["ErrorArchivarDatos"]) /*+ Convert.ToInt32(fi["ErroNumFactura"])+ Convert.ToInt32(fi["SinConceptosFacturar"])*/  + Convert.ToInt32(fi["FaltaTitular"]) +
                                 Convert.ToInt32(fi["ErrorAlFacturar"]) + Convert.ToInt32(fi["ErrorEnMemoria"]) + Convert.ToInt32(fi["PeriodoExcEnDias"]) + Convert.ToInt32(fi["ErrorIndeterminado"]));

            PdfPCell TotalNoImp = (new PdfPCell(new Paragraph(TotalLeidasNoImpresas.ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            TotalNoImp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(TotalNoImp);
            PdfPCell fi9 = (new PdfPCell(new Paragraph(fi["LeidasNoImpresas"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi9);
            PdfPCell fi11 = (new PdfPCell(new Paragraph(fi["NoImpresasImpDesconec"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi11.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi11);
            PdfPCell fi12 = (new PdfPCell(new Paragraph(fi["NoImpresasFueraDeRango"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi12.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi12);
            //PdfPCell fi13 = (new PdfPCell(new Paragraph(fi["NoImpresasEstadoNeg"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            //fi13.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(fi13);
            PdfPCell fi14 = (new PdfPCell(new Paragraph(fi["NoImpresasErrorDato"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi14.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi14);
            PdfPCell fi15 = (new PdfPCell(new Paragraph(fi["NoImpresasDomPostal"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi15.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi15);
            PdfPCell fi16 = (new PdfPCell(new Paragraph(fi["NoImpresasIndicDato"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi16.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi16);
            //PdfPCell fi18 = (new PdfPCell(new Paragraph(fi["SubTotalNeg"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            //fi18.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(fi18);
            PdfPCell fi19 = (new PdfPCell(new Paragraph(fi["ErrorArchivarDatos"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi19.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi19);
            //PdfPCell fi20 = (new PdfPCell(new Paragraph(fi["ErroNumFactura"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            //fi20.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(fi20);
            //PdfPCell fi21 = (new PdfPCell(new Paragraph(fi["SinConceptosFacturar"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            //fi21.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //table.AddCell(fi21);
            PdfPCell fi22 = (new PdfPCell(new Paragraph(fi["FaltaTitular"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi22.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi22);
            PdfPCell fi23 = (new PdfPCell(new Paragraph(fi["ErrorAlFacturar"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi23.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi23);
            PdfPCell fi24 = (new PdfPCell(new Paragraph(fi["ErrorEnMemoria"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi24.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi24);
            PdfPCell fi25 = (new PdfPCell(new Paragraph(fi["PeriodoExcEnDias"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi25.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi25);
            PdfPCell fi26 = (new PdfPCell(new Paragraph(fi["ErrorIndeterminado"].ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL))));
            fi26.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(fi26);
        }

        /// <summary>
        /// Genera un documento .pdf con el informe correspondiente al periodo/fecha que se esta trabajando
        /// segun los parametros que se reciben.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tipiinform"></param>
        /// /// <param name="column"></param>
        private void GenerarInformeDescarga(string periodo, string tipinforme, int colum, string NombreInforme, bool creainforme)
        {
            //tvInformes.Nodes.Clear();
            DataTable Tabla = new DataTable();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string rutaInformes = Vble.CarpetaTrabajo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaInformesDescargas);
            DirectoryInfo RutaDescargas = new DirectoryInfo(rutaInformes);
            int NumPag = 0;
            Paragraph TextoNumPag = new Paragraph();
            int cantidad = 0, colectoras = 0, totimpresas = 0, totLeidasNOimpr = 0, totImposLeer = 0, totNOleidas = 0, totLeidasNoImprOtros = 0, TotalNOIMPRESAS = 0, CantXpagina = 1, Pag = 1;
            string acomparar;
            try
            {
                //Consulta las descargas de la tabla "Descargas"
                txSQL = "SELECT * FROM descargas WHERE Periodo = " + periodo;
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                //Variable que contendrá el total de Conexiones descargadas pertenecientes
                //al periodo en el que se esta trabajando
                int TotalGeneral = 0;
                string PathInformes = Vble.CarpetaTrabajo + Vble.ValorarUnNombreRuta(Vble.CarpetaInformes);
                DateTime fechadescarga;

                if (Tabla.Rows.Count > 0)
                {
                    if (MessageBox.Show("¿Desea generar el informe correspondiente " + tipinforme + " que se está trabajando?",
                                         "Generar Informe", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        if (creainforme == true)
                        {
                            //Recorro la columna de cantidad de cada descarga para guardar en la variable TotalGeneral el total que corresponde al periodo
                            foreach (DataRow fi in Tabla.Rows)
                            {
                                if (colum == 2)
                                {
                                    acomparar = Convert.ToDateTime(fi["FechaDescarga"]).ToString("dd-MM-yyyy").ToString();
                                }
                                else
                                {
                                    acomparar = fi["FechaDescarga"].ToString();
                                }

                                fechadescarga = Convert.ToDateTime(fi["FechaDescarga"]);
                                if (acomparar == periodo.ToString())
                                {
                                    TotalGeneral = TotalGeneral + Convert.ToInt32(fi["CantidadConex"]);
                                }
                            }
                            //si no existe la carpeta de informes la creo para guardar el documento .pdf respecto al informe del periodo 
                            if (!Directory.Exists(PathInformes))
                            {
                                Vble.CrearDirectorioVacio(PathInformes);
                            }
                            //Creo el docuemento .pdf con el formato especificado
                            Document document = new Document(PageSize.A4);
                            //Gira la hoja en posicion horizontal
                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                            //PdfWriter.GetInstance(document,
                            //              new FileStream(PathInformes + "\\Informe" + NombreInforme + periodo + "_Hora" + DateTimeOffset.Now.ToString("HHmmss") + ".pdf",
                            //                     FileMode.OpenOrCreate));

                            PdfWriter wri = PdfWriter.GetInstance(document, new FileStream(PathInformes + "\\Informe" + NombreInforme + periodo + "_Hora" + DateTimeOffset.Now.ToString("HHmmss") + ".pdf", FileMode.OpenOrCreate));
                            //PdfWriter.GetInstance(document, new FileStream(PathInformesAltas + ".pdf", FileMode.OpenOrCreate));
                            wri.PageEvent = new PageEventHelper();
                            wri.Open();

                            document.Open();

                            // ********************Creamos la imagen de DPEC y le ajustamos el tamaño
                            iTextSharp.text.Image imagenDPEC = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
                            imagenDPEC.BorderWidth = 0;
                            //imagenDPEC.Alignment = Element.ALIGN_RIGHT;

                            //imagenDPEC.SetAbsolutePosition(40f, 790f);  posicion de imagen para hoja vertical
                            imagenDPEC.SetAbsolutePosition(40f, 510f);//  posicion de imagen para hoja horizontal
                            float percentage1 = 0.0f;                 //  
                            percentage1 = 70 / imagenDPEC.Width;      //  Edito tamaño de imagen
                            imagenDPEC.ScalePercent(percentage1 * 100);//

                            //*******************Creamos la imagen de MacroIntell y le ajustamos el tamaño
                            iTextSharp.text.Image imagenMINTELL = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\MacroIntell Isologo.jpg");
                            imagenMINTELL.BorderWidth = 0;
                            //imagenMINTELL.Alignment = Element.ALIGN_LEFT;
                            //imagenMINTELL.SetAbsolutePosition(500f, 790f);  posicion de imagen para hoja vertical
                            imagenMINTELL.SetAbsolutePosition(750f, 530f);// posicion de imagen para hoja horizontal
                            float percentage2 = 0.0f;                     //
                            percentage2 = 50 / imagenMINTELL.Width;       //edito Tamaño de imagen
                            imagenMINTELL.ScalePercent(percentage2 * 100);//
                                                                          //*************************************************************************************************************************

                            //datos del informe                       
                            document.Add(imagenMINTELL);
                            document.Add(imagenDPEC);
                            document.Add(new Paragraph("  "));
                            Chunk chunk = new Chunk("Informe de Descargas pertenecientes " + tipinforme + ": " + periodo + "\n", FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 102, 0)));
                            chunk.SetUnderline(0.9f, -1.8f);
                            Paragraph titulo = new Paragraph();
                            titulo.Add(chunk);
                            titulo.Alignment = Element.ALIGN_CENTER;
                            document.Add(new Paragraph(titulo));
                            Paragraph infoinforme = new Paragraph("Fecha: " + DateTime.Today.ToString("dd/MM/yyyy") + "\n Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
                            infoinforme.Alignment = Element.ALIGN_RIGHT;
                            //document.Add(new Paragraph("                                                                                            "+
                            //                           "                                                                                          Fecha: " + DateTime.Today.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL)));
                            //document.Add(new Paragraph("                                                                                            " + 
                            //                           "                                                                                          Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL)));
                            //Cargo las variables totales que van en el resumen general del informe
                            foreach (DataRow fi in Tabla.Rows)
                            {
                                if (colum == 2)
                                {
                                    acomparar = Convert.ToDateTime(fi[colum]).ToString("dd-MM-yyyy").ToString();
                                }
                                else
                                {
                                    acomparar = fi[colum].ToString();
                                }

                                fechadescarga = Convert.ToDateTime(fi["FechaDescarga"]);
                                if (acomparar == periodo.ToString())
                                {
                                    colectoras = colectoras + 1;  //acumulador total Colectoras descargadas
                                    cantidad = (cantidad + Convert.ToInt32(fi["CantidadConex"]));//acumulador total conexiones
                                    totNOleidas = (totNOleidas + Convert.ToInt32(fi["Noleidas"]));//acumulador total No Leidas
                                    totLeidasNOimpr = (totLeidasNOimpr + Convert.ToInt32(fi["LeidasNoImpresas"])); //acumulador total leidas no impresas
                                    totimpresas = (totimpresas + Convert.ToInt32(fi["LeidasImpresas"]));//acumulador total leidas Impresas
                                    totImposLeer = (totImposLeer + Convert.ToInt32(fi["ImposibleLeer"]));//acumulador total Imposibles leer
                                    totLeidasNoImprOtros = (totLeidasNoImprOtros + (Convert.ToInt32(fi["NoImpresasImpDesconec"]) + Convert.ToInt32(fi["NoImpresasFueraDeRango"]) + Convert.ToInt32(fi["NoImpresasEstadoNeg"]) + Convert.ToInt32(fi["NoImpresasErrorDato"]) +
                                                                                       Convert.ToInt32(fi["NoImpresasDomPostal"]) + Convert.ToInt32(fi["NoImpresasIndicDato"]) + Convert.ToInt32(fi["SubTotalNeg"]) + Convert.ToInt32(fi["ErrorArchivarDatos"]) +
                                                                                       Convert.ToInt32(fi["ErroNumFactura"]) + Convert.ToInt32(fi["SinConceptosFacturar"]) + Convert.ToInt32(fi["FaltaTitular"]) + Convert.ToInt32(fi["ErrorAlFacturar"]) + Convert.ToInt32(fi["ErrorEnMemoria"]) +
                                                                                       Convert.ToInt32(fi["PeriodoExcEnDias"]) + Convert.ToInt32(fi["ErrorIndeterminado"])));
                                    TotalNOIMPRESAS = (totLeidasNOimpr + totLeidasNoImprOtros);
                                }
                            }

                            document.Add(infoinforme);
                            document.Add(new Paragraph("Resumen del Informe: ", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Total Colectoras Descargadas: " + colectoras + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Total de Conexiones Descargadas: " + TotalGeneral + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Total Impresas: " + totimpresas + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Total NO Leidas: " + totNOleidas + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Total Imposible Leer: " + totImposLeer + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Total Leidas NO Impresas: " + TotalNOIMPRESAS + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("   - Total Leidas: " + totLeidasNOimpr + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("   - Total NO Impresas (otros motivos): " + totLeidasNoImprOtros + "\n", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                            document.Add(new Paragraph("Resumen por Descargas: ", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));

                            //declaracion de tabla para volcar datos de descargas
                            iTextSharp.text.Rectangle page = document.PageSize;
                            PdfPTable table = new PdfPTable(19);
                            table.WidthPercentage = 180;
                            table.TotalWidth = page.Width - 90;
                            table.LockedWidth = true;
                            //asigno el ancho de las columnas
                            float[] widths = new float[] { 1.1f, 1.6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f};
                            table.SetWidths(widths);

                            //Estructura de tabla:
                            //Descarga | Info. de Conexiones | Cant Conex | Impresas | Leidas | No Impresas (6 casos de no impresas) | Imposible Leer | Subtotal Neg. | Error Archivar Datos| Error Nº Fact | Sin Cpto Dato | Erro al facturar | Per. Exc. en dias |


                            //datos de cada columna de la tabla 
                            AgregarEncabezado(table);

                            //double CantidadXpagina = Tabla.Rows.Count / 8;
                            //if (CantidadXpagina == 0)
                            //{
                            //    CantidadXpagina = Tabla.Rows.Count;
                            //}
                            //int cantPag = Convert.ToInt32(Math.Ceiling(CantidadXpagina));
                            int contador = 0;

                            foreach (DataRow fi in Tabla.Rows)
                            {
                                //verifica que solo cargue 8 registros de descarga por pagina,
                                //si hay mas va agregando paginas
                                if (contador == 9)
                                {                                   
                                    NumPag = NumPag + 1;                                    
                                    document.Add(new Paragraph(" "));
                                    document.Add(table);
                                    document.Add(new Paragraph(" "));
                                    document.Add(new Paragraph(" "));
                                    //Chunk chunkPag = new Chunk(NumPag.ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
                                    //TextoNumPag.Add(chunkPag);
                                    //TextoNumPag.Alignment = Element.ALIGN_CENTER;
                                    //document.Add(new Paragraph(TextoNumPag));
                                    //MessageBox.Show("iteracion medio " +NumPag.ToString());

                                    table = new PdfPTable(19);
                                    table.WidthPercentage = 180;
                                    table.TotalWidth = page.Width - 90;
                                    table.LockedWidth = true;
                                    //asigno el ancho de las columnas
                                    float[] widths2 = new float[] { 1.1f, 1.6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f, .6f };
                                    table.SetWidths(widths2);

                                    AgregarEncabezado(table);
                                    document.NewPage();
                                    document.Add(new Paragraph("  "));
                                    document.Add(new Paragraph("  "));
                                    document.Add(new Paragraph("  "));
                                    document.Add(imagenMINTELL);
                                    document.Add(imagenDPEC);
                                    document.Add(infoinforme);


                                    contador = 0;
                                }

                                if (colum == 2)
                                {
                                    acomparar = Convert.ToDateTime(fi[colum]).ToString("dd-MM-yyyy").ToString();
                                }
                                else
                                {
                                    acomparar = fi[colum].ToString();
                                }

                                fechadescarga = Convert.ToDateTime(fi["FechaDescarga"]);
                                if (acomparar == periodo.ToString())
                                {
                                    //Carga datos de la tabla a tabla armada del PDF
                                    AgregaDatosdeTabla(fi, table, TotalGeneral);
                                }

                                contador++;
                            }

                            document.Add(new Paragraph(" "));
                            document.Add(table);
                            //Agrego el numero de Pag
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            
                            //NumPag = NumPag + 1;                           
                            //Chunk chunkPag2 = new Chunk(NumPag.ToString(), FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
                            //TextoNumPag = new Paragraph("");
                            //TextoNumPag.Add(chunkPag2);
                            //TextoNumPag.Alignment = Element.ALIGN_CENTER;
                            //document.Add(new Paragraph(TextoNumPag));
                            //document.Close();
                            //document.OpenDocument();


                            wri.Add(imagenMINTELL);
                            wri.Add(imagenDPEC);
                            wri.Add(new Paragraph("  "));
                            wri.Add(titulo);
                            wri.Add(new Paragraph(""));
                            wri.Add(new Paragraph(infoinforme));
                            wri.Add(new Paragraph(""));
                            wri.Add(table);
                            //wri.Add(Total);
                            //wri.Close();
                            //document.Add(Total);

                            document.Add(new Paragraph(" "));
                            document.Close();





                            MessageBox.Show("Se genero el informe de descargas pertenecientes " + tipinforme + " " + periodo, "Informes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Disculpe no existen Descargas " + tipinforme + " para realizar su correspondiente Informe", "Informes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Disculpe no existe ninguna descarga registrada en la base de datos", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                datosAdapter.Dispose();

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);

            }
        }

        public bool HojaLLena(string[,] matriz)
        {
            foreach (string s in matriz)
            {
                if (string.IsNullOrWhiteSpace(s)) return false;
            }
            return true;
        }


        /// <summary>
        /// Obtengo la fecha de ultima modificacion del archivo pasado por parametro (en este caso la base dbFIS_DPEC que contiene los 
        /// datos para la descarga) para que habilite la opcion de poder hacer la descarga. Si la fecha de modificacion es actual, es decir,
        /// esta en un rango de 20 minutos anterior a la que muestra la pc habilitara el combobox que contiene la descripcion
        /// de la colectora para comenzar con la descarga, caso contrario el archivo que se encuentra en la colectora
        /// todavia no se sincronizó y no tendrá los datos de lectura correspondientes.
        /// </summary>
        /// <param name="fsi"></param>
        /// <param name="chekOK"></param>
        public void UltimaModificacion(FileSystemInfo fsi, FileSystemInfo Temp, PictureBox chekOK)
        {
            StringBuilder stb = new StringBuilder();

            //Leer y actualizar el número de Descarga
            Inis.GetPrivateProfileString("Numeros Descargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
            int Carga = int.Parse(stb.ToString()) + 1;
            

            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
            CultureInfo.CurrentCulture);
            Vble.NomCarpDescarga = string.Format("DP{0:yyyyMM}_{1}_D{2:00000}.{3:yyMMdd_HHmm}", Per,
                Vble.ColectoraConectada, Carga, DateTime.Now);


            string UltimaModificacion, Ahora;
            UltimaModificacion = fsi.LastWriteTime.ToString();
            Ahora = DateTime.Now.ToString();

            int result = DateTime.Compare(fsi.LastWriteTime, Temp.LastWriteTime);

            if (result > 0)
            {
                FechaArchivoBDparaDescarga.Text = "Fecha última Modificación: " + fsi.LastWriteTime.ToString("dd/MM/yyyy hh:mm");
                chekOK.Visible = true;
                cmbDevices.Enabled = true;

                if (pictureBox1.Visible == true)
                {

                    if (cmbDevices.Text != "")
                    {
                        Vble.CarpetaRespaldo = "C:\\InterfaceDPEC\\Pruebas\\Respaldo\\" + Vble.Periodo + "\\Descargadas\\" +
                                            DateTime.Now.ToString("dd-MM-yyyy") + "\\" + Vble.NomCarpDescarga;

                      

                        //Vble.CarpetaRespaldo = Vble.CarpetaRespaldo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasNoProcesadas) + "\\" +
                        //                       DateTime.Now.ToString("dd-MM-yyyy") + "\\" + cmbDevices.Text + "_" +
                        //                       Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora);

                        Directory.CreateDirectory(Vble.CarpetaRespaldo);
                        File.Copy(Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseSqlite(), Vble.CarpetaRespaldo + "\\" + Vble.NombreArchivoBaseSqlite(), true);
                        //File.Copy(Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseFijaSqlite(), Vble.CarpetaRespaldo + "\\" + Vble.NombreArchivoBaseFijaSqlite(), true);
                        File.Copy(Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoInfoCarga(), Vble.CarpetaRespaldo + "\\" + Vble.NombreArchivoInfoCarga(), true);
                       
                    }

                }
            }
            else
            {
                FechaArchivoBDparaDescarga.Text = "Fecha última Modificación: " + fsi.LastWriteTime.ToString("dd/MM/yyyy hh:mm");
            }                      
        }
        

        #region REGION BOTONES 

        void btnCerrar_Click(object sender, EventArgs e)
        {
            //this.Close();
            timer2.Stop();
            timer1.Stop();
            this.Close();
            //timer1.Stop();
            
        }

        /// <summary>
        /// Tilda todas las carpetas de descargas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarpTodo_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Itm in lsCarpetas.Items)
                Itm.Checked = true;

        }

        /// <summary>
        /// Saca el tilde de todas las carpetas de descargas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarpNada_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Itm in lsCarpetas.Items)
                Itm.Checked = false;
        }

        /// <summary>
        /// Elimina la carpeta seleccionada de la lista de carpeta de descargas
        /// Pide confirmación, lo que hace en realidad es eliminarla del 
        /// archivo ini de configuración
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            ListViewItem Itm = new ListViewItem();
            if (lsCarpetas.SelectedItems.Count > 0)
            {
                lsCarpetas.HideSelection = false;
                Itm = lsCarpetas.SelectedItems[0];
                int idx = Itm.Index;
                if (MessageBox.Show("Está seguro que desea quitar la carpeta \n" +
                    Itm.Text.Trim() + "\nde la lista de carpetas de descargas??",
                    "Quitar Carpeta de Descarga", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    //Confirmado la quita
                    lsCarpetas.Items.Remove(Itm);
                    GuardarCarpetas();
                    CargarCarpetas();
                }
            }
            else
                MessageBox.Show("No hay carpeta de descarga seleccionada",
                    "Quitar carpeta de descarga", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            lsCarpetas.HideSelection = true;
        }

        /// <summary>
        /// Agregar una nueva carpeta a la lista de descargas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog opDir = new FolderBrowserDialog();
            if (lsCarpetas.Items.Count > 0)
                opDir.SelectedPath = Directory.GetParent(lsCarpetas.Items[0].Text).ToString();
            opDir.Description = "Carpeta de Decargas de Colectoras";
            opDir.ShowNewFolderButton = true;

            if (opDir.ShowDialog(this) == DialogResult.OK)
            {
                string sK, sI;
                //Si no existe, agregar
                sI = opDir.SelectedPath.Trim();
                sK = sI.ToUpper();

                if (!lsCarpetas.Items.ContainsKey(sK))
                {
                    lsCarpetas.Items.Add(sK, sI, 0);
                    GuardarCarpetas();
                }
            }
        }


        #endregion  //Fin Region BOTONES

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        ///Lee y muestra en cmbDevice Dispositivos la colectora que esta conectada en caso de que lo esté
        /// </summary>        
        public void TomaColectoraConectada()
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var deviceIds = new Dictionary<string, string>();
                var dispositvos = new StandardWindowsPortableDeviceService();
                /// Localidades que reconocen la colectora con dispositivos.Devices.Count = 1: (Instalador Capital)
                /// Esquina
                /// Santo Tome                
                /// Localidad que reconoce la colectora con dispositivo.Devices.Count = 0: (Instalador Interior)
                /// Monte Caseros.
                /// Curuzu Cuatia.
                /// Goya
                /// Capital PC Macro intell
                /// Localidad que reconoce la colectora con dispositivo.Devices.Count = 0 EN CARGAS Y EN DESCARGAS: (Instalador Interior)
                /// Santa Rosa
                if (dispositvos.Devices.Count() == 0)
                {
                    deviceIds.Clear();
                    cmbDevices.Items.Clear();
                    btnDescargar.Enabled = false;
                    if (Directory.Exists(Vble.RutaTemporal))
                    {
                        Directory.Delete(Vble.RutaTemporal, true);
                    }
                }
                else
                {
                    foreach (var device in dispositvos.Devices)
                    {
                        device.Connect();
                        if (device.FriendlyName.Contains("MIC"))
                        {                           
                            if (deviceIds.Count() == 0)
                            {
                                deviceIds.Add(device.FriendlyName, device.DeviceID);
                                var rootFolder = device.GetContents().Files;
                                currentFolder = device.GetContents();
                                var carpetas = currentFolder.Files;
                                foreach (var CarpetaAlmacIntComp in carpetas)
                                {
                                    if (cmbDevices.Items.Count == 0)
                                    {
                                        if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
                                        {
                                            var contents = device.GetContents();
                                            cmbDevices.Items.Add(device.ToString());
                                            //cmbDevicesWifi.Items.Add(device.ToString());
                                            Vble.dispositivo = device.ToString();
                                            cmbDevices.SelectedIndex = 0;                                         
                                            btnDescargar.Enabled = true;
                                            device.Disconnect();
                                            //break;
                                        }
                                    }
                                }
                            }
                            device.Disconnect();
                        }
                        else
                        {
                            //device.Disconnect();
                            //deviceIds.Clear();
                            //cmbDevices.Items.Clear();
                            //btnDescargar.Enabled = false;

                            //cmbDevices.Items.Clear();
                            //timer2.Interval = 2000;                     
                            //ListViewColectora.Visible = false;                      
                            ////ListViewColectora.Visible = false;
                        }

                    }
                    GC.SuppressFinalize(this);
                }
            }
            catch (Exception)
            {

            }


            //try
            //{            
            //var deviceIds = new Dictionary<string, string>();
            //var dispositvos = new StandardWindowsPortableDeviceService();
            //    /// Localidades que reconocen la colectora con dispositivos.Count = 1:
            //    /// Capital
            //    /// Esquina
            //    ///Localidad que reconoce la colectora con dispositivo.Count = 0:
            //    /// Monte Caseros.
            //    /// Curuzu Cuatia.
            //    /// Santa Rosa
            //    /// capital Enzo
            //    if (dispositvos.Devices.Count() == 0)
            //    {

            //        deviceIds.Clear();
            //        cmbDevices.Items.Clear();
            //        timer1.Interval = 2000;
            //        Descargado = false;
            //        copiado = false;
            //        if (Directory.Exists(Vble.RutaTemporal))
            //        {
            //            Directory.Delete(Vble.RutaTemporal, true);
            //        }

            //    }
            //    else
            //    {
            //        foreach (var device in dispositvos.Devices)
            //    {
            //        device.Connect();
            //        if (device.FriendlyName.Contains("MIC"))
            //        {
            //            //if (deviceIds.Count() == 0)
            //            //{
            //                deviceIds.Add(device.FriendlyName, device.DeviceID);
            //                var rootFolder = device.GetContents().Files;
            //                var currentFolder = device.GetContents();
            //                var carpetas = currentFolder.Files;

            //                foreach (var CarpetaAlmacIntComp in carpetas)
            //                {
            //                    if (cmbDevices.Items.Count == 0)
            //                    {
            //                        if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
            //                        {
            //                            var contents = device.GetContents();
            //                            cmbDevices.Items.Add(device.ToString());
            //                            //cmbDevicesWifi.Items.Add(device.ToString());
            //                            Vble.dispositivo = device.ToString();
            //                            cmbDevices.SelectedIndex = 0;
            //                            timer1.Interval = 1500;
            //                            btnDescargar.Enabled = true;
            //                            device.Disconnect();
            //                            break;
            //                            }
            //                    }
            //                }
            //            //}
            //            //device.Disconnect();
            //        }
            //        else
            //         {
            //                device.Disconnect();
            //                deviceIds.Clear();
            //                cmbDevices.Items.Clear();
            //                timer1.Interval = 2000;
            //                btnDescargar.Enabled = false;
            //                Descargado = false;
            //                copiado = false;
            //                //cmbDevices.Items.Clear();
            //                //timer2.Interval = 2000;                     
            //                //ListViewColectora.Visible = false;                      
            //                ////ListViewColectora.Visible = false;
            //            }

            //            //else
            //            //{
            //            //    device.Disconnect();

            //            //        //deviceIds.Clear();
            //            //        //cmbDevices.Items.Clear();
            //            //        //timer1.Interval = 2000;
            //            //        //btnDescargar.Enabled = false;
            //            //        //Descargado = false;
            //            //        //copiado = false;
            //            //        //if (Directory.Exists(Vble.RutaTemporal))
            //            //        //    {
            //            //        //        Directory.Delete(Vble.RutaTemporal, true);
            //            //        //    }
            //            //    }

            //        }
            //    GC.SuppressFinalize(this);
            //    }

            //}
            //catch (Exception)
            //{      

            //}








            //try
            //{
            //    cmbDevices.Items.Clear();
            //    IList<WindowsPortableDevice> devices = service.Devices;
            //    devices.ToList().ForEach(device =>
            //    {
            //        device.Connect();
            //        if (Funciones.BuscarColectora(device.ToString()))
            //        {
            //            var rootFolder = device.GetContents().Files;
            //            var currentFolder = device.GetContents();
            //            var carpetas = currentFolder.Files;
            //            foreach (var CarpetaAlmacIntComp in carpetas)
            //            {
            //                if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
            //                {

            //                    var contents = device.GetContents();
            //                    cmbDevices.Items.Add(device.ToString());
            //                    btnDescargar.Enabled = true;
            //                    device.Disconnect();
            //                }
            //            }

            //            device.Disconnect();

            //        }                  
            //    });


            //    //Verifica si el combobox que contedrá el nombre de la colectora conectada contiene el nombre del dispositivo
            //    //se selecciona el item del combobox y se activa el timer2 que contiene el procedimiento de hacer el back up
            //    //temporal para la descarga.
            //    if (cmbDevices.Items.Count > 0)
            //    {                   
            //        cmbDevices.SelectedIndex = 0;
            //        //timer2.Start();
            //        //timer1.Stop();
            //    }
            //    else
            //    {
            //        ShellItem folder = new ShellItem(Environment.SpecialFolder.MyComputer);
            //        shellViewDescargas.CurrentFolder = folder;
            //        cmbDevices.Items.Clear();
            //        Descargado = false;
            //        copiado = false;
            //        timer1.Stop();
            //        timer2.Stop();
            //        btnDescargar.Enabled = false;
            //        if (Directory.Exists(Vble.RutaTemporal))
            //        {
            //            Directory.Delete(Vble.RutaTemporal, true);
            //        }

            //    }

            //}
            //catch (Exception)
            //{
            //    //MessageBox.Show(r.Message + " Error en el timer1");
            //}
        }

        private void CargarComboColectoras()
        {
            StringBuilder stb = new StringBuilder(650);
            if (Vble.ArrayZona[0].ToString() != "")
            {
                string Zona = Vble.ArrayZona[0].ToString();
                Inis.GetPrivateProfileString("Colectoras", Zona, "", stb, 650, Ctte.ArchivoIniName);
                string colectoras = stb.ToString();

                string[] individual = colectoras.Split(',');

                foreach (string subDir in individual)
                {
                    cmbDevices.Items.Add(subDir);
                }

            }

        }


        private void lsCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void cmbDevices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string Colectora = cmbDevices.Text;
                string Temporal = "C:\\Users\\" + Environment.UserName + "\\Documents\\Temporal";
                DirectoryInfo ArchivosEnTemporal = new DirectoryInfo(Temporal);
                //Pregunto si existe la carpeta temporal, si existe borra los archivos que hay en el para no prestar confusion
                //con otra ruta, etc. Si no existe crea la carpeta temporal vacia para trabajar con los archivos que
                //que se encuentran en la colectora

                if (!Directory.Exists(Temporal))
                {
                    Directory.CreateDirectory(Temporal);
                }
                else
                {
                    if (ArchivosEnTemporal.GetFiles().Length > 0)
                    {
                        foreach (var item in ArchivosEnTemporal.GetFiles())
                        {
                            File.Delete(item.FullName);
                        }
                    }

                }

                Vble.RutaColectoraConectada = Temporal;

               
                //CopiarColectora_a_Temporal.RunWorkerAsync();

                //ESTO SE HACIA CON LAS COLECTORAS CASIO DT-X200 
                //YA QUE AHORA TIENE QUE DESCARGARSE DESDE EL SERVIDOR DE WIROOS DONDE SE DEJAN LOS ARCHIVOS CUANDO LA COLECTORA(CELULARES)
                //DESCARGA PREVIAMENTE EN CADA CARPETA CORRESPONDIENTE
                //Vble.DescargarArchivosDeColectora(ArchivosEnTemporal);

                //Vble.DescargarColectora(Temporal, Vble.ArrayZona[0].ToString(), Vble.Periodo.ToString().Substring(0, 4), Vble.Periodo.ToString().Substring(5), textRemesa.Text, cmbDevices.Text, textRuta.Text);


                ///Consulta que opcion está seleccionada para asi enviar por la opción correcta
                ///Wifi o Cable
                //if (RBRecCable.Checked == true)
                //{
                    /////llamo al metodo que envia los archivos generados que estan en la pc como rutas procesadas y los envia
                    /////a la colectora
                    Vble.DescargarArchivosDeColectora(ArchivosEnTemporal);
                //}
                //else if (RBRecWifi.Checked == true)
                //{
                //    if (Vble.ExistenArchEnSeridor(Vble.ArrayZona[0].ToString(), colectora) == "NO")
                //    {
                //     Vble.DescargarColectora(Temporal, Vble.ArrayZona[0].ToString(), Vble.Periodo.ToString().Substring(0, 4), 
                //                             Vble.Periodo.ToString().Substring(5), CBRemesa.Text, cmbDevices.Text, 
                //                             textRuta.Text);
                //    }
                //}
                
                //DirectoryInfo di = new DirectoryInfo(Vble.RutaColectoraConectada);
                Vble.ArchivoInfoCargaColectora = Vble.RutaColectoraConectada + "\\" + "InfoCarga.txt";
                Vble.RutaBaseSQLiteColectora = Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseSqlite();
                Vble.BaseChicaColectora = Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseFijaSqlite();
                Vble.ColectoraConectada = cmbDevices.Text;                

                //BaseADescargar = new SQLiteConnection()

                //Lee y obtiene el nombre de la base Sqlite
                StringBuilder stb1 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                string ArchivoBase = stb1.ToString();
                //Lee y obtiene el nombre de la base Sqlite
                StringBuilder stb2 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
                string ArchivoBaseChica = stb2.ToString();
                ////recorre los dispositivos conectados y va consultando si existe alguna colectora Con la denominación "MICC-0000"
                //foreach (var item in shellViewDescargas.CurrentFolder)
                //{
                //    if (item.DisplayName == cmbDevices.Text)
                //    {
                        //shellView2.Visible = true;
                        //recorre las unidades que contiene la colectora y busca el directorio Raiz "\" para ingresar
                        //foreach (var raiz in item)
                        //{
                        //    if (raiz.DisplayName == "Almacenamiento interno compartido")
                        //    {
                        //        //recorre las subcarpetas del directorio Raiz y busca la carpeta a la cual se van a enviar los archivos desde la PC
                        //        //vble.DestinoArchivoColectora = "Datos DPEC" en archivo.ini
                        //        foreach (var carpeta in raiz)
                        //        {
                        //            if (carpeta.DisplayName == Vble.DestinoArchivosColectora)
                        //            {
                                                if (File.Exists(Vble.RutaBaseSQLiteColectora) & File.Exists(Vble.BaseChicaColectora) & File.Exists(Vble.ArchivoInfoCargaColectora))   
                                                {
                                                    //if (Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora) != "")
                                                    //{
                                                    DialogResult = MessageBox.Show("La Colectora contiene la Carga: \n" + Funciones.LeerArchivostxt(Vble.RutaColectoraConectada + "\\InfoCarga.txt")
                                                                       + ". \n Desea comenzar la descarga de las conexiones?", "Descarga de Conexiones",
                                                                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                                    if (DialogResult == DialogResult.OK)
                                                    {
                                                        lsDesc.Items.Clear();
                                                        this.ControlBox = false;
                                                        btnCerrar.Enabled = false;
                                                        this.cmbDevices.Enabled = false;
                                                        this.btnDetener.Visible = true;                                                
                                                        BGDescargaFTP.RunWorkerAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("La colectora no contiene Conexiones para descargar", "Colectora Vacia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                        //            }
                        //        }
                        //    }
                        //}
                //    }
                //}
            }
            catch (Exception R)
            {
                Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + R.Message +
                                               " Error al seleccionar Dispositivo Conectado para descarga de Colectoras \n");
                MessageBox.Show(R.Message.Substring(0, 31) + " de informacion de las conexiones", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Obtiene la cantidad De conexiones que contiene la base de la colectora para descargar que se utiliza para hacer dormir al proceso de
        /// descargar conexiones en segundo plano de BackgroundDescargarCol.
        /// </summary>
        /// <returns></returns>        
        private int CantidadRegistrosADescargar(string tabla)
        {
            ////Lee y obtiene el nombre de la base Sqlite            
            //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
            
            int count = 0;

            string txSQL;
            SQLiteCommand da;

            DataTable Tabla = new DataTable();
            try
            {
                //BaseACargar.Open();
                txSQL = "select Count(*) From " + tabla;
                da = new SQLiteCommand(txSQL, BaseADescargar);
                if (BaseADescargar.State == ConnectionState.Closed)
                {
                    BaseADescargar.Open();
                }
                count = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();
                //BaseACargar.Close();

            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message + " La colectora no contiene ninguna Ruta para Descargar. Error 001-X-SelCo*FrCon");
            }

            return count;
        }


        /// <summary>
        /// Obtiene la cantidad De conexiones que contiene la base de la colectora para descargar que se utiliza para hacer dormir al proceso de
        /// descargar conexiones en segundo plano de BackgroundDescargarCol.
        /// </summary>
        /// <returns></returns>
        private int CantidadRegistrosADescargarDistinctCF(string path, string tabla, int Periodo)
        {
            ////Lee y obtiene el nombre de la base Sqlite
            //StringBuilder stb1 = new StringBuilder("", 100);
            //Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            //string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();

            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
            //BaseACargar.Open();
            int count = 0;

            string txSQL;
            SQLiteCommand da;

            DataTable Tabla = new DataTable();
            try
            {
                BaseACargar.Open();
                txSQL = "SELECT DISTINCT Count(conexionID) FROM " + tabla + " WHERE Periodo = " + Periodo;
                da = new SQLiteCommand(txSQL, BaseACargar);
                count = Convert.ToInt32(da.ExecuteScalar());

                da.Dispose();
                BaseACargar.Close();

            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message + " La colectora no contiene ninguna Ruta para Descargar. Error 001-X-SelCo*FrCon");
            }

            return count;
        }

        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        /// que se recibe como parametro.
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="LeyendaImpresion"></param>
        /// <param name="ImpresionOBS"></param>                    
        private void VerDetallePreDescarga(string RutaDatos, string LeyendaImpresion, string ImpresionOBS)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DetalleImpresos.IndicadorTipoInforme = "PreDescarga";          
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.ImpresionOBS = ImpresionOBS;
            DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.Show();          
        }


        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga que se imprimieron
        /// </summary>
        /// <param name="RutaDatos"></param>
        private void VerDetalleImpresos(string RutaDatos, string LeyendaImpresion)
        {

            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DataTable Tabla = new DataTable();
            string txSQL;
            SQLiteDataAdapter datosAdapter;
            SQLiteCommandBuilder comandoSQL;
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
            BaseACargar.Open();
                       
            txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario FROM Conexiones C WHERE C.ImpresionOBS = 2";
            datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
            comandoSQL = new SQLiteCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            DetalleImpresos.DGResumenExp.DataSource = Tabla;

            DetalleImpresos.LabCantidad.Text = "Cantidad: " + (DetalleImpresos.DGResumenExp.RowCount).ToString();
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.ImpresionOBS = "2";
            DetalleImpresos.RutaDatos = RutaDatos;

            DetalleImpresos.Show();

            comandoSQL.Dispose();
            datosAdapter.Dispose();
            BaseACargar.Close();
        }
        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga que se Leyeron pero no se Imprimieron
        /// </summary>
        /// <param name="RutaDatos"></param>
        private void VerDetalleLeidasNoImpresas(string RutaDatos, string LeyendaImpresion)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DataTable Tabla = new DataTable();
            string txSQL;
            SQLiteDataAdapter datosAdapter;
            SQLiteCommandBuilder comandoSQL;
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
            BaseACargar.Open();
            

            txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario " +
                    "FROM Conexiones C WHERE C.ImpresionOBS = 1";
            datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
            comandoSQL = new SQLiteCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            DetalleImpresos.DGResumenExp.DataSource = Tabla;

            DetalleImpresos.LabCantidad.Text = "Cantidad: " + (DetalleImpresos.DGResumenExp.RowCount).ToString();
            DetalleImpresos.Show();
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.ImpresionOBS = "1";


            comandoSQL.Dispose();
            datosAdapter.Dispose();
            BaseACargar.Close();
        }
        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga que no se imprimieron por fuera de rango
        /// </summary>
        /// <param name="RutaDatos"></param>
        private void VerDetalleLeidasNoImpresasFueraDeRango(string RutaDatos, string LeyendaImpresion)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DataTable Tabla = new DataTable();
            string txSQL;
            SQLiteDataAdapter datosAdapter;
            SQLiteCommandBuilder comandoSQL;
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
            BaseACargar.Open();


            txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario " +
                    "FROM Conexiones C WHERE C.ImpresionOBS = 4";
            datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
            comandoSQL = new SQLiteCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            DetalleImpresos.DGResumenExp.DataSource = Tabla;

            DetalleImpresos.LabCantidad.Text = "Cantidad: " + (DetalleImpresos.DGResumenExp.RowCount).ToString();
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.ImpresionOBS = "4";
            DetalleImpresos.Show();


            comandoSQL.Dispose();
            datosAdapter.Dispose();
            BaseACargar.Close();
        }
        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga que no fueron leidas ni impresas es decir estado 0
        /// </summary>
        /// <param name="RutaDatos"></param>
        private void VerDetalleSaldos(string RutaDatos, string LeyendaImpresion)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DataTable Tabla = new DataTable();
            string txSQL;
            SQLiteDataAdapter datosAdapter;
            SQLiteCommandBuilder comandoSQL;
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
            BaseACargar.Open();


            txSQL = "SELECT C.Ruta, C.conexionID AS Nº_Conexion, C.ConsumoFacturado, C.Importe1 AS Importe_Cuota1, C.Importe2 AS Importe_Cuota2, C.Operario " +
                    "FROM Conexiones C WHERE C.ImpresionOBS = 0";
            datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
            comandoSQL = new SQLiteCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);                        
            DetalleImpresos.DGResumenExp.DataSource = Tabla;
            DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.ImpresionOBS = "0";

            comandoSQL.Dispose();
            datosAdapter.Dispose();
            BaseACargar.Close();


            DetalleImpresos.LabCantidad.Text = "Cantidad: " + (DetalleImpresos.DGResumenExp.RowCount).ToString();
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.Show();
            
        }







        /// <summary>
        /// Rellena la lista de Conexiones que se encuentra en la colectora para visualizar los datos y comenzar con la descarga de 
        /// las conexiones tanto leidas como no leidas
        /// </summary>
        /// <param name="RutaDatos"></param>
        private void MostrarRutasDeColectoras(string RutaDatos)
        {
            DataTable Tabla = new DataTable();
            int Impresas = 0;
            int LeidasNOImpresas = 0;
            int NOImpresasFueraDeRango = 0;
            int Saldos = 0;
            string Colectora = cmbDevices.Text;

            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;
                SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos);
                //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaDatos + "; Password=alVlgeDdL");
                BaseACargar.Open();
                int i = 1;
                int J = 0;
                //int cant = 0;

                txSQL = "SELECT * FROM Conexiones";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                if (Tabla.Rows.Count > 0)
                {
                    txSQL = "SELECT * FROM Conexiones";
                    SQLiteCommand command = new SQLiteCommand(txSQL, BaseACargar);
                    SQLiteDataReader reader = command.ExecuteReader();



                    //if (reader.HasRows)
                    //{
                    while (reader.Read() && this.btnDetener.Visible == true)
                    {
                        ListViewItem Datos = new ListViewItem(reader[1].ToString());//Columna Periodo
                        Datos.SubItems.Add(reader["conexionID"].ToString());//Columna Conex   
                        Datos.SubItems.Add(reader["Ruta"].ToString());//Columna Ruta                        
                        Vble.rutas = reader["Ruta"].ToString();//Almaceno el numero de Ruta descargada para utilizarlo en el directorio donde se almacenara la ruta de informes de novedades
                        Datos.SubItems.Add(reader["ImpresionOBS"].ToString());//Columna Estado
                        if (reader["ImpresionOBS"].ToString() == "2")
                        {
                            LeidasNOImpresas++;
                        }
                        else if (reader["ImpresionOBS"].ToString() == "1")
                        {
                            Impresas++;
                        }
                        else if (reader["ImpresionOBS"].ToString() == "4")
                        {
                            LeidasNOImpresas++;
                            //NOImpresasFueraDeRango++;
                        }
                        else if (reader["ImpresionOBS"].ToString() == "0")
                        {
                            Saldos++;
                        }
                        //MessageBox.Show(cmbDevices.Text);
                        Datos.SubItems.Add(Colectora);//Columna Dispositivo
                        Datos.SubItems.Add(reader["Remesa"].ToString());//Columna Remesa
                        Vble.Remesa = Convert.ToInt16(reader["Remesa"].ToString());//Almaceno el la remesa de la ruta descargada para utilizarlo en el directorio donde se almacenara la ruta de informes de novedades
                        //Datos.SubItems.Add(SubconsultaDistrito(reader["CodPostalSumin"].ToString())[0].ToString() +
                        //                   " - " + SubconsultaDistrito(reader["CodPostalSumin"].ToString())[2].ToString());//Columna Distrito y Localidad   
                        //Vble.Distrito = Convert.ToInt32(SubconsultaDistrito(reader["CodPostalSumin"].ToString())[0].ToString());
                        Datos.SubItems.Add(SubconsultaFechaLectura(BaseACargar, reader[0].ToString())[0].ToString() + " - Hora: " +
                                           SubconsultaFechaLectura(BaseACargar, reader[0].ToString())[1].ToString()); //Columna Fecha y Hora
                        Datos.SubItems.Add(reader["Operario"].ToString());//Columna Operario
                        if (Convert.ToInt16(reader["Operario"]) > 0)
                        {
                            Vble.Operario = Convert.ToInt16(reader["Operario"]);

                        }
                        else
                        {
                            Vble.Operario = 0;
                        }

                        lsDesc.Items.Add(Datos);
                        label4.Text = "Conexiones a Descargar: " + i + " de " + Tabla.Rows.Count;
                        i++;


                        //}
                    }

                    command.Dispose();
                    reader.Close();


                    //datosAdapter.Dispose();
                    //BaseACargar.Close();
                    ///Cargo los labels que informan los totales de la pre descarga 
                    //LabImprPre.Text = "Total Impresas: " + Impresas.ToString();
                    //LabLeidNoImprePre.Text = "Total Leidas No impresas: " + LeidasNOImpresas.ToString();
                    //LabNoImprFueraRango.Text = "Total NO Impresas Fuera de Rango: " + NOImpresasFueraDeRango.ToString();
                    //LabSaldos.Text = "Saldos: " + Saldos.ToString();


                }
                else
                {
                    MessageBox.Show("Disculpe la Base de datos de las conexiones existe pero la misma está vacia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Tabla.Dispose();
                comandoSQL.Dispose();
                datosAdapter.Dispose();
                BaseACargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + ". Error al mostrar datos en pre-descarga");
            }
        }

        /// <summary>
        /// Devuelve datos del Distrito como ser Codigo Interno, localidad y provincia recibiendo como parametro el codigo postal
        /// y la ruta donde se encuentra la base de datos que contiene la colectora
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="conex"></param>
        /// <returns></returns>
        private ArrayList SubconsultaDistrito(string CodigoPostal)
        {
            DataTable Tabla = new DataTable();
            ArrayList DatosDistrito = new ArrayList();
            CodigoPostal = Convert.ToInt32(CodigoPostal) < 3000 ||
                           Convert.ToInt32(CodigoPostal) == 0 || CodigoPostal.Length > 4 ||
                           CodigoPostal.Length < 4 ? "3400" : CodigoPostal;
            try
            {
                string txSQL;
                MySqlDataAdapter datosAdapter;
                MySqlCommandBuilder comandoSQL;

                txSQL = "SELECT CodigoInt, Provincia, Localidad FROM Localidades WHERE CodigoPostal = '" + CodigoPostal + "'";
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    DatosDistrito.Add(fi[0].ToString());//CodigoInt
                    DatosDistrito.Add(fi[1].ToString());//Provincia
                    DatosDistrito.Add(fi[2].ToString());//Localidad
                    //CodigoInt = fi[0].ToString();
                }
                datosAdapter.Dispose();
                return DatosDistrito;

            }

            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            return DatosDistrito;
        }

        public int VerificarRegistrosEnBaseMysql()
        {
            string txSQL = "select Count(*) From Conexiones";
            MySqlCommand da = new MySqlCommand(txSQL, DB.conexBD);
            int count = Convert.ToInt32(da.ExecuteScalar());
            da.Dispose();
            return count;
        }


        /// <summary>
        /// Devuelve la fecha de lectura de cada conexion y la muestra en la lista 
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="conex"></param>
        /// <returns></returns>
        private ArrayList SubconsultaFechaLectura(SQLiteConnection BaseACargar, string conex)
        {
            DataTable Tabla = new DataTable();
            ArrayList Fechas = new ArrayList();
            //SQLiteConnection BaseACargar = new SQLiteConnection(RutaDatos);
            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;

                txSQL = "SELECT ActualFecha, ActualHora FROM Medidores WHERE conexionID = " + conex;
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);
                if (Tabla.Rows.Count > 0)
                {               
                    foreach (DataRow fi in Tabla.Rows)
                    {
                        Fechas.Add((fi[0]));//FechaLectura
                        Fechas.Add(fi[1]);//HoraLectura   
                    }
                }
                else
                {
                    Fechas.Add("0");//FechaLectura
                    Fechas.Add("0");//HoraLectura 
                }
                comandoSQL.Dispose();
                datosAdapter.Dispose();
                //BaseACargar.Close();
                return Fechas;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

            //BaseACargar.Close();
            return Fechas;
        }

        /// <summary>
        /// Cambia el estado impresionOBS de cada conexion a 05xx que se esta descargando a la colectora de la base
        /// MySQL general donde estan todas las conexiones, respetando los codigos de impresion que se editaron en la lectura
        /// </summary>
        /// 
        //public void CambiarEstadoRecibidoMySql(string RutaRecibir, int StatusChange)
        public void CambiarEstadoRecibidoMySql(int StatusChange)
        {
            DataTable Tabla = new DataTable();
            DataTable TablaRuta = new DataTable();
            Vble.NºInstalacionImpresos.Clear();
            Vble.ContratoImpresos.Clear();
            Vble.TitularImpresos.Clear();
            Vble.FacturaImpresos.Clear();

            //DataTable tablamedidores = new DataTable();
            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter = new SQLiteDataAdapter(); ;
                SQLiteCommandBuilder comandoSQL = new SQLiteCommandBuilder();
                Int32 conexionID, Operario = 0, OpBel;
                Int32 ImpresionOBS, ImpresionCANT, ConsumoFacturado, ActualEstado, TipoLectura, OrdenTomado; 
                //Int32 PuntoVenta, FacturaNro1, FacturaNro2;
                double Latitud, Longitud;
                //string FacturaLetra;
                string ActualFecha, ActualHora, VencimientoProx;
                string EstadoCorreccion = "", EstadoFacturado = "", EstadoReactiva = "", EstadoInyectada = "";

                Vble.ArrayRutas.Clear();

                //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                //BaseACargar.Open();

                txSQL = "SELECT DISTINCT C.conexionID, C.OpBel, C.Contrato, C.TitularID, C.Periodo, C.ImpresionOBS, " +
                "C.ImpresionCANT, C.Operario, C.Ruta, C.ConsumoFacturado, C.Zona, C.Remesa, C.OrdenTomado, C.VencimientoProx, M.ActualFecha, M.ActualHora, M.ActualEstado, M.Latitud, M.Longitud, " +
                " M.TipoLectura, M.EstadoCorreccion, M.EstadoFacturado, M.EstadoReactiva, M.EstadoInyectada " +
                " FROM Conexiones C " +
                "INNER JOIN Medidores M ON M.conexionID = C.conexionID";

                datosAdapter = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    //////asignación a variables locales para manejar en el UPDATE
                    conexionID = Convert.ToInt32(fi["conexionID"]);
                    Vble.PeriodoEnColectora = Convert.ToInt32(fi["Periodo"]);
                    ImpresionOBS = Convert.ToInt32(fi["ImpresionOBS"]);
                    ImpresionCANT = Convert.ToInt32(fi["ImpresionCANT"]);
                    if (Convert.ToInt32(fi["Operario"]) > 0) {
                        Operario = Convert.ToInt32(fi["Operario"]);
                        Vble.Operario = Convert.ToInt32(fi["Operario"]);
                    }                
                    else
                    {
                        Operario = 0;
                        Vble.Operario = 0;
                    }
                    //Vble.Operario = Convert.ToInt32(fi["Operario"]);
                    ConsumoFacturado = Convert.ToInt32(fi["ConsumoFacturado"]);
                    OpBel = Convert.ToInt32(fi["OpBel"]);
                    Vble.Distrito = fi["Zona"].ToString() == "" || fi["Zona"].ToString().Any(x => !char.IsNumber(x)) ? 0 : Convert.ToInt32(fi["Zona"]);
                    //Importe1 = Convert.ToDouble(fi["Importe1"], CultureInfo.CreateSpecificCulture("en-US"));                                    
                    OrdenTomado = Convert.ToInt32(string.IsNullOrEmpty(fi["OrdenTomado"].ToString()) ? 0 : Convert.ToInt32(fi["OrdenTomado"].ToString()));
                
                    VencimientoProx = fi["VencimientoProx"].ToString();
                    Vble.CantConex = Tabla.Rows.Count;                   
                    Vble.Remesa = Convert.ToInt32(fi["Remesa"]);

                    string update;//Declaración de string que contendra la consulta UPDATE               
                    update = "UPDATE Conexiones SET ImpresionOBS = " + (ImpresionOBS + (StatusChange * 100)) +
                                                    ", ImpresionCANT = " + ImpresionCANT +
                                                    ", Periodo = " + Vble.PeriodoEnColectora +
                                                    ", Operario = " + Operario +
                                                    ", ConsumoFacturado = " + ConsumoFacturado +
                                                    ", OpBel = " + OpBel +
                                                    ", OrdenTomado = " + OrdenTomado +
                                                    ", VencimientoProx = '" + VencimientoProx + "'" + 
                                                    " WHERE ConexionID = " + conexionID + " AND Periodo = " + Vble.PeriodoEnColectora;
                    //preparamos la cadena pra insercion
                    MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //comandoSQL.Dispose();

                    //conexionID = Convert.ToInt32(fi["conexionID"]);                   
                    ActualFecha = (fi["ActualFecha", DataRowVersion.Original].ToString() == "0" ||
                                   fi["ActualFecha", DataRowVersion.Original].ToString() == "" ||
                                   fi["ActualFecha", DataRowVersion.Original].ToString().Contains("1/1/2000") ? "2000/01/01" : fi["ActualFecha"].ToString());
                    ActualHora = (fi["ActualHora", DataRowVersion.Original].ToString() == "0" ||
                                  fi["ActualHora", DataRowVersion.Original].ToString() == "" ? "00:00:00" : fi["ActualHora"].ToString());
                    ActualEstado = Convert.ToInt32(fi["ActualEstado"]);
                    Latitud = fi["Latitud"].ToString() == "" ? 0 : Convert.ToDouble(fi["Latitud"], CultureInfo.CreateSpecificCulture("es-AR"));
                    Longitud = fi["Longitud"].ToString() == "" ? 0 : Convert.ToDouble(fi["Longitud"], CultureInfo.CreateSpecificCulture("es-AR"));
                    EstadoCorreccion = string.IsNullOrEmpty(fi["EstadoCorreccion"].ToString()) ? "" : fi["EstadoCorreccion"].ToString();
                    EstadoFacturado = string.IsNullOrEmpty(fi["EstadoFacturado"].ToString()) ? "" : fi["EstadoFacturado"].ToString();
                    EstadoReactiva = string.IsNullOrEmpty(fi["EstadoReactiva"].ToString()) ? "" : fi["EstadoReactiva"].ToString();
                    EstadoInyectada = string.IsNullOrEmpty(fi["EstadoInyectada"].ToString()) ? "" : fi["EstadoInyectada"].ToString();

                    TipoLectura = Convert.ToInt32(fi["TipoLectura"]);

                    string updateMedidores;//Declaración de string que contendra la consulta UPDATE               
                    updateMedidores = "UPDATE Medidores SET ActualFecha = '" + Convert.ToDateTime(ActualFecha).ToString("yyyy/MM/dd") + "', " +
                                      "ActualHora = '" + Convert.ToDateTime(ActualHora).ToString("HH:mm") + "', " +
                                      " ActualEstado = " + ActualEstado + ", " +
                                      " TipoLectura = " + TipoLectura + ", " +
                                      " Latitud = " + Latitud.ToString().Replace(",", ".") + ", " +
                                      " Longitud = " + Longitud.ToString().Replace(",", ".") + ", " +
                                      " EstadoCorreccion = '" + EstadoCorreccion.ToString().Replace(",", ".") + "', " +
                                      " EstadoFacturado = '" + EstadoFacturado.ToString().Replace(",", ".") + "', " +
                                      " EstadoReactiva = '" + EstadoReactiva.ToString().Replace(",", ".") + "', " +
                                      " EstadoInyectada = '" + EstadoInyectada.ToString().Replace(",", ".") + "' " +
                                      " WHERE ConexionID = " + conexionID + " AND Periodo = " + Vble.PeriodoEnColectora;

                    //preparamos la cadena pra insercion
                    MySqlCommand command2 = new MySqlCommand(updateMedidores, DB.conexBD);
                    //y la ejecutamos
                    command2.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command2.Dispose();

                    backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                    AvanceDescarga++;
                }

                comandoSQL.Dispose();
                datosAdapter.Dispose();

                string txSQLDistinctRuta = "SELECT DISTINCT Ruta FROM Conexiones";

                datosAdapter = new SQLiteDataAdapter(txSQLDistinctRuta, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(TablaRuta);                

                foreach (DataRow fi in TablaRuta.Rows)
                {
                    Vble.ArrayRutas.Add(fi["Ruta"].ToString());
                }

                comandoSQL.Dispose();
                datosAdapter.Dispose();
                //BaseACargar.Close();

            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message);
            }
        }

        /// <summary>
        ///Metodo que genera la carpeta de Secuencias Seleccionadas Descargadas de acuerdo a parametros de creacion como Periodo,
        ///Distrito, Carga Nº, Fecha de generación del procesamiento.
        /// </summary>
        private void GenerarCarpetaDescarga()
        {
            try
            {
                string ArchivoTabla;
                string archivosecuencia;
                //string Carp;
                int Carga = 0;
                StringBuilder stb = new StringBuilder();

                //Lee y obtiene el nombre de la base Sqlite                        
                StringBuilder stb1 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                string archivo = stb1.ToString();

                //Leer y actualizar el número de Descarga
                Inis.GetPrivateProfileString("Numeros Descargas", Vble.Distrito.ToString(), "0", stb, 50, Ctte.ArchivoIniName);
                Carga = int.Parse(stb.ToString()) + 1;
                Inis.WritePrivateProfileString("Numeros Descargas", Vble.Distrito.ToString(),
                Carga.ToString().Trim(), Ctte.ArchivoIniName);

                DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
                CultureInfo.CurrentCulture);
                Vble.NomCarpDescarga = string.Format("DP{0:yyyyMM}_{1}_D{2:00000}.{3:yyMMdd_HHmm}", Per,
                    cmbDevices.Text, Carga, DateTime.Now);

                //Genero Ruta donde se va a almacenar datos de la descarga
                ArchivoTabla = Vble.CarpetaTrabajo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasRecibidas) + Vble.NomCarpDescarga;
                Vble.DirectorioDescarga = ArchivoTabla;
                //Genero la carpeta que contendra el archivo de base sqlite de la descarga realizada.
                Vble.CrearDirectorioVacio(ArchivoTabla);
                string destino = ArchivoTabla + "\\" + archivo;

                //copia el archivo de base de datos Sqlite al directorio generado anteriormente.
                CopiaArchivos(Vble.RutaBaseSQLiteColectora, destino);

                Vble.ActualizarPanelesEnCargasDeColectoras(Vble.ArchivoInfoCargaColectora, Vble.DirectorioDescarga);


                ////A PARTIR DE ACA GENERO ARCHIVO CON DATOS DE LA CARGA QUE SE PROCESO 
                ////COMO SER RUTA, SECUENCIA, CANTIDAD DE REGISTROS CARGADOS, ETC
                Vble.ArchivoInfoDescargaColectora = "InfoDescarga.txt";
                //string filename = "InfoDescarga.txt";
                archivosecuencia = System.IO.Path.Combine(ArchivoTabla, Vble.ArchivoInfoDescargaColectora);
                ////Llamo al metodo que crea el archivo InfoCarga.txt que contiene informacion de la descarga procesada
                CrearArchivoInfoDescarga(Vble.RutaColectoraConectada, Vble.ColectoraConectada, Vble.NomCarpDescarga, Vble.ArchivoInfoDescargaColectora, Vble.DirectorioDescarga);

                /// Metodo que copia el directorio completo de la descarga realizada al file Server ubicado en la NAS.
                CopiaEnFileServer(Vble.DirectorioDescarga);

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al generar el directorio de Descargas ");
            }

        }

        /// <summary>
        /// Recibe como parametro la variable que contiene el directorio donde se descarga de manera local los archivos de la ruta
        /// y se hace una copia en el File Server NAS para respaldo centralizado.
        /// </summary>
        /// <param name="directorioDescarga"></param>
        private void CopiaEnFileServer(string directorioDescarga)
        {
            try
            {
                //Cierro toda conexion por las dudas que este abierta sino, no me va a dejar importar
                //Vble.CerrarUnidadDeRed();   
             
                if (DB.Entorno == "PRD")
                {
                    //Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaSAPImportacion);
                    DirectoryInfo directoryName = new DirectoryInfo(directorioDescarga);
                    string DirecDescServer = Vble.CarpetaCar_Desc_ColectorasNAS_PRD + "\\Descargas\\" + Vble.Periodo + "\\Remesa_" + Vble.Remesa + "\\" + Vble.Distrito + "\\" + directoryName.Name;
                    if (!Directory.Exists(DirecDescServer))
                    {
                        Directory.CreateDirectory(DirecDescServer);
                    }
                    foreach (var file in Directory.GetFiles(directorioDescarga))
                    {
                        FileInfo fileName = new FileInfo(file);
                        File.Copy(file, DirecDescServer + "\\" + fileName.Name, true);
                    }
                }
                else if (DB.Entorno == "QAS")
                {
                    //Vble.AbrirUnidadDeRed(@"Y:", @"" + Vble.CarpetaSAPImportacionPRUEBA);
                    DirectoryInfo directoryName = new DirectoryInfo(directorioDescarga);
                    string DirecDescServer = Vble.CarpetaCar_Desc_ColectorasNAS_QAS + "\\Descargas\\" + Vble.Periodo + "\\Remesa_" + Vble.Remesa + "\\" + Vble.Distrito +  "\\" + directoryName.Name;
                    if (!Directory.Exists(DirecDescServer))
                    {
                        Directory.CreateDirectory(DirecDescServer);
                    }
                    foreach (var file in Directory.GetFiles(directorioDescarga))
                    {
                        FileInfo fileName = new FileInfo(file);                        
                        File.Copy(file, DirecDescServer + "\\" + fileName.Name, true);
                    }   
                }
                //Vble.CerrarUnidadDeRed();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Error al intentar copiar los archivos descargados al File Server", "Erro al copiar en File Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Copiar archivo
        /// </summary>
        public static void CopiaArchivos(string RutaOrigen, string RutaDestino)
        {
            Computer mycomputer = new Computer();
            try
            {
                if (RutaOrigen != "" && RutaDestino != "")
                {
                    mycomputer.FileSystem.CopyFile(RutaOrigen, RutaDestino);
                }
                else
                {
                    MessageBox.Show("No se generó correctamente el Archivo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Funcion que crea el archivo vacio, con la secuencia como nombre para utilizar como información a la hora de mostrar en el listview1
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        private void CrearArchivoInfoDescarga(string archivosecuencia, string colectora, string carp, string filename, string rutadescargada)
        {
            try
            {

                StringBuilder stb1 = new StringBuilder("", 250);
                Inis.GetPrivateProfileString("Archivos", "NombreArchivoInfoDescarga", "", stb1, 250, Ctte.ArchivoIniName);
                string nombrearchivo = stb1.ToString();

                //int i = 0;               
                StringBuilder stB = new StringBuilder();
                lsCarpetas.Items.Clear();

                DirectoryInfo directorio = new DirectoryInfo(archivosecuencia);
                tvInformes.BeginInvoke(new InvokeDelegate(InvokeMethod));
                //Descarga = carp;
                Vble.lineas = Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora);

                PrintRecursive(nodoraiz);
                CreateInfoCarga(rutadescargada + "\\" + filename, filename, Vble.lineas);

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al crear archivo de informacion de descarga");
            }
        }
        /// <summary>
        /// recorre recursivamente los nodos de la descarga que se esta generando para almacenar datos en Vble.Lineas que luego
        /// se utiliza para generar archivo txt con datos
        /// </summary>
        /// <param name="treeNode"></param>
        private void PrintRecursive(TreeNode treeNode)
        {
            // Print the node.

            Vble.lineas += treeNode.Text + "\n";
            // Print each node recursively.
            foreach (TreeNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn);
            }
        }

        public void InvokeMethod()
        {
            nodoraiz = tvInformes.Nodes.Add(Vble.NomCarpDescarga);
            nodoraiz.Nodes.Add("Equipo: " + cmbDevices.Text);
            nodoraiz.Nodes.Add("Lecturista: " + Vble.Operario);
            string infoconexion = "Informacion de Conexiones: ";
            nodoraiz.Nodes.Add(infoconexion);
            nodoraiz.LastNode.Nodes.Add(Vble.InfoDescarga);
            string cantidadConexiones = "Cantidad de Conexiones: " + Vble.CantConex;
            nodoraiz.Nodes.Add(cantidadConexiones);
            nodoraiz.LastNode.Nodes.Add("No Leidos: " + Vble.ConexNoLeidas);
            nodoraiz.LastNode.Nodes.Add("Leidas NO Impresas: " + Vble.ConexLeidasNoImpresas);
            nodoraiz.LastNode.Nodes.Add("Leidas Impresas: " + Vble.ConexLeidasImpresas);
            nodoraiz.LastNode.Nodes.Add("NO Impresas Impresora Desconectada: " + Vble.ConexNoImpresasImpresoraDesc);
            nodoraiz.LastNode.Nodes.Add("NO Impresas Fuera de Rango: " + Vble.ConexNoImpresasFueradeRango);
            nodoraiz.LastNode.Nodes.Add("NO Impresas Estado Negativo: " + Vble.ConexNoImpresasEstadoNegativo);
            nodoraiz.LastNode.Nodes.Add("NO Impresas Error Dato: " + Vble.ConexNoImpresasErrorDato);
            nodoraiz.LastNode.Nodes.Add("NO Impresas Domicilio Postal: " + Vble.ConexNoImpresasDomicilioPostal);
            nodoraiz.LastNode.Nodes.Add("NO Impresas Indicado Dato: " + Vble.ConexNoImpresasIndicadoDato);
            nodoraiz.LastNode.Nodes.Add("Imposible Leer: " + Vble.ConexImposibleLeer);
            nodoraiz.LastNode.Nodes.Add("Sub Total Negativo: " + Vble.ConexSubtNeg);
            nodoraiz.LastNode.Nodes.Add("Error al Archivar Datos: " + Vble.ConexErrorArchDatos);
            nodoraiz.LastNode.Nodes.Add("Error en Nº de Factura: " + Vble.ConexErrorNFact);
            nodoraiz.LastNode.Nodes.Add("Sin Conceptos a Facturar: " + Vble.ConexSinConcepFacturar);
            nodoraiz.LastNode.Nodes.Add("Falta Titular: " + Vble.FaltaTitular);
            nodoraiz.LastNode.Nodes.Add("Error al Facturar: " + Vble.ConexErrorFacturando);
            nodoraiz.LastNode.Nodes.Add("Error en Memoria: " + Vble.ErroEnMemoria);
            nodoraiz.LastNode.Nodes.Add("Periodo Excedido en Días: " + Vble.ConexPerExcDias);
            nodoraiz.LastNode.Nodes.Add("Error por otros Motivos:" + Vble.ErrorIndeterminado);
            nodoraiz.LastNode.Nodes.Add("--------------------------------------------------------------------------------");

        }


        //Crea archivo en la ruta que se pasa como parametro en variable 
        //archivosecuencia, con el nombre y los datos dentro
        public void CreateInfoCarga(string archivosecuencia, string filename, string lineas)
        {
            //Creo el archivo que contiene la información de la carga que se proceso y esta lista para enviar
            if (!System.IO.File.Exists(archivosecuencia))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(archivosecuencia))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
            else
            {
                MessageBox.Show("Archivo \"{0}\" no exite.", filename);

                return;
            }

            System.IO.File.WriteAllText(archivosecuencia, Vble.lineas);
        }


        private void btnAgregar_Click_1(object sender, EventArgs e)
        {

        }

        private void btnCarpTodo_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < lsDesc.Items.Count; i++)
            {
                lsDesc.Items[i].Selected = true;
            }

        }



        /// <summary>
        /// Boton que realiza la descarga de conexiones desde la colectora a la pc de acuerdo a los codigos de impresion que tienen cada conexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {

                string Colectora = cmbDevices.Text;
                string Temporal = "C:\\Users\\" + Environment.UserName + "\\Documents\\Temporal";
                DirectoryInfo ArchivosEnTemporal = new DirectoryInfo(Temporal);
                //Pregunto si existe la carpeta temporal, si existe borra los archivos que hay en el para no prestar confusion
                //con otra ruta, etc. Si no existe crea la carpeta temporal vacia para trabajar con los archivos que
                //que se encuentran en la colectora

                if (!Directory.Exists(Temporal))
                {
                    Directory.CreateDirectory(Temporal);
                }
                else
                {
                    if (ArchivosEnTemporal.GetFiles().Length > 0)
                    {
                        foreach (var item in ArchivosEnTemporal.GetFiles())
                        {
                            try
                            {
                                item.Delete();
                            }
                            catch (Exception)
                            {
                                SQLiteConnection Base = new SQLiteConnection("Data Source=" + item.FullName);
                                if (Base.State == ConnectionState.Open)
                                {
                                    Base.Dispose();
                                    Base.Close();
                                }       
                            }                           
                        }
                    }
                }

                Vble.RutaColectoraConectada = Temporal;

                ///*****Consulta que opcion está seleccionada para asi descargar por la opción correcta
                ///*****Wifi o Cable
                if (RBRecCable.Checked == true)
                {
                    ///****llamo al metodo que envia los archivos generados que estan en la pc como rutas procesadas y los envia
                    ///****a la colectora
                    bool filesDownload;
                    filesDownload = Vble.DescargarArchivosDeColectora(ArchivosEnTemporal);
                    //DirectoryInfo di = new DirectoryInfo(Vble.RutaColectoraConectada);
                    Vble.ArchivoInfoCargaColectora = Vble.RutaColectoraConectada + "\\" + "InfoCarga.txt";
                    Vble.RutaBaseSQLiteColectora = Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseSqlite();
                    Vble.BaseChicaColectora = Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseFijaSqlite();
                    Vble.ColectoraConectada = cmbDevices.Text;
                    if (filesDownload)
                    {
                        BaseADescargar = new SQLiteConnection("Data Source=" + Vble.RutaBaseSQLiteColectora);
                        BaseADescargar.Open();
                        //Lee y obtiene el nombre de la base Sqlite
                        StringBuilder stb1 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                        string ArchivoBase = stb1.ToString();
                        //Lee y obtiene el nombre de la base Sqlite
                        StringBuilder stb2 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
                        string ArchivoBaseChica = stb2.ToString();
                    }
                    //if (lsDesc.Items.Count > 0)//esto comente para probar la descarga sin cargar en el listview para no tardar 
                    //{
                    if (File.Exists(Vble.RutaBaseSQLiteColectora) & File.Exists(Vble.BaseChicaColectora) & File.Exists(Vble.ArchivoInfoCargaColectora))
                    {
                        if (MessageBox.Show("¿Está seguro que desea realizar la descarga de la colectora que contiene: \n " +
                            Funciones.LeerArchivostxt(Vble.RutaColectoraConectada + "\\InfoCarga.txt") + "?", "Descarga de Colectora", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            if (VerificarRegistrosEnBaseMysql() > 0)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    progressBar.Visible = true;
                                    labelPorcDesc.Visible = true;
                                });                                
                                backgroundDescargarAPC.RunWorkerAsync();
                            }
                            else
                            {
                                MessageBox.Show("Disculpe no se va a poder realizar la descarga debido a que la Base de Datos General no se encuentra cargada.",
                                                "Descarga de Colectora", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            if (BaseADescargar.State == ConnectionState.Open)
                            {
                                BaseADescargar.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("La colectora no contiene Conexiones para descargar", "Colectora Vacia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (RBRecWifi.Checked == true)
                {
                    PanelDescargaFTP.Visible = true;
                    LbleDescargandoFTP.Text = "Verificando Ruta en servidor FTP...";
                   
                    await Task.Run(async () =>
                   {
                       await HabilitarWaitingDescarga(Temporal);
                   });
                    
                }   
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        private async Task HabilitarWaitingDescarga(string Temporal)
        {
           Task oTask = new Task(MetodoDescargaFTP);
            oTask.Start();
            await oTask;

        }

        /// <summary>
        /// Dentro de éste metodo se encuentran las conexiones con el servidor FTP
        /// el cual es wiroos de Macro Intell,
        /// donde se dejan los archivos para las cargas y descargas de las colectoras
        /// en esta caso verifica las carpetas de las descargas
        /// </summary>
        public async void MetodoDescargaFTP()
        {
            string Colectora = cmbDevices.Text;
            string Temporal = "C:\\Users\\" + Environment.UserName + "\\Documents\\Temporal";

            string fechaSeleccionado = DTPikerDescarga.Value.Year.ToString("0000") + DTPikerDescarga.Value.Month.ToString("00") + DTPikerDescarga.Value.Day.ToString("00");
            string localDescargar = CBZona.Text.Substring(0, CBZona.Text.IndexOf("-"));
            ///se comenta la siguiente linea que hace verifica que existan los 3 archivos que se encuentran en el servidor con la versión 1.2.4 de la aplicación de colectora el cual lo deja así
            //string existearchivo = await Vble.ExistenArchEnSeridor(Vble.locCentroInterfaz, Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text, Temporal).ConfigureAwait(true);
            ///la siguiente linea, hace lo mismo que la anterior, pero solamente verifica que exista 1 archivo, que es el archivo comprimido que genéra la version 1.2.5 de la aplicación de la colectora,
            ///de esta forma se busco optimizar tanto en la demora del envio desde la aplicacion de la colectora como en las perdidas de algun archivo en el envio imparcial de la version anterior.
            //string existearchivo = await Vble.ExisteArchZipEnServidor(Vble.locCentroInterfaz, Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text, Temporal).ConfigureAwait(true);
            int existearchivo = await Vble.CantFilesInServer(localDescargar.Replace(" ", ""), Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text, Temporal).ConfigureAwait(true);


            if (existearchivo == 1)
            {

                //LbleDescargandoFTP.Text = "Descargando desde servidor FTP...";               
                //string filesDownload = await Vble.DescargarColectora(Temporal, Vble.locCentroInterfaz, Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text);
                string filesDownload = await Vble.DescargarArchivoZip(Temporal, localDescargar.Replace(" ", ""), Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text);

                DirectoryInfo archivoZipEnTemporal = new DirectoryInfo(Temporal);
                string patharchivoZip = "";
                string archivoZip = "";

                foreach (var item in archivoZipEnTemporal.GetFiles())
                {
                    patharchivoZip = item.FullName;
                    archivoZip = item.Name;
                }

                Vble.DescomprimirArchivo(patharchivoZip, Temporal);


                if (MessageBox.Show("¿Se encontró la ruta:" + Funciones.LeerArchivostxt(Temporal + "\\InfoCarga.txt") +
                                    "en el servidor, desea descargar?", "Descarga de Colectora", MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {                    

                    if (filesDownload == "SI")
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            progressBar.Visible = true;
                            labelPorcDesc.Visible = true;
                        });
                        StringBuilder stb1 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                        string ArchivoBase = stb1.ToString();

                        //Lee y obtiene el nombre de la base Sqlite
                        StringBuilder stb2 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
                        string ArchivoBaseChica = stb2.ToString();

                        //Lee y obtiene el nombre de la base Sqlite
                        StringBuilder stb3 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "NombreArchivoInfo", "", stb3, 100, Ctte.ArchivoIniName);
                        string InfoCarga = stb3.ToString();

                        Vble.RutaBaseSQLiteColectora = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal) + ArchivoBase;
                        Vble.RutaBaseFijaSQLiteColectora =  Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal) + ArchivoBaseChica;
                        Vble.ArchivoInfoCargaColectora = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal) + InfoCarga;
                        Vble.BaseChicaColectora = Vble.RutaBaseFijaSQLiteColectora;

                        Vble.CarpetaTemporal = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal);
                        BaseADescargar = new SQLiteConnection("Data Source=" + Vble.CarpetaTemporal+ ArchivoBase);
                        BaseADescargar.Open();                             

                    }
                    //BaseADescargar.Close();
                    ///Despues de haber descargado elimina los archivos del servidor
                    if (filesDownload == "SI")
                    {
                        string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
                        string RutaVerificar = ficFTP + localDescargar.Replace(" ", "") + "/" + Vble.Periodo.ToString().Replace("-", "") + "/" + CBRemesa.Text + "/" + fechaSeleccionado + "/" + cmbDevicesWifi.Text +"/";
                        //Vble.EliminarArchivos(RutaVerificar);
                        Vble.EliminarArchivoZIP(RutaVerificar+ archivoZip);
                    }

                    PanelDescargaFTP.Visible = false;
                    //this.Cursor = Cursors.WaitCursor;
                    if (VerificarRegistrosEnBaseMysql() > 0)
                    {
                        //progressBar.Visible = true;
                        backgroundDescargarAPC.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Disculpe no se va a poder realizar la descarga debido a que la Base de Datos General no se encuentra cargada.",
                                        "Descarga de Colectora", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

               
                }
                else
                {
                    PanelDescargaFTP.Visible = false;
                    if (BaseADescargar.State == ConnectionState.Open)
                    {
                        BaseADescargar.Close();
                    }
                }
            }
            else if (existearchivo == 3)
            {
                //LbleDescargandoFTP.Text = "Descargando desde servidor FTP...";               
                string filesDownload = await Vble.DescargarColectora(Temporal, localDescargar.Replace(" ", ""), Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text);
                //string filesDownload = await Vble.DescargarArchivoZip(Temporal, Vble.locCentroInterfaz, Vble.Periodo.ToString().Replace("-", ""), CBRemesa.Text, fechaSeleccionado, cmbDevicesWifi.Text);

                //DirectoryInfo archivoZipEnTemporal = new DirectoryInfo(Temporal);
                //string patharchivoZip = "";
                //string archivoZip = "";

                //foreach (var item in archivoZipEnTemporal.GetFiles())
                //{
                //    patharchivoZip = item.FullName;
                //    archivoZip = item.Name;
                //}
                //Vble.DescomprimirArchivo(patharchivoZip, Temporal);


                if (MessageBox.Show("¿Se encontró la ruta:" + Funciones.LeerArchivostxt(Temporal + "\\InfoCarga.txt") +
                                    "en el servidor, desea descargar?", "Descarga de Colectora", MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    if (filesDownload == "SI")
                    {
                        StringBuilder stb1 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                        string ArchivoBase = stb1.ToString();

                        //Lee y obtiene el nombre de la base Sqlite
                        StringBuilder stb2 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
                        string ArchivoBaseChica = stb2.ToString();

                        //Lee y obtiene el nombre de la base Sqlite
                        StringBuilder stb3 = new StringBuilder("", 100);
                        Inis.GetPrivateProfileString("Archivos", "NombreArchivoInfo", "", stb3, 100, Ctte.ArchivoIniName);
                        string InfoCarga = stb3.ToString();

                        Vble.RutaBaseSQLiteColectora = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal) + ArchivoBase;
                        Vble.RutaBaseFijaSQLiteColectora = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal) + ArchivoBaseChica;
                        Vble.ArchivoInfoCargaColectora = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal) + InfoCarga;
                        Vble.BaseChicaColectora = Vble.RutaBaseFijaSQLiteColectora;

                        Vble.CarpetaTemporal = Vble.ValorarUnNombreRuta(Vble.CarpetaTemporal);
                        BaseADescargar = new SQLiteConnection("Data Source=" + Vble.CarpetaTemporal + ArchivoBase);
                        BaseADescargar.Open();

                    }
                    //BaseADescargar.Close();
                    ///Despues de haber descargado elimina los archivos del servidor
                    if (filesDownload == "SI")
                    {
                        string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
                        string RutaVerificar = ficFTP + localDescargar.Replace(" ", "") + "/" + Vble.Periodo.ToString().Replace("-", "") + "/" + CBRemesa.Text + "/" + fechaSeleccionado + "/" + cmbDevicesWifi.Text + "/";
                        Vble.EliminarArchivos(RutaVerificar);
                        //Vble.EliminarArchivoZIP(RutaVerificar + "/" + archivoZip);
                    }

                    PanelDescargaFTP.Visible = false;
                    //this.Cursor = Cursors.WaitCursor;
                    if (VerificarRegistrosEnBaseMysql() > 0)
                    {
                        backgroundDescargarAPC.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Disculpe no se va a poder realizar la descarga debido a que la Base de Datos General no se encuentra cargada.",
                                        "Descarga de Colectora", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
                else
                {
                    PanelDescargaFTP.Visible = false;
                    if (BaseADescargar.State == ConnectionState.Open)
                    {
                        BaseADescargar.Close();
                    }
                }
            }
            else
            {
                PanelDescargaFTP.Visible = false;
                MessageBox.Show("No existe ninguna ruta disponible de la colectora seleccionada en el servidor",
                                         "Descarga de Colectora", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

          

        }


        /// <summary>
        /// Verifica si hubo cambio de contraseña del lecturista, si hubo actualiza en la base de datos que se utiliza 
        /// cuando se generan las cargas, así cuando se vuelva a cargar alguna ruta, tome la base actualizada 
        /// con la contraseña cambiada 
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void VerificarCambioContraseña(string RutaRecibir)
        {
            DataTable Tabla = new DataTable();
            DataTable TablaAltas = new DataTable();
            string ContraseñaColectora, ContraseñaPC, PrivilegioPC, PrivilegioColectora, StringFechaCol, StringFechaPC;
            DateTime FechaClaveColectora, FechaClavePC;
            Int32 CodigoEnColectora = 0, CodigoEnPC = 0;
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source = " + RutaRecibir);
            DataTable LecturistasEnColectora = new DataTable();
            DataTable LecturistasEnPC = new DataTable();
            Int32 CodigoLec = 0;
            //double FechaColectora, FechaPC;
            try
            {
                string txSQL;
                SQLiteDataAdapter da;
                MySqlDataAdapter daMysql;
               
                //SQLiteConnection BaseChicaSQLitePC = new SQLiteConnection("Data Source = " + Vble.BaseChicaFISPC());
                //SQLiteConnection BaseChicaSQLitePC = new SQLiteConnection("Data Source = " + Ctte.CarpetaRecursos + "\\" + Vble.NombreArchivoBaseFijaSqlite());
                //MySqlConnection BaseChicaSQLitePC = new MySqlConnection("Data Source = " + Ctte.CarpetaRecursos + "\\" + Vble.NombreArchivoBaseFijaSqlite());
             

                BaseACargar.Open();

                //Obtengo la Fecha de modificación de clave que esta en la colectora para comparar con el de la PC
                txSQL = "SELECT * FROM Lecturistas"; // WHERE Codigo = " + Vble.Operario.ToString();
                da = new SQLiteDataAdapter(txSQL, BaseACargar);
                da.Fill(LecturistasEnColectora);
                //da.Parameters.AddWithValue("Clave", Vble.Operario.ToString());                
                //Contraseña = Convert.ToString((da.ExecuteScalar()));
                da.Dispose();
                

                //BaseChicaSQLitePC.Open();
                //Obtengo la Fecha de modificación de clave que esta en la Base SQLite para comparar con el de la colectora
                txSQL = "SELECT * FROM Lecturistas";  //WHERE Codigo = " + Vble.Operario.ToString();
                daMysql = new MySqlDataAdapter(txSQL, DB.conexBD);
                //da.Parameters.AddWithValue("Codigo", Vble.Operario.ToString());
                //FechaClavePC = Convert.ToDateTime(da.ExecuteScalar());
                daMysql.Fill(LecturistasEnPC);
                daMysql.Dispose();

                //BaseChicaSQLitePC.Close();

                if (LecturistasEnColectora.Rows.Count > 0 && LecturistasEnPC.Rows.Count > 0)
                {
                    foreach (DataRow Colectora in LecturistasEnColectora.Rows)
                    {
                        StringFechaCol = Colectora["FechaClave"].ToString().Replace('.', ' ');
                        StringFechaCol = StringFechaCol.Replace('a', ' ');
                        StringFechaCol = StringFechaCol.Replace('m', ' ');
                        StringFechaCol = StringFechaCol.Replace('p', ' ');
                        StringFechaCol = StringFechaCol.Trim(' ');
                                                                      

                        CodigoEnColectora = Convert.ToInt32(Colectora["Codigo"].ToString());
                        CodigoLec = CodigoEnColectora;
                        ContraseñaColectora = Colectora["Clave"].ToString();
                        FechaClaveColectora = Convert.ToDateTime(StringFechaCol);
                        PrivilegioColectora = Colectora["Privilegio"].ToString();

                        foreach (DataRow PC in LecturistasEnPC.Rows)
                        {

                            StringFechaPC = (PC["FechaClave"].ToString());
                            StringFechaPC = StringFechaPC.Replace('a', ' ');
                            StringFechaPC = StringFechaPC.Replace('m', ' ');
                            StringFechaPC = StringFechaPC.Replace('p', ' ');
                            StringFechaPC = StringFechaPC.Trim(' ');

                            CodigoEnPC = Convert.ToInt32(PC["Codigo"].ToString());
                            ContraseñaPC = PC["Clave"].ToString();
                            FechaClavePC = Convert.ToDateTime(StringFechaPC);


                            if (CodigoEnColectora == CodigoEnPC)
                            {
                                if (DateTime.Compare(FechaClaveColectora, FechaClavePC) > 0)
                                {
                                    string update;//Declaración de string que contendra la consulta UPDATE
                                    //BaseChicaSQLitePC.Open();
                                    update = "UPDATE Lecturistas SET Clave = '" + ContraseñaColectora + "', FechaClave = '" + FechaClaveColectora + "', " +
                                                                 " Privilegio = '" + PrivilegioColectora + "' WHERE Codigo = " + CodigoEnColectora;
                                    //preparamos la cadena pra insercion
                                    MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                                    //y la ejecutamos
                                    command.ExecuteNonQuery();
                                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                                    command.Dispose();
                                    //BaseChicaSQLitePC.Close();
                                }
                            }
                        }
                    }
                }

                if (BaseACargar.State == ConnectionState.Open)
                {
                    LecturistasEnColectora.Dispose();
                    LecturistasEnPC.Dispose();
                    daMysql.Dispose();
                    BaseACargar.Close();                   
                }
                
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al verificar cambio de contraseña del codigo de Lecturista: " + CodigoLec.ToString());
                if (BaseACargar.State == ConnectionState.Open)
                {
                    LecturistasEnColectora.Dispose();
                    LecturistasEnPC.Dispose();
                    
                    BaseACargar.Close();
                }
            }
        }

        /// <summary>
        /// Toma todos los concetos Facturados de la Base Sqlite que esta en la colectora y cargo a la Base General
        /// MySQL
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void DescargarTablaImpresor(string RutaRecibir)
        {
            try
            {
                ///Declaracion de variables 
                string txSQL, informa1, informa2, informa3, informa4, informa5, Nomb_Ape, Dom_Calle, Dom_Numero, Dom_Piso, Dom_Dpto,
                    Dom_Suministro, Dom_Localidad, Dom_Cod_Postal, Dom_CodLocalidad, Texto_Debito, Jubilado, Porcion, Un_Lectura, CUIT,
                    IVA, Tarifa, Medidor_Tipo, Medidor_Nro, Lec_Anterior, Lec_Actual, Lect_X, Fec_Anterior, Fec_Actual, Dias_Fact,
                    PeriodoFac, Anio, PeriodoB, Consumo, Fec_Emision, Prox_Vto, Estimado, Desconexion1, Total1, Fec_Vto1, Nro_Fact1,
                    Desconexion2, Total2, Fec_Vto2, Nro_Fact2, Adeudados, Text_Ctrol, LecAnt_TxCtl, Saldo_Favor,
                    Cod_Barra28_A, Cod_Barra60_A, Cod_Barra28_B, Cod_Barra60_B, CESP, CodQr, CodQrCont, CodQrb, CodQrbCont;
                        
                        
                Int32 ConexionID, Periodo, titularID, Contrato,  OpBel;

              
                SQLiteDataAdapter da;
                SQLiteCommandBuilder comandoSQL;

                DataTable TablaCFDistinct = new DataTable();
                DataTable TablaCFDistinctPeriodo = new DataTable();
                DataTable TablaCFaDescargar = new DataTable();
                SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseADescargar.Open();               
                ///----------------------------------------------------------------------------------------
                ///
                /// 
                ///Obtengo todas las conexiones/usuarios registrados en la tabla Impresor para descargar en la 
                ///base MySQL
                ///
                txSQL = "SELECT DISTINCT * FROM Impresor";
                SQLiteDataAdapter daD = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQLD = new SQLiteCommandBuilder(daD);
                daD.Fill(TablaCFDistinct);
                //daD.Dispose();
                comandoSQLD.Dispose();
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                string SelectPeriodo = "SELECT DISTINCT Periodo FROM Impresor";
                SQLiteDataAdapter daDPer = new SQLiteDataAdapter(SelectPeriodo, BaseADescargar);
                SQLiteCommandBuilder comandoSQLPer = new SQLiteCommandBuilder(daDPer);
                daDPer.Fill(TablaCFDistinctPeriodo);
                //daD.Dispose();
                comandoSQLPer.Dispose();
                daDPer.Dispose();

                string PeriodoImpresor = Vble.Periodo.ToString();

                if (TablaCFDistinctPeriodo.Rows.Count > 0)
                {
                    PeriodoImpresor = TablaCFDistinctPeriodo.Rows[0]["Periodo"].ToString();
                }

                //foreach (DataRow fi in TablaCFDistinctPeriodo.Rows)
                //{                    
                //    if (fi["Periodo"].ToString() != "")
                //    {
                //        PeriodoImpresor = fi["Periodo"].ToString();
                //    }
                //}

                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los registros por cada conexionID IMPRESO
                txSQL = "SELECT * FROM Impresor";
                da = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(da);

                da.Fill(TablaCFaDescargar);
                comandoSQL.Dispose();
               
                ////dataGridView2.DataSource = TablaCFaDescargar;
                /////----------------------------------------------------------------------------------------
                //if (TablaCFDistinct.Rows.Count > 0)
                //{
                //    CantCFDistinct = TablaCFDistinct.Rows.Count;
                //    foreach (var item in Vble.ArrayRutas)
                //    {
                //        if (ExisteEnImpresor(item, PeriodoImpresor) > 0)
                //        {
                //            string DeleteCF = "DELETE FROM Impresor where ConexionID in (SELECT ConexionID FROM Conexiones WHERE Ruta = " + item + 
                //                              " AND " + " Periodo = " + PeriodoImpresor + ")";
                //            MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                //            cmdSQL2.ExecuteNonQuery();
                //            cmdSQL2.Dispose();
                //        }
                //    }                    
                //    //foreach (DataRow fi in TablaCFDistinct.Rows)
                //    //{                      
                //    //    //Elimina Registros al Exportar de la tabla Personas
                //    //    string DeleteCF = "delete from Impresor " + 
                //    //                      "where (ConexionID = " + Convert.ToInt32(fi["ConexionID"]) + 
                //    //                      " AND " + "Periodo = " + Convert.ToInt32(fi["Periodo"]) + ")";
                //    //    MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                //    //    cmdSQL2.ExecuteNonQuery();
                //    //    cmdSQL2.Dispose();
                //    //    //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                //    //    //AvanceDescarga++;
                //    //}
                //}
                comandoSQLD.Dispose();

                //--------------------------------------------------------------------------------------------------------
                int contador = 0;
                string insertImpresor = "INSERT INTO Impresor " +
                            "(ConexionID, Periodo, titularID, Informa1, Informa2, Informa3, Informa4, " +
                            "Informa5, Contrato, Nomb_Ape, Dom_Calle, Dom_Numero, Dom_Piso, Dom_Dpto, " +
                            "Dom_Suministro, Dom_Localidad, Dom_Cod_Postal, Dom_CodLocalidad, Texto_Debito, " +
                            "Jubilado, Un_Lectura, Porcion, CUIT, IVA, Tarifa, Medidor_Tipo, Medidor_Nro, " +
                            "Lec_Anterior, Lec_Actual, Lect_X, Fec_Anterior, Fec_Actual, Dias_Fact, PeriodoFac, " +
                            "Anio, PeriodoB, Consumo, Fec_Emision, Prox_Vto, Estimado, Desconexion1, Total1, " +
                            "Fec_Vto1, Nro_Fact1, Desconexion2, Total2, Fec_Vto2, Nro_Fact2, Adeudados, Text_Ctrol, " +
                            "LecAnt_TxCtl, Saldo_Favor, Cod_Barra28_A, Cod_Barra60_A, Cod_Barra28_B, Cod_Barra60_B, " +
                            "CESP, OpBel, CodQr, CodQrCont, CodQrb, CodQrbCont ) " +
                            "VALUES ";
                
                foreach (DataRow fi in TablaCFaDescargar.Rows)
                //foreach (DataRow Fila in Tabla.Rows)
                {
                    contador++;
                    ConexionID = fi["ConexionID"].ToString() == "" ? 0 :  Convert.ToInt32(fi["ConexionID"]);
                    Periodo = fi["Periodo"].ToString() == "" ? 0 : Convert.ToInt32(fi["Periodo"]);
                    titularID = fi["titularID"].ToString() == "" ? 0 : Convert.ToInt32(fi["titularID"]);
                    informa1 = fi["Informa1"].ToString().Replace("'", "`");
                    informa2 = fi["Informa2"].ToString().Replace("'", "`");
                    informa3 = fi["Informa3"].ToString().Replace("'", "`");
                    informa4 = fi["Informa4"].ToString().Replace("'", "`");
                    informa5 = fi["Informa5"].ToString().Replace("'", "`");
                    Contrato = fi["Contrato"].ToString() == "" ? 0 : Convert.ToInt32(fi["Contrato"]);
                    Nomb_Ape = fi["Nomb_Ape"].ToString().Replace("'", "`");
                    Dom_Calle = fi["Dom_Calle"].ToString().Replace("'", "`");
                    Dom_Numero = string.IsNullOrEmpty(fi["Dom_Numero"].ToString()) ? "" : fi["Dom_Numero"].ToString();
                    Dom_Piso = string.IsNullOrEmpty(fi["Dom_Piso"].ToString()) ? "" : fi["Dom_Piso"].ToString();
                    Dom_Dpto = string.IsNullOrEmpty(fi["Dom_Dpto"].ToString()) ? "" : fi["Dom_Dpto"].ToString();
                    Dom_Suministro = fi["Dom_Suministro"].ToString().Replace("'", "`");
                    Dom_Localidad = fi["Dom_Localidad"].ToString().Replace("'", "`");
                    Dom_Cod_Postal = string.IsNullOrEmpty(fi["Dom_Cod_Postal"].ToString()) ? "" : fi["Dom_Cod_Postal"].ToString();
                    Dom_CodLocalidad = string.IsNullOrEmpty(fi["Dom_CodLocalidad"].ToString()) ? "" : fi["Dom_CodLocalidad"].ToString();
                    Texto_Debito = string.IsNullOrEmpty(fi["Texto_Debito"].ToString()) ? "" : fi["Texto_Debito"].ToString();
                    Jubilado = string.IsNullOrEmpty(fi["Jubilado"].ToString()) ? "" : fi["Jubilado"].ToString();
                    Un_Lectura = string.IsNullOrEmpty(fi["Un_Lectura"].ToString()) ? "" : fi["Un_Lectura"].ToString();
                    Porcion = string.IsNullOrEmpty(fi["Porcion"].ToString()) ? "" : fi["Porcion"].ToString();
                    CUIT = string.IsNullOrEmpty(fi["CUIT"].ToString()) ? "" : fi["CUIT"].ToString();
                    IVA = string.IsNullOrEmpty(fi["IVA"].ToString()) ? "" : fi["IVA"].ToString();
                    Tarifa = string.IsNullOrEmpty(fi["Tarifa"].ToString()) ? "" : fi["Tarifa"].ToString();
                    Medidor_Tipo = string.IsNullOrEmpty(fi["Medidor_Tipo"].ToString()) ? "" : fi["Medidor_Tipo"].ToString();
                    Medidor_Nro = string.IsNullOrEmpty(fi["Medidor_Nro"].ToString()) ? "" : fi["Medidor_Nro"].ToString();
                    Lec_Anterior = string.IsNullOrEmpty(fi["Lec_Anterior"].ToString()) ? "" : fi["Lec_Anterior"].ToString();
                    Lec_Actual = string.IsNullOrEmpty(fi["Lec_Actual"].ToString()) ? "" : fi["Lec_Actual"].ToString();
                    Lect_X = string.IsNullOrEmpty(fi["Lect_X"].ToString()) ? "" : fi["Lect_X"].ToString();
                    Fec_Anterior = string.IsNullOrEmpty(fi["Fec_Anterior"].ToString()) ? "" : fi["Fec_Anterior"].ToString();
                    Fec_Actual = string.IsNullOrEmpty(fi["Fec_Actual"].ToString()) ? "" : fi["Fec_Actual"].ToString();
                    Dias_Fact = string.IsNullOrEmpty(fi["Dias_Fact"].ToString()) ? "" : fi["Dias_Fact"].ToString();
                    PeriodoFac = string.IsNullOrEmpty(fi["PeriodoFac"].ToString()) ? "" : fi["PeriodoFac"].ToString();
                    Anio = string.IsNullOrEmpty(fi["Anio"].ToString()) ? "" : fi["Anio"].ToString();
                    PeriodoB = string.IsNullOrEmpty(fi["PeriodoB"].ToString()) ? "" : fi["PeriodoB"].ToString();
                    Consumo = string.IsNullOrEmpty(fi["Consumo"].ToString()) ? "" : fi["Consumo"].ToString();
                    Fec_Emision = string.IsNullOrEmpty(fi["Fec_Emision"].ToString()) ? "" : fi["Fec_Emision"].ToString();
                    Prox_Vto = string.IsNullOrEmpty(fi["Prox_Vto"].ToString()) ? "" : fi["Prox_Vto"].ToString();
                    Estimado = string.IsNullOrEmpty(fi["Estimado"].ToString()) ? "" : fi["Estimado"].ToString();
                    Desconexion1 = string.IsNullOrEmpty(fi["Desconexion1"].ToString()) ? "" : fi["Desconexion1"].ToString();
                    Total1 = string.IsNullOrEmpty(fi["Total1"].ToString()) ? "" : fi["Total1"].ToString();
                    Fec_Vto1 = string.IsNullOrEmpty(fi["Fec_Vto1"].ToString()) ? "" : fi["Fec_Vto1"].ToString();
                    Nro_Fact1 = string.IsNullOrEmpty(fi["Nro_Fact1"].ToString()) ? "" : fi["Nro_Fact1"].ToString();
                    Desconexion2 = string.IsNullOrEmpty(fi["Desconexion2"].ToString()) ? "" : fi["Desconexion2"].ToString();
                    Total2 = string.IsNullOrEmpty(fi["Total2"].ToString()) ? "" : fi["Total2"].ToString();
                    Fec_Vto2 = string.IsNullOrEmpty(fi["Fec_Vto2"].ToString()) ? "" : fi["Fec_Vto2"].ToString();
                    Nro_Fact2 = string.IsNullOrEmpty(fi["Nro_Fact2"].ToString()) ? "" : fi["Nro_Fact2"].ToString();
                    Adeudados = string.IsNullOrEmpty(fi["Adeudados"].ToString()) ? "" : fi["Adeudados"].ToString();
                    Text_Ctrol = string.IsNullOrEmpty(fi["Text_Ctrol"].ToString()) ? "" : fi["Text_Ctrol"].ToString();
                    LecAnt_TxCtl = string.IsNullOrEmpty(fi["LecAnt_TxCtl"].ToString()) ? "" : fi["LecAnt_TxCtl"].ToString();
                    Saldo_Favor = string.IsNullOrEmpty(fi["Saldo_Favor"].ToString()) ? "" : fi["Saldo_Favor"].ToString();
                    Cod_Barra28_A = string.IsNullOrEmpty(fi["Cod_Barra28_A"].ToString())? "": fi["Cod_Barra28_A"].ToString();
                    Cod_Barra60_A = string.IsNullOrEmpty(fi["Cod_Barra60_A"].ToString()) ? "" : fi["Cod_Barra60_A"].ToString();
                    Cod_Barra28_B = string.IsNullOrEmpty(fi["Cod_Barra28_B"].ToString()) ? "" : fi["Cod_Barra28_B"].ToString();
                    Cod_Barra60_B = string.IsNullOrEmpty(fi["Cod_Barra60_B"].ToString()) ? "" : fi["Cod_Barra60_B"].ToString();
                    CESP = fi["CESP"].ToString();
                    OpBel = fi["OpBel"].ToString() == "" ? 0 : Convert.ToInt32(fi["OpBel"]);    
                    CodQr = string.IsNullOrEmpty(fi["CodQr"].ToString()) ? "-" : fi["CodQr"].ToString();
                    CodQrCont = string.IsNullOrEmpty(fi["CodQrCont"].ToString()) ? "-" : fi["CodQrCont"].ToString();
                    CodQrb = string.IsNullOrEmpty(fi["CodQrb"].ToString()) ? "-" : fi["CodQrb"].ToString();
                    CodQrbCont = string.IsNullOrEmpty(fi["CodQrbCont"].ToString()) ? "-" : fi["CodQrbCont"].ToString();

                    if (contador == 499)
                    {
                        insertImpresor += "(" + ConexionID + ", " + Periodo + ", " + titularID + ", '" + informa1 + "', '" +
                            informa2 + "', '" + informa3 + "', '" + informa4 + "', '" + informa5 + "', " +
                            Contrato + ", '" + Nomb_Ape + "', '" + Dom_Calle + "', '" + Dom_Numero + "', '" +
                            Dom_Piso + "', '" + Dom_Dpto + "', '" + Dom_Suministro + "', '" + Dom_Localidad + "', '" + Dom_Cod_Postal + "', '" +
                            Dom_CodLocalidad + "', '" + Texto_Debito + "', '" + Jubilado + "', '" + Un_Lectura + "', '" + Porcion + "', '" +
                            CUIT + "', '" + IVA + "', '" + Tarifa + "', '" + Medidor_Tipo + "',  '" + Medidor_Nro + "', '" + Lec_Anterior + "', '" +
                            Lec_Actual + "', '" + Lect_X + "', '" + Fec_Anterior + "', '" + Fec_Actual + "', '" + Dias_Fact + "', '" +
                            PeriodoFac + "', '" + Anio + "', '" + PeriodoB + "', '" + Consumo + "', '" + Fec_Emision + "', '" + Prox_Vto + "', '" +
                            Estimado + "', '" + Desconexion1 + "', '" + Total1 + "', '" + Fec_Vto1 + "', '" + Nro_Fact1 + "', '" + Desconexion2 + "', '" +
                            Total2 + "', '" + Fec_Vto2 + "', '" + Nro_Fact2 + "', '" + Adeudados + "', '" + Text_Ctrol + "', '" + LecAnt_TxCtl + "', '" +
                            Saldo_Favor + "', '" + Cod_Barra28_A + "', '" + Cod_Barra28_B + "', '" + Cod_Barra60_A + "', '" + Cod_Barra60_B + "', '" +
                            CESP + "', " + OpBel + ", '" + CodQr + "', '" + CodQrCont + "', '" + CodQrb + "', '"  + CodQrbCont + "')";

                        //preparamos la cadena pra insercion
                        MySqlCommand command = new MySqlCommand(insertImpresor, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();                        
                        insertImpresor = "INSERT INTO  Impresor " +
                             "(ConexionID, Periodo, titularID, Informa1, Informa2, Informa3, Informa4, " +
                             "Informa5, Contrato, Nomb_Ape, Dom_Calle, Dom_Numero, Dom_Piso, Dom_Dpto, " +
                             "Dom_Suministro, Dom_Localidad, Dom_Cod_Postal, Dom_CodLocalidad, Texto_Debito, " +
                             "Jubilado, Un_Lectura, Porcion, CUIT, IVA, Tarifa, Medidor_Tipo, Medidor_Nro, " +
                             "Lec_Anterior, Lec_Actual, Lect_X, Fec_Anterior, Fec_Actual, Dias_Fact, PeriodoFac, " +
                             "Anio, PeriodoB, Consumo, Fec_Emision, Prox_Vto, Estimado, Desconexion1, Total1, " +
                             "Fec_Vto1, Nro_Fact1, Desconexion2, Total2, Fec_Vto2, Nro_Fact2, Adeudados, Text_Ctrol, " +
                             "LecAnt_TxCtl, Saldo_Favor, Cod_Barra28_A, Cod_Barra60_A, Cod_Barra28_B, Cod_Barra60_B, " +
                             "CESP, OpBel, CodQr, CodQrCont, CodQrb, CodQrbCont ) " +
                             "VALUES ";
                    }
                    else if (contador == TablaCFaDescargar.Rows.Count)
                    {
                        insertImpresor += "(" + ConexionID + ", " + Periodo + ", " + titularID + ", '" + informa1 + "', '" +
                            informa2 + "', '" + informa3 + "', '" + informa4 + "', '" + informa5 + "', " +
                            Contrato + ", '" + Nomb_Ape + "', '" + Dom_Calle + "', '" + Dom_Numero + "', '" +
                            Dom_Piso + "', '" + Dom_Dpto + "', '" + Dom_Suministro + "', '" + Dom_Localidad + "', '" + Dom_Cod_Postal + "', '" +
                            Dom_CodLocalidad + "', '" + Texto_Debito + "', '" + Jubilado + "', '" + Un_Lectura + "', '" + Porcion + "', '" +
                            CUIT + "', '" + IVA + "', '" + Tarifa + "', '" + Medidor_Tipo + "',  '" + Medidor_Nro + "', '" + Lec_Anterior + "', '" +
                            Lec_Actual + "', '" + Lect_X + "', '" + Fec_Anterior + "', '" + Fec_Actual + "', '" + Dias_Fact + "', '" +
                            PeriodoFac + "', '" + Anio + "', '" + PeriodoB + "', '" + Consumo + "', '" + Fec_Emision + "', '" + Prox_Vto + "', '" +
                            Estimado + "', '" + Desconexion1 + "', '" + Total1 + "', '" + Fec_Vto1 + "', '" + Nro_Fact1 + "', '" + Desconexion2 + "', '" +
                            Total2 + "', '" + Fec_Vto2 + "', '" + Nro_Fact2 + "', '" + Adeudados + "', '" + Text_Ctrol + "', '" + LecAnt_TxCtl + "', '" +
                            Saldo_Favor + "', '" + Cod_Barra28_A + "', '" + Cod_Barra28_B + "', '" + Cod_Barra60_A + "', '" + Cod_Barra60_B + "', '" +
                            CESP + "', " + OpBel + ", '" + CodQr + "', '" + CodQrCont + "', '" + CodQrb + "', '"  + CodQrbCont + "')";
                    }
                    else if (contador < TablaCFaDescargar.Rows.Count)
                    {
                        insertImpresor += "(" + ConexionID + ", " + Periodo + ", " + titularID + ", '" + informa1 + "', '" +
                             informa2 + "', '" + informa3 + "', '" + informa4 + "', '" + informa5 + "', " +
                             Contrato + ", '" + Nomb_Ape + "', '" + Dom_Calle + "', '" + Dom_Numero + "', '" +
                             Dom_Piso + "', '" + Dom_Dpto + "', '" + Dom_Suministro + "', '" + Dom_Localidad + "', '" + Dom_Cod_Postal + "', '" +
                             Dom_CodLocalidad + "', '" + Texto_Debito + "', '" + Jubilado + "', '" + Un_Lectura + "', '" + Porcion + "', '" +
                             CUIT + "', '" + IVA + "', '" + Tarifa + "', '" + Medidor_Tipo + "',  '" + Medidor_Nro + "', '" + Lec_Anterior + "', '" +
                             Lec_Actual + "', '" + Lect_X + "', '" + Fec_Anterior + "', '" + Fec_Actual + "', '" + Dias_Fact + "', '" +
                             PeriodoFac + "', '" + Anio + "', '" + PeriodoB + "', '" + Consumo + "', '" + Fec_Emision + "', '" + Prox_Vto + "', '" +
                             Estimado + "', '" + Desconexion1 + "', '" + Total1 + "', '" + Fec_Vto1 + "', '" + Nro_Fact1 + "', '" + Desconexion2 + "', '" +
                             Total2 + "', '" + Fec_Vto2 + "', '" + Nro_Fact2 + "', '" + Adeudados + "', '" + Text_Ctrol + "', '" + LecAnt_TxCtl + "', '" +
                             Saldo_Favor + "', '" + Cod_Barra28_A + "', '" + Cod_Barra28_B + "', '" + Cod_Barra60_A + "', '" + Cod_Barra60_B + "', '" +
                             CESP + "', " + OpBel + ", '" + CodQr + "', '" + CodQrCont + "', '" + CodQrb + "', '"  + CodQrbCont + "'), ";
                    }
                    //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                    //AvanceDescarga++;
                }
                //preparamos la cadena pra insercion                
                MySqlCommand command2 = new MySqlCommand(insertImpresor, DB.conexBD);
                //y la ejecutamos              
                command2.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command2.Dispose();      
                comandoSQL.Dispose();
                da.Dispose();
                daD.Dispose();
                //comandoSQLD.Dispose();
                BaseADescargar.Close();
                //_-----------------------------------------------------------------------------------------------------   

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Cargar la tabla Impresor");
                BaseADescargar.Close();
                //DB.conexBD.Dispose();
            }

        }

        
        private int ExisteEnImpresor(object Ruta, string Periodo)
        {
           

            string selectCount = "SELECT Count(*) FROM Impresor where ConexionID in " +
                                    "(SELECT ConexionID FROM Conexiones WHERE Ruta = " + Ruta.ToString() + " AND Periodo = " + Periodo + ") " +
                                 "AND Periodo = " + Periodo;

            
            MySqlCommand da = new MySqlCommand(selectCount, DB.conexBD);
            da.Dispose();
            int count = Convert.ToInt32(da.ExecuteScalar());

            if (count == 0)
                return count;
            else
                return count;

            

        }


        /// <summary>
        /// Descarga todas las facturas registradas a la Base General
        /// MySQL
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void DescargarTablaFacturas(string RutaRecibir)
        {
            try
            {
                ///Declaracion de variables 
                string txSQL, Detalle, Importe;
                Int32 ConexionID, Periodo, Renglon, Resaltar;
                

                SQLiteDataAdapter da;
                SQLiteCommandBuilder comandoSQL;

                DataTable TablaCFDistinct = new DataTable();
                DataTable TablaCFaDescargar = new DataTable();
                SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseADescargar.Open();
                ///----------------------------------------------------------------------------------------
                ///
                /// 
                ///Obtengo todas las conexiones/usuarios registrados en la tabla Impresor para descargar en la 
                ///base MySQL
                ///
                txSQL = "SELECT DISTINCT * FROM Facturas";
                SQLiteDataAdapter daD = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQLD = new SQLiteCommandBuilder(daD);
                daD.Fill(TablaCFDistinct);
                //daD.Dispose();
                comandoSQLD.Dispose();
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los conceptos facturados por cada conexionID 
                txSQL = "SELECT * FROM Facturas";
                da = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(da);
                da.Fill(TablaCFaDescargar);
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                if (TablaCFDistinct.Rows.Count > 0)
                {
                    CantCFDistinct = TablaCFDistinct.Rows.Count;
                    foreach (DataRow fi in TablaCFDistinct.Rows)
                    {
                        //Elimina Registros al Exportar de la tabla Personas
                        string DeleteCF = "delete from Facturas " +
                                          "where (ConexionID = " + Convert.ToInt32(fi["ConexionID"]) +
                                          " AND " + "Periodo = " + Convert.ToInt32(fi["Periodo"]) + ")";
                        MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                        cmdSQL2.ExecuteNonQuery();
                        cmdSQL2.Dispose();
                        //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                        //AvanceDescarga++;
                    }
                    comandoSQLD.Dispose();
                }
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                int contador = 0;
                string insertFacturas = "INSERT INTO Facturas " +
                            "(ConexionID, Periodo, Renglon, Detalle, Importe, Resaltar) " +
                            "VALUES ";

                foreach (DataRow fi in TablaCFaDescargar.Rows)
                //foreach (DataRow Fila in Tabla.Rows)
                {
                    contador++;

                    ConexionID = Convert.ToInt32(fi["ConexionID"]);
                    Periodo = Convert.ToInt32(fi["Periodo"]);
                    Renglon = Convert.ToInt32(fi["Renglon"]);
                    Detalle = fi["Detalle"].ToString();
                    Importe = fi["Importe"].ToString();
                    Importe = Importe.Replace(".", "");
                    Importe = Importe.Replace(",", ".");
                    Resaltar = Convert.ToInt32(fi["Resaltar"]);

                    if (contador == 499)
                    {
                        insertFacturas += "(" + ConexionID + ", " + Periodo + ", " + Renglon + ", '" + Detalle + "', " + Importe + ", " +
                            Resaltar + ")";

                        //preparamos la cadena pra insercion
                        MySqlCommand command = new MySqlCommand(insertFacturas, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                        insertFacturas = "INSERT INTO Facturas " +
                            "(ConexionID, Periodo, Renglon, Detalle, Importe, Resaltar) " +
                            "VALUES ";
                    }
                    else if (contador == TablaCFaDescargar.Rows.Count)
                    {

                        insertFacturas += "(" + ConexionID + ", " + Periodo + ", " + Renglon + ", '" + Detalle + "', " + Importe + ", " +
                            Resaltar + ")";

                    }
                    else if (contador < TablaCFaDescargar.Rows.Count)
                    {
                        insertFacturas += "(" + ConexionID + ", " + Periodo + ", " + Renglon + ", '" + Detalle + "', " + Importe + ", " +
                            Resaltar + "), ";
                    }


                    backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                    AvanceDescarga++;
                }
                //preparamos la cadena pra insercion
                MySqlCommand command2 = new MySqlCommand(insertFacturas, DB.conexBD);
                //y la ejecutamos
                command2.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command2.Dispose();


                comandoSQL.Dispose();
                da.Dispose();
                daD.Dispose();
                //comandoSQLD.Dispose();
                BaseADescargar.Close();
                //---------------------------------------------------------------------------------------------------------
                                             
                /////Recorrido de Tabla Sqlite(Colectora) y carga de datos a tabla MySQL(NAS)            
                //if (TablaCFaDescargar.Rows.Count > 0)
                //{
                //    foreach (DataRow fi in TablaCFaDescargar.Rows)
                //    {
                //        ConexionID = Convert.ToInt32(fi["ConexionID"]);
                //        Periodo = Convert.ToInt32(fi["Periodo"]);
                //        Renglon = Convert.ToInt32(fi["Renglon"]);
                //        Detalle = fi["Detalle"].ToString();
                //        Importe = fi["Importe"].ToString();
                //        Importe = Importe.Replace(".", "");
                //        Importe = Importe.Replace(",", ".");
                //        Resaltar = Convert.ToInt32(fi["Resaltar"]);

                //        ////if ((VerificarExistenciaRegistro("conceptosfacturados", "ConexionID", conexionID, Periodo, Orden) == 0))
                //        ////{
                //        string insert = "";//Declaración de string que contendra la consulta INSERT
                //        insert = "INSERT INTO Facturas " +
                //            "(ConexionID, Periodo, Renglon, Detalle, Importe, Resaltar) " +
                //            "VALUES " +
                //            "(" + ConexionID + ", " + Periodo + ", " + Renglon + ", '" + Detalle + "', " + Importe + ", " + 
                //            Resaltar + ")";

                //        //preparamos la cadena pra insercion
                //        MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                //        //y la ejecutamos
                //        command.ExecuteNonQuery();
                //        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                //        command.Dispose();
                //        //}
                //        backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                //        AvanceDescarga++;
                //    }
                //}
                //comandoSQL.Dispose();
                //da.Dispose();
                //daD.Dispose();
                ////comandoSQLD.Dispose();
                //BaseADescargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Cargar la tabla Facturas");
            }

        }


        /// <summary>
        /// Descarga todas las facturas registradas a la Base General CON Nueva estructura
        /// MySQL
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void DescargarTablaFacturasBIS(string RutaRecibir)
        {
            try
            {
                ///Declaracion de variables 
                string txSQL;
                Int32 ConexionID, Periodo;

                var lstItemFac = new List<List<string>>();


                SQLiteDataAdapter da;
                SQLiteCommandBuilder comandoSQL;

                DataTable TablaFDistinct = new DataTable();
                DataTable TablaFDescargarDistinct = new DataTable();
                DataTable TablaCFDistinctPeriodo = new DataTable();
                DataTable tablaFac = new DataTable();
                SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseADescargar.Open();
                ///----------------------------------------------------------------------------------------
                ///
                /// 
                ///Obtengo todas las conexiones/usuarios registrados en la tabla Impresor para descargar en la 
                ///base MySQL
                ///
                txSQL = "SELECT DISTINCT ConexionID, Periodo FROM Facturas";
                SQLiteDataAdapter daD = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQLD = new SQLiteCommandBuilder(daD);
                daD.Fill(TablaFDistinct);
               
                //comandoSQLD.Dispose();
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los conceptos facturados por cada conexionID 
                txSQL = "SELECT DISTINCT ConexionID, Periodo FROM Facturas";
                da = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(da);
                da.Fill(TablaFDescargarDistinct);
                comandoSQL.Dispose();
                da.Dispose();



                string SelectPeriodo = "SELECT DISTINCT Periodo FROM Impresor";
                SQLiteDataAdapter daDPer = new SQLiteDataAdapter(SelectPeriodo, BaseADescargar);
                SQLiteCommandBuilder comandoSQLPer = new SQLiteCommandBuilder(daDPer);
                daDPer.Fill(TablaCFDistinctPeriodo);
                //daD.Dispose();
                comandoSQLPer.Dispose();
                daDPer.Dispose();

                string PeriodoImpresor = Vble.Periodo.ToString();

                foreach (DataRow fi in TablaFDescargarDistinct.Rows)
                {
                    if (fi["Periodo"].ToString() != "")
                    {
                        PeriodoImpresor = fi["Periodo"].ToString();
                    }
                }

                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------
                ///
                //Elimina Registros al Exportar de la tabla Personas
                string DeleteCF = "delete from FacturasBIS " +
                                          "where (ConexionID = 0 " +
                                          " AND " + "Periodo = 0) ";

                if (TablaFDistinct.Rows.Count > 0)
                {
                    CantCFDistinct = TablaFDistinct.Rows.Count;


                    foreach (var item in Vble.ArrayRutas)
                    {
                        if (ExisteEnImpresor(item, PeriodoImpresor) > 0)
                        {
                            foreach (DataRow fi in TablaFDistinct.Rows)
                            {
                                DeleteCF += $"OR (ConexionID = {Convert.ToInt32(fi["ConexionID"])} AND Periodo = {Convert.ToInt32(fi["Periodo"])}) ";
                            }

                            ////Elimina Registros al Exportar de la tabla Personas
                            //string DeleteCF = "delete from FacturasBIS " +
                            //                  "where (ConexionID = " + Convert.ToInt32(fi["ConexionID"]) +
                            //                  " AND " + "Periodo = " + Convert.ToInt32(fi["Periodo"]) + ")";
                            MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                            cmdSQL2.CommandTimeout = 900;
                            cmdSQL2.ExecuteNonQuery();
                            cmdSQL2.Dispose();
                            //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                            //AvanceDescarga++;


                        }
                    }
                    comandoSQLD.Dispose();
                }
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                int contador = 0;
                string insertFacturas = "INSERT INTO FacturasBIS " +
                            "(ConexionID, Periodo, Grupo, " +
                            "Detalle1, Importe1, Resaltar1, " +
                            "Detalle2, Importe2, Resaltar2, " +
                            "Detalle3, Importe3, Resaltar3, " +
                            "Detalle4, Importe4, Resaltar4, " +
                            "Detalle5, Importe5, Resaltar5, " +
                            "Detalle6, Importe6, Resaltar6, " +
                            "Detalle7, Importe7, Resaltar7, " +
                            "Detalle8, Importe8, Resaltar8, " +
                            "Detalle9, Importe9, Resaltar9, " +
                            "Detalle10, Importe10, Resaltar10) " +
                            "VALUES ";

                List<string> ListaRegisros = new List<string>();
                int t = 0;


                foreach (DataRow fi in TablaFDescargarDistinct.Rows)
                //foreach (DataRow Fila in Tabla.Rows)
                {
                    contador++;
                    ConexionID = Convert.ToInt32(fi["ConexionID"]);
                    Periodo = Convert.ToInt32(fi["Periodo"]);
                    tablaFac.Clear();
                    string txSQLReg = "SELECT * FROM Facturas WHERE ConexionID = " + ConexionID;
                    SQLiteDataAdapter daReg = new SQLiteDataAdapter(txSQLReg, BaseADescargar);
                    SQLiteCommandBuilder comandoSQLReg = new SQLiteCommandBuilder(daReg);
                    daReg.Fill(tablaFac);                    
                    comandoSQLReg.Dispose();
                    daReg.Dispose();
                    int group = 0;                    

                    //foreach (DataRow item in tablaFac.Rows)
                    //{
                        int i = 0;
                        while (i*10 < tablaFac.Rows.Count)
                        {
                            lstItemFac = new List<List<string>>();
                            for (int j =0; j <10; j++)
                                {
                                if (j+i*10 < tablaFac.Rows.Count)
                                {
                                lstItemFac.Add(new List<string>()
                                                                    {tablaFac.Rows[j+i*10]["Detalle"].ToString(),
                                                                     tablaFac.Rows[j+i*10]["Importe"].ToString(),
                                                                     tablaFac.Rows[j+i*10]["Resaltar"].ToString()});
                                //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                                //AvanceDescarga++;
                            }
                                else
                                {
                                    lstItemFac.Add(new List<string>() { "", "", "0" });
                                }
                            }
                            group = i;
                            i++;
                            //Armar metodo del registro (i+1)= grupo
                            string s10ItemsFact = $"({ConexionID}, {Periodo}," +
                                                      $"{group.ToString()}";
                            for (int l = 0; l < lstItemFac.Count; l++)
                            {
                                s10ItemsFact += $",'{lstItemFac[l][0]}', '{lstItemFac[l][1]}', {lstItemFac[l][2]}";
                            }
                            s10ItemsFact += ")";
                            ListaRegisros.Add(s10ItemsFact);

                        }
                }
                    
                    float Prom = lstItemFac.Count / ListaRegisros.Count;
                string sep = "";
                MySqlCommand command2 = new MySqlCommand();
                int CantImpor = 0;
                int Importadas = 0;
                int k = 499;
                int y = 0;

                for (y = Importadas; Importadas < ListaRegisros.Count; y = y + k)
                {
                    var insertTemp = insertFacturas;
                    for (int j = 0; j < k & j < ListaRegisros.Count; j++)
                    {
                        if (Importadas == ListaRegisros.Count)
                        {

                        }
                        else
                        {
                            insertTemp += $"{sep}{ListaRegisros[Importadas]}";
                            sep = ",";
                            CantImpor = j;
                            Importadas++;
                        }

                    }
                    //CantImpor = (int)(CantImpor * Prom);                 
                    ////preparamos la cadena pra insercion
                    //command2 = new MySqlCommand(insertTemp, DB.conexBD);
                    ////y la ejecutamos
                    //command2.ExecuteNonQuery();
                    ////finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    //command2.Dispose();
                    //AvanceDescarga = AvanceDescarga + CantImpor;
                    //backgroundDescargarAPC.ReportProgress(AvanceDescarga);

                    CantImpor = (int)(CantImpor * Prom);
                    //preparamos la cadena pra insercion
                    command2 = new MySqlCommand(insertTemp, DB.conexBD);

                    command2.CommandTimeout = 900;
                    //y la ejecutamos
                    command2.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command2.Dispose();
                    sep = "";

                }
                daD.Dispose();
                comandoSQLD.Dispose();
                BaseADescargar.Close();
                    //---------------------------------------------------------------------------------------------------------
                
            }
            catch (Exception r)
            {
                BaseADescargar.Close();
                MessageBox.Show(r.Message + " Error al Cargar la tabla Facturas, presione aceptar para continuar, si el error persiste cierre y vuelva a abrir la aplicacion");

            }

        }

        /// <summary>
        /// Descarga todas las facturas registradas a la Base General
        /// MySQL
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void DescargarTablaLogErrores(string RutaRecibir)
        {
            try
            {
                ///Declaracion de variables 
                string txSQL, Entorno, Equipo, Fecha, Hora, CodigoError, TextoError, Mensaje;
                Int32 ConexionID, Periodo, Ruta, Lecturista;

                SQLiteDataAdapter da;
                SQLiteCommandBuilder comandoSQL;

                DataTable TablaCLogErroresDistinct = new DataTable();
                DataTable TablaLogErroresADescargar = new DataTable();
                SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                BaseADescargar.Open();
                ///----------------------------------------------------------------------------------------
                ///
                /// 
                ///Obtengo todas las conexiones/usuarios registrados en la tabla LogErrores para descargar en la 
                ///base MySQL
                ///
                txSQL = "SELECT DISTINCT * FROM LogErrores";
                SQLiteDataAdapter daD = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQLD = new SQLiteCommandBuilder(daD);
                daD.Fill(TablaCLogErroresDistinct);
                //daD.Dispose();
                comandoSQLD.Dispose();
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los conceptos facturados por cada conexionID 
                txSQL = "SELECT * FROM LogErrores";
                da = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(da);
                da.Fill(TablaLogErroresADescargar);
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                if (TablaCLogErroresDistinct.Rows.Count > 0)
                {                     
                    foreach (DataRow fi in TablaCLogErroresDistinct.Rows)
                    {
                        //Elimina Registros al Exportar de la tabla Personas
                        string DeleteCF = "delete from LogErrores " +
                                          "where (ConexionID = " + Convert.ToInt32(fi["ConexionID"]) +
                                          " AND " + "Periodo = " + Convert.ToInt32(fi["Periodo"]) + ")";
                        MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                        cmdSQL2.ExecuteNonQuery();
                        cmdSQL2.Dispose();
                        //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                        //AvanceDescarga++;
                    }
                    comandoSQLD.Dispose();
                }
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                int contador = 0;
                string insertLogErrores = "INSERT INTO LogErrores " +
                            "(ConexionID, Periodo, Entorno, Equipo, Fecha, Hora, Ruta, Lecturista, CodigoError, TextoError, Mensaje) " +
                            "VALUES ";

                string Fecha_ddMMyyyy = "";
                

                foreach (DataRow fi in TablaLogErroresADescargar.Rows)
                //foreach (DataRow Fila in Tabla.Rows)
                {
                    contador++;

                    ConexionID = Convert.ToInt32(fi["ConexionID"]);
                    Periodo = Convert.ToInt32(fi["Periodo"]);
                    Entorno = fi["Entorno"].ToString();
                    Equipo = fi["Equipo"].ToString();

                    Fecha_ddMMyyyy = fi["Fecha"].ToString();

                    if (Fecha_ddMMyyyy != "")
                    {  
                        Fecha = Fecha_ddMMyyyy.Substring(6) + "-" + Fecha_ddMMyyyy.Substring(3, 2) + "-" + Fecha_ddMMyyyy.Substring(0, 2);
                    }
                    else
                    {
                        Fecha = "2000-01-01";
                    }                    
                    //Fecha = fi["Fecha"].ToString();
                    Hora = fi["Hora"].ToString();
                    Ruta = Convert.ToInt32(fi["Ruta"].ToString());
                    Lecturista = Convert.ToInt32(fi["Lecturista"].ToString());
                    CodigoError = fi["CodigoError"].ToString().Replace("'", "`");
                    TextoError = fi["TextoError"].ToString().Replace("'", "`");
                    Mensaje = fi["Mensaje"].ToString().Replace("'", "`");

                    if (contador == 499)
                    {
                        insertLogErrores += "(" + ConexionID + ", " + Periodo + ", '" + Entorno + "', '" + Equipo + "', '" + Fecha + "', '" +
                            Hora + "', " + Ruta + ", " + Lecturista + ", '" + CodigoError + "', '" + TextoError + "', '" + Mensaje +"')";

                        //preparamos la cadena pra insercion
                        MySqlCommand command = new MySqlCommand(insertLogErrores, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                        insertLogErrores = "INSERT INTO LogErrores " +
                            "(ConexionID, Periodo, Entorno, Equipo, Fecha, Hora, Ruta, Lecturista, CodigoError, TextoError, Mensaje) " +
                            "VALUES ";
                    }
                    else if (contador == TablaLogErroresADescargar.Rows.Count)
                    {

                        insertLogErrores += "(" + ConexionID + ", " + Periodo + ", '" + Entorno + "', '" + Equipo + "', '" + Fecha + "', '" +
                            Hora + "', " + Ruta + ", " + Lecturista + ", '" + CodigoError + "', '" + TextoError + "', '" + Mensaje + "')";

                    }
                    else if (contador < TablaLogErroresADescargar.Rows.Count)
                    {
                        insertLogErrores += "(" + ConexionID + ", " + Periodo + ", '" + Entorno + "', '" + Equipo + "', '" + Fecha + "', '" +
                            Hora + "', " + Ruta + ", " + Lecturista + ", '" + CodigoError + "', '" + TextoError + "', '" + Mensaje + "'), ";
                    }

                    //backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                    //AvanceDescarga++;
                }
                //preparamos la cadena pra insercion
                MySqlCommand command2 = new MySqlCommand(insertLogErrores, DB.conexBD);
                //y la ejecutamos
                command2.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command2.Dispose();

                comandoSQL.Dispose();
                da.Dispose();
                daD.Dispose();
                //comandoSQLD.Dispose();
                //BaseADescargar.Close();
               
            }
            catch (Exception r)
            {
                BaseADescargar.Close();
                MessageBox.Show(r.Message + " Error al Cargar la tabla Log Errores, presione aceptar para continuar, si el error persiste cierre y vuelva a abrir la aplicacion");
            }

        }

        /// <summary>
        /// Toma todos los concetos Facturados de la Base Sqlite que esta en la colectora y cargo a la Base General
        /// MySQL
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void CargarTablaConceptosFacturados(string RutaRecibir)
        {
            try
            {            
            ///Declaracion de variables 
            string txSQL, CodigoDpec, TextoDescripcion, Agrupador, Resaltar, ImprimeSubtotal;
            Int32 conexionID, Periodo, Orden, CodigoConcepto, CodigoEscalon, CodigoAux, CodigoGrupo;
            double Cantidad, Unitario, Importe, ImportePA;
            SQLiteDataAdapter da;
            SQLiteCommandBuilder comandoSQL;

                DataTable TablaCFDistinct = new DataTable();
                DataTable TablaCFaDescargar = new DataTable();
            SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaRecibir);
            BaseADescargar.Open();
                //int i = 0;

                ///----------------------------------------------------------------------------------------
                ///
                /// 
                ///Obtengo conceptos facturados uno solo registro por cada conexionID para utilizarlo en la consulta delete antes de insertarlo nuevamente
                ///
                txSQL = "SELECT DISTINCT conexionID, Periodo FROM ConceptosFacturados";
                SQLiteDataAdapter daD = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQLD = new SQLiteCommandBuilder(daD);
                daD.Fill(TablaCFDistinct);
                //daD.Dispose();
                comandoSQLD.Dispose();
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------

                ///----------------------------------------------------------------------------------------
                ///Obtengo todos los conceptos facturados por cada conexionID 
                txSQL = "SELECT * FROM ConceptosFacturados";
                da = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(da);
                da.Fill(TablaCFaDescargar);
                //dataGridView2.DataSource = TablaCFaDescargar;
                ///----------------------------------------------------------------------------------------


                if (TablaCFDistinct.Rows.Count > 0)
                {
                    CantCFDistinct = TablaCFDistinct.Rows.Count;
                    foreach (DataRow fi in TablaCFDistinct.Rows)
                    {
                        //Elimina Registros al Exportar de la tabla Personas
                        string DeleteCF = "delete from conceptosfacturados where (conexionID = " + Convert.ToInt32(fi["conexionID"]) + " AND " + "Periodo = " + Convert.ToInt32(fi["Periodo"]) + ")";
                        MySqlCommand cmdSQL2 = new MySqlCommand(DeleteCF, DB.conexBD);
                        cmdSQL2.ExecuteNonQuery();
                        cmdSQL2.Dispose();
                        backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                        AvanceDescarga++;
                    }
                    comandoSQLD.Dispose();
                   

                }

            ///Recorrido de Tabla Sqlite(Colectora) y carga de datos a tabla MySQL(NAS)            
            if (TablaCFaDescargar.Rows.Count > 0)
            {           
                foreach (DataRow fi in TablaCFaDescargar.Rows)
                {
                    conexionID = Convert.ToInt32(fi["conexionID"]);
                    Periodo = Convert.ToInt32(fi["Periodo"]);
                    Orden = Convert.ToInt32(fi["Orden"]);
                    CodigoConcepto = Convert.ToInt32(fi["CodigoConcepto"]);
                    CodigoDpec = fi["CodigoDpec"].ToString(); //habilitar cuando este disponible la nueva estructura
                    //CodigoDpec = "0a1";
                    CodigoEscalon = Convert.ToInt32(fi["CodigoEscalon"]);
                    CodigoAux = Convert.ToInt32(fi["CodigoAux"]);
                    CodigoGrupo = Convert.ToInt32(fi["CodigoGrupo"]);
                    TextoDescripcion = fi["TextoDescripcion"].ToString();
                    Cantidad = Convert.ToDouble(fi["Cantidad"], CultureInfo.CreateSpecificCulture("en-US"));                        
                    Unitario = Convert.ToDouble(fi["Unitario"], CultureInfo.CreateSpecificCulture("en-US")); 
                    Importe = Convert.ToDouble(fi["Importe"], CultureInfo.CreateSpecificCulture("en-US"));
                    ImportePA = Convert.ToDouble(fi["ImportePA"]); //habilitar cuando este disponible la nueva estructura
                    //ImportePA = 0;
                    Agrupador = fi["Agrupador"].ToString();
                    Resaltar = fi["Resaltar"].ToString();
                    ImprimeSubtotal = fi["ImprimeSubtotal"].ToString();

                        //if ((VerificarExistenciaRegistro("conceptosfacturados", "ConexionID", conexionID, Periodo, Orden) == 0))
                        //{
                        string insert;//Declaración de string que contendra la consulta INSERT
                            insert = "INSERT INTO conceptosfacturados (conexionID, Periodo, Orden, " +
                                     "CodigoConcepto, CodigoDpec, CodigoEscalon, CodigoAux, CodigoGrupo, " +
                                     "TextoDescripcion, Cantidad, Unitario, Importe, ImportePA, Agrupador, " +
                                     "Resaltar, ImprimeSubtotal) " +
                                     "VALUES (" + conexionID + ", " + Periodo + ", " + Orden + ", " + CodigoConcepto + ", '" +
                                                  CodigoDpec + "', " + CodigoEscalon + ", " + CodigoAux + ", " + CodigoGrupo + ", '" +
                                                  TextoDescripcion + "', " + Cantidad.ToString(CultureInfo.CreateSpecificCulture("en-US")) +
                                                  ", " + Unitario.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ", " +
                                                  Importe.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ", " +
                                                  ImportePA.ToString(CultureInfo.CreateSpecificCulture("en-US")) + ", '" + Agrupador + "', '" + Resaltar + "', '" + ImprimeSubtotal + "')";

                            //insert = "INSERT INTO conceptosfacturados (conexionID, Periodo, Orden, " +
                            //     "CodigoConcepto, CodigoEscalon, CodigoAux, CodigoGrupo, " +
                            //     "TextoDescripcion, Cantidad, Unitario, Importe, Agrupador, " +
                            //     "Resaltar, ImprimeSubtotal) " +
                            //     "VALUES (" + conexionID + ", " + Periodo + ", " + Orden + ", " + CodigoConcepto + ", " +
                            //                  CodigoEscalon + ", " + CodigoAux + ", " + CodigoGrupo + ", '" +
                            //                  TextoDescripcion + "', " + Cantidad + ", " + Unitario + ", " + Importe + ", '" +
                            //                  Agrupador + "', '" + Resaltar + "', '" + ImprimeSubtotal + "')";
                            //preparamos la cadena pra insercion
                            MySqlCommand command2 = new MySqlCommand(insert, DB.conexBD);
                            //y la ejecutamos
                            command2.ExecuteNonQuery();
                            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                            command2.Dispose();
                        //}


                        backgroundDescargarAPC.ReportProgress(AvanceDescarga);
                        AvanceDescarga++;

                    }
            }


            comandoSQL.Dispose();
            da.Dispose();
            daD.Dispose();
            //comandoSQLD.Dispose();
            BaseADescargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al Cargar la tabla ConceptosFacturados");
            }

        }

        /// <summary>
        /// Verifica si existe ya el medidor con el numero de conexionID y el periodo que se esta cargando para que nose repita el registro
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="ClavePrimaria">Nombre de la Clave primaria enviada como parametro</param>
        /// <param name="ValorPK">El valor que se pasa a comparar con el nombre de la clave primaria</param>
        /// <param name="Periodo"></param>
        /// <returns></returns>
        public static int VerificarExistenciaRegistroAlta(string tabla, string ClavePrimaria1, string ValorPK1, string ClavePrimaria2, 
            Int64 ValorPK2, string ClavePrimaria3, string ValorPK3, string ClavePrimaria4,
            string ValorPK4, string ClavePrimaria5, string ValorPK5)
        {
            string txSQL;
            MySqlCommand da;
            int count = 0;

            txSQL = "SELECT Count(*) FROM " + tabla + " WHERE (" + ClavePrimaria1 + " = '" + ValorPK1 + "' and " + ClavePrimaria2 + " = " + ValorPK2 +
                                                                " and " + ClavePrimaria3 + " = '" + ValorPK3 + "' and " + ClavePrimaria4 + " = '" + ValorPK4 +
                                                                "' and " + ClavePrimaria5+ " = '" + ValorPK5 + "')";
            da = new MySqlCommand(txSQL, DB.conexBD);
            da.Parameters.AddWithValue(ClavePrimaria1, ValorPK1);
            count = Convert.ToInt32(da.ExecuteScalar());
            if (count == 0)
                return count;
            else
                return count;

        }


        /// <summary>
        /// Realizo los select para obtener el resumen con el mismo formato que se ve en la colectora e informar a la hora
        /// de descargar
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void ObtenerResumenDescarga(string RutaRecibir)
        {
            DataTable Tabla = new DataTable();
            DataTable TablaAltas = new DataTable();

            try
            {
                string txSQL;
                SQLiteCommand da;
                SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                DataTable Tabla1 = new DataTable();
                DataTable Tabla2 = new DataTable();
                DataTable Tabla3 = new DataTable();
                BaseACargar.Open();

                //ConsultaCantidadConexionesNoLeidas
                txSQL = "SELECT DISTINCT Ruta, count(ImpresionOBS) AS Impresos FROM Conexiones GROUP BY ImpresionOBS, Ruta HAVING ImpresionOBS = 1";
                SQLiteDataAdapter datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                SQLiteCommandBuilder comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla1);
                datosAdapter.Dispose();
                comandoSQL.Dispose();

                //ConsultaCantidadConexionesLeidasNoImpresas
                txSQL = "SELECT DISTINCT Ruta, count(ImpresionOBS) AS Saldo  FROM Conexiones GROUP BY ImpresionOBS, Ruta HAVING ImpresionOBS = 0";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla2);
                datosAdapter.Dispose();
                comandoSQL.Dispose();

                //ConsultaCantidadConexionesLeidasImpresas
                txSQL = "SELECT Ruta, SUM(IF(ImpresionOBS > 0, 1, 0)) AS Leidos, Remesa FROM Conexiones " +
                        "WHERE ImpresionOBS > 600 GROUP BY Ruta ORDER BY Ruta ";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla3);
                datosAdapter.Dispose();
                comandoSQL.Dispose();

                if (Tabla.Rows.Count > 0)
                {
                    

                    foreach (DataRow item1 in Tabla1.Rows)
                    {
                        
                        //ListViewItem Datos = new ListViewItem(item[0].ToString());//Columna Ruta
                        //Datos.SubItems.Add(item["Impresos"].ToString());//Columna Conex  

                        //lsDetalleDesc.Items.Add(item["Ruta"].ToString());
                    }                   
                }


                BaseACargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error en obtener datos Descargas ");
            }
        }



        /// <summary>
        /// Realizo los select para obtener la cantidad de conexiones segun codigos de Impresion con los que vuelven de
        /// de la colectora a la PC
        /// </summary>
        /// <param name="RutaRecibir"></param>
        //private void ObtenerDatosDescarga(string RutaRecibir)
        private void ObtenerDatosDescarga()
        {
            DataTable Tabla = new DataTable();
            DataTable TablaAltas = new DataTable();

            try
            {
                string txSQL;
                SQLiteCommand da;
                //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                //BaseACargar.Open();

                //ConsultaCantidadConexionesNoLeidas
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoLeido);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoLeido));
                Vble.ConexNoLeidas = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesLeidasNoImpresas
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.LeidoNoImpreso);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.LeidoNoImpreso));
                Vble.ConexLeidasNoImpresas = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesLeidasImpresas
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.LeidoImpreso);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.LeidoImpreso));
                Vble.ConexLeidasImpresas = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresasImpresoraDesc
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoImpresoraDes);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoImpresoraDes));
                Vble.ConexNoImpresasImpresoraDesc = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresasFueraDeRango
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoFueraRango);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoFueraRango));
                Vble.ConexNoImpresasFueradeRango = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresasEstadoNegativo
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoEstadoNegativo);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoEstadoNegativo));
                Vble.ConexNoImpresasEstadoNegativo = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresoErrorDato
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoErrorDato);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoErrorDato));
                Vble.ConexNoImpresasErrorDato = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresoDomicilioPostal
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoDomicilioPostal);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoDomicilioPostal));
                Vble.ConexNoImpresasDomicilioPostal = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesNoImpresoIndicadoDato
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.NoImpresoIndicadoDato);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.NoImpresoIndicadoDato));
                Vble.ConexNoImpresasIndicadoDato = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesImposibleLeer
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ImposibleLeer);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ImposibleLeer));
                Vble.ConexImposibleLeer = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesSubtotalNegativo
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.subNegativo);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.subNegativo));
                Vble.ConexSubtNeg = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesError al archivar Datos
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ErrorArchDatos);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ErrorArchDatos));
                Vble.ConexErrorArchDatos = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexionesError numero de factura
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ErrorNFact);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ErrorNFact));
                Vble.ConexErrorNFact = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex Sin Concepto que Facturar
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ConexSinConcepFacturar);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ConexSinConcepFacturar));
                Vble.ConexSinConcepFacturar = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex FaltaTitular
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.FaltaTitular);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.FaltaTitular));
                Vble.FaltaTitular = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex Error al Facturar
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ConexErrorFacturando);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ConexErrorFacturando));
                Vble.ConexErrorFacturando = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex Error al Facturar
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ErroEnMemoria);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ErroEnMemoria));
                Vble.ErroEnMemoria = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();


                //ConsultaCantidadConexiones Conex Periodo Excedido en Dias
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ConexPerExcDias);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ConexPerExcDias));
                Vble.ConexPerExcDias = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //ConsultaCantidadConexiones Conex ErrorIndeterminado
                txSQL = "SELECT Count(ImpresionOBS) FROM Conexiones WHERE ImpresionOBS = " + Convert.ToInt32(cteCodImpres.ErrorIndeterminado);
                da = new SQLiteCommand(txSQL, BaseADescargar);
                da.Parameters.AddWithValue("ImprsionOBS", Convert.ToInt32(cteCodImpres.ErrorIndeterminado));
                Vble.ErrorIndeterminado = Convert.ToInt32(da.ExecuteScalar());
                da.Dispose();

                //BaseACargar.Close();

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error en el método obtener datos Descargas ");
            }
        }

        /// <summary>
        /// Cargo la tabla Descargas de la base general MySQL con los datos pertenecientes a las conexiones que estaban en la colectora 
        /// </summary>
        /// <param name="RutaRecibir"></param>
        private void CargarTablaDescargas()
        {
            DataTable Tabla = new DataTable();
            try
            {
                //string fechadescarga = string.Format("{0:yyMMdd_HHmm}", DateTime.Now);

                string insert;//Declaración de string que contendra la consulta INSERT
                insert = "INSERT INTO descargas (Periodo, FechaDescarga, Carpeta, Dispositivo, Lecturista, InfConexiones, CantidadConex," +
                    " Noleidas, LeidasNoImpresas, LeidasImpresas, NoImpresasImpDesconec, NoImpresasFueraDeRango, NoImpresasEstadoNeg," +
                    " NoImpresasErrorDato, NoImpresasDomPostal, NoImpresasIndicDato, ImposibleLeer, SubTotalNeg, ErrorArchivarDatos," +
                    " ErroNumFactura, SinConceptosFacturar, FaltaTitular, ErrorAlFacturar, ErrorEnMemoria, PeriodoExcEnDias, ErrorIndeterminado) " +
                    "VALUES ('" + Vble.PeriodoEnColectora + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "', '" + Vble.NomCarpDescarga + "', '" + cmbDevices.Text + "', '" + Vble.Operario +
                    "', '" + Vble.InfoDescarga + "', '" + Vble.CantConex + "', '" + Vble.ConexNoLeidas + "', '" + Vble.ConexLeidasNoImpresas +
                    "', '" + Vble.ConexLeidasImpresas + "', '" + Vble.ConexNoImpresasImpresoraDesc + "', '" + Vble.ConexNoImpresasFueradeRango +
                    "', '" + Vble.ConexNoImpresasEstadoNegativo + "', '" + Vble.ConexNoImpresasErrorDato + "', '" + Vble.ConexNoImpresasDomicilioPostal +
                    "', '" + Vble.ConexNoImpresasIndicadoDato + "', '" + Vble.ConexImposibleLeer + "', '" + Vble.ConexSubtNeg +
                    "', '" + Vble.ConexErrorArchDatos + "', '" + Vble.ConexErrorNFact + "', '" + Vble.ConexSinConcepFacturar + "', '" + Vble.FaltaTitular +
                    "', '" + Vble.ConexErrorFacturando + "', '" + Vble.ErroEnMemoria + "', '" + Vble.ConexPerExcDias + "', '" + Vble.ErrorIndeterminado + "')";


                //preparamos la cadena pra insercion
                MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                //y la ejecutamos
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message, "Error al generar archivo de descarga");
            }


        }


        /// <summary>
        /// Carga la tabla Altas de la base general MySQL con los datos de la tabla altas de la base SQLite que viene en cada colectora en caso de que existan
        /// </summary>
        /// <param name="RutaRecibir"></param>
        //private void CargarTablaAltas(string RutaRecibir)
        private void CargarTablaAltas()
        {
            try
            {
                DataTable TablaAltas = new DataTable();
                SQLiteDataAdapter datosAdapter = new SQLiteDataAdapter();
                SQLiteCommandBuilder comandoSQL = new SQLiteCommandBuilder();
                SQLiteCommand da = new SQLiteCommand();
                string Hora;
                DateTime fecha;
                string modelo, numero, activa, domicilio, observaciones, ABM, estado, ConexionID, Digitos, FactorMul, Ruta, Operario;
                double latitud, longitud;
                Int64 CantAltas = 0;


                //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaRecibir);
                //BaseACargar.Open();
                ///esta porcion verifica si existen altas en la tabla altas de la base sqlite de la colectora,
                ///si existe carga la tabla general MySQL con las nuevas altas 
                string txSQL = "SELECT Count(*) FROM Altas";
                da = new SQLiteCommand(txSQL, BaseADescargar);
                CantAltas = Convert.ToInt64(da.ExecuteScalar());
                da.Dispose();

                if (CantAltas > 0)
                {
                    txSQL = "select * From Altas";
                    datosAdapter = new SQLiteDataAdapter(txSQL, BaseADescargar);
                    comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                    datosAdapter.Fill(TablaAltas);

                    foreach (DataRow fi in TablaAltas.Rows)
                    {
                        Vble.Periodo = Convert.ToInt32(fi["Periodo"]);
                        ABM = fi["ABM", DataRowVersion.Original].ToString();//corroborar una descarga si funciona con la nueva estructura de la tabla altas
                        fecha = Convert.ToDateTime(fi["Fecha", DataRowVersion.Original]);
                        Hora = fi["Hora", DataRowVersion.Original].ToString();
                        ConexionID = fi["ConexionID"].ToString() == "" ? "-" : fi["ConexionID"].ToString();
                        modelo = fi["Modelo"].ToString() == "" ? "-" : fi["Modelo"].ToString();
                        numero = fi["Numero"].ToString() == "" ? "-" : fi["Numero"].ToString();
                        Digitos = fi["Digitos"].ToString() == "" ? "-" : fi["Digitos"].ToString();
                        FactorMul = fi["FactorMult"].ToString() == "" ? "-" : fi["FactorMult"].ToString();
                        activa = fi["Activa"].ToString() == "" ? "-" : fi["Activa"].ToString();
                        estado = fi["Estado"].ToString() == "" ? "-" : fi["Estado"].ToString();
                        domicilio = fi["Domicilio"].ToString() == "" ? "-" : fi["Domicilio"].ToString();
                        observaciones = fi["Observaciones"].ToString() == "" ? "-" : fi["Observaciones"].ToString();                      
                        Ruta = fi["Ruta"].ToString() == "" ? "0" : fi["Ruta"].ToString();
                        int rutaInt = Convert.ToInt32(Ruta);
                        //Ruta = (Int64)fi["Ruta"] == 0 ? 0 : (Int64)fi["Ruta"];
                        Operario = fi["Operario"].ToString() == "" ? "0" : fi["Operario"].ToString();
                        latitud  =  fi["Latitud"].ToString() == "-28" ? -28 : Convert.ToDouble(fi["Latitud"].ToString().Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        longitud = fi["Longitud"].ToString() == "-58" ? -28 : Convert.ToDouble(fi["Longitud"].ToString().Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //Convert.ToDouble(substrings[10], CultureInfo.CreateSpecificCulture("en-US"));

                        //if (ABM.ToUpper() == "A")
                        //{
                        if ((VerificarExistenciaRegistroAlta("Altas", "Periodo", Vble.Periodo.ToString(), "Ruta", rutaInt, "Fecha", fecha.ToString("yyyy/MM/dd"), "Hora", Hora, "Numero", numero) == 0))
                        {
                            string insert = "INSERT INTO Altas (Periodo, ABM, Ruta, Operario, Fecha, Hora, ConexionID, Modelo, Numero, Digitos, FactorMult, Activa, Estado," +
                                            " Domicilio, Observaciones, Latitud, Longitud) " +
                                            "VALUES ('" + Vble.Periodo + "', '" + ABM + "', " + Ruta + ", " + Operario + ", '" + Convert.ToDateTime(fecha).ToString("yyyy/MM/dd") + "', '" + 
                                            Convert.ToDateTime(Hora).ToString("HH:mm") + "', '" + 
                                            ConexionID + "', '" + modelo + "', '" + numero + "', '" + 
                                            Digitos + "', '" + FactorMul + "', '" + activa + "', '" + 
                                            estado + "', '" + domicilio + "', '" + observaciones + "', '" + latitud.ToString().Replace(",", ".") + "', '" + longitud.ToString().Replace(",", ".") + "') ";

                            //preparamos la cadena pra insercion
                            MySqlCommand command = new MySqlCommand(insert, DB.conexBD);
                            //y la ejecutamos
                            command.ExecuteNonQuery();
                            //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                            command.Dispose();
                        }
                        //}
                    }
                }

                comandoSQL.Dispose();
                datosAdapter.Dispose();
                //BaseACargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message, "Error al cargar altas");
            }


        }

        /// <summary>
        /// Carga tabla de Novedades Conexion en MySQL en caso de que la descarga de la colectora contenga novedades en alguna de las conexiones leidas.
        /// </summary>
        /// <param name="RutaSqlite"></param>
        //private void AgregarNovedadesConex(string RutaSqlite)
        private void AgregarNovedadesConex()
        {
            string txSQL;
            //SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaSqlite);            
            //SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaSqlite + "; Password=alVlgePcDdL");
            //BaseADescargar.Open();
            string Observacion = "";

            try
            {
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;
                DataTable Tabla = new DataTable();

                txSQL = "SELECT * FROM NovedadesConex";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseADescargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    txSQL = "select Count(*) From NovedadesConex WHERE Periodo = " + Convert.ToInt32(fi["Periodo"].ToString()) +
                                                                " and ConexionID = " + Convert.ToInt32(fi["conexionID"].ToString());
                    MySqlCommand da = new MySqlCommand(txSQL, DB.conexBD);
                    int count = Convert.ToInt32(da.ExecuteScalar());
                    Observacion = (fi["Observ"].ToString());

                    if (count > 0)
                    {
                        string delete = "delete from NovedadesConex where ConexionID = " + Convert.ToInt32(fi["conexionID"].ToString()) + " AND Periodo = " + Convert.ToInt32(fi["Periodo"].ToString());
                        MySqlCommand command = new MySqlCommand(delete, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                    }
                    da.Dispose();
                }


                foreach (DataRow fi in Tabla.Rows)
                {
                    string insert;//Declaración de string que contendra la consulta INSERT
                    insert = "INSERT INTO NovedadesConex (ConexionID, Orden, Periodo, Codigo, Observ) " +
                             "VALUES (" + Convert.ToInt32(fi["conexionID"].ToString()) + ", " + Convert.ToInt32(fi["Orden"].ToString()) + ", " +
                              Convert.ToInt32(fi["Periodo"].ToString()) + ", " + Convert.ToInt32(fi["Codigo"].ToString()) + ", '" + (fi["Observ"].ToString()) + "')";
                    //preparamos la cadena pra insercion
                    MySqlCommand command2 = new MySqlCommand(insert, DB.conexBD);
                    //y la ejecutamos
                    command2.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command2.Dispose();

                    //if ((fi["Observ"].ToString().Contains('|')))
                    //{
                    //    Int32 EstadoACorregir = Convert.ToInt32(fi["Observ"].ToString().Substring(0, fi["Observ"].ToString().IndexOf('|')));
                    //    CorregirEstado(Convert.ToInt32(fi["conexionID"].ToString()), Convert.ToInt32(fi["Periodo"].ToString()), EstadoACorregir, Observacion);
                    //}


                }
                comandoSQL.Dispose();
                datosAdapter.Dispose();
                //BaseADescargar.Close();

                
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error al agregar Novedades/Ordenativos a la base");
            }
        }

        public void CorregirEstado(Int32 ConexionID, Int32 Periodo, Int32 EstadoCorregido, string Observacion)
        {
            try
            {

            string ConsultaImpreso = "select ImpresionOBS From conexiones WHERE Periodo = " + Periodo + " and ConexionID = " + ConexionID; //+ " AND Convert(ImpresionOBS, tinytext) like '%02'";
            MySqlCommand da = new MySqlCommand(ConsultaImpreso, DB.conexBD);
            string count = (da.ExecuteScalar()).ToString();
            da.Dispose();

            if (!count.Contains("02"))
            {
                ///Obtengo el ActualEstado tomado erroneo de la tabla medidores para 
                ///agregar despues del pipe en la tabla novedadesconex una vez corregido y así quede informado los dos estados
                string SelectActualEstado = "select ActualEstado From medidores WHERE Periodo = " + Periodo + " and ConexionID = " + ConexionID; //+ " AND Convert(ImpresionOBS, tinytext) like '%02'";
                MySqlCommand obt = new MySqlCommand(SelectActualEstado, DB.conexBD);
                string ActualEstado = (obt.ExecuteScalar()).ToString();


                ///Modifico el ActualEstado de la tabla medidor con el estado que se encuentra en la tabla novedadesconex a corregir
                ///el mismo siempre aparecera antes del primer signo pipe | 
                string update;//Declaración de string que contendra la consulta UPDATE               
                update = "UPDATE medidores SET ActualEstado = " + EstadoCorregido +
                        " WHERE ConexionID = " + ConexionID + " AND Periodo  =" + Periodo;                
                MySqlCommand command = new MySqlCommand(update, DB.conexBD);                
                command.ExecuteNonQuery();                
                command.Dispose();              

                ///Una vez que corrigo el ActualEstado de la tabla medidores agrego despues del pipe del estado a corregir en la tabla novedadesconex 
                ///el Estado efectivo que habia tomado el lecturista y se debió corregir para tener ambos datos informados
                ///quedando este dato de la siguiente forma
                ///Estado a corregir|EstadoTomado por el Lecturista
                ///    0000         |           0000               |
                string updateNovConex;//Declaración de string que contendra la consulta UPDATE               
                    updateNovConex = "UPDATE novedadesconex SET Observ = '|" + ActualEstado.ToString() + "|" + Observacion + "' WHERE ConexionID = " + ConexionID + " AND Periodo = " + Periodo;
                //preparamos la cadena pra insercion
                MySqlCommand command2 = new MySqlCommand(updateNovConex, DB.conexBD);
                //y la ejecutamos
                command2.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command2.Dispose();

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Excepcion en metodo 'CorregirEstado'");
            }

        }

        /// <summary>
        /// Toma el punto de Venta asociada a la colectora y actualiza el valor en la base General MySQL
        /// </summary>
        /// <param name="BaseSQLite"></param>
        private void ActualizarPuntoVenta(string RutaSqlite)
        {
            try
            {

                string txSQL;
                SQLiteConnection BaseADescargar = new SQLiteConnection("Data Source=" + RutaSqlite);
                BaseADescargar.Open();
                DataTable Tabla = new DataTable();

                //
                //Busco el punto de venta asociada a la colectora en tabla Varios
                //
                txSQL = "select * From Varios";
                SQLiteDataAdapter datosAdapter = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQL = new SQLiteCommandBuilder(datosAdapter);

                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {

                    if (fi["Parametro"].ToString() == "puntoVenta")
                    {
                        Vble.PuntoVenta = (fi["Valor"].ToString());
                        //MessageBox.Show("Punto de Venta de la colectora en Varios: " + Vble.PuntoVenta);
                    }
                }
                comandoSQL.Dispose();
                datosAdapter.Dispose();

                //
                //Otengo el Numero asiado a ese punto de Venta que contiene la colectora en Comprobantes
                //para luego comparar en la tabla general y decidir si actualizar o no el valor
                //
                txSQL = "select * From Comprobantes";
                SQLiteDataAdapter datosAdapter2 = new SQLiteDataAdapter(txSQL, BaseADescargar);
                SQLiteCommandBuilder comandoSQL2 = new SQLiteCommandBuilder(datosAdapter2);

                datosAdapter2.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {

                    if (fi["PuntoVenta"].ToString() == Vble.PuntoVenta)
                    {
                        if (fi["Letra"].ToString() == "A")
                        {
                            Vble.NumeroPuntoVentaA = (fi["Numero"].ToString());
                        }
                        else if (fi["Letra"].ToString() == "B")
                        {
                            Vble.NumeroPuntoVentaB = (fi["Numero"].ToString());
                        }
                        //MessageBox.Show("Numero del Punto de Venta en Comprobantes: " + fi["Numero"].ToString());
                       
                    }

                }

                comandoSQL2.Dispose();
                datosAdapter2.Dispose();

                //Obtengo el Numero del punto de Venta de la Base general MySQL y actualiza o no en los siguientes casos:
                //Valor del punto de Venta de Colectora > Valor del Punto de Venta en MySql = Actualiza.
                //Valor del punto de Venta de Colectora = Valor del Punto de Venta en MySql = Deja como esta en MySql.
                //Valor del punto de Venta de Colectora < Valor del Punto de Venta en MySql = Muestra Mje que tiene un valor menor por el registrado anteriormente.
                txSQL = "select * From comprobantes";
                MySqlDataAdapter datosAdapter3 = new MySqlDataAdapter(txSQL, DB.conexBD);
                MySqlCommandBuilder comandoSQL3 = new MySqlCommandBuilder(datosAdapter3);
                datosAdapter3.Fill(Tabla);


                foreach (DataRow fil in Tabla.Rows)
                {
                    if (fil["PuntoVenta"].ToString() == Vble.PuntoVenta)
                    {

                        if (fil["Letra"].ToString() == "A")
                        {
                            if (Convert.ToInt32(fil["Numero"].ToString()) > Convert.ToInt32(Vble.NumeroPuntoVentaA))
                            {
                                MessageBox.Show("Atencion: El punto de venta que se esta procesando tiene un valor asociado menor al registrado anteriormente, " +
                                   "por favor tenga en cuenta este dato, el Punto de Venta es: " + Vble.PuntoVenta, "Valor del punto de venta por debajo del registrado", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                            {
                                string update;//Declaración de string que contendra la consulta UPDATE               
                                update = "UPDATE comprobantes SET Numero = " + (Vble.NumeroPuntoVentaA) +
                                                                " WHERE PuntoVenta = " + Vble.PuntoVenta + " AND Letra = 'A'";
                                //preparamos la cadena pra insercion
                                MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                                //y la ejecutamos
                                command.ExecuteNonQuery();
                                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                                command.Dispose();
                            }
                        }
                        else if (fil["Letra"].ToString() == "B")
                        {
                            if (Convert.ToInt32(fil["Numero"].ToString()) > Convert.ToInt32(Vble.NumeroPuntoVentaB))
                            {
                                MessageBox.Show("Atencion: El punto de venta que se esta procesando tiene un valor asociado menor al registrado anteriormente, " +
                                   "por favor tenga en cuenta este dato, el Punto de Venta es: " + Vble.PuntoVenta, "Valor del punto de venta por debajo del registraod", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                            {
                                string update;//Declaración de string que contendra la consulta UPDATE               
                                update = "UPDATE comprobantes SET Numero = " + (Vble.NumeroPuntoVentaB) +
                                                                " WHERE PuntoVenta = " + Vble.PuntoVenta + " AND Letra = 'B'"; ;
                                //preparamos la cadena pra insercion
                                MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                                //y la ejecutamos
                                command.ExecuteNonQuery();
                                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                                command.Dispose();
                            }

                        }
                       
                    }
                }
                comandoSQL3.Dispose();
                datosAdapter3.Dispose();
                BaseADescargar.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }

        }


        /// <summary>
        /// Copio los archivos que se enviaron a la colectora a la carpeta temporal para comparar la sincronización 
        /// y verificar con la fecha actual de la colectora
        /// </summary>
        public void CopiarArchivosEnTemporal(string Ruta, string RutaTemporal)
        {

            Vble.RutaTemporal = RutaTemporal;

            if (!Directory.Exists(Vble.RutaTemporal))
            {
                //Creo la Carpeta temporal que contendrá los archivos para comparar al momento de sincronizar
                Directory.CreateDirectory(Vble.RutaTemporal);

                //Ruta = Vble.ValorarUnNombreRuta(Vble.LeerUnNombreDeCarpeta("Dir Directorio Colectora en PC")) + Vble.Colectora;
                Vble.RutaColectoraConectada = Ruta;
                Vble.RutaBaseSQLiteColectora = Ruta + Vble.NombreArchivoBaseSqlite();
                //Vble.RutaBaseFijaSQLiteColectora = Ruta + Vble.NombreArchivoBaseFijaSqlite();
                Vble.RutaArchivoInfoCarga = Ruta + Vble.NombreArchivoInfoCarga();
                


                //asignación de variables temporales para la comparacion de sincronización
                //Vble.TemporalBaseFija = Vble.RutaTemporal + "\\" + Vble.NombreArchivoBaseFijaSqlite();
                Vble.TemporalBaseVariable = Vble.RutaTemporal + "\\" + Vble.NombreArchivoBaseSqlite();
                Vble.TemporalInfoCarga = Vble.RutaTemporal + "\\" + Vble.NombreArchivoInfoCarga();
                
                //File.Copy(Vble.RutaBaseFijaSQLiteColectora, Vble.TemporalBaseFija, true);
                File.Copy(Vble.RutaBaseSQLiteColectora, Vble.TemporalBaseVariable, true);
                File.Copy(Vble.RutaArchivoInfoCarga, Vble.TemporalInfoCarga, true);
                
            }
            else
            {
                

                //timer3.Stop();
                Vble.TemporalBaseVariable = Vble.RutaTemporal + "\\" + Vble.NombreArchivoBaseSqlite(); ;
                string variable = Vble.TemporalBaseVariable;
                copiado = true;
                
            }



        }


        private void splitContainer4_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        /// <summary>
        /// Contiene las funciones que crea el informe de las descargas pertenecientes al Periodo 
        /// en el que se está trabajando con formato PDF 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click_2(object sender, EventArgs e)
        {
            DataTable Tabla = new DataTable();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            bool CreaInforme = false;
            try
            {
                txSQL = "SELECT * FROM descargas";
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    if (fi[1].ToString() == Vble.Periodo.ToString())
                    {
                        CreaInforme = true;
                    }
                }

                GenerarInformeDescarga(Vble.Periodo.ToString(), "al Periodo", 1, "Periodo", CreaInforme);
                datosAdapter.Dispose();


            }

            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        private void btnCarpTodo_Click_2(object sender, EventArgs e)
        {
            LeerInformes(Vble.Periodo.ToString(), 1, "del PERIODO " + Vble.Periodo);
        }

        private void btnCarpNada_Click_1(object sender, EventArgs e)
        {
            tvInformes.Nodes.Clear();
            fraDescargas.Text = "Descargas ";
            lbColDesc.Text = "Total Colectoras Descargadas: " + 0;
            lbDescTotal.Text = "Total Conexiones Descargadas:   " + 0;
            lbTotImp.Text = "Total Impresas:   " + 0;
            lbTotLeiNoImp.Text = "Total Leidas No Impresas: " + 0;
            lbTotImposLeer.Text = "Total Imposibles Leer: " + 0;
            lbNOLeidas.Text = "Total NO Leidas: " + 0;
            lbLeiNOimpOtros.Text = "Total Leidas No Impresas (otros motivos): " + 0;
        }



        /// <summary>
        /// Contiene las funciones que crea el informe de las descargas pertenecientes a la Fecha 
        /// en el que se está trabajando con formato PDF 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuitar_Click_1(object sender, EventArgs e)
        {
            DataTable Tabla = new DataTable();
            string txSQL;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            bool CreaInforme = false;
            DateTime fechadescarga;
            try

            {
                txSQL = "SELECT * FROM descargas";
                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    fechadescarga = Convert.ToDateTime(fi["FechaDescarga"]);
                    if (fechadescarga.ToString("dd-MM-yyyy") == DateTime.Today.ToString("dd-MM-yyyy"))
                    {
                        CreaInforme = true;
                    }
                }


                GenerarInformeDescarga(DateTime.Today.ToString("dd-MM-yyyy"), "a la Fecha", 2, "Fecha", CreaInforme);
                datosAdapter.Dispose();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        private void btnResDia_Click(object sender, EventArgs e)
        {
            LeerInformes(DateTime.Today.ToString("dd-MM-yyyy"), 2, "de la FECHA " + DateTime.Today.ToString("dd/MM/yyyy"));
        }

        private void btnContraerTodo_Click(object sender, EventArgs e)
        {
            //Vble.CarpetaRespaldo = Vble.CarpetaRespaldo + "\\" + Vble.ValorarUnNombreRuta(Vble.CarpetaDescargasNoProcesadas) + "\\" +
            //                                           DateTime.Now.ToString("dd-MM-yyyy") + "\\" + cmbDevices.Text + "_" +
            //                                           Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora);

            //MessageBox.Show(Vble.CarpetaRespaldo);

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    //Lee y obtiene el nombre de la base Sqlite
            //    StringBuilder stb1 = new StringBuilder("", 100);
            //    Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            //    string ArchivoBase = stb1.ToString();
            //    //Lee y obtiene el nombre de la base Sqlite
            //    StringBuilder stb2 = new StringBuilder("", 100);
            //    Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
            //    string ArchivoBaseChica = stb2.ToString();
            //    //recorre los dispositivos conectados y va consultando si existe alguna colectora Con la denominación "MICC-0000"
            //    foreach (var item in shellViewDescargas.CurrentFolder)
            //    {
            //        if (item.DisplayName == cmbDevices.Text)
            //        {
            //            //shellView2.Visible = true;
            //            //recorre las unidades que contiene la colectora y busca el directorio Raiz "\" para ingresar
            //            foreach (var raiz in item)
            //            {
            //                if (raiz.DisplayName == "\\")
            //                {
            //                    //recorre las subcarpetas del directorio Raiz y busca la carpeta a la cual se van a enviar los archivos desde la PC
            //                    //vble.DestinoArchivoColectora = "Datos DPEC" en archivo.ini
            //                    foreach (var carpeta in raiz)
            //                    {
            //                        if (carpeta.DisplayName == Vble.DestinoArchivosColectora)
            //                        {
            //                            //shellView2.CurrentFolder = carpeta;                                    
            //                            foreach (var destino in carpeta)
            //                            {
            //                                //////determina como directorio del shellview a mostrar el que se especifico buscar en el archivo.ini
            //                                if (destino.DisplayName == Vble.CarpetaDestinoColectora)
            //                                {
            //                                    //declaro y utilizo algunas variables que se utilizan para la DESCARGA de conexiones 
            //                                    string Dispositivo = Funciones.BuscarNombreColectora(Vble.DirectorioColectoraenPC);
            //                                    Vble.Colectora = Dispositivo + cmbDevices.Text + "\\" + Vble.CarpetaDestinoColectora + "\\";
            //                                    Vble.RutaColectoraConectada = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;
            //                                    DirectoryInfo di = new DirectoryInfo(Vble.RutaColectoraConectada);
            //                                    Vble.ArchivoInfoCargaColectora = Vble.RutaColectoraConectada + "InfoCarga.txt";
            //                                    Vble.RutaBaseSQLiteColectora = Vble.RutaColectoraConectada + ArchivoBase;
            //                                    Vble.BaseChicaFIS = Vble.RutaColectoraConectada + ArchivoBaseChica;
            //                                    Vble.ColectoraConectada = cmbDevices.Text;

            //                                    if (File.Exists(Vble.RutaBaseSQLiteColectora))
            //                                    {
            //                                        VerificarCambioContraseña(Vble.BaseChicaFIS);
            //                                    }
            //                                    else
            //                                    {
            //                                        MessageBox.Show("La colectora no contiene Conexiones para descargar", "Colectora Vacia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    //MessageBox.Show("Disculpe no se encuentra la carpeta destino", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                                    shellViewDescargas.Visible = false;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception R)
            //{
            //    MessageBox.Show(R.Message.Substring(0, 31) + " de informacion de las conexiones", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }

        private void backgroundDescargaCol_DoWork(object sender, DoWorkEventArgs e)
        {            
            //simulateHeavyWork(CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Conexiones"));
            //CheckForIllegalCrossThreadCalls = false;
            //Vble.InfoDescarga = Funciones.LeerArchivostxt(Vble.RutaColectoraConectada + "\\"+ "InfoCarga.txt");
            ////MessageBox.Show(Vble.RutaBaseSQLiteColectora);
            /////MostrarRutasDeColectoras(Vble.RutaBaseSQLiteColectora);
            //if (backgroundDescargaCol.CancellationPending)
            //{
            //    e.Cancel = true;
            //    return;
            //}
            //Deshabilito los demas botones cuando se está prdoduciendo la descarga para que no haya interrupoción del proceso.

           






        }

        private void simulateHeavyWork(int cantidad)
        {
            Thread.Sleep(cantidad);

        }

        private void backgroundDescargaCol_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //labelPorcDesc.Visible = true;
            //progressBar.Visible = true;            
            //progressBar.Value = (e.ProgressPercentage * 100) / CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora);
            //labelPorcDesc.Text = "% " + (e.ProgressPercentage * 100) / CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora) + " completado    ";
            PanelDescargaFTP.Visible = true;
        }

        private void backgroundDescargaCol_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //File.Delete(Vble.RutaBaseSQLiteColectora);
            //File.Delete(Vble.ArchivoInfoCargaColectora);
            //if (Directory.Exists(Vble.RutaTemporal))
            //{
            //    Directory.Delete(Vble.RutaTemporal, true);
            //}


            if (this.btnDetener.Visible == false)
            {
                btnDescargar.Visible = false;
                PanelDescargaFTP.Visible = false;
            }
            else
            {
                CheckForIllegalCrossThreadCalls = true;
                //Habilito nuevamente los botones que podian haber ocacionado interrupcion al proceso de descarga
                btnDescargar.Visible = true;
                btnCerrar.Enabled = true;
                this.ControlBox = true;
                this.cmbDevices.Enabled = true;
                this.btnDetener.Visible = false;
                PanelDescargaFTP.Visible = false;
            }

            
            //MessageBox.Show("Descarga Finalizada");
            //}

        }

        /// <summary>
        /// Metodo que contiene procesos de Descarga ejecutandose en segundo Plano
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundDescargarAPC_DoWork(object sender, DoWorkEventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;

            //string SQLitedesdePC = "C:\\Users\\usuario\\Documents\\Documentos en MICC-4004\\Datos DPEC\\dbFIS-DPEC.db";
            //simulateHeavyWork(CantidadRegistrosADescargar(SQLitedesdePC));
            //progressBar.Maximum = CantidadRegistrosADescargar(SQLitedesdePC);
            //CambiarEstadoRecibidoMySql(SQLitedesdePC, Convert.ToInt32(cteCodEstado.Descargado));

            
            //CantConex = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Conexiones");
            //CantCFact = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Facturas");
            //CantImpresor = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Impresor");
            //CantLogErr = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "LogErrores");

            CantConex = CantidadRegistrosADescargar("Conexiones");
            //CantCFact = CantidadRegistrosADescargar("Facturas");
            CantImpresor = CantidadRegistrosADescargar("Impresor");
            //CantLogErr = CantidadRegistrosADescargar("LogErrores");

            TotalRegistros = CantConex;// + CantImpresor + CantCFact + CantLogErr;

            //simulateHeavyWork(TotalRegistros);


            //Deshabilito los botones que pueden interrumpir el proceso que se esta ejecutando
            btnCerrar.Enabled = false;
            this.ControlBox = false;
            btnDescargar.Enabled = false;
            Form3Descargas form3 = new Form3Descargas();
            form3.ControlBox = false;
            Form0 form0 = new Form0();
            form0.ControlBox = false;
            cmbDevices.Enabled = false;
            btnResumTodo.Enabled = false;
            btnResDia.Enabled = false;
            btnCarpNada.Enabled = false;
            btnInformeDia.Enabled = false;
            btnInformePer.Enabled = false;


            //Inician procesos de descarga

            progressBar.Maximum = TotalRegistros;

            //CambiarEstadoRecibidoMySql(Vble.RutaBaseSQLiteColectora, Convert.ToInt32(cteCodEstado.Descargado));
            CambiarEstadoRecibidoMySql(Convert.ToInt32(cteCodEstado.Descargado));
            //AgregarNovedadesConex(Vble.RutaBaseSQLiteColectora);
            AgregarNovedadesConex();
            ////ActualizarPuntoVenta(Vble.RutaBaseSQLiteColectora); En android no se aplica el punto de venta
            //ObtenerDatosDescarga(Vble.RutaBaseSQLiteColectora);
            ObtenerDatosDescarga();


          

            ///////Verificar este procesooo CUALQUIER ERROR QUE APAREZCA A LA HORA DE DESCARGAR LAS COLECORAS Y SE ENCUENTRE AQUI
            ///COMENTAR LA LINEA DE DESCARGA DE TABLA IMPRESOR
            /////--------------------------------------------------------------------------
            if (CantImpresor > 0)
            {
                DescargarTablaImpresor(Vble.RutaBaseSQLiteColectora);
            }
            //if (CantCFact > 0)
            //{
            //    //DescargarTablaFacturas(Vble.RutaBaseSQLiteColectora);
            //    DescargarTablaFacturasBIS(Vble.RutaBaseSQLiteColectora);
            //}
            //if (CantLogErr > 0)
            //{
            //    DescargarTablaLogErrores(Vble.RutaBaseSQLiteColectora);
            //}

            ////backgroundDescargaCol.RunWorkerAsync();                       
            //////_--------------------------------------------------------------------------
            ////CargarTablaAltas(Vble.RutaBaseSQLiteColectora);
            CargarTablaAltas();

            //CargarTablaConceptosFacturados(Vble.RutaBaseSQLiteColectora);
            VerificarCambioContraseña(Vble.BaseChicaColectora);
            GenerarCarpetaDescarga();

            //Vble.InformesNovedades = Vble.ValorarUnNombreRuta(Vble.InformesNovedades) + "Ruta " + Vble.rutas + "\\InformesNovedades.xlsx";

            //Vble.ExportarExcel(Vble.NºInstalacionImpresos, Vble.ContratoImpresos, Vble.TitularImpresos, Vble.FacturaImpresos,
            //                   Vble.NºInstalacionFueraDeRango, Vble.ContratoFueraDeRango, Vble.TitularFueraDeRango,
            //                   Vble.ObservacionesFueraDeRango, Vble.InformesNovedades);


            CargarTablaDescargas();
            ////Vble.ModificarInfoConex(DateTime.Today.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), DB.sDbUsu, 0, "Descarga", Vble.RutaBaseSQLiteColectora);
            //Vble.ModificarInfoConex(DateTime.Today.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), DB.sDbUsu, 0, "Descarga", BaseADescargar);

            //bgWorkDescFacImpr.RunWorkerAsync();

            //progressBar.Value = TotalRegistros;
            //Task oTask = new Task(DescargaTablasImpreFacLog);
            //oTask.Start();
            //await oTask;

        }

        
        private void backgroundDescargarAPC_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            //int CantConex = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Conexiones");
            //int CantFact = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Impresor");
            //int CantImpresor = CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora, "Facturas");
            //int TotalRegistros = (CantConex + CantFact + CantImpresor);           
            //labelPorcDesc.Visible = true;
            //progressBar.Visible = true;
            progressBar.Value = e.ProgressPercentage /** 100) / CantidadRegistrosADescargar(Vble.RutaBaseSQLiteColectora)*/;           
            labelPorcDesc.Text = (e.ProgressPercentage * 100) / TotalRegistros + "% completado    ";
        }


       private void DescargaTablasImpreFacLog()
        {
            try
            {           
                CantCFact = CantidadRegistrosADescargar("Facturas");
                CantImpresor = CantidadRegistrosADescargar("Impresor");
                CantLogErr = CantidadRegistrosADescargar("LogErrores");
                CantRegistros = CantCFact + CantImpresor + CantLogErr;

                simulateHeavyWork(CantRegistros);
                CheckForIllegalCrossThreadCalls = false;

                Vble.ModificarInfoConex(DateTime.Today.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), DB.sDbUsu, 0, "Descarga", BaseADescargar);

                /////Verificar este procesooo
                ///--------------------------------------------------------------------------
                if (CantImpresor > 0)
                {
                    DescargarTablaImpresor(Vble.RutaBaseSQLiteColectora);
                }
                if (CantCFact > 0)
                {
                    //DescargarTablaFacturas(Vble.RutaBaseSQLiteColectora);
                    DescargarTablaFacturasBIS(Vble.RutaBaseSQLiteColectora);
                }
                if (CantLogErr > 0)
                {
                    DescargarTablaLogErrores(Vble.RutaBaseSQLiteColectora);
                }
                //_--------------------------------------------------------------------------

                BaseADescargar.Dispose();
                BaseADescargar.Close();



                if (File.Exists(Vble.RutaBaseSQLiteColectora))
                {
                    File.Delete(Vble.RutaBaseSQLiteColectora);
                }

                if (File.Exists(Vble.ArchivoInfoCargaColectora))
                {
                    File.Delete(Vble.ArchivoInfoCargaColectora);
                }               
                
                if (Directory.Exists(Vble.RutaTemporal))
                {
                    Directory.Delete(Vble.RutaTemporal, true);
                }
            }
            catch (Exception)
            {
                BaseADescargar.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseSqlite()))
                {
                    if (Descargado == false)
                    {
                        CopiarArchivosEnTemporal(Vble.RutaColectoraConectada, Vble.RutaTemporal);
                        if (copiado == false)
                        {                        
                            FileInfo Temp = new FileInfo(Vble.TemporalBaseVariable);
                            FileInfo fi = new FileInfo(Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseSqlite());
                            string temporal = Temp.LastWriteTime.ToString();
                            string actualizado = fi.LastWriteTime.ToString();
                            UltimaModificacion(fi, Temp, pictureBox1);
                        }
                        else
                        {
                            FileInfo Temp = new FileInfo(Vble.TemporalBaseVariable);
                            FileInfo fi = new FileInfo(Vble.RutaColectoraConectada + "\\" + Vble.NombreArchivoBaseSqlite());
                            UltimaModificacion(fi, Temp, pictureBox1);
                        }
                    }
                    else
                    {
                        if (Directory.Exists(Vble.RutaTemporal))
                        {
                            Directory.Delete(Vble.RutaTemporal, true);
                        }
                    }
                    //UltimaModificacion(fi, pictureBox1);
                }
                else
                {
                    FechaArchivoBDparaDescarga.Text = "No existen archivos en la colectora";
                    //statusStrip1.Items.Add("No existen archivos en la colectora");
                    pictureBox1.Visible = false;
                    cmbDevices.Enabled = false;
                }
            }
            catch (Exception)
            {
                
            }

        }



        

        private void backgroundDescargarAPC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //try
            //{                           
           
             

                if (e.Result is Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Ctte.ArchivoLogEnzo.EscribirLog(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "---> " + ex.Message +
                                                   " Error al finalizar proceso en segundo plano en Descargas de Colectora \n");
                    DetenerTareaSegundoPlano(backgroundDescargarAPC);
                    this.progressBar.Value = 0;
                    this.progressBar.Visible = false;
                    this.labelPorcDesc.Visible = false;
                }
            else
            {
                DirectoryInfo ColectCone = new DirectoryInfo(Vble.RutaColectoraConectada);
                //File.Delete(Vble.RutaBaseSQLiteColectora);
                //File.Delete(Vble.ArchivoInfoCargaColectora);            
                ////File.Delete(Vble.BaseChicaFIS);

                this.Invoke((MethodInvoker)delegate
                {
                    ColectCone.Refresh();
                    progressBar.Visible = false;
                    progressBar.Value = 0;
                    labelPorcDesc.Visible = false;
                    lsDesc.Items.Clear();
                    lsDesc.Dispose();


                    BaseADescargar.Close();

                    if (Directory.Exists(Vble.RutaTemporal))
                    {
                        Directory.Delete(Vble.RutaTemporal, true);
                    }

                    copiado = true;
                    Descargado = true;


                    if (RBRecCable.Checked)
                    {
                        Vble.EliminarArchivosEnColectora();
                    }
                    //progressBar.Value = progressBar.Maximum;
                    //Thread.Sleep(2000);
                    this.Cursor = Cursors.Default;
                    btnCerrar.Enabled = true;
                    this.ControlBox = true;
                    Form3Descargas form3 = new Form3Descargas();
                    form3.ControlBox = true;
                    Form0 form0 = new Form0();
                    form0.ControlBox = true;
                    btnDescargar.Enabled = true;
                    btnDescargar.Visible = true;
                    cmbDevices.Enabled = false;
                    btnResumTodo.Enabled = true;
                    btnResDia.Enabled = true;
                    btnCarpNada.Enabled = true;
                    btnInformeDia.Enabled = true;
                    btnInformePer.Enabled = true;
                    //CantConex = 0;
                    //CantCFact = 0;
                    //CantImpresor = 0;
                    //TotalRegistros = 0;
                    AvanceDescarga = 0;

                });

                MessageBox.Show("La descarga ha finalizado", "Descarga completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void btnCerrar_BackgroundImageLayoutChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Metodo que detiene la tarea en segundo plano (backgroundworker) que se esta ejecuando,
        /// recibe como parametro dicha proceso.
        /// </summary>
        /// <param name="tareasegundoplano"></param>
        private void DetenerTareaSegundoPlano(BackgroundWorker tareasegundoplano)
        {
            // Cancel the asynchronous operation.
            tareasegundoplano.CancelAsync();
            tareasegundoplano.Dispose();

            //Habilito nuevamente los botones que podian haber ocacionado interrupcion al proceso de descarga                     
            this.btnCerrar.Enabled = true;
            this.ControlBox = true;
            this.cmbDevices.Enabled = true;
            this.btnDescargar.Visible = false;
            this.btnDetener.Visible = false;
            
        }

        private void btnDetener_Click(object sender, EventArgs e)
        {
            DetenerTareaSegundoPlano(BGDescargaFTP);
        }

        private void botExpulsaColec_Click(object sender, EventArgs e)
        {
            IList<WindowsPortableDevice> devices = service.Devices;
            devices.ToList().ForEach(device =>
            {
                device.Connect();
                if (Funciones.BuscarColectora(device.ToString()))
                {
                    device.Disconnect();
                    cmbDevices.Items.Clear();
                    timer1.Stop();
                    Thread.Sleep(700);
                    toolTip2.Show("Es seguro quitar el dispositivo", cmbDevices, 3000);
                    lsDesc.Items.Clear();
                    btnDescargar.Visible = false;
                }

            });
        }

        private void BotActPanPC_Click(object sender, EventArgs e)
        {
            btnCarpNada_Click_1(sender, e);
            timer1.Start();
        }

        private void iTalk_Button_21_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "" && this.textBox2.Text != "" && this.textBox3.Text != "" )
            {
              try
               {
               //Lee y obtiene el nombre de la base Sqlite
              StringBuilder stb1 = new StringBuilder("", 100);
              Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
              string ArchivoBase = stb1.ToString();
              //Lee y obtiene el nombre de la base Sqlite
              StringBuilder stb2 = new StringBuilder("", 100);
              Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
              string ArchivoBaseChica = stb2.ToString();

              Vble.Colectora = "Documentos en MICC-4021" + "\\" + Vble.CarpetaDestinoColectora + "\\";
              Vble.RutaColectoraConectada = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;


              Vble.ArchivoInfoCargaColectora = this.textBox3.Text;
              Vble.RutaBaseSQLiteColectora = this.textBox1.Text;
              Vble.BaseChicaFIS = this.textBox2.Text;
              Vble.ColectoraConectada = "MICC-4021";
              if (File.Exists(Vble.RutaBaseSQLiteColectora))
               {
              //if (Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora) != "")
              //{
              DialogResult = MessageBox.Show("La Colectora contiene la Carga: \n" + Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora)
               + ". \n Desea comenzar la descarga de las conexiones?", "Descarga de Conexiones",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (DialogResult == DialogResult.OK)
                 {
                 lsDesc.Items.Clear();
                 this.ControlBox = false;
                 btnCerrar.Enabled = false;
                 this.btnDetener.Visible = true;
                 BGDescargaFTP.RunWorkerAsync();
                 }

               }
                else
                 {
                 MessageBox.Show("Por favor seleccione los tres archivos correspondientes para realizar la descarga",
                                "Sin archivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }

                 this.textBox1.Text = "dbFIS-DPEC.db";
                 this.textBox1.ForeColor = Color.LightGray;
                 this.textBox1.Font = new System.Drawing.Font(this.textBox1.Font, FontStyle.Italic);
                 this.textBox2.Text = "Datos_FIS.db";
                 this.textBox2.ForeColor = Color.LightGray;
                 this.textBox2.Font = new System.Drawing.Font(this.textBox1.Font, FontStyle.Italic);
                 this.textBox3.Text = "InfoCarga.txt";
                 this.textBox3.ForeColor = Color.LightGray;
                 this.textBox3.Font = new System.Drawing.Font(this.textBox1.Font, FontStyle.Italic);

                 }
                  catch (Exception R)
                 {
                  MessageBox.Show(R.Message.Substring(0, 31) + " de informacion de las conexiones", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
                        
            }
            else
            {
                MessageBox.Show("Por favor seleccione los tres archivos correspondientes para realizar la descarga", 
                                "Sin archivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void iTalk_Button_22_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox1.Text = openFileDialog1.FileName;
                this.textBox1.ForeColor = Color.Black;
            }
        }

        private void iTalk_Button_23_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox2.Text = openFileDialog1.FileName;
                this.textBox2.ForeColor = Color.Black;
            }
        }

        private void iTalk_Button_24_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox3.Text = openFileDialog1.FileName;
                this.textBox3.ForeColor = Color.Black;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "" && this.textBox2.Text != "" && this.textBox3.Text != "")
            {
                try
                {
                    //Lee y obtiene el nombre de la base Sqlite
                    StringBuilder stb1 = new StringBuilder("", 100);
                    Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                    string ArchivoBase = stb1.ToString();
                    //Lee y obtiene el nombre de la base Sqlite
                    StringBuilder stb2 = new StringBuilder("", 100);
                    Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb2, 100, Ctte.ArchivoIniName);
                    string ArchivoBaseChica = stb2.ToString();

                    Vble.Colectora = "Documentos en MICC-4001" + "\\" + Vble.CarpetaDestinoColectora + "\\";
                    Vble.RutaColectoraConectada = Vble.ValorarUnNombreRuta(Vble.DirectorioColectoraenPC) + Vble.Colectora;


                    Vble.ArchivoInfoCargaColectora = this.textBox3.Text;
                    Vble.RutaBaseSQLiteColectora = this.textBox1.Text;
                    Vble.BaseChicaFIS = this.textBox2.Text;
                    Vble.ColectoraConectada = "MICC-4001";
                    if (File.Exists(Vble.RutaBaseSQLiteColectora))
                    {
                        //if (Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora) != "")
                        //{
                        DialogResult = MessageBox.Show("La Colectora contiene la Carga: \n" + Funciones.LeerArchivostxt(Vble.ArchivoInfoCargaColectora)
                         + ". \n Desea comenzar la descarga de las conexiones?", "Descarga de Conexiones",
                          MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (DialogResult == DialogResult.OK)
                        {
                            lsDesc.Items.Clear();
                            this.ControlBox = false;
                            btnCerrar.Enabled = false;
                            this.btnDetener.Visible = true;
                            BGDescargaFTP.RunWorkerAsync();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Por favor seleccione los tres archivos correspondientes para realizar la descarga",
                                       "Sin archivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.textBox1.Text = "dbFIS-DPEC.db";
                    this.textBox1.ForeColor = Color.LightGray;
                    this.textBox1.Font = new System.Drawing.Font(this.textBox1.Font, FontStyle.Italic);
                    this.textBox2.Text = "Datos_FIS.db";
                    this.textBox2.ForeColor = Color.LightGray;
                    this.textBox2.Font = new System.Drawing.Font(this.textBox1.Font, FontStyle.Italic);
                    this.textBox3.Text = "InfoCarga.txt";
                    this.textBox3.ForeColor = Color.LightGray;
                    this.textBox3.Font = new System.Drawing.Font(this.textBox1.Font, FontStyle.Italic);

                }
                catch (Exception R)
                {
                    MessageBox.Show(R.Message.Substring(0, 31) + " de informacion de las conexiones", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Por favor seleccione los tres archivos correspondientes para realizar la descarga",
                                "Sin archivos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void CopiarColectora_a_Temporal_DoWork(object sender, DoWorkEventArgs e)
        {
            BeginInvoke(new InvokeDelegate(InvocarMetodo));           
        }

        public void InvocarMetodo()
        {
            DirectoryInfo ArchivosEnTemporal = new DirectoryInfo(Vble.RutaColectoraConectada);
            Vble.DescargarArchivosDeColectora(ArchivosEnTemporal);
        }

        private void CopiarColectora_a_Temporal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (lsDesc.Items.Count > 0)
            {
                VerDetallePreDescarga(Vble.RutaBaseSQLiteColectora, LabImprPre.Text, "2");
                
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lsDesc.Items.Count > 0)
            {
                //VerDetalleLeidasNoImpresas(Vble.RutaBaseSQLiteColectora, LabLeidNoImprePre.Text);
                VerDetallePreDescarga(Vble.RutaBaseSQLiteColectora, LabLeidNoImprePre.Text, "1");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lsDesc.Items.Count > 0)
            {
                //VerDetalleLeidasNoImpresasFueraDeRango(Vble.RutaBaseSQLiteColectora, LabNoImprFueraRango.Text);
                VerDetallePreDescarga(Vble.RutaBaseSQLiteColectora, LabNoImprFueraRango.Text, "4");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           

            if (lsDesc.Items.Count > 0)
            {
                //VerDetalleSaldos(Vble.RutaBaseSQLiteColectora, LabSaldos.Text);
                VerDetallePreDescarga(Vble.RutaBaseSQLiteColectora, LabSaldos.Text, "0");
            }
        }

        private void btnDescNada_Click(object sender, EventArgs e)
        {
            //Vble.ActualizarPanelesEnCargasDeColectoras(Vble.ArchivoInfoCargaColectora);
            //MessageBox.Show(Ctte.ArchivoIniName + Environment.NewLine + Ctte.ArchivoLogEnzo.ToString() + Environment.NewLine +
            //    Ctte.ArchivoEstructuraColectora + Environment.NewLine + Ctte.CarpetaRecursos);

            //Vble.Periodo = 201806;
            //Vble.Remesa = 3;
           
            Vble.InformesNovedades = Vble.ValorarUnNombreRuta(Vble.InformesNovedades);            
            MessageBox.Show(Vble.InformesNovedades);
           
        }

        private void button6_Click(object sender, EventArgs e)
        {

            //if (RBEnvCable.Checked == true)
            //{
            //    /////llamo al metodo que envia los archivos generados que estan en la pc como rutas procesadas y los envia
            //    /////a la colectora
            //    EnviarArchivosAColectora(Origen);
            //}
            //else if (RBEnvWifi.Checked == true)
            //{
            //    if (Vble.ExistenArchEnSeridor(Vble.ArrayZona[0].ToString(), colectora) == "NO")
            //    {
            //        Vble.EnviarArchivosAServidor(Origen.FullName, ColectoraWifi, Vble.ArrayZona[0].ToString());
            //    }
            //}

        }

        private async void bgWorkDescFacImpr_DoWork(object sender, DoWorkEventArgs e)
        {
            Task oTask = new Task(DescargaTablasImpreFacLog);
            oTask.Start();
            await oTask;
            var estado = oTask.Status;
        }

        private void botBuscarColectora_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            //timer1_Tick(sender, e);
            timer1.Start();
        }

        private void RBRecCable_CheckedChanged(object sender, EventArgs e)
        {
            if (RBRecCable.Checked)
            {
                cmbDevices.Visible = true;
                cmbDevicesWifi.Visible = false;
            }
            else if (RBRecWifi.Checked)
            {
                cmbDevices.Visible = false;
                cmbDevicesWifi.Visible = true;
            }
        }

        private void RBRecWifi_CheckedChanged(object sender, EventArgs e)
        {
            if (RBRecCable.Checked)
            {
                cmbDevices.Visible = true;
                cmbDevicesWifi.Visible = false;
                btnDescargar.Enabled = false;
                panelRem.Visible = false;
            }
            else if (RBRecWifi.Checked)
            {
                cmbDevices.Visible = false;
                cmbDevicesWifi.Visible = true;
                btnDescargar.Enabled = true;
                panelRem.Visible = true;
            }
        }
    }
}
