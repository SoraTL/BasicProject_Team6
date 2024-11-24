using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteGenZ.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } 
        public AppUser User { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        public OrderStatus Status { get; set; }

        public List<OrderDetailsModel> OrderDetails { get; set; } = new List<OrderDetailsModel>();

        public int ShipmentId { get; set; }

        public ShippingMethodModel ShippingMethod { get; set; }


    }

    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}
