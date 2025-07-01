using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class OrdenInversaOut
    {
        public string lecturaRFID { get; set; }
        public bool comprobarErrores { get; set; }

        public string textoError { get; set; }

        public OrdenInversaOut(string lecturaRFID, bool comprobarErrores, string textoError)
        {
            this.lecturaRFID = lecturaRFID;
            this.comprobarErrores = comprobarErrores;
            this.textoError = textoError;
        }
    }
}
