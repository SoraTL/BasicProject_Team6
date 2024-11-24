namespace WebsiteGenZ.Models.ViewModels
{
    public class HomeViewModel
    {

        public List<ProductModel> BrandProducts { get; set; }

        public List<BrandModel> Brands { get; set; }

        public List<ProductModel> DiscountProducts { get; set; }
        public string ? SearchQuery { get; set; }

    }
}
