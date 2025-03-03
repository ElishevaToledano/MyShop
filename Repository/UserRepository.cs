using Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        ApiOrmContext _ApiOrmContext;
        public UserRepository(ApiOrmContext ApiOrmContext)
        {
            _ApiOrmContext = ApiOrmContext;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _ApiOrmContext.Users.Include(u => u.Orders).FirstOrDefaultAsync(user => user.UserId == id);
        }

        public async Task<User> AddUser(User user)
        {
            await _ApiOrmContext.Users.AddAsync(user);
            await _ApiOrmContext.SaveChangesAsync();
            return user;

        }
        public async Task<User> UpdateUser(int id, User userToUpdate)
        {
            userToUpdate.UserId = id;
             _ApiOrmContext.Users.Update(userToUpdate);
            await _ApiOrmContext.SaveChangesAsync();
            return userToUpdate;

        }

        public async Task<User> LogIn(string userName, string password)
        {
             return  await _ApiOrmContext.Users.FirstOrDefaultAsync(user=> user.UserName== userName && user.Password== password);
             
        }



    }
}

       
    

