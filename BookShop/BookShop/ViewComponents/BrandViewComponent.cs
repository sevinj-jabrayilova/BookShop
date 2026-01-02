using BookShop.Services;
using BookShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly IBrandService _brandService;

        public BrandViewComponent(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = await _brandService.GetAllAsync();
            return View(brands);
        }
    }
}
