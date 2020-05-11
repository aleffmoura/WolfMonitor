using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings
{
    public class ItemsEndPoint : BaseEndPoint
    {
        private string _endpoint = "monitoring/items";
        public ItemsEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }

        public async Task<Result<Exception, Guid>> Post<T>(T Item)
        {
            return await InnerAsync<Guid,T>(_endpoint, Item, HttpMethod.Post);
        }

        public async Task<Result<Exception, PageResult<T>>> GetServicesByAgentId<T>(Guid agentId)
        {
            return await InnerGetAsync<PageResult<T>>($"{_endpoint}/services/{agentId}");
        }

        public async Task<Result<Exception, PageResult<T>>> GetconfigsByAgentId<T>(Guid agentId)
        {
            return await InnerGetAsync<PageResult<T>>($"{_endpoint}/configs/{agentId}");
        }

        public async Task<Result<Exception, Unit>> Delete(Guid agentId, Guid id)
        {
            return await InnerAsync<Unit, object>($"{_endpoint}/{agentId}/{id}", null, HttpMethod.Delete);
        }

        public async Task<Result<Exception, PageResult<T>>> GetItemHistoric<T>(Guid id, string take, string skip)
        {
            return await InnerGetAsync<PageResult<T>>($"{_endpoint}/historic/{id}?$count=true&$top={take}&$skip={skip}");
        }

        public async Task<Result<Exception, PageResult<T>>> GetSolicitations<T>(Guid id, string take, string skip)
        {
            return await InnerGetAsync<PageResult<T>>($"{_endpoint}/solicitations/{id}?$count=true&$take={take}&$skip={skip}");
        }

    }
}
