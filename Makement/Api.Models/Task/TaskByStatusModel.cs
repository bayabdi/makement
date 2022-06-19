using Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Task
{
    public class TaskByStatusModel
    {
        public string UserId { get; set; }
        public int TeamId { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
