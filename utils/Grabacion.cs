using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Grabacion
    {
        public int id { get; set; }
        public Orden orden { get; set; }
        public string tag { get; set; }

        public int linea { get; set; }


        public DateTime fecha { get; set; }


        public Grabacion(int id, Orden orden, string tag, int linea, DateTime fecha)
        {
            this.id = id;
            this.orden = orden;
            this.tag = tag;
            this.linea = linea;
            this.fecha = fecha;
        }
        public Grabacion()
        {
        }
    }

}
