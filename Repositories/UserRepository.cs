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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext) 
        {
            
        }

        public async Task<User> FindUserByEmail(string email)
        {
            await _context.Database.EnsureCreatedAsync();

            return _context.Set<User>().Where(entity => entity.Email == email).FirstOrDefault();
        }
    }
}
