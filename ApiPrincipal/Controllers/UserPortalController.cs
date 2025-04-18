using ApiPrincipal.Routes;
using Bussines;
using Entities.General;
using Interfaces.Bussines;
using Entities.Input.PortalUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.UserPortalController)]
    [Route(RoutesPath.UserPortalController)]
    public class UserPortalController : BaseController
    {
        private readonly IUserPortalServices _UserPortalServices;
        private readonly IDispersionServices _DispersionServices;
        public UserPortalController(
             ILogService logService,
             IConfiguration config,
             IUserPortalServices userPortalServices,
             IDispersionServices dispersionServices) : base(logService, config)
        {
            _UserPortalServices = userPortalServices;
            _DispersionServices = dispersionServices;
        }

        [HttpPost(RoutesPath.UserPortalController_LoginUser)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> LoginUser([FromBody] Login request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.Correo))
                {
                    throw new CustomException("Campo no valido [Correo]");
                }
                ValidateAccess(RoutesPath.GatewayController_GatewayBank, new { });
                response = await _UserPortalServices.LoginPortal(request);
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
        [HttpPost(RoutesPath.UserPortalController_ValidOtp)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> ValidOtp([FromBody] RequestValidOtp request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.Correo))
                {
                    throw new CustomException("Campo no valido [Correo]");
                }
                if (string.IsNullOrEmpty(request.Otp))
                {
                    throw new CustomException("Campo no valido [Otp]");
                }
                ValidateAccess(RoutesPath.UserPortalController_ValidOtp, new { });
                response = await _UserPortalServices.ValidOtp(request);
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

        [HttpGet(RoutesPath.UserPortalController_ConfigPayOt)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> ConfigPayOut([FromQuery] string IdAplicacion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.UserPortalController_ConfigPayOt, new { });

                response = await _DispersionServices.ConfigPayOut(IdAplicacion);
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

        [HttpGet(RoutesPath.UserPortalController_ConfigPayIN)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> ConfigPayIN([FromQuery] string IdAplicacion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.UserPortalController_ConfigPayIN, new { });

                response = await _UserPortalServices.ConfigPayIN(IdAplicacion);
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
