using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebsiteGenZ.Models;

namespace WebsiteGenZ
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên danh mục")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tả Danh mục")]
        public string Description { get; set; }
        public int Status { get; set; }

        [NotMapped]
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        [NotMapped]
        public int TotalProducts { get; set; }
    }
}
