using System;
using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices
{
    public class ItemDetailViewModel
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string AboutCurrentValue { get; set; }
        public string UpdatedIn { get; set; }

        public List<ItemHistoric> Historic { get; set; }
    }
}
