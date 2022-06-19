namespace DAL.Entities
{
    public class RegistrationCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string RoleId { get; set; }
    }
}
