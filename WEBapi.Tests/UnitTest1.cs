using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBapi.Context;
using WEBapi.Controllers;
using WEBapi.Models;
using Xunit;
using System.Linq;

namespace WEBapi.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void Post_ValidUser_ReturnsOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DataContext(options))
            {
                var controller = new UserController(context);
                var user = new User { Name = "Test User"};

                // Act
                var result = controller.Post(user);

                // Assert
                Assert.IsType<OkResult>(result);
                Assert.Single(context.Users);
                Assert.Equal("Test User", context.Users.First().Name);
            }
        }

        [Fact]
        public void Get_UsersExist_ReturnsListOfUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DataContext(options))
            {
                context.Users.Add(new User { Name = "User 1"});
                context.Users.Add(new User { Name = "User 2"});
                context.SaveChanges();

                var controller = new UserController(context);

                // Act
                var result = controller.Get();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
                Assert.Equal(3, users.Count());
                Assert.Contains(users, u => u.Name == "User 1");
                Assert.Contains(users, u => u.Name == "User 2");
            }
        }

        [Fact]
        public void Get_NoUsersExist_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DataContext(options))
            {
                var controller = new UserController(context);

                // Act
                var result = controller.Get();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
                Assert.Empty(users);
            }
        }

    }
}