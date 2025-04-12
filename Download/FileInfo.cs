using System;
using System.Collections.Generic;
using System.Text;

namespace FileDownload
{
    /// <summary>
    /// 需要下载的文件信息
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_dowPath">下载路径</param>
        /// <param name="_savPath">文件保存路径</param>
        /// <param name="_fileName">文件名称</param>
        public FileInfo(string _dowPath, string _savPath, string _fileName)
        {
            fileDownloadPath = _dowPath;
            fileSavePath = _savPath;
            fileName = _fileName;
        }

        /// <summary>
        /// 文件下载路径,http路径
        /// </summary>
        private string fileDownloadPath;
        /// <summary>
        /// 文件下载路径,http路径
        /// </summary>
        public string FileDownloadPath
        {
            get { return fileDownloadPath; }
            set { fileDownloadPath = value; }
        }

        /// <summary>
        /// 文件存储路径,如D:\test\1.txt
        /// </summary>
        private string fileSavePath;
        /// <summary>
        /// 文件存储路径,如D:\test\1.txt
        /// </summary>
        public string FileSavePath
        {
            get { return fileSavePath; }
            set { fileSavePath = value; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        private string fileName;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        /// <summary>
        /// 文件总大小
        /// </summary>
        private long sumSize;
        /// <summary>
        /// 文件总大小
        /// </summary>
        public long SumSize
        {
            get { return sumSize; }
            set { sumSize = value; }
        }
        /// <summary>
        /// 已下载文件大小
        /// </summary>
        private long downloadedSize;
        /// <summary>
        /// 已下载文件大小
        /// </summary>
        public long DownloadedSize
        {
            get { return downloadedSize; }
            set { downloadedSize = value; }
        }
    }
}
