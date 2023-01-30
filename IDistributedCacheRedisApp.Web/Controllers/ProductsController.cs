using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IDistributedCache _distributedCache;

    public ProductsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<IActionResult> Index()
    {
        var product = new Product { Id = 1, Name = "Kalem", Price = 185.00M };
        var productSerialize = JsonSerializer.Serialize(product);
        var byteProduct = Encoding.UTF8.GetBytes(productSerialize);

        await _distributedCache.SetAsync("product:1", byteProduct, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        });

        return View();
    }

    public async Task<IActionResult> Show()
    {
        var byteProduct = await _distributedCache.GetAsync("product:1");
        var product = Encoding.UTF8.GetString(byteProduct);
        ViewBag.product = JsonSerializer.Deserialize<Product>(product);

        return View();
    }

    public async Task<IActionResult> Remove()
    {
        await _distributedCache.RemoveAsync("product:1");
        return View();
    }
}