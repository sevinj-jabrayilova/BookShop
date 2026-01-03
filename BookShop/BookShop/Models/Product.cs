namespace BookShop.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int SellCount { get; set; }
        public string Description { get; set; }
        //public int CategoryId { get; set; }
        //public Category Category { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
