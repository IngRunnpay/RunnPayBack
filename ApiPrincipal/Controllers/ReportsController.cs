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
        
        [HttpPost(RoutesPath.ReportsController_PayInConsiliation)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayInConsiliation([FromBody] RequestPayInConsiliation request)
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
                ValidateAccess(RoutesPath.ReportsController_PayInConsiliation, new { });
                response = await _TransactionServices.PayInConsiliation(request);
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
        [HttpPost(RoutesPath.ReportsController_PayInConsiliationExcel)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayInConsiliationExcel([FromBody] RequestPayInConsiliationExcel request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.IdAplicacion))
                {
                    throw new CustomException("Campo no valido [IdAplicacion]");
                }
                ValidateAccess(RoutesPath.ReportsController_PayInConsiliationExcel, new { });
                response = await _TransactionServices.PayInConsiliationExcel(request);
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
        [HttpPost(RoutesPath.ReportsController_PayOutConsiliation)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayOutConsiliation([FromBody] RequestPayOutConsiliation request)
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
                ValidateAccess(RoutesPath.ReportsController_PayInConsiliation, new { });
                response = await _DispersionServices.PayOutConsiliation(request);
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

        [HttpPost(RoutesPath.ReportsController_PayOutConsiliationExcel)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayOutConsiliationExcel([FromBody] RequestPayOutConsiliationExcel request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.IdAplicacion))
                {
                    throw new CustomException("Campo no valido [IdAplicacion]");
                }
                ValidateAccess(RoutesPath.ReportsController_PayOutConsiliationExcel, new { });
                response = await _DispersionServices.PayOutConsiliationExcel(request);
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
        [HttpPost(RoutesPath.ReportsController_DataComision)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> DataComision([FromBody] RequestDataComision request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(request.IdAplicacion))
                {
                    throw new CustomException("Campo no valido [IdAplicacion]");
                }
                ValidateAccess(RoutesPath.ReportsController_DataComision, new { });
                response = await _TransactionServices.DataComision(request);
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
