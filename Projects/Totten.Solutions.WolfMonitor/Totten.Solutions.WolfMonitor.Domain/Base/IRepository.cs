using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Base
{
    public interface IRepository<E> where E : Entity, new()
    {
        Task<Result<Exception, E>> CreateAsync(E entity);
        Task<Result<Exception, Unit>> UpdateAsync(E entity);
        Task<Result<Exception, E>> GetByIdAsync(Guid id);
        Result<Exception, IQueryable<E>> GetAll();
    }
}
