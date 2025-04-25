using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class GrabacionMozo
    {
        public string lecturaRFID { get; set; }
        public string noOrdenProduccion { get; set; }
        public int noLinOrdenProducc { get; set; }
        public int cantidad { get; set; }

        public GrabacionMozo(string lecturaRFID, string noOrdenProduccion, int noLinOrdenProducc, int cantidad)
        {
            this.lecturaRFID = lecturaRFID;
            this.noOrdenProduccion = noOrdenProduccion;
            this.noLinOrdenProducc = noLinOrdenProducc;
            this.cantidad = cantidad;
        }
    }
}
