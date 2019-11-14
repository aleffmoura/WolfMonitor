using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.IdentityServer.Configs
{
    public class ProfileService : IProfileService
    {

        public ProfileService()
        {
           
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint)
            {
                var sub = context.Subject.Claims.FirstOrDefault()?.Value;
            }
            else
            {
                context.IssuedClaims = context.Subject.Claims.ToList();
            }

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}
