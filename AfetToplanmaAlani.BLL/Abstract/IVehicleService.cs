using AfetToplanmaAlani.EL.Models;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IVehicleService
    {
        Task CreateVehicleAsync(Vehicle vehicle);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string? searchPlate);
        Task<Vehicle?> GetVehicleByIdAsync(int id);
    }
}
