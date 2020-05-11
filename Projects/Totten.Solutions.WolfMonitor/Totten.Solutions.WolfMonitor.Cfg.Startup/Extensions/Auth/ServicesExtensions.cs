using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Auth
{
    public static class ServicesExtensions
    {
        public static void AddAuth(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(opt =>
                    {
                        opt.Authority = configuration["identityServerAddress"];
                        opt.RequireHttpsMetadata = false;
                        opt.ApiName = configuration["apiName"];
                        opt.ApiSecret = configuration["apiSecret"];
                    });
            services.AddAuthorization();
        }
    }
}
