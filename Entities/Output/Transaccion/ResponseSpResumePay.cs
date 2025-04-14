using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Transaccion
{
    public class ResponseSpResumePay
    {
        public DateTime FechaCreacion {  get; set; }
        public string RazonSocial {  get; set; }
        public string UsuDocumento { get; set; }
        public string UsuTelefono {  get; set; }
        public string UsuCorreo {  get; set; }
        public string UsuNombre { get; set; }
        public string Referencia { get; set; }
        public string DescripcionCompra {  get; set; }
        public decimal MontoFinal {  get; set; }
        public decimal Monto {  get; set; }
        public string Impuesto {  get; set; }
        public int IdEstadoTransaccion {  get; set; }
    }
}
