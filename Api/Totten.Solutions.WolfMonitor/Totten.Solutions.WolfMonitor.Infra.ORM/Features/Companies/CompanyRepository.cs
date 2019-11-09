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
        {
            return Result.Run(() => _context.Companies.Where(a => !a.Removed));
        }

        public async Task<Result<Exception, Company>> GetByIdAsync(Guid id)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(c => !c.Removed && c.Id == id);

            if (company == null)
            {
                return new NotFoundException("Company not found");
            }
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
