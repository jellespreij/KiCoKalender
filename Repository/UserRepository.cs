using KiCoKalender.Controllers;
using KiCoKalender.Models;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Repository
{
    class UserRepository
    {
        ILogger Logger { get; }

        public UserRepository(ILogger<UserHttpTrigger> Logger) 
        {
            this.Logger = Logger;
        }

        public async Task<User> FindUserById(long? userId) 
        {
            User user = new User() { userId = 33, userName = "Dirk Dirskma", userRole = Role.Parent };
            Logger.LogInformation("Found user by id: ", userId);

            return user;
        }

        public async void AddUser(User user) 
        {
            Logger.LogInformation("Inserted user");
        }

        public async Task<User> FindUserByName(string name) 
        {
            User user = new User() { userId = 33, userName = "Dirk Dirksma", userRole = Role.Parent };
            Logger.LogInformation("Found user by name: ", name);

            return user;
        }
        public async void UpdateUser(User user)
        {
            Logger.LogInformation("Updated user");
        }
        public async void DeleteUser(User user)
        {
            Logger.LogInformation("Deleted user");
        }
    }
}
