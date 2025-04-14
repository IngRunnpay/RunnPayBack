using Entities.General;
using Entities.Input.Gateway;
using Entities.Input.Transaccion;
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
        Task<BaseResponse> GatewayGetDataTransaction(string IdTransaccion); 
        Task<BaseResponse> GatewayController_ResumePay(string IdTransaccion);
        Task<BaseResponse> GetMetodPagoXUsuario(string IdTransaccion);
        Task<BaseResponse> Payment(RequestPaymentContinue ObjRequest, string Endpoint);
        Task<BaseResponse> WebHook(RequestStarterBePay ObjRequest, string Endpoint);

    }
}
