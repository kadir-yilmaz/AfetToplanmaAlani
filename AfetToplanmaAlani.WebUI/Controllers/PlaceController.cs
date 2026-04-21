using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.EL.Dtos.Place;
using Microsoft.AspNetCore.Mvc;
using AfetToplanmaAlani.EL.Models;
using X.PagedList.Extensions;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly IPlaceCategoryService _placeCategoryService;
        private readonly ILocationService _locationService;

        public PlaceController(IPlaceService placeService, IPlaceCategoryService placeCategoryService, ILocationService locationService)
        {
            _placeService = placeService;
            _placeCategoryService = placeCategoryService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            // Sayfa boyutu sınırlaması (min 20, max 100)
            pageSize = Math.Max(20, Math.Min(100, pageSize));
            
            var places = await _placeService.GetAllPlacesWithCategoryAsync();
            var paginatedList = places.ToPagedList(pageNumber, pageSize);
            
            ViewBag.PageSize = pageSize;
            return View(paginatedList);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _placeCategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories?.ToList() ?? new List<PlaceCategory>();
            
            var cities = await _locationService.GetCitiesAsync();
            ViewBag.Cities = cities?.ToList() ?? new List<string> { "İstanbul", "Ankara", "İzmir" };

            var model = new PlaceDtoForCreate
            {
                City = "İstanbul",
                District = "Avcılar",
                Neighborhood = "Cihangir"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaceDtoForCreate dto)
        {
            if (ModelState.IsValid)
            {
                await _placeService.CreatePlaceAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _placeCategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories?.ToList() ?? new List<PlaceCategory>();
            
            var cities = await _locationService.GetCitiesAsync();
            ViewBag.Cities = cities?.ToList() ?? new List<string> { "İstanbul", "Ankara", "İzmir" };
            
            return View(dto);
        }

        public async Task<IActionResult> Update(int id)
        {
            var dto = await _placeService.GetPlaceForUpdateAsync(id);
            if (dto == null)
            {
                return NotFound();
            }

            var categories = await _placeCategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories?.ToList() ?? new List<PlaceCategory>();
            
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PlaceDtoForUpdate dto)
        {
            if (ModelState.IsValid)
            {
                await _placeService.UpdatePlaceAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _placeCategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories?.ToList() ?? new List<PlaceCategory>();
            
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _placeService.DeletePlaceAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
