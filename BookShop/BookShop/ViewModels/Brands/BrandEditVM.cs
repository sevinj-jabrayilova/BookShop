using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Brands
{
    public class BrandEditVM
    {
        public int Id { get; set; }
        public string? ExistImage { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
