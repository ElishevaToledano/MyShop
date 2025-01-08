using Entity;

namespace service
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
    }
}