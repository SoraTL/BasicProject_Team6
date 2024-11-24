using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteGenZ.Models;
using WebsiteGenZ.Models.ViewModels;
using WebsiteGenZ.Repository;

namespace WebsiteGenZ.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    [Authorize(Roles = "ADMIN")]
    public class HomeAdminController : Controller
    {
        private DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeAdminController(DataContext dataContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _signInManager = signInManager;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        

        [Route("product")]
        public IActionResult Product()
        {
            var products = _dataContext.Products.ToList();
            return View(products);
        }

        [Route("AddNewProduct")]
        public IActionResult AddNewProduct()
        {
            // Lấy danh sách Brand và Category từ cơ sở dữ liệu
            ViewBag.BrandId = new SelectList(_dataContext.Brands.ToList(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_dataContext.Categories.ToList(), "Id", "Name");

            // Trả về view AddNewProduct
            return View(new ProductModel()); // Truyền model rỗng để sử dụng cho binding
        }

        [HttpPost]
        [Route("AddNewProduct")]
        [ValidateAntiForgeryToken] 
        public IActionResult AddNewProduct(ProductModel product)
        {
            ModelState.Remove("Brand");
            ModelState.Remove("Category");
            ModelState.Remove("ProductColors");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    Console.WriteLine($"Key: {entry.Key}");
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
                ViewBag.BrandId = new SelectList(_dataContext.Brands.ToList(), "Id", "Name");
                ViewBag.CategoryId = new SelectList(_dataContext.Categories.ToList(), "Id", "Name");

                return View(product);
            }

            _dataContext.Products.Add(product);
            _dataContext.SaveChanges();

            return RedirectToAction("Product");
        }

        [Route("product-image")]
        public IActionResult ProductImage()
        {
            var images = _dataContext.Images.ToList();

            foreach (var image in images) 
            {
                image.Product = _dataContext.Products
                                     .FirstOrDefault(p => p.Id == image.ProductId);
            }


            return View(images);
        }

        [Route("AddNewProductImage")]
        public IActionResult AddNewProductImage()
        {
            ViewBag.ProductId = new SelectList(_dataContext.Products.ToList(), "Id", "Name");
            return View(new ImageModel());
        }

        //[Route("AddNewProductImage")]
        //public IActionResult AddNewProductImage(ImageModel image)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var entry in ModelState)
        //        {
        //            Console.WriteLine($"Key: {entry.Key}");
        //            foreach (var error in entry.Value.Errors)
        //            {
        //                Console.WriteLine($"Error: {error.ErrorMessage}");
        //            }
        //        }
        //        ViewBag.ProductId = new SelectList(_dataContext.Products.ToList(), "Id", "Name");

        //        return View(image);
        //    }

        //    _dataContext.Images.Add(image);
        //    _dataContext.SaveChanges();

        //    return RedirectToAction("Product");
        //}


        [Route("account")]
        public IActionResult Account()
        {
            var accounts = _userManager.Users.ToList();

            return View(accounts);
        }

        [Route("AddNewAccount")]
        public IActionResult AddNewAccount()
        {
            return View();
        }

        [HttpPost]
        [Route("AddNewAccount")]
        public async Task<IActionResult> AddNewAccount(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Hiển thị lỗi nếu model không hợp lệ
                foreach (var entry in ModelState)
                {
                    Console.WriteLine($"Key: {entry.Key}");
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }

                // Gửi danh sách Brands vào ViewBag để tạo dropdown
                ViewBag.Role = new SelectList(_dataContext.Brands.ToList(), "Id", "Name");

                return View(model);
            }

            // Tạo đối tượng AppUser từ RegisterViewModel
            var user = new AppUser
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Thêm người dùng vào Identity
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Nếu tạo tài khoản thành công, chuyển hướng tới trang danh sách tài khoản (hoặc một trang khác)
                return RedirectToAction("Account");
            }

            // Nếu có lỗi, hiển thị thông báo lỗi
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Nếu thêm không thành công, trả về lại View với các lỗi
            return View(model);
        }

        [Route("brand")]
        public IActionResult Brand()
        {
            var brands = _dataContext.Brands.ToList();

            return View(brands);
        }

        [Route("category")]
        public IActionResult Category()
        {
            var categories = _dataContext.Categories.ToList();
            return View(categories);
        }

    }
}
