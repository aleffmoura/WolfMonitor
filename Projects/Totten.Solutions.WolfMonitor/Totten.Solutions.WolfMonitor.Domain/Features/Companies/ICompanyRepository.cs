using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Companies
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Result<Exception, Company>> GetByFantasyNameAsync(string name);
        Task<Result<Exception, Company>> GetByNameOrCnpjAsync(string name, string cnpj);
    }
}
