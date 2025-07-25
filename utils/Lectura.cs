using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UHFAPP.utils
{
    internal class Lectura
    {
        public string epc { get; set; }
        public int lote { get; set; }
        public int linea { get; set; }

        public Lectura(string epc, int lote, int linea)
        {
            this.epc = epc;
            this.lote = lote;
            this.linea = linea;
        }

    }
}
