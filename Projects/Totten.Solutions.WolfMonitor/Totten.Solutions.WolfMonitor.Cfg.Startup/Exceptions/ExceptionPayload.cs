using System;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Exceptions
{
    public class ExceptionPayload
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public static ExceptionPayload New<T>(T exception) where T : Exception
        {
            int errorCode;

            if (exception is BusinessException businessException)
                errorCode = businessException.ErrorCode.GetHashCode();
            else
                errorCode = ErrorCodes.Unhandled.GetHashCode();

            return new ExceptionPayload
            {
                ErrorCode = errorCode,
                ErrorMessage = exception.Message,
            };
        }
    }
}
