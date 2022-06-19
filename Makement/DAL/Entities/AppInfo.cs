using System;

namespace DAL.Entities
{
    public class AppInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Info { get; set; }
        public TimeSpan Time { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
