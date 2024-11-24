namespace WebsiteGenZ.Models.ViewModels
{
    public class CheckoutViewModel
    {

        public List<AddressModel>? Addresses { get; set; }
        public List<ShippingMethodModel> ShippingMethods { get; set; }
        public List<CartItemModel> CartItems { get; set; }
        public OrderModel Order { get; set; }
        public PaymentModel PaymentInfo { get; set; }
    }
}
