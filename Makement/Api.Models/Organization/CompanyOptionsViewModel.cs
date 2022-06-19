using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class CompanyOptionsViewModel
    {
        public int Id { get; set; }
        public bool IsTrackActivity { get; set; }
        public bool IsTrackAppUsage { get; set; }
        public bool IsTrackLocation { get; set; }
        public bool IsTrackScreenShot { get; set; }
        public int CompanyId { get; set; }
    }
}
