using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Client.BPay
{
    public class RequestPaymentContinueBePay
    {
        public string direccion { get; set; }
        public string apellidos { get; set; }
        public int ciudad { get; set; }
        public int pais { get; set; }
        public string company_terms { get; set; }
        public string email { get; set; }
        public string nombres { get; set; }
        public string numero_documento { get; set; }
        public string telefono { get; set; }
        public int tipo_documento { get; set; }
        public string transaction_id { get; set; }
        public string transaction_ip { get; set; }
        public string codigo_postal { get; set; }
        public string metodopago { get; set; }
        public string Token { get; set; }
        public int account_id { get; set; }
        public string redirect_url { get; set; }
        public string nequi_push_phone { get; set; }  //Obligatorio para nequiPush
        public string typeuser { get; set; }  //Obligatorio para PSE
        public int bank { get; set; }  //Obligatorio para PSE

    }
}
