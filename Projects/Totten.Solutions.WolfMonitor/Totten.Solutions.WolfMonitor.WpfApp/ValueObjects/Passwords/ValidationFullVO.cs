using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords
{
    public class ValidationFullVO
    {
        public string Company { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid RecoverSolicitationCode { get; set; }
        public Guid TokenSolicitationCode { get; set; }
    }
}
