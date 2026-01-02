using BookShop.Services.Interfaces;
using BookShop.ViewModels.Baskets;
using BookShop.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.ViewComponents
{
    public class BestSellingViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BestSellingViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketUIVM> basketDatas = new();

            var basketCookie = Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basketCookie))
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketUIVM>>(basketCookie);
            }

            BestSellingProductUIVM model = null;

            if (basketDatas.Any())
            {
                var topItem = basketDatas.OrderByDescending(b => b.ProductCount).First();
                var product = await _productService.GetByIdAsync(topItem.ProductId);

                model = new BestSellingProductUIVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Author = product.Author,
                    Price = product.Price,
                    Description = product.Description,
                    Image = product.ProductImages.FirstOrDefault(pi => pi.IsMain)?.Image
                };
            }

            return View(model);
        }
    }
}
