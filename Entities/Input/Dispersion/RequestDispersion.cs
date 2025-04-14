using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Dispersion
{
   
    public class RequestDispersion
    {
        public int TipoDocumento {  get; set; }
        public string NumeroDocumento { get; set; }
        public string Banco {  get; set; }
        public int TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public List<DataDispersion> Data {  get; set; }
    }

    public class DataDispersion
    {
        public int IdTransaccion { get; set; }
        public string Referencia { get; set; }
    }
}
