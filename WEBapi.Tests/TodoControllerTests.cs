using Microsoft.AspNetCore.Mvc;
using WEBapi.Controllers;
using WEBapi.Models;
using WEBapi.Context;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WEBapi.Services;

namespace WEBapi.Tests.Controllers
{
    public class TodoControllerTests
    {
        private DbContextOptions<DataContext> _options;
        private DataContext _dataContext;
        private TodoController _controller;

        public TodoControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dataContext = new DataContext(_options);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
            _controller = new TodoController(new DataService<TodoItem>(_dataContext));
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithTodoItems()
        {
            // Arrange
            _controller.Add(new TodoItem { Title = "Test" });

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Single(returnedItems);
        }

        [Fact]
        public void GetById_ExistingId_ShouldReturnOkWithTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Test" };
            _controller.Add(todoItem);

            // Act
            var result = _controller.GetById(todoItem.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItem = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(todoItem.Id, returnedItem.Id);
        }

        [Fact]
        public void GetById_NonExistingId_ShouldReturnNotFound()
        {
            // Act
            var result = _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Add_ShouldReturnCreatedAtActionWithNewTodoItem()
        {
            // Arrange
            var newItem = new TodoItem { Title = "Test" };

            // Act
            var result = _controller.Add(newItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TodoController.GetById), createdAtActionResult.ActionName);
            var returnedItem = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(returnedItem.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(newItem.Title, returnedItem.Title);
        }

        [Fact]
        public void Update_MatchingIds_ShouldReturnNoContent()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Test" };
            _controller.Add(todoItem);
            todoItem.Title = "Updated";

            // Act
            var result = _controller.Update(todoItem.Id, todoItem);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_NonMatchingIds_ShouldReturnBadRequest()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Test" };
            _controller.Add(todoItem);
            var newItem = new TodoItem { Id = 2, Title = "Updated" };

            // Act
            var result = _controller.Update(newItem.Id, newItem);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Test" };
            _controller.Add(todoItem);

            // Act
            var result = _controller.Delete(todoItem.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}