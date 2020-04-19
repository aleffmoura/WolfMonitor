﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserCreateCommand command)
            => HandleCommand(await _mediator.Send(new UserCreate.Command(
                                                        CompanyId, command.Email, command.Cpf,
                                                        command.FirstName, command.LastName,
                                                        command.Language, command.Login, command.Password
                                                  )));

        #endregion
    }
}
