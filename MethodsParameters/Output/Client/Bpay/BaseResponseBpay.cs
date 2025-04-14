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
        public Data data { get; set; }
        public List<object> meta { get; set; }
        public string message { get; set; }
    }

    public class Data
    {
        public string? ide { get; set; }
        public string? total { get; set; }
        public string? link { get; set; }
        public object? qr { get; set; }
        public string? pseReturnCode { get; set; }
        public string? trazabilityCode { get; set; }
        public string? status { get; set; }


    }
    public class ResponseLogin
    {
        public bool success { get; set; }
        public string data { get; set; }
        public List<object> meta { get; set; }
        public string message { get; set; }
    }

}
