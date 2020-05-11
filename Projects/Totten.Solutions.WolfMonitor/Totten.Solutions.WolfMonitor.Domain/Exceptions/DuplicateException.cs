using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class DuplicateException : BusinessException
    {
        public DuplicateException(string msg = "O item ja existe no banco de dados") : base(ErrorCodes.AlreadyExists, msg)
        {
        }
    }
}
