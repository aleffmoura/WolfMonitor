using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Companies
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string FantasyName { get; set; }
        public string Cnpj { get; set; }


        public List<Agent> Agents { get; set; }
        //public List<User> Users { get; set; }
        public override void Validate() { }
    }
}
