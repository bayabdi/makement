using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class TeamDeleteMemberModel
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
    }
}
