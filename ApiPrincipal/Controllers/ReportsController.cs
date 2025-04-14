using Entities.General;
using Interfaces.Bussines;
using Entities.Input.PortalUser;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiPrincipal.Routes;
using Entities.Input.Reports;
using Microsoft.IdentityModel.Tokens;
using Bussines;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.ReportsController)]
    [Route(RoutesPath.ReportsController)]
    public class ReportsController : BaseController
    {
        private readonly ITransactionServices _TransactionServices;
        private readonly IDispersionServices _DispersionServices;
        public ReportsController(
             ILogService logService,
             IConfiguration config,
             IUserPortalServices userPortalServices,
             ITransactionServices transactionServices,
             IDispersionServices dispersionServices) : base(logService, config)
        {
            _TransactionServices = transactionServices;
            _DispersionServices = dispersionServices;
        }

        [HttpPost(RoutesPath.ReportsController_Transactions)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Transactions([FromBody] RequestSpTransactions request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.IdAplicacion))
                {
                    throw new CustomException("Campo no valido [IdAplicacion]");
                }
                if (request.IdUsuario <= 0)
                {
                    throw new CustomException("Campo no valido [IdUsuario]");

                }
                if (request.Ini <= 0)
                {
                    throw new CustomException("Campo no valido [Ini]");

                }
                if (request.Fin <= 0)
                {
                    throw new CustomException("Campo no valido [Fin]");

                }
                ValidateAccess(RoutesPath.ReportsController_Transactions, new { });
                response = await _TransactionServices.Resporttransaction(request);
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

        [HttpPost(RoutesPath.ReportsController_Dispersion)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Dispersion([FromBody] RequestSpDispersion request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.IdAplicacion))
                {
                    throw new CustomException("Campo no valido [IdAplicacion]");
                }
                if (request.Ini <= 0)
                {
                    throw new CustomException("Campo no valido [Ini]");

                }
                if (request.Fin <= 0)
                {
                    throw new CustomException("Campo no valido [Fin]");

                }
                ValidateAccess(RoutesPath.ReportsController_Dispersion, new { });
                response = await _DispersionServices.Dispersion(request);
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
