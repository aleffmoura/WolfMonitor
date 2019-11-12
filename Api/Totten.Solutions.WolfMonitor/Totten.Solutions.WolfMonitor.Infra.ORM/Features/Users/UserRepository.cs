using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;


namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;

        public UserRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, User>> CreateAsync(User user)
        {
            user.Validate();

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> newPatient = _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return newPatient.Entity;
        }

        public Result<Exception, IQueryable<User>> GetAll(Guid companyId)
        {
            return Result.Run(() => _context.Users.Where(a => a.CompanyId == companyId && !a.Removed));
        }

        public Result<Exception, IQueryable<User>> GetAll()
        {
            return Result.Run(() => _context.Users.Where(a => !a.Removed));
        }

        public async Task<Result<Exception, User>> GetByCredentials(Guid companyId, string login, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.CompanyId == companyId &&
                                                                    (u.Login.Equals(login) || u.Email.Equals(login) || u.Cpf.Equals(login)) &&
                                                                    u.Password == password && !u.Removed);
            if (user == null)
            {
                return new InvalidCredentialsException();
            }
            return user;
        }

        public async Task<Result<Exception, User>> GetByIdAsync(Guid id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (user == null)
            {
                return new NotFoundException();
            }

            return user;
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return Unit.Successful;
        }
    }
}
