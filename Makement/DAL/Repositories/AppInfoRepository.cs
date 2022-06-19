using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AppInfoRepository : GenericRepository<AppInfo, int>, IAppInfoRepository
    {
        public AppInfoRepository(DatabaseContext context) : base(context) { }

        public async Task Save(AppInfo info)
        {
            var model = context.AppInfo.FirstOrDefault(x =>
                            x.UserId == info.UserId && x.Info == info.Info
                            && x.Date.Date == info.Date.Date);
            if (model == null)
            {
                await context.AddAsync(info);
            }
            else
            {
                model.Time = info.Time;
                context.AppInfo.Update(model);
            }
        }
    }
}
