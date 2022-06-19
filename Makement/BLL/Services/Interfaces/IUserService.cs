using Api.Models.User;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IUserService : IService
    {
        void Edit(EditModel model);
        IEnumerable<UserViewModel> GetByTeamId(int teamId);
        IEnumerable<UserViewModel> GetByCompanyId(int companyId);
        string AddRegistrationCode(string email, int companyId, string roleId);
        bool ExistRegistrationCode(string email, string code);
        void RemoveRegistrationCode(string email, string code);
        int GetCompanyIdByUserId(string userId);
        RegistrationCode GetRegistrationCode(string email, string code);
        string GetRoleName(string roleId);
        IEnumerable<RoleViewModel> GetRoles();
        IEnumerable<UserViewModel> GetUsersInMyTeams(string id);
        void DeleteUser(string userId);
    }
}
