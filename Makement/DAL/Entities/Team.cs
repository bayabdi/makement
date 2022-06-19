using System.Collections.Generic;

namespace DAL.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<UserTeam> UserTeams { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
