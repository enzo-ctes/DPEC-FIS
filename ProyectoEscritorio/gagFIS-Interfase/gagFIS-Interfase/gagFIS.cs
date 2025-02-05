/*
 * Creado por 
 * Usuario: Gerardo Graff
 * Fecha: 01/05/2015
 * Hora: 17:29
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Globalization;
using gagLogs;
using System.Data.SQLite;
using System.Collections;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using WindowsPortableDevicesLib;
using WindowsPortableDevicesLib.Domain;
using System.Threading;
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;
using System.Threading.Tasks;
using System.IO.Compression;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Google.Protobuf.WellKnownTypes;

//using System.Devices;



namespace gagFIS_Interfase {

    public enum enumGeneraPara{
        Servidor=0,
        CD        
    }
    

    
    /// <summary>
    /// Metodos para ller y escribir en archivos del tipo .ini
    /// y archivos de estructura de datos
    /// </summary>
    public static class Inis
    {
        
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section,
                string key, string def, StringBuilder retVal,
                int size, string filePath);

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string section,
            string key, string val, string filePath);


        //[DllImport("kernel32")]
        //public static extern long EscribirEnIni(string section,
        //   string key, string val, string filePath);


        /// <summary>Lee desde Archivo la estructura de la Tabla y devuelve la misma
        /// como una colección tipo Dictionary, la clave es el nombre de campo.
        /// </summary>
        /// <param name="Archivo">Ruta completa del archivo donde buscar la estructura de la tabla</param>
        /// <param name="Tabla">Nombre de la tabla, puede incluirse o no la extensión</param>
        /// <returns>Devuelve una colección dictionary con la estructura de los campos</returns>
        public static Dictionary<string,clCampoArchivo > LeerArchivoEstructura(string Archivo, string Tabla) {
            Dictionary<string, clCampoArchivo> Estructura = new Dictionary<string, clCampoArchivo>();
            clCampoArchivo Cpo;
            int i1, i2;
            string Linea, Tb;
            string[] Tbs;
            char[] sp = { '=' };
            bool Coinc = false;

            try {
                var stR = new StreamReader(Archivo);
                Linea = stR.ReadLine();
                //Busca primero el nombre de la tabla
                while(Linea != null) {
                    Coinc = false;
                    if(Linea.Contains("[") && Linea.Contains("]")) {
                        //Estan los corchetes, ver el contenido
                        i1 = Linea.IndexOf("[");
                        i2 = Linea.IndexOf("]");
                        Tb = Linea.Substring(i1+1, i2 - i1 - 1).Trim();
                        //Separa los nombres de tablas
                        Tbs = Tb.Split(sp);
                        foreach(string tb in Tbs) {
                            //Ver si coincide con el nombre de la tabla, no se tienen en cuenta mayusculas
                            if(tb.Trim().ToUpper() == Tabla.Trim().ToUpper())                               
                                Coinc = true;
                        }
                        if(Coinc) break;  //si alguno coincide sale del while

                    }
                    Linea=stR.ReadLine();
                }
                //Lee los campos
                Linea = stR.ReadLine();
                while(Linea != null) {                                      
                    if(Linea.Contains("[") && Linea.Contains("]"))
                        break;      //Si encontró otra sección sale
                    Cpo = new clCampoArchivo(Linea);
                    //Si campo tiene nombre válido, asigan el indice y carga a la colección
                    if(Cpo.Nombre != "") {
                        Cpo.Indice = Estructura.Count();
                        Estructura.Add(Cpo.Nombre, Cpo);
                    }
                    Linea = stR.ReadLine();  

                }
                stR.Close();
            }
            catch(Exception e) {
                MessageBox.Show(e.Message, "Leyendo estructura de tabla: " + Tabla);
                
            }

            
            return Estructura;
        }

        /// <summary>
        /// Equivalente al GetPrivateProfile, devuelve la cadena asociada a una clave
        /// instalada dentro de un archivo del tipo '.ini'
        /// No discrimina mayúsculas de minúsculas, e ignora espacios antes y después.
        /// </summary>
        /// <param name="Seccion">Nombre de la sección, en el archivo esta entre corchetes</param>
        /// <param name="Clave">Nombre de la clave cuyo valor se quiere recuperar, seguido de un '='.  </param>
        /// <param name="Defecto">Valor de retorno, si no encuentra la clave</param>
        /// <param name="retValor">Valor devuelto</param>
        /// <param name="Longitud">Tamaño máximo de la cadena a devolver, si se pone '0' no hay límite</param>
        /// <param name="filePath">Ruta completa del archivo donde leer</param>
        /// <returns></returns>
        public static int LeerCadenaPerfilPrivado(string Seccion,
                string Clave, string Defecto, out string retValor,
                int Longitud, string filePath) {
            string Linea;
            string secc = "";
            string key = "";
            int i1, i2, i3;

            retValor = Defecto;
            try {
                StreamReader stR = new StreamReader(filePath);
                //Busca sección
                Linea = stR.ReadLine();
                while(Linea != null) {
                    //Si hay un ';' saca lo que sigue, es comentario
                    i3 = Linea.IndexOf(";");
                    if(i3 >= 0) Linea = Linea.Substring(0, i3);
                    //Busca los '[' y ']'
                    i1 = Linea.IndexOf("[");
                    i2 = Linea.IndexOf("]");
                    if (i1>=0 && i2 > i1) {
                        secc = Linea.Substring(i1 + 1, i2 - i1 - 1).Trim().ToUpper();
                        if(secc == Seccion.Trim().ToUpper())
                            break;
                    }
                    Linea = stR.ReadLine();
                }
                //Si encontró la sección busca la clave dentro de la misma 
                if(secc == Seccion.Trim().ToUpper()) {
                    Linea = stR.ReadLine();
                    while(Linea != null) {
                        //Si hay un ';' saca lo que sigue, es comentario
                        i3 = Linea.IndexOf(";");
                        if(i3 >= 0) Linea = Linea.Substring(0, i3);
                        //Busca los '[' y ']' y si hay, índica termino la sección
                        i1 = Linea.IndexOf("[");
                        i2 = Linea.IndexOf("]");
                        if(i1 >= 0 && i2 > i1)
                            break;
                        //Busca un '=' y extrae la parte anterior
                        i3 = Linea.IndexOf("=");
                        if(i3 > 1)
                            key = Linea.Substring(0, i3).Trim().ToUpper();
                        //Verifica si es la calve buscada
                        if(key == Clave.Trim().ToUpper()) {
                            retValor = Linea.Substring(i3 + 1);
                            break;
                        }
                        Linea = stR.ReadLine();
                    }
                }
                if(Longitud > 0)
                    retValor = retValor.Substring(0, Longitud);

            }
            catch (Exception e ){
                MessageBox.Show(e.Message, "Error en Leer Clave Privada de Perfil");
            }
            
            return retValor.Length;
        }
        
    }

    

    /// <summary>
    /// Se guardan aquí todas las constantes del proyecto
    /// </summary>
    public static class Ctte{
        
        ///Constantes para los botones de GeneraPara en Form1
        public const int GenParaAltoActivo = 40;
        public const int GenParaAltoNoActivo = 25;

        ///Carpeta donde se almacenan los recursos, como iconos, y otros.
        ///Debe estar el mismo nivel que la carpeta que contiene el ejecutable
        //public static readonly string CarpetaRecursos =
        //    Directory.GetParent(Directory.GetParent(Application.ExecutablePath).FullName +
        //        "\\..\\..\\Resources") + "\\Resources";

        public static readonly string CarpetaRecursos =
           Directory.GetParent(Directory.GetParent(Application.ExecutablePath).FullName) + "\\Resources";

        ///Archivo INI de configuraciones, debe estar en una carpeta Resources
        public static readonly string ArchivoIniName = CarpetaRecursos + "\\GAG_MoverDatos.ini";       

        ///Archivo con el formato de tablas para colectora
        public static readonly string ArchivoEstructuraColectora = CarpetaRecursos + "\\gagDatos.btx";

        ///Archivo donde se guardan los logs
        public static readonly gagLog ArchivoLog = new gagLog(CarpetaRecursos + "\\gagDpec.log");

        ///Archivo donde se guardan los logs de Exportaciones
        public static readonly gagLog ArchivoLogExportacion = new gagLog(@"\\10.1.3.125\Exportacion\emrInfo.log");

        ///Logs de Errores en tiempo de ejecucuión Enzo
        public static readonly gagLog ArchivoLogEnzo = new gagLog(CarpetaRecursos + "\\LogErrores.log");

        /// <summary>
        /// Variable que contiene direccion del archivo ZONAFIS el cual contendra cada interfaz con sus correspondientes zonas 2xx
        /// </summary>
        public static readonly string ArchivoZonaFIS = "C:\\Windows\\ZonaFIS.txt";
        //public static readonly string ArchivoZonaFIS = "D:\\ZonaFIS.txt";
        public static readonly iTextSharp.text.Image imagenMINTELL = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\MacroIntell Isologo.jpg");
        public static readonly iTextSharp.text.Image imagenDPEC = iTextSharp.text.Image.GetInstance(Ctte.CarpetaRecursos + "\\LogoDPEC.jpg");
        public static readonly Paragraph fechainforme = new Paragraph("Fecha: " + DateTime.Today.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
        public static readonly Paragraph usuarioinforme = new Paragraph("Operario: " + DB.sDbUsu, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL));
        public static readonly Phrase espacio = new Phrase("\r\n");
        public static readonly Phrase titulo = new Phrase();
        public static Chunk chunkLeyenda = new Chunk("         " + Vble.leyenda,
                                     FontFactory.GetFont("Arial", 20, iTextSharp.text.Font.BOLD,
                                     new iTextSharp.text.BaseColor(0, 102, 0)));
        public static PdfPTable table = new PdfPTable(13);

    }


    /// <summary>
    /// Se guardan aqui las variables de alcance general que deban 
    /// ocuparse en mas de una clase y que deban mantenerse
    /// </summary>
    public static class Vble {

        //Objetos que se utilizan para enviar archivos a colectoras, los mismos son variables donde se almacenan las carpetas
        //para navegar dentro de las colectora.
        public static PortableDeviceFolder currentFolder = null;
        public static PortableDeviceFolder MisDocumentos = null;
        public static PortableDeviceFolder CarpetaDatosDPEC = null;
        public static IList<PortableDeviceObject> currentContent = null;
        public static string PeriodoFORM0 = "";
        public static string ServidorBD { get; set; }
        public static string centroInterfaz { get; set; }

        public static DataTable TablaLecturistas = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        public static string dniBorrado { get; set; }
        public static string passBorrado { get; set; }

        public static Int16 contRelev { get; set; }

        public static Int32 contOrdenativos { get; set; }

        //public static ArrayList lecturistas { get; set; }

        public static ArrayList lecturistas = new ArrayList();

        public static Dictionary<string, int> lectOrd = new Dictionary<string, int>();
        public static string queryInicialExpor { get; set; }
        /// <summary>
        /// Variable que va a contener el codigo del centro de interfaz donde corre la app
        /// </summary>
        public static string locCentroInterfaz { get; set; }
        /// <summary>
        /// Variable que va a contener las localidades asociadas al centro de interfaz
        /// </summary>
        public static string LocAsociadas { get; set; }
        /// <summary>
        /// Variable que va a contener todas las colectoras asignadas al centro de interfaz donde se ejecuta el instalador
        /// </summary>
        public static string colectorasCentroInterfaz { get; set; }

        private static int periodo;
        private static string empresa = "DPEC";

        public static FormLoading loading;

        public static void ShowLoading()
        {
            loading = new FormLoading();
            loading.Show();
            
        }

        public static void HideLoading()
        {
            if (loading != null)
                //loading.Dispose();
            loading.Close();
            
        }

        /// <summary>Obtiene o establece el nombre de la empresa donde se prestará el servicio.
        /// </summary>
        public static string Empresa {
            get { return empresa; }
            set { empresa = value; }
        }


        



        /// <summary> Obtiene o establece el periodo que se está procesando      
        /// </summary>
        public static int Periodo {
            get { return periodo; }
            set { periodo = value; }
        }

        /// <summary> Obtiene o establece el periodo que se tendrá en cuenta para modificar solo las conexiones que estan 
        /// procesadas y y se van a enviar a la colectora asi se cambia en la Base MySQL
        /// </summary>
        public static int PeriodoEnviados
        {
            get { return periodo; }
            set { periodo = value; }
        }

        /// <summary> Obtiene o establece la RUTA que se tendrá en cuenta para modificar solo las conexiones que estan 
        /// procesadas y y se van a enviar a la colectora asi se cambia en la Base MySQL
        /// </summary>
        public static int RutaEnviados
        {
            get { return periodo; }
            set { periodo = value; }
        }

        /// <summary> Obtiene o establece la ConexionID que se está procesando en cada registro de Importacion
        /// </summary>
        public static int ConexionID { get; set; }

        /// <summary> Obtiene o establece el Domicilio de Suministro que se está leyendo en la importación      
        /// </summary>
        public static string DomicSumin { get; set; }
        /// <summary> Obtiene o establece el periodo que se está procesando      
        /// </summary>
        public static string LocalidadSumin { get; set; }

        /// <summary> Obtiene o establece el Codigo Postal de Suminitro que se va esta leyendo en la importación      
        /// </summary>
        public static string CodPostalSumin { get; set; }

        /// <summary> Obtiene o establece el Codigo Postal de Suminitro que se va esta leyendo en la importación      
        /// </summary>
        public static string TipoInforme { get; set; }
        /// <summary> Obtiene o establece el el consumo Residual de la linea que se está leyendo en la importación      
        /// </summary>
        public static Int32 ConsumoResidual { get; set; }

        /// <summary> Obtiene o establece la CondicionIVA que se está procesando      
        /// </summary>
        public static int CondIVA { get; set; }

        /// <summary> Obtiene o establece HistoPeriodo01 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo01 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo01 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo01 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo02 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo02 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo02 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo02 { get; set; }


        /// <summary> Obtiene o establece HistoPeriodo03 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo03 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo03 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo03 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo04 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo04 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo04 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo04 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo05 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo05 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo05 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo05 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo06 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo06 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo06 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo06 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo07 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo07 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo07 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo07 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo08 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo08 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo08 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo08 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo09 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo09 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo09 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo09 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo10 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo10 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo10 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo10 { get; set; }
        /// <summary> Obtiene o establece HistoPeriodo11 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo11 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo11 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo11 { get; set; }

        /// <summary> Obtiene o establece HistoPeriodo11 que se está procesando      
        /// </summary>
        public static Int32 HistoPeriodo12 { get; set; }

        /// <summary> Obtiene o establece HistoConsumo11 que se está procesando      
        /// </summary>
        public static Int32 HistoConsumo12 { get; set; }
        /// <summary> En esta variable se indica si el campo ImpresionOBS se va a setear a 0 en caso de que el usuario que se esta importando 
        ///si ya existe con el mismo conexionID y Periodo tenga codigo de que se leyo pero no se imprimio por x motivo.
        /// </summary>
        

        ///
        /// Variable que se va a utilizar para identificar cuando el arhivo a importar es un archivo que contiene usuarios Especiales o GU.
        public static string TipoArchivo { get; set; }

        public static String[] substringHMD { get; set; }

        public static int ActualizarDatosUsuario
        {
            get { return ActualizarDatosUsuario; }
            set { ActualizarDatosUsuario = value; }
        }

        /// <summary> Obtiene o establece la fecha "DESDE" que se ingresa en el panel de exportacion en el textbocDesde para 
        /// filtrar los usuarios por fecha de lectura       
        /// </summary>
        public static string TextDesdeInformes { get; set; }

        /// <summary> Obtiene o establece la fecha "HASTA" que se ingresa en el panel de exportacion en el textbocHasta para 
        /// filtrar los usuarios por fecha de lectura       
        /// </summary>
        public static string TextHastaInformes { get; set; }

        /// <summary> Obtiene o establece la REMESA que se ingresa en el panel de exportacion en el ComboBox para 
        /// filtrar los usuarios por Ruta, Remesa y fecha de lectura.       
        /// </summary>
        public static string RemesaInformes { get; set; }

        /// <summary>
        /// Variable que contiene la Carpeta Descargada y Exportadas donde se encuentran los archivo:             /// 
        /// Ruta.txt, 
        /// Medidor.txt
        /// Medidos.txt
        /// de la colectora que se va a descargar
        ///  C:\InterfaceDPEC\Pruebas\EmpresaLocal\Exportadas\TXT\201702\MICC-4001_1021
        /// </summary> 
        public static string RutaCarpetaDescYExportadasTXT { get; set; }
        /// Variable que contiene la Carpeta Descargada y Exportadas donde se encuentra los archivos
        /// de bases de datos de las rutas que se descargaron de las colectoras: 
        /// C:\InterfaceDPEC\Pruebas\EmpresaLocal\Exportadas\Bases\201702\MICC-4001_1021
        /// Datos_FIS.db
        /// dbFIS-DPEC.db
        /// </summary> 
        public static string RutaCarpetaDescYExportadasBASES { get; set; }

        /// <summary>
        /// Variable que contiene la la Carpeta donde se encuentra el archivo Ruta.txt  de la colectora
        /// que se va a descargar
        /// </summary> 
        public static string RutaArchivoRutaTXTenPC { get; set; }

        /// Variable que contiene la Carpeta donde se encuentra el archivo Medidor.txt  de la colectora
        /// que se va a descargar
        /// </summary> 
        public static string RutaArchivoMedidorTXTenPC { get; set; }

        /// <summary>
        /// Variable que contiene la Carpeta donde se encuentra el/los archivo Medidor.txt  de la colectora
        /// que se va a descargar
        /// </summary> 
        public static ArrayList ArrayArchivosMedidorTXTenPC = new ArrayList();
                          

        /// <summary>
        /// Obtiene o establece el codigo interno de localidad, que identifica el distrito
        /// </summary>
        public static int Distrito { get; set; }

        /// <summary>
        /// Obtiene o establece el número de lote en proceso
        /// </summary>
        public static int Lote { get; set; }

        /// <summary>
        /// Obtiene o establece el número de ruta en proceso
        /// </summary>
        public static int Ruta { get; set; }

        /// <summary>
        /// Obtiene o establece el número de Secuencia en proceso
        /// </summary>
        public static int Secuencia { get; set; }

        /// <summary>
        /// Obtiene o establece el número de Particion en proceso
        /// </summary>
        public static string Particion { get; set; }

        /// <summary>
        /// Obtiene o establece el número e Remesa en proceso
        /// </summary>
        public static int Remesa { get; set; }


        /// <summary>
        /// Obtiene o establece el número de ruta en proceso
        /// </summary>
        public static int desde { get; set; }
        /// <summary>
        /// Obtiene o establece el número de ruta en proceso
        /// </summary>
        public static int hasta { get; set; }

        /// <summary>
        /// Contendra el numero de conexiones pertenecientes a la secuencia seleccionada para utilizar en archivo InfoCargas
        /// </summary>
        public static int CantConex { get; set; }

        /// <summary>
        /// Contendra el valor de la porcion seleccionado del ListView LogImportados para mostrar en el form LogImportados.
        /// </summary>
        public static string PorcionImp { get; set; }

        /// <summary>
        /// Contendra el valor del total de usuarios seleccionado del ListView LogImportados para mostrar en el form LogImportados.
        /// </summary>
        public static string TotalUsuariosImp { get; set; }

        /// <summary>
        /// Contendra el valor del total de usuarios importados seleccionado del ListView LogImportados para mostrar en el form LogImportados.
        /// </summary>
        public static string TotalImportados { get; set; }

        /// <summary>
        /// Contendra el valor del total de usuarios importados seleccionado del ListView LogImportados para mostrar en el form LogImportados.
        /// </summary>
        public static string TotalApartados { get; set; }
        
        /// <summary>
        /// Contendra el valor del IDLogImportacion seleccionado del ListView LogImportados para mostrar en el form LogImportados.
        /// </summary>
        public static string IDLogImportacion { get; set; }

        /// <summary>
        /// Variable que contiene valor si se cancelo la Importacion o no para que no elimine el archivo cuando se cancela
        /// </summary>
        public static bool CancelarImportacion { get; set; } = false;

        public static bool ExistenArchImportacion { get; set; }

        /// <summary>
        /// Contendra las rutas seleccionadas para archivo de información de carga procesada
        /// </summary>
        public static string rutas { get; set; }

        public static string lineas { get; set; }

        /// <summary>
        /// Contendra la leyenda que se colocara en el informe PDF
        /// </summary>
        public static string leyenda { get; set; }

        public static string ResumenNoImpresos { get; set; }

        //string que se utilizan para el UPLOAD
        public static string LineaHCX { get; set; }
        public static string LineaHMD { get; set; }
        public static string LineaHNC { get; set; }
        public static string LineaImpresos { get; set; }
        public static string LineaHCF { get; set; }
        public static string LineasGPS { get; set; }

        public static string LineaExportartxt { get; set; }

        /// <summary>
        /// Variables enteras que contienen las cantidades de
        /// CANTImpresos: numero de usuarios que fueron impresos
        /// CANTLecturias: numero de usuarios que fueron leidos sin contar los impresos
        /// CANTLog: numero de usuarios que devolvieron algun error y fueron registrados en LogErrores
        /// CANTSaldos: numero de usuarios sin leer de la ruta
        /// </summary>
        public static int CANTImpresos { get; set; }
        public static int CANTLecturas { get; set; }
        public static int CANTLog { get; set; }
        public static int CANTSaldos { get; set; }

        /// <summary>
        /// Linea que contendra todas las HFS una vez que se recorre la tabla
        /// </summary>
        public static string LineaHFS { get; set; }
        /// <summary>
        /// Linea que contendrá cada linea de la tabla para armar la linea HFS general.
        /// </summary>
        public static string LineaHFSindiv { get; set; } = "";
        public static string EstadoCorregido { get; set; }
        public static string EnergiaInyectada { get; set; }
        public static string EnergiaReactiva { get; set; }
        /// <summary>
        /// Linea que contendrá cada linea de la tabla para armar la linea HFS general.
        /// </summary>
        public static string LineaLogErrores { get; set; } = "";
        /// <summary>
        /// //Contendra los Nº de Instalación de las conexiones impresas
        /// </summary>
        public static ArrayList NºInstalacionImpresos = new ArrayList();
        /// <summary>
        /// //Contendra el Nº de Contrato de las conexiones impresas
        /// </summary>
        public static ArrayList ContratoImpresos = new ArrayList();
        /// <summary>
        /// Contendra el titular de las conexiones impresas
        /// </summary>
        public static ArrayList TitularImpresos = new ArrayList();
        /// <summary>
        /// Contendra el punto de venta de las conexiones impresas
        /// </summary>
        public static ArrayList FacturaImpresos = new ArrayList();
           
        /// <summary>
        /// //Contendra los Nº de Instalación de las conexiones Fuera de Rango
        /// </summary>
        public static ArrayList NºInstalacionFueraDeRango = new ArrayList();
        /// <summary>
        /// //Contendra el Nº de Contrato de las conexiones Fuera de Rango
        /// </summary>
        public static ArrayList ContratoFueraDeRango = new ArrayList();
        /// <summary>
        /// Contendra el titular de las conexiones Fuera de Rango
        /// </summary>
        public static ArrayList TitularFueraDeRango = new ArrayList();      
        /// <summary>
        /// Contendra el titular de las conexiones Fuera de Rango
        /// </summary>
        public static ArrayList ObservacionesFueraDeRango = new ArrayList();

        //public static string LineaLeidosImpresos { get; set; }
        //public static string LineaLeidosFueraDeRango { get; set; }

        /// <summary>
        /// //contiene la cantidad de nodos que se van seleccionando 
        /// </summary>
        public static int CantNodosDesde { get; set; }
        /// <summary>
        /// //contiene la informacion de la colectora conectada
        /// </summary>
        public static string Colectora { get; set; }

        public static string dispositivo { get; set; }

        //public static int CantNodosLocalidad { get; set; }
        //Contendra la cantidad de registros por Carga que se generará en el archivo SQLite  
        public static int CantRegistros { get; set; }


        /// <summary>
        ///Contendrá la cantidad de registros con codigo importacion mayor a 500
        ///es decir las conexiones que pueden ser exportadas para generar los archivos upload
        /// </summary>
        public static int CantAExportar { get; set; }

        /// <summary>
        /// Variable que contiene la ruta completa de la Carpeta Carga Generada al seleccionar en del ListView
        /// </summary>
        public static string RutaCarpetaOrigen { get; set; }
        /// <summary>
        /// Variable que contiene la FechaCalp al momento de generar el archivo upload por si viene vacio o
        /// distinto al formato permitido, se aplica el valor de ésta variable que tomaria el mismo valor 
        /// que el resto de forma correcta
        /// /// </summary>
        public static string FechaCalp { get; set; }
        /// <summary>
        /// Variable que contiene la la Carpeta Carga Generada al seleccionar en del ListView
        /// </summary>
        public static string CarpetaSeleccionada { get; set; }
        /// <summary>
        /// Variable que contiene la la Carpeta Carga Enviadas
        /// </summary> 
        public static string RutaCarpetaEnviadas { get; set; }
        /// <summary>
        /// Variable que contiene la la Carpeta donde se encuentra la base SQLite de la colectora que se va a descargar
        /// </summary> 
        public static string RutaBaseSQLiteColectora { get; set; }

        /// <summary>
        /// Variable que contiene la la Carpeta donde se encuentra la base Fija SQLite de la colectora que se va a descargar
        /// </summary> 
        public static string RutaBaseFijaSQLiteColectora { get; set; }

        /// Variable que contiene la Carpeta donde se encuentra el archivo Medidor.txt  de la colectora
        /// que se va a descargar
        /// </summary> 
        public static string RutaArchivoInfoCarga { get; set; }


        /// <summary>
        /// Variable que contiene el archivo Datos_FIS.db temporal para la comparacion de sincronizacion
        /// </summary> 
        public static string TemporalBaseFija { get; set; }

        /// <summary>
        /// Variable que contiene el archivo dbFIS_DPEC.db temporal para la comparacion de sincronizacion
        /// </summary> 
        public static string TemporalBaseVariable { get; set; }

        /// <summary>
        /// Variable que contiene el archivo Medidor.txt temporal para la comparacion de sincronizacion
        /// </summary> 
        public static string TemporalInfoCarga { get; set; }

        /// <summary>
        /// Variable que contiene la la Carpeta donde se encuentra los archivos de la colectora que se va a descargar
        /// </summary> 
        public static string RutaColectoraConectada { get; set; }


        /// <summary>
        /// Variable que contiene la la Temporal donde se encuentra los archivos de la colectora antes de sincronizar para
        /// su comparación 
        /// </summary> 
        public static string RutaTemporal = "C:\\Users\\operario\\Documents\\Temporal";

        /// <summary>
        /// Variable que contiene el nombre de infocargacolectora
        /// </summary> 
        public static string ArchivoInfoCargaColectora { get; set; }
        /// <summary>
        /// Variable que contiene el nombre de infoDescargacolectora
        /// </summary> 
        public static string ArchivoInfoDescargaColectora { get; set; }
        /// Variable que contiene el nombre de directorio donde se encuentra los archivos de descargas
        /// </summary> 
        public static string DirectorioDescarga { get; set; }
        /// <summary>
        /// Variable que contiene la Localidad/Distrito seleccionado del Lisview1
        /// </summary>
        public static string Localidad = "000";
        /// <summary>
        /// Variable que contiene la Ruta Carpeta Respaldo
        /// </summary>
        public static string RespaldoEnviadas;
        /// <summary>
        /// Variable que almacena el Punto de Venta de la Colectora que se esta descargando.
        /// </summary>
        public static string PuntoVenta;
        /// <summary>
        /// Variable que almacena el Valor del Punto de Venta de la Colectora
        /// que se esta descargando para comparar con la base general Mysql y verifica si actualiza o no el Numero.
        /// </summary>
        public static string NumeroPuntoVentaA;
        /// <summary>
        /// Variable que almacena el Valor del Punto de Venta de la Colectora
        /// que se esta descargando para comparar con la base general Mysql y verifica si actualiza o no el Numero.
        /// </summary>
        public static string NumeroPuntoVentaB;

        public static string BaseChicaColectora;
        
        public static ArrayList ArchivosAdescargar = new ArrayList();

        public static StringBuilder PanelLogImp = new StringBuilder();

        public static StringBuilder PanelLogExp = new StringBuilder();


        /// <summary>
        /// Variables de estado de impresion e Informacion de descarga que se van a usar para cargar Informes de Descargas 
        /// </summary>

        public static string ColectoraConectada { get; set; }
        public static string NomCarpDescarga { get; set; }
        public static string InfoDescarga { get; set; }
        public static int PeriodoEnColectora { get; set; }
        public static int Operario { get; set; }
        public static int ConexNoLeidas { get; set; }
        public static int ConexLeidasNoImpresas { get; set; }
        public static int ConexLeidasImpresas { get; set; }
        public static int ConexNoImpresasImpresoraDesc { get; set; }
        public static int ConexNoImpresasFueradeRango { get; set; }
        public static int ConexNoImpresasEstadoNegativo { get; set; }
        public static int ConexNoImpresasErrorDato { get; set; }
        public static int ConexNoImpresasDomicilioPostal { get; set; }
        public static int ConexNoImpresasIndicadoDato { get; set; }
        public static int ConexImposibleLeer { get; set; }
        public static int ConexSubtNeg { get; set; }
        public static int ConexErrorArchDatos { get; set; }
        public static int ConexErrorNFact { get; set; }
        public static int ConexSinConcepFacturar { get; set; }
        public static int FaltaTitular { get; set; }
        public static int ConexErrorFacturando { get; set; }
        public static int ErroEnMemoria { get; set; }
        public static int ConexPerExcDias { get; set; }
        public static int ErrorIndeterminado { get; set; }

        
        /// <summary>
        /// ArrayList que contendran las conexiones, personas o medidores que se aparten al momento de importar
        /// porque no cumplen la cantidad de campos o datos de acuerdo a la estructura especificada.
        /// </summary>
        public static ArrayList ImporConex = new ArrayList();
        public static ArrayList ImporRuta = new ArrayList();
        public static ArrayList ImporPers = new ArrayList();
        public static ArrayList ImporMed = new ArrayList();
        public static ArrayList ImporOrdenesDeLecturas = new ArrayList();
        public static Int32 CantImportados = 0;
        public static Int32 CantApartados = 0;
        /// <summary>
        /// Variable booleana que estará true si el usuario en una de las tres lineas HCX, HPS, HMD no cumple con la 
        /// cantidad de campos correspondientes a la estructura especificada en la documentación.
        /// </summary>
        public static bool ConexBloImp = false;

        public static ArrayList ArrayRutasXLoc = new ArrayList();
        public static ArrayList ArrayZona = new ArrayList();
        public static ArrayList ArrayRutasImportadas = new ArrayList();
        public static ArrayList ArrayRemesas = new ArrayList();

        /// <summary>
        /// Array donde se almacena las rutas que contiene el archivo sqlite que se va a descargar 
        /// para utilizar al momento de controlar con la base MySQL si la tabla impresor contiene registros ya descargados para esa ruta
        /// asi, si existe, elimina previamente todos los registros de impresor antes de volver a agregar los correspondientes a esa
        /// ruta verificada.
        /// </summary>
        public static ArrayList ArrayRutas = new ArrayList();

        public static GongSolutions.Shell.FilterItemEventArgs even { get; set; }

        public static string NombreArchivoImportacion { get; set; }

        public static string ArchivoImportación { get; set; }

        public static GongSolutions.Shell.ShellItem pathInformes { get; set; }


        // public static string CarpetasCargasAcargar { get; set; }

        /*         static enumGeneraPara GeneraPara = enumGeneraPara.CD;
        public static System.Drawing.Color GenParaColorActivo =
            System.Drawing.Color(214, 240, 217);
        public static System.Drawing.Color GenParaColorNoActivo =
            System.Drawing.Color(194, 220, 197);
        */
        /////////////////////////////////////////////////////////
        #region Carpetas y archivos de datos
        /// Carpeta donde se almacena los datos para intercambio con la empresa
        public static string CarpetaEmpresa { get; set; }
        /// Carpeta donde se almacenan lo datos para intercabio, con medios extraibles
        public static string CarpetaTrabajo { get; set; }
        /// Carpeta local de respaldo
        public static string CarpetaRespaldo { get; set; }
        public static string CarpetaRespaldoQAS { get; set; }
        public static string CarpetaExportacion { get; set; }
        public static string CarpetaImportacion { get; set; }
        public static string CarpetaBorradoRutas { get; set; }
        public static string CarpetaSAPImportacion { get; set; }
        public static string CarpetaSAPImportacionPRUEBA { get; set; }
        public static string CarpetaDefectoInformes { get; set; }
        public static string DownloadsHechas { get; set; }
        public static string DownloadsHechasPRUEBA { get; set; }
        public static string DownloadEntregadas { get; set; }
        public static string DownloadEntregadasPRUEBA { get; set; }
        public static string CarpetaUpload { get; set; }
        public static string CarpetaGPSPRD { get; set; }
        public static string CarpetaGPSQAS { get; set; }
        public static string CarpetaUploadSap { get; set; }
        public static string CarpetaUploadPrueba { get; set; }
        public static string CarpetaUploadProcesados { get; set; }
        public static string CarpetaInformes { get; set; }
        public static string CarpetaInformesPDF { get; set; }
        public static string InformesNovedades { get; set; }
        public static string CarpetaTemporal { get; set; }
        public static string CarpetaDescargasNoProcesadas { get; set; }
        public static string CarpetaDescargasSiProcesadas { get; set; }
        public static string CarpetaCargasNoEnviadas { get; set; }
        public static string CarpetaDescargasRecibidas { get; set; }
        public static string CarpetaInformesDescargas { get; set; }
        public static string CarpetaInformesAltas { get; set; }
        public static string CarpetaCargasSiEnviadas { get; set; }
        public static string CarpetasCargasAcargar { get; set; }
        public static string CarpetasCargasRecibidas { get; set; }
        public static string CarpetasGenerada { get; set; }
        public static string CarpetaCar_Desc_ColectorasNAS_QAS { get; set;}
        public static string CarpetaCar_Desc_ColectorasNAS_PRD { get; set; }
        public static string NomenclaturaColectora { get; set; }
        public static string DestinoArchivosColectora { get; set; }
        public static string DirectorioColectoraenPC { get; set; }
        public static string CarpetaDestinoColectora { get; set; }
        public static string CarpetaEnviadas { get; set; }
        public static string CarpetaSqlite { get; set; }        
        public static string CarpetaPeriodo { get; set; }
        public static string DominioYUsuarioRed { get; set; }
        public static string ContraseñaRed { get; set; }
        public static string IdClave { get; set; }
        public static int TotalConexiones { get; set; }
        public static string BaseChicaFIS { get; set; }
        public static string BaseChicaFISPC { get; set; }
        public static string PassDescrip { get; set; }
        public static bool EstadoCentroDispositivo { get; set; }
        public static string EnviadasColectoras { get; set; }
        public static string DescargasColectoras { get; set; }
        public static string NombreArchivoParaImprimirResumen { get; set; }
        public static bool CarpetasConfLeidas { get; set; }
        //public static string RutaArchivoBAT { get; set; }



        /// <summary>Lee desde el archivo ini, los nombres de las  carpetas
        /// y archivos donde se manipulan los datos de la empresa.
        /// </summary>
        /// <returns>Si no comete ningun error devuelve true</returns>
        public static bool LeerNombresCarpetas() {
            
            Vble.CarpetaEmpresa = LeerUnNombreDeCarpeta("Dir Empresa Servidor");
            Vble.CarpetaTrabajo = LeerUnNombreDeCarpeta("Dir Trabajo Local");
            Vble.CarpetaRespaldo = LeerUnNombreDeCarpeta("Dir Respaldo Local");
            Vble.CarpetaRespaldoQAS = LeerUnNombreDeCarpeta("Dir Respaldo Local Prueba");
            Vble.CarpetaExportacion = LeerUnNombreDeCarpeta("Dir Exportacion");
            Vble.CarpetaImportacion = LeerUnNombreDeCarpeta("Dir Importacion");
            Vble.CarpetaBorradoRutas = LeerUnNombreDeCarpeta("Dir RutasBorradas");
            Vble.CarpetaSAPImportacion = LeerUnNombreDeCarpeta("Dir SAPImportacion");
            Vble.CarpetaSAPImportacionPRUEBA = LeerUnNombreDeCarpeta("Dir SAPImportacionPRUEBA");
            Vble.CarpetaDefectoInformes = LeerUnNombreDeCarpeta("Dir DirectorioInformes");
            Vble.DownloadsHechas = LeerUnNombreDeCarpeta("Dir DownloadHechas");
            Vble.DownloadsHechasPRUEBA = LeerUnNombreDeCarpeta("Dir DownloadHechasPRUEBA");
            Vble.DownloadEntregadas = LeerUnNombreDeCarpeta("Dir DownloadEntregadas");
            Vble.DownloadEntregadasPRUEBA = LeerUnNombreDeCarpeta("Dir DownloadEntregadasPRUEBA");
            Vble.CarpetaUpload = LeerUnNombreDeCarpeta("Dir Upload");
            Vble.CarpetaGPSPRD = LeerUnNombreDeCarpeta("Dir GPSPRD");
            Vble.CarpetaGPSQAS = LeerUnNombreDeCarpeta("Dir GPSQAS");
            Vble.CarpetaCar_Desc_ColectorasNAS_QAS = LeerUnNombreDeCarpeta("Dir CarpetaCar_Desc_ColectorasNAS_QAS");
            Vble.CarpetaCar_Desc_ColectorasNAS_PRD = LeerUnNombreDeCarpeta("Dir CarpetaCar_Desc_ColectorasNAS_PRD");
            Vble.CarpetaUploadPrueba = LeerUnNombreDeCarpeta("Dir UploadPrueba");
            Vble.CarpetaUploadSap = LeerUnNombreDeCarpeta("Dir UploadSap");
            Vble.CarpetaUploadProcesados = LeerUnNombreDeCarpeta("Dir UploadProcesados");
            Vble.CarpetaInformes = LeerUnNombreDeCarpeta("Dir Informes");
            Vble.InformesNovedades = LeerUnNombreDeCarpeta("Dir InformesNovedadesPRUEBA");
            Vble.CarpetaTemporal = LeerUnNombreDeCarpeta("Dir Temporal");
            Vble.EnviadasColectoras = LeerUnNombreDeCarpeta("Dir EnviadasColectoras");
            Vble.DescargasColectoras = LeerUnNombreDeCarpeta("Dir DescargasColectoras");
            Vble.CarpetaDescargasNoProcesadas = LeerUnNombreDeCarpeta("Dir Descargas Sin Proceso");
            Vble.CarpetaDescargasSiProcesadas = LeerUnNombreDeCarpeta("Dir Descargas Procesadas");
            Vble.CarpetaCargasNoEnviadas = LeerUnNombreDeCarpeta("Dir Cargas Enviar");
            Vble.CarpetaCargasSiEnviadas = LeerUnNombreDeCarpeta("Dir Cargas Enviadas");
            Vble.CarpetasCargasAcargar = LeerUnNombreDeCarpeta("Dir ArchivosSinEnviar");
            Vble.CarpetasCargasRecibidas = LeerUnNombreDeCarpeta("Dir ArchivosRecibidos"); 
            Vble.DestinoArchivosColectora = LeerUnNombreDeCarpeta("Dir CarpetaColectora");
            Vble.DirectorioColectoraenPC = LeerUnNombreDeCarpeta("Dir Directorio Colectora en PC");
            Vble.CarpetaDestinoColectora = LeerUnNombreDeCarpeta("Dir CarpetaDestinoColectora");
            Vble.CarpetaEnviadas = LeerUnNombreDeCarpeta("Dir CarpetaEnviadas");
            Vble.CarpetaSqlite = LeerUnNombreDeCarpeta("Dir Directorio Sqlite");
            Vble.CarpetaDescargasRecibidas = LeerUnNombreDeCarpeta("Dir Descargas Recibidas");
            Vble.CarpetaInformesDescargas = LeerUnNombreDeCarpeta("Dir Informes Descargas");            
            Vble.CarpetaInformesAltas = LeerUnNombreDeCarpeta("Dir Informes Altas");
            Vble.CarpetaInformesPDF = LeerUnNombreDeCarpeta("Dir InformesPDF");
            Vble.CarpetaPeriodo = LeerUnNombreDeCarpeta("Dir Periodo");
            Vble.DominioYUsuarioRed = LeerUnNombreDeCarpeta("Dir DominioYUsuarioRed");
            Vble.ContraseñaRed = LeerUnNombreDeCarpeta("Dir ContraseñaRed");
            Vble.IdClave = LeerUnNombreDeCarpeta("Dir IDClave");
            Vble.BaseChicaFIS = LeerUnNombreDeCarpeta("Dir BaseFija");
            Vble.BaseChicaFISPC = LeerUnNombreDeCarpeta("Dir BaseFija");
            Vble.PassDescrip = LeerUnNombreDeCarpeta("Dir PassDescencriptar");
            Vble.NombreArchivoParaImprimirResumen = LeerUnNombreDeCarpeta("Dir NombreArchivoParaImprimirResumen");
            //Vble.RutaArchivoBAT = LeerUnNombreDeCarpeta("Dir RutaArchivoBAT");
          
            Vble.CarpetasConfLeidas = true;

            return true;
        }


     

        /// <summary>
        /// Metodo que tomara los archivos que se encuentran en la colectora antes de descargar y copia en la PC local para trabajarlos.
        /// </summary>
        public static void DescargarArchivosDeColectora(string cmbDevicesDesc, string CarpetaLocalDescargas)
        {
            ArchivosAdescargar.Clear();
            StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
            IList<WindowsPortableDevice> devices = services.Devices;
            devices.ToList().ForEach(device =>
            {
                device.Connect();

                if (Funciones.BuscarColectora(device.ToString()))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    var rootFolder = device.GetContents().Files;
                    currentFolder = device.GetContents((PortableDeviceFolder)rootFolder.Last());
                    var carpetas = currentFolder.Files;

                    foreach (var CarpetaDatosDpec in carpetas)
                    {
                        if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                        {
                            CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                            var Archivos = CarpetaDatosDPEC.Files;
                            if (Archivos.Count > 0)
                            {
                                foreach (PortableDeviceFile arc in Archivos)
                                {
                                    device.DownloadFile(arc, CarpetaLocalDescargas);
                                }
                            }
                        }
                    }
                    Cursor.Current = Cursors.Default;
                    //MessageBox.Show("Archivos descargados con éxito", "Proceso Terminado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    device.Disconnect();
                    ;
                }
            });
        }




        private static string LeerUnNombreDeCarpeta(string ClaveInis) {
            string arIni = Ctte.ArchivoIniName;
            string Nombre;        

            Inis.LeerCadenaPerfilPrivado("Carpetas", ClaveInis, "", out Nombre, 0, Ctte.ArchivoIniName);

            return Nombre;
        }


        public static string ExportarExcelImpresos()
        {
            string arIni = Ctte.ArchivoIniName;
            string Condicion;

            Inis.LeerCadenaPerfilPrivado("Datos", "ExportarExcel", "", out Condicion, 0, Ctte.ArchivoIniName);

            return Condicion;
        }

        /// <summary>
        /// Devuelve el nombre del archivo de la base SQLite que contiene las tablas variables (dbFIS-DPEC.db).
        /// </summary>
        /// <returns></returns>
        public static string NombreArchivoBaseSqlite()
        {
            //Lee y obtiene el archivo con la ruta donde se encuentra la base dbFIS-DPEC.db cargado despues
            //de realizar la importación                        
            StringBuilder stb3 = new StringBuilder("", 500);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb3, 500, Ctte.ArchivoIniName);
            string ArchivoSqlite = stb3.ToString();///dbFIS-DPEC.db

            return ArchivoSqlite;
        }
        
        public static string RutaArchivoBAT()
        {

            string ArchivoBat = "C:\\Users\\" + Environment.UserName + "\\CerrarUnidadesRed.bat";

            return ArchivoBat;

        }

        /// <summary>
        /// Recibe como parametro el nombre del archivo de importacion que se va a verificar si pertenece al centro de interfaz
        /// para poder importar la ruta/conexiones si le corresponde, devuelve true si le corresponde enviar a la pc para su procesamiento
        /// o false si no lo va a hacer y pasa al siguiente archivo en caso de que exista.
        /// Formato del archivo que recibe como parametro:
        /// DAAAAPP_LLLLLLLL_UUU-RRRR_YYYYMMDDHHmmss
        /// DONDE:
        /// D = Inicial de la palabra download que hace referencia al tipo de archivo que se esta procesando;
        /// AAAA= Año del periodo al cual pertenece el archivo de importacion/download;
        /// PP = Periodo del archivo vigente;
        /// LLLLLLLL = Numero de lote (el cual hoy no se esta usando);
        /// UUU = Unidad operativa y es el numero que se analizara para identificar si le pertenece importar o no;
        /// RRRR = Numero de Ruta que se va a importar o unidad de lectura;
        /// YYYY= Año de cracion del archivo;
        /// MM = Mes de creacion del archivo;
        /// HH = Hora de creacion del archivo;
        /// mm = Minuto de creacion del archivo;
        /// ss = Segundo de creacion del archivo;
        /// </summary>
        /// <returns></returns>
        public static bool IdentificarArchImport(string ArchivoImport)
        {
            bool marca = new bool();
            //Vble.ArrayZona.Clear();
            //LeerArchivoZonaFIS();
            string LocArch = ArchivoImport.Substring(17, 3);

            ///registra en la variable idUsEsp los 4 digitos que identifica a Usuarios Especiales para verificar 
            ///cuando lee este tipo de archivo
            string idUsEsp = ArchivoImport.Substring(17, 4);
            //Vble.TipoArchivo = ArchivoImport.Substring(17, 4);

            try
            {            
                foreach (string item in Vble.ArrayZona)
                {
                    if (LocArch == item.Trim(' ') || idUsEsp == item.Trim(' '))
                    {
                        marca = true;
                        ArrayRutasImportadas.Add(item.Trim(' '));
                        break;
                    }
                    else
                    {
                        marca = false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return marca;
        }

        /// <summary>
        /// Proceso que lee el archivo ZonaFIS.txt que contiene las localidades de la interfaz en el cual se esta trabajando, 
        /// la misma esta ubicada en el directorio C:\Windows\ZonaFIS.txt ubicación común para todas las interfaces de GagFIS-Interface 
        /// </summary>
        public static void LeerArchivoZonaFIS()
        {

            //Vble.ArrayZona.Clear();
            int cant = Vble.ArrayZona.Count;

            for (int i = 0; i < cant; i++)
            {
                Vble.ArrayZona.Remove(i);
            }

            try
            {
                using (StreamReader sr = new StreamReader(Ctte.ArchivoZonaFIS))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        String value = line;
                        Char delimiter = ',';
                        String[] substrings = value.Split(delimiter);

                        for (int i = 0; i < substrings.Length; i++)
                        {
                            Vble.ArrayZona.Add(substrings[i]);

                        }
                    }

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al leer el archivo que contiene informacion de localidades de la Interfaz.",
                                            "Error de Archivo Interfaz", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Consulta con iteracion para cargar la cantidad de conexiones que pertenecen a la secuencia seleccionada
        /// </summary>
        /// <returns></returns>
        public static string iteracionZona()
        {
            string where = "";
            try
            {
                for (int i = 0; i < Vble.ArrayZona.Count; i++)
                {
                    where += " OR C.Zona = " + Vble.ArrayZona[i];
                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return where;

        }

        public static string iteracionRuta()
        {
            string whereRuta = "";
            try
            {
                for (int i = 0; i < Vble.ArrayRutasXLoc.Count; i++)
                {

                    whereRuta += " OR Ruta = " + Vble.ArrayRutasXLoc[i];

                }

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + "Error Al realizar Iteración de Nodos Seleccionado", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return whereRuta;
        }



        /// <summary>
        /// Devuelve el nombre del archivo de la base Fija SQLite que contiene las tablas fijas (Datos_FIS.db).
        /// </summary>
        /// <returns></returns>
        public static string NombreArchivoBaseFijaSqlite()
        {
            //Lee y obtiene el archivo con la ruta donde se encuentra la base dbFIS-DPEC.db cargado despues
            //de realizar la importación                        
            StringBuilder stb3 = new StringBuilder("", 500);
            Inis.GetPrivateProfileString("Archivos", "BaseSqliteFija", "", stb3, 500, Ctte.ArchivoIniName);
            string ArchivoSqliteFija = stb3.ToString();///dbFIS-DPEC.db
            return ArchivoSqliteFija;

        }


        /// <summary>
        /// Devuelve el nombre del archivo de la base Fija SQLite que contiene las tablas fijas (Datos_FIS.db).
        /// </summary>
        /// <returns></returns>
        public static string NombreArchivoInfoCarga()
        {
            //Lee y obtiene el archivo con la ruta donde se encuentra la base dbFIS-DPEC.db cargado despues
            //de realizar la importación                        
            StringBuilder stb3 = new StringBuilder("", 500);
            Inis.GetPrivateProfileString("Archivos", "NombreArchivoInfo", "", stb3, 500, Ctte.ArchivoIniName);
            string ArchivoSqliteFija = stb3.ToString();///dbFIS-DPEC.db
            return ArchivoSqliteFija;

        }

        /// <summary>
        /// Variable que lee el nombre del usuario administrador ubicado en archivo .ini, 
        /// el cual posibilitará funciones que el usuario común no podrá realizar
        /// </summary>
        /// <returns></returns>
        public static string UserAdmin()
        {
            StringBuilder stb = new StringBuilder(50);
            Inis.GetPrivateProfileString("Datos", "UserPrincipal", "", stb, 50, Ctte.ArchivoIniName);
            string User = stb.ToString();
            return User;
        }

        /// <summary>
        /// Variable que lee el nombre del usuario administrador ubicado en archivo .ini, 
        /// el cual posibilitará funciones que el usuario común no podrá realizar
        /// </summary>
        /// <returns></returns>
        public static string UserFTP()
        {
            StringBuilder stb = new StringBuilder(50);
            Inis.GetPrivateProfileString("Datos", "UserFtp", "", stb, 50, Ctte.ArchivoIniName);
            string UserFTP = stb.ToString();
            return UserFTP;
        }
        /// <summary>
        /// Variable que lee el nombre del usuario administrador ubicado en archivo .ini, 
        /// el cual posibilitará funciones que el usuario común no podrá realizar
        /// </summary>
        /// <returns></returns>
        public static string PassAdmin()
        {
            StringBuilder stb = new StringBuilder(50);
            Inis.GetPrivateProfileString("Datos", "PassPrincipal", "", stb, 50, Ctte.ArchivoIniName);
            string Pass = stb.ToString();
            return Pass;
        }

        /// <summary>
        /// Variable que lee el nombre del usuario administrador ubicado en archivo .ini, 
        /// el cual posibilitará funciones que el usuario común no podrá realizar
        /// </summary>
        /// <returns></returns>
        public static string OperAdmin()
        {
            StringBuilder stb = new StringBuilder(50);
            Inis.GetPrivateProfileString("Datos", "OperAdmin", "", stb, 50, Ctte.ArchivoIniName);
            string User = stb.ToString();
            return User;
        }

        /// <summary> Devuelve el nombre de ruta cuando se le pasa una cadena que contiene nombres
        /// de Variables de Sistema, tales como 'Distrito', 'Zona', 'Periodo', etc. El parámetro debe 
        /// seguir normas en su sintaxis, y cada variable es remplazada por su valor vigente al
        /// momento de procesar, y de acuerdo con el formato solicitado dentro del parametro
        /// </summary>
        /// <param name="Ruta">Nombre de la ruta con las variables y formatos incluidos.</param>
        /// <returns>Devuelve el nombre de la ruta con las variables remplazadas por su valor.</returns>
        public static string ValorarUnNombreRuta(string Ruta) {
            string VarNom = "";
            List<string> VarVal = new List<string>();


            //ArchivoTabla = Vble.CarpetaTrabajo + "\\" + Vble.CarpetaCargasNoEnviadas + "\\" + SubCarp + "\\";

            //remplaza las variables dentro de la cadena
            int i1, i2, i3;  // i1:'{'  -  i2:';'  -  i3:'}'
            i1 = Ruta.IndexOf("{");
            while(i1 >= 0) {
                i3 = Ruta.IndexOf("}", i1);       //Busca cierre llave
                if(i3 > i1) {
                    i2 = Ruta.IndexOf(":", i1, i3 - i1);  //Busca dos puntos
                    if(i2 < i1) i2 = i3;
                    VarNom = Ruta.Substring(i1 + 1, i2 - i1 - 1);
                    VarVal.Add(VarNom);

                    Ruta = Ruta.Substring(0, i1 + 1) + 
                            (VarVal.Count - 1).ToString().Trim() +
                            Ruta.Substring(i2);
                }
                i1 = Ruta.IndexOf("{", i1 + 1);
            }  //Hasta aca se tiene la cadena de formato
           // MessageBox.Show(VarVal[0]);
            //Generar el arreglo con los datos de remplazo
            object[] VarDat = new object[VarVal.Count];
            #region For_Variable
            for(int i = 0; i < VarVal.Count; i++) {
                switch (VarVal[i].Trim().ToUpper()) {
                    case "EMPRESA":
                        VarDat[i] = Empresa;
                        break;
                    case "PERIODO":
                        VarDat[i] = DateTime.ParseExact(Periodo.ToString("000000"), "yyyyMM", CultureInfo.CurrentCulture); 
                        break;
                    case "AHORA":
                        VarDat[i] = DateTime.Now;
                        break;
                    case "LOTE":
                        VarDat[i] = Lote;
                        break;
                    case "ZONA":
                    case "DISTRITO":
                        VarDat[i] = Distrito;                        
                        break;
                    case "REMESA":
                        VarDat[i] = Remesa;
                        break;
                    case "RUTA":
                        VarDat[i] = Vble.Ruta;
                        break;
                    case "DOCUMENTOS EN MICC-":
                        VarDat[i] = Vble.Colectora;
                        break;
                    case "FECHA":
                        VarDat[i] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        break;
                    case "USUARIO":
                        VarDat[i] = Environment.UserName;
                        break;
                    default:
                        VarDat[i] = "";
                        break;

                }   
            }
            #endregion For_Variable

            return string.Format(Ruta, VarDat);
        }

        public static DataTable TablaConexSelec = new DataTable();

 




        #endregion Carpetas y archivos de datos
        /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////
        #region Estructura de las tablas de datos de Colectora

        public static Dictionary<string, clCampoArchivo> cposAltas { get; set; }
        public static Dictionary<string, clCampoArchivo> cposAlumbrado { get; set; }
        public static Dictionary<string, clCampoArchivo> cposComprobantes { get; set; }
        public static Dictionary<string, clCampoArchivo> cposConceptosDatos { get; set; }
        public static Dictionary<string, clCampoArchivo> cposConceptosFacturados { get; set; }
        public static Dictionary<string, clCampoArchivo> cposConceptosFijos { get; set; }
        public static Dictionary<string, clCampoArchivo> cposConceptosTarifa { get; set; }
        public static Dictionary<string, clCampoArchivo> cposCondicionIVA { get; set; }
        public static Dictionary<string, clCampoArchivo> cposConexiones { get; set; }
        public static Dictionary<string, clCampoArchivo> cposExcepciones { get; set; }
        public static Dictionary<string, clCampoArchivo> cposGeneral { get; set; }
        public static Dictionary<string, clCampoArchivo> cposLecturistas { get; set; }
        public static Dictionary<string, clCampoArchivo> cposLocalidades { get; set; }
        public static Dictionary<string, clCampoArchivo> cposMedidores { get; set; }
        public static Dictionary<string, clCampoArchivo> cposNovedades { get; set; }
        public static Dictionary<string, clCampoArchivo> cposNovedadesConex { get; set; }
        public static Dictionary<string, clCampoArchivo> cposPersonas { get; set; }
        public static Dictionary<string, clCampoArchivo> cposTextosVarios { get; set; }
        public static Dictionary<string, clCampoArchivo> cposVencimientos { get; set; }
        public static string PathFileName { get; set; }
        public static string FileName { get; set; }
        public static string NombreArchivo { get; set; }
        


        #endregion Estructura de las tablas de datos de Colectora
        /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////
        #region METODOS en la clase Vble     



        /// <summary>
        /// Borra las tablas con las conexiones que tengan Codigo de Impresion 500 o solo aquellas que fueron leidas e Impresas
        /// es decir descargadas desde FIS segun seleccion,
        /// Si selecciono Todo del panel Opciones de Exportacion y SI del panel exportar Saldos, exportara todas las rutas
        /// y todas las conecciones que tengan codigo 5xx es decir descargadas desde FIS
        /// Si selecciono Rutas del panle de opciones de Exportacion limpiara las rutas seleccionadas segun lo que este seleccionado
        /// del panel Exportar Saldos
        /// </summary>
        /// <param name="txSQL"></param>
        public static void LimpiarTablas(string txSQL)
        {
            string DeleteConexiones, DeletePersonas, DeleteMedidres, DeleteTextosVarios,
                   DeleteNovedadesConex, DeleteInfoConex, DeleteConceptosDatos, DeleteConceptosFacturados;

            DataTable Tabla = new DataTable();
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            Form1Inicio Inicio = new Form1Inicio();
            if (Tabla.Rows.Count > 0)
            {

                Inicio.Cursor = Cursors.WaitCursor;
                foreach (DataRow item in Tabla.Rows)
                {
                    ////Elimina Registros al Exportar de la tabla Personas
                    //DeletePersonas = "delete from personas where personaID > 0";
                    //MySqlCommand cmdSQL2 = new MySqlCommand(DeletePersonas, DB.conexBD);
                    //cmdSQL2.ExecuteNonQuery();
                    //cmdSQL2.Dispose();

                    //Elimina Registros al Exportar de la tabla Personas
                    DeletePersonas = "delete from personas where (personaID = " + item.Field<Int32>("ConexionID") + " OR " + "personaID = " + item.Field<Int32>("TitularID") + ") AND (Periodo = " + item.Field<Int32>("Periodo") + ")";
                    MySqlCommand cmdSQL2 = new MySqlCommand(DeletePersonas, DB.conexBD);
                    cmdSQL2.ExecuteNonQuery();
                    cmdSQL2.Dispose();

                    ////Elimina Registros al Exportar de la tabla Medidores
                    //DeleteMedidres = "delete from medidores where ConexionId > 0";
                    //MySqlCommand cmdSQL3 = new MySqlCommand(DeleteMedidres, DB.conexBD);
                    //cmdSQL3.ExecuteNonQuery();
                    //cmdSQL3.Dispose();

                    //Elimina Registros al Exportar de la tabla Medidores
                    DeleteMedidres = "delete from medidores where ConexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL3 = new MySqlCommand(DeleteMedidres, DB.conexBD);
                    cmdSQL3.ExecuteNonQuery();
                    cmdSQL3.Dispose();

                    //Elimina Registros al Exportar de la tabla textosvarios
                    //DeleteTextosVarios = "delete from textosvarios where ConexionId > 0";
                    //MySqlCommand cmdSQL4 = new MySqlCommand(DeleteTextosVarios, DB.conexBD);
                    //cmdSQL4.ExecuteNonQuery();
                    //cmdSQL4.Dispose();

                    //Elimina Registros al Exportar de la tabla textosvarios
                    DeleteTextosVarios = "delete from textosvarios where ConexionId = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL4 = new MySqlCommand(DeleteTextosVarios, DB.conexBD);
                    cmdSQL4.ExecuteNonQuery();
                    cmdSQL4.Dispose();

                    ////Elimina Registros al Exportar de la tabla Novedades Conexion
                    //DeleteNovedadesConex = "delete from novedadesconex Where ConexionId > 0";
                    //MySqlCommand cmdSQL5 = new MySqlCommand(DeleteNovedadesConex, DB.conexBD);
                    //cmdSQL5.ExecuteNonQuery();
                    //cmdSQL5.Dispose();

                    //Elimina Registros al Exportar de la tabla Novedades Conexion
                    DeleteNovedadesConex = "delete from novedadesconex Where ConexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL5 = new MySqlCommand(DeleteNovedadesConex, DB.conexBD);
                    cmdSQL5.ExecuteNonQuery();
                    cmdSQL5.Dispose();

                    ////Elimina Registros al Exportar de la tabla conceptosdatos
                    //DeleteConceptosDatos = "delete from conceptosdatos Where ConexionId > 0";
                    //MySqlCommand cmdSQL6 = new MySqlCommand(DeleteConceptosDatos, DB.conexBD);
                    //cmdSQL6.ExecuteNonQuery();
                    //cmdSQL6.Dispose();

                    //Elimina Registros al Exportar de la tabla conceptosdatos
                    DeleteConceptosDatos = "delete from conceptosdatos Where ConexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL6 = new MySqlCommand(DeleteConceptosDatos, DB.conexBD);
                    cmdSQL6.ExecuteNonQuery();
                    cmdSQL6.Dispose();

                    //Elimina Registros al Exportar de la tabla ConceptosFacturados
                    DeleteConceptosFacturados = "delete from conceptosfacturados Where conexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL8 = new MySqlCommand(DeleteConceptosFacturados, DB.conexBD);
                    cmdSQL8.ExecuteNonQuery();
                    cmdSQL8.Dispose();

                    //Elimina Registros al Exportar de la tabla InfoConex
                    DeleteInfoConex = "delete from infoconex Where ConexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL7 = new MySqlCommand(DeleteInfoConex, DB.conexBD);
                    cmdSQL7.ExecuteNonQuery();
                    cmdSQL7.Dispose();

                    //Elimina Registros al Exportar de la tabla Conexiones
                    DeleteConexiones = "delete from conexiones where ConexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL1 = new MySqlCommand(DeleteConexiones, DB.conexBD);
                    cmdSQL1.ExecuteNonQuery();
                    cmdSQL1.Dispose();
                    
                }
                Inicio.Cursor = Cursors.Default;
                MessageBox.Show("Las conexiones pertenecientes al periodo ingresado se borraron correctamente", "Conexiones Borradas",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se encontro ningun registro con los parametros ingresados", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// Borra los datos de la tabla conceptos facturados de la ruta y el periodo ingresado datos para poder realziar una nueva carga
        /// </summary>
        /// <param name="txSQL"></param>
        public static void LimpiarConceptosFacturados(string txSQL)
        {
            string DeleteConceptosFacturados;

            DataTable Tabla = new DataTable();
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);
            Form1Inicio Inicio = new Form1Inicio();
            if (Tabla.Rows.Count > 0)
            {

                Inicio.Cursor = Cursors.WaitCursor;
                foreach (DataRow item in Tabla.Rows)
                {                   
                    //Elimina Registros al Exportar de la tabla ConceptosFacturados
                    DeleteConceptosFacturados = "delete from conceptosfacturados Where conexionID = " + item.Field<Int32>("ConexionID") + " AND Periodo = " + item.Field<Int32>("Periodo");
                    MySqlCommand cmdSQL8 = new MySqlCommand(DeleteConceptosFacturados, DB.conexBD);
                    cmdSQL8.ExecuteNonQuery();
                    cmdSQL8.Dispose();                   
                }
                Inicio.Cursor = Cursors.Default;
                MessageBox.Show("Las conexiones pertenecientes al periodo ingresado se borraron correctamente", "Conexiones Borradas",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se encontro ningun registro con los parametros ingresados", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Metodo que busca constantemente que el centro de dispositivo de windows Mobile Center este activado para 
        /// reconocer las colectoras, el mismo se necesitó hacer debido a que las colectoras DT-X200-10E no se sincornizaba
        /// automaticamente como las DT-X10
        /// </summary>       
        public static void HabilitarCentroDeDispositivo()
        {
            Process[] localAll = Process.GetProcesses();
            Process myProcess = new Process();


            foreach (Process item in localAll)
            {

                if (item.ProcessName == "wmdc")
                {
                    Vble.EstadoCentroDispositivo = true;
                    item.CloseMainWindow();

                    return;

                }
                else
                {
                    Vble.EstadoCentroDispositivo = false;

                }
            }


            if (EstadoCentroDispositivo == false)
            {
                myProcess.StartInfo.UseShellExecute = false;
                // You can start any process, HelloWorld is a do-nothing example.
                myProcess.StartInfo.FileName = "C:\\Windows\\WindowsMobile\\wmdc.exe";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                //EstadoCentroDispositivo = true;
            }
        }

        /// <summary>
        /// Metodo que corrobora que existan los 3 archivos en el servidor FTP de Macro intell en Wiroos
        /// </summary>
        /// <param name="Localidad"></param>
        /// <param name="Periodo"></param>
        /// <param name="Remesa"></param>
        /// <param name="FechaDescarga"></param>
        /// <param name="Colectora"></param>
        /// <param name="Temporal"></param>
        /// <returns></returns>
        public static async Task<int> CantFilesInServer(string Localidad, string Periodo, string Remesa, string FechaDescarga, string Colectora, string Temporal)
        {

            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            int Existe = 0;

            //Task<string> Existe = new Task<string>("NO");
            try
            {
                //string RutaVerificar = Localidad + "/" + Colectora;
                string RutaVerificar = ficFTP + Localidad + "/" + Periodo + "/" + Remesa + "/" + FechaDescarga + "/" + Colectora + "/" ;
                //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

                // Obtiene el objeto que se utiliza para comunicarse con el servidor.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string file = streamReader.ReadLine();
                //Obtiene el contenido y lo agrega al List<string>.
                while (!string.IsNullOrEmpty(file))
                {
                    if (file == "." || file == "..")
                    {
                        file = streamReader.ReadLine();
                    }
                    else
                    {
                        if (file == "InfoCarga.txt" || file == "Datos_FIS.db" || file == "dbFIS-DPEC.db" || file.Contains(".zip"))
                        {
                            directories.Add(file);
                            file = streamReader.ReadLine();
                        }

                    }
                }

                if (directories.Count() == 3)
                {
                    Existe = 3;
                }
                else if (directories.Count() == 1)
                {
                    if (directories[0].Contains(".zip"))
                    {
                        Existe = 1;
                    }

                }
                else
                {
                    Existe = directories.Count();
                }
                streamReader.Close();
                response.Close();

                return Existe;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al conectar con servidor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            return Existe;
            //return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Metodo que corrobora que existan los 3 archivos en el servidor FTP de Macro intell en Wiroos
        /// </summary>
        /// <param name="Localidad"></param>
        /// <param name="Periodo"></param>
        /// <param name="Remesa"></param>
        /// <param name="FechaDescarga"></param>
        /// <param name="Colectora"></param>
        /// <param name="Temporal"></param>
        /// <returns></returns>
        public static async Task<string> ExistenArchEnServidor(string Localidad, string Periodo, string Remesa, string FechaDescarga, string Colectora, string Temporal)
        {
            
            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            string Existe = "NO";
           
            //Task<string> Existe = new Task<string>("NO");
            try
            {        
                //string RutaVerificar = Localidad + "/" + Colectora;
                string RutaVerificar = ficFTP + Localidad + "/" + Periodo + "/" + Remesa  + "/" + FechaDescarga + "/" + Colectora + "/" ;
                //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

                // Obtiene el objeto que se utiliza para comunicarse con el servidor.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse response =  (FtpWebResponse) await request.GetResponseAsync();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string file = streamReader.ReadLine();
                //Obtiene el contenido y lo agrega al List<string>.
                while (!string.IsNullOrEmpty(file))
                {
                    if (file == "." || file == "..")
                    {
                        file = streamReader.ReadLine();
                    }
                    else
                    {
                        if (file == "InfoCarga.txt" || file == "Datos_FIS.db" || file == "dbFIS-DPEC.db")
                        {
                            directories.Add(file);
                            file = streamReader.ReadLine();
                        }
                        
                    }
                }

                if (directories.Count() == 3)
                {
                    Existe = "SI";
                }
                else
                {
                    Existe = "NO";
                }
                streamReader.Close();
                response.Close();

                return Existe;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al conectar con servidor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               
            }
            return Existe;
            //return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Metodo que corrobora que el archivo .zip que contiene la ruta exista en el servidor FTP de Macro intell en Wiroos        /// 
        /// </summary>
        /// <param name="Localidad"></param>
        /// <param name="Periodo"></param>
        /// <param name="Remesa"></param>
        /// <param name="FechaDescarga"></param>
        /// <param name="Colectora"></param>
        /// <param name="Temporal"></param>
        /// <returns></returns>
        public static async Task<string> ExisteArchZipEnServidor(string Localidad, string Periodo, string Remesa, string FechaDescarga, string Colectora, string Temporal)
        {

            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            string Existe = "NO";

            //Task<string> Existe = new Task<string>("NO");
            try
            {
                //string RutaVerificar = Localidad + "/" + Colectora;
                string RutaVerificar = ficFTP + Localidad + "/" + Periodo + "/" + Remesa + "/" + FechaDescarga + "/" + Colectora + "/";
                //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

                // Obtiene el objeto que se utiliza para comunicarse con el servidor.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string file = streamReader.ReadLine();
                //Obtiene el contenido y lo agrega al List<string>.
                while (!string.IsNullOrEmpty(file))
                {
                    if (file == "." || file == "..")
                    {
                        file = streamReader.ReadLine();
                    }
                    else
                    {
                        if (file.Contains(".zip"))
                        {
                            directories.Add(file);
                            file = streamReader.ReadLine();
                        }

                    }
                }

                if (directories.Count() == 1)
                {
                    Existe = "SI";
                }
                else
                {
                    Existe = "NO";
                }
                streamReader.Close();
                response.Close();

                return Existe;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al conectar con servidor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            return Existe;
            //return Task.FromResult(string.Empty);
        }


        public static int VerificarRegistrosEnBaseMysql()
        {
            string txSQL = "select Count(*) From Conexiones";
            MySqlCommand da = new MySqlCommand(txSQL, DB.conexBD);
            int count = Convert.ToInt32(da.ExecuteScalar());
            da.Dispose();
            return count;
        }

        public static string ExistenArchEnServidor(string Localidad, string Colectora, string Proceso)
        {

            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/" +Proceso + "/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            string Existe = "NO";

            //string RutaVerificar = Localidad + "/" + Colectora;
            string RutaVerificar = ficFTP + Localidad + "/" + Periodo + "/" + Remesa  + "/" + Colectora;
            //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

            // Obtiene el objeto que se utiliza para comunicarse con el servidor.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
            request.Credentials = new NetworkCredential(user, pass);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            List<string> directories = new List<string>();

            string file = streamReader.ReadLine();
            //Obtiene el contenido y lo agrega al List<string>.
            while (!string.IsNullOrEmpty(file))
            {
                if (file == "." || file == "..")
                {
                    file = streamReader.ReadLine();
                }
                else
                {
                    if (file == "InfoCarga.txt" || file == "Datos_FIS.db" || file == "dbFIS-DPEC.db")
                    {
                        directories.Add(file);
                        file = streamReader.ReadLine();
                    }

                }
            }

            if (directories.Count() == 3)
            {
                Existe = "SI";
            }
            else
            {
                Existe = "NO";
            }
            streamReader.Close();
            response.Close();

            return Existe;
        }

        public static string ExistenArchEnServCargas(string Localidad, string Colectora, string Proceso)
        {

            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/" +Proceso + "/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            string Existe = "NO";

            //string RutaVerificar = Localidad + "/" + Colectora;
            string RutaVerificar = ficFTP + Localidad + "/" + Colectora + "/";
            //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

            // Obtiene el objeto que se utiliza para comunicarse con el servidor.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
            request.Credentials = new NetworkCredential(user, pass);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            List<string> directories = new List<string>();

            string file = streamReader.ReadLine();
            //Obtiene el contenido y lo agrega al List<string>.
            while (!string.IsNullOrEmpty(file))
            {
                if (file == "." || file == "..")
                {
                    file = streamReader.ReadLine();
                }
                else
                {
                    if (file == "InfoCarga.txt" || file == "Datos_FIS.db" || file == "dbFIS-DPEC.db")
                    {
                        directories.Add(file);
                        file = streamReader.ReadLine();
                    }

                }
            }

            if (directories.Count() == 3)
            {
                Existe = "SI";
            }
            else
            {
                Existe = "NO";
            }
            streamReader.Close();
            response.Close();

            return Existe;
        }


        /// <summary>
        /// Metodo que envia los archivos que se procesaron para la carga a la colectora conectada
        /// </summary>
        public static bool DescargarArchivosDeColectora(DirectoryInfo RutaCarpetaTemporal)
        {
            bool descargar = false;
            try
            {
               
                Cursor.Current = Cursors.WaitCursor;
                StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
                IList<WindowsPortableDevice> devices = services.Devices;
                devices.ToList().ForEach(device =>
                {
                device.Connect();

                    if (Funciones.BuscarColectora(device.ToString()))
                    {
                            var rootFolder = device.GetContents().Files;
                            currentFolder = device.GetContents();
                            var carpetas = currentFolder.Files;

                        foreach (var CarpetaAlmacIntComp in carpetas)
                        {
                            if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
                            {
                                currentFolder = device.GetContents((PortableDeviceFolder)CarpetaAlmacIntComp);
                                carpetas = currentFolder.Files;

                                foreach (var CarpetaDatosDpec in carpetas)
                                {
                                    if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                                    {

                                        CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                                        var Archivos = CarpetaDatosDPEC.Files;

                                        //Este For se encarga de descargar los archivos 
                                        //que se encuentran en la colectora a la carpeta temporal de la PC si tuviera alguna ruta cargada
                                        //para comenzar con la descarga del mismo
                                        if (Archivos.Count > 1)
                                        {
                                            
                                            foreach (PortableDeviceFile item in Archivos)
                                            {
                                                //device.DownloadFile(item, RutaCarpetaTemporal + "\\");
                                                device.GetFile(item, RutaCarpetaTemporal + "\\");

                                            }

                                            descargar = true;
                                        }
                                    }
                                }
                            }
                            //else if (CarpetaAlmacIntComp.Name == "\\")
                            //{
                            //    string NombreArchivoRuta = "";
                            //    Vble.RutaCarpetaDescYExportadasTXT = Vble.LeerUnNombreDeCarpeta("Dir Ruta DescYExportadasTXT");
                            //    Vble.RutaCarpetaDescYExportadasBASES = Vble.LeerUnNombreDeCarpeta("Dir Ruta DescYExportadasBASES") + "\\" +
                            //                                           DateTime.Now.ToString("dd-MM-yyyy") + "\\" + device.ToString() + "\\";

                            //    if (!Directory.Exists(Vble.RutaCarpetaDescYExportadasBASES))
                            //    {
                            //        Directory.CreateDirectory(Vble.RutaCarpetaDescYExportadasBASES);
                            //    }
                            //    DirectoryInfo CarpetaArchivosColectora = new DirectoryInfo(Vble.RutaCarpetaDescYExportadasBASES);

                            //    foreach (var item in CarpetaArchivosColectora.GetFiles())
                            //    {
                            //        File.Delete(item.FullName);
                            //    }

                            //    Vble.DescargarArchivosDeColectora(device.ToString(), RutaTemporal + "\\");
                            //    //Vble.DescargarArchivosDeColectora(cmbDevicesDesc.Text, "F:\\");

                            //}
                        }
                        device.Disconnect();
                    }
                });
            
                Cursor.Current = Cursors.Default;
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + ". Error en el método Descargar Archivos de Colectoras");
            }

            return descargar;
        }


        /// <summary>
        /// Metodo que tomara los archivos que se encuentran en la colectora y los elimina, excepto a la base chica, quien 
        /// quedará en la colectora para que se pueda seguir manejando el cierre de la aplicación el cual necesita la tabla 
        /// lecturistas que se encuentran en dicha base.            
        /// </summary>
        public static void EliminarArchivosEnColectora()
        {

            StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
            IList<WindowsPortableDevice> devices = services.Devices;
            devices.ToList().ForEach(device =>
            {
                device.Connect();

                if (Funciones.BuscarColectora(device.ToString()))
                {
                    var rootFolder = device.GetContents().Files;
                    currentFolder = device.GetContents();
                    var carpetas = currentFolder.Files;

                    foreach (var CarpetaAlmacIntComp in carpetas)
                    {
                        if (CarpetaAlmacIntComp.Name == "Almacenamiento interno compartido")
                        {
                            currentFolder = device.GetContents((PortableDeviceFolder)CarpetaAlmacIntComp);
                            carpetas = currentFolder.Files;

                            foreach (var CarpetaDatosDpec in carpetas)
                            {
                                if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                                {

                                    CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                                    var Archivos = CarpetaDatosDPEC.Files;
                                    //Pregunto si existen archivos antes de enviar la nueva carga, 
                                    //si existe elimina lo que habia para poder copiar la nueva carga
                                    //si no existen archivos pasa el if y no elimina nada.
                                    if (Archivos.Count > 0)
                                    {

                                        foreach (PortableDeviceFile arc in Archivos)
                                        {
                                            if (arc.Name.Contains("."))
                                            {
                                                if (arc.Name != "Datos_FIS.db")
                                                {
                                                    device.DeleteFile(arc);
                                                }
                                            }
                                            else
                                            {
                                                if (arc.Name != "Datos_FIS")
                                                {
                                                    device.DeleteFile(arc);
                                                }
                                            }
                                          
                                        }
                                    }
                                }
                            }


                            //if (Directory.Exists(Vble.RutaTemporal))
                            //{
                            //    Directory.Delete(Vble.RutaTemporal, true);
                            //}

                            ////Este For se encarga de cargar los archivos procesados con la nueva carga
                            ////que se encuentran almacenados en un arraylist, en donde cada elemento
                            ////contiene el nombre del archivo a enviar con la ubicación del mismo

                        }
                        //    }
                        //}
                        //device.Disconnect();

                        //if (Directory.Exists(Vble.RutaTemporal))
                        //{
                        //    Directory.Delete(Vble.RutaTemporal, true);
                        //}

                    }
                }
            });
        }




        /// <summary>
        /// Metodo que envia los archivos que se procesaron para la carga a la colectora conectada
        /// </summary>
        public static void EnviarArchivosAColectora(DirectoryInfo RutaCarpetaOrigen)
        {
            try
            {
                StandardWindowsPortableDeviceService services = new StandardWindowsPortableDeviceService();
                IList<WindowsPortableDevice> devices = services.Devices;
                devices.ToList().ForEach(device =>
                {
                    device.Connect();

                    if (Funciones.BuscarColectora(device.ToString()))
                    {
                        var rootFolder = device.GetContents().Files;
                        currentFolder = device.GetContents((PortableDeviceFolder)rootFolder.Last());
                        var carpetas = currentFolder.Files;

                        foreach (var CarpetaDatosDpec in carpetas)
                        {
                            if (CarpetaDatosDpec.Name.Contains("Datos DPEC"))
                            {

                                CarpetaDatosDPEC = device.GetContents((PortableDeviceFolder)CarpetaDatosDpec);
                                var Archivos = CarpetaDatosDPEC.Files;

                                //Pregunto si existen archivos antes de enviar la nueva carga, 
                                //si existe elimina lo que habia para poder copiar la nueva carga
                                //si no existen archivos pasa el if y no elimina nada.
                                if (Archivos.Count > 0)
                                {
                                    foreach (PortableDeviceFile arc in Archivos)
                                    {
                                        device.DeleteFile(arc);

                                    }
                                }
                                //Este For se encarga de cargar los archivos procesados con la nueva carga
                                //que se encuentran almacenados en un arraylist, en donde cada elemento
                                //contiene el nombre del archivo a enviar con la ubicación del mismo
                                foreach (var item in RutaCarpetaOrigen.GetFiles())
                                {
                                    device.TransferContentToDevice(item.FullName, CarpetaDatosDpec.Id);                                                                    
                                }

                                //for (int i = 0; i < ArchivosProcesados.Count; i++)
                                //{
                                //    device.TransferContentToDevice(ArchivosProcesados[i].ToString(), CarpetaDatosDpec.Id);
                                //}
                            }
                        }
                        //}
                        //}
                        device.Disconnect();
                    }
                });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Metodo que envia los archivos que se procesaron para la carga a la colectora conectada
        /// </summary>
        public static string EnviarArchivosAServidor(string Archivos, string DirectorioColecEnServidor, string Localidad)
        {
            string Cargado = "NO";
            try
            {
                string user = Vble.UserFTP();
                string pass = Vble.PassAdmin();
                string ServerDownloadPath = "ftp://macrointell.com.ar/DPEC-FIS/Cargas/" + Localidad + "/" + DirectorioColecEnServidor + "/";
                DirectoryInfo Archivo = new DirectoryInfo(Archivos);

                foreach (var item in Archivo.GetFiles())
                {
                    if (item.Name == "InfoCarga.txt" || item.Name == "dbFIS-DPEC.db" || item.Name == "Datos_FIS.db" ||
                        item.Name == "InfoCarga" || item.Name == "dbFIS-DPEC" || item.Name == "Datos_FIS")
                    {
                        FileInfo objFile = new FileInfo(item.FullName);
                        FtpWebRequest objFTPRequest;
                        // Create FtpWebRequest object 
                        objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ServerDownloadPath + objFile.Name));
                        // Set Credintials
                        objFTPRequest.Credentials = new NetworkCredential(user, pass);
                        // By default KeepAlive is true, where the control connection is 
                        // not closed after a command is executed.
                        objFTPRequest.KeepAlive = false;
                        // Set the data transfer type.
                        objFTPRequest.UseBinary = true;
                        // Set content length
                        objFTPRequest.ContentLength = objFile.Length;
                        objFTPRequest.EnableSsl = false;
                        // Set request method
                        objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        // Set buffer size
                        int intBufferLength = 16 * 1024;
                        byte[] objBuffer = new byte[intBufferLength];
                        // Opens a file to read

                        FileStream objFileStream = objFile.OpenRead();
                        try
                        {
                            // Get Stream of the file
                            Stream objStream = objFTPRequest.GetRequestStream();
                            int len = 0;
                            while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
                            {
                                // Write file Content 
                                objStream.Write(objBuffer, 0, len);
                            }
                            objStream.Close();
                            objFileStream.Close();

                        }
                        catch (Exception ex)
                        {
                            //DependencyService.Get<IMessage>().LongAlert(ex.Message);
                            Cargado = "NO";
                            return Cargado;
                        }

                    }
                }

                Cargado = "SI";
            }
            catch (Exception ex)
            {

                Cargado = "NO";
                
                MessageBox.Show(ex.Message + "Error al enviar al Servidor Wiroos");
            }

            return Cargado;
        }



        /// <summary>
        /// Buscará si hay archivos en cada carpeta que le corresponda a cada colectora dentro del servidor FTP
        /// y en caso de que existan archivos los tomara y quedara la colectora cargada.
        /// Devuelve:
        /// SI = si se cargaron correctamente los archivos
        /// NO = Si no se cargaron los archivos
        /// No se encuentran los archivos = cuando no existen los tres archivos necesarios para la carga
        /// 
        /// </summary>
        /// <param name="Localidad"></param>
        /// <param name="Colectora"></param>
        public async static Task<string> DescargarColectora(string Directorio, string Localidad, string Periodo, string Remesa, string FechaDescarga, string Colectora)
        {
            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            string dirLocal = Directorio;
            //string dirLocal = "/storage/emulated/0/Datos DPEC";
            string DESCARGADO = "NO";
            try
              {   //string RutaVerificar = Localidad + "/" + Colectora;
                string RutaVerificar = ficFTP + Localidad + "/" + Periodo + "/" + Remesa + "/" + FechaDescarga + "/" + Colectora +"/";
                //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

                // Obtiene el objeto que se utiliza para comunicarse con el servidor.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string file = streamReader.ReadLine();
                //Obtiene el contenido y lo agrega al List<string>.
                while (!string.IsNullOrEmpty(file))
                {
                    if (file == "." || file == "..")
                    {
                        file = streamReader.ReadLine();
                    }
                    else
                    {
                        if (file == "InfoCarga.txt" || file == "Datos_FIS.db" || file == "dbFIS-DPEC.db")
                        {
                            string remoteUri = RutaVerificar;
                            string fileName = file, myStringWebResource = null;
                            // Create a new WebClient instance.
                            WebClient myWebClient = new WebClient();
                            // Concatenate the domain with the Web resource filename.
                            myStringWebResource = remoteUri + "/" + fileName;
                            myWebClient.Credentials = new NetworkCredential(Vble.UserFTP(), Vble.PassAdmin());
                            //// Download the Web resource and save it into the current filesystem folder.
                            myWebClient.DownloadFile(myStringWebResource, dirLocal + "\\" + fileName);
                            directories.Add(file);
                            file = streamReader.ReadLine();
                        }
                    }
                }

                if (directories.Count == 3)
                {
                    DESCARGADO = "SI";
                }
                else
                {
                    DESCARGADO = "NO";
                }
                streamReader.Close();
                response.Close();
                

                ///Comento esto para probar descargar los 3 archivos al momento en que lee,
                ///cualquier inconsistencia volveré a usar esto despues de leer los archivos que se encuentran en el servidor.
                // Obtiene el objeto que se utiliza para comunicarse con el servidor.
                //FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(RutaVerificar);
                //request2.Method = WebRequestMethods.Ftp.ListDirectory;
                //// Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
                //request2.Credentials = new NetworkCredential(user, pass);
                //FtpWebResponse response2 = (FtpWebResponse)request2.GetResponse();
                //StreamReader streamReader2 = new StreamReader(response2.GetResponseStream());    
                //if (directories.Count == 3)
                //{
                //    string line = streamReader2.ReadLine();
                //    //Obtiene el contenido y lo agrega al List<string>.
                //    while (!string.IsNullOrEmpty(line))
                //    {
                //        //directories.Add(line);
                //        //line = streamReader2.ReadLine();
                //        line = streamReader2.ReadLine();
                //        if (line == "InfoCarga.txt" || line == "Datos_FIS.db" || line == "dbFIS-DPEC.db")
                //        {
                //            string remoteUri = RutaVerificar;
                //            string fileName = line, myStringWebResource = null;
                //            // Create a new WebClient instance.
                //            WebClient myWebClient = new WebClient();
                //            // Concatenate the domain with the Web resource filename.
                //            myStringWebResource = remoteUri + "/" + fileName;

                //            myWebClient.Credentials = new NetworkCredential("macroint", "Micc4001");                            
                //            //// Download the Web resource and save it into the current filesystem folder.
                //            myWebClient.DownloadFile(myStringWebResource, dirLocal + "\\" + fileName);    
                //        }                     
                //    }
                //    DESCARGADO = "SI";
                //}
                //else
                //{
                //    DESCARGADO = "NO";
                //}
                //streamReader2.Close();
                //// reader.Close();
                //response2.Close();
                ///-------------------------------------------------------------------


              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al conectar con servidor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);               
            }
            return DESCARGADO;
        }

        /// <summary>
        /// Buscará si hay archivos en cada carpeta que le corresponda a cada colectora dentro del servidor FTP
        /// y en caso de que existan archivos los tomara y quedara la colectora cargada.
        /// Devuelve:
        /// SI = si se cargaron correctamente los archivos
        /// NO = Si no se cargaron los archivos
        /// No se encuentran los archivos = cuando no existen los tres archivos necesarios para la carga
        /// 
        /// </summary>
        /// <param name="Localidad"></param>
        /// <param name="Colectora"></param>
        public async static Task<string> DescargarArchivoZip(string Directorio, string Localidad, string Periodo, string Remesa, string FechaDescarga, string Colectora)
        {
            string ficFTP = "ftp://macrointell.com.ar/DPEC-FIS/Descargas/";
            string user = Vble.UserFTP();
            string pass = Vble.PassAdmin();
            string dirLocal = Directorio;
            //string dirLocal = "/storage/emulated/0/Datos DPEC";
            string DESCARGADO = "NO";
            try
            {   //string RutaVerificar = Localidad + "/" + Colectora;
                string RutaVerificar = ficFTP + Localidad + "/" + Periodo + "/" + Remesa + "/" + FechaDescarga + "/" + Colectora + "/";
                //VerificaCarpetaRutaCarg(RutaVerificar, Localidad, Colectora);

                // Obtiene el objeto que se utiliza para comunicarse con el servidor.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RutaVerificar);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string file = streamReader.ReadLine();
                //Obtiene el contenido y lo agrega al List<string>.
                while (!string.IsNullOrEmpty(file))
                {
                    if (file == "." || file == "..")
                    {
                        file = streamReader.ReadLine();
                    }
                    else
                    {
                        if (file.Contains(".zip"))
                        {
                            string remoteUri = RutaVerificar;
                            string fileName = file, myStringWebResource = null;
                            // Create a new WebClient instance.
                            WebClient myWebClient = new WebClient();
                            // Concatenate the domain with the Web resource filename.
                            myStringWebResource = remoteUri + "/" + fileName;
                            myWebClient.Credentials = new NetworkCredential(Vble.UserFTP(), Vble.PassAdmin());
                            //// Download the Web resource and save it into the current filesystem folder.
                            myWebClient.DownloadFile(myStringWebResource, dirLocal + "\\" + fileName);
                            directories.Add(file);
                            file = streamReader.ReadLine();

                        }
                    }
                }

                if (directories.Count == 1)
                {
                    DESCARGADO = "SI";
                }
                else
                {
                    DESCARGADO = "NO";
                }
                streamReader.Close();
                response.Close();

                ///-------------------------------------------------------------------



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al conectar con servidor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return DESCARGADO;
        }

        public static void DescomprimirArchivo(string zipFilePath, string extractPath)
        {
            // Verificar si el archivo zip existe
            if (System.IO.File.Exists(zipFilePath))
            {
                // Extraer todos los archivos del zip al directorio de destino
                ZipFile.ExtractToDirectory(zipFilePath, extractPath);
                Console.WriteLine("Archivo descomprimido correctamente en: " + extractPath);
            }
            else
            {
                Console.WriteLine("El archivo ZIP no existe.");
            }
        }
       


        /// <summary>
        /// Elimina los tres archivos que se encuentran en la carpeta de la colectora dentro del servidor FTP 
        /// para asegurarse de que no se vuelva a cargar la misma ruta.
        /// Si se desea volver a cargar se debera Descargar y volver a generar la Carga.
        /// </summary>
        /// <param name="RutaArchivos"></param>
        public static void EliminarArchivos(string RutaArchivos)
        {

            // Obtiene el objeto que se utiliza para comunicarse con el servidor.
            FtpWebRequest DeleteInfoCarga = (FtpWebRequest)WebRequest.Create(RutaArchivos + "/InfoCarga.txt");
            DeleteInfoCarga.Method = WebRequestMethods.Ftp.DeleteFile;
            // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
            DeleteInfoCarga.Credentials = new NetworkCredential("macroint", "Micc4001");
            FtpWebResponse response1 = (FtpWebResponse)DeleteInfoCarga.GetResponse();

            // Obtiene el objeto que se utiliza para comunicarse con el servidor.
            FtpWebRequest DeleteBaseFija = (FtpWebRequest)WebRequest.Create(RutaArchivos + "/Datos_FIS.db");
            DeleteBaseFija.Method = WebRequestMethods.Ftp.DeleteFile;
            // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
            DeleteBaseFija.Credentials = new NetworkCredential("macroint", "Micc4001");
            FtpWebResponse response2 = (FtpWebResponse)DeleteBaseFija.GetResponse();

            // Obtiene el objeto que se utiliza para comunicarse con el servidor.
            FtpWebRequest DeleteBaseVariable = (FtpWebRequest)WebRequest.Create(RutaArchivos + "/dbFIS-DPEC.db");
            DeleteBaseVariable.Method = WebRequestMethods.Ftp.DeleteFile;
            // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
            DeleteBaseVariable.Credentials = new NetworkCredential("macroint", "Micc4001");
            FtpWebResponse response3 = (FtpWebResponse)DeleteBaseVariable.GetResponse();

        }

        /// <summary>
        /// Elimina el archivo .zip que se encuentran en la carpeta de la colectora dentro del servidor FTP 
        /// para asegurarse de que no se vuelva a cargar la misma ruta.
        /// Si se desea volver a cargar se debera Descargar y volver a generar la Carga.
        /// </summary>
        /// <param name="RutaArchivos"></param>
        public static void EliminarArchivoZIP(string RutaArchivos)
        {

            // Obtiene el objeto que se utiliza para comunicarse con el servidor.
            FtpWebRequest DeleteInfoCarga = (FtpWebRequest)WebRequest.Create(RutaArchivos);
            DeleteInfoCarga.Method = WebRequestMethods.Ftp.DeleteFile;
            // Este ejemplo asume que el sitio FTP utiliza autenticación anónima.
            DeleteInfoCarga.Credentials = new NetworkCredential("macroint", "Micc4001");
            FtpWebResponse response1 = (FtpWebResponse)DeleteInfoCarga.GetResponse();

          

        }

        /// <summary>
        /// Cambia el estado impresionOBS de cada conexion en la base de datos MySql de acuerdo
        /// al estado que se desea cambiar el cual se recibe como parametro
        /// al llamar a la función
        /// </summary>
        /// 
        public static void CambiarEstadoConexionMySql(Int32 ConexionID, int StatusChange, Int32 Periodo)
        {
            //string Select = "SELECT ImpresionOBS FROM conexiones WHERE ConexionID = " + ConexionID;

            try
            {

                if (StatusChange == 5)
                {
                    string modEstado = "UPDATE Conexiones SET ImpresionOBS = " +
                        " (ImpresionOBS + 300) WHERE ConexionID = " + ConexionID + " And Periodo = " + Periodo;
                        //(ImpresionOBS.ToString().Replace(ImpresionOBS.ToString().Substring(0, 1), StatusChange.ToString())) +
                        //"WHERE ConexionID = " + ConexionID + " And Periodo = " + Periodo;
                    //preparamos la cadena para la modificiación
                    MySqlCommand command = new MySqlCommand(modEstado, DB.conexBD);
                    //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }
                else if (StatusChange == 0)
                {
                    string modEstado = "UPDATE Conexiones SET ImpresionOBS = 800 WHERE ConexionID = " + ConexionID + " And Periodo = " + Periodo;
                    //preparamos la cadena para la modificiación
                    MySqlCommand command = new MySqlCommand(modEstado, DB.conexBD);
                    //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }
                else
                {
                    string modEstado = "UPDATE Conexiones SET ImpresionOBS = " + StatusChange * 100 + " WHERE ConexionID = " + ConexionID + " And Periodo = " + Periodo;
                    //preparamos la cadena para la modificiación
                    MySqlCommand command = new MySqlCommand(modEstado, DB.conexBD);
                    //y la ejecutamos
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                }

              


            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message);
            }

        }


        /// <summary>
        /// Este metodo controlará el numero de carga que tiene el archivo InfoCarga con el formato C0000, y buscará a que ruta dentro del panel
        /// de cargas pertenece para enviar desde éste al panel de rutas descargadas.
        /// Lo unico que haria sería mover toda la carpeta que contiene dicha ruta desde la carpeta Enviadas a Recibidas
        /// </summary>
        public static void ActualizarPanelesEnCargasDeColectoras(string ArchivoInfoCarga, string Temporal)
        {
            Form4Cargas Cargas = new Form4Cargas();
            string InformacionArchivo = Funciones.LeerArchivostxt(ArchivoInfoCarga);

            if (InformacionArchivo.Contains("|"))
            {
                string Distrito = InformacionArchivo.Substring((InformacionArchivo.IndexOf('-') - 3), 3);
            }
            else
            {
                string Distrito = InformacionArchivo.Substring(0, (InformacionArchivo.IndexOf('-')));
            }

            if (InformacionArchivo.Contains("C"))
            {
                InformacionArchivo = InformacionArchivo.Substring(InformacionArchivo.IndexOf('C'), 6);
            }
            else
            {
                InformacionArchivo = "";
            }
                


            //ArrayCarpetasCargas.Clear();
            Cargas.ListViewCargados.ShowItemToolTips = true;
            Int32 IndiceDictionary = 0;

            DateTime Per = DateTime.ParseExact(Vble.Periodo.ToString("000000"), "yyyyMM",
            CultureInfo.CurrentCulture);
            //armo la carpeta donde estan los archivos tanto sin enviar como enviadas, a partir de ahi escalo las subcarpetas que me interesan
            string ArchivosCargados = string.Format(Vble.CarpetasCargasAcargar + Distrito, Per);
            string CarpetaDescargadas = string.Format(Vble.CarpetasCargasAcargar + Distrito, Per);

            DirectoryInfo di = new DirectoryInfo(ArchivosCargados);
            //DirectoryInfo ArcCole = new DirectoryInfo(ArchivoInfoCarga);

            if (Directory.Exists(di.ToString()))
            {
                foreach (var dis in di.GetDirectories())
                {
                    //foreach (var sub in dis.GetDirectories())
                    //{
                        if (dis.Name.Contains("Enviadas"))
                        {
                            foreach (var fi in dis.GetDirectories())
                            {
                                if (fi.Name.Contains("EP2"))
                                {                                    
                                    ListViewItem Datos = new ListViewItem(fi.Name);
                                    if (fi.Name.Substring(fi.Name.IndexOf('C'),6) == InformacionArchivo)
                                        {

                                    Form4Cargas.CopiarDirectorio(Temporal, CarpetaDescargadas + "\\Recibidas\\" + fi.Name, true);
                                    Directory.Delete(fi.FullName,true);
                                        //if (!Directory.Exists(Vble.CarpetasCargasRecibidas + fi.Name))
                                        //{
                                        //    Directory.CreateDirectory(Vble.CarpetasCargasRecibidas + fi.Name);
                                        //}                                 
                                        //foreach (var files in fi.GetFiles())
                                        //    {
                                        //    File.Copy(files.FullName, Vble.CarpetasCargasRecibidas + fi.Name, true);
                                        //    }

                                        }
                                }
                            }
                        }
                    //}
                }
                IndiceDictionary = 0;
            }
        

    }

        /// <summary>
        /// Cambia el estado impresionOBS de cada conexion que se esta enviando a la colectora de la base
        /// MySQL general donde estan todas las conexiones.
        /// </summary>
        /// 
        public static void CambiarEstadoEnviadasMySql(string RutaEnviadas, int StatusChange)
        {
            DataTable Tabla = new DataTable();
            try
            {
                string txSQL;
                SQLiteDataAdapter datosAdapter;
                SQLiteCommandBuilder comandoSQL;
                Int32 conexionID;

                //Lee y obtiene el nombre de la base Sqlite
                StringBuilder stb1 = new StringBuilder("", 100);
                Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                //string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();
                string path = RutaEnviadas.TrimEnd(' ') + "\\" + stb1.ToString();
                //Cambio el estado de las conexiones antes de ser enviadas a la colectora 
                //pasa de 300(Listo para Cargar) a 400(Cargados en Colectora)
                SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);

                BaseACargar.Open();//comentado recientemente

                txSQL = "SELECT conexionID, Periodo, Ruta FROM Conexiones";
                datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

                foreach (DataRow fi in Tabla.Rows)
                {
                    //asignación a variables locales para manejar en el UPDATE
                    conexionID = Convert.ToInt32(fi["conexionID"]);
                    Periodo = Convert.ToInt32(fi["Periodo"]);
                    Ruta = Convert.ToInt32(fi["Ruta"]);

                    string update;//Declaración de string que contendra la consulta UPDATE               
                    update = "UPDATE Conexiones SET ImpresionOBS = " + StatusChange * 100 + 
                        " WHERE conexionID = " + conexionID + " AND Periodo = " + Periodo + " AND Ruta = " + Ruta;
                    //preparamos la cadena pra insercion
                    MySqlCommand command = new MySqlCommand(update, DB.conexBD);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();

                }
                comandoSQL.Dispose();
                datosAdapter.Dispose();
                BaseACargar.Close();
            }
            catch (MySqlException r)
            {
                MessageBox.Show(r.Message);
            }
        }



        /// <summary>
        /// Cambia el estado impresionOBS de cada conexion en la base de datos SQLite ya generado 
        /// con las conexiones que se van a pasar a la colectora de acuerdo
        /// al estado que se desea cambiar el cual se recibe como parametro
        /// al llamar a la función
        /// </summary>
        /// 
        public static void CambiarEstadoConexionSqlite(int StatusChange, string NombreColectora)
        {
            //Lee y obtiene el nombre de la base Sqlite
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();
            //Cambio el estado de las conexiones antes de ser enviadas a la colectora 
            //pasa de 300(Listo para Cargar) a 400(Cargados en Colectora)
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
            BaseACargar.Open();

            try
            {
                string modEstado = "UPDATE conexiones SET ImpresionOBS = " + StatusChange * 100;
                //preparamos la cadena para la modificiación
                SQLiteCommand command = new SQLiteCommand(modEstado, BaseACargar);
                //y la ejecutamos
                command.CommandTimeout = 300;
                command.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                command.Dispose();



                string ModifColectora = "UPDATE Varios SET Valor = '" + NombreColectora + "' WHERE Parametro = 'CodigoEquipo'";
                //preparamos la cadena para la modificiación
                SQLiteCommand cmd = new SQLiteCommand(ModifColectora, BaseACargar);
                //y la ejecutamos
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();
                //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                cmd.Dispose();

               

            }
            catch (MySqlException)
            {

            }
            BaseACargar.Close();
        }



        /// <summary>
        /// Funcion que crea el archivo vacio, con la secuencia como nombre para utilizar como información a la hora de mostrar en el listview1
        /// </summary>
        /// <param name="archivosecuencia"></param>
        /// <param name="secuencia"></param>
        public static void CrearArchivoTXT(string lineas, string filename)
        {
            try
            {

                StringBuilder stb1 = new StringBuilder("", 250);
                Inis.GetPrivateProfileString("Carpetas", "Dir InfRutasExp", "", stb1, 250, Ctte.ArchivoIniName);
                string RutaCarpInformes = stb1.ToString();

           
                RutaCarpInformes = Vble.ValorarUnNombreRuta(RutaCarpInformes);
                if (!Directory.Exists(RutaCarpInformes))
                {
                    Directory.CreateDirectory(RutaCarpInformes);
                }

                RutaCarpInformes = RutaCarpInformes + filename;
                
                
                CreateInfoCarga(RutaCarpInformes, filename, lineas);

                MessageBox.Show("Se genero el txt de rutas exportadas", "Rutas Exportadas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(RutaCarpInformes);

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al crear archivo de informacion de descarga");
            }
        }


        /// <summary>
        /// Agrega informacion de Carga y descarga de las conexiones en la tabla infoconex de la base
        /// MySql que se utiliza luego para la importacion o exportacion de cada conexion
        /// recibiendo como parametro la conexiones SQLiteConennection par realizar la descarga de las colectoras.
        /// </summary>
        /// 
        //public static void ModificarInfoConex(string Fecha, string hora, string operario, int Cod_Impresion, string Operacion, string RutaEnviadas)
        public static void ModificarInfoConex(string Fecha, string hora, string operario, int Cod_Impresion, string Operacion, SQLiteConnection BaseACargar)
        {
            SQLiteDataAdapter datosAdapter = new SQLiteDataAdapter();
            SQLiteCommandBuilder comandoSQL = new SQLiteCommandBuilder();
            MySqlCommand command = new MySqlCommand();
            Int32 Oper;
            try
            {
                DataTable Tabla1 = new DataTable();
                string txSQL;

                Int32 conexionID;
                Int32 Periodo;
                if (Operacion == "Carga")
                {
                    ////Lee y obtiene el nombre de la base Sqlite
                    //StringBuilder stb1 = new StringBuilder("", 100);
                    //Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                    ////string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();
                    //string path = RutaEnviadas + "\\" + stb1.ToString();
                    //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
                    //BaseACargar.Open();

                    txSQL = "SELECT conexionID, Periodo FROM Conexiones";
                    datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                    comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                    datosAdapter.Fill(Tabla1);

                    Oper = operario == "admin" ? 9003 : Convert.ToInt32(Operario);

                    foreach (DataRow fi in Tabla1.Rows)
                    {
                        //si es una carga modifica los campos de carga FechaCarga, Hora Carga, OperCarga

                        //asignación a variables locales para manejar en el UPDATE
                        conexionID = Convert.ToInt32(fi[0]);
                        Periodo = Convert.ToInt32(fi["Periodo"]);
                        //conexionID = fi.Field<Int32>("ConexionID");
                        //Cod_Impresion = fi.Field<int>("ImpresionCOD");                        
                        string update;//Declaración de string que contendra la consulta UPDATE               
                        update = "UPDATE infoconex SET FechaCarga = " + Fecha + ", HoraCarga = " + hora + ", OperCarga = " + Oper.ToString() + ", CodigoImpresion = " + Cod_Impresion + " WHERE ConexionID = " + conexionID + " AND Periodo = " + Periodo;
                        //preparamos la cadena pra insercion
                        command = new MySqlCommand(update, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                    }
                    comandoSQL.Dispose();
                    datosAdapter.Dispose();
                    //BaseACargar.Close();

                }
                //si es una descarga modifica los campos de descarga FechaDescarga, HoraDescarga, OperDesCarga
                if (Operacion == "Descarga")
                {
                    //SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaEnviadas);
                    //BaseACargar.Open();

                    txSQL = "SELECT conexionID, ImpresionOBS FROM Conexiones";
                    datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                    comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                    datosAdapter.Fill(Tabla1);

                    foreach (DataRow fi in Tabla1.Rows)
                    {
                        //asignación a variables locales para manejar en el UPDATE
                        conexionID = Convert.ToInt32(fi[0]);//columna ConexionID
                        Cod_Impresion = Convert.ToInt32(fi[1]);

                        //Cod_Impresion = fi.Field<int>("ImpresionCOD");
                        string update;//Declaración de string que contendra la consulta UPDATE               
                        update = "UPDATE infoconex SET FechaDescarga = " + Fecha + ", HoraDescarga = " + hora + ", OperDescarga = '" + operario + "', CodigoImpresion = " + (Convert.ToInt32(cteCodEstado.Descargado) * 100 + Cod_Impresion) + " WHERE conexionID = " + conexionID;
                        //preparamos la cadena pra insercion
                        command = new MySqlCommand(update, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                        //BaseACargar.Close();
                    }

                    comandoSQL.Dispose();
                    datosAdapter.Dispose();
                    //BaseACargar.Close();
                }
            }

            catch (Exception r)
            {
                command.Dispose();
                comandoSQL.Dispose();
                datosAdapter.Dispose();
                BaseACargar.Close();
                MessageBox.Show(r.Message);
            }
        }



        /// <summary>
        /// Agrega informacion de Carga y descarga de las conexiones en la tabla infoconex de la base
        /// MySql que se utiliza luego para la importacion o exportacion de cada conexion       
        /// recibiendo como string el parametro donde se encuentra el archivo sqlite
        /// </summary>
        /// 
        public static void ModificarInfoConex(string Fecha, string hora, string operario, int Cod_Impresion, string Operacion, string RutaEnviadas)
        {
            SQLiteDataAdapter datosAdapter = new SQLiteDataAdapter();
            SQLiteCommandBuilder comandoSQL = new SQLiteCommandBuilder();
            MySqlCommand command = new MySqlCommand();
            Int32 Oper;
            try
            {
                DataTable Tabla1 = new DataTable();
                string txSQL;

                Int32 conexionID;
                Int32 Periodo;
                if (Operacion == "Carga")
                {
                    //Lee y obtiene el nombre de la base Sqlite
                    StringBuilder stb1 = new StringBuilder("", 100);
                    Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
                    //string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();
                    string path = RutaEnviadas.TrimEnd(' ') + "\\" + stb1.ToString();
                    SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
                    BaseACargar.Open();

                    txSQL = "SELECT conexionID, Periodo FROM Conexiones";
                    
                    datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                    comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                    datosAdapter.Fill(Tabla1);

                    Oper = operario == "admin" ? 9003 : Convert.ToInt32(Operario);

                    foreach (DataRow fi in Tabla1.Rows)
                    {
                        //si es una carga modifica los campos de carga FechaCarga, Hora Carga, OperCarga

                        //asignación a variables locales para manejar en el UPDATE
                        conexionID = Convert.ToInt32(fi[0]);
                        Periodo = Convert.ToInt32(fi["Periodo"]);
                        //conexionID = fi.Field<Int32>("ConexionID");
                        //Cod_Impresion = fi.Field<int>("ImpresionCOD");                        
                        string update;//Declaración de string que contendra la consulta UPDATE               
                        update = "UPDATE infoconex SET FechaCarga = " + Fecha + ", HoraCarga = " + hora + ", OperCarga = " + Oper.ToString() + ", CodigoImpresion = " + Cod_Impresion + " WHERE ConexionID = " + conexionID + " AND Periodo = " + Periodo;
                        //preparamos la cadena pra insercion
                        command = new MySqlCommand(update, DB.conexBD);
                        //y la ejecutamos
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                    }
                    comandoSQL.Dispose();
                    datosAdapter.Dispose();
                    //BaseACargar.Close();

                }
                //si es una descarga modifica los campos de descarga FechaDescarga, HoraDescarga, OperDesCarga
                if (Operacion == "Descarga")
                {
                    SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + RutaEnviadas);
                    BaseACargar.Open();

                    txSQL = "SELECT conexionID, ImpresionOBS FROM Conexiones";
                    datosAdapter = new SQLiteDataAdapter(txSQL, BaseACargar);
                    comandoSQL = new SQLiteCommandBuilder(datosAdapter);
                    datosAdapter.Fill(Tabla1);

                    foreach (DataRow fi in Tabla1.Rows)
                    {
                        //asignación a variables locales para manejar en el UPDATE
                        conexionID = Convert.ToInt32(fi[0]);//columna ConexionID
                        Cod_Impresion = Convert.ToInt32(fi[1]);

                        //Cod_Impresion = fi.Field<int>("ImpresionCOD");
                        string update;//Declaración de string que contendra la consulta UPDATE               
                        update = "UPDATE infoconex SET FechaDescarga = " + Fecha + ", HoraDescarga = " + hora + ", OperDescarga = '" + operario + "', CodigoImpresion = " + (Convert.ToInt32(cteCodEstado.Descargado) * 100 + Cod_Impresion) + " WHERE conexionID = " + conexionID;
                        //preparamos la cadena pra insercion
                        command.CommandTimeout = 300;
                        command = new MySqlCommand(update, DB.conexBD);
                        //y la ejecutamos
                        command.ExecuteNonQuery();
                        //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                        command.Dispose();
                        //BaseACargar.Close();
                    }

                    comandoSQL.Dispose();
                    datosAdapter.Dispose();
                    //BaseACargar.Close();
                }


            }

            catch (Exception r)
            {
                command.Dispose();
                comandoSQL.Dispose();
                datosAdapter.Dispose();                
                MessageBox.Show(r.Message);
            }

        }


        /// <summary>
        /// Funcion que abre una unidad de red para poder leer y copiar archivos dentro de la NAS
        /// (localname = nombre de la unidad de red temporal que abrira), 
        /// (remotename = direccion dentro de la unidad de red donde se ubicará al abrir)
        /// </summary>
        /// <param name="localname"></param>
        /// <param name="remotename"></param>
        public static void AbrirUnidadDeRed(string localname, string remotename)
        {
            try
            {
                //Se establece la conexion con la unidad de red donde estará disponible el archivo encriptado que provee SAP para la importación
                IWshRuntimeLibrary.IWshNetwork2 network = new IWshRuntimeLibrary.WshNetwork();
                //String localname = @"P:";
                //String remotename = @"\\10.1.3.125\UPLOAD";
                Object updateprofile = System.Type.Missing;
                Object username = Vble.DominioYUsuarioRed;
                Object pass = Vble.ContraseñaRed;
                //MessageBox.Show(Vble.DominioYUsuarioRed);
                //MessageBox.Show(Vble.ContraseñaRed);

                network.MapNetworkDrive(localname, remotename, ref updateprofile, ref username, ref pass);

            }
            catch (Exception r)
            {
                if (r.Message.Contains("Las conexiones múltiples para un servidor o recurso compartido " +
                                       "compatible por el mismo usuario, usando más de un nombre de usuario, " +
                                       "no están permitidas."))
                {

                }
                else
                {
                    MessageBox.Show(r.Message + " Error al conectar con la Unidad de Red");
                }
                
            }
        }

        /// <summary>
        /// Funcion que cierra las unidades de red abiertas para que no queden abiertas luego de realizar
        /// las tareas correspondientes en los directorios de la NAS
        /// </summary>
        public static void CerrarUnidadDeRed()
        {
            try
            {
                IWshRuntimeLibrary.IWshNetwork2 network = new IWshRuntimeLibrary.WshNetwork();
                Object updateprofile = System.Type.Missing;
                //buscamos todas las unidades de red para desconectar y no quede abierto el acceso a cualquier usuario
                DriveInfo[] drives;
                drives = System.IO.DriveInfo.GetDrives();

                foreach (DriveInfo strDrive in drives)
                {
                    if (DriveType.Network == strDrive.DriveType)
                    {
                        if (strDrive.Name == "T:\\" || strDrive.Name == "M:\\")
                        {

                        }
                        else
                        {
                            //borramos la unidad que sea de red
                            bool blnForce = true;
                            Object objblnForce = (object)blnForce;
                            //Object updateprofile = Type.Missing;
                            string strMapDriveLetter = strDrive.Name.Substring(0, 2);
                            network.RemoveNetworkDrive(strMapDriveLetter, ref objblnForce, ref updateprofile);
                            
                        }
                    

                    }
                }
                

                //string lineaCerrarConexionesRed = "@echo off \n net use * /delete";
                string lineaCerrarConexionesRed = "net use Y: /delete /yes";
               
                //string UbicacionArchivoBAT = "C:\\Users\\operario\\CerrarUnidadesRed.bat";
                string UbicacionArchivoBAT = Vble.RutaArchivoBAT();

                if (File.Exists(UbicacionArchivoBAT))
                {
                    File.Delete(UbicacionArchivoBAT);
                }

                Vble.CreateInfoCarga(UbicacionArchivoBAT, "CerrarUnidadesRed", lineaCerrarConexionesRed);

                Process proc = null;
                string _batDir = string.Format(UbicacionArchivoBAT);
                proc = new Process();
                proc.StartInfo.WorkingDirectory = _batDir;
                proc.StartInfo.FileName = UbicacionArchivoBAT;
                
                //esconde la ventana de comando para que no interrumpa al usuario
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                proc.StartInfo.Arguments = "s";
                //System.Diagnostics.Process.Start("C:\\Users\\operario\\CerrarUnidadesRed.bat");
                proc.Start();
                //SendKeys.SendWait("s");
                

                proc.WaitForExit();
                proc.Dispose();
                proc.Close();

                //Elimino el arhivo .batch que se genero para encriptar el archivo btx, ya que es un proceso temporal
                File.Delete(UbicacionArchivoBAT);
                //File.Delete(archivoBTX);     
                Thread.Sleep(3000);


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al cerrar Unidad de Red");
            }
        }


        //Por el momento no se va a actualizar la fecha y la hora al momento de cargar a la colectora
        //queda con la fecha y hora que se proceso la carga / se selecciono las conexiones a cargar
        /// <summary>
        /// Actualiza los campos de FECHA y HORA de la tabla varios cuando se realiza la carga a la colectora 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="RutaEnviadas"></param>
        public static void ActualizaTablaVarios(string fecha, string hora, string RutaEnviadas)
        {
            //Lee y obtiene el nombre de la base Sqlite
            StringBuilder stb1 = new StringBuilder("", 100);
            Inis.GetPrivateProfileString("Archivos", "BaseSqlite", "", stb1, 100, Ctte.ArchivoIniName);
            //string path = Vble.RutaCarpetaOrigen + "\\" + stb1.ToString();
            string path = RutaEnviadas + "\\" + stb1.ToString();
            SQLiteConnection BaseACargar = new SQLiteConnection("Data Source=" + path);
            BaseACargar.Open();

            //asignación a variables locales para manejar en el INSERT
            SQLiteDataAdapter da;
            SQLiteCommandBuilder comandoSQL;
            string txSQL;
            DataTable Tabla = new DataTable();
            txSQL = "select * From varios";
            da = new SQLiteDataAdapter(txSQL, BaseACargar);
            comandoSQL = new SQLiteCommandBuilder(da);
            da.Fill(Tabla);

            string Parametro, Valor;//variables que contendran los datos de cada fila del select anterior.           

            foreach (DataRow fi in Tabla.Rows)
            {
                Parametro = fi[0].ToString();
                Valor = fi[2].ToString();
                //actualiza la fecha actual de carga
                if (Parametro == "FechaCarga")
                {
                    Valor = DateTime.Now.ToString("dd-MM-yyyy");
                    string update;//Declaración de string que contendra la consulta UPDATE               
                    update = "UPDATE varios SET Valor = '" + Valor + "' WHERE Parametro = " + Parametro;
                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(update, BaseACargar);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //BaseACargar.Close();
                }
                //actualiza la Hora actual de carga
                if (Parametro == "HoraCarga")
                {
                    Valor = DateTime.Now.ToString("HH:mm");
                    string update;//Declaración de string que contendra la consulta UPDATE               
                    update = "UPDATE varios SET Valor = '" + Valor + "' WHERE Parametro = " + Parametro;
                    //preparamos la cadena pra insercion
                    SQLiteCommand command = new SQLiteCommand(update, BaseACargar);
                    //y la ejecutamos
                    command.ExecuteNonQuery();
                    //finalmente cerramos la conexion ya que solo debe servir para una sola orden
                    command.Dispose();
                    //BaseACargar.Close();
                }
            }

            BaseACargar.Close();

        }
        /// <summary>
        /// Crear directorio de CargasSinEnviar con formato especificado por el parametro fullPath
        /// si no existe el directorio lo crea
        /// <param name ="fullPath"></param>
        /// </summary>
        public static void CrearDirectorioVacio(string fullPath)
        {
            try
            {
                if (!System.IO.Directory.Exists(fullPath))
                {
                    System.IO.Directory.CreateDirectory(fullPath);
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Copiar archivo desde ruta de origen a ruta destino pasada por parametro
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
                    MessageBox.Show("No se genero correctamente el Archivo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Crea archivo en la ruta que se pasa como parametro en variable 
        //archivosecuencia, con el nombre y los datos dentro        
        public static void CreateInfoCarga(string archivosecuencia, string filename, string lineas)
        {
            try
            {
                //Creo el archivo que contiene la información de la carga que se proceso y esta lista para enviar
                if (!System.IO.File.Exists(archivosecuencia))
                {
                    
                    System.IO.FileStream fs = System.IO.File.Create(archivosecuencia);
                    using (fs)
                    {
                        for (byte i = 0; i < 100; i++)
                        {
                            fs.WriteByte(i);
                        }
                    }
                }
                else if(File.Exists(archivosecuencia))
                {                                      
                    File.Delete(archivosecuencia);
                    CreateInfoCarga(archivosecuencia, filename, lineas);                
                }
                else
                {
                    MessageBox.Show("No se pudo crear el archivo " + filename, "Error");

                    return;
                }

                System.IO.File.WriteAllText(archivosecuencia, lineas);
                
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al crear el archivo " + filename);
            }
        }



        //Crea archivo EXCEL con los datos de los usuarios LEIDOS NO IMPRESOS y se coloca en la misma carpeta del archivo UPLOAD por el momento.        
        //public static void ExportarExcel(DataTable oDataTableImpresos, DataTable dtLeidosFueraDeRango, string ruta)
        public static void ExportarExcel(ArrayList NºInstalacionImp, ArrayList ContratoImp, ArrayList TitularImp,
                                         ArrayList FacturaImp, ArrayList NºInstalacionFR, ArrayList ContratoFR, ArrayList TitularFR,
                                         ArrayList ObersvacionFR, string ruta)
        {
            try
            {          
            //if ((oDataTableImpresos == null) || (String.IsNullOrEmpty(ruta)))
            //{
            //    throw new ArgumentNullException();
            //}

           Excel.Application excel = null;
           Excel.Workbook book = null;
           Excel.Worksheet HojaImpresos = null;
           Excel.Worksheet HojaFueraDeRango = null;
           Excel.Range rango;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                if (!File.Exists(ruta))
                {
                    //Aqui se debe crear el archivo excel segun la ruta que se envia si no existe
                    //ExcelLibrary.DataSetHelper.CreateWorkbook(ruta);
                    //excel.Workbooks.Add(ruta);
                    //File.Create(ruta);
                    book = excel.Workbooks.Add();
                    HojaImpresos = book.Worksheets.Add();
                    HojaFueraDeRango = book.Worksheets.Add();
                    HojaFueraDeRango.Name = "FUERAS DE RANGO";
                    HojaImpresos.Name = "LEIDOS IMPRESOS";
                    ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Sheets["Hoja1"]).Delete();  
                    book.SaveAs(ruta);
                    book.Close();
                    excel.Quit();
                }

                // Abrimos el libro de trabajo.
                book = excel.Workbooks.Open(ruta);

                Excel.Worksheet ws = (Excel.Worksheet)book.Worksheets[2];

                //ws.Cells[1, 1].Value = "INSTALACION|CONTRATO|TITULAR|PUNTO DE VENTA|LETRA FACTURA|NºFACTURA";
                ws.Cells[1, 1].Value = "Nº INSTALACION";
                ws.Cells[1, 2].Value = "CONTRATO";
                ws.Cells[1, 3].Value = "TITULAR";
                ws.Cells[1, 4].Value = "FACTURA";                

                //Bordes a la celda
                rango = ws.Range["A1", "D1"];
                rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rango.Style.Font.Bold = true;
                //ws.Cells["A1:F1"].Style.Font.Bold = true;

                //Bordes a la celda
                rango = ws.Range["A2", "D2"];
                rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rango.Style.Font.Bold = false;

                rango = ws.Rows[1];
                rango.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                
                int indiceColumna = 0;
                //////Hoja Impresos
                ////foreach (DataColumn col in oDataTableImpresos.Columns)  //Columnas
                ////{
                ////    indiceColumna++;
                ////    ws.Cells[1, indiceColumna] = col.ColumnName;
                ////    //HojaImpresos.Cells[1, indiceColumna] = col.ColumnName;
                ////}
                //int indiceFila = 0;
                //foreach (DataRow row in oDataTableImpresos.Rows)  //Filas
                //{
                //    indiceFila++;
                //    indiceColumna = 0;

                //    foreach (DataColumn col in oDataTableImpresos.Columns)  //Columnas
                //    {
                //        indiceColumna++;
                //        ws.Cells[indiceFila + 1, indiceColumna] = row[col.ColumnName];
                //        //HojaImpresos.Cells[indiceFila + 1, indiceColumna].Value = row[col.ColumnName];
                //    }
                //    //HojaImpresos.Columns.AutoFit();
                //    ws.Columns.AutoFit();
                //}

                int indiceFila = 1;
                for (int i = 0; i < NºInstalacionImpresos.Count; i++)              
                {
                    indiceFila++;
                    indiceColumna = 1;                                          
                        ws.Cells[indiceFila, indiceColumna] = NºInstalacionImpresos[i];
                        ws.Cells[indiceFila, indiceColumna + 1] = ContratoImp[i];
                        ws.Cells[indiceFila, indiceColumna + 2] = TitularImp[i];
                        ws.Cells[indiceFila, indiceColumna + 3] = FacturaImpresos[i];
                        ws.Rows[indiceFila].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; ;
                      
                }

                //Cambiamos la hoja de trabajo para agregar las Conexiones Fuera de Rango que existan.
                ws = (Excel.Worksheet)book.Worksheets[1];
                //ws.Cells[1, 1].Value = "INSTALACION|CONTRATO|TITULAR|TOMADO|CORREGIDO|OBSERVACIONES";
                ws.Cells[1, 1].Value = "INSTALACION";
                ws.Cells[1, 2].Value = "CONTRATO";
                ws.Cells[1, 3].Value = "TITULAR";
                ws.Cells[1, 4].Value = "TOMADO|CORREGIDO|OBSERVACIONES";                

                //Bordes a la celda
                rango = ws.Range["A1", "D1"];
                rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rango.Style.Font.Bold = true;                

                rango = ws.Rows[1];
                rango.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;                

                ////indiceColumna = 0;
                //// if ((dtLeidosFueraDeRango != null))
                //// {
                ////     //HojaFuera de rango
                ////     foreach (DataColumn col in dtLeidosFueraDeRango.Columns)  //Columnas
                ////     {
                ////         indiceColumna++;
                ////         //excel.Cells[1, indiceColumna] = col.ColumnName;
                ////         HojaFueraDeRango.Cells[1, indiceColumna] = col.ColumnName;
                ////     }
                //int indiceFilaFR = 0;
                //foreach (DataRow row in dtLeidosFueraDeRango.Rows)  //Filas
                //{
                //    indiceFilaFR++;
                //    indiceColumna = 0;

                //    foreach (DataColumn col in dtLeidosFueraDeRango.Columns)  //Columnas
                //    {
                //        indiceColumna++;
                //        //excel.Cells[indiceFilaFR + 1, indiceColumna] = row[col.ColumnName];
                //        ws.Cells[indiceFila + 1, indiceColumna].Value = row[col.ColumnName];
                //    }
                //    ws.Columns.AutoFit();
                //}
                indiceFila = 1;
                for (int i = 0; i < NºInstalacionFR.Count; i++)
                {
                    indiceFila++;
                    indiceColumna = 1;
                    ws.Cells[indiceFila, indiceColumna] = NºInstalacionFR[i];
                    ws.Cells[indiceFila, indiceColumna + 1] = ContratoFR[i];
                    ws.Cells[indiceFila, indiceColumna + 2] = TitularFR[i];
                    ws.Cells[indiceFila, indiceColumna + 3] = ObersvacionFR[i];
                    ws.Rows[indiceFila].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }                
                //excel.Visible = true;
            }
            catch (Exception ex)
            {
                if (book != null)
                {
                    book.Saved = true;
                }

                throw new Exception(ex.Message);
            }
            finally
            {
                if (book != null)
                {
                    if (!book.Saved)
                    {
                        book.Save();
                    }
                    book.Close();
                }
                book = null;

                if (excel != null)
                {
                    // Si procede, cerramos Excel y disminuimos el recuento de referencias al objeto Excel.Application.
                    excel.Quit();

                    while (System.Runtime.InteropServices.Marshal.ReleaseComObject(excel) > 0)
                    {

                    }
                }
                excel = null;
            }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message + " Error al crear excel de Informes Impresos y Fuera de Rango");
                
            }
        }
     
        /// <summary>
        /// Realiza la encriptacion del archivo, creando un archivo .batch y ejecutandolo por debajo sin mostrar
        /// el proceso al usuario
        /// </summary>
        /// <param name="nombrearchivo"></param>
        /// <param name="CarpetaContenedora"></param>
        public static void EncriptarArchivo(string nombrearchivo, string CarpetaContenedora)
        {
            string archivoBAT = "", archivoBTX = "";
            try
            {
                archivoBAT = CarpetaContenedora + "Encriptador.bat";
                archivoBTX = CarpetaContenedora + "\\" + nombrearchivo;


                Vble.lineas = "@echo off \n gpg --encrypt --recipient " + Vble.IdClave + " " + nombrearchivo;
                Vble.CreateInfoCarga(archivoBAT, "Encriptador.bat", Vble.lineas);

                Process proc = null;
                string _batDir = string.Format(CarpetaContenedora);
                proc = new Process();
                proc.StartInfo.WorkingDirectory = _batDir;
                proc.StartInfo.FileName = "Encriptador.bat";

                //esconde la ventana de comando para que no interrumpa al usuario
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                proc.StartInfo.Arguments = "s";
                proc.Start();

                proc.WaitForExit();
                proc.Close();

                //Elimino el arhivo .batch que se genero para encriptar el archivo btx, ya que es un proceso temporal
                File.Delete(archivoBAT);
                //File.Delete(archivoBTX);            

            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al encriptar el archivo de Exportación", "Encriptado de archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Calcula el Periodo, partieendo del actual y tomando como referencia
        /// el dato RespectoActual.
        /// </summary>
        /// <param name="RespectoActual"> Si es 0 establece el actual, si es positivo suma al actual, 
        /// si es negativo resta al actual</param>
        /// <returns>devuelve el periodo en formato AAAAPP (201502)</returns>
        public static int SetPeriodo(int RespectoActual) {
            int retorno = 0;
            //El periodo actual es P=Parte entera de((mes actual+1)/2)
            int Pa = ((DateTime.Now.Month + 1) / 2) + RespectoActual;
            int Pb = (DateTime.Now.Year);            
            int iR = (Pa-1) % 6 + 1;
            int iC = (Pa + 1) / 6;
            if(iR < 1) {
                iR += 6;
                iC--;
            }
            retorno = (Pb + iC) * 100 + iR; 
            return retorno;
        }
                       
        /// <summary>
        /// Devuelve la cadena con la primera letra de cada palabra en mayúscula y 
        /// las demas en minúsculas.
        /// </summary>
        /// <param name="Cadena"></param>
        /// <returns></returns>
        public static string LetraCapital(string Cadena) {
            string cadena = Cadena.ToLower();
            int i = cadena.Length - 1;
            int j;

            while(i >= 0) {
                j = cadena.LastIndexOf(' ', i); ;
                cadena = (j >= 0) ?
                  cadena.Substring(0, j + 1) +
                  cadena.Substring(j + 1, 1).ToUpper() + cadena.Substring(j + 2)
                  : cadena.Substring(0, 1).ToUpper() + cadena.Substring(1);

                i = j - 1;
            }
            return Cadena.Substring(0, 1).ToUpper() +
                Cadena.Substring(1).ToLower();            
        }
        #endregion METODOS Vble.-
    }
    /// <summary> Metodos y propiedades para el manejo de las bases de datos. 
    /// Se trabaja con MySQL
    /// </summary>
    public static class DB {
        //Cadenas usadas para valores, encabezados, tipos y valores por defecto, en archivos de texto
        //con datos para ser cargados en una tabla
        static string[] sVal, sCbz, sTip, sDef;
        static string connectionString;
        public static string connectionStringAdmin { get; set; }
        static string connectionStringHistorial;

        public static string ServidorBD;
        public static string NombreBD;
        public static string NombreBDHistorial;


        public static string sDbUsu { get; set; }
        public static string sDbKey { get; set; }
       
        /// <summary>
        /// la variable Entorno puede ser un string "PRD" o "QAS" dependiendo de donde se desea trabajar.
        /// </summary>
        public static string Entorno { get; set; }
        public static MySqlConnection conexBD { get; set; }
        public static MySqlConnection conexBDHistorial { get; set; }
        public static SQLiteConnection con { get; set; }
        public static SQLiteConnection conSQLiteFija { get; set; }

        //conexion casa
        //public static SQLiteConnection con = new SQLiteConnection("Data Source=C:/Users/Enzo/Google Drive/MACRO INTELL - Software/gagFIS-Interfase/gagFIS-Interfase/dbFIS-DPEC.db");

        //conexion OFICINA       
        //public static SQLiteConnection con = new SQLiteConnection("Data Source=F:/Google Drive/MACRO INTELL - Software/gagFIS-Interfase/gagFIS-Interfase/Resources/dbFIS-DPEC.db");
        // public static SQLiteConnection con = new SQLiteConnection("Data Source=" +Vble.CarpetaSqlite);

        /// <summary> Carga una tabla de la base de datos con los datos del archivo.
        /// <para>El archivo debe cumplir los siguiente:</para>
        /// <para>Las lineas que epiezan con // No tienen datos.</para>
        /// <para>Si hay lineas con metadatos, deben estar al inicio, antes que los datos.</para>
        /// <para> Cada linea de metadatos se identifica así: </para> 
        /// <para> //N: Nombres de los campos.</para>
        /// <para> //T: Tipo de datos de campos.</para>
        /// <para> //D: Valores por defecto para cada campo.</para>
        /// <para>Los metadatos se buscan al inicio del archivo, hasta que se encuantra una <br />
        /// linea con valores, si no se encontró, ya se considera que NO están presentes.</para>
        /// </summary>
        /// <param name="DBConex">Conexión a la base de datos</param>
        /// <param name="Tabla">Tabla donde se cargaran datos</param>
        /// <param name="Archivo">Ruta completa del archivo donde está la tabla</param>
        /// <param name="Sep">Carácter separador de campos.</param>
        public static void CargarDesdeArchivo(MySqlConnection DBConex, string Tabla,
                                              string Archivo, char Sep) {

            string sLinea, linCbz, linVal;
            DateTime dT;
            double dB;
            int cnt = 0;

            //Limpia los arreglos de metadatos
            sVal = sCbz = sTip = sDef = null;

            //Recorre el archivo
            using (StreamReader fsr = new StreamReader(Archivo)) {

                sLinea = fsr.ReadLine();
                while (sLinea != null) {
                    sVal = null;

                    if (AnalizaLinea(sLinea, Sep) && sVal != null) {
                        //Hay datos nuevos que cargar
                        linCbz = string.Join(",", sCbz);  //linea de encabezados

                        for (int i = 0; i < sTip.Length || i < sVal.Length; i++) {
                            string tT = sTip[i].ToUpper();
                            //Si campo es texto hay que agregar ''
                            if (tT.Contains("TEXT") || tT.Contains("STRIN") || tT.Contains("CADE"))
                                sVal[i] = "'" + sVal[i] + "'";
                            //Si el campo es tipo fecha y no hay una fecha pone 1/1/1900
                            if (tT.Contains("DATE") || tT.Contains("FECH")) {
                                DateTime.TryParse(sVal[i], out dT);
                                sVal[i] = "'" + dT.ToString("yyyy/MM/dd") + "'";
                            }
                            //Si el campo es Hora poner 00:00
                            if (tT.Contains("HORA") || tT.Contains("TIME")) {
                                DateTime.TryParse(sVal[i], out dT);
                                sVal[i] = "'" + dT.ToString("HH:mm:ss") + "'";
                            }
                            //Si es booleano
                            if (tT.Contains("SI/NO") || tT.Contains("S/N") || tT.Contains("YES/NO") || tT.Contains("BOOL")) {
                                string sC = sVal[i].Trim().ToUpper();
                                sVal[i] = ((sC == "NO") || (sC == "N") || (sC == "0")) ? "0" : "1";
                            }
                            //si es un numero y no tiene uno pone 0
                            if (tT.Contains("NUMER") || tT.Contains("ENTER") || tT.Contains("DOBLE"))
                                if (!double.TryParse(sVal[i], out dB))
                                    sVal[i] = "0";

                        }


                        linVal = string.Join(",", sVal);

                        cnt += CargaUnRegistro(DBConex, Tabla, linCbz, linVal);
                    }
                    sLinea = fsr.ReadLine();
                }

            }
            MessageBox.Show(cnt.ToString() + " registros actualizados\n en la tabla " + Tabla);
        }

        /// <summary>
        /// Recibe una línea con campos separados por 'Sep' y carga el arreglo
        /// según corresponda al identificador de línea: //N: encabezados, //T:Tipos, 
        /// //D: Defecto, // : comentario, no devuelve nada. Sin // se asume Valores.
        /// </summary>
        /// <param name="Linea">Linea con los campos separados por Sep</param>
        /// <param name="Sep">caracter separador de campos</param>
        /// <returns>true si procesó una línea válida</returns>
        private static bool AnalizaLinea(string Linea, char Sep) {
            string sIni, sLin;
            string[] sCp;
            bool Ret = false;
            try {
                // si la linea tiene menos de tres caracteres no se considera
                if (Linea.Length < 3) return Ret;
                //Ver que tipo de línea es
                sIni = Linea.Substring(0, 3);

                //si hay // se despeja la parte válida
                if (sIni.Substring(0, 2) == "//")
                    sLin = Linea.Substring(3);
                else {
                    sLin = Linea;
                    sIni = "";
                }
                //Separa los campos, y saca los espacios
                sCp = sLin.Split(Sep);
                if (sIni != "")
                    for (int i = 0; i < sCp.Length; i++)
                        sCp[i] = sCp[i].Trim();

                //ver que caracter identificador y actuar en consecuencia
                switch (sIni.ToUpper()) {
                    case "//N":
                        //Nombres de campos
                        sCbz = sCp;
                        break;
                    case "//T":
                        //Tipos de datos de campos
                        sTip = sCp;
                        break;
                    case "//D":
                        //Valores por defecto de campos
                        sDef = sCp;
                        break;
                    case "":
                        //Valores de datos
                        sVal = sCp;
                        break;
                        //En cualquier otro caso no hace nada, se considera commentario
                }
                Ret = true;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message + " - en: " + e.TargetSite.Name);
                Ret = false;
            }

            return Ret;
        }

        /// <summary>Carga un registro de datos en la tabla de la base indicada, con 
        /// la distribución de campos, los valores y los valores por defecto dados
        /// </summary>
        /// <param name="DBConex">Conexión de base de datos. </param>
        /// <param name="Tabla">Tabla donde se cargará. </param>
        /// <param name="Encabezados">Listado de nombres de campos. </param>
        /// <param name="Valores">valores de datos a cargar. </param>
        /// <returns></returns>
        private static int CargaUnRegistro(MySqlConnection DBConex, string Tabla,
                                                    string Encabezados, string Valores) {
            int numeroRegistrosAfectados = 0;

            //try {                
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = DBConex;
            cmd.CommandText = "REPLACE INTO " + Tabla + " (" + Encabezados +
                            ") VALUES (" + Valores + " )";

            cmd.Prepare();
            numeroRegistrosAfectados = cmd.ExecuteNonQuery();
            /*}
            catch  (Exception e) {
                MessageBox.Show("error al cargar tabla: " + e.Message);
            }*/

            return numeroRegistrosAfectados;


        }

        /// <summary>
        /// Conecta a la base de datos MySQL de la NAS (10.1.3.125) con el usuario y contraseña que se encuentra en el archivo .ini
        /// </summary>
        /// <returns></returns>
        public static bool AbrirBaseDatos(string Entorno) {
            StringBuilder stB = new StringBuilder();
            bool retorno = true;
          

            try {
                if (conexBD != null /*|| conexBDHistorial != null*/)

                conexBD.Close();
                
                conexBD = new MySqlConnection();
                //conexBDHistorial = new MySqlConnection();

                Inis.GetPrivateProfileString("Archivos", "Servidor Base Datos", "", stB, 250, Ctte.ArchivoIniName);
                ServidorBD = stB.ToString().Trim();
                Vble.ServidorBD = ServidorBD;
                Inis.GetPrivateProfileString("Datos", "centroInterfaz", "", stB, 250, Ctte.ArchivoIniName);
                Vble.centroInterfaz = stB.ToString().Trim();
                Inis.GetPrivateProfileString("Datos", "locCentroInterfaz", "", stB, 250, Ctte.ArchivoIniName);
                Vble.locCentroInterfaz = stB.ToString().Trim();
                Inis.GetPrivateProfileString("Localidades", Vble.locCentroInterfaz, "", stB, 250, Ctte.ArchivoIniName);
                Vble.LocAsociadas = stB.ToString().Trim();




                //Inis.GetPrivateProfileString("Archivos", "BaseHistorial", "", stB, 250, Ctte.ArchivoIniName);
                //NombreBDHistorial = stB.ToString().Trim();
                //Esta conexion contendrá la base datosdpec que se utiliza para el uso diario de cargas y descargas de rutas
                //Server =10.1.3.125; Database =datosdpec; Uid =admin; Pwd =Micc4001; Convert Zero Datetime = True
                if (Entorno == "PRD" || Entorno == "SUP")
                {
                    Inis.GetPrivateProfileString("Archivos", "Base PRD", "", stB, 250, Ctte.ArchivoIniName);
                    NombreBD = stB.ToString().Trim();

                    //connectionString = string.Format("Server={0};Database={1}; Uid={2}; Pwd={3}; Convert Zero Datetime=True; Connection Timeout=900; SslMode = none",
                    connectionString = string.Format("Server={0};Database={1}; Uid={2}; Pwd={3}; Convert Zero Datetime=True; Connection Timeout=900; SslMode = none;Pooling=true", //Agrego dos parametros para probar las desconexiones; Pooling = true; Reconnect = true
                                       ServidorBD, NombreBD, sDbUsu, sDbKey);
                    connectionStringAdmin = string.Format("Server={0};Database={1}; Uid=admin; Pwd=Micc4001; Convert Zero Datetime=True; Connection Timeout=900; SslMode = none",
                                       ServidorBD, NombreBD);
                    conexBD.ConnectionString = connectionString;
                    conexBD.Open();
                }
                else if (Entorno == "QAS")
                {
                    Inis.GetPrivateProfileString("Archivos", "Base QAS", "", stB, 250, Ctte.ArchivoIniName);
                    NombreBD = stB.ToString().Trim();

                    // connectionString = string.Format("Server={0};Database={1}; Uid={2}; Pwd={3}; Convert Zero Datetime=True; Connection Timeout=900; SslMode = none",
                    connectionString = string.Format("Server={0};Database={1}; Uid={2}; Pwd={3}; Convert Zero Datetime=True; Connection Timeout=900; SslMode = none; Pooling=true",
                                    ServidorBD, NombreBD, sDbUsu, sDbKey);
                    conexBD.ConnectionString = connectionString;
                    conexBD.Open();
                    //; MultipleActiveResultSets=True
                }

                ////Esta conexion contendrá la base HistorialDatosDPEC que se utiliza para llevar el registro historico de todos los usuarios
                ////identificados por periodo y conexionID
                ////Server =10.1.3.125; Database =HistorialDatosDPEC; Uid =admin; Pwd =Micc4001; Convert Zero Datetime = True
                //connectionStringHistorial = string.Format("Server={0};Database={1}; Uid={2}; Pwd={3}; Convert Zero Datetime=True; Connection Timeout=500",
                //   ServidorBD, NombreBDHistorial, sDbUsu, sDbKey);
                //conexBDHistorial.ConnectionString = connectionStringHistorial;
                //conexBDHistorial.Open();

                //StringBuilder stb1 = new StringBuilder("", 250);
                //Inis.GetPrivateProfileString("Datos", "Base", "", stb1, 250, Ctte.ArchivoIniName);
                //Vble.CarpetaSqlite = stb1.ToString().Trim();




                Vble.CarpetaSqlite = Ctte.CarpetaRecursos + "\\" + Vble.NombreArchivoBaseSqlite();
                con = new SQLiteConnection("Data Source=" + Vble.CarpetaSqlite);
                //con = new SQLiteConnection("Data Source=" + Vble.CarpetaSqlite + "; Password=alVlgeDdL");
                //con.Open();

                Vble.BaseChicaFISPC = Ctte.CarpetaRecursos + "\\" + Vble.NombreArchivoBaseFijaSqlite();
                conSQLiteFija = new SQLiteConnection("Data Source=" + Vble.BaseChicaFISPC);


            }
            catch (MySqlException e) {
                Ctte.ArchivoLog.EscribirLog(e.Message);
                //MessageBox.Show("Error " + e.Message, "ERROR");
                retorno = false;
            }
            return retorno;
        }

       
        /// <summary> Se toman los datos desde el repositorio de DPEC y se cargan en la base de datos
        /// propia de esta aplicación. Dentro de este proceso tambien se carga la tabla InfoConex, en la
        /// que se coloca a la conexión inicialmente en la partición A, y el código de impresión en '0'.
        /// </summary>
        /// <returns></returns>
        public static bool ImportarDatos() {

            //por el momento solo se tomará la tabla existente de conexiones del periodo en proceso
            //y se agrega a la tabla InfoConex si es que NO está cargada
            DataTable Tabla;
            MySqlDataAdapter datosAdapter;
            MySqlCommandBuilder comandoSQL;
            string txSQL;
            //int Distr, Rem, Rut, Sec;
            
            txSQL = "INSERT INTO infoconex (conexionid) "+
                        "SELECT T1.conexionid " +
                        " FROM conexiones T1" +
                        " LEFT OUTER JOIN infoconex T2 " +
                        " ON T1.conexionid = T2.conexionid" +
                        " WHERE T1.periodo = " + Vble.Periodo +
                        " AND T2.conexionid is null " ;

                Tabla = new DataTable();

                datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
                comandoSQL = new MySqlCommandBuilder(datosAdapter);
                datosAdapter.Fill(Tabla);

            MessageBox.Show("Actualizados {0} registros", Tabla.Rows.Count.ToString());

            return true;
        }


    }  //Final clase DB

    /// <summary>
    /// Se cargan aquí los datos generales necesarios para el funcionamiento
    /// </summary>
    public static class DatosGenerales {

        /// <summary>
        /// Obtiene o establece el porcentaje a partir del cual lecturas por debajo
        /// de este valor se solicitará confirmación en la toma de lecturas
        /// </summary>
        public static int validaLecturaBaja { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje a partir del cual lecturas por encima
        /// de este valor se solicitará confirmación en la toma de lecturas
        /// </summary>
        public static int validaLecturaAlta { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje a partir del cual una lectura inferior a este
        /// valor en relación al promedio, NO se imprimirá.
        /// </summary>
        public static int validaImpresionBaja { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje a partir del cual una lectura superior a este
        /// valor en relación al promedio, NO se imprimirá.
        /// </summary>
        public static int validaImpresionAlta { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje a partir del cual una lectura superior a este
        /// valor en relación al promedio, pedirá confirmación para imprimir.
        /// </summary>
        public static int validaImpresionConfirma { get; set; }

        /// <summary>
        /// Obtiene o establece el número de lote de carga.
        /// </summary>
        public static int nroLote { get; set; }

        /// <summary>
        /// Obtiene o establece el número de secuencia actual, arranca en cero el las colectoras
        /// </summary>
        public static int nroSecuencia { get; set; }
        public static int puntoVenta { get; set; }
        public static int facturaA { get; set; }
        public static int facturaB { get; set; }
        public static int facturaX { get; set; }
        public static DateTime fechaCarga { get; set; }
        public static DateTime horaCarga { get; set; }
        public static int periodoFacturacion { get; set; }
        public static string resolucionTarifa { get; set; }
        public static string codigoLecturista { get; set; }
        public static string flgSubTotGravados { get; set; }
        public static int codigoEquipo { get; set; }
        public static int localidadEmision { get; set; }




    }

    #region Funciones
    public class Funciones
    {
        public static string ObtenerObservNovedades(Int32 ConexionID, int Periodo)
        {
            DataTable Tabla = new DataTable();
            string Observacion = "";
            string txSQL = "SELECT * FROM NovedadesConex WHERE ConexionID = " + ConexionID + " and Periodo = " + Periodo;
            MySqlDataAdapter datosAdapter = new MySqlDataAdapter(txSQL, DB.conexBD);
            MySqlCommandBuilder comandoSQL = new MySqlCommandBuilder(datosAdapter);
            datosAdapter.Fill(Tabla);

            foreach (DataRow fi in Tabla.Rows)
            {
                if (fi["Observ"].ToString().Contains("|"))
                {
                    Observacion = fi["Observ"].ToString();
                }                
            }
            return Observacion;
        }

        /// <summary>
        /// Proceso que lee el archivo ZonaFIS.txt que contiene las localidades de la interfaz en el cual se esta trabajando, 
        /// la misma esta ubicada en el directorio C:\Windows\ZonaFIS.txt ubicación común para todas las interfaces de GagFIS-Interface 
        /// </summary>
        public static void LeerArchivoZonaFIS()
        {

            //Vble.ArrayZona.Clear();
            int cant = Vble.ArrayZona.Count;

            for (int i = 0; i < cant; i++)
            {
                Vble.ArrayZona.Remove(i);
            }

            try
            {
                using (StreamReader sr = new StreamReader(Ctte.ArchivoZonaFIS))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        String value = line;
                        Char delimiter = ',';
                        String[] substrings = value.Split(delimiter);

                        for (int i = 0; i < substrings.Length; i++)
                        {
                            Vble.ArrayZona.Add(substrings[i]);

                        }
                    }

                }
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message + " Error al leer el archivo que contiene informacion de localidades de la Interfaz.",
                                            "Error de Archivo Interfaz", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        ///// <summary>
        ///// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        ///// que se recibe como parametro.
        ///// </summary>
        ///// <param name="RutaDatos"></param>
        ///// <param name="LeyendaImpresion"></param>
        ///// <param name="ImpresionOBS"></param>                    
        //public static void VerDetallePreDescarga(string LeyendaImpresion, string ImpresionOBS, int Periodo, string CONSULTA)
        //{
        //    FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
        //    DetalleImpresos.IndicadorTipoInforme = "Resumen";
        //    DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
        //    DetalleImpresos.ImpresionOBS = ImpresionOBS;
        //    //DetalleImpresos.RutaDatos = RutaDatos;
        //    DetalleImpresos.Periodo = Vble.Periodo;
        //    DetalleImpresos.CONSULTA = CONSULTA;
        //    DetalleImpresos.Show();
        //}

        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        /// que se recibe como parametro.
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="LeyendaImpresion"></param>
        /// <param name="ImpresionOBS"></param>                    
        public static void VerDetallePreDescarga(string LeyendaImpresion, int Periodo, string CONSULTA, 
                                                 bool NoImpresos, string ImpresionOBS, string Remesa, 
                                                 string Desde, string Hasta, string Ruta, string IndicadorTipoInforme)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            
            DetalleImpresos.IndicadorTipoInforme = IndicadorTipoInforme;
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            //DetalleImpresos.leyenda.Text = Leyenda;
            //DetalleImpresos.CONSULTANOIMPRESOS = "";
            DetalleImpresos.CONSULTANOIMPRESOS = CONSULTA;
            DetalleImpresos.ultimaConsultaReg = CONSULTA;
            Vble.queryInicialExpor = CONSULTA;
            DetalleImpresos.Periodo = Vble.Periodo;
            DetalleImpresos.RBSelectionRemesa = "NO";
            DetalleImpresos.TextBoxRuta.Text = Ruta;
            DetalleImpresos.Ruta = Ruta;
            //DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.NoImpr = NoImpresos;
            DetalleImpresos.WindowState = FormWindowState.Maximized;
            DetalleImpresos.ImpresionOBS = ImpresionOBS;
            DetalleImpresos.Remesa = Remesa;            
            DetalleImpresos.Desde = Desde;
            DetalleImpresos.Hasta = Hasta;
            DetalleImpresos.Visible = false;
            DetalleImpresos.CBRemesa.Text = Remesa;
            DetalleImpresos.DTPDesdeTomLect.Text = Convert.ToDateTime(Desde).ToString("dd/MM/yyyy");
            DetalleImpresos.DTPHastaTomLect.Text = Convert.ToDateTime(Hasta).ToString("dd/MM/yyyy");
            Vble.lectOrd.Clear();
            DetalleImpresos.Show();
         
        }

        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        /// que se recibe como parametro por Remesa
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="LeyendaImpresion"></param>
        /// <param name="ImpresionOBS"></param>                    
        public static void VerDetallePreDescargaR(string LeyendaImpresion, int Periodo, string CONSULTA,
                                                 bool NoImpresos, string ImpresionOBS, string Remesa,
                                                 string Ruta, string IndicadorTipoInforme, string Leyenda)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();

            DetalleImpresos.IndicadorTipoInforme = IndicadorTipoInforme;
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.leyenda.Text = Leyenda;
            //DetalleImpresos.CONSULTANOIMPRESOS = "";
            DetalleImpresos.CONSULTANOIMPRESOS = CONSULTA;
            DetalleImpresos.Periodo = Vble.Periodo;

            DetalleImpresos.TextBoxRuta.Text = Ruta;
            DetalleImpresos.Ruta = Ruta;
            //DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.NoImpr = NoImpresos;
            DetalleImpresos.WindowState = FormWindowState.Maximized;
            DetalleImpresos.ImpresionOBS = ImpresionOBS;
            DetalleImpresos.Remesa = Remesa;
            //DetalleImpresos.Desde = Desde;
            //DetalleImpresos.Hasta = Hasta;
            DetalleImpresos.Visible = false;
            DetalleImpresos.CBRemesa.Text = Remesa;
            //DetalleImpresos.DTPDesdeTomLect.Text = Convert.ToDateTime(Desde).ToString("dd/MM/yyyy");
            //DetalleImpresos.DTPHastaTomLect.Text = Convert.ToDateTime(Hasta).ToString("dd/MM/yyyy");
            
            DetalleImpresos.Show();
            
        }

        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        /// que se recibe como parametro.
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="LeyendaImpresion"></param>
        /// <param name="ImpresionOBS"></param>                    
        public static void VerDetallePreDescargaR(string LeyendaImpresion, int Periodo, string CONSULTA,
                                                 bool NoImpresos, string ImpresionOBS, string Remesa,
                                                 string Ruta, string IndicadorTipoInforme, string Leyenda, string TipoInforme)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();

            DetalleImpresos.IndicadorTipoInforme = IndicadorTipoInforme;
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.leyenda.Text = Leyenda;           
            //DetalleImpresos.CONSULTANOIMPRESOS = "";
            DetalleImpresos.CONSULTANOIMPRESOS = CONSULTA;
            DetalleImpresos.Periodo = Vble.Periodo;
            //DetalleImpresos.RBSelectionRemesa = "SI";
            DetalleImpresos.TextBoxRuta.Text = Ruta;
            DetalleImpresos.Ruta = Ruta;
            //DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.NoImpr = NoImpresos;
            DetalleImpresos.WindowState = FormWindowState.Maximized;
            DetalleImpresos.ImpresionOBS = ImpresionOBS;
            DetalleImpresos.Remesa = Remesa;

            //DetalleImpresos.Desde = Desde;
            //DetalleImpresos.Hasta = Hasta;
            DetalleImpresos.Visible = false;
            DetalleImpresos.CBRemesa.Text = Remesa;
            //DetalleImpresos.DTPDesdeTomLect.Text = Convert.ToDateTime(Desde).ToString("dd/MM/yyyy");
            //DetalleImpresos.DTPHastaTomLect.Text = Convert.ToDateTime(Hasta).ToString("dd/MM/yyyy");
            DetalleImpresos.TipoInforme = TipoInforme;
            DetalleImpresos.Show();

        }

        /// <summary>
        /// Método que realiza la consulta y muestra en una ventana secundaria las conexiones de la pre-descarga segun el estado ImpresionOBS
        /// que se recibe como parametro.
        /// </summary>
        /// <param name="RutaDatos"></param>
        /// <param name="LeyendaImpresion"></param>
        /// <param name="ImpresionOBS"></param>                    
        public static void VerDetallePreDescargaPorRemesa(string LeyendaImpresion, int Periodo, string CONSULTA,
                                                 bool NoImpresos, string ImpresionOBS, string Remesa, 
                                                 string IndicadorTipoInforme, string Leyenda, string TipoInforme, string RBSelection,
                                                 string ResZona)
        {
            FormDetallePreDescarga DetalleImpresos = new FormDetallePreDescarga();
            DetalleImpresos.RBSelectionRemesa = "SI";
            DetalleImpresos.ResZona = ResZona;
            DetalleImpresos.IndicadorTipoInforme = IndicadorTipoInforme;
            DetalleImpresos.LabelLeyenda.Text = LeyendaImpresion;
            DetalleImpresos.leyenda.Text = Leyenda;
            //DetalleImpresos.CONSULTANOIMPRESOS = "";
            DetalleImpresos.CONSULTANOIMPRESOS = CONSULTA;
            DetalleImpresos.Periodo = Vble.Periodo;

            DetalleImpresos.TextBoxRuta.Enabled = false;           
            //DetalleImpresos.RutaDatos = RutaDatos;
            DetalleImpresos.NoImpr = NoImpresos;
            DetalleImpresos.WindowState = FormWindowState.Maximized;
            DetalleImpresos.ImpresionOBS = ImpresionOBS;
            DetalleImpresos.Remesa = Remesa;
            //DetalleImpresos.Desde = Desde;
            //DetalleImpresos.Hasta = Hasta;
            DetalleImpresos.Visible = false;
            DetalleImpresos.CBRemesa.Text = Remesa;
            //DetalleImpresos.DTPDesdeTomLect.Text = Convert.ToDateTime(Desde).ToString("dd/MM/yyyy");
            //DetalleImpresos.DTPHastaTomLect.Text = Convert.ToDateTime(Hasta).ToString("dd/MM/yyyy");
            DetalleImpresos.TipoInforme = TipoInforme;
            DetalleImpresos.Show();

        }


        /// <summary>
        /// Busca el numero de la colectora conectada que comience con MICC- recibiendo la direccion de la carpeta contenedora del dispositivo
        /// </summary>
        /// <param name="Ruta"></param>
        /// <returns></returns>
        public static bool BuscarColectora(string Ruta)
        {
            bool existe = false;
            string VarNom = "";
            List<string> VarVal = new List<string>();

            //remplaza las variables dentro de la cadena
            int i1, i2, i3;  // i1:'{'  -  i2:';'  -  i3:'}'
            i1 = Ruta.IndexOf("M");
            while (i1 >= 0)
            {
                i3 = Ruta.IndexOf("-", i1);       //Busca cierre llave
                if (i3 > i1)
                {
                    i2 = Ruta.IndexOf("C", i1, i3 - i1);  //Busca dos puntos
                    if (i2 < i1) i2 = i3;
                    VarNom = Ruta.Substring(i1 + 1, i2 - i1 - 1);
                    VarVal.Add(VarNom);
                    Ruta = Ruta.Substring(0, i1 + 1) +
                            (VarVal.Count - 1).ToString().Trim() +
                            Ruta.Substring(i2);
                    existe = true;

                    return existe;
                }
                i1 = Ruta.IndexOf("{", i1 + 1);
            }  //Hasta aca se tiene la cadena de formato                       
            return existe;
        }

        /// <summary>
        /// Busca la descripcion de la colectora recibiendo como parametro el string donde se ubica la carpeta que contiene la colectora
        /// </summary>
        /// <param name="Ruta"></param>
        /// <returns></returns>
        public static string BuscarNombreColectora(string Ruta)
        {
            string existe = "";
            string VarNom = "";
            List<string> VarVal = new List<string>();

            //remplaza las variables dentro de la cadena
            int i1, i2, i3;  // i1:'{'  -  i2:';'  -  i3:'}'
            i1 = Ruta.IndexOf("{");

            while (i1 >= 0)
            {
                i3 = Ruta.IndexOf("}", i1);       //Busca cierre llave

                if (i3 > i1)
                {
                    i2 = Ruta.IndexOf(":", i1, i3 - i1);  //Busca dos puntos
                    if (i2 < i1) i2 = i3;
                    VarNom = Ruta.Substring(i1 + 1, i2 - i1 - 1);

                    VarVal.Add(VarNom);

                    existe = VarNom;

                    return existe;
                }

            }  //Hasta aca se tiene la cadena de formato                        
            return existe;
        }


        

        /// <summary>
        /// Lee el archivo InfoCarga.txt/InfoDescarga.txt que se genera al procesar la carga y se envia junto con la Base SQLite para saber 
        /// los datos que se procesaron a la hora de Cargar y Descargar la Colectora
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <returns></returns>
        public static string LeerArchivostxt(string RutaArchivo)
        {
            string line, linTool = " ";
            if (File.Exists(RutaArchivo))
            {
                using (StreamReader sr = new StreamReader(RutaArchivo, false))
                {                    
                    linTool = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        linTool += line + "\n";
                    }
                    sr.Dispose();
                    return linTool;                    
                }
            }
            else
            {
                return linTool = " * No existe archivo InfoCarga. \n  ";                
            }            
        }

        /// <summary>
        /// Metodo que verifica si el usuario es un prosumidor, es decir se cargo un estado de energia inyectada
        /// para cargarlo en el archivo upload en el campo observaciones.
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <param name="Periodo"></param>
        /// <param name="Numero"></param>
        /// <returns></returns>
        public static string EnergiaInyectada(Int32 ConexionID, int Periodo, string Numero)
        {
            string ValorInyectado = "";           
            string txSQL = "SELECT Estado FROM Altas WHERE ConexionID = " + ConexionID + " and Periodo = " + Periodo + " AND Numero = '" + Numero + "' " +
                           "AND Activa = 'Y' ORDER BY Fecha, Hora DESC";
            MySqlCommand cmd = new MySqlCommand(txSQL, DB.conexBD);
            if (cmd.ExecuteScalar() != null)
            {
                ValorInyectado = "(INY:" + cmd.ExecuteScalar().ToString() + ")";
            }            
            return ValorInyectado;
        }

        /// <summary>
        /// Metodo que busca si la conexion tiene registro de Energia Reactica (Grandes Usuarios) si tiene agrega una linea nueva
        /// </summary>
        /// <param name="ConexionID"></param>
        /// <param name="Periodo"></param>
        /// <param name="Numero"></param>
        /// <returns></returns>
        public static string EnergiaReactivaGU(Int32 ConexionID, int Periodo, string Numero)
        {
            string ValorReactiva = "";
            string txSQL = "SELECT Estado FROM Altas WHERE ConexionID = " + ConexionID + " and Periodo = " + Periodo + " AND Numero = '" + Numero + "' " +
                           "AND Activa = 'R' ORDER BY Fecha, Hora DESC";
            MySqlCommand cmd = new MySqlCommand(txSQL, DB.conexBD);
            if (cmd.ExecuteScalar() != null)
            {
                ValorReactiva = cmd.ExecuteScalar().ToString();
                Vble.EnergiaReactiva = ValorReactiva;
            }
            return ValorReactiva;
        }
    }    
    #endregion
    /// <summary> Estructura de los campos usados en los archivos donde se
    /// guarda la estructura de las tablas
    /// </summary>
    public class clCampoArchivo {
        public clCampoArchivo() { }
        /// <summary>Recibe una línea que fue leida de un archivo de estructura, y separa
        /// los datos segun se indica:
        /// Nombre (Sep) Tipo (Sep) Formato (charComentario) Comentario.
        /// Debe tener al menos los dos primeros datos, de lo contrario no se considera.
        /// </summary>
        /// <param name="Linea">Linea con datos</param>
        /// <param name="Sep">Caracter que separa los campos</param>
        /// <param name="charComentario">caracter que inicia un comentario</param>
        /// <param name="Indice">Posición del campo dentro de la estructura de la tabla, base cero</param>
        public clCampoArchivo(string Linea, string Separador, string charComentario, int indice) {
            SepararCampos(Linea, Separador, charComentario, indice);
        }
        /// <summary>Recibe una línea que fue leida de un archivo de estructura, y separa
        /// los datos segun se indica:
        /// Nombre (Sep) Tipo (Sep) Formato (charComentario) Comentario
        /// Debe tener al menos los dos primeros datos, de lo contrario no se considera.
        /// Asume: Separador='|', charCopmentario=';' e indice =-1, despues corrigue.
        /// </summary>
        /// <param name="Linea">Linea con los datos</param>
        public clCampoArchivo(string Linea) {
            SepararCampos(Linea, "|", ";", -1);
        } 
        /// Nombre del campo
        public string Nombre { get; set; }
        /// Nombre del tipo de dato que tiene el campo
        public string Tipo { get; set; }
        /// Formato del campo, si corresponde
        public string Formato { get; set; }
        ///Posición del campo dentro de la estructura (base cero)
        public int Indice { get; set; }
        ///Comentario que pudiera estar.
        public string Comentario { get; set; }

       
        /// <summary>Separa los campos de acuerdo con lo indicado 
        /// Sen el constructor
        /// </summary>
        /// <param name="Linea"></param>
        /// <param name="Separador"></param>
        /// <param name="charComentario"></param>
        /// <param name="indice"></param>
        private void SepararCampos(string Linea, string Separador, string charComentario, int indice) {
            string sDat;
            string[] Sep = { Separador };
            int iCom;
            Linea += " "; //agrega el espacio para evitar error si no hay nada despues del ';'

            //Separa la parte de comentario
            iCom = Linea.IndexOf(charComentario);
            if(iCom >= 0)
                Comentario = Linea.Substring(iCom + 1);
            else
                Comentario = "";

            if(iCom < 0)
                sDat = Linea;
            else
                sDat = Linea.Substring(0, iCom );
            
            //Separa los datos 
            string[] Dt = sDat.Split(Sep, StringSplitOptions.None);
            //Solo es válido si tiene al menos Nombre y Tipo
            if(Dt.Count() > 1) {
                Nombre = Dt[0].Trim();
                Tipo = Dt[1].Trim();
                Formato = (Dt.Count() > 2) ? Dt[2].Trim() : "";
                Indice = indice;
            }
            else {
                //Si no tiene al menos Nombre y Tipo, devuelve vacios:
                Comentario = "";
                Nombre = "";
                Tipo = "";
                Formato = "";
                Indice = -1;
            }
           
            
        }



    }

}
