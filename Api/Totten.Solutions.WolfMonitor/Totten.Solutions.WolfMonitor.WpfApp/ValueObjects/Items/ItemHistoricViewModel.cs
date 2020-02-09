using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items
{
    public class ItemHistoricViewModel
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Value { get; set; }
    }
}
