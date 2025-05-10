using System;


namespace UHFAPP.utils
{
    internal class GrabacionDTO
    {
        public int cantidad { get; set; }

        public String codigo { get; set; }

        

        public String tag { get; set; }

        public int linea { get; set; }

     


        public GrabacionDTO(int cantidad, string codigo, string tag, int linea)
        {
            this.cantidad = cantidad;
            this.codigo = codigo;
            this.tag = tag;
            this.linea = linea;
          
        }   
        
    }
}
