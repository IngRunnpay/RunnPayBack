using ApiPublica.Routes;
using Entities.General;
using Interfaces.Bussines;
using MethodsParameters.Input.Transaccion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using ApiPublica.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using Bussines;

namespace ApiPublica.Controllers
{
    [ApiController]
    [Tags(RoutesPath.TransactionController)]
    [Route(RoutesPath.TransactionController)]
    public class TransactionController : BaseController
    {
        private readonly ITransactionServices _TransactionServices;
        private readonly IConfiguration _configuration;
        public TransactionController(
            ILogService logService,
            IConfiguration config,
            ITransactionServices TransactionServices,
            IConfiguration configuration) : base(logService, config)
        {
            _TransactionServices = TransactionServices;
            _configuration = configuration;
        }

        [HttpPost(RoutesPath.TransactionController_Create)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Create([FromBody] RequestTransactionCreate Request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var montoMinimo = _configuration.GetSection("MontoMinimo").Value;
                var montoMaximo = _configuration.GetSection("MontoMaximo").Value;

                if (Request.Monto < Convert.ToDecimal(montoMinimo))
                {
                    throw new CustomException($"El monto minimo es de {montoMinimo}.");
                }
                if (Request.Monto > Convert.ToDecimal(montoMaximo))
                {
                    throw new CustomException($"El monto maximo es de {montoMaximo}.");
                }
                if (Request.Monto <= 0)
                {
                    throw new CustomException("Campo no valido [Monto]");
                }
                if (Request.IdTax <= 0)
                {
                    throw new CustomException("Campo no valido [IdTax]");
                }
                if (string.IsNullOrEmpty(Request.Referencia) || Request.Referencia.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Referencia]");
                }
                if (string.IsNullOrEmpty(Request.Descripcion) || Request.Descripcion.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Descripcion]");
                }
                if (Request.IdMoneda <= 0)
                {
                    throw new CustomException("Campo no valido [IdMoneda]");
                }
                if (Request.TipoDocumento <= 0)
                {
                    throw new CustomException("Campo no valido [TipoDocumento]");
                }
                if (string.IsNullOrEmpty(Request.Documento) || Request.Documento.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Documento]");
                }
                if (string.IsNullOrEmpty(Request.Telefono) || Request.Telefono.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Telefono]");
                }
                if (string.IsNullOrEmpty(Request.Correo) || Request.Correo.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [Correo]");
                }
                if (string.IsNullOrEmpty(Request.UsuNombre) || Request.UsuNombre.ToUpper() == "STRING")
                {
                    throw new CustomException("Campo no valido [UsuNombre]");
                }
                if (!string.IsNullOrEmpty(Request.banco))
                {
                    if (string.IsNullOrEmpty(Request.TipoPersona))
                    {
                        throw new CustomException("Campo no valido [TipoPersona]");
                    }
                    else
                    {
                        if(Request.TipoPersona != "NATURAL" && Request.TipoPersona != "JURÍDICA")
                        {
                            throw new CustomException("Campo no valido [TipoPersona]");
                        }
                    }
                }
                
                ValidateAccess(RoutesPath.TransactionController_Create, new { });

                Request.IdUsuario = Convert.ToInt32(IdUsuarioPersona);

                response = await _TransactionServices.Create(Request);
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

        [HttpGet(RoutesPath.TransactionController_ListTransaction)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> ListTransaction()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.TransactionController_ListTransaction, new { });

                SpGetListTransactionByUser ObjGet = new SpGetListTransactionByUser();
                ObjGet.IdUsuario = Convert.ToInt32(IdUsuarioPersona);

                response = await _TransactionServices.ListTransaction(ObjGet);
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

        [HttpGet(RoutesPath.TransactionController_HistoriTransaction)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> HistoriTransaction([FromQuery] int IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (IdTransaccion == 0)
                {
                    throw new CustomException("Campo no valido [IdTransaccion]");
                }

                ValidateAccess(RoutesPath.TransactionController_ListTransaction, new { });

                SpGetHistoriTransaction ObjGet = new SpGetHistoriTransaction();
                ObjGet.IdTransaccion = IdTransaccion;
                ObjGet.IdUsuario = Convert.ToInt32(IdUsuarioPersona);

                response = await _TransactionServices.HistoriTransaction(ObjGet);
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

        [HttpGet(RoutesPath.TransactionController_Estadotransaccion)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Estadotransaccion([FromQuery] int IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (IdTransaccion == 0)
                {
                    throw new CustomException("Campo no valido [IdTransaccion]");
                }

                ValidateAccess(RoutesPath.TransactionController_Estadotransaccion, new { });

                SpGetHistoriTransaction ObjGet = new SpGetHistoriTransaction();
                ObjGet.IdTransaccion = IdTransaccion;
                ObjGet.IdUsuario = Convert.ToInt32(IdUsuarioPersona);

                response = await _TransactionServices.Estadotransaccion(ObjGet);
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

        [HttpGet(RoutesPath.TransactionController_Balance)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> Balance()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.TransactionController_ListTransaction, new { });

                SpGetListTransactionByUser ObjGet = new SpGetListTransactionByUser();
                var IdAplicacion = IdUsuario;

                response = await _TransactionServices.Balance(IdAplicacion);
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

        [HttpGet(RoutesPath.TransactionController_Bancos)]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult> GatewayBank()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ValidateAccess(RoutesPath.TransactionController_Bancos, new { });
                response = await _TransactionServices.GatewayBank();
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
