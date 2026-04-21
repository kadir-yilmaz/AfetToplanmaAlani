using AfetToplanmaAlani.EL.Models;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IPlaceCategoryService
    {
        Task CreateCategoryAsync(PlaceCategory category);
        Task UpdateCategoryAsync(PlaceCategory category);
        Task DeleteCategoryAsync(int id);
        Task<IEnumerable<PlaceCategory>> GetAllCategoriesAsync();
        Task<PlaceCategory?> GetCategoryByIdAsync(int id);
    }
}
