using Xunit;
using Moq;
using WEBapi.Controllers;
using WEBapi.Services;
using WEBapi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WEBapi.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var dataServiceMock = new Mock<IDataService<User>>();
            var controller = new UserController(dataServiceMock.Object);
            var user = new User { Username = "TestUser" };

            dataServiceMock.Setup(service => service.GetAll()).Returns(new List<User>());
            dataServiceMock.Setup(service => service.Add(user));

            // Act
            var result = controller.Register(user) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("User registered successfully.", result.Value);
            dataServiceMock.Verify(service => service.Add(user), Times.Once);
        }

        [Fact]
        public void Register_ExistingUsername_ReturnsBadRequest()
        {
            // Arrange
            var dataServiceMock = new Mock<IDataService<User>>();
            var controller = new UserController(dataServiceMock.Object);
            var user = new User { Username = "ExistingUser" };
            var existingUsers = new List<User> { new User { Username = "ExistingUser" } };

            dataServiceMock.Setup(service => service.GetAll()).Returns(existingUsers);

            // Act
            var result = controller.Register(user) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Username already exists.", result.Value);
            dataServiceMock.Verify(service => service.Add(user), Times.Never);
        }

        [Fact]
        public void Register_DbUpdateException_ReturnsInternalServerError()
        {
            // Arrange
            var dataServiceMock = new Mock<IDataService<User>>();
            var controller = new UserController(dataServiceMock.Object);
            var user = new User { Username = "TestUser" };

            dataServiceMock.Setup(service => service.GetAll()).Returns(new List<User>());
            dataServiceMock.Setup(service => service.Add(user)).Throws(new DbUpdateException("Test Exception", new Exception()));

            // Act
            var result = controller.Register(user) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Database error:", result.Value.ToString());
            dataServiceMock.Verify(service => service.Add(user), Times.Once);
        }

        [Fact]
        public void Register_GenericException_ReturnsInternalServerError()
        {
            // Arrange
            var dataServiceMock = new Mock<IDataService<User>>();
            var controller = new UserController(dataServiceMock.Object);
            var user = new User { Username = "TestUser" };

            dataServiceMock.Setup(service => service.GetAll()).Returns(new List<User>());
            dataServiceMock.Setup(service => service.Add(user)).Throws(new Exception("Test Exception"));

            // Act
            var result = controller.Register(user) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error:", result.Value.ToString());
            dataServiceMock.Verify(service => service.Add(user), Times.Once);
        }
    }
}