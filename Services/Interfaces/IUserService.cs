using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        User FindUserByUserId(Guid userId);
        User AddUser(User user);
        User UpdateUser(User user, Guid id);
        User DeleteUser(Guid id);
        User FindUserByEmail(string email);
    }
}
