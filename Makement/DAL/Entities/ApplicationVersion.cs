using Common.Enum;

namespace DAL.Entities
{
    public class ApplicationVersion
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
    }
}
