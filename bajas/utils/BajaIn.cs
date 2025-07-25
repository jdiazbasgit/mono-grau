using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class BajaIn
    {

        public string dataContext { get; set; }
        public string dataTag { get; set; }
        public string systemId { get; set; }
        public string lecturaRFID { get; set; }
        public string noDocumento { get; set; }
        public string comprobarErrores { get; set; }
        public string textoError { get; set; }

        public BajaIn(string dataContext, string dataTag, string systemId, string lecturaRFID, string noDocumento, string comprobarErrores, string textoError)
        {
            this.dataContext = dataContext;
            this.dataTag = dataTag;
            this.systemId = systemId;
            this.lecturaRFID = lecturaRFID;
            this.noDocumento = noDocumento;
            this.comprobarErrores = comprobarErrores;
            this.textoError = textoError;
        }
    }
}
