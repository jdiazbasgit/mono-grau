using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using BLEDeviceAPI;
using UHFAPP.utils;

namespace UHFAPP
{
    public partial class ReceiveEPC : BaseForm
    {
       private const int max =1024 * 1024;
       private byte[] uhfOriginalData = new byte[max];
 
       private bool isRuning = true;
       private bool isOpen = false;
    

       int total = 0;
       long beginTime = System.Environment.TickCount;

       List<EpcInfo> epcList = new List<EpcInfo>();
 

       // 将text更新的界面控件的委托类型
       delegate void SetTextCallback(string epc, string tid, string rssi, string count, string ant, string user);
       SetTextCallback setTextCallback;



       delegate void GetRemotelyIPCallback(string remoteip);
       GetRemotelyIPCallback RemotelyIPCallback;
        public ReceiveEPC()
        {
            InitializeComponent();
        }
        private void ReceiveEPC_Load(object sender, EventArgs e)
        {
            MainForm.eventMainSizeChanged += MainForm_SizeChanged;
            setTextCallback = new SetTextCallback(UpdataEPC);
            //cmbMode.SelectedIndex = 0;
            RemotelyIPCallback = new GetRemotelyIPCallback(GetRemoteIP);
            InitIPAndSerialPort();
           
        }
        private void ReceiveEPC_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;
            isOpen = false;
            isRuning = false;
            DisConnect();
        }

        private void btnScanEPC_Click(object sender, EventArgs e)
        {
            if (btnScanEPC.Text == "Start")
            {
                if (Connect())
                {
                    isRuning = true;
                    //cmbCom.Enabled = false;
                   // cmbMode.Enabled = false;
                    btnScanEPC.Text = "Stop";
                    new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
                }
                else
                {
                    MessageBox.Show("fail");
                }
            }
            else
            {
                isRuning = false;
               // cmbCom.Enabled = true;
               // cmbMode.Enabled = true;
                btnScanEPC.Text = "Start";
                DisConnect();
            }
        }

    
        private void InitIPAndSerialPort()
        {
            string[] ArryPort = System.IO.Ports.SerialPort.GetPortNames();
           
           
        }

        private bool Connect()
        {
            // int ComPort = int.Parse(cmbCom.SelectedItem.ToString().ToString().Replace("COM", ""));
            return true;
        

        }

        private void DisConnect()
        {
            
                UHFAPI.getInstance().Close();
                isOpen = false;
           
        }


      

        //获取epc
        private void ReadEPC()
        {
            try
            {
                beginTime = System.Environment.TickCount;
               
                while (true)
                {
                    UHFTAGInfo info = uhf.ReadTagFromBuffer();
                    if (info != null)
                    {
                        this.BeginInvoke(setTextCallback, new object[] { info.Epc, info.Tid, info.Rssi, "1", info.Ant, info.User });
                    }
                    else
                    {
                        if (isRuning)
                        {
                            Thread.Sleep(5);
                        }
                        else
                        {
                            break;
                        }
                    }

                 

                }

              /*  lblTime.Invoke(new EventHandler(delegate
                {
                    lblTime.Text = ((System.Environment.TickCount - beginTime) / 1000) + "(s)";

                }));*/

            }
            catch (Exception ex)
            {

            }
           

        }



 
        private void UpdataEPC(string epc, string tid, string rssi, string count, string ant, string user)
        {
           //label1.Text = epc;
        }

        private void GetRemoteIP(string ip)
        {
           // textBox1.Text = ip;
        }

       

        private void MainForm_SizeChanged(FormWindowState state)
        {
            
        }
    }
}
