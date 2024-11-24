namespace WebsiteGenZ.Models.ViewModels
{
public class CartViewModel
{
    public string UserId { get; set; }

    public List<CartItemModel> CartItems { get; set; }

    public IEnumerable<ProductModel> Products { get; set; } 

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10; 
}
}