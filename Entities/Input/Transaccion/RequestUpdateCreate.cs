using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Input.Transaccion
{
    public class RequestUpdateCreate
    {
        public int IdTransaccion {  get; set; }
        public decimal MontoFinal {  get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public int IdUsuario { get; set; }
    }
}
