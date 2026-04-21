using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.EL.Dtos.Staff;
using AfetToplanmaAlani.EL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using X.PagedList.Extensions;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IPlaceService _placeService;
        private readonly ILocationService _locationService;
        private readonly IWebHostEnvironment _env;
        private readonly IToastNotification _toastNotification;

        public StaffController(IStaffService staffService, IPlaceService placeService, ILocationService locationService, IWebHostEnvironment env, IToastNotification toastNotification)
        {
            _staffService = staffService;
            _placeService = placeService;
            _locationService = locationService;
            _env = env;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(string search = "", string place = "", string city = "", string district = "", string neighborhood = "", int pageNumber = 1, int pageSize = 20)
        {
            // Sayfa boyutu sınırlaması (min 20, max 100)
            pageSize = Math.Max(20, Math.Min(100, pageSize));
            
            var filter = new StaffSearchFilter
            {
                Search = search,
                Place = place,
                City = city,
                District = district,
                Neighborhood = neighborhood
            };

            var staffList = await _staffService.SearchStaffsAsync(filter);
            var paginatedList = staffList.ToPagedList(pageNumber, pageSize);

            ViewBag.Cities = await _locationService.GetCitiesAsync();
            ViewBag.Districts = await _locationService.GetDistrictsAsync(city);
            ViewBag.Neighborhoods = await _locationService.GetNeighborhoodsAsync(city, district);
            ViewBag.Search = search;
            ViewBag.Place = place;
            ViewBag.City = city;
            ViewBag.District = district;
            ViewBag.Neighborhood = neighborhood;
            
            ViewBag.TotalCount = staffList.Count();
            ViewBag.FilteredCount = paginatedList.TotalItemCount;
            ViewBag.PageSize = pageSize;

            return View(paginatedList);
        }

        public async Task<IActionResult> Create(int? placeId = null)
        {
            var dto = new StaffDtoForCreate();

            if (placeId.HasValue)
            {
                dto.PlaceId = placeId.Value;
            }

            var places = await _placeService.GetAllPlacesAsync();
            ViewBag.Places = new SelectList(places.ToList(), nameof(Place.Id), nameof(Place.Name), dto.PlaceId);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] StaffDtoForCreate staffDto)
        {
            if (ModelState.IsValid)
            {
                await _staffService.CreateStaffAsync(staffDto);
                _toastNotification.AddSuccessToastMessage("Personel başarıyla eklendi.");
                return RedirectToAction(nameof(Index));
            }

            var places = await _placeService.GetAllPlacesAsync();
            ViewBag.Places = new SelectList(places.ToList(), nameof(Place.Id), nameof(Place.Name), staffDto.PlaceId);

            return View(staffDto);
        }

        public async Task<IActionResult> Update([FromRoute(Name = "id")] int id)
        {
            var staff = await _staffService.GetOneStaffForUpdateAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var places = await _placeService.GetAllPlacesAsync();
            ViewBag.Places = new SelectList(places.ToList(), nameof(Place.Id), nameof(Place.Name), staff.PlaceId);

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] StaffDtoForUpdate staffDto)
        {
            if (ModelState.IsValid)
            {
                await _staffService.UpdateStaffAsync(staffDto);
                _toastNotification.AddSuccessToastMessage("Personel başarıyla güncellendi.");
                return RedirectToAction(nameof(Index));
            }

            var places = await _placeService.GetAllPlacesAsync();
            ViewBag.Places = new SelectList(places.ToList(), nameof(Place.Id), nameof(Place.Name), staffDto.PlaceId);

            return View(staffDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var staff = await _staffService.GetOneStaffAsync(id);
                if (staff != null)
                {
                    await _staffService.DeleteStaffAsync(id);
                    _toastNotification.AddSuccessToastMessage($"{staff.Name} {staff.Surname} personeli başarıyla silindi.");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Silinecek personel bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Personel silinirken hata oluştu: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _toastNotification.AddErrorToastMessage("Lütfen bir Excel dosyası seçiniz.");
                return RedirectToAction(nameof(Index));
            }

            var allowedTypes = new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                _toastNotification.AddErrorToastMessage("Sadece Excel dosyaları (.xlsx, .xls) yüklenebilir.");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var stream = file.OpenReadStream();
                var result = await _staffService.ImportFromExcelAsync(stream);

                if (result.SuccessCount > 0)
                {
                    _toastNotification.AddSuccessToastMessage($"{result.SuccessCount} personel başarıyla eklendi.");
                }

                if (result.ErrorCount > 0)
                {
                    _toastNotification.AddWarningToastMessage($"{result.ErrorCount} satırda hata oluştu. Detaylar: {string.Join("; ", result.ErrorMessages.Take(3))}");
                }
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Excel dosyası işlenirken hata oluştu: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportExcel(string search = "", string place = "", string city = "", string district = "", string neighborhood = "")
        {
            try
            {
                var filter = new StaffSearchFilter
                {
                    Search = search,
                    Place = place,
                    City = city,
                    District = district,
                    Neighborhood = neighborhood
                };

                var content = await _staffService.ExportToExcelAsync(filter);

                var fileName = $"Personel_Listesi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(place) ||
                    !string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(district) ||
                    !string.IsNullOrWhiteSpace(neighborhood))
                {
                    fileName = $"Personel_Listesi_Filtrelenmiş_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                }

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Excel dosyası oluşturulurken hata oluştu: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
