using System;

namespace DAL.Entities
{
    public class UserActivity
    {
        public int Id { get; set; }
        public TimeSpan ActivityTime { get; set; }
        public TimeSpan AbsenceTime { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
