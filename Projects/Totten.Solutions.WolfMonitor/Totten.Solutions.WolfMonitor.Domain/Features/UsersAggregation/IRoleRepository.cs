using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation
{
    public interface IRoleRepository
    {
        Task<Result<Exception, Role>> GetRoleAsync(RoleLevelEnum roleEnum);
    }
}
