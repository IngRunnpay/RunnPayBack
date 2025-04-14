using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Input.Transaccion
{
    public class SpGetHistoriTransaction
    {
        public int IdTransaccion { get; set; }
        [JsonIgnore]
        public int IdUsuario {  get; set; }
    }
}
