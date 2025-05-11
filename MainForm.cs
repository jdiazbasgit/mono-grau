using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Shapes;
using ApodemiaC;
using BLEDeviceAPI;
using Newtonsoft.Json;



//using UHFAPP.barcode;
using UHFAPP.utils;
using WinForm_Test;
using WMPLib;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static Lucene.Net.Documents.Field;
using static UHFAPP.UHFAPI;
using static UHFAPP.utils.EpcInfo;



namespace UHFAPP
{
    public partial class MainForm : BaseForm
    {
        public static int MODE = 1;//0:串口   1:网口    2:usb
        public static string ip = "";
        public static uint portData = 0;
        int total = 0;
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
        private List<string> tags = new List<string>();
        private List<int> tagsCantidad = new List<int>();
        private string currentFormName = "";
        private bool isOpen = false;
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
        private string idProducto;
        SortedDictionary<string, int> tagsOrden = new SortedDictionary<string, int>();
        public static FormWindowState currState = FormWindowState.Normal;
        List<EpcInfo> epcList = new List<EpcInfo>();
        public bool isSearch = false;
        #region  OnDisconnect
        //elegate void SetTextCallback(string epc, DataGridView epcs);
        //SetTextCallback setTextCallback;
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
            }

        }

        UHFAPP.UHFAPI.OnDisconnectCallback DisconnectCallback = null;

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
            /*epcs.Columns.Add("PRODUCTO", "PRODUCTO");
            epcs.Columns.Add("LOTE", "LOTE");
            epcs.Columns.Add("NUMERO DE SERIE", "NUMERO DE SERIE");
            epcs.Columns.Add("LINEA", "LINEA");
            epcs.Columns.Add("FECHA", "FECHA");*/


            toolStripButton1_Click();
        }
        public void cargarDatos(string archivo)
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\" + archivo);
            //Read the first line of text
            apiAmipemBase = sr.ReadLine();
            apiMozoBase = sr.ReadLine();
            cliente = sr.ReadLine();
            usuario = sr.ReadLine();
            password = sr.ReadLine();
            ubicación = sr.ReadLine();
            //Continue to read until you reach end of file

            //close the file
            sr.Close();
            Console.ReadLine();
            loteActual++;

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {


        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSearch = false;
            UHFClose();
            if (eventOpen != null)
            {
                eventOpen(false);
            }
           // Thread.Sleep(500);
        }
        private bool UHFClose()
        {
            if (button1.Text.Trim() == strClose.Trim())
            {
                uhf.CloseUsb();
                return true;
            }
            return false;
        }


        private void toolStripButton1_Click()
        {
            btnScanEPC.BackColor = System.Drawing.Color.White;
            btnScanEPC.Enabled = false;
            btnScanEPC.Visible = true;
            string msg = "Conectando lector...";
            //StartReceiveThread();
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                bool result = false;
                result = uhf.OpenUsb();
                // UHFAPI.setOnDataReceived(onDataReceived);
                UHFAPI.SetDisconnectCallback(DisconnectCallback);
                if (result)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        isOpen = true;
                        if (eventOpen != null)
                        {
                            eventOpen(true);
                        }

                    }));
                   // Thread.Sleep(2000);
                }
                else
                {
                    caja("Error al conectar el lector", 3000);
                }
                byte b = 1;
                //uhf.SetGen2(0, 0, 1, 1, 4, 0, 15, 1, 2, 1, 00, 0, 0, 3);



            }, msg);
            f.BackColor = System.Drawing.Color.Gray;
            f.ShowDialog(this);
            button1.BackColor = System.Drawing.Color.GreenYellow;
            button1.Text = strClose;
            btnScanEPC.Visible = true;
            btnScanEPC.BackColor = System.Drawing.Color.Green;
            btnScanEPC.Text = strStart;
        }


        #region RS160 KeyDwon



        public int cantidadTotal { get; private set; }




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
            finLote.Visible = false;
            panel6.Visible = false;

            if (isStop)
            {
                if (uhf.StopInventory())
                {
                    btnScanEPC.Text = strStart;
                    btnScanEPC.BackColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                if (uhf.StartInventory())
                {
                    btnScanEPC.Text = strStop;
                    btnScanEPC.BackColor = System.Drawing.Color.Red;
                }
            }

        }

        private void Time() { }

        private void StopReceiveThread()
        {
            isVisible = false;
            isRuning = false;
           // Thread.Sleep(100);
        }


        private void StartReceiveThread(DataGridView epcs)
        {
            if (!isRuning)
            {
                isRuning = true;
                try
                {
                    /* hiloLectura = new Thread(new ThreadStart(delegate { ReadEPC(epcs); }));
                     hiloLectura.Start();*/
                    ReadEPC(() =>
                    {
                        
                    }, this);

                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }



        private void UpdataEPC(string epc)
        {

            if (epc == null)
            {
                return;
            }



            if (!ubicación.Equals("casa"))
            {
                try
                {
                    if (validaProducto(epc))
                    {

                        leerFicha(epc, Int32.Parse(tbLinea.Text), epcs);

                       
                        //leerOrden();
                    }
                    else
                    {
                        caja("Producto no corresponde a la orden", 3000);

                    }
                }
                catch (Exception e)
                {

                    Console.Write(e.Message);
                }
            }
            else
            {
                if (validaProducto(epc))
                {
                    leerFicha(epc, Int32.Parse(tbLinea.Text), epcs);

                }
            }

            if (epcs.InvokeRequired)
            {
                
                    epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" })));
                    
            }
            else
            {
                
                    epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" });
                    
                
            }
            Console.Write(epc);




        }



        private bool validaProducto(string epc)
        {
            // The following line was causing the CS7036 error because the required "uDatabuf" parameter was not provided.  
            // Fix: Provide a valid byte array for the "uDatabuf" parameter.  
            byte[] uno = { 0, 0, 0, 0 };
            byte[] uDatabuf = { 1, 0 };
            bool grabado = uhf.BlockWriteData(uno, 2, 0, 0, uno, 2, 0, 2, uDatabuf);

            bool resultado = false;
            if (!ubicación.Equals("casa"))
            {
                if (gtin.Trim().Length == 0)
                {
                    var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/productosArco?$filter=no eq '" + producto + "'";

                    var request = (HttpWebRequest)WebRequest.Create(url);
                    string username = this.usuario;

                    string password = this.password;
                    string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
                    request.Headers.Add("Authorization", "Basic " + svcCredentials);
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    request.Accept = "*/*";
                    try
                    {
                        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                        {
                            Stream stream1 = response.GetResponseStream();
                            StreamReader sr = new StreamReader(stream1);
                            string strsb = sr.ReadToEnd();
                            strsb = strsb.Replace("@odata.context", "dataContext");
                            strsb = strsb.Replace("@odata.etag", "dataTag");
                            Gtin gtinWeb = JsonConvert.DeserializeObject<Gtin>(strsb);
                            gtin = gtinWeb.value[0].gtin;
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        resultado = false;
                    }
                }

                if (epc.Substring(3, 13).Equals(gtin))
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                    try
                    {
                        string url = Environment.CurrentDirectory + "\\chimes.wav";
                        SoundPlayer simpleSound1 = new SoundPlayer(@url);
                        simpleSound1.Play();
                    }
                    catch (Exception e) { }
                    caja("EPC no corresponde a la orden", 3000);
                }

                return resultado;
            }
            else
            {
                /* int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                 if (lastRowIndex >= 0)
                 {
                     epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                 }*/
                return true;
            }
        }

        private bool leerOrden()
        {
            btnScanEPC.Enabled = false;
            bool resultado = false;
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();

            var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/linOrdenesProdArco?$filter=prodOrderNo eq '" + textBox1.Text + "'";
            cantidadReal = 0;
            var request = (HttpWebRequest)WebRequest.Create(url);
            string username = usuario;
            string password = this.password;
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                            return resultado;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            string responseBodyFormater = responseBody.Replace("@odata.context", "dataContext");
                            responseBodyFormater = responseBodyFormater.Replace("@odata.etag", "odataETag");
                            getMozo = JsonConvert.DeserializeObject<GetMozo>(responseBodyFormater);
                            if (getMozo.Value.Length > 0)
                            {
                                lReferencia.Text = getMozo.Value[0].ItemNo;
                                producto = getMozo.Value[0].ItemNo;
                                cantidadReal = getMozo.Value[0].RemainingQtyBase;
                                lDescripcion.Text = getMozo.Value[0].Description;
                                lCantidadTotal.Text = getMozo.Value[0].RemainingQtyBase.ToString();

                                pReferencia.Visible = true;
                            }
                            resultado = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // SoundPlayer simpleSound1 = new SoundPlayer(@url);
                //simpleSound1.Play();
                resultado = false;
                caja("Orden no existe", 3000);
                textBox1.Text = "";
            }
            return resultado;
        }

        private void leerTagsOrdenAmipem()
        {

            var url = apiAmipemBase + "leerTagsOrden/" + textBox1.Text;

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {

                            string responseBody = objReader.ReadToEnd();
                            Grabacion[] grabaciones = JsonConvert.DeserializeObject<Grabacion[]>(responseBody);
                            totalesValidos = grabaciones.Length;
                            totalesOrden.Text = "" + totalesValidos;


                            /*if (grabaciones.Length > 0)
                                loteActual = 1;*/
                            tagsOrden.Clear();
                            epcList.Clear();
                            epcs.Rows.Clear();
                            for (int i = 0; i < grabaciones.Length; i++)
                            {

                                // tagsOrden.Add(grabaciones[i].tag, Int32.Parse(tbLinea.Text));
                                //epcs.Rows.Add(new object[] { grabaciones[i].tag, grabaciones[i].lote, grabaciones[i].linea });
                                /* bool[] exist = new bool[1];
                                 int index = CheckUtils.getInsertIndex(epcList, grabaciones[i].tag, "", exist);
                                 EpcInfo epcInfo = new EpcInfo(grabaciones[i].tag, "", grabaciones.Length, DataConvert.HexStringToByteArray(grabaciones[i].tag), DataConvert.HexStringToByteArray(""), 1, "-79.20", "");
                                 epcList.Insert(index, epcInfo);*/
                                leerFicha(grabaciones[i].tag, grabaciones[i].linea, epcs);
                                //loteActual = grabaciones[i].lote;
                            }
                            cantidadReal = cantidadTotal - grabaciones.Length;
                            if (tagsOrden.Count == cantidadTotal)
                            {
                                StopReceiveThread();
                                StopEPC(true);
                                uhf.Close();
                                button3_Click(null, null);
                                epcList.Clear();
                                tbLotes.Enabled = false;
                                tbLinea.Enabled = false;
                                panel6.Visible = true;
                                epcs.Rows.Add(new object[] { "", "", "FIN DE ORDEN", "", "" });

                                // Fix for CS7036: Provide the required argument "includeFilter" to GetLastRow method  
                                int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                                if (lastRowIndex >= 0)
                                {
                                    // Fix for CS0103: Ensure DefaultCellStyle is accessed correctly  
                                    epcs.Rows[lastRowIndex - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                                }
                                btnScanEPC.Enabled = false;


                            }

                            totalesOrden.Text = "" + grabaciones.Length;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int f = 0;
            }
        }

        private void leerFicha(string tag, int linea, DataGridView epcs1)
        {
            try
            {
                tagsOrden.Add(tag, linea);

                if (ubicación.Equals("casa"))
                {


                    epcs1.Rows.Add(new object[] { "23207930", "25017318", "250173180017", linea, "01/01/2025" });


                }
                else
                {

                }

            }
            catch (Exception e)
            {

                int i = 0;
            }
        }

        private void caja(string texto, int duracion)
        {
            /* var wplayer = new WindowsMediaPlayer();
             wplayer.URL = Environment.CurrentDirectory + "\\error.mp3";
             wplayer.controls.play();*/

            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                this.FontHeight = 22;
                Thread.Sleep(duracion);

            }, duracion, texto, true, true);
            f.BackColor = System.Drawing.Color.Gray;
            f.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            f.ShowDialog(this);

        }
        private void leerOrdenAmipem()
        {
            if (textBox1.Text.Trim().Equals(""))
            {
                caja("Orden no puede estar vacia", 3000);

                return;
            }
            var url = apiAmipemBase + "leerOrden/" + textBox1.Text;

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {

                            string responseBody = objReader.ReadToEnd();
                            Orden orden = JsonConvert.DeserializeObject<Orden>(responseBody);
                            lReferencia.Text = orden.item;
                            lDescripcion.Text = orden.descripcion;
                            cantidadTotal = orden.cantidad;
                            try
                            {
                                lCantidadTotal.Text = "" + cantidadTotal + "/" + cantidadReal;
                            }
                            catch (Exception)
                            {

                                lCantidadTotal.Text = "" + cantidadTotal + "/0";
                            }
                            pReferencia.Visible = true;
                            pTotal.Visible = true;
                            idOrdenAmipem = orden.id;
                            btnScanEPC.Enabled = true;
                            leerTagsOrdenAmipem();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                caja("Orden no existe", 3000);
                textBox1.Text = "";
            }
        }

        private bool grabarOrdenAmipem()
        {
            bool resultado = false;
            btnScanEPC.Enabled = false;

            var url = apiAmipemBase + "grabarOrden/";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                Orden orden = new Orden(idOrdenAmipem, textBox1.Text, lDescripcion.Text, lReferencia.Text, Int32.Parse(lCantidadTotal.Text));
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

                    /*lotesOrden = ordenRetorno.lotes;
                    cantidadPorLote = Int32.Parse(lCantidadTotal.Text) / Int32.Parse(tbLotes.Text);
                    label25.Text = "Lotes: " + lotesOrden + " (" + cantidadPorLote + ")";
                    label9.Text = "Lote actual(" + loteActual + "): " + tagsOrden[loteActual].Count;*/
                    btnScanEPC.Enabled = true;
                    resultado = true;
                }
            }
            catch (Exception e)
            {
                resultado = false;

            }
            return resultado;
        }



        private bool grabarTagContadorAmipem(string epc, int lote)
        {
            //uhf.StopInventory();
            bool resultado = false;
            //epcs.Rows[epcs.Rows.Count - 2].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();

            var url = apiAmipemBase + "grabarTagContador";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {

                DateTime fecha = DateTime.Now;
                GrabacionDTO grabacionDTO = new GrabacionDTO(1, textBox1.Text, epc, Int32.Parse(tbLinea.Text.ToString()));
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
                    Grabacion[] grabaciones = JsonConvert.DeserializeObject<Grabacion[]>(strsb);

                    tagsOrden.Clear();
                    epcList.Clear();
                    epcs.Rows.Clear();
                    for (int i = 0; i < grabaciones.Length; i++)
                    {
                        //if (!tagsOrden.ContainsKey(grabaciones[i].lote))
                        //tagsOrden.Add(grabaciones[i].tag, grabaciones[i].linea);
                        //tagsOrden[grabaciones[i].lote].Add(new Lectura(grabaciones[i].tag, grabaciones[i].lote, grabaciones[i].linea));
                        bool[] exist = new bool[1];
                        int index = CheckUtils.getInsertIndex(epcList, grabaciones[i].tag, "", exist);
                        EpcInfo epcInfo = new EpcInfo(grabaciones[i].tag, "", grabaciones.Length, DataConvert.HexStringToByteArray(grabaciones[i].tag), DataConvert.HexStringToByteArray(""), 1, "-79.20", "");
                        epcList.Insert(index, epcInfo);
                        leerFicha(grabaciones[i].tag, grabaciones[i].linea, epcs);
                        // loteActual = grabaciones[i].lote;

                    }
                    if (grabaciones.Length == cantidadTotal)
                    {
                        panel6.Visible = true;
                        epcs.Rows.Add(new object[] { "", "", "FIN DE ORDEN", "", "" });
                        int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                        if (lastRowIndex >= 0)
                        {
                            // Fix for CS0103: Ensure DefaultCellStyle is accessed correctly  
                            epcs.Rows[lastRowIndex - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                        }
                        tbLotes.Enabled = false;
                        tbLinea.Enabled = false;
                        resultado = true;
                        StopEPC(true);
                    }
                    else
                    {


                        cantidadReal--;
                        lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabaciones.Length);

                    }

                    resultado = true;
                }
            }
            catch (Exception e)
            {
                caja("Tag ya grabado", 3000);
                resultado = false;

            }
            //uhf.StartInventory();
            return resultado;
        }


        private bool grabarTagContadorMozo(string epc, int lote)
        {

            bool resultado = false;
            //epcs.Rows[epcs.Rows.Count - 2].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();

            var url = apiMozoBase + "api/mozo/apiArco/v2.0/companies(" + cliente + ")/registrarSalidasRFID?";

            var request = (HttpWebRequest)WebRequest.Create(url);
            string username = this.usuario;
            string password = this.password;
            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + svcCredentials);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "*/*";
            try
            {
                GrabacionMozo grabacion = new GrabacionMozo(epc, textBox1.Text, 10000, 1);
                // grabacion.lecturaRFID = epc.Replace("100701","555666");
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
                    // Fix for CS7036: Provide the required argument "includeFilter" to GetLastRow method
                    int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                    if (lastRowIndex >= 0)
                    {
                        // Fix for CS0103: Ensure DefaultCellStyle is accessed correctly
                        epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    }
                    resultado = true;
                }
            }
            catch (Exception e)
            {
                caja("Tag ya grabado", 3000);
                resultado = false;
            }

            return resultado;
            //return true;
        }
        private void ScanEPCForm_Load(object sender, EventArgs e)
        {
            //setTextCallback = new SetTextCallback(UpdataEPC);

            var bounds = Screen.FromControl(this).Bounds;
            this.Width = bounds.Width - 50;
            this.Height = bounds.Height - 50;
            panel9.Width = this.Width;
            panel9.Height = this.Height;
            epcs.Width = panel9.Width - epcs.Location.X;
            epcs.Height = panel9.Height - epcs.Location.Y - 100;
            this.MaximumSize = SystemInformation.PrimaryMonitorMaximizedWindowSize;
            this.WindowState = FormWindowState.Maximized;
        }
        private void ReadEPC(Action callback, MainForm yo)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (obj, e) =>
                {
                    while (isRuning)
                    {
                        UHFTAGInfo info = uhf.ReadTagFromBuffer();
                        

                        // uhf.GetCW(ref 1);

                        // Fixed line
                        byte flag = 1; // Declare a variable to hold the value
                        //Console.Write(uhf.GetCW(ref flag)); // Pass the variable as a ref parameter
                        

                        if (info != null && !tagsOrden.ContainsKey(info.Epc))
                        {
                            //this.BeginInvoke(setTextCallback, new object[] { info.Epc ,epcs});
                            UpdataEPC(info.Epc);

                        }
                        //Thread.Sleep(100);
                    }
                };
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //int totalesTodasLineas = 0;
            /* foreach (int lote in tagsOrden.Keys)
             {
                 foreach (Lectura lectura in tagsOrden[lote])
                 {
                     totalesTodasLineas++;

                 }

             }*/
            if (btnScanEPC.Text == strStop)
            {
                StopReceiveThread();
                StopEPC(true);



            }
            else
            {
                StartReceiveThread(epcs);
                StopEPC(false);

            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            epcs.Rows.Clear();
            tagsOrden.Clear();
            tbLotes.Text = "1";
            gtin = "";
            epcList.Clear();
            loteActual = 1;
            tbLinea.Enabled = true;
            tbLotes.Enabled = true;



            totalesOrden.Text = "0";
            if (ubicación.Equals("casa"))
            {
                leerOrdenAmipem();
            }
            else
            {

                try
                {
                    leerOrden();
                    // grabarOrdenAmipem();

                }
                catch (Exception)
                {

                    int a = 0;
                }


                btnScanEPC.Enabled = true;
            }



            finLote.Visible = false;
            // panel6.Visible = false;
            StopEPC(true);
            btnScanEPC.Text = strStart;
            btnScanEPC.BackColor = System.Drawing.Color.Green;



            //epcs.Rows.Add("" ,"EPC");

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!tbLotes.Text.Trim().Equals(""))
            {
                try
                {
                    double parcial = (double)(Double.Parse(lCantidadTotal.Text) / Double.Parse(tbLotes.Text));
                    // cantidadPorLote = (int)Math.Ceiling(parcial);
                    //lCantidad.Text = "Lotes: " + Int32.Parse(tbLotes.Text) + " (" + cantidadPorLote + ")";
                }

                catch (Exception)
                {


                }
            }
        }

        private void actualizaLotes()
        {
            if (textBox1.Text.Trim().Equals(""))
            {
                caja("Orden no existe", 3000);

                return;
            }
            var url = apiAmipemBase + "leerOrden/" + textBox1.Text;

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {

                            string responseBody = objReader.ReadToEnd();
                            Orden orden = JsonConvert.DeserializeObject<Orden>(responseBody);


                            var url1 = apiAmipemBase + "grabarOrden/";

                            var request1 = (HttpWebRequest)WebRequest.Create(url1);

                            request1.Method = "POST";
                            request1.ContentType = "application/json";
                            request1.Accept = "application/json";
                            try
                            {
                                // Orden orden = new Orden(idOrdenAmipem, texorden.lotes = Int32.Parse(tbLotes.Text);
                                string salida = JsonConvert.SerializeObject(orden);
                                byte[] data = Encoding.UTF8.GetBytes(salida);
                                request1.ContentLength = data.Length;
                                Stream stream = request1.GetRequestStream();
                                stream.Write(data, 0, data.Length);
                                stream.Close();
                                using (HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse)
                                {
                                    //Leer el resultado de la llamada
                                    Stream stream1 = response1.GetResponseStream();
                                    StreamReader sr = new StreamReader(stream1);
                                    string strsb = sr.ReadToEnd();
                                    Orden ordenRetorno = JsonConvert.DeserializeObject<Orden>(strsb);


                                }
                            }
                            catch (Exception e)
                            {
                                caja("Lotes no grabados", 3000);

                            }





                        }
                    }
                }
            }
            catch (Exception ex)
            {
                caja("Orden no existe", 3000);
                textBox1.Text = "";
            }

        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            var bounds = Screen.FromControl(this).Bounds;
            this.Width = bounds.Width;
            this.Height = bounds.Height;
            panel9.Width = this.Width;
            panel9.Height = this.Height;
            epcs.Width = this.Width - epcs.Location.X;
            epcs.Height = this.Height - epcs.Location.Y - 100;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pReferencia_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lUnidades_Click(object sender, EventArgs e)
        {

        }

        private void totalesOrden_Click(object sender, EventArgs e)
        {

        }

        private void epcs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pLotes_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}