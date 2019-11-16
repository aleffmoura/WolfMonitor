using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation
{
    public enum RoleLevelEnum : int
    {
        Agent = 0,
        User = 1,
        Admin = 2,
        System = 3
    }
    public class Role : Entity
    {
        public string Name { get; set; }
        public RoleLevelEnum Level { get; set; }

        public List<User> Users { get; set; }
        public override void Validate() { }
    }
}
