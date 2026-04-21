using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.BLL.Concrete;
using AfetToplanmaAlani.DAL;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.WebUI.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // Uygulamanın kendi klasöründe 'Database' adında bir klasör oluştur
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dbFolder = Path.Combine(baseDirectory, "Database");

            // Electron modundaysak, resources/bin klasöründen ana dizine (AFAD.exe'nin yanına) çık
            if (ElectronNET.API.HybridSupport.IsElectronActive)
            {
                // resources/bin -> resources -> ana dizin (parent.parent)
                var rootDir = Directory.GetParent(baseDirectory)?.Parent?.FullName;
                if (!string.IsNullOrEmpty(rootDir))
                {
                    dbFolder = Path.Combine(rootDir, "Database");
                }
            }
            
            // Klasör yoksa oluştur
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }
            
            var dbPath = Path.Combine(dbFolder, "app.db");
            var connectionString = $"Data Source={dbPath}";

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
        }

        public static void ConfigureServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IWorkGroupStaffService, WorkGroupStaffService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IPlaceCategoryService, PlaceCategoryService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehicleStaffService, VehicleStaffService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IExcelService, ExcelService>();
        }
    }
}
