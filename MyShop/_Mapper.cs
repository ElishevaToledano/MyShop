using AutoMapper;
using dto;
using Entity;
namespace MyShop
{
    public class _Mapper : Profile
    {
        public _Mapper()
        {
            CreateMap<User, userDTO>();
            CreateMap< userDTO,User>();
            CreateMap<RegisterUserDTO, User>();
            CreateMap<User, RegisterUserDTO>();
            CreateMap<Product, productDTO>();
            CreateMap<Category, categoryDTO>();
            CreateMap<Order, orderDTO>();
            CreateMap< orderDTO, Order>();
            CreateMap<addOrderDTO, Order>();
            CreateMap<orderItemsDTO, OrderItem>();
            CreateMap<OrderItem, orderItemsDTO>();
        }
    }
}
