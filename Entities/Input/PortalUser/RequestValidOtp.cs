using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.PortalUser
{
    public class RequestValidOtp
    {
        public string Correo { get; set; }
        public string Otp {  get; set; }
    }
}
