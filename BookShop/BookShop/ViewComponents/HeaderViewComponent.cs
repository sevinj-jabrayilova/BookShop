//using BookShop.Services.Interfaces;
//using BookShop.ViewModels.Baskets;
//using BookShop.ViewModels.Header;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;

//namespace BookShop.ViewComponents
//{
//    public class HeaderViewComponent : ViewComponent
//    {
//        private readonly ILayoutService _layoutService;

//        public HeaderViewComponent(ILayoutService layoutService)
//        {
//            _layoutService = layoutService;
//        }



//        //public async Task<IViewComponentResult> InvokeAsync()
//        //{
//        //    //var settings = await _layoutService.GetAllSettingsAsync();
//        //    //return View(settings);

//        //    List<BasketUIVM> basketDatas = [];

//        //    if (Request.Cookies["basket"] != null)
//        //    {
//        //        basketDatas = JsonConvert.DeserializeObject<List<BasketUIVM>>(Request.Cookies["basket"]);
//        //    }

//        //    int basketCount = basketDatas.Sum(m => m.ProductCount);
//        //    decimal totalPrice = basketDatas.Sum(m => m.ProductCount * m.ProductPrice);
//        //    var settings = await _layoutService.GetAllSettingsAsync();

//        //    var model = new HeaderVM
//        //    {
//        //        BasketProductCount = basketCount,
//        //        TotalPrice = totalPrice,
//        //        HeaderLogo = settings["HeaderLogo"]
//        //    };

//        //    return View(model);
//        //}
//    }
//}


using BookShop.Data;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Baskets;
using BookShop.ViewModels.Header;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookShop.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ILayoutService _layoutService;
        private readonly AppDbContext _context; // <--- buraya əlavə et

        public HeaderViewComponent(ILayoutService layoutService, AppDbContext context)
        {
            _layoutService = layoutService;
            _context = context; // <--- buraya da əlavə et
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketUIVM> basketDatas = new();

            var basketCookie = Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basketCookie))
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketUIVM>>(basketCookie);
            }

            int basketCount = basketDatas.Sum(x => x.ProductCount);
            decimal totalPrice = 0;

            if (basketDatas.Any())
            {
                var productIds = basketDatas.Select(x => x.ProductId).ToList();

                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .Select(p => new { p.Id, p.Price })
                    .ToListAsync();

                foreach (var item in basketDatas)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                        totalPrice += product.Price * item.ProductCount;
                }
            }

            var settings = await _layoutService.GetAllSettingsAsync();

            var model = new HeaderVM
            {
                BasketProductCount = basketCount,
                TotalPrice = totalPrice,
                HeaderLogo = settings["HeaderLogo"]
            };

            return View(model);
        }
    }
}
