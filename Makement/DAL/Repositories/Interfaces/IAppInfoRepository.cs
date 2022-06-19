using DAL.Entities;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IAppInfoRepository : IGenericRepository<AppInfo, int>
    {
        Task Save(AppInfo info);
    }
}
