using Entity;

namespace service
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds);
        Task<Product> GetProductbyId(int id);
    }
}