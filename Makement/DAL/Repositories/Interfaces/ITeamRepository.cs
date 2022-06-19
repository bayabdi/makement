using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repositories.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team, int>
    {
        IEnumerable<Team> GetWithUserTeam();
    }
}
