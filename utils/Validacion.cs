using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Validacion
    {
        public string dataContext { get; set; }
        public string dataTag { get; set; }
        public string systemId { get; set; }
        public string lecturaRFID { get; set; }
        public string noProducto { get; set; }
        public string noLote { get; set; }
        public string codExplorer { get; set; }
        public string nSerie { get; set; }
        public string idImplantCard { get; set; }

        public Validacion(string dataContext, string dataTag, string systemId, string lecturaRFID, string noProducto, string noLote, string codExplorer, string nSerie, string idImplantCard)
        {
            this.dataContext = dataContext;
            this.dataTag = dataTag;
            this.systemId = systemId;
            this.lecturaRFID = lecturaRFID;
            this.noProducto = noProducto;
            this.noLote = noLote;
            this.codExplorer = codExplorer;
            this.nSerie = nSerie;
            this.idImplantCard = idImplantCard;
        }
    }
}
