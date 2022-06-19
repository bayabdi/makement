using Microsoft.AspNetCore.Http;
using System;

namespace Api.Models.Task
{
    public class TaskEditModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime DeadLine { get; set; }
        public IFormFile Doc { get; set; }
        public string DocName { get; set; }
    }
}
