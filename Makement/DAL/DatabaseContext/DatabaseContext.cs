using Common.AppConfiguration;
using DAL.Entities;
using DAL.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL.DatabseContext
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public class OptionBuild
        {
            public OptionBuild()
            {
                settings = new AppConfiguration();
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }

            private AppConfiguration settings { get; set; }
            public DbContextOptionsBuilder<DatabaseContext> opsBuilder { get; set; }
            public DbContextOptions<DatabaseContext> dbOptions { get; set; }
        }
        public DatabaseContext() : base() { }
        public static OptionBuild ops = new OptionBuild();
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<AppInfo> AppInfo { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<UserTaskPeriod> TaskPeriods { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RegistrationCode> RegistrationCodes { get; set; }
        public DbSet<AppInfoMobile> AppInfoMobiles { get; set; }
        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }
        public DbSet<ScreenShot> ScreenShots { get; set; }
        public DbSet<CompanyOptions> CompaniesOptions { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Seed();
        }
    }
}