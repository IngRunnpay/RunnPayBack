using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Output.Dispersion
{
    public class ResponseGetPyOut
    {
        public int IdDispersion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Banco { get; set; }
        public string TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string UsuNombre { get; set; }
        public string NotifyUrl { get; set; }
        public string Estado { get; set; }
        public string IdAplicacion {  get; set; }
    }
}
