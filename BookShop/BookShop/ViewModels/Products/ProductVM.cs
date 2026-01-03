namespace BookShop.ViewModels.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string Image { get; set; }
    }
}
