namespace WebsiteGenZ.Models.ViewModels
{
    public class ProductDetailsViewModel
    {

        public ProductModel product { get; set; }

        public List<ImageModel> images { get; set; }

        public List<ColorModel> colors { get; set; }

        public List<SubInfoModel> subInfos { get; set; }

    }
}
