using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApodemiaC
{
    internal class GetMozo
    {
        private String dataContext;
        private Datos[] value;


        public string DataContext { get => dataContext; set => dataContext = value; }
        public Datos[] Value { get => value; set => this.value = value; }
    }
}
