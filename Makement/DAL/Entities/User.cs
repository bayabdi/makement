using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public IEnumerable<AppInfo> AppInfo { get; set; }
        public IEnumerable<UserActivity> UserActivities { get; set; }
        public IEnumerable<UserTeam> UserTeams { get; set; }
        public IEnumerable<UserTaskPeriod> Tasks { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<AppInfoMobile> AppInfoMobiles { get; set; }
        public IEnumerable<ScreenShot> ScreenShots { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Position { get; set; }
    }
}