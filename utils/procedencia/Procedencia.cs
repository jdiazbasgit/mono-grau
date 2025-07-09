using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils.procedencia
{
    public class Procedencia
    {
       public  string dataContext { get; set; }
       public Value[] value { get; set; }
        
        public Procedencia(string dataContext, Value[] value)
        {
            this.dataContext = dataContext;
            this.value = value;
        }   
    }
}
