using BLL.Services.Interfaces;
using Common.Enum;
using DAL;
using DAL.Entities;
using System.IO;
using System.Reflection;

namespace BLL.Services
{
    public class EmailService : Service, IEmailService
    {
        public EmailService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void Confirm(string email, string code)
        {
            string body = string.Empty;
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            using (StreamReader reader = new StreamReader(Path.Combine(directory, "zzMessageInvite.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("#code", code);
            body = body.Replace("#email", email);

            var message = new EmailMessage()
            {
                Email = email,
                Text = body,
                Subject = "Invite",
                Type = EmailMessageTypeEnum.Request
            };

            UnitOfWork.EmailMessage.Add(message);
            UnitOfWork.Commit();
        }

        public void ForgotPassword(string email, string token)
        {
            string body = string.Empty;
            string url = $"makement.org/resetPassword?token={token}&email={email}";
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            using (StreamReader reader = new StreamReader(Path.Combine(directory, "zzForgotPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("#url", url);
            body = body.Replace("#email", email);

            var message = new EmailMessage()
            {
                Email = email,
                Text = body,
                Subject = "Reset",
                Type = EmailMessageTypeEnum.Request
            };

            UnitOfWork.EmailMessage.Add(message);
            UnitOfWork.Commit();
        }
    }
}
