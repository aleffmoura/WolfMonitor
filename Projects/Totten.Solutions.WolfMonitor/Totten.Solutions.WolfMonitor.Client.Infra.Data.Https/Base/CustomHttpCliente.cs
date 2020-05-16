using Newtonsoft.Json;
using System.Net.Http;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base
{
    public class CustomHttpCliente
    {
        private readonly string _uriBaseApi;

        public string UrlApi => _uriBaseApi;
        public HttpClient HttpClient { get; private set; }
        public UserLogin User { get; private set; }

        public CustomHttpCliente(string uriBaseApi, UserLogin userLogin, bool ignoreAuth = false)
        {
            _uriBaseApi = uriBaseApi;
            HttpClient = ignoreAuth ? new HttpClient() : new HttpClient(new AuthenticationHandler(this, new HttpClientHandler()));
            User = userLogin;
        }

        private string Concat(string partialUri)
        {
            return $@"{_uriBaseApi}/{partialUri}";
        }

        public HttpRequestMessage CreateRequest(HttpMethod httpMethod, string endPoint)
        {
            var concated = Concat(endPoint);
            var request = new HttpRequestMessage(httpMethod, concated);
            return request;
        }
    }
}
