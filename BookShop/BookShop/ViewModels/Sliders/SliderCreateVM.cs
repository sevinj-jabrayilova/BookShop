using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Sliders
{
    public class SliderCreateVM
    {
        [Required]
        public IEnumerable<IFormFile> NewImages { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
