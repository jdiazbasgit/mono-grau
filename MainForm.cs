using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BLEDeviceAPI;
//using UHFAPP.barcode;
using UHFAPP.Entity;
using UHFAPP.RFID;
using WinForm_Test;
using static UHFAPP.UHFAPI;

namespace UHFAPP
{
    public partial class MainForm : BaseForm
    {
        public static int MODE = 1;//0:串口   1:网口    2:usb
        public static string ip = "";
        public static uint portData = 0;

        public delegate void DelegateOpen(bool open);
        public static event DelegateOpen eventOpen = null;

        public delegate void DelegateSwitchUI();
        public static event DelegateSwitchUI eventSwitchUI = null;

        public delegate void MainSizeChanged(FormWindowState state);
        public static event MainSizeChanged eventMainSizeChanged = null;

        string strOpen = "  Conectar lector  ";
        string strClose = "  Desconectar lector  ";

        private string currentFormName = "";
        private bool isOpen = false;
        public MainForm mainform = null;
        private ReadEPCForm readEPCForm = null;
        public static FormWindowState currState = FormWindowState.Normal;

        public bool isSearch = false;
        List<ReaderDeviceInfo> listIP = new List<ReaderDeviceInfo>();
        #region  OnDisconnect
        //step1：定义断开回调函数
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MenuItemScanEPC_Click(null, null);
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
           /* Form form = ShowForm(new ReadWriteTagForm(), true);
            if (form != null)
            {
                if (form is ReadWriteTagForm)
                {
                    ((ReadWriteTagForm)form).SetTAG(isOpen, tag, bank);
                }
            }*/

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
        private void MenuItemScanEPC_Click(object sender, EventArgs e)
        {
            if (readEPCForm == null)
            {
                readEPCForm = new ReadEPCForm(isOpen, mainform);
            }
           // Form form = ShowForm(readEPCForm, false);
        }
        /// <summary>
        /// Config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = ShowForm(new ConfigForm(isOpen), false);
        }
        /// <summary>
        /// kill
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (button1.Text == strOpen)
            {
               // int type = combCommunicationMode.SelectedIndex;//0
                string msg =  "Conectando lector..." ;

                
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
                        frmWaitingBox.message = "Error al desconectar el lector";
                        Thread.Sleep(2000);
                    }
                }, msg);
                f.ShowDialog(this);
                 button1.Text = strClose;
                button1.BackColor = System.Drawing.Color.Green;

            }
            else
            {
                if (UHFClose())
                {
                    disableControls(false);
                    button1.Text = strOpen;
                    button1.BackColor = System.Drawing.Color.Red;
                    isOpen = false;
                    if (eventOpen != null)
                    {
                        eventOpen(false);
                    }
                }
            }

        }


        


        

        #endregion


        #region RS160 KeyDwon

        public delegate void KeyDownEventHandler(int keyCode);
        public static event KeyDownEventHandler keyDownEventHandler = null;

        public delegate void KeyUpEventHandler(int keyCode);
        public static event KeyUpEventHandler keyUpEventHandler = null;

        private UHFAPP.UHFAPI.OnDataReceived onDataReceived = DataReceived;
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
                        //  printf("unknow parameter:%d\n", type);
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
           // panel2.Height = this.Height - 128;
            //判断是否选择的是最小化按钮
            if (eventMainSizeChanged != null)
            {
                eventMainSizeChanged(WindowState);
            }
            // this.MdiParent = mainform;
            //  Form form = ShowForm(new ConfigForm(isOpen), false);
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
            if (e.Item.Text.Length == 0             //隐藏子窗体图标
              || e.Item.Text == "最小化(&N)"      //隐藏最小化按钮
              || e.Item.Text == "还原(&R)"           //隐藏还原按钮
              || e.Item.Text == "关闭(&C)")         //隐藏关闭按钮
            {
                e.Item.Visible = false;
            }
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
            from.MdiParent = this;
            from.AutoScaleMode = AutoScaleMode.Inherit;
           /* if (from.Name != "ReadEPCForm")
            {
                from.Left = 0;
            }
            else
            {
                if (from.Left != -8)
                {
                    from.Left = 303;
                }
            }*/

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
            toolStripButton1_Click(sender, e);  
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
