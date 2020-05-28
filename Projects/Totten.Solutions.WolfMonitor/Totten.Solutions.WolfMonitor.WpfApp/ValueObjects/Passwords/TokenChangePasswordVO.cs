using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords
{
    public class TokenChangePasswordVO
    {
        public string Company { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid TokenSolicitationCode { get; set; }
        public Guid RecoverSolicitationCode { get; set; }
    }
}
