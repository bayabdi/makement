using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class CompanyOptionsRepository : GenericRepository<CompanyOptions, int>, ICompanyOptionsRepository
    {
        public CompanyOptionsRepository(DatabaseContext context) : base(context) { }
    }
}
