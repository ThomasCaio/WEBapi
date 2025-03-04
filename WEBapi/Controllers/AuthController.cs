using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Context;
using Microsoft.AspNetCore.Authorization;
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
    private readonly DataContext _dataContext;

    public AuthController(JwtService _, DataContext __)
    {
        _jwtService = _;
        _dataContext = __;
    }


    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        // Valide as credenciais do usuário (exemplo simplificado)
        if (request.Username == "string" && request.Password == "string")
        {
            var token = _jwtService.GenerateToken(request.Username, "Admin");
            return Ok(new { Token = $"bearer {token}" });
        }

        return Unauthorized();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult ListAllUsers()
    {
        var result = _dataContext.Users.ToList();
        return Ok(result);
    }
}
