using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using service;
using Entity;
using AutoMapper;
using dto;
using System.Text.Json;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IDistributedCache _cache;

        public ProductsController(IProductService productService, IMapper mapper, IDistributedCache cache)
        {
            _productService = productService;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<List<productDTO>>> Get([FromQuery] string? desc, [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] int?[] categoryIds)
        {
            string cacheKey = $"products-{desc}-{minPrice}-{maxPrice}-{string.Join("-", categoryIds ?? Array.Empty<int?>())}";
            var cachedProducts = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedProducts))
            {
                var productsDTO = JsonSerializer.Deserialize<List<productDTO>>(cachedProducts);
                return Ok(productsDTO);
            }

            var products = await _productService.GetAllProducts(desc, minPrice, maxPrice, categoryIds);
            var productsDTOMapped = _mapper.Map<List<Product>, List<productDTO>>(products);

            var serializedData = JsonSerializer.Serialize(productsDTOMapped);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, serializedData, options);

            return Ok(productsDTOMapped);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<productDTO>> Get(int id)
        {
            string cacheKey = $"product-{id}";
            var cachedProduct = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedProduct))
            {
                var productDTO = JsonSerializer.Deserialize<productDTO>(cachedProduct);
                return Ok(productDTO);
            }

            var product = await _productService.GetProductbyId(id);
            if (product == null)
                return NotFound();

            var productDTOMapped = _mapper.Map<Product, productDTO>(product);

            var serializedData = JsonSerializer.Serialize(productDTOMapped);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, serializedData, options);

            return Ok(productDTOMapped);
        }
    }
}
