using Common.Enum;
using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public TaskStatusEnum Status { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<UserTaskPeriod> Periods { get; set; }
        public DateTime DeadLine { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string DocName { get; set; }
    }
}
