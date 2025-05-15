using System;


namespace UHFAPP.utils
{
    internal class GrabacionDTO
    {
        

        public String orden { get; set; }

        

        public String tag { get; set; }

        public int linea { get; set; }

     


        public GrabacionDTO( string orden, string tag, int linea)
        {
            
            this.orden = orden;
            this.tag = tag;
            this.linea = linea;
          
        }   
        
    }
}
