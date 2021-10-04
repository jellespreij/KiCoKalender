using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityBase, new()
    {
        public CosmosDBContext _context;
        public BaseRepository(CosmosDBContext context)
        {
            _context = context;
        }

        public T GetSingle(long id)
        {
            return _context.Set<T>().Where(entity => entity.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }


        public async void Add(T entity)
        {
            await _context.Database.EnsureCreatedAsync();

            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();
        }

        public async void Delete(T entity)
        {
            await _context.Database.EnsureCreatedAsync();

            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async void Update(T entity)
        {
            await _context.Database.EnsureCreatedAsync();

            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
