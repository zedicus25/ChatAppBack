using DataAccessEF.Data;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories
{
    public class GenericRepo<T> : IGenericRepository<T> where T : class
    {
        protected readonly ChatDbContext _dbContext;

        public GenericRepo(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Add(T item) => _dbContext.Set<T>().Add(item);


        public virtual void Delete(int id)
        {
            var itemForDelete = _dbContext.Set<T>().Find(id);
            _dbContext.Set<T>().Remove(itemForDelete);
        }

        public virtual async Task<IEnumerable<T>> GetAll() => await _dbContext.Set<T>().ToListAsync();


        public virtual async Task<T> GetById(int id) => await _dbContext.Set<T>().FindAsync(id);

        public virtual void Update(T item) => _dbContext.Set<T>().Update(item);
    }
}
