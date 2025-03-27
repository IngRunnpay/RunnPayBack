using ApiPublica.Routes;
using Bussines;
using Entities.General;
using Interfaces.Bussines;
using MethodsParameters.Input.Dispersion;
using MethodsParameters.Input.Transaccion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ApiPublica.Controllers
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


        [HttpPost(RoutesPath.DispersionController_PayOut)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> PayOut([FromBody] CreateDispersionSaldo request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (request.TipoDocumento <= 0)
                {
                    throw new CustomException("Campo no valido [TipoDocumento]");
                }
                if (string.IsNullOrEmpty(request.NumeroDocumento) || request.NumeroDocumento.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [NumeroDocumento]");
                }
                if (string.IsNullOrEmpty(request.Banco) || request.Banco.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Banco]");
                }
                if (request.TipoCuenta <= 0)
                {
                    throw new CustomException("Campo no valido [TipoCuenta]");
                }
                if (string.IsNullOrEmpty(request.NumeroCuenta) || request.NumeroCuenta.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [NumeroCuenta]");
                }
                if (request.Monto <= 0)
                {
                    throw new CustomException("Campo no valido [Monto]");
                }
                if (string.IsNullOrEmpty(request.Referencia) || request.Referencia.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Referencia]");
                }
                if (string.IsNullOrEmpty(request.Descripcion) || request.Descripcion.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Descripcion]");
                }
                if (string.IsNullOrEmpty(request.Telefono) || request.Telefono.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Telefono]");
                }
                if (string.IsNullOrEmpty(request.Correo) || request.Correo.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Correo]");
                }
                if (string.IsNullOrEmpty(request.UsuNombre) || request.UsuNombre.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [UsuNombre]");
                }
                if (string.IsNullOrEmpty(request.NotifyUrl) || request.NotifyUrl.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [NotifyUrl]");
                }

                ValidateAccess(RoutesPath.DispersionController_PayOut, new { });

                request.IdAplicacion = IdUsuario;

                response = await _DispersionServices.PayOutSaldo(request);
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

        [HttpGet(RoutesPath.DispersionController_DataPayOut)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> ListTransaction([FromQuery] int IdDispersion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.DispersionController_DataPayOut, new { });

                var IdAplicacion = IdUsuario;

                response = await _DispersionServices.DataDispersion(IdAplicacion, IdDispersion);
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
