using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Task
{
    public class PeriodViewModel
    {
        public int Id { get; set; }
        public DateTime? BeginTime { get; set; }
        public long TotalTime { get; set; }
    }
}
