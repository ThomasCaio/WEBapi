using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
        public DataContext _dataContext;

        public TodoServiceTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;;
            _dataContext = new DataContext(_options);
        }
    
        [Fact]
        public void Add_ShouldAddTodoItem()
        {
            // Arrange
            using (_dataContext)
            {
                TodoItem item = new TodoItem { Title = "Test Item", IsCompleted = false };

                // Act
                _dataContext.TodoItems.Add(item);
                _dataContext.SaveChanges();

                // Assert
                var addedItem = _dataContext.TodoItems.FirstOrDefault(i => i.Title == "Test Item");
                Assert.NotNull(addedItem);
                Assert.Equal("Test Item", addedItem.Title);
                Assert.False(addedItem.IsCompleted);
                Assert.Equal(1, addedItem.Id);
            }
        }

        [Fact]
        public void GetById_ShouldReturnCorrectItem()
        {
            // Arrange
            using (_dataContext)
            {
                var service = new TodoService(_dataContext);
                var item = new TodoItem { Title = "Test Correct Item ID", IsCompleted = false };
                _dataContext.TodoItems.Add(item);
                _dataContext.SaveChanges();

                // Act
                var foundItem = service.GetById(item.Id);

                // Assert
                Assert.NotNull(foundItem);
                Assert.Equal("Test Correct Item ID", foundItem.Title);
            }
        }

        [Fact]
        public void Update_ShouldModifyExistingItem()
        {
            // Arrange
            using (_dataContext)
            {
                var service = new TodoService(_dataContext);
                var item = new TodoItem { Title = "Test Item", IsCompleted = false };
                _dataContext.TodoItems.Add(item);
                _dataContext.SaveChanges();

                item.Title = "Updated Item";
                item.IsCompleted = true;

                // Act
                _dataContext.Update(item);
                var foundItem = service.GetById(item.Id);

                // Assert
                Assert.NotNull(foundItem);
                Assert.Equal("Updated Item", foundItem.Title);
                Assert.True(foundItem.IsCompleted);
            }
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            // Arrange
            using (_dataContext)
            {
                var service = new TodoService(_dataContext);
                var item = new TodoItem { Title = "Test Item", IsCompleted = false };
                _dataContext.TodoItems.Add(item);
                _dataContext.SaveChanges();

                // Act
                service.Delete(1);
                var foundItem = service.GetById(1);

                // Assert
                Assert.Null(foundItem);
            }
        }
    }
}