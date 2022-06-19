using Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Track
{
    public class LocationFilterModel
    {
        public LocationFilterEnum PastHour { get; set; }
        public string UserId { get; set; }

    }
}
