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
                new ApiResource("Agents", "Agents Service", new List<string> { "Role","UserId", "CompanyId"}){
                    ApiSecrets =
                    {
                        new Secret(_configuration["agentsApiSecret"].Sha256())
                    }
                },
                new ApiResource("Monitoring", "Monitoring Service", new List<string> { "Role","UserId", "CompanyId"}){
                    ApiSecrets =
                    {
                        new Secret(_configuration["monitoringApiSecret"].Sha256())
                    }
                },
                new ApiResource("Users", "Users Service", new List<string> { "Role","UserId", "CompanyId"}){
                    ApiSecrets =
                    {
                        new Secret(_configuration["usersApiSecret"].Sha256())
                    }
                },
                new ApiResource("Register", "Register Service", new List<string> { "Role","UserId", "CompanyId"}){
                    ApiSecrets =
                    {
                        new Secret(_configuration["registerApiSecret"].Sha256())
                    }
                },
                new ApiResource("Companies", "Companies Service", new List<string> { "Role","UserId", "CompanyId"}){
                    ApiSecrets =
                    {
                        new Secret(_configuration["companiesApiSecret"].Sha256())
                    }
                },
            };
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                    new IdentityResources.OpenId()
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
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
                });
            });

            return clients;
        }
    }
}
