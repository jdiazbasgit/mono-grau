using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    public class Biocam
    {
        public string dataContext { get; set; }

        public utils.biocam.Value[] value { get; set; }
        public Biocam(string dataContext, utils.biocam.Value[] value)
        {
            this.dataContext = dataContext;
            this.value = value;
        }

    }
}
