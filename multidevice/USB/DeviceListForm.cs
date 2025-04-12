using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UHFAPP.USB.multidevice
{
    public partial class DeviceListForm : Form
    {


        public DeviceListForm(List<string> list)
        {
            InitializeComponent();
            for (int k = 0; k < list.Count; k++)
            {
                comboBox1.Items.Add(list[k]);
            }
            
        }


        public string ChipId = null;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChipId = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            this.Close();
        }
    }
}
