using ECD.Utilidades.Recursos;
using Entities.General;
using MethodsParameters.Input.Client.BPay;
using MethodsParameters.Input.Gateway;
using MethodsParameters.Output.Client.Bpay;
using MethodsParameters.Output.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Client
{
    public class BPayClient
    {

        public static string UrlClient = "";
        public static string Usuario = "";
        public static string Contraseña = "";
        public static string CurrentToken = "";
        public static string TokenTransaccion = "";
        public static string Account = "";




        private static DateTime lastDateToken = DateTime.MinValue;


        public static T GetMapObject<T>(object input)
        {
            try
            {
                var resultData = JsonConvert.SerializeObject(input);
                T data = JsonConvert.DeserializeObject<T>(resultData);
                return data;
            }
            catch
            {
                return default(T);
            }
        }

        private static async Task Login ()
        {
            ConsumeServices ResponseExternal = new ConsumeServices();
            BaseResponseBpay response = await ResponseExternal.PostAsync<BaseResponseBpay>(UrlClient + "/api/v1/get-access-token", new
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
            }
            else
            {
                throw new CustomException("Información de token no valida");
            }
        }

        private static async Task CreateTransaction(RequestCreateTransaction ObjRequest)
        {
            ConsumeServices responseExternal = new ConsumeServices();
            try
            {
                Login().Wait();
                BaseResponseBpay response = await responseExternal.RestBearer<BaseResponseBpay>(UrlClient + "/api/v1/checkout/transactions", ObjRequest, CurrentToken, "BePay");
                if (response != null)
                {
                    if (response.success)
                    {
                        ReponseCreateTransaction ObjTransaction = JsonConvert.DeserializeObject<ReponseCreateTransaction>(response.data.ToString());
                        TokenTransaccion = ObjTransaction.ide;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al crear transaccion BePay.");

            }

        }

        public static BaseResponseBpay NequiPush(RequestNequiPush ObjRequest, RequestCreateTransaction ObjRequestTransactio)
        {
            BaseResponseBpay objresponse = new BaseResponseBpay();
            try
            {

                ObjRequest.direccion = " ";
                ObjRequest.apellidos = " ";
                ObjRequest.ciudad = 149237;
                ObjRequest.pais = 48;
                ObjRequest.company_terms = "Y";
                ObjRequest.transaction_id = "nulla";
                ObjRequest.transaction_ip = "0.0.0.0";
                ObjRequest.codigo_postal = "055422";
                ObjRequest.metodopago = "NEQUI_PUSH";
                ObjRequest.Token = TokenTransaccion;
                ObjRequestTransactio.account_id = ObjRequest.account_id = Convert.ToInt32(Account);

                ObjRequestTransactio.type = "link";
                ObjRequestTransactio.currency_code = "COP";
                ObjRequestTransactio.tax_percentage = "0";
                ObjRequestTransactio.extra2 = "";
                ObjRequestTransactio.extra3 = "";
                
                CreateTransaction(ObjRequestTransactio).Wait();
                ConsumeServices responseExternal = new ConsumeServices();
                var restBearer = responseExternal.RestBearer<BaseResponseBpay>(UrlClient + "/api/v1/checkout/checkoutNequiPush", ObjRequest, CurrentToken, "BePay");
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }

    }
}
