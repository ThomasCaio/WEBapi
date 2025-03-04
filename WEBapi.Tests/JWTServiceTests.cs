using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using Moq;

public class JwtServiceTests
{
    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c["Jwt:Key"]).Returns("A2F6Y2Q4NzEtNjY3Zi00ZWEyLTk5ZmItYjY2YmI5N2E2ZTY2"); // Substitua por uma chave válida
        configuration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        configuration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        var jwtService = new JwtService(configuration.Object);
        var username = "testuser";
        var role = "TestRole";

        // Act
        var tokenString = jwtService.GenerateToken(username, role);

        // Assert
        Assert.NotNull(tokenString);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        Assert.NotNull(token);
        Assert.Equal("TestIssuer", token.Issuer);
        Assert.Equal("TestAudience", token.Audiences.FirstOrDefault());

        var nameClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        var roleClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        Assert.NotNull(nameClaim);
        Assert.Equal(username, nameClaim.Value);
        Assert.NotNull(roleClaim);
        Assert.Equal(role, roleClaim.Value);

        Assert.True(token.ValidTo > DateTime.UtcNow); //Verifica se o tempo de expiração é maior que o tempo atual
    }

    [Fact]
    public void GenerateToken_InvalidKey_ShouldThrowException()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c["Jwt:Key"]).Returns("shortKey"); // Chave inválida
        configuration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        configuration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        var jwtService = new JwtService(configuration.Object);
        var username = "testuser";
        var role = "TestRole";

        //Act & Assert
         Assert.Throws<ArgumentOutOfRangeException>(() => jwtService.GenerateToken(username, role));
    }
}