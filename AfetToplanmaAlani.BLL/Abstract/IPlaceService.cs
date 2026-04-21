using AfetToplanmaAlani.EL.Dtos.Place;
using AfetToplanmaAlani.EL.Models;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IPlaceService
    {
        Task CreatePlaceAsync(PlaceDtoForCreate placeDtoForCreate);
        Task UpdatePlaceAsync(PlaceDtoForUpdate placeDtoForUpdate);
        Task DeletePlaceAsync(int id);
        Task<IEnumerable<Place>> GetAllPlacesAsync();
        Task<IEnumerable<Place>> GetAllPlacesWithCategoryAsync();
        Task<Place?> GetPlaceByIdAsync(int id);
        Task<PlaceDtoForUpdate?> GetPlaceForUpdateAsync(int id);
    }
}
