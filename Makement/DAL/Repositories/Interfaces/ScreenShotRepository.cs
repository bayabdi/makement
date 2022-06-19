using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ScreenShotRepository : GenericRepository<ScreenShot, string>, IScreenShotRepository
    {
        public ScreenShotRepository(DatabaseContext context) : base(context) { }
    }
}
