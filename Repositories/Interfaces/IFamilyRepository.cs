using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFamilyRepository : IBaseRepository<Family>
    {
        void AddUserToFamily(User user, Guid id);
        void AddFolderToFamily(Folder folder, Guid id);
        void RemoveUserFromFamily(User user);
    }
}
