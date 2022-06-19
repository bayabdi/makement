using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ApplicationVersionRepository : GenericRepository<ApplicationVersion, int>, IApplicationVersionRepository
    {
        public ApplicationVersionRepository(DatabaseContext context) : base(context) { }
    }
}
