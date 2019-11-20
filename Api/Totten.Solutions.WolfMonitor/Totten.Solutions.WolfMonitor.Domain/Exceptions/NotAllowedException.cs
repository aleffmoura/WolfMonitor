using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class NotAllowedException : BusinessException
    {
        public NotAllowedException(string msg = "Não permitido") : base(ErrorCodes.NotAllowed, msg)
        {
        }
    }
}
