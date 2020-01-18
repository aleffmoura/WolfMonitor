using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Users.ViewModels;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Users
{
    public class UserService : IUserService
    {
        private CustomHttpCliente _client;
        public UserService(CustomHttpCliente client)
        {
            _client = client;
        }
        public string GetClientCredentials()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"postman:postmanSecret"));
        }
        public UserLoginViewModel Authentication(string userName, string password)
        {
            var request = _client.CreateRequest(HttpMethod.Post, "identityserver/connect/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetClientCredentials());
            request.Headers.Host = _client.UrlApi.Replace("http://", "");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password"},
                { "username", userName },
                { "password", password},
                { "scope", "Users Agents Monitoring Register"}
            });

            using (var httpClient = new HttpClient())
            using (request)
            using (var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro na comunicação com a API, não foi possivel obter o token. status de erro: {response.StatusCode}");
                }

                dynamic content = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
                UserLoginViewModel userLoginViewModel = new UserLoginViewModel
                {
                    Token = content.access_token.ToString()
                };
                return userLoginViewModel;
            }
        }
    }
}
