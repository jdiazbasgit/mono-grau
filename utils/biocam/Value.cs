using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHFAPP.utils.biocam
{
    public class Value
    {
        public string odataETag { get; set; }
        public string no { get; set; }
        public string sellToCustomerNo { get; set; }
        public string sellToCustomerName { get; set; }
        public string shipToCode { get; set; }
        public string noAsociado { get; set; }
        public string codDirecEnvioAsociado { get; set; }
        public string salespersonCode { get; set; }
        public string codDireccionEnvioDelegado { get; set; }
        public string shipToName { get; set; }
        public string shipToName2 { get; set; }
        public string shipToAddress { get; set; }
        public string shipToAddress2 { get; set; }
        public string shipToCity { get; set; }
        public string shipToContact { get; set; }
        public string shipToPostCode { get; set; }
        public string shipToCounty { get; set; }
        public string shipToPhoneNo { get; set; }
        public string shipToCountryRegionCode { get; set; }
        public string orderDate { get; set; }
        public string horaPedido { get; set; }
        public string shipmentDate { get; set; }
        public string shippingAgentCode { get; set; }
        public string shippingAgentServiceCode { get; set; }
        public string condicional { get; set; }
        public string bioCAM { get; set; }
        public string deposito { get; set; }
        public string devolucion { get; set; }
        public LinPedidosVentaArco[] linPedidosVentaArco { get; set; }
        public Value(string odataETag, string no, string sellToCustomerNo, string sellToCustomerName, string shipToCode, string noAsociado, string codDirecEnvioAsociado, string salespersonCode, string codDireccionEnvioDelegado, string shipToName, string shipToName2, string shipToAddress, string shipToAddress2, string shipToCity, string shipToContact, string shipToPostCode, string shipToCounty, string shipToPhoneNo, string shipToCountryRegionCode, string orderDate, string horaPedido, string shipmentDate, string shippingAgentCode, string shippingAgentServiceCode, string condicional, string bioCAM, string deposito, string devolucion, LinPedidosVentaArco[] linPedidosVentaArco)
        {
            this.odataETag = odataETag;
            this.no = no;
            this.sellToCustomerNo = sellToCustomerNo;
            this.sellToCustomerName = sellToCustomerName;
            this.shipToCode = shipToCode;
            this.noAsociado = noAsociado;
            this.codDirecEnvioAsociado = codDirecEnvioAsociado;
            this.salespersonCode = salespersonCode;
            this.codDireccionEnvioDelegado = codDireccionEnvioDelegado;
            this.shipToName = shipToName;
            this.shipToName2 = shipToName2;
            this.shipToAddress = shipToAddress;
            this.shipToAddress2 = shipToAddress2;
            this.shipToCity = shipToCity;
            this.shipToContact = shipToContact;
            this.shipToPostCode = shipToPostCode;
            this.shipToCounty = shipToCounty;
            this.shipToPhoneNo = shipToPhoneNo;
            this.shipToCountryRegionCode = shipToCountryRegionCode;
            this.orderDate = orderDate;
            this.horaPedido = horaPedido;
            this.shipmentDate = shipmentDate;
            this.shippingAgentCode = shippingAgentCode;
            this.shippingAgentServiceCode = shippingAgentServiceCode;
            {
                this.odataETag = odataETag;
                this.no = no;
                this.sellToCustomerNo = sellToCustomerNo;
                this.sellToCustomerName = sellToCustomerName;
                this.shipToCode = shipToCode;
                this.noAsociado = noAsociado;
                this.codDirecEnvioAsociado = codDirecEnvioAsociado;
                this.salespersonCode = salespersonCode;
                this.codDireccionEnvioDelegado = codDireccionEnvioDelegado;
                this.shipToName = shipToName;
                this.shipToName2 = shipToName2;
                this.shipToAddress = shipToAddress;
                this.shipToAddress2 = shipToAddress2;
                this.shipToCity = shipToCity;
                this.shipToContact = shipToContact;
                this.shipToPostCode = shipToPostCode;
                this.shipToCounty = shipToCounty;
                this.shipToPhoneNo = shipToPhoneNo;
                this.shipToCountryRegionCode = shipToCountryRegionCode;
                this.orderDate = orderDate;
                this.horaPedido = horaPedido;
                this.shipmentDate = shipmentDate;
                this.shippingAgentCode = shippingAgentCode;
                this.shippingAgentServiceCode = shippingAgentServiceCode;
                this.condicional = condicional;
                this.bioCAM = bioCAM;
                this.deposito = deposito;
                this.devolucion = devolucion;
                this.linPedidosVentaArco = linPedidosVentaArco;
            }
        }
    }
}
