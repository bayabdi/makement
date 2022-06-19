namespace DAL.Entities
{
    public class CompanyOptions
    {
        public int Id { get; set; }
        public bool IsTrackActivity { get; set; }
        public bool IsTrackAppUsage { get; set; }
        public bool IsTrackLocation { get; set; }
        public bool IsTrackScreenShot { get; set; }

        public Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}
