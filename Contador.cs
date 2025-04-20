namespace UHFAPP
{
    internal class Contador
    {
        private string orden { get; set; }
        private int lote { get; set; }
        private int cantidad { get; set; }
        public Contador(string orden,  int lote, int cantidad)
        {
            this.orden = orden;
            this.lote = lote;
            this.cantidad = cantidad;
        }
    }
}