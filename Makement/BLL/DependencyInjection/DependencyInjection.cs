using BLL.Services;
using BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITrackingService, TrackingService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
