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
        public static Item GetInstance(this ETypeItem eTypeItem, Item item)
        {
            switch (eTypeItem)
            {
                case ETypeItem.SystemService:
                    return new SystemService(item);
                case ETypeItem.SystemConfig:
                    return new SystemConfig(item);

                default:
                    throw new BusinessException(ErrorCodes.InvalidObject, "Não existe um tipo cadastrado que corresponda ao informado.");
            }
        }
    }
}
