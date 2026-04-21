# Afet Toplanma Alanı Yönetim Sistemi

Bu proje, afet anında toplanma alanlarının yönetimi ve personel atamaları için geliştirilmiş bir Electron.NET uygulamasıdır.

## Teknolojiler
- .NET 8.0 (ASP.NET Core MVC)
- Electron.NET
- Entity Framework Core (SQLite)

## Geliştirme Ortamı Kurulumu
1. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) yüklü olduğundan emin olun.
2. [Node.js](https://nodejs.org/) (v18+) yüklü olduğundan emin olun.
3. Electron.NET CLI aracını yükleyin:
   ```bash
   dotnet tool install ElectronNET.CLI -g
   ```

## Uygulamayı Çalıştırma
Projeyi geliştirme modunda başlatmak için:
```bash
cd AfetToplanmaAlani.WebUI
dotnet electronize start
```

## Sürüm Yayınlama (Release)
Yeni bir sürüm yayınlamak için GitHub üzerinden bir "Tag" oluşturmanız yeterlidir:
1. Değişikliklerinizi commit yapın ve pushlayın.
2. Yeni bir etiket oluşturun:
   ```bash
   git tag v1.0.0
   git push --tags
   ```
3. GitHub Actions otomatik olarak build alacak ve "Releases" kısmına ZIP dosyasını yükleyecektir.
