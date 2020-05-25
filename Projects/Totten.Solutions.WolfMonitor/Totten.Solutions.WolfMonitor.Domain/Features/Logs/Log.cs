using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Logs
{
    public class Log : Entity
    {
        public Guid UserId { get; set; }
        public Guid UserCompanyId { get; set; }
        public Guid TargetId { get; set; }
        public ETypeEntity EntityType { get; set; }
        public ETypeLogMethod TypeLogMethod { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }


        public override void Validate()
        {

        }
    }
}
