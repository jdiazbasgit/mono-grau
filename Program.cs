using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Text;

namespace UHFAPP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {

                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new MainForm());
                //处理未捕获的异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                glExitApp = true;//标志应用程序可以退出
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        static bool glExitApp = false;

        /// <summary>
        /// 处理未捕获异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            SaveLog("-----------------------begin--------------------------");
            SaveLog("CurrentDomain_UnhandledException" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            SaveLog("IsTerminating : " + e.IsTerminating.ToString());
            SaveLog(e.ExceptionObject.ToString());
            SaveLog("-----------------------end----------------------------");
            while (true)
            {//循环处理，否则应用程序将会退出
                if (glExitApp)
                {//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                    SaveLog("ExitApp");
                    return;
                }
                System.Threading.Thread.Sleep(2 * 1000);
            };
        }

        /// <summary>
        /// 处理UI主线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            SaveLog("-----------------------begin--------------------------");
            SaveLog("Application_ThreadException:" + e.Exception.Message);
            SaveLog(e.Exception.StackTrace);
            SaveLog("-----------------------end----------------------------");
        }

        public static void SaveLog(string log)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\error.txt";
            //采用using关键字，会自动释放
            using (FileStream fs = new FileStream(filePath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine(log);
                }
            }

        }
    }
}
