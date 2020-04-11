using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
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

        public Result<Exception, IQueryable<User>> GetAllByCompanyId(Guid companyId)
        {
            return Result.Run(() => _context.Users.Where(u => u.CompanyId == companyId));
        }
        public async Task<Result<Exception, User>> GetByLoginAndEmail(string login, string email)
        {
            User userCallBack = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(user => user.Email == email && !user.Removed &&
                                                                          (user.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) || user.Email.Equals(login, StringComparison.InvariantCultureIgnoreCase) || user.Cpf.Equals(login)));
            if (userCallBack == null)
                return new InvalidCredentialsException();

            return userCallBack;
        }

        public async Task<Result<Exception, User>> GetByCredentials(Guid companyId, string login, string password)
        {
            User userCallBack = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(user => user.CompanyId == companyId &&
                                                                          (user.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) || user.Email.Equals(login, StringComparison.InvariantCultureIgnoreCase) || user.Cpf.Equals(login)) &&
                                                                          user.Password == password.GenerateHash() &&
                                                                          !user.Removed);
            if (userCallBack == null)
            {
                return new InvalidCredentialsException();
            }
            return userCallBack;
        }

        public async Task<Result<Exception, User>> GetByIdAsync(Guid id)
        {
            User user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

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
