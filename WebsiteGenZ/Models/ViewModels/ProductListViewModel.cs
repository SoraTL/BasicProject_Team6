namespace WebsiteGenZ.Models.ViewModels
{
    public class ProductListViewModel
    {
        public string Category { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<BrandModel> Brands { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 12;

        // Giá trị filter
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public List<int> SelectedBrandIds { get; set; }
        public string? SearchQuery { get; set; }
    }

}
