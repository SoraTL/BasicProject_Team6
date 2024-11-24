using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WebsiteGenZ.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
        [MaxLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả sản phẩm.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int quantity { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu.")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        public int CategoryId { get; set; }

        [ForeignKey("BrandId")]
        public BrandModel Brand { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
  

        public List<Product_ColorModel> ProductColors { get; set; }
        public List<ImageModel> Images { get; set; }
    }

}
