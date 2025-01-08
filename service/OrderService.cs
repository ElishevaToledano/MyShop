using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class OrderService : IOrderService
    {

        IOrderRepository orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await orderRepository.GetOrderById(id);

        }

        public async Task<Order> AddOrder(Order order)
        {

            return await orderRepository.AddOrder(order);

        }



    }
}

