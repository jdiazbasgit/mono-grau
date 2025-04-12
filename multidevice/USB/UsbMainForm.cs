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
 

namespace UHFAPP.USB.multidevice
{
    public partial class UsbMainForm : Form
    {

        private bool isRuning = false;
        private delegate void SetTextCallback(string epc, float rssi, int count, int ant, string ip);
        private SetTextCallback setTextCallback;
        private List<EpcInfo> epcList = new List<EpcInfo>();
        bool FlagInventory1 = false;
        bool FlagInventory2 = false;
 
        RFIDAPIManage rfidAPIManage = RFIDAPIManage.GetInstance;
        public UsbMainForm()
        {
            InitializeComponent();
            setTextCallback = new SetTextCallback(UpdataEPC);
        }


        /// <summary>
        /// 断开回调
        /// </summary>
        /// <param name="ip"></param>
        public void DisconnectEventHandler(string chipId)
        {
            this.Invoke(new EventHandler(delegate
            {
                //btnDisConn_Click(null, null);
                textBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + chipId + " disconnect!");
                textBox1.AppendText("\r\n");

                GetConnectionstatus();
            }));

          
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
        /// <summary>
        ///  连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConn_Click(object sender, EventArgs e)
        {
 
           // btnConn.Enabled = false;
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
         
                bool result = rfidAPIManage.Connect();
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
                    frmWaitingBox.message = "success";
                    rfidAPIManage.DisconnectEvent -= DisconnectEventHandler;
                    rfidAPIManage.DisconnectEvent += DisconnectEventHandler;
                    this.Invoke(new EventHandler(delegate
                    {
                        btnStart1.Enabled = true;
                        btnDisConn.Enabled = true;
                        GetConnectionstatus();
                    }));
               
                }

            }, "connecting...");
            f.ShowDialog(this);

        }
        /// <summary>
        ///  断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConn_Click(object sender, EventArgs e)
        {
            List<string> list= rfidAPIManage.GetConnectedDevice();
            DeviceListForm deviceList=new DeviceListForm(list);
            deviceList.ShowDialog();
            if (deviceList.ChipId != null && deviceList.ChipId.Length>0)
            {
                rfidAPIManage.Disconnect(deviceList.ChipId);

            }
        }
  
     



        /// <summary>
        /// IP1开始盘点、停止盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart1_Click(object sender, EventArgs e)
        {
            List<string> list = rfidAPIManage.GetConnectedDevice();  
            DeviceListForm deviceList = new DeviceListForm(list);
            deviceList.ShowDialog();
            if (deviceList.ChipId != null && deviceList.ChipId.Length > 0)
            {
                RFIDAPI rfidAPI = rfidAPIManage.GetDeviceByChipId(deviceList.ChipId);
                bool result = rfidAPI.StartInventory();
                if (result)
                {
                    rfidAPI.InventoryTagEvent -= InventoryTagEventHandler;
                    rfidAPI.InventoryTagEvent += InventoryTagEventHandler;
                    return;
                }
                MessageBox.Show("Failure!");
            }

        }
        /// <summary>
        /// 停止盘点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            List<string> list = rfidAPIManage.GetConnectedDevice();
            DeviceListForm deviceList = new DeviceListForm(list);
            deviceList.ShowDialog();
            if (deviceList.ChipId != null && deviceList.ChipId.Length > 0)
            {
                RFIDAPI rfidAPI = rfidAPIManage.GetDeviceByChipId(deviceList.ChipId);
                rfidAPI.StopInventory();
            }
        }


        /// <summary>
        /// 获取功率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerGet_Click(object sender, EventArgs e)
        {
            List<string> list = rfidAPIManage.GetConnectedDevice();
            DeviceListForm deviceList = new DeviceListForm(list);
            deviceList.ShowDialog(); 
            if (deviceList.ChipId != null && deviceList.ChipId.Length > 0)
            {
                RFIDAPI rfidAPI = rfidAPIManage.GetDeviceByChipId(deviceList.ChipId);
                if (!rfidAPI.IsConnected())
                {
                    showMessage("Failure!");
                    return;
                }
                int power = rfidAPI.GetAntennaPower();
                if (power == -1)
                {
                    showMessage("Failure!");
                    return;
                }

                cmbPowerAnt1.SelectedIndex = power - 1;
            }
            else
            {
                showMessage("Failure!");
            }




        
        }
        /// <summary>
        /// 设置功率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerSet_Click(object sender, EventArgs e)
        {
       
            List<string> list = rfidAPIManage.GetConnectedDevice();
            DeviceListForm deviceList = new DeviceListForm(list);
            deviceList.ShowDialog();
            if (deviceList.ChipId != null && deviceList.ChipId.Length > 0)
            {
                RFIDAPI rfidAPI = rfidAPIManage.GetDeviceByChipId(deviceList.ChipId);
                if (!rfidAPI.IsConnected())
                {
                    showMessage("Failure!");
                    return;
                }
                byte power = (byte)(cmbPowerAnt1.SelectedIndex + 1);
                bool result = rfidAPI.SetAntennaPower(power);
                if (!result)
                {
                    showMessage("Failure!");
                    return;
                }
                showMessage("Success!");
            }
            else
            {
                showMessage("Failure!");
            }




           
        }

        private void showMessage(string msg)
        {
            if (msg.Contains("失败") || msg.ToLower().Contains("fail"))
            {
                frmWaitingBox f = new frmWaitingBox((obj, args) =>
                {
                    System.Threading.Thread.Sleep(500);
                }, msg);
                f.ShowDialog(this);
            }
        }
       
        public void InventoryTagEventHandler(string chipId, InventoryTagEventArgs eventArgs) {
          
            if (eventArgs != null && eventArgs.UHFTagInfo != null)
            {
                UHFAPP.USB.multidevice.UHFTAGInfo info = eventArgs.UHFTagInfo; 
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
                    this.BeginInvoke(setTextCallback, new object[] { data, info.Rssi, info.Count, info.Ant, chipId });
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
                lvEPC.Items[index].SubItems["ChipId"].Text = ip;
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
                itemIP.Name = "ChipId";
                itemIP.Text = ip;
                lv.SubItems.Add(itemIP);

          
                lvEPC.Items.Insert(index, lv);// Add(lv);
                epcList.Insert(index, new EpcInfo(epc,null, count, DataConvert.HexStringToByteArray(epc), null));
            }
            lblCount.Text = (int.Parse(lblCount.Text) + 1).ToString();
 
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rfidAPIManage.DisconnectEvent -= DisconnectEventHandler;
            isRuning = false; 
            rfidAPIManage.DisconnectAll();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            epcList.Clear();
            lvEPC.Items.Clear();
            lblCount.Text = "0";
        }

        private void GetConnectionstatus()
        {
            List<string> list = rfidAPIManage.GetAllDevice();

            textBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " connected device:" + list.Count);
            textBox1.AppendText("\r\n");
            foreach (string s in list)
            {
                textBox1.AppendText(s);
                textBox1.AppendText("\r\n");
            }
        }

     
    }
}
