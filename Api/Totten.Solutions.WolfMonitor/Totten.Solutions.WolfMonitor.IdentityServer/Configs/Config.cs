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
                    ClientName = c.Id,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    PostLogoutRedirectUris = { "https://www.getpostman.com" },
                    AllowedCorsOrigins = { "https://www.getpostman.com" },
                    ClientSecrets =
                    {
                        new Secret(c.Secret.Sha256())
                    },
                    AllowedScopes = { " postman_api " },
                    RequireConsent = false
                });
            });

            return clients;
        }
    }
}
