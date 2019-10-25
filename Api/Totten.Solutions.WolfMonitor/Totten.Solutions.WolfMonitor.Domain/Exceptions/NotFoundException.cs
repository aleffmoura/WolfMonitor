using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message = "Registry not found") : base(ErrorCodes.NotFound, message) { }
    }
}
