using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Context;
namespace WEBapi.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext DbContext;

    public UserController(DataContext context)
    {
        DbContext = context;
    }


    [HttpPost()]
    public IActionResult Post(User request)
    {
        DbContext.Users.Add(request);
        DbContext.SaveChanges();
        return Ok();
    }

    [HttpGet()]
    public IActionResult Get()
    {
        var result = DbContext.Users.ToList();
        return Ok(result);
    }
}
