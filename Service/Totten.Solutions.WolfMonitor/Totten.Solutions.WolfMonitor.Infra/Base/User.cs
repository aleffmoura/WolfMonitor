using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.Base
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public string GetCredentialsFormatedToOAuth()
        {
            var name = Name;
            var pass = Password;
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{name}:{pass}"));
        }
    }
}
