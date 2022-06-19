using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Seeds
{
    public static class CompanySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company() { Id = 1, Name = "Company"}
            );
            modelBuilder.Entity<CompanyOptions>().HasData(
                new CompanyOptions() { Id = 1, CompanyId = 1, IsTrackActivity = true, IsTrackAppUsage = true, IsTrackLocation = true, IsTrackScreenShot = true }
            );
        }
    }
}
