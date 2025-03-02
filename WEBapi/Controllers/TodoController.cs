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
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _todoService.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _todoService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
            var newItem = _todoService.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _todoService.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _todoService.Delete(id);
            return NoContent();
        }
    }
}