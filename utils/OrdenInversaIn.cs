using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class OrdenInversaIn
    {
        public string dataContext { get; set; }
        public string dataTag { get; set; }
        public string systemId { get; set; }
        public string lecturaRFID { get; set; }
        public bool comprobarErrores { get; set; }

        public string textoError { get; set; }

        public OrdenInversaIn(string dataContext, string dataTag, string systemId, string lecturaRFID, bool comprobarErrores, string textoError)
        {
            this.dataContext = dataContext;
            this.dataTag = dataTag;
            this.systemId = systemId;
            this.lecturaRFID = lecturaRFID;
            this.comprobarErrores = comprobarErrores;
            this.textoError = textoError;
        }
    }
}
