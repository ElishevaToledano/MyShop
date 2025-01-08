using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        ApiOrmContext _ApiOrmContext;
        public CategoryRepository(ApiOrmContext ApiOrmContext)
        {
            _ApiOrmContext = ApiOrmContext;
        }


        public async Task<List<Category>> GetAllCategories()
        {
            return await _ApiOrmContext.Categories.ToListAsync();

        }


    }
}
