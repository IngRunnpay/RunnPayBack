using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Gateway
{
    public class RequestCreatePseTpago
    {
        public string bank_code { get; set; }
        public string order_id { get; set; }
        public string amount { get; set; }
        public string vat_amount { get; set; }
        public string description { get; set; }
        public string user_type { get; set; }
        public string buyer_email { get; set; }
        public string buyer_full_name { get; set; }
        public string document_type { get; set; }
        public string document_number { get; set; }
        public string redirect_url { get; set; }
        public string buyer_phone_number { get; set; }
    }

}
