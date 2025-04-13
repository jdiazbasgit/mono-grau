using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApodemiaC
{
    internal class Datos
    {
        private String  odataETag;
        private String systemId;
        private String prodOrderNo;
        private String lineNo;
        private String itemNo;
        private String description;
        private int remainingQtyBase;
        public int RemainingQtyBase { get => remainingQtyBase; set => remainingQtyBase = value; }
        public string Description { get => description; set => description = value; }
        public string ItemNo { get => itemNo; set => itemNo = value; }
        public string LineNo { get => lineNo; set => lineNo = value; }
        public string ProdOrderNo { get => prodOrderNo; set => prodOrderNo = value; }
        public string SystemId { get => systemId; set => systemId = value; }
        public string OdataETag { get => odataETag; set => odataETag = value; }
    }
}
