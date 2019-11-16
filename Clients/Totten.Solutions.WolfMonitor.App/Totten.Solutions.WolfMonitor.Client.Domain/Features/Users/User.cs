using System;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Features.Users
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }


        public string GetClientCredentials()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"postman:postmanSecret"));
        }
    }
}
