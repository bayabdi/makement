using DAL.Entities;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IUserActivityRepository : IGenericRepository<UserActivity, int>
    {
        Task Save(UserActivity activity);
    }
}
