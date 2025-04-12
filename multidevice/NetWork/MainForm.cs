using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinForm_Test;
using System.Threading;
using BLEDeviceAPI;
using UHFAPP.utils;
using UHFAPP.MultiDevice;
using UHFAPP.USB.multidevice;
using WindowsFormsControlLibrary1;
using static UHFAPP.IPConfig;

namespace UHFAPP.MultiDevice.NetWork.API
{
    public partial class MainForm : Form
    {

        private bool isRuning = false;
        private delegate void SetTextCallback(string epc, float rssi, int count, int ant, string ip);
        private SetTextCallback setTextCallback;
        private List<EpcInfo> epcList = new List<EpcInfo>();
        bool FlagInventory1 = false;
        bool FlagInventory2 = false;
 
        RFIDAPIManage rfidAPIManage = RFIDAPIManage.GetInstance;
        public MainForm()
        {
            InitializeComponent();
            setTextCallback = new SetTextCallback(UpdataEPC);

            List<IPEntity> list = IPConfig.getIPConfig2();
            if (list != null && list.Count>0)
            {
                txtIP.Text = list[0].StrIp;
                txtPort.Text = list[0].Port + "";

                txtIP2.Text = list[1].StrIp;
                txtPort2.Text = list[1].Port + "";
            }
            rfidAPIManage.InventoryTagEvent -= InventoryTagEventHandler;
            rfidAPIManage.InventoryTagEvent += InventoryTagEventHandler;
        }
        /// <summary>
        /// 断开回调
        /// </summary>
        /// <param name="ip"></param>
        public void DisconnectEventHandler(string ip)
        {
            if (txtIP.IsHandleCreated)
            {
                txtIP.Invoke(new EventHandler(delegate
                {
                    if (txtIP.Text == ip)
                    {
                        btnDisConn_Click(null, null);
                    }
                    else if (txtIP2.Text == ip)
                    {
                        btnDisConn2_Click(null, null);
                    }
                }));

            }


        }


        /// <summary>
        /// IP1连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConn_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            if (ip == txtIP2.Text && btnConn2.Enabled==false)
            {
                showMessage(BaseForm.IsChineseSimple()?"IP重复了!": "The IP is duplicated !");
                return;
            }
            int port = int.Parse(txtPort.Text);
            btnConn.Enabled = false;
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                RFIDAPI rFIDAPI = rfidAPIManage.GetDeviceByIP(ip);
                bool result = rFIDAPI.Connect(port);
                if (!result)
                {
                    frmWaitingBox.message = "fail";
                    Thread.Sleep(1000);
                    this.Invoke(new EventHandler(delegate
                    {
                        btnConn.Enabled = true;
                    }));
                }
                else
                {
                    IPConfig.setIPConfig(txtIP.Text,txtPort.Text,txtIP2.Text,txtPort2.Text);
                    rFIDAPI.DisconnectEvent -= DisconnectEventHandler;
                    rFIDAPI.DisconnectEvent += DisconnectEventHandler;
                    this.Invoke(new EventHandler(delegate
                    {
                        btnStart1.Enabled = true;
                        btnDisConn.Enabled = true;
                    }));

                }

            }, "connecting...");
            f.ShowDialog(this);

        }
        /// <summary>
        /// IP1断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConn_Click(object sender, EventArgs e)
        {
            btnStart1.Text = "Start";
            rfidAPIManage.GetDeviceByIP(txtIP.Text).Disconnect();
            btnDisConn.Enabled = false;
            btnStart1.Enabled = false;
            btnConn.Enabled = true;

        }
        /// <summary>
        /// IP2连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConn2_Click(object sender, EventArgs e)
        {
            string ip = txtIP2.Text;
            if (ip == txtIP.Text && btnConn.Enabled==false)
            {
                showMessage(BaseForm.IsChineseSimple() ? "IP重复了!" : "The IP is duplicated !");
                return;
            }
            int port = int.Parse(txtPort2.Text);
            btnConn2.Enabled = false;

            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                RFIDAPI rFIDAPI = rfidAPIManage.GetDeviceByIP(ip);
                bool result = rFIDAPI.Connect(port);
                if (!result)
                {
                    frmWaitingBox.message = "fail";
                    Thread.Sleep(1000);
                    this.Invoke(new EventHandler(delegate
                    {
                        btnConn2.Enabled = true;
                    }));
                }
                else
                {
                    IPConfig.setIPConfig(txtIP.Text, txtPort.Text, txtIP2.Text, txtPort2.Text);
                    rFIDAPI.DisconnectEvent -= DisconnectEventHandler;
                    rFIDAPI.DisconnectEvent += DisconnectEventHandler;
                    this.Invoke(new EventHandler(delegate
                    {
                        btnDisConn2.Enabled = true;
                        btnStart2.Enabled = true;
                    }));

                }

            }, "connecting...");
            f.ShowDialog(this);
        }
        /// <summary>
        /// IP2断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConn2_Click(object sender, EventArgs e)
        {
            btnStart2.Text = "Start";
            rfidAPIManage.GetDeviceByIP(txtIP2.Text).Disconnect();
            btnConn2.Enabled = true;
            btnDisConn2.Enabled = false;
            btnStart2.Enabled = false;
        }



        /// <summary>
        /// IP1开始盘点、停止盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart1_Click(object sender, EventArgs e)
        {
            if (btnStart1.Text == "Start")
            {
                RFIDAPI rFIDAPI = rfidAPIManage.GetDeviceByIP(txtIP.Text);
                bool result = rFIDAPI.StartInventory();
                if (result)
                {
                    btnStart1.Text = "Stop";
                    return;
                }
                MessageBox.Show("失败!");

            }
            else
            {
                btnStart1.Text = "Start";
                rfidAPIManage.GetDeviceByIP(txtIP.Text).StopInventory();
            }


        }
        /// <summary>
        /// IP2开始盘点、停止盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart2_Click(object sender, EventArgs e)
        {
            if (btnStart2.Text == "Start")
            {
                RFIDAPI rFIDAPI = rfidAPIManage.GetDeviceByIP(txtIP2.Text);
                bool result = rFIDAPI.StartInventory();
                if (result)
                {
                    btnStart2.Text = "Stop";
                    return;
                }
                MessageBox.Show("失败!");
            }
            else
            {
                btnStart2.Text = "Start";
                bool result = rfidAPIManage.GetDeviceByIP(txtIP2.Text).StopInventory();
            }
        }
 

        /// <summary>
        /// 获取功率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerGet_Click(object sender, EventArgs e)
        {
            if (txtDeviceIP.Text == null || txtDeviceIP.Text == "")
            {
                showMessage(BaseForm.IsChineseSimple() ? "请选择设备!" : "Please select device! ");
                return;
            }
            RFIDAPI rFIDAPI = rfidAPIManage.GetDeviceByIP(txtDeviceIP.Text);
            if (!rFIDAPI.IsConnected())
            {
                showMessage(BaseForm.IsChineseSimple() ? "失败!" : "Failure!");
                return;
            }
            List<AntennaPowerEntity> list= rFIDAPI.GetAntennaPower();
            if (list == null)
            {
                showMessage("Failure!");
                return;
            }
     
            for (int k = 0; k < list.Count; k++)
            {
                int Power = list[k].Power;
                if (list[k].Antenna == AntennaEnum.ANT1)
                {
                    cmbPowerAnt1.SelectedIndex = Power - 1;
                }
            }
            showMessage("Success!");
        }
        /// <summary>
        /// 设置功率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerSet_Click(object sender, EventArgs e)
        {
            if (txtDeviceIP.Text == null || txtDeviceIP.Text == "")
            {
                showMessage(BaseForm.IsChineseSimple() ? "请选择设备!" : "Please select device! ");
                return;
            }
            RFIDAPI rFIDAPI = rfidAPIManage.GetDeviceByIP(txtDeviceIP.Text);
            if (!rFIDAPI.IsConnected())
            {
                showMessage("Failure!");
                return;
            }
            byte power1 = (byte)(cmbPowerAnt1.SelectedIndex + 1);
            if (power1 < 1)
            {
                showMessage("Failed to set the power!");
                return;
            }
             
            if (!rFIDAPI.SetAntennaPower(AntennaEnum.ANT1, power1))
            {
                showMessage("Failed to set the power!");
                return;

            }

            showMessage("Success!");
        }

        private void showMessage(string msg)
        {
            //if (msg.Contains("失败") || msg.ToLower().Contains("fail"))
            {
                frmWaitingBox f = new frmWaitingBox((obj, args) =>
                {
                    System.Threading.Thread.Sleep(500);
                }, msg);
                f.ShowDialog(this);
            }
        }
       
        public void InventoryTagEventHandler(string ip, InventoryTagEventArgs eventArgs) {
          
            if (eventArgs != null && eventArgs.UHFTagInfo != null)
            {
                UHFAPP.MultiDevice.NetWork.API.UHFTAGInfo info = eventArgs.UHFTagInfo; 
                string data = info.Epc;
                if (info.Tid != null && info.Tid.Length > 0)
                {
                    data = "EPC:" + data;
                    data = data + "\r\nTID:" + info.Tid;
                }
                if (info.User != null && info.User.Length > 0)
                {
                    data = data + "\r\nUSER:" + info.User;
                }
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(setTextCallback, new object[] { data, info.Rssi, info.Count, info.Ant, ip });
                }
            } 
        }
        private void UpdataEPC(string epc, float rssi, int count, int ant, string ip)
        {
            bool[] exist = new bool[1];
            int index = CheckUtils.getInsertIndex(epcList, epc,null, exist);
            if (exist[0])
            {
                lvEPC.Items[index].SubItems["RSSI"].Text = rssi.ToString();
                lvEPC.Items[index].SubItems["COUNT"].Text = (int.Parse(lvEPC.Items[index].SubItems["COUNT"].Text) + count).ToString();
                lvEPC.Items[index].SubItems["ANT"].Text = ant.ToString();
                lvEPC.Items[index].SubItems["IP"].Text = ip;
            }
            else
            {
                ListViewItem lv = new ListViewItem();
                lv.Text = (index + 1).ToString();
                ListViewItem.ListViewSubItem itemEPC = new ListViewItem.ListViewSubItem();
                itemEPC.Name = "EPC";
                itemEPC.Text = epc;
                lv.SubItems.Add(itemEPC);

                ListViewItem.ListViewSubItem itemRssi = new ListViewItem.ListViewSubItem();
                itemRssi.Name = "RSSI";
                itemRssi.Text = rssi.ToString();
                lv.SubItems.Add(itemRssi);

                ListViewItem.ListViewSubItem itemCount = new ListViewItem.ListViewSubItem();
                itemCount.Name = "COUNT";
                itemCount.Text = count.ToString();
                lv.SubItems.Add(itemCount);

                ListViewItem.ListViewSubItem itemAnt = new ListViewItem.ListViewSubItem();
                itemAnt.Name = "ANT";
                itemAnt.Text = ant.ToString();
                lv.SubItems.Add(itemAnt);

                ListViewItem.ListViewSubItem itemIP = new ListViewItem.ListViewSubItem();
                itemIP.Name = "IP";
                itemIP.Text = ip;
                lv.SubItems.Add(itemIP);

          
                lvEPC.Items.Insert(index, lv);// Add(lv);
                epcList.Insert(index, new EpcInfo(epc,null, count, DataConvert.HexStringToByteArray(epc), null));
            }
            lblCount.Text = (int.Parse(lblCount.Text) + 1).ToString();
 
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

          
            Dictionary<string, RFIDAPI> dictionary= rfidAPIManage.GetAllDevice();

            foreach (KeyValuePair<string, RFIDAPI> kv in dictionary)
            {
                kv.Value.DisconnectEvent -= DisconnectEventHandler;
                kv.Value.Disconnect();
            }
            rfidAPIManage.InventoryTagEvent -= InventoryTagEventHandler;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            epcList.Clear();
            lvEPC.Items.Clear();
            lblCount.Text = "0";
        }

       
    }
}
