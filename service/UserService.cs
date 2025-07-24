using Entity;
using Repository;
using System.Text.Json;
using System.Security.Cryptography;

namespace service
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;

        public UserService(IUserRepository UserRepository)
        {
            userRepository = UserRepository;
        }
        public async Task<User> GetUserById(int id)
        {
            return await userRepository.GetUserById(id);
        }

        public string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            }
        }

        private string CreatePasswordField(string salt, string hash)
        {
            return $"{salt}:{hash}";
        }

        private (string salt, string hash) ParsePasswordField(string passwordField)
        {
            var parts = passwordField.Split(':');
            if (parts.Length != 2)
                return (null, null);
            return (parts[0], parts[1]);
        }

        public async Task<User?> AddUser(User user)
        {
            var existingUser = await userRepository.LogIn(user.UserName);
            if (existingUser != null)
            {
                throw new Repository.Exceptions.UserAlreadyExistsException($"User with username '{user.UserName}' already exists.");
            }

            if (CheckPassword(user.Password) > 2) 
            {
                string salt = GenerateSalt(16); 
                string hash = HashPassword(user.Password, salt, 10000, 32);
                user.PasswordSalt = salt;
                user.Password = hash;
                return await userRepository.AddUser(user);
            }
            return null;
        }

        public async Task<User?> UpdateUser(int id, User userToUpdate)
        {
            if (string.IsNullOrWhiteSpace(userToUpdate.Password))
                return null;

            if (userToUpdate.Password.Contains(":"))
                return null;

            if (CheckPassword(userToUpdate.Password) > 2)
            {
                string salt = GenerateSalt(16);
                string hash = HashPassword(userToUpdate.Password!, salt, 10000, 32);
                userToUpdate.Password = CreatePasswordField(salt, hash);
                return await userRepository.UpdateUser(id, userToUpdate);
            }
            return null;
        }

        public async Task<User?> LogIn(string userName, string password)
        {
            var user = await userRepository.LogIn(userName);
            if (user == null)
                return null;
            if (string.IsNullOrEmpty(user.PasswordSalt) || string.IsNullOrEmpty(user.Password))
                return null;
            string attemptHash = HashPassword(password, user.PasswordSalt, 10000, 32);
            if (attemptHash == user.Password)
                return user;
            return null;
        }

        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;
        }
    }
}
