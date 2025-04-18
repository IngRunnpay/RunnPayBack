using ECD.Utilidades.Recursos;
using Entities.Input.Gateway;
using Entities.Input.Webhook;
using Entities.Output.Gateway;
using Newtonsoft.Json;
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
                ConsumeExternalServices responseExternal = new ConsumeExternalServices();
                var restBearer = responseExternal.RestBearer<object>(urlClient, ObjRequest);
                LogShared.Loghttps("WebHook", urlClient, JsonConvert.SerializeObject(ObjRequest), JsonConvert.SerializeObject(restBearer.Result));
                LogShared.Loghttps("WebHook", urlClient, JsonConvert.SerializeObject(ObjRequest), JsonConvert.SerializeObject(restBearer));

                restBearer.Wait();
                response = restBearer.Result;

                return response;

            }
            catch { }
            return response;
        }
    }
}
