using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords
{
    public class TokenConfirmationRequestVO
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RecoverSolicitationCode { get; set; }
    }
}
