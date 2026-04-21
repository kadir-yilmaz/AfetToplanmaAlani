using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.WebUI.Infrastructure.Extension;
using ElectronNET.API;
using ElectronNET.API.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// NToastNotify ekleme
builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
{
    ProgressBar = true,
    PositionClass = NToastNotify.ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true,
    TimeOut = 3000
});

builder.WebHost.UseElectron(args);
builder.Services.AddElectron();

builder.Services.ConfigureDbContext(builder.Configuration, builder.Environment);
builder.Services.ConfigureServiceRegistration(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

await InitializeDatabaseAsync(app);

_ = Task.Run(async () =>
{
    var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
    {
        Width = 1200,
        Height = 800,
        Show = true,
        AutoHideMenuBar = true
    });
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// NToastNotify middleware
app.UseNToastNotify();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        await context.Database.EnsureCreatedAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veritabanı başlatma hatası: {ex.Message}");
    }
}
