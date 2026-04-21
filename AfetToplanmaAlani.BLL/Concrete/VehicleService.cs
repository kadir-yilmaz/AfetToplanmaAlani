using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _context;

        public VehicleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return;
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.AsNoTracking().ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string? searchPlate)
        {
            var query = _context.Vehicles.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(searchPlate))
            {
                query = query.Where(v => v.Plate != null && EF.Functions.Like(v.Plate, $"%{searchPlate}%"));
            }
            return await query.ToListAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
