using Microsoft.AspNetCore.Mvc;
using service;
using Entity;
using AutoMapper;
using dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryService CategoryService;
        IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            CategoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            List<Category> categories = await CategoryService.GetAllCategories();
            List<categoryDTO> categoriesDTO = _mapper.Map<List<Category>, List<categoryDTO>>(categories);
            return Ok(categoriesDTO);
            //return await service.GetCategories();
        }


    }
}
