using System.Collections.Generic;

namespace Api.Models.User
{
    public class UpdateTeamModel
    {
        public string UserId { get; set; }
        public List<int> Teams { get; set; }
    }
}