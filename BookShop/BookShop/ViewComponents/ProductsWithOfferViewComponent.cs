using BookShop.Services.Interfaces;
using BookShop.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookShop.ViewComponents
{
    public class ProductsWithOfferViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsWithOfferViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basketCookie = HttpContext.Request.Cookies["basket"];
            if (basketCookie is null)
                return Content("");

            var basketItems = JsonConvert
                .DeserializeObject<List<BasketUIVM>>(basketCookie);

            if (basketItems is null || basketItems.Count == 0)
                return Content("");

            var productIds = basketItems.Select(x => x.ProductId).ToList();

            var products = await _productService
                .GetProductsWithOfferAsync(productIds);

            return products.Any() ? View(products) : Content("");
        }
    }
}
