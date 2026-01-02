using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Brands
{
    public class BrandCreateVM
    {
        //public int Id { get; set; }
      
        //public IFormFile Image { get; set; }
        [Required]
        public IEnumerable<IFormFile> NewImages { get; set; }
    }
}
