using DAL.Repositories.Interfaces;
using System;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAppInfoRepository AppInfo { get; }
        IUserActivityRepository UserActivities { get; }
        ICompanyRepository Companies { get; }
        ITeamRepository Teams { get; }
        IUserTeamRepository UserTeams { get; }
        ITaskRepository Tasks { get; }
        ILocationRepository Locations { get; }
        IRegistrationCodeRepository RegistrationCodes { get; }
        IAppInfoMobileRepository AppInfoMobile { get; }
        ITaskPeriodsRepository TaskPeriods { get; }
        IApplicationVersionRepository ApplicationVersion { get; }
        IScreenShotRepository ScreenShot { get; }
        ICompanyOptionsRepository CompanyOption { get; }
        IEmailMessageRepository EmailMessage{ get; }
        void Commit();
    }
}
