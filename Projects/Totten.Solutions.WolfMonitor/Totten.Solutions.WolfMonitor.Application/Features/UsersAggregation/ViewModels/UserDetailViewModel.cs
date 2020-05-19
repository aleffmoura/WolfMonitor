using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.ViewModels
{
    public class UserDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserLevel { get; set; }
        public string RoleName { get; set; }
        public string LastLogin { get; set; }
    }
}
