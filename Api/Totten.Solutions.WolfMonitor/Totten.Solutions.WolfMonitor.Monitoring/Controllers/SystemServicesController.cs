using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Monitoring.Commands;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    [Route("SystemServices")]
    public class SystemServicesController : ApiControllerBase
    {
        private IMediator _mediator;

        public SystemServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromBody]SystemServiceCreateCommand command)
        {
            return HandleCommand(await _mediator.Send(new SystemServiceCreate.Command(CompanyId, command.AgentId, UserId, command.Name, command.DisplayName)));
        }
        #endregion

        #region HTTP PATCH
        [HttpPatch]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> PatchClient([FromBody]SystemServiceUpdateCommand command)
        {
            return HandleCommand(await _mediator.Send(new SystemServiceUpdate.Command(UserId, command.Value)));
        }
        #endregion

        #region HTTP GET
        [HttpGet]
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> ReadAllByAgentId(ODataQueryOptions<SystemService> queryOptions)
        {
            return await HandleQueryable<SystemService, SystemServiceResumeForAgentViewModel>(await _mediator.Send(new SystemServiceCollectionForAgent.Query(UserId)), queryOptions);
        }

        [HttpGet("{agentId}")]
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromRoute]Guid agentId, ODataQueryOptions<SystemService> queryOptions)
        {
            return await HandleQueryable<SystemService, SystemServiceResumeViewModel>(await _mediator.Send(new SystemServiceCollection.Query(agentId, CompanyId)), queryOptions);
        }

        [HttpGet("{agentId}/{serviceId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadResumeService([FromRoute]Guid serviceId, [FromRoute]Guid agentId)
        {
            return HandleQuery<SystemService, SystemServiceDetailViewModel>(await _mediator.Send(new SystemServiceResume.Query(CompanyId, agentId, serviceId)));
        }
        #endregion
    }
}
