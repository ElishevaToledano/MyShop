using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        ApiOrmContext _ApiOrmContext;
        public OrderRepository(ApiOrmContext ApiOrmContext)
        {
            _ApiOrmContext = ApiOrmContext;
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _ApiOrmContext.Orders.Include(u => u.User).Include(o => o.OrderItems)
                .FirstOrDefaultAsync(order => order.OrderId == id);

        }
        public async Task<Order> AddOrder(Order order)
        {
            await _ApiOrmContext.Orders.AddAsync(order);
            await _ApiOrmContext.SaveChangesAsync();
            return order;
        }

    }
}

