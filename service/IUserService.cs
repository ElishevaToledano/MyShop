using Entity;

namespace service
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        int CheckPassword(string password);
        Task<User> GetUserById(int id);
        Task<User> LogIn(string userName, string password);
        Task<User> UpdateUser(int id, User userToUpdate);
    }
}