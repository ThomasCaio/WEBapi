using System.Linq;
using WEBapi.Models;
using WEBapi.Services;
using WEBapi.Context;
using Xunit;

namespace WEBapi.Tests
{
    public class TodoServiceTests
    {
        [Fact]
        public void Add_ShouldAddTodoItem()
        {
            // Arrange
            var service = new TodoService();
            var item = new TodoItem { Title = "Test Item", IsCompleted = false };

            // Act
            var addedItem = service.Add(item);

            // Assert
            Assert.NotNull(addedItem);
            Assert.Equal("Test Item", addedItem.Title);
            Assert.False(addedItem.IsCompleted);
            Assert.Equal(1, addedItem.Id);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectItem()
        {
            // Arrange
            var service = new TodoService();
            var item = new TodoItem { Title = "Test Item", IsCompleted = false };
            service.Add(item);

            // Act
            var foundItem = service.GetById(1);

            // Assert
            Assert.NotNull(foundItem);
            Assert.Equal("Test Item", foundItem.Title);
        }

        [Fact]
        public void Update_ShouldModifyExistingItem()
        {
            // Arrange
            var service = new TodoService();
            var item = new TodoItem { Title = "Test Item", IsCompleted = false };
            service.Add(item);

            var updatedItem = new TodoItem { Id = 1, Title = "Updated Item", IsCompleted = true };

            // Act
            service.Update(updatedItem);
            var foundItem = service.GetById(1);

            // Assert
            Assert.NotNull(foundItem);
            Assert.Equal("Updated Item", foundItem.Title);
            Assert.True(foundItem.IsCompleted);
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            // Arrange
            var service = new TodoService();
            var item = new TodoItem { Title = "Test Item", IsCompleted = false };
            service.Add(item);

            // Act
            service.Delete(1);
            var foundItem = service.GetById(1);

            // Assert
            Assert.Null(foundItem);
        }
    }
}