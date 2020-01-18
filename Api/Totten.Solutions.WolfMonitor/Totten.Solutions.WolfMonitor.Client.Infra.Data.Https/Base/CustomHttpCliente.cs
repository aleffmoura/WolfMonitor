using Newtonsoft.Json;
using System.Net.Http;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base
{
    public class CustomHttpCliente
    {
        private readonly string _uriBaseApi;

        public string UrlApi => _uriBaseApi;
        public HttpClient HttpClient { get; private set; }
        public CustomHttpCliente(string uriBaseApi)
        {
            _uriBaseApi = uriBaseApi;
            //new AuthenticationHandler(this, new HttpClientHandler())
            HttpClient = new HttpClient();
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
