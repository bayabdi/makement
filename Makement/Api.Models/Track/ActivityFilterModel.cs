using Common.Enum;

namespace Api.Models.Track
{
    public class ActivityFilterModel
    {
        public TrackFilterEnum Date { get; set; }
        public string UserId { get; set; }
    }
}
