using System;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.EMails;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Services
{
    public interface IEmailService
    {
        Result<Exception, Guid> Send(string email, Guid token = default);
    }
    public class EmailService : IEmailService
    {

        public Result<Exception, Guid> Send(string email, Guid token = default)
        {
            if (string.IsNullOrEmpty(email))
                return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "Email incorreto");

            try
            {
                if (token == default)
                    token = Guid.NewGuid();
                var body = "Recuperação de senha:<br/> Nome: Totem Solutions<br/> Token : " + token + " <br/>Mensagem automática, não responda-a";

                EMail.Send("Recuperação de senha", body, email, "Totem Solutions", "tottenprogramming@gmail.com");

                return token;
            }
            catch
            {
                return new BusinessException(ErrorCodes.ServiceUnavailable, "Serviço indisponivel, contact o administrador");
            }
        }
    }
}
