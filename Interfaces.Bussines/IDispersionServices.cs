using Entities.General;
using Entities.Input.Dispersion;
using Entities.Input.Reports;
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
        Task<BaseResponse> BilleteraCliente(string IdAplicacion);
        Task<BaseResponse> Dispersion(RequestSpDispersion request);
    }
}
