using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class GetUsersNotInTeamModel
    {
        public int CompanyId { get; set; }
        public int TeamId { get; set; }
    }
}
