using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyVaultUtility.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RestSharp;

namespace KeyVaultUtility
{
    public class KeyVault
    {
        public const string RESOUCEURL = "https://vault.azure.net";
        public const string GETSECRETS = "https://{0}.vault.azure.net/secrets?api-version=7.2";
        public const string GETSECRET = "https://{0}.vault.azure.net/secrets/{1}?api-version=7.2";
        public const string SETSECRET = "https://{0}.vault.azure.net/secrets/{1}?api-version=7.2";
        public const string DELETESECRET = "https://{0}.vault.azure.net/secrets/{1}?api-version=7.2";
        


        Settings _setting;
        public KeyVault(Settings setting)
        {
            _setting = setting;
            
        }

        async Task<string> GetToken()
        {
            var tenant = _setting.Tenant;

            string authorityUrl = $"https://login.microsoftonline.com/{tenant}";
            var authContext = new AuthenticationContext(authorityUrl);

            var clientCred = new ClientCredential(_setting.ClientId, _setting.SecretId);
            AuthenticationResult authenticationResult = await authContext.AcquireTokenAsync(RESOUCEURL, clientCred);

            return authenticationResult.AccessToken;
        }

        public List<string> ListSecretValues()
        {
            var r = ListSecret();
            List<string> items = new List<string>();

            foreach (var item in r.value)
            {
                var id = item.id.Substring(item.id.LastIndexOf("/")+1);
                items.Add(id);
            }
            return items;
        }


        private GetSecretListResponse.RootElement ListSecret()
        {
            var url = string.Format(GETSECRETS, _setting.UniqueKeyVaultName);
            var responseJson = GetDataFromKeyVault(url);

            var responseRoot = SimpleJson.DeserializeObject<GetSecretListResponse.RootElement>(responseJson);

            return responseRoot;
        }

        public GetSecretRespone.Rootobject GetSecret(string secretId)
        {
            var url = string.Format(GETSECRET, _setting.UniqueKeyVaultName, secretId);
            var responseJson = GetDataFromKeyVault(url);

            var responseRoot = SimpleJson.DeserializeObject<GetSecretRespone.Rootobject>(responseJson);
            return responseRoot;
        }

        public PutSecretResponse.Rootobject SetSecret(string secretId, string value)
        {
            var url = string.Format(SETSECRET, _setting.UniqueKeyVaultName, secretId);

            var payload = " { \"value\": \"" + value + "\" } ";

            var token = GetToken();
            token.Wait();

            var responseJson = ApiService.ApiPut(url, token.Result, payload);

            var responseRoot = SimpleJson.DeserializeObject<PutSecretResponse.Rootobject>(responseJson);
            return responseRoot;
        }

        public DeleteSecretResponse.Rootobject DeleteSecret(string secretId)
        {
            var url = string.Format(DELETESECRET, _setting.UniqueKeyVaultName, secretId);
            var token = GetToken();
            token.Wait();
            var responseJson = ApiService.ApiDelete(url, token.Result);

            var responseRoot = SimpleJson.DeserializeObject<DeleteSecretResponse.Rootobject>(responseJson);
            return responseRoot;
        }


        private string GetDataFromKeyVault(string resourceUrl)
        {
            var token = GetToken();
            token.Wait();
            return ApiService.ApiGet(resourceUrl, token.Result);
        }


    }
}
