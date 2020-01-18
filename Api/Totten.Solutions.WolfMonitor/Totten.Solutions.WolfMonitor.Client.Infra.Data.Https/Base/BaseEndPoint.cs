//using Newtonsoft.Json;
//using System;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Totten.Solutions.WolfMonitor.Client.Domain.Base;
//using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ExtensionsMethods;
//using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

//namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base
//{
//    public class BaseEndPoint
//    {
//        protected static readonly HttpStatusCode[] KnownStatusCodeForPost =
//           {
//            HttpStatusCode.Created,
//            HttpStatusCode.BadRequest,
//            HttpStatusCode.Unauthorized,
//            HttpStatusCode.Forbidden,
//            HttpStatusCode.Conflict,
//            HttpStatusCode.InternalServerError,
//        };

//        private readonly CustomHttpCliente _httpCliente;
//        public BaseEndPoint(CustomHttpCliente customHttpCliente)
//        {
//            _httpCliente = customHttpCliente;
//        }

//        protected async Task<Result<Exception, TResult>> InnerGetAsync<TResult>(string methodPath)
//        {
//            var httpRequest = _httpCliente.CreateRequest(HttpMethod.Get, methodPath);

//            using (httpRequest)
//            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
//            {
//                var content = await httpResponse.Content.ReadAsStringAsync();
//                try
//                {
//                    return JsonConvert.DeserializeObject<TResult>(content);
//                }
//                catch
//                {
//                    return JsonConvert.DeserializeObject<Exception>(content);
//                }
//            }
//        }

//        protected async Task<Result<Exception, ApiResult<TResult>>> InnerGetListAsync<TResult>(string methodPath)
//        {
//            var httpRequest = _httpCliente.CreateRequest(HttpMethod.Get, methodPath);

//            using (httpRequest)
//            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
//            {
//                var content = await httpResponse.Content.ReadAsStringAsync();
//                try
//                {
//                    return JsonConvert.DeserializeObject<ApiResult<TResult>>(content);
//                }
//                catch
//                {
//                    return JsonConvert.DeserializeObject<Exception>(content);
//                }
//            }
//        }
//        protected async Task<TResult> InnerAsync<TResult, TPost>(string methodPath, TPost postBody, HttpMethod httpMethod)
//        {
//            if (postBody == null) throw new ArgumentNullException(nameof(postBody));

//            var httpRequest = _httpCliente.CreateRequest(httpMethod, methodPath)
//                .AddJsonBody(postBody, new JsonSerializerSettings());

//            using (httpRequest)
//            using (var httpResponse = await _httpCliente.HttpClient.SendAsync(httpRequest))
//            {
//                var str = await httpResponse.Content.ReadAsStringAsync();
//                return JsonConvert.DeserializeObject<TResult>(str);
//            }
//        }
//    }
//}
