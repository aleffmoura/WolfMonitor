using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Exceptions
{
    public enum ErrorCodes
    {
        Unauthorized = 401,
        Forbidden = 0403,
        NotFound = 0404,
        AlreadyExists = 0409,
        NotAllowed = 0405,
        InvalidObject = 0422,
        Unhandled = 0500,
        ServiceUnavailable = 0503,
    }
    public class BusinessException : Exception
    {
        public BusinessException(ErrorCodes errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCodes ErrorCode { get; }
    }
}