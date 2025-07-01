using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
//using System.Drawing;
using ApodemiaC;
using BLEDeviceAPI;
using Newtonsoft.Json;



//using UHFAPP.barcode;
using UHFAPP.utils;

using WMPLib;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static Lucene.Net.Documents.Field;
using static UHFAPP.UHFAPI;
using static UHFAPP.utils.EpcInfo;



namespace UHFAPP
{
    public partial class MainFormOrdenInversa : BaseForm
    {
        public static int MODE = 1;//0:串口   1:网口    2:usb
        public static string ip = "";
        public static uint portData = 0;
        private GetMozo getMozo;
        public delegate void DelegateOpen(bool open);
        public static event DelegateOpen eventOpen = null;
        public delegate void DelegateSwitchUI();
        public static event DelegateSwitchUI eventSwitchUI = null;
        public static bool isVisible = false;
        public delegate void MainSizeChanged(FormWindowState state);
        public static event MainSizeChanged eventMainSizeChanged = null;
        bool isRuning = false;
        int cantidadReal = 0;
        string strOpen = "  Conectar lector  ";
        string strClose = "  Desconectar lector  ";
        string strStart = "  Iniciar lectura  ";
        string strStop = "  Detener lectura  ";
        string strStartConsulta = "  Iniciar Orden Inversa  ";
        string strStopConsulta = "  Detener Orden Inversa  ";
        private List<string> tags = new List<string>();
        private List<int> tagsCantidad = new List<int>();
        private string currentFormName = "";
        private bool isOpen = false;
        private bool consulta;
        private int idOrdenAmipem;
        private string apiMozoBase;
        private string apiAmipemBase;
        private string cliente;
        private string usuario;
        private string password;
        private string producto;
        private string gtin;
        private int totalLote;
        private int totalesValidos;
        private int lotesOrden;
        public MainFormOrdenInversa mainform = null;
        private int cantidadPorLote;
        private int loteActual = 1;
        private string ubicación;
        private string session;
        private string potencia;
        private string tagFocus;
        private string buzzer;
        private bool sinValidar;
        private string idProducto;
        private Thread hiloLectura;

        private string ipAntena;
        private string puertoAntena;
        private bool biocam;
        private UHFAPP.UHFAPI.OnDataReceived onDataReceived = DataReceived;
        SortedDictionary<string, int> tagsOrden = new SortedDictionary<string, int>();
        SortedDictionary<string, int> tagsComprobacion = new SortedDictionary<string, int>();
        public static FormWindowState currState = FormWindowState.Normal;
        List<EpcInfo> epcList = new List<EpcInfo>();
        public bool isSearch = false;
        UHFAPP.UHFAPI.OnDisconnectCallback DisconnectCallback = null;
        private string epcErroneo;
        bool result = false;
        #region  OnDisconnect

        private static void DataReceived(IntPtr pdata, short len)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " DataReceived begin");
            short contentLen;
            short index = 0;
            byte type;

            byte[] cellData = new byte[len];
            Marshal.Copy(pdata, cellData, 0, len);
            // cellData= Utils.CopyArray(pdata,0,len);

            byte[] pcontent;
            // printf("OnReceivedData:");
            for (int i = 0; i < len; i++)
            {
                // Console.WriteLine("%02X", cellData[i]);
            }
            byte[] temp = null;
            string str;
            //  printf("\n");
            while (index < len)
            {
                type = cellData[index++];
                if ((cellData[index] & 0x80) == 0x80)
                {
                    contentLen = (short)(((cellData[index] & 0x7F) << 7) | (cellData[index + 1] & 0x7F));
                    index += 2;
                }
                else
                {
                    contentLen = cellData[index++];
                }

                pcontent = Utils.CopyArray(cellData, index, contentLen);

                index += contentLen;
            }
        }

        private void OnDisconnectCallback(int id)
        {
            try
            {
                if (!this.IsDisposed)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        button1.Text = strOpen;
                        button1.BackColor = System.Drawing.Color.Red;
                        isOpen = false;
                        if (eventOpen != null)
                        {
                            eventOpen(false);
                        }

                    }));

                }
            }
            catch (Exception ex)
            {
                log("OnDisconectCallback:" + ex.Message);
            }

        }

        #endregion
        public MainFormOrdenInversa()
        {
            cargarDatos("parametros.txt");
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.IsMdiContainer = true;
            mainform = this;
            button1.Text = strOpen;
            button1.BackColor = System.Drawing.Color.Red;
            DisconnectCallback = OnDisconnectCallback;
            Screen primaryScreen = Screen.PrimaryScreen;
            System.Drawing.Rectangle bounds = primaryScreen.Bounds;
            this.Size = new System.Drawing.Size(bounds.Width, bounds.Height);
            panel9.Size = new System.Drawing.Size(bounds.Width, bounds.Height);
            epcs.Size = new System.Drawing.Size((bounds.Width / 4)*3, (bounds.Height * 2 / 3));
            epcs.Location = new System.Drawing.Point(bounds.Width / 8, 200);
           // btnScanEPC.Location = new System.Drawing.Point(((bounds.Width / 4) * 2) + 50, bounds.Height - 120);
            buttonConsulta.Location = new System.Drawing.Point(((bounds.Width / 4) ), bounds.Height - 150);

        }
        public void cargarDatos(string archivo)
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\" + archivo);
            //Read the first line of text
            apiAmipemBase = leerParametro(sr);
            apiMozoBase = leerParametro(sr);
            cliente = leerParametro(sr);
            usuario = leerParametro(sr);
            password = leerParametro(sr);
            ubicación = leerParametro(sr);
            potencia = leerParametro(sr);
            session = leerParametro(sr);
            tagFocus = leerParametro(sr);
            buzzer = leerParametro(sr);
            sinValidar = Boolean.Parse(leerParametro(sr));
            ipAntena = leerParametro(sr);
            puertoAntena = leerParametro(sr);
            sr.Close();
            Console.ReadLine();
            //loteActual++;


        }
        private string leerParametro(StreamReader sr)
        {
            string lectura = sr.ReadLine();
            while (lectura.StartsWith("#"))
            {
                lectura = sr.ReadLine();
            }
            return lectura;

        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSearch = false;
            UHFClose();
            if (eventOpen != null)
            {

                eventOpen(false);
            }
        }
        private bool UHFClose()
        {
            isRuning = false;

            StopReceiveThread();
            try
            {
                if (hiloLectura != null && hiloLectura.IsAlive)
                {
                    hiloLectura.Abort();
                    hiloLectura.Interrupt();
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.Cancel();
                    hiloLectura.Join();
                    hiloLectura.Abort();

                    uhf.CloseUsb();
                    uhf.Close();
                }


            }
            catch (Exception e)
            {
                log("UHFClose:" + e.Message);
            }

            System.Windows.Forms.Application.Exit();
            Environment.Exit(0);

            return true;

        }


        private void toolStripButton1_Click()
        {


            try
            {
                if (!result)
                {
                    result = uhf.OpenUsb();
                    if (!result)
                    {
                        caja("No se pudo conectar al lector, compruebe que este conectado", null, false);
                        panelCaja.Refresh();
                        StopReceiveThread();
                        StopEPC(true);
                        Thread.Sleep(3000);
                        UHFClose();


                    }
                    else
                    {
                        uhf.SetPower(1, Byte.Parse(potencia));
                        uhf.SetTagfocus(Byte.Parse(tagFocus));
                        uhf.SetGen2(0, 0, 0, 1, 4, 0, 15, 1, 2, 1, 1, 1, 0, 3);
                        uhf.UHFSetBuzzer(Byte.Parse(buzzer));
                    }
                }
                // UHFAPI.setOnDataReceived(onDataReceived);

            }
            catch (Exception e)
            {
                log("toolStripButton1_Click:" + e.Message);

            }


            UHFAPI.SetDisconnectCallback(DisconnectCallback);
            button1.BackColor = System.Drawing.Color.GreenYellow;
            button1.Text = strClose;
            btnScanEPC.Visible = true;
            btnScanEPC.BackColor = System.Drawing.Color.Green;
            btnScanEPC.Text = strStart;


        }

        #region RS160 KeyDwon



        public int cantidadTotal { get; private set; }
        public bool finOrden { get; private set; }
        #endregion


        #region  UI
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            currState = WindowState;
            if (eventMainSizeChanged != null)
            {
                eventMainSizeChanged(WindowState);
            }
        }
        #endregion




        private void StopEPC(bool isStop)
        {
            if (isStop)
            {
                if (uhf.StopInventory())
                {
                    if (!consulta)
                        if (btnScanEPC.InvokeRequired)
                        {
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.Text = strStart));
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.BackColor = System.Drawing.Color.Green));
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.ForeColor = System.Drawing.Color.White));
                        }
                        else
                        {
                            btnScanEPC.Text = strStart;
                            btnScanEPC.BackColor = System.Drawing.Color.Green;
                            btnScanEPC.ForeColor = System.Drawing.Color.White;
                        }
                    else if (buttonConsulta.InvokeRequired)
                    {

                        buttonConsulta.Invoke(new Action(() => buttonConsulta.Text = strStartConsulta));
                        buttonConsulta.Invoke(new Action(() => buttonConsulta.BackColor = System.Drawing.Color.Green));
                        buttonConsulta.Invoke(new Action(() => buttonConsulta.ForeColor = System.Drawing.Color.White));

                    }
                    else
                    {
                        buttonConsulta.Text = strStartConsulta;
                        buttonConsulta.BackColor = System.Drawing.Color.Green;
                        buttonConsulta.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            else
            {
                if (uhf.StartInventory())
                {
                    if (!consulta)
                        if (buttonConsulta.InvokeRequired)
                        {
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.Text = strStop));
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.BackColor = System.Drawing.Color.Red));
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.ForeColor = System.Drawing.Color.White));
                        }
                        else
                        {
                            btnScanEPC.Text = strStop;
                            btnScanEPC.BackColor = System.Drawing.Color.Red;
                            btnScanEPC.ForeColor = System.Drawing.Color.White;
                        }
                    else
                    {
                        if (buttonConsulta.InvokeRequired)
                        {
                            buttonConsulta.Invoke(new Action(() => buttonConsulta.Text = strStopConsulta));
                            buttonConsulta.Invoke(new Action(() => buttonConsulta.BackColor = System.Drawing.Color.Red));
                            buttonConsulta.Invoke(new Action(() => buttonConsulta.ForeColor = System.Drawing.Color.White));
                        }
                        else
                        {
                            buttonConsulta.Text = strStopConsulta;
                            buttonConsulta.BackColor = System.Drawing.Color.Red;
                            buttonConsulta.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }

            }

        }


        private void StopReceiveThread()
        {
            isVisible = false;
            isRuning = false;

            // Thread.Sleep(100);
        }


        private void StartReceiveThread()
        {
            if (!isRuning)
            {
                isRuning = true;
                try
                {
                    hiloLectura = new Thread(new ThreadStart(delegate { ReadEPC(); }));
                    hiloLectura.IsBackground = true;
                    hiloLectura.Start();
                }
                catch (Exception e)
                {
                    log("StartReceiveThread:" + e.Message);
                }
            }
        }






        private void caja(string texto, string epc, bool boton)
        {
            try
            {
               

                StopEPC(true);
                uhf.StopInventory();
                epcErroneo = epc;
                textoCaja.Invoke(new Action(() => textoCaja.Text = texto));
                panelCaja.Invoke(new Action(() => panelCaja.Visible = true));
                if (boton)
                    textoCaja.Invoke(new Action(() => botonCaja.Visible = true));
                else
                    textoCaja.Invoke(new Action(() => botonCaja.Visible = false));
            

            }
            catch (Exception e)
            {

                log("caja:" + e.Message);
            }
        }

        private void log(string texto)
        {
            try
            {
                var url = apiAmipemBase + "log";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                string salida = "{\"texto\":\"" + texto + "\"}";
                byte[] data = Encoding.UTF8.GetBytes(salida);
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

            }
            catch (Exception e)
            {
                log(e.Message);
                //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
            }

        }


        private void ScanEPCForm_Load(object sender, EventArgs e)
        {
            //setTextCallback = new SetTextCallback(UpdataEPC);

            /*var bounds = Screen.FromControl(this).Bounds;
            this.Width = bounds.Width - 50;
            this.Height = bounds.Height - 50;
            panel9.Width = this.Width;
            panel9.Height = this.Height;
            epcs.Width = panel9.Width - epcs.Location.X;
            epcs.Height = panel9.Height - epcs.Location.Y - 100;*/
            // this.MaximumSize = SystemInformation.PrimaryMonitorMaximizedWindowSize;
            this.WindowState = FormWindowState.Maximized;
        }
        private void ReadEPC()
        {
            try
            {

                int i = 0;
                while (isRuning)
                {
                    UHFTAGInfo info = uhf.ReadTagFromBuffer();
                    if (info != null && info.Epc.StartsWith("0108"))
                    {


                        if (info != null && !tagsOrden.ContainsKey(info.Epc))
                        {
                            tagsOrden.Add(info.Epc,1);
                            if(ubicación.Equals("mozo"))
                            grabaOrdenInversa(info.Epc, true);
                            else
                                grabaOrdenInversaAmipem(info.Epc);
                        }
                    }
                }
                Console.Write("fin");
            }
            catch (Exception ex)
            {
                log("readEpc:" + ex.Message);
            }
        }

        private void grabaOrdenInversaAmipem(string epc)
        {
            try
            {
                if (tagsOrden.ContainsKey(epc))
                {
                    var url = apiAmipemBase + "ordenInversa";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    DateTime fecha = DateTime.Now;
                    //BajaAmipemOut bajaAmipemOut = new BajaAmipemOut(0, epc, descripcion);
                    string salida = "{\"lecturaRFID\":\"" + epc + "\"}";
                    byte[] data = Encoding.UTF8.GetBytes(salida);
                    request.ContentLength = data.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                  
                }
            }
            catch (Exception e1)
            {
                log("grabaOrdenInversaAmipem:" + e1.Message);
            }

        }

        private bool grabaOrdenInversa(string epc, bool comprobarErrores)
        {
            bool resultado = false;
            if (ubicación.Equals("mozo"))
            {
                var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/registrarOrdenesInversas";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string username = this.usuario;
                string password = this.password;
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + svcCredentials);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "*";
                try
                {
                    OrdenInversaOut ordenInversaOut = new OrdenInversaOut(epc, comprobarErrores, "");
                    string salida = JsonConvert.SerializeObject(ordenInversaOut);
                    byte[] data = Encoding.UTF8.GetBytes(salida);
                    request.ContentLength = data.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    stream.Close();
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        string strsb = sr.ReadToEnd();
                        strsb = strsb.Replace("@odata.context", "dataContext");
                        strsb = strsb.Replace("@odata.etag", "dataTag");
                        OrdenInversaIn ordenInversaIn = JsonConvert.DeserializeObject<OrdenInversaIn>(strsb);
                        int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                        resultado = true;
                        if (ordenInversaIn.textoError != null && ordenInversaIn.textoError.Length > 0 && comprobarErrores)
                        {

                            caja(ordenInversaIn.textoError + "\n" + desglosaEpcErroneo(epc), epc, true);
                            resultado = false;
                            return resultado;
                        }
                        if (ordenInversaIn.textoError != null && ordenInversaIn.textoError.Length == 0)
                        {
                            if (comprobarErrores)
                                grabaOrdenInversa(epc, false);
                            else
                            {
                              desglosaEpc(epc, DateTime.Now.ToString(), true);
                                grabaOrdenInversaAmipem(epc);
                            }
                            resultado = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    log("grabaOrdenInversa:" + e.Message);
                    resultado = false;
                }
            }
            return resultado;
        }

        private void desglosaEpc(string epc, string fecha, bool grabar)
        {

            try
            {
                var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/desglosarRFIDs";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string username = this.usuario;
                string password = this.password;
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + svcCredentials);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "*/*";
                string salida = "{'lecturaRfid':'" + epc + "'}";
                byte[] data = Encoding.UTF8.GetBytes(salida);
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    strsb = strsb.Replace("@odata.context", "dataContext");
                    strsb = strsb.Replace("@odata.etag", "dataTag");
                    Desglose desglose = JsonConvert.DeserializeObject<Desglose>(strsb);
                    if (epcs.InvokeRequired)
                    {
                        epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, "Orden Inversa", fecha })));
                    }
                    else
                    {
                        epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, "Orden Inversa", fecha });
                    }


                }
            }
            catch (Exception e)
            {
                log("desglosaEpc:" + e.Message);
                //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
            }
        }
        private string desglosaEpcErroneo(string epc)
        {
            string salida = "";
            if (ubicación.Equals("casa"))
            {
                return "producto: 123456789\r lote: 123456789\r serie: 1111111111";
            }
            else
            {
                try
                {
                    var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/desglosarRFIDs";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    string username = this.usuario;
                    string password = this.password;
                    string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                    request.Headers.Add("Authorization", "Basic " + svcCredentials);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "*/*";
                    string salidaServicio = "{'lecturaRfid':'" + epc + "'}";
                    byte[] data = Encoding.UTF8.GetBytes(salidaServicio);
                    request.ContentLength = data.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        Stream stream1 = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream1);
                        string strsb = sr.ReadToEnd();
                        strsb = strsb.Replace("@odata.context", "dataContext");
                        strsb = strsb.Replace("@odata.etag", "dataTag");
                        Desglose desglose = JsonConvert.DeserializeObject<Desglose>(strsb);
                        salida = "producto: " + desglose.noProducto + "\r lote: " + desglose.noLote + "\r serie: " + desglose.noSerie;
                    }
                }
                catch (Exception e)
                {
                    log("desglosaEpcErroneo" + e.Message);
                    //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
                }
            }
            return salida;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            consulta = false;

            if (btnScanEPC.Text == strStop)
            {
                StopReceiveThread();
                StopEPC(true);
            }
            else
            {
                StartReceiveThread();
                StopEPC(false);
            }
        }




        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Screen primaryScreen = Screen.PrimaryScreen;
            System.Drawing.Rectangle bounds = primaryScreen.Bounds;
            this.Size = new System.Drawing.Size(bounds.Width, bounds.Height);
            panel9.Size = new System.Drawing.Size(bounds.Width, bounds.Height);
            epcs.Size = new System.Drawing.Size((bounds.Width / 4) * 3, (bounds.Height * 2 / 3));
            epcs.Location = new System.Drawing.Point(bounds.Width / 8, 200);
            epcs.Refresh();
        }


        private void botonCaja_Click(object sender, EventArgs e)
        {

            //StopEPC(false);
            // uhf.StartInventory();
            panelCaja.Visible = false;
            try
            {
                tagsOrden.Remove(epcErroneo);
            }
            catch (Exception e1)
            {
                log("botonCaja_Click:" + e1.Message);
            }
            // StopEPC(false);
        }

        private void buttonConsulta_Click(object sender, EventArgs e)
        {
            
                            tagsOrden.Clear();
                epcs.Rows.Clear();
           
               result = false;
            if (!result)
                result = uhf.OpenUsb();
            if (!result)
            {
                try
                {
                    result = uhf.TcpConnect(ipAntena, UInt32.Parse(puertoAntena));
                }
                catch(Exception e1)
                {
                    log("buttonConsulta_Click:" + e1.Message);
                }
             
            }

            if (!result)
            {
                caja("No se pudo conectar al lector, compruebe que este conectado", null, false);
                panelCaja.Refresh();
                StopReceiveThread();
                StopEPC(true);
                Thread.Sleep(3000);
                UHFClose();


            }
            else
            {
                uhf.SetPower(1, Byte.Parse(potencia));
                uhf.SetTagfocus(Byte.Parse(tagFocus));
                uhf.SetGen2(0, 0, 0, 1, 4, 0, 15, 1, 2, 1, 1, 1, 0, 3);
                uhf.UHFSetBuzzer(Byte.Parse(buzzer));
            }
            if (buttonConsulta.Text == strStopConsulta)
            {
                StopReceiveThread();
                StopEPC(true);
                consulta = false;
                buttonConsulta.Text = strStartConsulta;
                buttonConsulta.BackColor = System.Drawing.Color.Green;
                buttonConsulta.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                consulta = true;
                StartReceiveThread();
                StopEPC(false);
                buttonConsulta.Text = strStopConsulta;
                buttonConsulta.BackColor = System.Drawing.Color.Red;
                buttonConsulta.ForeColor = System.Drawing.Color.White;
            }
        }

        private void buttonCalidad_Click(object sender, EventArgs e)
        {
            StopReceiveThread();
            isSearch = false;
            UHFClose();
            if (eventOpen != null)
            {

                eventOpen(false);
            }
            this.Hide();
           
        }

        private void buttonBajas_Click(object sender, EventArgs e)
        {
            StopReceiveThread();
            isSearch = false;
            UHFClose();
            if (eventOpen != null)
            {

                eventOpen(false);
            }
            this.Hide();
            
        }
    }
}