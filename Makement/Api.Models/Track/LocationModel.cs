using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Track
{
    public class LocationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }
        public double Speed { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
    }
}
