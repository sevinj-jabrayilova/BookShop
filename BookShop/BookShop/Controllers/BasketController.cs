using BookShop.Data;
using BookShop.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BasketUIVM> basketDatas = new();

            var basketCookie = Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basketCookie))
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketUIVM>>(basketCookie);
            }

            var productIds = basketDatas.Select(x => x.ProductId).ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Include(p => p.ProductImages)
                //.Include(p => p.Categories)
                .ToListAsync();

            List<BasketItemVM> items = new();

            foreach (var basketItem in basketDatas)
            {
                var dbProduct = products.FirstOrDefault(p => p.Id == basketItem.ProductId);
                if (dbProduct == null) continue;

                items.Add(new BasketItemVM
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    Count = basketItem.ProductCount,
                    //Categories = dbProduct.Categories?.Name ?? "No category",
                    Image = dbProduct.ProductImages?
                        .FirstOrDefault(pi => pi.IsMain)?.Image
                });
            }

            BasketDetailVM model = new()
            {
                Items = items,
                TotalPrice = items.Sum(x => x.Price * x.Count)
            };

            return View(model);
        }
    }
}
