using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    [Route("Items")]
    public class ItemsController : ApiControllerBase
    {
        private IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST

        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromBody]ItemCreateVO itemCreate)
             => HandleCommand(await _mediator.Send(new ItemCreate.Command(CompanyId, UserId, itemCreate.AgentId,  itemCreate.Name, itemCreate.DisplayName, itemCreate.Default, itemCreate.Interval)));

        #endregion


        #region HTTP PATCH

        [HttpPatch]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> PatchClient([FromBody]ItemUpdateVO itemUpdate)
            => HandleCommand(await _mediator.Send(new ItemUpdate.Command(UserId, itemUpdate.Name, itemUpdate.Value, itemUpdate.LastValue, itemUpdate.MonitoredAt)));

        #endregion


        #region HTTP Delete

        [HttpDelete("{agentId}/{Id}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> RemoveItem([FromRoute]Guid agentId, [FromRoute]Guid id)
            => HandleCommand(await _mediator.Send(new ItemRemove.Command(agentId, id)));

        #endregion


        #region HTTP GET

        [HttpGet]
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> ReadAllByAgentId(ODataQueryOptions<Item> queryOptions)
            => await HandleQueryable<Item, ItemResumeForAgentViewModel>(await _mediator.Send(new ItemCollectionForAgent.Query(UserId)), queryOptions);

        [HttpGet("services/{agentId}")]
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAllServices([FromRoute]Guid agentId, ODataQueryOptions<Item> queryOptions)
            => await HandleQueryable<Item, ItemResumeViewModel>(await _mediator.Send(new ItemCollection.Query(agentId, CompanyId, ETypeItem.SystemService)), queryOptions);

        [HttpGet("configs/{agentId}")]
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAllconfigs([FromRoute]Guid agentId, ODataQueryOptions<Item> queryOptions)
            => await HandleQueryable<Item, ItemResumeViewModel>(await _mediator.Send(new ItemCollection.Query(agentId, CompanyId, ETypeItem.SystemService)), queryOptions);

        [HttpGet("historic/{itemId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadResumeItem([FromRoute]Guid itemId, ODataQueryOptions<ItemHistoric> queryOptions)
            => await HandleQueryable<ItemHistoric, ItemHistoric>(await _mediator.Send(new HistoricCollection.Query(itemId)), queryOptions);

        #endregion

    }
}
