using ECD.Utilidades.Recursos;
using Entities.General;
using Entities.Input.Client.BPay;
using Entities.Input.Gateway;
using Entities.Output.Client.Bpay;
using Entities.Output.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MethodsParameters.Client
{
    public static class BePayClient
    {

        public static string UrlClient = "";
        public static string Usuario = "";
        public static string Contraseña = "";
        public static string CurrentToken = "";
        public static string TokenTransaccion = "";
        public static string Account = "";


        private static async Task Login ()
        {
            ConsumeExternalServices ResponseExternal = new ConsumeExternalServices();
            LogShared.LogDataDetail("Login", "", Usuario, Contraseña);

            ResponseLogin response = await ResponseExternal.PostAsync<ResponseLogin>(UrlClient + "/api/v1/get-access-token", new
            {
                email = Usuario,
                password = Contraseña
            });

            if (response != null)
            {
                if (response.success)
                {
                    CurrentToken = response.data;
                }
                else
                {
                    throw new CustomException("Información de token no valida1");
                }
            }
            else
            {
                throw new CustomException("Información de token no valida");
            }
        }

        private static async Task CreateTransaction(RequestCreateTransaction ObjRequest)
        {
            ConsumeExternalServices responseExternal = new ConsumeExternalServices();
            try
            {
                Login().Wait();
                LogShared.LogDataDetail("CreateTransaction", "", JsonConvert.SerializeObject(ObjRequest), CurrentToken);
                BaseResponseBpay response = await responseExternal.RestBearer<BaseResponseBpay>(UrlClient + "/api/v1/checkout/transactions", ObjRequest, CurrentToken, "BePay");
                LogShared.LogDataDetail("CreateTransaction", "", JsonConvert.SerializeObject(response), CurrentToken);

                if (response != null)
                {
                    if (response.success)
                    {
                        TokenTransaccion = response.data.ide;
                    }
                }
                else
                {
                    throw new CustomException($"Error al crear transaccion BePay. {Usuario}| {Contraseña}");
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al crear transaccion BePay.");

            }

        }

        public static BaseResponseBpay NequiPush(RequestPaymentContinueBePay ObjRequest, RequestCreateTransaction ObjRequestTransactio)
        {
            BaseResponseBpay objresponse = new BaseResponseBpay();
            try
            {
                ObjRequestTransactio.account_id = ObjRequest.account_id = Convert.ToInt32(Account);
                ObjRequestTransactio.type = "link";
                ObjRequestTransactio.currency_code = "COP";
                ObjRequestTransactio.tax_percentage = "0";
                ObjRequestTransactio.extra2 = "";
                ObjRequestTransactio.extra3 = "";
                
                CreateTransaction(ObjRequestTransactio).Wait();
                ObjRequest.Token = TokenTransaccion;
                if (string.IsNullOrEmpty(TokenTransaccion))
                {
                    throw new CustomException($"Error al crear transaccion BePay. {Usuario}| {Contraseña}");
                }
                else
                {
                    LogShared.LogDataDetail("NequiPush", $"token: {TokenTransaccion}", JsonConvert.SerializeObject(ObjRequest), CurrentToken);

                    ConsumeExternalServices responseExternal = new ConsumeExternalServices();
                    var restBearer = responseExternal.RestBearer<BaseResponseBpay>(UrlClient + "/api/v1/checkout/checkoutNequiPush", ObjRequest, CurrentToken, "BePay");
                    restBearer.Wait();
                    objresponse = restBearer.Result;
                    return objresponse;

                }

            }
            catch { }
            return objresponse;
        }

        public static BaseResponseBpay PSE(RequestPaymentContinueBePay ObjRequest, RequestCreateTransaction ObjRequestTransactio)
        {
            BaseResponseBpay objresponse = new BaseResponseBpay();
            try
            {
                ObjRequestTransactio.account_id = ObjRequest.account_id = Convert.ToInt32(Account);
                ObjRequestTransactio.type = "link";
                ObjRequestTransactio.currency_code = "COP";
                ObjRequestTransactio.tax_percentage = "0";
                ObjRequestTransactio.extra2 = "";
                ObjRequestTransactio.extra3 = "";

                CreateTransaction(ObjRequestTransactio).Wait();
                ObjRequest.Token = TokenTransaccion;
                if (string.IsNullOrEmpty(TokenTransaccion))
                {
                    throw new CustomException($"Error al crear transaccion BePay. {Usuario}| {Contraseña}");
                }
                else
                {
                    ConsumeExternalServices responseExternal = new ConsumeExternalServices();
                    var restBearer = responseExternal.RestBearer<BaseResponseBpay>(UrlClient + "/api/v1/checkout/checkoutPse", ObjRequest, CurrentToken, "BePay");
                    restBearer.Wait();
                    objresponse = restBearer.Result;

                    return objresponse;

                }
               

            }
            catch { }
            return objresponse;
        }

        public static BaseResponseBpay GetDataTransaction(string TokenBp)
        {
            BaseResponseBpay objresponse = new BaseResponseBpay();
            try
            {
                Login().Wait();

                ConsumeExternalServices responseExternal = new ConsumeExternalServices();
                var restBearer = responseExternal.GetAsync<BaseResponseBpay>(UrlClient + $"/api/v1/checkout/transactionStatus?Token={TokenBp}&account_id={Convert.ToInt32(Account)}", new { }, CurrentToken, "BePay");
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }


    }
}
