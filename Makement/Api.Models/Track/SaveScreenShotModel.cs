using Microsoft.AspNetCore.Http;
using System;

namespace Api.Models.Track
{
    public class SaveScreenShotModel
    {
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string File { get; set; }
    }
}
