using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Peticion
    {
        private string orden { get; set; }
        private int lote { get; set; }
        public Peticion(string orden,  int lote)
        {
            this.orden = orden;
            this.lote = lote;
        }
    }
   
}
