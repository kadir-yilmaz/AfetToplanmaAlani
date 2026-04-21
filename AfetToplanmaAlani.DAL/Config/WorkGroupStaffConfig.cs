using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AfetToplanmaAlani.DAL.Config
{
    public class WorkGroupStaffConfig : IEntityTypeConfiguration<WorkGroupStaff>
    {
        public void Configure(EntityTypeBuilder<WorkGroupStaff> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(w => w.Surname)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(w => w.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(w => w.WorkGroup)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(w => w.Crew)
                   .HasMaxLength(100);

            builder.Property(w => w.Duty)
                   .HasMaxLength(100);

            // WorkGroup listesi (JSON’dan alınan veriler gömüldü)
            string[] workGroups = new string[]
            {
                "Bilgi Yönetimi Değerlendirme Ve Haberleşme Çalışma Grubu",
                "Arama Kurtarma Çalışma Grubu",
                "Barınma Çalışma Grubu",
                "Hasar Tespit Çalışma Grubu",
                "Enkaz Kaldırma Çalışma Grubu",
                "Yangın Çalışma Grubu",
                "Beslenme Çalışma Grubu",
                "Sağlık Çalışma Grubu",
                "Ayni Bağış Ve Depo Yönetimi Çalışma Grubu",
                "Psikososyal Çalışma Grubu",
                "Güvenlik Ve Trafik Çalışma Grubu",
                "Tahliye Yerleştirme Çalışma Grubu",
                "Afet Zarar Tespit Çalışma Grubu",
                "Afet Kimliklendirme Ve Defin Çalışma Grubu",
                "Afet Finans Ve Kaynak Yönetimi Çalışma Grubu",
                "Afet Tarım Orman Gıda Su Ve Hayvancılık Çalışma Grubu",
                "Afet Enerji Çalışma Grubu",
                "Afet Haberleşme Çalışma Grubu",
                "Afet Ulaşım Alt Yapı Çalışma Grubu",
                "Afet Nakliye Çalışma Grubu",
                "Afet Alt Yapı Çalışma Grubu",
                "Afet Teknik Destek Ve İkmal Çalışma Grubu",
                "Afet İletişim Çalışma Grubu"
            };

            // Seed Data (örnek 20 kişi)
            builder.HasData(
                new WorkGroupStaff { Id = 1, Name = "Ali", Surname = "Yılmaz", PhoneNumber = "5551110001", WorkGroup = workGroups[0], Crew = "AFAD", Duty = "Lider" },
                new WorkGroupStaff { Id = 2, Name = "Ayşe", Surname = "Demir", PhoneNumber = "5551110002", WorkGroup = workGroups[1], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 3, Name = "Mehmet", Surname = "Kaya", PhoneNumber = "5551110003", WorkGroup = workGroups[2], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 4, Name = "Fatma", Surname = "Öztürk", PhoneNumber = "5551110004", WorkGroup = workGroups[3], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 5, Name = "Ahmet", Surname = "Çelik", PhoneNumber = "5551110005", WorkGroup = workGroups[4], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 6, Name = "Elif", Surname = "Ak", PhoneNumber = "5551110006", WorkGroup = workGroups[5], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 7, Name = "Can", Surname = "Yıldız", PhoneNumber = "5551110007", WorkGroup = workGroups[6], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 8, Name = "Selin", Surname = "Arslan", PhoneNumber = "5551110008", WorkGroup = workGroups[7], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 9, Name = "Deniz", Surname = "Kurt", PhoneNumber = "5551110009", WorkGroup = workGroups[8], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 10, Name = "Burak", Surname = "Demirtaş", PhoneNumber = "5551110010", WorkGroup = workGroups[9], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 11, Name = "Derya", Surname = "Polat", PhoneNumber = "5551110011", WorkGroup = workGroups[10], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 12, Name = "Ege", Surname = "Kara", PhoneNumber = "5551110012", WorkGroup = workGroups[11], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 13, Name = "Merve", Surname = "Yalçın", PhoneNumber = "5551110013", WorkGroup = workGroups[12], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 14, Name = "Ozan", Surname = "Güneş", PhoneNumber = "5551110014", WorkGroup = workGroups[13], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 15, Name = "Seda", Surname = "Erdoğan", PhoneNumber = "5551110015", WorkGroup = workGroups[14], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 16, Name = "Kaan", Surname = "Öz", PhoneNumber = "5551110016", WorkGroup = workGroups[15], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 17, Name = "Nazlı", Surname = "Uçar", PhoneNumber = "5551110017", WorkGroup = workGroups[16], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 18, Name = "Emre", Surname = "Şahin", PhoneNumber = "5551110018", WorkGroup = workGroups[17], Crew = "Kızılay", Duty = "Üye" },
                new WorkGroupStaff { Id = 19, Name = "Büşra", Surname = "Aydın", PhoneNumber = "5551110019", WorkGroup = workGroups[18], Crew = "AFAD", Duty = "Üye" },
                new WorkGroupStaff { Id = 20, Name = "Serkan", Surname = "Topal", PhoneNumber = "5551110020", WorkGroup = workGroups[19], Crew = "Kızılay", Duty = "Üye" }
            );
        }
    }
}
