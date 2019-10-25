using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Users
{
    public interface IUserRepository
    {
        Task<Result<Exception, User>> CreateAsync(User user);
        Task<Result<Exception, Unit>> UpdateAsync(User user);
        Task<Result<Exception, User>> GetByCredentials(string login, string password);
    }
}
