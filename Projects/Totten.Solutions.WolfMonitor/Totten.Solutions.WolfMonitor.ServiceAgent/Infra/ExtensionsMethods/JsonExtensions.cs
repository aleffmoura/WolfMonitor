using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.ExtensionsMethods
{
    public static class JsonExtensions
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
