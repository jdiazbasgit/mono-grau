using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UHFAPP
{
    public partial class BaseForm : Form
    {

        //当前操作系统是否为简体中文
        public static bool IsChineseSimple()
        {
            return   System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN";
        }

        public UHFAPI uhf = null;
        public BaseForm()
        {
            InitializeComponent();
            uhf = UHFAPI.getInstance();

        }
 
    }
}
