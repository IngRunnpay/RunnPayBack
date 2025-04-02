using Entities.General;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MethodsParameters.Input.Gateway;
using System.Text.Json;
using static MethodsParameters.Utilities;
using System.Data;
using MethodsParameters.Output.Gateway;
using Entities.Enums;
using System.Net;
using MethodsParameters;
using Newtonsoft.Json.Linq;
using MethodsParameters.Output.Transaccion;
using System.Web;
using MethodsParameters.Client;
using MethodsParameters.Input.Webhook;

namespace Bussines
{
    public class GatewayServices :IGatewayServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly ITransactionRepository _TransactionRepository;

        public GatewayServices(IConfiguration configuration, ILogRepository logRepository, ITransactionRepository transactionRepository)
        {
            _configuration = configuration;
            _logRepository = logRepository;
            _TransactionRepository = transactionRepository;
        }

        public async Task<BaseResponse> GatewayStarter(RequestQuicklyStarter ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            ResponseSpLogPasarelaExterna ResponseSpLog = new ResponseSpLogPasarelaExterna();
            try
            {
                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();

                ObjSpRequest.IdTransaccion = await _TransactionRepository.GetIdTranaccionbyReferenciaExterno(ObjRequest.charge_token);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = null;
                ObjSpRequest.Response = JsonSerializer.Serialize(ObjRequest);
                ObjSpRequest.Enviada = false;

                ResponseSpLog = await _logRepository.LoggExternalPasarela(ObjSpRequest);

                if (ResponseSpLog.IdTransaccion > 0)
                {
                    var resp = TpagaClient.GetInfoTransaccion(ObjRequest.charge_token);
                    if (resp != null)
                    {
                        int EstadoAnterior = ResponseSpLog.IdEstadoTransaccion;
                        if (EstadoAnterior == (int)enumEstadoTransaccion.Pendiente || EstadoAnterior == (int)enumEstadoTransaccion.PendientePSE)
                        {
                            ActualizarEstadoTransaccion ActualizarEstado = new ActualizarEstadoTransaccion();
                            ActualizarEstado.IdTransaccion = ResponseSpLog.IdTransaccion;
                            if (resp.status != "pending")
                            {
                                switch (resp.status)
                                {
                                    case "authorized":
                                    case "settled":
                                        ActualizarEstado.idEstadoTransaccio = (int)enumEstadoTransaccion.Aprobado;
                                        break;
                                    case "rejected":
                                    case "failed":
                                    case "charge-rejected":
                                        ActualizarEstado.idEstadoTransaccio = (int)enumEstadoTransaccion.Rechazado;
                                        break;
                                }

                                await _TransactionRepository.UpdateTransaction(ActualizarEstado);

                                ResponseSpDataWebHook ObjResponseDataWeb = await _TransactionRepository.GetDataWebHook(ObjSpRequest.IdTransaccion);
                                if (!string.IsNullOrEmpty(ObjResponseDataWeb.UrlDelivery))
                                {
                                    RequesWebHook ObjWeb = new RequesWebHook();
                                    ObjWeb.idTransaccion = OperacionEncriptacion.EncryptString(ObjResponseDataWeb.IdTransaccion.ToString(), ObjResponseDataWeb.UserName, ObjResponseDataWeb.Id);
                                    ObjWeb.descripcionEstado = OperacionEncriptacion.EncryptString(ObjResponseDataWeb.Descripcion, ObjResponseDataWeb.UserName, ObjResponseDataWeb.Id);
                                    ObjWeb.idEstado = OperacionEncriptacion.EncryptString(ObjResponseDataWeb.IdEstadoTransaccion.ToString(), ObjResponseDataWeb.UserName, ObjResponseDataWeb.Id);
                                    ObjWeb.mensaje = OperacionEncriptacion.EncryptString(ObjResponseDataWeb.Mensaje, ObjResponseDataWeb.UserName, ObjResponseDataWeb.Id);
                                    WebHookClient.SendClientWebhook(ObjResponseDataWeb.UrlDelivery, ObjWeb);
                                }
                            }
                          
                            response.CreateSuccess("OK", new { });
                        }
                        else
                        {
                            response.CreateError("La transacción ya fue rechazada o aprobada.");
                        }
                    }
                    else
                    {
                        response.CreateError("Transaccion no encontrada.");
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
        public async Task<BaseResponse> GatewayBank(string Endpoint, string token)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                //var resp = TpagaClient.GetBankPSE();
                var resp = await _TransactionRepository.GetBancosPSE();

                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();
                string IdTransaccion = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(token.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);
                ObjSpRequest.IdTransaccion = Convert.ToInt32(IdTransaccion);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = JsonSerializer.Serialize(new
                {
                    IdTransaccion = IdTransaccion,
                    Endpoint = Endpoint,
                });
                ObjSpRequest.Response = JsonSerializer.Serialize(resp);
                ObjSpRequest.Enviada = true;
                await _logRepository.LoggExternalPasarela(ObjSpRequest);

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
        public async Task<BaseResponse> GatewayCreated(RequestCreatedIdTransaccion ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string IdTransaccion = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(ObjRequest.IdTransaccion.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);

                RequestCreatePseTpago ObjRquestGateway = new RequestCreatePseTpago();
                ResponseSp_GetDataTransaccion ResponseTransaction= await _TransactionRepository.DataTransaction(Convert.ToInt32(IdTransaccion));
                string BaseRuta = _configuration.GetSection("RuteResume").Value;

                ObjRquestGateway.bank_code = ObjRequest.Banco;
                ObjRquestGateway.order_id = ResponseTransaction.IdTransaccion.ToString();
                ObjRquestGateway.amount = ResponseTransaction.MontoFinal.ToString("0");
                ObjRquestGateway.vat_amount = "0";
                ObjRquestGateway.description = $"RunnPay|{IdTransaccion}|{ResponseTransaction.Referencia}|{ResponseTransaction.DescripcionCompra}";
                ObjRquestGateway.user_type = ObjRequest.Persona;
                ObjRquestGateway.buyer_email = ResponseTransaction.UsuCorreo;
                ObjRquestGateway.buyer_full_name = ResponseTransaction.UsuNombre;
                ObjRquestGateway.document_type = ResponseTransaction.Documento;
                ObjRquestGateway.document_number = ResponseTransaction.UsuDocumento;
                ObjRquestGateway.redirect_url = (string.IsNullOrEmpty(ResponseTransaction.UrlClient))? $"{BaseRuta}{ObjRequest.IdTransaccion}": ResponseTransaction.UrlClient;
                ObjRquestGateway.buyer_phone_number = ResponseTransaction.UsuTelefono;

                var resp = TpagaClient.CreatePSETpago(ObjRquestGateway);

                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();
                ObjSpRequest.IdTransaccion = Convert.ToInt32(IdTransaccion);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = JsonSerializer.Serialize(ObjRquestGateway);
                ObjSpRequest.Response = JsonSerializer.Serialize(resp);
                ObjSpRequest.Enviada = true;
                await _logRepository.LoggExternalPasarela(ObjSpRequest);

                if (!string.IsNullOrEmpty(resp.token))
                {
                    if (resp.return_code == "SUCCESS")
                    {
                        response.CreateSuccess("OK", resp.bank_url);
                        _logRepository.LogLinKPSEExternalPasarela(new SpLogLinkPSEPasarelas
                        {
                            IdTransaccion = ObjSpRequest.IdTransaccion,
                            Url = resp.bank_url,
                            ReferenciaExterno = resp.token
                        });
                    }
                    else
                    {
                        response.CreateError("No logramos realizar tu gestión, intenta nuevamente.");
                    }

                }
                else
                {
                    response.CreateError((resp.error_message != null) ? resp.error_message : null);
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

        public async Task<BaseResponse> GatewayGetDataTransaction(string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string Token = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(IdTransaccion.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);

                ResponseSp_GetDataTransaccion Response = await _TransactionRepository.DataTransaction(Convert.ToInt32(Token));
                response.CreateSuccess("OK", Response);
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

        public async Task<BaseResponse> GatewayController_ResumePay(string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string Token = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(IdTransaccion.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);

                ResponseSpResumePay ResponseQuickly = await _TransactionRepository.QuicklypayController_ResumePay(Convert.ToInt32(Token));
                response.CreateSuccess("OK", ResponseQuickly);
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
        public async Task<BaseResponse> GetMetodPagoXUsuario(string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string Token = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(IdTransaccion.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);

                response = await _TransactionRepository.GetMetodPagoXUsuario(Convert.ToInt32(Token));
                response.CreateSuccess("OK", response.Data);
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
