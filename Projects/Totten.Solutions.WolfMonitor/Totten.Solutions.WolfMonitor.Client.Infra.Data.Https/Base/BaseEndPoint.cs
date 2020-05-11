using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Exceptions;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ExtensionsMethods;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

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

        public CustomHttpCliente Client => _httpCliente;
        protected readonly CustomHttpCliente _httpCliente;
        public BaseEndPoint(CustomHttpCliente customHttpCliente)
        {
            _httpCliente = customHttpCliente;
        }

        protected async Task<Result<Exception, TResult>> InnerGetAsync<TResult>(string methodPath)
        {
            var httpRequest = _httpCliente.CreateRequest(HttpMethod.Get, methodPath);

            using (httpRequest)
            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    var convert = JsonConvert.DeserializeObject<TResult>(content);
                    return convert;
                }
                return JsonConvert.DeserializeObject<BusinessException>(content);
            }
        }

        protected async Task<Result<Exception, TResult>> InnerAsync<TResult, TPost>(string methodPath, TPost postBody, HttpMethod httpMethod)
        {
            var httpRequest = _httpCliente.CreateRequest(httpMethod, methodPath);

            if (postBody != null)
                httpRequest = httpRequest.AddJsonBody(postBody, new JsonSerializerSettings());

            using (httpRequest)
            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    var convert = JsonConvert.DeserializeObject<TResult>(content);
                    return convert;
                }
                return JsonConvert.DeserializeObject<BusinessException>(content);
            }
        }
    }
}
