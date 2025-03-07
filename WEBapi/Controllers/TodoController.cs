using Microsoft.AspNetCore.Mvc;
using WEBapi.Models;
using WEBapi.Services;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var items = _dataService.GetAll();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _dataService.GetById(id);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
            try
            {
                var newItem = _dataService.Add(item);
                return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
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

        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem item)
        {
            try
            {
                if (id != item.Id)
                {
                    return BadRequest();
                }

                _dataService.Update(item);
                return NoContent();
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _dataService.Delete(id);
                return NoContent();
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
    }
}