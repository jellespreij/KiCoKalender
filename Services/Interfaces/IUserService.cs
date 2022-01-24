using Microsoft.Azure.Functions.Worker;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        User FindUserByUserId(Guid userId);
        UserDTO FindUserDTOByUserId(Guid userId);
        User AddUser(User user);
        User UpdateUser(UserUpdateDTO userUpdate, Guid id, Guid currentUser);
        Task<User> DeleteUser(Guid id, Guid currentUser);
        User FindUserByEmail(string email);
        UserDTO FindUserDTOByEmail(string email);
    }
}
