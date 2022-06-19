using Api.Models.Organization;
using Api.Models.Track;
using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface ITrackingService : IService
    {
        void SaveAppInfo(AppInfoModel info);
        void SaveActivity(ActivityModel activity);
        void SaveLocation(LocationModel model);
        void SaveAppInfoMobile(SaveAppInfoMobileModel model);
        LocationViewModel GetLocation(string userId);
        IEnumerable<LocationViewModel> FilterLocation(LocationFilterModel model);
        IEnumerable<AppInfoViewModel> GetAppInfo(string userId);
        AppInfoPagViewModel AppFilter(AppFilterModel model);
        AppInfoPagViewModel AppMobileFilter(AppFilterModel model);
        IEnumerable<ActivityViewModel> ActivityFilter(ActivityFilterModel model);
        ActivityViewModel GetActivity(string userId);
        void SaveScreenShot(SaveScreenShotModel model);
        ScreenShotPagViewModel ScreenShotsFilter(ScreenShotsFilterModel model);
    }
}
