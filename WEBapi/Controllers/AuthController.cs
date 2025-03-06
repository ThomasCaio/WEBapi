using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Services;

namespace WEBapi.Controllers;

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IDataService<User> _dataService;

    public AuthController(JwtService _, IDataService<User> __)
    {
        _jwtService = _;
        _dataService = __;
    }


    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        try
        {
            var existingUser = _dataService.GetAll().FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtService.GenerateToken(request.Username, "Admin");
            return Ok(new { Token = $"bearer {token}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
