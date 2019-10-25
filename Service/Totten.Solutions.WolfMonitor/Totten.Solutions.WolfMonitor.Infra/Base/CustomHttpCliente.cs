using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.Base
{
    public class CustomHttpCliente
    {
        private readonly string _uriBaseApi;
        public User User { get; private set; }
        public HttpClient HttpClient { get; private set; }
        public CustomHttpCliente(string uriBaseApi,ref User user)
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
