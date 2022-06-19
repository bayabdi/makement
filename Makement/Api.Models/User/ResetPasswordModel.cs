using System.ComponentModel.DataAnnotations;

namespace Api.Models.User
{
    public class ResetPasswordModel
    {
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
