using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class AppInfoMobileRepository : GenericRepository<AppInfoMobile, int>, IAppInfoMobileRepository
    {
        public AppInfoMobileRepository(DatabaseContext context) : base(context) { }
    }
}
