using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Companies
{
    public interface ICompanyRepository : IRepository<Company>
    {
    }
}
