using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IAppInfoMobileRepository, AppInfoMobileRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAppInfoRepository, AppInfoRepository>();
            services.AddTransient<IUserActivityRepository, UserActivityRepository>();
            services.AddTransient<IUserTeamRepository, UserTeamRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IRegistrationCodeRepository, RegistrationCodeRepository>();
            services.AddTransient<IApplicationVersionRepository, ApplicationVersionRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
