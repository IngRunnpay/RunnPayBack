using ECD.Utilidades.Recursos;
using MethodsParameters.Input.Gateway;
using MethodsParameters.Input.Webhook;
using MethodsParameters.Output.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters.Client
{
    public static class WebHookClient
    {
        public static object SendClientWebhook(string urlClient, object ObjRequest)
        {
            object response = new object();
            try
            {
                ConsumeServices responseExternal = new ConsumeServices();
                var restBearer = responseExternal.RestBearer<object>(urlClient, ObjRequest);
                restBearer.Wait();
                response = restBearer.Result;

                return response;

            }
            catch { }
            return response;
        }
    }
}
