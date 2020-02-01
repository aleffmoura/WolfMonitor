using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
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

        //#region HTTP POST
        //[HttpPost]
        //[CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        //public async Task<IActionResult> Create([FromBody]ItemCreateCommand command)
        //{
        //    return HandleCommand(await _mediator.Send(new ItemCreate.Command(CompanyId, command.AgentId, UserId, command.Name, command.DisplayName)));
        //}
        //#endregion

        //#region HTTP PATCH
        //[HttpPatch]
        //[CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        //public async Task<IActionResult> PatchClient([FromBody]ItemUpdateCommand command)
        //{
        //    return HandleCommand(await _mediator.Send(new ItemUpdate.Command(UserId, command.Name, command.Value)));
        //}
        //#endregion

        //#region HTTP GET
        //[HttpGet]
        //[ODataQueryOptionsValidate]
        //[CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        //public async Task<IActionResult> ReadAllByAgentId(ODataQueryOptions<Item> queryOptions)
        //{
        //    return await HandleQueryable<Item, ItemResumeForAgentViewModel>(await _mediator.Send(new ItemCollectionForAgent.Query(UserId)), queryOptions);
        //}

        //[HttpGet("{agentId}")]
        //[ODataQueryOptionsValidate]
        //[CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        //public async Task<IActionResult> ReadAll([FromRoute]Guid agentId, ODataQueryOptions<Item> queryOptions)
        //{
        //    return await HandleQueryable<Item, ItemResumeViewModel>(await _mediator.Send(new ItemCollection.Query(agentId, CompanyId)), queryOptions);
        //}

        //[HttpGet("{agentId}/{serviceId}")]
        //[CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        //public async Task<IActionResult> ReadResumeService([FromRoute]Guid serviceId, [FromRoute]Guid agentId)
        //{
        //    return HandleQuery<Item, ItemDetailViewModel>(await _mediator.Send(new ItemResume.Query(CompanyId, agentId, serviceId)));
        //}
        //#endregion
    }
}
