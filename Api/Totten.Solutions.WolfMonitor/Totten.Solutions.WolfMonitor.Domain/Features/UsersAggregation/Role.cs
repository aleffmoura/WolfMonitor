using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation
{
    public enum RoleLevelEnum
    {
        Agent,
        User,
        Admin,
        System
    }
    public class Role : Entity
    {
        public string Name { get; set; }
        public RoleLevelEnum Level { get; set; }

        public List<User> Users { get; set; }
        public override void Validate() { }
    }
}
