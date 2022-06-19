using Api.Models.Organization;
using Api.Models.Task;
using Api.Models.Track;
using Api.Models.User;
using AutoMapper;
using DAL.Entities;
using System;
using System.Linq;

namespace BLL.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            # region User
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Phone, x => x.MapFrom(m => m.PhoneNumber))
                .ForMember(x => x.SecondName, x => x.MapFrom(m => m.SecondName))
                .ForMember(x => x.FirstName, x => x.MapFrom(m => m.FirstName))
                .ForMember(x => x.Email, x => x.MapFrom(m => m.Email))
                .ForMember(x => x.Position, x => x.MapFrom(m => m.Position))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<UserViewModel, User>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(m => m.Phone))
                .ForMember(x => x.SecondName, x => x.MapFrom(m => m.SecondName))
                .ForMember(x => x.FirstName, x => x.MapFrom(m => m.FirstName))
                .ForMember(x => x.Email, x => x.MapFrom(m => m.Email))
                .ForMember(x => x.Position, x => x.MapFrom(m => m.Position))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region AppMobile
            CreateMap<SaveAppInfoMobileModel, AppInfoMobile>()
                .ForMember(x => x.TotalTime, x => x.MapFrom(m => m.TotalTime))
                .ForMember(x => x.PackageName, x => x.MapFrom(m => m.PackageName))
                .ForMember(x => x.LastTimeStamp, x => x.MapFrom(m => m.LastTimeStamp))
                .ForMember(x => x.FirstTimeStamp, x => x.MapFrom(m => m.FirstTimeStamp))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<AppInfoMobile, SaveAppInfoMobileModel>()
                .ForMember(x => x.TotalTime, x => x.MapFrom(m => m.TotalTime))
                .ForMember(x => x.PackageName, x => x.MapFrom(m => m.PackageName))
                .ForMember(x => x.LastTimeStamp, x => x.MapFrom(m => m.LastTimeStamp))
                .ForMember(x => x.FirstTimeStamp, x => x.MapFrom(m => m.FirstTimeStamp))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<AppInfoViewModel, AppInfo>()
                .ForMember(x => x.Time, x => x.MapFrom(m => m.Time))
                .ForMember(x => x.Info, x => x.MapFrom(m => m.Info))
                .ForMember(x => x.Date, x => x.MapFrom(m => m.Date))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<AppInfo, AppInfoViewModel>()
                .ForMember(x => x.Time, x => x.MapFrom(m => m.Time))
                .ForMember(x => x.Info, x => x.MapFrom(m => m.Info))
                .ForMember(x => x.Date, x => x.MapFrom(m => m.Date))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<AppInfoMobile, AppInfoViewModel>()
                .ForMember(x => x.Time, x => x.MapFrom(m => new TimeSpan(m.TotalTime / (1000 * 60 * 60), m.TotalTime / (1000 * 60), m.TotalTime / (1000))))
                .ForMember(x => x.Info, x => x.MapFrom(m => m.PackageName))
                .ForMember(x => x.Date, x => x.MapFrom(m => Convert.ToDateTime(m.FirstTimeStamp)))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region Team
            CreateMap<Team, TeamListViewModel>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                .ForMember(x => x.UsersNumber, x => x.MapFrom(m => m.UserTeams.Count()))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<TeamListViewModel, Team>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region Location
            CreateMap<Location, LocationViewModel>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Latitude, x => x.MapFrom(m => m.Latitude))
                .ForMember(x => x.Longitude, x => x.MapFrom(m => m.Longitude))
                .ForMember(x => x.Speed, x => x.MapFrom(m => m.Speed))
                .ForMember(x => x.Time, x => x.MapFrom(m => m.Time))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Accuracy, x => x.MapFrom(m => m.Accuracy))
                .ForMember(x => x.Address, x => x.MapFrom(m => m.Address))
                .ForMember(x => x.Altitude, x => x.MapFrom(m => m.Altitude))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<LocationViewModel, Location>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Latitude, x => x.MapFrom(m => m.Latitude))
                .ForMember(x => x.Longitude, x => x.MapFrom(m => m.Longitude))
                .ForMember(x => x.Speed, x => x.MapFrom(m => m.Speed))
                .ForMember(x => x.Time, x => x.MapFrom(m => m.Time))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Accuracy, x => x.MapFrom(m => m.Accuracy))
                .ForMember(x => x.Address, x => x.MapFrom(m => m.Address))
                .ForMember(x => x.Altitude, x => x.MapFrom(m => m.Altitude))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region Task
            CreateMap<TaskViewModel, UserTask>()
               .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
               .ForMember(x => x.Text, x => x.MapFrom(m => m.Text))
               .ForMember(x => x.Status, x => x.MapFrom(m => m.Status))
               .ForMember(x => x.DeadLine, x => x.MapFrom(m => m.DeadLine))
               .ForMember(x => x.DocName, x => x.MapFrom(m => m.DocName))
               .ForAllOtherMembers(x => x.Ignore());
            CreateMap<UserTask, TaskAddModel>()
               .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
               .ForMember(x => x.Text, x => x.MapFrom(m => m.Text))
               .ForMember(x => x.TeamId, x => x.MapFrom(m => m.TeamId))
               .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
               .ForMember(x => x.DeadLine, x => x.MapFrom(m => m.DeadLine))
               .ForMember(x => x.DocName, x => x.MapFrom(m => m.DocName))
               .ForAllOtherMembers(x => x.Ignore());
            CreateMap<TaskAddModel, UserTask>()
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Text, x => x.MapFrom(m => m.Text))
                .ForMember(x => x.TeamId, x => x.MapFrom(m => m.TeamId))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.DeadLine, x => x.MapFrom(m => m.DeadLine))
                .ForMember(x => x.DocName, x => x.MapFrom(m => m.DocName))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<UserTask, TaskViewModel>()
               .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
               .ForMember(x => x.Text, x => x.MapFrom(m => m.Text))
               .ForMember(x => x.Status, x => x.MapFrom(m => m.Status))
               .ForMember(x => x.DeadLine, x => x.MapFrom(m => m.DeadLine))
               .ForMember(x => x.User, x => x.MapFrom(m => new UserViewModel
               {
                   FirstName = m.User.FirstName,
                   SecondName = m.User.SecondName,
                   Id = m.User.Id,
                   Email = m.User.Email
               }))
               .ForMember(x => x.BeginTime, x => x.MapFrom(m => m.Periods.LastOrDefault().BeginTime))
               .ForMember(x => x.TotalTime, 
                    x => x.MapFrom(m => Convert.ToInt64(m.Periods.Where(x => x.EndTime != null).Sum(x => (x.EndTime - x.BeginTime).Value.TotalSeconds))))
               .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
               .ForMember(x => x.DocName, x => x.MapFrom(m => m.DocName))
               .ForAllOtherMembers(x => x.Ignore());
            CreateMap<TaskViewModel, UserTask>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
               .ForMember(x => x.Text, x => x.MapFrom(m => m.Text))
               .ForMember(x => x.Status, x => x.MapFrom(m => m.Status))
               .ForMember(x => x.DeadLine, x => x.MapFrom(m => m.DeadLine))
               .ForMember(x => x.User, x => x.MapFrom(m => new User
               {
                   FirstName = m.User.FirstName,
                   SecondName = m.User.SecondName,
                   Id = m.User.Id,
                   Email = m.User.Email
               }))
               .ForMember(x => x.DocName, x => x.MapFrom(m => m.DocName))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<UserTaskPeriod, PeriodViewModel>()
               .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
               .ForMember(x => x.BeginTime, x => x.MapFrom(m => m.BeginTime))
               .ForAllOtherMembers(x => x.Ignore());
            CreateMap<PeriodViewModel, UserTaskPeriod>()
               .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
               .ForMember(x => x.BeginTime, x => x.MapFrom(m => m.BeginTime))
               .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region Activity
            CreateMap<ActivityViewModel, UserActivity>()
                .ForMember(x => x.AbsenceTime, x => x.MapFrom(m => m.AbsenceTime))
                .ForMember(x => x.ActivityTime, x => x.MapFrom(m => m.ActivityTime))
                .ForMember(x => x.Date, x => x.MapFrom(m => m.Date))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<UserActivity, ActivityViewModel>()
                .ForMember(x => x.AbsenceTime, x => x.MapFrom(m => m.AbsenceTime))
                .ForMember(x => x.ActivityTime, x => x.MapFrom(m => m.ActivityTime))
                .ForMember(x => x.Date, x => x.MapFrom(m => m.Date))
                .ForMember(x => x.UserId, x => x.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region ScreenShot
            CreateMap<ScreenShot, ScreenShotViewModel>()
                .ForMember(x => x.DateTime, x => x.MapFrom(m => m.Time))
                .ForMember(x => x.File, x => x.MapFrom(m => (Convert.ToBase64String(m.Img))))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<ScreenShotViewModel, ScreenShot>()
                .ForMember(x => x.Time, x => x.MapFrom(m => m.DateTime))
                .ForMember(x => x.Img, x => x.MapFrom(m => m.File))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            #region Company
            CreateMap<CompanyOptions, CompanyOptionsViewModel>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.CompanyId, x => x.MapFrom(m => m.CompanyId))
                .ForMember(x => x.IsTrackActivity, x => x.MapFrom(m => m.IsTrackActivity))
                .ForMember(x => x.IsTrackAppUsage, x => x.MapFrom(m => m.IsTrackAppUsage))
                .ForMember(x => x.IsTrackLocation, x => x.MapFrom(m => m.IsTrackLocation))
                .ForMember(x => x.IsTrackScreenShot, x => x.MapFrom(m => m.IsTrackScreenShot))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<CompanyOptionsViewModel, CompanyOptions>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.CompanyId, x => x.MapFrom(m => m.CompanyId))
                .ForMember(x => x.IsTrackActivity, x => x.MapFrom(m => m.IsTrackActivity))
                .ForMember(x => x.IsTrackAppUsage, x => x.MapFrom(m => m.IsTrackAppUsage))
                .ForMember(x => x.IsTrackLocation, x => x.MapFrom(m => m.IsTrackLocation))
                .ForMember(x => x.IsTrackScreenShot, x => x.MapFrom(m => m.IsTrackScreenShot))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Company, CompanyViewModel>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<CompanyViewModel, Company>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
        }
    }
}
