using BookShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        //public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ProductCategory>()
        //        .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        //    modelBuilder.Entity<ProductCategory>()
        //        .HasOne(pc => pc.Product)
        //        .WithMany(p => p.ProductCategories)
        //        .HasForeignKey(pc => pc.ProductId);

        //    modelBuilder.Entity<ProductCategory>()
        //        .HasOne(pc => pc.Category)
        //        .WithMany(c => c.ProductCategories)
        //        .HasForeignKey(pc => pc.CategoryId);

        //    base.OnModelCreating(modelBuilder);
        //}



    }
}
