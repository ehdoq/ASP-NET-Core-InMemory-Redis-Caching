using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace InMemoryApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IMemoryCache _memoryCache;

    public ProductsController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public IActionResult Index()
    {
        //1.Yol = Key'in memory'de olup olmadığıyla ilgili kontrol. 
        /*if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            _memoryCache.Set("zaman", DateTime.Now.ToString());*/
        
        //2.Yol
        if(!_memoryCache.TryGetValue("zaman", out string zamanCache))
        {
            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };

            _memoryCache.Set("zaman", DateTime.Now.ToString(), options);
        }

        return View();
    }

    public IActionResult Show()
    {
        _memoryCache.TryGetValue("zaman", out string zamanCache);

        ViewBag.zaman = zamanCache;

        return View();
    }
}
