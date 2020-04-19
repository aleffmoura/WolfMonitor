using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords;
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
            => _mediator = mediator;

        [HttpGet()]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Count | AllowedQueryOptions.Skip)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromQuery]ODataQueryOptions<User> queryOptions)
            => await HandleQueryable<User, UserResumeViewModel>(await _mediator.Send(new UsersCollection.Query(CompanyId)), queryOptions);

        [HttpGet("info")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadInformations()
            => HandleQuery<User, UserResumeViewModel>(await _mediator.Send(new UserResume.Query(UserId)));

        [HttpPost("forgot-password")]
        public async Task<IActionResult> Create([FromBody]UserForgotPasswordCreate.Command command)
            => HandleCommand(await _mediator.Send(command));

        [HttpPost("forgot-password/validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody]UserValidateTokenCreate.Command command)
            => HandleCommand(await _mediator.Send(command));

        [HttpPost("forgot-password/change-password")]
        public async Task<IActionResult> ChangePasswordByToken([FromBody]UserChangePasswordByTokenCreate.Command command)
            => HandleCommand(await _mediator.Send(command));

    }
}
