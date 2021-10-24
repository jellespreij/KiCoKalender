using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAuthRepository : IBaseRepository<User>
    {
        Task<User> FindUser(Expression<Func<User, bool>> predicate);
    }
}
