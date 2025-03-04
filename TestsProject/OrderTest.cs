using Entity;
using Repository;
using Moq;
using Moq.EntityFrameworkCore;
using dto;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using service;


namespace TestProject1
{
    public class OrderTest
    {
        [Fact]
        public async void CheckOrderSum_ValidCredentialsReturnOrder()
        {
            var products = new List<Product>
        {
            new Product { ProductId = 1, Price = 40 },
            new Product { ProductId = 2, Price = 20 }
        };

            var orders = new List<Order>
        {
            new Order
            {
                UserId = 1,
                OrderSum = 100,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2 },
                    new OrderItem { ProductId = 2, Quantity = 1 }
                }
            }
        };

            var mockContext = new Mock<ApiOrmContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders);
            mockContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            var productRepository = new ProductRepository(mockContext.Object);
            var orderRepository = new OrderRepository(mockContext.Object);
            var mockLogger = new Mock<ILogger<OrderService>>();
            var orderService = new OrderService(orderRepository, productRepository, mockLogger.Object);

            var result = await orderService.AddOrder(orders[0]);
            Assert.Equal(result, orders[0]);
        }
        
        [Fact]
        public async Task CheckOrderSum_InvalidOrderSumUpdatesOrderSum()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product { ProductId = 1, Price = 40 },
        new Product { ProductId = 2, Price = 20 }
    };

            var invalidOrder = new Order
            {
                UserId = 1,
                OrderSum = 50, // סכום לא נכון
                OrderItems = new List<OrderItem>
        {
            new OrderItem { ProductId = 1, Quantity = 2 },
            new OrderItem { ProductId = 2, Quantity = 1 }
        }
            };

            var mockContext = new Mock<ApiOrmContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);
            mockContext.Setup(x => x.Orders).ReturnsDbSet(new List<Order>());
            mockContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.GetProductbyId(1)).ReturnsAsync(products[0]);
            productRepository.Setup(x => x.GetProductbyId(2)).ReturnsAsync(products[1]);

            var orderRepository = new OrderRepository(mockContext.Object);
            var mockLogger = new Mock<ILogger<OrderService>>();
            var orderService = new OrderService(orderRepository, productRepository.Object, mockLogger.Object);

            // Act
            var result = await orderService.AddOrder(invalidOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.OrderSum); // הסכום המעודכן צריך להיות 100
            Assert.Equal(invalidOrder.UserId, result.UserId);
            Assert.Equal(invalidOrder.OrderItems.Count, result.OrderItems.Count);
        }

    }
}
