using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Base
{
    public class ApiResult<T>
    {
        public List<T> Items { get; set; }
    }
}
