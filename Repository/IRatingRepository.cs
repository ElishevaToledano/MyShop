using Entity;

namespace Repository
{
    public interface IRatingRepository
    {
        Task<Rating> AddRating(Rating rating);
    }
}