using Microsoft.AspNetCore.Mvc;
using RedisExchange.Api.Models;
using RedisExchange.Api.Services;

namespace RedisExchange.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _productService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        return Created(string.Empty, await _productService.CreateAsync(product));
    }
}