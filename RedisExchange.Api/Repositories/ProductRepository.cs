using Microsoft.EntityFrameworkCore;
using RedisExchange.Api.Models;

namespace RedisExchange.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _appDbContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _appDbContext.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();

        return product;
    }    
}