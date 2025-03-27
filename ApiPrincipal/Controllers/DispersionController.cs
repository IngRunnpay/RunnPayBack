using ApiPrincipal.Controllers;
using ApiPrincipal.Routes;
using Bussines;
using Entities.General;
using Interfaces.Bussines;
using MethodsParameters.Input.Dispersion;
using MethodsParameters.Input.Transaccion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.DispersionController)]
    [Route(RoutesPath.DispersionController)]
    public class DispersionController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IDispersionServices _DispersionServices;
        public DispersionController(
            ILogService logService,
            IConfiguration config,
            IConfiguration configuration,
            IDispersionServices dispersionServices) : base(logService, config)
        {
            _configuration = configuration;
            _DispersionServices = dispersionServices;
        }


        [HttpPost(RoutesPath.DispersionController_Desicion)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayOut([FromBody] RequestDecisionDispersion request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.DispersionController_Desicion, new { });

                response = await _DispersionServices.DesicionDispersion(request);
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
