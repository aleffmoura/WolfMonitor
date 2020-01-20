using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Users.Controllers
{
    [Route("")]
    public class UsersController : ApiControllerBase
    {
        private IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{companyId}")]
        [ODataQueryOptionsValidate]
        public async Task<IActionResult> ReadAll([FromRoute]Guid companyId, ODataQueryOptions<User> queryOptions)
        {
            return await HandleQueryable<User, UserResumeViewModel>(await _mediator.Send(new UsersCollection.Query(companyId)), queryOptions);
        }


        [HttpGet("info")]
        public async Task<IActionResult> ReadInformations()
        {
            return HandleQuery<User, UserResumeViewModel>(await _mediator.Send(new UserResume.Query(UserId)));
        }
    }
}
