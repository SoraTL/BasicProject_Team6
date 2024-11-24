using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteGenZ.Models
{
    public class Wishlist
    {

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public List<WishlistItem> WishlistItems { get; set; }

        public Wishlist()
        {
            WishlistItems = new List<WishlistItem>();
        }

    }
}
