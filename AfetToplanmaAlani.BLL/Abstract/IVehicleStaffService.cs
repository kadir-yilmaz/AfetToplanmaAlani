using AfetToplanmaAlani.EL.Models;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IVehicleStaffService
    {
        Task<IEnumerable<VehicleStaff>> GetAllAsync(string? searchTerm = null);
        Task<VehicleStaff?> GetByIdAsync(int id);
        Task AddAsync(VehicleStaff vehicleStaff);
        Task UpdateAsync(VehicleStaff vehicleStaff);
        Task DeleteAsync(int id);
    }
}
