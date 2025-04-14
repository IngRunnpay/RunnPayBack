using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Client.BPay
{
    public class RequestCreateTransaction
    {
        public string type { get; set; }
        public string reference { get; set; }
        public string currency_code { get; set; }
        public string tax_percentage { get; set; }
        public string total { get; set; }
        public string description { get; set; }
        public string redirect_url { get; set; }
        public int account_id { get; set; }
        public string extra2 { get; set; }
        public string extra3 { get; set; }
    }
}
