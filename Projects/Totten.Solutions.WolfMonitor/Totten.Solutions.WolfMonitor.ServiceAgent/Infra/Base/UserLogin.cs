using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base
{
    public class UserLogin
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public static string Token { get; set; }

        public static string GetClientCredentials()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"agentService:agentServiceSecret"));
        }
    }
}
