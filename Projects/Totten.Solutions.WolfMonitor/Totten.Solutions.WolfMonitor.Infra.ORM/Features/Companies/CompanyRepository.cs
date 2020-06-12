using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Features.Companies
{
    public class CompanyRepository : ICompanyRepository
    {
        private WolfMonitorContext _context;

        public CompanyRepository(WolfMonitorContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, Company>> CreateAsync(Company company)
        {
            EntityEntry<Company> newCompany = _context.Companies.Add(company);

            await _context.SaveChangesAsync();

            return newCompany.Entity;
        }

        public Result<Exception, IQueryable<Company>> GetAll()
            => Result.Run(() => _context.Companies.Include(c => c.Agents).AsNoTracking().Where(a => !a.Removed));

        public async Task<Result<Exception, Company>> GetByIdAsync(Guid id)
        {
            Company company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(c => !c.Removed && c.Id == id);

            if (company == null)
                return new NotFoundException("Empresa não encontrada");

            return company;
        }

        public async Task<Result<Exception, Company>> GetByFantasyNameAsync(string fantasyName)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(c => !c.Removed && c.FantasyName.Equals(fantasyName, StringComparison.InvariantCultureIgnoreCase));

            if (company == null)
                return new NotFoundException("Empresa não encontrada");

            return company;
        }

        public async Task<Result<Exception, Company>> GetByNameOrCnpjAsync(string name, string cnpj)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(c => !c.Removed && (c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                                c.Cnpj.Equals(cnpj, StringComparison.InvariantCultureIgnoreCase)));

            if (company == null)
                return new NotFoundException("Empresa não encontrada");

            return company;
        }

        public async Task<Result<Exception, Unit>> UpdateAsync(Company entity)
        {
            entity.UpdatedIn = DateTime.Now;
            _context.Companies.Update(entity);
            await _context.SaveChangesAsync();

            return Unit.Successful;
        }
    }
}
