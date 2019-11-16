using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.IdentityServer.Configs
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _repository;
        private readonly ICompanyRepository _companyRepository;

        public ResourceOwnerPasswordValidator(IUserRepository repository, ICompanyRepository companyRepository)
        {
            _repository = repository;
            _companyRepository = companyRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            string[] splited = context.UserName.Split('@');
            string login = splited[0];
            string company = splited[1];


            Result<Exception, Company> companCallback = await _companyRepository.GetByNameAsync(company);

            if (companCallback.IsFailure)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The company name are incorrect", null);
            }
            else
            {
                Result<Exception, User> userCallback = await _repository.GetByCredentials(companCallback.Success.Id, login, context.Password);
                if (userCallback.IsSuccess)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(JwtClaimTypes.Role, userCallback.Success.Role.Name));
                    claims.Add(new Claim("RoleLevel", userCallback.Success.Role.Level.ToString()));
                    claims.Add(new Claim("Login", userCallback.Success.Login));
                    claims.Add(new Claim("CompanyId", userCallback.Success.CompanyId.ToString()));
                    claims.Add(new Claim("UserId", userCallback.Success.Id.ToString()));
                    context.Result = new GrantValidationResult(userCallback.Success.Id.ToString(), "password", claims, "local", null);
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The user or password are incorrect", null);
                }
            }

            await Task.FromResult(context.Result);
        }
    }
}
