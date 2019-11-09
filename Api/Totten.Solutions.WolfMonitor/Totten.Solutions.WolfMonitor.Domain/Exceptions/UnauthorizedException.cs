using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Exceptions
{
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException() : base(ErrorCodes.Unauthorized, "Não autenticado")
        {
        }
    }
}
