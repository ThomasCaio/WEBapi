using System;
using System.Collections.Generic;
using System.Linq;
using WEBapi.Context;
using Microsoft.EntityFrameworkCore;

namespace WEBapi.Services
{
    public interface IDataService<T> where T : class
    {
        List<T> GetAll();
        T? GetById(int id);
        T Add(T item);
        void Update(T item);
        void Delete(int id);
    }

    public class DataService<T> : IDataService<T> where T : class
    {
        private readonly DataContext _dbContext;

        public DataService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T? GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _dbContext.Set<T>().Add(item);
            _dbContext.SaveChanges();
            return item;
        }

        public void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _dbContext.Set<T>().Update(item);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _dbContext.Set<T>().Find(id);
            if (item != null)
            {
                _dbContext.Set<T>().Remove(item);
                _dbContext.SaveChanges();
            }
        }
    }
}