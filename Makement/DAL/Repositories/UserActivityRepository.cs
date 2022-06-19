using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserActivityRepository : GenericRepository<UserActivity, int>, IUserActivityRepository
    {
        public UserActivityRepository(DatabaseContext context) : base(context) { }
        public async Task Save(UserActivity activity)
        {
            var model = context.UserActivities.FirstOrDefault(x => x.UserId == activity.UserId && x.Date.Date == activity.Date.Date);

            if (model == null)
            {
                context.UserActivities.Add(activity);
            }
            else
            {
                model.ActivityTime = activity.ActivityTime;
                model.AbsenceTime = activity.AbsenceTime;
                context.UserActivities.Update(model);
            }
        }
    }
}
