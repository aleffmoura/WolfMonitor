using System;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels
{
    public enum EUserLevel
    {
        User = 1,
        Admin = 2,
        System = 3
    }
    public class UserBasicInformationViewModel
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public int UserLevel { get; set; } 
        public static string Token { get; set; } = string.Empty;

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        public string GetFormatedPass()
            => "****";
    }
}
