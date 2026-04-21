using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class PlaceCategoryService : IPlaceCategoryService
    {
        private readonly AppDbContext _context;

        public PlaceCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCategoryAsync(PlaceCategory category)
        {
            _context.PlaceCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.PlaceCategories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return;
            _context.PlaceCategories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlaceCategory>> GetAllCategoriesAsync()
        {
            return await _context.PlaceCategories.AsNoTracking().ToListAsync();
        }

        public async Task<PlaceCategory?> GetCategoryByIdAsync(int id)
        {
            return await _context.PlaceCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCategoryAsync(PlaceCategory category)
        {
            _context.PlaceCategories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
