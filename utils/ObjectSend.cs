using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Support;

namespace UHFAPP.utils
{
    internal class ObjectSend
    {
        string rutaUrl { get; set; }
        string metodo { get; set; }
        Object objeto { get; set; }
        HashMap<String ,String> parametros { get; set; }
        HashMap<String, String> cabeceras { get; set; }
        public ObjectSend(string rutaUrl, string metodo, Object objeto, HashMap<String, String> parametros, HashMap<String, String> cabeceras)
        {
            this.rutaUrl = rutaUrl;
            this.metodo = metodo;
            this.objeto = objeto;
            this.parametros = parametros;
            this.cabeceras = cabeceras;
        }
    }
}
