using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AfetToplanmaAlani.DAL
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Uygulama klasöründe veritabanı dosyası oluştur
            var dbPath = GetDatabasePath();
            var connectionString = $"Data Source={dbPath}";

            optionsBuilder.UseSqlite(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }

        private string GetDatabasePath()
        {
            // Veritabanını kullanıcı verisi klasöründe oluştur
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dbFolder = Path.Combine(appDataPath, "AFAD");

            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            return Path.Combine(dbFolder, "app.db");
        }

    }
}