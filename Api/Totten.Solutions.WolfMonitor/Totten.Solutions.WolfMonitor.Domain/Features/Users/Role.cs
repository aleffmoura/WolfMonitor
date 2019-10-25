using System;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Users
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
