using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Grabacion
    {
        public int id  { get; set; }
        public Orden orden { get; set; }
        public string tag { get; set; }
        
        public int lote { get; set; }

      
        public DateTime fecha { get; set; }


        public Grabacion(int id, Orden orden, string tag, int lote, DateTime fecha)
        {
            this.id = id;
            this.orden = orden;
            this.tag = tag;
            
            this.lote = lote;
            this.fecha = fecha;
        }
        public Grabacion()
        {
        }
    }
    
}
