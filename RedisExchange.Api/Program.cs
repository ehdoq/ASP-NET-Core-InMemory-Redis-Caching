using Microsoft.EntityFrameworkCore;
using RedisExchange.Api.Models;
using RedisExchange.Api.Repositories;
using RedisExchange.Api.Services;
using RedisExchange.Cache.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("MyDatabase");
});

builder.Services.AddScoped<IProductRepository>(sp =>
{
    var appDbContext = sp.GetRequiredService<AppDbContext>();

    var productRepository = new ProductRepository(appDbContext);

    var redisService = sp.GetRequiredService<RedisService>();

    return new ProductRepositoryWithCachingDecorator(productRepository, redisService);
});

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

var app = builder.Build();

using (var scoped = app.Services.CreateScope())
{
    var dbContext = scoped.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
