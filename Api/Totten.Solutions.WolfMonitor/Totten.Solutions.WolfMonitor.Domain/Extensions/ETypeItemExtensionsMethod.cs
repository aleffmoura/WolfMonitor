using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Domain.Extensions
{
    public static class ETypeItemExtensionsMethod
    {
        public static Item GetInstance(this ETypeItem eTypeItem, Item item) =>
            eTypeItem switch
            {
                ETypeItem.SystemService => new SystemService(item),
                ETypeItem.SystemConfig => new SystemConfig(item),
                _ => throw new BusinessException(ErrorCodes.InvalidObject, "Não existe um tipo cadastrado que corresponda ao informado.")
            };
    }
}
