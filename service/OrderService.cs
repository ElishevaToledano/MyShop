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
        IProductRepository _productsRepository;
        public OrderService(IOrderRepository orderRepository, IProductRepository productsRepository)
        {
            this.orderRepository = orderRepository;
            this._productsRepository= productsRepository;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await orderRepository.GetOrderById(id);
        }

        public async Task<Order> AddOrder(Order order)
        {
            if (!await CheckSum(order))
                return null;
            return await orderRepository.AddOrder(order);
        }

        private async Task<bool> CheckSum(Order order)
        {
            List<Product> products = await _productsRepository.GetAllProducts(null, null, null, []);
            decimal? amount = 0;
            foreach (var item in order.OrderItems)
            {
                amount += products.Find(product => product.ProductId == item.ProductId).Price;
            }
            return amount == order.OrderSum;
        }
    }
}

