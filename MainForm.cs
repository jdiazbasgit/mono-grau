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
using Microsoft.Graph.Models.TermStore;
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
        HashSet<string> tagsOrden = new HashSet<string>();
        //HashSet<string> tagsTotales = new HashSet<string>();
        //List<string> tagsTotales = new List<string>();
        HashSet<string> tagsComprobacion = new HashSet<string>();
        public static FormWindowState currState = FormWindowState.Normal;
        List<EpcInfo> epcList = new List<EpcInfo>();
        public bool isSearch = false;
        UHFAPP.UHFAPI.OnDisconnectCallback DisconnectCallback = null;
        private string epcErroneo;
        bool result = false;
        private string ordenBiocam;
        private int antena;
        delegate void SetTextCallback(string epc);
        SetTextCallback setTextCallback;

        private int timeOut = 100;
        private int lecturas;
        private bool reintento;
        private StringBuilder resultadoBiocam;
        private bool cargaOrden;
        private int pantalla;
        private bool forzarGrabadoAmipem = false;
        private int contadorComprobacion;
        private bool ordenInversa;
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
            button3.Location = new System.Drawing.Point(bounds.Width + 50, bounds.Height - 120);


            panelCaja.Location = new System.Drawing.Point(0, 321);
            panelCaja.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 200);
            textoCaja.Location = new System.Drawing.Point(0, 0);
            textoCaja.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 300);
            botonCaja.Location = new System.Drawing.Point(bounds.Width / 4 - 75, (bounds.Height * 2 / 3) - 250);
            botonError.Location = new System.Drawing.Point((bounds.Width / 4) + 3 - 125, (bounds.Height * 2 / 3) - 250);
            botonForzado.Location = new System.Drawing.Point((bounds.Width / 4) + 3, (bounds.Height * 2 / 3) - 250);

            panelError.Location = new System.Drawing.Point(bounds.Width / 2, 321);
            panelError.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 200);
            textoError.Location = new System.Drawing.Point(0, 0);
            textoError.Size = new System.Drawing.Size(bounds.Width / 2, (bounds.Height * 2 / 3) - 300);


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
        /* private void btnGetANT_Click(object sender, EventArgs e)
         {

             string msg = !IsChineseSimple() ? "Failure!" : "失败!";
             byte[] ant = new byte[4];
             if (uhf.GetANTTo32(ant))
             {
                 cmbAnt16.Checked = (ant[0] & 128) == 128 ? true : false;
                 cmbAnt15.Checked = (ant[0] & 64) == 64 ? true : false;
                 cmbAnt14.Checked = (ant[0] & 32) == 32 ? true : false;
                 cmbAnt13.Checked = (ant[0] & 16) == 16 ? true : false;
                 cmbAnt12.Checked = (ant[0] & 8) == 8 ? true : false;
                 cmbAnt11.Checked = (ant[0] & 4) == 4 ? true : false;
                 cmbAnt10.Checked = (ant[0] & 2) == 2 ? true : false;
                 cmbAnt9.Checked = (ant[0] & 1) == 1 ? true : false;

                 cmbAnt8.Checked = (ant[1] & 128) == 128 ? true : false;
                 cmbAnt7.Checked = (ant[1] & 64) == 64 ? true : false;
                 cmbAnt6.Checked = (ant[1] & 32) == 32 ? true : false;
                 cmbAnt5.Checked = (ant[1] & 16) == 16 ? true : false;
                 cmbAnt4.Checked = (ant[1] & 8) == 8 ? true : false;
                 cmbAnt3.Checked = (ant[1] & 4) == 4 ? true : false;
                 cmbAnt2.Checked = (ant[1] & 2) == 2 ? true : false;
                 cmbAnt1.Checked = (ant[1] & 1) == 1 ? true : false;

                 if (ant.Length == 4)
                 {
                     cbANT32.Checked = (ant[2] & 128) == 128 ? true : false;
                     cbANT31.Checked = (ant[2] & 64) == 64 ? true : false;
                     cbANT30.Checked = (ant[2] & 32) == 32 ? true : false;
                     cbANT29.Checked = (ant[2] & 16) == 16 ? true : false;
                     cbANT28.Checked = (ant[2] & 8) == 8 ? true : false;
                     cbANT27.Checked = (ant[2] & 4) == 4 ? true : false;
                     cbANT26.Checked = (ant[2] & 2) == 2 ? true : false;
                     cbANT25.Checked = (ant[2] & 1) == 1 ? true : false;

                     cbANT24.Checked = (ant[3] & 128) == 128 ? true : false;
                     cbANT23.Checked = (ant[3] & 64) == 64 ? true : false;
                     cbANT22.Checked = (ant[3] & 32) == 32 ? true : false;
                     cbANT21.Checked = (ant[3] & 16) == 16 ? true : false;
                     cbANT20.Checked = (ant[3] & 8) == 8 ? true : false;
                     cbANT19.Checked = (ant[3] & 4) == 4 ? true : false;
                     cbANT18.Checked = (ant[3] & 2) == 2 ? true : false;
                     cbANT17.Checked = (ant[3] & 1) == 1 ? true : false;
                 }



                 msg = !IsChineseSimple() ? "Success" : "成功!";
                 //  msg = Common.isEnglish?"success":"获取天线成功!("+ DataConvert.ByteArrayToHexString(ant)+")";
             }


         }*/

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
                    uhf.SetGen2(0, 0, 0, 1, 4, 0, 15, 1, 2, 1, 1, 1, 0, 3);
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
            //request.Timeout = timeOut;
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
                    if (result && !ordenInversa)
                        caja("Orden no puede estar vacia", "", true);

                    return;
                }
                orden = textBox1.Text.Trim();
            }
            try
            {
                // btnScanEPC.Enabled = false;
                bool resultado = false;

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
                //request.Timeout = timeOut;
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
                                    epcs.Invoke(new Action(() => lReferencia.Refresh()));
                                    //producto = getMozo.Value[0].ItemNo;
                                    epcs.Invoke(new Action(() => producto = getMozo.Value[0].ItemNo));

                                    //cantidadReal = getMozo.Value[0].RemainingQtyBase;
                                    //epcs.Invoke(new Action(() => cantidadReal = getMozo.Value[0].RemainingQtyBase));
                                    //lDescripcion.Text = getMozo.Value[0].Description;
                                    epcs.Invoke(new Action(() => lDescripcion.Text = getMozo.Value[0].Description));
                                    epcs.Invoke(new Action(() => lDescripcion.Refresh()));
                                    //lCantidadTotal.Text = getMozo.Value[0].RemainingQtyBase.ToString();
                                    epcs.Invoke(new Action(() => lCantidadTotal.Text = getMozo.Value[0].RemainingQtyBase.ToString()));
                                    epcs.Invoke(new Action(() => lCantidadTotal.Refresh()));
                                    if (getMozo.Value[0].RemainingQtyBase > 0)
                                        btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = true));
                                    //pReferencia.Visible = true;
                                    epcs.Invoke(new Action(() => pReferencia.Visible = true));
                                    epcs.Invoke(new Action(() => pReferencia.Refresh()));
                                }
                                else
                                {
                                    /*if (result)
                                      caja("Orden no existe", null, true);
                                    return false;*/
                                    //leerOrdenAmipem();
                                    //leerTagsOrdenAmipem();
                                    return;
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                cajaError("Problemas de conexion ", "");


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
            //request.Timeout = timeOut;
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
                               // if (!tagsOrden.Contains(grabacionRespuesta.grabaciones[i].tag))
                                    uhf.leidos.Add(grabacionRespuesta.grabaciones[i].tag);
                                //new Thread(new ThreadStart(delegate { desglosaEpc(grabacionRespuesta.grabaciones[i].tag, grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToLocalTime().ToString(), false); })).Start();
                                // desglosaEpc(grabacionRespuesta.grabaciones[i].tag, grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToLocalTime().ToString(), false, "", false);
                                // epcs.Refresh();
                                cantidadReal++;
                                //lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal;
                                lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal + " (" + (cantidadTotal - cantidadReal) + ")"));
                                //lCantidadTotal.Refresh();
                                //lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));
                                try
                                {
                                    grabarTagContadorMozo(grabacionRespuesta.grabaciones[i].tag, 1, false);
                                }
                                catch (Exception e)
                                {
                                    log("error grabación mozo: " + e.Message);
                                }
                                desglosaEpc(grabacionRespuesta.grabaciones[i].tag, DateTime.Now.ToString(), true, "", true);

                                // grabarTagContadorMozo(grabacionRespuesta.grabaciones[i].tag, 1, true);
                            }
                            cargaOrden = false;
                            // panelCaja.Visible = false;
                            int calculo = cantidadTotal - grabacionRespuesta.grabaciones.Length;
                            //lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal;
                            //lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal));
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));
                            if (calculo == 0 && !biocam)
                            {
                                String navision = getPendientesNavision();
                                Thread.Sleep(300);
                                caja("Orden finalizada " + "\n" + "Pendientes en Navision:" + navision, "", true);
                            }
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
                textoCaja.Invoke(new Action(() => textoCaja.Visible = false));
                panelCaja.Invoke(new Action(() => panelCaja.Visible = true));
                panelCaja.Invoke(new Action(() => panelCaja.Refresh()));
                textoCaja.Invoke(new Action(() => textoCaja.Refresh()));
                int i = 0;
            }
            catch (Exception e)
            {
                log("caja:" + e.Message);
            }
        }

        private void cajaError(string texto, string epc)
        {
            try
            {
                botonError.Invoke(new Action(() => botonError.Visible = true));
                botonForzado.Invoke(new Action(() => botonForzado.Visible = true));

                textoError.Invoke(new Action(() => textoError.Visible = true));
                panelError.Invoke(new Action(() => panelError.Visible = true));
                if (!biocam)
                {
                    StopEPC(true);
                    uhf.StopInventory();
                }
                epcErroneo = epc;
                textoError.Invoke(new Action(() => textoError.Text = texto));
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
                request.Accept = "application/json";
                //request.Timeout = timeOut;
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
            string rutaArchivo = "log.txt"; // Ruta donde se guardará el archivo


            try
            {
                using (StreamWriter sw = new StreamWriter(rutaArchivo, true))
                {
                    texto = DateTime.Now + " - " + texto;
                    sw.WriteLine(texto);
                    //Console.WriteLine("Texto añadido correctamente.");
                }
                // Console.WriteLine("Archivo guardado exitosamente.");
            }
            catch (Exception e)
            {
                // Console.WriteLine("Error al guardar el archivo: " + e.Message);
            }
        }

        private bool grabarOrdenAmipem()
        {
            bool resultado = false;
            try
            {
                btnScanEPC.Enabled = true;
                var url = apiAmipemBase + "grabarOrden";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                //request.Timeout = timeOut;
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
                //desglosaEpc(epc, DateTime.Now.ToString(), true, "", true);
                return;
            }
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var url = apiAmipemBase + "grabarTagContador";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                //request.Timeout = timeOut;
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
                    log("grabado amipem: " + epc);
                    /* if (lCantidadTotal.InvokeRequired)
                     {
                         lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabacionRespuesta.grabaciones.Length) + " (" + (cantidadTotal - cantidadReal) + ")"));
                     }
                     else
                     {
                         lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabacionRespuesta.grabaciones.Length + " (" + (cantidadTotal - cantidadReal) + ")");
                     }*/
                    //grabarTagContadorMozo(epc, 1, true);
                }
            }
            catch (Exception e1)
            {
                //caja(epc + " no se ha podido grabar", epc, true);
                //grabarTagContadorAmipem(epc, 1);
                log("grabar Amipem:" + e1.Message);
                //grabarTagContadorAmipem(epc, 1);
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

                            cajaError(ordenInversaIn.textoError + "\n" + desglosaEpcErroneo(epc), epc);
                            panelCaja.Invoke(new Action(() => panelCaja.Refresh()));
                            textoCaja.Invoke(new Action(() => textoCaja.Refresh()));
                            resultado = false;
                            return resultado;
                        }
                        if (ordenInversaIn.textoError != null && ordenInversaIn.textoError.Length == 0)
                        {
                            if (comprobarErrores)
                            {
                                grabaOrdenInversa(epc, false);
                                desglosaEpc(epc, DateTime.Now.ToString(), true, ordenInversaIn.textoError, true);
                                log(DateTime.Now + " - Orden Inversa correcta: " + epc);


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
        private void grabarTagContadorMozo(string epc, int lote, bool comprobarErrores)
        {

            if (ubicación.Equals("casa"))
            {
                //desglosaEpc(epc, DateTime.Now.ToString(), true, "", true);
                return;
            }

            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/registrarSalidasRFID?";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string username = this.usuario;
            string password = this.password;
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "*";
            request.Timeout = timeOut;
            GrabacionMozo grabacion = null;
            try
            {
                grabacion = new GrabacionMozo(epc, textBox1.Text, 10000, 1, comprobarErrores, "");
                string salida = JsonConvert.SerializeObject(grabacion);
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
                    GrabacionMozo ordenRetorno = JsonConvert.DeserializeObject<GrabacionMozo>(strsb);
                    int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);


                    if (ordenRetorno.textoError != null && ordenRetorno.textoError.Length > 0 && comprobarErrores && !biocam && !cargaOrden)
                    {

                        cajaError(ordenRetorno.textoError + " " + desglosaEpcErroneo(epc), epc);
                        return;

                    }
                    else if (ordenRetorno.textoError != null && ordenRetorno.textoError.Length == 0)
                    {

                        grabarTagContadorMozo(epc, lote, false);
                        cantidadReal++;
                        desglosaEpc(epc, DateTime.Now.ToString(), true, ordenRetorno.textoError, true);
                        log("grabado Navision: " + epc);
                        grabarTagContadorAmipem(epc, Int32.Parse(tbLinea.Text));

                        return;
                    }
                }

            }
            catch (Exception e)
            {

                //grabarTagContadorMozo(epc, lote, true);
                //borraUltimaGrabacion(epc);
                log("grabarTagContadorMozo:" + e.Message);
                //cajaError(e.Message, epc);
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
                //request.Timeout = timeOut;
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
                            /*tagsOrden.Remove(epc);
                            tagsTotales.Remove(epc);
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal));
                            lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));*/
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
            finally
            {
                tagsOrden.Remove(epc);
                //tagsTotales.Remove(epc);
                cantidadReal--;
            }
            StopEPC(true);

        }

        private void ScanEPCForm_Load(object sender, EventArgs e)
        {
            setTextCallback = new SetTextCallback(UpdataEPC);
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
                            // this.BeginInvoke(setTextCallback, new object[] { info.Epc });
                            UpdataEPC(info.Epc);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log("readEpc:" + ex.Message);
                }
            }
        }
        private void UpdataEPC(string epc)
        {


            if (string.IsNullOrEmpty(epc))
            {
                return;
            }
            if (true)
            //if (true)
            {

                //tagsTotales.Add(epc, 0);
                //tagsTotales.Add(epc);
                //contadorComprobacion++;

                if (epc.StartsWith("0108"))
                {
                    if (ordenInversa)
                    {
                        grabaOrdenInversa(epc, true);
                    }

                    if (!consulta)
                    {

                        //if (tagsOrden.Count <= cantidadTotal + 1)
                        //{
                        if (!tagsOrden.Contains(epc))
                        //if(true)
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

                                //grabarTagContadorAmipem(epc, Int32.Parse(tbLinea.Text));
                                grabarTagContadorMozo(epc, Int32.Parse(tbLinea.Text), true);
                                //this.BeginInvoke(grabarTagContadorAmipemCallback, new object[] { epc, Int32.Parse(tbLinea.Text) });

                            }
                        }
                    }
                    else
                    {
                        if (!tagsComprobacion.Contains(epc))
                        {
                            tagsComprobacion.Add(epc);
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
                    cajaError("PRODUCTO ERRONEO", "");
            }
        }
        private void recuperaInfoBiocam(string epc)
        {
            string lote = desglosaEpc(epc);
            string orden = dameOrdenFromLote(lote);
            ordenBiocam = orden;
            if (orden.StartsWith("OFBC"))
            {
                epcs.Invoke(new Action(() => textBox1.Text = orden));
                leerOrden();
                grabarOrdenAmipem();
                grabarTagContadorMozo(epc, 1, true);
                leerOrdenAmipem();
                //leerTagsOrdenAmipem();
            }
            else
                caja("Producto no es biocam", "", true);
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
                // cantidadComprobacion.Invoke(new Action(() => cantidadComprobacion.Text = "Cantidad: " + contadorComprobacion));
                cantidadComprobacion.Refresh();
                dataGridView1.Refresh();

                //Thread.Sleep(1000);
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
                    //request.Timeout = timeOut;
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

                            cantidadComprobacion.Invoke(new Action(() => cantidadComprobacion.Text = "Cantidad: " + tagsComprobacion.Count));
                            //cantidadComprobacion.Invoke(new Action(() => cantidadComprobacion.Text = "Cantidad: " + tagsTotales.Count));

                            cantidadComprobacion.Invoke(new Action(() => cantidadComprobacion.Refresh()));
                            dataGridView1.Invoke(new Action(() => dataGridView1.Refresh()));
                            log("desglosado consulta: " + epc);

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

        private void desglosaEpc(string epc, string fecha, bool grabar, string error, bool recarga)
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
                    lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal));
                    lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));

                    if (recarga)
                        epcs.Refresh();
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
                    //request.Timeout = timeOut;
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
                        if (!tagsOrden.Contains(epc))
                        {
                            if (desglose.noProducto != producto)
                            {
                                caja("Producto no corresponde a la orden", "", true);
                                return;
                            }
                            if (epcs.InvokeRequired)
                            {
                                epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, Int32.Parse(tbLinea.Text), fecha })));
                            }
                            else
                            {
                                epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, Int32.Parse(tbLinea.Text), fecha });
                            }
                            epcs.Refresh();
                            tagsOrden.Add(epc);
                        }

                        String navision = getPendientesNavision();
                        int totales = cantidadTotal - Int32.Parse(navision);

                        lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Text = cantidadTotal + "/" + totales + "-" + tagsOrden.Count + "[NAVISION:" + navision + "]"));
                        lCantidadTotal.Invoke(new Action(() => lCantidadTotal.Refresh()));

                        if (navision.Equals("0"))
                        {
                            if (result && !biocam)
                            {
                                caja("Orden finalizada " + "\n" + "Pendientes en Navision:" + getPendientesNavision(), "", true);
                                //}
                                StopEPC(true);
                                btnScanEPC.Text = strStart;
                                btnScanEPC.BackColor = System.Drawing.Color.Green;
                                btnScanEPC.ForeColor = System.Drawing.Color.White;
                                epcList.Clear();
                                btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = true));
                                btnScanEPC.Invoke(new Action(() => btnScanEPC.Text = strStart));
                            }
                            else
                            {
                                //recuperar orden para consultar pieza biocam
                                var urlPedido = apiMozoBase + "api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/codProcedenciaOrdenesProdArco?$filter=no eq '" + textBox1.Text.Trim() + "'";
                                var requestPedido = (HttpWebRequest)WebRequest.Create(urlPedido);
                                string svcCredentialsPedido = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                                requestPedido.Headers.Add("Authorization", "Basic " + svcCredentialsPedido);
                                requestPedido.Method = "GET";
                                requestPedido.ContentType = "application/json";
                                requestPedido.Accept = "*/*";
                                using (WebResponse response1 = requestPedido.GetResponse())
                                {
                                    using (Stream strReader = response1.GetResponseStream())
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
                }
            }
            catch (Exception e)
            {
                log("desglosaEpc:" + e.Message);
                //caja("Problemas de conexion con la base de datos, contacte con el administrador", "");
            }
        }

        private string getPendientesNavision()
        {
            String salida = "0";
            try
            {


                btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = false));
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/linOrdenesProdArco?$filter=prodOrderNo eq '" + textBox1.Text.Trim() + "'";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string username = usuario;
                string password = this.password;
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + svcCredentials);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                //request.Timeout = timeOut;
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            //if (strReader == null)
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                string responseBodyFormater = responseBody.Replace("@odata.context", "dataContext");
                                responseBodyFormater = responseBodyFormater.Replace("@odata.etag", "odataETag");
                                getMozo = JsonConvert.DeserializeObject<GetMozo>(responseBodyFormater);
                                if (getMozo.Value.Length > 0)
                                {
                                    //lReferencia.Text = getMozo.Value[0].ItemNo;

                                    salida = "" + getMozo.Value[0].RemainingQtyBase;

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
                //log("leerOrden:" + ex.Message);

            }
            return salida;
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
                                resultadoBiocam = new StringBuilder();
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
                                    caja(error + "\n" + resultadoBiocam.ToString() + "\nPendientes  navision: " + getPendientesNavision(), "", true);
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
                    //request.Timeout = timeOut;
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
            ordenInversa = false;
            uhf.leidos.Clear();
            if (textBox1.Text.Trim().Equals("") && ordenBiocam == null)
            {

                caja("Orden no puede estar vacia", "", true);

                return;
            }
            textoError.Text = "";
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

                //setTextCallback = new SetTextCallback(UpdataEPC);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ordenInversa = false;
            cargaOrden = true;
            textoError.Text = "";
            ordenBiocam = null;
            if (!result)
                toolStripButton1_Click();
            biocam = false;
            if (textBox1.Text.ToUpper().StartsWith("OFBC"))
                biocam = true;
            finOrden = false;
            epcs.Rows.Clear();
            tagsOrden.Clear();

            gtin = "";
            epcList.Clear();
            epcs.Refresh();
            loteActual = 1;
            tbLinea.Enabled = true;

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
            btnScanEPC.Invoke(new Action(() => btnScanEPC.Enabled = true));
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
            ordenInversa = false;
            contadorComprobacion = 0;
            uhf = UHFAPI.getInstance();
            uhf.leidos.Clear();

            if (!result)
                toolStripButton1_Click();
            //tagsTotales.Clear();
            try
            {
                lecturaDirecta = false;
                biocam = false;
                // dataGridView1.Rows.Clear();
                tagsComprobacion.Clear();
                if (!result)
                    result = uhf.OpenUsb();
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
                    //setTextCallback = new SetTextCallback(UpdataEPC);
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
            ordenInversa = true;
            uhf.leidos.Clear();
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
                    uhf.SetGen2(0, 0, 0, 1, 4, 0, 15, 1, 2, 1, 1, 1, 0, 3);
                    uhf.UHFSetBuzzer(Byte.Parse(buzzer));
                    StartReceiveThread();
                    StopEPC(false);
                }
                // UHFAPI.setOnDataReceived(onDataReceived);

            }
            catch (Exception ex)
            {
                log("toolStripButton1_Click:" + ex.Message);

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ordenInversa = false;
            //aqui lectura biocam
            textoError.Text = "";
            //tagsTotales.Clear();
            uhf.leidos.Clear();
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

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textoCaja_Click(object sender, EventArgs e)
        {

        }

        private void botonError_Click(object sender, EventArgs e)
        {
            panelError.Visible = false;
        }

        private void botonForzado_Click(object sender, EventArgs e)
        {
            grabarTagContadorAmipem(epcErroneo, 1);
        }
    }
};