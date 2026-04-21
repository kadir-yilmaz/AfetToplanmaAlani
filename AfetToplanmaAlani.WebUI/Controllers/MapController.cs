using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class MapController : Controller
    {
        private readonly AppDbContext _context;

        public MapController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Tüm toplanma noktalarını al
            var places = await _context.Places
                .Include(p => p.Staff)
                .Include(p => p.PlaceCategory)
                .Select(p => new PlaceViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    City = p.City,
                    District = p.District,
                    Neighborhood = p.Neighborhood,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    // Category'yi normalize et - frontend JavaScript'teki normalize fonksiyonuyla aynı mantık
                    Category = p.PlaceCategory != null ? p.PlaceCategory.Name : "",
                    Staff = p.Staff.Select(s => new StaffViewModel
                    {
                        Name = s.Name,
                        Surname = s.Surname,
                        PhoneNumber = s.PhoneNumber
                    }).ToList()
                })
                .ToListAsync();

            // Tüm kategorileri al - normalize etme, orijinal hallerini gönder
            var categories = await _context.PlaceCategories
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync();

            // Tüm şehirleri al - normalize etme, orijinal hallerini gönder
            var cities = places
                .Where(p => !string.IsNullOrEmpty(p.City))
                .Select(p => p.City)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(city => city, StringComparer.OrdinalIgnoreCase)
                .ToList();

            // ViewModel oluştur
            var vm = new HomeViewModel
            {
                TotalPlaces = places.Count,
                TotalStaff = places.Sum(p => p.Staff.Count),
                Places = places,
                Categories = categories,
                Cities = cities
            };

            return View(vm);
        }
    }
}
