using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace FileDownload
{
    /// <summary>
    /// 文件下载类
    /// </summary>
    public class Downloader
    {
        public delegate void DelProInfoArg(FileInfo _file);
        public delegate void DeldownloadEndArg();
        public delegate void DeldownloadException(Exception ex);
        /// <summary>
        /// 开始下载文件，事件
        /// </summary>
        public event DelProInfoArg DownloadStartingEvent;
        /// <summary>
        /// 分包下载 事件(通知显示更新进度)
        /// </summary>
        public event DelProInfoArg DownloadedEvent;
        /// <summary>
        /// 一个文件下载结束,事件
        /// </summary>
        public event DelProInfoArg DownloadEndEvent;
        /// <summary>
        /// 所有的文件下载结束，事件
        /// </summary>
        public event DeldownloadEndArg DownloadALLEnd;

        /// <summary>
        /// 所有的文件下载结束，事件
        /// </summary>
        public event DeldownloadException DownloadException;

        private List<FileInfo> _waitDownloadFiles;

        /// <summary>
        /// 下载一批文件文件
        /// </summary>
        /// <param name="list">文件列表</param>
        public Downloader(List<FileInfo> list)
        {
            _waitDownloadFiles = list;
        }
        public Downloader(FileInfo file)
        {
            List<FileInfo> list=new List<FileInfo>();
            list.Add(file);
            _waitDownloadFiles = list;
        }
        /// <summary>
        /// 开始下载
        /// </summary>
        public void Start()
        {
            System.Threading.Thread DownloadThread = new System.Threading.Thread(new System.Threading.ThreadStart(StartDownload));
            DownloadThread.IsBackground = true;
            DownloadThread.Start();
        }

        /// <summary>
        /// 线程启动，开始获取数据包
        /// </summary>
        private void StartDownload()
        {
            if (_waitDownloadFiles != null && _waitDownloadFiles.Count > 0)
            {

                int osize = 0;
                int StartTime = Environment.TickCount;
                foreach (FileInfo file in _waitDownloadFiles)
                {
                    if (DownloadStartingEvent != null)//开始下载
                    {
                        DownloadStartingEvent(file);
                    }
                    try
                    {
                        #region 此处加上这一段是为了能够正常下载https URL的文件，如果不加上这段服务器验证的回调并返回true,则下载https文件时会发生错误，会提示服务器无法通过验证，无法正常下载

                        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                        
                        #endregion

                        System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(file.FileDownloadPath);
                        System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                        file.SumSize = myrp.ContentLength;
           
                        System.IO.Stream st = myrp.GetResponseStream();
                        System.IO.Stream so = new System.IO.FileStream(file.FileSavePath+"\\"+ file.FileName, System.IO.FileMode.Create);
                        
                        byte[] by = new byte[1024];

                        osize = st.Read(by, 0, (int)by.Length);

                        while (osize > 0)
                        {
                            file.DownloadedSize = osize + file.DownloadedSize;

                            so.Write(by, 0, osize);
                         
                            osize = st.Read(by, 0, (int)by.Length);

                            if (Environment.TickCount - StartTime > 1000)
                            {
                                StartTime = Environment.TickCount;
                                if (DownloadedEvent != null)//获取一段数据包，更新显示
                                {
                                    DownloadedEvent(file);
                                }
                            }

                        }
                        so.Close();
                        st.Close();

                        if (DownloadEndEvent != null)//一个文件下载完成
                        {
                            DownloadEndEvent(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        // throw ex;
                        if (DownloadException != null)//一个文件下载完成
                        {
                            DownloadException(ex); 
                        }
                    }
                }

                if (DownloadALLEnd != null)//下载结束
                {
                    DownloadALLEnd();
                }
            }
            else
            {
                if (DownloadALLEnd != null)//下载结束
                {
                    DownloadALLEnd();
                }
            }
        }

        /// <summary>
        /// 自定义的服务器证书验证回调方法，这里始终认为存在证书可以下载,返回True
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        protected bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
