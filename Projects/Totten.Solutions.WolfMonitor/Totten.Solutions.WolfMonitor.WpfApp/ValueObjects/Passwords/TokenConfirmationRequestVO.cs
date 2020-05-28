using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords
{
    public class TokenConfirmationRequestVO
    {
        public string Company { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public Guid RecoverSolicitationCode { get; set; }
    }
}
