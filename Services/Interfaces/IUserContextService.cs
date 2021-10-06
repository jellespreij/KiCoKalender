using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserContextService
    {
        UserContext FindUserContextByUserId(long userId);
        void AddUserContext(UserContext userContext);
        UserContext FindUserByName(string name);
        void UpdateUserContext(UserContext userContext);
        void DeleteUserContext(UserContext userContext);
    }
}
