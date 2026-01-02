using BookShop.Services;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Brands;
using BookShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _brandService.GetAllAdminAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);

            if (brand is null) return NotFound();

            return View(brand);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _brandService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var existSlider = await _brandService.GetByIdAsync(id.Value);
            return View(new BrandEditVM { Id = id.Value, ExistImage = existSlider.Image });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BrandEditVM request)
        {
            if (id is null) return BadRequest();

            if (!ModelState.IsValid)
            {
                var existSlider = await _brandService.GetByIdAsync(id.Value);
                return View(new BrandEditVM { ExistImage = existSlider.Image });
            }

            await _brandService.EditAsync(id.Value, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
