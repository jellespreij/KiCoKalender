using Models;
using Repositories.Context;
using Repositories.Interfaces;
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

        public async Task<User> FindUser(Expression<Func<User, bool>> predicate)
        {
            await _context.Database.EnsureCreatedAsync();

            return _context.Users.FirstOrDefault(predicate);
        }
    }
}
