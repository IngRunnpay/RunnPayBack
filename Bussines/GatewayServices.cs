using Entities.General;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using Entities.Input.Transaccion;
using Entities.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Input.Gateway;
using System.Text.Json;
using static MethodsParameters.Utilities;
using System.Data;
using Entities.Output.Gateway;
using Entities.Enums;
using System.Net;
using MethodsParameters;
using Newtonsoft.Json.Linq;
using Entities.Output.Transaccion;
using System.Web;
using MethodsParameters.Client;
using Entities.Input.Webhook;
using Entities.Input.Client.BPay;
using Entities.Output.Client.Bpay;

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
        #region Public
        public async Task<BaseResponse> GatewayStarter(object ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            ResponseSpLogPasarelaExterna ResponseSpLog = new ResponseSpLogPasarelaExterna();
            try
            {
                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();

                ObjSpRequest.IdTransaccion = 1;
                ObjSpRequest.Endpoint = "WEBHOOK";
                ObjSpRequest.Request = "Pruebas de funcionamiento webhook";
                ObjSpRequest.Response = JsonSerializer.Serialize(ObjRequest);
                ObjSpRequest.Enviada = false;

                ResponseSpLog = await _logRepository.LoggExternalPasarela(ObjSpRequest);
                response.CreateSuccess("OK", true);

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
        public async Task<BaseResponse> GatewayGetDataTransaction(string IdTransaccion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string Token = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(IdTransaccion.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);
                ResponseSp_GetDataTransaccion Response = new ResponseSp_GetDataTransaccion();
                Response = await _TransactionRepository.DataTransaction(Convert.ToInt32(Token));
                if (response != null)
                {
                    if (Response.IdEstadoTransaccion == (int)enumEstadoTransaccion.PendienteConfirmacionPago)
                    {
                        ReponseFullDataTransaction DataFull = await _TransactionRepository.FullDataTransaction(Convert.ToInt32(Token));
                        RequestStarterBePay ObjRequest = new RequestStarterBePay();
                        ObjRequest.transaction_ide = DataFull.ReferenciaExterno;
                        ObjRequest.transacton_ide = DataFull.ReferenciaExterno;
                        await WebHook(ObjRequest, $"Ejecucion manual webhook con metodo GatewayGetDataTransaction");
                        Response = await _TransactionRepository.DataTransaction(Convert.ToInt32(Token));

                    }
                }
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
                ResponseSpResumePay Response = new ResponseSpResumePay();
                Response = await _TransactionRepository.QuicklypayController_ResumePay(Convert.ToInt32(Token));
                if (response != null)
                {
                    if (Response.IdEstadoTransaccion == (int)enumEstadoTransaccion.PendienteConfirmacionPago)
                    {
                        ReponseFullDataTransaction DataFull = await _TransactionRepository.FullDataTransaction(Convert.ToInt32(Token));
                        RequestStarterBePay ObjRequest = new RequestStarterBePay();
                        ObjRequest.transaction_ide = DataFull.ReferenciaExterno;
                        ObjRequest.transacton_ide = DataFull.ReferenciaExterno;
                        await WebHook(ObjRequest, $"Ejecucion manual webhook con metodo GatewayGetDataTransaction");
                        Response = await _TransactionRepository.QuicklypayController_ResumePay(Convert.ToInt32(Token)); ;

                    }
                }
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
        public async Task<BaseResponse> Payment(RequestPaymentContinue ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string IdTransaccion = OperacionEncriptacion.DecryptAES(HttpUtility.UrlDecode(ObjRequest.IdTransaccion.UrlErrorCharacteresEncode(true)), Utilities.OperacionEncriptacion.Keys.UserAgreementKEY, Utilities.OperacionEncriptacion.Keys.UserAgreementIV);
                ResponseSp_GetDataTransaccion ResponseTransaction = await _TransactionRepository.DataTransaction(Convert.ToInt32(IdTransaccion));
                string BaseRuta = _configuration.GetSection("RuteResume").Value;
                
                if (ResponseTransaction != null)
                {
                    RequestCreateTransaction Transaction = new RequestCreateTransaction();

                    Transaction.reference = $"RunnPay|{IdTransaccion}|{ResponseTransaction.Referencia}";
                    Transaction.total = ResponseTransaction.MontoFinal.ToString().Replace(",", ".");
                    Transaction.description = ResponseTransaction.DescripcionCompra;
                    Transaction.redirect_url =  (string.IsNullOrEmpty(ResponseTransaction.UrlClient)) ? $"{BaseRuta}{ObjRequest.IdTransaccion}" : ResponseTransaction.UrlClient;

                    RequestPaymentContinueBePay RequestBePay = await GenerateRequestBePay(ObjRequest, Transaction, ResponseTransaction);
                    BaseResponseBpay resp = new BaseResponseBpay();
                    switch (ObjRequest.IdmedioPago)
                    {
                        case (int)enumTypePayment.PSE:
                            resp = BePayClient.PSE(RequestBePay, Transaction);
                            break;
                        case (int)enumTypePayment.NEQUIPUSH:
                             resp = BePayClient.NequiPush(RequestBePay, Transaction);
                            break;
                    }

                    SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();
                    ObjSpRequest.IdTransaccion = Convert.ToInt32(IdTransaccion);
                    ObjSpRequest.Endpoint = Endpoint;
                    ObjSpRequest.Request = JsonSerializer.Serialize(RequestBePay);
                    ObjSpRequest.Response = JsonSerializer.Serialize(resp);
                    ObjSpRequest.Enviada = true;
                    await _logRepository.LoggExternalPasarela(ObjSpRequest);

                    if (resp.success)
                    {
                        var update = await _TransactionRepository.UpdateBePay(Convert.ToInt32(IdTransaccion), ObjRequest.IdmedioPago);
                        
                        response.CreateSuccess("Ok", (ObjRequest.IdmedioPago == (int)enumTypePayment.PSE)? resp.data.link : Transaction.redirect_url);
                        _logRepository.LogLinKPSEExternalPasarela(new SpLogLinkPSEPasarelas
                        {
                            IdTransaccion = ObjSpRequest.IdTransaccion,
                            Url = resp.data.link,
                            ReferenciaExterno = resp.data.ide
                        });
                    }
                    else
                    {
                        response.CreateError((resp.message != null) ? resp.message : "Error al continuar con la transacción.");
                    }
                }
                else
                {
                    response.CreateError("No se encuentra informacion de la transaccion indicada.");
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
        public async Task<BaseResponse> WebHook(RequestStarterBePay ObjRequest, string Endpoint)
        {
            BaseResponse response = new BaseResponse();
            ResponseSpLogPasarelaExterna ResponseSpLog = new ResponseSpLogPasarelaExterna();
            try
            {
                SpLogPasarelaExterna ObjSpRequest = new SpLogPasarelaExterna();

                ObjSpRequest.IdTransaccion = await _TransactionRepository.GetIdTranaccionbyReferenciaExterno(ObjRequest.transaction_ide);
                ObjSpRequest.Endpoint = Endpoint;
                ObjSpRequest.Request = null;
                ObjSpRequest.Response = JsonSerializer.Serialize(ObjRequest);
                ObjSpRequest.Enviada = false;

                ResponseSpLog = await _logRepository.LoggExternalPasarela(ObjSpRequest);

                if (ResponseSpLog.IdTransaccion > 0)
                {
                    BaseResponseBpay resp = BePayClient.GetDataTransaction(ObjRequest.transaction_ide);
                    if (resp != null)
                    {
                        ObjSpRequest.Endpoint = "BPayClient.GetDataTransaction";
                        ObjSpRequest.Request = JsonSerializer.Serialize(ObjRequest.transaction_ide);
                        ObjSpRequest.Response = JsonSerializer.Serialize(resp);
                        ObjSpRequest.Enviada = true;

                        ResponseSpLog = await _logRepository.LoggExternalPasarela(ObjSpRequest);
                        int EstadoAnterior = ResponseSpLog.IdEstadoTransaccion;
                        if (EstadoAnterior == (int)enumEstadoTransaccion.Pendiente || EstadoAnterior == (int)enumEstadoTransaccion.PendienteConfirmacionPago)
                        {
                            ActualizarEstadoTransaccion ActualizarEstado = new ActualizarEstadoTransaccion();
                            ActualizarEstado.IdTransaccion = ResponseSpLog.IdTransaccion;
                            if (resp.data.status.ToUpper() != "PENDING")
                            {
                                switch (resp.data.status.ToUpper())
                                {
                                    case "APPROVED":
                                        ActualizarEstado.idEstadoTransaccio = (int)enumEstadoTransaccion.Aprobado;
                                        break;
                                    case "REJECTED":
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

        #endregion
        #region private
        private async Task<RequestPaymentContinueBePay> GenerateRequestBePay(RequestPaymentContinue request, RequestCreateTransaction transactionBePay, ResponseSp_GetDataTransaccion dataTransaction)
        {
            RequestPaymentContinueBePay ObjRequest = new RequestPaymentContinueBePay();
            try
            {
                ObjRequest.email = dataTransaction.UsuCorreo;
                ObjRequest.nombres = dataTransaction.UsuNombre;
                ObjRequest.numero_documento = dataTransaction.UsuDocumento;
                ObjRequest.telefono = ObjRequest.nequi_push_phone = dataTransaction.UsuTelefono;
                ObjRequest.tipo_documento = await GetTypeDocumentBePay(dataTransaction.Documento);
                ObjRequest.redirect_url = transactionBePay.redirect_url;
                ObjRequest.direccion = "N/A";
                ObjRequest.apellidos = ObjRequest.nombres;
                ObjRequest.ciudad = 149237;
                ObjRequest.pais = 48;
                ObjRequest.company_terms = "Y";
                ObjRequest.transaction_ip = "0.0.0.0";
                ObjRequest.codigo_postal = "055422";
                

                switch (request.IdmedioPago)
                {
                    case (int)enumTypePayment.PSE:
                        ObjRequest.transaction_id = "voluptatem";
                        ObjRequest.metodopago = "PSE";
                        ObjRequest.typeuser = await GetTypePersonPSEBePay(request.Persona);
                        ObjRequest.bank = Convert.ToInt32(request.Banco);

                        break;
                    case (int)enumTypePayment.NEQUIPUSH:
                        ObjRequest.transaction_id = "nulla";
                        ObjRequest.metodopago = "NEQUI_PUSH";

                        break;
                }
            }
            catch (CustomException ex)
            {
                ObjRequest = null;
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                ObjRequest = null;
            }
            return ObjRequest;
        }
        private async Task<int> GetTypeDocumentBePay(string Documento)
        {
            int response = 0;
            try
            {
                switch (Documento)
                {
                    case "CC":
                        response = 2;
                        break;
                    case "CE":
                        response = 3;
                        break;
                    case "NIT":
                        response = 1;
                        break;
                    case "TI":
                        response = 5;
                        break;
                    case "PP":
                        response = 4;
                        break;
                }
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
            }
            return response;
        }
        private async Task<string> GetTypePersonPSEBePay(string Persona)
        {
            string response = "person";
            try
            {
                switch (Persona)
                {
                    case "NATURAL":
                        response = "person";
                        break;
                    case "JURIDÍCA":
                        response = "company";
                        break;
                }
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
            }
            return response;
        }
        #endregion

    }
}
