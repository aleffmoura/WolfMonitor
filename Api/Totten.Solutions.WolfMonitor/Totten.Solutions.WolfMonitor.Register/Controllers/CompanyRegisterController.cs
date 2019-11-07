using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;

namespace Totten.Solutions.WolfMonitor.Register.Controllers
{
    [Route("companies")]
    public class CompanyRegisterController : ApiControllerBase
    {
        private IMediator _mediator;
        public CompanyRegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CompanyCreate.Command command)
            => HandleCommand(await _mediator.Send(command));
        #endregion

    }
}
