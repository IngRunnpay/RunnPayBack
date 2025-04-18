using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Dispersion
{
    public class ReponseGetClientConfig
    {
        public string Id {  get; set; }
        public string UserName { get; set; }
        public decimal PayOut {  get; set; }
        public decimal PayOutNequi { get; set; }
        public decimal PorCentajePayOut { get; set; }
        public decimal PorCentajeNequiPayOut { get; set; }
    }
}
