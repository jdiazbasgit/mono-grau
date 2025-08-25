using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;

using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
//using System.Drawing;
using ApodemiaC;
using BLEDeviceAPI;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Graph;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;



//using UHFAPP.barcode;
using UHFAPP.utils;
using UHFAPP.utils.procedencia;
using WMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static Lucene.Net.Documents.Field;
using static UHFAPP.UHFAPI;
using static UHFAPP.utils.EpcInfo;
using Action = System.Action;



namespace UHFAPP
{
    public partial class MainForm : BaseForm
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
        string strStart = "  Iniciar Altas  ";
        string strStop = "  Detener Altas  ";
        string strStartConsulta = "  Iniciar lectura  ";
        string strStopConsulta = "  Detener lectura  ";
        string biocamStop = "  Detener Biocam  ";
        string biocamStart = "  Iniciar Biocam  ";
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
        public MainForm mainform = null;
        private int cantidadPorLote;
        private int loteActual = 1;
        private string ubicación;
        private string session;
        private string potencia;
        private string tagFocus;
        private string buzzer;
        private string ipAntena;
        private string puertoAntena;
        private bool sinValidar;
        private string idProducto;
        private Thread hiloLectura;
        private bool biocam;
        private UHFAPP.UHFAPI.OnDataReceived onDataReceived = DataReceived;
        SortedDictionary<string, int> tagsOrden = new SortedDictionary<string, int>();
        SortedDictionary<string, int> tagsTotales = new SortedDictionary<string, int>();
        SortedDictionary<string, int> tagsComprobacion = new SortedDictionary<string, int>();
        public static FormWindowState currState = FormWindowState.Normal;
        List<EpcInfo> epcList = new List<EpcInfo>();
        public bool isSearch = false;
        UHFAPP.UHFAPI.OnDisconnectCallback DisconnectCallback = null;
        private string epcErroneo;
        bool result = false;
        private string ordenBiocam;
        private int antena;
        delegate void SetTextCallback(string epc, string tid, string rssi, string count, string ant, string user);
        SetTextCallback setTextCallback;
        delegate void GrabarTagContadorAmipemCallback(string epc, int lote);
        GrabarTagContadorAmipemCallback grabarTagContadorAmipemCallback;
        delegate void GrabarTagContadorMozoCallback(string epc, int lote, bool comprobarErrores);
        GrabarTagContadorMozoCallback grabarTagContadorMozoCallback;
        private int timeOut = 100;
        private int lecturas;
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
            uhf.Close();
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
        public MainForm()
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
            dataGridView1.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 200);
            epcs.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 200);
            epcs.Location = new System.Drawing.Point(bounds.Width / 2, 321);
            btnScanEPC.Location = new System.Drawing.Point(((bounds.Width / 4) * 2) + 50, bounds.Height - 120);
            buttonExcel.Location = new System.Drawing.Point(((bounds.Width / 8) * 5) + 50, bounds.Height - 120);
            buttonConsulta.Location = new System.Drawing.Point((50), bounds.Height - 120);
            buttonBiocam.Location = new System.Drawing.Point(((bounds.Width / 4) * 3) + 50, bounds.Height - 120);
            button3.Location = new System.Drawing.Point(((bounds.Width / 8) * 2) + 50, bounds.Height - 120);
            panelCaja.Location = new System.Drawing.Point(((bounds.Width / 8) * 2) + 50, (bounds.Height / 3));
            uhf = UHFAPI.getInstance();
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
            antena = Int32.Parse(leerParametro(sr));
            timeOut = Int32.Parse(leerParametro(sr));
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
            btnScanEPC.BackColor = System.Drawing.Color.White;
            btnScanEPC.Enabled = false;
            btnScanEPC.Visible = true;

            try
            {
                if (!result)
                {
                    result = uhf.OpenUsb();
                    if (result)
                        bt = true;
                }
                if (!result)
                {
                    result = uhf.TcpConnect(ipAntena, UInt32.Parse(puertoAntena));
                    if (result)
                        tcp = true;
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
                    uhf.SetGen2(0, 0, 0, 1, 4, 0, 15, 1, 2, 1, 1, Byte.Parse(session), 0, 3);
                    uhf.UHFSetBuzzer(Byte.Parse(buzzer));
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
        public bool lecturaDirecta { get; private set; }
        public bool tcp { get; private set; }
        public bool bt { get; private set; }

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


        private void button1_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click();
        }

        private void StopEPC(bool isStop)
        {
            if (isStop)
            {
                if (uhf.StopInventory())
                {
                    hiloLectura.Interrupt();
                    if (!consulta)
                        if (!lecturaDirecta)
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
                        else
                        if (buttonBiocam.InvokeRequired)
                        {
                            buttonBiocam.Invoke(new Action(() => buttonBiocam.Text = biocamStart));
                            buttonBiocam.Invoke(new Action(() => buttonBiocam.BackColor = System.Drawing.Color.Green));
                            buttonBiocam.Invoke(new Action(() => buttonBiocam.ForeColor = System.Drawing.Color.White));
                        }
                        else
                        {
                            buttonBiocam.Text = biocamStart;
                            buttonBiocam.BackColor = System.Drawing.Color.Green;
                            buttonBiocam.ForeColor = System.Drawing.Color.White;
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
                hiloLectura = new Thread(new ThreadStart(delegate { ReadEPC(); }));
                hiloLectura.IsBackground = true;
                hiloLectura.Start();

                if (uhf.StartInventory())
                {
                    if (!consulta)
                        if (!lecturaDirecta)
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
                        if (buttonBiocam.InvokeRequired)
                        {
                            buttonBiocam.Invoke(new Action(() => buttonBiocam.Text = biocamStop));
                            buttonBiocam.Invoke(new Action(() => buttonBiocam.BackColor = System.Drawing.Color.Red));
                            buttonBiocam.Invoke(new Action(() => buttonBiocam.ForeColor = System.Drawing.Color.White));
                        }
                        else
                        {
                            buttonBiocam.Text = biocamStop;
                            buttonBiocam.BackColor = System.Drawing.Color.Red;
                            buttonBiocam.ForeColor = System.Drawing.Color.White;
                        }
                    else if (buttonConsulta.InvokeRequired)
                    {
                        hiloLectura.Start();
                        buttonConsulta.Invoke(new Action(() => buttonConsulta.Text = strStopConsulta));
                        buttonConsulta.Invoke(new Action(() => buttonConsulta.BackColor = System.Drawing.Color.Red));
                        buttonConsulta.Invoke(new Action(() => buttonConsulta.ForeColor = System.Drawing.Color.White));
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




        private void validaProducto(string epc)
        {
            if (!tagsOrden.ContainsKey(epc))
                tagsOrden.Add(epc, Int32.Parse(tbLinea.Text));


            grabarTagContadorAmipem(epc, Int32.Parse(tbLinea.Text));

        }



        private string dameOrdenFromLote(string lote)
        {
            string salidaOrden = "";
            var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/codProcedenciaOrdenesProdArco?$filter=sourceNo eq '" + lote + "'";
            cantidadReal = 0;
            var request = (HttpWebRequest)WebRequest.Create(url);
            string username = usuario;
            string password = this.password;
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Timeout = timeOut;
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {

                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            string responseBodyFormater = responseBody.Replace("@odata.context", "dataContext");
                            responseBodyFormater = responseBodyFormater.Replace("@odata.etag", "odataETag");
                            Procedencia procedencia = JsonConvert.DeserializeObject<Procedencia>(responseBodyFormater);
                            salidaOrden = procedencia.value[0].no;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                caja("producto no es biocam", "", true);
                log("leerOrden:" + ex.Message);
                salidaOrden = "";
                textBox1.Text = "";
            }
            return salidaOrden;
        }

        private void leerOrden()
        {
            if (ubicación.Equals("mozo1"))
            {
                leerOrdenAmipem();
                return;
            }
            string orden = "";
            if (ordenBiocam != null)
                orden = ordenBiocam;
            else
            {

                if (textBox1.Text.Trim().Equals(""))
                {
                    if (result)
                        caja("Orden no puede estar vacia", "", true);

                    return;
                }
                orden = textBox1.Text.Trim();
            }
            try
            {
                // btnScanEPC.Enabled = false;
                bool resultado = false;
                btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = false));
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/linOrdenesProdArco?$filter=prodOrderNo eq '" + orden + "'";
                cantidadReal = 0;
                var request = (HttpWebRequest)WebRequest.Create(url);
                string username = usuario;
                string password = this.password;
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + svcCredentials);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = timeOut;
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            if (strReader == null)
                                return;
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                string responseBodyFormater = responseBody.Replace("@odata.context", "dataContext");
                                responseBodyFormater = responseBodyFormater.Replace("@odata.etag", "odataETag");
                                getMozo = JsonConvert.DeserializeObject<GetMozo>(responseBodyFormater);
                                if (getMozo.Value.Length > 0)
                                {
                                    //lReferencia.Text = getMozo.Value[0].ItemNo;
                                    epcs.Invoke(new Action(() => lReferencia.Text = getMozo.Value[0].ItemNo));
                                    //producto = getMozo.Value[0].ItemNo;
                                    epcs.Invoke(new Action(() => producto = getMozo.Value[0].ItemNo));
                                    //cantidadReal = getMozo.Value[0].RemainingQtyBase;
                                    epcs.Invoke(new Action(() => cantidadReal = getMozo.Value[0].RemainingQtyBase));
                                    //lDescripcion.Text = getMozo.Value[0].Description;
                                    epcs.Invoke(new Action(() => lDescripcion.Text = getMozo.Value[0].Description));
                                    //lCantidadTotal.Text = getMozo.Value[0].RemainingQtyBase.ToString();
                                    epcs.Invoke(new Action(() => lCantidadTotal.Text = getMozo.Value[0].RemainingQtyBase.ToString()));

                                    //pReferencia.Visible = true;
                                    epcs.Invoke(new Action(() => pReferencia.Visible = true));
                                }
                                else
                                {
                                    /*if (result)
                                      caja("Orden no existe", null, true);
                                    return false;*/
                                    leerOrdenAmipem();
                                    leerTagsOrdenAmipem();
                                    return;
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                caja("Problemas de conexion ", "", true);


                //textBox1.Text = "";
                epcs.Invoke(new Action(() => textBox1.Text = ""));
                return;
                //log("leerOrden:" + ex.Message);

            }

        }

        private void leerTagsOrdenAmipem()
        {
            var url = apiAmipemBase + "leerTagsOrden";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Timeout = timeOut;
            string orden = ordenBiocam;
            if (orden == null || orden.Equals(""))
                orden = textBox1.Text;
            string salida = "{\"codigo\":\"" + orden + "\"}";
            byte[] data = Encoding.UTF8.GetBytes(salida);
            request.ContentLength = data.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {

                            Stream stream1 = response.GetResponseStream();
                            StreamReader sr = new StreamReader(stream1);
                            string responseBody = sr.ReadToEnd();
                            // string respuestaCambiada = responseBody.Replace("+02:00", ".00");
                            GrabacionRespuesta grabacionRespuesta = JsonConvert.DeserializeObject<GrabacionRespuesta>(responseBody);
                            epcList.Clear();
                            if (epcs.InvokeRequired)
                            {
                                epcs.Invoke(new Action(() => epcs.Rows.Clear()));
                            }
                            else
                            {
                                epcs.Rows.Clear();
                            }
                            cantidadReal = 0;
                            for (int i = 0; i < grabacionRespuesta.grabaciones.Length; i++)
                            {
                                if (!tagsOrden.ContainsKey(grabacionRespuesta.grabaciones[i].tag))
                                    tagsOrden.Add(grabacionRespuesta.grabaciones[i].tag, grabacionRespuesta.grabaciones[i].linea);
                                //new Thread(new ThreadStart(delegate { desglosaEpc(grabacionRespuesta.grabaciones[i].tag, grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToLocalTime().ToString(), false); })).Start();
                                desglosaEpc(grabacionRespuesta.grabaciones[i].tag, grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToLocalTime().ToString(), false, "");
                                // epcs.Refresh();
                                cantidadReal++;
                                //lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal;
                                lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal));
                                //lCantidadTotal.Refresh();
                                lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));
                            }
                            // panelCaja.Visible = false;
                            cantidadReal = cantidadTotal - grabacionRespuesta.grabaciones.Length;
                            //lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal;
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal));
                            if (cantidadReal == 0 && !biocam)
                                caja("Orden finalizada", "", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log("leerTagsOrdenAmipem:" + ex.Message);
            }
        }



        private void caja(string texto, string epc, bool boton)
        {
            try
            {
                if (boton)
                    textoCaja.Invoke(new Action(() => botonCaja.Visible = true));
                else
                    textoCaja.Invoke(new Action(() => botonCaja.Visible = false));
                if (!biocam)
                {
                    StopEPC(true);
                    uhf.StopInventory();
                }
                epcErroneo = epc;
                textoCaja.Invoke(new Action(() => textoCaja.Text = texto));
                panelCaja.Invoke(new Action(() => panelCaja.Visible = true));
                int i = 0;

            }
            catch (Exception e)
            {

                log("caja:" + e.Message);
            }
        }
        private void leerOrdenAmipem()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var url = apiAmipemBase + "leerOrden";

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = timeOut;
                //Orden orden = new Orden(idOrdenAmipem, textBox1.Text, lDescripcion.Text, lReferencia.Text, Int32.Parse(lCantidadTotal.Text));
                string numorden = "";
                if (textBox1.Text.Trim().Equals(""))
                    numorden = ordenBiocam;
                else
                    numorden = textBox1.Text.Trim();
                string salida = "{\"codigo\":\"" + numorden + "\"}";
                byte[] data = Encoding.UTF8.GetBytes(salida);
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {

                            string responseBody = objReader.ReadToEnd();
                            Orden orden = JsonConvert.DeserializeObject<Orden>(responseBody);
                            if (orden.id == 0)
                            {
                                if (result)
                                    caja("Orden no existe", "", true);
                                textBox1.Text = "";
                                return;
                            }
                            //lReferencia.Text = orden.item;
                            lReferencia.Invoke(new Action(() => lReferencia.Text = orden.item));
                            producto = orden.item;
                            //lDescripcion.Text = orden.descripcion;
                            lDescripcion.Invoke(new Action(() => lDescripcion.Text = orden.descripcion));
                            if (biocam)
                                cantidadTotal = 1;
                            else
                                cantidadTotal = orden.cantidad;
                            //lCantidadTotal.Text = "" + cantidadTotal + "/0";
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/0"));

                            pReferencia.Invoke(new Action(() => pReferencia.Visible = true));
                            idOrdenAmipem = orden.id;

                            btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = true));
                            /*lReferencia.Refresh();
                            lDescripcion.Refresh();
                            lCantidadTotal.Refresh();*/
                            leerTagsOrdenAmipem();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log("leerOrdenAmipem" + ex.Message);
                textBox1.Text = "";
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
                request.Timeout = timeOut;
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
                caja("Problemas de conexion con la base de datos", "", true);
            }

        }

        private bool grabarOrdenAmipem()
        {
            bool resultado = false;
            try
            {
                btnScanEPC.Enabled = false;
                var url = apiAmipemBase + "grabarOrden";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = timeOut;
                string ordenUrl = "";
                if (ordenBiocam != null)
                    ordenUrl = ordenBiocam;
                else
                    ordenUrl = textBox1.Text.Trim();
                Orden orden = new Orden(idOrdenAmipem, ordenUrl, lDescripcion.Text, lReferencia.Text, Int32.Parse(lCantidadTotal.Text), 0);
                string salida = JsonConvert.SerializeObject(orden);
                byte[] data = Encoding.UTF8.GetBytes(salida);
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Leer el resultado de la llamada
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    Orden ordenRetorno = JsonConvert.DeserializeObject<Orden>(strsb);
                    //btnScanEPC.Enabled = true;
                    btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = true));

                    resultado = true;
                }
            }
            catch (Exception e)
            {
                log("grabarOrdenAmipem:" + e.Message);
                resultado = false;
            }
            return resultado;
        }
        private void grabarTagContadorAmipem(string epc, int lote)
        {
            if (ubicación.Equals("casa"))
            {
                desglosaEpc(epc, DateTime.Now.ToString(), true, "");
                return;
            }
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var url = apiAmipemBase + "grabarTagContador";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = timeOut;
                DateTime fecha = DateTime.Now;
                GrabacionDTO grabacionDTO = new GrabacionDTO(textBox1.Text, epc, Int32.Parse(tbLinea.Text.ToString()));
                string salida = JsonConvert.SerializeObject(grabacionDTO);
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
                    GrabacionRespuesta grabacionRespuesta = JsonConvert.DeserializeObject<GrabacionRespuesta>(strsb);
                    if (lCantidadTotal.InvokeRequired)
                    {
                        lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabacionRespuesta.grabaciones.Length)));
                    }
                    else
                    {
                        lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabacionRespuesta.grabaciones.Length);
                    }


                    //this.BeginInvoke(grabarTagContadorMozoCallback, new object[] { epc, Int32.Parse(tbLinea.Text), true });
                    grabarTagContadorMozo(epc, 1, true);

                }

            }
            catch (Exception e1)
            {
                caja(epc + " no se ha podido grabar", epc, true);
                log("grabarTagContadorAmipem:" + e1.Message);
            }
        }
        private void grabarTagContadorMozo(string epc, int lote, bool comprobarErrores)
        {

            if (ubicación.Equals("casa"))
            {
                desglosaEpc(epc, DateTime.Now.ToString(), true, "");
                return;
            }

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/registrarSalidasRFID?";
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
                GrabacionMozo grabacion = new GrabacionMozo(epc, textBox1.Text, 10000, 1, comprobarErrores, "");
                string salida = JsonConvert.SerializeObject(grabacion);
                byte[] data = Encoding.UTF8.GetBytes(salida);
                request.ContentLength = data.Length;
                request.Timeout = timeOut;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    GrabacionMozo ordenRetorno = JsonConvert.DeserializeObject<GrabacionMozo>(strsb);
                    int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);

                    if (ordenRetorno.textoError != null && ordenRetorno.textoError.Length > 0 && comprobarErrores)
                    {
                        if (result & !biocam)
                            caja(ordenRetorno.textoError + "\n" + desglosaEpcErroneo(epc), epc, true);
                        return;
                    }
                    if (ordenRetorno.textoError != null && ordenRetorno.textoError.Length == 0)
                    {

                        grabarTagContadorMozo(epc, lote, false);
                        //this.BeginInvoke(grabarTagContadorMozoCallback, new object[] { epc, Int32.Parse(tbLinea.Text), true });
                        desglosaEpc(epc, DateTime.Now.ToString(), true, ordenRetorno.textoError);

                        return;


                    }

                }
            }
            catch (System.Net.WebException e)
            {


                borraUltimaGrabacion(epc);
                log("grabarTagContadorMozo:" + e.Message);

            }

            return;
        }

        private void borraUltimaGrabacion(string epc)
        {

            try
            {
                var url = apiAmipemBase + "borraUltimaGrabacion";

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = timeOut;
                Tag tag = new Tag(epc);
                string salida = JsonConvert.SerializeObject(tag);
                byte[] data = Encoding.UTF8.GetBytes(salida);
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                using (WebResponse response = request.GetResponse())
                {

                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {

                            //epcs.Rows.RemoveAt(epcs.Rows.Count - 1);
                            tagsOrden.Remove(epc);
                            tagsTotales.Remove(epc);
                            //cantidadReal++;
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal));
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log("borraUltimarabacion:" + ex.Message);
                caja("Problemas de conexion con la base de datos", "", true);
                textBox1.Text = "";
            }
            StopEPC(true);

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
            while (isRuning)
            {
                try
                {
                    UHFTAGInfo info = uhf.ReadTagFromBuffer();
                    if (tcp && Int32.Parse(info.Ant) == antena || bt)
                    {
                        if (info != null)
                        {
                            // Correctly invoke the delegate using Invoke or BeginInvoke
                            this.BeginInvoke(setTextCallback, new object[] { info.Epc, info.Tid, info.Rssi, "1", info.Ant, info.User });

                            //this.BeginInvoke(setTextCallback, new object[] { "010843548261926610BC25011142240000000002100111121082025170000000", info.Tid, info.Rssi, "1", info.Ant, info.User });
                        }
                    }
                }
                catch (Exception ex)
                {
                    log("readEpc:" + ex.Message);
                }
            }
        }

        private void UpdataEPC(string epc, string tid, string rssi, string couint, string ant, string user)
        {

            if (string.IsNullOrEmpty(epc))
            {
                return;
            }
            if (!tagsTotales.ContainsKey(epc))
            {

                tagsTotales.Add(epc, 0);
                if (epc.StartsWith("0108"))
                {

                    if (!consulta)
                    {

                        if (tagsOrden.Count <= cantidadTotal + 1)
                        {
                            if (!tagsOrden.ContainsKey(epc))
                            {
                                // new Thread(new ThreadStart(delegate { validaProducto(info.Epc); })).Start();
                                if (biocam)
                                {
                                    try
                                    {
                                        //hiloLectura = new Thread(new ThreadStart(delegate { recuperaInfoBiocam(info.Epc); }));
                                        recuperaInfoBiocam(epc);

                                    }
                                    catch (Exception e)
                                    {
                                        log("StartReceiveThread:" + e.Message);
                                    }

                                }
                                else
                                {

                                    grabarTagContadorAmipem(epc, Int32.Parse(tbLinea.Text));
                                    //this.BeginInvoke(grabarTagContadorAmipemCallback, new object[] { epc, Int32.Parse(tbLinea.Text) });

                                }


                            }
                        }

                    }
                    else
                    {
                        if (!tagsComprobacion.ContainsKey(epc))
                        {
                            tagsComprobacion.Add(epc, 1);
                            //new Thread(new ThreadStart(delegate { desglosaEpc(info.Epc); })).Start();
                            try
                            {
                                desglosaEpc(epc);

                            }
                            catch (Exception e)
                            {
                                log("StartReceiveThread:" + e.Message);
                            }


                        }
                    }
                }
                else
                    caja("PRODUCTO ERRONEO", "", true);
            }
        }
        private void recuperaInfoBiocam(string epc)
        {
            string lote = desglosaEpc(epc);
            string orden = dameOrdenFromLote(lote);
            ordenBiocam = orden;
            if (orden.StartsWith("OFBC"))
            {
                leerOrden();
                grabarOrdenAmipem();
                leerOrdenAmipem();
                leerTagsOrdenAmipem();
            }
            else
                caja("Producto no es biocam", "", true);

            //textBox1.Text = orden;
            epcs.Invoke(new Action(() => textBox1.Text = orden));
            /*try
            {
                hiloLectura = new Thread(new ThreadStart(delegate { validaProducto(epc); }));
                hiloLectura.IsBackground = true;
                hiloLectura.Start();
            }
            catch (Exception e)
            {
                log("StartReceiveThread:" + e.Message);
            }*/




            // leerOrdenAmipem();
            int lotes = 1;
            if (!tbLotes.Text.Trim().Equals(""))
                lotes = Int32.Parse(tbLotes.Text);
            /** grabarTagContadorAmipem(epc,lotes );
             if (ubicación.Equals("mozo"))
                 grabarTagContadorMozo(epc, lotes, true);*/



        }
        private string desglosaEpc(string epc)
        {


            string salidaLote = "";
            if (ubicación.Equals("mozo1"))
            {

                if (epcs.InvokeRequired)
                {

                    dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" })));

                }
                else
                {
                    dataGridView1.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" });

                }
                cantidadComprobacion.Invoke(new Action(() => cantidadComprobacion.Text = "Cantidad: " + tagsComprobacion.Count));
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
                    request.Timeout = timeOut;
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
                        salidaLote = desglose.noLote;
                        if (!biocam)
                        {
                            if (dataGridView1.InvokeRequired)
                            {
                                dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie })));
                            }
                            else
                            {
                                dataGridView1.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie });
                            }
                            cantidadComprobacion.Invoke(new Action(() => cantidadComprobacion.Text = "Cantidad: " + tagsComprobacion.Count + " totales: " + tagsTotales.Count));
                        }

                    }
                }
                catch (Exception e)
                {

                    log("desglosaEPC:" + e.Message);

                    //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
                }
            }
            return salidaLote;
        }

        private void desglosaEpc(string epc, string fecha, bool grabar, string error)
        {
            try
            {
                int ultimograbado = 0;
                if (ubicación.Equals("mozo1"))
                {

                    if (epcs.InvokeRequired)
                    {

                        epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" })));
                        ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                    }
                    else
                    {
                        epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" });
                        ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                    }
                    // epcs.Rows[ultimograbado - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                }
                else
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
                    request.Timeout = timeOut;
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
                            epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, Int32.Parse(tbLinea.Text), fecha })));
                        }
                        else
                        {
                            epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, Int32.Parse(tbLinea.Text), fecha });
                        }

                        /*if (biocam)
                        {
                            cargarDatos(desglose.noLote);
                        }*/
                        if (tagsOrden.Count >= cantidadTotal)
                        {

                            int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                            if (!finOrden)
                            {
                                finOrden = true;

                                if (result && !biocam)
                                    caja("Orden finalizada", "", true);

                            }
                            StopEPC(true);
                            btnScanEPC.Text = strStart;
                            btnScanEPC.BackColor = System.Drawing.Color.Green;
                            btnScanEPC.ForeColor = System.Drawing.Color.White;
                            epcList.Clear();

                            btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = false));
                            btnScanEPC.Invoke(new Action(() => btnScanEPC.Text = strStart));

                        }

                    }
                    if (biocam)
                    {

                        //recuperar orden para consultar pieza biocam
                        var urlPedido = apiMozoBase + "api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/codProcedenciaOrdenesProdArco?$filter=no eq '" + textBox1.Text.Trim() + "'";
                        var requestPedido = (HttpWebRequest)WebRequest.Create(urlPedido);

                        string svcCredentialsPedido = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                        requestPedido.Headers.Add("Authorization", "Basic " + svcCredentialsPedido);
                        requestPedido.Method = "GET";
                        requestPedido.ContentType = "application/json";
                        requestPedido.Accept = "*/*";


                        using (WebResponse response = requestPedido.GetResponse())
                        {
                            using (Stream strReader = response.GetResponseStream())
                            {
                                if (strReader == null)
                                    return;
                                using (StreamReader objReader = new StreamReader(strReader))
                                {
                                    string responseBody = objReader.ReadToEnd();
                                    string responseBodyFormater = responseBody.Replace("@odata.context", "dataContext");
                                    responseBodyFormater = responseBodyFormater.Replace("@odata.etag", "odataETag");
                                    OrigenOrden origenOrden = JsonConvert.DeserializeObject<OrigenOrden>(responseBodyFormater);
                                    response.Close();
                                    if (origenOrden != null)
                                    {
                                        if (origenOrden.value.Length == 0)
                                        {
                                            caja("BIOCAM YA PROCESADO", "", true);
                                            return;
                                        }

                                        leerBiocam(origenOrden.value[0].sourceNo, error);

                                    }
                                    else
                                    {
                                        if (result)
                                            caja("Orden no existe", null, true);
                                        return;
                                    }

                                }
                            }
                        }




                    }

                }
            }
            catch (Exception e)
            {
                log("desglosaEpc:" + e.Message);
                //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
            }

        }



        private void leerBiocam(String pedido, string error)
        {

            var url1 = apiMozoBase + "api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/pedidosVentaArco?$expand=linPedidosVentaArco($expand=vinculosEnsamblarPedidoArco($expand=linPedidosEnsambladoArco))&$filter=no eq '" + pedido + "'";
            var request1 = (HttpWebRequest)WebRequest.Create(url1);

            string svcCredentials1 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(usuario + ":" + password));
            request1.Headers.Add("Authorization", "Basic " + svcCredentials1);
            request1.Method = "GET";
            request1.ContentType = "application/json";
            request1.Accept = "*/*";

            try
            {
                using (WebResponse responseBiocam = request1.GetResponse())
                {
                    using (Stream strReaderBiocam = responseBiocam.GetResponseStream())
                    {
                        if (strReaderBiocam == null)
                            return;
                        using (StreamReader objReaderBiocam = new StreamReader(strReaderBiocam))
                        {
                            string responseBodyBiocam = objReaderBiocam.ReadToEnd();
                            string responseBodyFormaterBiocam = responseBodyBiocam.Replace("@odata.context", "dataContext");
                            responseBodyFormaterBiocam = responseBodyFormaterBiocam.Replace("@odata.etag", "odataETag");

                            Biocam biocam1 = JsonConvert.DeserializeObject<Biocam>(responseBodyFormaterBiocam);
                            if (biocam1 != null)
                            {
                                LinPedidosVentaArco[] linPedidosVentaArcos = biocam1.value[0].linPedidosVentaArco;
                                StringBuilder resultadoBiocam = new StringBuilder();
                                resultadoBiocam.Append(error + "\n\r");
                                bool contornillos = false;

                                for (int i = 0; i < linPedidosVentaArcos.Length; i++)
                                {

                                    if (linPedidosVentaArcos[i].description.StartsWith("Tornillo"))
                                    {
                                        contornillos = true;
                                        resultadoBiocam.Append(linPedidosVentaArcos[i].no + " - " + linPedidosVentaArcos[i].description + " (" + linPedidosVentaArcos[i].outstandingQuantity + ")\r\n");
                                    }
                                }
                                if (!contornillos)
                                    caja("No hay tornillos en la orden", "", true);
                                else
                                    caja(resultadoBiocam.ToString(), "", true);
                            }
                            else
                            {
                                if (result)
                                    caja("Orden no existe", null, true);
                                return;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log("desglosaEPC:" + ex.Message);
                caja("BIOCAM YA PROCESADO", "", true);

                //textBox1.Text = "";
            }
        }
        private string desglosaEpcErroneo(string epc)
        {
            //tagsOrden.Add(epc, Int32.Parse(tbLinea.Text));


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
                    request.Timeout = timeOut;
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
            if (textBox1.Text.Trim().Equals("") && ordenBiocam == null)
            {

                caja("Orden no puede estar vacia", "", true);

                return;
            }

            consulta = false;
            biocam = false;
            lecturaDirecta = false;

            if (btnScanEPC.Text == strStop)
            {
                StopReceiveThread();
                StopEPC(true);
            }
            else
            {
                StartReceiveThread();
                StopEPC(false);

                // Ensure the delegate is instantiated correctly  

                setTextCallback = new SetTextCallback(UpdataEPC);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ordenBiocam = null;
            if (!result)
                toolStripButton1_Click();
            biocam = false;
            if (textBox1.Text.ToUpper().StartsWith("OFBC"))
                biocam = true;
            finOrden = false;
            epcs.Rows.Clear();
            tagsOrden.Clear();
            tbLotes.Text = "1";
            gtin = "";
            epcList.Clear();
            epcs.Refresh();
            loteActual = 1;
            tbLinea.Enabled = true;
            tbLotes.Enabled = true;
            consulta = false;
            if (ubicación.Equals("mozo1"))
            {
                leerOrdenAmipem();
            }
            else
            {
                try
                {
                    leerOrden();
                    grabarOrdenAmipem();
                    leerOrdenAmipem();

                    if (tagsOrden.Count == cantidadTotal)
                        btnScanEPC.Enabled = false;
                    else
                        btnScanEPC.Enabled = true;
                }
                catch (Exception e1)
                {
                    log("button2_click:" + e1.Message);
                    //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
                }
            }
            StopEPC(true);
            btnScanEPC.Text = strStart;
            btnScanEPC.BackColor = System.Drawing.Color.Green;
            btnScanEPC.ForeColor = System.Drawing.Color.White;
        }


        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Screen primaryScreen = Screen.PrimaryScreen;
            System.Drawing.Rectangle bounds = primaryScreen.Bounds;
            this.Size = new System.Drawing.Size(bounds.Width, bounds.Height);
            panel9.Size = new System.Drawing.Size(bounds.Width, bounds.Height);
            dataGridView1.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 100);
            epcs.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 100);
            epcs.Refresh();
        }
        private void buttonConsulta_Click(object sender, EventArgs e)
        {
            uhf = UHFAPI.getInstance();
            if (!result)
                toolStripButton1_Click();
            tagsTotales.Clear();
            try
            {
                lecturaDirecta = false;
                biocam = false;
                // dataGridView1.Rows.Clear();
                // tagsComprobacion.Clear();
                consulta = true;
                if (buttonConsulta.Text == strStopConsulta)
                {
                    StopReceiveThread();
                    StopEPC(true);
                }
                else
                {
                    cantidadComprobacion.Text = "Cantidad: 0";
                    dataGridView1.Rows.Clear();
                    tagsComprobacion.Clear();
                    StartReceiveThread();
                    StopEPC(false);
                    setTextCallback = new SetTextCallback(UpdataEPC);
                }
            }
            catch (Exception ex)
            {
                log("buttonConsulta_Click:" + ex.Message);
                //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");

            }
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void epcs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        public Form ShowForm(Form nextForm, bool isCache)
        {
            isCache = false;

            //toolStripStatusLabel1.Visible = false;
            Form currForm = this.ActiveMdiChild;
            Form from = nextForm;
            if (currForm != null)
            {
                if (currForm.Name == from.Name)
                {
                    return null;
                }

                if (currForm.Name != "ReadEPCForm")// (currForm.Name == "ReadEPCForm" || currForm.Name == "ConfigForm")
                {
                    //Common.SaveForm(currForm);
                    currForm.Close();
                }
                else
                {
                    currForm.Hide();
                    // from = Common.GetForm(nextForm.GetType().Namespace, nextForm.Name, this);
                }
            }

            from.WindowState = FormWindowState.Maximized;
            from.MdiParent = this;//设置当前窗体为子窗体的父窗体
            from.AutoScaleMode = AutoScaleMode.Inherit;
            if (from.Name != "ReadEPCForm")// (currForm.Name == "ReadEPCForm" || currForm.Name == "ConfigForm")
            {
                from.Left = 303;
            }
            else
            {
                if (from.Left != -8)
                {
                    from.Left = 303;
                }
            }

            from.Show();//显示窗体
            return from;
        }




        private void buttonBajas_Click_1(object sender, EventArgs e)
        {
            uhf.CloseUsb();
            this.Hide();
            Form form = new MainFormBajas();
            form.Show();
        }

        private void buttonInversa_Click(object sender, EventArgs e)
        {
            uhf.CloseUsb();
            this.Hide();
            Form form = new MainFormOrdenInversa();
            form.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //aqui lectura biocam
            tagsTotales.Clear();
            try
            {
                epcs.Rows.Clear();
                tagsOrden.Clear();
                if (!result)
                    toolStripButton1_Click();
                lecturaDirecta = true;
                biocam = true;
                consulta = false;
                if (buttonBiocam.Text == biocamStop)
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
            catch (System.ArgumentException e1)
            {
                log("button3_Click_1:" + e1.Message);
            }



        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            try
            {
                // Crea un StreamWriter para escribir en el archivo
                using (StreamWriter writer = new StreamWriter(textBox1.Text.Trim() + ".txt"))
                {
                    writer.WriteLine("producto - lote - numero de serie - linea - fecha");
                    for (int i = 0; i < epcs.Rows.Count; i++)
                    {
                        string contenido = "";
                        for (int j = 0; j < epcs.Columns.Count; j++)
                        {
                            //if (j > 0)
                            if (epcs.Rows[i].Cells[j].Value != null)
                            {

                                contenido += epcs.Rows[i].Cells[j].Value.ToString();
                                if (j < epcs.Columns.Count - 1)
                                    contenido += " - ";
                            }

                        }
                        writer.WriteLine(contenido);
                    }

                }

                caja("Archivo guardado exitosamente en: " + textBox1.Text.Trim() + ".txt", "", true);
            }
            catch (Exception e1)
            {
                log("Error al guardar el archivo: " + e1.Message);
            }
        }

        private void buttonExcel_Click(object sender, EventArgs e)
        {


            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            string[,] datos = new string[epcs.Rows.Count + 2, epcs.Columns.Count]; // +1 por la cabecera
            for (int j = 0; j < epcs.Columns.Count; j++) //cabeceras
            {
                datos[1, j] = epcs.Columns[j].HeaderText;
            }
            datos[0, 0] = "ORDEN:";
            datos[0, 1] = textBox1.Text.Trim();
            datos[0, 2] = " ";
            datos[0, 3] = " ";
            datos[0, 4] = " ";

            for (int i = 0; i < epcs.Rows.Count; i++)
            {
                for (int j = 0; j < epcs.Columns.Count; j++)
                {
                    if (epcs.Rows[i].Cells[j].Value != null)
                        datos[i + 2, j] = epcs.Rows[i].Cells[j].Value.ToString() + " ";
                }

            }

            excel.Range[excel.Cells[1, 1], excel.Cells[datos.GetLength(0), datos.GetLength(1)]].Value = datos;
            excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;
            worksheet.Activate();
            worksheet.SaveAs(".\\excel\\" + textBox1.Text.Trim() + ".xlsx");
            caja("Excel grabado correctamente", "", true);

        }

        private void MainForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            MainForm_FormClosing(sender, e);
        }

        private void button3_Click_3(object sender, EventArgs e)
        {


            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            string[,] datos = new string[dataGridView1.Rows.Count + 2, dataGridView1.Columns.Count]; // +1 por la cabecera
            for (int j = 0; j < dataGridView1.Columns.Count; j++) //cabeceras
            {
                datos[1, j] = dataGridView1.Columns[j].HeaderText;
            }
            datos[0, 0] = "LISTADO DE COMPROBACION ORDEN:";
            datos[0, 1] = textBox1.Text.Trim();
            datos[0, 2] = " ";


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        datos[i + 2, j] = dataGridView1.Rows[i].Cells[j].Value.ToString() + " ";
                }

            }

            excel.Range[excel.Cells[1, 1], excel.Cells[datos.GetLength(0), datos.GetLength(1)]].Value = datos;
            excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;
            worksheet.Activate();
            // worksheet.SaveAs(".\\excel\\" + textBox1.Text.Trim() + ".xlsx");
            // caja("Excel grabado correctamente", "", true);
        }
    }
};