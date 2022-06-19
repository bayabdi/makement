using Api.Models.Organization;
using Api.Models.User;
using DAL.Entities;
using System;
using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IOrganizationService : IService
    {
        IEnumerable<Team> GetTeams();
        IEnumerable<Company> GetCompanies();
        void TeamAdd(TeamAddModel model);
        void CompanyAdd(CompanyAddModel model);
        void TeamEdit(TeamEditModel model);
        void CompanyEdit(CompanyEditModel model);
        void UpdateUserTeam(UpdateTeamModel model);
        void TeamAddMember(TeamAddMemberModel model);
        void TeamDeleteMember(TeamDeleteMemberModel model);
        IEnumerable<TeamListViewModel> TeamList(int companyId);
        TeamViewModel GetTeamById(int id);
        Company GetCompanyGetById(int companyId);
        CompanyViewModel GetCompanyByName(string name);
        IEnumerable<UserViewModel> GetUsersNotInTeam(GetUsersNotInTeamModel model);
        IEnumerable<TeamListViewModel> GetTeamsByUserId(string userId);
        TeamSearchViewModel TeamSearch(string userId, TeamSearchModel model);
        void DeleteTeam(int teamId);
        IEnumerable<JournalViewModel> GetJournal(DateTime date, int companyId);
        void AddCompanyOptions(int companyId);
        CompanyOptionsViewModel GetCompanyOptions(int companyId);
        void EditCompanyOptions(CompanyOptionsEditModel model);
        TeamSearchViewModel TeamSearchAdmin(string id, int companyId, TeamSearchModel model);
    }
}
