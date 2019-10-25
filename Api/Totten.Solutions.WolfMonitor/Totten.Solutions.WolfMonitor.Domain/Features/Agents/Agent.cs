﻿using System;
using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public class Agent : Entity
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public DateTime CreatedIn { get; set; }
        public DateTime FirstConnection { get; set; }
        public DateTime LastConnection { get; set; }
        public DateTime LastUpload { get; set; }
        public bool Configured { get; set; }

        public bool Removed { get; set; }

        public List<Item> Items { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
