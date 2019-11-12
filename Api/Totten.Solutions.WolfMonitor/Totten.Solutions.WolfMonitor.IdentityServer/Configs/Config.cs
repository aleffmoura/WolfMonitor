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
            this._configuration = configuration;
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Totten.Solutions.PCDoctor.Services", "Service API Totten PCDoctor"),
                new ApiResource("postman_api", "Postman Test Resource")
            };
        }

        public IEnumerable<Client> GetClients()
        {
            var clients = new List<Client>();

            var cli = _configuration.GetSection("clients").Get<Models.ServerClient[]>();

            cli.ToList().ForEach(c =>
            {
                clients.Add(new Client
                {
                    ClientId = c.Id,
                    ClientName = "Postman Test Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    PostLogoutRedirectUris = { "https://www.getpostman.com" },
                    AllowedCorsOrigins = { "https://www.getpostman.com" },
                    EnableLocalLogin = true,
                    Enabled = true,
                    LogoUri = null,
                    AllowedScopes = { " postman_api ",
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.OfflineAccess,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.Email },
                    ClientSecrets =
                    {
                        new Secret(c.Secret)
                    },
                });
            });

            return clients;
        }
    }
}
