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
using System.Collections;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace UHFAPP
{
    public partial class ConfigForm : BaseForm
    {
        bool isAllPower = false;

        private MainForm mainForm;
        public ConfigForm(bool isOpen,MainForm mainForm)
        {
           this.mainForm = mainForm;

            InitializeComponent();
            if (isOpen)
            {
                GetAllConfig();
                panel1.Enabled = false;
            }
            else
            {
                panel1.Enabled = false;
            }

            if (isAllPower)
            {
                groupBox12.Location = new Point(9, 9);
                groupBox12.Width = 406;
                groupBox12.Height = 170;
                groupBox12.Visible = true;
                groupBox6.Visible = false;
            }
        }

        void MainForm_eventOpen(bool open)
        {
            if (open)
            {
                GetAllConfig();
                panel1.Enabled = true;
                if (MainForm.MODE == 2)
                {
                    gbAnt.Enabled = true;
                    gbIP.Enabled = false;
                    gbIp2.Enabled = false;
                    bgGPIO.Enabled = false;
                    gbWorkMode.Enabled = false;
                    groupBox25.Enabled = false;
                    

                    gbIP.Visible = false;
                    gbIp2.Visible = false;
                    gbWorkMode.Visible = false;
                    gbAnt.Visible = true;
                    bgGPIO.Visible = false;
                    //btnReset.Visible = false;
                    groupBox4.Visible = false;
                    //groupBox18.Visible = false;
                    groupBox7.Location = new Point(426, 384);
                    groupBox18.Location = new Point(426, 500);
                }
                else
                {
                    gbAnt.Enabled = true;
                    gbIP.Enabled = true;
                    gbIp2.Enabled = true;
                    bgGPIO.Enabled = true;
                    gbWorkMode.Enabled = true;
                    groupBox25.Enabled = true;
                 

                    gbIP.Visible = true;
                    gbIp2.Visible = true;
                    groupBox7.Visible = true;
                    gbWorkMode.Visible = true;
                    gbAnt.Visible = true;
                    bgGPIO.Visible = true;
                 
                    groupBox4.Visible = true;
                    groupBox18.Visible = true;
                    groupBox7.Location = new Point(426, 566);
                    groupBox18.Location = new Point(426, 687);
                }
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {

            MainForm.eventMainSizeChanged += MainForm_SizeChanged;

            MainForm.eventOpen += MainForm_eventOpen;
            cmbLinkFrequency.SelectedIndex = 3;
 
            comboBox1.SelectedIndex = 0;
            cmbOutStatus.SelectedIndex = 0;
            cmbInput.SelectedIndex = 0;
            comRM.SelectedIndex = 0;
            cmbO2.SelectedIndex = 0;
            cmbO1.SelectedIndex = 0;

            if (MainForm.MODE == 2)
            {
                gbAnt.Enabled = true;
                gbIP.Enabled = false;
                gbIp2.Enabled = false;
                bgGPIO.Enabled = false;
                gbWorkMode.Enabled = false;
                groupBox25.Enabled = false;
                //  btnReset.Enabled = false;
               // groupBox18.Visible = false;
                gbIP.Visible = false;
                gbIp2.Visible = false;
                gbWorkMode.Visible = false;
                gbAnt.Visible = true;
                bgGPIO.Visible = false;
               // btnReset.Visible = false;
                groupBox4.Visible = false;
                groupBox7.Location = new Point(426, 384);
                groupBox18.Location = new Point(426,500);
            }
            else
            {
                gbAnt.Enabled = true;
                gbIP.Enabled = true;
                gbIp2.Enabled = true;
                bgGPIO.Enabled = true;
                gbWorkMode.Enabled = true;
                groupBox25.Enabled = true;
              
                groupBox18.Visible = true; 
                gbIP.Visible = true;
                gbIp2.Visible = true;
                groupBox7.Visible = true;
                gbWorkMode.Visible = true;
                gbAnt.Visible = true;
                bgGPIO.Visible = true;
              
                groupBox4.Visible = true;
                groupBox7.Location = new Point(426, 566); 
                groupBox18.Location = new Point(426, 687);
            }
            LoadUI();
            LoadRegionConfig();
            LoadFrequencyPoint();
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.eventMainSizeChanged -= MainForm_SizeChanged;

            MainForm.eventOpen -= MainForm_eventOpen;
     
        }
       #region 
        private void GetAllConfig()
        {
            string msg = "Cargando configuracíón.....";
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                int errorCount = 0;
                byte power = 0;
                if (uhf.GetPower(ref power))
                {
                    try{ 
                    this.Invoke(new EventHandler(delegate
                    {
                        cmbPower_ANT1.SelectedIndex = power - 1;
                    }));
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                    }
                }
                else
                {
                    errorCount++;
                }
                byte region = 0;
                if (uhf.GetRegion(ref region))
                {
                    int index = GetRegionIndex(region);
                    if (index >= 0)
                    {
                        try
                        {
                            this.Invoke(new EventHandler(delegate
                            {
                                cmbRegion.SelectedIndex = index;
                            }));

                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                        }

                    }
                }
                else
                {
                    errorCount++;
                }
                if (errorCount >= 2)
                {
                    return;
                }

                int type = uhf.GetProtocol();
                if (type > -1)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        cmbProtocol.SelectedIndex = type;
                    }));
                }
                byte mode = 0;
                if (uhf.GetRFLink(ref mode))
                {
                    this.Invoke(new EventHandler(delegate
                   {
                       switch (mode)
                       {
                           case 0:
                           case 1:
                           case 2:
                           case 3:
                           case 4:
                           case 5:
                               cmbRFLink.SelectedIndex = mode;
                               break;
                           case 10:
                           case 11:
                           case 12:
                           case 13:
                           case 14:
                           case 15:
                               cmbRFLink.SelectedIndex = mode - 4;
                               break;
                       }
                   }));
                }


         
                byte Target = 0;
                byte Action = 0;
                byte T = 0;
                byte Q = 0;
                byte StartQ = 0;
                byte MinQ = 0;
                byte MaxQ = 0;
                byte D = 0;
                byte Coding = 0;
                byte P = 0;
                byte Sel = 0;
                byte Session = 0;
                byte G = 0;
                byte LF = 0;
                bool result = uhf.GetGen2(ref  Target, ref   Action, ref   T, ref   Q,
                         ref   StartQ, ref   MinQ,
                         ref   MaxQ, ref   D, ref   Coding, ref   P,
                         ref   Sel, ref   Session, ref   G, ref   LF);
                if (result)
                {
                    this.Invoke(new EventHandler(delegate
                        {
                            cmbTarget.SelectedIndex = Target;
                            cmbAction.SelectedIndex = Action;
                            cmbT.SelectedIndex = T;
                            cmbQ.SelectedIndex = Q;
                            cmbCoding.SelectedIndex = Coding;
                            cmbP.SelectedIndex = P;
                            cmbSel.SelectedIndex = Sel;
                            cmbStartQ.SelectedIndex = StartQ;
                            cmbMinQ.SelectedIndex = MinQ;
                            cmbMaxQ.SelectedIndex = MaxQ;
                            cmbDr.SelectedIndex = D;
                            cmbSession.SelectedIndex = Session;
                            cmbG.SelectedIndex = G;
                            cmbLinkFrequency.SelectedIndex = LF;
                        }));
                }

               
                byte flag = 0;
                if (uhf.GetTagfocus(ref flag))
                {
                    this.Invoke(new EventHandler(delegate
                     {
                         if (flag == 0)
                         {
                             rbTagfocusEnable.Checked = false;
                             rbTagfocusDisable.Checked = true;
                         }
                         else if (flag == 1)
                         {
                             rbTagfocusEnable.Checked = true;
                             rbTagfocusDisable.Checked = false;
                         }
                     }));
                }


                 byte flag1 = 0;
                 if (uhf.GetFastID(ref flag1))
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        if (flag1 == 0)
                        {
                            rbFastIDEnable.Checked = false;
                            rbFastIDDisable.Checked = true;
                        }
                        else if (flag1 == 1)
                        {
                            rbFastIDEnable.Checked = true;
                            rbFastIDDisable.Checked = false;
                        }
                    }));
                }


                byte[] mode3 = new byte[10];
                if (uhf.UHFGetBuzzer(mode3))
                {
                    this.Invoke(new EventHandler(delegate
                    {

                        if (mode3[0] == 0)
                        {

                            rbEnableBuzzer.Checked = false;
                            rbDisableBuzzer.Checked = true;
                        }
                        else if (mode3[0] == 1)
                        {
                            rbDisableBuzzer.Checked = false;
                            rbEnableBuzzer.Checked = true;
                        }
                    }));
                }

                byte userPtr = 0;
                byte userLen = 0;
                int mode4= uhf.getEPCTIDUSERMode(ref userPtr, ref userLen);
                this.Invoke(new EventHandler(delegate
               {
                   switch (mode4)
                   {
                       case 0:
                           cbInventoryMode.SelectedIndex = 0;
                           break;
                       case 1:
                           cbInventoryMode.SelectedIndex = 1;
                           break;
                       case 2:
                           cbInventoryMode.SelectedIndex = 2;
                           txtUserLen.Text = userLen + "";
                           txtUserPtr.Text = userPtr + "";
                           break;
                       default:
                           cbInventoryMode.SelectedIndex = -1;
                           break;
                   }
               }));



                if (MainForm.MODE != 2)
                {
                    byte[] ant = new byte[4];
                    if (uhf.GetANTTo32(ant))
                    {
                        this.Invoke(new EventHandler(delegate
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
                        }));
                    }

                    int startTime = Environment.TickCount;
                    StringBuilder sIP = new StringBuilder(20);
                    StringBuilder sPort = new StringBuilder(20);
                    StringBuilder mask = new StringBuilder(20);
                    StringBuilder gate = new StringBuilder(20);
                    if (uhf.GetReaderIP(sIP, sPort, mask, gate))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            ipControlLocal.IpData = sIP.ToString().Split('.');// txtLocalIP.Text = sIP.ToString();
                            txtLocalPort.Text = sPort.ToString();
                            ipGateway.IpData = gate.ToString().Split('.');
                            ipControlSubnetMask.IpData = mask.ToString().Split('.');
                        }));
                    }

                    sIP = new StringBuilder();
                    sPort = new StringBuilder();
                    if (uhf.GetDestIP(sIP, sPort))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            ipControlDest.IpData = sIP.ToString().Split('.');
                            txtPortDest.Text = sPort.ToString();
                        }));
                    }
                    byte[] mode1 = new byte[2];
                    if (uhf.GetWorkMode(mode1))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            if (mode1[0] == 0)
                            {
                                workMode.SelectedIndex = 0;
                            }
                            else if (mode1[0] == 1)
                            {
                                workMode.SelectedIndex = 1;
                            }
                            else if (mode1[0] == 2)
                            {
                                workMode.SelectedIndex = 2;
                            }
                        }));
                    }


                    if (uhf.GetFastInventory(ref flag))
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            if (flag == 0)
                            {
                                rbOpenFastInventory.Checked = false;
                                rbCloseFastInventory.Checked = true;
                            }
                            else if (flag == 1)
                            {
                                rbOpenFastInventory.Checked = true;
                                rbCloseFastInventory.Checked = false;
                            }
                        }));
                    }

                }



            }, msg);
            f.ShowDialog(mainForm);
        }
        public int GetRegionIndex(int region)
        {
            switch (region)
            {
                case 0x01:
                    return 0;
                case 0x02:
                    return 1;
                case 0x04:
                    return 2;
                case 0x08:
                    return 3;
                case 0x16:
                    return 4;
                case 0x32:
                    return 5;
                case 0x33:
                    return 6;
                case 0x34:
                    return 7;
                case 0x35:
                    return 8;
                case 0x36:
                    return 9;
                case 0x37:
                    return 10;
                case 0x3B:
                    return 11;
                case 0x3C:
                    return 12;
                case 0x3D:
                    return 13;
                case 0x3E:
                    return 14;
                case 0x3F:
                    return 15;
                case 0x40:
                    return 16;
                case 0x41:
                    return 17;
                case 0x42:
                    return 18;
                case 0x43:
                    return 19;
                case 0x44:
                    return 20;

            }
            return -1;
        }
        #endregion
        
        #region 功率
        private void btnPowerGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            byte power =0;
            if (uhf.GetPower(ref power))
            {
                cmbPower_ANT1.SelectedIndex = power - 1;
                msg ="Correcto!" ;
            }
            showMessage(msg);
        }

        private void btnPowerSet_Click(object sender, EventArgs e)
        {
            string msg = "Error !" ;
            if (cmbPower_ANT1.SelectedIndex >= 0)
            {
                byte power1 = (byte)(cmbPower_ANT1.SelectedIndex + 1);

                byte save = (byte)(cbPower.Checked?1:0);
                if (uhf.SetPower(save, power1))
                {
                    msg = "Correcto..." ;
                }

            }
            showMessage(msg);
        } 

        #endregion

        #region 工作频率
//917.1
//917.3
//917.5
//917.7
//917.9
//918.1
//918.3
//918.5
//918.7
//918.9
//919.1
//919.3
//919.5
//919.7
//919.9
//920.1
//920.3
//920.5
//920.7
//920.9
//921.1
//921.3
//921.5
//921.7
//921.9
//922.1
//922.3
//922.5
//922.7
//922.9
//923.1
//923.3

        private void btnWorkModeSet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            try
             {
                 if (comboBox1.Text != "")
                 {
                     string frequency = comboBox1.Text.Replace(".", "");
                     if (frequency.Length == 3) {
                         frequency = frequency + "000";
                     }
                     else if (frequency.Length == 4) {
                         frequency = frequency + "00";
                     }
                     else if (frequency.Length == 5)
                     {
                         frequency = frequency + "0";
                     }
                     if (StringUtils.IsNumber(frequency))
                     {
                         int[] ifrequency = new int[] { int.Parse(frequency) };
                         if (uhf.SetJumpFrequency(1, ifrequency))
                         {
                            msg = "Correcto...";
                        }
                     }
                 }
             }
             catch (Exception ex) { 
             
             }

             showMessage(msg);
        }
   
        #endregion

        #region Gen2
        private void btnGen2Get_Click(object sender, EventArgs e)
        {
            byte Target = 0;
            byte Action = 0;
            byte T = 0;
            byte Q = 0;
            byte StartQ = 0;
            byte MinQ = 0;
            byte MaxQ = 0;
            byte D = 0;
            byte Coding = 0;
            byte P = 0;
            byte Sel = 0;
            byte Session = 0;
            byte G = 0;
            byte LF = 0;
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            int start = Environment.TickCount;

            bool result=uhf.GetGen2(ref  Target, ref   Action, ref   T, ref   Q,
                     ref   StartQ, ref   MinQ,
                     ref   MaxQ, ref   D, ref   Coding, ref   P,
                     ref   Sel, ref   Session, ref   G, ref   LF);
          //  MessageBox.Show("耗时：" + (Environment.TickCount-start));
            if (result)
            {
                cmbTarget.SelectedIndex = Target;
                cmbAction.SelectedIndex = Action;
                cmbT.SelectedIndex = T;
                cmbQ.SelectedIndex = Q;
                cmbCoding.SelectedIndex = Coding;
                cmbP.SelectedIndex = P;
                cmbSel.SelectedIndex = Sel;
                cmbStartQ.SelectedIndex = StartQ;
                cmbMinQ.SelectedIndex = MinQ;
                cmbMaxQ.SelectedIndex = MaxQ;
                cmbDr.SelectedIndex = D;
                cmbSession.SelectedIndex = Session;
                cmbG.SelectedIndex = G;
                cmbLinkFrequency.SelectedIndex = LF;
                msg = "Correcto...";
                btnGen2Set.Enabled = true;
            }


            showMessage(msg);
        }
        private void btnGen2Set_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            try
            {
                byte Target =(byte) cmbTarget.SelectedIndex;
                byte Action = (byte)cmbAction.SelectedIndex;
                byte T = (byte)cmbT.SelectedIndex;
                byte Q = (byte)cmbQ.SelectedIndex;
                byte StartQ = (byte)cmbStartQ.SelectedIndex;
                byte MinQ = (byte)cmbMinQ.SelectedIndex;
                byte MaxQ = (byte)cmbMaxQ.SelectedIndex;
                byte D = (byte)cmbDr.SelectedIndex;
                byte Coding = (byte)cmbCoding.SelectedIndex;
                byte P = (byte)cmbP.SelectedIndex;
                byte Sel = (byte)cmbSel.SelectedIndex;
                byte Session = (byte)cmbSession.SelectedIndex;
                byte G = (byte)cmbG.SelectedIndex;
                byte LF = (byte)cmbLinkFrequency.SelectedIndex;
                if (uhf.SetGen2(Target, Action, T, Q, StartQ, MinQ, MaxQ, D, Coding, P, Sel, Session, G, LF))
                {
                    msg = "Correcto...";
                }
                
            }
            catch (Exception ex)
            {
               
            }
            showMessage(msg);
        }
        #endregion

        #region CW
        private void btnGetCW_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (uhf.SetCW(1))
            {
                msg = "Correcto...";
            }
            showMessage(msg);
        }
        private void btnSetCW_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (uhf.SetCW(0))
            {
                msg = "Correcto...";
            }
            showMessage(msg);
        }
        #endregion

        #region 天线
        private void btnGetANT_Click(object sender, EventArgs e)
        {
  
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
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
    


                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                //  msg = Common.isEnglish?"Correcto!":"获取天线成功!("+ DataConvert.ByteArrayToHexString(ant)+")";
            }

            showMessage(msg);
        }
        private void btnSetAnt_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            int b1 = 0;
            int b2 = 0;
             
            if (cmbAnt16.Checked) b1 = b1 | 128;
            if (cmbAnt15.Checked) b1 = b1 | 64;
            if (cmbAnt14.Checked) b1 = b1 | 32;
            if (cmbAnt13.Checked) b1 = b1 | 16;
            if (cmbAnt12.Checked) b1 = b1 | 8;
            if (cmbAnt11.Checked) b1 = b1 | 4;
            if (cmbAnt10.Checked) b1 = b1 | 2;
            if (cmbAnt9.Checked) b1 = b1 | 1;


            if (cmbAnt8.Checked) b2 = b2 | 128;
            if (cmbAnt7.Checked) b2 = b2 | 64;
            if (cmbAnt6.Checked) b2 = b2 | 32;
            if (cmbAnt5.Checked) b2 = b2 | 16;
            if (cmbAnt4.Checked) b2 = b2 | 8;
            if (cmbAnt3.Checked) b2 = b2 | 4;
            if (cmbAnt2.Checked) b2 = b2 | 2;
            if (cmbAnt1.Checked) b2 = b2 | 1;

            int b3 = 0;
            int b4 = 0;
            if (cbANT32.Checked) b3 = b3 | 128;
            if (cbANT31.Checked) b3 = b3 | 64;
            if (cbANT30.Checked) b3 = b3 | 32;
            if (cbANT29.Checked) b3 = b3 | 16;
            if (cbANT28.Checked) b3 = b3 | 8;
            if (cbANT27.Checked) b3 = b3 | 4;
            if (cbANT26.Checked) b3 = b3 | 2;
            if (cbANT25.Checked) b3 = b3 | 1;

            if (cbANT24.Checked) b4 = b4 | 128;
            if (cbANT23.Checked) b4 = b4 | 64;
            if (cbANT22.Checked) b4 = b4 | 32;
            if (cbANT21.Checked) b4 = b4 | 16;
            if (cbANT20.Checked) b4 = b4 | 8;
            if (cbANT19.Checked) b4 = b4 | 4;
            if (cbANT18.Checked) b4 = b4 | 2;
            if (cbANT17.Checked) b4 = b4 | 1;

            byte[] ant = new byte[4] { (byte)b1, (byte)b2, (byte)b3, (byte)b4 };
            byte flag = cbAntSet.Checked ? (byte)1 : (byte)0;
            if (uhf.SetANTTo32(flag, ant))
            {
                msg = "Correcto...";                // msg = Common.isEnglish ? "Correcto!" : "设置天线成功!(" + DataConvert.ByteArrayToHexString(ant) + ")"; ;
            }
            showMessage(msg);

        }


 
     
        
        
        #endregion

        #region 区域
        private void btnRegionGet_Click(object sender, EventArgs e)
        {
            if (frequencyBandHashtable != null)
            {
                string msg1 = !IsChineseSimple() ? "Error!" : "失败!";
                byte region2 = 0;
                if (uhf.GetRegion(ref region2))
                {
                    cmbRegion.SelectedIndex = 0;
                    msg1 = "Correcto...";
                }
                showMessage(msg1);
                return;
            }

            //0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            byte region=0;
            if (uhf.GetRegion(ref region))
            {
                switch (region)
                {
                    case 0x01:
                        cmbRegion.SelectedIndex = 0;
                        break;
                    case 0x02:
                        cmbRegion.SelectedIndex = 1;
                        break;
                    case 0x04:
                        cmbRegion.SelectedIndex = 2;
                        break;
                    case 0x08:
                        cmbRegion.SelectedIndex = 3;
                        break;
                    case 0x16:
                        cmbRegion.SelectedIndex = 4;
                        break;
                    case 0x32:
                        cmbRegion.SelectedIndex = 5;
                        break;
                    case 0x33:
                        cmbRegion.SelectedIndex = 6;
                        break;
                    case 0x34:
                        cmbRegion.SelectedIndex = 7;
                        break;
                    case 0x35:
                        cmbRegion.SelectedIndex = 8;
                        break;
                    case 0x36:
                        cmbRegion.SelectedIndex = 9;
                        break;
                    case 0x37:
                        cmbRegion.SelectedIndex = 10;
                        break;
                    case 0x3B:
                        cmbRegion.SelectedIndex = 11;
                        break;
                    case 0x3C:
                        cmbRegion.SelectedIndex = 12;
                        break;
                    case 0x3D:
                        cmbRegion.SelectedIndex = 13;
                        break;
                    case 0x3E:
                        cmbRegion.SelectedIndex = 14;
                        break;
                    case 0x3F:
                        cmbRegion.SelectedIndex = 15;
                        break;
                    case 0x40:
                        cmbRegion.SelectedIndex = 16;
                        break;
                    case 0x41:
                        cmbRegion.SelectedIndex = 17;
                        break;
                    case 0x42:
                        cmbRegion.SelectedIndex = 18;
                        break;
                    case 0x43:
                        cmbRegion.SelectedIndex = 19;
                        break;
                    case 0x44:
                        cmbRegion.SelectedIndex = 20;
                        break;

                }
                msg = "Correcto...";
            }

            showMessage(msg);
        }

        private void btnRegionSet_Click(object sender, EventArgs e)
        {
            //0x01(China1),0x02(China2),0x04(Europe),0x08(USA),0x16(Korea),0x32(Japan)
            if (frequencyBandHashtable != null)
            {
                if (frequencyBandHashtable.ContainsKey(cmbRegion.Text))
                {
                    int flag2 = cbRegionSave.Checked ? 1 : 0;
                    string msg2 = !IsChineseSimple() ? "Error!" : "失败!";
                    string value = (string)frequencyBandHashtable[cmbRegion.Text];
                    if (uhf.SetRegion((byte)flag2, (byte)Convert.ToInt32(value.Replace("0x",""), 16)))
                    {
                        msg2 = "Correcto...";
                    }
                    showMessage(msg2);
                }
                return;
            }
            int flag = cbRegionSave.Checked ? 1 : 0;
            int region = -1;
            switch (cmbRegion.SelectedIndex)
            {
                case 0:
                    region = 0x01;
                    break;
                case 1:
                    region = 0x02;
                    break;
                case 2:
                    region = 0x04;
                    break;
                case 3:
                    region = 0x08;
                    break;
                case 4:
                    region = 0x16;
                    break;
                case 5:
                    region = 0x32;
                    break;
                case 6:
                    region = 0x33;
                    break;
                case 7:
                    region = 0x34;
                    break;
                case 8:
                    region = 0x35;
                    break;
                case 9:
                    region = 0x36;
                    break;
                case 10:
                    region = 0x37;
                    break;
                case 11:
                    region = 0x3B;
                    break;
                case 12:
                    region = 0x3C;
                    break;
                case 13:
                    region = 0x3D;
                    break;
                case 14:
                    region = 0x3E;
                    break;
                case 15:
                    region = 0x3F;
                    break;
                case 16:
                    region = 0x40;
                    break;
                case 17:
                    region = 0x41;
                    break;
                case 18:
                    region = 0x42;
                    break;
                case 19:
                    region = 0x43;
                    break;
                case 20:
                    region = 0x44;
                    break;

            }
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (region >= 0)
            {
                if (uhf.SetRegion((byte)flag, (byte)region))
                {
                    msg = "Correcto...";
                }
                 
            }
            showMessage(msg);
        }
        #endregion
 

        #region 链路组合
        private void btnRFLinkGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            byte mode = 0;
            if (uhf.GetRFLink(ref mode))
            {
                switch (mode)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        cmbRFLink.SelectedIndex = mode;
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        cmbRFLink.SelectedIndex = mode-4;
                        break;
                }
                msg = "Correcto...";
            }

            showMessage(msg);
        }
        private void btnRFLinkSet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            int flag = cbRFLink.Checked ? 1 : 0;
            if (cmbRFLink.SelectedIndex >= 0)
            {
                int v = cmbRFLink.SelectedIndex;
                switch (cmbRFLink.SelectedIndex)
                {
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        v = cmbRFLink.SelectedIndex+4;
                        break;
                }
                if (uhf.SetRFLink((byte)flag, (byte)v)) {
                    msg = "Correcto...";
                }
            }

            showMessage(msg);
        }

        #endregion

        #region FastID
        private void btnFastIDGet_Click(object sender, EventArgs e)
        {
            byte flag = 0;
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (uhf.GetFastID(ref flag))
            {
                if (flag == 0)
                {
                    rbFastIDEnable.Checked = false;
                    rbFastIDDisable.Checked = true;
                    msg = !IsChineseSimple() ? "Suc<cess" : "成功!";
                }
                else if (flag == 1)
                {
                    rbFastIDEnable.Checked = true;
                    rbFastIDDisable.Checked = false;
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }
            }
            showMessage(msg);
        }
        private void btnFastIDSet_Click(object sender, EventArgs e)
        {
            int flag = -1;
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (rbFastIDEnable.Checked)
            {
                flag = 1;
            }
            else if (rbFastIDDisable.Checked)
            {
                flag = 0;
            }

            if (flag >= 0)
            {
                if (uhf.SetFastID((byte)flag))
                {
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";

                    if (flag == 1) {
                        if (uhf.SetTagfocus(0))
                        {
                            rbTagfocusDisable.Checked = true;
                        }
                        if (uhf.setEPCMode(false))
                        {
                            cbInventoryMode.SelectedIndex = 0;
                        }
                    }

                }
               
            }

            showMessage(msg);
        }
        #endregion

        #region Tagfocus
        private void btnrbTagfocusGet_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            byte flag = 0;
            if (uhf.GetTagfocus(ref flag))
            {
                if (flag == 0)
                {
                    rbTagfocusEnable.Checked = false;
                    rbTagfocusDisable.Checked = true;
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }
                else if (flag == 1)
                {
                    rbTagfocusEnable.Checked = true;
                    rbTagfocusDisable.Checked = false;
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }
            }

            showMessage(msg);
        }
        private void btnrbTagfocusSet_Click(object sender, EventArgs e)
        {
            int flag = -1;
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (rbTagfocusEnable.Checked)
            {
                flag = 1;
            }
            else if (rbTagfocusDisable.Checked)
            {
                flag = 0;
            }

            if (flag >= 0)
            {
                if (uhf.SetTagfocus((byte)flag))
                {
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                    if (flag == 1)
                    {

                        if (uhf.SetFastID(0))
                        {
                            rbFastIDDisable.Checked = true;
                        }
                 
                        if (uhf.setEPCMode(false))
                        {
                            cbInventoryMode.SelectedIndex = 0;
                        }
                    }
                }
                
            }
            showMessage(msg);
        }
        #endregion
        #region 设置软复位
        private void btnReset_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (uhf.SetSoftReset())
            {
                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
            }

            showMessage(msg);
        }
        #endregion

        #region FastInventory
        private void btnGetFastInventory_Click(object sender, EventArgs e)
        {
            byte flag = 0;
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (uhf.GetFastInventory(ref flag))
            {
                if (flag == 0)
                {
                    rbOpenFastInventory.Checked = false;
                    rbCloseFastInventory.Checked = true;
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }
                else if (flag == 1)
                {
                    rbOpenFastInventory.Checked = true;
                    rbCloseFastInventory.Checked = false;
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }
            }
            showMessage(msg);
        }

        private void btnSetFastInventory_Click(object sender, EventArgs e)
        {
            int flag = -1;
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            if (rbOpenFastInventory.Checked)
            {
                flag = 1;
            }
            else if (rbCloseFastInventory.Checked)
            {
                flag = 0;
            }

            if (flag >= 0)
            {
                if (uhf.SetFastInventory((byte)flag))
                {
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }

            }

            showMessage(msg);
        }
        #endregion


        #region 协议
        private void button2_Click_1(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            int type = uhf.GetProtocol();
            if (type>-1)
            {
                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                cmbProtocol.SelectedIndex = type;
            }
            showMessage(msg);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            int type = cmbProtocol.SelectedIndex;
            if (type >= 0)
            {
                if (uhf.SetProtocol((byte)type))
                {
                    msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                }
            }
            showMessage(msg);
        }


        #endregion  

      

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string data = textBox1.Text.Trim().Replace(" ", "");
            if (data.Length > 0) {

                
                int index=textBox1.SelectionStart-1;
                if (index >= 0)
                {
                    string charData = data.Substring(index, 1);
                    if (charData != "0" && charData != "1" && charData != "2" 
                        && charData != "3" && charData != "4" && charData != "5" && charData != "6" && charData != "7"
                        && charData != "8" && charData != "9" && charData != ".")
                    {
                        textBox1.Text = textBox1.Text.Remove(index, 1);
                        textBox1.SelectionStart = textBox1.Text.Length;
                    }
                }
            }
   
        }



    

        public void showMessage(string msg,int time) {
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(time);
            }, msg);
            f.ShowDialog(this);
        }
        public void showMessage(string msg)
        {
            frmWaitingBox f = new frmWaitingBox((obj, args) =>
            {
                System.Threading.Thread.Sleep(500);
            }, msg);
            f.ShowDialog(this);
        }

        #region IP
        //设置本地IP
        private void btnSetIPLocal_Click(object sender, EventArgs e)
        {
            string port = txtLocalPort.Text.Trim();

            StringBuilder ip = new StringBuilder();
            ip.Append(ipControlLocal.IpData[0]);
            ip.Append(".");
            ip.Append(ipControlLocal.IpData[1]);
            ip.Append(".");
            ip.Append(ipControlLocal.IpData[2]);
            ip.Append(".");
            ip.Append(ipControlLocal.IpData[3]);


            StringBuilder sbMask = new StringBuilder();
            sbMask.Append(ipControlSubnetMask.IpData[0]);
            sbMask.Append(".");
            sbMask.Append(ipControlSubnetMask.IpData[1]);
            sbMask.Append(".");
            sbMask.Append(ipControlSubnetMask.IpData[2]);
            sbMask.Append(".");
            sbMask.Append(ipControlSubnetMask.IpData[3]);


            StringBuilder gate = new StringBuilder();
            gate.Append(ipGateway.IpData[0]);
            gate.Append(".");
            gate.Append(ipGateway.IpData[1]);
            gate.Append(".");
            gate.Append(ipGateway.IpData[2]);
            gate.Append(".");
            gate.Append(ipGateway.IpData[3]);

            if (!StringUtils.isIP(ip.ToString()) || !StringUtils.isIP(sbMask.ToString()) || !StringUtils.isIP(gate.ToString()))
            {
                string msg = !IsChineseSimple() ? "Error!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            if (!StringUtils.IsNumber(port))
            {
                string msg = !IsChineseSimple() ? "Error!" : "设置IP失败!";
                showMessage(msg);
                return;
            }

            if (ipControlLocal.IpData[0] == "255")
            {
                string msg = !IsChineseSimple() ? "Error!" : "IP地址不能255开头,设置IP失败!";
                showMessage(msg);
                return;
            }
            if (ipControlSubnetMask.IpData[0] != "255")
            {
                string msg = !IsChineseSimple() ? "Error!" : "子网掩码必须255开头,设置IP失败!";
                showMessage(msg);
                return;
            }
            if (ipGateway.IpData[0] == "255")
            {
                string msg = !IsChineseSimple() ? "Error!" : "网关不能255开头,设置IP失败!";
                showMessage(msg);
                return;
            }
            if (!uhf.SetReaderIP(ip.ToString(), int.Parse(port), sbMask.ToString(), gate.ToString()))
            {
                string msg = !IsChineseSimple() ? "Error!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
             string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
             showMessage(msg2);
           }
        //获取本地IP
        private void btnGetIPLocal_Click(object sender, EventArgs e)
        {
            int startTime = Environment.TickCount;
            StringBuilder sIP=new StringBuilder(20);
            StringBuilder sPort = new StringBuilder(20);
            StringBuilder mask = new StringBuilder(20);
            StringBuilder gate = new StringBuilder(20);
            if (uhf.GetReaderIP(sIP, sPort, mask, gate))
            {
                ipControlLocal.IpData = sIP.ToString().Split('.');// txtLocalIP.Text = sIP.ToString();
                txtLocalPort.Text = sPort.ToString();
                ipGateway.IpData = gate.ToString().Split('.');
                ipControlSubnetMask.IpData = mask.ToString().Split('.');
            }
            else
            {
                string msg = !IsChineseSimple() ? "Error!" : "获取IP失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);

        }
 
        //获取目标IP
        private void btnGetIpDest_Click(object sender, EventArgs e)
        {
            StringBuilder sIP = new StringBuilder();
            StringBuilder sPort = new StringBuilder();
            if (uhf.GetDestIP(sIP, sPort))
            {
               // txtIPDest.Text = sIP.ToString();
                ipControlDest.IpData = sIP.ToString().Split('.');
                txtPortDest.Text = sPort.ToString();
            }
            else
            {
                string msg = !IsChineseSimple() ? "Error!" : "获取IP失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }

        //设置目标IP
        private void btnSetIpDest_Click(object sender, EventArgs e)
        {
            string port = txtPortDest.Text.Trim();

            string[] tempIp = ipControlDest.IpData;
            StringBuilder sb = new StringBuilder();
            sb.Append(tempIp[0]);
            sb.Append(".");
            sb.Append(tempIp[1]);
            sb.Append(".");
            sb.Append(tempIp[2]);
            sb.Append(".");
            sb.Append(tempIp[3]);
            string ip = sb.ToString();
  
            if (!StringUtils.isIP(ip))
            {
                string msg = !IsChineseSimple() ? "Error!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            if (!StringUtils.IsNumber(port))
            {
                string msg = !IsChineseSimple() ? "Error!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            if (ipControlDest.IpData[0] == "255")
            {
                string msg = !IsChineseSimple() ? "Error!" : "IP地址不能255开头，设置IP失败!";
                showMessage(msg);
                return;
            }

            if (!uhf.SetDestIP(ip, int.Parse(port)))
            {
                string msg = !IsChineseSimple() ? "Error!" : "设置IP失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }

        #endregion

        #region 蜂鸣器
        //获取蜂鸣器
        private void btnGetBuzzer_Click(object sender, EventArgs e)
        {
            byte[] mode=new byte[10];
            if (!uhf.UHFGetBuzzer(mode))
            {
                string msg = !IsChineseSimple() ? "Error!" : "失败!";
                showMessage(msg);
                return;
            }
            else
            {
                if (mode[0] == 0)
                {

                    rbEnableBuzzer.Checked = false;
                    rbDisableBuzzer.Checked = true;
                }
                else if (mode[0] == 1)
                {
                    rbDisableBuzzer.Checked = false;
                    rbEnableBuzzer.Checked = true;
                }

            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }
        //设置蜂鸣器
        private void btnSetBuzzer_Click(object sender, EventArgs e)
        {
            //0x00表示关闭蜂鸣器；0x01表示打开蜂鸣器
            byte mode =0;
            if (rbEnableBuzzer.Checked)
            {
                mode = 1;
            }
            else if (rbDisableBuzzer.Checked)
            {
                mode = 0;
            }
            else {

                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;
            }

            if (!uhf.UHFSetBuzzer(mode))
            {
                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }
        #endregion

        #region 工作模式
        private void button2_Click(object sender, EventArgs e)
        {
            //get
            byte[] mode=new byte[2];
            if (uhf.GetWorkMode(mode))
            {
                if (mode[0] == 0)
                {
                    workMode.SelectedIndex = 0;
                }
                else if (mode[0] == 1)
                {
                    workMode.SelectedIndex = 1;
                }
                else if (mode[0] == 2)
                {
                    workMode.SelectedIndex = 2;
                }
            }
            else
            {
                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;

            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte mode=(byte) workMode.SelectedIndex;
            if (!uhf.SetWorkMode(mode))
            {
                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;
             
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }

        private void btnWorkModeParaSet_Click(object sender, EventArgs e)
        {
            UHFAPI uhfAPI = uhf as UHFAPI;
            if (uhfAPI != null)
            {
                if (txtWT.Text.Trim().Length == 0)
                {
                    string msg = !IsChineseSimple() ? "Error" : "失败!";
                    showMessage(msg);
                }
                if (txtIT.Text.Trim().Length == 0)
                {
                    string msg = !IsChineseSimple() ? "Error" : "失败!";
                    showMessage(msg);
                }

                int input = cmbInput.SelectedIndex;
                int workTime = int.Parse(txtWT.Text);
                int waitTime = int.Parse(txtIT.Text);
                int receiveMode = comRM.SelectedIndex;
                if (!uhfAPI.SetWorkModePara((byte)input, workTime, waitTime, (byte)receiveMode))
                {
                    string msg = !IsChineseSimple() ? "Error" : "失败!";
                    showMessage(msg);
                }
                string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
                showMessage(msg2);
            }

        }

        private void btnWorkModeParaGet_Click(object sender, EventArgs e)
        {
            UHFAPI uhfAPI = uhf as UHFAPI;
            if (uhfAPI != null)
            {
                 byte ioControl=0;
                 int workTime=100;
                 int intervalTime=0;
                 byte mode=0;
                if (uhfAPI.GetWorkModePara(ref  ioControl, ref  workTime, ref intervalTime, ref mode))
                {
                    cmbInput.SelectedIndex = ioControl;
                    txtWT.Text = workTime.ToString();
                    txtIT.Text = intervalTime.ToString();
                    comRM.SelectedIndex = mode;
                    string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
                    showMessage(msg2);

                }
                else
                {
                    string msg = !IsChineseSimple() ? "Error" : "失败!";
                    showMessage(msg);
                }            
            }
        }

        #endregion
 

    
 
        #region GPIO
        private void button6_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            if (!uhf.getIOControl(data))
            {
                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;
            }
            else {
                comboBox2.SelectedIndex = data[0];
                comboBox3.SelectedIndex = data[1];
                string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
                showMessage(msg2);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int status = cmbOutStatus.SelectedIndex;
            int ouput2 = cmbO2.SelectedIndex;
            int ouput1 = cmbO1.SelectedIndex;
 
            if (!uhf.setIOControl((byte)ouput1, (byte)ouput2, (byte)status))
            {
                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }
        #endregion

   

        #region  盘点模式设置

        private void button11_Click(object sender, EventArgs e)
        {
            byte userPtr = 0;
            byte userLen = 0;
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
 
            int mode=uhf.getEPCTIDUSERMode(ref userPtr,ref userLen);
            switch (mode)
            {
                case 0:
                    cbInventoryMode.SelectedIndex = 0;
                    break;
                case 1:
                    cbInventoryMode.SelectedIndex = 1;
                    break;
                case 2:
                    cbInventoryMode.SelectedIndex = 2;
                    txtUserLen.Text = userLen+"";
                    txtUserPtr.Text = userPtr + "";
                    break;
                default:
                    msg2 = !IsChineseSimple() ? "Error" : "失败!";
                    cbInventoryMode.SelectedIndex = -1;
                    break;
            }
     
            showMessage(msg2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int mode = cbInventoryMode.SelectedIndex;
            bool result = false;
            bool isSave=checkBox2.Checked;
            switch (mode)
            {
                case 0:
                    result=uhf.setEPCMode(isSave);
                    break;
                case 1:
                    result=uhf.setEPCAndTIDMode(isSave);
                    break;
                case 2:
                    int userPtr = int.Parse(txtUserPtr.Text);
                    int userLen = int.Parse(txtUserLen.Text);
                    result = uhf.setEPCAndTIDUSERMode(isSave,(byte)userPtr, (byte)userLen);
                    break;
            }

            if (!result)
            {
                string msg = !IsChineseSimple() ? "Error" : "失败!";
                showMessage(msg);
                return;
            }
            string msg2 = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg2);
        }

        private void cbInventoryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInventoryMode.SelectedIndex == 2)
            {
                txtUserLen.Visible = true;
                txtUserPtr.Visible = true;
                label46.Visible = true;
                label47.Visible = true;
            }
            else
            {
                txtUserLen.Visible = false;
                txtUserPtr.Visible = false;
                label46.Visible = false;
                label47.Visible = false;
            }
        }
        #endregion

    

        private void workMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (workMode.SelectedIndex == 2)
            {
                plWorkModePara.Visible = true;
                btnWorkModeParaGet_Click(null,null);
            }
            else
            {
                plWorkModePara.Visible = false;

            }
        }

        private void btnAntennaConnectionState_Click(object sender, EventArgs e)
        {
            cbANT1_state.Checked = false;
            cbANT2_state.Checked = false;
            cbANT3_state.Checked = false;
            cbANT4_state.Checked = false;
            cbANT5_state.Checked = false;
            cbANT6_state.Checked = false;
            cbANT7_state.Checked = false;
            cbANT8_state.Checked = false;

            string msg = !IsChineseSimple() ? "Error" : "失败!";
            short[] antstate = new short[1];
            if (uhf.GetANTLinkStatus(antstate))
            {
                short antS = antstate[0];
                if (((antS >> 7) & 1) == 1) cbANT8_state.Checked = true;
                if (((antS >> 6) & 1) == 1) cbANT7_state.Checked = true;
                if (((antS >> 5) & 1) == 1) cbANT6_state.Checked = true;
                if (((antS >> 4) & 1) == 1) cbANT5_state.Checked = true;
                if (((antS >> 3) & 1) == 1) cbANT4_state.Checked = true;
                if (((antS >> 2) & 1) == 1) cbANT3_state.Checked = true;
                if (((antS >> 1) & 1) == 1) cbANT2_state.Checked = true;
                if ((antS & 1) == 1) cbANT1_state.Checked = true;
                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
            }
            showMessage(msg);
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            int result = uhf.CalibrationVoltage();
            txtCalibration.Text = result + "";
           
        }

        private void button12_Click(object sender, EventArgs e)
        {
            byte[] statusData = new byte[2];
            string msg = !IsChineseSimple() ? "Error" : "失败!";
            if (uhf.GetInputStatus(statusData))
            {
                cmbInput1.SelectedIndex = statusData[0];
                cmbInput2.SelectedIndex = statusData[1];
                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
            }
            showMessage(msg);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] outData = new byte[5];
            outData[3] = (byte)cmbOutput1.SelectedIndex;
            outData[4] = (byte)cmbOutput2.SelectedIndex;
            string msg = !IsChineseSimple() ? "Error" : "失败!";
            if (uhf.SetOutput(outData))
            {

                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
            }
            showMessage(msg);
        }


        private void MainForm_SizeChanged(FormWindowState state)
        {
            //判断是否选择的是最小化按钮
            panel1.Left = 308;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string id = uhf.GetUHFGetDeviceID();
            string msg = !IsChineseSimple() ? "Error" : "失败!";
            if (id !=null)
            {
                
                textBox2.Text = id;
                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
                showMessage(msg);
                return;
            }
            showMessage(msg);
        }
        private void LoadUI()
        {
            if (!IsChineseSimple())
            {
                int index2 = comboBox2.SelectedIndex;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("low voltage");
                comboBox2.Items.Add("high voltage");
                if (index2 >= 0)
                    comboBox2.SelectedIndex = index2;

                int cmbO1Index = cmbO1.SelectedIndex;
                cmbO1.Items.Clear();
                cmbO1.Items.Add("low voltage");
                cmbO1.Items.Add("high voltage");
                if (cmbO1Index >= 0)
                    cmbO1.SelectedIndex = cmbO1Index;

                cmbO1Index = cmbO2.SelectedIndex;
                cmbO2.Items.Clear();
                cmbO2.Items.Add("low voltage");
                cmbO2.Items.Add("high voltage");
                if (cmbO1Index >= 0)
                    cmbO2.SelectedIndex = cmbO1Index;

                int index3 = comboBox3.SelectedIndex;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("low voltage");
                comboBox3.Items.Add("high voltage");
                if (index3 >= 0)
                    comboBox3.SelectedIndex = index3;

                int index4 = cmbInput1.SelectedIndex;
                cmbInput1.Items.Clear();
                cmbInput1.Items.Add("low voltage");
                cmbInput1.Items.Add("high voltage");
                if (index4 >= 0)
                    cmbInput1.SelectedIndex = index4;

                int index5 = cmbInput2.SelectedIndex;
                cmbInput2.Items.Clear();
                cmbInput2.Items.Add("low voltage");
                cmbInput2.Items.Add("high voltage");
                if (index5 >= 0)
                    cmbInput2.SelectedIndex = index5;

                int index6 = cmbOutput1.SelectedIndex;
                cmbOutput1.Items.Clear();
                cmbOutput1.Items.Add("low voltage");
                cmbOutput1.Items.Add("high voltage");
                if (index6 >= 0)
                    cmbOutput1.SelectedIndex = index6;

                int index7 = cmbOutput2.SelectedIndex;
                cmbOutput2.Items.Clear();
                cmbOutput2.Items.Add("low voltage");
                cmbOutput2.Items.Add("high voltage");
                if (index7 >= 0)
                    cmbOutput2.SelectedIndex = index7;

                int index1 = cmbOutStatus.SelectedIndex;
                cmbOutStatus.Items.Clear();
                cmbOutStatus.Items.Add("Disconnect");
                cmbOutStatus.Items.Add("close");
                if (index1 >= 0)
                    cmbOutStatus.SelectedIndex = index1;

                groupBox19.Text = "Protocol";
                label35.Text = "Protocol:";

                label2.Text = "";
                groupBox6.Text = "Power";
                label24.Text = "Output Power:";
                // cbSave.Text = "Save";


                groupBox11.Text = "Region";
                label1.Text = "Region:";
                cbRegionSave.Text = "Save";

                label5.Text = "RFLink:";
                groupBox3.Text = "RFLink";
                cbRFLink.Text = "cbSave";

                groupBox7.Text = "Rrequency";
                label28.Text = "Rrequency:";

                

                gbAnt.Text = "ANT";
              
                label24.Location = new Point(8, 34);
            

                gbIP.Text = "Local IP";
                gbIp2.Text = "Destination IP";
                label9.Text = label31.Text = "IP:";
                label25.Text = label30.Text = "Port:";
                groupBox9.Text = "Buzzer";

                int index = workMode.SelectedIndex == -1 ? 0 : workMode.SelectedIndex;
                workMode.Items.Clear();
                workMode.Items.Add("command mode");
                workMode.Items.Add("auto mode");
                workMode.Items.Add("trigger mode");

                groupBox1.Text = "cw";

                workMode.SelectedIndex = index;


                label39.Text = "input1:";
                label40.Text = "input2:";
                label38.Text = "Relay:";

                gbInventoryMode.Text = "Inventory Mode";
                label45.Text = "Mode:";
                label46.Text = "User Ptr:";
                label47.Text = "User Len:";

                label53.Text = "Subnet mask:";
                label54.Text = "Gateway:";

                gbFastInventory.Text = "Fast Inventory";
            }
            else
            {
                label53.Text = "   子网掩码:";
                label54.Text = "   网关:";

                int index1 = cmbOutStatus.SelectedIndex;
                cmbOutStatus.Items.Clear();
                cmbOutStatus.Items.Add("断开");
                cmbOutStatus.Items.Add("闭合");
                if (index1 >= 0)
                    cmbOutStatus.SelectedIndex = index1;


                groupBox19.Text = "协议";
                label35.Text = "协议";

                label2.Text = "设置Gen2之前先获取";
                groupBox6.Text = "功率";
                label24.Text = "输出功率:";
                //cbSave.Text = "保存";


                groupBox11.Text = "区域";
                label1.Text = "区域:";
                cbRegionSave.Text = "保存";

                label5.Text = "链路组合:";
                groupBox3.Text = "链路";
                cbRFLink.Text = "保存";

                groupBox7.Text = "定频";
                label28.Text = "频点:";

             

                gbAnt.Text = "天线";
     

                groupBox1.Text = "连续波";


                label39.Text = "输入1:";
                label40.Text = "输入2:";
                label38.Text = "继电器:";

                label24.Location = new Point(30, 34);
                //      label1.Location = new Point(63, 39);
                //      label5.Location = new Point(40, 33);
                //       label28.Location = new Point(55, 42);

             

                gbIP.Text = "本地IP";
                gbIp2.Text = "目标IP";
                //  label9.Text = label31.Text = "IP地址:";
                //  label25.Text = label30.Text = "端口号:";
                groupBox9.Text = "蜂鸣器";


                int index = workMode.SelectedIndex == -1 ? 0 : workMode.SelectedIndex;
                workMode.Items.Clear();
                workMode.Items.Add("命令工作模式");
                workMode.Items.Add("自动工作模式");
                workMode.Items.Add("触发模式");
                workMode.SelectedIndex = index;

                gbInventoryMode.Text = "盘点模式";
                label45.Text = "模式        :";
                label46.Text = "User起始地址:";
                label47.Text = "User长度    :";
            }
        }


        private Hashtable frequencyBandHashtable = null;
        private void LoadRegionConfig()
        {
            
            string path = System.Environment.CurrentDirectory + "\\frequencyBand.txt";
            if (File.Exists(path))
            {
                List<string> list = FileManage.ReadFileToArr(path);
                if (list.Count == 0)
                {
                    return;
                }
                frequencyBandHashtable = new Hashtable();
                cmbRegion.Items.Clear();
                for (int k = 0; k < list.Count; k++)
                {
                    string[] arr = list[k].Split(',');
                    frequencyBandHashtable.Add(arr[0], arr[1]);
                    cmbRegion.Items.Add(arr[0]);
                }
                cmbRegion.SelectedIndex = 0;
            }
 

        }

        private void LoadFrequencyPoint()
        {

            string path = System.Environment.CurrentDirectory + "\\frequencyPoint.txt";
            if (File.Exists(path))
            {
                List<string> list = FileManage.ReadFileToArr(path);
                if (list.Count == 0)
                {
                    return;
                }
                comboBox1.Items.Clear();
                for (int k = 0; k < list.Count; k++)
                {
                    comboBox1.Items.Add(list[k]);
                }
                comboBox1.SelectedIndex = 0;
            }


        }

        private void btnGetAllPower_Click(object sender, EventArgs e)
        {
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            byte[] power = uhf.GetAntennaAllPower();
            if (power != null && power.Length > 0)
            {
                for (int k = 1; k <= power.Length; k++)
                {
                    switch (k)
                    {
                        case 1:
                            cmbPower1.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 2:
                            cmbPower2.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 3:
                            cmbPower3.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 4:
                            cmbPower4.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 5:
                            cmbPower5.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 6:
                            cmbPower6.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 7:
                            cmbPower7.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 8:
                            cmbPower8.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 9:
                            cmbPower9.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 10:
                            cmbPower10.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 11:
                            cmbPower11.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 12:
                            cmbPower12.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 13:
                            cmbPower13.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 14:
                            cmbPower14.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 15:
                            cmbPower15.SelectedIndex = power[k - 1] - 1;
                            break;
                        case 16:
                            cmbPower16.SelectedIndex = power[k - 1] - 1;
                            break;
                    }
                }
                msg = !IsChineseSimple() ? "Correcto!" : "成功!";
            }
            showMessage(msg);
        }

        private void btnSetAllPower_Click(object sender, EventArgs e)
        {
            List<ComboBox> list = new List<ComboBox>();
            list.Add(cmbPower1);
            list.Add(cmbPower2);
            list.Add(cmbPower3);
            list.Add(cmbPower4);
            list.Add(cmbPower5);
            list.Add(cmbPower6);
            list.Add(cmbPower7);
            list.Add(cmbPower8);
            list.Add(cmbPower9);
            list.Add(cmbPower10);
            list.Add(cmbPower11);
            list.Add(cmbPower12);
            list.Add(cmbPower13);
            list.Add(cmbPower14);
            list.Add(cmbPower15);
            list.Add(cmbPower16);
            string msg = !IsChineseSimple() ? "Error!" : "失败!";
            bool result = false;
            byte save =cbSaveAllPower.Checked?(byte)1: (byte)0;

            for (int k = 0; k < list.Count; k++) {
                ComboBox cb = list[k];
                if (cb.SelectedIndex >= 0)
                {
                    int ant = k + 1;
                    int Power= cb.SelectedIndex + 1;
                    result = uhf.SetAntennaPower(save,(byte)ant, (byte)Power);
                    if (!result)
                    {
                        showMessage(msg);
                        return;
                    }
                }

            }
            if (!result)
            {
                showMessage(msg);
                return;

            }
            msg = !IsChineseSimple() ? "Correcto!" : "成功!";
            showMessage(msg);
        }

        private void cbDedebug_Click(object sender, EventArgs e)
        {
                UHFAPI.getInstance().SetDebug(cbDedebug.Checked);
                UHFAPI.getInstance().SaveLog(cbDedebug.Checked);
        }

    }
}
