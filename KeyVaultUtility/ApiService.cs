using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultUtility
{
    public class ApiService
    {

        //Llamada al cliente Http con el token Bearer.
        public static string ApiGet(string serviceUri, string token)
        {
            return CallApi(serviceUri, token, HttpMethod.Get);
        }

        public static string ApiPost(string serviceUri, string token)
        {
            return CallApi(serviceUri, token, HttpMethod.Post);
        }

        public static string ApiPost(string serviceUri, string token, string payload)
        {
            return CallApi(serviceUri, token, HttpMethod.Post, payload);
        }

        public static string ApiPut(string serviceUri, string token, string payload)
        {
            return CallApi(serviceUri, token, HttpMethod.Put, payload);
        }

        public static string ApiDelete(string serviceUri, string token)
        {
            return CallApi(serviceUri, token, HttpMethod.Delete);
        }

        private static string CallApi(string serviceUri, string token, HttpMethod method, string payload = null)
        {
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //autenticacion
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //send a HTTP GET request for a given resource
                    string content = string.Empty;

                    Task<HttpResponseMessage> response = null;
                    if (string.IsNullOrEmpty(payload))
                    {
                        var request = new HttpRequestMessage(method, serviceUri);
                        response = client.SendAsync(request);
                    }
                    else
                    {
                        if(method == HttpMethod.Get)
                        {
                            response = client.GetAsync(serviceUri);
                        }
                        else if (method == HttpMethod.Delete)
                        {
                            response = client.DeleteAsync(serviceUri);
                        }
                        else if (method == HttpMethod.Post)
                        {
                            HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                            response = client.PostAsync(serviceUri, httpContent);
                        }
                        else if (method == HttpMethod.Put)
                        {
                            HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                            response = client.PutAsync(serviceUri, httpContent);
                        }
                    }
                    content = response.GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    result = content.ToString();
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
                return result;
            }
        }

    }
}
