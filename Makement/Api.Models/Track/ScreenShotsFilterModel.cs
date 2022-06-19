using Common.Enum;

namespace Api.Models.Track
{
    public class ScreenShotsFilterModel
    {
        public TrackFilterEnum Date { get; set; }
        public string UserId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
