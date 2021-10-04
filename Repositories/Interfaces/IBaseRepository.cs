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
        T GetSingle(long id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
