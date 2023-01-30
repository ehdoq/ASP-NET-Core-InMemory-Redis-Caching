using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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

        await _distributedCache.SetStringAsync("product:1", productSerialize, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        });

        return View();
    }

    public async Task<IActionResult> Show()
    {
        var product = await _distributedCache.GetStringAsync("product:1");
        ViewBag.product = JsonSerializer.Deserialize<Product>(product);
        return View();
    }

    public async Task<IActionResult> Remove()
    {
        await _distributedCache.RemoveAsync("product:1");
        return View();
    }
}
