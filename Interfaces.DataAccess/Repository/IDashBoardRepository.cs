using Entities.General;
using Entities.Output.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface IDashBoardRepository
    {
        Task<BaseResponse> DashboarHistorial(int IdUsuario);
        Task<ResponseDashboardMont> PorcentajeMensual(int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<BaseResponse> TransaccionesAño(int IdUsuario);
        Task<BaseResponse> Contador(string IdAplicacion, DateTime fecha);
    }
}
