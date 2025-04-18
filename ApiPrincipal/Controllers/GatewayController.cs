using ApiPrincipal.Routes;
using Bussines;
using Entities.General;
using Interfaces.Bussines;
using Entities.Input.Gateway;
using Entities.Input.Transaccion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.GatewayController)]
    [Route(RoutesPath.GatewayController)]
    public class GatewayController : BaseController
    {
        private readonly IGatewayServices _GatewayServices;
        public GatewayController(
             ILogService logService,
             IConfiguration config,
             IGatewayServices gatewayServices) : base(logService, config)
        {
            _GatewayServices = gatewayServices;
        }
        
        [HttpPost(RoutesPath.GatewayController_GatewayStarter)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> GatewayStarter([FromBody] object ObjRequest)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _GatewayServices.GatewayStarter(ObjRequest, RoutesPath.GatewayController_GatewayStarter);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }

        [HttpGet(RoutesPath.GatewayController_GatewayBank)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> GatewayBank([FromQuery] string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(IdTransaccion))
                {
                    throw new CustomException("Campo no valido [IdTransaccion]");
                }
                ValidateAccess(RoutesPath.GatewayController_GatewayBank, new { });
                response = await _GatewayServices.GatewayBank(RoutesPath.GatewayController_GatewayBank, IdTransaccion);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }       

        [HttpGet(RoutesPath.GatewayController_GatewayGetDataTransaction)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> GatewayGetDataTransaction([FromQuery] string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.GatewayController_GatewayGetDataTransaction, new { });
                response = await _GatewayServices.GatewayGetDataTransaction(IdTransaccion);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }

        [HttpGet(RoutesPath.GatewayController_ResumePay)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> GatewayController_ResumePay([FromQuery] string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.GatewayController_ResumePay, new { });
                response = await _GatewayServices.GatewayController_ResumePay(IdTransaccion);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }
        [HttpGet(RoutesPath.GatewayController_GetMetodPagoXUsuario)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> GetMetodPagoXUsuario([FromQuery] string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.GatewayController_ResumePay, new { });
                response = await _GatewayServices.GetMetodPagoXUsuario(IdTransaccion);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }
        [HttpPost(RoutesPath.GatewayController_GatewayPayment)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Payment([FromBody] RequestPaymentContinue ObjRequest)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.GatewayController_GatewayPayment, new { });
                response = await _GatewayServices.Payment(ObjRequest, RoutesPath.GatewayController_GatewayPayment);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }
        [HttpPost(RoutesPath.GatewayController_GatewayStarteBP)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayStarteBPment([FromBody] RequestStarterBePay ObjRequest)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _GatewayServices.WebHook(ObjRequest, RoutesPath.GatewayController_GatewayStarteBP);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await LogError(ex);
                response.CreateError(ex);
            }
            return Ok(response);
        }
    }
}
