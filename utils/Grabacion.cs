using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Grabacion
    {
        private int id  { get; set; }
        private Orden orden { get; set; }
        private string tag { get; set; }
        private int lote { get; set; }
       
        public Grabacion(int id, Orden orden, string tag, int lote)
        {
            this.id = id;
            this.orden = orden;
            this.tag = tag;
            this.lote = lote;
        }
    }
    
}
