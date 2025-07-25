using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils
{
    internal class GrabacionRespuesta
    {
        public Grabacion[] grabaciones { get; set; }

        public GrabacionRespuesta(Grabacion[] grabaciones)
        {
            this.grabaciones = grabaciones;
        }
    }
}
