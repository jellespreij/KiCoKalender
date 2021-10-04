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
        User FindUserById(long userId);
        void AddUser(User user);
        User FindUserByName(string name);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
