using WebsiteGenZ.Models;
using WebsiteGenZ.Services.elasticsearch;

namespace WebsiteGenZ.Repository
{
    public class ProductRepository
    {
        private readonly ElasticSearchRepository _elasticRepository;
        private readonly DataContext _dataContext;

        public ProductRepository(DataContext dataContext, ElasticSearchRepository elasticSearchRepository)
        {
            _dataContext = dataContext;
            _elasticRepository = elasticSearchRepository;
        }

        public async Task AddProduct(ProductModel product)
        {
            // Lưu sản phẩm vào database (ví dụ: Entity Framework)
            _dataContext.Products.Add(product);
            await _dataContext.SaveChangesAsync();

            // Index sản phẩm trong Elasticsearch
            await _elasticRepository.IndexDocumentAsync("products", product);
        }

        public async Task UpdateProduct(ProductModel product)
        {
            // Kiểm tra sự tồn tại của sản phẩm trong database
            var existingProduct = await _dataContext.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                Console.WriteLine($"Product with ID {product.Id} not found.");
                return;
            }

            // Cập nhật sản phẩm trong database
            _dataContext.Products.Update(product);
            await _dataContext.SaveChangesAsync();

            // Re-index sản phẩm trong Elasticsearch
            await _elasticRepository.IndexDocumentAsync("products", product);
        }

        public async Task DeleteProduct(int productId)
        {
            // Kiểm tra sự tồn tại của sản phẩm trong database
            var product = await _dataContext.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine($"Product with ID {productId} not found.");
                return;
            }

            // Xóa sản phẩm trong database
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();

            // Xóa sản phẩm khỏi Elasticsearch
            await _elasticRepository.DeleteDocumentAsync("products", product.Id.ToString());
        }

        public async Task<List<ProductModel>> SearchProductsAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                throw new ArgumentException("Search query cannot be null or empty.", nameof(searchQuery));
            }

            // Tìm kiếm trên Elasticsearch
            return await _elasticRepository.SearchDocumentsAsync("products", searchQuery);
        }
    }

}
