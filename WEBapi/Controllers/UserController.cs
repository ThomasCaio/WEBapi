using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Context;
using WEBapi.Services;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WEBapi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IDataService<User> _dataService;

    public UserController(IDataService<User> dataService)
    {
        _dataService = dataService;
    }


    [HttpPost("register")]
    public IActionResult Register(User request)
    {
        try
            {
                if (_dataService.GetAll().Any(u => u.Username == request.Username))
                {
                    return BadRequest("Username already exists.");
                }
                _dataService.Add(request);

                return Ok("User registered successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
    }

    [HttpGet()]
    public IActionResult Get()
    {
        var result = _dataService.GetAll().Select(u => u.ToPublic());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _dataService.GetById(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list-users")]
    public IActionResult ListAllUsers()
    {
        var result = _dataService.GetAll();
        return Ok(result);
    }
}
