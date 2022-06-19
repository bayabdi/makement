using Common.Enum;

namespace Api.Models.Task
{
    public class ChangeStatusModel
    {
        //Task Id
        public int Id { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string UserId { get; set; }
    }
}
