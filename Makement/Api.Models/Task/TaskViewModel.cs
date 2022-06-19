using Api.Models.User;
using Common.Enum;
using System;

namespace Api.Models.Task
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public TaskStatusEnum Status { get; set; }
        public UserViewModel User { get; set; }
        public DateTime BeginTime { get; set; }
        public string UserId { get; set; }
        public DateTime DeadLine { get; set; }
        public string DocName { get; set; }
        public long? TotalTime { get; set; }
    }
}
