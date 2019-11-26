using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Logs
{
    public class MonitoringLog : Entity
    {
        public Guid AgentId { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public bool IsSuccess { get; set; }
        public string JsonResult { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
