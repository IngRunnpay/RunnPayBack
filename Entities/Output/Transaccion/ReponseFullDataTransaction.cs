using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Transaccion
{
    public class ReponseFullDataTransaction
    {
        public string IdTransaccion { get; set; }
        public string IdMediosPago { get; set; }
        public string IdEstadoTransaccion { get; set; }
        public string Monto { get; set; }
        public string IdTax { get; set; }
        public string MontoFinal { get; set; }
        public string Referencia { get; set; }
        public string DescripcionCompra { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdMoneda { get; set; }
        public int UsuTipoDocumento { get; set; }
        public string UsuDocumento { get; set; }
        public string UsuTelefono { get; set; }
        public string UsuCorreo { get; set; }
        public string Url { get; set; }
        public int IdUsuario { get; set; }
        public string UsuNombre { get; set; }
        public string ReferenciaExterno { get; set; }
        public string UrlClient { get; set; }

    }
}
