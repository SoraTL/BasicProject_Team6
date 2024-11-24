using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteGenZ.Models;
using WebsiteGenZ.Models.ViewModels;
using WebsiteGenZ.Repository;

namespace WebsiteGenZ.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private  DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    ViewBag.FullName = $"{user.FirstName} {user.LastName}";
                }
            }

            var products = _dataContext.Products.ToList();
            if (products == null || !products.Any())
            {
                return View(null);
            }


            foreach (var product in products)
            {
                product.Images = _dataContext.Images
                                             .Where(img => img.ProductId == product.Id)
                                             .ToList();
            }

            var model = new HomeViewModel
            {   
                Brands = _dataContext.Brands.ToList(),
                BrandProducts = products
            };

            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
