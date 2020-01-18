using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base
{
    public class UserLogin
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
