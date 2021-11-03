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
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext)
        {
           
        }
    }
}
