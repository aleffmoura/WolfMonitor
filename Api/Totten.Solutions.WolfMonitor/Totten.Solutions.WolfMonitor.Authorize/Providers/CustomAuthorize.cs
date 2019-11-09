using Aloji.AspNetCore.JwtSecurity.Context;
using Aloji.AspNetCore.JwtSecurity.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Authorizations;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;

namespace Totten.Solutions.WolfMonitor.Authorize.Providers
{
    public class CustomAuthorize : AuthorizationServerProvider
    {
        private IMediator _mediator;

        public CustomAuthorize(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task GrantClientCredentialsAsync(GrantResourceOwnerCredentialsContext context)
        {
            IFormCollection form = context.Request.Form;

            StringValues clientId = form.First(c => c.Key.Equals("client_id")).Value;

            if (string.IsNullOrEmpty(clientId))
            {
                context.SetError("ErrorCode:001 - The client_id is not set");
                return;
            }

            AppClient client = AppClientsStore.FindClient(clientId);

            if (client == null)
            {
                context.SetError("ErrorCode:002 - The client_id is incorrect");
                return;
            }

            string[] dealerUser = context.UserName.Split('@');
            string login = dealerUser[0];
            string company = dealerUser[1];

            Infra.CrossCutting.Structs.Result<Exception, Domain.Features.Users.User> authVerifyCallback = await _mediator.Send(new AuthorizeLogin.Query() { UserName = login, Password = context.Password, Company = company });

            if (authVerifyCallback.IsFailure)
            {
                context.SetError("ErrorCode:003 - Invalid user authentication");
                return;
            }

            User user = authVerifyCallback.Success;

            List<Claim> claims = new List<Claim>
            {
                 new Claim("Company", user.Company.Name),
                 new Claim("CompanyId", user.CompanyId.ToString()),
                 new Claim("UserId", user.Id.ToString()),
                 new Claim("Login", user.Login),
                 new Claim("UserEmail", user.Email),
                 new Claim("aud", clientId),
            };

            context.Validated(claims);
            await Task.FromResult(0);
        }
    }
}
