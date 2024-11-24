using System.ComponentModel.DataAnnotations;

namespace WebsiteGenZ.Models
{
    public class ColorModel
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ColorCode {  get; set; }

        public List<Product_ColorModel> ProductColors { get; set; }

    }
}
