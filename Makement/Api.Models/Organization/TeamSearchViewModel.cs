using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class TeamSearchViewModel
    {
        public IEnumerable<TeamViewModel> Teams { get; set; }
        public int TotalTeams { get; set; }
    }
}
