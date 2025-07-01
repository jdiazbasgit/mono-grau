using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class BajaAmipemIn
    {

        public int id { get; set; }
        public Grabacion grabacion { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }

        public BajaAmipemIn(int id, Grabacion grabacion, string descripcion, string fecha)
        {
            this.id = id;
            this.grabacion = grabacion;
            this.descripcion = descripcion;
            this.fecha = fecha;
        }


    }
}
