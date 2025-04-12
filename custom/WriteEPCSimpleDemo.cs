using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UHFAPP.custom
{
    public partial class WriteEPCSimpleDemo : Form
    {
        public WriteEPCSimpleDemo()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            byte[] uAccessPwd = new byte[8];
            byte FilterBank = 1;
            int FilterStartaddr = 0;
            int FilterLen = 0;
            byte[] FilterData = new byte[2];
            byte uBank = 1;
            int uPtr = 2;
            int uCnt = 2;

          string data= UHFAPI.getInstance().ReadData(uAccessPwd, FilterBank, FilterStartaddr, FilterLen, FilterData, uBank, uPtr, uCnt);
          txtDataR.Text = "";
          label3.Text = "读取失败";
          label3.ForeColor = Color.Red;
          if (data == null || data.Length == 0)
          {
              return;
          }
          label3.Text = "读取成功！";
          label3.ForeColor = Color.Green;
          txtDataR.Text = data.Replace(" ","") ;

        }
        string z = "000000000000000000000000000000000000000000000000000000000000000000000000";
        private void btnWrite_Click(object sender, EventArgs e)
        {
            byte[] accessPwd = new byte[4];
            byte filterBank = 1;
            int filterPtr = 0;
            int filterCnt = 0;
            byte[] filterData = new byte[1];
            string text = txtDataW.Text.Replace(" ","") ;
            label3.ForeColor = Color.Red;
            if (text.Length == 0)
            {
                label3.Text = "录入的数据不能为空!";
                return;
            } 
            if (text.Length < 8)
            {
                text = z.Substring(0, 8 - text.Length) + text;
            }
            byte[] writeData=DataConvert.HexStringToByteArray(text);
            label3.Text = "录入失败";

            bool result = UHFAPI.getInstance().writeDataToEpc(accessPwd, filterBank, filterPtr, filterCnt, filterData, writeData);
            if (result)
            {
                label3.ForeColor = Color.Green;
                label3.Text = "录入成功";
            } 
        }

        private void WriteEPCSimpleDemo_Load(object sender, EventArgs e)
        {
            bool result = UHFAPI.getInstance().OpenUsb();
            if (!result)
            {
                MessageBox.Show("找不到设备!");
                Close();
            }
        }

        private void WriteEPCSimpleDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            UHFAPI.getInstance().CloseUsb();
        }
    }
}
