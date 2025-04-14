using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Otp
{
    public class ResponseSp_GetOtp
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string CodigoOtp {  get; set; }
        public bool Validado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaMaxValidacion { get; set; }
        public DateTime? FechaValidacion { get; set; }
        public string Data {  get; set; }
    }
}
