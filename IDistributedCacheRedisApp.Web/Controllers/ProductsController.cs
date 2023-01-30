using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
        await _distributedCache.SetStringAsync("name", "Mustafa", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        });
        return View();
    }

    public async Task<IActionResult> Show()
    {
        ViewBag.name = await _distributedCache.GetStringAsync("name");
        return View();
    }

    public async Task<IActionResult> Remove()
    {
        await _distributedCache.RemoveAsync("name");
        return View();
    }
}
