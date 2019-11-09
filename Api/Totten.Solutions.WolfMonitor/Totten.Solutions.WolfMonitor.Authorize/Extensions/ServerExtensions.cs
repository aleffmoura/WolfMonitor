using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Totten.Solutions.WolfMonitor.Authorize.Providers;

namespace Totten.Solutions.WolfMonitor.Authorize.Extensions
{
    public static class ServerExtensions
    {
        public static void UseJWTServer(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            app.UseJwtServer(
                options =>
                {
                    options.TokenEndpointPath = configuration["Endpoint"];
                    options.AccessTokenExpireTimeSpan = new TimeSpan(Convert.ToInt32(configuration["Expiration"]), 0, 0);
                    options.Issuer = configuration["Issuer"];
                    options.IssuerSigningKey = configuration["Secret"];
                    options.AuthorizationServerProvider = new CustomAuthorize(app.ApplicationServices.CreateScope().ServiceProvider.GetService<IMediator>());
                }
            );
        }
    }
}
