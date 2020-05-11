using System;
using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;

namespace Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation
{
    public class User : Entity
    {
        public Guid CompanyId { get; set; }
        public Guid RoleId { get; set; }

        public string Login { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }

        public DateTime? LastLogin { get; set; }

        public string Token { get; set; }
        public string TokenSolicitationCode { get; set; }
        public string RecoverSolicitationCode { get; set; }

        public virtual Role Role { get; set; }
        public virtual Company Company { get; set; }
        public List<Agent> Agents { get; set; }

        public override void Validate() { }
    }
}
