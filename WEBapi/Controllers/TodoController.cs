using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Services;
// using WEBapi.Context;

namespace WEBapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IDataService<TodoItem> _dataService;

        public TodoController(IDataService<TodoItem> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _dataService.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _dataService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
            var newItem = _dataService.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _dataService.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _dataService.Delete(id);
            return NoContent();
        }
    }
}