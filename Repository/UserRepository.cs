using Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiOrmContext _context;

        public UserRepository(ApiOrmContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(int id, User userToUpdate)
        {
            userToUpdate.UserId = id;
            _context.Users.Update(userToUpdate);
            await _context.SaveChangesAsync();
            return userToUpdate;
        }

        public async Task<User?> LogIn(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
