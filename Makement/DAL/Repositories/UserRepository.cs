using DAL.DatabseContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class UserRepository : GenericRepository<User, string>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }

        public IEnumerable<User> GetByCompanyId(int companyId)
        {
            return context.Users.Where(x => x.CompanyId == companyId && !x.IsDeleted);
        }

        public IEnumerable<User> GetByTeamId(int teamId)
        {
            return context.Users.Include(x => x.UserTeams).Where(x => x.UserTeams.Any(x => x.TeamId == teamId) && !x.IsDeleted);
        }

        public IdentityRole GetRole(string roleId)
        {
            return context.Roles.First(x => x.Id == roleId);
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return context.Roles.ToList();
        }

        public User GetWithCompany(string userId)
        {
            return context.Users.Include(x => x.Company).FirstOrDefault(x => x.Id == userId && !x.IsDeleted);
        }
    }
}
