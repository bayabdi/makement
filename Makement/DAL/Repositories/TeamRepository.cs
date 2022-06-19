using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class TeamRepository : GenericRepository<Team, int>, ITeamRepository
    {
        public TeamRepository(DatabaseContext context) : base(context) { }

        public IEnumerable<Team> GetWithUserTeam()
        {
            return context.Teams.Include(x => x.UserTeams).ThenInclude(y => y.User);
        }
    }
}
