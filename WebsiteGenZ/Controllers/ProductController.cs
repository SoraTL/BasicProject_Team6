using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteGenZ.Models;
using WebsiteGenZ.Models.ViewModels;
using WebsiteGenZ.Repository;
using FuzzySharp;

namespace WebsiteGenZ.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }
         
        public async Task<IActionResult> Search(string searchTerm)
        {   
            var viewModel = new ProductListViewModel
            {
                Products = new List<ProductModel>(),
                Categories = await _dataContext.Categories.ToListAsync(),
                Brands = await _dataContext.Brands.ToListAsync(),
                CurrentPage = 1,
                TotalPages = 1,
                PageSize = 12
            };

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ViewBag.Message = "Vui lòng nhập từ khóa để tìm kiếm.";
                return View(viewModel);
            }

            searchTerm = searchTerm.Trim().ToLower();

            var productResults = await _dataContext.Products
                .FromSqlRaw("SELECT * FROM Products WHERE FREETEXT(Name, {0}) OR FREETEXT(Description, {0})", searchTerm)
                .ToListAsync();

            if (productResults.Count == 0)
            {
                productResults = await _dataContext.Products.ToListAsync();
                productResults = productResults
                    .Where(p => CalculateFuzzyScore(p, searchTerm) > 0) 
                    .OrderByDescending(p => CalculateFuzzyScore(p, searchTerm))
                    .ToList();
            }

            viewModel.Products = productResults;

            if (productResults.Count == 0)
            {
                ViewBag.Message = "Không tìm thấy sản phẩm nào.";
            }
            else
            {
                ViewBag.Keyword = searchTerm;
            }

            return View(viewModel);
        }

        // Phương thức tính toán điểm độ liên quan bằng Fuzzy Matching
        private double CalculateFuzzyScore(ProductModel product, string searchTerm)
        {
            double score = 0;

            // Chuẩn hóa tên và mô tả sản phẩm
            var normalizedProductName = product.Name?.ToLower().Trim() ?? "";
            var normalizedProductDescription = product.Description?.ToLower().Trim() ?? "";

            // Tính độ tương đồng giữa từ khóa và tên/mô tả sản phẩm
            int nameSimilarity = Fuzz.PartialRatio(normalizedProductName, searchTerm);
            int descriptionSimilarity = Fuzz.PartialRatio(normalizedProductDescription, searchTerm);

            // Đặt trọng số cho độ tương đồng
            if (nameSimilarity > 50) // Giảm ngưỡng để dễ tìm kiếm hơn
            {
                score += nameSimilarity * 0.02; // Trọng số cao hơn cho tên sản phẩm
            }

            if (descriptionSimilarity > 50)
            {
                score += descriptionSimilarity * 0.01; // Trọng số thấp hơn cho mô tả sản phẩm
            }

            return score;
        }

        public IActionResult Details(int? productId)
        {
            if (productId == null || productId <= 0)
            {
                return BadRequest("Product ID is required.");
            }

            var product = _dataContext.Products
                .FirstOrDefault(prd => prd.Id == productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var imgs = _dataContext.Images
                .Where(img => img.ProductId == productId)
                .ToList();

            var response = new ProductDetailsViewModel
            {
                product = product,
                images = imgs
            };

            return View(response);
        }
    }
}
