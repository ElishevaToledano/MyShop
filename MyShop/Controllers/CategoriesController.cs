using Microsoft.AspNetCore.Mvc;
using service;
using Entity;
using AutoMapper;
using dto;
using Microsoft.Extensions.Caching.Memory;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryService CategoryService;
        IMapper _mapper;
        IMemoryCache _memoryCache;
        public CategoriesController(ICategoryService categoryService, IMapper mapper, IMemoryCache memoryCache)
        {
            CategoryService = categoryService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<List<categoryDTO>>> Get()
        {
            if (!_memoryCache.TryGetValue("categories", out List<Category> categories))
            {
                categories = await CategoryService.GetAllCategories();
                _memoryCache.Set("categories", categories, TimeSpan.FromMinutes(30));
            }
            //List<Category> categories = await CategoryService.GetAllCategories();
            List<categoryDTO> categoriesDTO = _mapper.Map<List<Category>, List<categoryDTO>>(categories);
            return Ok(categoriesDTO);
            //return await service.GetCategories();
        }


    }
}

