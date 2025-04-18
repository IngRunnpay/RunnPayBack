using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Input.Reports
{
    public class RequestPayOutConsiliation
    {
        public string IdAplicacion { get; set; }
        public int? Ini { get; set; }
        public int? Fin { get; set; }
        public string? Fecha { get; set; }
    }
    public class RequestPayOutConsiliationExcel
    {
        public string IdAplicacion { get; set; }
        public string? FechaIni { get; set; }
        public string? FechaFin { get; set; }

    }
}
