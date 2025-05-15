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
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;
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

            //  uhf.SetTagfocus(0);
            //uhf.SetGen2()

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
            bool result = false;
            result = uhf.OpenUsb();
            // UHFAPI.setOnDataReceived(onDataReceived);
            UHFAPI.SetDisconnectCallback(DisconnectCallback);

            //uhf.SetGen2(0, 0, 1, 1, 4, 0, 15, 1, 2, 1, 00, 0, 0, 3);




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


        private void button1_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click();
        }



        private void StopEPC(bool isStop)
        {
            //finLote.Visible = false;
            //panel6.Visible = false;

            if (isStop)
            {
                if (uhf.StopInventory())
                {
                    if (epcs.InvokeRequired)
                    {

                        epcs.Invoke(new Action(() => btnScanEPC.Text = strStart));
                        epcs.Invoke(new Action(() => btnScanEPC.BackColor = System.Drawing.Color.Green));
                        epcs.Invoke(new Action(() => btnScanEPC.ForeColor = System.Drawing.Color.White));

                    }
                    else
                    {
                        btnScanEPC.Text = strStart;
                        btnScanEPC.BackColor = System.Drawing.Color.Green;
                        btnScanEPC.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            else
            {
                if (uhf.StartInventory())
                {
                    if (epcs.InvokeRequired)
                    {

                        epcs.Invoke(new Action(() => btnScanEPC.Text = strStop));
                        epcs.Invoke(new Action(() => btnScanEPC.BackColor = System.Drawing.Color.Red));
                        epcs.Invoke(new Action(() => btnScanEPC.ForeColor = System.Drawing.Color.White));
                    }
                    else
                    {
                        btnScanEPC.Text = strStop;
                        btnScanEPC.BackColor = System.Drawing.Color.Red;
                        epcs.Invoke(new Action(() => btnScanEPC.ForeColor = System.Drawing.Color.White));
                    }
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


        private void StartReceiveThread()
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



        /*private void UpdataEPC(string epc)
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

                        //leerFicha(epc, Int32.Parse(tbLinea.Text), epcs);


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
                    // leerFicha(epc, Int32.Parse(tbLinea.Text), epcs);
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
            }





        */



        private bool validaProducto(string epc,Action callback)
        {


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
                    /*ry
                    {
                        
                        if (epcs.InvokeRequired)
                        {
                            epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "", "", "EPC no corresponde a la orden", "", "" })));
                        }
                        else
                        {
                            epcs.Rows.Add(new object[] { "", "", "EPC no corresponde a la orden", "", "" });
                        }
                        epcs.Rows[epcs.Rows.GetLastRow(DataGridViewElementStates.Visible)-1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                    }
                    catch (Exception e) {
                        int i = 0;
                    }*/
                    //  caja("Producto no corresponde a la orden\r "+desglosaEpcErroneo(epc), 3000);
                }

                return resultado;
            }
            else
            {

                return true;
            }
        }

        private bool leerOrden()
        {
            if (ubicación.Equals("casa"))
            {
                leerOrdenAmipem();
                return true;
            }
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

            var url = apiAmipemBase + "leerTagsOrden" ;

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            string salida = "{\"codigo\":\"" + textBox1.Text + "\"}";
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
                            GrabacionRespuesta grabacionRespuesta = JsonConvert.DeserializeObject<GrabacionRespuesta>(responseBody);
                            totalesValidos = grabacionRespuesta.grabaciones.Length;



                            /*if (grabaciones.Length > 0)
                                loteActual = 1;*/
                            //tagsOrden.Clear();
                            epcList.Clear();
                            if (epcs.InvokeRequired)
                            {
                                epcs.Invoke(new Action(() => epcs.Rows.Clear()));
                            }
                            else
                            {
                                epcs.Rows.Clear();
                            }

                            for (int i = 0; i < grabacionRespuesta.grabaciones.Length; i++)
                            {

                                tagsOrden.Add(grabacionRespuesta.grabaciones[i].tag, Int32.Parse(tbLinea.Text));

                                desglosaEpc(grabacionRespuesta.grabaciones[i].tag, grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToShortDateString() + " " + grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToShortTimeString());


                            }
                            cantidadReal = cantidadTotal - grabacionRespuesta.grabaciones.Length;
                            /* if (grabaciones.Length == cantidadTotal)
                             {
                                 StopReceiveThread();
                                 StopEPC(true);
                                 uhf.Close();
                                 button3_Click(null, null);
                                 epcList.Clear();



                                 int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                                 if (epcs.InvokeRequired)
                                 {
                                     epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "", "", "Orden finalizada", "", "" })));



                                 }
                                 else
                                 {
                                     epcs.Rows.Add(new object[] { "", "", "Orden finalizada", "", "" });

                                 }
                                 btnScanEPC.Enabled = false;
                                 epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                                 epcs.Rows[lastRowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                                 btnScanEPC.Enabled = false;
                                 StopEPC(true);
                                 StopReceiveThread();


                             }*/



                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int f = 0;
            }
        }



        private void caja(string texto, int duracion)
        {
            // Replacing the problematic 'DisplayAlert' with a MessageBox for Windows Forms.  
            System.Windows.Forms.MessageBox.Show(texto, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void leerOrdenAmipem()
        {
            if (textBox1.Text.Trim().Equals(""))
            {
                caja("Orden no puede estar vacia", 3000);

                return;
            }
            var url = apiAmipemBase + "leerOrden";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //Orden orden = new Orden(idOrdenAmipem, textBox1.Text, lDescripcion.Text, lReferencia.Text, Int32.Parse(lCantidadTotal.Text));
            string salida = "{\"codigo\":\"" + textBox1.Text + "\"}";
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

                            string responseBody = objReader.ReadToEnd();
                            Orden orden = JsonConvert.DeserializeObject<Orden>(responseBody);
                            lReferencia.Text = orden.item;
                            producto = orden.item;
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

            var url = apiAmipemBase + "Ordenes/grabarOrden";

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



        private void grabarTagContadorAmipem(string epc, int lote, Action callback, MainForm yo)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (obj, e) =>
            {

                try
                {
                    if(!tagsOrden.ContainsKey(epc))
                    {
                        
                    


                    var url = apiAmipemBase + "grabarTagContador";

                    var request = (HttpWebRequest)WebRequest.Create(url);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    DateTime fecha = DateTime.Now;
                    GrabacionDTO grabacionDTO = new GrabacionDTO( textBox1.Text, epc, Int32.Parse(tbLinea.Text.ToString()));
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
                            tagsOrden.Add(epc, Int32.Parse(tbLinea.Text));

                            int ultimograbado = desglosaEpc(epc, grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToShortDateString() + " " + grabacionRespuesta.grabaciones[grabacionRespuesta.grabaciones.Length - 1].fecha.ToShortTimeString());

                            grabarTagContadorMozo(epc, Int32.Parse(tbLinea.Text), () =>
                            {

                            }, this, ultimograbado);
                            if (!finOrden)
                                if (epcs.InvokeRequired)
                                {
                                    epcs.Invoke(new Action(() => epcs.Rows[ultimograbado].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow));
                                    epcs.Invoke(new Action(() => epcs.Rows[ultimograbado].DefaultCellStyle.ForeColor = System.Drawing.Color.Black));
                                }
                                else
                                {
                                    epcs.Rows[ultimograbado].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                                    epcs.Rows[ultimograbado].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                                }

                            cantidadReal--;
                            if (lCantidadTotal.InvokeRequired)
                            {
                                epcs.Invoke(new Action(() => lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabacionRespuesta.grabaciones.Length)));
                            }
                            else
                            {
                                lCantidadTotal.Text = "" + cantidadTotal + "/" + (cantidadTotal - grabacionRespuesta.grabaciones.Length);
                            }



                        }

                    }
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);


                }
                //uhf.StartInventory();

            };
            worker.RunWorkerAsync();
        }


        private bool grabarTagContadorMozo(string epc, int lote, Action callback, MainForm yo, int posicion)
        {

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (obj, e) =>
                {
                    //Thread.Sleep(3000);
                    if (tagsOrden.Count >= cantidadTotal)
                    {
                        StopEPC(true);
                        StopReceiveThread();


                    }
                    // Thread.Sleep(2000);

                    epcs.Rows[posicion - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    epcs.Rows[posicion - 1].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    //StartReceiveThread();
                };
                worker.RunWorkerAsync();
            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }
            return true;

            /* if (!ubicación.Equals("casa"))
             {

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
                 request.Accept = "*";
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

                         int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                         if (lastRowIndex >= 0)
                         {
                             // Fix for CS0103: Ensure DefaultCellStyle is accessed correctly
                             epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                             epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Black;
                         }
                         if (tagsOrden.Count == cantidadTotal)
                         {
                             if (epcs.InvokeRequired)
                             {

                                 epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "", "", "Orden finalizada", "", "" })));
                                 epcs.Rows[epcs.Rows.GetLastRow(DataGridViewElementStates.Visible) - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                             }
                             else
                             {
                                 epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "", "", "Orden finalizada", "", "" })));
                                 epcs.Rows[epcs.Rows.GetLastRow(DataGridViewElementStates.Visible) - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                             }

                             StopEPC(true);
                             StopReceiveThread();
                         }
                         resultado = true;
                     }
                 }
                 catch (Exception e)
                 {
                     borraUltimaGrabacion(epc);
                     //borrar ultimo registro

                     resultado = false;
                 }

             }
             else
             {
                 int lastRowIndex = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                 epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                 epcs.Rows[lastRowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Black;
             }

             return resultado;*/

        }

        private void borraUltimaGrabacion(string epc)
        {
            var url = apiAmipemBase + "borraUltimaGrabacion/" + epc;

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

                            epcs.Rows.RemoveAt(epcs.Rows.Count - 1);
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
                    int i = 0;
                    while (isRuning)
                    {



                        if (tagsOrden.Count <= cantidadTotal + 1)
                        {
                            UHFTAGInfo info = uhf.ReadTagFromBuffer();

                            if (info != null && !tagsOrden.ContainsKey(info.Epc))
                            {
                                // caja(info.Epc, 1);
                                //StopReceiveThread();
                                validaProducto(info.Epc,() => { });
                                    grabarTagContadorAmipem(info.Epc, Int32.Parse(tbLinea.Text), () =>
                                    {

                                    }, this);
                            }

                        }
                        //Thread.Sleep(200);
                    }
                };
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private int desglosaEpc(string epc, string fecha)
        {
            //tagsOrden.Add(epc, Int32.Parse(tbLinea.Text));


            int ultimograbado = 0;
            if (ubicación.Equals("casa"))
            {




                if (epcs.InvokeRequired)
                {

                    epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" })));
                    ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                }
                else
                {

                    //epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" });
                    epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" });
                    ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                }
                epcs.Rows[ultimograbado - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;


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
                        //Thread.Sleep(500);
                        if (epcs.InvokeRequired)
                        {


                            epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, Int32.Parse(tbLinea.Text), fecha })));
                        }
                        else
                        {



                            epcs.Rows.Add(new object[] { desglose.noProducto, desglose.noLote, desglose.noSerie, Int32.Parse(tbLinea.Text), fecha });
                        }
                        ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                        epcs.Rows[ultimograbado - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        epcs.Rows[ultimograbado - 1].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        if (tagsOrden.Count >= cantidadTotal)
                        {
                            finOrden = true;
                            if (epcs.InvokeRequired)
                            {

                                epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "", "", "Orden finalizada", "", "" })));
                                epcs.Rows[epcs.Rows.GetLastRow(DataGridViewElementStates.Visible) - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                            }
                            else
                            {
                                epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "", "", "Orden finalizada", "", "" })));
                                epcs.Rows[epcs.Rows.GetLastRow(DataGridViewElementStates.Visible) - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                            }

                            StopEPC(true);
                            StopReceiveThread();
                            // leerTagsOrdenAmipem();
                        }

                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }



            }

            return ultimograbado;
        }



        private string desglosaEpcErroneo(string epc)
        {
            //tagsOrden.Add(epc, Int32.Parse(tbLinea.Text));


            string salida = "";
            if (ubicación.Equals("casa"))
            {




                /* if (epcs.InvokeRequired)
                 {

                     epcs.Invoke(new Action(() => epcs.Rows.Add(new object[] { "23207930-", "25017318", "250173180017", 1, "01/01/2025" })));
                     ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                 }
                 else
                 {

                     //epcs.Rows.Add(new object[] { "23207930", "25017318", "250173180017", 1, "01/01/2025" });
                     epcs.Rows.Add(new object[] { "23207930-", "25017318", "250173180017", 1, "01/01/2025" });
                     ultimograbado = epcs.Rows.GetLastRow(DataGridViewElementStates.Visible);
                 }
                 epcs.Rows[ultimograbado - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Red;*/

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
                        //Thread.Sleep(500);

                        salida = "producto: " + desglose.noProducto + "\r lote: " + desglose.noLote + "\r serie: " + desglose.noSerie;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }



            }

            return salida;
        }
        private void button3_Click(object sender, EventArgs e)
        {


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

        private void button2_Click(object sender, EventArgs e)
        {
            finOrden = false;
            epcs.Rows.Clear();
            tagsOrden.Clear();
            tbLotes.Text = "1";
            gtin = "";
            epcList.Clear();
            loteActual = 1;
            tbLinea.Enabled = true;
            tbLotes.Enabled = true;




            if (ubicación.Equals("casa"))
            {
                leerOrdenAmipem();
            }
            else
            {

                try
                {
                   
                    if (leerOrden())
                        grabarOrdenAmipem();
                    leerOrdenAmipem();
                    if (tagsOrden.Count == cantidadTotal)
                    {
                        btnScanEPC.Enabled = false;
                    }


                }
                catch (Exception e1)
                {

                    Console.WriteLine(e1.Message);
                }


            }



            // panel6.Visible = false;
            StopEPC(true);
            btnScanEPC.Text = strStart;
            btnScanEPC.BackColor = System.Drawing.Color.Green;
            btnScanEPC.ForeColor = System.Drawing.Color.White;


            //epcs.Rows.Add("" ,"EPC");

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!tbLotes.Text.Trim().Equals(""))
            {
                try
                {
                    double parcial = (double)(Double.Parse(lCantidadTotal.Text) / Double.Parse(tbLotes.Text));

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

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}