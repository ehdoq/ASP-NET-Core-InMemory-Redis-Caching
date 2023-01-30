using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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
        //if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
        //    _memoryCache.Set("zaman", DateTime.Now.ToString());
        
        //2.Yol
        if(!_memoryCache.TryGetValue("zaman", out string zamanCache))
            _memoryCache.Set("zaman", DateTime.Now.ToString());

        ViewBag.zaman = zamanCache;

        return View();
    }

    public IActionResult Show()
    {
        ViewBag.zaman = _memoryCache.GetOrCreate("zaman", entry =>
        {
            return DateTime.Now.ToString();
        });

        _memoryCache.Remove("zaman");

        return View();
    }
}
