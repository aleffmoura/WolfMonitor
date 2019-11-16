using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Filters
{
    public class CustomAuthorizeAttributte : ActionFilterAttribute
    {
        private readonly RoleLevelEnum[] Roles;
        public CustomAuthorizeAttributte(params RoleLevelEnum[] roles)
        {
            Roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RoleLevelEnum? roleEnum;
            try
            {
                string roleLevel = GetClaimValue(context.HttpContext, "Role");

                roleEnum = Enum.Parse<RoleLevelEnum>(roleLevel);
            }
            catch
            {
                throw new UnauthorizedException();
            }
            if (!roleEnum.HasValue)
            {
                throw new ForbiddenException();
            }
            var permissions = Roles.ToList().Contains(roleEnum.Value);

            if (!permissions)
                throw new ForbiddenException();

            base.OnActionExecuting(context);
        }

        private string GetClaimValue(HttpContext httpContext, string type)
        {
            return ((ClaimsIdentity)httpContext.User.Identity).FindFirst(type)?.Value;
        }
    }
}
