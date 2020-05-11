using System.Collections.Generic;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Requests
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public string NextPageLink { get; set; }
        public string Count { get; set; }
    }
}
