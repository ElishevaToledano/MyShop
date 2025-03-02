using Entity;

namespace service
{
    public interface IRatingService
    {
        Task<Rating> AddRating(Rating rating);
    }
}