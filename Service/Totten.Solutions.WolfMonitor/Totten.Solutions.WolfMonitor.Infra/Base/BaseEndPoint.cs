using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.Extensions;

namespace Totten.Solutions.WolfMonitor.Infra.Base
{
    public class BaseEndPoint
    {
        private readonly CustomHttpCliente _httpCliente;
        protected string _endPoint;

        protected static readonly HttpStatusCode[] KnownStatusCodeForPost =
           {
            HttpStatusCode.Created,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden,
            HttpStatusCode.Conflict,
            HttpStatusCode.InternalServerError,
        };
        public BaseEndPoint(CustomHttpCliente customHttpCliente)
        {
            _httpCliente = customHttpCliente;
        }

        protected async Task<TResult> SendAsync<TResult, TPost>(string methodPath, HttpMethod httpMethod, TPost postBody = null) where TPost : class
        {
            var httpRequest = _httpCliente.CreateRequest(httpMethod, methodPath);

            if(postBody != null)
                httpRequest.AddJsonBody(postBody, new JsonSerializerSettings());

            using (httpRequest)
            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
            {
                var str = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(str);
            }
        }
    }
}
