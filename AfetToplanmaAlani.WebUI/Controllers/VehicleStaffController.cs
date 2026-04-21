using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.EL.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using X.PagedList.Extensions;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class VehicleStaffController : Controller
    {
        private readonly IVehicleStaffService _vehicleStaffService;
        private readonly IVehicleService _vehicleService;
        private readonly IToastNotification _toastNotification;

        public VehicleStaffController(IVehicleStaffService vehicleStaffService, IVehicleService vehicleService, IToastNotification toastNotification)
        {
            _vehicleStaffService = vehicleStaffService;
            _vehicleService = vehicleService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(string searchTerm, int pageNumber = 1, int pageSize = 20)
        {
            // Sayfa boyutu sınırlaması (min 20, max 100)
            pageSize = Math.Max(20, Math.Min(100, pageSize));
            
            var staffList = await _vehicleStaffService.GetAllAsync(searchTerm);
            var paginatedList = staffList.ToPagedList(pageNumber, pageSize);
            
            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageSize = pageSize;
            return View(paginatedList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var staff = await _vehicleStaffService.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        public async Task<IActionResult> Create()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            ViewBag.Vehicles = vehicles.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleStaff vehicleStaff)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                ViewBag.Vehicles = vehicles.ToList();
                return View(vehicleStaff);
            }

            await _vehicleStaffService.AddAsync(vehicleStaff);
            _toastNotification.AddSuccessToastMessage("Araç personeli başarıyla eklendi.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var staff = await _vehicleStaffService.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            ViewBag.Vehicles = vehicles.ToList();
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VehicleStaff vehicleStaff)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                ViewBag.Vehicles = vehicles.ToList();
                return View(vehicleStaff);
            }

            await _vehicleStaffService.UpdateAsync(vehicleStaff);
            _toastNotification.AddSuccessToastMessage("Araç personeli başarıyla güncellendi.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _vehicleStaffService.DeleteAsync(id);
            _toastNotification.AddSuccessToastMessage("Araç personeli başarıyla silindi.");
            return RedirectToAction(nameof(Index));
        }
    }
}
