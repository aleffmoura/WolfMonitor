using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
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

            var newPatient = _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return newPatient.Entity;
        }

        public async Task<Result<Exception, User>> GetByCredentials(string login, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => (u.Login.Equals(login) || u.Email.Equals(login) || u.Cpf.Equals(login))
                                                                        && u.Password == password);
            if (user == null)
            {
                return new InvalidCredentialsException();
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
