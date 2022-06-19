using Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Organization
{
    public class AppFilterModel
    {
        public TrackFilterEnum Date { get; set; }
        public string UserId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
