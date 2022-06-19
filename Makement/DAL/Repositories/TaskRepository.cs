using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TaskRepository : GenericRepository<UserTask, int>, ITaskRepository
    {
        public TaskRepository(DatabaseContext context) : base(context) { }

        public async Task<IEnumerable<UserTask>> GetWithUser()
        {
            return await context.Tasks.Include(x => x.User).ToListAsync();
        }

        public async Task<UserTask> GetWithPeriod(int id)
        {
            return await context.Tasks.Include(x => x.Periods).FirstAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<UserTask>> GetTasksWithPeriod()
        {
            return await context.Tasks.Include(x => x.User).Include(x => x.Periods).Where(x => x.IsDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<UserTask>> GetWithCompanies()
        {
            return await context.Tasks.Include(x => x.User).Include(x => x.Team).ThenInclude(x => x.Company).ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetWithUserTeams()
        {
            return await context.Tasks.Include(x => x.User).Include(x => x.Team).ThenInclude(x => x.UserTeams).ToListAsync();
        }
    }
}
