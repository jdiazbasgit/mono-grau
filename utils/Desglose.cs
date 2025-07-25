using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Desglose
    {

        public string dataContext { get; set; }
        public string dataTag { get; set; }
        public string systemId { get; set; }
        public string lecturaRfid { get; set; }
        public string noProducto { get; set; }
        public string noLote { get; set; }
        public string codExplorer { get; set; }
        public string noSerie { get; set; }
        public string idImplantCard { get; set; }

        public Desglose(string dataContext, string dataTag, string systemId, string lecturaRfid, string noProducto, string noLote, string codExplorer, string noSerie, string idImplantCard)
        {
            this.dataContext = dataContext;
            this.dataTag = dataTag;
            this.systemId = systemId;
            this.lecturaRfid = lecturaRfid;
            this.noProducto = noProducto;
            this.noLote = noLote;
            this.codExplorer = codExplorer;
            this.noSerie = noSerie;
            this.idImplantCard = idImplantCard;
        }
    }
}
