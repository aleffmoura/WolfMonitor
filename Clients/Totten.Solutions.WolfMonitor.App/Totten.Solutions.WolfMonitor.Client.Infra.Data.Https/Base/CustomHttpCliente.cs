using Newtonsoft.Json;
using System.Net.Http;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Users;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base
{
    public class CustomHttpCliente
    {
        private readonly string _uriBaseApi;

        public User User { get; private set; }
        public string UrlApi => _uriBaseApi;
        public HttpClient HttpClient { get; private set; }
        public CustomHttpCliente(string uriBaseApi, User user)
        {
            _uriBaseApi = uriBaseApi;
            HttpClient = new HttpClient(new AuthenticationHandler(this, new HttpClientHandler()));
            User = user;
        }
        private string Concat(string partialUri)
        {
            return $@"{_uriBaseApi}/{partialUri}";
        }

        public HttpRequestMessage CreateRequest(HttpMethod httpMethod, string endPoint)
        {
            var request = new HttpRequestMessage(httpMethod, Concat(endPoint));
            return request;
        }
    }
}
