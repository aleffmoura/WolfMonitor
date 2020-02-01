using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Requests
{
    public class UserLogin
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public static string Token { get; set; }

        public string GetClientCredentials()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"postman:postmanSecret"));
        }
    }
}
