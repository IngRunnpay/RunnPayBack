using ECD.Utilidades.Recursos;
using MethodsParameters.Input.Gateway;
using MethodsParameters.Output.Gateway;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Client
{
    public static class TpagaClient
    {

        public static string UrlClient = "";
        public static string TokenClient = "";

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

        public static List<ResponseGetBank> GetBankPSE()
        {
            List<ResponseGetBank> objresponse = new List<ResponseGetBank>();
            try
            {
                ConsumeServices responseExternal = new ConsumeServices();                
                var restBearer = responseExternal.GetAsync<List<ResponseGetBank>>(UrlClient + "/public/banks", new { },TokenClient, "Tpaga");
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }
        public static ResponseCreatePse CreatePSETpago(RequestCreatePseTpago ObjRequest)
        {
            ResponseCreatePse objresponse = new ResponseCreatePse();
            try
            {
                ConsumeServices responseExternal = new ConsumeServices();
                var restBearer = responseExternal.RestBearer<ResponseCreatePse>(UrlClient + "/public/charge", ObjRequest, TokenClient, "Tpaga");
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }

        public static GetTransactionStatus GetInfoTransaccion(string tokenConsulta)
        {
            GetTransactionStatus objresponse = new GetTransactionStatus();
            try
            {
                ConsumeServices responseExternal = new ConsumeServices();
                var restBearer = responseExternal.GetAsync<GetTransactionStatus>(UrlClient + $"/public/charge/{tokenConsulta}", new { }, TokenClient, "Tpaga");
                restBearer.Wait();
                objresponse = restBearer.Result;

                return objresponse;

            }
            catch { }
            return objresponse;
        }

    }
}
