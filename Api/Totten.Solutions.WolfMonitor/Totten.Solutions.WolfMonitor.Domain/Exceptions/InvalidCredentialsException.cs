using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException() : base(ErrorCodes.Unauthorized, "The login or password is incorrect")
        {
        }
        public InvalidCredentialsException(string msg) : base(ErrorCodes.Unauthorized, msg)
        {
        }
    }
}
