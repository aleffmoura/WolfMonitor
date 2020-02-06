using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings
{
    public class ItemsEndPoint : BaseEndPoint
    {
        public ItemsEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }


        //private async Task<Result<Exception, Unit>> InnerAsync(SystemService services, HttpMethod httpMethod)
        //{
        //    return await InnerAsync<Result<Exception, Unit>, SystemService>("monitoring/systemservices", agent, httpMethod);
        //}
        public async Task<Result<Exception, Guid>> Post<T>(T Item)
        {
            return await InnerAsync<Guid,T>("monitoring/items", Item, HttpMethod.Post);
        }
        public async Task<Result<Exception, PageResult<T>>> GetServicesByAgentId<T>(Guid agentId)
        {
            return await InnerGetAsync<PageResult<T>>($"monitoring/items/services/{agentId}");
        }
        public async Task<Result<Exception, PageResult<T>>> GetconfigsByAgentId<T>(Guid agentId)
        {
            return await InnerGetAsync<PageResult<T>>($"monitoring/configs/{agentId}");
        }

        public async Task<Result<Exception, Unit>> Delete(Guid agentId, Guid id)
        {
            return await InnerAsync<Unit, object>($"monitoring/{agentId}/{id}", null, HttpMethod.Delete);
        }
    }
}
