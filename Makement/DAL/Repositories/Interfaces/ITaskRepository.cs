using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ITaskRepository : IGenericRepository<UserTask, int>
    {
        Task<UserTask> GetWithPeriod(int id);
        Task<IEnumerable<UserTask>> GetWithUser();
        Task<IEnumerable<UserTask>> GetWithCompanies();
        Task<IEnumerable<UserTask>> GetWithUserTeams();
        Task<IEnumerable<UserTask>> GetTasksWithPeriod();
    }
}
