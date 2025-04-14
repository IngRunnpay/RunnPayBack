using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Transaccion
{
    public class ResponseSp_GetDataTransaccion
    {
        public int IdTransaccion {  get; set; }
        public string Moneda { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoFinal {  get; set; }
        public string Referencia {  get; set; }
        public string DescripcionCompra { get; set; }
        public DateTime? FechaVencimiento {  get; set; }
        public string Documento { get; set; }
        public string UsuDocumento {  get; set; }
        public string UsuTelefono { get; set; }
        public string UsuCorreo { get; set; }
        public string UsuNombre { get; set; }
        public string Impuesto { get; set; }
        public int IdEstadoTransaccion { get; set; }
        public string UrlClient { get; set; }
        public decimal ValorImpuesto { get; set; }


    }
}
