using System;
using DAL.DatabseContext;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext context;
        private UserRepository users;
        private AppInfoRepository appInfo;
        private UserActivityRepository userActivities;
        private CompanyRepository companies;
        private TeamRepository teams;
        private UserTeamRepository userTeams;
        private TaskRepository tasks;
        private LocationRepository locations;
        private RegistrationCodeRepository registrationCodes;
        private AppInfoMobileRepository appInfoMobile;
        private TaskPeriodsRepository taskPeriods;
        private ApplicationVersionRepository applicationVersion;
        private ScreenShotRepository screenShot;
        private CompanyOptionsRepository companyOption;
        private EmailMessageRepository emailMessage;

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
        }

        public IUserRepository Users => users = users ?? new UserRepository(context);
        public IAppInfoRepository AppInfo => appInfo = appInfo ?? new AppInfoRepository(context);
        public IUserActivityRepository UserActivities => userActivities = userActivities ?? new UserActivityRepository(context);
        public ICompanyRepository Companies => companies = companies ?? new CompanyRepository(context);
        public ITeamRepository Teams => teams = teams ?? new TeamRepository(context);
        public IUserTeamRepository UserTeams => userTeams = userTeams ?? new UserTeamRepository(context);
        public ITaskRepository Tasks => tasks = tasks ?? new TaskRepository(context);
        public ILocationRepository Locations => locations = locations ?? new LocationRepository(context);
        public IAppInfoMobileRepository AppInfoMobile => appInfoMobile = appInfoMobile ?? new AppInfoMobileRepository(context);
        public IRegistrationCodeRepository RegistrationCodes => registrationCodes = registrationCodes ?? new RegistrationCodeRepository(context);
        public ITaskPeriodsRepository TaskPeriods => taskPeriods = taskPeriods ?? new TaskPeriodsRepository(context);
        public IApplicationVersionRepository ApplicationVersion => applicationVersion = applicationVersion ?? new ApplicationVersionRepository(context);
        public IScreenShotRepository ScreenShot => screenShot = screenShot ?? new ScreenShotRepository(context);
        public ICompanyOptionsRepository CompanyOption => companyOption = companyOption ?? new CompanyOptionsRepository(context);
        public IEmailMessageRepository EmailMessage => emailMessage = emailMessage ?? new EmailMessageRepository(context);

        public void Commit()
        {
            context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
    }
}
