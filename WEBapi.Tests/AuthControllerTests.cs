using Xunit;
using WEBapi.Controllers;
using WEBapi.Models;
using WEBapi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;

namespace WEBapi.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IDataService<User>> _mockDataService;
        private readonly JwtService _jwtService; // Use JwtService diretamente

        public AuthControllerTests()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["Jwt:Key"]).Returns("A2F6Y2Q4NzEtNjY3Zi00ZWEyLTk5ZmItYjY2YmI5N2E2ZTY2");
            configuration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            configuration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _jwtService = new JwtService(configuration.Object); // Instancie JwtService diretamente
            _mockDataService = new Mock<IDataService<User>>();
            _controller = new AuthController(_jwtService, _mockDataService.Object); // Passe JwtService diretamente
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "password" };
            var user = new User { Username = "testuser", Password = "password" };
            var users = new List<User> { user };

            _mockDataService.Setup(service => service.GetAll()).Returns(users);

            // Act
            var result = _controller.Login(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value); // Garante que o Value não é nulo
            Assert.Contains("bearer ", result.Value.ToString()); // Verifica se a string contém "bearer "
            _mockDataService.Verify(service => service.GetAll(), Times.Once);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "wrongpassword" };
            var users = new List<User> { new User { Username = "testuser", Password = "password" } };

            _mockDataService.Setup(service => service.GetAll()).Returns(users);

            // Act
            var result = _controller.Login(request) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            _mockDataService.Verify(service => service.GetAll(), Times.Once);
        }

        [Fact]
        public void Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "password" };

            _mockDataService.Setup(service => service.GetAll()).Throws(new Exception("Test exception"));

            // Act
            var result = _controller.Login(request) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Internal server error:", result.Value.ToString());
            _mockDataService.Verify(service => service.GetAll(), Times.Once);
        }
    }
}