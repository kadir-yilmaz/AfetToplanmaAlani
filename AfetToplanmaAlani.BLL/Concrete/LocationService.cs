using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;

        public LocationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<string>> GetCitiesAsync()
        {
            return await _context.Places
                .Where(p => !string.IsNullOrEmpty(p.City))
                .Select(p => p.City!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetDistrictsAsync(string city = "")
        {
            var query = _context.Places
                .Where(p => !string.IsNullOrEmpty(p.District));

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(p => p.City == city);
            }

            return await query
                .Select(p => p.District!)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetNeighborhoodsAsync(string city = "", string district = "")
        {
            var query = _context.Places
                .Where(p => !string.IsNullOrEmpty(p.Neighborhood));

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(p => p.City == city);
            }

            if (!string.IsNullOrWhiteSpace(district))
            {
                query = query.Where(p => p.District == district);
            }

            return await query
                .Select(p => p.Neighborhood!)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        }
    }
}
