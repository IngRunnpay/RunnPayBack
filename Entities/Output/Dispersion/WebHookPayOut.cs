using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Output.Dispersion
{
    public class WebHookPayOut
    {
        public int IdDispersion { get; set; }
        public string DescripcionEstado { get; set; }
        public string Mensaje { get; set; }
        public int idEstado { get; set; }
        public string Aplicacion { get; set; }
        public string UserName { get; set; }

    }
}
