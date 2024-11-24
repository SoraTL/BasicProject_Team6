using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class ImageModel
    {
        
        [Key]
        public string Id { get; set; }


        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }


        [Required(ErrorMessage = "Vui lòng cung cấp URL cho hình ảnh.")]
        public string Image { get; set; } = "noimage.jpg";


    }
}
