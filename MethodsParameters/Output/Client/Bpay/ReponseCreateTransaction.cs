using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Output.Client.Bpay
{
    public class ReponseCreateTransaction
    {
        public string ide { get; set; }
        public string total { get; set; }
        public string link { get; set; }
        public object qr { get; set; }
    }
}
