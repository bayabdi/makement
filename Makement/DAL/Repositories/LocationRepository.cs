using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.DatabseContext;

namespace DAL.Repositories
{
    public class LocationRepository : GenericRepository<Location, int>, ILocationRepository
    {
        public LocationRepository(DatabaseContext context) : base(context) { }
    }
}
