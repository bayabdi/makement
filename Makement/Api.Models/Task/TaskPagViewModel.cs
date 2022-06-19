using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Task
{
    public class TaskPagViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; set; }
        public int Total { get; set; }
    }
}
