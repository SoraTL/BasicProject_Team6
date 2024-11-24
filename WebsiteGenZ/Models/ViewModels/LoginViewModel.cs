using System.ComponentModel.DataAnnotations;


namespace WebsiteGenZ.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập UserName")]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập Password")]
        public string Password { get; set; }

        // Thuộc tính RememberMe để cho phép người dùng chọn tùy chọn ghi nhớ đăng nhập
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
