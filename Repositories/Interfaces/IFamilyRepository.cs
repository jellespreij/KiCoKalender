using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFamilyRepository : IBaseRepository<Family>
    {
        Task<Family> AddUserToFamily(User user, Guid id);
        void AddFolderToFamily(Folder folder, Guid id);
        void RemoveUserFromFamily(User user);
    }
}
