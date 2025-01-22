using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RatingRepository : IRatingRepository
    {

        ApiOrmContext _ApiOrmContext;
        public RatingRepository(ApiOrmContext ApiOrmContext)
        {
            _ApiOrmContext = ApiOrmContext;
        }

        public async Task AddRaiting(Rating ratingObj)
        {
            await _ApiOrmContext.Ratings.AddAsync(ratingObj);
            await _ApiOrmContext.SaveChangesAsync();
        }

    }



}


