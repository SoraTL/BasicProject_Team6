using Nest;
using WebsiteGenZ.Models;

namespace WebsiteGenZ.Services.elasticsearch
{
    public class ElasticSearchRepository
    {
        private readonly ElasticClient _client;

        public ElasticSearchRepository(ElasticClient client)
        {
            _client = client;
        }

        // Kiểm tra Index đã tồn tại chưa
        public async Task<bool> IndexExistsAsync(string indexName)
        {
            var response = await _client.Indices.ExistsAsync(indexName);
            return response.Exists;
        }

        // Tạo Index
        public async Task CreateIndexAsync(string indexName)
        {
            if (await IndexExistsAsync(indexName))
            {
                Console.WriteLine($"Index '{indexName}' already exists.");
                return;
            }

            var response = await _client.Indices.CreateAsync(indexName, c => c
                .Map<ProductModel>(m => m.AutoMap()) // Tự động map model Product
            );

            if (response.IsValid)
            {
                Console.WriteLine($"Index '{indexName}' created successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to create index '{indexName}': {response.ServerError?.Error.Reason}");
            }
        }

        // Xóa Index
        public async Task DeleteIndexAsync(string indexName)
        {
            var response = await _client.Indices.DeleteAsync(indexName);

            if (response.IsValid)
            {
                Console.WriteLine($"Index '{indexName}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete index '{indexName}': {response.ServerError?.Error.Reason}");
            }
        }

        // Thêm Document vào Index
        public async Task IndexDocumentAsync(string indexName, ProductModel product)
        {
            if (!await IndexExistsAsync(indexName))
            {
                Console.WriteLine($"Index '{indexName}' does not exist. Creating index...");
                await CreateIndexAsync(indexName);
            }

            var response = await _client.IndexAsync(product, idx => idx.Index(indexName));

            if (response.IsValid)
            {
                Console.WriteLine("Document indexed successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to index document: {response.ServerError?.Error.Reason}");
            }
        }

        // Lấy Document theo ID
        public async Task<ProductModel> GetDocumentByIdAsync(string indexName, string documentId)
        {
            var response = await _client.GetAsync<ProductModel>(documentId, g => g.Index(indexName));

            if (response.IsValid)
            {
                return response.Source;
            }
            else
            {
                Console.WriteLine($"Failed to get document: {response.ServerError?.Error.Reason}");
                return null;
            }
        }

        // Xóa Document theo ID
        public async Task DeleteDocumentAsync(string indexName, string documentId)
        {
            var response = await _client.DeleteAsync<ProductModel>(documentId, d => d.Index(indexName));

            if (response.IsValid)
            {
                Console.WriteLine($"Document with ID '{documentId}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete document: {response.ServerError?.Error.Reason}");
            }
        }

        // Tìm kiếm Document
        public async Task<List<ProductModel>> SearchDocumentsAsync(string indexName, string searchQuery)
        {
            var response = await _client.SearchAsync<ProductModel>(s => s
                .Index(indexName)
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(searchQuery)
                        .Fields(f => f
                            .Field(p => p.Name)
                            .Field(p => p.Description)
                        )
                    )
                )
            );

            if (response.IsValid)
            {
                return response.Documents.ToList();
            }
            else
            {
                Console.WriteLine($"Search failed: {response.ServerError?.Error.Reason}");
                return new List<ProductModel>();
            }
        }
    }
}
