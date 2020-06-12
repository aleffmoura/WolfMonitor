using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class ForbiddenException : BusinessException
    {
        public ForbiddenException(string msg = "Não autorizado") : base(ErrorCodes.Forbidden, msg)
        {
        }
    }
}
