using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class UserTeamViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public IEnumerable<string> Role { get; set; }
    }
}
