using Entities.Enums;
using Entities.General;
using Entities.Identity;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters;
using MethodsParameters.Input;
using MethodsParameters.Input.Gateway;
using MethodsParameters.Input.Reports;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Output.Transaccion;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static MethodsParameters.Utilities;

namespace Bussines
{
    public class TransactionServices: ITransactionServices
    {
        private readonly ITransactionRepository _iTransactionRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly IGatewayServices _gatewayServices;

        public TransactionServices(IConfiguration configuration, ITransactionRepository transactionRepository,  ILogRepository logRepository, IGatewayServices gatewayServices)
        {
            _iTransactionRepository = transactionRepository;
            _configuration = configuration;
            _logRepository = logRepository;
            _gatewayServices = gatewayServices;
        }
        public async Task<BaseResponse> Create(RequestTransactionCreate Request)
        {
            BaseResponse response = new BaseResponse();
            ResponseCreate ObjResponseSp = new ResponseCreate();
            try
            {
                if (Request.IdTax == (int)enumTax.Sin_Interes)
                {
                    Request.MontoFinal = Request.Monto;
                }
                else
                {
                    Request.MontoFinal = Request.Monto * (decimal)0.19;
                }
                ObjResponseSp = await _iTransactionRepository.Create(Request);
                var BaseUrl = "";
                var IdToken = HttpUtility.UrlEncode(Utilities.OperacionEncriptacion.EncryptString(ObjResponseSp.IdTransaccion.ToString(), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV)).UrlErrorCharacteresEncode();

                bool FlagUpdate = true;
                if (!string.IsNullOrEmpty(Request.banco))
                {
                    BaseResponse ResponseCreatePSELink = await _gatewayServices.GatewayCreated(new RequestCreatedIdTransaccion
                    {
                        Banco = Request.banco,
                        Persona =  Request.TipoPersona,
                        IdTransaccion = IdToken,
                    }, "Crear link directo PSE api publica.");
                    if (ResponseCreatePSELink.Success)
                    {
                        ObjResponseSp.Url = ResponseCreatePSELink.Data.ToString();
                    }
                    else
                    {
                        FlagUpdate = false;
                    }
                }
                else
                {
                    BaseUrl = _configuration.GetSection("RuteCheckOut").Value;
                    ObjResponseSp.Url = $"{BaseUrl}{IdToken}";
                }
                if (FlagUpdate)
                {
                    var result = await _iTransactionRepository.UpdateCreate(new RequestUpdateCreate
                    {
                        IdTransaccion = ObjResponseSp.IdTransaccion,
                        MontoFinal = (decimal)Request.MontoFinal,
                        Url = ObjResponseSp.Url,
                        IdUsuario = (int)Request.IdUsuario
                    });

                    response.CreateSuccess("OK", ObjResponseSp);
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
        
        public async Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _iTransactionRepository.ListTransaction(Request);               
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
        public async Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _iTransactionRepository.HistoriTransaction(Request);
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
        public async Task<BaseResponse> Estadotransaccion(SpGetHistoriTransaction Request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _iTransactionRepository.Estadotransaccion(Request);
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
        public async Task<BaseResponse> Resporttransaction(RequestSpTransactions request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _iTransactionRepository.Resporttransaction(request);
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
        public async Task<BaseResponse> Balance(string request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response.Data = await _iTransactionRepository.Balance(request);
                response.CreateSuccess("Ok", response.Data);
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
        public async Task<BaseResponse> GatewayBank()
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _iTransactionRepository.GetBancosPSE();

                response.CreateSuccess("OK", resp.Data);
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
