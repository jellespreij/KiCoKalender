using KiCoKalender.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(long? userId);
        void AddUser(User user);
        Task<User> GetUserByRole(Role role);
    }
}
