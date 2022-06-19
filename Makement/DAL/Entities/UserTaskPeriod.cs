using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class UserTaskPeriod
    {
        public int Id { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TaskId { get; set; }
        public UserTask Task { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
