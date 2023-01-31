﻿using RedisExchange.Api.Models;
using RedisExchange.Api.Repositories;

namespace RedisExchange.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }
}