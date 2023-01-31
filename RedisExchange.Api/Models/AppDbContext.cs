using Microsoft.EntityFrameworkCore;

namespace RedisExchange.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Kalem 1", Price = 1.2M },
            new Product { Id = 2, Name = "Kalem 2", Price = 1.1M },
            new Product { Id = 3, Name = "Kalem 3", Price = 1.0M }
        );

        base.OnModelCreating(modelBuilder);
    }
}