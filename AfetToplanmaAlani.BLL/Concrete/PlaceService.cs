using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.EL.Dtos.Place;
using AfetToplanmaAlani.EL.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class PlaceService : IPlaceService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlaceService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreatePlaceAsync(PlaceDtoForCreate dto)
        {
            var entity = _mapper.Map<Place>(dto);
            _context.Places.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlaceAsync(int id)
        {
            var entity = await _context.Places.FirstOrDefaultAsync(p => p.Id == id);
            if (entity is null)
            {
                return;
            }

            _context.Places.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Place>> GetAllPlacesAsync()
        {
            return await BuildPlaceQuery().ToListAsync();
        }

        public async Task<IEnumerable<Place>> GetAllPlacesWithCategoryAsync()
        {
            return await BuildPlaceQuery()
                .Include(p => p.PlaceCategory)
                .ToListAsync();
        }

        public async Task<Place?> GetPlaceByIdAsync(int id)
        {
            return await BuildPlaceQuery().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PlaceDtoForUpdate?> GetPlaceForUpdateAsync(int id)
        {
            var entity = await _context.Places.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return entity is null ? null : _mapper.Map<PlaceDtoForUpdate>(entity);
        }

        public async Task UpdatePlaceAsync(PlaceDtoForUpdate dto)
        {
            var entity = _mapper.Map<Place>(dto);
            _context.Places.Update(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Place> BuildPlaceQuery()
        {
            return _context.Places.Include(p => p.Staff).AsNoTracking();
        }
    }
}
