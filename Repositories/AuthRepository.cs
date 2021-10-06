using Context;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AuthRepository : BaseRepository<User>, IAuthRepository
    {
        public AuthRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext)
        {

        }

        public User FindUser(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.FirstOrDefault(predicate);
        }
    }
}
