using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Webhook
{
    public class RequesWebHook
    {
        public string? idTransaccion { get; set; }
        public string? descripcionEstado { get; set; }
        public string? idEstado { get; set; }
        public string? mensaje { get; set; }
    }
}
