using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Items.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Items.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Items.Controllers
{
    [Route("services")]
    public class ServicesController : ApiControllerBase
    {
        private IMediator _mediator;
        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{agentId}")]
        public async Task<IActionResult> ReadById([FromRoute]string agentId)
            => HandleQuery<Item, ItemResumeViewModel>(await _mediator.Send(new ItemResume.Query(agentId)));

    }
}
