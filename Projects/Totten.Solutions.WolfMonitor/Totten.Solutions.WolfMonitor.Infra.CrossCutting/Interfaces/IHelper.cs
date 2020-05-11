using Microsoft.IdentityModel.Tokens;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces
{
    public interface IHelper
    {
        int GenerateRandomPort();
        string GetLocalIpAddress();
        string GetServiceName();
        string GetAssemblyName();
        string GetAssemblyVersion();
        SigningCredentials GetSigningCredentials();
        string GetConfiguration(string key);
        string GetMACAddress();
        string GetHostName();
    }
}
