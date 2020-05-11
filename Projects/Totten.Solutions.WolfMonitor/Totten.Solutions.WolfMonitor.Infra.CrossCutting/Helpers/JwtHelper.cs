using Microsoft.IdentityModel.Tokens;
using System.Text;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Configurations;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers
{
    public static class JwtHelper
    {
        public static SigningCredentials GetSigningCredentials()
        {
            string keyString = Cfg.JWT_SIGNING_KEY;
            byte[] symmetricKeyBytes = Encoding.ASCII.GetBytes(keyString);
            var symmetricKey = new SymmetricSecurityKey(symmetricKeyBytes);
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            return signingCredentials;
        }
    }
}
