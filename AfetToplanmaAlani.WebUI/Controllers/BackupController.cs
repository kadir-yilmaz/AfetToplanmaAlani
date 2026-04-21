using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AfetToplanmaAlani.DAL;
using ElectronNET.API;
using ElectronNET.API.Entities;
using NToastNotify;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class BackupController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toastNotification;

        public BackupController(AppDbContext context, IWebHostEnvironment environment, IConfiguration configuration, IToastNotification toastNotification)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Electron modunda çalışıyorsa Save Dialog açar ve yedek alır
        /// Web modunda doğrudan dosya indirir
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                // Veritabanı dosya yolunu al
                var dbPath = GetDatabasePath();
                
                if (!System.IO.File.Exists(dbPath))
                {
                    _toastNotification.AddErrorToastMessage("Veritabanı dosyası bulunamadı.");
                    return RedirectToAction("Index");
                }

                // WAL (Write-Ahead Log) dosyasındaki verileri ana dosyaya işle
                // Bu işlem yapılmazsa .db dosyası eksik veya boş görünebilir
                await _context.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(TRUNCATE);");

                // Dosya adını oluştur
                // Not: Windows dosya sisteminde : (iki nokta) kullanılamaz.
                // Profesyonel ve sıralanabilir format: Yıl-Ay-Gün_Saat-Dakika-Saniye
                var fileName = $"AFAD_Yedek_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.db";

                // Electron modunda mı kontrol et
                if (HybridSupport.IsElectronActive)
                {
                    // Save Dialog aç
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new SaveDialogOptions
                    {
                        Title = "Yedek Dosyasını Kaydet",
                        DefaultPath = fileName, // Sadece dosya adı öner, konum kullanıcıya kalsın
                        Filters = new FileFilter[]
                        {
                            new FileFilter { Name = "SQLite Database", Extensions = new string[] { "db" } }
                        }
                    };

                    var savePath = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, options);

                    if (!string.IsNullOrEmpty(savePath))
                    {
                        // Veritabanı bağlantılarını kesin olarak kapat
                        await _context.Database.CloseConnectionAsync();
                        // Connection Pool'u temizle ve dosya kilidini kaldır
                        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        System.IO.File.Copy(dbPath, savePath, overwrite: true);
                        
                        _toastNotification.AddSuccessToastMessage($"Yedek başarıyla kaydedildi: {Path.GetFileName(savePath)}");
                    }
                    else
                    {
                        _toastNotification.AddInfoToastMessage("Yedekleme iptal edildi.");
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    // Web modunda VACUUM INTO ile güvenli yedek al (Non-Blocking)
                    // Bu yöntem veritabanı kilitliyken bile çalışır ve Transaction-Safe bir kopya oluşturur.
                    
                    var tempPath = Path.GetTempFileName();
                    // TempFileName boş dosya oluşturur, VACUUM INTO üzerine yazamaz, bu yüzden siliyoruz
                    System.IO.File.Delete(tempPath); 

                    // VACUUM INTO 'dosya_yolu' komutu, o anki veritabanının birebir kopyasını oluşturur.
                    // WAL dosyasıyla uğraşmaya gerek kalmaz, otomatik olarak birleştirilmiş temiz bir .db verir.
                    var vacuumCommand = $"VACUUM INTO '{tempPath}'";
                    await _context.Database.ExecuteSqlRawAsync(vacuumCommand);

                    try
                    {
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(tempPath);
                        return File(fileBytes, "application/octet-stream", fileName);
                    }
                    finally
                    {
                        // Geçici dosyayı temizle
                        if (System.IO.File.Exists(tempPath))
                        {
                            System.IO.File.Delete(tempPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Yedekleme hatası: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Electron modunda Open Dialog açar ve yedekten geri yükler
        /// Web modunda dosya yükleme formu kullanır
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RestoreBackup(IFormFile? backupFile = null)
        {
            try
            {
                var dbPath = GetDatabasePath();
                string? sourcePath = null;

                // Electron modunda mı kontrol et
                if (HybridSupport.IsElectronActive && backupFile == null)
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new OpenDialogOptions
                    {
                        Title = "Yedek Dosyasını Seç",
                        Properties = new OpenDialogProperty[] { OpenDialogProperty.openFile },
                        Filters = new FileFilter[]
                        {
                            new FileFilter { Name = "SQLite Database", Extensions = new string[] { "db" } }
                        }
                    };

                    var result = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);

                    if (result != null && result.Length > 0)
                    {
                        sourcePath = result[0];
                    }
                    else
                    {
                        _toastNotification.AddInfoToastMessage("Geri yükleme iptal edildi.");
                        return RedirectToAction("Index");
                    }
                }
                else if (backupFile != null && backupFile.Length > 0)
                {
                    // Web modunda yüklenen dosyayı kullan
                    var tempPath = Path.GetTempFileName();
                    using (var stream = new FileStream(tempPath, FileMode.Create))
                    {
                        await backupFile.CopyToAsync(stream);
                    }
                    sourcePath = tempPath;
                }

                if (string.IsNullOrEmpty(sourcePath))
                {
                    _toastNotification.AddErrorToastMessage("Yedek dosyası seçilmedi.");
                    return RedirectToAction("Index");
                }

                // SQLite dosyası olduğunu doğrula (basit kontrol)
                var header = new byte[16];
                using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    await fs.ReadAsync(header, 0, 16);
                }
                
                var headerText = System.Text.Encoding.ASCII.GetString(header);
                if (!headerText.StartsWith("SQLite format"))
                {
                    _toastNotification.AddErrorToastMessage("Geçersiz veritabanı dosyası. Lütfen geçerli bir SQLite yedek dosyası seçin.");
                    return RedirectToAction("Index");
                }

                // Mevcut veritabanını yedekle (güvenlik için)
                var backupPath = dbPath + $".backup-{DateTime.Now:yyyyMMdd-HHmmss}";

                // Veritabanı bağlantılarını kesin olarak kapat
                await _context.Database.CloseConnectionAsync();
                await _context.DisposeAsync();

                // Connection Pool'u temizle ve dosya kilidini kaldır
                Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
                // Kısa bir bekleme
                await Task.Delay(100);

                if (System.IO.File.Exists(dbPath))
                {
                    // Eski yedeği al
                    System.IO.File.Copy(dbPath, backupPath, overwrite: true);
                }

                // Yeni veritabanını kopyala
                System.IO.File.Copy(sourcePath, dbPath, overwrite: true);

                _toastNotification.AddSuccessToastMessage("Yedek başarıyla geri yüklendi. Uygulama yeniden başlatılıyor...");
                
                // Electron modunda uygulamayı yeniden başlat
                if (HybridSupport.IsElectronActive)
                {
                    await Task.Delay(2000); // Kullanıcının mesajı görmesi için bekle
                    Electron.App.Relaunch();
                    Electron.App.Exit();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Geri yükleme hatası: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        private string GetDatabasePath()
        {
            // Veritabanı yolunu ServiceExtension ile aynı şekilde al
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dbFolder = Path.Combine(appDataPath, "AFAD");
            return Path.Combine(dbFolder, "app.db");
        }
    }
}
