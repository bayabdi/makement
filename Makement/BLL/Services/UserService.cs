using Api.Models.User;
using BLL.Services.Interfaces;
using DAL;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using DAL.Entities;
using System;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class UserService : Service, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        public void Edit(EditModel model)
        {
            var user = UnitOfWork.Users.Get(model.UserId).Result;
            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.PhoneNumber = model.Phone;
            user.Position = model.Position;
            UnitOfWork.Users.Update(user);
            UnitOfWork.Commit();
        }
        public IEnumerable<UserViewModel> GetByCompanyId(int companyId)
        {
            var list = UnitOfWork.Users.GetByCompanyId(companyId).Where(x => x.IsDeleted == false)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    Phone = x.PhoneNumber,
                    Position = x.Position
                }); 
            return list;
        }
        public IEnumerable<UserViewModel> GetByTeamId(int teamId)
        {
            var list = UnitOfWork.Users.GetByTeamId(teamId).Where(x => x.IsDeleted == false)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    Phone = x.PhoneNumber,
                    Position = x.Position
                });

            return list;
        }        
        public async Task SendEmailAsync(string email, string subject, string fileName, string href)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("One way", "onewaysender@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            string body = string.Empty;
            //using streamreader for reading my htmltemplate  3r
            using (StreamReader reader = new StreamReader(Path.Combine("wwwroot/Email", fileName + ".html")))
            {
                body = reader.ReadToEnd();
            }

            var newBody = body.Replace("#hash", href);

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = newBody
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("onewaysender@gmail.com", "PFqW^4+=66yk");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }        
        public string AddRegistrationCode(string email, int companyId, string roleId)
        {
            var registrationCode = UnitOfWork.RegistrationCodes.GetAll().Result.FirstOrDefault(x => x.Email == email);

            if (registrationCode == null)
            {
                registrationCode = new RegistrationCode
                {
                    Code = Guid.NewGuid().ToString(),
                    Email = email,
                    CompanyId = companyId,
                    RoleId = roleId
                };
                UnitOfWork.RegistrationCodes.Add(registrationCode);
                UnitOfWork.Commit();
            }

            return registrationCode.Code;
        }        
        public bool ExistRegistrationCode(string email, string code)
        {
            return UnitOfWork.RegistrationCodes.GetAll().Result.Any(x => x.Email == email && x.Code == code);
        }        
        public void RemoveRegistrationCode(string email, string code)
        {
            var entity = UnitOfWork.RegistrationCodes.GetAll().Result.First(x => x.Code == code && x.Email == email);
            UnitOfWork.RegistrationCodes.Delete(entity);
            UnitOfWork.Commit();
        }
        public int GetCompanyIdByUserId(string userId)
        {
            return UnitOfWork.Users.GetWithCompany(userId).Company.Id;
        }
        public RegistrationCode GetRegistrationCode(string email, string code)
        {
            return UnitOfWork.RegistrationCodes.GetAll().Result.FirstOrDefault(x => x.Email == email && x.Code == code);
        }
        public string GetRoleName(string roleId)
        {
            return UnitOfWork.Users.GetRole(roleId).Name;
        }
        public IEnumerable<RoleViewModel> GetRoles()
        {
            return UnitOfWork.Users.GetRoles().Select(x => new RoleViewModel { Id = x.Id, Name = x.Name }); 
        }
        //no ideal
        public IEnumerable<UserViewModel> GetUsersInMyTeams(string id)
        {
            var teams = UnitOfWork.Teams.GetWithUserTeam().Where(x => x.IsDeleted == false).Where(x => x.UserTeams.Any(z => z.UserId == id));
            
            var users = teams.Select(x => x.UserTeams.Select(z => z.User));
            
            var model = new List<User>();
            
            foreach(var user in users)
            {
                model.AddRange(user);
            }
            model = model.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            model.Remove(model.FirstOrDefault(z => z.Id == id));
            return mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(model);
        }
        public void DeleteUser(string userId)
        {
            var user = UnitOfWork.Users.Get(userId).Result;
            user.IsDeleted = true;
            UnitOfWork.Users.Update(user);
            var tasks = UnitOfWork.Tasks.GetAll().Result.Where(x => x.UserId == userId);

            foreach (var task in tasks)
            {
                task.IsDeleted = true;
                UnitOfWork.Tasks.Update(task);
            }
            UnitOfWork.Commit();
        }
    }
}
