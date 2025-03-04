using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Context;
using WEBapi.Services;
namespace WEBapi.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IDataService<User> _dataService;

    public UserController(IDataService<User> dataService)
    {
        _dataService = dataService;
    }


    [HttpPost()]
    public IActionResult Post(User request)
    {
        _dataService.Add(request);
        return Ok();
    }

    [HttpGet()]
    public IActionResult Get()
    {
        var result = _dataService.GetAll();
        return Ok(result);
    }
}
