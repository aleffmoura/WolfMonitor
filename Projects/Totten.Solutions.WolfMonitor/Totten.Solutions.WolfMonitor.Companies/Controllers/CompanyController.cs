using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Companies.Commands;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

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

        #region Delete
        [HttpDelete("{agentId}/{Id}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System)]
        public async Task<IActionResult> RemoveItem([FromRoute]Guid agentId, [FromRoute]Guid id)
            => HandleCommand(await _mediator.Send(new CompanyRemove.Command(id, UserId, CompanyId)));
        #endregion

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System)]
        public async Task<IActionResult> Create([FromBody]CompanyCreateCommand command)
            => HandleCommand(await _mediator.Send(new CompanyCreate.Command {
                                                        Address= command.Address,
                                                        Cnae = command.Cnae,
                                                        Cnpj = command.Cnpj,
                                                        Email = command.Email,
                                                        FantasyName = command.FantasyName,
                                                        MunicipalRegistration = command.MunicipalRegistration,
                                                        Name = command.Name,
                                                        Phone = command.Phone,
                                                        StateRegistration = command.StateRegistration,
                                                        UserCreatedCompanyId = CompanyId,
                                                        UserCreatedId = UserId
                                                   }));
        #endregion

        #region HTTP PATCH
        #endregion

        #region HTTP GET
        [HttpGet]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Count)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System)]
        public async Task<IActionResult> ReadAll(ODataQueryOptions<CompanyResumeViewModel> queryOptions)
        {
            return await HandleQueryable<CompanyResumeViewModel, CompanyResumeViewModel>(await _mediator.Send(new CompaniesCollection.Query()), queryOptions);
        }


        [HttpGet("{companyId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAllInfo([FromRoute]Guid companyId)
            => HandleQuery<Company, CompanyDetailViewModel>(await _mediator.Send(new CompanyDetail.Query(companyId, CompanyId, Role)));

        #endregion

    }
}
