using Models;
using Repositories.Context;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext)
        {

        }
    }
}
