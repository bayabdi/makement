using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder
                .HasMany(t => t.Teams)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId);

            builder
                .HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
