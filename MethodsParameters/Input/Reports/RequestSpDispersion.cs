using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Input.Reports
{
    public class RequestSpDispersion
    {
        public string IdAplicacion { get; set; }
        public int? Ini { get; set; }
        public int? Fin { get; set; }
        public int? IdDispersion { get; set; }
        public string? Referencia { get; set; }
        public string? Documento { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }

    }
}
