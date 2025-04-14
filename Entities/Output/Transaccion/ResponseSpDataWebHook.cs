using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Transaccion
{
    public class ResponseSpDataWebHook
    {
        public int IdTransaccion {  get; set; }
        public string Descripcion {  get; set; }
        public int IdEstadoTransaccion { get; set; }
        public string Mensaje {  get; set; }
        public string UrlDelivery { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}
