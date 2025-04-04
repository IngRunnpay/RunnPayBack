using Entities.General;
using MethodsParameters.Input.Dispersion;
using MethodsParameters.Input.Reports;
using MethodsParameters.Output.Dispersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface IDispersionRepository
    {
        Task<BaseResponse> PayOutSaldo(CreateDispersionSaldo request);
        Task<BaseResponse> TransaccionesDispersion(string request);
        Task<responseSpDataCuenta> DataCuenta(string request);
        Task<object> ValidarTransaccionesLiquidadas(List<DataDispersion> request);
        Task<int> CreateDispersion(CrearDispersion request);
        Task<int> DesicionDispersion(RequestDecisionDispersion request);
        Task<ResponseGetPyOut> GetDispersion(int IdDispersion);
        Task<WebHookPayOut> GetWebHookDispersion(int IdDispersion);
        Task<ResponseGetPyOut> GetDispersionXReferencia(string referencia, string IdAplicacion);
        Task<ReponseGetClientConfig> GetConfigClient(string IdAplicacion);
        Task<BaseResponse> Dispersion(RequestSpDispersion request);

    }
}
