using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string msg = "Não autenticado") : base(ErrorCodes.Unauthorized, msg)
        {
        }
    }
}
