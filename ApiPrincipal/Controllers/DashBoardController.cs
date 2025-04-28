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
    [Tags(RoutesPath.DashboardController)]
    [Route(RoutesPath.DashboardController)]
    public class DashBoardController : BaseController
    {
        private readonly IDashBoardServices _DashBoardServices;
        public DashBoardController(
             ILogService logService,
             IConfiguration config,
             IDashBoardServices dashBoardServices) : base(logService, config)
        {
            _DashBoardServices = dashBoardServices;
        }
        [HttpGet(RoutesPath.DashboardController_HistorialTransacciones)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> DashboarHistorial([FromQuery] int IdUsuario)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.DashboardController_HistorialTransacciones, new { });
                response = await _DashBoardServices.DashboarHistorial(IdUsuario);
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
        [HttpGet(RoutesPath.DashboardController_PorcentajeMensual)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PorcentajeMensual([FromQuery] int IdUsuario)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.DashboardController_HistorialTransacciones, new { });
                response = await _DashBoardServices.PorcentajeMensual(IdUsuario);
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
        [HttpGet(RoutesPath.DashboardController_TransaccionesAño)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> TransaccionesAño([FromQuery] int IdUsuario)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.DashboardController_HistorialTransacciones, new { });
                response = await _DashBoardServices.TransaccionesAño(IdUsuario);
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
        [HttpGet(RoutesPath.DashboardController_Contador)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Contador([FromQuery] string IdAplicacion, DateTime fecha)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.DashboardController_Contador, new { });
                response = await _DashBoardServices.Contador(IdAplicacion, fecha);
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
