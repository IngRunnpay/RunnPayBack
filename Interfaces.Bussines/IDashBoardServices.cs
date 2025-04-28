using Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface IDashBoardServices
    {        
        Task<BaseResponse> DashboarHistorial(int IdUsuario); 
        Task<BaseResponse> PorcentajeMensual(int IdUsuario);
        Task<BaseResponse> TransaccionesAño(int IdUsuario);
        Task<BaseResponse> Contador(string IdAplicacion, DateTime fecha);
    }
}
