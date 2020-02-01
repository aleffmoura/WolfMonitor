using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Requests
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private static readonly object Locker = new object();

        private readonly CustomHttpCliente _client;


        public AuthenticationHandler(CustomHttpCliente client, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(UserLogin.Token))
                UserLogin.Token = GetToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", UserLogin.Token);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                UserLogin.Token = GetToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", UserLogin.Token);
                return await base.SendAsync(request, cancellationToken);
            }
            return response;
        }

        private string GetToken()
        {
            lock (Locker)
            {
                var token = RequestNewToken();
                return token;
            }
        }
        public string GetClientCredentials()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"postman:postmanSecret"));
        }

        private string RequestNewToken()
        {
            var request = _client.CreateRequest(HttpMethod.Post, "identityserver/connect/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _client.User.GetClientCredentials());
            request.Headers.Host = _client.UrlApi.Replace("http://", "");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password"},
                { "username", _client.User.Login },
                { "password", _client.User.Password},
                { "scope", "Agents Monitoring Users"}
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
                return content.access_token.ToString();
            }
        }
    }
}
