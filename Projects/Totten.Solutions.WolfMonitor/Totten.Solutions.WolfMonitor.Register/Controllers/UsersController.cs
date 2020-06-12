using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Register.Commands;

namespace Totten.Solutions.WolfMonitor.Register.Controllers
{
    [Route("users")]
    public class UsersController : ApiControllerBase
    {
        private IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost("{companyId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromRoute]Guid companyId, [FromBody]UserCreateCommand command)
            => HandleCommand(await _mediator.Send(new UserCreate.Command(
                                                        userId: UserId,
                                                        userCompany: CompanyId,
                                                        companyId,
                                                        command.Email, command.Cpf,
                                                        command.FirstName, command.LastName,
                                                        command.Language, command.Login, command.Password,
                                                        Role
                                                  )));

        #endregion
    }
}
