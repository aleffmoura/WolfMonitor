using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.ServiceAgent.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Agents
{
    public class AgentInformation : BaseEndPoint
    {
        private Agent _agent;

        public AgentInformation(Agent agent, CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {
            _agent = agent;
        }
        public string Login()
        {
            var request = base.Client.CreateRequest(HttpMethod.Post, "identityserver/connect/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", UserLogin.GetClientCredentials());
            request.Headers.Host = base.Client.UrlApi.Replace("http://", "");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password"},
                { "username",  $"{base.Client.User.Login}@{_agent.Company}#agent" },
                { "password",  base.Client.User.Password},
                { "scope", "Agents Monitoring"}
            });

            using (var httpClient = new HttpClient())
            using (request)
            using (var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Erro na comunicação com a API, não foi possivel obter o token. status de erro: {response.StatusCode}");

                dynamic content = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
                return content.access_token.ToString();
            }
        }

        public Result<Exception, Unit> Send(Item item)
             => InnerAsync<Result<Exception, Unit>, Item>("monitoring/Items", item, HttpMethod.Patch).ConfigureAwait(false).GetAwaiter().GetResult();
        
        public Result<Exception, PageResult<Item>> GetItems()
             =>  InnerGetAsync<PageResult<Item>>("monitoring/items").ConfigureAwait(false).GetAwaiter().GetResult();

        public Result<Exception, Unit> Update(Agent agent)
             =>  InnerAsync(agent, HttpMethod.Patch).ConfigureAwait(false).GetAwaiter().GetResult();

        public Result<Exception, Agent> GetInfo()
             =>  InnerGetAsync<Agent>("agents/info").ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task<Result<Exception, Unit>> InnerAsync(Agent agent, HttpMethod httpMethod)
             =>  await InnerAsync<Result<Exception, Unit>, Agent>("agents", agent, httpMethod);

    }
}
