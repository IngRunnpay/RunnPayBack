using Entities.General;
using MethodsParameters.Input.Dispersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface IDispersionServices
    {
        Task<BaseResponse> PayOutSaldo(CreateDispersionSaldo request);
        Task<BaseResponse> TransaccionesDispersion(string request);
        Task<BaseResponse> DispersionCuenta(List<DataDispersion> request, string IdAplicacion);
        Task<BaseResponse> DispersionTerceros(RequestDispersion request, string IdAplicacion);
        Task<BaseResponse> DesicionDispersion(RequestDecisionDispersion request);
        Task<BaseResponse> DataDispersion(string IdAplicacion, int idDispersion);


    }
}
