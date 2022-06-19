using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class UserTeamRepository : GenericRepository<UserTeam, int>, IUserTeamRepository
    {
        public UserTeamRepository(DatabaseContext context) : base(context) { }
    }
}
