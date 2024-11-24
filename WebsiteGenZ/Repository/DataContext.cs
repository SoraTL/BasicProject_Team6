using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteGenZ.Models;

namespace WebsiteGenZ.Repository
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Định nghĩa các DbSet (bảng)
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<ColorModel> Colors { get; set; }
        public DbSet<Product_ColorModel> ProductColors { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<ShippingMethodModel> ShippingMethods { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ Product - Brand
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Product - Category
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ nhiều-nhiều Product - Color (Product_ColorModel)
            modelBuilder.Entity<Product_ColorModel>()
                .HasKey(pc => new { pc.ProductId, pc.ColorId });

            modelBuilder.Entity<Product_ColorModel>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<Product_ColorModel>()
                .HasOne(pc => pc.Color)
                .WithMany(c => c.ProductColors)
                .HasForeignKey(pc => pc.ColorId);

            // Quan hệ Order - OrderDetails
            modelBuilder.Entity<OrderDetailsModel>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ OrderDetails - Product
            modelBuilder.Entity<OrderDetailsModel>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Cart - User
            modelBuilder.Entity<CartModel>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ Cart - CartItems
            modelBuilder.Entity<CartModel>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ CartItem - Product
            modelBuilder.Entity<CartItemModel>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
