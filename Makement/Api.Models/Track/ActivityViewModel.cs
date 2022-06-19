using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Track
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public TimeSpan ActivityTime { get; set; }
        public TimeSpan AbsenceTime { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}
