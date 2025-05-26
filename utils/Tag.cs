using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Tag
    {
        public string lecturaRFID { get; set; }

        public Tag(string lecturaRFID)
        {
            this.lecturaRFID = lecturaRFID;
        }
    }
}
