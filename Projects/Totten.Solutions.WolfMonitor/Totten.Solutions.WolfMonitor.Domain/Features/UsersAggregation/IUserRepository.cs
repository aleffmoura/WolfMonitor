using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation
{
    public interface IUserRepository : IRepository<User>
    {
        Task<Result<Exception, User>> GetByCredentials(Guid companyId, string login, string password);
        Task<Result<Exception, User>> GetByLoginAndEmail(Guid companyId, string login, string email);
        Task<Result<Exception, User>> GetByLogin(Guid companyId, string login);
        Task<Result<Exception, User>> GetByEmail(Guid companyId, string email);
        Result<Exception, IQueryable<User>> GetAll(Guid companyId);
        Result<Exception, IQueryable<User>> GetAllByCompanyId(Guid companyId);
    }
}
