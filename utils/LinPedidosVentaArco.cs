namespace UHFAPP.utils
{
    public class LinPedidosVentaArco
    {
       
        public string odataETag { get; set; }
        public string systemId { get; set; }
        public string documentNo { get; set; }
        public string lineNo { get; set; }
        public string type { get; set; }
        public string no { get; set; }
        public string description { get; set; }
        public string locationCode { get; set; }
        public string itemCategoryCode { get; set; }
        public string outstandingQuantity { get; set; }
        public string codPromocion { get; set; }
        public string[] vinculosEnsamblarPedidoArco { get; set; }

        public LinPedidosVentaArco(string odataETag, string systemId, string documentNo, string lineNo, string type, string no, string description, string locationCode, string itemCategoryCode, string outstandingQuantity, string codPromocion, string[] vinculosEnsamblarPedidoArco)
        {
            this.odataETag = odataETag;
            this.systemId = systemId;
            this.documentNo = documentNo;
            this.lineNo = lineNo;
            this.type = type;
            this.no = no;
            this.description = description;
            this.locationCode = locationCode;
            this.itemCategoryCode = itemCategoryCode;
            this.outstandingQuantity = outstandingQuantity;
            this.codPromocion = codPromocion;
            this.vinculosEnsamblarPedidoArco = vinculosEnsamblarPedidoArco;
        }

    }
}