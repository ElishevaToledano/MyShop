using AutoMapper;
using Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RatingRepository : IRatingRepository
    {
        ApiOrmContext manageDbContext;
        private readonly ILogger<UserRepository> _logger;

        public RatingRepository(IMapper mapper, ApiOrmContext manageDbContext, ILogger<UserRepository> logger)
        {
            this.manageDbContext = manageDbContext;
            _logger = logger;
        }
        public async Task<Rating> AddRating(Rating rating)
        {
            await manageDbContext.Ratings.AddAsync(rating);
            await manageDbContext.SaveChangesAsync();
            return rating;
            //string connaction = "Data Source=srv2\\pupils;Initial Catalog=AdeNetManage;Integrated Security=True;Pooling=False";
            //string query = "INSERT INTO RATING (HOST,METHOD,PATH,REFERE,USERAGENT,RECORDDATE)" +
            //    "VALUES (@Host,@Method,@Path,@Referer,@UserAgent,@RecordDate)";




            //using (SqlConnection cn = new SqlConnection(connaction))
            //using (SqlCommand cmd = new SqlCommand(query, cn))
            //{
            //    cmd.Parameters.Add("@Host", SqlDbType.Int).Value = rating.Host;
            //    cmd.Parameters.Add("@Method", SqlDbType.NChar, 20).Value = rating.Method;
            //    cmd.Parameters.Add("@Path", SqlDbType.NVarChar, int.MaxValue).Value = rating.Path;
            //    cmd.Parameters.Add("@Referer", SqlDbType.NVarChar, 50).Value = rating.Referer;
            //    cmd.Parameters.Add("@UserAgent", SqlDbType.NVarChar, 50).Value = rating.UserAgent;
            //    cmd.Parameters.Add("@RecordDate", SqlDbType.NVarChar, 50).Value = rating.RecordDate;

            //    cn.Open();
            //    int rowAffected = cmd.ExecuteNonQuery();
            //    cn.Close();




            //    return rating;

            //}
        }

    }
}
