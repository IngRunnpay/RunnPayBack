using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Otp
{
    public class RequestOtpLogin
    {
        public int IdUsuario { get; set; }
        public string CodigoOTP {get;set;}
        public string Data {  get; set; }
    }
}
