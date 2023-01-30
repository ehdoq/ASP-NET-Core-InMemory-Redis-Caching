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
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                //SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"key: {key} -> value: {value} -> sebep: {reason} -> {state}");
            });

            _memoryCache.Set("zaman", DateTime.Now.ToString(), options);
        }

        return View();
    }

    public IActionResult Show()
    {
        _memoryCache.TryGetValue("zaman", out string zamanCache);
        _memoryCache.TryGetValue("callback", out string callback);

        ViewBag.zaman = zamanCache;
        ViewBag.callback = callback;

        return View();
    }
}
