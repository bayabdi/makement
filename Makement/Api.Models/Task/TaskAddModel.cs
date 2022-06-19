using Microsoft.AspNetCore.Http;
using System;

namespace Api.Models.Task
{
    public class TaskAddModel
    {
        public string Text { get; set; }
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public DateTime DeadLine { get; set; }
        public IFormFile Doc { get; set; }
        public string DocName { get; set; }
    }
}
