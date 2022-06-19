using System;

namespace Api.Models.Track
{
    public class AppInfoViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Info { get; set; }
        public TimeSpan Time { get; set; }
        public string UserId { get; set; }
    }
}
