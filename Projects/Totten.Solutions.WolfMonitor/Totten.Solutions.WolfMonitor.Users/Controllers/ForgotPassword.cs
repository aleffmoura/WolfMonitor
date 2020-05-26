using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;

namespace Totten.Solutions.WolfMonitor.Users.Controllers
{
    [Route("ForgotPassword")]
    public class ForgotPassword : ApiControllerBase
    {
        private IMediator _mediator;

        public ForgotPassword(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserForgotPasswordCreate.Command command)
            => HandleCommand(await _mediator.Send(command));

        [HttpPost("token-resend")]
        public async Task<IActionResult> Send([FromBody]UserTokenReSend.Command command)
            => HandleCommand(await _mediator.Send(command));

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody]UserValidateTokenCreate.Command command)
            => HandleCommand(await _mediator.Send(command));

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordByToken([FromBody]UserChangePasswordByTokenCreate.Command command)
            => HandleCommand(await _mediator.Send(command));
    }
}
