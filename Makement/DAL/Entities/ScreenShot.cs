using System;

namespace DAL.Entities
{
    public class ScreenShot
    {
        public string Id { get; set; }
        public byte[] Img { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Time { get; set; }
    }
}
