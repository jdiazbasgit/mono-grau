using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UHFAPP.utils
{
    public class OrigenOrden
    {

        public string dataContext { get; set; }
        public UHFAPP.utils.segundos.Value[] value { get; set; }

        public OrigenOrden(string dataContext, UHFAPP.utils.segundos.Value[] value)
        {
            this.dataContext = dataContext;
            this.value = value;
        }


    }
}
