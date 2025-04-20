using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using ApodemiaC;
using BLEDeviceAPI;
using Lucene.Net.Support;
using Newtonsoft.Json;



//using UHFAPP.barcode;
using UHFAPP.Entity;
using UHFAPP.RFID;
using UHFAPP.utils;
using WinForm_Test;
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
        string strOpen = "  Conectar lector  ";
        string strClose = "  Desconectar lector  ";
        string strStart = "  Iniciar lectura  ";
        string strStop = "  Detener lectura  ";
        private List<string> tags = new List<string>();
        private List<int> tagsCantidad = new List<int>();
        private string currentFormName = "";
        private bool isOpen = false;
        private int idOrdenAmipem;
        private int totalesLeidos;
        private int totalesValidos;
        public MainForm mainform = null;
        private int cantidadPorLote;
        private int loteActual;
        public static FormWindowState currState = FormWindowState.Normal;
        List<EpcInfo> epcList = new List<EpcInfo>();
        public bool isSearch = false;
        List<ReaderDeviceInfo> listIP = new List<ReaderDeviceInfo>();
        #region  OnDisconnect
        delegate void SetTextCallback(string epc, string tid, string rssi, string count, string ant, string user);
        SetTextCallback setTextCallback;
        private void OnDisconnectCallback(int id)
        {
            try
            {
                if (!this.IsDisposed)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        disableControls(false);
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
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.IsMdiContainer = true;
            mainform = this;
            button1.Text = strOpen;
            button1.BackColor = System.Drawing.Color.Red;
            DisconnectCallback = OnDisconnectCallback;
            toolStripButton1_Click();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setComPort();
            disableControls(false);

            btnSearch_Click(null, null);
            LoadUI();
            //combCommunicationMode.SelectedIndex = 1;
            if (eventMainSizeChanged != null)
            {
                eventMainSizeChanged(WindowState);
            }
        }

        public void ReadWriteTag(string tag, int bank)
        {


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
            Thread.Sleep(500);
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



        #region MenuItem_Click
        /// <summary>
        /// R/W
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemReadWriteTag_Click(object sender, EventArgs e)
        {
            ReadWriteTag("", 0);
        }
        /// <summary>
        /// Inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void ocultarControles(bool dato)
        {

            label1.Visible = dato;
            textBox1.Visible = dato;
            button2.Visible = dato;
            //textBox2.Visible = dato;
            btnScanEPC.Visible = dato;
            label2.Visible = dato;
            label3.Visible = dato;
            //  label4.Visible = dato;
            label5.Visible = dato;
            label6.Visible = dato;
            label7.Visible = dato;
            label8.Visible = dato;
            label9.Visible = dato;
            label14.Visible = dato;
            label15.Visible = dato;
            // label16.Visible = dato;
            label17.Visible = dato;
            label18.Visible = dato;
            label19.Visible = dato;
            label20.Visible = dato;
            label22.Visible = dato;
            // label26.Visible = dato;
            label27.Visible = dato;
        }


        private void toolStripButton1_Click()
        {
            btnScanEPC.BackColor = System.Drawing.Color.White;
            
                btnScanEPC.Enabled = true;
                btnScanEPC.Visible = true;

                string msg = "Conectando lector...";

                StartReceiveThread();

                frmWaitingBox f = new frmWaitingBox((obj, args) =>
                {
                    bool result = false;

                    result = uhf.OpenUsb();
                    UHFAPI.setOnDataReceived(onDataReceived);

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
                            enableControls();

                        }));
                        Thread.Sleep(2000);


                    }
                    else
                    {
                        frmWaitingBox.message = "Error al conectar el lector";
                        Thread.Sleep(2000);
                    }
                }, msg);
                f.BackColor = System.Drawing.Color.Gray;
                f.ShowDialog(this);
                // Thread.Sleep(5000);
                button1.BackColor = System.Drawing.Color.GreenYellow;
                button1.Text = strClose;
                btnScanEPC.Visible = true;
                btnScanEPC.BackColor = System.Drawing.Color.Green;
                btnScanEPC.Text = strStart;
        }







        #endregion


        #region RS160 KeyDwon

        public delegate void KeyDownEventHandler(int keyCode);
        public static event KeyDownEventHandler keyDownEventHandler = null;

        public delegate void KeyUpEventHandler(int keyCode);
        public static event KeyUpEventHandler keyUpEventHandler = null;

        private UHFAPP.UHFAPI.OnDataReceived onDataReceived = DataReceived;
        private int suma;

        private static void DataReceived(IntPtr pdata, short len)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " DataReceived begin");
            short contentLen;
            short index = 0;
            byte type;

            byte[] cellData = new byte[len];
            Marshal.Copy(pdata, cellData, 0, len);

            byte[] pcontent;

            byte[] temp = null;
            string str;
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
                switch (type)
                {
                    case CELL_KEY_CODE:
                        if (pcontent[0] == 03)
                        {
                            int keyCode = pcontent[0];
                            if (keyDownEventHandler != null)
                            {
                                keyDownEventHandler(keyCode);
                            }
                        }
                        else if (pcontent[0] == 04)
                        {
                            int keyCode = pcontent[0];
                            if (keyUpEventHandler != null)
                            {
                                keyUpEventHandler(keyCode);
                            }
                        }
                        break;
                    default:
                        break;
                }
                index += contentLen;
            }
        }

        #endregion 


        #region Search

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }







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



        public void enableControls()
        {
            MenuItemScanEPC.Enabled = true;
            configToolStripMenuItem.Enabled = true;



        }
        public void disableControls(bool isInventory)
        {
            MenuItemScanEPC.Enabled = false;
            configToolStripMenuItem.Enabled = false;



        }
        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)
        {

        }



        private void setComPort()
        {

        }

        public Form ShowForm(Form nextForm, bool isCache)
        {
            isCache = false;

            ///toolStripStatusLabel1.Visible = false;
            Form currForm = this.ActiveMdiChild;
            Form from = nextForm;
            if (currForm != null)
            {
                if (currForm.Name == from.Name)
                {
                    return null;
                }

                if (currForm.Name != "ReadEPCForm")

                {
                    currForm.Close();
                }
                else
                {
                    currForm.Hide();
                }
            }

            from.WindowState = FormWindowState.Maximized;
            from.MdiParent = this;
            from.AutoScaleMode = AutoScaleMode.Inherit;


            from.Show();
            return from;
        }

        private void LoadUI()
        {

        }

        #endregion

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void StopEPC(bool isStop)
        {
            bool reuslt = uhf.StopInventory();

            if (!reuslt)
            {
                MessageBox.Show("Stop fail");
            }

            btnScanEPC.Text = strStart;
            btnScanEPC.BackColor = System.Drawing.Color.Green;
            mainform.enableControls();
            // Thread.Sleep(100);
        }

        private void Time() { }
        private void StartReceiveThread()
        {
            if (!isRuning)
            {
                isRuning = true;
                try
                {
                    new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }
        private void StopReceiveThread()
        {
            isVisible = false;
            isRuning = false;
            Thread.Sleep(100);
        }
        private void UpdataEPC(string epc, string tid, string rssi, string count, string ant, string user)
        {

            if (epc == null)
            {
                return;
            }
            // label6.Text = (tempCount += int.Parse(count)).ToString();

            bool[] exist = new bool[1];
            int index = CheckUtils.getInsertIndex(epcList, epc, tid, exist);

            total++;

            if (exist[0])
            {
                epcList[index].AddAntennaInfoByAnt(int.Parse(ant), rssi);

                List<AntennaInfo> list = epcList[index].AntList;
                StringBuilder stringBuilderANT = new StringBuilder();
                StringBuilder stringBuilderRSSI = new StringBuilder();
                for (int k = 0; k < list.Count; k++)
                {
                    stringBuilderANT.Append("ANT");
                    stringBuilderANT.Append(list[k].AntennaPort);
                    stringBuilderANT.Append(": ");
                    stringBuilderANT.Append(list[k].Count);
                    stringBuilderRSSI.Append(list[k].Rssi);
                    if (k != list.Count - 1)
                    {
                        stringBuilderANT.Append(System.Environment.NewLine);
                        stringBuilderRSSI.Append(System.Environment.NewLine);
                    }
                }
                epcList[index].Count = epcList[index].Count + 1;
                epcList[index].User = user;
                epcList[index].Tid = tid;
                epcList[index].TidBytes = DataConvert.HexStringToByteArray(tid);

               

            }
            else
            {
                EpcInfo epcInfo = new EpcInfo(epc, tid, int.Parse(count), DataConvert.HexStringToByteArray(epc), DataConvert.HexStringToByteArray(tid), int.Parse(ant), rssi, user);
                epcList.Insert(index, epcInfo);



                label10.Text = total.ToString();
                label23.Text = epcList.Count.ToString();
                totalesLeidos += total;
                totalesValidos += epcList.Count;
                label22.Text = "" + totalesLeidos;
                label4.Text = "" + totalesValidos;
                //if (total >= Int32.Parse(label7.Text))
                if (Int32.Parse(label23.Text) >= cantidadPorLote)
                {
                    panel5.Visible = true;
                    total = 0;
                    StopEPC(true);
                    epcList.Clear();
                }

            }
           

        }

        private void leerOrden()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();

            var url = "https://webservice.mozo-grau.com:7048/BC200_MG_DESARROLLO/api/mozo/apiArco/v2.0/companies(0c0574f1-f098-ed11-965c-6045bd89ef6b)/linOrdenesProdArco?$filter=prodOrderNo eq '" + textBox1.Text + "'";

            var request = (HttpWebRequest)WebRequest.Create(url);
            string username = "WEBSERVICE";
            string password = "BLZNChVZ4LAab/kewV3J0v1J/2rYZSwuDUcyyOWEkbY=";
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
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            string responseBodyFormater = responseBody.Replace("@odata.context", "dataContext");
                            responseBodyFormater = responseBodyFormater.Replace("@odata.etag", "odataETag");
                            getMozo = JsonConvert.DeserializeObject<GetMozo>(responseBodyFormater);
                            label5.Text = getMozo.Value[0].ItemNo;
                            label6.Text = getMozo.Value[0].Description;
                            label7.Text = getMozo.Value[0].RemainingQtyBase.ToString();
                            label2.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orden no existe"); // Handle error
                textBox1.Text = "";
            }
        }

        private void leerOrdenAmipem()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();

            var url = "http://localhost:8080/leerOrden/"+textBox1.Text;

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
                            label5.Text = orden.item;
                            label6.Text = orden.descripcion;
                            label7.Text = ""+orden.cantidad;
                            label11.Text ="Lote actual:"+loteActual;
                            label2.Visible = true;
                            idOrdenAmipem = orden.id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orden no existe"); // Handle error
                textBox1.Text = "";
            }
        }

        private void grabarOrdenAmipem()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();

            var url = "http://localhost:8080/grabarOrden/";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                Orden orden = new Orden(idOrdenAmipem, textBox1.Text, label6.Text, label5.Text, Int32.Parse(textBox3.Text), Int32.Parse(label7.Text));
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
                    cantidadPorLote = Int32.Parse(label7.Text) / Int32.Parse(textBox3.Text);
                    label25.Text = "LOTES: " + ordenRetorno.lotes+" ("+cantidadPorLote + ")";
                    loteActual = 1;
                }
            }
            catch (Exception e) { }



           /*try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            label5.Text = orden.item;
                            label6.Text = orden.descripcion;
                            label7.Text = "" + orden.cantidad;
                            label2.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Orden no existe"); // Handle error
                textBox1.Text = "";
            }*/
        }

        private void ScanEPCForm_Load(object sender, EventArgs e)
        {
            setTextCallback = new SetTextCallback(UpdataEPC);

            LoadUI();

        }



        private void ScanEPCForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (btnScanEPC.Text == strStop)
            {
                StopEPC(true);
            }
            StopReceiveThread();
        }

        private void ReadEPC()
        {

            try
            {
                while (isRuning)
                {

                    if (!isVisible)
                    {
                        Thread.Sleep(10);
                    }
                    UHFTAGInfo info = uhf.ReadTagFromBuffer();

                    if (info != null)
                    {
                        this.BeginInvoke(setTextCallback, new object[] { info.Epc, info.Tid, info.Rssi, "1", info.Ant, info.User });

                    }
                    else
                    {
                        this.BeginInvoke(setTextCallback, new object[] { null, null, null, null, null, null });

                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (btnScanEPC.Text == strStop)
            {
                StopEPC(true);
                btnScanEPC.Text = strStart;
                btnScanEPC.BackColor = System.Drawing.Color.Green;

            }
            else
            {
                btnScanEPC.BackColor = System.Drawing.Color.Red;
                btnScanEPC.Text = strStop;
                mainform.disableControls(true);
                panel5.Visible = false;
                loteActual++;
                label11.Text = "Lote actual:" + loteActual;

                if (uhf.StartInventory())
                {

                    btnScanEPC.Text = strStop;

                    new Thread(new ThreadStart(delegate { Time(); })).Start();
                }
                else
                {
                    MessageBoxEx.Show(this, "Inventory failure!");
                    mainform.enableControls();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //leerOrden();
            leerOrdenAmipem();
        }

        

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("uno\n");
            sb.Append("dos\n");
            sb.Append("tres\n");
            sb.Append("cuatro\n");
            sb.Append("cinco\n");
            sb.Append("seis\n");

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            grabarOrdenAmipem();
        }
    }
}