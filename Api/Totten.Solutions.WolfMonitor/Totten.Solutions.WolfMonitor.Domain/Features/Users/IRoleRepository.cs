using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Users
{
    public interface IRoleRepository
    {
        Task<Result<Exception, Role>> GetRoleAsync(RoleLevelEnum roleEnum);
    }
}
