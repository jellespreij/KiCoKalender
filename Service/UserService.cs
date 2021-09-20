using KiCoKalender.Interfaces;
using KiCoKalender.Models;
using KiCoKalender.Repository;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Service
{
    class UserService : IUserService
    {
        private UserRepository UserRepository { get; }

        public UserService(UserRepository userRepository) 
        {
            this.UserRepository = userRepository;
        }

        public async Task<User> GetUserById(long? userId)
        {
            return await UserRepository.GetUserById(userId);
        }

        public void AddUser(User user) 
        {
            UserRepository.AddUser(user);
        }

        public async Task<User> GetUserByRole(Role role)
        {
            return await UserRepository.GetUserByRole(role);
        }
    }
}
