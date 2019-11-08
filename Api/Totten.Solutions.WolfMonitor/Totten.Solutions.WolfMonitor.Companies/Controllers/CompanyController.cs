using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;

namespace Totten.Solutions.WolfMonitor.Companies.Controllers
{
    [Route("")]
    public class CompanyController : ApiControllerBase
    {
        private IMediator _mediator;
        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CompanyCreate.Command command)
            => HandleCommand(await _mediator.Send(command));
        #endregion

        //#region HTTP PATCH
        //[HttpPatch]
        //public async Task<IActionResult> PatchClient([FromBody]AgentUpdate.Command command)
        //{
        //    return HandleCommand(await _mediator.Send(command));
        //}
        //#endregion

        #region HTTP GET
        [HttpGet]
        [ODataQueryOptionsValidate]
        public async Task<IActionResult> ReadAll(ODataQueryOptions<Company> queryOptions)
        {
            return await HandleQueryable<Company, CompanyResumeViewModel>(await _mediator.Send(new CompaniesCollection.Query()), queryOptions);
        }
        //[HttpGet("{companyId}/{agentId}")]
        //public async Task<IActionResult> ReadById([FromRoute]Guid companyId, [FromRoute]Guid agentId)
        //{
        //    return HandleQuery<Agent, AgentDetailViewModel>(await _mediator.Send(new AgentResume.Query(companyId, agentId)));
        //}
        #endregion

    }
}
