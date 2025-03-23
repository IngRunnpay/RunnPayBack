using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Dispersion
{
    public class CreateDispersionSaldo
    {
        [JsonIgnore]
        public string? IdAplicacion { get; set; }
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Banco { get; set; }
        public int TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string UsuNombre { get; set; }
        public string NotifyUrl { get; set; }
    }
}
