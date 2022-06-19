namespace BLL.Services.Interfaces
{
    public interface IEmailService : IService
    {
        void Confirm(string email, string code);
        void ForgotPassword(string email, string token);
    }
}
