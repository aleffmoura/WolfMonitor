using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Base
{
    public abstract class Item : Entity
    {
        public string Name { get; set; }
        public string LastValue { get; set; }
        public string ActualValue { get; set; }
        public EType Type { get; set; }
        public abstract Item Check();
    }
}
