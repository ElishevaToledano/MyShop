using Entity;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests;
using Xunit;
namespace Test
{
    public class UserRepositoryIntegrationTests:IClassFixture<DatabaseFixure>
    {
        private readonly ApiOrmContext _context;
        private readonly UserRepository _reposetory;

        public UserRepositoryIntegrationTests(DatabaseFixure fixture)
        {
            _context = fixture.Context; 
            _reposetory = new UserRepository(_context);
        }

        [Fact]
        public async Task Get_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { UserName = "test@example.com", Password = "password123##", FirstName = "John", LastName = "Doe" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act

            var retrievedUser = await _reposetory.GetUserById(user.UserId); 

           // var retrievedUser = await _context.Users.FindAsync(user.Id);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.UserName, retrievedUser.UserName);
            Assert.Equal(user.FirstName, retrievedUser.FirstName);
            Assert.Equal(user.LastName, retrievedUser.LastName);
        }

        [Fact]
        public async Task Get_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            //var retrievedUser = await _context.Users.FindAsync(-1); // מזהה לא קיים
            var retrievedUser = await _reposetory.GetUserById(-1);
            // Assert
            Assert.Null(retrievedUser);
        }

        [Fact]
        public async Task Post_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var user = new User { UserName = "newuser@example.com", Password = "password123##" };

            // Act
            // var addedUser = await _context.Users.AddAsync(user);
             var addedUser = await _reposetory.AddUser(user);


            //await _context.SaveChangesAsync();

            // Assert
            Assert.NotNull(addedUser);
            Assert.Equal(user.UserName, addedUser.UserName);
            Assert.True(addedUser.UserId > 0); // נניח שהמזהה יוקצה אוטומטית
        }

        [Fact]
        public async Task Login_ShouldReturnUser_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User { UserName = "testuser@example.com", Password = "password123##" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Act 

            var loggedInUser =await _reposetory.LogIn("testuser@example.com", "password123##");
            // Assert
            Assert.NotNull(loggedInUser);
            Assert.Equal(user.UserName, loggedInUser.UserName.Trim());
            Assert.Equal(user.Password, loggedInUser.Password.Trim());
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Act
            var loggedInUser =  await _reposetory.LogIn("Ttestuser@example.com", "password123##");

            // Assert
            Assert.Null(loggedInUser);
        }

    }
}



