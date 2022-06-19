using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class AppInfoMobile
    {
        public int Id { get; set; }
        public int TotalTime { get; set; }
        public DateTime LastTimeStamp { get; set; }
        public DateTime FirstTimeStamp { get; set; }
        public string PackageName { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
