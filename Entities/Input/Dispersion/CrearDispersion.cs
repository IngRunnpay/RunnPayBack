using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Dispersion
{
    public class CrearDispersion
    {
        public List<DataDispersion> Data { get; set; }
        public string IdAplicacion {  get; set; }
        public int TipoDispersion { get; set; }
        public string Request {  get; set; }
    }
}
