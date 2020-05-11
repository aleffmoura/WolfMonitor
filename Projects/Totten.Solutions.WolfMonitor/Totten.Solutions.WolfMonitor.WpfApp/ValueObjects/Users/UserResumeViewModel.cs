using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Users
{
    public class UserResumeViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string RoleName { get; set; }
        public string LastLogin { get; set; }
    }
}
