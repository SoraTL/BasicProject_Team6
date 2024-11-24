using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class OrderDetailsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }

        [Required]
        public string OrderCode { get; set; } 

        [Required]
        public string UserName { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; } 
        public ProductModel Product { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
    }
}
