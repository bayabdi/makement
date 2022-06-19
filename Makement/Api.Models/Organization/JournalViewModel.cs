using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class JournalViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
