using log4net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IFamilyRepository _familyRepository;
        ILogger Logger { get; }

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IFamilyRepository familyRepository)
        {
            Logger = logger;
            _userRepository = userRepository;
            _familyRepository = familyRepository;
        }

        public User AddUser(User user)
        {
            User foundUser = _userRepository.FindUserByEmail(user.Email).Result;
            if (foundUser is null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                return _userRepository.Add(user).Result;
            }
            else
            {
                Logger.LogError("User already exists");
                return null;
            }
        }

        public async Task<User> DeleteUser(Guid id, Guid currentUser)
        {
            if(currentUser == id)
            {
                User user = _userRepository.GetSingle(id);
                _familyRepository.RemoveUserFromFamily(user);
                return _userRepository.Delete(id).Result;
            }
            return null;

        }

        public User FindUserByUserId(Guid userId)
        {
            return _userRepository.GetSingle(userId);
        }
        public User UpdateUser(User user, Guid id, Guid currentUser)
        {
            if (currentUser == id)
            {
                return _userRepository.Update(user, id).Result;
            }
            return null;

        }

        public User FindUserByEmail(string email)
        {
            return _userRepository.FindUserByEmail(email).Result;
        }
    }
}
