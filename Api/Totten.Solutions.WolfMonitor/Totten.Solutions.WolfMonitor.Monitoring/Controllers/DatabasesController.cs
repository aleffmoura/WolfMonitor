using MediatR;
using Microsoft.AspNetCore.Mvc;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    [Route("Databases")]
    public class DatabasesController : ApiControllerBase
    {
        private IMediator _mediator;

        public DatabasesController(IMediator mediator)
        {
            _mediator = mediator;
        }

    }
}
