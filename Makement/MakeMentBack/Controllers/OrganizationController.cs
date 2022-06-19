using Api.Models.Organization;
using BLL.Services.Interfaces;
using Common.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Web.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IOrganizationService organizationService;
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        public OrganizationController(UserManager<User> userManager, IOrganizationService organizationService, IUserService userService, IEmailService emailService)
        {
            this.organizationService = organizationService;
            this.userManager = userManager;
            this.userService = userService;
            this.emailService = emailService;
        }

        [HttpPost("TeamAdd")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult TeamAdd(TeamAddModel model)
        {
            organizationService.TeamAdd(model);
            var users = userManager.GetUsersInRoleAsync("Admin").Result.Where(x => x.CompanyId == model.CompanyId);
            foreach (var user in users)
            {
                var userTeam = new TeamAddMemberModel();
                userTeam.TeamId = organizationService.GetTeams().LastOrDefault(x => x.Name == model.Name).Id;
                userTeam.UserId = user.Id;
                organizationService.TeamAddMember(userTeam);
            }
            return Ok();
        }

        [HttpPost("TeamEdit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult TeamEdit(TeamEditModel model)
        {
            organizationService.TeamEdit(model);
            return Ok();
        }

        [HttpPost("TeamAddMember")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public IActionResult TeamAddMember(TeamAddMemberModel model)
        {
            organizationService.TeamAddMember(model);
            return Ok();
        }

        [HttpPost("TeamDeleteMember")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public IActionResult TeamDeleteMember(TeamDeleteMemberModel model)
        {
            organizationService.TeamDeleteMember(model);
            return Ok();
        }

        [HttpGet("TeamGetById")]
        public IActionResult TeamGetById(int id)
        {
            TeamViewModel model = organizationService.GetTeamById(id);
            for (int i = 0; i < model.Users.ToList().Count; i++)
            {
                var userModel = userManager.FindByIdAsync(model.Users.ToList()[i].Id).Result;
                var roles = userManager.GetRolesAsync(userModel).Result;
                model.Users.ToList()[i].Role = roles;
            }

            return Ok(model);
        }

        [HttpGet("TeamList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult TeamList(int companyId)
        {
            var list = organizationService.TeamList(companyId);
            return Ok(list);
        }
        
        [HttpGet("GetTeamsByUserId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetTeamsByUserId(string userId)
        {
            var list = organizationService.GetTeamsByUserId(userId);
            return Ok(list);
        }

        /*[HttpGet("GetCurrentCompany")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCurrentCompany()
        {
            
        }*/

        [HttpPost("CompanyAdd")]
        public IActionResult CompanyAdd(CompanyAddModel model)
        {
            if (model.Code != CompanyAddCode.Code)
                return BadRequest();
            organizationService.CompanyAdd(model);
            
            var company = organizationService.GetCompanyByName(model.Name);
            
            organizationService.AddCompanyOptions(company.Id);

            var code = userService.AddRegistrationCode(model.Email, company.Id, UserRolesEnum.Admin);
            
            emailService.Confirm(model.Email, code);
            
            return Ok();
        }

        [HttpPost("CompanyEdit")]
        public IActionResult CompanyEdit(CompanyEditModel model)
        {
            organizationService.CompanyEdit(model);
            return Ok();
        }
        
        [HttpPost("GetUsersNotInTeam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public IActionResult GetUsersNotInTeam(GetUsersNotInTeamModel model)
        {
            var users = organizationService.GetUsersNotInTeam(model);
            return Ok(users);
        }
        
        [HttpPost("TeamSearch")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult TeamSearch(TeamSearchModel model)
        {
            var user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            var roles = userManager.GetRolesAsync(user).Result;

            if (roles.Any(x => x == "Admin"))
            {
                return Ok(organizationService.TeamSearchAdmin(user.Id, user.CompanyId, model));
            }
            
            var list = organizationService.TeamSearch(user.Id, model);

            return Ok(list);
        }
        
        [HttpGet("DeleteTeam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult DeleteTeam(int teamId)
        {
            organizationService.DeleteTeam(teamId);
            return Ok();
        }
        
        [HttpGet("GetJournal")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult GetJournal(DateTime date, int companyId)
        {
            var list = organizationService.GetJournal(date, companyId);
            
            return Ok(list);
        }
        [HttpGet("GetCompanyOption")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCompanyOption()
        {
            var user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            var list = organizationService.GetCompanyOptions(user.CompanyId);
            return Ok(list);
        }
        [HttpPost("EditCompanyOption")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult EditCompanyOption(CompanyOptionsEditModel model)
        {
            var user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            model.CompanyId = user.CompanyId;
            
            organizationService.EditCompanyOptions(model);
            return Ok();
        }

        [HttpGet("GetCompany")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCompany()
        {
            var user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            var company = organizationService.GetCompanyGetById(user.CompanyId);
            return Ok(company);
        }
    }
}
