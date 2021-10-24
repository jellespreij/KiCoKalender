using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBaseRepository<T> where T : class, IEntityBase, new()
    {
        T GetSingle(Guid id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> Add(T item);
        Task<T> Update(T item, Guid id);
        Task<T> Delete(Guid id);
        void Commit();
    }
}
