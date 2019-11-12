using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.ItemsAggregation.Handlers.Services;
using Totten.Solutions.WolfMonitor.Application.Features.ItemsAggregation.ViewModels;
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

        #region HTTP GET
        [HttpGet]
        [Route("{agentId}")]
        public async Task<IActionResult> ReadById([FromRoute]string agentId)
            => HandleQuery<Item, ItemResumeViewModel>(await _mediator.Send(new ServicesResume.Query(agentId)));
        #endregion
        #region HTTP POST
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody]ServicesCreate.Command command)
        //{
        //    command.UserWhoCreated = UserId;
        //    return HandleCommand(await _mediator.Send(command));
        //}
        #endregion
    }
}
