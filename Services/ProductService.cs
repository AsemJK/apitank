using apitank.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace apitank.Services
{
    public class ProductService : IProductService
    {
        private readonly IDistributedCache _cache;

        public ProductService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task SetProductAsync(string key, string value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(key, value, options);
        }
        public async Task<Product?> GetProductAsync(int id)
        {
            string cacheKey = $"product_{id}";
            var cachedProduct = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedProduct))
            {
                Console.WriteLine("Fetching from Redis Cache...");
                return System.Text.Json.JsonSerializer.Deserialize<Product>(cachedProduct);
            }
            var products = await GetProductsFromDbAsync(id);
            var product = products.FirstOrDefault();
            if (product != null)
            {
                await SetProductAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(product));
            }
            Console.WriteLine("Fetching from db...");
            return product;
        }
        public async Task RemoveProductAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
        public async Task<IEnumerable<Product>> GetProductsFromDbAsync(int id = 0)
        {
            // Simulate database access with a delay
            await Task.Delay(100); // Simulate async database call
            var products = new List<Product>();
            //seed with fixed ids for testing
            for(int i = 0; i < 999999; i++)
            {
                products.Add(new Product { Id = i, Name = $"Product {i+4}", Price = (i+4) * 10.0m, CreatedAt = DateTime.UtcNow ,Key = $"product_{i}"});
            }
            if (id > 0)
            {
                return products.Where(p => p.Id == id);
            }
            return products;
        }
    }
    public interface IProductService
    {
        Task<Product?> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsFromDbAsync(int id = 0);
        Task RemoveProductAsync(string key);
        Task SetProductAsync(string key, string value);
    }
}
