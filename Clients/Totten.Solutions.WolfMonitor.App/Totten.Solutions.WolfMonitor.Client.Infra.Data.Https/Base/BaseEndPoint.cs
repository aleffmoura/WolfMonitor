using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ExtensionsMethods;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base
{
    public class BaseEndPoint
    {
        protected static readonly HttpStatusCode[] KnownStatusCodeForPost =
           {
            HttpStatusCode.Created,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden,
            HttpStatusCode.Conflict,
            HttpStatusCode.InternalServerError,
        };

        private readonly CustomHttpCliente _httpCliente;
        public BaseEndPoint(CustomHttpCliente customHttpCliente)
        {
            _httpCliente = customHttpCliente;
        }

        protected async Task<T> InnerGetAsync<T>(string methodPath)
        {
            var httpRequest = _httpCliente.CreateRequest(HttpMethod.Get, methodPath);

            using (httpRequest)
            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        protected async Task<TResult> InnerAsync<TResult, TPost>(string methodPath, TPost postBody, HttpMethod httpMethod)
        {
            if (postBody == null) throw new ArgumentNullException(nameof(postBody));

            var httpRequest = _httpCliente.CreateRequest(httpMethod, methodPath)
                .AddJsonBody(postBody, new JsonSerializerSettings());

            using (httpRequest)
            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
            {
                var str = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(str);
            }
        }
    }
}
