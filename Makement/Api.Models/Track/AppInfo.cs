using Microsoft.AspNetCore.Http;
using System;

namespace Api.Models.Track
{
    public class AppInfoModel
    {
        public string UserId { get; set; }
        public string File { get; set; }
        public DateTime Date { get; set; }
    }
}
