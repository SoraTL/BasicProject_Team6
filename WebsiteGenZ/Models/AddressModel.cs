using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class AddressModel
    {

        [Key]
        public int Id { get; set; }

        public string PhoneNumber { get; set; }
        public string ReceiverName { get; set; }
        public int AddressType { get; set; }
        public string Address { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public bool IsDefault { get; set; }
    }
}
