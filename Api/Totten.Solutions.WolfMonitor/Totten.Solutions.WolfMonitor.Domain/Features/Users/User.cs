using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Users
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Password { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
