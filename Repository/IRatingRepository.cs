using Entity;

namespace Repository
{
    public interface IRatingRepository
    {
        Task AddRaiting(Rating ratingObj);
    }
}