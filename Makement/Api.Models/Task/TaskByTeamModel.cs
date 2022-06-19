using Common.Enum;

namespace Api.Models.Task
{
    public class TaskByTeamModel
    {
        public int TeamId { get; set; }
        public TaskStatusEnum TaskStatus { get; set;}
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
