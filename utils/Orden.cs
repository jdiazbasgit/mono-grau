using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class Orden
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string item { get; set; }
        public int cantidad { get; set; }
        public int parcial { get; set; }



        public Orden(int id, string codigo, string descripcion, string item, int cantidad,int parcial)
        {
            this.id = id;
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.item = item;
            this.parcial = parcial;
            this.cantidad = cantidad;
        }
        public Orden()
        {
        }
    }
}
