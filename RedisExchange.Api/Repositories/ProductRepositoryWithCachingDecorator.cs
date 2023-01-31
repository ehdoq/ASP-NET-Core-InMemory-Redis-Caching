using RedisExchange.Api.Models;
using RedisExchange.Cache.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExchange.Api.Repositories;

public class ProductRepositoryWithCachingDecorator : IProductRepository
{
    private const string productCache = "productCache";
    private readonly IProductRepository _productRepository;
    private readonly RedisService _redisService;
    private readonly IDatabase _cacheRepository;

    public ProductRepositoryWithCachingDecorator(IProductRepository productRepository, RedisService redisService)
    {
        _productRepository = productRepository;
        _redisService = redisService;
        _cacheRepository = _redisService.GetDb(1);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        if (!await _cacheRepository.KeyExistsAsync(productCache))
            return await LoadToCacheFromDatabaseAsync();

        var products = new List<Product>();
        var cache = await _cacheRepository.HashGetAllAsync(productCache);

        foreach (var item in cache.ToList())
        {
            var product = JsonSerializer.Deserialize<Product>(item.Value);
            products.Add(product);
        }

        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        if (await _cacheRepository.KeyExistsAsync(productCache))
        {
            var product = await _cacheRepository.HashGetAsync(productCache, id);
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
        }

        var products = await LoadToCacheFromDatabaseAsync();

        return products.FirstOrDefault(product => product.Id == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var newProduct = await _productRepository.CreateAsync(product);

        if (await _cacheRepository.KeyExistsAsync(productCache))
            await _cacheRepository.HashSetAsync(productCache, product.Id, JsonSerializer.Serialize<Product>(product));

        return newProduct;
    }

    private async Task<List<Product>> LoadToCacheFromDatabaseAsync()
    {
        var products = await _productRepository.GetAllAsync();

        products.ForEach(product =>
        {
            _cacheRepository.HashSetAsync(productCache, product.Id, JsonSerializer.Serialize(product));
        });

        return products;
    }
}