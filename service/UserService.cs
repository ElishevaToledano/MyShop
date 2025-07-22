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

        // יצירת סולט אקראי
        public string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // יצירת Hash עם סולט ועיבוד חוזר (Rfc2898)
        public string HashPassword(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            }
        }

        // פונקציה לבניית מחרוזת Password שמכילה סולט והאש (מופרדים ב־:)
        private string CreatePasswordField(string salt, string hash)
        {
            return $"{salt}:{hash}";
        }

        // פונקציה לפירוק מחרוזת Password לשני חלקים - סולט והאש
        private (string salt, string hash) ParsePasswordField(string passwordField)
        {
            var parts = passwordField.Split(':');
            if (parts.Length != 2)
                return (null, null);
            return (parts[0], parts[1]);
        }

        public async Task<User?> AddUser(User user)
        {
            // נוודא שהסיסמה היא לא null או ריקה
            if (string.IsNullOrWhiteSpace(user.Password))
                return null;

            // אם הסיסמה כבר בפורמט salt:hash - אז היא מוצפנת, לא נרצה להריץ עליה ZXCVBN
            if (user.Password.Contains(":"))
                return null;  // או throw exception שמתאר שהסיסמה כבר מוצפנת

            if (CheckPassword(user.Password) > 2)  // כאן בטוח שמפעילים ZXCVBN על סיסמה גולמית
            {
                string salt = GenerateSalt(16);
                string hash = HashPassword(user.Password!, salt, 10000, 32);
                user.Password = CreatePasswordField(salt, hash);
                return await userRepository.AddUser(user);
            }
            return null;
        }

        public async Task<User?> UpdateUser(int id, User userToUpdate)
        {
            if (string.IsNullOrWhiteSpace(userToUpdate.Password))
                return null;

            if (userToUpdate.Password.Contains(":"))
                return null;  // לא ניתן לעדכן סיסמה שכבר מוצפנת

            if (CheckPassword(userToUpdate.Password) > 2)
            {
                string salt = GenerateSalt(16);
                string hash = HashPassword(userToUpdate.Password!, salt, 10000, 32);
                userToUpdate.Password = CreatePasswordField(salt, hash);
                return await userRepository.UpdateUser(id, userToUpdate);
            }
            return null;
        }


        // התחברות - מקבל את השדה Password, מפצל לסולט והאש, ומשווה
        public async Task<User> LogIn(string userName, string password)
        {
            var user = await userRepository.LogIn(userName);
            if (user == null)
                return null;

            var (salt, storedHash) = ParsePasswordField(user.Password);
            if (salt == null || storedHash == null)
                return null;

            string attemptHash = HashPassword(password, salt, 10000, 32);

            if (attemptHash == storedHash)
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


//using Entity;
//using Repository;
//using System.Security.Cryptography;

//namespace service
//{
//    public class UserService : IUserService
//    {
//        private readonly IUserRepository userRepository;

//        public UserService(IUserRepository userRepository)
//        {
//            this.userRepository = userRepository;
//        }

//        public async Task<User> GetUserById(int id)
//        {
//            return await userRepository.GetUserById(id);
//        }

//        public string GenerateSalt(int length)
//        {
//            var saltBytes = new byte[length];
//            using (var provider = new RNGCryptoServiceProvider())
//            {
//                provider.GetNonZeroBytes(saltBytes);
//            }
//            return Convert.ToBase64String(saltBytes);
//        }

//        public string HashPassword(string password, string salt, int iterations, int hashLength)
//        {
//            var saltBytes = Convert.FromBase64String(salt);
//            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations))
//            {
//                return Convert.ToBase64String(deriveBytes.GetBytes(hashLength));
//            }
//        }

//        private string CreatePasswordField(string salt, string hash) =>
//            $"{salt}:{hash}";

//        private (string? salt, string? hash) ParsePasswordField(string? passwordField)
//        {
//            if (string.IsNullOrWhiteSpace(passwordField))
//                return (null, null);

//            var parts = passwordField.Split(':');
//            if (parts.Length != 2)
//                return (null, null);

//            return (parts[0], parts[1]);
//        }

//        public async Task<User?> AddUser(User user)
//        {
//            if (CheckPassword(user.Password) > 2)
//            {
//                string salt = GenerateSalt(16);
//                string hash = HashPassword(user.Password!, salt, 10000, 32);
//                user.Password = CreatePasswordField(salt, hash);
//                return await userRepository.AddUser(user);
//            }
//            return null;
//        }

//        public async Task<User?> UpdateUser(int id, User userToUpdate)
//        {
//            if (CheckPassword(userToUpdate.Password) > 2)
//            {
//                string salt = GenerateSalt(16);
//                string hash = HashPassword(userToUpdate.Password!, salt, 10000, 32);
//                userToUpdate.Password = CreatePasswordField(salt, hash);
//                return await userRepository.UpdateUser(id, userToUpdate);
//            }
//            return null;
//        }

//        public async Task<User?> LogIn(string userName, string password)
//        {
//            var user = await userRepository.LogIn(userName);
//            if (user == null || string.IsNullOrEmpty(user.Password))
//                return null;

//            var (salt, storedHash) = ParsePasswordField(user.Password);
//            if (salt == null || storedHash == null)
//            {
//                // fallback – סיסמה לא הוצפנה (משתמש ישן)
//                return user.Password == password ? user : null;
//            }

//            string attemptHash = HashPassword(password, salt, 10000, 32);
//            return attemptHash == storedHash ? user : null;
//        }

//        public int CheckPassword(string password)
//        {
//            var result = Zxcvbn.Core.EvaluatePassword(password);
//            return result.Score;
//        }
//    }
//}
