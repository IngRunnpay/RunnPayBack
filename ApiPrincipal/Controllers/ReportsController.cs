using Entities.General;
using Interfaces.Bussines;
using MethodsParameters.Input.PortalUser;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiPrincipal.Routes;
using MethodsParameters.Input.Reports;
using Microsoft.IdentityModel.Tokens;

namespace ApiPrincipal.Controllers
{
    [ApiController]
    [Tags(RoutesPath.ReportsController)]
    [Route(RoutesPath.ReportsController)]
    public class ReportsController : BaseController
    {
        private readonly ITransactionServices _TransactionServices;
        public ReportsController(
             ILogService logService,
             IConfiguration config,
             IUserPortalServices userPortalServices,
             ITransactionServices transactionServices) : base(logService, config)
        {
            _TransactionServices = transactionServices;
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
    }
}
