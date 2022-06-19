using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.User
{
    public class OptionModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsTrackActivity { get; set; }
        public bool IsTrackAppUsage { get; set; }
        public bool IsTrackAppMobileUsage { get; set; }
        public bool IsTrackLocation { get; set; }
        public bool IsTrackScreenShot { get; set; }
    }
}
