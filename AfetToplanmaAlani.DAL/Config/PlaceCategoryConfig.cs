using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PlaceCategoryConfig : IEntityTypeConfiguration<PlaceCategory>
{
    public void Configure(EntityTypeBuilder<PlaceCategory> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.Property(pc => pc.Name).IsRequired().HasMaxLength(100);

        // Seed Data - tüm isimleri küçük harf ve İngilizce yap
        builder.HasData(
            new PlaceCategory { Id = 1, Name = "Toplanma Noktaları" },
            new PlaceCategory { Id = 2, Name = "Çadır Alanları" },
            new PlaceCategory { Id = 3, Name = "Su Tesisleri" },
            new PlaceCategory { Id = 4, Name = "Fırın" },
            new PlaceCategory { Id = 5, Name = "Eczane" }
        );
    }
}
