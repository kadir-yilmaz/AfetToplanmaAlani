using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.EL.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using X.PagedList.Extensions;

namespace AfetToplanmaAlani.WebUI.Controllers
{
    public class PlaceCategoryController : Controller
    {
        private readonly IPlaceCategoryService _placeCategoryService;
        private readonly IToastNotification _toastNotification;

        public PlaceCategoryController(IPlaceCategoryService placeCategoryService, IToastNotification toastNotification)
        {
            _placeCategoryService = placeCategoryService ?? throw new ArgumentNullException(nameof(placeCategoryService));
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            // Sayfa boyutu sınırlaması (min 20, max 100)
            pageSize = Math.Max(20, Math.Min(100, pageSize));
            
            try
            {
                var categories = await _placeCategoryService.GetAllCategoriesAsync();
                var paginatedList = categories.ToPagedList(pageNumber, pageSize);
                
                ViewBag.PageSize = pageSize;
                return View(paginatedList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                ViewBag.ErrorMessage = "Kategoriler yüklenirken bir hata oluştu.";
                return View(new List<PlaceCategory>().ToPagedList(1, pageSize));
            }
        }

        public IActionResult Create()
        {
            return View(new PlaceCategory());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaceCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError("Name", "Kategori adı boş olamaz.");
                    return View(category);
                }

                await _placeCategoryService.CreateCategoryAsync(category);
                _toastNotification.AddSuccessToastMessage("Kategori başarıyla eklendi.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                ModelState.AddModelError("", "Kategori eklenirken bir hata oluştu.");
                return View(category);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var category = await _placeCategoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _toastNotification.AddErrorToastMessage("Kategori bulunamadı.");
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update GET: {ex.Message}");
                _toastNotification.AddErrorToastMessage("Kategori yüklenirken bir hata oluştu.");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, PlaceCategory category)
        {
            try
            {
                if (id != category.Id)
                {
                    ModelState.AddModelError("", "Geçersiz kategori ID'si.");
                    return View(category);
                }

                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError("Name", "Kategori adı boş olamaz.");
                    return View(category);
                }

                await _placeCategoryService.UpdateCategoryAsync(category);
                _toastNotification.AddSuccessToastMessage("Kategori başarıyla güncellendi.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update POST: {ex.Message}");
                ModelState.AddModelError("", "Kategori güncellenirken bir hata oluştu.");
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _placeCategoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _toastNotification.AddErrorToastMessage("Silinecek kategori bulunamadı.");
                    return RedirectToAction(nameof(Index));
                }

                await _placeCategoryService.DeleteCategoryAsync(id);
                _toastNotification.AddSuccessToastMessage("Kategori başarıyla silindi.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                _toastNotification.AddErrorToastMessage("Kategori silinirken bir hata oluştu. Bu kategoriye ait yerler olabilir.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
