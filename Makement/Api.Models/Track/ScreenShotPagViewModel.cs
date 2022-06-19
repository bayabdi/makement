using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Models.Track
{
    public class ScreenShotPagViewModel
    {
        public IEnumerable<ScreenShotViewModel> ScreenShot { get; set; }
        public int TotalScreenShots { get; set;}
    }
}
