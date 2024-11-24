using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteGenZ.Models
{
    public class WishlistItem
    {

        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public CartModel Cart { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

    }
}
