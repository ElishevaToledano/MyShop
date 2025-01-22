using Entity;
using Microsoft.EntityFrameworkCore;
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
        IRatingService ratingService;
        public RatingService(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        public async Task AddRaiting(Rating ratingObj)
        {
            return await RatingRepository.AddRating(ratingObj);
        }
    }
}
