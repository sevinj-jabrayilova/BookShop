using BookShop.Services.Interfaces;
using BookShop.ViewModels;
using BookShop.ViewModels.Baskets;
using BookShop.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;

        public HomeController(ISliderService sliderService, 
                            IBrandService brandService,
                            ICategoryService categoryService,
                            IProductService productService,
                            IBlogService blogService)
        {
            _sliderService = sliderService;
            _brandService = brandService;
            _categoryService = categoryService;
            _productService = productService;
            _blogService = blogService;
        }

        public async Task<ActionResult> Index()
        {
            HomeVM model = new HomeVM
            {
                Sliders = await _sliderService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Products = await _productService.GetAllAsync(),
                Blogs = await _blogService.GetAllAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToBasket(int id)
        {
            List<BasketUIVM> basketDatas = new();

            var basketCookie = Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basketCookie))
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketUIVM>>(basketCookie);
            }

            var data = basketDatas.FirstOrDefault(b => b.ProductId == id);

            if (data != null)
            {                
                data.ProductCount++;
            }
            else
            {
                basketDatas.Add(new BasketUIVM
                {
                    ProductId = id,
                    ProductCount = 1
                });
            }


            Response.Cookies.Append(
                "basket",
                JsonConvert.SerializeObject(basketDatas),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                });

            int basketCount = basketDatas.Sum(x => x.ProductCount);

            return Ok(new { count = basketCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetBestSelling()
        {
            var basketCookie = Request.Cookies["basket"];
            List<BasketUIVM> basketDatas = new();
            if (!string.IsNullOrEmpty(basketCookie))
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketUIVM>>(basketCookie);
            }

            if (!basketDatas.Any())
                return Json(null);

            var topItem = basketDatas.OrderByDescending(b => b.ProductCount).First();
            var product = await _productService.GetByIdAsync(topItem.ProductId);

            return Json(new
            {
                product.Id,
                product.Name,
                product.Author,
                product.Price,
                product.Description,
                Image = product.ProductImages.FirstOrDefault(pi => pi.IsMain)?.Image
            });
        }


    }
}
