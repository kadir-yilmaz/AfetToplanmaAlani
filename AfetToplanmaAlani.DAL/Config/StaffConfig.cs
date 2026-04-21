using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AfetToplanmaAlani.DAL.Config
{
    public class StaffConfig : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(s => s.Surname)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(s => s.PhoneNumber)
                   .HasMaxLength(20);

            // Place ile ilişki
            builder
                .HasOne(s => s.Place)
                .WithMany(p => p.Staff)
                .HasForeignKey(s => s.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            /*
            // Seed Data (örnek)
            builder.HasData(
                new Staff
                {
                    Id = 1,
                    Name = "Ali",
                    Surname = "Yılmaz",
                    PhoneNumber = "5551112233",
                    PlaceId = 1
                },
                new Staff
                {
                    Id = 2,
                    Name = "Ayşe",
                    Surname = "Demir",
                    PhoneNumber = "5552223344",
                    PlaceId = 1
                },
                new Staff
                {
                    Id = 3,
                    Name = "Mehmet",
                    Surname = "Kaya",
                    PhoneNumber = "5553334455",
                    PlaceId = 5
                }
            );
            */
        }
    }
}