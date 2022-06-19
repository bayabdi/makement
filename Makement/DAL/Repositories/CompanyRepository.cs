using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CompanyRepository : GenericRepository<Company, int>, ICompanyRepository
    {
        public CompanyRepository(DatabaseContext context) : base(context) { }

        public async Task<IEnumerable<Company>> GetAllWithUsers()
        {
            return await context.Companies.Include(x => x.Users).ToListAsync();
        }
    }
}
