using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên Thương Hiệu ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả Thương Hiệu ")]
        public string Description { get; set; }

        public string BrandIcon { get; set; }

        public int Status { get; set; }

        [NotMapped]
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        [NotMapped]
        public int TotalProducts { get; set; }
    }
}
