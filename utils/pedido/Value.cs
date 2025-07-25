using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils.segundos
{
    public class Value
    {
       
        public string no { get; set; }
        public string sourceNo { get; set; }
        public string auxiliaryIndex1 { get; set; }

        public 
            Value( string no, string sourceNo, string auxiliaryIndex1)
        {
           
            this.no = no;
            this.sourceNo = sourceNo;
            this.auxiliaryIndex1 = auxiliaryIndex1;
        }
    }
}
