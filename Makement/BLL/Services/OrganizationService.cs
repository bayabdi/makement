using Api.Models.Organization;
using Api.Models.User;
using BLL.Services.Interfaces;
using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class OrganizationService : Service, IOrganizationService
    {
        public OrganizationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void CompanyAdd(CompanyAddModel model)
        {
            var company = new Company()
            {
                Name = model.Name
            };

            UnitOfWork.Companies.Add(company);
            UnitOfWork.Commit();
        }

        public CompanyViewModel GetCompanyByName(string name)
        {
            var company = UnitOfWork.Companies.GetAll().Result.FirstOrDefault(x => x.Name == name);
            return mapper.Map<Company, CompanyViewModel>(company);
        }

        public void CompanyEdit(CompanyEditModel model)
        {
            var company = UnitOfWork.Companies.Get(model.Id).Result;
            company.Name = model.Name;

            UnitOfWork.Companies.Update(company);
            UnitOfWork.Commit();
        }

        public IEnumerable<Company> GetCompanies()
        {
            return UnitOfWork.Companies.GetAll().Result;
        }

        public Company GetCompanyGetById(int companyId)
        {
            return UnitOfWork.Companies.Get(companyId).Result;
        }

        public TeamViewModel GetTeamById(int id)
        {
            var team = UnitOfWork.Teams.GetWithUserTeam().Where(x => x.IsDeleted == false).First(x => x.Id == id);

            return new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Users = team.UserTeams.Select(x => 
                    new UserTeamViewModel 
                    { 
                        Id = x.UserId, 
                        FirstName = x.User.FirstName, 
                        SecondName = x.User.SecondName,
                        Position = x.User.Position
                    }
                ).ToList()
            };
        }

        public IEnumerable<Team> GetTeams()
        {
            return UnitOfWork.Teams.GetAll().Result.Where(x => x.IsDeleted == false);
        }

        public IEnumerable<UserViewModel> GetUsersNotInTeam(GetUsersNotInTeamModel model)
        {
            var users = UnitOfWork.Companies.GetAllWithUsers().Result
                .FirstOrDefault(x => x.Id == model.CompanyId)
                .Users.Where(x => !x.IsDeleted).ToList();
            var userTeams = UnitOfWork.UserTeams.GetAll().Result.Where(x => x.TeamId == model.TeamId);
            var list = users.FindAll(x => !userTeams.Any(z => z.UserId == x.Id) && !x.IsDeleted);
            return mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(list);
        }

        public void TeamAdd(TeamAddModel model)
        {
            var team = new Team
            {
                Company = UnitOfWork.Companies.Get(model.CompanyId).Result,
                Name = model.Name,
            };

            UnitOfWork.Teams.Add(team);
            UnitOfWork.Commit();
        }

        public void TeamAddMember(TeamAddMemberModel model)
        {
            var userTeam = new UserTeam();
            var team = UnitOfWork.Teams.Get(model.TeamId).Result;
            var user = UnitOfWork.Users.Get(model.UserId).Result;

            userTeam.UserId = model.UserId;
            userTeam.User = user;
            userTeam.Team = team;
            userTeam.TeamId = model.TeamId;

            UnitOfWork.UserTeams.Add(userTeam);
            UnitOfWork.Commit();
        }

        public void TeamDeleteMember(TeamDeleteMemberModel model)
        {
            var userTeam = UnitOfWork.UserTeams.GetAll().Result.First(x => x.TeamId == model.TeamId && x.UserId == model.UserId);
            UnitOfWork.UserTeams.Delete(userTeam);
            UnitOfWork.Commit();
        }

        public void TeamEdit(TeamEditModel model)
        {
            var team = UnitOfWork.Teams.Get(model.Id).Result;
            team.Name = model.Name;

            UnitOfWork.Teams.Update(team);
            UnitOfWork.Commit();
        }

        public IEnumerable<TeamListViewModel> TeamList(int companyId)
        {
            var list = UnitOfWork.Teams.GetWithUserTeam().Where(x => x.CompanyId == companyId).Where(x => x.IsDeleted == false);

            var model = mapper.Map<IEnumerable<Team>, IEnumerable<TeamListViewModel>>(list);
            
            return model;
        }

        public void UpdateUserTeam(UpdateTeamModel model)
        {
            var userTeams = UnitOfWork.UserTeams.GetAll().Result.Where(x => x.UserId == model.UserId);
            var deleteTeams = userTeams.Where(x => !model.Teams.Contains(x.TeamId));
            //var user = UnitOfWork.Users.GetAll().Result.First(x => x.Id == model.UserId);
            var addTeams = model.Teams.Where(x => !userTeams.Any(y => y.TeamId == x))
                .Select(x => new UserTeam { TeamId = x, UserId = model.UserId });

            foreach (var userTeam in deleteTeams)
            {
                UnitOfWork.UserTeams.Delete(userTeam);
            }
            foreach (var userTeam in addTeams)
            {
                UnitOfWork.UserTeams.Add(userTeam);
            }
            UnitOfWork.Commit();
        }

        public IEnumerable<TeamListViewModel> GetTeamsByUserId(string userId)
        {
            var teams = UnitOfWork.Teams.GetWithUserTeam()
                                        .Where(x => x.UserTeams.Any(z => z.UserId == userId)).Where(x => x.IsDeleted == false);
            var model = mapper.Map<IEnumerable<Team>, IEnumerable<TeamListViewModel>>(teams);
            return model;
        }

        public TeamSearchViewModel TeamSearch(string userId, TeamSearchModel model)
        {
            if (model.TeamName == null)
                model.TeamName = "";

            var list = UnitOfWork.Teams.GetWithUserTeam()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserTeams.Any(x => x.UserId == userId))
                .Where(x => x.Name.ToLower().Contains(model.TeamName.ToLower()))
                .Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(x => new TeamViewModel {
                    Id = x.Id,
                    Name = x.Name,
                    TotalUsers = x.UserTeams.Count()
                });

            var teams = new TeamSearchViewModel()
            {
                Teams = list,
                TotalTeams = UnitOfWork.Teams.GetWithUserTeam()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserTeams.Any(x => x.UserId == userId))
                .Where(x => x.Name.ToLower().Contains(model.TeamName.ToLower()))
                .Count()
            };

            return teams;
        }
        public void DeleteTeam(int teamId)
        {
            var team = UnitOfWork.Teams.Get(teamId).Result;
            team.IsDeleted = true;
            UnitOfWork.Teams.Update(team);

            var tasks = UnitOfWork.Tasks.GetAll().Result.Where(x => x.TeamId == teamId);

            foreach(var task in tasks)
            {
                task.IsDeleted = true;
                UnitOfWork.Tasks.Update(task);
            }

            UnitOfWork.Commit();
        }

        private JournalViewModel GetJournalRow(User user, DateTime date)
        {
            var row = new JournalViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName
            };

            var beginDate = date.Date;
            var endDate = date.Date.AddDays(1);

            var list = UnitOfWork.TaskPeriods.GetAll().Result.Where(x => x.UserId == user.Id);

            var lastActive = list.FirstOrDefault(x => x.EndTime == null);
            
            // Active Task Doesn't Exist
            if (lastActive == null || lastActive.BeginTime > endDate)
            {
                row.BeginTime = list.OrderBy(x => x.BeginTime)
                    .FirstOrDefault(x => x.BeginTime < endDate && beginDate < x.EndTime)?.BeginTime;
                row.EndTime = list.OrderByDescending(x => x.EndTime)
                    .FirstOrDefault(x => x.BeginTime < endDate && beginDate < x.EndTime)?.EndTime;
                    

                if (row.BeginTime != null && row.BeginTime < beginDate)
                    row.BeginTime = beginDate;

                if (row.EndTime != null && row.EndTime > endDate)
                    row.EndTime = endDate;
            }
            else
            {
                row.BeginTime = list.OrderBy(x => x.BeginTime)
                    .FirstOrDefault(x => x.Id != lastActive.Id && x.BeginTime < endDate && beginDate < x.EndTime)?.BeginTime;
                row.EndTime = DateTime.Now;

                if (row.BeginTime == null)
                    row.BeginTime = lastActive.BeginTime;
                if (row.BeginTime < beginDate)
                    row.BeginTime = beginDate;
            }

            if (row.BeginTime != null && row.EndTime != null)
                row.Duration = (DateTime)row.EndTime - (DateTime)row.BeginTime;

            return row;
        }

        public IEnumerable<JournalViewModel> GetJournal(DateTime date, int companyId)
        {
            return UnitOfWork.Users.GetByCompanyId(companyId).Where(x => x.IsDeleted == false)
                .Select(x => GetJournalRow(x, date));
        }

        public void AddCompanyOptions(int companyId)
        {
            var options = new CompanyOptions
            {
                CompanyId = companyId,
                IsTrackActivity = true,
                IsTrackAppUsage = true,
                IsTrackLocation = true,
                IsTrackScreenShot = true
            };
            UnitOfWork.CompanyOption.Add(options);
            UnitOfWork.Commit();
        }

        public CompanyOptionsViewModel GetCompanyOptions(int companyId)
        {
            var options = UnitOfWork.CompanyOption.GetAll().Result.FirstOrDefault(x => x.CompanyId == companyId);
            return mapper.Map<CompanyOptions, CompanyOptionsViewModel>(options);
        }
        
        public void EditCompanyOptions(CompanyOptionsEditModel model)
        {
            var options = UnitOfWork.CompanyOption.GetAll().Result.FirstOrDefault(x => x.CompanyId == model.CompanyId);

            options.IsTrackActivity = model.IsTrackActivity;
            options.IsTrackAppUsage = model.IsTrackAppUsage;
            options.IsTrackLocation = model.IsTrackLocation;
            options.IsTrackScreenShot = model.IsTrackScreenShot;

            UnitOfWork.CompanyOption.Update(options);
            UnitOfWork.Commit();
        }

        public TeamSearchViewModel TeamSearchAdmin(string userId, int companyId, TeamSearchModel model)
        {
            if (model.TeamName == null)
                model.TeamName = "";

            var list = UnitOfWork.Teams.GetAll().Result
                .Where(x => x.CompanyId == companyId)
                .Where(x => x.IsDeleted == false)
                .Where(x => x.Name.ToLower().Contains(model.TeamName.ToLower()))
                .Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(x => new TeamViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TotalUsers = x.UserTeams.Count()
                });

            var teams = new TeamSearchViewModel()
            {
                Teams = list,
                TotalTeams = UnitOfWork.Teams.GetWithUserTeam()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.CompanyId == companyId)
                .Where(x => x.Name.ToLower().Contains(model.TeamName.ToLower()))
                .Count()
            };

            return teams;
        }
    }
}
