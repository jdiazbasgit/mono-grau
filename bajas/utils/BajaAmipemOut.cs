using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class BajaAmipemOut
    {
        public int id { get; set; }
        public string tag { get; set; }
        public string descripcion { get; set; }

        public BajaAmipemOut(int id, string tag, string descripcion)
        {
            this.id = id;
            this.tag = tag;
            this.descripcion = descripcion;
        }
    }
}
