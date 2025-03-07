using System.Linq;
using Microsoft.EntityFrameworkCore;
using WEBapi.Models;
using WEBapi.Services;
using WEBapi.Context;
using Xunit;

namespace WEBapi.Tests
{
    public class TodoServiceTests
    {
        private DbContextOptions<DataContext> _options;
        private DataContext _dataContext;
        private DataService<TodoItem> _service;

        public TodoServiceTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dataContext = new DataContext(_options);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
            _service = new DataService<TodoItem>(_dataContext);
        }

        [Fact]
        public void Add_ShouldAddTodoItem()
        {
            // Arrange
            TodoItem item = new TodoItem { Title = "Test Add_ShouldAddTodoItem", IsCompleted = false };

            // Act
            _service.Add(item);

            // Assert
            var addedItem = _service.GetById(item.Id);
            Assert.NotNull(addedItem);
            Assert.Equal("Test Add_ShouldAddTodoItem", addedItem.Title);
            Assert.False(addedItem.IsCompleted);
            Assert.Equal(1, addedItem.Id);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectItem()
        {
            // Arrange
            var item = new TodoItem { Title = "Test Correct Item ID", IsCompleted = false };
            _service.Add(item);

            // Act
            var foundItem = _service.GetById(item.Id);

            // Assert
            Assert.NotNull(foundItem);
            Assert.Equal("Test Correct Item ID", foundItem.Title);
        }

        [Fact]
        public void Update_ShouldModifyExistingItem()
        {
            // Arrange
            var item = new TodoItem { Title = "Test Item", IsCompleted = false };
            _service.Add(item);

            item.Title = "Updated Item";
            item.IsCompleted = true;

            // Act
            _service.Update(item);

            // Assert
            var foundItem = _service.GetById(item.Id);
            Assert.NotNull(foundItem);
            Assert.Equal("Updated Item", foundItem.Title);
            Assert.True(foundItem.IsCompleted);
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            // Arrange
            var item = new TodoItem { Title = "Test Item", IsCompleted = false };
            _service.Add(item);

            // Act
            _service.Delete(item.Id);

            // Assert
            var foundItem = _service.GetById(item.Id);
            Assert.Null(foundItem);
        }
    }
}