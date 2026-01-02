using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Sliders
{
    public class SliderEditVM
    {
        public string? ExistImage { get; set; }
        [Required]
        public IFormFile NewImage { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
