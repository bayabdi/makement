using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }
        public double Speed { get; set; }
        public string Address { get; set; }
        public DateTime Time { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
