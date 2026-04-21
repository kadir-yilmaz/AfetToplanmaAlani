using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.EL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using NToastNotify;
using X.PagedList.Extensions;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class WorkGroupStaffController : Controller
    {
        private readonly IWorkGroupStaffService _staffService;
        private readonly IWebHostEnvironment _env;
        private readonly IToastNotification _toastNotification;

        public WorkGroupStaffController(IWorkGroupStaffService staffService, IWebHostEnvironment env, IToastNotification toastNotification)
        {
            _staffService = staffService;
            _env = env;
            _toastNotification = toastNotification;
        }

        private List<string> GetWorkGroups()
        {
            var filePath = Path.Combine(_env.WebRootPath, "data", "workgroups.json");
            if (!System.IO.File.Exists(filePath))
            {
                return new List<string>();
            }

            var json = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        public async Task<IActionResult> Index(string search = "", string workGroup = "", int pageNumber = 1, int pageSize = 20)
        {
            // Sayfa boyutu sınırlaması (min 20, max 100)
            pageSize = Math.Max(20, Math.Min(100, pageSize));
            
            var staffList = await _staffService.SearchStaffsAsync(search, workGroup);
            var paginatedList = staffList.ToPagedList(pageNumber, pageSize);

            ViewBag.Search = search;
            ViewBag.WorkGroup = workGroup;
            ViewBag.WorkGroups = GetWorkGroups();
            
            var allStaffs = await _staffService.GetAllStaffsAsync();
            ViewBag.TotalCount = allStaffs.Count();
            ViewBag.FilteredCount = paginatedList.TotalItemCount;
            ViewBag.PageSize = pageSize;

            return View(paginatedList);
        }

        public IActionResult Create()
        {
            ViewBag.WorkGroups = GetWorkGroups();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkGroupStaff staff)
        {
            if (ModelState.IsValid)
            {
                await _staffService.CreateStaffAsync(staff);
                _toastNotification.AddSuccessToastMessage("Personel başarıyla eklendi.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.WorkGroups = GetWorkGroups();
            return View(staff);
        }

        public async Task<IActionResult> Update(int id)
        {
            var staff = await _staffService.GetOneStaffAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            ViewBag.WorkGroups = GetWorkGroups();
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(WorkGroupStaff staff)
        {
            if (ModelState.IsValid)
            {
                await _staffService.UpdateStaffAsync(staff);
                _toastNotification.AddSuccessToastMessage("Personel başarıyla güncellendi.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.WorkGroups = GetWorkGroups();
            return View(staff);
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

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var staff = await _staffService.GetOneStaffAsync(id);
            if (staff == null)
            {
                _toastNotification.AddErrorToastMessage("Silinecek personel bulunamadı.");
                return RedirectToAction(nameof(Index));
            }

            return View(staff);
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
        public async Task<IActionResult> ExportExcel(string search = "", string workGroup = "")
        {
            try
            {
                var content = await _staffService.ExportToExcelAsync(search, workGroup);

                var fileName = $"Calisma_Grubu_Personel_Listesi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(workGroup))
                {
                    fileName = $"Calisma_Grubu_Personel_Listesi_Filtrelenmiş_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
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
