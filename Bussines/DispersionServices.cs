using Entities.Enums;
using Entities.General;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters.Input;
using MethodsParameters.Input.Dispersion;
using MethodsParameters.Output.Dispersion;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bussines
{
    public class DispersionServices : IDispersionServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly IDispersionRepository _DispersionRepository;
        private readonly ITransactionRepository _TransactionRepository;

        public DispersionServices(IConfiguration configuration, ILogRepository logRepository, IDispersionRepository dispersionRepository, ITransactionRepository transactionRepository)
        {
            _configuration = configuration;
            _logRepository = logRepository;
            _DispersionRepository = dispersionRepository;
            _TransactionRepository = transactionRepository;
        }
        public async Task<BaseResponse> PayOutSaldo(CreateDispersionSaldo request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ReponseBalance balance = await _TransactionRepository.Balance(request.IdAplicacion);
                if (balance.IdBalance > 0)
                {
                    if (balance.montoDisponible > request.Monto)
                    {
                        response = await _DispersionRepository.PayOutSaldo(request);
                        if (response.Success)
                        {
                            response.CreateSuccess("Ok", response.Data);
                        }
                        else
                        {
                            response.CreateError("No logramos realizar tu solicitud, intenta nuevamente.");
                        }
                    }
                    else
                    {
                        response.CreateError("El monto solicitado es mayor al disponible.");

                    }
                }
                else
                {
                    response.CreateError("No logramos realizar tu solicitud, intenta nuevamente.");
                }
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
        public async Task<BaseResponse> TransaccionesDispersion(string request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _DispersionRepository.TransaccionesDispersion(request);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
        public async Task<BaseResponse> DispersionCuenta(List<DataDispersion> request, string IdAplicacion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                responseSpDataCuenta resp = await _DispersionRepository.DataCuenta(IdAplicacion);
                if (string.IsNullOrEmpty(resp.NumeroCuenta))
                {
                    response.CreateError("No tiene una cuenta para dispersion configurada.");
                }
                else
                {
                    object RespValid = await _DispersionRepository.ValidarTransaccionesLiquidadas(request);
                    if(RespValid == null)
                    {
                        object RequestSave = new
                        {
                            TipoDocuemnto = resp.TipoDocumento,
                            NumeroDocumento = resp.NumDocuemnto,
                            Banco = resp.Banco,
                            TipoCuenta = resp.TipoCuenta,
                            NumeroCuenta = resp.NumeroCuenta,
                            Data = request
                        };
                        CrearDispersion ObjCrearDispersion = new CrearDispersion();
                        ObjCrearDispersion.Data = request;
                        ObjCrearDispersion.IdAplicacion = IdAplicacion;
                        ObjCrearDispersion.TipoDispersion = (int)enumTipoDispersion.Personal;
                        ObjCrearDispersion.Request = System.Text.Json.JsonSerializer.Serialize(RequestSave); ;
                        int IdDispersion = await _DispersionRepository.CreateDispersion(ObjCrearDispersion);
                        if (IdDispersion > 0)
                        {
                            response.CreateSuccess("Solicitud de dispersión generada correctamente.", IdDispersion);
                        }
                        else
                        {
                            response.CreateError("Solicitud de dispersión rechazada.");
                        }
                    }
                    else
                    {
                        response.Code = 400;
                        response.Message = $"No es posible realizar la solicitud ya que hay transacciones en estado de dispersión o dispersadas.";
                        response.Success = false;
                        response.Data = RespValid;

                    }
                }
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
        public async Task<BaseResponse> DispersionTerceros(RequestDispersion request, string IdAplicacion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                object RespValid = await _DispersionRepository.ValidarTransaccionesLiquidadas(request.Data);
                if (RespValid == null)
                {
                    CrearDispersion ObjCrearDispersion = new CrearDispersion();
                    ObjCrearDispersion.Data = request.Data;
                    ObjCrearDispersion.IdAplicacion = IdAplicacion;
                    ObjCrearDispersion.TipoDispersion = (int)enumTipoDispersion.Terceros;
                    ObjCrearDispersion.Request = System.Text.Json.JsonSerializer.Serialize(request); ;
                    int IdDispersion = await _DispersionRepository.CreateDispersion(ObjCrearDispersion);
                    if (IdDispersion > 0)
                    {
                        response.CreateSuccess("Solicitud de dispersión generada correctamente.", IdDispersion);
                    }
                    else
                    {
                        response.CreateError("Solicitud de dispersión rechazada.");
                    }
                }
                else
                {
                    response.Code = 400;
                    response.Message = $"No es posible realizar la solicitud ya que hay transacciones en estado de dispersión o dispersadas.";
                    response.Success = false;
                    response.Data = RespValid;

                }
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
    }
}
