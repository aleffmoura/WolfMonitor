using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Totten.Solutions.WolfMonitor.IdentityServer.Configs
{
    public class Config
    {
        private readonly IConfigurationRoot _configuration;

        public Config(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Gateway", "Gateway Service Wolf Monitor"),
                new ApiResource("Agents", "Agents Service"),
                new ApiResource("Companies", "Gateway Service Wolf Monitor"),
                new ApiResource("Itens", "Gateway Service Wolf Monitor"),
                new ApiResource("Register", "Register Service"),
                new ApiResource("users", "Users Service"),
                new ApiResource("postman_api", "Postman Test Resource")
            };
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
            };
        }
        public IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>();

            Models.ServerClient[] cli = _configuration.GetSection("clients").Get<Models.ServerClient[]>();

            cli.ToList().ForEach(c =>
            {
                clients.Add(new Client
                {
                    ClientId = c.Id,
                    ClientName = c.Name,
                    ClientSecrets =
                    {
                        new Secret(c.Secret.Sha256())
                    },
                    AllowedScopes = c.Scopes,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    Enabled = true,
                    LogoUri = null
                });
            });

            return clients;
        }
    }
}
