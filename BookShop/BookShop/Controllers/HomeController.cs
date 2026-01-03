using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Interfaces;
using BookShop.ViewModels;
using BookShop.ViewModels.Baskets;
using BookShop.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userInManager;

        public HomeController(ISliderService sliderService,
                            IBrandService brandService,
                            ICategoryService categoryService,
                            IProductService productService,
                            IBlogService blogService,
                            UserManager<AppUser> signInManager,
                            AppDbContext appDbContext)
        {
            _sliderService = sliderService;
            _brandService = brandService;
            _categoryService = categoryService;
            _productService = productService;
            _blogService = blogService;
            _userInManager = signInManager;
            _appDbContext = appDbContext;
        }

        public async Task<ActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = await _userInManager.FindByNameAsync(User.Identity.Name);
                ViewBag.Email = user.Email;
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string? email)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userInManager.FindByNameAsync(User.Identity.Name);

                Subscriber subs = new Subscriber()
                {
                    AppUserId = user.Id
                };

                await _appDbContext.Subscribers.AddAsync(subs);
                await _appDbContext.SaveChangesAsync();

                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.Email = user.Email;
                }

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

            return Ok();
        }
    }
}
