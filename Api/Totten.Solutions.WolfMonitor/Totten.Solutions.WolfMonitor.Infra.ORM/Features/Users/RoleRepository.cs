using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Users
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthContext _context;

        public RoleRepository(AuthContext context)
        {
            _context = context;
        }
        public async Task<Result<Exception, Role>> GetRoleAsync(RoleLevelEnum roleLevel)
        {
            Role role = await _context.Roles.FirstOrDefaultAsync(a => a.Level == roleLevel && !a.Removed);

            if (role == null)
            {
                return new NotFoundException("Role not found");
            }
            return role;
        }
    }
}
