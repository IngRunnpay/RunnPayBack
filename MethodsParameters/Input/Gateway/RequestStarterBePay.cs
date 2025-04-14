using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Gateway
{
    public class RequestStarterBePay
    {
        public string? RECOMMENDATION { get; set; }
        public string? status { get; set; }
        public string? paymentmethod { get; set; }
        public object? qr_type { get; set; }
        public string? transaction_ide { get; set; }
        public string? transacton_ide { get; set; }
        public int? transaction_id { get; set; }
        public int? transacton_id { get; set; }
        public string? transaction_total { get; set; }
        public string? transacton_total { get; set; }
        public string? transaction_tax { get; set; }
        public string? transaction_description { get; set; }
        public string? transaction_extra1 { get; set; }
        public object? transaction_extra2 { get; set; }
        public object? transaction_extra3 { get; set; }
        public object? transaction_extra4 { get; set; }
        public object? transaction_extra5 { get; set; }
        public string? traceability_code { get; set; }
        public string? started_at { get; set; }
        public string? processed_at { get; set; }
        public string? payer_name { get; set; }
        public string? payer_document { get; set; }
        public string? payer_address { get; set; }
        public string? payer_phone { get; set; }
        public string? payer_email { get; set; }
        public string? payer_ip { get; set; }
        public int? account_id { get; set; }
        public string? financial_entity { get; set; }
        public object? details { get; set; }
        public bool? signature { get; set; }
    }
}
