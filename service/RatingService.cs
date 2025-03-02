using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class RatingService : IRatingService
    {
        IRatingRepository ratingRepository;


        public RatingService(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }
        public async Task<Rating> AddRating(Rating rating)
        {
            return await ratingRepository.AddRating(rating);
        }
    }
}
