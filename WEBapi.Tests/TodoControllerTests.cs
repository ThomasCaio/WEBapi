using Microsoft.AspNetCore.Mvc;
using WEBapi.Controllers;
using WEBapi.Models;
using WEBapi.Services;
using Xunit;
using Moq;
using System.Collections.Generic;

namespace WEBapi.Tests.Controllers
{
    public class TodoControllerTests
    {
        private DbContextOptions<DataContext> _options;
        public DataContext _dataContext;

        public TodoControllerTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;;
            _dataContext = new DataContext(_options);
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithTodoItems()
        {
            // Arrange
            var todoItems = new List<TodoItem> { new TodoItem { Title = "Test" } };

            // Act
            var result = _dataContext.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Single(returnedItems);
        }

        [Fact]
        public void GetById_ExistingId_ShouldReturnOkWithTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Test" };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(todoItem);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItem = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(1, returnedItem.Id);
        }

        [Fact]
        public void GetById_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(1)).Returns((TodoItem)null);

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
            var createdItem = new TodoItem { Id = 1, Title = "Test" };
            _mockTodoService.Setup(service => service.Add(newItem)).Returns(createdItem);

            // Act
            var result = _controller.Add(newItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TodoController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            var returnedItem = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(1, returnedItem.Id);
        }

        [Fact]
        public void Update_MatchingIds_ShouldReturnNoContent()
        {
            // Arrange
            var updatedItem = new TodoItem { Id = 1, Title = "Updated" };
            _mockTodoService.Setup(service => service.Update(updatedItem));

            // Act
            var result = _controller.Update(1, updatedItem);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_NonMatchingIds_ShouldReturnBadRequest()
        {
            // Arrange
            var updatedItem = new TodoItem { Id = 2, Title = "Updated" };

            // Act
            var result = _controller.Update(1, updatedItem);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent()
        {
            // Arrange
            _mockTodoService.Setup(service => service.Delete(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}