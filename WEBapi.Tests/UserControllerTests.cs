using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBapi.Context;
using WEBapi.Controllers;
using WEBapi.Models;
using Xunit;
using System.Linq;
using WEBapi.Services;

namespace WEBapi.Tests.Controllers
{
    public class UserControllerTests
    {
        private DbContextOptions<DataContext> _options;
        private DataContext _dataContext;
        private UserController _controller;

        public UserControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dataContext = new DataContext(_options);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
            _controller = new UserController(new DataService<User>(_dataContext));
        }

        [Fact]
        public void Post_ValidUser_ReturnsOk()
        {
            // Arrange
            var user = new User { Name = "Test User" };

            // Act
            var result = _controller.Post(user);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Single(_dataContext.Users);
            Assert.Equal("Test User", _dataContext.Users.First().Name);
        }

        [Fact]
        public void Get_UsersExist_ReturnsListOfUsers()
        {
            // Arrange
            var user1 = new User { Name = "User 1" };
            var user2 = new User { Name = "User 2" };
            _controller.Post(user1);
            _controller.Post(user2);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void Get_NoUsersExist_ReturnsEmptyList()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Empty(users);
        }
    }
}