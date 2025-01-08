using AutoMapper;
using dto;
using Entity;
using Microsoft.AspNetCore.Mvc;
using service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IOrderService OrderService;
        IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            OrderService = orderService;
            _mapper = mapper; 
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order order = await OrderService.GetOrderById(id);
            orderDTO orderDTO = _mapper.Map<Order, orderDTO>(order);
            return Ok(orderDTO);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] addOrderDTO order)
        {
            Order neworder= _mapper.Map<addOrderDTO, Order>(order);
            Order newOrder = await OrderService.AddOrder(neworder);
            return newOrder != null? Ok(): Unauthorized();
        }

    }
}
