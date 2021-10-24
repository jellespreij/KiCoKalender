using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                Console.WriteLine("User already exists");
                return null;
            }
        }

        public User DeleteUser(Guid id)
        {
            return _userRepository.Delete(id).Result;
        }

        public User FindUserByUserId(Guid userId)
        {
            return _userRepository.GetSingle(userId);
        }
        public User UpdateUser(User user, Guid id)
        {
            return _userRepository.Update(user, id).Result;
        }

        public User FindUserByEmail(string email)
        {
            return _userRepository.FindUserByEmail(email).Result;
        }
    }
}
