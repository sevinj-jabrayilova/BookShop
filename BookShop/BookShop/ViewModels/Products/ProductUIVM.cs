namespace BookShop.ViewModels.Products
{
    public class ProductUIVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        //public int CategoryId { get; set; }
       public IEnumerable<int> CategoryIds { get; set; }
    }
}
