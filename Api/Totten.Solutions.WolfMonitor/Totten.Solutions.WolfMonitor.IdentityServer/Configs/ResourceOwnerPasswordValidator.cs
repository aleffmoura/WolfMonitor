using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.IdentityServer.Configs
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _repository;

        public ResourceOwnerPasswordValidator(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            Result<Exception, User> userCallback = await _repository.GetByCredentials(Guid.Empty, context.UserName, context.Password);

            if (userCallback.IsSuccess)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Role, userCallback.Success.Role.Name)
                };
                context.Result = new GrantValidationResult(userCallback.Success.Id.ToString(), "password", claims, "local", null);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The user or password are incorrect", null);
            }
            await Task.FromResult(context.Result);
        }
    }
}
