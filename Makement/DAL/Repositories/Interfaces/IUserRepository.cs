using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, string>
    {
        IEnumerable<User> GetByTeamId(int teamId);
        IEnumerable<User> GetByCompanyId(int companyId);
        User GetWithCompany(string userId);
        IdentityRole GetRole(string roleId);
        IEnumerable<IdentityRole> GetRoles();
    }
}
