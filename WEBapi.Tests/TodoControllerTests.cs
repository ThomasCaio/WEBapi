using Microsoft.AspNetCore.Mvc;
using WEBapi.Controllers;
using WEBapi.Models;
using WEBapi.Services;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace WEBapi.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly Mock<IDataService<TodoItem>> _mockDataService;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockDataService = new Mock<IDataService<TodoItem>>();
            _controller = new TodoController(_mockDataService.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithTodoItems()
        {
            // Arrange
            var items = new List<TodoItem> {
                new TodoItem { Id = 1, Title = "Item 1", IsCompleted = false },
                new TodoItem { Id = 2, Title = "Item 2", IsCompleted = false } 
            };
            _mockDataService.Setup(service => service.GetAll()).Returns(items);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Equal(2, Enumerable.Count(returnedItems));
        }

        [Fact]
        public void GetById_ExistingId_ShouldReturnOkWithTodoItem()
        {
            // Arrange
            var item = new TodoItem { Id = 1, Title = "Item 1" };
            _mockDataService.Setup(service => service.GetById(1)).Returns(item);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItem = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(1, returnedItem.Id);
            Assert.Equal("Item 1", returnedItem.Title);
        }

        [Fact]
        public void GetById_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            _mockDataService.Setup(service => service.GetById(1)).Returns((TodoItem)null!);

            // Act
            var result = _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Add_ShouldReturnCreatedAtActionWithNewTodoItem()
        {
            // Arrange
            var newItem = new TodoItem { Title = "New Item" };
            _mockDataService.Setup(service => service.Add(newItem)).Returns(new TodoItem { Id = 1, Title = "New Item" });

            // Act
            var result = _controller.Add(newItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TodoController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues!["id"]);
            var returnedItem = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(1, returnedItem.Id);
            Assert.Equal("New Item", returnedItem.Title);
        }

        [Fact]
        public void Update_MatchingIds_ShouldReturnNoContent()
        {
            // Arrange
            var item = new TodoItem { Id = 1, Title = "Updated Item" };

            // Act
            var result = _controller.Update(1, item);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockDataService.Verify(service => service.Update(item), Times.Once);
        }

        [Fact]
        public void Update_NonMatchingIds_ShouldReturnBadRequest()
        {
            // Arrange
            var item = new TodoItem { Id = 2, Title = "Updated Item" };

            // Act
            var result = _controller.Update(1, item);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            _mockDataService.Verify(service => service.Update(item), Times.Never);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent()
        {
            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockDataService.Verify(service => service.Delete(1), Times.Once);
        }
    }
}