namespace WebsiteGenZ.Models
{
    public class ShippingMethodModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; } 
        public int EstimatedDeliveryDays { get; set; }
    }

}
