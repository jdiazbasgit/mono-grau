using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Value
    {
        public string dataTag { get; set; }
        public string systemId { get; set; }
        public string no { get; set; }
        public string description { get; set; }
        public string gtin { get; set; }
        public string codLineaCatalogo { get; set; }

        public Value(string dataTag, string systemId, string no, string description, string gtin, string codLineaCatalogo)
        {
            this.dataTag = dataTag;
            this.systemId = systemId;
            this.no = no;
            this.description = description;
            this.gtin = gtin;
            this.codLineaCatalogo = codLineaCatalogo;
        }
    }
}
