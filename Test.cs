using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UHFAPP
{
    public class Test
    {

        private bool isRuning = false;
        public void main()
        {
            //Step 1: connect to UR4
            //Connect to UR4 via network
            int result = TCPConnect(new StringBuilder("192.168.99.202"), 8888);
            if (result != 0)
            {
                //connect fail
                return;
            }
            /*
            //Connect to UR4 via serial port.
            result = ComOpen(1);//COM1
            //Disconnect --> ClosePort();
            if (result != 0)
            {
                //connect fail
                return;
            }
           //Connect to R3 via USB.
            result = UsbOpen();
            //Disconnect --> UsbClose();
            if (result != 0)
            {
                //connect fail
                return;
            }
            */
            //connect success
            UHFAPI.SetDisconnectCallback(OnDisconnect);

            //Step 2: set parameters
            byte save = 1;
            if (UHFSetPower(save, 30) == 0)
            {
                //success
            }
            //........
            //........
            //Step 3: Start Inventory
            if (UHFInventory() == 0)
            {
                isRuning = false;
                new Thread(new ThreadStart(delegate { ReadEPC(); })).Start();
                //success
            }
            //........
            //Stop Inventory
            if (UHFStopGet() == 0)
            {
                //success
                isRuning = false;
            }
            //Step 4: Disconnect
            TCPDisconnect();

        }




        /// <summary>
        /// USB
        /// </summary>
        /// <returns></returns>
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int UsbOpen();
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void UsbClose();
        /// <summary>
        /// TCP/IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="hostport"></param>
        /// <returns></returns>
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int TCPConnect(StringBuilder ip, uint hostport);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void TCPDisconnect();
        /// <summary>
        /// serial port
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int ComOpen(int comName);
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void ClosePort();

        /// <summary>
        ///  disconnect callback
        /// </summary>
        /// <param name="type"></param>
        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate void OnDisconnectCallback(int type);
        [DllImport("UHFAPI.dll", EntryPoint = "SetDisconnectCallback", CallingConvention = CallingConvention.Cdecl)]
        public extern static int SetDisconnectCallback(OnDisconnectCallback disconnectCallback);
        /// <summary>
        /// set power
        /// </summary>
        /// <param name="save">1:save</param>
        /// <param name="uPower">1-30</param>
        /// <returns></returns>
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFSetPower(byte save, byte uPower);
        /// <summary>
        ///Start Inventory
        /// </summary>
        /// <returns></returns>
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFInventory();
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHFStopGet();
        /**********************************************************************************************************
        * if(epcMode)
        * {
        *     data =  1byte PcAndEPC len + PcAndEPC data + 1byte TID len + 2byte Rssi +1byte ANT
        * }
        * else if(epcAndTidMode)
        * {
        *     data =  1byte PcAndEPC len + PcAndEPC data + 1byte TID len + 12bytes Tid data + 2byte Rssi +1byte ANT
        * }
        * else if(epcAndTidUser)
        * {
        *     data =  1byte PcAndEPC len + PcAndEPC data + 1byte TIDAndUser len + 12bytes Tid data+ UserData + 2byte Rssi +1byte ANT
        * }
       *********************************************************************************************************/
        [DllImport("UHFAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int UHF_GetReceived_EX(ref int uLenUii, byte[] data);

        public class UHFTAGInfo
        {
            public string Pc { get; set; }
            public string Epc { get; set; }
            public string Tid { get; set; }
            public string User { get; set; }
            public string Ant { get; set; }
            public string Rssi { get; set; }
            public string Sensor { get; set; }

        }
        /// <summary>
        /// 获取缓冲区的标签数据
        /// Get tag data from buffer
        /// 
        ///  {@link #StartInventory() }启动识别标签之后，在子线程循环调用此函数不断获取缓冲区的标签信息，每次返回一张标签数据
        ///  {@link #startInventoryTag() } After tag reading has been enabled, call this function in sub threads to get data information continously, return one tag information for each time.<br>
        /// </summary>
        /// <returns></returns>
        public UHFTAGInfo ReadTagFromBuffer()
        {
            int uLen = 0;
            byte[] bufData = new byte[256];
            if (UHF_GetReceived_EX(ref uLen,bufData)==0)
            {
                string epc_data = string.Empty;
                string uii_data = string.Empty;//uii 
                string tid_data = string.Empty; //tid 
                string rssi_data = string.Empty;
                string ant_data = string.Empty;
                string user_data = string.Empty;

                int uii_len = bufData[0];//uii 
                int tid_leng = bufData[uii_len + 1];//tid 
                int tid_idex = uii_len + 2;//tid 
                int rssi_index = 1 + uii_len + 1 + tid_leng;
                int ant_index = rssi_index + 2;

                string strData = BitConverter.ToString(bufData, 0, uLen).Replace("-", "");
                epc_data = strData.Substring(6, uii_len * 2 - 4);  //Epc

                if (tid_leng > 12)
                {
                    tid_data = strData.Substring(tid_idex * 2, 24); //Tid
                    user_data = strData.Substring(tid_idex * 2 + 24, (tid_leng - 12) * 2); //Tid
                }
                else
                {
                    tid_data = strData.Substring(tid_idex * 2, tid_leng * 2); //Tid
                    if (tid_data.Length < 8)
                    {
                        tid_data = "";
                    }
                }

                string temp = strData.Substring(rssi_index * 2, 4);
                int rssiTemp = Convert.ToInt32(temp, 16) - 65535;
                rssi_data = ((float)rssiTemp / 10.0).ToString();// RSSI  =  (0xFED6   -65535)/10
                if (!rssi_data.Contains("."))
                {
                    rssi_data = rssi_data + ".0";
                }
                ant_data = Convert.ToInt32((strData.Substring(ant_index * 2, 2)), 16).ToString();

                UHFTAGInfo info = new UHFTAGInfo();
                info.Epc = epc_data;
                info.Tid = tid_data;
                info.Rssi = rssi_data;
                info.Ant = ant_data;
                info.User = user_data;

                return info;
            }
            else
            {
                return null;
            }
        }
         
        private void OnDisconnect(int id)
        {
            System.Console.WriteLine("OnDisconnect");
        }
        private void ReadEPC()
        {
            while (isRuning)
            {
                UHFTAGInfo info = ReadTagFromBuffer();
                if (info != null)
                {
                    /*
                    info.Epc;
                    info.Tid;
                    info.User;
                    info.Ant;
                    info.Rssi;
                    */
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }
    }
}
