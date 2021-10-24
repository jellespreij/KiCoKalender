using Context;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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

        public T GetSingle(Guid id)
        {
            return _context.Set<T>().Where(entity => entity.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task<T> Add(T entity)
        {
            await _context.Database.EnsureCreatedAsync();
            _context.Set<T>().Add(entity);

            Commit();

            return entity;
        }

        public async Task<T> Delete(Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            var itemToDelete = _context.Set<T>().Where(entity => entity.Id == id).FirstOrDefault();

            if(itemToDelete is not null)
            {
                EntityEntry dbEntityEntry = _context.Entry(itemToDelete);
                dbEntityEntry.State = EntityState.Deleted;
                Commit();
            }

            return itemToDelete;
        }

        public async Task<T> Update(T entity, Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            var itemToUpdate = _context.Set<T>().Where(entity => entity.Id == id).FirstOrDefault();

            if (itemToUpdate is not null)
            {
                _context.Entry(itemToUpdate).CurrentValues.SetValues(entity);
                Commit();
            }

            return itemToUpdate;
        }

        public async void Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
