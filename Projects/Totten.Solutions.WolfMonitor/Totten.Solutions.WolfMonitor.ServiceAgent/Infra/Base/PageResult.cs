using System.Collections.Generic;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public string NextPageLink { get; set; }
        public string Count { get; set; }

        public PageResult()
        {
            Items = new List<T>();
        }
    }
}
