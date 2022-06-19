using Microsoft.EntityFrameworkCore;

namespace DAL.Seeds
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            RoleSeed.Seed(modelBuilder);
            CompanySeed.Seed(modelBuilder);
        }
    }
}
