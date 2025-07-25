using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class BajaOut
    {
        public string lecturaRFID { get; set; }
        public string noDocumento { get; set; }
        public bool comprobarErrores { get; set; }
        public string textoError { get; set; }

        public BajaOut(string lecturaRFID, string noDocumento, bool comprobarErrores, string textoError)
        {
            this.lecturaRFID = lecturaRFID;
            this.noDocumento = noDocumento;
            this.comprobarErrores = comprobarErrores;
            this.textoError = textoError;
        }
    }
}
