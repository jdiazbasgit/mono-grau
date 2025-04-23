using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class GrabacionDTO
    {
        public int cantidad { get; set; }

        public String orden { get; set; }

        public int lote { get; set; }

        public String tag { get; set; }
        public GrabacionDTO(int cantidad, String orden, int lote, string tag)
        {
            this.cantidad = cantidad;
            this.orden = orden;
            this.lote = lote;
            this.tag = tag;
        }
    }
}
