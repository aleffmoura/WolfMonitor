using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.Base;
using Totten.Solutions.WolfMonitor.Infra.Configurations;

namespace Totten.Solutions.WolfMonitor.Infra.Features.Items
{
    public class ItemApiRepository : BaseEndPoint, ItemRepository
    {
        public ItemApiRepository(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {
            base._endPoint = $"{Cfg.ApiUrl}{(Cfg.ApiUrl[Cfg.ApiUrl.Length - 1].Equals("/") ? "items" : " /items")}";
        }
        public async Task<Guid> AddAsync(Item item)
        {
            return await SendAsync<Guid, Item>(_endPoint, HttpMethod.Post, item);
        }
        public async Task<List<Item>> GetAll(Guid agentId, EType type)
        {
            return await SendAsync<List<Item>, Item>($"{_endPoint}/{agentId}/{type.ToString()}", HttpMethod.Get, null);
        }
    }
}
