using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Companies
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string FantasyName { get; set; }
        public string Cnpj { get; set; }
        public string StateRegistration { get; set; }
        public string MunicipalRegistration { get; set; }
        public string Cnae { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public List<Agent> Agents { get; set; }
        public List<User> Users { get; set; }

        public override void Validate() { }
    }
}
