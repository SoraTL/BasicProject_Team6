using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Threading.Tasks;
using WebsiteGenZ.Models.ViewModels;
using WebsiteGenZ.Models;
using WebsiteGenZ.Repository;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace WebBanDoDienTu.Controllers
{
    public class CategoryController : Controller
    {   

        private DataContext _dataContext;
        private ProductRepository _productRepository;

        public CategoryController(DataContext dataContext, ProductRepository productRepository)
        {

            _dataContext = dataContext;
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index(string ?searchQuery, decimal? fromPrice, decimal? toPrice, List<int> selectedBrandIds, int page = 1)
        {
            const int pageSize = 12;

            // Lấy danh sách sản phẩm
            var query = _dataContext.Products.AsQueryable();

            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.Name.Contains(searchQuery));
            }

            // Lọc theo giá
            if (fromPrice.HasValue)
            {
                query = query.Where(p => p.Price >= fromPrice.Value);
            }
            if (toPrice.HasValue)
            {
                query = query.Where(p => p.Price <= toPrice.Value);
            }

            // Lọc theo thương hiệu
            if (selectedBrandIds != null && selectedBrandIds.Any())
            {
                query = query.Where(p => selectedBrandIds.Contains(p.BrandId));
            }

            // Phân trang
            var totalItems = query.Count();
            var products = query
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .Select(p => new ProductModel
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        BrandId = p.BrandId,
        Images = p.Images.Where(img => img.ProductId == p.Id).ToList() // Lấy danh sách URL hình ảnh
    })
    .ToList();

            // Chuẩn bị ViewModel
            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                FromPrice = fromPrice,
                ToPrice = toPrice,
                SelectedBrandIds = selectedBrandIds,
                SearchQuery = searchQuery,
                Categories = _dataContext.Categories.ToList(),
                Brands = _dataContext.Brands.ToList()
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SearchProduct(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            try
            {
                // Gọi repository để tìm kiếm
                var products = await _productRepository.SearchProductsAsync(query);

                // Trả về kết quả dưới dạng view
                return View(products);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về thông báo lỗi
                Console.WriteLine($"Search failed: {ex.Message}");
                return StatusCode(500, "An error occurred while searching for products.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductCountByBrand(int brandId)
        {
            var brandExists = await _dataContext.Brands.AnyAsync(b => b.Id == brandId);
            if (!brandExists)
            {
                return NotFound(new { message = "Brand not found" });
            }

            var productCount = await _dataContext.Products.CountAsync(p => p.BrandId == brandId);

            return Ok(new { brandId = brandId, productCount = productCount });
        }

    }


}
