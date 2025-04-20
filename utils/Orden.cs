using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class Orden
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string item { get; set; }
        public int lotes { get; set; }
        public int cantidad { get; set; }

        public Orden(int id, string codigo, string descripcion, string item, int lotes, int cantidad)
        {
            this.id = id;
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.item = item;
            this.lotes = lotes;
            this.cantidad = cantidad;
        }
    }
}
