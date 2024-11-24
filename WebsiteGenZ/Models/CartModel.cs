using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public List<CartItemModel> CartItems { get; set; }

        public CartModel()
        {
            CartItems = new List<CartItemModel>();
        }

    }

}
