using Entity;

namespace Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds);
        Task<Product> GetProductbyId(int id);
    }
}