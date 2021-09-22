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

        public async Task<User> FindUserById(long? userId)
        {
            return await UserRepository.FindUserById(userId);
        }

        public void AddUser(User user) 
        {
            UserRepository.AddUser(user);
        }

        public async Task<User> FindUserByName(string name)
        {
            return await UserRepository.FindUserByName(name);
        }

        public void UpdateUser(User user)
        {
            UserRepository.UpdateUser(user);
        }

        public void DeleteUser(User user)
        {
            UserRepository.DeleteUser(user);
        }
    }
}
