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
using Totten.Solutions.WolfMonitor.Users.Controllers.Commands;

namespace Totten.Solutions.WolfMonitor.Users.Controllers
{
    [Route("")]
    public class UsersController : ApiControllerBase
    {
        private IMediator _mediator;

        public UsersController(IMediator mediator)
            => _mediator = mediator;
        #region GET
        [HttpGet()]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Count | AllowedQueryOptions.Skip)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromQuery]ODataQueryOptions<User> queryOptions)
            => await HandleQueryable<User, UserResumeViewModel>(await _mediator.Send(new UsersCollection.Query(CompanyId, CompanyId, Role)), queryOptions);

        [HttpGet("info")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadInformations()
            => HandleQuery<User, UserDetailViewModel>(await _mediator.Send(new UserResume.Query(UserId)));
        #endregion

        [HttpPatch("info")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> Update([FromBody]UserUpdateCommand command)
            => HandleCommand(await _mediator.Send(new UserUpdate.Command(UserId, CompanyId,
                                                                        command.Login,
                                                                        command.Password,
                                                                        command.Email,
                                                                        command.FirstName,
                                                                        command.LastName)));

        #region DELETE
        [HttpDelete("{Id}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> RemoveItem([FromRoute]Guid id)
            => HandleCommand(await _mediator.Send(new UserRemove.Command(id, CompanyId, UserId)));
        #endregion
    }
}
