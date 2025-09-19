using apitank.DTOs;
using apitank.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace apitank.Services
{
    public class ProductService : IProductService
    {
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public ProductService(IDistributedCache cache,
            IMapper mapper)
        {
            _cache = cache;
            _mapper = mapper;
        }
        public async Task SetProductAsync(string key, string value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(key, value, options);
        }
        public async Task<ProductDto?> GetProductAsync(int id)
        {
            string cacheKey = $"product_{id}";
            var cachedProduct = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedProduct))
            {
                var productDto = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(cachedProduct);
                productDto.Response = "Fetching from cache...";
                return productDto;
            }
            var products = await GetProductsFromDbAsync(id);
            var product = products.FirstOrDefault();
            if (product != null)
            {
                await SetProductAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(product));
            }
            Console.WriteLine("Fetching from db...");
            var productDtoDb = product.Adapt<ProductDto>();
            productDtoDb.Response = "Fetching from db...";
            productDtoDb.Key = cacheKey;
            return productDtoDb;
        }
        public async Task RemoveProductAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
        public async Task<IEnumerable<Product>> GetProductsFromDbAsync(int id = 0)
        {
            // Simulate database access with a delay
            await Task.Delay(100); // Simulate async database call
            var products = new List<ProductDto>();
            //seed with fixed ids for testing
            for(int i = 0; i < 999; i++)
            {
                products.Add(new ProductDto { Id = i, Name = $"Product {i+4}", Price = (i+4) * 10.0m, CreatedAt = DateTime.UtcNow ,Key = $"product_{i}"});
            }
            if (id > 0)
            {
                return products.Where(p => p.Id == id).Adapt<List<Product>>();
            }
            return products.Adapt<List<Product>>();
        }
    }
    public interface IProductService
    {
        Task<ProductDto?> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsFromDbAsync(int id = 0);
        Task RemoveProductAsync(string key);
        Task SetProductAsync(string key, string value);
    }
}
