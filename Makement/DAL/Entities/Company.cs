using System.Collections.Generic;

namespace DAL.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<User> Users { get; set; }
        public CompanyOptions Option { get; set; }
    }
}
