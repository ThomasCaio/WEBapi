using System.Collections.Generic;
using System.Linq;
using WEBapi.Models;
using WEBapi.Context;
using Microsoft.EntityFrameworkCore;

namespace WEBapi.Services
{
    public interface ITodoService
    {
        List<TodoItem> GetAll();
        TodoItem? GetById(int id);
        TodoItem Add(TodoItem item);
        void Update(TodoItem item);
        void Delete(int id);
    }

    public class TodoService : ITodoService
    {
        private readonly DataContext _dbContext;

        public TodoService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TodoItem> GetAll()
        {
            return _dbContext.TodoItems.ToList();
        }

        public TodoItem? GetById(int id)
        {
            return _dbContext.TodoItems.Find(id);
        }

        public TodoItem Add(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _dbContext.TodoItems.Add(item);
            _dbContext.SaveChanges();
            return item;
        }

        public void Update(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var existingItem = _dbContext.TodoItems.Find(item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.IsCompleted = item.IsCompleted;
                _dbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var item = _dbContext.TodoItems.Find(id);
            if (item != null)
            {
                _dbContext.TodoItems.Remove(item);
                _dbContext.SaveChanges();
            }
        }
    }
}