using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Track
{
    public class AppInfoPagViewModel
    {
        public IEnumerable<AppInfoViewModel> Apps { get; set; }
        public int TotalApps { get; set; }
    }
}
