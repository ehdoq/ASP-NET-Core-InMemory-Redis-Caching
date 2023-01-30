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

    public IActionResult Index()
    {
        _distributedCache.SetString("name", "Mustafa", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        });
        return View();
    }

    public IActionResult Show()
    {
        ViewBag.name = _distributedCache?.GetString("name")?.ToString();
        return View();
    }

    public IActionResult Remove()
    {
        _distributedCache?.Remove("name");
        return View();
    }
}
