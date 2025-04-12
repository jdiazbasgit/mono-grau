using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using BLEDeviceAPI;
using UHFAPP.utils;
using UHFAPP.excel;
using WinForm_Test;
using System.IO;
using System.Xml.Linq;
using static UHFAPP.utils.EpcInfo;
using System.Security.Cryptography;
using System.Reflection;
using System.Diagnostics;
using static System.Windows.Forms.AxHost;
using System.Runtime.InteropServices;
using static UHFAPP.UHFAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
///using UHFAPP.barcode;

namespace UHFAPP
{
    public partial class ReadEPCForm : BaseForm
    {
        bool isPz = false;
        MainForm mainform;
        string strStart = "Start";
        string strStart2 = "开始";
        string strStop = "Stop";
        string strStop2 = "停止";
        bool isRuning = false;
        public static bool isVisible = false;
        int beginTime = 0;
        int workTime = 0;
        int total = 0;
        
        static bool isUIFast = false;
        List<EpcInfo> epcList = new List<EpcInfo>();
        // 将text更新的界面控件的委托类型
        delegate void SetTextCallback(string epc, string tid, string rssi, string count, string ant, string user);
        SetTextCallback setTextCallback;


       
       
        public ReadEPCForm(bool isOpen, MainForm mainform)
        {
            //设置窗体的双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();
            //利用反射设置DataGridView的双缓冲
            Type dgvType = this.dgData.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dgData, true, null);

            if (isOpen)
            {
                panel1.Enabled = true;
            }
            else {
                panel1.Enabled = false;
            }
            this.mainform = mainform;

        }

        void MainForm_eventOpen(bool open)
        {
            if (open)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
                if (btnScanEPC.Text == strStop)
                {
                    StopEPC(true);
                }
            }
        }

        private void LoadDataGridView()
        {
            //设置自动换行  
            dgData.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            if (isUIFast)
            {
                dgData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            }
            else
            {
                dgData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //  DisplayedCells
                dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }


            foreach (DataGridViewColumn col in dgData.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("宋体", 18F, FontStyle.Regular, GraphicsUnit.Pixel);
            }

            this.dgData.Columns[0].FillWeight = 3;
            this.dgData.Columns[1].FillWeight = 25;//EPC
            this.dgData.Columns[2].FillWeight = 22;//TID
            this.dgData.Columns[3].FillWeight = 20;//USER
            this.dgData.Columns[4].FillWeight = 8;//RSSI 
            this.dgData.Columns[5].FillWeight = 6;//COUNT
            this.dgData.Columns[6].FillWeight = 8;//ANT 
            //禁止排序
            foreach (DataGridViewColumn dgvc in dgData.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }
        private void AutoCellsWidth(bool auto)
        {
            if (!isUIFast)
            {
                if (auto)
                {
                    if (dgData.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
                    {
                        dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dgData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        // dgData.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                }
                else
                {

                    if (dgData.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.Fill)
                    {
                        dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                    }

                }
            }


        }
        private void ReadEPCForm_Activated(object sender, EventArgs e)
        {
            if (MainForm.currState == FormWindowState.Normal)
            {
                panel1.Size = new Size(1139 - 50, 673 - 60);
                dgData.Size = new Size(1097, 390);
                panel2.Location = new Point(3, 482);
            }
            else if (MainForm.currState == FormWindowState.Maximized)
            {
                panel1.Size = new Size(Size.Width - 350, Size.Height - 50);
                dgData.Size = new Size(Size.Width - 350, Size.Height - 280);
                panel2.Location = new Point(panel2.Location.X, Size.Height - 180);
            }

            if (!first) panel1.Left = 310;// 380;
        }

        private void ScanEPCForm_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            MainForm.eventOpen += MainForm_eventOpen;
            setTextCallback = new SetTextCallback(UpdataEPC);
            MainForm.eventMainSizeChanged += MainForm_SizeChanged;
            //cbUIFast.Checked = isUIFast;
 
            //private void UpdataEPC(string epc, string tid, string rssi, string count,string ant)
            // UpdataEPC("112233","","","1","1");
            //sUpdataEPC("445566778899", "", "", "1", "1");
            LoadUI();

            MainForm.keyDownEventHandler -= KeyDownEventHandler;
            MainForm.keyDownEventHandler += KeyDownEventHandler;

            MainForm.keyUpEventHandler -= KeyUpEventHandler;
            MainForm.keyUpEventHandler += KeyUpEventHandler;

        }



        private void ScanEPCForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventOpen -= MainForm_eventOpen;
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;
            if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
            {
                StopEPC(true);
            }
            StopReceiveThread();
        }
       
      

       
 
      
        private void StopEPC(bool isStop) {
            bool reuslt = uhf.StopInventory();
            workTime = 0;
            if (!reuslt)
            {
                MessageBox.Show(!IsChineseSimple() ? "Stop fail" : "停止失败");
            }
 
            //groupBox8.Enabled = true;
           // cmbFormat.Enabled = true;
           // cbUIFast.Enabled = true;
            btnScanEPC.Text = !IsChineseSimple() ? strStart : strStart2;
            mainform.enableControls();
            Thread.Sleep(100);
        }

        private void StartReceiveThread()
        {
            if (!isRuning)
            {
                isRuning = true;
                new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
            }
        }
        private void StopReceiveThread()
        {
            isRuning = false;
            Thread.Sleep(100);
        }
        private void Time()
        {
           
        }
      
        private void ReadEPC()
        {
            Console.WriteLine("ReadEPC begin.");
            try
            {
                while (isRuning) 
                {
                    if (!isVisible)
                    {
                        Thread.Sleep(10);
                       // Console.WriteLine("isVisible false");
                        continue;
                    }
                    UHFTAGInfo info = uhf.ReadTagFromBuffer();

                    if (info != null)
                    {
                        this.BeginInvoke(setTextCallback, new object[] { info.Epc, info.Tid, info.Rssi, "1", info.Ant, info.User });
                    }
                    else
                    {
                        this.BeginInvoke(setTextCallback, new object[] { null, null, null, null, null, null });
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            Console.WriteLine("ReadEPC end.");
        }

        int tempCount = 0;
        StringBuilder sb = new StringBuilder(100);
        private void UpdataEPC(string epc, string tid, string rssi, string count, string ant, string user)
        {
 
            if (epc == null)
            {
                return;
            }
            //label6.Text = (tempCount += int.Parse(count)).ToString();

            bool[] exist = new bool[1];
            int index = CheckUtils.getInsertIndex(epcList, epc, tid, exist);


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
                    // stringBuilderRSSI.Append("RSSI:");
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

              
                this.dgData.Rows[index].Cells[1].Value = epc;
                this.dgData.Rows[index].Cells[2].Value = tid;
                this.dgData.Rows[index].Cells[3].Value = user;
                this.dgData.Rows[index].Cells[4].Value = stringBuilderRSSI.ToString();
                this.dgData.Rows[index].Cells[5].Value = epcList[index].Count;
                this.dgData.Rows[index].Cells[6].Value = stringBuilderANT.ToString();

            }
            else {
                EpcInfo epcInfo = new EpcInfo(epc, tid, int.Parse(count), DataConvert.HexStringToByteArray(epc), DataConvert.HexStringToByteArray(tid), int.Parse(ant), rssi, user);
                epcList.Insert(index, epcInfo);

                total++;
          

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("ANT");
                stringBuilder.Append(ant);
                stringBuilder.Append(": 1");

                object[] values = new object[] { (index + 1), epc, tid, user, rssi, 1, stringBuilder.ToString() };
                dgData.Rows.Insert(index, values);

               // lblTotal.Text = (dgData.RowCount - 1).ToString();

            }
            if (epc.Length > 40 || (user != null && user.Length > 40))
            {
                AutoCellsWidth(true);
            }

        }


       



        private void dgData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
            {
                StopEPC(true);
            }

            int Col_index = dgData.CurrentCell.ColumnIndex;
            if (Col_index == 1 || Col_index == 2 || Col_index == 3)
            {
                int Row_index = dgData.CurrentRow.Index;
                EpcInfo epcinfo = epcList[Row_index];
                string data = "";
                if (Col_index == 1)
                {
                    data = epcinfo.Epc;
                }
                else if (Col_index == 2)
                {
                    data = epcinfo.Tid;
                }
                else if (Col_index == 3)
                {
                    data = epcinfo.User;
                }

                if (!string.IsNullOrEmpty(data))
                {
                    mainform.ReadWriteTag(data, Col_index);
                    Common.tag = data;
                    Common.bank = Col_index;
                }
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {

            int Col_index = dgData.CurrentCell.ColumnIndex;
            if (Col_index == 1 || Col_index == 2 || Col_index == 3)
            {
                int Row_index = dgData.CurrentRow.Index;
                if (epcList.Count < Row_index + 1) {
                    return;
                }
                EpcInfo epcinfo = epcList[Row_index];
                string data = "";
                if (Col_index == 1)
                {
                    data = epcinfo.Epc;
                }
                else if (Col_index == 2)
                {
                    data = epcinfo.Tid;
                }
                else if (Col_index == 3)
                {
                    data = epcinfo.User;
                }

                if (!string.IsNullOrEmpty(data))
                {

                    Clipboard.SetDataObject(data);
                }
            }

        }

       
      
   

       

      

        static bool first = true;
        private void MainForm_SizeChanged(FormWindowState state)
        {

            //判断是否选择的是最小化按钮
            if (!first)
            {
                panel1.Left = 308;
            }
            first = false;
            // this.WindowState = FormWindowState.Maximized ;
            if (state == FormWindowState.Normal)
            {
                panel1.Size = new Size(1139 - 50, 673 - 60);
                dgData.Size = new Size(1097, 390);
                panel2.Location = new Point(3, 482);
            }
            else if (state == FormWindowState.Maximized)
            {
                panel1.Size = new Size(Size.Width - 350, Size.Height - 50);
                dgData.Size = new Size(Size.Width - 350, Size.Height - 280);
                panel2.Location = new Point(panel2.Location.X, Size.Height - 180);
            }


        }

        private void LoadUI()
        {

           

        }

       


        public void KeyDownEventHandler(int keyCode)
        {
            Console.WriteLine("KeyDownEventHandler");
        }

        public void KeyUpEventHandler(int keyCode)
        {
            Console.WriteLine("KeyUpEventHandler");
        }

        private void ReadEPCForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((ReadEPCForm)sender).Visible)
            {
                isVisible = true;
            }
            else
            {
              //  isVisible =false;
            }
              
        }

        private void btnScanEPC_Click(object sender, EventArgs e)
        {
            if (btnScanEPC.Text == strStop || btnScanEPC.Text == strStop2)
            {
                StopEPC(true);
            }
            else
            {
                mainform.disableControls(true);
                if (uhf.StartInventory())
                {
                   //cbUIFast.Enabled = false;
                    //groupBox8.Enabled = false;
                   // cmbFormat.Enabled = false;
                    btnScanEPC.Text = !IsChineseSimple() ? strStop : strStop2;
                    StartReceiveThread();

                    //+++++++++++++++++
                    beginTime = System.Environment.TickCount;
                    workTime = 0;
                   
                    if (workTime == 0)
                    {
                        workTime = int.MaxValue;
                    }
                    new Thread(new ThreadStart(delegate { Time(); })).Start();
                    //+++++++++++++++++
                }
                else
                {
                    MessageBoxEx.Show(this, "Inventory failure!");
                    mainform.enableControls();
                }
            }
        }
    }
}
