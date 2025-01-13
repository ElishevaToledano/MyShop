using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository

    {

        ApiOrmContext _ApiOrmContext;
        public ProductRepository(ApiOrmContext ApiOrmContext)
        {
            _ApiOrmContext = ApiOrmContext;
        }



        public async Task<List<Product>> GetAllProducts(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds)
        {

            var query = _ApiOrmContext.Products.Where(Product =>
            (desc == null ? (true) : (Product.ProductName.Contains(desc)))
        && ((minPrice == null) ? (true) : (Product.Price >= minPrice))
        && ((maxPrice == null) ? (true) : (Product.Price <= maxPrice))
        && ((categoryIds.Length == 0) ? (true) : (categoryIds.Contains(Product.CategoryId))))
        .OrderBy(Product => Product.Price).Include(p => p.Category);
            Console.WriteLine(query.ToQueryString());
            List<Product> products = await query.ToListAsync();
            return products;

        }


    }
}

