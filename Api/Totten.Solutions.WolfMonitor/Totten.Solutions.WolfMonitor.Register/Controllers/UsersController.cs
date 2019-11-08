using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Users.Handlers;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;

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
        public async Task<IActionResult> Create([FromBody]UserCreate.Command command)
        {
            return HandleCommand(await _mediator.Send(command));
        }
        #endregion
    }
}
