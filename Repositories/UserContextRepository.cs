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
    public class UserContextRepository : BaseRepository<UserContext>, IUserContextRepository
    {
        public UserContextRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext) 
        {
            
        }
    }
}
