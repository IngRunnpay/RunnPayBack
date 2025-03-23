using Entities.General;
using MethodsParameters.Input.Gateway;
using MethodsParameters.Input.Transaccion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface IGatewayServices
    {
        Task<BaseResponse> GatewayStarter(RequestQuicklyStarter ObjRequest, string Endpoint);
        Task<BaseResponse> GatewayBank(string Endpoint, string token);
        Task<BaseResponse> GatewayCreated(RequestCreatedIdTransaccion ObjRequest, string Endpoint);
        Task<BaseResponse> GatewayGetDataTransaction(string IdTransaccion);
        Task<BaseResponse> GatewayController_ResumePay(string IdTransaccion); 
    }
}
