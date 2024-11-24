using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class Product_ColorModel
    {
        [Key] // Khóa chính phức hợp sẽ được cấu hình trong Fluent API
        public int ProductId { get; set; } // Khóa ngoại đến bảng Product
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

        public int ColorId { get; set; } // Khóa ngoại đến bảng Color
        [ForeignKey("ColorId")]
        public ColorModel Color { get; set; }
    }

}
