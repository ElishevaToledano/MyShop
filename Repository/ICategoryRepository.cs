using Entity;

namespace Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
    }
}