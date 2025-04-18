using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Newtonsoft.Json.Linq;
using MethodsParameters.Client;
using System.Threading.Channels;

namespace ECD.Utilidades.Recursos
{
    public class ConsumeExternalServices
    {
        private HttpClient Client;

        public async Task<T> GetAsync<T>(string url, object request)
        {
            CreateNewInstanClient();

            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);

            using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
            {
                LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), stringContent.ToString(), "GetAsync");

                var response = Client.GetAsync(Client.BaseAddress).Result;

                if (!response.IsSuccessStatusCode)
                {
                    LogShared.Loghttps("GetAsync", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result));

                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }

                var responseString = response.Content.ReadAsStringAsync().Result;
                LogShared.Loghttps("GetAsync", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(responseString));
                LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), responseString.ToString(), "GetAsync");

                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
        public async Task<T> PostAsync<T>(string url, object request, string username = null, string password = null, string channel = null)
        {
            CreateNewInstanClient();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            }
            if (!string.IsNullOrEmpty(channel))
            {
                Client.DefaultRequestHeaders.Add("ChannelAuthorization", "Authorization " + channel);
            }
            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);

            using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
            {
                LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), JsonConvert.SerializeObject(request), channel);
                var response = Client.PostAsync(Client.BaseAddress, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    LogShared.Loghttps("PostAsync", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result));
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                var responseString = response.Content.ReadAsStringAsync().Result;
                LogShared.Loghttps("PostAsync", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(responseString));
                LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), responseString, channel);


                return JsonConvert.DeserializeObject<T>(responseString);
            }

        }
        public async Task<T> RestBearer<T>(string url, object request, string token = null, string channel = null)
        {
            CreateNewInstanClient();
            if (!string.IsNullOrEmpty(token))
            {
                string encodedUser = "";
                switch (channel)
                {
                    case "Tpaga":
                        encodedUser = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedUser);

                        break;
                    case "BePay":
                        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        break;
                }
            }

            Client.BaseAddress = new Uri(url);
            var jsonRequest = JsonConvert.SerializeObject(request);
            LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), jsonRequest.ToString(), "RestBearer");


            using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
            {

                var response = Client.PostAsync(Client.BaseAddress, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;
                    LogShared.Loghttps("RestBearer|PostAsync", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(obj));

                    return JsonConvert.DeserializeObject<T>(obj);
                }
                var responseString = response.Content.ReadAsStringAsync().Result;
                LogShared.Loghttps("RestBearer|PostAsync", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(responseString));
                LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), responseString.ToString(), "RestBearer");

                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
        public async Task<T> GetAsync<T>(string url, object request, string accessToken = null, string channel = null)
        {
            try
            {
                CreateNewInstanClient();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    string encodedUser = "";
                    switch (channel)
                    {
                        case "Tpaga":
                            encodedUser = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessToken));
                            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedUser);

                            break;
                        case "BePay":
                            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            break;
                    }
                }

                Client.BaseAddress = new Uri(url);
                var jsonRequest = JsonConvert.SerializeObject(request);
                LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), jsonRequest.ToString(), "GetAsync");

                using (var stringContent = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json"))
                {
                    var response = Client.GetAsync(Client.BaseAddress).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        LogShared.Loghttps("GetAsync|155", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result));

                        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                    }

                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<T>(responseString);
                    LogShared.Loghttps("GetAsync|162", url, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(responseString));
                    LogShared.LogDataDetail(url, Client.BaseAddress.ToString(), result.ToString(), "GetAsync");

                    return result;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        private void CreateNewInstanClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            Client = new HttpClient(clientHandler);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
        }
    }
}
