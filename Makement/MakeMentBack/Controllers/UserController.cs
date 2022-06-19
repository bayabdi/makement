using Api.Models.User;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Web.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IOrganizationService organizationService;
        private readonly IEmailService emailService;

        public UserController(UserManager<User> userManager, IConfiguration configuration, IUserService userService, IOrganizationService organizationService, IEmailService emailService)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.organizationService = organizationService;
            this.configuration = configuration;
            this.emailService = emailService;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
            var user = userManager.FindByNameAsync(model.Email).Result;
            if (user != null && userManager.CheckPasswordAsync(user, model.Password).Result && !user.IsDeleted)
            {
                var userRoles = userManager.GetRolesAsync(user).Result;

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                JwtSecurityToken token;

                token = new JwtSecurityToken(
                        expires: DateTime.Now.AddHours(10),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userRoles = userRoles,
                    userId = user.Id,
                    CompanyId = user.CompanyId
                });
            }
            return Unauthorized();
        }
        
        [HttpPost("SignUp")]
        public IActionResult SignUp(RegisterModel model)
        {
            var user_ = userManager.FindByNameAsync(model.Email).Result;
           
            if (user_ != null)
            {
                return BadRequest("3");
            }

            User user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                CompanyId= 1,
                Position = model.Position
            };

            var result = userManager.CreateAsync(user, model.Password).Result;
            var role = userManager.AddToRoleAsync(user, "USER").Result;

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
        
        [HttpPost("UpdateTeam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateTeam(UpdateTeamModel model)
        {
            organizationService.UpdateUserTeam(model);
            return Ok();
        }
        
        [HttpPost("Edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Edit(EditModel model)
        {
            userService.Edit(model);
            return Ok();
        }
        
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            var user = userManager.FindByIdAsync(model.UserId).Result;
            userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword).Wait();
            return Ok();
        }
        
        [HttpGet("List")]
        public IActionResult List(int teamId)
        {
            var list = userService.GetByTeamId(teamId);

            return Ok(teamId);
        }
        
        [HttpGet("GetCompanyUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCompanyUsers(int companyId)
        {
            var list = userService.GetByCompanyId(companyId);
            return Ok(list);
        } 
        
        [HttpGet("GetCurrent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCurrent()
        {
            var user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            return Ok(
                new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Phone = user.PhoneNumber,
                    Position = user.Position
                });
        }
        
        [HttpPost("Invite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Invite(InviteModel model)
        {
            var user = userManager.FindByEmailAsync(model.Email).Result;

            if (user != null)
                return BadRequest("3");

            var code = userService.AddRegistrationCode(model.Email, model.CompanyId, model.RoleId);
            emailService.Confirm(model.Email, code);
            return Ok();
        }

        [HttpGet("GetUsersByTeamId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUsersByTeamId(int teamId)
        {
            var model = userService.GetByTeamId(teamId).ToList();

            for (int i = 0; i < model.Count(); i++)
            {
                var userModel = userManager.FindByIdAsync(model[i].Id).Result;
                model[i].Roles = userManager.GetRolesAsync(userModel).Result;
            }

            return Ok(model);
        }

        [HttpGet("GetUsersByCompanyId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult GetUsersByCompanyId(int companyId)
        {
            var model = userService.GetByCompanyId(companyId).ToList();

            for (int i = 0; i < model.Count(); i++) 
            {
                var userModel = userManager.FindByIdAsync(model[i].Id).Result;
                model[i].Roles = userManager.GetRolesAsync(userModel).Result;
            }

            return Ok(model);
        }

        [HttpGet("GetUserById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUserById(string id)
        {
            var model = userManager.FindByIdAsync(id).Result;

            var modelRoles = userManager.GetRolesAsync(model).Result;

            return Ok(new { user = model, roles = modelRoles });
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            return Ok(userService.GetRoles());
        }
        
        [HttpGet("GetUsersInMyTeams")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult GetUsersInMyTeams()
        {
            var id = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var model = userService.GetUsersInMyTeams(id);
            return Ok(model);
        }

        [HttpGet("DeleteUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            userService.DeleteUser(userId);
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword([Required]string email)
        {
            var user = userManager.FindByEmailAsync(email).Result;

            if (user == null)
                return BadRequest("1");

            var token = userManager.GeneratePasswordResetTokenAsync(user).Result;

            emailService.ForgotPassword(email, HttpUtility.UrlEncode(token));

            return Ok();
        }
        
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            //model.Token = HttpUtility.UrlDecode(model.Token);
            var user = userManager.FindByEmailAsync(model.Email).Result;

            if (user == null)
                return BadRequest("1");

            var resetPassResult = userManager.ResetPasswordAsync(user, model.Token, model.Password).Result;

            if (!resetPassResult.Succeeded)
                return BadRequest("2");

            return Ok();
        }
        
        [HttpGet("MakeManager")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult MakeManager(string id)
        {
            var user = userManager.FindByIdAsync(id).Result;
            
            if (userManager.IsInRoleAsync(user, "USER").Result)
            {
                userManager.RemoveFromRoleAsync(user, "USER").Wait();
            }

            userManager.AddToRoleAsync(user, "MANAGER").Wait();

            return Ok();
        }
    }
}
