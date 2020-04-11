using System;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Services
{
    public interface IEmailService
    {
        Result<Exception, string> Send(string email);
    }
    public class EmailService : IEmailService
    {

        public Result<Exception, string> Send(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "Email incorreto");
            }

            return "";
        }
    }
}
