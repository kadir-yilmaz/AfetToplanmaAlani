using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.EL.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using X.PagedList.Extensions;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IToastNotification _toastNotification;

        public VehicleController(IVehicleService vehicleService, IToastNotification toastNotification)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(string? searchPlate, int pageNumber = 1, int pageSize = 20)
        {
            // Sayfa boyutu sınırlaması (min 20, max 100)
            pageSize = Math.Max(20, Math.Min(100, pageSize));
            
            try
            {
                var vehicles = await _vehicleService.SearchVehiclesAsync(searchPlate);
                var paginatedList = vehicles.ToPagedList(pageNumber, pageSize);
                
                ViewBag.SearchPlate = searchPlate;
                ViewBag.PageSize = pageSize;
                return View(paginatedList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                ViewBag.ErrorMessage = "Araçlar yüklenirken bir hata oluştu.";
                return View(new List<Vehicle>().ToPagedList(1, pageSize));
            }
        }

        public IActionResult Create()
        {
            return View(new Vehicle());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vehicle);
                }

                if (string.IsNullOrWhiteSpace(vehicle.Plate))
                {
                    ModelState.AddModelError("Plate", "Plaka boş olamaz.");
                    return View(vehicle);
                }

                await _vehicleService.CreateVehicleAsync(vehicle);
                _toastNotification.AddSuccessToastMessage("Araç başarıyla eklendi.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                ModelState.AddModelError("", "Araç eklenirken bir hata oluştu.");
                return View(vehicle);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    _toastNotification.AddErrorToastMessage("Araç bulunamadı.");
                    return RedirectToAction(nameof(Index));
                }

                return View(vehicle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update GET: {ex.Message}");
                _toastNotification.AddErrorToastMessage("Araç yüklenirken bir hata oluştu.");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Vehicle vehicle)
        {
            try
            {
                if (id != vehicle.Id)
                {
                    ModelState.AddModelError("", "Geçersiz araç ID'si.");
                    return View(vehicle);
                }

                if (!ModelState.IsValid)
                {
                    return View(vehicle);
                }

                if (string.IsNullOrWhiteSpace(vehicle.Plate))
                {
                    ModelState.AddModelError("Plate", "Plaka boş olamaz.");
                    return View(vehicle);
                }

                await _vehicleService.UpdateVehicleAsync(vehicle);
                _toastNotification.AddSuccessToastMessage("Araç başarıyla güncellendi.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update POST: {ex.Message}");
                ModelState.AddModelError("", "Araç güncellenirken bir hata oluştu.");
                return View(vehicle);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    _toastNotification.AddErrorToastMessage("Silinecek araç bulunamadı.");
                    return RedirectToAction(nameof(Index));
                }

                await _vehicleService.DeleteVehicleAsync(id);
                _toastNotification.AddSuccessToastMessage("Araç başarıyla silindi.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                _toastNotification.AddErrorToastMessage("Araç silinirken bir hata oluştu. Bu araca bağlı personeller olabilir.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
