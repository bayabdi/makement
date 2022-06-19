using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
namespace DAL.Repositories
{
    public class TaskPeriodsRepository : GenericRepository<UserTaskPeriod, int>, ITaskPeriodsRepository
    {
        public TaskPeriodsRepository(DatabaseContext context) : base(context) { }

    }
}
