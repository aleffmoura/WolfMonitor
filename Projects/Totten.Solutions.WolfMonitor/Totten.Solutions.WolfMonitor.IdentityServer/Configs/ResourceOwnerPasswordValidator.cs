using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.IdentityServer.Configs
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _repository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAgentRepository _agentRepository;

        public ResourceOwnerPasswordValidator(IUserRepository repository,
                                              ICompanyRepository companyRepository,
                                              IAgentRepository agentRepository)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _agentRepository = agentRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            string[] data = context.UserName.Split('@');
            string login = data[0];

            string[] companyData = data[1].Split('#');
            string company = companyData[0];
            string userType = companyData[1];

            var companCallback = await _companyRepository.GetByFantasyNameAsync(company);

            if (companCallback.IsFailure)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The company name are incorrect", null);
            }
            else
            {

                if (userType.Equals("Agent", StringComparison.InvariantCultureIgnoreCase))
                {
                    var agentCallback = await _agentRepository.Authentication(companCallback.Success.Id, login, context.Password);
                    
                    if (agentCallback.IsSuccess)
                    {
                        List<Claim> claims = MakeClaims(RoleLevelEnum.Agent,
                                                        login,
                                                        companyId: companCallback.Success.Id,
                                                        userId: agentCallback.Success.Id);

                        context.Result = new GrantValidationResult(agentCallback.Success.Id.ToString(), "password", claims, "local", null);
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The agent login or password are incorrect", null);
                    }
                }
                else
                {
                    var userCallback = await _repository.GetByCredentials(companCallback.Success.Id, login, context.Password);
                    
                    if (userCallback.IsSuccess)
                    {

                        List<Claim> claims = MakeClaims(userCallback.Success.Role.Level,
                                                        login,
                                                        companyId: companCallback.Success.Id,
                                                        userId: userCallback.Success.Id);

                        context.Result = new GrantValidationResult(userCallback.Success.Id.ToString(), "password", claims, "local", null);
                        userCallback.Success.LastLogin = DateTime.Now;

                        await _repository.UpdateAsync(userCallback.Success);
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The user or password are incorrect", null);
                    }
                }
            }
            await Task.FromResult(context.Result);
        }

        private List<Claim> MakeClaims(RoleLevelEnum role, string login, Guid companyId, Guid userId)
        {
            return new List<Claim>
                {
                    new Claim("Role", role.ToString()),
                    new Claim("Login", login),
                    new Claim("CompanyId", companyId.ToString()),
                    new Claim("UserId", userId.ToString())
                };
        }
    }
}