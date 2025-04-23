using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class GrabacionMozo
    {
        private string lecturaRFID { get; set; }
        private string noOrdenProduccion { get; set; }
        private int noLinOrdenProducc { get; set; }
        private int cantidad { get; set; }

        public GrabacionMozo(string lecturaRFID, string noOrdenProduccion, int noLinOrdenProducc, int cantidad)
        {
            this.lecturaRFID = lecturaRFID;
            this.noOrdenProduccion = noOrdenProduccion;
            this.noLinOrdenProducc = noLinOrdenProducc;
            this.cantidad = cantidad;
        }
    }
}
