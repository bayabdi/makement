using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Track
{
    public class SaveAppInfoMobileModel
    {
        public int TotalTime { get; set; }
        public DateTime LastTimeStamp { get; set; }
        public DateTime FirstTimeStamp { get; set; }
        public string PackageName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
