using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using Totten.Solutions.WolfMonitor.ServiceAgent.Base;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base
{
    public class CustomHttpCliente
    {
        private readonly string _uriBaseApi;

        public string UrlApi => _uriBaseApi;
        public HttpClient HttpClient { get; private set; }
        public UserLogin User => JsonConvert.DeserializeObject<AgentSettings>(File.ReadAllText("./AgentSettings.json")).User;

        public CustomHttpCliente(string uriBaseApi)
        {
            _uriBaseApi = uriBaseApi;
            HttpClient = new HttpClient(new AuthenticationHandler(this, new HttpClientHandler()));
        }
        private string Concat(string partialUri)
        {
            var adc = $@"{_uriBaseApi}/{partialUri}";
            return adc;
        }

        public HttpRequestMessage CreateRequest(HttpMethod httpMethod, string endPoint)
        {
            var request = new HttpRequestMessage(httpMethod, Concat(endPoint));
            return request;
        }
    }
}
