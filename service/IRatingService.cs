using Entity;

namespace service
{
    public interface IRatingService
    {
        Task AddRaiting(Rating ratingObj);
    }
}