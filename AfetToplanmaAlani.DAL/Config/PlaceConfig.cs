using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AfetToplanmaAlani.DAL.Config
{
    public class PlaceConfig : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
            builder.Property(p => p.City).IsRequired().HasMaxLength(100);
            builder.Property(p => p.District).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Neighborhood).HasMaxLength(100);
            builder.Property(p => p.ContactNumber).HasMaxLength(20);

            // İlişki konfigürasyonu düzeltmesi
            builder
                .HasOne(p => p.PlaceCategory)  // Navigation property belirt
                .WithMany(pc => pc.Places)
                .HasForeignKey(p => p.PlaceCategoryId)  // FK property belirt
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasData(
    new Place { Id = 1, Name = "Merkez Park Toplanma Alanı", City = "İstanbul", District = "Kadıköy", Neighborhood = "Fenerbahçe", Latitude = 40.9890, Longitude = 29.0280, ContactNumber = "02120000000", PlaceCategoryId = 1 },
    new Place { Id = 2, Name = "Moda Çadır Alanı", City = "İstanbul", District = "Kadıköy", Neighborhood = "Moda", Latitude = 40.9912, Longitude = 29.0321, ContactNumber = "02120000001", PlaceCategoryId = 2 },
    new Place { Id = 3, Name = "Avcılar Su Tesisi", City = "İstanbul", District = "Avcılar", Neighborhood = "Cihangir", Latitude = 40.9791, Longitude = 28.7221, ContactNumber = "02120000002", PlaceCategoryId = 3 },
    new Place { Id = 4, Name = "Bakırköy Fırını", City = "İstanbul", District = "Bakırköy", Neighborhood = "Kartaltepe", Latitude = 40.9805, Longitude = 28.8742, ContactNumber = "02120000003", PlaceCategoryId = 4 },
    new Place { Id = 5, Name = "Beşiktaş Eczanesi", City = "İstanbul", District = "Beşiktaş", Neighborhood = "Levent", Latitude = 41.0438, Longitude = 29.0094, ContactNumber = "02120000004", PlaceCategoryId = 5 },

    new Place { Id = 6, Name = "Kızılay Toplanma Alanı", City = "Ankara", District = "Çankaya", Neighborhood = "Kızılay", Latitude = 39.9208, Longitude = 32.8541, ContactNumber = "03120000000", PlaceCategoryId = 1 },
    new Place { Id = 7, Name = "Bahçelievler Çadır Alanı", City = "Ankara", District = "Çankaya", Neighborhood = "Bahçelievler", Latitude = 39.9334, Longitude = 32.8597, ContactNumber = "03120000001", PlaceCategoryId = 2 },
    new Place { Id = 8, Name = "Ulus Su Tesisi", City = "Ankara", District = "Altındağ", Neighborhood = "Ulus", Latitude = 39.9272, Longitude = 32.8640, ContactNumber = "03120000002", PlaceCategoryId = 3 },
    new Place { Id = 9, Name = "Mamak Fırını", City = "Ankara", District = "Mamak", Neighborhood = "OSB", Latitude = 39.9460, Longitude = 32.8580, ContactNumber = "03120000003", PlaceCategoryId = 4 },
    new Place { Id = 10, Name = "Yenimahalle Eczanesi", City = "Ankara", District = "Yenimahalle", Neighborhood = "Batıkent", Latitude = 39.9512, Longitude = 32.7534, ContactNumber = "03120000004", PlaceCategoryId = 5 },

    new Place { Id = 11, Name = "Alsancak Toplanma Alanı", City = "İzmir", District = "Konak", Neighborhood = "Alsancak", Latitude = 38.4192, Longitude = 27.1287, ContactNumber = "02320000000", PlaceCategoryId = 1 },
    new Place { Id = 12, Name = "Göztepe Çadır Alanı", City = "İzmir", District = "Konak", Neighborhood = "Göztepe", Latitude = 38.4149, Longitude = 27.1282, ContactNumber = "02320000001", PlaceCategoryId = 2 },
    new Place { Id = 13, Name = "Bornova Su Tesisi", City = "İzmir", District = "Bornova", Neighborhood = "Kazım Dirik", Latitude = 38.4660, Longitude = 27.2213, ContactNumber = "02320000002", PlaceCategoryId = 3 },
    new Place { Id = 14, Name = "Buca Fırını", City = "İzmir", District = "Buca", Neighborhood = "Evka 3", Latitude = 38.4085, Longitude = 27.1115, ContactNumber = "02320000003", PlaceCategoryId = 4 },
    new Place { Id = 15, Name = "Karşıyaka Eczanesi", City = "İzmir", District = "Karşıyaka", Neighborhood = "Alaybey", Latitude = 38.4520, Longitude = 27.0511, ContactNumber = "02320000004", PlaceCategoryId = 5 },

    new Place { Id = 16, Name = "Nilüfer Toplanma Alanı", City = "Bursa", District = "Osmangazi", Neighborhood = "Nilüfer", Latitude = 40.1897, Longitude = 29.0617, ContactNumber = "02240000000", PlaceCategoryId = 1 },
    new Place { Id = 17, Name = "Beşevler Çadır Alanı", City = "Bursa", District = "Osmangazi", Neighborhood = "Beşevler", Latitude = 40.2002, Longitude = 29.0723, ContactNumber = "02240000001", PlaceCategoryId = 2 },
    new Place { Id = 18, Name = "Hüdavendigar Su Tesisi", City = "Bursa", District = "Yıldırım", Neighborhood = "Hüdavendigar", Latitude = 40.1970, Longitude = 29.0750, ContactNumber = "02240000002", PlaceCategoryId = 3 },
    new Place { Id = 19, Name = "Görükle Fırını", City = "Bursa", District = "Nilüfer", Neighborhood = "Görükle", Latitude = 40.1850, Longitude = 28.9900, ContactNumber = "02240000003", PlaceCategoryId = 4 },
    new Place { Id = 20, Name = "Altınova Eczanesi", City = "Bursa", District = "Osmangazi", Neighborhood = "Altınova", Latitude = 40.2030, Longitude = 29.0550, ContactNumber = "02240000004", PlaceCategoryId = 5 },

    new Place { Id = 21, Name = "Sıhhiye Toplanma Alanı", City = "Ankara", District = "Çankaya", Neighborhood = "Sıhhiye", Latitude = 39.9331, Longitude = 32.8590, ContactNumber = "03120000005", PlaceCategoryId = 1 },
    new Place { Id = 22, Name = "Bahçelievler Çadır Alanı", City = "Ankara", District = "Çankaya", Neighborhood = "Bahçelievler", Latitude = 39.9340, Longitude = 32.8600, ContactNumber = "03120000006", PlaceCategoryId = 2 },
    new Place { Id = 23, Name = "Kızılay Su Tesisi", City = "Ankara", District = "Çankaya", Neighborhood = "Kızılay", Latitude = 39.9320, Longitude = 32.8550, ContactNumber = "03120000007", PlaceCategoryId = 3 },
    new Place { Id = 24, Name = "Çankaya Fırını", City = "Ankara", District = "Çankaya", Neighborhood = "Çankaya", Latitude = 39.9350, Longitude = 32.8580, ContactNumber = "03120000008", PlaceCategoryId = 4 },
    new Place { Id = 25, Name = "Batıkent Eczanesi", City = "Ankara", District = "Yenimahalle", Neighborhood = "Batıkent", Latitude = 39.9510, Longitude = 32.7530, ContactNumber = "03120000009", PlaceCategoryId = 5 },

    new Place { Id = 26, Name = "Karşıyaka Toplanma Alanı", City = "İzmir", District = "Karşıyaka", Neighborhood = "Karşıyaka", Latitude = 38.4510, Longitude = 27.0520, ContactNumber = "02320000005", PlaceCategoryId = 1 },
    new Place { Id = 27, Name = "Bostanlı Çadır Alanı", City = "İzmir", District = "Karşıyaka", Neighborhood = "Bostanlı", Latitude = 38.4550, Longitude = 27.0500, ContactNumber = "02320000006", PlaceCategoryId = 2 },
    new Place { Id = 28, Name = "Bornova Su Tesisi2", City = "İzmir", District = "Bornova", Neighborhood = "Kazım Dirik", Latitude = 33.4660, Longitude = 27.2210, ContactNumber = "02320000007", PlaceCategoryId = 3 },
    new Place { Id = 29, Name = "Konak Fırını", City = "İzmir", District = "Konak", Neighborhood = "Konak", Latitude = 38.4170, Longitude = 27.1280, ContactNumber = "02320000008", PlaceCategoryId = 4 },
    new Place { Id = 30, Name = "Göztepe Eczanesi", City = "İzmir", District = "Konak", Neighborhood = "Göztepe", Latitude = 38.4150, Longitude = 27.1290, ContactNumber = "02320000009", PlaceCategoryId = 5 }
);


        }
    }
}