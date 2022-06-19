using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserTeamViewModel> Users { get; set; }
        public int TotalUsers { get; set; }
    }
}
