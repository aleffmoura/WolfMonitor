using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.Extensions
{
    public static class Ext
    {
        public static HttpRequestMessage AddJsonBody(this HttpRequestMessage httpRequestMessage, object jsonObject, JsonSerializerSettings serializationSettings)
        {
            var json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented, serializationSettings);
            httpRequestMessage.Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");
            return httpRequestMessage;
        }
    }
}
