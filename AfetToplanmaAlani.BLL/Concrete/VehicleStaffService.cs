using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class VehicleStaffService : IVehicleStaffService
    {
        private readonly AppDbContext _context;

        public VehicleStaffService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleStaff>> GetAllAsync(string? searchTerm = null)
        {
            var query = _context.VehicleStaff
                .Include(vs => vs.Vehicle)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    (s.Name != null && EF.Functions.Like(s.Name, $"%{searchTerm}%")) ||
                    (s.Surname != null && EF.Functions.Like(s.Surname, $"%{searchTerm}%")) ||
                    (s.Vehicle != null && s.Vehicle.Plate != null && EF.Functions.Like(s.Vehicle.Plate, $"%{searchTerm}%")));
            }

            return await query.ToListAsync();
        }

        public async Task<VehicleStaff?> GetByIdAsync(int id)
        {
            return await _context.VehicleStaff
                .Include(vs => vs.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(vs => vs.Id == id);
        }

        public async Task AddAsync(VehicleStaff vehicleStaff)
        {
            await _context.VehicleStaff.AddAsync(vehicleStaff);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleStaff vehicleStaff)
        {
            _context.VehicleStaff.Update(vehicleStaff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.VehicleStaff.FindAsync(id);
            if (entity != null)
            {
                _context.VehicleStaff.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
