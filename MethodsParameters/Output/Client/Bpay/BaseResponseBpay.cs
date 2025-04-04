using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Output.Client.Bpay
{
    public class BaseResponseBpay
    {
        public bool success { get; set; }
        public string data { get; set; }
        public List<object> meta { get; set; }
        public string message { get; set; }
    }
}
